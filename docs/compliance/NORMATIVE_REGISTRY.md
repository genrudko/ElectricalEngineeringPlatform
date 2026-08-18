# Начальный реестр нормативных источников

Статус: канонический Foundation registry  
Дата verification snapshot: **2026-08-18**

> Этот registry — исходный source baseline, а не заявление, что все требования перечисленных документов уже реализованы. Каждое software rule отдельно требует scope extraction, applicability review и acceptance evidence.

## 1. Дисциплина проверки источников

Приоритет source authority:

```text
Официальный интернет-портал правовой информации / официальный issuer
→ официальный каталог стандартов Росстандарта
→ authoritative legal-reference system для consolidated/current-edition cross-check
→ secondary source только для discovery/support
```

Для production source по возможности сохраняются official publication/registration identifiers.

Публичные государственные/отраслевые документы и ГОСТ/ЕСКД metadata перед реализацией соответствующего compliance work item повторно проверяются онлайн. Вручную загруженный пакет нормативных документов не является обязательным input.

## 2. Electrical safety / operational regulations

### `RU-OT-903N` — ПОТЭЭ

- Issuer: Министерство труда и социальной защиты Российской Федерации.
- Приказ: **15.12.2020 № 903н**.
- Наименование: `Об утверждении Правил по охране труда при эксплуатации электроустановок`.
- Регистрация Минюста: **30.12.2020 № 61957**.
- Official publication identifier: **0001202012300142**, опубликован 30.12.2020.
- Identified amendment: приказ **29.04.2022 № 279н**, official publication **0001202206010011**.
- Current-edition cross-check на 2026-08-18: редакция от **29.04.2025**, включая приказ **№ 287н**, effective с 01.09.2025; consolidated references указывают действие до **01.09.2031**.
- Registry state: `ACTIVE / SOURCE_TEXT_EXTRACTION_PENDING`.
- Product relevance: electrical-safety requirements к работам/персоналу/actions/checks, которые могут ограничивать Switching workflows.
- Caution: многие требования являются organizational/procedural и не сводятся к topology predicates.

### `RU-PTEEP-811` — ПТЭЭП / ПТЭЭПЭЭ

- Issuer: Министерство энергетики Российской Федерации.
- Приказ: **12.08.2022 № 811**.
- Наименование: `Об утверждении Правил технической эксплуатации электроустановок потребителей электрической энергии`.
- Регистрация Минюста: **07.10.2022 № 70433**.
- Official publication identifier: **0001202210070065**, опубликован 07.10.2022.
- Effective from: **07.01.2023**.
- Current-edition cross-check на 2026-08-18: в проверенном consolidated reference более поздняя amendment rule text не выявлена; требуется дальнейший monitoring.
- Registry state: `ACTIVE / SOURCE_TEXT_EXTRACTION_PENDING`.
- Product relevance: эксплуатация электроустановок потребителей, documentation/personnel/maintenance rules, selected switching/project validation в зависимости от role/scope проекта.
- Applicability warning: правила не применяются одинаково автоматически ко всем объектам/пользователям; Compliance Core должен resolve organization/object scope.

### `RU-PTEES-1070` — ПТЭЭС

- Issuer: Министерство энергетики Российской Федерации.
- Приказ: **04.10.2022 № 1070**.
- Наименование: `Об утверждении Правил технической эксплуатации электрических станций и сетей Российской Федерации и о внесении изменений ...`.
- Регистрация Минюста: **06.12.2022 № 71384**.
- Official publication identifier: **0001202212060056**.
- Identified amendment chain:
  - 29.11.2024 **№ 2321**;
  - 09.12.2024 **№ 2398**;
  - 30.10.2025 **№ 1427** (registered 30.03.2026 № 85786, effective 11.04.2026);
  - 08.04.2026 **№ 343** (registered 15.06.2026 № 87018, effective 27.06.2026).
- Current-edition cross-check на 2026-08-18: **08.04.2026**.
- Registry state: `ACTIVE / SOURCE_TEXT_EXTRACTION_PENDING`.
- Product relevance: эксплуатация электрических станций/сетей, technical state/operations и selected switching/topology requirements для applicable energy-sector objects.

### `RU-SWITCH-757` — Правила переключений в электроустановках

- Issuer: Министерство энергетики Российской Федерации.
- Приказ: **13.09.2018 № 757**.
- Наименование: `Об утверждении Правил переключений в электроустановках`.
- Регистрация Минюста: **22.11.2018 № 52754**.
- Identified consolidated amendment chain:
  - 23.06.2022 **№ 582**;
  - 12.08.2022 **№ 811** (changes to order-level provisions);
  - 04.10.2022 **№ 1070**;
  - 01.09.2023 **№ 714**, official publication **0001202312210014**;
  - 09.12.2024 **№ 2398**;
  - 08.04.2026 **№ 343**, effective 27.06.2026.
- Current-edition cross-check на 2026-08-18: **08.04.2026**.
- Registry state: `ACTIVE / HIGH_PRIORITY_FOR_SWITCHING_MODULE`.
- Product relevance: прямые требования к organization/order/sequence switching, switching programs, standard programs, switching forms и standard forms.
- Priority: один из первых sources для decomposition в machine-checkable/assisted-review coverage.

## 3. ПУЭ

Registry ID family: `RU-PUE-*`.

**Не создавать fictitious `PUE_CURRENT` source.**

ПУЭ исторически выпускались/изменялись отдельными editions/chapters и имеют сложную историю applicability/status.

Relevant chapters регистрируются отдельно с:

- exact chapter/edition;
- approving act/source;
- effective/status evidence;
- project applicability;
- replacement/supersession relationships.

Foundation state: `DISCOVERY_REQUIRED_PER_CHAPTER`.

Initial priority chapters выбираются из real product workflows, а не через попытку ingest all ПУЭ at once.

## 4. ЕСКД / standards для электрических схем

Source authority для status/changes — официальный каталог Росстандарта.

### `GOST-2.701-2008`

- `Единая система конструкторской документации. Схемы. Виды и типы. Общие требования к выполнению`.
- Rosstandart status на verification: **Действует**.
- Registration order: **25.12.2008 № 702-ст**.
- Known correction: зарегистрирована 05.12.2011 / опубликована в ИУС 2-2012.
- Product role: baseline для scheme type/document/common execution.
- Registry state: `ACTIVE / GRAPHICS_PROFILE_SOURCE`.

### `GOST-2.702-2011`

- `Единая система конструкторской документации. Правила выполнения электрических схем`.
- Rosstandart status на verification: **Действует**.
- Registration order: **03.08.2011 № 211-ст**.
- Effective date: **01.01.2012**.
- Known correction: ИУС 1-2021.
- Product role: electrical scheme execution rules.
- Registry state: `ACTIVE / HIGH_PRIORITY_GRAPHICS_PROFILE_SOURCE`.

### Related ESKD graphical-symbol family — discovery/coverage set

Native symbol/profile work должен проверить applicable current members family УГО/lines по официальному каталогу, а не rely на memory или NPT/Visio appearance.

Initial candidate family включает standards из `ГОСТ 2.72x`, `ГОСТ 2.75x` и related ESKD series по applicable equipment/connection types.

Registry state: `OFFICIAL_CATALOGUE_REVIEW_REQUIRED_BEFORE_ENCODING`.

Coverage family нельзя заявлять, пока exact designation/status каждого relevant standard не проверен в Rosstandart catalogue.

## 5. Enterprise/site/manufacturer sources

Source categories:

```text
MANUFACTURER_MANUAL
MANUFACTURER_TECHNICAL_REQUIREMENT
ENTERPRISE_STANDARD
ENTERPRISE_OPERATION_INSTRUCTION
SITE_SWITCHING_INSTRUCTION
SITE_EQUIPMENT_INSTRUCTION
PROJECT_REQUIREMENT
```

Каждый instance требует:

- organization/site/equipment scope;
- document identifier/revision/date;
- approval/effective period where known;
- authorized storage/reference;
- mapped rules/templates;
- relation to mandatory baseline.

Shared development не требует real internal enterprise/site instructions. Mechanics проверяются на synthetic/cleared policy packages; реальные internal sources остаются в authorized deployment environment.

См. `LOCAL_POLICY_OVERLAYS.md`.

## 6. Initial rule-extraction priority

### Priority A — Switching vertical slice

1. `RU-SWITCH-757` current applicable edition;
2. applicable sections `RU-PTEES-1070` и/или `RU-PTEEP-811` по target object role;
3. applicable `RU-OT-903N` requirements;
4. synthetic stricter local-policy overlay;
5. selected public manufacturer/equipment constraint when useful.

### Priority B — Scheme/graphics vertical slice

1. `GOST-2.701-2008`;
2. `GOST-2.702-2011`;
3. exact current УГО/line standards для первых equipment families;
4. optional enterprise graphical overlay только на synthetic/cleared example, пока реальное deployment не требует иного.

## 7. Registry update policy

Каждый entry имеет `last_verified_at` и recheck перед release, если:

- прошло значительное время;
- опубликована известная amendment;
- source status изменился;
- affected module готовится к operational release.

Changed source создаёт review task; production rules не переписываются silently.

## 8. Known limitation initial snapshot

Foundation проверил identity/current-edition chain highest-priority sources, но **не выполнял clause-by-clause rule extraction или legal applicability opinion для каждого project type**.

Это входит в последующие Compliance Core work items.