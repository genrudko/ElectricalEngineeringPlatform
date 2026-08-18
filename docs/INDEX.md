# Electrical Engineering Platform — индекс документации

Статус: **канонический индекс для `UNIFIED-FOUNDATION-001`**  
Репозиторий: `genrudko/ElectricalEngineeringPlatform`  
Активный issue: #1  
Активная ветка: `architecture/unified-foundation-001`

## 1. Порядок чтения

1. `README.md` — краткое назначение и направление продукта.
2. `AGENTS.md` — обязательный operating/development contract.
3. `docs/project/CURRENT_STATE.md` — фактический текущий срез.
4. `docs/project/LANGUAGE_POLICY.md` — русский язык продукта/документации и английский язык внутренней технической части.
5. `docs/project/RU_EN_ENGINEERING_GLOSSARY.md` — canonical RU ↔ EN terminology для UI/domain mapping.
6. `docs/project/UNIFIED_PRODUCT_VISION.md` — цель и user value.
7. `docs/project/UNIFIED_SCOPE_AND_ROADMAP.md` — scope, исключения, фазы и vertical slices.
8. `docs/project/MIGRATION_PLAN_UNIFIED.md` — что реально мигрируется, а что пишется с нуля.
9. `docs/project/NEXT_WORK_ITEMS.md` — точные контракты следующих work items.
10. `docs/project/LEGACY_SOURCE_INDEX.md` — исторические исследования/корпуса и их disposition.
11. `docs/project/INPUT_ASSETS_AND_STORAGE.md` — что требуется от владельца и где хранится.
12. `docs/architecture/UNIFIED_SYSTEM_ARCHITECTURE.md` — modular-monolith структура и ownership.
13. `docs/architecture/DOMAIN_AND_PROJECT_MODEL.md` — `ElectricalProject`, equipment/terminal/connection/topology/state/view invariants.
14. `docs/architecture/UI_CORE.md` — design system, workspace, multi-window, shared controls/canvas и UX budgets.
15. `docs/architecture/SCHEME_AND_EQUIPMENT_LIBRARY.md` — Scheme module, semantic equipment library и representations.
16. `docs/architecture/IMPORT_AND_AUTO_LAYOUT.md` — CSV/XLSX mapping, staging, reconciliation, topology construction, layout constraints.
17. `docs/architecture/SWITCHING_AND_INTERLOCKS.md` — clean-sheet switching sequence/state transition/interlock model.
18. `docs/architecture/NPT_COMPATIBILITY_BOUNDARY.md` — XSDE/XTABL/NPT signal compatibility без загрязнения Core.
19. `docs/architecture/EOD_INTEGRATION_BOUNDARY.md` — optional EOD bridge и reject/cost gate.
20. `docs/compliance/NORMATIVE_ARCHITECTURE.md` — normative rule engine, provenance, applicability и versioning.
21. `docs/compliance/NORMATIVE_REGISTRY.md` — initial source registry и lifecycle.
22. `docs/compliance/GRAPHICS_GOST_PROFILE.md` — graphics ГОСТ/ЕСКД profile architecture.
23. `docs/compliance/LOCAL_POLICY_OVERLAYS.md` — manufacturer/enterprise/site/project тонкая настройка и non-weakening.
24. `docs/compliance/SAFETY_BOUNDARIES.md` — границы автоматизации и safety claims.
25. `docs/development/DEVELOPMENT_PLATFORM.md` — GitHub + existing VPS + self-hosted runner.
26. `docs/development/PLATFORM_STACK_SPIKE.md` — Avalonia vs Qt executable decision contract.
27. `docs/development/CI_AND_ACCEPTANCE.md` — risk-based CI, visual-first acceptance, preview builds.
28. `docs/decisions/0001_unified_electrical_platform.md` — единый продукт.
29. `docs/decisions/0002_domain_model_source_of_truth.md` — neutral domain authority.
30. `docs/decisions/0003_layered_normative_policy.md` — rule layering/non-weakening.
31. `docs/decisions/0004_github_vps_development_plane.md` — control/execution plane.
32. `docs/decisions/0005_platform_stack_pending.md` — PENDING Avalonia-vs-Qt decision.
33. `docs/decisions/0006_optional_eod_integration_pending.md` — PENDING EOD feasibility decision.

## 2. Языковая политика

Обязательная схема:

```text
Язык продукта и документации → русский
Язык кода/internal API/schema/identifiers → английский
Внутренняя электротехническая терминология → профессиональный engineering English
```

Пользователь видит русские термины и объяснения; internal model использует стабильные английские identifiers, например `CircuitBreaker`, `Disconnector`, `EarthingSwitch`, `TopologyGraph`, `SwitchingOperation`.

Транслит в technical identifiers не допускается.

Новая canonical документация пишется на русском языке. Английские слова внутри неё используются как точные technical identifiers/industry terms, а не как язык narrative text.

Foundation PR должен привести уже созданные canonical docs к этой политике до финальной owner acceptance/merge.

## 3. Owner decisions now explicit

- Старый TBP code/config/rules **не мигрируется**. `Modules.Switching` реализуется с нуля.
- Внутренние инструкции предприятий/объектов **не являются shared development inputs**. `Local Policy Overlay` разрабатывается на synthetic/cleared examples и применяется в контролируемом deployment environment.
- Государственные нормативные документы и ГОСТ/ЕСКД metadata получаются и повторно проверяются онлайн по authoritative sources по мере выполнения соответствующих compliance work items; вручную собирать большой пакет документов не требуется.
- Русский — обязательный язык UI и документации; английский — обязательный язык внутренней technical/domain части.

## 4. PENDING решения

До доказательного spike не считаются принятыми:

- final platform stack: Avalonia/C#/.NET vs Qt 6/C++/QML;
- exact native project package format;
- full plugin/dynamic-module mechanism;
- EOD integration implementation;
- полнота извлекаемой NPT topology из `nodes`/Tech relationships;
- exact scope автоматизируемых normative rules;
- final product/brand name beyond current working name.

## 5. Legacy/research sources

Старые репозитории и локальные материалы не являются новым canonical product state.

- `genrudko/electroscheme-studio` — Visio/VSDX/VSSX, prototype, Tauri/platform-spike, market/reference и UI research.
- NPT/Modus archives and extracted research — compatibility corpus outside Git.
- Old TBP — historical only, `DO_NOT_MIGRATE` unless owner later reopens one specific artifact.
- EOD — отдельный canonical repository, взаимодействие только через bounded integration work.

См. `docs/project/LEGACY_SOURCE_INDEX.md`.

## 6. Source authority

При конфликте содержания:

```text
explicit owner instruction
→ accepted ADR
→ canonical architecture/compliance docs from this INDEX
→ CURRENT_STATE / roadmap / migration plan
→ current research evidence
→ historical project/prototype documents
```

`AGENTS.md` имеет высший приоритет только по operating process.

## 7. Normative-source authority

```text
official publication / official issuer
→ official standards catalogue
→ authoritative legal/reference system used as cross-check
→ secondary explanatory source
```

Ни одна редакция не считается бессрочно актуальной. `NORMATIVE_REGISTRY.md` хранит дату проверки, effective dates, amendments/supersedes и review status.

Для ГОСТ official status/edition metadata обязательно; детальная extraction выполняется по lawful full-text source при необходимости. Случайные unofficial copies не являются нормативным authority.

## 8. Правило изменения документации

Если код/архитектура меняют source-of-truth model, module boundary, normative behavior, project storage, import semantics, safety boundary, UI Core contract, language/terminology contract или development/deployment process — соответствующий canonical owner document обновляется в том же PR.