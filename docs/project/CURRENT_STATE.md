# Текущее состояние — Electrical Engineering Platform

Дата среза: **2026-08-18**  
Активная программа: `UNIFIED-FOUNDATION-001`

## 1. Фактическое состояние GitHub

- Канонический репозиторий: `genrudko/ElectricalEngineeringPlatform`.
- Default branch: `main`.
- Bootstrap commit: `5aa73328917447fb410489e8f67685ee4907ae66`.
- Активный issue: #1 `UNIFIED-FOUNDATION-001`.
- Активная ветка: `architecture/unified-foundation-001`.
- Foundation Draft PR: #2 `UNIFIED-FOUNDATION-001 — establish canonical platform foundation`.
- PR #2 остаётся **OPEN / DRAFT / NOT MERGED** до отдельной owner acceptance и явной команды на Ready/Merge.
- Предыдущий Foundation source в `genrudko/electroscheme-studio` issue #5 / Draft PR #6 закрыт как superseded и остаётся historical migration evidence.
- Tauri desktop spike в `genrudko/electroscheme-studio` issue #3 / Draft PR #4 остаётся research evidence only.

Exact head, compare state, changed files и workflow/check state являются volatile и перед implementation/acceptance всегда читаются напрямую из GitHub.

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
- structured CSV/XLSX Import + reconciliation + auto-layout;
- ГОСТ/ЕСКД graphic profiles;
- switching rules с привязкой к current Russian energy-sector normative sources;
- local policy overlays не могут ослаблять applicable mandatory baseline;
- NPT compatibility находится за module/adapter boundary;
- optional EOD integration — только через strict feasibility/cost gate;
- GitHub — canonical control plane;
- existing VPS — development execution plane;
- risk-based CI и visual-first UI acceptance;
- final platform stack — только после equivalent Avalonia-vs-Qt executable spike;
- русский — язык продукта/UI/документации;
- английский — язык internal technical/domain части с профессиональной power-engineering terminology.

## 4. Решения владельца, уточнённые во время Foundation

### 4.1. Язык проекта

```text
Пользовательский интерфейс и каноническая documentation → русский
Internal technical/domain implementation                → английский
```

Точные internal entities используют engineering English: `CircuitBreaker`, `Disconnector`, `EarthingSwitch`, `Busbar`, `TopologyGraph`, `SwitchingOperation`, `InterlockRule` и т. п.

Транслит в identifiers запрещён.

Канонические owners:

- `docs/project/LANGUAGE_POLICY.md`;
- `docs/project/RU_EN_ENGINEERING_GLOSSARY.md`.

### 4.2. Старый TBP

Старый TBP project **не мигрируется** в новый продукт.

- no code migration;
- no YAML/rule migration;
- нет требования загружать/архивировать его для этого проекта;
- он не используется как normative authority/test oracle.

Новый Switching module реализуется с нуля на Domain Core + Compliance Core + current verified normative sources.

### 4.3. Внутренние инструкции enterprise/site

Внутренние enterprise/site instructions **не являются normal shared development inputs** и не должны загружаться в GitHub/ChatGPT/shared VPS corpus.

Продукт поддерживает fine-grained Local Policy Overlays на deployment, но shared development использует synthetic/cleared policy examples.

### 4.4. Public normative sources

Government/sector acts и ГОСТ/ЕСКД source metadata получаются и повторно проверяются онлайн по authoritative sources при начале соответствующей compliance/profile работы.

Вручную собранный большой normative document pack сейчас не требуется.

Для ГОСТ authoritative catalogue/status обязателен; detailed full-text extraction использует lawful source и не считает random internet copies нормативным authority.

## 5. Доказанный ChatGPT → VPS development bridge

2026-08-18 выполнен практический feasibility test прямого интерактивного доступа из ChatGPT Plus к existing VPS.

Фактически доказана цепочка:

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
- bridge service user `eepbridge` — непривилегированный;
- FastAPI/Uvicorn слушает только `127.0.0.1:8788`;
- Caddy публикует HTTPS endpoint;
- сертификат Let's Encrypt успешно получен;
- UFW открывает только необходимые HTTP/HTTPS порты для bridge поверх уже существующих правил;
- запрос без Bearer получает `401 Unauthorized`;
- запрос с корректным Bearer получает `200 OK`.

### ChatGPT-side evidence

Доказано:

1. Custom GPT Action работает на текущем ChatGPT Plus;
2. Action реально вызывает внешний bridge — VPS log зафиксировал `GET /health ... 200 OK`;
3. Custom GPT Action работает внутри существующего Project-чата `Electrical Engineering Platform`;
4. в том же Project-чате сохраняется рабочий доступ к GitHub connector;
5. после GitHub query bridge снова успешно вызывается в том же разговоре.

### Архитектурное следствие

Целевая Development Platform использует два контура:

```text
Интерактивный development loop:
ChatGPT Project
→ EEP Development Bridge
→ VPS

Формальный verification loop:
GitHub
→ self-hosted runner on VPS
→ exact-head build/test/benchmark/package
→ GitHub checks/artifacts
→ owner acceptance
```

Bridge не является универсальным remote shell. Следующая версия должна использовать typed/allowlisted operations, fixed workspace, timeout, task IDs, audit log и ограниченный набор execution profiles.

Таким образом, Business/MCP/OpenAI API не являются обязательными dependencies для текущего development workflow.

## 6. Что ещё не доказано / не выбрано

- Avalonia vs Qt final selection;
- heavy-canvas performance на representative workload;
- exact native project package format/schema v1;
- complete equipment-type library;
- completeness/correctness NPT `nodes` как topology source;
- production-safe creation arbitrary new XSDE topology objects;
- full normative rule coverage;
- exact EOD bridge API/context-security/deployment cost;
- installer/update channel;
- final branding beyond repository/product working name;
- hardened production-quality Development Bridge API;
- self-hosted GitHub runner exact-head workflow в новом репозитории.

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
- evidence, что NPT `nodes` topology-related, но нет доказательства complete neutral electrical graph;
- current Mnemo renderer fidelity остаётся недостаточной.

Full vendor/reference corpus остаётся вне GitHub.

### 7.3. Старый TBP

Historical only. Не является migration source для нового Switching module.

## 8. Normative baseline state

Foundation устанавливает registry mechanism, а не утверждает, что все требования уже encoded.

Initial high-priority source set включает:

- ГОСТ 2.701-2008;
- ГОСТ 2.702-2011;
- ПОТЭЭ №903н;
- ПТЭЭП/ПТЭЭПЭЭ №811;
- ПТЭЭС №1070;
- Правила переключений №757;
- applicable ПУЭ chapters, учитываемые отдельно, а не как одна synthetic version.

Каждое production rule требует current-source verification, source-level extraction, applicability classification, testability decision и domain review.

## 9. Development sequence после Foundation

```text
UNIFIED-FOUNDATION-001
        ↓
INFRASTRUCTURE-SPIKE-001
hardening Development Bridge
+ self-hosted GitHub runner
+ exact-head build/test/artifact workflow
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

## 10. Явно вне текущего scope

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

Foundation должен защитить проект как минимум от шести известных failure modes:

1. competing domain models;
2. legacy-looking/unusable UI despite correct backend;
3. untraceable normative folklore embedded in code;
4. development pipeline, где маленький visible repair превращается в дни unrelated CI;
5. хаотичный mixed-language product;
6. unrestricted remote shell/agent access к VPS вместо bounded development control API.