# Следующие work items после Unified Foundation

Статус: канонический sequencing contract

## 1. Правило последовательности

Не переходить от Foundation сразу к широкому feature development.

```text
Infrastructure
→ Platform Stack
→ UI Core + Minimal Domain Core
→ Import-to-Scheme vertical slice
```

Каждый work item получает собственные issue/branch/Draft PR после принятия required dependency.

---

# `INFRASTRUCTURE-SPIKE-001`

## Цель

Превратить уже доказанный ChatGPT→VPS feasibility в нормальный production-quality development contour и доказать formal GitHub exact-head CI на existing VPS.

Сам факт прямого ChatGPT Plus → Custom GPT Action → VPS **уже доказан 2026-08-18** и не является открытым вопросом этого spike.

## Доказанный baseline

Уже подтверждено:

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

Дополнительно подтверждено, что GitHub connector работает в том же Project-чате.

## Scope

### A. Hardened Development Bridge

- dedicated unprivileged `eepbridge` service;
- fixed repository/workspace boundary;
- typed/allowlisted API operations;
- **no arbitrary `runCommand(string)` shell endpoint**;
- read-only operations для status/files/search/diff/logs/artifacts;
- bounded execution operations для build/test/benchmark/preview;
- task IDs и asynchronous long-task status;
- timeout/cancellation policy;
- concurrency limit;
- audit log;
- output size/truncation policy;
- secret/token rotation procedure;
- service restart/health/recovery documentation.

Минимальный целевой API contract:

```text
READ
getBridgeHealth
getServerStatus
getWorkspaceStatus
getRepositoryStatus
readWorkspaceFile
searchWorkspace
getGitStatus
getGitDiff
getTaskStatus
getTaskLog
listArtifacts

EXECUTE
prepareWorkspace(ref)
runBuild(profile)
runTests(suite)
runBenchmark(profile)
buildPreview(profile)
renderUiGallery()
```

Фактические operation names уточняются реализацией, но arbitrary shell запрещён.

### B. Self-hosted GitHub runner

- dedicated unprivileged runner account/service;
- repository-scoped runner where practical;
- exact PR-head checkout/verification;
- minimal `./dev`-style command contract;
- success/failure test job;
- publish logs/result artifact;
- private corpus path outside Git;
- workspace cleanup/retention;
- security boundary documentation.

## Acceptance

1. ChatGPT Project вызывает bridge read-only status operation без SSH.
2. ChatGPT Project запускает bounded test/build task и получает task result/log.
3. Bridge не позволяет arbitrary shell/path escape.
4. push tiny branch change запускает self-hosted workflow.
5. runner проверяет exact head.
6. deterministic test/build успешно выполняется.
7. small artifact публикуется в GitHub.
8. coordinator читает state/results через GitHub.
9. owner получает artifact без SSH.
10. deliberately failing job выдаёт полезную диагностическую информацию.
11. runner возвращается в clean/usable state.
12. private corpus/secrets не публикуются как artifacts.

---

# `PLATFORM-STACK-SPIKE-001`

## Dependency

`INFRASTRUCTURE-SPIKE-001` accepted.

## Цель

Выбрать Avalonia/C#/.NET или Qt 6/C++/QML через executable contract из `docs/development/PLATFORM_STACK_SPIKE.md`.

## Required outputs

- equivalent candidate apps;
- heavy semantic canvas benchmarks;
- 100k-row table benchmark;
- multi-window/workspace implementation;
- UI Gallery/visual evidence;
- Windows/Linux packaging;
- development-loop measurements;
- license/dependency report;
- owner manual acceptance;
- accepted Platform Stack ADR.

---

# `UI-CORE-FOUNDATION-001`

## Dependency

Platform Stack ADR accepted.

## Scope

- design tokens/theme;
- UI Gallery;
- application shell;
- workspace/document tabs;
- detachable/multi-window infrastructure;
- workspace persistence/recovery;
- Property Inspector framework;
- tree/virtual table controls;
- command/shortcut system;
- dialogs/notifications/status;
- shared canvas viewport/selection primitives;
- HiDPI/mixed-DPI baseline;
- centralized Russian user-facing strings;
- targeted screenshot/interaction tests.

## Acceptance

Owner вручную принимает visual/interaction direction до broad module work.

---

# `DOMAIN-CORE-FOUNDATION-001`

## Dependency

Platform Stack ADR accepted. Может частично идти параллельно с UI Core после стабилизации project layout.

## Scope

- project identity/schema version;
- equipment/type reference;
- terminals;
- connections;
- topology graph;
- semantic state с `UNKNOWN`;
- transactions/commands;
- validation result model;
- provenance skeleton;
- compliance profile refs;
- versioned persistence и round-trip/migration tests.

## Prohibited

- full CIM;
- full equipment catalogue;
- NPT vendor fields в Core;
- full switching engine;
- UI-framework object serialization как native project model.

---

# `IMPORT-TO-SCHEME-VERTICAL-SLICE-001`

## Dependencies

UI Core + Domain Core baseline accepted.

## Scenario

```text
CSV/XLSX
→ mapping profile
→ staging
→ explicit ambiguity resolution
→ Equipment/Terminals/Connections
→ topology validation
→ auto-layout
→ editable one-line view
→ manual position/route constraints
→ native save/reopen
→ changed spreadsheet re-import
→ reconciliation diff
→ apply update
→ manual layout preserved
```

Использовать небольшой, но representative electrical structure, а не произвольные graph nodes/rectangles.

## Acceptance

- electrical identity/topology survives save/reopen;
- ambiguity никогда не угадывается молча;
- layout proposal понятен;
- пользователь исправляет результат с низкой interaction cost;
- re-import сохраняет manual corrections;
- diagnostics различают topology/import/layout/profile issues;
- owner принимает workflow/UI value.

---

# `NPT-TOPOLOGY-MAPPING-EXPERIMENT-001`

Может идти после minimal Domain Core contracts; не блокирует первый CSV/XLSX vertical slice.

Required evidence:

- one known real NPT cell/mnemonic;
- extracted raw relationships;
- neutral equipment/terminal/connection graph;
- unresolved mapping report;
- comparison with visible one-line scheme;
- state-dependent continuity/energization test;
- conclusion: `SUFFICIENT / PARTIAL / UNSUITABLE`.

---

# `COMPLIANCE-RULES-SLICE-001`

## Цель

Доказать lifecycle:

```text
current authoritative source
→ semantic rule
→ implementation class
→ tests
→ diagnostic/explanation
```

Inputs re-verify online from authoritative public sources at execution time; manually uploaded normative pack не требуется.

Minimum slice:

- one switching rule из current applicable Order 757 text;
- one supporting ПТЭЭС/ПТЭЭП/ПОТЭЭ applicability example;
- one **synthetic stricter local-policy overlay** без confidential instructions;
- one attempted weakening conflict;
- one `UNKNOWN` prerequisite scenario.

No old TBP rule/config reuse и no claim of complete document coverage.

---

# `SWITCHING-FOUNDATION-001`

## Dependency

Minimal Domain Core + Compliance Core contracts accepted.

## Цель

Реализовать Switching/TBP module **с нуля**, без migration old TBP codebase/YAML/rules.

## Initial scope

- semantic `SwitchingOperation`;
- ordered `SwitchingSequence`;
- simulated state отдельно от observed/imported state;
- rule/interlock evaluation result;
- topology recalculation после каждого simulatable step;
- explainable `ALLOW / BLOCK / UNKNOWN / REQUIRES_CONFIRMATION` results;
- draft document projection;
- mandatory human-review boundary;
- synthetic local-policy overlay support.

## Source policy

- current government/sector normative acts re-verify online from authoritative sources;
- ГОСТ/ПУЭ/source metadata follow Compliance Core provenance rules;
- internal enterprise/site instructions не являются required shared-development inputs;
- old TBP implementation не является test oracle или normative authority.

---

# `EOD-INTEGRATION-FEASIBILITY-001`

Optional; предпочтительно после появления standalone app/API/module boundaries.

Scenario:

- EOD first-party `ELECTRICAL-BRIDGE` module через existing registry;
- typed site/project/equipment context;
- launch/focus standalone app;
- direct navigation;
- callback/deep link to EOD;
- inactive bridge blocked by EOD registry;
- standalone run с удалённым adapter;
- measure dependencies/release impact.

Required decision:

```text
ACCEPT_LEVEL_1
ACCEPT_LEVEL_2
DEFER
REJECT_TOO_EXPENSIVE
```

## Первый meaningful product milestone

Первый meaningful product milestone — accepted `IMPORT-TO-SCHEME-VERTICAL-SLICE-001`, а не сам факт успешных infrastructure/platform spikes.