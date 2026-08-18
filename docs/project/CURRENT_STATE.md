# Текущее состояние — Electrical Engineering Platform

Дата среза: **2026-08-18**  
Текущий переход: `INFRASTRUCTURE-SPIKE-001` accepted/merged → `PLATFORM-STACK-SPIKE-001` contract review перед formal opening

## 1. Фактическое состояние GitHub

- Канонический repository: `genrudko/ElectricalEngineeringPlatform`.
- Default branch: `main`.
- Bootstrap commit: `5aa73328917447fb410489e8f67685ee4907ae66`.
- `UNIFIED-FOUNDATION-001`: Issue #1 **CLOSED / COMPLETED**; PR #2 **MERGED**; merge commit `c62a67064145610cc41711c21b79d0cc0eecb5b9`.
- `INFRASTRUCTURE-SPIKE-001`: Issue #3 **CLOSED / COMPLETED**; PR #4 **MERGED**; accepted head `530c7f8058a0c4d30a60f2e62db0acf888f16687`; merge commit `b9cf480474f63b2145a22f541c21190b007cb120`.
- На момент этого canonical-state refresh отдельные Issue/branch/Draft PR для `PLATFORM-STACK-SPIKE-001` **ещё не созданы**; formal opening выполняется только после owner review final contract.
- Previous Foundation source в `genrudko/electroscheme-studio` issue #5 / Draft PR #6 закрыт как superseded и остаётся historical migration evidence.
- Tauri desktop spike в `genrudko/electroscheme-studio` issue #3 / Draft PR #4 остаётся research evidence only.

Exact current `main`, compare state, changed files и workflow/check state являются volatile и перед implementation/acceptance всегда читаются напрямую из GitHub. SHA `b9cf480474f63b2145a22f541c21190b007cb120` — post-merge baseline непосредственно перед этим docs-only canonical-state refresh, а не вечный alias для `main`.

## 2. Принятое направление продукта

Цель — один standalone, desktop-first/local-first **модульный электротехнический инженерный комплекс**, содержащий:

1. Scheme Studio;
2. NPT Compatibility;
3. новый clean-sheet Switching / TBP module;
4. общие Import/Reconciliation, Domain Core, UI Core и Compliance Core.

`ElectricalProject` / neutral domain model является source of truth. Diagram geometry, CSV/XLSX, NPT files и switching-form documents — bounded views/imports/adapters.

## 3. Принятые Foundation-инварианты

- modular monolith;
- один neutral Domain Core;
- first-class UI Core;
- Compliance Core с versioned normative provenance;
- topology отделена от geometry;
- State model с first-class `UNKNOWN`;
- observed/baseline, simulated и planned/target state contexts разделены на model/API boundary;
- structured CSV/XLSX Import + reconciliation + auto-layout;
- ГОСТ/ЕСКД graphic profiles;
- switching rules с привязкой к current Russian energy-sector normative sources;
- Local Policy resolution различает authority, applicability, specificity и explicit delegation;
- local policy overlays не могут ослаблять applicable locked mandatory baseline, кроме корректного выбора внутри явно delegated bounds;
- NPT compatibility находится за module/adapter boundary;
- optional EOD integration — только через strict feasibility/cost gate;
- GitHub — canonical control plane;
- existing VPS — development execution plane;
- risk-based CI и visual-first UI acceptance;
- final platform stack — только после equivalent Avalonia-vs-Qt executable spike;
- Windows build/package lane должна быть доказана отдельно, потому что existing VPS является Linux host;
- русский — язык продукта/UI/canonical documentation;
- английский — язык internal technical/domain layer с professional power-engineering terminology.

## 4. Решения владельца, уточнённые во время Foundation

### 4.1. Язык проекта

```text
Пользовательский интерфейс и каноническая documentation → русский
Internal technical/domain implementation                → английский
```

Точные internal entities используют engineering English: `CircuitBreaker`, `Disconnector`, `EarthingSwitch`, `Busbar`, `TopologyGraph`, `SwitchingOperation`, `InterlockRule` и т. п.

Transliteration в identifiers запрещена.

Канонические owners:

- `docs/project/LANGUAGE_POLICY.md`;
- `docs/project/RU_EN_ENGINEERING_GLOSSARY.md`.

Foundation canonical docs приведены к этой policy. English сохраняется для exact technical identifiers, established engineering terms и machine-readable values, но не для explanatory narrative.

### 4.2. Старый TBP

Старый TBP project **не мигрируется** в новый продукт.

- no code migration;
- no YAML/rule migration;
- нет requirement загружать/архивировать его для этого project;
- он не используется как normative authority/test oracle.

Новый Switching module реализуется с нуля на Domain Core + Compliance Core + current verified normative sources.

### 4.3. Внутренние enterprise/site instructions

Internal enterprise/site instructions **не являются normal shared development inputs** и не должны загружаться в GitHub/ChatGPT/shared VPS corpus.

Продукт поддерживает fine-grained Local Policy Overlays на deployment, но shared development использует synthetic/cleared policy examples.

### 4.4. Public normative sources

Government/sector acts и ГОСТ/ЕСКД source metadata получаются и повторно проверяются онлайн по authoritative sources при начале соответствующей compliance/profile работы.

Manually assembled large normative document pack сейчас не требуется.

Для ГОСТ authoritative catalogue/status обязателен; detailed full-text extraction использует lawful source и не считает random internet copies normative authority.

## 5. Доказанная Development Platform после `INFRASTRUCTURE-SPIKE-001`

2026-08-18 practical feasibility прямого interactive access из ChatGPT Plus к existing VPS был доведён до bounded Bridge v0.2, а formal GitHub contour — до repository-level self-hosted runner с exact-head evidence.

Доказанная interactive chain:

```text
ChatGPT Plus Project chat
→ @Custom GPT (`EEP Bridge test`)
→ GPT Action
→ HTTPS + Bearer
→ Caddy
→ FastAPI `EEP Development Bridge`
→ existing VPS
```

### Серверная сторона

На VPS:

- Ubuntu 24.04.4 LTS;
- bridge service user `eepbridge` — unprivileged;
- FastAPI/Uvicorn слушает только `127.0.0.1:8788`;
- Caddy публикует HTTPS endpoint;
- Let's Encrypt certificate получен успешно;
- UFW открывает required HTTP/HTTPS ports поверх already existing rules;
- request без Bearer получает `401 Unauthorized`;
- request с correct Bearer получает `200 OK`.

### ChatGPT-side evidence

Доказано:

1. Custom GPT Action работает на current ChatGPT Plus;
2. Action реально вызывает external bridge;
3. Custom GPT Action работает внутри existing Project chat `Electrical Engineering Platform`;
4. в том же Project chat сохраняется working GitHub connector;
5. Bridge v0.2 предоставляет fixed read-only repository view, typed/allowlisted operations, bounded tasks/status/log/cancel, timeout/output/concurrency policy и не предоставляет arbitrary shell.

### Formal runner evidence

- runner: `eep-development-vps`;
- service account: `eep-runner`, не root;
- repository-level self-hosted runner;
- exact accepted PR #4 head: `530c7f8058a0c4d30a60f2e62db0acf888f16687`;
- final exact-head workflow run: `32180025769` — `success`;
- evidence artifact: `infrastructure-smoke-evidence`, id `9340434117`;
- accepted Bridge runtime source deployed из `bd442d3ee4c0de39dd2c8c73a11aab90b6119ded`;
- runner restart/re-acquisition доказан; deliberate full VPS reboot специально не выполнялся.

### Архитектурное следствие

Target Development Platform использует два contours:

```text
Interactive development loop:
ChatGPT Project
→ bounded EEP Development Bridge
→ VPS

Formal verification loop:
GitHub
→ self-hosted runner on VPS
→ exact-head build/test/benchmark/package
→ GitHub checks/artifacts
→ owner acceptance
```

Bridge v0.2 уже реализует bounded execution contract, но его текущие execution profiles ограничены `bridge_compile` и `bridge_selftest`. Stack-specific Avalonia/Qt build/benchmark/preview profiles ещё не определены и не считаются доказанными capabilities.

Business/MCP/OpenAI API не являются required dependencies current development baseline.

## 6. Что ещё не доказано / не выбрано

- Avalonia vs Qt final selection;
- heavy semantic canvas performance на representative workload;
- reproducible Windows build/test/package lane в no-new-mandatory-paid-service baseline;
- exact stack-specific Bridge execution profiles и необходимость их использования в `PLATFORM-STACK-SPIKE-001`;
- exact native project package format/schema v1;
- complete equipment-type library;
- completeness/correctness NPT `nodes` как topology source;
- production-safe creation arbitrary new XSDE topology objects;
- full normative rule coverage;
- exact EOD bridge API/context-security/deployment cost;
- installer/update channel;
- final branding beyond repository/product working name.

## 7. Важное inherited evidence

### 7.1. ElectroScheme Studio

Полезные evidence/assets:

- object/terminal/connection research;
- SVG/editor interaction experiments;
- VSDX/VSSX/ShapeSheet tooling;
- snapping/busbar/symbol research;
- desktop packaging/Tauri spike evidence;
- market/reference-product research.

Это migration inputs, а не автоматически принятая production architecture.

### 7.2. NPT Engineering Toolkit / source materials

Research baseline включает:

- ~475 parseable real XSDE files и large industrial corpus;
- lossless XSDE round-trip для studied corpus;
- identified `scd*` semantics и non-KKS exceptions;
- embedded `CustElem` и external `.menu` library role;
- large ASU/TECH/KKS signal catalog research;
- lossless XTABL v6.0 core для seven production tables / 1461 records;
- unknown/preserve-only XTABL fields;
- evidence, что NPT `nodes` topology-related, но нет proof complete neutral electrical graph;
- current Mnemo renderer fidelity остаётся insufficient.

Full vendor/reference corpus остаётся вне GitHub.

### 7.3. Старый TBP

Historical only. Не является migration source для нового Switching module.

## 8. Normative baseline state

Foundation устанавливает registry mechanism, а не утверждает, что все requirements уже encoded.

Initial high-priority source set включает:

- ГОСТ 2.701-2008;
- ГОСТ 2.702-2011;
- ПОТЭЭ №903н;
- ПТЭЭП/ПТЭЭПЭЭ №811;
- ПТЭЭС №1070;
- Правила переключений №757;
- applicable ПУЭ chapters, учитываемые отдельно, а не как одна synthetic version.

Каждое production rule требует current-source verification, source-level extraction, applicability classification, testability decision и domain review.

## 9. Текущая development sequence

```text
UNIFIED-FOUNDATION-001              [ACCEPTED / MERGED PR #2]
        ↓
INFRASTRUCTURE-SPIKE-001            [ACCEPTED / MERGED PR #4]
        ↓
PLATFORM-STACK-SPIKE-001            [NEXT — FORMAL OPENING PENDING]
Avalonia vs Qt
+ reproducible Windows/Linux build/package lanes
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

## 10. Явно вне current scope

Без нового owner decision не строить:

- replacement SCADA runtime;
- IEC-104 server/client platform как product objective;
- historian;
- P/Q control;
- redundancy/failover SCADA;
- remote real-equipment switching execution;
- microservice platform;
- dynamic plugin marketplace;
- universal CAD replacement.

## 11. Acceptance posture

Принятый Foundation защищает project как минимум от девяти known failure modes:

1. competing domain models;
2. legacy-looking/unusable UI despite correct backend;
3. untraceable normative folklore embedded in code;
4. development pipeline, где small visible repair превращается в дни unrelated CI;
5. хаотичный mixed-language product;
6. unrestricted remote shell/agent access к VPS вместо bounded development control API;
7. mixing observed/simulated/planned equipment state;
8. treating policy specificity as automatic normative authority;
9. declaring Windows support without a reproducible Windows build/package lane.