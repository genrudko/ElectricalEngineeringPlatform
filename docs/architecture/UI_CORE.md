# Архитектура UI Core

Статус: канонический Foundation-документ

## 1. Почему UI Core — архитектурный слой

Продукт не должен превратиться в technically correct engineering engine, скрытый за archaic, неудобным или утомительным interface.

UI Core — first-class shared subsystem и проектируется с той же серьёзностью, что Domain Core.

Целевой UX:

> современный, плотный, профессиональный desktop engineering UI для длительной работы, больших projects, keyboard+mouse и нескольких monitors.

Не принимаются две крайности:

- legacy/MS-DOS/early-Win32 usability и visual language;
- sparse/mobile/tablet composition на desktop, где useful area теряется из-за oversized controls и whitespace.

## 2. Язык интерфейса

Основной user-facing язык UI — **русский**.

Русскими должны быть:

- menus и commands;
- panel/tab titles;
- Property Inspector labels;
- dialogs/notifications;
- validation/error/warning explanations;
- command palette labels;
- встроенная help;
- user-facing diagnostics;
- generated UI-facing descriptions.

Internal identifiers остаются English:

```text
CircuitBreaker
TopologyGraph
SwitchingOperation
POLICY_CONFLICT_WEAKENING
```

Пользователь не должен видеть internal enum/error code вместо нормального Russian explanation.

Exceptions допустимы для real technical data: KKS, NPT command names, filenames, standard designations, external field names и т. п.

Canonical language rules:

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
- основную navigation без бессмысленной потери working area;
- document tabs;
- split document regions там, где это реально useful;
- detachable documents/windows;
- persistent tool panels;
- status/diagnostics;
- global search и command access;
- module-contributed views/actions через explicit contracts.

Не копировать Office Ribbon или IDE docking один в один только из-за familiarity. Использовать их идеи там, где они реально уменьшают user task cost.

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

- open views/documents;
- positions/sizes top-level windows;
- monitor assignment с fallback при изменении monitor set;
- splits/tabs;
- visible panels;
- размеры и порядок panels.

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
- engineering status colors отдельно от decorative theme.

Важный engineering meaning нельзя кодировать только цветом.

## 7. UI Gallery

UI Gallery создаётся рано и остаётся permanently runnable.

Она показывает shared controls и значимые states без запуска full product runtime:

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

- fast visual iteration;
- screenshot regression;
- theme/HiDPI checks;
- agent/developer inspection;
- защита от module-specific control duplication;
- проверка Russian UI text без запуска full product.

## 8. Property Inspector

Shared Inspector framework поддерживает typed descriptors/editors, но UI Core не знает business semantics конкретного equipment type.

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

Internal property может называться `RatedVoltage`, а user sees `Номинальное напряжение`.

## 9. Trees и tables

Electrical projects могут содержать десятки/сотни тысяч listable entities.

UI использует virtualization/incremental loading там, где это требуется measurements.

Требования:

- stable selection;
- быстрые sort/filter/search;
- keyboard navigation;
- column presets;
- copy/export selected data;
- ясные validation/status indicators;
- отсутствие полного создания heavyweight control для каждой row, если benchmark показывает плохую scalability.

Platform Stack Spike включает representative table на 100 000 rows.

## 10. Command System

Actions адресуются через shared command registry:

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

Не реализовывать каждый graphic primitive как separate heavyweight desktop control без benchmark evidence.

## 12. UX редактирования схем

Основные правила:

- selection predictable и reversible;
- reconnect semantic terminal визуально/семантически отличается от moving route geometry;
- destructive operation показывает effect до commit, когда risk этого требует;
- auto-layout никогда молча не уничтожает locked/manual placement;
- validation позволяет быстро перейти к affected entity/view/rule;
- selection/property context по возможности сохраняется между views.

Critical distinctions:

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

## 13. Ошибки и неопределённость

Минимальные classes:

```text
INFO
WARNING
ERROR
BLOCKED
UNKNOWN / REQUIRES_CONFIRMATION
```

Internal codes остаются English, но user explanation — Russian.

Message должен отвечать:

- что произошло;
- какой object/rule/source затронут;
- можно ли продолжать;
- что требуется для resolution.

В engineering/switching context uncertainty нельзя визуально normalise в ordinary safe state.

## 14. UX budgets

Initial qualitative targets:

- project navigation/search быстрые и keyboard-accessible;
- property editing не превращается в sequence modal dialogs;
- repetitive structures создаются data-driven, а не вручную row-by-row;
- multi-monitor workspace восстанавливается без manual rearrangement каждый launch;
- import собирает ambiguity/conflicts в один review workflow;
- основные actions названы понятными Russian engineering terms.

## 15. Developer UX budget

- shared component запускается в Gallery;
- visual snapshot/headless rendering доступен where framework permits;
- small UI patch быстро даёт targeted preview;
- visual acceptance происходит до unrelated full-system gates;
- theme/layout changes используют centralized tokens/components, а не override stacks;
- UI text централизован для consistent Russian terminology.

## 16. HiDPI и mixed DPI

Acceptance покрывает:

- common scale classes;
- перенос windows между monitors с разными scale factors;
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

Для small UI-only patch нельзя блокировать first visual review full NPT corpus/topology/switching suite.

## 19. Framework independence

Document framework-neutral.

Avalonia и Qt должны доказать в Platform Stack Spike, что реализуют этот contract без degradation professional desktop UX, Russian user-facing layer, large data views и heavy engineering canvas.