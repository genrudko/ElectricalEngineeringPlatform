# Архитектура Switching и Interlocks

Статус: канонический Foundation-документ

## 1. Назначение

Switching/TBP module использует shared electrical model для подготовки, simulation и validation switching sequences, а также для generation/checking switching-form documents.

Это **clean-sheet implementation**. Старый TBP codebase/configuration/rules не являются migration inputs или normative authority.

Module не является real-equipment control system.

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

Operation является semantic, а не просто текстовой строкой.

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

Operation families вводятся только по мере подтверждения real product/normative use cases.

## 4. Sequence model

`SwitchingSequence` поддерживает:

- manual authoring;
- template-based generation;
- future assisted generation;
- insert/remove/reorder с revalidation;
- per-step simulation;
- whole-sequence validation;
- document rendering/export;
- rule provenance;
- explicit unresolved conditions.

Reordering одного step может invalid downstream assumptions, поэтому validation учитывает state после каждого preceding step.

## 5. State transition

Перед simulated operation:

1. verify target identity/type;
2. evaluate applicable mandatory rules;
3. evaluate local/manufacturer constraints;
4. определить, известны ли required state/quality facts;
5. calculate transition;
6. recalculate affected topology;
7. сохранить explanation/evidence.

Simulation state никогда не записывается молча как observed real-world state.

## 6. Interlock engine

Interlock engine оценивает predicates над:

- equipment type/state;
- topology;
- energization knowledge;
- dependent devices;
- configured site/project relationships;
- rule/profile data.

Иллюстративный semantic shape:

```text
operation: CLOSE_EARTHING_SWITCH(target)
requires:
  target_section energization == DEENERGIZED_PROVEN
  AND no known closed energized path
  AND conflicting devices satisfy required states
```

`UNKNOWN` никогда не кодируется как success.

## 7. Explainability

Каждый block/warning/unknown result должен по возможности содержать:

- operation;
- equipment/section;
- violated/unsatisfied rule ID;
- normative/local source;
- facts/states used;
- missing facts;
- topology dependency/path where relevant;
- required confirmation/action.

Opaque сообщение `Operation not allowed` недостаточно.

## 8. Normative rules и physical interlocks

Явно разделяются три слоя:

1. normative/logical operational rules, реализованные software;
2. site/project operational restrictions/instructions;
3. physical/hardwired/protection/controller interlocks.

Software model не заявляет, что заменяет или верифицирует physical implementation, если отдельный engineering workflow этого не доказывает.

## 9. Human review boundary

Foundation safety baseline:

> Generated forms/sequences являются draft/decision-support output и требуют qualified human review до operational use.

Изменение этой границы требует отдельного safety/legal/normative acceptance и не может выполняться обычной configuration.

## 10. Normative integration

Switching module получает resolved rules из Compliance Core вместе с:

- source/version/effective dates;
- applicability;
- authority/severity;
- local-overlay resolution;
- test/evidence status.

Applicable government/sector sources re-verify online по authoritative sources при реализации соответствующего compliance work.

Old TBP implementation не используется как source of normative truth.

## 11. Local instructions и enterprise policy

Продукт должен поддерживать site-specific rules, которые могут добавлять:

- extra checks;
- stricter sequences;
- equipment-specific restrictions;
- local operational names;
- mandatory intermediate steps;
- approved wording;
- special conditions для normal/repair schemes.

Они не могут suppress applicable locked mandatory requirement. Conflict — explicit error.

Shared development не требует real internal enterprise/site instructions. Mechanics разрабатываются на synthetic/cleared policy fixtures. Реальные instructions остаются внутри authorized deployment environment и formalized в local policy packages там.

## 12. Operational names vs identity

Display/dispatch names — attributes/views над stable equipment identity.

Это позволяет использовать local names, aliases, imported/NPT IDs и equipment renaming без разрушения historical sequence identity.

## 13. Topology dependency

Module снижает confidence или блокирует вывод, когда:

- topology incomplete;
- connections unresolved;
- equipment state `UNKNOWN`;
- signal quality bad/uncertain;
- required policy data missing.

Missing evidence никогда не становится pass.

## 14. Scheme integration

Target UX:

```text
выбрать equipment на operational scheme
→ выбрать semantic operation
→ validate
→ append to sequence
→ simulate
→ update visual state/topology
→ continue
```

Textual form и visual simulation синхронизируются через semantic operation/entity IDs, а не text parsing.

## 15. Document generation

Switching-form document — output/projection semantic sequence + organization/profile requirements.

Formatting/templates могут различаться по enterprise/site без изменения semantic operation identity.

Blocked/unresolved sequence нельзя молча render как approved/ready.

## 16. Testing strategy

- unit tests для predicates/rule resolution;
- scenario tests на known schemes/states;
- invariant/property tests where useful;
- synthetic/cleared regression scenarios;
- normative rule tests, привязанные к rule IDs/source versions;
- negative tests для `UNKNOWN`/incomplete topology;
- sequence revalidation после reorder/change;
- local-policy tests для stricter overlays и weakening rejection.

Old TBP tests не становятся canonical regression fixtures только потому, что уже существуют.

## 17. Prohibited early features

- direct equipment command transmission;
- bypass human review;
- automatic approval/signature;
- claim, что simulation гарантирует physical safety;
- local switch, отключающий mandatory rules;
- black-box AI-generated sequence без deterministic validation/provenance;
- migration old TBP rules/configuration без нового explicit owner decision.