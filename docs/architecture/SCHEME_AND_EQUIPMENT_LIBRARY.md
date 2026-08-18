# Scheme Module and Equipment Library

Статус: canonical foundation document

## 1. Purpose

The Scheme module turns the neutral electrical model into editable engineering representations. The Equipment Library defines reusable semantic equipment types and their permitted graphical/state representations.

```text
Equipment Library = what the equipment is
Scheme Module     = how project entities are represented/arranged in a view
```

## 2. Equipment Library responsibilities

A type definition may provide:

- semantic category/type ID;
- terminal schema and roles;
- typed properties/units/constraints;
- supported states;
- domain validation hooks/data;
- applicable manufacturer/model profiles;
- graphic representation bindings per profile/view;
- naming/designation metadata where appropriate.

Project instances such as `QF-17` do not belong in the type library.

## 3. Native vs compatibility representations

One semantic type can bind to several representations:

```text
circuit-breaker
├── native ГОСТ-oriented single-line symbol
├── operational-state native symbol
├── imported Visio/legacy representation
└── NPT CustElem compatibility representation
```

Compatibility representation is not automatically normative/native authority.

## 4. Scheme View

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

Electrical connections remain in Domain Core. A route is not the connection itself.

## 5. Core editing operations

Expected foundation operations:

- add existing project entity to view;
- create equipment through domain command then place representation;
- select/multi-select;
- move/align/distribute;
- rotate/orient where permitted;
- reconnect semantic terminal through explicit domain command;
- edit visual route without changing semantic endpoints;
- edit typed properties through Inspector;
- copy/paste with controlled identity semantics;
- clearly distinguish delete-from-view from delete-from-project;
- undo/redo;
- search/navigate by equipment identity/name/KKS/source.

## 6. Create/delete semantics

Dangerous ambiguity must be eliminated:

```text
Remove from this scheme view
≠
Delete equipment from project
```

```text
Move line bend
≠
Reconnect terminal
```

## 7. Auto-layout integration

Scheme module consumes `LayoutProposal` from Import/Auto-layout services and owns accepted geometry/constraints.

Manual corrections become constraints/user-owned routes, enabling incremental future updates.

The layout engine cannot invent/change topology to improve appearance.

## 8. View families

Long-term architecture may support normal single-line, temporary/repair variants, detailed bay/switchgear, operational and other controlled electrical representations.

Do not implement all families in the first MVP.

## 9. State visualization

Renderer receives semantic state/quality plus view/profile rules.

Must support explicit uncertainty and simulation distinction where needed:

```text
CLOSED + GOOD
OPEN + GOOD
INTERMEDIATE
UNKNOWN / BAD QUALITY
SIMULATED CLOSED
```

## 10. Large-project strategy

Expected techniques, selected by platform evidence:

- scene graph/custom drawing rather than one heavyweight widget per primitive;
- spatial indexing;
- viewport culling;
- batched render/update;
- incremental layout/routing;
- text/geometry caches where safe;
- background computation for expensive non-UI tasks with deterministic commit back to model.

## 11. Routing

Connection routing is view geometry.

Requirements:

- route endpoints anchored to representation terminals;
- junction/crossing semantics consistent with profile;
- user-owned bends/routes;
- reroute impacted region rather than whole diagram where feasible;
- diagnostics for impossible/overlapping routes.

Universal perfect autorouting is not an MVP requirement.

## 12. Labels and designations

Labels may come from equipment operational/designation names, KKS/identifier properties, rated values, state/value properties and view-specific annotations.

Profile controls which labels are required/permitted and how they are placed.

## 13. Validation layers

```text
DOMAIN_ERROR
GRAPHIC_PROFILE_ERROR
LAYOUT_WARNING
IMPORT_PROVENANCE_WARNING
NPT_COMPATIBILITY_ERROR
```

One generic `invalid object` bucket is insufficient.

## 14. Manufacturer profiles

A manufacturer/model profile may define ratings/defaults/limits, terminal layout/roles if model-specific, state constraints, additional switching restrictions and optional detailed representation.

It cannot weaken mandatory applicable constraints and requires source/revision provenance.

## 15. Equipment library evolution

Start with a small family sufficient for the real vertical slice. Promotion gate for each type includes domain meaning, terminal semantics, state semantics, validation, native graphic-profile evidence, Gallery render, save/load, import mapping and switching semantics where relevant.

## 16. Search/library UX

Search/filter by engineering meaning: category/type, voltage/application, manufacturer/model, standard/profile, tags/aliases, recent/frequent use and project-available types.

NPT library palette is a separate compatibility projection.

## 17. First vertical slice relationship

`IMPORT-TO-SCHEME-VERTICAL-SLICE-001` exercises this module with a real small equipment set imported from structured data, auto-layout, manual correction, save/reopen and re-import.
