# Current State — Electrical Engineering Platform

Дата среза: **2026-08-18**  
Активная программа: `UNIFIED-FOUNDATION-001`

## GitHub factual state

- Canonical repository: `genrudko/ElectricalEngineeringPlatform`.
- Default branch: `main`.
- Bootstrap commit: `5aa73328917447fb410489e8f67685ee4907ae66`.
- Active issue: #1 `UNIFIED-FOUNDATION-001`.
- Active branch: `architecture/unified-foundation-001`.
- Foundation Draft PR: to be opened after migration of the canonical document set into this branch.
- Previous working Foundation source: `genrudko/electroscheme-studio` issue #5 / Draft PR #6 / branch `architecture/unified-foundation-001`; it is historical migration evidence after this new repository becomes canonical.
- Previous Tauri desktop spike remains in `genrudko/electroscheme-studio` issue #3 / Draft PR #4 as research evidence only.

Exact heads/compare/workflow state are volatile and must be read from GitHub before implementation/acceptance actions.

## Product decision in force

The target is one standalone, desktop-first/local-first **modular electrical-engineering complex** unifying:

1. Scheme Studio;
2. NPT Engineering Toolkit / compatibility;
3. Switching / TBP;
4. shared import/topology/state/normative/UI foundations.

`ElectricalProject` / neutral domain model is the source of truth. Diagram geometry, CSV/XLSX, NPT files and switching-form documents are bounded views/imports/adapters.

## Foundation accepted direction

- modular monolith;
- one neutral Domain Core;
- first-class UI Core;
- Compliance Core with versioned normative provenance;
- topology separated from geometry;
- explicit State model with `UNKNOWN` first-class;
- structured CSV/XLSX Import + reconciliation + auto-layout;
- ГОСТ/ЕСКД graphic profiles;
- switching rules traced to Russian energy-sector normative sources;
- enterprise/site/equipment/project policy overlays that cannot weaken applicable mandatory baseline;
- NPT compatibility behind module/adapter boundary;
- optional EOD integration behind strict feasibility/cost gate;
- GitHub as canonical control plane and existing VPS as intended execution plane;
- risk-based CI and visual-first UI acceptance;
- final platform stack selected only by equivalent Avalonia-vs-Qt executable spike.

## Not yet proven / not yet selected

- Avalonia vs Qt final selection;
- heavy-canvas performance on representative workload;
- exact native project package format/schema v1;
- complete equipment-type library;
- completeness/correctness of NPT `nodes` as topology source;
- production-safe creation of arbitrary new XSDE topology objects;
- full normative rule coverage;
- exact EOD bridge API/context-security/deployment cost;
- installer/update channel;
- final branding beyond repository/product working name.

## Important inherited evidence

### ElectroScheme Studio source repository

Useful evidence/assets include:

- object/terminal/connection research;
- SVG/editor interaction experiments;
- VSDX/VSSX/ShapeSheet tooling;
- snapping/busbar/symbol research;
- desktop packaging and Tauri spike evidence;
- market/reference-product research.

These are migration inputs, not automatically accepted production architecture.

### NPT Engineering Toolkit / source materials

Current research baseline includes:

- ~475 parseable real XSDE files and large industrial corpus;
- lossless XSDE round-trip for studied corpus;
- identified `scd*` semantics and non-KKS exceptions;
- embedded `CustElem` and external `.menu` library role;
- large ASU/TECH/KKS signal catalog research;
- lossless XTABL v6.0 core for seven production tables / 1461 records;
- unknown/preserve-only XTABL fields;
- evidence that NPT `nodes` are topology-related but not proof of a complete neutral electrical graph;
- current Mnemo renderer fidelity remains insufficient.

Full vendor/reference corpus stays outside GitHub.

### TBP source materials

Useful inherited concepts include:

- draft switching-form generation with mandatory human review;
- profile-driven site-specific behavior;
- normative-reference structures;
- operational wording/pattern research;
- rule-authority concepts.

At Foundation time the TBP source should be treated as an explicit migration source; do not invent a canonical GitHub repository if the source remains local.

## Normative baseline state

Foundation establishes the registry mechanism, not a claim that all requirements are encoded.

Initial high-priority source set includes:

- ГОСТ 2.701-2008 and ГОСТ 2.702-2011;
- ПОТЭЭ under Mintrud order №903н;
- PTEEP consumer rules under Minenergo order №811;
- PTEES under Minenergo order №1070 with amendment chain;
- Switching Rules under Minenergo order №757 with amendment chain;
- applicable ПУЭ chapters/sources tracked separately rather than as one synthetic version.

Every production rule still needs source-level extraction, applicability classification, testability decision and domain review.

## Development sequence after Foundation

```text
UNIFIED-FOUNDATION-001
        ↓
INFRASTRUCTURE-SPIKE-001
GitHub ↔ self-hosted VPS runner ↔ artifacts
        ↓
PLATFORM-STACK-SPIKE-001
Avalonia vs Qt
        ↓
UI-CORE-FOUNDATION-001
+
DOMAIN-CORE-FOUNDATION-001
        ↓
IMPORT-TO-SCHEME-VERTICAL-SLICE-001
CSV/XLSX → topology → auto-layout → manual correction → save → reimport
        ↓
module expansion: Scheme / NPT / Switching
```

## Explicitly out of current scope

Without a new owner decision, do not build:

- replacement SCADA runtime;
- IEC-104 server/client platform as product objective;
- historian;
- P/Q control;
- redundancy/failover SCADA platform;
- remote real-equipment switching execution;
- microservice platform;
- dynamic plugin marketplace;
- universal CAD replacement.

## Acceptance posture

Foundation must protect the project from four known failure modes:

1. three competing domain models;
2. legacy-looking/unusable UI despite correct backend;
3. untraceable normative folklore embedded in code;
4. development pipelines where a tiny visible repair becomes days of unrelated CI work.
