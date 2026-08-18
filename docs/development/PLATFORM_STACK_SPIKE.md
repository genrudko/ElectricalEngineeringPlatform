# Platform Stack Spike — Avalonia vs Qt 6

Статус: канонический decision contract  
Decision: **PENDING — НЕ ВЫБИРАТЬ ПО ПРЕДПОЧТЕНИЮ**

## 1. Цель

Выбрать desktop/UI/runtime stack на основе equivalent executable evidence для representative electrical-engineering workloads.

Final candidates:

```text
A) C# / .NET + Avalonia
B) C++ + Qt 6 / QML
```

Historical Tauri/Vue/TypeScript/SVG work из `genrudko/electroscheme-studio` остаётся research evidence и не является final candidate по новой unified scope.

## 2. Decision dimensions

Winning stack минимизирует **total project risk и iteration cost**, а не только raw FPS.

Weighted areas:

1. heavy electrical canvas performance/scalability;
2. professional desktop UI quality/productivity;
3. large tree/table performance;
4. multi-window/multi-monitor/mixed-DPI correctness;
5. deterministic rendering/print/export path;
6. headless/visual testability;
7. domain-core/test productivity;
8. Windows/Linux packaging;
9. debugging/profiling/tooling;
10. dependency/licensing/maintenance burden;
11. build time и feedback-loop duration;
12. agent/code-maintenance complexity для small team.

Weights/thresholds фиксируются до final result review.

## 3. Equivalent architecture

Оба candidates реализуют одну conceptual structure:

```text
App Shell
UI Core
Minimal Domain Core
Scheme Canvas
Virtualized Equipment Table
Properties Inspector
Command/Undo path
Project persistence fixture
```

Использовать idiomatic, но comparable approaches; нельзя намеренно sabotage одного candidate.

## 4. Required executable scenarios

### A — Application Shell

- launch native desktop app;
- open/create sample project;
- document tabs;
- properties panel;
- equipment tree/table;
- status/diagnostics;
- design-system sample с русскими user-facing labels.

### B — Multi-window/workspace

- detach document во second top-level window;
- persist/restore positions;
- manual mixed-monitor/scale test;
- simulate missing second monitor и recover layout.

### C — Heavy Scheme Canvas

Render semantic electrical symbols/connections, а не только arbitrary rectangles.

Datasets:

```text
S:   2,000 visible objects
M:  10,000 visible objects
L:  25,000 visible objects
XL: 50,000 visible objects or platform limit with documented reason
```

Включить symbols, labels, buses/lines/routes, state variation, selection overlays, hit-test spatial index и zoom/pan.

Measure:

- startup/load;
- first render;
- zoom/pan frame timing;
- hit-test latency;
- drag latency;
- memory/CPU;
- update cost;
- screenshot/export time.

Если item-per-object architecture fail, candidate может использовать custom scene/drawing layer; implementation complexity учитывается в score.

### D — Electrical edit path

- select `CircuitBreaker`/`Disconnector`;
- move representation без topology change;
- reconnect semantic `Terminal` как explicit command;
- undo/redo;
- edit typed property;
- state change updates representation;
- validation error locates item.

### E — 100k Equipment Table

- 100,000 rows;
- text/numeric/enum/status columns;
- virtualized scrolling;
- sort/filter/search;
- multi-select;
- edit through command path;
- copy selected rows.

### F — Import Review

Representative staging diff table:

- 10k source rows;
- resolved/new/changed/conflict states;
- conflict filter;
- inline resolution editor;
- apply small `ImportPlan`.

### G — UI Gallery / visual testing

- render shared controls/states без full product;
- capture reproducible screenshot headlessly или в virtual display;
- compare baseline/tolerance;
- produce CI artifact.

Если true headless rendering недоступен, document nearest reliable strategy/cost.

### H — Print/vector output

- render representative single-line page;
- export PDF/vector или equivalent deterministic print representation;
- verify text/line geometry;
- открыть output в independent viewer при owner acceptance.

### I — Packaging

Для Windows и Linux:

- clean build from pinned environment;
- package runnable preview;
- measure package size/startup/memory;
- list external runtime dependencies;
- restore/run artifact from clean directory.

## 5. Development-loop benchmark

Для каждого candidate измерить wall-clock/developer steps для:

1. UI-only change: Inspector spacing/icon alignment → targeted compile/test → Gallery screenshot → preview.
2. Domain rule: add one equipment validation rule + tests.
3. Canvas behavior: change selection handle behavior + interaction test.

Record:

- changed files/LOC;
- cold/warm compile time;
- runner time;
- Bridge interactive task time where useful;
- artifact time;
- framework boilerplate;
- debugging effort.

Этот metric имеет explicit decision weight.

## 6. Domain Core comparison

Оба candidates реализуют same tiny model/tests:

```text
ElectricalProject
Equipment
Terminal
Connection
TopologyGraph
EquipmentState/UNKNOWN
Command transaction
serialization round-trip
```

Compare:

- type safety;
- test ergonomics;
- serialization/versioning options;
- graph/domain complexity;
- property/fuzz testing;
- debugging/profiling.

Full Domain Core в spike не строится.

## 7. Native integration

Доказать file open/save, clipboard structured data, drag/drop file, print/export, top-level window management, safe paths и HiDPI reporting через adapters.

Framework object не становится canonical project serialization.

## 8. Test matrix

Automated minimum:

- Linux runner build/test;
- Windows build/test where available;
- unit/domain tests;
- UI Gallery capture;
- canvas/table benchmarks;
- package/restore/start check.

Manual owner evidence:

- Windows interaction;
- visual quality;
- multi-monitor/mixed-DPI where hardware permits;
- input latency/desktop feel;
- independent PDF/print viewing.

## 9. Licensing/dependency gate

До selection записать exact framework/version licenses/distribution obligations, linking implications, commercial-license dependencies if any, third-party components, support policy и security/update strategy.

Не использовать remembered license summaries как authority.

## 10. Scoring

Сначала показать raw measurements, затем scoring.

| Dimension | Weight | Avalonia | Qt | Evidence |
|---|---:|---:|---:|---|
| canvas scalability | high | | | |
| UI/desktop quality | high | | | |
| DevEx iteration cost | high | | | |
| tables/tree | medium-high | | | |
| multi-monitor/HiDPI | high | | | |
| testing/visual CI | high | | | |
| packaging | medium | | | |
| domain/test productivity | medium-high | | | |
| maintenance/licensing | medium-high | | | |

## 11. Selection rule

Qt wins, если measured performance/desktop capabilities дают **material advantage**, достаточное для justification более высокой implementation/toolchain complexity.

Avalonia wins, если проходит representative performance/desktop thresholds с более простым/быстрым maintainable C#/.NET development.

Если ни один candidate не проходит mandatory thresholds, stop и revisit architecture.

## 12. Relationship to old Tauri spike

Historical Tauri evidence остаётся полезным для packaging baseline, native-dialog/clipboard/drop lessons, deterministic artifact methodology и Visio tooling boundary.

Он не является automatic winner.

## 13. Outputs

- оба candidate implementations;
- benchmark datasets/generator;
- raw measurements;
- screenshots/videos as needed;
- package artifacts;
- dependency/license report;
- comparative matrix;
- owner manual acceptance record;
- final ADR selecting stack;
- target repository layout;
- immediate UI/Domain next work item.

## 14. Prohibited scope

В spike не входят:

- full product migration;
- full symbol library;
- full NPT renderer;
- full Switching engine;
- design-system perfection;
- stack selection до equivalent measurements.