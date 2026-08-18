# Electrical Engineering Platform

Единый модульный электротехнический инженерный комплекс для построения, импорта, редактирования, визуализации и анализа электрической модели объекта, выпуска электрических схем, совместимости с NPT Expert/Modus и подготовки/проверки оперативных переключений.

Репозиторий `genrudko/ElectricalEngineeringPlatform` является каноническим репозиторием продукта.

Функциональные направления:

- Scheme Studio;
- NPT Compatibility;
- новый Switching / TBP module;
- общие Domain Core, UI Core, Compliance Core, Import/Reconciliation и Development Platform.

Старый TBP code/config/rules **не мигрируется**. Switching module проектируется и реализуется с нуля поверх нового `ElectricalProject`, `TopologyGraph`, State model и Compliance Core.

## Язык проекта

Язык продукта и всей канонической документации — **русский**.

На русском языке выполняются:

- основной пользовательский интерфейс;
- пользовательские сообщения, предупреждения и диагностические объяснения;
- встроенная справка;
- документация в `docs/`;
- пользовательские отчёты/формы, если конкретный профиль вывода не требует другого языка.

Внутренняя техническая часть использует **английский язык и профессиональную англоязычную терминологию электроэнергетики**:

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

Код, API, schema keys, identifiers, internal events/error codes и tests — английские. Транслит в technical identifiers не допускается.

Канонические правила:

- `docs/project/LANGUAGE_POLICY.md`;
- `docs/project/RU_EN_ENGINEERING_GLOSSARY.md`.

## Источник истины

Каноническая инженерная модель:

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

Схемы, CSV/XLSX, XSDE, XTABL, Visio и бланки переключений являются представлениями, импортами/экспортами, документами либо совместимостными адаптерами общей модели, а не параллельными источниками истины.

## Целевая архитектура

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

Архитектурный стиль: **modular monolith**.

Главные инварианты:

- topology отделена от diagram geometry;
- `UNKNOWN` является полноценным состоянием и не превращается молча в безопасное;
- NPT-specific identifiers остаются в NPT module/adapters;
- локальные policy overlays могут ужесточать обязательный baseline, но не ослаблять его;
- standalone operation не зависит от EOD.

## Нормативный контур

- графическая часть схем строится через versioned ГОСТ/ЕСКД profiles с явной provenance и coverage matrix;
- Switching использует versioned normative registry для применимых ПОТЭЭ, ПТЭЭП/ПТЭЭПЭЭ, ПТЭЭС, Правил переключений, отдельных глав ПУЭ и иных применимых источников;
- государственные нормативные документы и ГОСТ/ЕСКД metadata повторно проверяются онлайн по authoritative sources перед соответствующим compliance work item;
- внутренние инструкции предприятий/объектов не являются shared development inputs и не загружаются в общий Git/VPS corpus;
- локальные требования внедрения поддерживаются через контролируемые `LocalPolicy`/`PolicyPackage` механизмы внутри разрешённого deployment environment.

## UI/UX

UI Core — самостоятельный архитектурный фундамент. Цель — современный, плотный, профессиональный desktop UX для длительной инженерной работы, больших проектов, мыши+клавиатуры и нескольких мониторов.

UI не должен выглядеть как legacy/MS-DOS/ранний Win32 и не должен быть разреженным mobile-first интерфейсом на desktop.

## Platform stack

Финальный стек пока **не выбран**.

Кандидаты:

- Avalonia + C#/.NET;
- Qt 6 + C++/QML.

Выбор выполняется только после эквивалентного `PLATFORM-STACK-SPIKE-001`.

## Development Platform

Канонический state и изменения контролируются через GitHub, а существующий VPS используется как execution plane.

Дополнительно 2026-08-18 фактически доказан прямой интерактивный контур:

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

- Custom GPT Action работает на текущем ChatGPT Plus;
- Action работает внутри существующего Project-чата;
- в том же Project-чате сохраняется доступ к GitHub;
- запрос `GET /health` реально проходит до VPS и возвращает `200 OK`;
- bridge работает за Caddy/HTTPS/Let's Encrypt и FastAPI на localhost под непривилегированным service account.

Целевая схема разработки:

```text
быстрый интерактивный контур:
ChatGPT Project → EEP Development Bridge → VPS

формальный контур:
GitHub → self-hosted runner on VPS → checks/artifacts → PR acceptance
```

Bridge не должен превращаться в универсальный удалённый shell. Выполняемые операции задаются ограниченными typed/allowlisted API contracts.

Новые обязательные платные Business/MCP/API-сервисы Foundation не требует.

## EOD

Интеграция с Electronic Operational Documentation рассматривается как optional bridge/module integration через существующий EOD module registry и отдельный feasibility/cost gate. Standalone operation комплекса обязательна.

## Текущий этап

Активный work item: `UNIFIED-FOUNDATION-001`.

После принятия Foundation:

```text
INFRASTRUCTURE-SPIKE-001
→ PLATFORM-STACK-SPIKE-001
→ UI-CORE-FOUNDATION-001 + DOMAIN-CORE-FOUNDATION-001
→ IMPORT-TO-SCHEME-VERTICAL-SLICE-001
```

`INFRASTRUCTURE-SPIKE-001` теперь не доказывает сам факт ChatGPT→VPS доступа — он уже доказан. Spike должен hardened-оформить bridge, развернуть self-hosted GitHub runner и доказать нормальный build/test/artifact workflow.

Начинать чтение следует с `AGENTS.md` и `docs/INDEX.md`.