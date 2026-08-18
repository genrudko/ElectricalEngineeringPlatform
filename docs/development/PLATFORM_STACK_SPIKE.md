# PLATFORM-STACK-SPIKE-001 — Avalonia vs Qt 6

Статус: **G0 FROZEN / OWNER-ACCEPTED RESEARCH BASIS / P1 NOT STARTED**  
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
- P1 implementation начинается только после отдельной owner-команды после acceptance frozen G0.

## 5. G0 — frozen environment/version contract

G0 research и read-only Linux inventory приняты owner. Значения ниже являются frozen comparison baseline для `PLATFORM-STACK-SPIKE-001`.

### 5.1. Exact comparison pins

| Item | Frozen G0 value | Freeze rule |
|---|---|---|
| Avalonia | `12.1.1` | exact framework patch |
| Avalonia Desktop | `12.1.1` | exact package version |
| Avalonia Fluent Theme | `12.1.1` | exact package version |
| .NET SDK | `10.0.302` | exact SDK; `global.json`, `rollForward=disable`, `allowPrerelease=false` |
| .NET Runtime | `10.0.10` | exact servicing runtime |
| Avalonia TFM | `net10.0` | Windows/Linux |
| Qt | `6.11.1` | exact Qt patch |
| Linux C++ | GCC/G++ `13.3.0`, Ubuntu package `13.3.0-6ubuntu2~24.04.1` | existing runner compiler |
| Windows C++ | MSVC v143 / Visual Studio 2022 `17.14`, x64 | `windows-2022`; exact compiler fingerprint recorded per run |
| Windows SDK | `10.0.22621.0` | exact selected SDK |
| CMake | `4.4.2` | exact version on Windows/Linux |
| Ninja | `1.13.2` | exact version on Windows/Linux |
| Qt Online Installer | `4.11.0` | checksum-verified acquisition tool |

The GitHub-hosted `windows-2022` image is mutable. G0 freezes supported toolchain family and selected Windows SDK, while every formal Windows run records:

- runner image/version;
- Visual Studio version;
- VC Tools version;
- `cl /Bv`;
- Windows SDK;
- CMake;
- Ninja.

Formal Avalonia-vs-Qt paired Windows evidence is valid only when both candidate runs use the same image/toolchain fingerprint. If the hosted image changes between candidate runs, the affected pair is rerun.

### 5.2. Dependency/acquisition tooling

.NET dependency policy:

- repository-root `global.json` exact SDK pin;
- repository-root `Directory.Packages.props` when candidate implementation exists;
- explicit direct package versions;
- committed `packages.lock.json`;
- CI restore in locked mode.

Qt build/dependency baseline:

- CMake + Ninja are mandatory build-system tooling;
- baseline does not require qmake;
- baseline does not require Qt Creator;
- baseline does not require Conan;
- baseline does not require vcpkg;
- baseline does not require third-party `aqtinstall`;
- no custom Qt mirror is introduced in G0/P1.

Qt acquisition is frozen as:

```text
Windows:
qt-online-installer-windows-x64-4.11.0.exe
sha256: ae919bc9b224b8ccdada69ec787a9f69330001f227f3fcbfb4a11a4adb3786f6
requested component: qt.qt6.6111.win64_msvc2022_64
requested Qt patch: 6.11.1

Linux:
qt-online-installer-linux-x64-4.11.0.run
sha256: 40b76bdf74f6a396341efb70ae2e754fcd878474babb6cd9d7f07eff12a85c62
requested component: qt.qt6.6111.linux_gcc_64
requested Qt patch: 6.11.1
```

Installer hash + requested component являются acquisition identity, но **не считаются окончательной identity фактически установленного Qt payload**.

После первой фактической P1 acquisition для каждого relevant environment обязательно сохраняются:

- installed Qt version;
- exact installed component inventory;
- generated/available Qt SPDX SBOM;
- deployment/runtime manifest;
- SHA-256 manifest/deployment inventory;
- installer version/hash;
- requested component;
- acquisition Git SHA/workflow run provenance.

Это является **P1 materialization of G0 acquisition identity**, а не новым G0 research и не поводом создавать собственный Qt mirror.

### 5.3. Existing Linux execution baseline

Accepted read-only inventory evidence:

```text
workflow run: 32186577578
runner: eep-development-vps
host: eod-development-vps
user: eep-runner
uid: 1002

OS: Ubuntu 24.04.4 LTS
kernel: 6.8.0-137-generic
glibc: 2.39

CPU:
4 vCPU
Intel Xeon Platinum 8173M @ 2.00 GHz
KVM

RAM: ~7.71 GiB
swap: ~2 GiB
root disk free: ~42.1 GiB

Git: 2.43.0
Python: 3.12.3
GCC/G++: 13.3.0

missing at G0 inventory time:
dotnet
cmake
ninja
Qt 6 toolchain/dev tree
```

G0 inventory ничего не устанавливал и не изменял runtime configuration.

Перед P1 installation отдельно read-only проверяются:

- Avalonia native Linux runtime dependencies;
- X11/font configuration;
- Xvfb/virtual-display availability;
- Qt display/OpenGL runtime dependencies.

Не проверенная dependency не объявляется missing только по предположению. Clang не является baseline compiler.

### 5.4. Windows build/package lane

Primary formal lane:

```text
GitHub-hosted windows-2022 x64
```

Properties:

- clean ephemeral VM per job;
- no permanently running owner PC;
- exact environment fingerprint recorded in every run;
- Release build/test/package;
- downloadable GitHub Actions artifact;
- suitable for automated build/package/unit/headless checks;
- not authority for physical GPU/input/DPI performance.

Free disk is recorded before and after tool acquisition. If Qt acquisition/build cannot execute reliably within the standard runner limits, this becomes formal evidence and a fallback returns to owner review. No paid/larger/self-hosted Windows runner is created automatically.

Owner-PC self-hosted runner, dedicated Windows self-hosted host and Linux cross-build are not G0 baseline.

### 5.5. Reference physical Windows acceptance environment

Reference machine is the owner's current primary physical Windows x64 workstation.

It runs the exact downloadable GitHub artifact without requiring a development environment.

It is authority for:

- physical UI interaction;
- GPU/renderer behavior;
- input latency;
- DPI/multi-monitor behavior;
- desktop feel.

It is **not** authority for canonical compilation.

One fingerprint block is required before P2/P3 physical performance evidence:

- Windows edition/version/build;
- display language/locale;
- CPU exact model;
- physical/logical core count;
- RAM;
- GPU adapter(s);
- GPU driver version;
- storage type/free space;
- AC/battery state and power mode;
- monitor count/model where available;
- per-monitor native/effective resolution;
- per-monitor refresh rate;
- per-monitor scaling;
- primary monitor;
- HDR state;
- ClearType/font-smoothing state;
- Hardware Accelerated GPU Scheduling state where available;
- mixed-DPI capability.

Missing this fingerprint does not block P1 after accepted G0.

### 5.6. Development Bridge decision

```text
G0/P1: NO NEW BRIDGE PROFILE
```

Formal Linux execution is already covered by the self-hosted GitHub runner. Formal Windows execution is covered by the GitHub-hosted Windows lane. Physical Windows evidence is produced on the owner workstation.

Bridge is expanded only after measured evidence that one specific narrow allowlisted operation materially improves the development loop. Convenience alone is not sufficient reason to expand the remote execution surface.

### 5.7. Comparison pins are not production/Foundation pins

```text
G0 comparison pins
!=
post-selection product/Foundation pins
```

G0 versions exist to make **this spike** reproducible. They do not become a long-lived product baseline merely because the spike ran on them.

After a candidate is selected and **before** `UI-CORE-FOUNDATION-001` / `DOMAIN-CORE-FOUNDATION-001`:

- re-check the then-current supported/stable/LTS framework/runtime/toolchain baseline;
- evaluate moving to the current supported patch/branch;
- assess compatibility/evidence impact of that move;
- explicitly accept a separate production/Foundation version pin.

`Avalonia 12.1.1`, `.NET SDK 10.0.302` / runtime `10.0.10`, `Qt 6.11.1` and the rest of this G0 table therefore must not be copied automatically into the post-selection Foundation contract.

Изменение G0 framework/toolchain version **внутри comparative spike после начала measurements** требует documented rerun обоих candidates либо явного доказательства, почему comparability не нарушена.

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
- frame time raw samples;
- input-to-paint latency;
- hit-test latency;
- drag latency;
- update latency;
- CPU;
- working set/private memory;
- allocations/GC where applicable;
- renderer/backend;
- screenshot/export duration.

Canonical `p50 / p95 / p99` не вычисляются независимо candidate applications; statistical authority определён в §17.

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

### 8.3. Mandatory table SLO and Avalonia implementation freedom

На agreed reference machine:

- scrolling `p95 ≤ 33.3 ms/frame`;
- normal cell/selection feedback `≤100 ms`;
- simple full-dataset sort/filter `≤2 s`;
- no 100k-row UI-object materialization;
- mandatory baseline не зависит от commercial grid component.

Mandatory Avalonia P3 gate должен быть пройден **любым idiomatic FOSS implementation**, не требующим нового обязательного commercial component/service.

Допустимые paths включают, но не ограничиваются:

- `Avalonia.Controls.DataGrid 12.1.2`;
- FOSS `TableView` + собственный editing/virtualization layer;
- другую разумную framework-native/FOSS composition.

Конкретный Avalonia P3 implementation path выбирается **в P3 по evidence**, а не навязывается G0.

`Avalonia.Controls.DataGrid 12.1.2` остаётся доступным MIT/FOSS reference option; его deprecated / bug-fix-only status фиксируется как maintenance-risk evidence, а не как mandatory architecture.

Current Avalonia `TreeDataGrid` commercial/Pro рассматривается только как **optional evidence** и не может закрыть mandatory baseline gate.

Mandatory requirement остаётся:

```text
100k editable table
+ virtualization
+ required interactions
+ no new mandatory commercial component/service
```

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

Для framework comparison используется `Noto Sans v2.015`, source commit `c4a321e123e4d4ff315f57f4e0adf294fe3a95be`, license `SIL OFL 1.1`.

Selected Regular 400 and SemiBold 600 font binaries получают exact SHA-256 в fixture manifest до первого visual evidence. Отдельно выполняется system-font/fallback check.

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

Canonical fixture schema:

```text
eep.stack-fixture/v1
```

Payload:

```text
manifest.json
scene.jsonl
equipment.jsonl
import_review.jsonl
typography.txt
```

Encoding:

```text
UTF-8
```

Generator:

```text
PRNG: SplitMix64
seed: 0x454550535441434B
```

Semantic IDs/data являются framework-neutral.

`manifest.json` stores:

- fixture schema;
- generator version;
- PRNG algorithm;
- seed;
- expected semantic counts;
- SHA-256 каждого payload;
- SHA-256 каждого font file.

Canonical fixture identity определяется generated payload hashes, а не seed alone.

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

Canonical G0 lane — GitHub-hosted `windows-2022` x64.

Workflow records exact image/toolchain fingerprint and publishes portable output through GitHub Actions artifacts.

The hosted lane is authority for reproducible build/package/automated-test evidence, but not physical GPU/input/DPI performance.

Owner-PC self-hosted runner, dedicated Windows host and Linux cross-build are fallback options, not baseline.

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

### 16.1. Avalonia/.NET baseline

P1-P4 framework/runtime baseline:

- `Avalonia 12.1.1`;
- `Avalonia.Desktop 12.1.1`;
- `Avalonia.Themes.Fluent 12.1.1`;
- `.NET SDK 10.0.302`;
- `.NET Runtime 10.0.10`;
- transitive runtime/rendering dependencies are captured from lock/deployment inventory.

For P3, `Avalonia.Controls.DataGrid 12.1.2` is an available MIT/FOSS reference option, but **not a mandatory G0-selected table architecture**. Its deprecated / bug-fix-only status is maintenance-risk evidence.

Current commercial/Pro Avalonia `TreeDataGrid` is optional evidence only and cannot close a mandatory baseline gate.

Mandatory commercial component/service:

```text
NO
```

### 16.2. Qt baseline

P1-P4 baseline modules:

- Qt Core;
- Qt Gui;
- Qt Qml;
- Qt Quick;
- Qt Quick Controls;
- Qt Test / Qt Quick Test for tests.

`QtQuick.Layouts`, `QtQuick.Shapes` and normal QML functionality remain inside the Qt Quick/QML baseline.

P4 PDF generation may use `QPdfWriter` from Qt Gui. Qt PDF is not a mandatory baseline module.

Baseline excludes GPL-only/additional modules unless an owner-approved requirement explicitly reopens them.

Mandatory commercial component/service:

```text
NO
```

This is conditional on the actual distribution model satisfying applicable LGPLv3 obligations. Baseline packaging therefore records dynamic/runtime deployment model, licenses/notices and relevant redistribution/relinking obligations.

Qt final distributed-payload identity is established from the P1 installed component inventory + available/generated Qt SPDX SBOM + deployment/runtime manifest + SHA-256 inventory and acquisition provenance, not from installer filename/hash alone.

Mandatory gate:

1. core spike scenario не зависит от нового обязательного commercial component/service;
2. никакая licensing obligation не скрывается;
3. dependency с incompatible distribution requirements блокирует candidate до resolution;
4. optional commercial component/control/service может исследоваться как дополнительный path для любого candidate, но **не считается baseline evidence и не закрывает failed mandatory gate**.

G0 и final dependency/license report опираются на accepted authoritative licensing research и фактический P1–P4 distributed dependency inventory.

Это engineering distribution inventory, а не замена final legal review.

## 17. Evidence provenance, raw schemas and canonical aggregation

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

### 17.1. Release measurement policy

Formal measurements:

- `Release` configuration only;
- exact Git SHA;
- debugger detached;
- profiler detached for formal samples;
- cold-start and warmed runs recorded separately;
- diagnostic profiler runs are not formal benchmark samples.

Initial baseline does not enable:

- PGO;
- LTO;
- NativeAOT;
- ReadyToRun;
- candidate-specific optimization not equivalently available for the other candidate.

Qt:

```text
Release
CMake 4.4.2
Ninja 1.13.2
```

Avalonia:

```text
Release
net10.0
.NET SDK 10.0.302
```

Packaging mode/runtime inclusion is recorded explicitly.

Renderer/backend is not forced at G0. Every run records actual renderer/compositor, graphics API/backend, GPU adapter/driver, hardware/software rendering flag, viewport pixel dimensions and DPI/scale. Any later renderer override must be explicit and symmetric.

### 17.2. Environment record

Canonical schema:

```text
eep.environment/v1
```

Minimum logical shape:

```json
{
  "schema": "eep.environment/v1",
  "work_item": "PLATFORM-STACK-SPIKE-001",
  "candidate": "avalonia|qt",
  "git_sha": "40-char SHA",
  "run_id": "string",
  "timestamp_utc": "ISO-8601",
  "os": {
    "name": "string",
    "version": "string",
    "build": "string",
    "kernel": "string",
    "arch": "x64"
  },
  "host": {
    "cpu_model": "string",
    "logical_cpus": 0,
    "ram_bytes": 0,
    "disk_free_bytes": 0
  },
  "gpu": [
    {
      "name": "string",
      "driver": "string"
    }
  ],
  "displays": [
    {
      "width_px": 0,
      "height_px": 0,
      "scale": 1.0,
      "refresh_hz": 0
    }
  ],
  "toolchain": {
    "dotnet_sdk": "string|null",
    "dotnet_runtime": "string|null",
    "compiler": "string",
    "compiler_detail": "string",
    "windows_sdk": "string|null",
    "cmake": "string|null",
    "ninja": "string|null",
    "runner_image": "string|null"
  },
  "framework": {
    "name": "avalonia|qt",
    "version": "string"
  },
  "build": {
    "configuration": "Release",
    "options": {}
  },
  "renderer": {
    "name": "string",
    "backend": "string",
    "software": false
  },
  "fixture": {
    "schema": "eep.stack-fixture/v1",
    "manifest_sha256": "hex"
  },
  "font": {
    "family": "Noto Sans",
    "version": "2.015",
    "files_sha256": []
  }
}
```

No credentials, secrets or personal usernames are written to environment evidence.

### 17.3. Raw benchmark record

Canonical schema:

```text
eep.stack-benchmark/v1
```

Minimum logical shape:

```json
{
  "schema": "eep.stack-benchmark/v1",
  "work_item": "PLATFORM-STACK-SPIKE-001",
  "candidate": "avalonia|qt",
  "git_sha": "40-char SHA",
  "environment_sha256": "hex",
  "fixture_sha256": "hex",
  "scenario": "string",
  "scene_tier": "S|M|L|XL|null",
  "viewport_mode": "NORMAL|DENSE|ZOOM_TO_FIT|null",
  "warmup_iterations": 0,
  "iterations": 0,
  "samples": [
    {
      "metric": "string",
      "unit": "string",
      "values": []
    }
  ],
  "status": "pass|fail|error",
  "errors": []
}
```

Raw `samples` are authoritative benchmark measurements.

Raw benchmark records do **not** contain:

- `winner`;
- `recommended_stack`;
- final `score`.

Candidate applications may emit provisional/local summaries for diagnostics, but those summaries are not canonical decision evidence.

### 17.4. Shared canonical percentile aggregator

Canonical measurement chain:

```text
candidate app
→ RAW SAMPLES
→ shared stack-neutral aggregator
→ p50 / p95 / p99
→ decision/scoring evidence
```

One shared stack-neutral aggregator is the **only authority** for canonical `p50 / p95 / p99` and any subsequent scoring inputs derived from benchmark distributions.

Before comparative benchmark runs begin, that aggregator must freeze and version:

- percentile definition;
- sample ordering rules;
- interpolation policy;
- treatment of NaN/invalid/error samples;
- minimum sample-count validation;
- units/normalization rules;
- aggregator version/hash.

Both candidates feed the same aggregator from `eep.stack-benchmark/v1` raw records. Candidate-specific percentile implementations cannot become canonical evidence.

Aggregator output is generated evidence derived from immutable raw records; it references raw-record SHA-256 and environment/fixture identity.

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

Raw measurements и canonical shared-aggregator summaries всегда показываются рядом со score.

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
Owner-accepted research basis, exact comparison pins, acquisition/provenance policy, Linux baseline, Windows lane, fixture/font policy, evidence schemas, shared aggregation authority и production-pin boundary зафиксированы до P1/comparative optimization.

Physical owner-PC fingerprint может быть заполнен позже, но обязателен до P2/P3 physical performance evidence.

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
Automated raw measurements, shared canonical aggregation, provenance и visual artifacts воспроизводимы.

**G7 — Licensing/dependencies**  
Exact distributed inventory готов; mandatory baseline не зависит от нового обязательного commercial component/service.

**G8 — Owner manual acceptance**  
Оба exact Windows artifacts проверены side-by-side.

**G9 — Decision**  
Только после G0–G8 разрешён accepted Platform Stack ADR. После selection выполняется отдельный current version/support review и production/Foundation pin acceptance до перехода к `UI-CORE-FOUNDATION-001` + `DOMAIN-CORE-FOUNDATION-001`.

## 21. Outputs

- обе candidate implementations;
- shared benchmark datasets/generator;
- raw measurements;
- shared canonical percentile aggregator + generated summaries;
- environment/provenance records;
- screenshots/videos as needed;
- Windows/Linux portable package artifacts;
- documented Windows build/package lane;
- dependency/license report;
- comparative weighted matrix;
- owner manual acceptance record;
- final Platform Stack ADR;
- post-selection Foundation version-pin review;
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

G0 frozen contract зафиксирован на owner-accepted research basis.

**P1 implementation NOT STARTED.**  
**Decision PENDING.**  
Следующий шаг — отдельная owner/координаторская приёмка frozen G0 и только затем отдельная owner-команда на `P1 — Professional Shell / UI Gallery`.
