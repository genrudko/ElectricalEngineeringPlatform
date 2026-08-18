from __future__ import annotations

import tempfile
import unittest
from pathlib import Path

from tools.development_bridge.bridge_core import (
    bounded_utf8,
    resolve_read_path,
    truncate_log,
    validate_git_ref,
)


class ValidateGitRefTests(unittest.TestCase):
    def test_accepts_full_sha(self) -> None:
        value = "a" * 40
        self.assertEqual(validate_git_ref(value), value)

    def test_accepts_origin_branch(self) -> None:
        value = "origin/infrastructure/infrastructure-spike-001"
        self.assertEqual(validate_git_ref(value), value)

    def test_rejects_revision_operators(self) -> None:
        bad_values = (
            "origin/main~1",
            "origin/main^",
            "origin/main^{commit}",
            "origin/main..origin/other",
            "origin/main@{1}",
            "--help",
        )
        for value in bad_values:
            with self.subTest(value=value):
                with self.assertRaises(ValueError):
                    validate_git_ref(value)


class PathPolicyTests(unittest.TestCase):
    def test_reads_regular_file_below_root(self) -> None:
        with tempfile.TemporaryDirectory() as tmp:
            root = Path(tmp)
            target = root / "docs" / "x.txt"
            target.parent.mkdir()
            target.write_text("ok", encoding="utf-8")
            self.assertEqual(resolve_read_path(root, "docs/x.txt"), target.resolve())

    def test_rejects_absolute_path(self) -> None:
        with tempfile.TemporaryDirectory() as tmp:
            with self.assertRaises(ValueError):
                resolve_read_path(Path(tmp), "/etc/passwd")

    def test_rejects_parent_escape(self) -> None:
        with tempfile.TemporaryDirectory() as tmp:
            root = Path(tmp) / "root"
            root.mkdir()
            outside = Path(tmp) / "outside.txt"
            outside.write_text("secret", encoding="utf-8")
            with self.assertRaises(ValueError):
                resolve_read_path(root, "../outside.txt")

    def test_rejects_symlink_escape(self) -> None:
        with tempfile.TemporaryDirectory() as tmp:
            base = Path(tmp)
            root = base / "root"
            root.mkdir()
            outside = base / "outside.txt"
            outside.write_text("secret", encoding="utf-8")
            (root / "link.txt").symlink_to(outside)
            with self.assertRaises(ValueError):
                resolve_read_path(root, "link.txt")


class BoundedOutputTests(unittest.TestCase):
    def test_bounded_utf8_reports_truncation(self) -> None:
        text, truncated = bounded_utf8(b"abcdef", 3)
        self.assertEqual(text, "abc")
        self.assertTrue(truncated)

    def test_truncate_log_preserves_utf8(self) -> None:
        text, truncated = truncate_log("абвгд", 7)
        self.assertTrue(truncated)
        self.assertIn("[output truncated]", text)


if __name__ == "__main__":
    unittest.main()
