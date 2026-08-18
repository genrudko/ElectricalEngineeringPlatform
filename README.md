# Electrical Engineering Platform

Единый модульный электротехнический инженерный комплекс для построения, импорта, редактирования, визуализации и анализа электрической модели объекта, выпуска электрических схем, совместимости с NPT Expert/Modus и подготовки/проверки оперативных переключений.

Этот репозиторий является новым каноническим umbrella-repository для объединения трёх прежних направлений:

- ElectroScheme Studio;
- NPT Engineering Toolkit;
- TBP / подготовка и проверка бланков переключений.

## Главный принцип

Источником истины является нейтральная модель проекта:

```text
ElectricalProject
├── Sites / Objects
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

## Нормативная база

- графическая часть схем — через версионируемые ГОСТ/ЕСКД profiles с трассируемыми правилами и coverage matrix;
- switching/TBP — через versioned normative registry с применимыми ПОТЭЭ, ПТЭЭП/ПТЭЭПЭЭ, ПТЭЭС, Правилами переключений, главами ПУЭ и локальными требованиями;
- локальные manufacturer/enterprise/site/project policies могут дополнять или ужесточать обязательный baseline, но не ослаблять его.

## UI/UX

UI Core — самостоятельный фундамент. Цель — современный плотный professional desktop UX для длительной инженерной работы, больших проектов, мыши+клавиатуры и нескольких мониторов.

## Platform stack

Финальный стек пока не выбран. Кандидаты:

- Avalonia + C#/.NET;
- Qt 6 + C++/QML.

Выбор выполняется только после эквивалентного executable Platform Stack Spike.

## Development Platform

```text
Owner / ChatGPT
      ↓
GitHub — canonical control plane
      ↓
self-hosted runner
      ↓
existing VPS — build/test/corpus/artifacts
      ↓
portable preview
      ↓
owner acceptance endpoint
```

Не предполагается обязательный новый платный Business/MCP/control-plane сервис.

## EOD

Интеграция с Electronic Operational Documentation рассматривается как optional bridge/module integration через существующий EOD module registry и отдельный feasibility/cost gate. Standalone operation комплекса обязательна.

## Foundation

Первый work item нового репозитория — перенос и принятие Unified Foundation. После него:

```text
Infrastructure Spike
→ Avalonia vs Qt Platform Stack Spike
→ UI Core + Minimal Domain Core
→ Import-to-Scheme Vertical Slice
```

Начинать чтение после Foundation migration следует с `AGENTS.md` и `docs/INDEX.md`.
