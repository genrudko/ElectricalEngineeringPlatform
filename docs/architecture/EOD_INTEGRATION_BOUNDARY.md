# Optional EOD Integration Boundary

Статус: canonical foundation document  
Decision state: **FEASIBILITY-GATED / NOT COMMITTED**

## 1. Objective

Explore whether the electrical platform can be exposed as an optional module/capability inside Electronic Operational Documentation (EOD) **without compromising standalone architecture or materially increasing development/release cost**.

Integration is desirable only if cheap, bounded and operationally useful.

## 2. Factual EOD baseline verified during Foundation

The EOD repository already contains an accepted and implemented optional-module control plane.

Verified evidence at 2026-08-18:

- EOD `MODULE-ACTIVATION-CONTRACT-001` was accepted/merged in PR #62;
- EOD `MODULE-REGISTRY-001` was completed/merged in PR #68;
- v1 activation scopes are `ORGANIZATION`, `ENERGY_SITE`, `WORKPLACE`;
- the registry has stable module IDs/capabilities, scoped activation, lifecycle/audit semantics and central access decisions;
- optional integration is an explicit supported concept and does not need to become a hard dependency.

### Architectural implication

This materially improves feasibility of Level 2 integration.

Preferred design:

```text
EOD
└── ELECTRICAL-BRIDGE module
    ├── EOD manifest/capabilities
    ├── ORGANIZATION/SITE/WORKPLACE activation
    ├── navigation/deep-link UI
    ├── context mapping
    └── optional references to released artifacts
             │
             ▼
Standalone Electrical Desktop Application
```

EOD registry controls whether the bridge capability is available in a given EOD scope. It does not automatically authorize mutations inside the desktop application.

## 3. Non-negotiable principle

```text
Standalone Electrical Product
          │
          └── optional EOD Adapter / EOD Bridge Module
```

Never:

```text
EOD runtime
   ↓ required dependency
Electrical Product Core
```

Domain Core, project storage, Scheme, Import and Switching must work when EOD is absent.

## 4. Potential useful integration scenarios

- EOD module registry/launcher opens the standalone complex;
- deep link from EOD object/workplace to electrical project/view/equipment;
- deep link from electrical product back to related EOD document/journal/instruction;
- bounded context handoff: organization/site/workplace/user/project/equipment IDs where both products support them;
- attach/reference released scheme or switching form in EOD workflow;
- optional shared identity/session context if a future explicit contract makes it cheap and secure;
- module availability/activation controlled by existing EOD registry without EOD owning electrical project data.

Prefer links/contracts over embedding the entire desktop UI runtime.

## 5. Integration levels

### Level 0 — none

Products operate independently. Always supported.

### Level 1 — launcher/deep-link bridge

EOD launches/focuses standalone application with typed context.

Conceptual URIs:

```text
electrical://project/{id}/view/{id}
electrical://project/{id}/equipment/{id}
electrical://project/{id}/switching/{sequenceId}
```

Exact URI/security contract is PENDING.

### Level 2 — bounded EOD bridge module + context integration

EOD exposes a first-party bridge module through its existing module registry and passes bounded context to the standalone application. Runtime and electrical project storage remain independent.

Because EOD already has accepted scoped activation and optional-integration semantics, this is the preferred feasibility target subject to an executable spike.

### Level 3 — embedded native UI/runtime integration

High cost/risk. Not accepted by default. The existence of an EOD module registry does not justify embedding Qt/Avalonia desktop UI into Django/web runtime.

## 6. Feasibility gate

Accept only if a small spike demonstrates:

1. standalone application remains unchanged in Core architecture;
2. EOD bridge/adapter can be removed/disabled cleanly;
3. Domain Core contains zero EOD-specific mandatory fields/types;
4. narrow versioned integration contract;
5. existing EOD module manifest/activation semantics reused rather than forked;
6. no separate fork of electrical UI shell;
7. packaging/release duplication is small and measurable;
8. EOD unavailability does not prevent local project work;
9. access control/context handoff is explicit and auditable;
10. EOD scope activation is not mistaken for desktop-project authorization;
11. maintenance burden is acceptable for a small team.

## 7. Reject/cost triggers

Reject/defer if integration requires:

- EOD-specific entities in neutral Domain Core;
- required EOD network/runtime dependency for normal work;
- duplicate embedded vs standalone UI;
- pervasive `if (eodMode)` branches;
- separate long-lived product fork;
- duplicated authoritative electrical databases with bidirectional sync;
- substantial independent installer/release pipeline;
- privileged EOD/server credentials on development runners;
- desktop/web framework compromise solely for embedding;
- second competing activation mechanism instead of existing EOD registry.

## 8. Data ownership

| Data | Owner |
|---|---|
| electrical project/equipment/topology/views | electrical product |
| scheme release/export artifact | electrical product; EOD may reference/copy released artifact |
| switching semantic sequence | electrical product |
| EOD journal/document/workflow records | EOD |
| EOD bridge activation/audit | EOD module registry |
| cross-links/context mappings | explicit integration record / bridge contract |

Do not create silent mirrored authoritative copies.

## 9. Identity mapping

```text
EOD Organization / EnergySite / Workplace
↔ ElectricalProject site/context

EOD related object/document
↔ Electrical equipment/view/sequence/release reference
```

Mapping must be explicit, versioned and repairable. Display names are not stable identity keys.

## 10. Authentication and permissions

- adapter receives minimum required claims/context;
- standalone local mode remains defined;
- electrical mutation authorization remains owned by electrical product unless explicitly delegated by accepted contract;
- EOD `ModuleAccessDecision` controls EOD bridge access, not every desktop action;
- a deep link is not authorization by itself;
- signed/short-lived launch context should be evaluated rather than trusting arbitrary URI parameters when identity matters.

## 11. UX boundary

Preferred UX is seamless-but-separate: EOD exposes the capability only where registry allows it, one action opens/focuses the desktop application on relevant context, and cross-links avoid duplicate manual navigation.

Do not force an embedded web-like experience if it degrades professional desktop UX or multi-monitor support.

## 12. Suggested feasibility spike

1. inspect/reuse current EOD module manifest/capability API;
2. register `ELECTRICAL-BRIDGE` test module/capability;
3. activate it for selected EOD scopes;
4. pass typed context where mappings exist;
5. open/focus standalone electrical preview app;
6. produce callback/deep link to EOD record;
7. prove inactive bridge is hidden/blocked through existing EOD access seam;
8. measure changed files/dependencies/packaging/maintenance burden;
9. demonstrate standalone electrical operation with EOD adapter removed.

No shared database and no embedded full UI in first spike.

## 13. Current feasibility assessment

Likely low-cost path:

```text
EOD first-party bridge module
+ existing scoped activation registry
+ versioned deep-link/context protocol
+ standalone desktop app
```

Exact bridge API, context security and deployment cost remain unproven.

## 14. Decision outcome

```text
ACCEPT_LEVEL_1
ACCEPT_LEVEL_2
DEFER
REJECT_TOO_EXPENSIVE
```

`REJECT_TOO_EXPENSIVE` is a valid engineering result; EOD integration must not distort the main product.
