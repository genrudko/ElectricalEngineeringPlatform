# Русско-английский инженерный glossary

Статус: canonical terminology baseline  
Назначение: связать русский пользовательский язык продукта с английской внутренней domain terminology.

> Это не универсальный электротехнический словарь. Здесь фиксируются именно термины, значимые для модели и интерфейса проекта.

## 1. Общие правила

- Русский столбец определяет пользовательский/UI термин по умолчанию.
- Английский столбец определяет предпочтительное внутреннее technical name.
- Если термин зависит от контекста, это явно указывается.
- Не создавать транслитерированные identifiers.
- Не заменять устоявшийся engineering English буквальной калькой без необходимости.

## 2. Базовая domain terminology

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Электроэнергетическая система / сеть | `PowerGrid`, `Grid`, `ElectricalNetwork` | Выбор зависит от уровня модели; не считать эти термины полностью взаимозаменяемыми |
| Электрическая сеть | `ElectricalNetwork` | Предпочтительно для абстрактной network topology |
| Электроустановка | `ElectricalInstallation` | Использовать, когда речь именно об installation scope |
| Электрический проект | `ElectricalProject` | Корневая canonical project entity |
| Объект / площадка | `Site` | Для физического объекта/площадки; при иной семантике уточнять |
| Уровень напряжения | `VoltageLevel` | Canonical domain term |
| Оборудование | `Equipment` | Общая domain entity |
| Устройство | `Device` | Не использовать автоматически как синоним `Equipment`; зависит от контекста |
| Выключатель | `CircuitBreaker` | Не `Switch` |
| Разъединитель | `Disconnector` | Предпочтительный IEC/industry term |
| Заземляющий разъединитель / заземляющий нож | `EarthingSwitch` | Русская пользовательская подпись зависит от фактического типа/профиля оборудования |
| Шина | `Busbar` | Физическая шина |
| Секция шин | `BusSection` | Отдельная секция busbar |
| Присоединение | `Bay` / `Feeder` | Контекст обязателен: `bay` и `feeder` не являются полными синонимами |
| Фидер | `Feeder` | Использовать там, где это действительно feeder |
| Ячейка | `Bay`, `Panel`, `Cubicle` | Нельзя выбрать единый перевод без оборудования/контекста |
| Трансформатор | `Transformer` | Общий тип |
| Силовой трансформатор | `PowerTransformer` | Конкретизация |
| Автотрансформатор | `Autotransformer` | Canonical term |
| Трансформатор тока | `CurrentTransformer` | Допустимо сокращение `CT` во внутренних technical contexts |
| Трансформатор напряжения | `VoltageTransformer` | `VT`; не смешивать без причины с `PotentialTransformer` |
| Линия электропередачи | `TransmissionLine` / `PowerLine` | Выбор зависит от voltage/system scope |
| Кабельная линия | `CableLine` / `PowerCableCircuit` | Уточнять по модели |
| Нагрузка | `Load` | Canonical term |
| Генератор | `Generator` | Canonical term |
| Терминал / вывод | `Terminal` | Семантическая точка соединения |
| Соединение | `Connection` | Canonical electrical/domain relation |
| Электрическая топология | `ElectricalTopology` / `TopologyGraph` | `TopologyGraph` — конкретная внутренняя структура |
| Граф топологии | `TopologyGraph` | Internal data structure |
| Узел сети | `NetworkNode` | Не путать автоматически с NPT `nodes` |
| Состояние | `State` | Общий термин |
| Положение коммутационного аппарата | `SwitchingDevicePosition` | Typed state concept |
| Открыт / отключён | `OPEN` | Для internal enum; пользовательская формулировка зависит от аппарата |
| Закрыт / включён | `CLOSED` | Аналогично |
| Промежуточное положение | `INTERMEDIATE` | Internal enum |
| Неизвестно | `UNKNOWN` | First-class state |
| Качество сигнала | `SignalQuality` | Typed quality concept |
| Под напряжением | `ENERGIZED` / `Energized` | Semantic energization state |
| Подтверждённо без напряжения | `DEENERGIZED_PROVEN` | Не сокращать до простого boolean `false` |
| Неизвестное состояние напряжения | `UNKNOWN` / `UnknownEnergization` | Зависит от exact type |

## 3. Схемы и графика

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Электрическая схема | `ElectricalDiagram` | Общий термин |
| Однолинейная схема | `SingleLineDiagram` | Предпочтительно `SLD` только как общепринятое сокращение |
| Вид / представление | `View` | Не источник истины |
| Представление схемы | `DiagramView` | View over domain model |
| Графическое представление | `GraphicRepresentation` | Binding equipment → symbol/profile |
| Условное графическое обозначение | `GraphicSymbol`, `SymbolDefinition` | Exact internal type определяется архитектурой |
| Точка присоединения символа | `TerminalAnchor` | Visual anchor mapped to semantic `Terminal` |
| Маршрут линии | `ConnectionRoute` | Geometry only, not topology |
| Излом / точка маршрута | `RouteBend`, `BendPoint` | View geometry |
| Автокомпоновка | `AutoLayout` | User-facing term may be «Автокомпоновка» |
| Предложение компоновки | `LayoutProposal` | Result before apply |
| Ограничение компоновки | `LayoutConstraint` | Preserves manual intent |
| Закреплённая позиция | `PinnedPosition` | Example constraint |
| Выравнивание | `Alignment` | UI/layout term |
| Масштабирование | `Zoom` | UI command may be «Масштаб» |
| Панорамирование | `Pan` | В UI лучше русская формулировка «Перемещение области просмотра»/мышью |
| Область просмотра | `Viewport` | Internal/UI architecture term |

## 4. Импорт и данные

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Импорт | `Import` | |
| Профиль сопоставления | `MappingProfile` | Column/source mapping |
| Сопоставление полей | `FieldMapping` | |
| Нормализация | `Normalization` | |
| Предварительная модель импорта | `ImportCandidate` | Staging object |
| Предварительный просмотр | `Preview` | UI term |
| План импорта | `ImportPlan` | Approved mutation plan |
| Сверка изменений | `Reconciliation` | Для repeat import/update |
| Неоднозначность | `Ambiguity` | |
| Конфликт | `Conflict` | |
| Требует подтверждения | `REQUIRES_CONFIRMATION` | Internal status, Russian UI text |
| Источник данных / происхождение | `Provenance` | В UI обычно «Источник»/«Происхождение данных» |

## 5. Переключения и блокировки

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Переключение | `Switching` | Domain/module name |
| Операция переключения | `SwitchingOperation` | Semantic operation |
| Последовательность переключений | `SwitchingSequence` | Ordered operations |
| Бланк переключений | `SwitchingForm` | Document projection; exact English naming may be refined for export/domain context |
| Программа переключений | `SwitchingProgram` | Не смешивать автоматически с `SwitchingForm` |
| Блокировка | `Interlock` | General internal term |
| Правило блокировки | `InterlockRule` | |
| Проверка условий | `RuleEvaluation` / `ConditionEvaluation` | Depends on service/type |
| Разрешено | `ALLOW` | Internal result |
| Запрещено | `BLOCK` | Internal result |
| Требуется подтверждение | `REQUIRES_CONFIRMATION` | Internal result |
| Моделируемое состояние | `SimulatedState` | Distinct from observed state |
| Исходное/наблюдаемое состояние | `ObservedState`, `BaselineState` | Exact distinction defined by State model |

## 6. Нормативность и локальные правила

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
| Попытка ослабления | `WeakeningAttempt` | Conflict class |
| Конфликт политик | `PolicyConflict` | |

## 7. UI Core

| Русский термин | Внутренний английский термин | Комментарий |
|---|---|---|
| Рабочее пространство | `Workspace` | |
| Документ | `Document` | UI document concept, not necessarily file |
| Панель свойств | `PropertyInspector` | User-facing label may be «Свойства» |
| Дерево проекта | `ProjectTree` | |
| Палитра оборудования | `EquipmentPalette` | |
| Галерея компонентов | `UIGallery` | Internal developer surface |
| Команда | `Command` | Shared command system |
| Горячая клавиша | `Shortcut` / `KeyboardShortcut` | |
| Контекстное меню | `ContextMenu` | |
| Уведомление | `Notification` | |
| Диагностика | `Diagnostics` | |

## 8. Требования к развитию glossary

Новый термин добавляется сюда, если он:

- является shared Domain/Core concept;
- появляется в нескольких модулях;
- имеет риск неоднозначного перевода;
- используется одновременно в UI и code/API;
- относится к safety/compliance semantics.

Не нужно раздувать glossary каждой кнопкой или локальной переменной.

Если существующий английский термин оказывается неверным с точки зрения отраслевой практики, исправление проводится как controlled terminology migration: код, serialization compatibility и UI mappings обновляются осознанно, а не частично.