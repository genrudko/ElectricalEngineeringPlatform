# Normative / Compliance Architecture

Статус: canonical foundation document

## 1. Purpose

The product must convert applicable engineering and operational requirements into **traceable, versioned, explainable rules/profiles** without pretending that naming a document equals implementing it.

Compliance architecture serves:

1. graphics/scheme execution;
2. electrical/domain validation;
3. switching/TBP/interlocks.

## 2. Core principle

A production rule is not just code.

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

If a requirement cannot be automated reliably, it may remain an assisted-review/checklist/documentation rule rather than being omitted or falsely automated.

## 3. Normative source types

Conceptual source classes:

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

Actual legal force/applicability is determined per source/context; software classifications are not legal conclusions by themselves.

## 4. Source registry

Every source record should include:

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

For standards, record official status/current changes from national standards catalogue.

## 5. Rule registry

Rule IDs remain stable across implementation refactors.

Conceptual examples:

```text
SWITCH.RU.757.<section>.<semantic-name>
GRAPHICS.ESKD.2_702.<semantic-name>
SITE.<site>.<instruction>.<semantic-name>
```

Rule record includes source references, semantic requirement note, applicability predicate, implementation type, severity, non-weakening lock state, tests and review status.

## 6. Applicability resolution

Applicability may depend on organization role, equipment/object category, voltage class, installation type, operation type, work conditions, date/baseline edition, scheme/document type, site/project profile and manufacturer/model.

Compliance Core must explain why a rule is or is not applicable.

## 7. Baseline date and reproducibility

A project/released document should be verifiable against a recorded normative baseline date and resolved source/rule versions.

When registry updates, product must distinguish old release baseline from new current rules and expose migration/revalidation requirements.

## 8. Amendment/supersession lifecycle

Registry supports amendment chains and replacements/supersession.

A source update triggers registry update, impacted-rule identification, review, test updates, project/profile migration decision and release note where behavior changes.

## 9. Implementation classes

### MACHINE_BLOCKING

Deterministic condition can be checked and violation must block an operation/output under selected profile.

### MACHINE_ERROR / WARNING

Deterministic validation finding without universal operational block.

### ASSISTED_REVIEW

Software collects facts/checks partial conditions but qualified human judgement remains required.

### DOCUMENTATION_ONLY

Relevant requirement is surfaced but not machine-checkable in current model.

### OUT_OF_PRODUCT_BOUNDARY

Requirement concerns physical/process/organizational control outside software authority.

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

`UNKNOWN` is not PASS.

## 11. Rule layers and non-weakening

Resolved project policy is composed from baseline + local layers. Mandatory locked rules cannot be disabled or relaxed by local overlays.

Local rules may add checks, narrow permitted conditions, require extra steps, choose stricter limits, define local naming/templates and encode equipment-specific restrictions.

Attempted weakening produces `POLICY_CONFLICT`.

## 12. Graphics profiles

Standards-oriented scheme rules are handled as graphic/document profiles, not hard-coded renderer magic.

A profile may define/check symbol family, line styles/weights, connection semantics, labels/designations, orientation/transformation, sheet/document conventions and output rules.

## 13. Switching rules

Switching/TBP consumes the same rule registry but uses specialized evaluators over topology/state/sequence context.

Rule engine preserves source and explanation in diagnostics and audit/report output where required.

## 14. Local policy storage

Local/company/site policies are versioned assets with organization/site scope, source document ID/revision/date, effective period, mapped rules/templates and supersedes relation.

Do not encourage storing unauthorized full proprietary manuals in GitHub.

## 15. Review workflow

```text
source identified
→ current edition verified
→ relevant provision scoped
→ semantic rule proposal
→ implementation class chosen
→ domain/legal/normative review as appropriate
→ tests/evidence
→ accepted profile release
```

LLM extraction can assist discovery/drafting but is not normative authority.

## 16. No fake full-compliance claims

Until a defined coverage matrix is complete, use scoped claims such as `ГОСТ 2.702 profile: implemented rules X/Y/Z` rather than `fully compliant with all ГОСТ/ПУЭ/ПТЭЭС`.

## 17. Testing

Compliance tests are indexed by rule ID and source version and include positive, negative, boundary, NOT_APPLICABLE, UNKNOWN, stricter-overlay, weakening-conflict and amendment-migration scenarios where relevant.

## 18. Update monitoring

Future automation may monitor official registries for changes, but detected changes never automatically alter production rule semantics without human/domain review.
