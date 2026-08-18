# Unified System Architecture

Статус: canonical foundation document

## 1. Architectural style

Target architecture is a **modular monolith desktop application**.

Rationale:

- one primary user workstation/local project;
- shared in-memory/project domain model;
- strong transactional and undo/redo requirements;
- low value from network boundaries between modules;
- low operational complexity for a small team;
- modules still need explicit ownership and test isolation.

Microservices, separate web backend and plugin marketplace are not architectural goals.

## 2. High-level structure

```text
┌───────────────────────────────────────────────────────┐
│                     Application                       │
│  startup / module composition / workspace / commands │
└──────────────┬────────────────────────────────────────┘
               │
      ┌────────┴────────┐
      │     UI Core     │
      │ shell + shared  │
      │ desktop UX      │
      └────────┬────────┘
               │
┌──────────────┴────────────────────────────────────────┐
│                    Domain Core                        │
│ project/equipment/terminal/connection/topology/state │
│ validation/transactions/versioned persistence model  │
└─────┬──────────┬──────────┬──────────┬───────────────┘
      │          │          │          │
      ▼          ▼          ▼          ▼
 Equipment    Import      Schemes    Compliance
 Library      Module      Module      Core
      │          │          │          │
      └──────────┼──────────┼──────────┘
                 │          │
                 ▼          ▼
              NPT Module  Switching/TBP
                 │          │
                 └────┬─────┘
                      │
                 Optional adapters
                 ├── platform/native
                 ├── EOD (feasibility-gated)
                 └── future exchange
```

## 3. Ownership rule

Every concept has one authoritative owner.

| Concept | Owner |
|---|---|
| equipment identity/type/properties | Domain Core + Equipment Library definition |
| terminals/connections | Domain Core |
| electrical topology | Domain Core topology service/model |
| equipment state | Domain Core state model |
| diagram geometry | Scheme module view model |
| manual layout constraints | Scheme module |
| import mapping/staging | Import module |
| normative source/rule metadata | Compliance Core |
| NPT `sTag`/RTID/scd serialization | NPT module |
| switching sequence | Switching module |
| app shell/workspace | UI Core |
| EOD module registration/deep links | optional EOD adapter/bridge |

If two modules both persist authoritative copies of the same engineering fact, architecture is wrong unless an explicit synchronization contract exists.

## 4. Dependency direction

```text
App
↓
UI Core / module UIs
↓
Module application services
↓
Domain Core + Compliance Core
↓
Infrastructure/platform/storage adapters
```

NPT, EOD, Excel, Visio or other external formats must not be dependencies of neutral domain types.

Forbidden examples:

- `Domain.Device` containing required `SdeTag` or `EodJournalId`;
- topology service calling NPT XML parser directly;
- UI Core depending on switching-form templates;
- native project storage requiring Excel workbook presence;
- Scheme renderer using NPT `CustElem` as the only native symbol model.

## 5. Module contracts

Modules communicate through typed in-process contracts and domain IDs, not direct mutation of each other's private storage.

```text
Import Module
  produces ImportPlan
       ↓ approved transaction
Domain Core
  updates Equipment/Connections
       ↓ domain change set
Scheme Module
  computes incremental layout proposal
```

```text
Switching Module
  proposes Operation
       ↓
Compliance/Interlock evaluator
       ↓
Domain state transition simulation
       ↓
Topology recalculation
       ↓
Switching sequence result
```

## 6. Project persistence layers

```text
Project Package
├── domain model
├── view/layout data
├── compliance/profile references
├── module-owned extension data
├── source provenance/import mappings
└── attachments/metadata as needed
```

Exact physical format is PENDING. Requirements:

- explicit schema version;
- deterministic IDs;
- migrations;
- atomic save/backup/recovery;
- module extension policy;
- diagnostics/diffability where reasonable;
- no dependency on GUI-framework object serialization.

## 7. Transactions and commands

Engineering mutations must support validation, explicit undo/redo or non-undoable classification, atomic persistence boundary, change-set generation, dependent-view notification and provenance for imported/generated changes where relevant.

UI components must not directly patch arbitrary project dictionaries/JSON.

## 8. Topology vs view separation

Electrical topology:

```text
Terminal A ─ Connection ─ Terminal B
```

Visual route:

```text
screen polyline / bends / layers / labels
```

The same connection may have different routes in different views. Moving a symbol must not silently change electrical topology. Reconnecting a terminal is an explicit engineering mutation.

## 9. State model

State is semantic and separate from rendered colors/shapes.

```text
CircuitBreakerState = OPEN | CLOSED | INTERMEDIATE | UNKNOWN
Quality = GOOD | BAD | UNCERTAIN | UNKNOWN
```

Renderer maps state/profile to appearance. `UNKNOWN` cannot be collapsed into a safe/disabled state.

## 10. Compliance integration

Compliance Core provides source registry, rule registry, applicability resolution, profile composition, local overlays, conflict/non-weakening checks and explainable validation results.

## 11. UI architecture boundary

UI Core owns interaction infrastructure, not engineering meaning. Shared Property Inspector infrastructure may host module-provided typed editors/validators without knowing their domain rules.

## 12. Module activation

Early implementation may compile all first-party modules into one application. Module enablement is configuration/composition, not a dynamic plugin requirement.

A module declares module ID/version, dependencies, contributed capabilities/views/commands, project-extension ownership and migration requirements.

## 13. External adapters

- **Platform adapter** — filesystem/dialogs/clipboard/print/windowing/OS integration.
- **NPT adapter** — vendor file semantics/identifiers/resources.
- **EOD adapter** — optional, independently removable, no reverse Core dependency.
- **Future adapters** — CIM/other ECAD/CAD/data sources only when justified.

## 14. Performance strategy

Performance is part of architecture for tens of thousands of rendered objects, large tables, hit-testing, topology recalculation, import reconciliation and multi-window workspaces.

Do not model every visual primitive as a heavyweight desktop control if benchmark evidence shows it does not scale.

## 15. Security and trust boundary

- imported/vendor files are untrusted data;
- no arbitrary code execution from project/import files;
- file/path operations are validated;
- EOD/NPT integration gets only required privileges;
- development runners contain no production SCADA credentials.

## 16. Architectural review triggers

ADR/update required before changing source-of-truth ownership, adding required separate runtime services, adding a second authoritative topology store, incompatible native-format change, allowing local policy to weaken mandatory baseline, adding real-equipment command execution, making EOD/NPT required, or switching platform after accepted Platform Stack ADR.
