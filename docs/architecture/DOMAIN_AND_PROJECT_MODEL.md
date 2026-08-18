# Domain и Project Model

Статус: канонический Foundation-документ

## 1. Source of truth

Каноническим инженерным источником истины является `ElectricalProject`.

```text
ElectricalProject
├── ProjectIdentity
├── Sites / Facilities
├── VoltageLevels
├── Equipment
├── Terminals
├── Connections
├── Signals
├── States
├── ComplianceProfileRefs
├── Views
├── SourceProvenance
└── ModuleExtensions
```

Exact classes/schema остаются PENDING до implementation spike. Этот документ определяет semantics/invariants, а не фиксированную serialization syntax.

## 2. Identity

Все canonical engineering entities используют stable internal IDs, независимые от display names/KKS/vendor IDs.

Правила:

- изменение display name не меняет identity;
- external IDs являются mapping/provenance data, а не canonical identity;
- duplicate external IDs диагностируются;
- stable IDs сохраняются при layout/view changes.

## 3. Equipment

`Equipment` представляет semantic device/object, а не картинку.

Core attributes намеренно компактны:

- stable ID;
- equipment type reference;
- names/designations;
- object/site/voltage context;
- typed properties;
- terminal set;
- ссылки/bindings на state data where applicable;
- source/provenance references.

Observed/imported, simulated и planned state не должны смешиваться внутри одного неразличимого `currentState` field. Их context определяется отдельно, см. раздел 9.

Equipment type definitions живут в Equipment Library, а не в giant inheritance hierarchy.

Representative types по мере реальной необходимости:

- `CircuitBreaker`;
- `Disconnector`;
- `EarthingSwitch`;
- `Busbar` / `BusSection`;
- `Transformer` / `Autotransformer`;
- `TransmissionLine` / `CableLine`;
- `Generator`;
- `Load`;
- `CurrentTransformer`;
- `VoltageTransformer`.

## 4. Equipment Library

Type definition может объявлять:

- semantic type/category;
- terminal schema;
- supported properties;
- supported states;
- validation constraints;
- graphic representation bindings;
- optional manufacturer/model profiles.

Manufacturer/model profile может добавлять stricter ratings/constraints без переопределения core equipment category.

## 5. Terminals

`Terminal` — semantic connection point оборудования.

Coordinates не являются terminal identity. Symbol exposes visual anchors, mapped to semantic terminals; перемещение symbol не пересоздаёт terminals.

## 6. Connections

`Connection` представляет electrical/network relationship между semantic endpoints и не владеет screen polyline geometry.

Правила:

- endpoints должны существовать;
- terminal/type/cardinality rules валидируются;
- connection changes — explicit transactions;
- одна `Connection` может render по-разному в нескольких views.

## 7. TopologyGraph

Ожидаемые capabilities:

- connectivity queries;
- path search;
- electrical islands/sections;
- state-dependent continuity;
- energized/de-energized/unknown propagation с explicit assumptions;
- switching impact calculation;
- diagnostics impossible/dangling structures.

Критическое различие:

```text
NO_PATH
PATH_OPEN_BY_KNOWN_STATE
PATH_CLOSED
PATH_STATUS_UNKNOWN
```

Unknown data не превращается в optimistic safe state.

## 8. State semantics

State состоит из typed values с knowledge/quality semantics.

```text
CircuitBreakerPosition:
  OPEN | CLOSED | INTERMEDIATE | UNKNOWN

Energization:
  ENERGIZED | DEENERGIZED_PROVEN | UNKNOWN

SignalQuality:
  GOOD | BAD | UNCERTAIN | UNKNOWN
```

`DEENERGIZED_PROVEN` намеренно отличается от отсутствия известного energized path при incomplete data.

## 9. State contexts

Safety-relevant state всегда существует в явном context. Минимально архитектура различает:

```text
OBSERVED_BASELINE
SIMULATED
PLANNED_TARGET
```

Exact type names могут уточняться при реализации, но semantic separation обязательна.

### `OBSERVED_BASELINE`

Состояние, полученное из наблюдения, импорта, ручного ввода или другого accepted source. Должно сохранять source/provenance, quality и timestamp/revision where applicable.

### `SIMULATED`

Отдельный state context, получаемый при последовательном применении `SwitchingOperation` к выбранному baseline. Он не изменяет observed baseline и не утверждает фактическое состояние оборудования.

### `PLANNED_TARGET`

Желаемое/целевое состояние, если workflow требует его хранить. Оно не является ни observed, ни simulated state.

Правила:

- state query всегда выполняется относительно explicit context/snapshot;
- `TopologyGraph` calculation, rule evaluation и Switching simulation получают state context явно;
- simulated state нельзя silently persist как observed/current real-world state;
- UI обязан различать baseline/simulation/target, если различие влияет на решение пользователя;
- `UNKNOWN` и quality semantics сохраняются независимо в каждом context.

## 10. Signals

`Signal` — domain binding/reference, не обязательно online runtime channel.

Он может содержать:

- source/system;
- KKS/other designation;
- type/unit;
- relation to equipment/property/state;
- quality semantics;
- provenance.

NPT ASU/TECH implementation details остаются в NPT adapter/catalog.

## 11. Views

`View` — controlled representation поверх project entities.

Возможные views:

- general single-line;
- voltage-level;
- detailed bay/switchgear;
- operational;
- NPT-compatible mnemonic;
- tabular;
- switching simulation.

View хранит entity inclusion/filters, representation profile, geometry/layout, labels/annotations, layer/group settings и layout constraints, но не duplicate authoritative topology.

## 12. Layout constraints

Manual correction после auto-layout является first-class data:

- pinned position;
- relative ordering;
- orientation;
- bus extent/orientation;
- group/bay placement;
- user-owned edge route/bends;
- protected label placement;
- layout regions.

Auto-layout обязан сохранять protected constraints при incremental updates.

## 13. Provenance

Imported/generated entities сохраняют достаточно provenance для объяснения origin и reconciliation:

```text
SourceRef
├── source type
├── source identity/file fingerprint
├── external row/object ID
├── mapping profile version
├── import revision
└── transformation notes/confidence
```

Provenance не является canonical identity.

## 14. Compliance profile references

```text
ProjectCompliance
├── baseline date
├── mandatory regulatory profile
├── graphic standard profile
├── equipment/manufacturer profiles
├── enterprise profile
├── site profile
└── project overlay
```

Resolved rules/source versions, использованные для release, должны быть snapshot-able/auditable.

## 15. Module extensions

Vendor/module-specific information, которое должно пережить round-trip, но не имеет neutral meaning, хранится в namespaced module extensions, например `extensions.npt`.

Extension не становится required для interpretation neutral topology без отдельного ADR/model change.

## 16. Undo/redo и transactions

Engineering changes представлены как transactions/change sets:

- add equipment;
- reconnect terminal;
- apply approved `ImportPlan`;
- change property;
- change layout constraint;
- apply simulated switching operation к explicit `SIMULATED` context.

Observed/imported baseline state, simulated state и planned/target state остаются разделёнными на model/API boundary, а не только визуально в UI.

## 17. Native project format requirements

До выбора serialization implementation должен доказать:

- schema versioning;
- deterministic round-trip;
- migrations;
- stable identity;
- atomic write/recovery;
- module-extension preservation;
- diffability/diagnostics;
- отсутствие hidden dependency на GUI-framework object serialization.

## 18. Non-goals initial Core

Не включать заранее:

- full CIM ontology;
- every protection/automation data class;
- realtime SCADA channel engine;
- complete manufacturer database;
- universal calculation model.

Core расширяется только через реальные product workflows.