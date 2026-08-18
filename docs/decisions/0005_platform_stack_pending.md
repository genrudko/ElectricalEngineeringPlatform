# ADR 0005 — Desktop Platform Stack

Статус: **PENDING EVIDENCE**  
Дата открытия: 2026-08-18

## Контекст

Unified scope требует heavy professional desktop application: large engineering canvas, 100k-row data views, multi-window/multi-monitor workspace, rich properties/tables и strong visual testing.

Historical Tauri/Vue/TypeScript/SVG work остаётся research evidence only.

## Candidates для final spike

1. C#/.NET + Avalonia.
2. C++ + Qt 6/QML.

## Decision rule

Ни один candidate не принимается до выполнения equivalent executable contract из `docs/development/PLATFORM_STACK_SPIKE.md` и появления owner manual evidence.

Selected stack должен балансировать:

- representative canvas performance;
- professional desktop UI/multi-monitor behavior;
- large table/tree performance;
- testing/visual CI;
- packaging;
- iteration speed;
- small-team maintainability;
- exact licensing/dependency obligations.

## Required output

`PLATFORM-STACK-SPIKE-001` обновляет этот ADR до `ACCEPTED` с raw benchmark references, owner evidence и explicit rationale.