# Граница NPT Compatibility

Статус: канонический Foundation-документ

## 1. Роль

NPT Expert/Modus compatibility — отдельный product module/adaptation layer, а не source architecture единой платформы.

NPT даёт проекту:

1. ценный реальный industrial corpus для research equipment/topology/state/format behavior;
2. compatibility target для XSDE/XTABL/project workflows.

## 2. Boundary

```text
NPT project/files/catalog
        ↕
Modules.Npt
├── parsers/writers
├── lossless vendor payload
├── signal/catalog adapter
├── vendor IDs/counters
├── NPT renderer/editor semantics
└── compatibility validation
        ↕ mapping
Neutral Domain Core
```

Core не знает, как NPT сериализует `sTag`, `TechData`, `RTID`, `CustElem` или `scd*`.

## 3. Доказанный research baseline

Текущее evidence включает:

- 475 studied XSDE documents успешно parse;
- large real object corpus (~145k objects в изученном наборе);
- lossless unchanged XSDE round-trip для studied corpus;
- embedded `customView`/`CustElem` definitions покрывают used custom views в изученных документах;
- external `.menu` libraries важны для insertion/palette, а embedded definitions являются source of truth existing saved file;
- NPT custom-value semantics typed и не сводятся к KKS;
- `scdCommand` может быть KKS-like reference или named NPT command;
- `scdValue`, `scdColor`, `scdFormat` и другие literals нельзя считать signal references;
- существует large-scale ASU/TECH/KKS catalog research;
- XTABL v6.0 lossless core доказан для seven current tables / 1461 records;
- несколько XTABL fields остаются preserve-only/unknown;
- NPT ID/counter allocation имеет hidden/reserved behavior, поэтому simple `+1` unsafe для arbitrary object creation.

Это compatibility facts, а не neutral domain invariants.

## 4. Lossless/preserve-first rule

При editing vendor files unknown data сохраняется по умолчанию.

XSDE writer должен сохранять where required:

- unknown attributes/elements;
- XML order;
- comments;
- embedded definitions;
- vendor values;
- untouched byte representation там, где lossless core это позволяет.

Нельзя reconstruct full document из simplified neutral representation с потерей unknown content.

## 5. Safe editing vs creation

Текущий posture:

- reading existing objects — strong evidence;
- lossless unchanged write — strong evidence;
- editing known non-topological properties/objects — bounded/promising evidence;
- inserting arbitrary new Tech-backed objects — experimental до repeated native Modus/SCADA acceptance;
- electrical topology creation/reconnection и `nodes` manipulation — blocked до explicit proof.

UI должен явно отражать capability level.

## 6. Renderer fidelity

Existing prototype Mnemo renderer не принят как visually faithful.

До broad editor expansion:

1. выбрать known mnemonic(s);
2. получить native NPT/Modus screenshot при known state/scale;
3. render тот же XSDE;
4. сравнить geometry, text, colors, line styles, custom symbols, images/resources и clipping;
5. отделить static-render defects от runtime-state/value semantics;
6. закрыть high-impact fidelity gaps.

## 7. NPT signal catalog

Shared NPT catalog может expose neutral signal references/search results, но сохраняет source-system semantics.

Neutral Domain `SignalBinding` ссылается на catalog identity через adapter contracts. Core не копирует NPT SQL/data model.

## 8. Typed property model

NPT property UI требует typed descriptors:

```text
signal reference
command reference or named command
literal state-to-text mapping
literal color mapping
format string
device reference
quality/static flags
preserve-only unknown
```

Generic `name=value string` editing недостаточно как primary UX.

## 9. XTABL direction

High-value target:

```text
select equipment set (например WTG 1..84)
+ select parameter columns
+ choose template/profile
→ generate table records
→ review/edit
→ write compatible XTABL
```

Использовать только proven field semantics и known-good templates; unknown fields copied/preserved, а не inferred.

## 10. Topology extraction experiment

Критический unknown: достаточно ли `nodes`/Tech relationships для usable electrical connectivity.

Required experiment должен дать:

- neutral graph artifact;
- mapping table NPT object ↔ domain equipment/terminal;
- unresolved references;
- visual/manual comparison;
- state-dependent connectivity test.

До acceptance NPT topology extraction остаётся research и не считается promised import capability.

## 11. Native assets и IP/provenance

NPT symbol/library assets могут быть необходимы для compatibility rendering/editing там, где usage rights это разрешают.

Они не становятся автоматически native Scheme Studio symbol library. Native symbols получают independent provenance и applicable standards basis.

## 12. Corpus location

Full NPT reference corpus/vendor binaries остаются вне GitHub на controlled development storage/VPS.

Repository содержит synthetic fixtures, cleared/minimal examples where permissible, schemas/tests и project-created normalized outputs.

Full-corpus compatibility tests выполняются только в controlled runner lane.

## 13. Module deliverables

Eventually:

- project/file discovery;
- XSDE Mnemo Editor;
- faithful renderer;
- typed properties;
- signal picker/catalog;
- validation/diagnostics;
- safe backup/save/reparse;
- XTABL editor/generator;
- controlled resource-library palette;
- optional topology import after proof.

Текущий scope не включает replacement NPT runtime/server/SCADA services.