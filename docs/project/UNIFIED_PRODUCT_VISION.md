# Единое видение продукта

Статус: канонический Foundation-документ

## 1. Тезис продукта

Продукт — единый локальный desktop-комплекс для электротехнической инженерии и эксплуатации, в котором **одна нейтральная модель электроустановки** используется для создания и редактирования схем, импорта структурированных перечней, совместимости с NPT и подготовки/проверки оперативных переключений.

Ценность комплекса возникает из общей электротехнической семантики:

```text
Equipment
+ Terminals
+ Connections
+ Topology
+ State
+ Signals
+ Normative Rules
+ controlled Views
```

## 2. Почему объединение оправдано

Scheme Studio, NPT Compatibility и Switching требуют одних и тех же базовых сущностей: equipment identity, terminal/connection model, topology, state и rules.

Три независимые архитектуры означали бы три базы оборудования, три topology/state model и постоянное расхождение инженерных данных.

Цель объединения — не механически сложить старые проекты, а создать один общий `ElectricalProject` и подключать к нему специализированные модули.

## 3. Основное обещание пользователю

Инженер описывает объект данными один раз и использует одну модель в разных сценариях:

```text
структурированный перечень оборудования/соединений
        ↓
ElectricalProject
        ├── обычная однолинейная схема
        ├── operational view
        ├── NPT-compatible representation
        ├── equipment/table views
        └── switching simulation / SwitchingForm
```

## 4. Главный продуктовый workflow

> Импортировать структурированный CSV/XLSX перечень оборудования и соединений, получить построенную electrical topology и автоматически сгенерированную схему, после чего инженер исправляет только неоднозначности и компоновку.

После импорта пользователь должен видеть:

- однозначно распознанные сущности;
- случаи, требующие подтверждения;
- конфликты;
- источник и provenance изменений;
- reconciliation diff при повторном импорте.

## 5. Ценности продукта

### 5.1. Инженерная семантика важнее графических примитивов

Выключатель — не картинка. Он имеет identity, type, terminals, state, properties, signal bindings, applicable rules и representations.

### 5.2. Инженер сохраняет контроль над автоматизацией

Автоматизация сокращает рутину, но не скрывает инженерные допущения и неоднозначности.

### 5.3. Трассируемое соответствие вместо «нормативного фольклора»

Нормативное требование существует в продукте как правило с источником, редакцией, областью применимости и доказуемым поведением.

### 5.4. Современный профессиональный UX

Сложная domain model не оправдывает архаичный интерфейс. Complex engineering software должно быть современным, плотным, быстрым и пригодным для длительной работы.

### 5.5. Быстрая итерация — требование к Development Platform

Небольшое UI change должно быстро становиться видимым. Надёжность достигается risk-based gates, а не запуском полного набора тяжёлых проверок после изменения двух элементов интерфейса.

### 5.6. Standalone first

Комплекс работает самостоятельно. NPT и EOD — optional compatibility/integration boundaries, а не обязательные runtime dependencies.

## 6. Целевые пользователи и среда

Основные пользователи:

- оперативный и оперативно-ремонтный персонал;
- инженерно-технический персонал электроэнергетики;
- специалисты по схемам, мнемосхемам и operational documentation;
- инженеры, формирующие и проверяющие переключения;
- специалисты по поддержке NPT-compatible projects.

Предполагаемая среда:

- desktop workstation;
- несколько мониторов;
- большие проекты;
- длительные рабочие сессии;
- keyboard + mouse;
- local/offline operation;
- Windows и Linux как target classes, точный support фиксируется после Platform Stack Spike.

## 7. Модули продукта

- **Domain Core** — project/equipment/terminals/connections/topology/state/signals/rules/views.
- **UI Core** — application shell, workspace, design system, commands, inspectors, data views, canvas infrastructure.
- **Equipment Library** — semantic equipment types, terminal configurations, properties, states и approved representations.
- **Import & Reconciliation** — CSV/XLSX mapping, staging, normalization, ambiguity handling, diff/reconciliation и provenance.
- **Scheme Studio** — electrical views, domain-aware auto-layout, manual layout constraints, rendering/editing/output.
- **NPT Compatibility** — XSDE/XTABL, NPT properties/IDs, signal catalog/adapters и validation.
- **Switching / TBP** — clean-sheet operations, sequences, rule evaluation, interlocks, simulation и document generation.
- **Compliance Core** — normative registry, applicability, profile composition и local policy overlays.
- **Optional EOD Adapter** — тонкая интеграция только при успешном feasibility/cost gate.

## 8. Чем продукт не является

Текущее vision не включает:

- replacement full SCADA runtime;
- historian;
- universal CAD;
- remote real-equipment control platform;
- arbitrary automation platform;
- обязательную cloud collaboration platform.

## 9. Критерии успеха

1. Structured equipment/topology list превращается в usable project/scheme с существенно меньшей ручной работой, чем отрисовка с нуля.
2. Engineer corrections сохраняются при re-import.
3. Одна domain model используется несколькими модулями/views.
4. Scheme symbols/output трассируются к применимым standards profiles.
5. Switching validation выявляет uncertainty и объясняет ограничения через versioned rules.
6. Site-specific rules настраиваются без ослабления mandatory requirements.
7. NPT compatibility сохраняет необходимую vendor-format информацию без загрязнения neutral Core.
8. UI остаётся responsive и multi-monitor-capable на representative sizes.
9. Common UI changes можно визуально принять за минуты/часы, а не дни.
10. Standalone product остаётся usable без NPT или EOD integrations.
11. Интерактивный ChatGPT→VPS development loop и formal GitHub CI разделены и не требуют от владельца ежедневной SSH-работы.