# Local Policy Overlays

Статус: canonical foundation document

## 1. Purpose

Different enterprises, sites and equipment fleets may have legitimate additional restrictions, instructions, naming conventions and approved workflows.

The product must support this **without allowing local configuration to weaken an applicable mandatory requirement**.

Important development-boundary decision:

> Internal enterprise/site instructions are not required shared development inputs and should not be uploaded into GitHub, ChatGPT project storage or the common VPS corpus. The capability is developed using synthetic/cleared examples and applied inside the authorized deployment environment.

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

This is not simple `last write wins`.

## 3. Non-weakening invariant

For a locked mandatory rule `R`:

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

Attempted weakening produces a configuration error and mandatory baseline remains effective.

## 4. Policy package

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

Exact serialization is PENDING.

The package contains the formalized local policy needed by the software. It does not require the original internal instruction document to be copied into the shared development repository/corpus.

## 5. Applicability

A local package may target enterprise, branch/site/object, voltage level, equipment type, manufacturer/model, specific bay/installation class, switching operation category or document/scheme profile.

Do not encode site-name checks throughout application code.

## 6. Manufacturer/equipment constraints

Examples:

- sequence restrictions from operating manual;
- mechanical/electrical interlock dependencies known for the model;
- allowed operating states/ratings;
- required delay/check before another action;
- special maintenance/inspection prerequisites.

Require source manual revision/model applicability and distinguish from government norms/company policy.

Public manufacturer sources may be used during development where available. Restricted manuals stay in the authorized local/deployment environment unless redistribution is permitted.

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

Source layer must be shown to user.

Shared development verifies these mechanics with synthetic/cleared rules, not real confidential instructions.

## 8. Rule composition

Illustrative limit rule:

```text
baseline: X <= 100
enterprise: X <= 80
resolved: X <= 80
```

Attempted site `X <= 120` produces `POLICY_CONFLICT_WEAKENING`.

For boolean prerequisites:

```text
mandatory requires A
local requires B
resolved requires A AND B
```

Where rules are not mathematically comparable, require explicit composition semantics or human review rather than guessing.

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

Diagnostics identify both rules/sources.

## 10. Policy authoring UX

Target editor should support source metadata, applicability selectors, typed parameters, explanation, comparison with inherited baseline, preview of resolved policy, conflict validation, test scenarios and revision history/diff.

Early implementation may use structured local policy files plus validation before a full UI exists.

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

Record which package versions were active for released schemes/switching forms/project baseline.

## 12. Project portability

A project references required policy package identities/versions and reports available/resolved, missing, newer version, incompatible or superseded baseline.

If required safety/normative policy is missing, do not silently use defaults and show green validation.

## 13. Development vs deployment storage

### Shared development

May contain:

- policy schema;
- synthetic/cleared example packages;
- non-weakening tests;
- public manufacturer examples where redistribution is clear.

Must not require:

- real enterprise instructions;
- site operational instructions;
- confidential approval documents.

### Real deployment

Authorized enterprise/site administrators may create/install policy packages from their internal documentation inside the controlled environment.

Original internal source documents remain governed by the enterprise and do not need to leave that environment.

## 14. Deployment profiles

Prefer:

```text
same executable
+ project/site policy package
+ enabled modules
+ local symbol/template package if authorized
```

Avoid per-site product forks.

## 15. Tests

Every operational local policy package should have schema validation, source metadata validation, non-weakening check, positive/negative scenarios for critical rules, compatibility with declared baseline, effective-date checks and resolution snapshot test.

The product/framework tests use synthetic packages. Site-specific acceptance tests may run locally against the actual deployed policy package without publishing internal source material.

## 16. Anti-goals

Do not implement unrestricted scripting from policy files, `disableMandatoryRule=true`, arbitrary site-specific code branches where data suffices, opaque precedence based on file load order, or a development process that requires uploading confidential enterprise instructions to the project.