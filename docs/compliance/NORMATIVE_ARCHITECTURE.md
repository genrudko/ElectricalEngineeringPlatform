# Архитектура Normative / Compliance

Статус: канонический Foundation-документ

## 1. Назначение

Продукт должен превращать применимые engineering и operational requirements в **traceable, versioned, explainable rules/profiles**, не создавая ложное впечатление, что упоминание документа означает его полную реализацию.

Compliance architecture обслуживает:

1. graphics/scheme execution;
2. electrical/domain validation;
3. Switching/TBP/interlocks.

## 2. Core principle

Production rule — это не просто code.

```text
Rule
├── identity
├── authority / layer
├── source provenance
├── edition / amendment chain
├── effective period
├── applicability
├── semantic requirement
├── machine-checkable implementation (optional)
├── severity / blocking policy
├── explanation
├── test/evidence
└── exceptions/limitations
```

Если requirement нельзя надёжно автоматизировать, он остаётся assisted-review/checklist/documentation rule, а не удаляется и не получает fake automation.

## 3. Типы normative sources

Conceptual classes:

```text
FEDERAL_MANDATORY_RULE
SECTOR_MANDATORY_RULE
TECHNICAL_REGULATION
STANDARD_GOST_ESKD
OTHER_STANDARD_PROFILE
MANUFACTURER_REQUIREMENT
ENTERPRISE_STANDARD_OR_INSTRUCTION
SITE_OBJECT_INSTRUCTION
PROJECT_REQUIREMENT
RECOMMENDATION_GUIDANCE
```

Actual legal force/applicability определяется для конкретного source/context; software classification сама по себе не является legal conclusion.

## 4. Source registry

Каждый source record должен содержать минимум:

```text
source_id
issuer
identifier
full_title
source_type
publication/registration reference
edition/amendment identifiers
effective_from
effective_until
status
scope/applicability notes
last_verified_at
verification_source
```

Для standards official status/current changes проверяются по national standards catalogue.

## 5. Rule registry

Rule IDs сохраняют стабильность при implementation refactors.

Conceptual examples:

```text
SWITCH.RU.757.<section>.<semantic-name>
GRAPHICS.ESKD.2_702.<semantic-name>
SITE.<site>.<instruction>.<semantic-name>
```

Rule record содержит source references, semantic requirement, applicability predicate, implementation type, severity, non-weakening lock state, tests и review status.

## 6. Applicability resolution

Applicability может зависеть от:

- organization role;
- equipment/object category;
- voltage class;
- installation type;
- operation type;
- work conditions;
- date/baseline edition;
- scheme/document type;
- site/project profile;
- manufacturer/model.

Compliance Core должен объяснять, почему rule применяется или не применяется.

## 7. Baseline date и reproducibility

Project/released document должен быть проверяемым против recorded normative baseline date и resolved source/rule versions.

При registry update продукт различает:

- historical release baseline;
- current rules;
- migration/revalidation requirement.

## 8. Amendment/supersession lifecycle

Registry поддерживает amendment chains и replacements/supersession.

Source update запускает:

- registry update;
- impacted-rule identification;
- review;
- test updates;
- project/profile migration decision;
- release note, если behavior меняется.

## 9. Implementation classes

### `MACHINE_BLOCKING`

Deterministic condition проверяется программно, а violation блокирует operation/output в выбранном profile.

### `MACHINE_ERROR` / `WARNING`

Deterministic validation finding без universal operational block.

### `ASSISTED_REVIEW`

Software собирает facts/частично проверяет condition, но qualified human judgement остаётся required.

### `DOCUMENTATION_ONLY`

Relevant requirement показывается пользователю, но пока не machine-checkable в current model.

### `OUT_OF_PRODUCT_BOUNDARY`

Requirement относится к physical/process/organizational control вне authority продукта.

## 10. Rule evaluation result

```text
rule_id
status: PASS | FAIL | UNKNOWN | NOT_APPLICABLE | REQUIRES_REVIEW
severity
affected_entity/operation
facts_used
missing/unknown facts
source reference
explanation
suggested resolution
```

`UNKNOWN` не является PASS.

## 11. Rule layers и non-weakening

Resolved project policy складывается из baseline + local layers.

Mandatory locked rules нельзя отключить или ослабить local overlay.

Local rules могут:

- добавлять checks;
- сужать permitted conditions;
- требовать extra steps;
- выбирать stricter limits;
- задавать local naming/templates;
- кодировать equipment-specific restrictions.

Attempted weakening даёт `POLICY_CONFLICT`.

## 12. Graphics profiles

Standards-oriented scheme rules оформляются как graphic/document profiles, а не hard-coded renderer magic.

Profile может определять/check:

- symbol family;
- line styles/weights;
- connection semantics;
- labels/designations;
- orientation/transformation;
- sheet/document conventions;
- output rules.

## 13. Switching rules

Switching/TBP использует тот же rule registry, но specialized evaluators работают над topology/state/sequence context.

Rule engine сохраняет source и explanation в diagnostics/audit/report output where required.

## 14. Local policy storage

Local/company/site policies — versioned assets с organization/site scope, source document ID/revision/date, effective period, mapped rules/templates и supersedes relation.

Shared development использует synthetic/cleared examples. Internal enterprise/site source documents не являются общим development corpus.

## 15. Review workflow

```text
source identified
→ current edition verified
→ relevant provision scoped
→ semantic rule proposal
→ implementation class chosen
→ domain/legal/normative review where appropriate
→ tests/evidence
→ accepted profile release
```

LLM может помогать discovery/drafting, но не является normative authority.

## 16. Запрет fake full-compliance claims

Пока coverage matrix не завершена, использовать scoped claims, например:

```text
ГОСТ 2.702 profile: implemented rules X/Y/Z
```

а не:

```text
полностью соответствует всем ГОСТ/ПУЭ/ПТЭЭС
```

## 17. Testing

Compliance tests indexed by rule ID/source version и покрывают where relevant:

- positive;
- negative;
- boundary;
- NOT_APPLICABLE;
- UNKNOWN;
- stricter-overlay;
- weakening-conflict;
- amendment-migration scenarios.

## 18. Source update monitoring

Future automation может monitor official registries, но обнаруженное изменение никогда не переписывает production rule semantics автоматически без human/domain review.