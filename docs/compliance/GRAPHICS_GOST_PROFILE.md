# Архитектура графических ГОСТ/ЕСКД profiles

Статус: канонический Foundation-документ

## 1. Цель

Native electrical schemes создаются и редактируются через **versioned graphic-standard profiles**, чтобы claims по ГОСТ/ЕСКД были traceable и testable.

Продукт не приравнивает `looks like an electrical scheme` к standards compliance.

## 2. Foundation baseline

Verified primary baseline:

- ГОСТ 2.701-2008 — виды/типы схем и общие требования к выполнению;
- ГОСТ 2.702-2011 — правила выполнения электрических схем.

Applicable УГО/line/lettering/document standards для каждой implemented equipment/document family добавляются только после official-catalogue verification.

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

Profile version становится immutable после использования в accepted/released document. Future change создаёт new profile version/migration choice.

## 4. Semantic symbol library

Native symbol — не просто geometry.

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

Semantic equipment identity остаётся в Domain Core; symbol является одним representation.

## 5. Multiple graphic representations

Один equipment type может иметь несколько valid representations в зависимости от:

- scheme/document type;
- detail level;
- normative profile;
- operational/normal view;
- compatibility format.

NPT representation не становится автоматически native ГОСТ profile.

## 6. Required validation dimensions

Where applicable проверяются:

- symbol type/profile permitted для semantic object;
- terminal/connection points соответствуют semantic terminals;
- required line types/weights/classes;
- crossings/junction indications;
- labels/designations/content;
- orientation/allowed transformations;
- document/sheet annotations;
- consistency across related views/documents;
- print/export readability;
- profile-specific spacing/placement rules только при traceable source/profile.

Нельзя придумывать numerical rules и называть их ГОСТ requirement без source support.

## 7. Layout vs compliance

Различать:

```text
NORMATIVE_VIOLATION
PROFILE_VIOLATION
LAYOUT_QUALITY_WARNING
```

Product layout heuristic не является автоматически нормативным требованием.

## 8. Symbol provenance

Каждый promoted native symbol фиксирует:

- normative sources;
- independent authoring/import source;
- VSDX/VSSX master used as reference, если применимо;
- NPT material used only for comparison, если применимо;
- licensing/usage status;
- validation evidence.

Нельзя молча копировать proprietary NPT/third-party assets в native library.

## 9. Роль VSDX/VSSX

Visio masters могут быть engineering sources для geometry/connection points/historical corporate libraries, но не являются normative authority.

Imported masters требуют source/profile mapping. Unsupported ShapeSheet behavior диагностируется.

Non-compliant legacy shape остаётся compatibility/legacy representation или требует correction.

## 10. Operational state rendering

```text
EquipmentState
   ↓
RepresentationStateResolver
   ↓
geometry/style/text variant
```

Colors/styles не меняют canonical state.

Unknown state должен иметь explicit representation и не выглядеть идентично known OPEN/CLOSED, если это вводит пользователя в заблуждение.

## 11. Print/export

Profile acceptance включает deterministic high-quality output.

Initial targets после stack selection:

- vector PDF where practical;
- high-resolution print;
- controlled SVG/export where appropriate;
- VSDX compatibility только в отдельно supported subset.

## 12. Coverage matrix

Каждый profile release содержит matrix:

| Area | Source | Rules identified | Implemented | Tested | Limitations |
|---|---|---:|---:|---:|---|
| scheme type/general | ГОСТ 2.701-2008 | TBD | TBD | TBD | extraction pending |
| electrical execution | ГОСТ 2.702-2011 | TBD | TBD | TBD | extraction pending |
| breaker УГО | exact verified УГО standard | TBD | TBD | TBD | source pending |
| transformer УГО | exact verified УГО standard | TBD | TBD | TBD | source pending |

Profile не получает label `full GOST compliant`, пока строки coverage остаются undefined.

## 13. First implementation slice

Для первого representative single-line fragment:

1. verify applicable Rosstandart standards для chosen equipment families;
2. создать semantic type/terminal schema;
3. создать independent native symbol;
4. добавить state variants;
5. добавить automated/profile validation;
6. render Gallery samples;
7. выполнить print/export comparison;
8. owner visual acceptance.

## 14. Enterprise graphical rules

Enterprise/site standards могут добавлять stricter conventions как explicit profile overlays.

Diagnostics обязаны показывать actual source layer, а не называть enterprise convention ГОСТ requirement.

## 15. Non-goals

Foundation не пытается:

- ingest every ESKD standard;
- freeze complete symbol library;
- infer norms из legacy NPT/Visio drawings.