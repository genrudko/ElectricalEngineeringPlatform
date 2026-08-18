# Electrical Engineering Platform — Documentation Index

Статус: **canonical index for `UNIFIED-FOUNDATION-001`**  
Repository: `genrudko/ElectricalEngineeringPlatform`  
Active issue: #1  
Active branch: `architecture/unified-foundation-001`

## 1. Порядок чтения

1. `README.md` — краткое назначение и product direction.
2. `AGENTS.md` — operating/development contract.
3. `docs/project/CURRENT_STATE.md` — фактический текущий срез.
4. `docs/project/UNIFIED_PRODUCT_VISION.md` — цель и user value.
5. `docs/project/UNIFIED_SCOPE_AND_ROADMAP.md` — scope, исключения, фазы и vertical slices.
6. `docs/project/MIGRATION_PLAN_UNIFIED.md` — что реально мигрируется, а что пишется с нуля.
7. `docs/project/NEXT_WORK_ITEMS.md` — точные контракты следующих work items.
8. `docs/project/LEGACY_SOURCE_INDEX.md` — исторические исследования/корпуса и их disposition.
9. `docs/project/INPUT_ASSETS_AND_STORAGE.md` — что требуется от владельца и где хранится.
10. `docs/architecture/UNIFIED_SYSTEM_ARCHITECTURE.md` — modular-monolith структура и ownership.
11. `docs/architecture/DOMAIN_AND_PROJECT_MODEL.md` — `ElectricalProject`, equipment/terminal/connection/topology/state/view invariants.
12. `docs/architecture/UI_CORE.md` — design system, workspace, multi-window, shared controls/canvas и UX budgets.
13. `docs/architecture/SCHEME_AND_EQUIPMENT_LIBRARY.md` — Scheme module, semantic equipment library и representations.
14. `docs/architecture/IMPORT_AND_AUTO_LAYOUT.md` — CSV/XLSX mapping, staging, reconciliation, topology construction, layout constraints.
15. `docs/architecture/SWITCHING_AND_INTERLOCKS.md` — clean-sheet switching sequence/state transition/interlock model.
16. `docs/architecture/NPT_COMPATIBILITY_BOUNDARY.md` — XSDE/XTABL/NPT signal compatibility без загрязнения Core.
17. `docs/architecture/EOD_INTEGRATION_BOUNDARY.md` — optional EOD bridge и reject/cost gate.
18. `docs/compliance/NORMATIVE_ARCHITECTURE.md` — normative rule engine, provenance, applicability и versioning.
19. `docs/compliance/NORMATIVE_REGISTRY.md` — initial source registry и lifecycle.
20. `docs/compliance/GRAPHICS_GOST_PROFILE.md` — graphics ГОСТ/ЕСКД profile architecture.
21. `docs/compliance/LOCAL_POLICY_OVERLAYS.md` — manufacturer/enterprise/site/project тонкая настройка и non-weakening.
22. `docs/compliance/SAFETY_BOUNDARIES.md` — границы автоматизации и safety claims.
23. `docs/development/DEVELOPMENT_PLATFORM.md` — GitHub + existing VPS + self-hosted runner.
24. `docs/development/PLATFORM_STACK_SPIKE.md` — Avalonia vs Qt executable decision contract.
25. `docs/development/CI_AND_ACCEPTANCE.md` — risk-based CI, visual-first acceptance, preview builds.
26. `docs/decisions/0001_unified_electrical_platform.md` — единый продукт.
27. `docs/decisions/0002_domain_model_source_of_truth.md` — neutral domain authority.
28. `docs/decisions/0003_layered_normative_policy.md` — rule layering/non-weakening.
29. `docs/decisions/0004_github_vps_development_plane.md` — control/execution plane.
30. `docs/decisions/0005_platform_stack_pending.md` — PENDING Avalonia-vs-Qt decision.
31. `docs/decisions/0006_optional_eod_integration_pending.md` — PENDING EOD feasibility decision.

## 2. Owner decisions now explicit

- Old TBP code/config/rules are **not migrated**. `Modules.Switching` is a clean-sheet implementation.
- Internal enterprise/site instructions are **not shared development inputs**. Local Policy Overlay capability is developed using synthetic/cleared examples and applied inside controlled deployments.
- Public Russian normative acts and standards metadata are acquired/re-verified online from authoritative sources as relevant work begins; no manual bulk document pack is required.

## 3. PENDING decisions

До доказательного spike не считаются принятыми:

- final platform stack: Avalonia/C#/.NET vs Qt 6/C++/QML;
- exact native project package format;
- full plugin/dynamic-module mechanism;
- EOD integration implementation;
- полнота извлекаемой NPT topology из `nodes`/Tech relationships;
- exact scope автоматизируемых normative rules;
- final product/brand name beyond current working name.

## 4. Legacy/research sources

Старые репозитории и локальные материалы не являются новым canonical product state.

- `genrudko/electroscheme-studio` — Visio/VSDX/VSSX, prototype, Tauri/platform-spike, market/reference and UI research.
- NPT/Modus archives and extracted research — compatibility corpus outside Git.
- Old TBP — historical only, `DO_NOT_MIGRATE` unless owner later reopens one specific artifact.
- EOD — separate canonical repository used only through bounded integration work.

See `docs/project/LEGACY_SOURCE_INDEX.md`.

## 5. Source authority

При конфликте содержания:

```text
explicit owner instruction
→ accepted ADR
→ canonical architecture/compliance docs from this INDEX
→ CURRENT_STATE / roadmap / migration plan
→ current research evidence
→ historical project/prototype documents
```

`AGENTS.md` has highest priority for operating process only.

## 6. Normative-source authority

```text
official publication / official issuer
→ official standards catalogue
→ authoritative legal/reference system used as cross-check
→ secondary explanatory source
```

Ни одна редакция не считается бессрочно актуальной. `NORMATIVE_REGISTRY.md` хранит дату проверки, effective dates, amendments/supersedes и review status.

For ГОСТ, official status/edition metadata is mandatory; detailed extraction uses a lawful full-text source where required. Random unofficial copies are not normative authority.

## 7. Documentation change rule

Если код/архитектура меняют source-of-truth model, module boundary, normative behavior, project storage, import semantics, safety boundary, UI Core contract или development/deployment process — соответствующий canonical owner document обновляется в том же PR.