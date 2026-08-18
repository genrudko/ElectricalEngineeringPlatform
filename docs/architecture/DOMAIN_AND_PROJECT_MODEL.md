# Domain and Project Model

Статус: canonical foundation document

## 1. Source of truth

The canonical engineering authority is `ElectricalProject`.

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

Exact classes/schema remain PENDING until implementation spike. This document defines semantics/invariants, not frozen serialization syntax.

## 2. Identity

All canonical engineering entities use stable internal IDs independent from display names/KKS/vendor IDs.

Rules:

- display name changes do not change identity;
- external IDs are mapped/provenance data, not canonical identity;
- duplicate external IDs are diagnosable;
- stable IDs survive layout/view changes.

## 3. Equipment

`Equipment` represents a semantic device/object, not its picture.

Core attributes are intentionally small: stable ID, equipment type reference, names/designations, object/site/voltage context, typed properties, terminal set, state reference/current simulated state where applicable, and source/provenance references.

Equipment type definitions live in Equipment Library rather than a giant inheritance hierarchy.

Typical types may include circuit breaker, disconnector, earthing switch, bus/bus section, transformer/autotransformer, line/cable, generator, load and measurement transformers as real workflows justify.

## 4. Equipment Library

A type definition may declare semantic type/category, terminal schema, supported properties, supported states, validation constraints, graphic representation bindings and optional manufacturer/model profiles.

Manufacturer/model profile may add stricter ratings/constraints without redefining the core equipment category.

## 5. Terminals

A terminal is the semantic connection point of equipment.

Coordinates are not terminal identity. A symbol exposes visual anchors mapped to semantic terminals; moving the symbol does not recreate terminals.

## 6. Connections

A connection represents an electrical/network relationship between semantic endpoints and does not own screen polyline geometry.

Endpoints must exist; terminal/type/cardinality rules are validated; connection changes are explicit transactions; the same connection may be rendered differently in multiple views.

## 7. Topology Graph

Expected capabilities:

- connectivity queries;
- path search;
- electrical islands/sections;
- state-dependent continuity;
- energized/de-energized/unknown propagation with explicit assumptions;
- switching impact calculation;
- diagnostics for impossible/dangling structures.

Important distinction:

```text
NO_PATH
PATH_OPEN_BY_KNOWN_STATE
PATH_CLOSED
PATH_STATUS_UNKNOWN
```

Unknown data must not become an optimistic safe state.

## 8. State

State consists of typed values with knowledge/quality semantics.

```text
CircuitBreakerPosition:
  OPEN | CLOSED | INTERMEDIATE | UNKNOWN

Energization:
  ENERGIZED | DEENERGIZED_PROVEN | UNKNOWN

SignalQuality:
  GOOD | BAD | UNCERTAIN | UNKNOWN
```

`DEENERGIZED_PROVEN` is deliberately different from absence of a known energized path under incomplete data.

## 9. Signals

A signal is a domain binding/reference, not necessarily an online runtime channel. It may carry source/system, KKS/other designation, type/unit, relation to equipment/property/state, quality semantics and provenance.

NPT ASU/TECH implementation details remain in the NPT adapter/catalog.

## 10. Views

A `View` is a controlled representation over project entities.

Possible views include general single-line, voltage-level, detailed bay/switchgear, operational, NPT-compatible mnemonic, tabular and switching simulation views.

A view stores entity inclusion/filters, representation profile, geometry/layout, labels/annotations, layer/group settings and layout constraints, but not duplicate authoritative topology.

## 11. Layout constraints

Manual correction after auto-layout is first-class data: pinned position, relative ordering, orientation, bus extent/orientation, group/bay placement, user-owned edge route/bends, protected label placement and layout regions.

Auto-layout must preserve protected constraints during incremental updates.

## 12. Provenance

Imported/generated entities retain enough provenance to explain origin and reconcile updates:

```text
SourceRef
├── source type
├── source identity/file fingerprint
├── external row/object ID
├── mapping profile version
├── import revision
└── transformation notes/confidence
```

Provenance is not canonical identity.

## 13. Compliance profile references

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

Resolved rules/source versions used for a release should be snapshot-able/auditable.

## 14. Module extensions

Vendor/module-specific information that must survive round-trip but has no neutral meaning belongs in namespaced module extensions such as `extensions.npt`.

Extensions must not become required to interpret neutral topology unless promoted through an ADR/model change.

## 15. Undo/redo and transactions

Engineering changes are represented as transactions/change sets: add equipment, reconnect terminal, apply approved import plan, change property, change layout constraint, apply simulated switching operation.

Simulation state and persisted observed/current state must remain clearly separated.

## 16. Native project format requirements

Before selecting serialization, implementation must prove schema versioning, deterministic round-trip, migrations, stable identity, atomic write/recovery, module-extension preservation, diffability/diagnostics and no hidden dependency on GUI-framework object serialization.

## 17. Non-goals of initial Core

Do not include full CIM ontology, every protection/automation data class, realtime SCADA channel engine, complete manufacturer database or universal calculation model before product workflows require them.
