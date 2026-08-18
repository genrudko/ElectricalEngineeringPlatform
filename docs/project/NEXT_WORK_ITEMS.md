# Next Work Items after Unified Foundation

Статус: canonical sequencing contract

## Rule

Do not jump directly from Foundation into feature breadth.

```text
Infrastructure
→ Platform Stack
→ UI Core + Minimal Domain Core
→ Import-to-Scheme vertical slice
```

Each work item gets its own issue/branch/Draft PR after required dependency is accepted.

---

# INFRASTRUCTURE-SPIKE-001

## Objective

Prove that GitHub controls normal development while existing VPS executes builds/tests and returns artifacts without routine owner SSH.

## Scope

- dedicated unprivileged self-hosted runner account/service;
- exact PR-head checkout;
- minimal `./dev`-style command contract;
- success/failure test job;
- publish logs/result artifact;
- private corpus path outside Git;
- workspace cleanup/retention;
- security boundary documentation.

## Acceptance

1. push tiny branch change;
2. workflow dispatches to self-hosted VPS runner;
3. exact head is verified;
4. deterministic test/build succeeds;
5. small artifact published;
6. coordinator reads state/results through GitHub;
7. owner obtains artifact without SSH;
8. deliberately failing job reports useful error;
9. runner returns to clean/usable state.

---

# PLATFORM-STACK-SPIKE-001

## Dependency

`INFRASTRUCTURE-SPIKE-001` accepted.

## Objective

Select Avalonia/C#/.NET or Qt 6/C++/QML using `docs/development/PLATFORM_STACK_SPIKE.md`.

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

# UI-CORE-FOUNDATION-001

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
- targeted screenshot/interaction tests.

## Acceptance

Owner manually accepts visual/interaction direction before broad module work.

---

# DOMAIN-CORE-FOUNDATION-001

## Dependency

Platform Stack ADR accepted. May run partly parallel with UI Core once project layout is stable.

## Scope

- project identity/schema version;
- equipment/type reference;
- terminals;
- connections;
- topology graph;
- semantic state with UNKNOWN;
- transactions/commands;
- validation result model;
- provenance skeleton;
- compliance profile refs;
- versioned persistence and round-trip/migration tests.

## Prohibited

- full CIM;
- full equipment catalogue;
- NPT vendor fields in Core;
- full switching engine;
- UI-framework object serialization as native project model.

---

# IMPORT-TO-SCHEME-VERTICAL-SLICE-001

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

Use a small but real/representative electrical structure, not arbitrary graph nodes/rectangles.

## Acceptance

- electrical identity/topology survives save/reopen;
- ambiguity is never silently guessed;
- layout proposal is understandable;
- user can correct it with low interaction cost;
- re-import preserves manual corrections;
- diagnostics distinguish topology/import/layout/profile issues;
- owner accepts workflow/UI value.

---

# NPT-TOPOLOGY-MAPPING-EXPERIMENT-001

May run after minimal Domain Core contracts exist; does not block first CSV/XLSX vertical slice.

Required evidence:

- one known real NPT cell/mnemonic;
- extracted raw relationships;
- neutral equipment/terminal/connection graph;
- unresolved mapping report;
- comparison with visible one-line scheme;
- state-dependent continuity/energization test;
- conclusion: sufficient / partial / unsuitable.

---

# COMPLIANCE-RULES-SLICE-001

Prove current-source → rule → test → diagnostic lifecycle with a very small set of requirements.

Inputs are acquired/re-verified online from authoritative public sources at execution time; no manually uploaded normative pack is required.

Minimum slice:

- one switching rule from the current applicable Order 757 text;
- one supporting PTEES/PTEEP/POTEE applicability example;
- one **synthetic stricter local-policy overlay** demonstrating the enterprise/site mechanism without using confidential instructions;
- one attempted weakening conflict;
- one UNKNOWN prerequisite scenario.

No old TBP rule/config reuse and no claim of complete document coverage.

---

# SWITCHING-FOUNDATION-001

## Dependency

Minimal Domain Core + Compliance Core contracts accepted.

## Objective

Implement the Switching/TBP module **from scratch**, without migration of the old TBP codebase/YAML/rules.

## Initial scope

- semantic `SwitchingOperation`;
- ordered `SwitchingSequence`;
- simulated state distinct from observed/imported state;
- rule/interlock evaluation result;
- topology recalculation after each simulatable step;
- explainable `ALLOW / BLOCK / UNKNOWN / REQUIRES_CONFIRMATION` results;
- draft document projection;
- mandatory human-review boundary;
- synthetic local-policy overlay support.

## Source policy

- current government/sector normative acts are re-verified online from authoritative sources;
- ГОСТ/ПУЭ/source metadata follow Compliance Core provenance rules;
- internal enterprise/site instructions are not required for shared development;
- old TBP implementation is not a test oracle or normative authority.

---

# EOD-INTEGRATION-FEASIBILITY-001

Optional; prefer after standalone app/API/module boundaries exist.

Scenario:

- EOD first-party `ELECTRICAL-BRIDGE` module using existing registry;
- typed site/project/equipment context;
- launch/focus standalone app;
- direct navigation;
- callback/deep link to EOD;
- inactive bridge blocked by EOD registry;
- standalone run with adapter removed;
- measure dependencies/release impact.

Required decision:

```text
ACCEPT_LEVEL_1
ACCEPT_LEVEL_2
DEFER
REJECT_TOO_EXPENSIVE
```

## First product milestone

The first meaningful product milestone is the accepted `IMPORT-TO-SCHEME-VERTICAL-SLICE-001`, not merely successful infrastructure/platform spikes.