# Инструкции агентам — Electrical Engineering Platform

## 1. Идентичность продукта

Репозиторий `genrudko/ElectricalEngineeringPlatform` является каноническим umbrella для одного **standalone, desktop-first, local-first, modular electrical-engineering software complex**.

Продукт содержит три крупных функциональных направления:

- Scheme Studio;
- NPT Engineering Toolkit / compatibility;
- Switching / TBP.

Disposition legacy-источников различается:

- `genrudko/electroscheme-studio` — исторический research/migration source;
- NPT/Modus materials — compatibility/reference corpus;
- **старый TBP project имеет disposition `DO_NOT_MIGRATE`**. Новый Switching module реализуется с нуля на текущих Domain/Compliance contracts и проверенных нормативных источниках.

## 2. Языковой contract

Обязательная схема проекта:

```text
Пользовательский интерфейс и документация → русский язык
Внутренняя technical/domain часть       → английский язык
```

Правила:

1. Весь основной UI, user-facing diagnostics, help и generated user documents по умолчанию русскоязычные.
2. Вся новая canonical документация пишется на русском языке.
3. Точные имена technical entities в документации остаются английскими: `ElectricalProject`, `CircuitBreaker`, `TopologyGraph`, `SwitchingOperation` и т. п.
4. Classes, methods, variables, enums, API/schema keys, module IDs, internal event/error codes, tests и source-code terminology — английские.
5. Внутренняя терминология должна использовать профессиональный engineering English электроэнергетики: `grid`, `circuit breaker`, `disconnector`, `earthing switch`, `busbar`, `feeder`, `interlock` и т. п.
6. Транслит в identifiers (`Vykluchatel`, `Razedinitel` и т. п.) запрещён.
7. Machine-readable error/rule code может быть английским, но пользовательское explanation — русское.
8. Российские нормативные названия/требования в UI/docs сохраняются на русском; internal `rule_id` — английский/machine-readable.

Canonical owners:

- `docs/project/LANGUAGE_POLICY.md`;
- `docs/project/RU_EN_ENGINEERING_GLOSSARY.md`.

При добавлении shared domain entity проверять glossary и не создавать случайный новый перевод/синоним.

## 3. Канонический источник и порядок приоритетов

GitHub является каноническим источником code, architecture, plans, issues, branches, PRs, tests и accepted evidence.

Перед началом работы самостоятельно восстановить из GitHub:

1. current `main`;
2. active issue;
3. active branch и Draft PR;
4. exact PR head / compare state;
5. changed files;
6. applicable workflow state;
7. canonical documents из `docs/INDEX.md`.

Не просить владельца сообщать handoff/SHA/state, которые можно получить из GitHub.

Для product/architecture meaning действует приоритет:

```text
explicit owner instruction
→ accepted ADR
→ canonical architecture/compliance docs from docs/INDEX.md
→ CURRENT_STATE / roadmap
→ research and migration evidence
→ historical prototype documents
```

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
2. Не коммитить напрямую в `main`, кроме неизбежного initial repository bootstrap.
3. Не переводить PR в Ready for Review без явной команды владельца.
4. Не merge без явной команды владельца.
5. Держать изменения risk-bounded; избегать repair-on-repair churn.
6. Full/nuclear CI не является обязательным налогом на маленькое UI change.
7. Visual acceptance UI changes по возможности происходит до дорогих unrelated gates.
8. Фактический status определяется GitHub, а не chat memory/local patches.

## 5. Core architecture invariants

1. `ElectricalProject` / neutral domain model — engineering source of truth.
2. Diagram geometry — view/projection, а не electrical topology.
3. CSV/XLSX, VSDX/VSSX, XSDE, XTABL и switching-form documents — imports/exports/views/adapters, а не параллельные canonical models.
4. Equipment, terminals, connections, signals и rules используют stable identifiers.
5. Connections ссылаются на semantic terminals/ports, а не screen coordinates.
6. Mutations проходят через explicit command/transaction paths, совместимые с undo/redo и validation.
7. Project storage versioned и migratable.
8. `UNKNOWN` — first-class. Unknown position/quality/energization никогда не интерпретируется молча как open/off/de-energized.
9. Domain Core не содержит NPT identifiers вроде `sTag`, `RTID`, `TechData`, `CustElem`, `scd*`.
10. UI Core не содержит product-specific business rules, принадлежащих modules/domain services.
11. Domain modules тестируются без запуска полного UI.
12. Standalone operation сохраняется при отсутствии EOD integration.

## 6. Target modular-monolith boundary

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

Не вводить shared abstraction, пока хотя бы один реальный cross-module use case не докажет необходимость.

## 7. Import and auto-layout invariants

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

- CSV/XLSX не является native project storage.
- Не угадывать неоднозначный terminal/connection молча.
- Imported entities сохраняют source/provenance identifiers.
- Re-import показывает diff/reconciliation plan до destructive change.
- Manual layout corrections хранятся как layout constraints и не уничтожаются routine re-import/auto-layout.
- Electrical correctness и layout confidence — разные diagnostics.

## 8. NPT compatibility boundary

NPT/Modus material — ценное industrial evidence и compatibility input, но не архитектура нового продукта.

Без доказательств не предполагать:

- что NPT `nodes` дают полный electrical topology graph;
- что каждый `scd*` value является KKS;
- что reconstruction из simplified XML model lossless;
- что experimentally generated XSDE objects native-Modus-safe до native acceptance.

NPT-specific IDs/storage rules принадлежат `Modules.Npt` / adapters.

Полный NPT corpus/vendor binaries не коммитить в GitHub. Использовать controlled VPS corpus storage; Git содержит только synthetic/cleared fixtures и project-authored tooling.

## 9. Normative/compliance discipline

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

Не заявлять full compliance целому ГОСТ, ПУЭ, ПОТЭЭ, ПТЭЭС, ПТЭЭП/ПТЭЭПЭЭ или switching-rule set, пока claimed scope не определён, не traced и не accepted.

ПУЭ не моделировать как одну монолитную современную версию; учитывать applicable chapters/sources/revisions.

Public normative acts/standards получать и re-verify online по authoritative sources при начале соответствующей работы. Старый TBP implementation и случайные internet copies не являются normative authority.

Для ГОСТ official catalogue/status metadata обязательно; detailed extraction использует lawful full-text source при необходимости.

## 10. Non-weakening local policy rule

```text
Mandatory regulatory baseline
→ applicable standards/profile baseline
→ manufacturer/equipment constraints
→ enterprise policy
→ site/object policy
→ project policy
```

Lower/local layer может добавлять требования или выбирать stricter alternative. Попытка ослабить locked mandatory requirement — configuration error, а не override.

### Confidentiality boundary

Внутренние enterprise/site instructions **не являются shared development inputs** и не должны запрашиваться/загружаться как normal prerequisite.

Local Policy Overlay mechanics разрабатываются на synthetic/cleared fixtures. Реальные внутренние инструкции/source documents остаются внутри authorized deployment environment; там потребляется только formalized local policy package/source metadata по правилам конкретного внедрения.

## 11. Graphics and ГОСТ/ЕСКД

Electrical-scheme graphics — semantic assets под versioned graphic-standard profiles.

Не принимать symbol только потому, что он визуально знаком. Native symbols требуют domain/type mapping, terminal semantics, state variants, normative source/profile, geometry evidence, connection points, labels/designations, output evidence и provenance clarity.

NPT graphical assets допускаются как compatibility/reference evidence; proprietary assets не копировать молча в native symbol library.

## 12. Switching, state and interlocks

`Modules.Switching` — **clean-sheet module**. Не мигрировать старый TBP source/config/rules/tests, если владелец явно не переоткроет один конкретный artifact.

Safety boundaries:

- software simulation не является physical equipment control;
- logical/project interlock не заменяет relay/PLC/hardwired interlock;
- generated switching forms/sequences остаются draft/decision-support output с qualified human review, пока отдельное решение не установит иное;
- operation не считается safe только из-за отсутствия contradictory data;
- denial/uncertainty должно быть explainable пользователю.

Не добавлять real SCADA command execution, IEC-104 server, historian, P/Q control или redundancy без нового explicit owner decision.

## 13. UI Core and UX quality

UI Core — first-class architecture, а не cosmetic styling.

Цель: современный, плотный, профессиональный desktop engineering UX для долгих сессий, больших проектов, keyboard+mouse и multi-monitor work.

Обязательный foundation включает application shell/workspace, document tabs/splits/detachable windows, design system/UI Gallery, Property Inspector, trees/virtualized tables, command system, dialogs/notifications/status, shared canvas infrastructure, HiDPI/mixed-DPI и keyboard/focus behavior.

Reject MS-DOS/legacy-looking UI и sparse/mobile-first desktop composition.

Все user-facing labels/commands/messages — русские согласно языковому contract. Английские internal identifiers не должны протекать в основной UI без реальной технической причины.

UI changes требуют visible evidence.

## 14. Platform stack rule

Final stack PENDING.

Candidates:

- Avalonia + C#/.NET;
- Qt 6 + C++/QML.

Historical Tauri/WebView work в `genrudko/electroscheme-studio` PR #4 — research evidence only.

Не выбирать stack по familiarity/preference. Platform Stack Spike сравнивает equivalent canvas, tables, multi-window, HiDPI, visual testing, packaging и development-iteration scenarios.

## 15. Development Platform and CI

```text
ChatGPT/owner
→ GitHub
→ self-hosted runner on existing VPS
→ targeted build/test/benchmark/package
→ GitHub logs/artifacts
→ owner acceptance
```

Mandatory Business/MCP/new paid service не предполагается.

Local PC — acceptance endpoint, не required build environment.

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

## 17. Legacy/prototype preservation

Не копировать старые продукты механически.

Disposition examples:

- ElectroScheme Studio: selective research reuse only;
- NPT: preserve compatibility knowledge/corpus outside Git;
- old TBP: `DO_NOT_MIGRATE`;
- EOD: separate repository, bounded adapter only.

## 18. Documentation ownership

Начинать с `docs/INDEX.md` и обновлять canonical owner document в том же PR, если меняется его решение.

Canonical docs должны соответствовать `LANGUAGE_POLICY.md`: narrative — русский, точные technical identifiers — английские.

## 19. Current work item

Для `UNIFIED-FOUNDATION-001`:

- repository `genrudko/ElectricalEngineeringPlatform`;
- issue #1;
- branch `architecture/unified-foundation-001`;
- Draft PR only;
- documentation/governance/architecture contracts first;
- no bulk product-code migration;
- no final platform-stack selection;
- no Ready/Merge без explicit owner command.

Immediate sequence после accepted Foundation:

```text
Infrastructure Spike
→ Avalonia vs Qt Platform Stack Spike
→ UI Core + minimal Domain Core
→ structured import / topology / auto-layout vertical slice
```