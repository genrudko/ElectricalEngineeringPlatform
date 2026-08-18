# Единая системная архитектура

Статус: канонический Foundation-документ

## 1. Архитектурный стиль

Целевая архитектура — **modular monolith desktop application**.

Причины выбора:

- один основной user workstation/local project;
- общая in-memory/project domain model;
- сильные требования к transactions и undo/redo;
- низкая ценность network boundaries между локальными модулями;
- минимальная operational complexity для небольшой команды;
- при этом modules сохраняют явный ownership и test isolation.

Microservices, отдельный обязательный web backend и plugin marketplace не являются архитектурными целями.

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

## 3. Правило ownership

Каждый инженерный concept имеет одного authoritative owner.

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

Если два модуля сохраняют authoritative copies одного инженерного факта, architecture считается ошибочной, пока не существует explicit synchronization contract с отдельным обоснованием.

## 4. Направление зависимостей

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

NPT, EOD, Excel, Visio и другие external formats не должны становиться dependencies neutral domain types.

Запрещённые примеры:

- `Domain.Device` с обязательным `SdeTag` или `EodJournalId`;
- topology service, напрямую вызывающий NPT XML parser;
- UI Core, зависящий от switching-form templates;
- native project storage, требующий наличия Excel workbook;
- Scheme renderer, использующий NPT `CustElem` как единственный native symbol model.

## 5. Module contracts

Modules взаимодействуют через typed in-process contracts и domain IDs, а не через direct mutation private storage друг друга.

```text
Import Module
  produces ImportPlan
       ↓ approved transaction
Domain Core
  updates Equipment/Connections
       ↓ domain change set
Scheme Module
  computes incremental LayoutProposal
```

```text
Switching Module
  proposes SwitchingOperation
       ↓
Compliance/Interlock evaluator
       ↓
Domain state transition simulation
       ↓
Topology recalculation
       ↓
SwitchingSequence result
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

Exact physical format остаётся PENDING.

Обязательные свойства:

- explicit schema version;
- deterministic IDs;
- migrations;
- atomic save/backup/recovery;
- module extension policy;
- diagnostics/diffability where reasonable;
- отсутствие зависимости от GUI-framework object serialization.

## 7. Transactions и commands

Engineering mutations должны поддерживать validation, explicit undo/redo или явную non-undoable classification, atomic persistence boundary, change-set generation, dependent-view notification и provenance для imported/generated changes, когда это требуется.

UI components не должны напрямую patch arbitrary project dictionaries/JSON.

## 8. Разделение topology и view geometry

Electrical topology:

```text
Terminal A ─ Connection ─ Terminal B
```

Visual route:

```text
screen polyline / bends / layers / labels
```

Одна `Connection` может иметь разные routes в разных `View`.

Перемещение symbol не должно менять electrical topology. Reconnect terminal — отдельная explicit engineering mutation.

## 9. State model

State является semantic и отделён от rendered colors/shapes.

```text
CircuitBreakerPosition = OPEN | CLOSED | INTERMEDIATE | UNKNOWN
SignalQuality = GOOD | BAD | UNCERTAIN | UNKNOWN
```

Renderer переводит state/profile в appearance. `UNKNOWN` нельзя collapse в safe/disabled state.

## 10. Compliance integration

Compliance Core предоставляет source registry, rule registry, applicability resolution, profile composition, local overlays, conflict/non-weakening checks и explainable validation results.

## 11. UI architecture boundary

UI Core владеет interaction infrastructure, но не engineering meaning.

Shared Property Inspector может принимать module-provided typed editors/validators, не зная domain rules конкретного оборудования.

## 12. Module activation

На раннем этапе все first-party modules могут компилироваться в одно приложение.

Module enablement — configuration/composition, а не требование dynamic plugin mechanism.

Module объявляет module ID/version, dependencies, contributed capabilities/views/commands, project-extension ownership и migration requirements.

## 13. External adapters

- **Platform adapter** — filesystem/dialogs/clipboard/print/windowing/OS integration.
- **NPT adapter** — vendor file semantics/identifiers/resources.
- **EOD adapter** — optional, independently removable, без reverse Core dependency.
- **Future adapters** — CIM/other ECAD/CAD/data sources только при доказанной необходимости.

## 14. Performance strategy

Performance является частью architecture для tens of thousands rendered objects, large tables, hit-testing, topology recalculation, import reconciliation и multi-window workspaces.

Не моделировать каждый visual primitive как heavyweight desktop control, если benchmark evidence показывает плохую scalability.

## 15. Security и trust boundary

- imported/vendor files — untrusted data;
- project/import files не исполняют код;
- file/path operations валидируются;
- EOD/NPT integrations получают minimum required privileges;
- development runners не содержат production SCADA credentials.

## 16. Development Platform boundary

Development Platform не является частью product runtime. Канонический state остаётся в GitHub, existing VPS используется как execution plane, а быстрый interactive loop и formal CI остаются разными контурами.

## 17. Триггеры архитектурного review

ADR/update требуется перед изменением source-of-truth ownership, добавлением required separate runtime service, появлением второго authoritative topology store, incompatible native-format change, разрешением local policy ослабить mandatory baseline, добавлением real-equipment command execution, превращением EOD/NPT в required dependency или сменой platform после accepted Platform Stack ADR.