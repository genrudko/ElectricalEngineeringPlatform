# ADR 0002 — Neutral Domain Model as Source of Truth

Status: **ACCEPTED by owner direction / recorded in UNIFIED-FOUNDATION-001**  
Date: 2026-08-18

## Context

The product must support native schemes, structured CSV/XLSX import, NPT XSDE/XTABL compatibility, Visio interoperability and Switching/TBP without creating competing authoritative models.

## Decision

`ElectricalProject` and its neutral electrical domain model are the engineering source of truth.

Core semantic ownership includes stable equipment identity, equipment type/properties, terminals, connections, topology, state/quality semantics, neutral signal bindings, project/view references and compliance profile references.

Views and external formats are projections/adapters.

NPT-specific identifiers/serialization and EOD-specific identifiers must not become required Core fields.

Topology is distinct from diagram geometry.

`UNKNOWN` is first-class and never silently collapsed into a safe/negative state.

## Consequences

- CSV/XLSX import uses staging/reconciliation before Domain mutation;
- Scheme module stores layout/views separately from electrical connections;
- NPT module translates/preserves vendor data behind adapter boundary;
- Switching module consumes shared topology/state;
- project persistence must be versioned/migratable and framework-neutral.

## Rejected alternatives

- Diagram canvas as source of truth — rejected because moving geometry must not redefine topology.
- XSDE/NPT as source of truth — rejected due vendor lock/pollution of native semantics.
- Excel/CSV as native project format — rejected due insufficient topology/state/view/versioning semantics.
- TBP own equipment database — rejected due duplicate identity/topology.
