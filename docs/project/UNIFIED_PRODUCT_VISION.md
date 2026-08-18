# Unified Product Vision

Статус: canonical foundation document

## 1. Product thesis

Продукт — единый локальный desktop-комплекс для электротехнической инженерии и эксплуатации, в котором **одна нейтральная модель электроустановки** используется для создания/редактирования схем, импорта структурированных перечней, совместимости с NPT и подготовки/проверки оперативных переключений.

Ценность комплекса возникает из общей электротехнической семантики:

```text
оборудование
+ терминалы
+ соединения
+ topology
+ state
+ signals
+ normative rules
+ controlled views
```

## 2. Почему объединение оправдано

Scheme Studio, NPT Engineering Toolkit и Switching/TBP независимо пришли к одной и той же необходимости: оборудование, terminal/connection model, topology, state и rules.

Три независимые архитектуры означали бы три базы оборудования, три topology/state model и постоянное расхождение данных.

## 3. Core user promise

Инженер описывает объект данными один раз и использует одну модель в разных сценариях:

```text
структурированный перечень оборудования/соединений
        ↓
ElectricalProject
        ├── normal single-line view
        ├── operational view
        ├── NPT-compatible representation
        ├── equipment/table views
        └── switching simulation / TBP
```

## 4. Killer workflow

> Импортировать структурированный CSV/XLSX перечень оборудования и соединений, получить построенную electrical topology и автоматически сгенерированную схему, после чего инженер исправляет только неоднозначности и компоновку.

После импорта пользователь должен видеть однозначно распознанные сущности, предположения для подтверждения, конфликты и diff при повторном импорте.

## 5. Product values

### Engineering semantics over drawing primitives

Выключатель — не картинка. Он имеет identity, type, terminals, state, properties, signal bindings, applicable rules и representations.

### Human authority over silent automation

Автоматизация сокращает рутину, но не прячет инженерные допущения.

### Traceable compliance over folklore

Нормативное требование существует в продукте как правило с источником, редакцией, областью применимости и доказуемым поведением.

### Modern professional UX

Сложная domain model не оправдывает архаичный интерфейс. Complex engineering software должно быть современным, плотным и быстрым.

### Fast iteration is a development requirement

Малый UI change должен быстро становиться видимым. Надёжность достигается risk-based gates, а не запуском всего мира после изменения двух кнопок.

### Standalone first

Комплекс работает самостоятельно. NPT и EOD — optional compatibility/integration boundaries, а не обязательные runtime dependencies.

## 6. Target users and environments

Primary users:

- оперативный и оперативно-ремонтный персонал;
- инженерно-технический персонал электроэнергетики;
- специалисты по схемам/мнемосхемам/operational documentation;
- инженеры, формирующие и проверяющие переключения;
- специалисты по поддержке NPT-compatible projects.

Usage assumptions:

- desktop workstation;
- potentially several monitors;
- large projects;
- long continuous sessions;
- keyboard+mouse;
- local/offline operation required;
- Windows and Linux target classes, exact support fixed after platform spike.

## 7. Product modules

- **Domain Core** — project/equipment/terminals/connections/topology/state/signals/rules/views.
- **UI Core** — application shell, workspace, design system, commands, inspectors, data views, canvas infrastructure.
- **Equipment Library** — semantic equipment types, terminal configurations, properties, states and approved representations.
- **Import & Reconciliation** — CSV/XLSX mapping, staging, normalization, ambiguity handling, diff/reconciliation and provenance.
- **Scheme Studio** — electrical views, domain-aware auto-layout, manual layout constraints, rendering/editing/output.
- **NPT Compatibility** — XSDE/XTABL, NPT properties/IDs, signal catalog/adapters and validation.
- **Switching / TBP** — operations, sequences, rule evaluation, interlocks, simulation and document generation.
- **Compliance Core** — normative registry, applicability, profile composition and local policy overlays.
- **Optional EOD Adapter** — thin integration only if feasibility/cost gate passes.

## 8. What the product is not

Current vision does not include a replacement full SCADA runtime, historian, universal CAD, remote control platform or arbitrary automation platform.

## 9. Success criteria

1. Structured equipment/topology list becomes a usable project/scheme with much less manual work than drawing from scratch.
2. Engineer corrections survive re-import.
3. One domain model feeds multiple modules/views.
4. Scheme symbols/output are traceable to applicable standards profiles.
5. Switching validation identifies uncertainty and explains restrictions using versioned rules.
6. Site-specific rules can be configured without weakening mandatory requirements.
7. NPT compatibility preserves required vendor-format information without contaminating neutral Core.
8. UI stays responsive and multi-monitor capable on realistic sizes.
9. Common UI changes can be visually accepted in minutes/hours rather than days.
10. Standalone product remains usable without NPT or EOD integrations.
