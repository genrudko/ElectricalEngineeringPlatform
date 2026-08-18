# Platform Stack Spike — Avalonia vs Qt 6

Статус: canonical decision contract  
Decision: **PENDING — DO NOT SELECT BY PREFERENCE**

## 1. Objective

Choose the desktop/UI/runtime stack using equivalent executable evidence on realistic electrical-engineering workloads.

Final candidates:

```text
A) C# / .NET + Avalonia
B) C++ + Qt 6 / QML
```

Historical Tauri/Vue/TypeScript/SVG work in `genrudko/electroscheme-studio` remains research evidence, not a final candidate under the new unified scope.

## 2. Decision dimensions

The winning stack minimizes **total project risk and iteration cost**, not only raw FPS.

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
11. build time and feedback-loop duration;
12. agent/code-maintenance complexity for a small team.

Weights/thresholds are written before final results are reviewed.

## 3. Equivalent architecture

Both candidates implement the same conceptual layers:

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

Use idiomatic but comparable approaches; do not intentionally sabotage one candidate.

## 4. Required executable scenarios

### A — Application shell

- launch native desktop app;
- open/create sample project;
- document tabs;
- properties panel;
- equipment tree/table;
- status/diagnostics;
- design-system sample.

### B — Multi-window/workspace

- detach one document into second top-level window;
- persist/restore positions;
- manual mixed-monitor/scale test;
- simulate missing second monitor and recover layout.

### C — Heavy scheme canvas

Render semantic electrical symbols/connections, not arbitrary rectangles only.

Datasets:

```text
S:   2,000 visible objects
M:  10,000 visible objects
L:  25,000 visible objects
XL: 50,000 visible objects or platform limit with documented reason
```

Include symbols, labels, buses/lines/routes, state variation, selection overlays, hit-test spatial index and zoom/pan.

Measure startup/load, first render, zoom/pan frame timing, hit-test latency, drag latency, memory/CPU, update cost and screenshot/export time.

If item-per-object fails, candidate may use custom scene/drawing layer, but implementation complexity is part of score.

### D — Electrical edit path

- select breaker/disconnector;
- move representation without changing topology;
- reconnect semantic terminal as explicit command;
- undo/redo;
- edit typed property;
- state change updates representation;
- validation error locates item.

### E — 100k equipment table

- 100,000 rows;
- text/numeric/enum/status columns;
- virtualized scrolling;
- sort/filter/search;
- multi-select;
- edit through command path;
- copy selected rows.

### F — Import review

Representative staging diff table:

- 10k source rows;
- resolved/new/changed/conflict states;
- conflict filter;
- inline resolution editor;
- apply small ImportPlan.

### G — UI Gallery / visual testing

- render shared controls/states without full product;
- capture reproducible screenshot headlessly or in virtual display;
- compare baseline/tolerance;
- produce CI artifact.

If true headless rendering is impossible, document nearest reliable strategy/cost.

### H — print/vector output

- render representative single-line page;
- export PDF/vector or equivalent deterministic print representation;
- verify text/line geometry;
- open output in independent viewer during owner acceptance.

### I — packaging

For Windows and Linux:

- clean build from pinned environment;
- package runnable preview;
- measure package size/startup/memory;
- list external runtime dependencies;
- restore/run artifact from clean directory.

## 5. Development-loop benchmark

For each candidate measure wall-clock/developer steps for:

1. UI-only: adjust Inspector spacing/icon alignment → targeted compile/test → Gallery screenshot → preview.
2. Domain rule: add one equipment validation rule + tests.
3. Canvas behavior: change selection handle behavior + interaction test.

Record changed files/LOC, cold/warm compile time, runner time, artifact time, framework boilerplate and debugging effort.

This metric has explicit decision weight.

## 6. Domain core comparison

Implement the same tiny model/tests:

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

Compare type safety, test ergonomics, serialization/versioning options, graph/domain complexity, property/fuzz testing and debugging/profiling.

Do not build full Domain Core during spike.

## 7. Native integration

Prove file open/save, clipboard structured data, drag/drop file, print/export, top-level window management, safe paths and HiDPI reporting through adapters.

No framework object becomes canonical project serialization.

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

Before selection, record exact framework/version licenses/distribution obligations, linking implications, commercial-license dependencies if any, third-party components, support policy and security/update strategy.

Do not rely on remembered license summaries.

## 10. Scoring

Show raw measurements first, then scoring.

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

Qt wins if measured performance/desktop capabilities provide a **material advantage** large enough to justify higher implementation/toolchain complexity.

Avalonia wins if it meets representative performance/desktop thresholds with simpler/faster maintainable C#/.NET development.

If neither meets mandatory thresholds, stop and revisit architecture.

## 12. Relationship to old Tauri spike

Historical Tauri evidence remains useful for packaging baseline, native-dialog/clipboard/drop lessons, deterministic artifact methodology and Visio tooling boundary. It is not an automatic winner.

## 13. Outputs

- both candidate implementations;
- benchmark datasets/generator;
- raw measurements;
- screenshots/videos as needed;
- package artifacts;
- dependency/license report;
- comparative matrix;
- owner manual acceptance record;
- final ADR selecting stack;
- target repository layout and immediate UI/Domain work item.

## 14. Prohibited scope

No full product migration, full symbol library, full NPT renderer, full switching engine, design-system perfection or stack selection before equivalent measurements.
