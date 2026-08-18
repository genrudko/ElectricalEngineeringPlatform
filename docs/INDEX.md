# Electrical Engineering Platform — индекс документации

Статус: **канонический индекс**  
Репозиторий: `genrudko/ElectricalEngineeringPlatform`  
Текущее issue/branch/PR состояние здесь намеренно не дублируется: фактический срез хранится в `docs/project/CURRENT_STATE.md` и перед работой перепроверяется напрямую в GitHub.

## 1. Порядок чтения

1. `README.md` — краткое назначение, архитектурные инварианты и текущий development model.
2. `AGENTS.md` — обязательный operating/development contract.
3. `docs/project/CURRENT_STATE.md` — фактический текущий срез и доказанные/недоказанные решения.
4. `docs/project/LANGUAGE_POLICY.md` — русский язык продукта/документации и английский язык внутренней технической части.
5. `docs/project/RU_EN_ENGINEERING_GLOSSARY.md` — RU ↔ EN terminology для UI/domain mapping.
6. `docs/project/UNIFIED_PRODUCT_VISION.md` — цель, user value и границы продукта.
7. `docs/project/UNIFIED_SCOPE_AND_ROADMAP.md` — scope, исключения, фазы и vertical slices.
8. `docs/project/MIGRATION_PLAN_UNIFIED.md` — что переносится как knowledge/evidence, а что пишется с нуля.
9. `docs/project/NEXT_WORK_ITEMS.md` — точные contracts следующих work items.
10. `docs/project/LEGACY_SOURCE_INDEX.md` — historical research/corpora и их disposition.
11. `docs/project/INPUT_ASSETS_AND_STORAGE.md` — какие исходные материалы реально нужны и где они хранятся.
12. `docs/architecture/UNIFIED_SYSTEM_ARCHITECTURE.md` — modular-monolith структура и ownership.
13. `docs/architecture/DOMAIN_AND_PROJECT_MODEL.md` — `ElectricalProject`, equipment/terminal/connection/topology/state/view invariants.
14. `docs/architecture/UI_CORE.md` — design system, workspace, multi-window, shared controls/canvas и UX budgets.
15. `docs/architecture/SCHEME_AND_EQUIPMENT_LIBRARY.md` — Scheme module, semantic equipment library и representations.
16. `docs/architecture/IMPORT_AND_AUTO_LAYOUT.md` — CSV/XLSX mapping, staging, reconciliation, topology construction и layout constraints.
17. `docs/architecture/SWITCHING_AND_INTERLOCKS.md` — clean-sheet switching sequence/state transition/interlock model.
18. `docs/architecture/NPT_COMPATIBILITY_BOUNDARY.md` — XSDE/XTABL/NPT signal compatibility без загрязнения Core.
19. `docs/architecture/EOD_INTEGRATION_BOUNDARY.md` — optional EOD bridge и reject/cost gate.
20. `docs/compliance/NORMATIVE_ARCHITECTURE.md` — normative rule engine, provenance, applicability и versioning.
21. `docs/compliance/NORMATIVE_REGISTRY.md` — initial source registry и lifecycle.
22. `docs/compliance/GRAPHICS_GOST_PROFILE.md` — graphics ГОСТ/ЕСКД profile architecture.
23. `docs/compliance/LOCAL_POLICY_OVERLAYS.md` — manufacturer/enterprise/site/project настройка и non-weakening.
24. `docs/compliance/SAFETY_BOUNDARIES.md` — границы automation и safety claims.
25. `docs/development/DEVELOPMENT_PLATFORM.md` — ChatGPT Project + GitHub + Development Bridge + existing VPS + self-hosted runner.
26. `docs/development/CHATGPT_PROJECT_CONTEXT.md` — Project Memory, Project Sources, retrieval-first chats, checkpoints и отказ от routine giant handoffs.
27. `docs/development/PLATFORM_STACK_SPIKE.md` — Avalonia vs Qt executable decision contract.
28. `docs/development/CI_AND_ACCEPTANCE.md` — risk-based CI, visual-first acceptance и preview artifacts.
29. `docs/decisions/0001_unified_electrical_platform.md` — единый продукт.
30. `docs/decisions/0002_domain_model_source_of_truth.md` — neutral domain authority.
31. `docs/decisions/0003_layered_normative_policy.md` — rule layering/non-weakening.
32. `docs/decisions/0004_github_vps_development_plane.md` — control/execution plane и proven ChatGPT→VPS bridge.
33. `docs/decisions/0005_platform_stack_pending.md` — PENDING Avalonia-vs-Qt decision.
34. `docs/decisions/0006_optional_eod_integration_pending.md` — PENDING EOD feasibility decision.

## 2. Языковая политика

Обязательная схема:

```text
Язык продукта и канонической документации → русский
Язык кода/internal API/schema/identifiers → английский
Внутренняя электротехническая терминология → professional engineering English
```

Пользователь видит русские terms и explanations; internal model использует stable English identifiers, например `CircuitBreaker`, `Disconnector`, `EarthingSwitch`, `TopologyGraph`, `SwitchingOperation`.

Transliteration в technical identifiers не допускается.

English внутри Russian documentation используется для exact technical identifiers, established engineering terms и machine-readable values, а не как язык explanatory narrative.

Foundation documentation приведена к этой policy; дальнейшие нарушения считаются documentation defect.

## 3. Явные решения владельца

- Старый TBP code/config/rules **не мигрируется**. `Modules.Switching` реализуется clean-sheet.
- Внутренние инструкции предприятий/объектов **не являются shared development inputs**. `LocalPolicy` mechanics разрабатываются на synthetic/cleared examples и применяются в controlled deployment environment.
- Государственные нормативные документы и ГОСТ/ЕСКД metadata получаются и повторно проверяются онлайн по authoritative sources по мере выполнения соответствующих compliance work items; вручную собирать большой package документов не требуется.
- Русский — обязательный язык UI и канонической documentation; английский — обязательный язык internal technical/domain layer.
- ChatGPT Plus + Project chat + Custom GPT Action может использоваться как interactive bridge к existing VPS; этот path фактически доказан 2026-08-18.
- Для continuity внутри одного ChatGPT Project принят retrieval-first workflow: previous project chats + редкие Project Source checkpoints + stable Project Instructions; routine giant handoff-файлы не являются default mechanism.
- Project Memory используется для восстановления контекста/причин решений, но GitHub/runtime остаются authority для ответа «что истинно сейчас».

## 4. ChatGPT Project context strategy

Целевая coordination model:

```text
Project chats
→ рабочая история и retrieval

Project Sources
→ редкие accepted checkpoints / decision notes

Project Instructions
→ устойчивые operating rules без volatile SHA/status

GitHub
→ current canonical project/development state

EEP Development Bridge / runner
→ actual runtime/execution evidence
```

Новый chat внутри Project начинается с retrieval-запроса по нескольким уникальным anchors, затем coordinator обязан отдельно re-verify current GitHub/runtime state.

Tasks/reminders используются как time/recurrence mechanism, а не как engineering-state database. Branch chats используются для альтернативных hypotheses, а не как замена ADR/checkpoint.

Canonical contract: `docs/development/CHATGPT_PROJECT_CONTEXT.md`.

## 5. Доказанный Development Bridge

Фактически подтверждена цепочка:

```text
ChatGPT Plus Project chat
→ @Custom GPT
→ GPT Action
→ HTTPS + Bearer
→ Caddy
→ EEP Development Bridge
→ FastAPI localhost
→ existing VPS
```

Также подтверждено, что GitHub connector работает в том же Project chat.

Это не отменяет formal GitHub CI. Целевая model — два contours:

```text
interactive:
ChatGPT Project → EEP Development Bridge → VPS

formal:
GitHub → self-hosted runner on VPS → checks/artifacts → PR acceptance
```

Bridge API остаётся bounded/allowlisted; arbitrary remote shell не является допустимым Foundation direction.

## 6. PENDING решения

До доказательного spike не считаются принятыми:

- final platform stack: Avalonia/C#/.NET vs Qt 6/C++/QML;
- exact native project package format;
- full plugin/dynamic-module mechanism;
- EOD integration implementation;
- полнота извлекаемой NPT topology из `nodes`/Tech relationships;
- exact scope автоматизируемых normative rules;
- final product/brand name beyond current working name.

## 7. Legacy/research sources

Старые repositories и local materials не являются новым canonical product state.

- `genrudko/electroscheme-studio` — Visio/VSDX/VSSX, prototype, Tauri/platform-spike, market/reference и UI research.
- NPT/Modus archives and extracted research — compatibility corpus outside Git.
- Old TBP — historical only, `DO_NOT_MIGRATE`, пока owner не переоткроет один specific artifact.
- EOD — отдельный canonical repository; взаимодействие только через bounded integration work.

См. `docs/project/LEGACY_SOURCE_INDEX.md`.

## 8. Приоритет источников

При конфликте содержания:

```text
явная инструкция владельца
→ accepted ADR
→ canonical architecture/compliance docs из этого INDEX
→ CURRENT_STATE / roadmap / migration plan
→ current research evidence
→ historical project/prototype documents
```

`AGENTS.md` имеет высший приоритет только по operating process.

Project Memory и Project Source checkpoints помогают continuity, но не переопределяют GitHub/runtime evidence.

## 9. Приоритет нормативных источников

```text
official publication / official issuer
→ official standards catalogue
→ authoritative legal/reference system used as cross-check
→ secondary explanatory source
```

Ни одна edition не считается бессрочно актуальной. `NORMATIVE_REGISTRY.md` хранит verification date, effective dates, amendments/supersedes и review status.

Для ГОСТ official status/edition metadata обязательно; detailed extraction выполняется по lawful full-text source при необходимости. Random unofficial copies не являются normative authority.

## 10. Правило изменения документации

Если code/architecture меняют source-of-truth model, module boundary, normative behavior, project storage, import semantics, safety boundary, UI Core contract, language/terminology contract или development/deployment/coordination process — соответствующий canonical owner document обновляется в том же PR.