#!/usr/bin/env python3
"""Materialize frozen CMake/Ninja from exact upstream GitHub releases with digest verification."""
from __future__ import annotations

import argparse
import hashlib
import json
import os
import shutil
import stat
import tarfile
import tempfile
import urllib.request
import zipfile
from pathlib import Path

VERSIONS = {"cmake": "4.4.2", "ninja": "1.13.2"}
REPOS = {"cmake": "Kitware/CMake", "ninja": "ninja-build/ninja"}


def request_json(url: str) -> dict:
    headers = {"Accept": "application/vnd.github+json", "User-Agent": "eep-p1-materializer"}
    token = os.environ.get("GITHUB_TOKEN")
    if token:
        headers["Authorization"] = f"Bearer {token}"
    with urllib.request.urlopen(urllib.request.Request(url, headers=headers), timeout=60) as response:
        return json.load(response)


def download(url: str, destination: Path) -> str:
    headers = {"User-Agent": "eep-p1-materializer"}
    token = os.environ.get("GITHUB_TOKEN")
    if token and url.startswith("https://api.github.com/"):
        headers["Authorization"] = f"Bearer {token}"
        headers["Accept"] = "application/octet-stream"
    digest = hashlib.sha256()
    with urllib.request.urlopen(urllib.request.Request(url, headers=headers), timeout=120) as response, destination.open("wb") as out:
        while chunk := response.read(1024 * 1024):
            digest.update(chunk)
            out.write(chunk)
    return digest.hexdigest()


def asset_for(release: dict, name: str) -> dict:
    for asset in release.get("assets", []):
        if asset.get("name") == name:
            return asset
    raise RuntimeError(f"exact release asset not found: {name}")


def verify_release_asset(repo: str, tag: str, asset_name: str, destination: Path) -> dict:
    release_url = f"https://api.github.com/repos/{repo}/releases/tags/{tag}"
    release = request_json(release_url)
    asset = asset_for(release, asset_name)
    sha256 = download(asset["browser_download_url"], destination)
    api_digest = asset.get("digest")
    if not api_digest or not api_digest.startswith("sha256:"):
        raise RuntimeError(f"upstream release asset has no SHA-256 digest: {repo} {asset_name}")
    if sha256.lower() != api_digest.split(":", 1)[1].lower():
        raise RuntimeError(f"SHA-256 mismatch for {asset_name}")
    return {
        "repository": repo,
        "tag": tag,
        "release_id": release["id"],
        "asset_id": asset["id"],
        "asset_name": asset_name,
        "download_url": asset["browser_download_url"],
        "sha256": sha256,
        "release_api_digest": api_digest,
    }


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--platform", choices=["linux-x64", "windows-x64"], required=True)
    parser.add_argument("--root", type=Path, required=True)
    parser.add_argument("--provenance", type=Path, required=True)
    args = parser.parse_args()

    names = {
        "linux-x64": {
            "cmake": "cmake-4.4.2-linux-x86_64.tar.gz",
            "ninja": "ninja-linux.zip",
        },
        "windows-x64": {
            "cmake": "cmake-4.4.2-windows-x86_64.zip",
            "ninja": "ninja-win.zip",
        },
    }[args.platform]

    args.root.mkdir(parents=True, exist_ok=True)
    args.provenance.parent.mkdir(parents=True, exist_ok=True)
    records: dict[str, dict] = {"schema": "eep.tool-acquisition/v1", "platform": args.platform, "tools": {}}

    with tempfile.TemporaryDirectory(prefix="eep-p1-tools-") as tmp:
        temp = Path(tmp)
        cmake_archive = temp / names["cmake"]
        ninja_archive = temp / names["ninja"]
        records["tools"]["cmake"] = verify_release_asset(REPOS["cmake"], f"v{VERSIONS['cmake']}", names["cmake"], cmake_archive)
        records["tools"]["ninja"] = verify_release_asset(REPOS["ninja"], f"v{VERSIONS['ninja']}", names["ninja"], ninja_archive)

        cmake_dest = args.root / f"cmake-{VERSIONS['cmake']}"
        ninja_dest = args.root / f"ninja-{VERSIONS['ninja']}"
        shutil.rmtree(cmake_dest, ignore_errors=True)
        shutil.rmtree(ninja_dest, ignore_errors=True)
        ninja_dest.mkdir(parents=True)

        extracted = temp / "cmake-extracted"
        extracted.mkdir()
        if cmake_archive.suffix == ".zip":
            with zipfile.ZipFile(cmake_archive) as archive:
                archive.extractall(extracted)
        else:
            with tarfile.open(cmake_archive, "r:gz") as archive:
                archive.extractall(extracted, filter="data")
        roots = [path for path in extracted.iterdir() if path.is_dir()]
        if len(roots) != 1:
            raise RuntimeError("unexpected CMake archive layout")
        shutil.move(str(roots[0]), cmake_dest)

        with zipfile.ZipFile(ninja_archive) as archive:
            archive.extractall(ninja_dest)
        ninja_binary = ninja_dest / ("ninja.exe" if args.platform == "windows-x64" else "ninja")
        if not ninja_binary.exists():
            raise RuntimeError("ninja executable missing after extraction")
        if args.platform == "linux-x64":
            ninja_binary.chmod(ninja_binary.stat().st_mode | stat.S_IXUSR | stat.S_IXGRP | stat.S_IXOTH)

    records["paths"] = {
        "cmake_bin": str(cmake_dest / "bin"),
        "ninja_bin": str(ninja_dest),
    }
    args.provenance.write_text(json.dumps(records, indent=2) + "\n", encoding="utf-8")
    print(json.dumps(records))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
