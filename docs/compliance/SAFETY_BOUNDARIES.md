# Safety Boundaries

Статус: canonical foundation document

## 1. Why this document exists

The product will reason about electrical topology, equipment state and switching operations. That can create dangerous overconfidence if software capability is described more strongly than evidence supports.

## 2. Product role

Current role:

```text
engineering model
+ scheme/document authoring
+ compatibility tooling
+ deterministic validation
+ switching simulation / decision support
+ draft switching-form generation/checking
```

Current scope does **not** include autonomous real-equipment control.

## 3. UNKNOWN is first-class

Invariant:

> Lack of evidence that equipment/section is energized is not proof that it is de-energized.

Examples:

- unknown breaker position ≠ OPEN;
- missing signal ≠ OFF;
- bad signal quality ≠ safe state;
- missing topology edge ≠ open circuit;
- no modeled voltage-source path under incomplete model ≠ absence of voltage.

Safety-sensitive rules propagate uncertainty.

## 4. Energization semantics

Prefer explicit semantic states such as:

```text
ENERGIZED
DEENERGIZED_PROVEN
UNKNOWN
```

instead of a simple boolean when evidence cannot justify it.

## 5. Simulation vs observation

Clearly separate:

```text
OBSERVED/IMPORTED BASELINE STATE
SIMULATED STATE
PLANNED/TARGET STATE
```

A simulated operation never silently becomes a claimed real-world state.

## 6. Logical interlock vs physical interlock

The system may implement generic logical/domain interlocks, project/site restrictions and knowledge about manufacturer/device constraints.

It does not thereby verify physical locks, PLC/relay/controller logic, hardwired circuits, mechanical interlock health or actual absence/presence of voltage in the field.

## 7. Switching-form generation

Until a separately accepted safety case changes it:

> Any generated or assisted switching form/sequence is a draft requiring qualified human review and approval under applicable organizational procedure.

No UI label should imply automatic approval merely because software validation passes.

## 8. Normative coverage boundary

A passing machine validation means only:

```text
all implemented/applicable rules in selected profile passed or were resolved
```

It does not mean all possible requirements of Russian law/standards/site instructions are satisfied unless a formally defined coverage scope supports that claim.

## 9. Incomplete project model

When project data has unresolved equipment identity, connections/topology, state, applicable policy or source version, safety-sensitive functions degrade explicitly to `BLOCKED`, `UNKNOWN` or `REQUIRES_CONFIRMATION` rather than silently proceeding.

## 10. Imported data trust

CSV/XLSX, NPT, Visio and other imported information is not trusted merely because parsing succeeded. Safety-relevant import requires semantic validation and explicit ambiguity review.

## 11. Automated topology inference

Auto-detected/inferred topology is not equivalent to verified topology. Critical logic may require confirmed connections according to profile.

NPT `nodes` extraction remains research until systematically verified against known schemes.

## 12. AI/LLM boundary

AI may assist discovery, mapping suggestions, explanation, draft rule extraction, layout suggestions and documentation.

AI output is not normative authority and must not bypass deterministic safety validation/human review.

## 13. Real-time / SCADA integration

NPT signal semantics may be imported for engineering/compatibility. Current scope does not authorize online control.

Adding real-equipment commands requires a separate work item covering threat/safety model, authorization, command confirmation, communication quality, fail-safe behavior, audit, commissioning and applicable requirements.

## 14. Fail-safe defaults

- unknown rule source/version → do not claim validated baseline;
- missing critical policy → do not silently pass;
- parse failure → preserve original and avoid partial destructive write;
- failed save validation → retain/restore backup and show error;
- unknown switching prerequisite → do not auto-allow;
- mandatory/local policy conflict → report conflict and keep mandatory baseline effective.

## 15. Auditability

For critical validation/simulation outcomes retain enough to explain project/model revision, state baseline, sequence revision, rule/profile versions, relevant facts, result and permitted user confirmations.

## 16. Prohibited claims/features without separate acceptance

- `100% safe switching guaranteed`;
- `fully compliant with all ПУЭ/ПОТЭЭ/ПТЭЭС` without coverage evidence;
- automatic bypass of blocked mandatory rules;
- treating unknown state as safe;
- executing real switching based solely on model result;
- implying software replaces field verification, protective devices or physical interlocks.
