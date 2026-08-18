# Текущее состояние — Electrical Engineering Platform

Дата среза: **2026-08-18**  
Активная программа: `UNIFIED-FOUNDATION-001`

## GitHub factual state

- Canonical repository: `genrudko/ElectricalEngineeringPlatform`.
- Default branch: `main`.
- Bootstrap commit: `5aa73328917447fb410489e8f67685ee4907ae66`.
- Active issue: #1 `UNIFIED-FOUNDATION-001`.
- Active branch: `architecture/unified-foundation-001`.
- Foundation Draft PR: #2 `UNIFIED-FOUNDATION-001 — establish canonical platform foundation`.
- PR #2: OPEN / DRAFT / NOT MERGED на момент этого documentation snapshot.
- Previous working Foundation source: `genrudko/electroscheme-studio` issue #5 / Draft PR #6; теперь это historical migration evidence.
- Previous Tauri desktop spike остаётся в `genrudko/electroscheme-studio` issue #3 / Draft PR #4 как research evidence only.

Exact heads/compare/workflow state являются volatile и перед implementation/acceptance читаются напрямую из GitHub.

## Принятое направление продукта

Цель — один standalone, desktop-first/local-first **модульный электротехнический инженерный комплекс**, содержащий:

1. Scheme Studio;
2. NPT Engineering Toolkit / compatibility;
3. новый clean-sheet Switching / TBP module;
4. shared import/topology/state/normative/UI foundations.

`ElectricalProject` / neutral domain model является source of truth. Diagram geometry, CSV/XLSX, NPT files и switching-form documents — bounded views/imports/adapters.

## Принятый Foundation direction

- modular monolith;
- один neutral Domain Core;
- first-class UI Core;
- Compliance Core с versioned normative provenance;
- topology отделена от geometry;
- State model с first-class `UNKNOWN`;
- structured CSV/XLSX Import + reconciliation + auto-layout;
- ГОСТ/ЕСКД graphic profiles;
- switching rules с привязкой к current Russian energy-sector normative sources;
- enterprise/site/equipment/project policy overlays, которые не могут ослаблять applicable mandatory baseline;
- NPT compatibility за module/adapter boundary;
- optional EOD integration за strict feasibility/cost gate;
- GitHub как canonical control plane и существующий VPS как intended execution plane;
- risk-based CI и visual-first UI acceptance;
- final platform stack только после equivalent Avalonia-vs-Qt executable spike;
- русский язык как язык продукта/UI/документации;
- английский язык как язык internal technical/domain части с профессиональной электроэнергетической terminology.

## Owner decisions, добавленные во время Foundation

### Язык проекта

Canonical policy:

```text
Пользовательский интерфейс и documentation → русский
Internal technical/domain implementation → английский
```

Точные internal entities используют engineering English: `CircuitBreaker`, `Disconnector`, `EarthingSwitch`, `Busbar`, `TopologyGraph`, `SwitchingOperation`, `InterlockRule` и т. п.

Транслит в identifiers запрещён.

Canonical owners:

- `docs/project/LANGUAGE_POLICY.md`;
- `docs/project/RU_EN_ENGINEERING_GLOSSARY.md`.

Уже созданные Foundation docs с англоязычным narrative text должны быть приведены к этой политике **до финальной owner acceptance/merge PR #2**. Английские filenames и exact technical identifiers могут сохраняться.

### Старый TBP

Старый TBP project **не мигрируется** в новый продукт.

- no code migration;
- no YAML/rule migration;
- нет требования загружать/архивировать его для этого проекта;
- не используется как normative authority/test oracle.

Новый Switching module реализуется с нуля на Domain Core + Compliance Core + current verified normative sources.

### Внутренние инструкции enterprise/site

Internal enterprise/site instructions **не являются normal development inputs** и не должны загружаться в GitHub/ChatGPT/shared VPS corpus.

Продукт всё равно поддерживает fine-grained Local Policy Overlays на deployment, но shared development использует synthetic/cleared policy examples.

### Public normative sources

Government/sector acts и ГОСТ/ЕСКД source metadata получаются/re-verify online по authoritative sources при начале соответствующей compliance/profile работы. Вручную собранный большой normative document pack сейчас не требуется.

Для ГОСТ authoritative catalogue/status обязателен; detailed full-text extraction использует lawful source и не считает random internet copies нормативным authority.

## Ещё не доказано / не выбрано

- Avalonia vs Qt final selection;
- heavy-canvas performance на representative workload;
- exact native project package format/schema v1;
- complete equipment-type library;
- completeness/correctness NPT `nodes` как topology source;
- production-safe creation arbitrary new XSDE topology objects;
- full normative rule coverage;
- exact EOD bridge API/context-security/deployment cost;
- installer/update channel;
- final branding beyond repository/product working name.

## Важное inherited evidence

### ElectroScheme Studio source repository

Useful evidence/assets включают object/terminal/connection research, SVG/editor interaction experiments, VSDX/VSSX/ShapeSheet tooling, snapping/busbar/symbol research, desktop packaging/Tauri spike evidence и market/reference-product research.

Это migration inputs, а не автоматически принятая production architecture.

### NPT Engineering Toolkit / source materials

Current research baseline включает:

- ~475 parseable real XSDE files и large industrial corpus;
- lossless XSDE round-trip для studied corpus;
- identified `scd*` semantics и non-KKS exceptions;
- embedded `CustElem` и external `.menu` library role;
- large ASU/TECH/KKS signal catalog research;
- lossless XTABL v6.0 core для seven production tables / 1461 records;
- unknown/preserve-only XTABL fields;
- evidence, что NPT `nodes` topology-related, но нет доказательства complete neutral electrical graph;
- current Mnemo renderer fidelity остаётся недостаточной.

Full vendor/reference corpus остаётся вне GitHub.

### Старый TBP

Historical only. Не является migration source для нового Switching module.

## Normative baseline state

Foundation устанавливает registry mechanism, а не утверждает, что все требования уже encoded.

Initial high-priority source set включает ГОСТ 2.701-2008, ГОСТ 2.702-2011, ПОТЭЭ №903н, PTEEP №811, PTEES №1070, Switching Rules №757 и applicable ПУЭ chapters, которые учитываются отдельно, а не как одна synthetic version.

Каждое production rule требует current-source verification, source-level extraction, applicability classification, testability decision и domain review.

## Development sequence после Foundation

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
module expansion: Scheme / NPT / clean-sheet Switching
```

## Explicitly out of current scope

Без нового owner decision не строить replacement SCADA runtime, IEC-104 server/client platform как product objective, historian, P/Q control, redundancy/failover SCADA, remote real-equipment switching execution, microservice platform, dynamic plugin marketplace или universal CAD replacement.

## Acceptance posture

Foundation должен защитить проект как минимум от пяти известных failure modes:

1. competing domain models;
2. legacy-looking/unusable UI despite correct backend;
3. untraceable normative folklore embedded in code;
4. development pipeline, где маленький visible repair превращается в дни unrelated CI;
5. хаотичный mixed-language product, где русский UI, английские internal identifiers и инженерная terminology используются непоследовательно.
