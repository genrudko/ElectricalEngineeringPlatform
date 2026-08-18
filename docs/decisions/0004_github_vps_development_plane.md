# ADR 0004 — GitHub Control Plane + Existing VPS Execution Plane

Status: **ACCEPTED by owner direction / recorded in UNIFIED-FOUNDATION-001**  
Date: 2026-08-18

## Context

Previous workflows became operationally expensive: local manual patching, SSH intervention, broad CI before visual acceptance and too many infrastructure steps for small changes.

The owner already has GitHub and an existing VPS and does not want a new mandatory paid platform for normal development.

## Decision

Use:

```text
GitHub = canonical control plane
existing VPS + self-hosted runner = execution/build/test plane
owner workstation = acceptance endpoint
```

Normal development uses issue → branch → Draft PR → targeted runner checks/artifacts → owner acceptance → explicit merge.

Local SSH is an infrastructure/admin escape hatch, not normal workflow.

CI is risk-based and UI visual acceptance occurs before expensive unrelated gates where practical.

## Consequences

Positive:

- no new required paid Business/MCP/control service;
- build toolchains/corpora stay off owner workstation;
- private NPT/site corpora can be tested on controlled VPS;
- GitHub remains auditable canonical state;
- targeted previews shorten UI feedback loops.

Costs/risks:

- one-time runner hardening/setup;
- self-hosted runner maintenance;
- VPS capacity limits must be measured;
- private corpus/security isolation must be deliberate.

## Rejected alternatives

- Owner workstation as mandatory build machine — rejected due friction/environment drift.
- New paid orchestration platform by default — rejected until measurable value exceeds GitHub+existing VPS.
- Full CI after every trivial change — rejected due poor feedback/risk ratio.
