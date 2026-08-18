# Import, Reconciliation and Auto Layout

Статус: canonical foundation document

## 1. Product role

Structured import is a first-class module and one of the central product differentiators.

The objective is not merely `CSV → objects on canvas`.

```text
structured source
→ normalized engineering candidate
→ validated topology
→ canonical ElectricalProject
→ domain-aware diagram proposal
→ engineer corrections
→ safe re-import/update
```

## 2. Supported first sources

Initial target:

- CSV;
- XLSX.

The importer must not force one rigid corporate template as the only accepted source.

Users map external columns to canonical semantics and save mapping profiles.

```text
"Поз."          → equipment.external_id
"Тип"           → equipment.type
"Наименование"  → equipment.name
"Откуда"        → connection.from
"Куда"          → connection.to
"U, кВ"         → equipment.rated_voltage
```

## 3. Import pipeline

```text
Source file
  ↓
Source reader
  ↓
Mapping profile
  ↓
Normalization
  ↓
ImportCandidate
  ↓
Semantic/type validation
  ↓
Connection/terminal resolution
  ↓
Staging review
  ↓
Reconciliation against project
  ↓
Approved ImportPlan
  ↓ atomic transaction
ElectricalProject
```

Each stage must be independently diagnosable/testable.

## 4. Mapping profiles

A mapping profile records profile ID/version/name, accepted source sheet/table patterns, column mappings, transforms/normalizers, units, equipment-type mapping, identifier strategy, connection-reference syntax, required/optional columns, defaults with provenance and optional organization/site scope.

Mapping profile is configuration, not arbitrary executable code from the spreadsheet.

## 5. Confidence and ambiguity

Minimum resolution classes:

```text
RESOLVED
REQUIRES_CONFIRMATION
CONFLICT
INVALID
```

If a source connection points to equipment with several possible terminals and no deterministic rule resolves it, the correct behavior is to present candidates/reason — not silently choose the first terminal.

## 6. Staging model

`ImportCandidate` is separate from canonical project.

Staging review summarizes new/matched equipment, changed properties, new/changed/removed connections, unresolved references, duplicate IDs, type conflicts, voltage/domain conflicts and useful raw-source diagnostics.

No canonical mutation happens until ImportPlan is approved.

## 7. Reconciliation

Repeat import is a primary scenario.

Possible results:

```text
UNCHANGED
ADD
UPDATE
REMOVE_CANDIDATE
RELINK
CONFLICT
MANUAL_MATCH_REQUIRED
```

Potential removal must distinguish source-authoritative deletion, source omission/filter, project-owned entity and ambiguous disappearance.

Do not delete automatically unless source authority/profile and user approval permit it.

## 8. Provenance

Retain where practical:

- source file/content fingerprint;
- sheet/table;
- row/external ID;
- mapping profile/version;
- raw value relevant to reconciliation;
- normalized value;
- transform/rule used;
- import revision.

## 9. Topology construction

Validation includes endpoint existence, terminal compatibility, duplicate connection, impossible loops where forbidden, voltage-level mismatch, terminal cardinality, dangling network fragments, unsupported equipment and site/object consistency.

Exact electrical rules grow through Compliance/Equipment Library profiles.

## 10. Auto-layout is not topology

```text
TopologyGraph + ViewProfile + ExistingConstraints
        ↓
LayoutProposal
```

The proposal may be rejected/edited without changing topology.

## 11. Domain-aware layout

Initial deterministic strategies may understand voltage levels, buses/sections, bay/feeder grouping, transformers as level boundaries, switching-equipment sequence along a path, earthing switches, lines/generators/loads, orientation preferences and standard label regions.

Avoid generic force-directed layout as the primary electrical strategy.

## 12. Layout constraints

Examples:

```text
PinnedPosition
FixedOrientation
RelativeOrder
GroupRegion
BusOrientation
UserOwnedRoute
ProtectedLabelPosition
KeepTogether
MinimumSpacingOverride
```

Impossible constraint sets produce diagnostics rather than corrupted geometry.

## 13. Incremental layout

Priority:

1. preserve user-owned/locked constraints;
2. preserve unchanged local neighborhoods where possible;
3. place new/changed entities near semantic neighbors;
4. reroute only impacted routes where feasible;
5. expose layout conflicts for review.

Global full re-layout is an explicit command, not default update behavior.

## 14. Layout profiles

Possible future profiles:

- substation single-line;
- switchgear/bay;
- wind farm collector system;
- auxiliary power;
- free/legacy imported view.

Introduce only after repeated real examples justify them.

## 15. First vertical-slice acceptance

1. open CSV/XLSX;
2. map fields or select profile;
3. create equipment/terminals/connections;
4. show staging summary;
5. resolve deliberately ambiguous case;
6. apply ImportPlan;
7. render auto-generated one-line view;
8. manually move/lock/re-route selected elements;
9. save/reopen native project;
10. import modified source;
11. show reconciliation diff;
12. apply update;
13. prove manual layout constraints survive;
14. validate topology before/after.

## 16. Failure modes to prevent

- source rows become native project storage;
- importer mutates canonical project while parsing;
- ambiguous terminals chosen silently;
- entity matching by display name only;
- every re-import destroys layout;
- layout engine changes topology to make drawing prettier;
- manual edits cannot be distinguished from generated geometry;
- every corporate spreadsheet requires a one-off hard-coded parser when mapping profile is sufficient.

## 17. Future source adapters

The same staging/reconciliation contracts can later support NPT project import, controlled Visio extraction, CIM/exchange formats and corporate databases/APIs.
