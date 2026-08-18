# Graphics / ГОСТ-ЕСКД Profile Architecture

Статус: canonical foundation document

## 1. Goal

Native electrical schemes must be generated and edited through **versioned graphic-standard profiles** so that ГОСТ/ЕСКД claims are traceable and testable.

The product must not equate `looks like an electrical scheme` with standards compliance.

## 2. Foundation baseline

Verified primary baseline includes:

- ГОСТ 2.701-2008 — scheme kinds/types and general execution requirements;
- ГОСТ 2.702-2011 — rules for presentation/execution of electrical schemes.

Applicable UGO/line/lettering/document standards for each implemented equipment/document family are added explicitly after official-catalogue verification.

## 3. Profile composition

```text
GraphicProfile
├── profile ID/version
├── normative baseline date
├── source standards
├── scheme type(s)
├── symbol bindings
├── line/connection rules
├── text/designation rules
├── layout/document rules
├── output/print rules
├── enterprise stricter additions (optional)
└── coverage matrix
```

A profile version is immutable once used for an accepted/released document; future changes create a new profile version/migration choice.

## 4. Semantic symbol library

Native symbol is not just geometry.

```text
GraphicSymbolDefinition
├── semantic equipment/type binding
├── graphic profile binding
├── source/provenance
├── geometry
├── terminals/anchors
├── allowed orientation/rotation
├── parameterization/stretch rules
├── state variants
├── designation/text anchors
├── print behavior
└── validation evidence
```

Semantic equipment identity remains in Domain Core; symbol is one representation.

## 5. Multiple graphic representations

One equipment type may have multiple valid representations depending on scheme/document type, detail level, normative profile, operational/normal view and compatibility format.

NPT representation is not automatically promoted into native ГОСТ profile.

## 6. Required validation dimensions

Where applicable, validate:

- symbol type/profile permitted for semantic object;
- terminal/connection points correspond to semantic terminals;
- required line types/weights/classes;
- crossings/junction indications;
- labels/designations/content;
- orientation/allowed transformations;
- document/sheet annotations;
- consistency across related views/documents;
- print/export readability;
- profile-specific spacing/placement rules only where traceable to source/profile.

Do not invent numerical rules and label them ГОСТ without source support.

## 7. Layout vs compliance

Distinguish:

```text
NORMATIVE_VIOLATION
PROFILE_VIOLATION
LAYOUT_QUALITY_WARNING
```

A product layout heuristic is not automatically a normative requirement.

## 8. Symbol provenance

Each promoted native symbol records normative sources, independent authoring/import source, any VSDX/VSSX master used as reference, any NPT material used only for comparison, licensing/usage status and validation evidence.

Never silently copy proprietary NPT/third-party assets into native library.

## 9. VSDX/VSSX role

Visio masters may be engineering sources for geometry/connection points/historical corporate libraries, but they are not normative authority.

Imported masters require source/profile mapping. Unsupported ShapeSheet behavior is diagnosed. A non-compliant legacy shape remains compatibility/legacy representation or requires correction.

## 10. Operational state rendering

```text
EquipmentState
   ↓
RepresentationStateResolver
   ↓
geometry/style/text variant
```

Colors/styles do not change canonical state. Unknown state needs explicit representation and must not look identical to known OPEN/CLOSED where misleading.

## 11. Print/export

Profile acceptance includes deterministic high-quality output. Initial targets after stack selection:

- vector PDF where practical;
- high-resolution print;
- controlled SVG/export where appropriate;
- VSDX compatibility only according to a separately supported subset.

## 12. Coverage matrix

Each profile release maintains a matrix:

| Area | Source | Rules identified | Implemented | Tested | Limitations |
|---|---|---:|---:|---:|---|
| scheme type/general | ГОСТ 2.701-2008 | TBD | TBD | TBD | extraction pending |
| electrical execution | ГОСТ 2.702-2011 | TBD | TBD | TBD | extraction pending |
| breaker UGO | exact verified UGO standard | TBD | TBD | TBD | source pending |
| transformer UGO | exact verified UGO standard | TBD | TBD | TBD | source pending |

A profile is never labelled `full GOST compliant` while rows remain undefined.

## 13. First implementation slice

For the first representative single-line fragment:

1. verify applicable Rosstandart standards for chosen equipment families;
2. create semantic type/terminal schema;
3. create independent native symbol;
4. add state variants;
5. add automated/profile validation;
6. render Gallery samples;
7. print/export comparison;
8. owner visual acceptance.

## 14. Enterprise graphical rules

Enterprise/site standards may add stricter conventions as explicit profile overlays. Diagnostics identify the actual source layer rather than calling enterprise convention a ГОСТ requirement.

## 15. Non-goals

Foundation does not attempt to ingest every ESKD standard, freeze a complete symbol library or infer norms from legacy NPT/Visio drawings.
