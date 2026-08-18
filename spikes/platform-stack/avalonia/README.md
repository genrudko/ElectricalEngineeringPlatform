# P1 Candidate A — Avalonia

Frozen comparison baseline:

- Avalonia 12.1.1
- Avalonia.Desktop 12.1.1
- Avalonia.Themes.Fluent 12.1.1
- .NET SDK 10.0.302
- .NET Runtime 10.0.10 servicing baseline
- `net10.0`

The application consumes `../shared/fixtures/p1-shell-fixture.json`. It is a P1 presentation shell only: no production Domain Core, semantic canvas, large engineering table or platform-stack decision is implemented here.

`Assets/Fonts` is materialized in CI from the frozen Noto Sans 2.015 release before build. Font binaries are intentionally not silently replaced by host fonts.
