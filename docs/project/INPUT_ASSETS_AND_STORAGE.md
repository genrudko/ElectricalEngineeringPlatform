# Входные материалы и правила хранения

Статус: каноническое Foundation-guidance

## 1. Принцип

Не каждый полезный исходный файл должен попадать в GitHub, и не каждый legacy/internal source вообще нужно собирать.

Product repository хранит project-owned source, documentation, tests и cleared fixtures.

Вне Git остаются:

- vendor distributions;
- full industrial corpora;
- secrets/credentials;
- internal enterprise documentation;
- restricted source materials без права redistribution.

## 2. Уже доступно — ручная загрузка не нужна

### 2.1. ElectroScheme Studio

Source: `genrudko/electroscheme-studio`.

Не копировать repository целиком. Historical research читается напрямую; в новый продукт переносится или реализуется заново только конкретный accepted asset за новым contract.

### 2.2. EOD

Source: `genrudko/electronic-operational-docs`.

Не копировать EOD code. Future integration использует отдельный bounded bridge work item.

## 3. Old TBP project — не нужен

Owner decision: **не использовать старый TBP project как migration source**.

Не требуется передавать:

- `TBP_project_py`;
- его Git history;
- YAML profiles;
- old rules;
- tests/configuration.

Новый Switching/TBP module пишется с нуля против Domain Core + Compliance Core + current normative sources.

Если позже конкретный historical artifact окажется полезен, он рассматривается отдельно без изменения clean-sheet baseline.

## 4. NPT / Modus raw corpus — только private reference

Known inputs:

```text
scada-npt-expert.zip
LW.zip / 000. LW.zip
```

Они содержат vendor/deployed-system binaries, libraries, project corpora и database dumps.

Предпочтительное controlled VPS location:

```text
/opt/electrical-engineering-platform/corpus/npt-reference/
├── scada-npt-expert/
├── lw/
├── manifests/
│   ├── source_sha256.txt
│   ├── corpus_inventory.json
│   └── provenance.md
└── derived/
```

Не commit:

- raw archives;
- vendor EXE/DLL/OCX libraries;
- full station project tree;
- raw SQL/database dumps;
- large generated SQLite catalogues.

## 5. NPT-derived assets, которые могут попасть в Git после review

Project-created/sanitized assets могут размещаться в:

```text
tests/fixtures/npt/xsde/
tests/fixtures/npt/xtabl/
tests/fixtures/npt/visual/
migration/evidence/npt/
tools/npt/
```

Примеры:

- minimal XSDE regression fixture;
- minimal XTABL fixture без sensitive plant data;
- sanitized/cropped renderer reference;
- derived inventory с counts/types без proprietary payload;
- project-authored parser/analyzer/generator code.

## 6. NPT signal catalogue

Large generated catalogue databases хранятся на controlled VPS/local storage, а не в Git.

Project-authored catalogue/search code может позже жить в:

```text
tools/npt/catalog/
```

Synthetic/tiny sanitized fixtures:

```text
tests/fixtures/npt/catalog/
```

## 7. Public normative sources — получать онлайн, а не загружать заранее

Government/sector normative acts и ГОСТ/ЕСКД metadata получаются из current online authoritative sources по мере начала соответствующей compliance/profile работы.

Приоритет источников:

```text
official publication / official issuer
→ official Rosstandart standards catalogue
→ authoritative consolidated legal/reference cross-check when needed
```

Не требуется вручную загружать большой government-normative document pack.

Для ГОСТ:

- official status/edition/change metadata проверяется по authoritative catalogue sources;
- detailed rule extraction при необходимости использует lawful full-text source;
- random unofficial internet copies не являются normative authority;
- полные texts стандартов не commit в Git без clear redistribution rights и практической необходимости.

Git хранит project-authored outputs:

```text
docs/compliance/NORMATIVE_REGISTRY.md
compliance/rules/...
compliance/tests/...
```

## 8. Enterprise/site instructions — не загружать для normal development

Внутренние enterprise/site instructions не являются development inputs и не должны загружаться в GitHub, ChatGPT Project storage или shared VPS corpus по умолчанию.

Продукт поддерживает fine-grained local policy на deployment через `LocalPolicy`/`PolicyPackage`, но разработка механизма использует synthetic/cleared examples.

На реальном предприятии internal source documents остаются внутри authorized local environment.

## 9. Manufacturer documentation

Использовать public manufacturer manuals/technical sources, когда они доступны.

Restricted/proprietary manuals остаются вне Git.

Manufacturer-specific rules требуют exact model/revision/applicability provenance.

## 10. First Import/Auto Layout fixtures — должны попасть в Git

Для первого vertical slice требуется небольшой **synthetic или sanitized** CSV/XLSX dataset оборудования и связей.

Target location:

```text
tests/fixtures/import/
├── representative_single_line.xlsx
├── representative_single_line.csv
├── representative_single_line.expected.json
└── README.md
```

Fixture должен содержать достаточно electrical semantics для выбранного scenario с circuit breaker/disconnector/earthing switch/busbar/line/transformer, но не confidential station data без явного clearance.

## 11. Native ГОСТ/ЕСКД symbol assets — должны попадать в Git после authoring

Project-authored native symbols с independent provenance размещаются в:

```text
assets/symbols/
├── gost-eskkd/
│   ├── <profile-version>/
│   └── provenance/
└── ui/
```

Не заполнять эту библиотеку copied NPT/vendor assets.

## 12. Visual NPT reference media

Raw native NPT/Modus screenshots/videos реального объекта по умолчанию хранятся в private reference storage:

```text
/opt/electrical-engineering-platform/corpus/npt-reference/visual/
```

В Git попадают только sanitized regression images.

## 13. Development Bridge secrets

Bearer token, `.env`, passwords, SCADA credentials, certificates/private keys и API tokens никогда не commit в Git.

Development Bridge secrets хранятся в controlled server secret files / GitHub secrets по назначению.

Bridge endpoint может быть публично routable по HTTPS, но authentication secret и execution permissions остаются закрытыми.

## 14. Что реально требуется от владельца на ближайших этапах

Внешние/reference материалы, которые действительно нужны:

1. `scada-npt-expert.zip` — private NPT corpus only;
2. `LW.zip` / `000. LW.zip` — private NPT/LW corpus only;
3. selected native NPT/Modus screenshots для renderer fidelity work, когда NPT module будет возобновлён;
4. один synthetic/sanitized representative equipment-connectivity CSV/XLSX для первого import vertical slice.

Не требуется:

- old TBP project;
- enterprise/site internal instructions;
- manually assembled package российских нормативных документов/ГОСТов.

Public normative sources приобретаются и re-verify online при начале соответствующего compliance work item.