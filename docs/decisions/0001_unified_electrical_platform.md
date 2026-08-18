# ADR 0001 — Unified Electrical Engineering Platform

Status: **ACCEPTED by owner direction / recorded in UNIFIED-FOUNDATION-001**  
Date: 2026-08-18

## Context

ElectroScheme Studio, NPT Engineering Toolkit and TBP/switching-form work independently converged on the same concepts: equipment identity, terminals, connections, topology, state and engineering/operational rules.

Keeping three independent architectures would duplicate project models, equipment libraries, topology logic, state handling, UI infrastructure and normative configuration.

## Decision

Build one standalone modular electrical-engineering product with shared Domain Core, UI Core and Compliance Core.

Former projects become modules/migration sources:

- Scheme Studio;
- NPT Compatibility;
- Switching/TBP.

CSV/XLSX Import and Equipment Library become first-class shared modules.

Architectural style: modular monolith unless a later ADR proves a distributed boundary necessary.

## Consequences

Positive:

- one project/equipment identity;
- topology/state reuse across scheme, NPT and switching;
- one UI/design foundation;
- one normative policy model;
- import once, reuse across modules;
- less maintenance duplication.

Costs/risks:

- Foundation/migration before feature expansion;
- common Core must avoid becoming an over-general mega-framework;
- old code cannot be merged mechanically;
- platform stack needs reevaluation under unified scope.

## Rejected alternatives

- Three permanently independent products — rejected due domain divergence/repeated maintenance.
- Immediate codebase concatenation — rejected due incompatible models/UI stacks/prototypes.
- Microservices around former products — rejected for local-first desktop scope and small-team cost.
