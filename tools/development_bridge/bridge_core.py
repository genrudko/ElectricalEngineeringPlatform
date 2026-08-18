from __future__ import annotations

import re
from pathlib import Path

MAX_TEXT_BYTES = 128 * 1024
MAX_LOG_BYTES = 64 * 1024
MAX_SEARCH_RESULTS = 50
MAX_SEARCH_FILE_BYTES = 1024 * 1024

_FULL_SHA_RE = re.compile(r"^[0-9a-f]{40}$")
_REMOTE_REF_RE = re.compile(r"^origin/[A-Za-z0-9][A-Za-z0-9._/-]{0,199}$")


def validate_git_ref(value: str) -> str:
    """Return a bounded read-only ref or raise ValueError.

    Accepted forms are a full lowercase commit SHA or an origin/* remote-tracking
    branch. Git revision operators and option-like values are intentionally
    rejected.
    """

    if not isinstance(value, str):
        raise ValueError("ref must be a string")
    value = value.strip()
    if _FULL_SHA_RE.fullmatch(value):
        return value
    if not _REMOTE_REF_RE.fullmatch(value):
        raise ValueError("ref must be a full SHA or origin/<branch>")
    if any(token in value for token in ("..", "//", "@{", "~", "^", ":", "\\")):
        raise ValueError("ref contains a forbidden revision/path operator")
    return value


def resolve_read_path(root: Path, relative: str) -> Path:
    """Resolve an existing regular file below root without path escape."""

    if not isinstance(relative, str) or not relative.strip():
        raise ValueError("path is required")
    candidate_text = relative.strip()
    if "\x00" in candidate_text:
        raise ValueError("path contains NUL")
    supplied = Path(candidate_text)
    if supplied.is_absolute():
        raise ValueError("absolute paths are not allowed")

    root_resolved = root.resolve(strict=True)
    candidate = (root_resolved / supplied).resolve(strict=True)
    try:
        candidate.relative_to(root_resolved)
    except ValueError as exc:
        raise ValueError("path escapes repository root") from exc

    if not candidate.is_file():
        raise ValueError("path is not a regular file")
    return candidate


def bounded_utf8(data: bytes, limit: int = MAX_TEXT_BYTES) -> tuple[str, bool]:
    """Decode UTF-8 text and cap returned bytes; invalid UTF-8 is rejected."""

    if limit <= 0:
        raise ValueError("limit must be positive")
    truncated = len(data) > limit
    chunk = data[:limit]
    return chunk.decode("utf-8", errors="strict"), truncated


def truncate_log(text: str, limit: int = MAX_LOG_BYTES) -> tuple[str, bool]:
    """Bound task output by UTF-8 byte size without returning invalid text."""

    encoded = text.encode("utf-8", errors="replace")
    if len(encoded) <= limit:
        return text, False
    clipped = encoded[:limit]
    while clipped:
        try:
            return clipped.decode("utf-8") + "\n[output truncated]", True
        except UnicodeDecodeError:
            clipped = clipped[:-1]
    return "[output truncated]", True
