# Electrical Engineering Platform

Единый модульный электротехнический инженерный комплекс для построения, импорта, редактирования, визуализации и анализа электрической модели объекта, выпуска электрических схем, совместимости с NPT Expert/Modus и подготовки/проверки оперативных переключений.

Репозиторий `genrudko/ElectricalEngineeringPlatform` является каноническим repository продукта.

Функциональные направления:

- Scheme Studio;
- NPT Compatibility;
- новый clean-sheet Switching / TBP module;
- общие Domain Core, UI Core, Compliance Core, Import/Reconciliation и Development Platform.

Старый TBP code/config/rules **не мигрируется**. Switching module проектируется и реализуется с нуля поверх нового `ElectricalProject`, `TopologyGraph`, State model и Compliance Core.

## Язык проекта

Язык продукта и всей канонической documentation — **русский**.

На русском языке выполняются:

- основной user interface;
- пользовательские messages, warnings и diagnostic explanations;
- встроенная help;
- documentation в `docs/`;
- user reports/forms, если конкретный output profile не требует другого языка.

Internal technical layer использует **English и professional power-engineering terminology**:

```text
ElectricalProject
CircuitBreaker
Disconnector
EarthingSwitch
Busbar
VoltageLevel
TopologyGraph
SwitchingOperation
InterlockRule
```

Code, API, schema keys, identifiers, internal events/error codes и tests — English. Transliteration в technical identifiers не допускается.

Canonical rules:

- `docs/project/LANGUAGE_POLICY.md`;
- `docs/project/RU_EN_ENGINEERING_GLOSSARY.md`.

## Source of truth

Каноническая engineering model:

```text
ElectricalProject
├── Sites / Facilities
├── VoltageLevels
├── Equipment
├── Terminals
├── Connections
├── Topology
├── Signals
├── States
├── Rules / Policies
├── Metadata
└── Views
```

Схемы, CSV/XLSX, XSDE, XTABL, Visio и switching forms являются representations, imports/exports, documents или compatibility adapters общей model, а не parallel sources of truth.

## Target architecture

```text
Application / UI Shell
├── UI Core
├── Domain Core
├── Compliance Core
├── Equipment Library
├── Import & Reconciliation
├── Scheme Studio & Auto Layout
├── NPT Compatibility
└── Switching / TBP & Interlocks
```

Architectural style: **modular monolith**.

Главные invariants:

- topology отделена от diagram geometry;
- `UNKNOWN` является first-class state и не превращается silently в safe state;
- NPT-specific identifiers остаются в NPT module/adapters;
- local policy overlays могут tighten mandatory baseline, но не weaken его;
- standalone operation не зависит от EOD.

## Normative contour

- graphics schemes строятся через versioned ГОСТ/ЕСКД profiles с explicit provenance и coverage matrix;
- Switching использует versioned normative registry для applicable ПОТЭЭ, ПТЭЭП/ПТЭЭПЭЭ, ПТЭЭС, Правил переключений, отдельных chapters ПУЭ и других applicable sources;
- public regulatory sources и ГОСТ/ЕСКД metadata re-verify online по authoritative sources перед соответствующим compliance work item;
- internal enterprise/site instructions не являются shared development inputs и не загружаются в общий Git/VPS corpus;
- local deployment requirements поддерживаются через controlled `LocalPolicy`/`PolicyPackage` mechanism внутри authorized environment.

## UI/UX

UI Core — самостоятельный architectural foundation. Цель — современный, плотный, профессиональный desktop UX для длительной инженерной работы, больших projects, keyboard+mouse и multi-monitor.

UI не должен выглядеть как legacy/MS-DOS/early Win32 и не должен быть sparse mobile-first interface на desktop.

## Platform stack

Final stack пока **не выбран**.

Candidates:

- Avalonia + C#/.NET;
- Qt 6 + C++/QML.

Selection выполняется только после equivalent `PLATFORM-STACK-SPIKE-001`.

## Development Platform

Canonical state и changes контролируются через GitHub, existing VPS используется как execution plane.

2026-08-18 фактически доказан direct interactive contour:

```text
ChatGPT Plus Project chat
        ↓ @Custom GPT + Action
HTTPS + Bearer
        ↓
EEP Development Bridge
        ↓
existing VPS
```

Доказано:

- Custom GPT Action работает на current ChatGPT Plus;
- Action работает inside existing Project chat;
- в том же Project chat доступен GitHub connector;
- `GET /health` реально доходит до VPS и возвращает `200 OK`;
- bridge работает за Caddy/HTTPS/Let's Encrypt, FastAPI слушает localhost под unprivileged service account.

Target development model:

```text
Interactive contour:
ChatGPT Project → EEP Development Bridge → VPS

Formal contour:
GitHub → self-hosted runner on VPS → checks/artifacts → PR acceptance
```

Bridge не должен становиться universal remote shell. Execution operations задаются bounded typed/allowlisted API contracts.

New mandatory paid Business/MCP/API services Foundation не требует.

## EOD

Integration с Electronic Operational Documentation рассматривается как optional bridge/module integration через existing EOD module registry и separate feasibility/cost gate. Standalone operation обязательна.

## Current stage

Active work item: `UNIFIED-FOUNDATION-001`.

После accepted Foundation:

```text
INFRASTRUCTURE-SPIKE-001
→ PLATFORM-STACK-SPIKE-001
→ UI-CORE-FOUNDATION-001 + DOMAIN-CORE-FOUNDATION-001
→ IMPORT-TO-SCHEME-VERTICAL-SLICE-001
```

`INFRASTRUCTURE-SPIKE-001` не доказывает сам факт ChatGPT→VPS access — он уже proven. Spike должен harden bridge, развернуть self-hosted GitHub runner и доказать exact-head build/test/artifact workflow.

Начинать чтение следует с `AGENTS.md` и `docs/INDEX.md`.