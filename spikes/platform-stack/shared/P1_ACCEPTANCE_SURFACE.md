# P1 acceptance surface — remaining Avalonia candidate

This document defines the frozen functional surface for P1. It was originally candidate-neutral and remains unchanged in substance after Qt withdrawal.

Owner amendment: `docs/development/PLATFORM_STACK_SPIKE_OWNER_AMENDMENT_2026-08-19.md`.

## Composition

The remaining Avalonia candidate implements one light-theme dense engineering desktop shell:

```text
Menu
Command toolbar
Equipment Tree | Document workspace + tabs | Properties Inspector
Diagnostics / status
```

The logical content and initial state come only from `shared/fixtures/p1-shell-fixture.json`.

## Mandatory commands

- File / Открыть… (`Ctrl+O`)
- File / Сохранить (`Ctrl+S`)
- File / Выход
- Edit / Отменить (`Ctrl+Z`)
- Edit / Повторить (`Ctrl+Y`)
- View / Панель оборудования
- View / Свойства
- View / Диагностика
- Help / О программе

Open/Save operate only on harmless demo state in P1. No production project serialization is defined.

## Mandatory interaction

1. App starts and loads the deterministic fixture.
2. Equipment Tree is visible with the frozen initial expansion state.
3. Selecting an equipment leaf updates the Properties Inspector demo state.
4. Document tab switching works.
5. View commands toggle the three corresponding surfaces without corrupting layout.
6. Validation states render: normal, warning, error, read-only and `UNKNOWN`.
7. Shortcut mappings exist without obvious conflicts.
8. UI Gallery renders without exception.

## UI Gallery states

The candidate must show the frozen logical examples:

- primary/default, secondary and disabled button;
- text and numeric input;
- combo/select;
- checkbox;
- radio choice;
- editable and read-only property rows;
- warning and error validation;
- `UNKNOWN` state;
- selected/unselected tree item;
- expanded/collapsed tree node;
- tabs;
- status badges;
- info/warning/error notification;
- long Russian label;
- multiline error text.

## Typography

Pinned fixture: Noto Sans 2.015, Regular 400 and SemiBold 600. `shared/fixtures/font-fixture.json` contains the exact binary SHA-256 values used by accepted evidence.

No silent system-font fallback is accepted for the mandatory visual fixture.

## Required visual evidence names

```text
01-shell-default
02-ui-gallery
03-properties-validation
04-long-russian-text
```

The frozen logical content and requested viewport remain the P1 visual contract. Pixel-perfect styling against a withdrawn second framework is not an acceptance requirement.

## Decision boundary

Passing P1 means only that Avalonia may proceed to the next viability gate. It is not a stack-selection decision.

If a mandatory P1 requirement cannot be met without violating the active hard constraints, P1 fails and the spike result becomes `NO_STACK_PASSES_CURRENT_CONTRACT` unless the owner explicitly changes the contract.

## Scope boundary

P1 explicitly does not implement semantic canvas, 100k tables/Import Review, detachable multi-window behavior, production project/domain model, final packaging or final stack selection.
