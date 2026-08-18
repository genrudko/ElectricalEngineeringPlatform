# UI Core Architecture

Статус: canonical foundation document

## 1. Why UI Core is architecture

The product must not become a technically correct engineering engine hidden behind an archaic or exhausting interface.

UI Core is a first-class shared subsystem with the same design seriousness as Domain Core.

Target UX:

> modern, dense, professional desktop engineering UI for long work sessions, large projects, keyboard+mouse and multiple monitors.

Reject both legacy/MS-DOS/early-Win32 usability and sparse mobile/tablet desktop composition.

## 2. UI Core ownership

```text
UI Core
├── Application Shell
├── Workspace / Documents
├── Multi-window
├── Design System / Themes
├── Commands / Shortcuts
├── Selection infrastructure
├── Property Inspector framework
├── Tree/List/Table infrastructure
├── Search / Command Palette
├── Dialogs / Notifications
├── Status / Diagnostics surfaces
├── Canvas framework primitives
├── Clipboard / Drag-drop UX contracts
├── HiDPI / mixed-DPI handling
├── Accessibility / keyboard focus
└── UI Gallery / visual test fixtures
```

UI Core does not own switching rules, NPT semantics or equipment-specific validation.

## 3. Application shell

The shell must support project/workspace identity, main navigation without wasting working area, document tabs, split document regions where useful, detachable documents/windows, persistent tool panels, status/diagnostics, global search/command access and module-contributed views/actions.

Avoid copying Office Ribbon or IDE docking blindly.

## 4. Workspace and multi-monitor

Multi-monitor is a first-class acceptance scenario.

```text
Monitor 1 — main electrical scheme
Monitor 2 — equipment/properties/search
Monitor 3 — switching/TBP sequence
Monitor 4 — table/NPT/diagnostics/secondary view
```

Persist open views, window positions/sizes, monitor assignment with fallback, splits/tabs, visible panels and sizing/order. A stale/corrupt workspace must have safe/default recovery.

## 5. Design system

Define typography, spacing, density, separators, semantic colors, focus/hover/pressed/selected/disabled/error/warning states, icons, table/tree density, panel headers, dialogs and theme behavior.

Do not encode important engineering meaning only by color.

## 6. UI Gallery

Create early and keep continuously runnable.

Gallery contains shared controls/states without full project/runtime: buttons/toolbars, editors, property groups, tabs, trees, virtual tables, search results, dialogs, notifications, validation messages, context menus, command palette, canvas handles, symbol state samples and empty/loading/error states.

Purposes: rapid iteration, screenshot regression, theme/HiDPI checks, agent/developer inspection and prevention of per-module UI reinvention.

## 7. Property Inspector

Shared Inspector framework supports typed descriptors/editors without making UI Core know business semantics.

Needs single/multi-selection, mixed-value state, validation feedback, undo/redo integration, searchable groups and keyboard-friendly editing.

Modules may contribute specialized editors such as equipment state/property, signal/KKS chooser, NPT typed `scdCommand`, compliance/source selector.

## 8. Trees and tables

Electrical projects can contain tens/hundreds of thousands of listable entities. Use virtualization/incremental loading where appropriate.

Requirements: stable selection, fast sort/filter/search, keyboard navigation, column presets, copy/export and clear diagnostics.

Platform Spike includes a 100k-row representative table.

## 9. Command system

Actions should be addressable through a shared command registry:

```text
command ID
label
icon
shortcut
availability/canExecute
execution
context
```

This enables consistent toolbar/context-menu/command-palette/shortcut behavior.

## 10. Canvas framework

UI Core owns shared viewport interaction infrastructure while Scheme/NPT modules own rendering semantics.

Shared concerns may include viewport transform, zoom/pan, hit-testing contract, selection model, marquee selection, pointer capture, snapping/guides, overlays/handles, keyboard navigation and performance instrumentation.

Do not implement one heavyweight desktop control per visual primitive without benchmark evidence.

## 11. Scheme editing UX principles

- selection predictable/reversible;
- reconnecting a semantic terminal visibly differs from moving line geometry;
- destructive operations show effect before commit;
- auto-layout never silently destroys locked/manual placement;
- validation locates affected entity/view/rule;
- selection/property context survives reasonable view transitions.

## 12. Error and uncertainty UX

Differentiate:

```text
INFO
WARNING
ERROR
BLOCKED
UNKNOWN / REQUIRES_CONFIRMATION
```

Messages answer what happened, what entity/rule/source is involved, whether work can continue and what resolves it.

## 13. UX budgets

Initial qualitative targets:

- project navigation/search immediate and keyboard-accessible;
- property editing avoids modal-dialog chains;
- repetitive 84-WTG table/scheme structure is data-driven, not row-by-row;
- multi-monitor workspace restores without manual rearrangement every launch;
- import surfaces ambiguity in one review workflow rather than dozens of modal interruptions.

## 14. Developer UX budget

- shared components runnable in Gallery;
- visual snapshots/headless rendering where framework permits;
- small UI patch produces targeted preview quickly;
- visual acceptance precedes unrelated full-system gates;
- theme/layout changes use centralized tokens/components rather than override stacks.

## 15. HiDPI and mixed DPI

Acceptance covers common scale classes, moving windows between monitors with different scale factors, crisp vector scheme rendering, stable text/control sizes and no coordinate mismatch between canvas rendering and hit-testing.

## 16. Accessibility

Visible focus, logical tab order, predictable shortcuts, readable contrast, non-color-only status cues and scalable text/control metrics within practical engineering density.

## 17. Visual acceptance contract

A UI change is not accepted because it compiles. Evidence may include UI Gallery screenshot, targeted headless screenshot, interaction test, owner preview build and mixed-DPI/multi-window manual check.

For a small UI-only patch, do not block first visual review on full NPT corpus/topology/switching suites.

## 18. Framework independence

This document is framework-neutral. Avalonia and Qt must each prove they can implement these contracts during Platform Stack Spike.
