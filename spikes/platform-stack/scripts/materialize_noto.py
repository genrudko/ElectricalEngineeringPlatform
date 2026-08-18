#!/usr/bin/env python3
import argparse
import hashlib
import json
import os
from pathlib import Path
import shutil
import tempfile
import urllib.request
import zipfile

RELEASE_API = "https://api.github.com/repos/notofonts/latin-greek-cyrillic/releases/tags/NotoSans-v2.015"
ASSET_NAME = "NotoSans-v2.015.zip"
REQUIRED = {
    "Regular": "NotoSans-Regular.ttf",
    "SemiBold": "NotoSans-SemiBold.ttf"
}


def request(url: str):
    headers = {
        "Accept": "application/vnd.github+json",
        "User-Agent": "eep-platform-stack-spike/1",
        "X-GitHub-Api-Version": "2022-11-28"
    }
    token = os.environ.get("GITHUB_TOKEN")
    if token:
        headers["Authorization"] = f"Bearer {token}"
    return urllib.request.Request(url, headers=headers)


def sha256_file(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for chunk in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def choose_font(zf: zipfile.ZipFile, basename: str) -> str:
    matches = [name for name in zf.namelist() if Path(name).name == basename]
    hinted = [name for name in matches if "/hinted/ttf/" in f"/{name.lower()}"]
    if len(hinted) == 1:
        return hinted[0]
    if len(matches) == 1:
        return matches[0]
    raise SystemExit(f"Ambiguous {basename}; matches={matches}. No implicit font-binary choice is allowed.")


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--destination", required=True)
    parser.add_argument("--provenance", required=True)
    args = parser.parse_args()

    with urllib.request.urlopen(request(RELEASE_API), timeout=60) as response:
        release = json.load(response)
    assets = [asset for asset in release.get("assets", []) if asset.get("name") == ASSET_NAME]
    if len(assets) != 1:
        raise SystemExit(f"Expected exact release asset {ASSET_NAME}, found {len(assets)}")
    asset = assets[0]

    destination = Path(args.destination).resolve()
    provenance_path = Path(args.provenance).resolve()
    destination.mkdir(parents=True, exist_ok=True)
    provenance_path.parent.mkdir(parents=True, exist_ok=True)

    with tempfile.TemporaryDirectory(prefix="eep-noto-") as temp:
        archive = Path(temp) / ASSET_NAME
        with urllib.request.urlopen(request(asset["browser_download_url"]), timeout=120) as response, archive.open("wb") as output:
            shutil.copyfileobj(response, output)
        archive_sha256 = sha256_file(archive)
        api_digest = asset.get("digest")
        if api_digest and api_digest.startswith("sha256:") and archive_sha256.lower() != api_digest.split(":", 1)[1].lower():
            raise SystemExit(f"GitHub release digest mismatch for {ASSET_NAME}")

        files = []
        with zipfile.ZipFile(archive) as zf:
            for style, basename in REQUIRED.items():
                member = choose_font(zf, basename)
                target = destination / basename
                with zf.open(member) as source, target.open("wb") as output:
                    shutil.copyfileobj(source, output)
                files.append({
                    "style": style,
                    "file": basename,
                    "archive_member": member,
                    "sha256": sha256_file(target)
                })

    provenance = {
        "schema": "eep.font-acquisition/v1",
        "family": "Noto Sans",
        "version": "2.015",
        "tag": release.get("tag_name"),
        "source_commit": "c4a321e123e4d4ff315f57f4e0adf294fe3a95be",
        "release_id": release.get("id"),
        "asset_id": asset.get("id"),
        "asset_name": asset.get("name"),
        "asset_size": asset.get("size"),
        "release_api_digest": asset.get("digest"),
        "archive_sha256": archive_sha256,
        "files": files
    }
    provenance_path.write_text(json.dumps(provenance, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps(provenance, ensure_ascii=False))


if __name__ == "__main__":
    main()
