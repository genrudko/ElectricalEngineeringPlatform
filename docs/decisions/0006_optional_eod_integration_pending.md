# ADR 0006 — Optional EOD Integration

Статус: **PENDING FEASIBILITY / COST GATE**  
Дата открытия: 2026-08-18

## Контекст

Electrical product может дать дополнительную ценность рядом с EOD, особенно через shared object/workplace context и references к schemes/switching forms.

Foundation inspection actual EOD repository materially повысил feasibility:

- `MODULE-ACTIVATION-CONTRACT-001` accepted/merged;
- `MODULE-REGISTRY-001` completed/merged;
- EOD уже имеет stable first-party manifests;
- scoped activation для `ORGANIZATION / ENERGY_SITE / WORKPLACE`;
- lifecycle/audit semantics;
- optional-integration behavior.

## Текущее решение

Standalone operation обязательна.

EOD integration допускается только за isolated adapter/bridge boundary.

Preferred feasibility target — **Level 2** через небольшой EOD-side bridge module, который использует existing EOD registry и launch/deep-link standalone desktop application.

```text
EOD module registry
      ↓
ELECTRICAL-BRIDGE first-party module
      ↓ typed launch/context contract
Standalone Electrical Desktop Application
```

Это preference, а не production acceptance. Exact context-security, mapping и packaging cost требуют executable spike.

## Reject/defer conditions

Интеграция откладывается/отклоняется, если требует:

- EOD-specific mandatory Core fields;
- mandatory EOD runtime/network dependency;
- forked UI/product code;
- pervasive mode conditionals;
- duplicate authoritative databases;
- significant independent release/deploy burden;
- second competing module-activation mechanism.

Критическая boundary:

> EOD module activation authorizes EOD bridge capability, но не arbitrary mutations в standalone electrical application.

## Required output

Future bounded spike классифицирует result как:

```text
ACCEPT_LEVEL_1
ACCEPT_LEVEL_2
DEFER
REJECT_TOO_EXPENSIVE
```