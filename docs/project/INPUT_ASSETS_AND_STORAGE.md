# Input Assets and Storage Map

Статус: canonical Foundation guidance

## 1. Principle

Not every useful source file belongs in GitHub, and not every legacy/internal source needs to be collected at all.

The product repository stores project-owned source, documentation, tests and cleared fixtures. Vendor distributions, full industrial corpora, secrets and internal enterprise documentation stay outside Git.

## 2. Already available — no manual upload needed

### ElectroScheme Studio

Source: `genrudko/electroscheme-studio`.

Do not copy the whole repository here. Migration work reads historical research directly and moves/reimplements only accepted assets behind new contracts.

### EOD

Source: `genrudko/electronic-operational-docs`.

Do not copy EOD code. Future integration uses a bounded bridge work item.

## 3. Old TBP project — NOT NEEDED

Owner decision: **do not use the old TBP project as a migration source**.

Therefore there is no immediate handoff requirement for `TBP_project_py`, its Git history, YAML profiles, old rules or tests.

The new Switching/TBP module is written from scratch against the new Domain Core + Compliance Core + current normative sources.

If one day a narrowly identified historical artifact becomes useful, it can be reviewed separately without changing this clean-sheet baseline.

## 4. NPT / Modus raw corpus — private reference only

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

Do not commit raw archives, vendor EXE/DLL/OCX libraries, full station project tree, raw SQL/database dumps or large generated SQLite catalogues.

## 5. NPT-derived assets that MAY enter Git after review

Project-created/sanitized assets may be committed to:

```text
tests/fixtures/npt/xsde/
tests/fixtures/npt/xtabl/
tests/fixtures/npt/visual/
migration/evidence/npt/
tools/npt/
```

Examples:

- minimal XSDE regression fixture;
- minimal XTABL fixture without sensitive plant data;
- sanitized/cropped renderer reference;
- derived inventory with counts/types but no proprietary payload;
- project-authored parser/analyzer/generator code.

## 6. NPT signal catalogue

Large generated catalogue databases belong on controlled VPS/local storage, not Git.

Project-authored catalogue/search code may later live under:

```text
tools/npt/catalog/
```

Synthetic/tiny sanitized fixtures may live under:

```text
tests/fixtures/npt/catalog/
```

## 7. Public normative sources — fetch online, do not pre-upload a corpus

Government/sector normative acts and ГОСТ/ЕСКД metadata should be obtained from current online authoritative sources as the relevant compliance/profile work begins.

Preferred source order:

```text
official publication / official issuer
→ official Rosstandart standards catalogue
→ authoritative consolidated legal/reference cross-check where needed
```

There is no need to manually upload a large government-normative document pack into this project now.

For ГОСТ:

- official status/edition/change metadata must be verified through authoritative catalogue sources;
- use a lawful full-text source when detailed rule extraction requires the text;
- do not rely on random unofficial internet copies as normative authority;
- do not commit full standards texts to Git unless redistribution rights are clear and there is a concrete need.

Git stores the project-authored outputs:

```text
docs/compliance/NORMATIVE_REGISTRY.md
compliance/rules/...
compliance/tests/...
```

## 8. Enterprise/site instructions — DO NOT UPLOAD for normal development

Internal enterprise/site instructions are not required as development inputs and should not be uploaded to GitHub, ChatGPT project storage or the shared VPS corpus by default.

The product still supports fine-grained local policy at deployment time through `Local Policy Overlays`, but development uses synthetic/cleared examples.

At an actual enterprise/site, internal source documents remain inside the authorized local environment. Only the resulting configured policy package/source metadata need to be consumed by that deployment as required by its governance.

## 9. Manufacturer documentation

Use public manufacturer manuals/technical sources when available. Restricted/proprietary manuals stay outside Git.

Manufacturer-specific rules require exact model/revision/applicability provenance.

## 10. First import/auto-layout fixtures — SHOULD enter Git

For the first vertical slice prepare one small **synthetic or sanitized** CSV/XLSX equipment/connectivity dataset.

Target location:

```text
tests/fixtures/import/
├── representative_single_line.xlsx
├── representative_single_line.csv
├── representative_single_line.expected.json
└── README.md
```

The fixture should contain enough real electrical semantics to exercise the selected breaker/disconnector/earthing-switch/bus/line/transformer scenario, but no confidential station data unless explicitly cleared.

## 11. Native ГОСТ/ЕСКД symbol assets — SHOULD enter Git when authored

Project-authored native symbols with independent provenance belong in:

```text
assets/symbols/
├── gost-eskkd/
│   ├── <profile-version>/
│   └── provenance/
└── ui/
```

Do not populate this with copied NPT/vendor assets.

## 12. Visual NPT reference media

Raw native NPT/Modus screenshots/videos from a real object stay in private reference storage by default:

```text
/opt/electrical-engineering-platform/corpus/npt-reference/visual/
```

Only sanitized regression images enter Git.

## 13. Secrets and certificates

Never commit `.env`, passwords, SCADA credentials, certificates/private keys or API tokens. Development/VPS secrets use GitHub secrets or controlled local secret storage.

## 14. Immediate input list

For the next project stages the only externally supplied/reference materials we actually need are:

1. `scada-npt-expert.zip` — private NPT corpus only;
2. `LW.zip` / `000. LW.zip` — private NPT/LW corpus only;
3. selected native NPT/Modus screenshots for renderer fidelity work when that module is resumed;
4. one synthetic/sanitized representative equipment-connectivity CSV/XLSX for the first import vertical slice.

Not required:

- old TBP project;
- enterprise/site internal instructions;
- a manually assembled package of public Russian regulations/ГОСТs.

Public normative sources are acquired and re-verified online when the relevant compliance work item begins.