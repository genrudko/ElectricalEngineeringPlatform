# ADR 0003 — Layered Normative Policy and Non-Weakening

Status: **ACCEPTED by owner direction / recorded in UNIFIED-FOUNDATION-001**  
Date: 2026-08-18

## Context

Russian energy-sector operational requirements come from multiple legal/normative sources. Real sites also have manufacturer manuals, enterprise standards and local instructions that can add or tighten requirements.

A simple `override=true` model could accidentally weaken an applicable mandatory baseline.

## Decision

Implement a versioned Compliance Core with explicit source provenance, applicability and layered policy resolution.

```text
mandatory regulatory baseline
→ applicable standard/profile baseline
→ manufacturer/equipment constraints
→ enterprise policy
→ site/object policy
→ project policy
```

Lower/local layers may add or tighten requirements but may not disable/relax an applicable locked mandatory rule.

Attempted weakening is a policy conflict/error.

Every production rule identifies source/version/scope and machine/review behavior.

## Consequences

- normative documents registered with editions/amendments/effective dates;
- projects can snapshot normative baseline date/profile;
- local policy packages are versioned/separately deployable;
- diagnostics identify source/conflict;
- switching forms retain mandatory human-review boundary until separately changed;
- graphics profiles distinguish ГОСТ/ЕСКД requirements from enterprise conventions/layout heuristics.

## Rejected alternatives

- Hard-code rules throughout modules — poor provenance/update/conflict handling.
- Last-file-wins configuration — unsafe authority model.
- Treat local instructions as equal to mandatory rules — source authority/applicability differs.
- Encode every rule immediately — fake completeness is worse than scoped coverage.
