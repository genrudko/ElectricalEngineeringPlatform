from __future__ import annotations

import json
import os
import secrets
import shutil
import socket
import subprocess
import sys
import tarfile
import threading
import time
import uuid
from concurrent.futures import ThreadPoolExecutor
from dataclasses import dataclass, field
from datetime import datetime, timezone
from pathlib import Path
from typing import Literal

from fastapi import Depends, FastAPI, HTTPException, Query, Request, status
from fastapi.responses import JSONResponse, PlainTextResponse
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer
from pydantic import BaseModel, Field

from bridge_core import (
    MAX_LOG_BYTES,
    MAX_SEARCH_FILE_BYTES,
    MAX_SEARCH_RESULTS,
    MAX_TEXT_BYTES,
    bounded_utf8,
    resolve_read_path,
    truncate_log,
    validate_git_ref,
)

SERVICE_NAME = "eep-dev-bridge"
SERVICE_VERSION = "0.2.0"
REPOSITORY_ROOT = Path(
    os.environ.get(
        "EEP_BRIDGE_REPOSITORY_ROOT",
        "/home/eep-workspace/workspace/ElectricalEngineeringPlatform",
    )
)
STATE_ROOT = Path(os.environ.get("EEP_BRIDGE_STATE_ROOT", "/var/lib/eep-dev-bridge"))
TASK_ROOT = STATE_ROOT / "tasks"
AUDIT_LOG = STATE_ROOT / "audit.jsonl"
MAX_TASKS = 50
TASK_RETENTION_SECONDS = 24 * 60 * 60
TASK_TIMEOUT_SECONDS = 120
MAX_ARCHIVE_BYTES = 64 * 1024 * 1024

PROFILE_COMMANDS: dict[str, tuple[str, ...]] = {
    "bridge_compile": (
        sys.executable,
        "-m",
        "compileall",
        "-q",
        "tools/development_bridge",
    ),
    "bridge_selftest": (
        sys.executable,
        "-m",
        "unittest",
        "discover",
        "-s",
        "tools/development_bridge/tests",
        "-v",
    ),
}


def _load_bearer_token() -> tuple[str, str]:
    preferred = (
        "EEP_BRIDGE_TOKEN",
        "EEP_DEV_BRIDGE_TOKEN",
        "BRIDGE_TOKEN",
        "EEP_BRIDGE_API_KEY",
    )
    for name in preferred:
        value = os.environ.get(name)
        if value:
            return name, value

    candidates = [
        name
        for name, value in os.environ.items()
        if value
        and ("BRIDGE" in name.upper() or name.upper().startswith("EEP_"))
        and ("TOKEN" in name.upper() or "KEY" in name.upper())
    ]
    if len(candidates) == 1:
        name = candidates[0]
        return name, os.environ[name]
    raise RuntimeError(
        "Bridge bearer credential not found or ambiguous; candidate names: "
        + ", ".join(sorted(candidates))
    )


TOKEN_ENV_NAME, BEARER_TOKEN = _load_bearer_token()
AUTH = HTTPBearer(auto_error=False)

app = FastAPI(
    title="EEP Development Bridge",
    version=SERVICE_VERSION,
    docs_url=None,
    redoc_url=None,
    openapi_url=None,
)


def utc_now() -> str:
    return datetime.now(timezone.utc).isoformat()


def audit(event: str, **fields: object) -> None:
    STATE_ROOT.mkdir(parents=True, exist_ok=True)
    record = {"ts": utc_now(), "event": event, **fields}
    line = json.dumps(record, ensure_ascii=False, separators=(",", ":")) + "\n"
    with AUDIT_LOG.open("a", encoding="utf-8") as handle:
        handle.write(line)


def require_auth(
    credentials: HTTPAuthorizationCredentials | None = Depends(AUTH),
) -> None:
    if credentials is None or credentials.scheme.lower() != "bearer":
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Bearer authentication required",
            headers={"WWW-Authenticate": "Bearer"},
        )
    if not secrets.compare_digest(credentials.credentials, BEARER_TOKEN):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Invalid bearer token",
            headers={"WWW-Authenticate": "Bearer"},
        )


def run_git(args: list[str], timeout: int = 15) -> str:
    completed = subprocess.run(
        ["git", "-C", str(REPOSITORY_ROOT), *args],
        check=False,
        stdout=subprocess.PIPE,
        stderr=subprocess.STDOUT,
        text=True,
        timeout=timeout,
    )
    if completed.returncode != 0:
        text, _ = truncate_log(completed.stdout or "git command failed")
        raise RuntimeError(text)
    return completed.stdout.strip()


def resolve_commit(ref: str) -> str:
    safe_ref = validate_git_ref(ref)
    value = run_git(["rev-parse", "--verify", f"{safe_ref}^{{commit}}"])
    if len(value) != 40 or any(ch not in "0123456789abcdef" for ch in value):
        raise RuntimeError("resolved ref is not a full commit SHA")
    return value


@dataclass
class TaskRecord:
    task_id: str
    profile: str
    requested_ref: str
    state: str = "queued"
    created_at: str = field(default_factory=utc_now)
    started_at: str | None = None
    finished_at: str | None = None
    resolved_sha: str | None = None
    exit_code: int | None = None
    output: str = ""
    output_truncated: bool = False
    cancel_requested: bool = False
    process: subprocess.Popen[str] | None = field(default=None, repr=False)

    def public(self) -> dict[str, object]:
        return {
            "task_id": self.task_id,
            "profile": self.profile,
            "requested_ref": self.requested_ref,
            "resolved_sha": self.resolved_sha,
            "state": self.state,
            "created_at": self.created_at,
            "started_at": self.started_at,
            "finished_at": self.finished_at,
            "exit_code": self.exit_code,
            "output_truncated": self.output_truncated,
        }


TASKS: dict[str, TaskRecord] = {}
TASK_LOCK = threading.RLock()
EXECUTOR = ThreadPoolExecutor(max_workers=1, thread_name_prefix="eep-bridge-task")


def cleanup_old_tasks() -> None:
    now = time.time()
    with TASK_LOCK:
        candidates = list(TASKS.items())
    for task_id, record in candidates:
        if record.state in {"queued", "running"}:
            continue
        task_dir = TASK_ROOT / task_id
        try:
            age = now - task_dir.stat().st_mtime
        except FileNotFoundError:
            age = TASK_RETENTION_SECONDS + 1
        if age > TASK_RETENTION_SECONDS:
            shutil.rmtree(task_dir, ignore_errors=True)
            with TASK_LOCK:
                TASKS.pop(task_id, None)


def prepare_snapshot(task_id: str, ref: str) -> tuple[Path, str]:
    sha = resolve_commit(ref)
    task_dir = TASK_ROOT / task_id
    workspace = task_dir / "workspace"
    archive_path = task_dir / "source.tar"
    shutil.rmtree(task_dir, ignore_errors=True)
    workspace.mkdir(parents=True, exist_ok=True)

    with archive_path.open("wb") as archive_handle:
        completed = subprocess.run(
            ["git", "-C", str(REPOSITORY_ROOT), "archive", "--format=tar", sha],
            check=False,
            stdout=archive_handle,
            stderr=subprocess.PIPE,
            timeout=30,
        )
    if completed.returncode != 0:
        error = completed.stderr.decode("utf-8", errors="replace")
        raise RuntimeError(error or "git archive failed")
    if archive_path.stat().st_size > MAX_ARCHIVE_BYTES:
        raise RuntimeError("repository snapshot exceeds bridge archive limit")

    with tarfile.open(archive_path, "r:") as archive:
        archive.extractall(workspace, filter="data")
    archive_path.unlink(missing_ok=True)
    return workspace, sha


def execute_task(task_id: str) -> None:
    with TASK_LOCK:
        record = TASKS[task_id]
        if record.cancel_requested:
            record.state = "cancelled"
            record.finished_at = utc_now()
            return
        record.state = "running"
        record.started_at = utc_now()

    audit("task_started", task_id=task_id, profile=record.profile, ref=record.requested_ref)
    try:
        workspace, sha = prepare_snapshot(task_id, record.requested_ref)
        with TASK_LOCK:
            record.resolved_sha = sha
            if record.cancel_requested:
                record.state = "cancelled"
                record.finished_at = utc_now()
                return

        command = PROFILE_COMMANDS[record.profile]
        process = subprocess.Popen(
            list(command),
            cwd=workspace,
            stdout=subprocess.PIPE,
            stderr=subprocess.STDOUT,
            text=True,
            start_new_session=True,
        )
        with TASK_LOCK:
            record.process = process

        try:
            output, _ = process.communicate(timeout=TASK_TIMEOUT_SECONDS)
        except subprocess.TimeoutExpired:
            process.kill()
            output, _ = process.communicate()
            with TASK_LOCK:
                record.state = "timeout"
        else:
            with TASK_LOCK:
                if record.cancel_requested:
                    record.state = "cancelled"
                elif process.returncode == 0:
                    record.state = "success"
                else:
                    record.state = "failure"

        clipped, truncated = truncate_log(output or "", MAX_LOG_BYTES)
        with TASK_LOCK:
            record.exit_code = process.returncode
            record.output = clipped
            record.output_truncated = truncated
            record.finished_at = utc_now()
            record.process = None
    except Exception as exc:  # task errors become task state, never arbitrary API traceback
        text, truncated = truncate_log(f"{type(exc).__name__}: {exc}", MAX_LOG_BYTES)
        with TASK_LOCK:
            record.state = "failure"
            record.output = text
            record.output_truncated = truncated
            record.finished_at = utc_now()
            record.process = None
    finally:
        audit(
            "task_finished",
            task_id=task_id,
            state=record.state,
            resolved_sha=record.resolved_sha,
            exit_code=record.exit_code,
        )


class RunTaskRequest(BaseModel):
    profile: Literal["bridge_compile", "bridge_selftest"]
    ref: str = Field(min_length=1, max_length=220)


@app.exception_handler(RuntimeError)
async def runtime_error_handler(_: Request, exc: RuntimeError) -> JSONResponse:
    return JSONResponse(status_code=409, content={"detail": str(exc)})


@app.get("/health", dependencies=[Depends(require_auth)])
def health() -> dict[str, object]:
    return {"ok": True, "service": SERVICE_NAME, "version": SERVICE_VERSION}


@app.get("/status", dependencies=[Depends(require_auth)])
def server_status() -> dict[str, object]:
    return {
        "ok": True,
        "service": SERVICE_NAME,
        "version": SERVICE_VERSION,
        "hostname": socket.gethostname(),
        "python": sys.version.split()[0],
        "pid": os.getpid(),
        "uid": os.getuid(),
        "repository_root": str(REPOSITORY_ROOT),
        "task_concurrency": 1,
        "token_env_name": TOKEN_ENV_NAME,
    }


@app.get("/workspace/status", dependencies=[Depends(require_auth)])
def workspace_status() -> dict[str, object]:
    head = run_git(["rev-parse", "HEAD"])
    branch = run_git(["branch", "--show-current"]) or "DETACHED"
    porcelain = run_git(["status", "--porcelain"])
    fetch_head = REPOSITORY_ROOT / ".git" / "FETCH_HEAD"
    fetch_mtime = None
    if fetch_head.exists():
        fetch_mtime = datetime.fromtimestamp(
            fetch_head.stat().st_mtime, tz=timezone.utc
        ).isoformat()
    return {
        "repository_root": str(REPOSITORY_ROOT),
        "head": head,
        "branch": branch,
        "clean": not bool(porcelain),
        "last_fetch_utc": fetch_mtime,
    }


@app.get("/repository/status", dependencies=[Depends(require_auth)])
def repository_status() -> dict[str, object]:
    return {
        "head": run_git(["rev-parse", "HEAD"]),
        "origin_main": run_git(["rev-parse", "--verify", "origin/main^{commit}"]),
        "origin": run_git(["remote", "get-url", "origin"]),
    }


@app.get("/git/status", dependencies=[Depends(require_auth)])
def git_status() -> dict[str, object]:
    text = run_git(["status", "--short", "--branch"])
    bounded, truncated = truncate_log(text, MAX_TEXT_BYTES)
    return {"text": bounded, "truncated": truncated}


@app.get("/git/diff", dependencies=[Depends(require_auth)])
def git_diff() -> dict[str, object]:
    text = run_git(["diff", "--no-ext-diff", "--"])
    bounded, truncated = truncate_log(text, MAX_TEXT_BYTES)
    return {"text": bounded, "truncated": truncated}


@app.get("/files", dependencies=[Depends(require_auth)])
def read_workspace_file(path: str = Query(min_length=1, max_length=500)) -> dict[str, object]:
    try:
        target = resolve_read_path(REPOSITORY_ROOT, path)
    except (ValueError, FileNotFoundError) as exc:
        raise HTTPException(status_code=400, detail=str(exc)) from exc
    data = target.read_bytes()
    try:
        text, truncated = bounded_utf8(data, MAX_TEXT_BYTES)
    except UnicodeDecodeError as exc:
        raise HTTPException(status_code=415, detail="file is not UTF-8 text") from exc
    return {
        "path": str(target.relative_to(REPOSITORY_ROOT.resolve())),
        "text": text,
        "truncated": truncated,
        "size_bytes": len(data),
    }


@app.get("/search", dependencies=[Depends(require_auth)])
def search_workspace(
    query: str = Query(min_length=2, max_length=100),
    suffix: str | None = Query(default=None, max_length=20),
) -> dict[str, object]:
    if suffix and (not suffix.startswith(".") or "/" in suffix or "\\" in suffix):
        raise HTTPException(status_code=400, detail="suffix must look like .md or .py")

    needle = query.casefold()
    results: list[dict[str, object]] = []
    root = REPOSITORY_ROOT.resolve(strict=True)
    for candidate in root.rglob("*"):
        if len(results) >= MAX_SEARCH_RESULTS:
            break
        if ".git" in candidate.parts or candidate.is_symlink() or not candidate.is_file():
            continue
        if suffix and candidate.suffix != suffix:
            continue
        try:
            if candidate.stat().st_size > MAX_SEARCH_FILE_BYTES:
                continue
            text = candidate.read_text(encoding="utf-8")
        except (OSError, UnicodeDecodeError):
            continue
        for line_number, line in enumerate(text.splitlines(), start=1):
            if needle in line.casefold():
                results.append(
                    {
                        "path": str(candidate.relative_to(root)),
                        "line": line_number,
                        "text": line[:240],
                    }
                )
                if len(results) >= MAX_SEARCH_RESULTS:
                    break
    return {"query": query, "results": results, "truncated": len(results) >= MAX_SEARCH_RESULTS}


@app.post("/tasks/run", status_code=202, dependencies=[Depends(require_auth)])
def run_task(payload: RunTaskRequest) -> dict[str, object]:
    cleanup_old_tasks()
    try:
        safe_ref = validate_git_ref(payload.ref)
    except ValueError as exc:
        raise HTTPException(status_code=400, detail=str(exc)) from exc

    with TASK_LOCK:
        active = sum(1 for task in TASKS.values() if task.state in {"queued", "running"})
        if active >= 1:
            raise HTTPException(status_code=429, detail="bridge task concurrency limit reached")
        if len(TASKS) >= MAX_TASKS:
            cleanup_old_tasks()
        task_id = uuid.uuid4().hex
        record = TaskRecord(task_id=task_id, profile=payload.profile, requested_ref=safe_ref)
        TASKS[task_id] = record

    audit("task_queued", task_id=task_id, profile=payload.profile, ref=safe_ref)
    EXECUTOR.submit(execute_task, task_id)
    return record.public()


@app.get("/tasks/{task_id}", dependencies=[Depends(require_auth)])
def task_status(task_id: str) -> dict[str, object]:
    with TASK_LOCK:
        record = TASKS.get(task_id)
        if record is None:
            raise HTTPException(status_code=404, detail="task not found")
        return record.public()


@app.get("/tasks/{task_id}/log", dependencies=[Depends(require_auth)])
def task_log(task_id: str) -> PlainTextResponse:
    with TASK_LOCK:
        record = TASKS.get(task_id)
        if record is None:
            raise HTTPException(status_code=404, detail="task not found")
        return PlainTextResponse(record.output or "")


@app.post("/tasks/{task_id}/cancel", dependencies=[Depends(require_auth)])
def cancel_task(task_id: str) -> dict[str, object]:
    with TASK_LOCK:
        record = TASKS.get(task_id)
        if record is None:
            raise HTTPException(status_code=404, detail="task not found")
        if record.state not in {"queued", "running"}:
            return record.public()
        record.cancel_requested = True
        process = record.process
        if process is not None and process.poll() is None:
            process.terminate()
    audit("task_cancel_requested", task_id=task_id)
    return record.public()


@app.on_event("startup")
def startup() -> None:
    STATE_ROOT.mkdir(parents=True, exist_ok=True)
    TASK_ROOT.mkdir(parents=True, exist_ok=True)
    audit(
        "service_start",
        version=SERVICE_VERSION,
        pid=os.getpid(),
        repository_root=str(REPOSITORY_ROOT),
        token_env_name=TOKEN_ENV_NAME,
    )
