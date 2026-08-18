#!/usr/bin/env python3
import argparse
import hashlib
import json
import os
from pathlib import Path
import shutil
import tarfile
import tempfile
import urllib.request
import zipfile

METADATA_URL = "https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/10.0/releases.json"


def fetch_json(url: str):
    req = urllib.request.Request(url, headers={"User-Agent": "eep-platform-stack-spike/1"})
    with urllib.request.urlopen(req, timeout=60) as response:
        return json.load(response)


def download(url: str, target: Path) -> str:
    digest = hashlib.sha512()
    req = urllib.request.Request(url, headers={"User-Agent": "eep-platform-stack-spike/1"})
    with urllib.request.urlopen(req, timeout=120) as response, target.open("wb") as output:
        while True:
            chunk = response.read(1024 * 1024)
            if not chunk:
                break
            digest.update(chunk)
            output.write(chunk)
    return digest.hexdigest()


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--version", required=True)
    parser.add_argument("--rid", required=True)
    parser.add_argument("--install-dir", required=True)
    parser.add_argument("--provenance", required=True)
    args = parser.parse_args()

    metadata = fetch_json(METADATA_URL)
    matching_sdks = [release.get("sdk") for release in metadata.get("releases", []) if release.get("sdk", {}).get("version") == args.version]
    if len(matching_sdks) != 1:
        raise SystemExit(f"Expected exactly one SDK {args.version} in official metadata, found {len(matching_sdks)}")
    sdk = matching_sdks[0]
    matching_files = [item for item in sdk.get("files", []) if item.get("rid") == args.rid]
    if len(matching_files) != 1:
        raise SystemExit(f"Expected exactly one SDK file for {args.version}/{args.rid}, found {len(matching_files)}")
    item = matching_files[0]
    url = item["url"]
    expected_sha512 = item["hash"].lower()

    install_dir = Path(args.install_dir).resolve()
    provenance_path = Path(args.provenance).resolve()
    install_dir.parent.mkdir(parents=True, exist_ok=True)
    provenance_path.parent.mkdir(parents=True, exist_ok=True)

    with tempfile.TemporaryDirectory(prefix="eep-dotnet-") as temp:
        archive = Path(temp) / Path(url).name
        actual_sha512 = download(url, archive)
        if actual_sha512.lower() != expected_sha512:
            raise SystemExit(f"SHA-512 mismatch for {url}: expected {expected_sha512}, got {actual_sha512}")
        if install_dir.exists():
            shutil.rmtree(install_dir)
        install_dir.mkdir(parents=True)
        if archive.name.endswith(".tar.gz"):
            with tarfile.open(archive, "r:gz") as tf:
                tf.extractall(install_dir, filter="data")
        elif archive.name.endswith(".zip"):
            with zipfile.ZipFile(archive) as zf:
                zf.extractall(install_dir)
        else:
            raise SystemExit(f"Unsupported SDK archive: {archive.name}")

    provenance = {
        "schema": "eep.tool-acquisition/v1",
        "tool": ".NET SDK",
        "version": args.version,
        "rid": args.rid,
        "metadata_url": METADATA_URL,
        "download_url": url,
        "sha512": actual_sha512,
        "official_metadata_sha512": expected_sha512
    }
    provenance_path.write_text(json.dumps(provenance, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps(provenance, ensure_ascii=False))


if __name__ == "__main__":
    main()
