# ADR 0006 — Optional EOD Integration

Status: **PENDING FEASIBILITY / COST GATE**  
Date opened: 2026-08-18

## Context

The electrical product could provide value as an optional capability alongside EOD, especially through shared object/workplace context and references to schemes/switching forms.

Foundation inspection of actual EOD repository materially improved feasibility:

- `MODULE-ACTIVATION-CONTRACT-001` accepted/merged;
- `MODULE-REGISTRY-001` completed/merged;
- EOD already has stable first-party manifests, scoped activation for `ORGANIZATION / ENERGY_SITE / WORKPLACE`, lifecycle/audit semantics and optional-integration behavior.

## Current decision

Standalone operation is mandatory.

EOD integration is allowed only behind an isolated adapter/bridge boundary.

Preferred feasibility target is **Level 2** using a small EOD-side bridge module that conforms to existing EOD registry and launches/deep-links into the standalone desktop application.

```text
EOD module registry
      ↓
ELECTRICAL-BRIDGE first-party module
      ↓ typed launch/context contract
Standalone Electrical Desktop Application
```

This preference is not production acceptance. Exact context-security, mapping and packaging cost require executable spike.

## Reject/defer if integration requires

- EOD-specific mandatory Core fields;
- mandatory EOD runtime/network dependency;
- forked UI/product code;
- pervasive mode conditionals;
- duplicate authoritative databases;
- significant independent release/deploy burden;
- second competing module-activation mechanism.

Important boundary:

> EOD module activation authorizes the EOD bridge capability; it does not automatically authorize arbitrary mutations in the standalone electrical application.

## Required output

Future bounded spike classifies result as:

```text
ACCEPT_LEVEL_1
ACCEPT_LEVEL_2
DEFER
REJECT_TOO_EXPENSIVE
```
