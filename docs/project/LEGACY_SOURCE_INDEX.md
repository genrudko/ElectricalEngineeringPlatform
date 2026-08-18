# Индекс legacy-источников

Статус: канонический migration/reference index

## 1. Назначение

Новый repository является authority продукта.

Existing repositories/corpora используются только там, где они дают конкретное validated evidence, которое дешевле и безопаснее переиспользовать, чем заново исследовать ту же проблему.

## 2. ElectroScheme Studio

Canonical historical source:

```text
genrudko/electroscheme-studio
```

Важные области для reference/migration evidence:

- Visio/VSDX/VSSX interoperability research;
- ShapeSheet inspection/conversion tooling;
- prototype/editor interaction research;
- snapping/grid/busbar/symbol experiments;
- market/reference-product research;
- Tauri/WebView desktop spike и packaging/native-integration evidence;
- old UI behavior/examples для comparison.

Не копировать весь repository tree в новый repo.

Если компонент рассматривается для reuse, переносить или реализовывать заново только accepted asset за новым module contract и фиксировать original commit/path в migration PR.

## 3. NPT / Modus reference corpus

Known source archives/materials:

```text
scada-npt-expert.zip
LW.zip / 000. LW.zip
```

Они содержат vendor/deployed-system materials, включая NPT/Modus binaries, project files, XSDE/XTABL corpora, libraries и database dumps.

### 3.1. Storage rule

**Не commit эти archives или extracted full vendor trees в GitHub**, даже если repository сегодня private.

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

В Git могут попасть только project-created sanitized/minimal fixtures и derived non-proprietary test data после explicit review.

## 4. Old TBP project

Owner disposition: **`DO_NOT_MIGRATE`**.

Old TBP codebase/configuration/rule implementation не является input dependency для нового `Modules.Switching` и не требуется для старта его разработки.

Новый Switching module реализуется clean-sheet на основе:

- Domain Core;
- Compliance Core;
- current verified normative sources;
- synthetic/cleared switching scenarios;
- future local-policy configuration через новый overlay mechanism.

Old TBP может оставаться в существующем локальном архиве только как historical context. Он не является normative source или test oracle.

## 5. EOD reference

Canonical existing repository:

```text
genrudko/electronic-operational-docs
```

Relevant accepted implementation evidence:

- `MODULE-ACTIVATION-CONTRACT-001` / merged PR #62;
- `MODULE-REGISTRY-001` / merged PR #68;
- scoped activation для `ORGANIZATION`, `ENERGY_SITE`, `WORKPLACE`;
- optional integration semantics;
- lifecycle/audit/access-decision patterns.

Не копировать EOD code wholesale. Future electrical bridge module разрабатывается через отдельный bounded EOD integration work item.

## 6. Public normative sources

Government/sector rules и applicable standards должны обнаруживаться и re-verify по current online authoritative sources при реализации соответствующего compliance slice.

Приоритет authority:

```text
official publication / official issuer
→ official Rosstandart standards catalogue
→ authoritative consolidated legal/reference cross-check when needed
```

Repository хранит:

- source metadata;
- effective/amendment information;
- rule IDs/provenance references;
- project-authored machine requirements;
- tests/coverage matrices.

Full source texts копируются/хранятся только при разрешённом доступе/redistribution и реальной практической необходимости. Random internet copy не становится normative authority только потому, что она доступна для скачивания.

## 7. Enterprise/site internal instructions

Internal enterprise/site instructions **не входят в shared development corpus и не должны загружаться в этот repository или normal project staging**.

Продукт поддерживает их как optional deployment-time Local Policy Overlay capability. Development механизма использует synthetic/cleared examples.

При реальном deployment internal instructions остаются внутри authorized enterprise environment.

## 8. Manufacturer documentation

Manufacturer manuals/technical requirements рассматриваются case-by-case.

Использовать public manufacturer sources, когда доступны. Copyrighted/restricted manuals остаются вне Git без clear redistribution permission.

Machine constraints требуют explicit model/revision/applicability provenance.

## 9. Что можно commit в repository

Allowed/expected:

- наш source code;
- architecture/docs/ADRs;
- synthetic или cleared CSV/XLSX import fixtures;
- sanitized/minimal XSDE/XTABL fixtures;
- project-authored native symbol assets с provenance;
- derived inventories/catalog schemas без proprietary payload;
- benchmark generators/datasets проекта;
- rule metadata и machine tests;
- synthetic local-policy fixtures.

## 10. Что по умолчанию остаётся вне Git

- complete NPT installation/deployed backup;
- Modus/NPT executables/DLLs/vendor libraries без explicit redistribution right;
- full station project corpus;
- raw LW SQL dumps/databases с operational/proprietary data;
- internal enterprise/site instructions;
- manufacturer manuals с redistribution restrictions;
- secrets/certificates/credentials;
- private production/exported EOD data;
- old TBP source tree, пока владелец явно не переоткроет конкретную narrow migration need.