# Legacy Source Index

Статус: canonical migration/reference index

## 1. Purpose

This repository is the new product authority. Existing repositories/corpora are used only where they provide concrete validated evidence that is cheaper and safer to reuse than rebuilding from the new contracts.

## 2. ElectroScheme Studio

Canonical historical source:

```text
genrudko/electroscheme-studio
```

Important areas to retain as reference/migration evidence:

- Visio/VSDX/VSSX interoperability research;
- ShapeSheet inspection/conversion tooling;
- prototype/editor interaction research;
- snapping/grid/busbar/symbol experiments;
- market/reference-product research;
- Tauri/WebView desktop spike and packaging/native-integration evidence;
- old UI behavior/examples useful for comparison.

Do **not** copy the whole repository tree into this repo during Foundation.

When a component is considered for reuse, copy/reimplement only the accepted asset behind the new module contract and record the original commit/path in the migration PR.

## 3. NPT / Modus reference corpus

Current known source archives/materials:

```text
scada-npt-expert.zip
LW.zip / 000. LW.zip
```

These contain vendor/deployed-system materials including NPT/Modus binaries, project files, XSDE/XTABL corpora, libraries and database dumps.

### Storage rule

**Do not commit these archives or extracted full vendor trees to GitHub**, even if repository visibility is private today.

Preferred controlled location:

```text
/opt/electrical-engineering-platform/corpus/npt-reference/
```

Suggested contents:

```text
npt-reference/
├── scada-npt-expert/
├── lw/
├── manifests/
│   ├── source_sha256.txt
│   ├── corpus_inventory.json
│   └── provenance.md
└── derived/
    ├── sanitized-fixtures/
    └── analysis-reports/
```

Only project-created sanitized/minimal fixtures and derived non-proprietary test data may be promoted into this Git repository after explicit review.

## 4. Old TBP project

Owner disposition: **DO_NOT_MIGRATE**.

The old TBP codebase/configuration/rule implementation is not an input dependency for the new `Modules.Switching` and does not need to be uploaded, archived into this repository or inventoried before development.

The new Switching module is written clean-sheet against:

- Domain Core;
- Compliance Core;
- current verified normative sources;
- synthetic/cleared switching scenarios;
- future local-policy configuration through the new overlay mechanism.

The old TBP project may remain in its existing local archive only as historical context. It is not a normative source or test oracle.

## 5. EOD reference

Canonical existing repository:

```text
genrudko/electronic-operational-docs
```

Relevant accepted implementation evidence:

- `MODULE-ACTIVATION-CONTRACT-001` / merged PR #62;
- `MODULE-REGISTRY-001` / merged PR #68;
- scoped activation over `ORGANIZATION`, `ENERGY_SITE`, `WORKPLACE`;
- optional integration semantics;
- lifecycle/audit/access-decision patterns.

Do not copy EOD code wholesale. A future electrical bridge module is developed through a separate bounded EOD integration work item.

## 6. Public normative sources

Government/sector rules and applicable standards should be discovered and re-verified from current online authoritative sources when the relevant compliance slice is implemented.

Preferred authority:

```text
official publication / official issuer
→ official Rosstandart standards catalogue
→ authoritative consolidated legal/reference cross-check where needed
```

Repository stores:

- source metadata;
- effective/amendment information;
- rule IDs/provenance references;
- project-authored machine requirements;
- tests/coverage matrices.

Full source texts are stored/copied only when access and redistribution are permitted and when doing so provides practical value. A random internet copy is never promoted to normative authority merely because it is downloadable.

## 7. Enterprise/site internal instructions

Internal enterprise/site instructions are **not part of the shared development corpus and should not be uploaded to this repository or normal project staging**.

The product supports them as an optional deployment-time Local Policy Overlay capability. Development of that capability uses synthetic/cleared examples.

At a real deployment, internal instructions remain within the authorized enterprise environment. The project needs only the formalized local policy package and source metadata required by that deployment's governance; the shared GitHub development process does not need the original internal documents.

## 8. Manufacturer documentation

Manufacturer manuals/technical requirements are handled case-by-case. Use public manufacturer sources where available; copyrighted/restricted manuals stay outside Git unless redistribution is clearly allowed.

Machine constraints need explicit model/revision/applicability provenance.

## 9. What may be committed to this repository

Allowed/expected:

- our source code;
- our architecture/docs/ADRs;
- synthetic or cleared CSV/XLSX import fixtures;
- sanitized/minimal XSDE/XTABL fixtures when legally/operationally cleared;
- project-authored native symbol assets with provenance;
- derived inventories/catalog schemas without proprietary payload;
- benchmark generators/datasets created by the project;
- rule metadata and machine tests;
- synthetic local-policy fixtures.

## 10. What must stay out of Git by default

- complete NPT installation/deployed backup;
- Modus/NPT executables/DLLs/vendor libraries unless explicit redistribution right exists;
- full station project corpus;
- raw LW SQL dumps/databases containing operational/proprietary data;
- internal enterprise/site instructions;
- manufacturer manuals with redistribution restrictions;
- secrets/certificates/credentials;
- private production/exported EOD data;
- old TBP source tree unless the owner later reopens a specific narrow migration need.