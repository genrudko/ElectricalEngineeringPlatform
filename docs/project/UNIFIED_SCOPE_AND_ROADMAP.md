# Единый scope и roadmap

Статус: канонический Foundation-документ

## 1. Принцип развития

Проект развивается через доказанные vertical slices, а не через одновременную реализацию всех модулей.

Общий Core существует для устранения реального дублирования между модулями, а не для построения абстрактной mega-platform до появления продуктовых workflows.

## 2. Входит в scope

### 2.1. Foundation capabilities

- neutral electrical project/domain model;
- equipment/terminal/connection/topology/state concepts;
- versioned project persistence;
- UI Core и professional desktop shell;
- Windows/Linux packaging после измеренного выбора stack;
- compliance/normative registry и policy layering;
- Development Bridge + self-hosted runner development platform;
- risk-based CI и visual-first acceptance.

### 2.2. Scheme Studio

- intelligent electrical objects;
- несколько views над одной project model;
- electrical connections независимо от visual routing;
- domain-aware auto-layout;
- manual layout editing/constraints;
- ГОСТ/ЕСКД-oriented graphic profiles с traceability;
- validation, search и property editing;
- print/export.

### 2.3. Import

- CSV/XLSX sources;
- configurable mapping profiles;
- normalization;
- staging/preview;
- ambiguity/conflict resolution;
- reconciliation/update;
- topology construction;
- provenance;
- handoff to auto-layout.

### 2.4. NPT Compatibility

- lossless/preserve-first XSDE handling;
- typed `scd*` editing;
- NPT signal catalog/search/bindings;
- native compatibility validation;
- XTABL editor/generator;
- project-resource resolution;
- topology extraction research where needed;
- interoperability без reimplementation SCADA runtime.

### 2.5. Switching / TBP

Switching module реализуется **с нуля**, а не переносится из старого TBP.

Scope:

- switching operation model;
- sequence model;
- current/simulated state;
- topology recalculation;
- normative validation;
- explainable interlocks;
- switching-form generation/checking;
- local policy overlays для разрешённых deployment-specific правил;
- mandatory human review, пока отдельный safety case не изменит эту границу.

### 2.6. Optional integration

- EOD adapter/bridge только при успешном feasibility/cost gate.

## 3. Явно вне текущего scope

Пока владелец не откроет отдельный work item:

- complete SCADA runtime replacement;
- online IEC-104 control infrastructure;
- historian;
- wind-farm P/Q control;
- SCADA redundancy;
- automatic real-equipment execution of switching sequences;
- cloud collaboration как обязательная dependency;
- multi-user realtime editing;
- universal CAD/mechanical/building drafting;
- full arbitrary Visio fidelity;
- arbitrary DWG ecosystem;
- plugin marketplace;
- AI/ML auto-layout до доказательства deterministic domain-aware layout;
- unrestricted remote shell from ChatGPT to VPS.

## 4. Roadmap

### F0 — Unified Foundation

Цель: однозначно зафиксировать архитектуру, normative boundaries, UI/DevEx, язык проекта, migration/disposition и первые work items.

В Foundation не выполняется bulk migration product code.

Отдельно в рамках Foundation уже доказан feasibility прямого ChatGPT Plus Project → Custom GPT Action → VPS bridge.

### F1 — `INFRASTRUCTURE-SPIKE-001`

Feasibility ChatGPT→VPS уже доказан и не повторяется как основной вопрос.

Нужно доказать production-quality development infrastructure:

```text
Интерактивный контур:
ChatGPT Project
→ hardened EEP Development Bridge
→ bounded build/test/inspect operations
→ existing VPS

Формальный контур:
GitHub exact PR head
→ self-hosted runner on existing VPS
→ targeted build/test/package
→ checks/artifacts
→ owner acceptance
```

Acceptance:

- unprivileged Development Bridge service;
- typed/allowlisted bridge API без arbitrary shell;
- fixed workspace/repository boundaries;
- timeout, task ID, audit log и concurrency policy;
- self-hosted runner под отдельным unprivileged account;
- exact-head checkout/verification;
- deterministic test/build command;
- useful failure output;
- artifact published/available without owner SSH;
- private NPT corpus остаётся вне Git;
- cleanup/retention policy;
- no dependency on new mandatory paid service.

### F2 — `PLATFORM-STACK-SPIKE-001`

Эквивалентные Avalonia и Qt implementations проверяют representative shared UI/core scenarios.

Результат — accepted ADR, выбирающий один stack.

### F3 — `UI-CORE-FOUNDATION-001` + `DOMAIN-CORE-FOUNDATION-001`

UI Core:

- app shell;
- workspace/documents;
- multi-window;
- design system/UI Gallery;
- properties;
- tree/table controls;
- command system;
- basic shared canvas;
- русский user-facing text contract.

Domain Core:

- project;
- equipment;
- terminal;
- connection;
- topology graph;
- state;
- validation;
- persistence/versioning.

### F4 — `IMPORT-TO-SCHEME-VERTICAL-SLICE-001`

Первое реальное product proof:

```text
representative CSV/XLSX
→ map fields
→ staging
→ resolve ambiguities
→ build Equipment/Terminals/Connections
→ topology validation
→ auto-layout
→ manual correction
→ save/reopen
→ changed source re-import
→ reconciliation
→ manual layout preserved
```

### F5 — Scheme Studio MVP foundation

Расширить Equipment Library, controlled symbols, scheme hierarchy/views, connectors/routing, alignment/grid/layout tools, searches/diagnostics, ГОСТ profile validation и output.

### F6 — NPT Compatibility Module

Интегрировать proven lossless cores и signal catalog под новой model.

Приоритет:

1. renderer fidelity для known mnemonic screenshots;
2. safe existing-object editing;
3. typed NPT properties;
4. XTABL data-driven generator/editor;
5. topology extraction experiment;
6. broader creation/editing только после native compatibility proof.

### F7 — clean-sheet Switching / TBP Module

Реализовать заново поверх shared topology/state/compliance model.

Первые rules берутся из current verified normative sources и synthetic/cleared examples, а не из old TBP code/config.

### F8 — Interlocks и advanced operational simulation

Добавить explainable logical/project interlocks и richer simulation после доказательства качества topology/state model.

### F9 — EOD feasibility / optional adapter

Может быть исследован раньше небольшим architecture spike, но production integration начинается только после стабилизации standalone boundaries.

## 5. Порядок зависимостей

```text
Foundation
   ↓
Infrastructure
   ↓
Platform Stack Decision
   ↓
UI Core ─────────────┐
Domain Core ─────────┼→ Import-to-Scheme vertical slice
Compliance Core ─────┘
                         ↓
                Scheme / NPT / Switching
```

## 6. Приоритеты product acceptance

При конфликте компромиссов приоритет:

1. engineering correctness и safety boundary;
2. traceable/preservable data;
3. practical user workflow и visible usability;
4. development iteration cost;
5. broad feature count.

## 7. Stop conditions против overengineering

Не создавать:

- generalized plugin API до появления конкретных повторяющихся module needs;
- generalized rule language до реальных repeated patterns;
- generalized layout ontology до representative schemes;
- distributed services там, где достаточно in-process calls;
- universal symbol DSL до proof реальных profile requirements;
- unrestricted development agent/shell surface там, где достаточно bounded typed operations.