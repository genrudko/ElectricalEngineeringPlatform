# ADR 0005 — Desktop Platform Stack

Status: **PENDING EVIDENCE**  
Date opened: 2026-08-18

## Context

The new unified scope requires a heavy professional desktop application: large engineering canvas, 100k-row data views, multi-window/multi-monitor workspace, rich properties/tables and strong visual testing.

Historical Tauri/Vue/TypeScript/SVG work remains research evidence only.

## Candidates admitted to final spike

1. C#/.NET + Avalonia.
2. C++ + Qt 6/QML.

## Decision rule

No candidate is accepted before the equivalent executable contract in `docs/development/PLATFORM_STACK_SPIKE.md` is completed and owner manual evidence is available.

The selected stack must balance representative canvas performance, professional desktop UI/multi-monitor behavior, large table/tree performance, testing/visual CI, packaging, iteration speed, small-team maintainability and exact licensing/dependency obligations.

## Required output

The Platform Stack Spike updates this document to ACCEPTED with raw benchmark references and explicit rationale.
