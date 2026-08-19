# PLATFORM-STACK-SPIKE-001 — Avalonia viability workspace

This directory contains isolated spike code/evidence for the active `PLATFORM-STACK-SPIKE-001` viability proof.

Current stage: **P1 — Professional Shell / UI Gallery**.

## Active candidate

```text
Avalonia 12.1.1 / C# / .NET 10
```

Qt was **WITHDRAWN BY OWNER HARD CONSTRAINT AT P1 ENTRY** because the credentialed vendor acquisition path is unacceptable and credential-free alternatives materially increase build/toolchain burden.

Canonical amendment: `docs/development/PLATFORM_STACK_SPIKE_OWNER_AMENDMENT_2026-08-19.md`.

Qt history/evidence is retained in Git/Actions, but Qt is not an active build, acceptance or selection candidate under the current contract.

## P1 invariants

- The remaining candidate consumes the frozen stack-neutral presentation fixture.
- P1 does not define the production `ElectricalProject` format or production UI/Domain architecture.
- Avalonia implementation remains idiomatic; no cross-framework abstraction is introduced merely to preserve the withdrawn comparison shape.
- Mandatory baseline uses one light desktop theme.
- P2 canvas, P3 large tables and P4 multi-window/platform integration are not part of the P1 implementation.
- P1 does not select Avalonia automatically: all later viability gates remain mandatory.

## Layout

```text
spikes/platform-stack/
  shared/
    fixtures/
    schemas/
  avalonia/
  qt/
    WITHDRAWN.md
  scripts/
```

`shared/fixtures/p1-shell-fixture.json` remains the canonical P1 presentation/demo input.
