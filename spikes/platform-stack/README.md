# PLATFORM-STACK-SPIKE-001 — executable comparison workspace

This directory is isolated spike code/evidence for the owner-approved comparison between Avalonia 12.1.1/.NET 10 and Qt 6.11.1/QML.

Current stage: **P1 — Professional Shell / UI Gallery**.

## P1 invariants

- Both candidates consume the same stack-neutral presentation fixture.
- P1 does not define the production `ElectricalProject` format or production UI/Domain architecture.
- Candidate implementations are idiomatic to their framework; no shared cross-framework UI abstraction is introduced.
- Mandatory baseline uses one light desktop theme.
- P2 canvas, P3 large tables, P4 multi-window/platform integration, weighted scoring and stack selection are out of scope.

## Layout

```text
spikes/platform-stack/
  shared/
    fixtures/
    schemas/
    evidence/
  avalonia/
  qt/
  scripts/
```

`shared/fixtures/p1-shell-fixture.json` is the canonical P1 presentation/demo input used by both candidates.
