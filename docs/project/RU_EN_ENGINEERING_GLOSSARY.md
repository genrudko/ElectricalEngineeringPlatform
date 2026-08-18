# Русско-английский инженерный глоссарий

Статус: канонический terminology baseline  
Назначение: связать русский user-facing язык продукта с английской internal domain terminology.

> Это не универсальный электротехнический словарь. Здесь фиксируются terms, значимые именно для model, UI и public/internal contracts проекта.

## 1. Общие правила

- Русский столбец определяет предпочтительный user-facing/UI term.
- Английский столбец определяет preferred internal technical term.
- Context-dependent terms сопровождаются explicit notes.
- Transliteration identifiers запрещены.
- Устоявшийся engineering English не заменяется буквальной калькой без необходимости.
- Если один русский term имеет несколько корректных English mappings, выбор фиксируется на уровне конкретного domain context.

## 2. Базовая domain terminology

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Электроэнергетическая система / сеть | `PowerGrid`, `Grid`, `ElectricalNetwork` | Выбор зависит от abstraction level; terms не полностью взаимозаменяемы |
| Электрическая сеть | `ElectricalNetwork` | Предпочтительно для abstract network topology |
| Электроустановка | `ElectricalInstallation` | Использовать при installation scope |
| Электрический проект | `ElectricalProject` | Root canonical project entity |
| Объект / площадка | `Site` | Для physical site; при иной semantics уточнять |
| Уровень напряжения | `VoltageLevel` | Canonical domain term |
| Оборудование | `Equipment` | Общая domain entity |
| Устройство | `Device` | Не использовать автоматически как synonym `Equipment` |
| Выключатель | `CircuitBreaker` | Не `Switch` |
| Разъединитель | `Disconnector` | Preferred IEC/industry term |
| Заземляющий разъединитель / заземляющий нож | `EarthingSwitch` | Russian UI label зависит от actual equipment/profile |
| Шина | `Busbar` | Physical busbar |
| Секция шин | `BusSection` | Отдельная busbar section |
| Присоединение | `Bay` / `Feeder` | Context обязателен; terms не полные synonyms |
| Фидер | `Feeder` | Использовать только для actual feeder semantics |
| Ячейка | `Bay`, `Panel`, `Cubicle` | Universal mapping нет; зависит от equipment/context |
| Трансформатор | `Transformer` | Общий type |
| Силовой трансформатор | `PowerTransformer` | Конкретизация |
| Автотрансформатор | `Autotransformer` | Canonical term |
| Трансформатор тока | `CurrentTransformer` | `CT` допустимо во внутренних technical contexts |
| Трансформатор напряжения | `VoltageTransformer` | `VT`; `PotentialTransformer` не использовать без context reason |
| Линия электропередачи | `TransmissionLine` / `PowerLine` | Выбор зависит от system/voltage context |
| Кабельная линия | `CableLine` / `PowerCableCircuit` | Exact model term уточняется реализацией |
| Нагрузка | `Load` | Canonical term |
| Генератор | `Generator` | Canonical term |
| Терминал / вывод | `Terminal` | Semantic connection point |
| Соединение | `Connection` | Canonical electrical/domain relation |
| Электрическая топология | `ElectricalTopology` / `TopologyGraph` | `TopologyGraph` — internal data structure |
| Граф топологии | `TopologyGraph` | Internal data structure |
| Узел сети | `NetworkNode` | Не смешивать автоматически с NPT `nodes` |
| Состояние | `State` | Общий term |
| Положение коммутационного аппарата | `SwitchingDevicePosition` | Typed state concept |
| Открыт / отключён | `OPEN` | Internal enum; Russian UI wording зависит от apparatus |
| Закрыт / включён | `CLOSED` | Internal enum; UI wording context-dependent |
| Промежуточное положение | `INTERMEDIATE` | Internal enum |
| Неизвестно | `UNKNOWN` | First-class state |
| Качество сигнала | `SignalQuality` | Typed quality concept |
| Под напряжением | `ENERGIZED` / `Energized` | Semantic energization state |
| Подтверждённо без напряжения | `DEENERGIZED_PROVEN` | Не сокращать до boolean `false` |
| Неизвестное состояние напряжения | `UNKNOWN` / `UnknownEnergization` | Зависит от exact type |

## 3. Схемы и графика

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Электрическая схема | `ElectricalDiagram` | Общий term |
| Однолинейная схема | `SingleLineDiagram` | `SLD` допустимо как established abbreviation |
| Вид / представление | `View` | Не source of truth |
| Представление схемы | `DiagramView` | View over domain model |
| Графическое представление | `GraphicRepresentation` | Binding equipment → symbol/profile |
| Условное графическое обозначение | `GraphicSymbol`, `SymbolDefinition` | Exact internal type определяется architecture |
| Точка присоединения symbol | `TerminalAnchor` | Visual anchor mapped to semantic `Terminal` |
| Маршрут линии | `ConnectionRoute` | Geometry only, not topology |
| Излом / точка маршрута | `RouteBend`, `BendPoint` | View geometry |
| Автокомпоновка | `AutoLayout` | Preferred Russian UI term: «Автокомпоновка» |
| Предложение компоновки | `LayoutProposal` | Result before apply |
| Ограничение компоновки | `LayoutConstraint` | Preserves manual intent |
| Закреплённая позиция | `PinnedPosition` | Example constraint |
| Выравнивание | `Alignment` | UI/layout term |
| Масштабирование | `Zoom` | UI label обычно «Масштаб»/«Масштабирование» |
| Панорамирование | `Pan` | UI wording может быть «Перемещение области просмотра» |
| Область просмотра | `Viewport` | Internal/UI architecture term |

## 4. Импорт и данные

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Импорт | `Import` | |
| Профиль сопоставления | `MappingProfile` | Column/source mapping |
| Сопоставление полей | `FieldMapping` | |
| Нормализация | `Normalization` | |
| Предварительная модель импорта | `ImportCandidate` | Staging object |
| Предварительный просмотр | `Preview` | User-facing UI term |
| План импорта | `ImportPlan` | Approved mutation plan |
| Сверка изменений | `Reconciliation` | Repeat import/update |
| Неоднозначность | `Ambiguity` | |
| Конфликт | `Conflict` | |
| Требует подтверждения | `REQUIRES_CONFIRMATION` | Internal status; UI text Russian |
| Источник / происхождение данных | `Provenance` | UI обычно показывает «Источник»/«Происхождение данных» |

## 5. Переключения и блокировки

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Переключение | `Switching` | Domain/module term |
| Операция переключения | `SwitchingOperation` | Semantic operation |
| Последовательность переключений | `SwitchingSequence` | Ordered operations |
| Бланк переключений | `SwitchingForm` | Document projection; exact export naming may later be refined |
| Программа переключений | `SwitchingProgram` | Не смешивать автоматически с `SwitchingForm` |
| Блокировка | `Interlock` | General internal term |
| Правило блокировки | `InterlockRule` | |
| Проверка условий | `RuleEvaluation` / `ConditionEvaluation` | Depends on service/type |
| Разрешено | `ALLOW` | Internal result |
| Запрещено | `BLOCK` | Internal result |
| Требуется подтверждение | `REQUIRES_CONFIRMATION` | Internal result |
| Моделируемое состояние | `SimulatedState` | Distinct from observed state |
| Наблюдаемое состояние | `ObservedState` | Реально observed/imported fact where applicable |
| Исходное состояние моделирования | `BaselineState` | Initial state for simulation/context |

## 6. Нормативность и local policy

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Нормативное требование | `NormativeRequirement` | |
| Нормативное правило | `NormativeRule` / `ComplianceRule` | Exact type depends on Compliance Core |
| Применимость | `Applicability` | |
| Источник требования | `RuleSource` / `NormativeSource` | |
| Нормативный профиль | `ComplianceProfile` | |
| Графический профиль | `GraphicProfile` | ГОСТ/ЕСКД-oriented profile |
| Локальная политика | `LocalPolicy` | |
| Пакет локальных правил | `PolicyPackage` | |
| Ужесточение требования | `StricterConstraint` / `Tightening` | Prefer semantic names in code |
| Попытка ослабления | `WeakeningAttempt` | Conflict concept |
| Конфликт политик | `PolicyConflict` | |

## 7. UI Core

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Рабочее пространство | `Workspace` | |
| Документ | `Document` | UI document concept, not necessarily file |
| Панель свойств | `PropertyInspector` | Preferred user-facing label: «Свойства» |
| Дерево проекта | `ProjectTree` | |
| Палитра оборудования | `EquipmentPalette` | |
| Галерея компонентов | `UIGallery` | Internal developer surface |
| Команда | `Command` | Shared command system |
| Горячая клавиша | `Shortcut` / `KeyboardShortcut` | |
| Контекстное меню | `ContextMenu` | |
| Уведомление | `Notification` | |
| Диагностика | `Diagnostics` | |

## 8. Development Platform

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Мост разработки | `DevelopmentBridge` / `EEPDevelopmentBridge` | Bounded API between ChatGPT Project and VPS |
| Задача разработки | `DevelopmentTask` | Bounded build/test/preview task |
| Рабочая копия | `Workspace` / `RepositoryWorkspace` | Не source of truth; GitHub canonical |
| Артефакт | `Artifact` | Build/test/preview result |
| Формальная проверка | `VerificationGate` / `CIGate` | Exact naming зависит от CI model |

## 9. Правила развития glossary

Новый term добавляется, если он:

- shared Domain/Core concept;
- используется в нескольких modules;
- имеет риск ambiguous translation;
- одновременно появляется в UI и code/API;
- относится к safety/compliance semantics;
- является stable Development Platform contract.

Не нужно добавлять каждую кнопку или local variable.

Если accepted English term позже оказывается неверным с точки зрения industry practice, проводится controlled terminology migration: code, serialization compatibility, docs и UI mappings меняются осознанно и целостно.