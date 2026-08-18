# Архитектура UI Core

Статус: canonical foundation document

## 1. Почему UI Core — это архитектура

Продукт не должен превратиться в технически правильный инженерный engine, спрятанный за архаичным, неудобным или утомительным интерфейсом.

UI Core — first-class shared subsystem, который проектируется с той же серьёзностью, что и Domain Core.

Целевой UX:

> современный, плотный, профессиональный desktop engineering UI для длительной работы, больших проектов, клавиатуры+мыши и нескольких мониторов.

Не принимаются оба крайних варианта:

- legacy/MS-DOS/early-Win32 usability и визуальный язык;
- sparse/mobile/tablet composition на desktop, где полезная площадь теряется из-за огромных controls и whitespace.

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

Исключения допускаются для реальных technical data: KKS, NPT command names, filenames, standard designations, external field names и т. п.

Canonical языковые правила определены в:

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
- основную навигацию без бессмысленного съедания рабочей площади;
- document tabs;
- split document regions там, где это реально полезно;
- detachable documents/windows;
- persistent tool panels;
- status/diagnostics;
- global search и command access;
- module-contributed views/actions через явные contracts.

Не копировать Office Ribbon или IDE docking один в один только потому, что это знакомые шаблоны. Использовать их идеи только там, где они реально уменьшают стоимость задачи пользователя.

## 5. Workspace и multi-monitor

Multi-monitor — first-class acceptance scenario.

Типичный рабочий расклад:

```text
Монитор 1 — основная электрическая схема
Монитор 2 — оборудование / свойства / поиск
Монитор 3 — Switching / TBP sequence
Монитор 4 — таблица / NPT / diagnostics / secondary view
```

Workspace должен сохранять:

- открытые views/documents;
- positions/sizes top-level windows;
- monitor assignment с fallback, если состав мониторов изменился;
- splits/tabs;
- visible panels;
- размеры и порядок панелей.

Повреждённый или устаревший workspace не должен блокировать открытие проекта: требуется safe/default recovery.

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

Важный инженерный смысл нельзя кодировать только цветом.

## 7. UI Gallery

UI Gallery создаётся рано и остаётся постоянно запускаемой.

Она должна показывать shared controls и все значимые состояния без необходимости открывать полный проект/runtime:

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
- защита от ситуации, где каждый модуль изобретает свои controls;
- проверка русского UI текста без запуска всего продукта.

## 8. Property Inspector

Shared Inspector framework поддерживает typed descriptors/editors, но UI Core не должен знать business semantics конкретного оборудования.

Нужно поддержать:

- single/multi-selection;
- mixed-value state;
- validation feedback;
- undo/redo command integration;
- searchable/filterable property groups;
- keyboard-friendly editing.

Modules могут подключать специализированные editors:

- equipment state/property editor;
- signal/KKS chooser;
- NPT typed `scdCommand` editor;
- compliance/source selector.

Внутреннее свойство может называться `RatedVoltage`, но пользователь видит, например, `Номинальное напряжение`.

## 9. Trees и tables

Electrical projects могут содержать десятки и сотни тысяч listable entities. UI должен использовать virtualization/incremental loading там, где это требуется измерениями.

Требования:

- stable selection;
- быстрые sort/filter/search;
- keyboard navigation;
- column presets;
- copy/export selected data;
- ясные validation/status indicators;
- отсутствие полного создания heavyweight control для каждой строки, если benchmark показывает плохую масштабируемость.

Platform Stack Spike включает representative table на 100 000 строк.

## 10. Command System

Действия должны адресоваться через shared command registry:

```text
command ID
русская label
icon
shortcut
availability / canExecute
execution
context
```

Это позволяет одной semantic command согласованно появляться в toolbar, context menu, command palette и keyboard shortcut без дублирования enablement logic.

## 11. Canvas Framework

UI Core владеет общей viewport/interaction infrastructure, а Scheme/NPT modules — rendering semantics.

Shared concerns могут включать:

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

## 12. UX принцип редактирования схем

Основные правила:

- selection предсказуем и обратим;
- reconnect semantic terminal визуально и семантически отличается от перемещения route geometry;
- destructive operation показывает эффект до commit, если риск это требует;
- auto-layout никогда молча не уничтожает locked/manual placement;
- validation позволяет быстро перейти к affected entity/view/rule;
- selection/property context по возможности сохраняется при переходах между views.

Особенно важно различать:

```text
Удалить из текущего вида
≠
Удалить оборудование из проекта
```

и:

```text
Изменить изгиб линии
≠
Переподключить терминал
```

## 13. UX ошибок и неопределённости

Различать как минимум:

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
- какой объект/rule/source затронут;
- можно ли продолжать работу;
- что требуется сделать для разрешения ситуации.

В engineering/switching context неопределённость нельзя визуально нормализовать в обычное безопасное состояние.

## 14. UX budgets

Начальные qualitative targets:

- project navigation/search быстрые и keyboard-accessible;
- property editing не превращается в цепочку modal dialogs;
- repetitive 84-WTG table/scheme structure создаётся data-driven способом, а не построчно;
- multi-monitor workspace восстанавливается без ручной перестановки окон каждый запуск;
- import собирает ambiguity/conflicts в один review workflow вместо десятков modal interruptions;
- основные действия названы понятными русскими инженерными терминами.

## 15. Developer UX budget

- shared component запускается в Gallery;
- visual snapshot/headless rendering доступен там, где framework это позволяет;
- небольшой UI patch быстро даёт targeted preview;
- visual acceptance выполняется до unrelated full-system gates;
- theme/layout changes используют centralized tokens/components, а не override stacks;
- UI text централизован настолько, чтобы можно было поддерживать consistent Russian terminology без поиска строк по всему codebase.

## 16. HiDPI и mixed DPI

Acceptance должен покрывать:

- распространённые scale classes;
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
- scalable text/control metrics при сохранении разумной engineering density.

## 18. Visual acceptance contract

UI change не считается принятым только потому, что он компилируется.

В зависимости от scope evidence может включать:

- UI Gallery screenshot;
- targeted headless screenshot;
- interaction test;
- owner preview build;
- mixed-DPI/multi-window manual check.

Для небольшого UI-only patch нельзя блокировать первую visual review полным NPT corpus/topology/switching suite.

## 19. Framework independence

Документ framework-neutral.

Avalonia и Qt должны доказать во время Platform Stack Spike, что способны реализовать этот contract без деградации профессионального desktop UX, русского user-facing слоя, больших data views и heavy engineering canvas.