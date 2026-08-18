# ADR 0003 — Layered Normative Policy и Non-Weakening

Статус: **ACCEPTED по owner direction / зафиксирован в `UNIFIED-FOUNDATION-001`**  
Дата: 2026-08-18

## Контекст

Russian energy-sector operational requirements приходят из нескольких legal/normative sources.

Реальные sites дополнительно могут иметь manufacturer manuals, enterprise standards и local instructions, которые добавляют или ужесточают требования.

Simple `override=true` model может случайно ослабить applicable mandatory baseline.

Одновременно простой linear precedence тоже недостаточен: более specific local rule не обязательно имеет большую normative authority, а mandatory source может явно делегировать локальный выбор параметра или procedure внутри заданных bounds.

## Решение

Реализовать versioned Compliance Core с explicit source provenance, applicability и layered policy resolution.

```text
mandatory regulatory baseline
→ applicable standard/profile baseline
→ manufacturer/equipment constraints
→ enterprise policy
→ site/object policy
→ project policy
```

Эта последовательность описывает composition layers, но не является `last-write-wins` или безусловной hierarchy override.

Resolution отдельно учитывает:

- authority / mandatory status;
- applicability / scope;
- specificity;
- effective date/version;
- explicit delegation / permitted local choice;
- comparability of constraints.

Lower/local layers могут добавлять/ужесточать requirements, но не могут disable/relax applicable locked mandatory rule.

Если mandatory source явно делегирует локальный выбор внутри defined bounds, корректная local configuration внутри этих bounds не является weakening.

Attempted weakening или выход за delegated bounds является policy conflict/error.

Если authority/applicability/composition нельзя определить надёжно, требуется explicit review/conflict result вместо guessed precedence.

Каждое production rule идентифицирует source/version/scope и machine/review behavior.

Internal enterprise/site instructions не являются shared development inputs; механизм Local Policy развивается на synthetic/cleared examples и применяется внутри authorized deployment environment.

## Последствия

- normative documents регистрируются с editions/amendments/effective dates;
- projects могут snapshot normative baseline date/profile;
- local policy packages versioned/separately deployable;
- diagnostics показывают source/conflict/authority/delegation context;
- switching forms сохраняют mandatory human-review boundary;
- graphics profiles различают ГОСТ/ЕСКД requirements, enterprise conventions и layout heuristics;
- shared development не требует публикации confidential internal instructions;
- policy engine не может трактовать specificity как автоматическое право override mandatory baseline;
- delegated local choices тестируются на allowed bounds так же, как non-weakening conflicts.

## Отклонённые альтернативы

- **Hard-code rules throughout modules** — poor provenance/update/conflict handling.
- **Last-file-wins configuration** — unsafe authority model.
- **Pure linear precedence by layer** — путает specificity с normative authority и не моделирует explicit delegation.
- **Treat local instructions as equal to mandatory rules** — source authority/applicability различаются.
- **Encode every rule immediately** — fake completeness хуже scoped coverage.