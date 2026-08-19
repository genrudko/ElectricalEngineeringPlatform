# P1 remaining candidate — Avalonia

Active viability baseline after owner withdrawal of Qt:

- Avalonia 12.1.1
- Avalonia.Desktop 12.1.1
- Avalonia.Themes.Fluent 12.1.1
- .NET SDK 10.0.302
- .NET Runtime 10.0.10 servicing baseline
- `net10.0`

Owner amendment: `docs/development/PLATFORM_STACK_SPIKE_OWNER_AMENDMENT_2026-08-19.md`.

The application consumes `../shared/fixtures/p1-shell-fixture.json`. It is a P1 presentation shell only: no production Domain Core, semantic canvas, large engineering table or final platform-stack decision is implemented here.

`Assets/Fonts` is materialized in CI from the frozen Noto Sans 2.015 release before build. Font binaries are intentionally not silently replaced by host fonts.

Passing P1 does not select Avalonia. It only permits progression to the next mandatory viability gate; any unresolved mandatory gate failure leads to `NO_STACK_PASSES_CURRENT_CONTRACT` under the active owner contract.
