# Инструкции агентам — Electrical Engineering Platform

## 1. Назначение продукта

Репозиторий `genrudko/ElectricalEngineeringPlatform` является каноническим репозиторием одного standalone, desktop-first, local-first модульного электротехнического инженерного комплекса.

Крупные функциональные направления:

- Scheme Studio;
- NPT Compatibility;
- новый Switching / TBP module;
- общие Domain Core, UI Core, Compliance Core, Import/Reconciliation и Development Platform.

Статус legacy-источников:

- `genrudko/electroscheme-studio` — historical research/migration source;
- NPT/Modus materials — compatibility/reference corpus;
- старый TBP project — `DO_NOT_MIGRATE`; новый Switching module реализуется с нуля;
- EOD — отдельный продукт/repository; допускается только bounded optional integration.

## 2. Языковой контракт

Обязательное разделение:

```text
Пользовательский интерфейс и каноническая документация → русский
Внутренняя technical/domain часть                     → английский
```

Правила:

1. Основной UI, user-facing diagnostics, help и generated user documents по умолчанию русскоязычные.
2. Вся новая каноническая документация пишется на русском языке.
3. Точные имена technical entities сохраняются английскими: `ElectricalProject`, `CircuitBreaker`, `TopologyGraph`, `SwitchingOperation` и т. п.
4. Classes, methods, variables, enums, API/schema keys, module IDs, internal event/error codes, tests и source-code terminology — английские.
5. Использовать профессиональный engineering English электроэнергетики: `grid`, `circuit breaker`, `disconnector`, `earthing switch`, `busbar`, `feeder`, `interlock` и т. п.
6. Транслит в identifiers запрещён.
7. Machine-readable code может быть английским, но пользовательское объяснение должно быть русским.
8. Российские нормативные названия/требования в UI/docs сохраняются на русском; internal `rule_id` — английский/machine-readable.

Канонические документы:

- `docs/project/LANGUAGE_POLICY.md`;
- `docs/project/RU_EN_ENGINEERING_GLOSSARY.md`.

## 3. Канонический источник и приоритеты

GitHub является источником истины для code, architecture, plans, issues, branches, PRs, tests и accepted evidence.

Перед началом работы самостоятельно восстановить из GitHub:

1. current `main`;
2. active issue;
3. active branch и Draft PR;
4. exact PR head / compare state;
5. changed files;
6. applicable workflow state;
7. canonical documents из `docs/INDEX.md`.

Не просить владельца сообщать handoff/SHA/state, которые доступны в GitHub.

При конфликте содержания действует приоритет:

```text
явная инструкция владельца
→ accepted ADR
→ canonical architecture/compliance docs из docs/INDEX.md
→ CURRENT_STATE / roadmap
→ current research evidence
→ historical/prototype documents
```

`AGENTS.md` имеет приоритет по operating process, но не переопределяет accepted engineering decisions.

## 4. Work-item workflow

Нормальный поток:

```text
issue
→ dedicated branch
→ Draft PR
→ targeted implementation/checks
→ visual/manual evidence when applicable
→ owner acceptance
→ explicit Ready/Merge command
```

Правила:

1. Для одного work item переиспользовать существующий issue/branch/Draft PR; не создавать duplicates.
2. Не коммитить напрямую в `main`, кроме неизбежного initial repository bootstrap или явно owner-authorized маленького docs-only canonical-state refresh вне active work item.
3. Не переводить PR в Ready for Review без явной команды владельца.
4. Не merge без явной команды владельца.
5. Держать изменения risk-bounded; избегать repair-on-repair churn.
6. Full/nuclear CI не является обязательным налогом на небольшое UI change.
7. Visual acceptance UI changes по возможности происходит до дорогих unrelated gates.
8. Фактический status определяется GitHub, а не памятью чата или local workspace.

## 5. Основные архитектурные инварианты

1. `ElectricalProject` / neutral domain model — engineering source of truth.
2. Diagram geometry — view/projection, а не electrical topology.
3. CSV/XLSX, VSDX/VSSX, XSDE, XTABL и switching-form documents — imports/exports/views/adapters, а не параллельные canonical models.
4. Equipment, terminals, connections, signals и rules используют stable identifiers.
5. Connections ссылаются на semantic terminals/ports, а не screen coordinates.
6. Mutations проходят через explicit command/transaction paths, совместимые с undo/redo и validation.
7. Project storage versioned и migratable.
8. `UNKNOWN` — first-class; неизвестные position/quality/energization не интерпретируются молча как safe/open/off/de-energized.
9. Domain Core не содержит NPT identifiers вроде `sTag`, `RTID`, `TechData`, `CustElem`, `scd*`.
10. UI Core не содержит product-specific business rules, принадлежащих modules/domain services.
11. Domain modules тестируются без запуска полного UI.
12. Standalone operation сохраняется при отсутствии EOD integration.

## 6. Целевая modular-monolith граница

```text
Core.Domain
Core.UI
Core.Compliance
Core.ProjectStorage

Modules.EquipmentLibrary
Modules.Import
Modules.Schemes
Modules.Npt
Modules.Switching

Adapters.Platform
Adapters.Eod        # optional / feasibility-gated
App
```

Separate assemblies/libraries ожидаемы; distributed microservices и dynamic plugin marketplace не являются ранними требованиями.

Не вводить shared abstraction, пока реальный cross-module use case не докажет необходимость.

## 7. Инварианты Import и Auto Layout

```text
source CSV/XLSX
→ mapping profile
→ normalization
→ staging candidate
→ validation/ambiguity resolution
→ reconciliation
→ ElectricalProject update
→ topology validation
→ auto-layout proposal
→ engineer review
```

Правила:

- CSV/XLSX не является native project storage;
- неоднозначный terminal/connection не угадывается молча;
- imported entities сохраняют source/provenance identifiers;
- re-import показывает diff/reconciliation plan до destructive change;
- manual layout corrections хранятся как layout constraints и не уничтожаются routine re-import/auto-layout;
- electrical correctness и layout confidence — разные diagnostics.

## 8. Граница NPT compatibility

NPT/Modus material — ценное industrial evidence и compatibility input, но не архитектура нового продукта.

Без доказательств не предполагать:

- что NPT `nodes` дают полный electrical topology graph;
- что каждый `scd*` value является KKS;
- что reconstruction из simplified XML model lossless;
- что experimentally generated XSDE objects native-Modus-safe до native acceptance.

NPT-specific IDs/storage rules принадлежат `Modules.Npt` / adapters.

Полный NPT corpus/vendor binaries не коммитить в GitHub. Использовать controlled VPS corpus storage; Git содержит только synthetic/cleared fixtures и project-authored tooling.

## 9. Нормативная дисциплина

Каждое encoded normative rule должно иметь минимум:

- stable rule ID;
- source document и issuer;
- edition/amendment/effective dates;
- applicability/scope;
- normative level/authority;
- machine-checkable predicate/action where possible;
- severity;
- explanation;
- source/provenance reference;
- test/evidence status.

Не заявлять full compliance целому ГОСТ, ПУЭ, ПОТЭЭ, ПТЭЭС, ПТЭЭП/ПТЭЭПЭЭ или switching-rule set без явно определённого, traced и accepted scope.

ПУЭ не моделировать как одну монолитную современную версию; учитывать applicable chapters/sources/revisions.

Государственные нормативные акты и ГОСТ/ЕСКД metadata повторно проверять онлайн по authoritative sources при начале соответствующей работы. Старый TBP и случайные internet copies не являются normative authority.

## 10. Local Policy и правило non-weakening

```text
Mandatory regulatory baseline
→ applicable standards/profile baseline
→ manufacturer/equipment constraints
→ enterprise policy
→ site/object policy
→ project policy
```

Lower/local layer может добавлять требования или выбирать stricter alternative. Попытка ослабить locked mandatory requirement — configuration error, а не override.

Внутренние enterprise/site instructions не являются shared development inputs и не должны запрашиваться/загружаться как normal prerequisite.

Local Policy mechanics разрабатываются на synthetic/cleared fixtures. Реальные внутренние документы остаются внутри authorized deployment environment.

## 11. Graphics и ГОСТ/ЕСКД

Electrical-scheme graphics — semantic assets под versioned graphic-standard profiles.

Native symbols требуют domain/type mapping, terminal semantics, state variants, normative source/profile, geometry evidence, connection points, labels/designations, output evidence и provenance clarity.

NPT graphical assets допускаются как compatibility/reference evidence; proprietary assets не копировать в native symbol library без прав и отдельного решения.

## 12. Switching, state и interlocks

`Modules.Switching` — clean-sheet module. Не мигрировать старый TBP source/config/rules/tests, если владелец явно не переоткроет конкретный artifact.

Safety boundaries:

- software simulation не является physical equipment control;
- logical/project interlock не заменяет relay/PLC/hardwired interlock;
- generated switching forms/sequences остаются draft/decision-support output с qualified human review;
- operation не считается safe только из-за отсутствия contradictory data;
- denial/uncertainty должно быть explainable пользователю.

Не добавлять real SCADA command execution, IEC-104 server, historian, P/Q control или redundancy без нового explicit owner decision.

## 13. UI Core и UX

UI Core — first-class architecture, а не cosmetic styling.

Цель: современный, плотный, профессиональный desktop engineering UX для долгих сессий, больших проектов, keyboard+mouse и multi-monitor work.

Обязательный foundation включает application shell/workspace, document tabs/splits/detachable windows, design system/UI Gallery, Property Inspector, trees/virtualized tables, command system, dialogs/notifications/status, shared canvas infrastructure, HiDPI/mixed-DPI и keyboard/focus behavior.

Reject legacy/MS-DOS-looking UI и sparse/mobile-first desktop composition.

Все user-facing labels/commands/messages — русские согласно языковому контракту.

UI changes требуют visible evidence.

## 14. Platform stack

Final stack остаётся PENDING.

Кандидаты:

- Avalonia + C#/.NET;
- Qt 6 + C++/QML.

Historical Tauri/WebView work в `genrudko/electroscheme-studio` PR #4 — research evidence only.

Не выбирать stack по familiarity/preference. `PLATFORM-STACK-SPIKE-001` сравнивает equivalent canvas, tables, multi-window, HiDPI, visual testing, packaging и development-iteration scenarios.

## 15. Development Platform

Два контура разработки:

```text
Интерактивный контур:
ChatGPT Plus Project chat
→ @EEP Development Bridge
→ HTTPS/Bearer
→ existing VPS

Формальный контур:
GitHub
→ self-hosted runner on existing VPS
→ targeted build/test/benchmark/package
→ GitHub checks/artifacts
→ owner acceptance
```

Прямой ChatGPT→VPS feasibility **доказан 2026-08-18**:

- Custom GPT Action работает на текущем ChatGPT Plus;
- Action работает внутри существующего Project-чата;
- в том же чате доступен GitHub connector;
- внешний `GET /health` дошёл до VPS и вернул `200 OK`;
- bridge работает за Caddy/HTTPS/Let's Encrypt, FastAPI слушает localhost, service account непривилегированный.

Bridge не должен предоставлять универсальный `runCommand(string)`/remote shell. Execution API строится как typed/allowlisted operations с fixed workspace, timeout, audit и ограниченным набором параметров.

Mandatory Business/MCP/new paid service не предполагается.

Local PC — acceptance endpoint, не required build environment. SSH остаётся инфраструктурным escape hatch.

Risk-based lanes:

- UI-only: compile + targeted UI/headless/visual evidence + preview;
- domain: core + affected module + serialization/migration;
- NPT: lossless/round-trip/corpus/format invariants;
- topology/switching/compliance: scenario/invariant/property/rule tests;
- full suite: release/nightly или genuinely systemic changes.

## 16. Optional EOD integration

EOD integration feasibility-gated и optional. Предпочтителен thin EOD bridge module через существующий EOD module registry + deep-link/context handoff в standalone desktop product.

Reject integration, если она требует EOD-specific entities в Domain Core, mandatory runtime dependency on EOD, отдельный product fork, duplicate UI shell, pervasive conditionals или substantial independent release/deploy burden.

Standalone product tests проходят при отсутствии EOD adapter.

## 17. Legacy/prototype disposition

- ElectroScheme Studio: selective research reuse only;
- NPT: preserve compatibility knowledge/corpus outside Git;
- old TBP: `DO_NOT_MIGRATE`;
- EOD: separate repository, bounded adapter only.

## 18. Documentation ownership

Начинать с `docs/INDEX.md` и обновлять canonical owner document в том же PR, если меняется его решение.

Каноническая документация должна соответствовать `LANGUAGE_POLICY.md`: narrative — русский, точные technical identifiers — английские.

## 19. Текущий work item и volatile state

`AGENTS.md` не является хранилищем volatile issue/branch/PR/SHA state. Перед началом работы текущий work item определяется по `docs/project/CURRENT_STATE.md` и затем перепроверяется непосредственно в GitHub.

На момент canonical-state refresh 2026-08-18:

- `UNIFIED-FOUNDATION-001` — accepted/merged;
- `INFRASTRUCTURE-SPIKE-001` — accepted/merged;
- следующий work item по roadmap — `PLATFORM-STACK-SPIKE-001`;
- Issue/branch/Draft PR для `PLATFORM-STACK-SPIKE-001` ещё не создаются до отдельной owner-проверки его final contract.

Принятая последовательность:

```text
PLATFORM-STACK-SPIKE-001
→ UI-CORE-FOUNDATION-001 + DOMAIN-CORE-FOUNDATION-001
→ IMPORT-TO-SCHEME-VERTICAL-SLICE-001
```

Final platform stack остаётся PENDING до equivalent executable evidence и owner acceptance.