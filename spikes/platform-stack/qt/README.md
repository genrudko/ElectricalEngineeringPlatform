# P1 Qt/QML baseline

P1-only professional engineering desktop shell. It consumes the same stack-neutral `p1-shell-fixture.json` as the Avalonia candidate and intentionally contains no production Domain Core, canvas, large-table, multi-window, or stack-selection code.

Frozen toolchain: Qt 6.11.1, C++20, QML/Qt Quick/Qt Quick Controls, CMake 4.4.2, Ninja 1.13.2. Qt Creator, qmake, Conan, vcpkg, aqtinstall, commercial modules and GPL-only optional modules are not baseline dependencies.

The `fonts/` directory is materialized from the frozen Noto Sans 2.015 release during evidence builds and is not a source-of-truth copy of font binaries.
