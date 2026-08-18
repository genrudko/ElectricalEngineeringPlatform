# Migration Plan — Three Directions to One Product

Статус: canonical foundation document

## 1. Purpose

Migration does **not** mean copying three repositories/codebases into one tree.

It means extracting proven knowledge/behavior/data contracts from each previous direction, assigning a new module owner, adding missing tests/provenance, and retiring duplicate state models only after equivalent behavior is accepted.

## 2. Migration classification

For every significant asset classify:

```text
RETAIN_AS_EVIDENCE
SALVAGE_BEHIND_NEW_CONTRACT
REIMPLEMENT_FROM_BEHAVIOR
MIGRATE_DATA_ONLY
ARCHIVE_HISTORICAL
RETIRE_AFTER_ACCEPTANCE
```

Every salvaged/migrated asset records source/path/commit where possible, current responsibility, known limitations, new owner module, required tests/evidence, dependency/licensing concerns and retirement condition.

## 3. ElectroScheme Studio migration

Source repository: `genrudko/electroscheme-studio`.

Likely retain as evidence:

- VSDX/VSSX inspection/ShapeSheet research;
- market/reference-product research;
- prototype quarantine/disposition method;
- snapping/grid/busbar experiments;
- platform/packaging measurements;
- Tauri spike implementation and failure evidence;
- Visio controlled fixtures/round-trip knowledge.

Likely reimplement from new contract:

- application shell;
- old CSS/design/ribbon/property composition;
- large canvas ownership/state composition;
- duplicated frontend/backend document state;
- product-level command/mutation path if it conflicts with new Domain Core.

Pending after Platform Spike:

- Vue/TypeScript/SVG code reuse;
- Rust/Tauri native adapters;
- current web build pipeline.

No Tauri/WebView asset is promoted merely because historical CI was green.

## 4. NPT Engineering Toolkit migration

NPT research is a major compatibility/domain corpus and should be preserved carefully.

### Proven concepts to carry forward

- lossless/preserve-first XML handling;
- XSDE document/object inventory and semantics;
- embedded CustElem vs external `.menu` distinction;
- typed custom-value semantics (`scdState`, `scdCommand`, `scdValue`, `scdColor`, etc.);
- ASU/TECH/KKS signal catalog model/search;
- XTABL v6.0 lossless records and proven field meanings;
- preserve-only handling for unknown XTABL fields;
- native resource/path alias handling;
- explicit safe/unsafe creation boundaries.

### Must remain NPT-specific

- `sTag` allocation/counters;
- `RTID`/`TechData` registry semantics;
- `CustElem` embedding/storage;
- `scd*` serialization;
- XSDE XML order/comments/unknown fields;
- XTABL raw-record specifics.

These live in NPT adapters/storage and translate to/from neutral domain entities where justified.

### Critical unresolved experiment

NPT `nodes` are topology-related but are not yet proven to reconstruct a complete neutral electrical graph.

Required evidence:

1. select one small known real cell/mnemonic;
2. extract object/Tech/node relations;
3. map to neutral terminals/connections;
4. render/graph topology independently;
5. manually compare with visible one-line diagram;
6. test energized/de-energized propagation under known switching states;
7. record unmapped/ambiguous relationships.

Until this passes, do not make NPT topology import a Core invariant.

### Renderer status

Existing Mnemo renderer is not accepted for fidelity. Compare known mnemonic(s) side-by-side with native NPT/Modus before broad editor expansion.

## 5. TBP / switching migration

At Foundation time, TBP source is treated as a migration source; do not invent a canonical GitHub repository if the authoritative source remains local.

Valuable concepts:

- draft-generation workflow;
- mandatory human review boundary;
- profiles/local customization;
- operational wording/patterns;
- normative reference data;
- rule-authority hierarchy;
- real project tests/examples.

Existing code/YAML behavior is **not normative authority by itself**.

Each migrated rule must be reclassified as:

```text
normative mandatory
normative conditional
enterprise/site policy
manufacturer/equipment constraint
formatting/wording convention
heuristic/recommendation
```

and linked to source/version/applicability metadata.

TBP must consume shared equipment identity, terminals/connections/topology, state and compliance rules rather than recreate a second project database.

## 6. Data migration philosophy

Old data may be imported through explicit adapters/migrations, but native project format is not required to mimic any old format.

Where round-trip compatibility is required, preserve vendor-specific payload in adapter-owned structures/extension blocks rather than flattening unknown information into neutral Core.

## 7. Repository strategy

During Foundation and early spikes:

- keep this new repository clean;
- do not bulk-vendor old product trees;
- do not commit NPT vendor binaries/full corpus;
- do not assume local TBP source has a GitHub authority until explicitly imported;
- create production source tree only after platform stack selection;
- record asset disposition before retirement.

## 8. Migration gates

An old subsystem can be retired only if:

1. target module/owner exists;
2. required behavior/data has a mapping decision;
3. representative tests pass;
4. required provenance/unknown fields are not lost;
5. owner accepts replacement workflow;
6. rollback/reference material remains available where appropriate.

## 9. Anti-goal

Do not optimize migration for preserving old implementation effort. Preserve **validated knowledge, user value, format semantics and evidence**, while discarding architecture that reproduces old limitations.
