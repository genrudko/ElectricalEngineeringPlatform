# Switching and Interlock Architecture

Статус: canonical foundation document

## 1. Purpose

Switching/TBP module uses the shared electrical model to prepare, simulate and validate switching sequences and to generate/check switching-form documents.

It is a **clean-sheet implementation**. The old TBP codebase/configuration/rules are not migration inputs or normative authority.

It is not a real-equipment control system.

## 2. Core flow

```text
ElectricalProject
+ current/baseline state
+ applicable normative/local policies
        ↓
SwitchingOperation proposal
        ↓
Rule / interlock evaluation
        ↓
ALLOW / BLOCK / UNKNOWN / REQUIRES_CONFIRMATION
        ↓
if simulated apply:
StateTransition
        ↓
Topology recalculation
        ↓
next simulated state
        ↓
SwitchingSequence
```

## 3. SwitchingOperation

Operation is semantic, not merely a text line.

Conceptual fields:

- stable operation ID;
- operation type;
- target equipment/terminal/network object;
- intended state/action;
- source (manual/generated/template);
- rule evaluation result;
- generated wording/document fragment;
- sequence position;
- simulation before/after references.

Possible operation families are introduced only as real product/normative evidence justifies them.

## 4. Sequence model

SwitchingSequence supports manual authoring, template-based generation, future assisted generation, insert/remove/reorder with revalidation, per-step simulation, whole-sequence validation, document rendering/export, rule provenance and explicit unresolved conditions.

Reordering one step can invalidate downstream assumptions; sequence validation must account for state after each preceding step.

## 5. State transition

Before applying a simulated operation:

1. verify target identity/type;
2. evaluate applicable mandatory rules;
3. evaluate local/manufacturer constraints;
4. determine whether required state/quality evidence is known;
5. calculate transition;
6. recalculate affected topology;
7. store explanation/evidence.

Simulation state is never silently written back as observed real-world state.

## 6. Interlock engine

Interlock engine evaluates predicates over equipment type/state, topology, energization knowledge, dependent devices, configured site/project relationships and rule/profile data.

Illustrative semantic shape only:

```text
operation: CLOSE_EARTHING_SWITCH(target)
requires:
  target_section energization == DEENERGIZED_PROVEN
  AND no known closed energized path
  AND conflicting devices satisfy required states
```

The engine must not encode `UNKNOWN` as success.

## 7. Explainability

Every block/warning/unknown result should include operation, equipment/section, violated/unsatisfied rule ID, normative/local source, facts/states used, missing facts, topology dependency/path where relevant and what confirmation/action is needed.

Avoid opaque `Operation not allowed` messages.

## 8. Normative rules vs physical interlocks

Keep three layers explicit:

1. normative/logical operational rules implemented by software;
2. site/project operational interlocks/instructions;
3. physical/hardwired/protection/controller interlocks.

The software model does not claim to replace or verify physical implementation unless a separate engineering workflow explicitly does so.

## 9. Human review boundary

Foundation safety baseline:

> generated forms/sequences are drafts/decision support requiring qualified human review before operational use.

Changing this boundary requires separate safety/legal/normative acceptance and cannot be relaxed by ordinary configuration.

## 10. Normative integration

Switching module consumes resolved rules from Compliance Core with source/version/effective dates, applicability, authority/severity, local-overlay resolution and test/evidence status.

Applicable government/sector sources are acquired/re-verified online from authoritative sources as the relevant compliance work is implemented. The old TBP implementation is not used as a source of normative truth.

## 11. Local instructions and enterprise policy

The product must support site-specific rules that add checks, stricter sequences, equipment-specific restrictions, local operational names, mandatory intermediate steps, approved wording and special conditions for normal/repair schemes.

They cannot suppress an applicable locked mandatory requirement. Conflict is an explicit error.

Shared development does **not** require real internal enterprise/site instructions. These mechanics are developed against synthetic/cleared policy fixtures. Real internal instructions remain inside the authorized deployment environment and are formalized into local policy packages there.

## 12. Operational names vs identity

Display/dispatch names are attributes/views over stable equipment identity. This supports local names, aliases, imported/NPT IDs and equipment renaming without breaking historical sequence identity.

## 13. Topology dependency

The module reports degraded confidence when topology is incomplete, connections unresolved, equipment state UNKNOWN, signal quality bad/uncertain or required policy data missing.

Missing evidence never becomes a pass.

## 14. Scheme integration

Target UX:

```text
select equipment on operational scheme
→ choose semantic operation
→ validate
→ append to sequence
→ simulate
→ update visual state/topology
→ continue
```

Textual form and visual simulation stay synchronized through semantic operation/entity IDs, not text parsing.

## 15. Document generation

Switching-form document is an output/projection of semantic sequence plus organization/profile requirements.

Formatting/templates may vary by enterprise/site without changing semantic operation identity.

A blocked/unresolved sequence must never be silently rendered as approved/ready.

## 16. Testing strategy

- unit tests for predicates/rule resolution;
- scenario tests on known schemes/states;
- invariant/property tests where useful;
- synthetic/cleared regression scenarios;
- normative rule tests bound to rule IDs/source versions;
- negative tests for UNKNOWN/incomplete topology;
- sequence revalidation after reorder/change;
- local-policy tests proving stricter overlays and rejection of weakening attempts.

Old TBP tests are not accepted as canonical regression fixtures merely because they already exist.

## 17. Prohibited early features

- direct equipment command transmission;
- bypass of human review;
- automatic approval/signature;
- assertion that simulation guarantees physical safety;
- local switch that disables mandatory rules;
- black-box AI-generated sequence accepted without deterministic validation/provenance;
- migration of old TBP rules/configuration without a new explicit owner decision.