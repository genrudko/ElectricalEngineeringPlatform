# Scheme Module и Equipment Library

Статус: канонический Foundation-документ

## 1. Назначение

Scheme module превращает neutral electrical model в editable engineering representations.

Equipment Library определяет reusable semantic equipment types и допустимые graphical/state representations.

```text
Equipment Library = что представляет собой оборудование
Scheme Module     = как project entities отображаются и размещаются в конкретном View
```

## 2. Ответственность Equipment Library

Type definition может предоставлять:

- semantic category/type ID;
- terminal schema и roles;
- typed properties/units/constraints;
- supported states;
- domain validation hooks/data;
- applicable manufacturer/model profiles;
- graphic representation bindings per profile/view;
- naming/designation metadata where appropriate.

Project instances вроде `QF-17` не принадлежат type library.

## 3. Native и compatibility representations

Один semantic type может иметь несколько representations:

```text
CircuitBreaker
├── native ГОСТ-oriented single-line symbol
├── operational-state native symbol
├── imported Visio/legacy representation
└── NPT CustElem compatibility representation
```

Compatibility representation не становится автоматически normative/native authority.

## 4. SchemeView

```text
SchemeView
├── identity/name/type
├── inclusion/filtering
├── graphic profile
├── entity placements
├── connection routes
├── labels/annotations
├── groups/regions/layers
├── layout constraints
├── viewport/document metadata
└── validation/output state
```

Electrical connections остаются в Domain Core. `ConnectionRoute` не является самой `Connection`.

## 5. Основные editing operations

Foundation-level operations:

- add existing project entity to view;
- create equipment через domain command и затем place representation;
- select/multi-select;
- move/align/distribute;
- rotate/orient where permitted;
- reconnect semantic terminal через explicit domain command;
- edit visual route без изменения semantic endpoints;
- edit typed properties через Inspector;
- copy/paste с controlled identity semantics;
- явно различать remove-from-view и delete-from-project;
- undo/redo;
- search/navigation по equipment identity/name/KKS/source.

## 6. Create/delete semantics

Dangerous ambiguity должна быть исключена:

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

## 7. Auto-layout integration

Scheme module получает `LayoutProposal` от Import/Auto-layout services и владеет accepted geometry/constraints.

Manual corrections превращаются в constraints/user-owned routes, что позволяет выполнять incremental updates.

Layout engine не может придумывать или менять topology ради красивого рисунка.

## 8. View families

Долгосрочно могут поддерживаться:

- normal single-line;
- temporary/repair variants;
- detailed bay/switchgear;
- operational;
- другие controlled electrical representations.

Все families не должны реализовываться одновременно в первом MVP.

## 9. State visualization

Renderer получает semantic state/quality вместе с view/profile rules.

Должны быть различимы:

```text
CLOSED + GOOD
OPEN + GOOD
INTERMEDIATE
UNKNOWN / BAD QUALITY
SIMULATED CLOSED
```

Unknown и simulated state не должны визуально сливаться с known observed state.

## 10. Large-project strategy

Техники выбираются по platform evidence и могут включать:

- scene graph/custom drawing вместо one heavyweight widget per primitive;
- spatial indexing;
- viewport culling;
- batched render/update;
- incremental layout/routing;
- text/geometry caches where safe;
- background computation для expensive non-UI tasks с deterministic commit обратно в model.

## 11. Routing

Connection routing — view geometry.

Требования:

- route endpoints anchored to representation terminals;
- junction/crossing semantics consistent with profile;
- user-owned bends/routes;
- reroute impacted region вместо whole diagram where feasible;
- diagnostics для impossible/overlapping routes.

Universal perfect autorouting не является MVP requirement.

## 12. Labels и designations

Labels могут формироваться из:

- equipment operational/designation names;
- KKS/identifier properties;
- rated values;
- state/value properties;
- view-specific annotations.

Profile определяет обязательные/разрешённые labels и правила placement.

## 13. Validation layers

```text
DOMAIN_ERROR
GRAPHIC_PROFILE_ERROR
LAYOUT_WARNING
IMPORT_PROVENANCE_WARNING
NPT_COMPATIBILITY_ERROR
```

Одного generic `invalid object` недостаточно.

## 14. Manufacturer profiles

Manufacturer/model profile может определять:

- ratings/defaults/limits;
- terminal layout/roles when model-specific;
- state constraints;
- additional switching restrictions;
- optional detailed representation.

Он не может ослаблять mandatory applicable constraints и требует source/revision provenance.

## 15. Evolution Equipment Library

Начинать с малого набора типов, достаточного для first real vertical slice.

Promotion gate для каждого type:

- domain meaning;
- terminal semantics;
- state semantics;
- validation;
- native graphic-profile evidence;
- Gallery render;
- save/load;
- import mapping;
- switching semantics where relevant.

## 16. Search/library UX

Search/filter должен работать по engineering meaning:

- category/type;
- voltage/application;
- manufacturer/model;
- standard/profile;
- tags/aliases;
- recent/frequent use;
- project-available types.

NPT library palette остаётся отдельной compatibility projection.

## 17. Связь с первым vertical slice

`IMPORT-TO-SCHEME-VERTICAL-SLICE-001` использует этот module на real small equipment set, импортированном из structured data, и проверяет auto-layout, manual correction, save/reopen и re-import.