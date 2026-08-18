# Архитектура UI Core

Статус: канонический Foundation-документ

## 1. Почему UI Core — архитектурный слой

Продукт не должен превратиться в технически правильный engineering engine, спрятанный за архаичным, неудобным или утомительным интерфейсом.

UI Core — first-class shared subsystem и проектируется с той же серьёзностью, что Domain Core.

Целевой UX:

> современный, плотный, профессиональный desktop engineering UI для длительной работы, больших проектов, keyboard+mouse и нескольких мониторов.

Не принимаются две крайности:

- legacy/MS-DOS/early-Win32 usability и visual language;
- sparse/mobile/tablet composition на desktop, где полезная площадь теряется из-за oversized controls и whitespace.

## 2. Язык интерфейса

Основной пользовательский язык UI — **русский**.

Русскими должны быть:

- меню и команды;
- названия панелей и вкладок;
- Property Inspector labels;
- dialogs/notifications;
- validation/error/warning explanations;
- command palette labels;
- встроенная справка;
- user-facing diagnostics;
- generated UI-facing descriptions.

Внутренние identifiers остаются английскими:

```text
CircuitBreaker
TopologyGraph
SwitchingOperation
POLICY_CONFLICT_WEAKENING
```

Пользователь не должен видеть internal enum/error code вместо нормального русского объяснения.

Исключения допускаются для real technical data: KKS, NPT command names, filenames, standard designations, external field names и т. п.

Канонические языковые правила:

- `docs/project/LANGUAGE_POLICY.md`;
- `docs/project/RU_EN_ENGINEERING_GLOSSARY.md`.

## 3. Зона ответственности UI Core

```text
UI Core
├── Application Shell
├── Workspace / Documents
├── Multi-window
├── Design System / Themes
├── Commands / Shortcuts
├── Selection infrastructure
├── Property Inspector framework
├── Tree/List/Table infrastructure
├── Search / Command Palette
├── Dialogs / Notifications
├── Status / Diagnostics surfaces
├── Canvas framework primitives
├── Clipboard / Drag-drop UX contracts
├── HiDPI / mixed-DPI handling
├── Accessibility / keyboard focus
└── UI Gallery / visual test fixtures
```

UI Core не владеет switching rules, NPT semantics или equipment-specific validation.

## 4. Application Shell

Application Shell должен поддерживать:

- project/workspace identity;
- основную навигацию без бессмысленной потери рабочей площади;
- document tabs;
- split document regions там, где это реально полезно;
- detachable documents/windows;
- persistent tool panels;
- status/diagnostics;
- global search и command access;
- module-contributed views/actions через explicit contracts.

Не копировать Office Ribbon или IDE docking один в один только из-за знакомого визуального шаблона. Использовать их идеи только там, где они реально уменьшают user task cost.

## 5. Workspace и multi-monitor

Multi-monitor — first-class acceptance scenario.

Representative layout:

```text
Монитор 1 — основная electrical scheme
Монитор 2 — equipment / properties / search
Монитор 3 — Switching sequence
Монитор 4 — table / NPT / diagnostics / secondary view
```

Workspace сохраняет:

- открытые views/documents;
- positions/sizes top-level windows;
- monitor assignment с fallback при изменении monitor set;
- splits/tabs;
- visible panels;
- размеры и порядок панелей.

Corrupted/outdated workspace не должен блокировать opening project; требуется safe/default recovery.

## 6. Design System

Design System определяет минимум:

- typography scale;
- spacing scale;
- control density;
- borders/separators/elevation;
- semantic colors;
- focus/hover/pressed/selected/disabled/error/warning states;
- icons/pictograms;
- table/tree row density;
- panel headers;
- dialogs;
- theme behavior;
- engineering status colors отдельно от декоративной темы.

Важный engineering meaning нельзя кодировать только цветом.

## 7. UI Gallery

UI Gallery создаётся рано и остаётся постоянно запускаемой.

Она показывает shared controls и значимые states без запуска полного product runtime:

```text
Buttons / Toolbars
Text / Numeric / Enum editors
Property rows/groups
Tabs / Document headers
Trees / virtual lists
Virtual tables
Search results
Dialogs
Notifications
Validation messages
Context menus
Command palette
Canvas selection handles
Symbol state samples
Empty/loading/error states
```

Назначение Gallery:

- быстрая visual iteration;
- screenshot regression;
- theme/HiDPI checks;
- agent/developer inspection;
- защита от module-specific control duplication;
- проверка русского UI текста без запуска всего продукта.

## 8. Property Inspector

Shared Inspector framework поддерживает typed descriptors/editors, но UI Core не знает business semantics конкретного оборудования.

Требуется:

- single/multi-selection;
- mixed-value state;
- validation feedback;
- undo/redo command integration;
- searchable/filterable property groups;
- keyboard-friendly editing.

Modules могут подключать specialized editors:

- equipment state/property editor;
- signal/KKS chooser;
- NPT typed `scdCommand` editor;
- compliance/source selector.

Внутреннее свойство может называться `RatedVoltage`, а пользователь видит `Номинальное напряжение`.

## 9. Trees и tables

Electrical projects могут содержать десятки и сотни тысяч listable entities.

UI использует virtualization/incremental loading там, где это требуется измерениями.

Требования:

- stable selection;
- быстрые sort/filter/search;
- keyboard navigation;
- column presets;
- copy/export selected data;
- ясные validation/status indicators;
- отсутствие полного создания heavyweight control для каждой строки, если benchmark показывает плохую scalability.

Platform Stack Spike включает representative table на 100 000 строк.

## 10. Command System

Действия адресуются через shared command registry:

```text
command ID
русская label
icon
shortcut
availability / canExecute
execution
context
```

Одна semantic command должна согласованно появляться в toolbar, context menu, command palette и keyboard shortcut без duplicate enablement logic.

## 11. Canvas Framework

UI Core владеет общей viewport/interaction infrastructure, а Scheme/NPT modules — rendering semantics.

Shared concerns:

- viewport transform;
- zoom/pan;
- spatial hit-testing contract;
- selection model;
- marquee selection;
- pointer capture;
- snapping/guides;
- overlays/handles;
- keyboard navigation;
- viewport diagnostics/performance instrumentation.

Не реализовывать каждый graphic primitive как отдельный heavyweight desktop control без benchmark evidence.

## 12. UX редактирования схем

Основные правила:

- selection предсказуем и обратим;
- reconnect semantic terminal визуально и семантически отличается от перемещения route geometry;
- destructive operation показывает effect до commit, если risk этого требует;
- auto-layout никогда молча не уничтожает locked/manual placement;
- validation позволяет быстро перейти к affected entity/view/rule;
- selection/property context по возможности сохраняется между views.

Критические различия:

```text
Удалить из текущего вида
≠
Удалить оборудование из проекта
```

```text
Изменить изгиб линии
≠
Переподключить Terminal
```

## 13. UX ошибок и неопределённости

Минимальные classes:

```text
INFO
WARNING
ERROR
BLOCKED
UNKNOWN / REQUIRES_CONFIRMATION
```

Internal codes остаются английскими, но UI explanation — русское.

Сообщение должно отвечать:

- что произошло;
- какой object/rule/source затронут;
- можно ли продолжать;
- что требуется для разрешения ситуации.

В engineering/switching context uncertainty нельзя визуально нормализовать в ordinary safe state.

## 14. UX budgets

Начальные qualitative targets:

- project navigation/search быстрые и keyboard-accessible;
- property editing не превращается в цепочку modal dialogs;
- repetitive structures создаются data-driven, а не вручную построчно;
- multi-monitor workspace восстанавливается без ручной перестановки окон каждый запуск;
- import собирает ambiguity/conflicts в один review workflow;
- основные действия названы понятными русскими инженерными терминами.

## 15. Developer UX budget

- shared component запускается в Gallery;
- visual snapshot/headless rendering доступен where framework permits;
- небольшой UI patch быстро даёт targeted preview;
- visual acceptance происходит до unrelated full-system gates;
- theme/layout changes используют centralized tokens/components, а не override stacks;
- UI text централизован для consistent Russian terminology.

## 16. HiDPI и mixed DPI

Acceptance покрывает:

- common scale classes;
- перенос окон между мониторами с разными scale factors;
- crisp vector scheme rendering;
- stable text/control sizes;
- отсутствие coordinate mismatch между canvas rendering и hit-testing.

## 17. Accessibility

Нужны:

- visible keyboard focus;
- logical tab order;
- predictable shortcuts;
- readable contrast;
- non-color-only status cues;
- scalable text/control metrics при сохранении engineering density.

## 18. Visual acceptance contract

UI change не считается accepted только потому, что compile green.

Evidence по scope может включать:

- UI Gallery screenshot;
- targeted headless screenshot;
- interaction test;
- owner preview build;
- mixed-DPI/multi-window manual check.

Для небольшого UI-only patch нельзя блокировать первую visual review полным NPT corpus/topology/switching suite.

## 19. Framework independence

Документ framework-neutral.

Avalonia и Qt должны доказать в Platform Stack Spike, что реализуют этот contract без деградации professional desktop UX, русского user-facing layer, large data views и heavy engineering canvas.