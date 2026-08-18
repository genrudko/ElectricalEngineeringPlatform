# План миграции legacy-источников в единый продукт

Статус: канонический Foundation-документ

## 1. Назначение

Migration **не означает** копирование нескольких старых codebases в одно дерево.

У нового продукта три разных отношения к legacy-источникам:

1. **ElectroScheme Studio** — research/prototype source, из которого после отдельной проверки могут быть переиспользованы конкретные evidence/tools;
2. **NPT Engineering Toolkit / NPT materials** — compatibility/research source, чьи доказанные format semantics нужно сохранить за NPT boundary;
3. **старый TBP project** — **не является источником code/config migration** для нового Switching module. Новый Switching/TBP реализуется с нуля поверх Domain Core, Compliance Core и current verified normative sources.

Цель — сохранять доказанные знания только там, где они реально снижают риск, не перетаскивая устаревшую архитектуру в новый продукт.

## 2. Классификация legacy assets

Для каждого значимого legacy asset, рассматриваемого для reuse, используется один из статусов:

```text
RETAIN_AS_EVIDENCE
SALVAGE_BEHIND_NEW_CONTRACT
REIMPLEMENT_FROM_BEHAVIOR
MIGRATE_DATA_ONLY
ARCHIVE_HISTORICAL
DO_NOT_MIGRATE
RETIRE_AFTER_ACCEPTANCE
```

Для reused/migrated asset по возможности фиксируются:

- source repository/path/commit;
- исходная ответственность;
- известные ограничения;
- новый owner module;
- required tests/evidence;
- dependency/licensing concerns;
- условие retirement.

## 3. ElectroScheme Studio

Source repository: `genrudko/electroscheme-studio`.

### 3.1. Сохранять как research/evidence

- VSDX/VSSX inspection/ShapeSheet research;
- market/reference-product research;
- prototype quarantine/disposition method;
- snapping/grid/busbar experiments;
- platform/packaging measurements;
- Tauri spike implementation и failure evidence;
- Visio controlled fixtures/round-trip knowledge.

### 3.2. Скорее реализовать заново по новому contract

- application shell;
- old CSS/design/ribbon/property composition;
- large canvas ownership/state composition;
- duplicated frontend/backend document state;
- product-level command/mutation path, если он конфликтует с новым Domain Core.

### 3.3. Решается только после Platform Stack Spike

- Vue/TypeScript/SVG code reuse;
- Rust/Tauri native adapters;
- current web build pipeline.

Ни один Tauri/WebView asset не становится production baseline только потому, что historical CI был green.

## 4. NPT Engineering Toolkit

NPT research — важный compatibility/domain corpus и должен сохраняться осторожно.

### 4.1. Доказанные знания, которые нужно перенести

- lossless/preserve-first XML handling;
- XSDE document/object inventory и semantics;
- distinction embedded `CustElem` vs external `.menu`;
- typed custom-value semantics (`scdState`, `scdCommand`, `scdValue`, `scdColor`, etc.);
- ASU/TECH/KKS signal catalog model/search;
- XTABL v6.0 lossless records и proven field meanings;
- preserve-only handling для unknown XTABL fields;
- native resource/path alias handling;
- explicit safe/unsafe creation boundaries.

### 4.2. Что остаётся NPT-specific

- `sTag` allocation/counters;
- `RTID`/`TechData` registry semantics;
- `CustElem` embedding/storage;
- `scd*` serialization;
- XSDE XML order/comments/unknown fields;
- XTABL raw-record specifics.

Эти данные живут в NPT adapters/storage и переводятся в neutral domain entities только там, где mapping доказан.

### 4.3. Критический unresolved experiment

NPT `nodes` связаны с topology, но пока не доказано, что они позволяют восстановить complete neutral electrical graph.

Required evidence:

1. выбрать небольшой known real cell/mnemonic;
2. извлечь object/Tech/node relations;
3. сопоставить их neutral terminals/connections;
4. построить topology graph независимо от NPT renderer;
5. вручную сравнить с visible one-line diagram;
6. проверить energized/de-energized propagation на известных switching states;
7. зафиксировать unmapped/ambiguous relationships.

До принятия этого эксперимента NPT topology import не является Core invariant или обещанной capability.

### 4.4. Renderer status

Существующий Mnemo renderer не принят по fidelity. До расширения editor scope требуется side-by-side comparison с native NPT/Modus.

## 5. Switching / TBP — clean-sheet implementation

Owner decision: **не мигрировать старый TBP codebase, YAML/configuration, rules и tests в новый продукт**.

Новый `Modules.Switching` пишется с нуля на основе:

- neutral `ElectricalProject` model;
- shared equipment/terminal/connection/topology/state semantics;
- Compliance Core;
- current verified Russian normative sources;
- synthetic/cleared engineering scenarios;
- будущей deployment-specific настройки через Local Policy Overlay mechanism.

Старый TBP может оставаться в прежнем локальном архиве только как historical context. Он не является dependency, test oracle, normative authority или required migration input.

### 5.1. Что может сохраниться только как уже принятый продуктовый принцип

- switching-form generation — decision support, а не real-equipment control;
- qualified human review остаётся обязательным;
- rules требуют provenance/applicability/versioning;
- object/site-specific stricter policy является поддерживаемой capability.

Эти принципы заново определены в текущих canonical documents и не требуют копирования старой реализации.

## 6. Стратегия нормативных источников

Федеральные/отраслевые нормативные акты и standards получаются из current online authoritative sources непосредственно при rule/profile development.

Приоритет:

```text
official publication / official issuer
→ official standards catalogue
→ authoritative legal/reference cross-check when needed
```

Старый TBP repository не используется как источник current normative truth.

Для ГОСТ official status/metadata берётся из Rosstandart/catalogue sources. Full-text access/redistribution используется только через lawful sources; случайные internet copies не становятся нормативным authority.

## 7. Enterprise/site instructions

Внутренние enterprise/site instructions **не являются required development inputs и не загружаются в общий project/repository по умолчанию**.

Platform должна поддерживать такие local policies на deployment, но Foundation/development tests используют synthetic или deliberately cleared overlay examples.

На реальном объекте authorized administrators формируют local policy package внутри контролируемого environment. Shared GitHub development process не требует оригиналов внутренних документов.

## 8. Философия data migration

Old data импортируется через explicit adapters/migrations только при наличии реального product workflow.

Если требуется round-trip compatibility, особенно для NPT, vendor-specific payload сохраняется в adapter-owned structures/extensions, а не теряется при flattening в neutral Core.

## 9. Repository strategy

Во время Foundation и ранних spikes:

- держать новый repository чистым;
- не bulk-vendor старые product trees;
- не импортировать old TBP project;
- не commit NPT vendor binaries/full corpus;
- не собирать internal enterprise/site instructions как development assets;
- fetch/re-verify public normative sources при начале конкретной compliance работы;
- создавать production source tree только после platform stack selection;
- фиксировать disposition до любого legacy reuse.

## 10. Migration gates

Legacy asset переиспользуется только если:

1. существует конкретный target module/use case;
2. reuse дешевле/безопаснее clean reimplementation;
3. behavior/data semantics понятны;
4. representative tests могут это доказать;
5. required provenance/unknown fields не теряются;
6. владелец принимает reuse decision.

Для old TBP текущий disposition — `DO_NOT_MIGRATE`, пока владелец явно не переоткроет решение для конкретного narrowly identified artifact.

## 11. Anti-goal

Не оптимизировать migration под сохранение уже потраченного времени.

Сохранять **validated knowledge, compatibility semantics и полезное research evidence**, когда это реально снижает риск. Product logic, не соответствующий новой архитектуре, реализуется заново.