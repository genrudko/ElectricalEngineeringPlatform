# Agent Instructions — Electrical Engineering Platform

## 1. Product identity

Repository `genrudko/ElectricalEngineeringPlatform` is the canonical umbrella for one **standalone, desktop-first, local-first, modular electrical-engineering software complex**.

The product contains three major functional directions:

- Scheme Studio;
- NPT Engineering Toolkit / compatibility;
- Switching / TBP.

Legacy-source disposition is not identical for all three:

- `genrudko/electroscheme-studio` is historical research/migration evidence;
- NPT/Modus materials are compatibility/reference corpus;
- the **old TBP project is explicitly `DO_NOT_MIGRATE`**. The new Switching module is a clean-sheet implementation against current Domain/Compliance contracts and current verified normative sources.

## 2. Canonical source and precedence

GitHub is the canonical source for code, architecture, plans, issues, branches, PRs, tests and accepted evidence.

Before work restore from GitHub:

1. current `main`;
2. active issue;
3. active branch and Draft PR;
4. exact PR head / compare state;
5. changed files;
6. applicable workflow state;
7. canonical documents from `docs/INDEX.md`.

Do not ask the owner for handoffs/SHA/state that GitHub can provide.

For product/architecture meaning use:

```text
explicit owner instruction
→ accepted ADR
→ canonical architecture/compliance docs from docs/INDEX.md
→ CURRENT_STATE / roadmap
→ research and migration evidence
→ historical prototype documents
```

## 3. Work-item workflow

Normal flow:

```text
issue
→ dedicated branch
→ Draft PR
→ targeted implementation/checks
→ visual/manual evidence when applicable
→ owner acceptance
→ explicit Ready/Merge command
```

Rules:

1. Reuse an existing issue/branch/Draft PR for the same work item; do not create duplicates.
2. Do not commit directly to `main` except unavoidable initial repository bootstrap.
3. Do not mark Ready for Review without explicit owner command.
4. Do not merge without explicit owner command.
5. Keep changes risk-bounded; avoid repair-on-repair churn.
6. Full/nuclear CI is not the default tax on a small UI change.
7. Visual acceptance for UI changes must happen before expensive unrelated gates where feasible.
8. GitHub state, not chat memory/local patches, determines factual status.

## 4. Core architecture invariants

1. `ElectricalProject` / neutral domain model is the engineering source of truth.
2. Diagram geometry is a view/projection and is not electrical topology.
3. CSV/XLSX, VSDX/VSSX, XSDE, XTABL and switching-form documents are imports/exports/views/adapters, not parallel canonical models.
4. Equipment, terminals, connections, signals and rules use stable identifiers.
5. Connections reference semantic terminals/ports, not incidental screen coordinates.
6. Mutations use explicit command/transaction paths compatible with undo/redo and validation.
7. Project storage is versioned and migratable.
8. `UNKNOWN` is first-class. Unknown position/quality/energization must never be silently interpreted as open/off/de-energized.
9. Domain Core contains no NPT implementation identifiers such as `sTag`, `RTID`, `TechData`, `CustElem` or `scd*`.
10. UI Core contains no product-specific business rules that belong to modules/domain services.
11. Domain modules are testable without launching the full UI.
12. Standalone operation must remain possible with EOD integration absent.

## 5. Target modular-monolith boundary

```text
Core.Domain
Core.UI
Core.Compliance
Core.ProjectStorage

Modules.EquipmentLibrary
Modules.Import
Modules.Schemes
Modules.Npt
Modules.Switching

Adapters.Platform
Adapters.Eod        # optional / feasibility-gated
App
```

Separate assemblies/libraries are allowed and expected; distributed microservices and dynamic plugin marketplace are not early requirements.

Do not introduce a shared abstraction until at least one real cross-module use case proves it.

## 6. Import and auto-layout invariants

```text
source CSV/XLSX
→ mapping profile
→ normalization
→ staging candidate
→ validation/ambiguity resolution
→ reconciliation
→ ElectricalProject update
→ topology validation
→ auto-layout proposal
→ engineer review
```

Rules:

- CSV/XLSX is not native project storage.
- Never silently guess an ambiguous terminal or connection.
- Imported entities keep source/provenance identifiers.
- Re-import must show a diff/reconciliation plan before destructive change.
- Manual layout corrections are stored as layout constraints and must not be erased by routine re-import/auto-layout.
- Electrical correctness and layout confidence are separate diagnostics.

## 7. NPT compatibility boundary

NPT/Modus material is valuable industrial evidence and compatibility input, not the architecture of the new platform.

Never assume without evidence:

- that NPT `nodes` are a complete electrical topology graph;
- that every `scd*` value is KKS;
- that reconstruction from a simplified XML model is lossless;
- that experimentally generated XSDE objects are native-Modus-safe before native acceptance is proven.

NPT-specific IDs/storage rules belong in `Modules.Npt` / adapters.

Full NPT corpus/vendor binaries must not be committed to GitHub. Use controlled VPS corpus storage; Git contains only synthetic/cleared fixtures and project-authored tooling.

## 8. Normative/compliance discipline

Every encoded normative rule must carry at least:

- stable rule ID;
- source document and issuer;
- edition/amendment/effective dates;
- applicability/scope;
- normative level/authority;
- machine-checkable predicate/action where possible;
- severity;
- explanation;
- source/provenance reference;
- test/evidence status.

Never claim full compliance to a whole ГОСТ, ПУЭ, ПОТЭЭ, ПТЭЭС, ПТЭЭП/ПТЭЭПЭЭ or switching-rule set unless the claimed scope is explicitly encoded, traced and accepted.

ПУЭ must not be treated as one monolithic modern version; track applicable chapters/sources/revisions.

Public normative acts/standards are acquired and re-verified online from authoritative sources when relevant work begins. Do not rely on the old TBP implementation or random internet copies as normative authority.

For ГОСТ, official catalogue/status metadata is mandatory; detailed extraction uses a lawful full-text source where required.

## 9. Non-weakening local policy rule

```text
Mandatory regulatory baseline
→ applicable standards/profile baseline
→ manufacturer/equipment constraints
→ enterprise policy
→ site/object policy
→ project policy
```

A lower/local layer may add requirements or choose a stricter alternative. An attempted weakening of a locked mandatory requirement is a configuration error, not an override.

### Confidentiality boundary

Internal enterprise/site instructions are **not shared development inputs** and must not be requested/uploaded as a normal prerequisite.

Develop Local Policy Overlay mechanics using synthetic/cleared fixtures. Real internal instructions and source documents remain inside the authorized deployment environment; only formalized local policy packages/source metadata are consumed there as required.

## 10. Graphics and ГОСТ/ЕСКД

Electrical-scheme graphics are semantic assets governed by versioned graphic-standard profiles.

Do not promote a symbol because it merely looks familiar. Native symbols need domain/type mapping, terminal semantics, state variants, normative source/profile, geometry evidence, connection points, labels/designations, output evidence and provenance clarity.

NPT graphical assets may be used as compatibility/reference evidence; do not silently copy proprietary assets into the native symbol library.

## 11. Switching, state and interlocks

`Modules.Switching` is a **clean-sheet module**. Do not migrate old TBP source/config/rules/tests unless the owner explicitly reopens one narrowly identified artifact.

Safety boundaries:

- software simulation is not physical equipment control;
- logical/project interlock is not a substitute for relay/PLC/hardwired interlock;
- generated switching forms/sequences remain draft/decision-support output requiring qualified human review until separately accepted otherwise;
- no operation is considered safe merely because the model lacks contradictory data;
- denial/uncertainty must be explainable to the user.

Do not add real SCADA command execution, IEC-104 server, historian, P/Q control or redundancy without a new explicit owner decision.

## 12. UI Core and UX quality

UI Core is first-class architecture, not cosmetic styling.

Target: modern, dense, professional desktop engineering UX suitable for long sessions, large projects, keyboard+mouse and multi-monitor work.

Required foundation includes application shell/workspace, document tabs/splits/detachable windows, design system/UI Gallery, Property Inspector, trees/virtualized tables, command system, dialogs/notifications/status, shared canvas infrastructure, HiDPI/mixed-DPI and keyboard/focus behavior.

Reject MS-DOS/legacy-looking UI as well as sparse/mobile-first desktop composition.

UI changes require visible evidence.

## 13. Platform stack rule

Final stack is PENDING.

Candidates:

- Avalonia + C#/.NET;
- Qt 6 + C++/QML.

Historical Tauri/WebView work in `genrudko/electroscheme-studio` PR #4 is research evidence only.

Do not select the stack by familiarity/preference. The Platform Stack Spike must compare equivalent canvas, tables, multi-window, HiDPI, visual testing, packaging and development-iteration scenarios.

## 14. Development Platform and CI

```text
ChatGPT/owner
→ GitHub
→ self-hosted runner on existing VPS
→ targeted build/test/benchmark/package
→ GitHub logs/artifacts
→ owner acceptance
```

No mandatory Business/MCP/new paid service is assumed.

The local PC is an acceptance endpoint, not a required build environment.

Risk-based lanes:

- UI-only: compile + targeted UI/headless/visual evidence + preview;
- domain: core + affected module + serialization/migration;
- NPT: lossless/round-trip/corpus/format invariants;
- topology/switching/compliance: scenario/invariant/property/rule tests;
- full suite: release/nightly or genuinely systemic changes.

## 15. Optional EOD integration

EOD integration is feasibility-gated and optional. Prefer a thin EOD bridge module using the existing EOD module registry plus deep-link/context handoff into the standalone desktop product.

Reject integration if it requires EOD-specific entities in Domain Core, mandatory runtime dependency on EOD, a separate product fork, duplicate UI shell, pervasive conditionals or substantial independent release/deploy burden.

Standalone product tests must pass with EOD adapter absent.

## 16. Legacy/prototype preservation

Do not mechanically copy old products into this repository.

Disposition examples:

- ElectroScheme Studio: selective research reuse only;
- NPT: preserve compatibility knowledge/corpus outside Git;
- old TBP: `DO_NOT_MIGRATE`;
- EOD: separate repository, bounded adapter only.

## 17. Documentation ownership

Start with `docs/INDEX.md` and update the canonical owner document in the same PR when its decision changes.

## 18. Current work item

For `UNIFIED-FOUNDATION-001`:

- repository `genrudko/ElectricalEngineeringPlatform`;
- issue #1;
- branch `architecture/unified-foundation-001`;
- Draft PR only;
- documentation/governance/architecture contracts first;
- no bulk product-code migration;
- no final platform-stack selection;
- no Ready/Merge without explicit owner command.

Immediate sequence after accepted Foundation:

```text
Infrastructure Spike
→ Avalonia vs Qt Platform Stack Spike
→ UI Core + minimal Domain Core
→ structured import / topology / auto-layout vertical slice
```