# Local Policy Overlays

Статус: канонический Foundation-документ

## 1. Назначение

Разные предприятия, объекты и equipment fleets могут иметь дополнительные ограничения, naming conventions и approved workflows.

Продукт должен поддерживать это **без возможности ослабить applicable mandatory requirement локальной configuration**.

Важная development-boundary договорённость:

> Internal enterprise/site instructions не являются required shared development inputs и не должны загружаться в GitHub, ChatGPT Project storage или common VPS corpus. Capability разрабатывается на synthetic/cleared examples и применяется внутри authorized deployment environment.

## 2. Layer model

```text
Mandatory regulatory baseline
        ↓
Applicable standards/profile baseline
        ↓
Manufacturer/equipment constraints
        ↓
Enterprise policy/instruction
        ↓
Site/object instruction
        ↓
Project-specific policy
```

Это не `last write wins`.

## 3. Non-weakening invariant

Для locked mandatory rule `R`:

```text
local overlay may:
  add stricter condition
  add extra prerequisite
  reduce allowed range
  add required check/step
  add local wording/template

local overlay may NOT:
  disable R
  enlarge allowed range beyond R
  convert blocking requirement into warning
  treat UNKNOWN as PASS where R requires proof
  suppress mandatory step solely by configuration
```

Attempted weakening создаёт configuration error, mandatory baseline остаётся effective.

## 4. PolicyPackage

```text
PolicyPackage
├── policy_id
├── name
├── layer
├── organization/site/equipment scope
├── source document metadata
├── revision/effective dates
├── approval metadata where appropriate
├── parent/dependencies
├── rules
├── templates/naming conventions
├── mappings
└── provenance metadata
```

Exact serialization остаётся PENDING.

Package содержит formalized local policy, необходимую software. Original internal instruction не требуется копировать в shared development repository/corpus.

## 5. Applicability

Local package может target:

- enterprise;
- branch/site/object;
- voltage level;
- equipment type;
- manufacturer/model;
- specific bay/installation class;
- switching operation category;
- document/scheme profile.

Site-name checks не должны размазываться по application code.

## 6. Manufacturer/equipment constraints

Examples:

- sequence restrictions из operating manual;
- mechanical/electrical interlock dependencies известные для model;
- allowed operating states/ratings;
- required delay/check перед другой action;
- special maintenance/inspection prerequisites.

Требуется source manual revision/model applicability и separation от government norms/company policy.

Public manufacturer sources могут использоваться during development. Restricted manuals остаются внутри authorized local/deployment environment, если redistribution не разрешена.

## 7. Enterprise/site rules

Examples:

- stricter switching sequence;
- extra position confirmation;
- additional dispatcher/shift checks;
- local operational names;
- approved switching-form wording;
- forbidden operation combinations specific to installed arrangement;
- requirement to use particular views/documents;
- stricter scheme-release/approval process.

Source layer должен быть visible пользователю.

Shared development проверяет mechanics на synthetic/cleared rules, а не real confidential instructions.

## 8. Rule composition

Illustrative limit rule:

```text
baseline: X <= 100
enterprise: X <= 80
resolved: X <= 80
```

Attempted site rule `X <= 120` создаёт `POLICY_CONFLICT_WEAKENING`.

Для boolean prerequisites:

```text
mandatory requires A
local requires B
resolved requires A AND B
```

Если rules нельзя сравнить формально, требуется explicit composition semantics или human review, а не guess.

## 9. Conflict classes

```text
WEAKENING_ATTEMPT
CONTRADICTORY_REQUIREMENTS
AMBIGUOUS_PRECEDENCE
MISSING_REQUIRED_SOURCE
OUT_OF_SCOPE_REFERENCE
EXPIRED_POLICY
UNRESOLVED_APPLICABILITY
```

Diagnostics идентифицируют обе conflicting rules/sources.

## 10. Policy authoring UX

Target editor должен поддерживать:

- source metadata;
- applicability selectors;
- typed parameters;
- explanation;
- comparison с inherited baseline;
- preview resolved policy;
- conflict validation;
- test scenarios;
- revision history/diff.

Early implementation может использовать structured local policy files + validation до появления full UI.

## 11. Policy lifecycle

Suggested states:

```text
DRAFT
UNDER_REVIEW
APPROVED
ACTIVE
SUPERSEDED
EXPIRED
```

Нужно фиксировать package versions, active для released schemes/switching forms/project baseline.

## 12. Project portability

Project references required policy package identities/versions и показывает:

- available/resolved;
- missing;
- newer version;
- incompatible;
- superseded baseline.

Если required safety/normative policy отсутствует, нельзя silently use defaults и показывать green validation.

## 13. Development vs deployment storage

### Shared development

Может содержать:

- policy schema;
- synthetic/cleared example packages;
- non-weakening tests;
- public manufacturer examples where redistribution is clear.

Не должен требовать:

- real enterprise instructions;
- site operational instructions;
- confidential approval documents.

### Real deployment

Authorized enterprise/site administrators могут создавать/install policy packages из internal documentation внутри controlled environment.

Original internal source documents остаются governed предприятием и не должны покидать его environment.

## 14. Deployment profiles

Предпочтительный pattern:

```text
same executable
+ project/site policy package
+ enabled modules
+ local symbol/template package if authorized
```

Избегать per-site product forks.

## 15. Tests

Каждый operational local policy package должен иметь:

- schema validation;
- source metadata validation;
- non-weakening check;
- positive/negative scenarios для critical rules;
- compatibility с declared baseline;
- effective-date checks;
- resolution snapshot test.

Product/framework tests используют synthetic packages. Site-specific acceptance tests могут выполняться locally против actual deployed policy package без publishing internal source material.

## 16. Anti-goals

Не реализовывать:

- unrestricted scripting из policy files;
- `disableMandatoryRule=true`;
- arbitrary site-specific code branches where data suffices;
- opaque precedence based on file load order;
- development process, требующий загрузки confidential enterprise instructions в проект.