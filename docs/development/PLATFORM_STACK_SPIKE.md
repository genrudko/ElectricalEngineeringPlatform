# PLATFORM-STACK-SPIKE-001 — Avalonia vs Qt 6

Статус: **ACTIVE CONTRACT / IMPLEMENTATION NOT STARTED**  
Issue: #5  
Decision: **PENDING — НЕ ВЫБИРАТЬ ПО ПРЕДПОЧТЕНИЮ И НЕ ДЕЛАТЬ STACK-SELECTION CONCLUSIONS ДО RAW EVIDENCE + OWNER ACCEPTANCE**

## 1. Цель

Выбрать desktop/UI/runtime stack для Electrical Engineering Platform через две **эквивалентные executable implementations** representative electrical-engineering workloads:

```text
Candidate A — Avalonia 12 / C# / .NET
Candidate B — Qt 6 / C++ / QML
```

Решение принимается по reproducible evidence, а не по familiarity, popularity, remembered framework reputation, synthetic microbenchmarks или заранее предполагаемой сложности toolchain.

Historical Tauri/Vue/TypeScript/SVG work из `genrudko/electroscheme-studio` остаётся research evidence и не является final candidate.

## 2. Dependency и formal opening baseline

`INFRASTRUCTURE-SPIKE-001` accepted и merged через PR #4.

Formal opening `PLATFORM-STACK-SPIKE-001` выполнен от exact `main`:

```text
0ac64c731bbaa9e85cf6ba8419cc31b6a1c97c5f
```

GitHub остаётся canonical control plane. Existing VPS/self-hosted runner — Linux execution plane; owner Windows workstation — manual acceptance endpoint, но не единственный canonical Windows build environment.

## 3. Hard constraints

Оба candidates обязаны:

1. реализовать один и тот же functional contract и использовать одинаковые deterministic fixtures;
2. собираться из exact Git commit;
3. иметь Linux и reproducible Windows build/package path;
4. проходить mandatory baseline gates **без нового обязательного commercial component/service**;
5. сохранять neutral domain model вне UI-framework serialization;
6. показывать русский production-like UI;
7. выдавать raw machine-readable benchmark evidence до любого scoring;
8. позволять owner запустить готовый Windows artifact без установки development environment;
9. использовать idiomatic framework-native approaches без deliberate sabotage одного candidate;
10. не считаться выбранными до полного raw evidence и owner acceptance.

Optional commercial controls/services могут исследоваться отдельно как дополнительный evidence, но **не закрывают mandatory baseline gate ни для Avalonia, ни для Qt**.

Если candidate не проходит хотя бы один mandatory gate, weighted score не может этот failure компенсировать.

## 4. Обязательная последовательность work item

Implementation выполняется **последовательно внутри одного work item**:

```text
G0 — freeze contract/environment
↓
P1 — Professional Shell / UI Gallery
↓
P2 — Semantic Scheme Canvas
↓
P3 — Engineering Data Workspace
↓
P4 — Multi-window / platform integration
↓
Packaging + reproducible evidence
↓
Owner side-by-side acceptance
↓
Decision / ADR
```

Правила sequence:

- не разращивать обе candidate apps параллельно за пределами текущего gate;
- следующий prototype stage начинается только когда предыдущий stage имеет comparable baseline у обоих candidates;
- framework-specific optimization допустима после появления equivalent baseline и должна быть явно зафиксирована;
- P1 implementation не начинается до отдельной owner-команды после formal opening этого contract.

## 5. G0 — environment/version freeze

Все exact framework/SDK/compiler/toolchain versions до G0 являются **provisional**.

Непосредственно перед freeze нужно повторно проверить по authoritative/current sources и зафиксировать:

- exact Avalonia patch version;
- exact .NET SDK/runtime version;
- exact Qt patch version;
- exact C++ compiler/toolchain/CMake/Ninja or equivalent versions;
- dependency/package-manager versions;
- build configuration;
- Linux runner identity/OS;
- Windows build execution mechanism;
- reference Windows acceptance machine fingerprint;
- CPU/RAM/GPU/renderer;
- display resolution/scaling;
- benchmark fixture version и random seed;
- font fixture version.

Ни одна exact version, названная до G0 в chat/research/issue, не считается frozen contract value.

Изменение framework/toolchain version после начала comparative measurements требует documented rerun обоих candidates либо явного доказательства, почему comparability не нарушена.

Перед первым implementation/build выполняется короткий Infrastructure preflight:

- current GitHub exact head;
- self-hosted runner usable;
- disk/RAM/build tools state;
- Bridge health/repository access, только если Bridge будет участвовать в interactive loop.

Full повтор `INFRASTRUCTURE-SPIKE-001` не требуется.

## 6. P1 — Professional Shell / UI Gallery

Каждый candidate реализует одинаковый минимальный professional desktop shell:

- native desktop app launch;
- document tabs;
- equipment tree;
- Properties Inspector;
- status/diagnostics area;
- toolbar/menu/commands;
- forms/buttons/inputs/validation states;
- design-system/UI Gallery sample;
- русский user-facing text.

Цель — оценить dense professional desktop UI, а не framework Hello World.

P1 не является full UI Core и не должен перерасти в production design-system implementation.

## 7. P2 — Semantic Scheme Canvas

Canvas обязан работать с electrical semantics, а не только arbitrary rectangles.

Минимальные entities:

- `CircuitBreaker`;
- `Disconnector`;
- `EarthingSwitch`;
- `Busbar`;
- semantic `Terminal`;
- routed electrical connections;
- equipment labels/designations;
- state-dependent representation;
- selection/hover/highlight;
- validation markers.

Обязательные interactions:

- zoom/pan;
- hit testing;
- select;
- drag representation;
- reconnect semantic terminal;
- typed property/state update;
- undo/redo.

Перемещение visual representation не изменяет topology. Reconnect выполняется через explicit command path.

### 7.1. Scene-size benchmark

Размер **всей semantic scene**:

| Tier | Total semantic scene |
|---|---:|
| S | 2 000 |
| M | 10 000 |
| L | 25 000 |
| XL | 50 000 |

Это не означает, что все entities обязаны одновременно быть fully rendered в viewport.

### 7.2. Viewport-density benchmark

Отдельно измеряются режимы:

- `NORMAL` — ориентир около 500 visible semantic elements;
- `DENSE` — ориентир около 2 000 visible semantic elements;
- `ZOOM_TO_FIT` — stress case с максимально большой долей scene на экране.

Candidate должен использовать разумный culling/spatial-index/custom-drawing strategy, если item-per-object architecture становится узким местом. Implementation complexity такого path учитывается как evidence, а не автоматически как defect.

### 7.3. Operations

Для relevant tiers измеряются:

- load fixture;
- first render;
- scripted pan;
- scripted zoom;
- repeated hit-test;
- selection;
- drag;
- reconnect;
- 1% bulk state update;
- validation overlay update;
- screenshot/export.

### 7.4. Raw measurements

Записываются минимум:

- time to first interactive;
- frame time `p50 / p95 / p99`;
- input-to-paint latency;
- hit-test latency;
- drag latency;
- update latency;
- CPU;
- working set/private memory;
- allocations/GC where applicable;
- renderer/backend;
- screenshot/export duration.

### 7.5. Proposed responsiveness gate

На agreed reference Windows machine:

- normal interactive canvas: preferred `p95 ≤ 16.7 ms`;
- mandatory `p95 ≤ 33.3 ms`;
- hit test `p95 ≤ 25 ms`;
- drag/reconnect visual feedback `p95 ≤ 50 ms`;
- no unexplained UI stall `>250 ms` during routine M/L interaction;
- XL должен оставаться operable без crash/OOM.

`ZOOM_TO_FIT XL` — stress measurement, а не artificial 60-FPS hard gate.

Memory сравнивается raw и relative; repeated interaction не должен давать необъяснимый unbounded growth.

## 8. P3 — Engineering Data Workspace

Один representative workspace содержит:

### 8.1. Equipment Table

- 100 000 rows;
- text/numeric/enum/status/identifier columns;
- virtualized scrolling;
- sort;
- filter;
- search;
- multi-selection;
- editable cells;
- copy selected rows;
- keyboard navigation.

### 8.2. Import Review

- 10 000 source rows;
- resolved/new/changed/conflict states;
- conflict filter;
- inline resolution editor;
- apply small `ImportPlan`.

### 8.3. Mandatory table SLO

На agreed reference machine:

- scrolling `p95 ≤ 33.3 ms/frame`;
- normal cell/selection feedback `≤100 ms`;
- simple full-dataset sort/filter `≤2 s`;
- no 100k-row UI-object materialization;
- mandatory baseline не зависит от commercial grid component.

Optional commercial table/tree control может быть benchmarked отдельно для **любого** candidate, но не закрывает baseline gate.

## 9. Russian typography / desktop-quality gate

Оба candidates render одинаковый corpus:

- Cyrillic upper/lower case;
- `ё/Ё`;
- mixed Cyrillic/Latin engineering identifiers;
- `№`;
- `кВ`, `МВт`, `А`, `%`;
- long dispatcher/equipment titles;
- non-breaking spaces;
- en/em dash;
- mathematical minus;
- Greek symbols used in engineering notation;
- multiline validation/error text.

Проверяются scale factors:

- 100%;
- 125%;
- 150%;
- 200%.

Mandatory:

- no missing glyphs;
- no silent character substitution;
- no baseline/line-height corruption;
- no unintended clipping;
- correct ellipsis/wrapping;
- copy/paste preserves text.

Для framework comparison используется один pinned redistributable Cyrillic-capable font fixture; отдельно выполняется system-font/fallback check.

## 10. P4 — Multi-window / platform integration

Обязательные capabilities:

- detachable document;
- second native top-level window;
- focus/keyboard behavior;
- persist/restore workspace;
- missing-monitor recovery;
- file open/save;
- clipboard structured data;
- file drag/drop;
- PDF/vector export;
- runtime DPI reporting.

Manual Windows scenario:

1. открыть два documents;
2. detach один document;
3. переместить окно на второй monitor;
4. проверить focus/keyboard/menus/dialog ownership;
5. проверить mixed DPI, если hardware позволяет;
6. закрыть и восстановить workspace;
7. повторно запустить приложение без второго monitor.

Mandatory:

- окно не теряется off-screen;
- restored layout остаётся usable;
- dialogs появляются на sensible owner monitor;
- input/focus не ломается после detach;
- DPI transition не приводит к corrupt layout/rendering.

## 11. Shared deterministic data

Candidate-specific hand-written benchmark datasets запрещены.

Один stack-neutral fixture/generator создаёт одинаковые datasets и фиксированный random seed.

Для canvas отдельно записываются:

- semantic entity count;
- connection count;
- label count;
- actual framework visual/render-node count;
- viewport density.

Framework implementations могут иметь разное количество internal render objects — это measurement, а не искусственное условие equivalence.

## 12. Windows/Linux packaging gate

### 12.1. Linux

На existing self-hosted VPS runner:

- exact-head clean build;
- package;
- dependency inventory;
- restore artifact в clean directory;
- launch/smoke where environment permits;
- checksum.

### 12.2. Windows

Должен существовать reproducible build/package lane, независимый от unique developer-workstation state.

Допустимые механизмы оцениваются только по фактическому evidence:

- GitHub-hosted Windows lane, если он укладывается в текущие operational/cost constraints;
- controlled Windows self-hosted runner;
- genuine cross-build, только если полученный artifact затем доказан runnable на Windows;
- иной reproducible mechanism, явно принятый owner.

Owner PC может быть manual acceptance machine, но не единственным canonical compiler/package environment.

В spike обязательна portable preview; polished installer/update system не является обязательным.

Для package записываются:

- compressed/uncompressed size;
- runtime dependencies;
- first/cold launch time;
- idle memory;
- manifest;
- checksum;
- licenses/notices.

## 13. Development-loop benchmark

Для обоих candidates выполняются одни и те же три изменения.

### D1 — UI-only

Изменить spacing/alignment в Properties Inspector:

```text
edit → targeted build → targeted test → UI Gallery screenshot → preview artifact
```

### D2 — Domain

Добавить одну typed equipment validation rule и unit tests.

### D3 — Canvas

Изменить selection-handle behavior и соответствующий interaction test.

Каждый сценарий выполняется несколько раз и записывает:

- files changed;
- LOC changed;
- commands/actions;
- cold build;
- warm incremental build;
- targeted test duration;
- visual artifact duration;
- amount of framework-specific glue/boilerplate;
- number of failed/retry steps;
- debugging/profiling path.

Developer familiarity не используется как metric. IDE convenience может учитываться только как дополнительный evidence; CLI/repository reproducibility обязательна.

## 14. Minimal Domain Core comparison

Оба candidates реализуют одинаковый tiny model:

```text
ElectricalProject
Equipment
Terminal
Connection
TopologyGraph
EquipmentState including UNKNOWN
Command transaction
serialization round-trip
```

Сравниваются:

- type safety;
- test ergonomics;
- serialization/versioning burden;
- graph/model implementation complexity;
- property/fuzz-testing feasibility;
- debugging/profiling.

Full Domain Core в spike запрещён.

Framework object не становится canonical project serialization.

## 15. Visual/test evidence

Автоматически сохраняются:

- UI Gallery screenshots;
- representative canvas screenshots;
- table screenshots;
- benchmark JSON;
- environment JSON;
- test reports;
- package manifests;
- dependency/license reports;
- artifact checksums.

Headless/virtual-display screenshots допустимы для deterministic regression evidence.

Они **не заменяют** physical Windows desktop interaction benchmark для latency, DPI и desktop feel.

## 16. Licensing/dependency gate

Правило симметрично для обоих candidates.

До selection фиксируются:

- exact framework/version license;
- every direct dependency;
- relevant transitive dependencies;
- modules/components actually distributed;
- source/binary linking/distribution model where applicable;
- required notices;
- commercial-only dependencies;
- security/update mechanism;
- package/runtime footprint.

Mandatory gate:

1. core spike scenario не зависит от нового обязательного commercial component/service;
2. никакая licensing obligation не скрывается;
3. dependency с incompatible distribution requirements блокирует candidate до resolution;
4. optional commercial component/control/service может исследоваться как дополнительный path для любого candidate, но **не считается baseline evidence и не закрывает failed mandatory gate**.

Не использовать remembered license summaries как authority. G0 и final dependency/license report опираются на current authoritative licensing sources.

Это engineering distribution inventory, а не замена final legal review.

## 17. Evidence provenance

Каждый benchmark record содержит минимум:

- work item;
- candidate;
- Git commit SHA;
- build/run ID;
- exact framework/SDK/compiler versions;
- OS;
- CPU;
- RAM;
- GPU;
- graphics renderer/backend;
- display scale;
- build configuration;
- fixture version;
- seed;
- timestamp.

Benchmark numbers вручную из console в итоговую таблицу не переписываются. Final matrix строится из generated raw results.

## 18. Weighted decision matrix

Hard gates применяются **до scoring**.

| Dimension | Weight |
|---|---:|
| Heavy semantic canvas scalability | 30 |
| Desktop / multi-window / HiDPI / Russian typography | 15 |
| Development loop / maintainability | 15 |
| Large tables / Import Review | 10 |
| Windows/Linux packaging | 10 |
| Visual/automated testability | 8 |
| Licensing/dependency/maintenance footprint | 7 |
| Domain/test ergonomics | 5 |
| **Total** | **100** |

После завершения measurements каждая dimension получает `0…5`:

- `5` — materially exceeds target;
- `4` — passes with clear margin;
- `3` — passes target;
- `2` — technically passes, но требует material workaround/cost;
- `1` — serious deficiency;
- `0` — hard failure.

Weighted score:

```text
Σ(weight × score / 5)
```

Raw measurements всегда показываются рядом со score.

## 19. Decision rule

1. Candidate с failed mandatory gate не может быть выбран.
2. Если оба имеют mandatory failure — `NO_STACK_PASSES_CURRENT_CONTRACT`.
3. Если проходит только один — он может быть принят только после owner manual acceptance.
4. Если проходят оба — сравнивается weighted evidence.
5. Performance advantage одного candidate не оправдывает complexity автоматически; сравнивается **измеренная**, а не предполагаемая complexity.
6. Если итоговая разница `<5/100`, результат считается near-tie; решающим становится measured development/maintenance cost и owner desktop acceptance, а не один лучший microbenchmark.
7. **Никаких stack-selection conclusions, предварительного winner language или ADR выбора до полного raw evidence и owner side-by-side acceptance.**

Допустимые exit states:

```text
SELECT_AVALONIA
SELECT_QT
NO_STACK_PASSES_CURRENT_CONTRACT
```

## 20. Acceptance gates

**G0 — Frozen contract/environment**  
Versions, fixtures, weights, SLO, Windows lane и environment fingerprint зафиксированы до comparative optimization.

**G1 — Equivalent P1 functional apps**  
Обе implementations дают comparable Professional Shell / UI Gallery baseline.

**G2 — Semantic canvas**  
Обе implementations дают comparable P2; mandatory interaction SLO пройден либо documented failure фиксирует candidate limitation.

**G3 — Engineering Data Workspace**  
100k Equipment Table + 10k Import Review функциональны и responsive.

**G4 — Professional desktop/platform behavior**  
P4, Russian typography, keyboard/focus, multi-window и available multi-monitor/DPI scenarios проверены.

**G5 — Reproducible exact-head build/package**  
Linux и required Windows lanes воспроизводимо собирают exact commit; runnable portable artifacts восстановлены из clean output.

**G6 — Testing/evidence**  
Automated raw measurements, provenance и visual artifacts воспроизводимы.

**G7 — Licensing/dependencies**  
Exact inventory готов; mandatory baseline не зависит от нового обязательного commercial component/service.

**G8 — Owner manual acceptance**  
Оба exact Windows artifacts проверены side-by-side.

**G9 — Decision**  
Только после G0–G8 разрешён accepted Platform Stack ADR и переход к `UI-CORE-FOUNDATION-001` + `DOMAIN-CORE-FOUNDATION-001`.

## 21. Outputs

- обе candidate implementations;
- shared benchmark datasets/generator;
- raw measurements;
- environment/provenance records;
- screenshots/videos as needed;
- Windows/Linux portable package artifacts;
- documented Windows build/package lane;
- dependency/license report;
- comparative weighted matrix;
- owner manual acceptance record;
- final Platform Stack ADR;
- target repository layout;
- immediate UI/Domain next work item.

## 22. Relationship to old Tauri spike

Historical Tauri evidence может использоваться для packaging baseline, native-dialog/clipboard/drop lessons, deterministic artifact methodology и Visio tooling boundary.

Он не является automatic winner и не меняет candidate set.

## 23. Prohibited scope

В spike не входят:

- full product migration;
- full UI Core;
- full Domain Core;
- production Scheme Studio;
- full symbol library;
- full NPT renderer;
- full Switching engine;
- design-system perfection;
- installer/update framework perfection;
- custom generic widget library;
- framework-agnostic abstraction ради гипотетической будущей смены framework;
- optimization одного candidate, которую не дают эквивалентно попробовать второму;
- новый mandatory commercial component/service для прохождения baseline;
- final stack selection до raw evidence и owner acceptance.

## 24. Current stop point

Formal work item открыт, contract зафиксирован.

**P1 implementation пока не начинать.** Следующий шаг — отдельная owner-команда на начало G0.