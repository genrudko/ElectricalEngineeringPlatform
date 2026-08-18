# Граница optional EOD integration

Статус: канонический Foundation-документ  
Decision state: **FEASIBILITY-GATED / NOT COMMITTED**

## 1. Цель

Проверить, можно ли представить Electrical Engineering Platform как optional capability/module рядом с Electronic Operational Documentation (EOD) **без нарушения standalone architecture и без существенного роста development/release cost**.

Интеграция имеет смысл только если она дешёвая, bounded и operationally useful.

## 2. Фактический EOD baseline

В EOD repository уже существует accepted optional-module control plane.

Verified evidence на 2026-08-18:

- `MODULE-ACTIVATION-CONTRACT-001` accepted/merged в PR #62;
- `MODULE-REGISTRY-001` completed/merged в PR #68;
- activation scopes: `ORGANIZATION`, `ENERGY_SITE`, `WORKPLACE`;
- registry имеет stable module IDs/capabilities, scoped activation, lifecycle/audit semantics и central access decisions;
- optional integration является explicit supported concept и не требует hard dependency.

### Архитектурное следствие

Это materially повышает feasibility Level 2 integration.

Предпочтительный design:

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

EOD registry определяет доступность bridge capability в конкретном EOD scope, но не authorizes автоматически mutations внутри desktop application.

## 3. Non-negotiable principle

```text
Standalone Electrical Product
          │
          └── optional EOD Adapter / EOD Bridge Module
```

Не допускается:

```text
EOD runtime
   ↓ required dependency
Electrical Product Core
```

Domain Core, project storage, Scheme, Import и Switching работают при полном отсутствии EOD.

## 4. Полезные integration scenarios

- EOD module registry/launcher открывает standalone complex;
- deep link из EOD object/workplace в electrical project/view/equipment;
- deep link из electrical product обратно в related EOD document/journal;
- bounded context handoff: organization/site/workplace/user/project/equipment IDs там, где обе стороны это поддерживают;
- reference released scheme/switching form в EOD workflow;
- optional shared identity/session context, если future contract докажет низкую стоимость и достаточную security;
- module availability/activation контролируется existing EOD registry без передачи EOD ownership электрического project data.

Предпочтение — links/contracts, а не embedding всего desktop UI runtime.

## 5. Integration levels

### Level 0 — none

Products работают независимо. Этот режим поддерживается всегда.

### Level 1 — launcher/deep-link bridge

EOD запускает/focus standalone application с typed context.

Conceptual URIs:

```text
electrical://project/{id}/view/{id}
electrical://project/{id}/equipment/{id}
electrical://project/{id}/switching/{sequenceId}
```

Exact URI/security contract остаётся PENDING.

### Level 2 — bounded EOD bridge module + context integration

EOD exposes first-party bridge module через existing module registry и передаёт bounded context standalone application.

Runtime и electrical project storage остаются независимыми.

Поскольку EOD уже имеет accepted scoped activation и optional-integration semantics, Level 2 является preferred feasibility target subject to executable spike.

### Level 3 — embedded native UI/runtime integration

High cost/risk. Не принимается по умолчанию.

Наличие EOD module registry не является основанием для embedding Qt/Avalonia desktop UI в Django/web runtime.

## 6. Feasibility gate

Интеграция принимается только если small spike докажет:

1. standalone application сохраняет Core architecture;
2. EOD bridge/adapter cleanly removable/disable-able;
3. Domain Core содержит zero EOD-specific mandatory fields/types;
4. integration contract narrow и versioned;
5. existing EOD module manifest/activation semantics reused rather than forked;
6. нет separate fork electrical UI shell;
7. packaging/release duplication мала и измерима;
8. EOD unavailability не блокирует local project work;
9. access control/context handoff explicit и auditable;
10. EOD scope activation не путается с desktop-project authorization;
11. maintenance burden приемлем для небольшой команды.

## 7. Reject/cost triggers

Reject/defer, если integration требует:

- EOD-specific entities в neutral Domain Core;
- required EOD network/runtime dependency для normal work;
- duplicate embedded vs standalone UI;
- pervasive `if (eodMode)` branches;
- separate long-lived product fork;
- duplicated authoritative electrical databases с bidirectional sync;
- substantial independent installer/release pipeline;
- privileged EOD/server credentials на development runners;
- desktop/web framework compromise только ради embedding;
- second competing activation mechanism вместо existing EOD registry.

## 8. Data ownership

| Data | Owner |
|---|---|
| electrical project/equipment/topology/views | electrical product |
| scheme release/export artifact | electrical product; EOD may reference/copy released artifact |
| switching semantic sequence | electrical product |
| EOD journal/document/workflow records | EOD |
| EOD bridge activation/audit | EOD module registry |
| cross-links/context mappings | explicit integration record / bridge contract |

Silent mirrored authoritative copies запрещены.

## 9. Identity mapping

```text
EOD Organization / EnergySite / Workplace
↔ ElectricalProject site/context

EOD related object/document
↔ Electrical equipment/view/sequence/release reference
```

Mapping должен быть explicit, versioned и repairable. Display names не являются stable identity keys.

## 10. Authentication и permissions

- adapter получает minimum required claims/context;
- standalone local mode остаётся defined;
- electrical mutation authorization принадлежит electrical product, если accepted contract явно не делегирует её;
- EOD `ModuleAccessDecision` контролирует EOD bridge access, а не каждый desktop action;
- deep link сам по себе не является authorization;
- signed/short-lived launch context должен рассматриваться вместо доверия arbitrary URI parameters, когда identity имеет значение.

## 11. UX boundary

Предпочтительный UX — seamless-but-separate:

- EOD показывает capability только там, где registry её разрешает;
- одно действие открывает/focus desktop application на relevant context;
- cross-links убирают duplicate navigation.

Не форсировать embedded web-like experience, если он ухудшает professional desktop UX или multi-monitor support.

## 12. Suggested feasibility spike

1. inspect/reuse current EOD module manifest/capability API;
2. зарегистрировать `ELECTRICAL-BRIDGE` test module/capability;
3. активировать его для selected EOD scopes;
4. передать typed context, где mappings существуют;
5. открыть/focus standalone electrical preview app;
6. создать callback/deep link to EOD record;
7. доказать, что inactive bridge hidden/blocked через existing EOD access seam;
8. измерить changed files/dependencies/packaging/maintenance burden;
9. продемонстрировать standalone electrical operation с removed EOD adapter.

No shared database и no embedded full UI в first spike.

## 13. Текущая feasibility assessment

Likely low-cost path:

```text
EOD first-party bridge module
+ existing scoped activation registry
+ versioned deep-link/context protocol
+ standalone desktop app
```

Exact bridge API, context security и deployment cost пока не доказаны.

## 14. Decision outcome

```text
ACCEPT_LEVEL_1
ACCEPT_LEVEL_2
DEFER
REJECT_TOO_EXPENSIVE
```

`REJECT_TOO_EXPENSIVE` является допустимым инженерным результатом. EOD integration не должна искажать основной продукт.