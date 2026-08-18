# ADR 0001 — Единая Electrical Engineering Platform

Статус: **ACCEPTED по owner direction / зафиксирован в `UNIFIED-FOUNDATION-001`**  
Дата: 2026-08-18

## Контекст

Scheme Studio, NPT Engineering Toolkit/Compatibility и Switching/TBP требуют одних и тех же базовых concepts: equipment identity, terminals, connections, topology, state и engineering/operational rules.

Три независимые архитектуры привели бы к duplicate project models, equipment libraries, topology logic, state handling, UI infrastructure и normative configuration.

Старый TBP implementation отдельно признан unsuitable для migration и не определяет новую Switching architecture.

## Решение

Создавать один standalone modular electrical-engineering product с общими Domain Core, UI Core и Compliance Core.

Функциональные modules:

- Scheme Studio;
- NPT Compatibility;
- clean-sheet Switching/TBP;
- first-class CSV/XLSX Import/Reconciliation;
- Equipment Library.

Архитектурный стиль — modular monolith, пока future ADR не докажет необходимость distributed boundary.

## Последствия

Положительные:

- одна project/equipment identity;
- topology/state reuse между Scheme, NPT и Switching;
- одна UI/design foundation;
- одна normative policy model;
- import once, reuse across modules;
- меньше maintenance duplication.

Costs/risks:

- Foundation/migration decisions до broad feature expansion;
- common Core не должен превратиться в over-general mega-framework;
- old code нельзя merge механически;
- platform stack должен быть переоценён под unified scope.

## Отклонённые альтернативы

- **Three permanently independent products** — rejected из-за domain divergence и repeated maintenance.
- **Immediate codebase concatenation** — rejected из-за incompatible models/UI stacks/prototypes.
- **Microservices вокруг former products** — rejected для local-first desktop scope и small-team cost.
- **Reuse old TBP architecture as Switching baseline** — rejected по owner decision; module создаётся clean-sheet.