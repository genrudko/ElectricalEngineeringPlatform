# Migration Plan — Legacy Sources into One Product

Статус: canonical foundation document

## 1. Purpose

Migration does **not** mean copying three old codebases into one tree.

The new product has three different legacy relationships:

1. **ElectroScheme Studio** — research/prototype source from which selected evidence/tools may be reused after review;
2. **NPT Engineering Toolkit / NPT materials** — compatibility/research source whose proven format knowledge must be preserved behind the NPT boundary;
3. **old TBP project** — **not a code/config migration source for the new Switching module**. The Switching/TBP module is designed and implemented from scratch against the new Domain Core, Compliance Core and current verified normative sources.

The objective is to preserve proven knowledge only where it is actually useful, without dragging obsolete architecture into the new product.

## 2. Migration classification

For every significant legacy asset that is considered for reuse classify:

```text
RETAIN_AS_EVIDENCE
SALVAGE_BEHIND_NEW_CONTRACT
REIMPLEMENT_FROM_BEHAVIOR
MIGRATE_DATA_ONLY
ARCHIVE_HISTORICAL
DO_NOT_MIGRATE
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

## 5. Switching / TBP — clean-sheet implementation

Owner decision: **do not migrate the old TBP codebase, YAML/configuration or rule implementation into the new product.**

The new `Modules.Switching` is written from scratch using:

- the neutral `ElectricalProject` model;
- shared equipment/terminal/connection/topology/state semantics;
- the new Compliance Core;
- current verified Russian normative sources;
- synthetic/cleared engineering scenarios;
- later user/site configuration through the Local Policy Overlay mechanism.

The old TBP project may remain archived outside the new repository as historical context only. It is not a dependency, test oracle, normative authority or required migration input.

### What may be retained conceptually

Only product-level lessons already explicitly accepted in the new Foundation may survive, for example:

- switching-form generation is decision support, not real-equipment control;
- qualified human review remains required;
- rules require provenance/applicability/versioning;
- object/site-specific stricter policy is a supported capability.

These principles are redefined in current canonical documents and do **not** require copying old TBP implementation.

## 6. Normative source strategy

Federal/sector normative acts and standards are obtained from current online authoritative sources during rule/profile development.

Priority:

```text
official publication / official issuer
→ official standards catalogue
→ authoritative legal/reference cross-check where needed
```

Do not use the old TBP repository as a source of current normative truth.

For ГОСТ, official status/metadata must come from Rosstandart/catalogue sources. Full-text access/redistribution is handled only through legally permitted sources; random internet copies are not promoted to normative authority.

## 7. Enterprise/site instructions

Internal enterprise/site instructions are **not required development inputs and should not be uploaded into this project/repository by default**.

The platform must support such local policy in deployment, but Foundation/development tests use synthetic or deliberately cleared overlay examples.

At a real enterprise/site, authorized administrators may configure local policy packages from internal documentation inside that controlled environment without publishing the source documents to GitHub or the shared development corpus.

## 8. Data migration philosophy

Old data may be imported through explicit adapters/migrations only where a real product workflow requires it.

Where round-trip compatibility is required, especially NPT, preserve vendor-specific payload in adapter-owned structures/extension blocks rather than flattening unknown information into neutral Core.

## 9. Repository strategy

During Foundation and early spikes:

- keep this repository clean;
- do not bulk-vendor old product trees;
- do not import the old TBP project;
- do not commit NPT vendor binaries/full corpus;
- do not collect internal enterprise/site instructions as development assets;
- fetch/re-verify public normative sources when rule/profile work begins;
- create production source tree only after platform stack selection;
- record asset disposition before any reuse.

## 10. Migration gates

A legacy asset is reused only if:

1. a concrete target module/use case exists;
2. reuse is cheaper/safer than clean reimplementation;
3. behavior/data semantics are understood;
4. representative tests can prove it;
5. required provenance/unknown fields are not lost;
6. owner accepts the reuse decision.

For the old TBP project, the current disposition is explicitly `DO_NOT_MIGRATE` unless the owner later reopens that decision for a narrowly identified artifact.

## 11. Anti-goal

Do not optimize for preserving old implementation effort.

Preserve **validated knowledge, compatibility semantics and useful research evidence** where they materially reduce risk. Rebuild product logic cleanly where the old implementation does not fit the new architecture.