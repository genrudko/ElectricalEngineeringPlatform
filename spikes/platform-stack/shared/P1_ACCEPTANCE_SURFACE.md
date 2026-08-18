# P1 candidate-neutral acceptance surface

This document is the stack-neutral functional contract for both P1 applications. It is fixed before candidate-specific implementation.

## Composition

Both candidates implement one light-theme dense engineering desktop shell:

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

Both candidates show the same logical examples:

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

Pinned fixture: Noto Sans 2.015, Regular 400 and SemiBold 600. `shared/fixtures/font-fixture.json` must receive exact binary SHA-256 values before first accepted visual evidence.

No silent system-font fallback is accepted for the mandatory visual fixture.

## Required visual evidence names

```text
01-shell-default
02-ui-gallery
03-properties-validation
04-long-russian-text
```

Logical content and requested viewport size are kept equal. Pixel-perfect styling is not an acceptance requirement.

## Scope boundary

P1 explicitly does not implement semantic canvas, 100k tables/Import Review, detachable multi-window behavior, production project/domain model, final packaging, weighted scoring or stack selection.
