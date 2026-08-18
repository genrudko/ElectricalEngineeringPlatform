# ADR 0002 — Neutral Domain Model как Source of Truth

Статус: **ACCEPTED по решению владельца / зафиксирован в `UNIFIED-FOUNDATION-001`**  
Дата: 2026-08-18

## Контекст

Продукт должен поддерживать native schemes, structured CSV/XLSX import, NPT XSDE/XTABL compatibility, Visio interoperability и Switching/TBP без создания competing authoritative models.

## Решение

`ElectricalProject` и его neutral electrical domain model являются engineering source of truth.

Core semantic ownership включает:

- stable equipment identity;
- equipment type/properties;
- terminals;
- connections;
- topology;
- state/quality semantics;
- neutral signal bindings;
- project/view references;
- compliance profile references.

Views и external formats являются projections/adapters.

NPT-specific identifiers/serialization и EOD-specific identifiers не становятся required Core fields.

Topology отделена от diagram geometry.

`UNKNOWN` — first-class state и никогда не collapse молча в safe/negative state.

## Последствия

- CSV/XLSX import использует staging/reconciliation до Domain mutation;
- Scheme module хранит layout/views отдельно от electrical connections;
- NPT module переводит и preserves vendor data за adapter boundary;
- Switching module потребляет shared topology/state;
- project persistence versioned/migratable и framework-neutral.

## Отклонённые альтернативы

- **Diagram canvas как source of truth** — отклонено: moving geometry не должна redefine topology.
- **XSDE/NPT как source of truth** — отклонено из-за vendor lock и pollution native semantics.
- **Excel/CSV как native project format** — отклонено из-за недостаточной topology/state/view/versioning semantics.
- **Отдельная equipment database для Switching/TBP** — отклонено из-за duplicate identity/topology.