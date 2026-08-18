# Input Assets and Storage Map

Статус: canonical Foundation guidance

## 1. Principle

Not every useful source file belongs in GitHub.

The product repository stores project-owned source, documentation, tests and cleared fixtures. Vendor distributions, full industrial corpora, confidential instructions and large generated databases stay in controlled private storage and are referenced by manifests/provenance.

This rule applies even while the repository is private, because repository visibility may change later and Git history is hard to clean reliably.

## 2. Already available — no manual upload needed

### ElectroScheme Studio

Source: `genrudko/electroscheme-studio`.

Do not copy the whole repository here. Migration work reads the old repo directly and moves only accepted assets behind new contracts.

### EOD

Source: `genrudko/electronic-operational-docs`.

Do not copy EOD code. Future integration uses a bounded bridge work item.

## 3. TBP / switching source — manual source needed

The authoritative TBP project has historically existed as a local working tree (for example `TBP_project_py`). Before code migration, preserve one exact source snapshot and inventory it.

### Preferred first handoff

Provide the complete TBP project tree as one archive to the project working context / controlled VPS staging, including Git metadata separately if history exists.

Do **not** immediately dump the archive into the product repository.

After inventory, project-owned files selected for migration will land under production locations such as:

```text
src/.../Modules.Switching/        # after platform stack is chosen
src/.../Core.Compliance/          # only neutral/shared rule infrastructure

migration/evidence/tbp/           # small project-authored migration manifests/reports only
```

Do not move local regulatory PDFs/manuals into Git merely because TBP references them.

## 4. NPT / Modus raw corpus — NEVER Git by default

Known inputs:

```text
scada-npt-expert.zip
LW.zip / 000. LW.zip
```

These contain vendor/deployed-system binaries, libraries, project corpora and database dumps.

Preferred controlled VPS location:

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

Do not commit:

- raw archives;
- vendor EXE/DLL/OCX libraries;
- full station project tree;
- raw SQL/database dumps;
- large derived catalog SQLite databases.

## 5. NPT-derived assets that MAY enter Git after review

Project-created/sanitized assets may be committed in these locations:

```text
tests/fixtures/npt/xsde/           # minimal cleared XSDE fixtures
tests/fixtures/npt/xtabl/          # minimal cleared XTABL fixtures
tests/fixtures/npt/visual/         # sanitized visual reference screenshots
migration/evidence/npt/            # inventories/mapping reports, no vendor payload
tools/npt/                          # project-authored parsers/analyzers/generators after migration
```

Examples that need explicit review before Git:

- tiny XSDE reduced to the minimum object set needed for a regression;
- tiny XTABL fixture containing no sensitive plant data;
- side-by-side screenshot cropped/sanitized to remove confidential operational information;
- derived JSON inventory with counts/types but no proprietary database dump.

## 6. NPT signal catalogue

A large generated SQLite catalogue (for example the previously built `NPT_SIGNAL_CATALOG_MIN.sqlite`) belongs on controlled VPS/local storage, not Git.

Project-authored catalogue-building/search code may later live under:

```text
tools/npt/catalog/
```

Synthetic or tiny sanitized catalog fixtures may live under:

```text
tests/fixtures/npt/catalog/
```

## 7. Normative source documents

Official acts, ГОСТ texts, enterprise/site instructions and manufacturer manuals are source evidence, but full document redistribution rights and confidentiality differ.

Preferred controlled storage:

```text
/opt/electrical-engineering-platform/corpus/normative/
├── federal/
├── gost/
├── manufacturer/
├── enterprise/
└── site/
```

Git stores:

```text
docs/compliance/NORMATIVE_REGISTRY.md
compliance/rules/...                # after implementation begins
compliance/tests/...                # project-authored tests
```

Do not put confidential site instructions or licensed standards text into Git without explicit rights review.

## 8. First import/auto-layout fixtures — SHOULD enter Git

For the first vertical slice prepare one small **synthetic or sanitized** CSV/XLSX equipment/connectivity dataset.

Target location after implementation starts:

```text
tests/fixtures/import/
├── representative_single_line.xlsx
├── representative_single_line.csv
├── representative_single_line.expected.json
└── README.md                       # provenance/meaning/expected ambiguity cases
```

The fixture should contain enough real electrical semantics to exercise breakers/disconnectors/earthing switch/bus/line/transformer as selected for the slice, but no confidential station data unless explicitly cleared.

## 9. Native ГОСТ/ЕСКД symbol assets — SHOULD enter Git when authored

Project-authored native symbols with independent provenance belong in:

```text
assets/symbols/
├── gost-eskkd/
│   ├── <profile-version>/
│   └── provenance/
└── ui/
```

Do not populate this with copied NPT/vendor assets.

## 10. Visual reference media

Raw native NPT/Modus screenshots/videos from a real object should stay in private reference storage by default:

```text
/opt/electrical-engineering-platform/corpus/npt-reference/visual/
```

Only sanitized regression images enter:

```text
tests/fixtures/npt/visual/
```

## 11. Secrets and certificates

Never commit `.env`, passwords, SCADA credentials, certificates/private keys or API tokens. Development/VPS secrets use GitHub secrets or controlled local secret storage.

## 12. Immediate owner handoff list

To continue migration/research efficiently, the owner should provide/retain access to:

1. complete local `TBP_project_py` source snapshot, preferably with `.git` history metadata if it exists;
2. `scada-npt-expert.zip` — private corpus only, not Git;
3. `LW.zip` / `000. LW.zip` — private corpus only, not Git;
4. one or more real native NPT/Modus screenshots of the same known XSDE mnemonics used for renderer comparison — private by default;
5. one representative equipment/connectivity spreadsheet for the future import vertical slice, sanitized if necessary;
6. selected enterprise/site switching instructions and equipment manufacturer instructions needed for the first compliance slice — controlled private source storage, not Git by default.

ElectroScheme Studio and EOD source code need no manual re-upload because they are already accessible through GitHub.
