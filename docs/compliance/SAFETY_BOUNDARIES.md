# Границы безопасности

Статус: канонический Foundation-документ

## 1. Зачем нужен этот документ

Продукт будет работать с electrical topology, equipment state и switching operations. Это создаёт риск dangerous overconfidence, если software capability описана сильнее, чем позволяет evidence.

## 2. Роль продукта

Текущая роль:

```text
engineering model
+ scheme/document authoring
+ compatibility tooling
+ deterministic validation
+ switching simulation / decision support
+ draft switching-form generation/checking
```

Текущий scope **не включает autonomous real-equipment control**.

## 3. `UNKNOWN` — first-class state

Инвариант:

> Отсутствие доказательства наличия напряжения не является доказательством его отсутствия.

Примеры:

- unknown breaker position ≠ `OPEN`;
- missing signal ≠ `OFF`;
- bad signal quality ≠ safe state;
- missing topology edge ≠ open circuit;
- no modeled voltage-source path при incomplete model ≠ absence of voltage.

Safety-sensitive rules должны propagate uncertainty.

## 4. Energization semantics

Предпочтительны explicit semantic states:

```text
ENERGIZED
DEENERGIZED_PROVEN
UNKNOWN
```

а не simple boolean там, где evidence недостаточно.

## 5. Simulation vs observation

Явно разделяются:

```text
OBSERVED/IMPORTED BASELINE STATE
SIMULATED STATE
PLANNED/TARGET STATE
```

Simulated operation никогда не превращается молча в claimed real-world state.

## 6. Logical interlock vs physical interlock

System может реализовать generic logical/domain interlocks, project/site restrictions и knowledge о manufacturer/device constraints.

Это не означает verification physical locks, PLC/relay/controller logic, hardwired circuits, mechanical interlock health или фактического absence/presence of voltage на объекте.

## 7. Switching-form generation

Пока отдельный accepted safety case не изменит границу:

> Любая generated/assisted switching form или sequence является draft и требует qualified human review/approval по applicable organizational procedure.

Никакой UI label не должен намекать на automatic approval только потому, что software validation passed.

## 8. Normative coverage boundary

Passing machine validation означает только:

```text
все implemented/applicable rules выбранного profile прошли или были resolved
```

Это не означает выполнение всех возможных требований Russian law/standards/site instructions без formally defined coverage scope.

## 9. Incomplete project model

Если project data содержит unresolved equipment identity, connections/topology, state, applicable policy или source version, safety-sensitive functions должны явно переходить в:

```text
BLOCKED
UNKNOWN
REQUIRES_CONFIRMATION
```

а не продолжать молча.

## 10. Imported data trust

CSV/XLSX, NPT, Visio и other imported information не становятся trusted только потому, что parsing succeeded.

Safety-relevant import требует semantic validation и explicit ambiguity review.

## 11. Automated topology inference

Auto-detected/inferred topology не равна verified topology.

Critical logic может требовать confirmed connections according to profile.

NPT `nodes` extraction остаётся research до systematic verification against known schemes.

## 12. AI/LLM boundary

AI может помогать:

- discovery;
- mapping suggestions;
- explanations;
- draft rule extraction;
- layout suggestions;
- documentation.

AI output не является normative authority и не bypass deterministic safety validation/human review.

## 13. Real-time / SCADA integration

NPT signal semantics могут импортироваться для engineering/compatibility.

Current scope не authorizes online control.

Добавление real-equipment commands требует отдельного work item с threat/safety model, authorization, command confirmation, communication quality, fail-safe behavior, audit, commissioning и applicable requirements.

## 14. Fail-safe defaults

- unknown rule source/version → не claim validated baseline;
- missing critical policy → не pass silently;
- parse failure → preserve original и avoid partial destructive write;
- failed save validation → retain/restore backup и показать error;
- unknown switching prerequisite → не auto-allow;
- mandatory/local policy conflict → report conflict и keep mandatory baseline effective.

## 15. Auditability

Для critical validation/simulation outcomes сохраняется достаточно data, чтобы объяснить:

- project/model revision;
- state baseline;
- sequence revision;
- rule/profile versions;
- relevant facts;
- result;
- permitted user confirmations.

## 16. Prohibited claims/features без separate acceptance

- `100% safe switching guaranteed`;
- `fully compliant with all ПУЭ/ПОТЭЭ/ПТЭЭС` без coverage evidence;
- automatic bypass blocked mandatory rules;
- treating unknown state as safe;
- executing real switching solely by model result;
- implication, что software заменяет field verification, protective devices или physical interlocks.