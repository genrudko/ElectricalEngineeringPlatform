# Legacy Source Index

Статус: canonical migration/reference index

## 1. Purpose

This repository is the new product authority, but several existing repositories/local corpora contain valuable engineering evidence. This document records where that evidence lives and whether it belongs in Git.

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

When a component is migrated, copy/reimplement only the accepted asset behind the new module contract and record the original commit/path in the migration PR.

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
VPS private corpus root
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

## 4. TBP / switching source

Current authoritative source was historically maintained locally (for example under a local `TBP_project_py` working tree in prior work). Do not invent a GitHub source-of-truth if it is not actually established.

Recommended migration approach:

1. archive the local source exactly once with commit/history metadata if Git history exists;
2. inventory code/config/tests/rule sources;
3. copy only project-owned material needed for migration into controlled staging;
4. move accepted rule/domain behavior into `Modules.Switching` only after provenance classification;
5. keep source normative documents/instructions outside public Git unless redistribution rights are clear.

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

Do not copy EOD code wholesale. A future electrical bridge module should be developed through a separate bounded EOD integration work item.

## 6. Normative source corpus

Production rule extraction may require controlled copies/links of:

- official legal/normative acts;
- official ГОСТ/ЕСКД texts available under permitted access;
- enterprise/site instructions;
- manufacturer manuals;
- approved operating/switching instructions.

Keep confidential/licensed source documents outside this Git repository unless redistribution is clearly allowed.

Repository stores:

- source metadata;
- rule IDs/provenance references;
- paraphrased machine requirements;
- tests/coverage matrices;
- synthetic/cleared examples.

## 7. What may be committed to this repository

Allowed/expected:

- our source code;
- our architecture/docs/ADRs;
- synthetic or cleared CSV/XLSX import fixtures;
- sanitized/minimal XSDE/XTABL fixtures when legally/operationally cleared;
- project-authored native symbol assets with provenance;
- derived inventories/catalog schemas without proprietary payload;
- benchmark generators/datasets created by the project;
- rule metadata and machine tests, not unauthorized full source documents.

## 8. What must stay out of Git by default

- complete NPT installation/deployed backup;
- Modus/NPT executables/DLLs/vendor libraries unless explicit redistribution right exists;
- full station project corpus;
- raw LW SQL dumps/databases containing operational/proprietary data;
- confidential enterprise/site instructions;
- manufacturer manuals with redistribution restrictions;
- secrets/certificates/credentials;
- private production/exported EOD data.
