# Import, Reconciliation и Auto Layout

Статус: канонический Foundation-документ

## 1. Роль в продукте

Structured import — first-class module и один из центральных product differentiators.

Цель — не просто `CSV → objects on canvas`.

```text
structured source
→ normalized engineering candidate
→ validated topology
→ canonical ElectricalProject
→ domain-aware diagram proposal
→ engineer corrections
→ safe re-import/update
```

## 2. Первые source formats

Initial target:

- CSV;
- XLSX.

Importer не должен навязывать один rigid corporate template.

Пользователь сопоставляет external columns с canonical semantics и сохраняет `MappingProfile`.

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
MappingProfile
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

Каждый stage должен быть независимо diagnosable/testable.

## 4. MappingProfile

Mapping profile хранит:

- profile ID/version/name;
- accepted source sheet/table patterns;
- column mappings;
- transforms/normalizers;
- units;
- equipment-type mapping;
- identifier strategy;
- connection-reference syntax;
- required/optional columns;
- defaults with provenance;
- optional organization/site scope where justified.

Mapping profile — configuration, а не arbitrary executable code из spreadsheet.

## 5. Confidence и ambiguity

Минимальные resolution classes:

```text
RESOLVED
REQUIRES_CONFIRMATION
CONFLICT
INVALID
```

Если source connection ссылается на equipment с несколькими возможными terminals и deterministic rule не даёт однозначный результат, система показывает candidates/reason и требует решения пользователя, а не выбирает первый terminal молча.

## 6. Staging model

`ImportCandidate` отделён от canonical project.

Staging review показывает:

- new/matched equipment;
- changed properties;
- new/changed/removed connections;
- unresolved references;
- duplicate IDs;
- type conflicts;
- voltage/domain conflicts;
- useful raw-source diagnostics.

До approval `ImportPlan` canonical mutation не выполняется.

## 7. Reconciliation

Repeat import — primary scenario.

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

Potential removal должен различать:

- source-authoritative deletion;
- source omission/filter;
- project-owned entity;
- ambiguous disappearance.

Automatic deletion запрещён, пока source authority/profile и user approval явно его не разрешают.

## 8. Provenance

По возможности сохраняются:

- source file/content fingerprint;
- sheet/table;
- row/external ID;
- mapping profile/version;
- raw value, значимый для reconciliation;
- normalized value;
- transform/rule used;
- import revision.

## 9. Topology construction

Validation включает:

- endpoint existence;
- terminal compatibility;
- duplicate connection;
- impossible loops where forbidden;
- voltage-level mismatch;
- terminal cardinality;
- dangling network fragments;
- unsupported equipment;
- site/object consistency.

Exact electrical rules растут через Compliance/Equipment Library profiles.

## 10. Auto-layout не является topology

```text
TopologyGraph + ViewProfile + ExistingConstraints
        ↓
LayoutProposal
```

`LayoutProposal` можно отклонить/исправить без изменения topology.

## 11. Domain-aware layout

Initial deterministic strategies могут учитывать:

- voltage levels;
- busbars/sections;
- bay/feeder grouping;
- transformers как level boundaries;
- switching-equipment sequence along a path;
- earthing switches;
- lines/generators/loads;
- orientation preferences;
- standard label regions.

Generic force-directed layout не используется как primary electrical strategy.

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

Impossible constraint sets дают diagnostics, а не corrupted geometry.

## 13. Incremental layout

Приоритеты:

1. preserve user-owned/locked constraints;
2. preserve unchanged local neighborhoods where possible;
3. place new/changed entities near semantic neighbors;
4. reroute only impacted routes where feasible;
5. expose layout conflicts for review.

Global full re-layout — explicit command, а не default update behavior.

## 14. Layout profiles

Possible future profiles:

- substation single-line;
- switchgear/bay;
- wind farm collector system;
- auxiliary power;
- free/legacy imported view.

Profile вводится только после repeated real examples.

## 15. Acceptance первого vertical slice

1. открыть CSV/XLSX;
2. сопоставить поля или выбрать profile;
3. создать Equipment/Terminals/Connections;
4. показать staging summary;
5. разрешить deliberately ambiguous case;
6. применить `ImportPlan`;
7. render auto-generated one-line view;
8. вручную move/lock/re-route выбранные элементы;
9. save/reopen native project;
10. импортировать modified source;
11. показать reconciliation diff;
12. применить update;
13. доказать сохранение manual layout constraints;
14. validate topology before/after.

## 16. Failure modes, которые нужно исключить

- source rows становятся native project storage;
- importer мутирует canonical project во время parsing;
- ambiguous terminals выбираются молча;
- entity matching выполняется только по display name;
- каждый re-import уничтожает layout;
- layout engine меняет topology ради внешнего вида;
- manual edits нельзя отличить от generated geometry;
- каждый corporate spreadsheet требует one-off hard-coded parser, хотя достаточно MappingProfile.

## 17. Future source adapters

Те же staging/reconciliation contracts позже могут поддержать NPT project import, controlled Visio extraction, CIM/exchange formats и corporate databases/APIs.