# Языковая политика проекта

Статус: канонический Foundation-документ  
Применяется ко всему `ElectricalEngineeringPlatform`.

## 1. Основное правило

Проект использует **два языка с разными областями ответственности**:

```text
Пользовательская и документальная часть → русский
Техническая и внутренняя часть          → английский
```

Это архитектурная договорённость проекта, а не вопрос личного стиля разработчика.

## 2. Русский — язык продукта

На русском языке выполняются:

- весь основной пользовательский интерфейс;
- меню, команды, панели, диалоги и подсказки;
- пользовательские сообщения об ошибках, предупреждениях и неопределённости;
- Property Inspector labels и названия пользовательских свойств;
- UI Gallery в части пользовательских подписей;
- встроенная справка;
- вся каноническая документация в `docs/`;
- `README.md` и `AGENTS.md`;
- пользовательские отчёты и диагностические объяснения;
- генерируемые бланки/формы/отчёты, если конкретный output profile не требует другого языка;
- описание нормативных требований и результатов их проверки.

Пример пользовательского UI:

```text
Выключатель
Разъединитель
Заземляющий нож
Проверить положение
Операция запрещена
```

Внутренняя английская model не должна без необходимости протекать в основной UI:

```text
CircuitBreakerPosition: UNKNOWN   # internal — нормально
CircuitBreaker State: UNKNOWN     # основной русский UI — неправильно
```

Корректный пользовательский вариант:

```text
Положение выключателя: Неизвестно
```

## 3. Английский — язык внутренней технической части

На английском языке выполняются:

- имена classes/interfaces/methods/functions;
- variables/constants;
- namespaces/packages/module IDs;
- enum values;
- schema/serialization keys;
- API contracts;
- native database field/table names;
- internal event/command IDs;
- machine-readable error/rule codes;
- unit/integration test names;
- source-code comments, если комментарий действительно нужен;
- internal architecture/domain entity names;
- technical filenames/directories, где английский упрощает tooling и cross-platform development.

Пример:

```text
CircuitBreaker
Disconnector
EarthingSwitch
Busbar
VoltageLevel
Terminal
Connection
TopologyGraph
SwitchingOperation
InterlockRule
EnergizationState
```

Внутренние identifiers не транслитерируются:

```text
Vykluchatel          # запрещено
ZazemlyayushchiyNozh # запрещено
CircuitBreaker       # правильно
EarthingSwitch       # правильно
```

## 4. Профессиональный инженерный английский

Во внутренней технической части используется устоявшийся professional engineering English электроэнергетики, а не буквальные кальки и самодельная терминология.

При выборе internal term приоритет:

1. устоявшаяся международная power-engineering terminology;
2. IEC/IEEE/industry usage, когда применимо;
3. terminology официальных technical formats/protocols;
4. canonical project glossary.

Базовые примеры:

```text
grid / power grid
electrical network
circuit breaker
disconnector
earthing switch
busbar
bus section
feeder
transformer
power transformer
current transformer
voltage transformer
terminal
connection
voltage level
switchgear
bay
switching operation
switching sequence
interlock
energized / de-energized
unknown state
```

Один русский термин не обязан иметь один универсальный английский эквивалент. Например, «ячейка» в зависимости от equipment/context может соответствовать `Bay`, `Panel`, `Cubicle` или другому term.

## 5. RU ↔ EN глоссарий

Канонический mapping между русским UI и английской internal model хранится в:

```text
docs/project/RU_EN_ENGINEERING_GLOSSARY.md
```

Glossary определяет:

- русский user-facing term;
- preferred English internal term;
- context/limitations;
- допустимые synonyms;
- undesirable/forbidden variants where needed.

При добавлении важной shared domain entity сначала проверяется glossary. Если mapping отсутствует, новый term добавляется осознанно и централизованно.

## 6. Документация

**Вся новая каноническая документация пишется на русском языке.**

Английские technical names сохраняются там, где являются точными identifiers:

```text
ElectricalProject
TopologyGraph
CircuitBreaker
ImportPlan
UNKNOWN
```

Допустимый стиль:

> `TopologyGraph` хранит электрическую топологию и не зависит от экранной geometry схемы.

Недопустимый без специальной причины стиль:

> The TopologyGraph stores electrical topology and is independent from screen geometry.

То есть explanatory narrative — русский; exact technical identifier — английский.

Имена documentation files могут оставаться английскими (`DOMAIN_AND_PROJECT_MODEL.md`, `UI_CORE.md` и т. п.), поскольку являются частью internal repository structure. Их human-readable content остаётся русским.

## 7. Интерфейс и localization

Русский — **обязательный первый язык интерфейса**.

На раннем этапе не требуется сложная multi-language localization platform только ради hypothetical future translation. Однако user-facing strings не должны хаотично находиться в domain logic; выбранный UI framework должен позволять centralized ownership strings.

Если появится реальная потребность во втором языке, localization добавляется отдельным work item без изменения internal English domain identifiers.

## 8. Допустимые English/Latin данные в UI

Английские/латинские значения могут отображаться пользователю, если это реальные technical data или standard identifiers:

- KKS/FTS/ASU/TECH identifiers;
- vendor command names;
- IEC/ГОСТ designations;
- units и standard letter designations;
- оригинальные names импортируемых fields;
- filenames/paths;
- diagnostic technical details в advanced mode.

Основное explanation при этом остаётся русским.

Пример:

```text
Команда NPT: CDF_CMD_WRITE_Q
Сигнал: 35AAB10CQ001
Качество сигнала: Неизвестно
```

## 9. Логи и диагностика

Machine-readable codes — английские:

```text
POLICY_CONFLICT_WEAKENING
UNKNOWN_EQUIPMENT_STATE
IMPORT_TERMINAL_AMBIGUITY
```

User-facing messages — русские:

```text
Локальное правило пытается ослабить обязательное требование.
Положение оборудования неизвестно.
Не удалось однозначно определить присоединительный терминал.
```

Это сохраняет stable internal codes и нормальный Russian UX одновременно.

## 10. Commits, branches, issue IDs и work-item IDs

Stable technical identifiers остаются английскими:

```text
UNIFIED-FOUNDATION-001
PLATFORM-STACK-SPIKE-001
architecture/unified-foundation-001
```

Commit subject предпочтительно писать на английском в короткой technical form:

```text
docs: define project language policy
feat: add topology graph validation
fix: preserve manual layout constraints on reimport
```

Issue/PR body и owner-facing explanation — русские, кроме exact technical identifiers/code.

## 11. Нормативная документация

Названия российских нормативных документов, требования и user-facing explanations сохраняются на русском языке.

Internal `rule_id` — English/machine-readable.

Пример structure:

```text
rule_id: SWITCHING.EARTHING_SWITCH.REQUIRES_PROVEN_DEENERGIZATION

Источник:
Правила переключений в электроустановках ...

Сообщение:
Перед включением заземляющего ножа требуется подтверждённое отсутствие напряжения в соответствии с применимым правилом.
```

Пример показывает только separation language layers. Exact rule semantics всегда определяется verified authoritative source.

## 12. Acceptance rule

Foundation и subsequent work items не считаются language-complete, если:

- canonical docs содержат English narrative paragraphs без реальной причины;
- основной UI содержит необоснованные English labels;
- internal identifiers написаны транслитом или хаотичным RU/EN mix;
- одна domain entity получает несколько случайных English names;
- пользователь видит internal enum/error code вместо русского explanation.

Языковая политика является частью architectural quality и проверяется при review наравне с UI/Domain boundaries.