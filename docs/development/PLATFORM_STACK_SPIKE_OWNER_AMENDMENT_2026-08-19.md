# PLATFORM-STACK-SPIKE-001 — owner amendment: Qt withdrawal at P1 entry

Status: **ACTIVE OWNER HARD CONSTRAINT / SUPERSEDES CONFLICTING COMPARATIVE CLAUSES**  
Effective date: **2026-08-19**  
Issue: #5  
Draft PR: #6

## 1. Owner decision

At P1 entry the owner changes the active candidate set as follows:

```text
Qt candidate:
WITHDRAWN BY OWNER HARD CONSTRAINT AT P1 ENTRY

Reason:
credentialed vendor acquisition path unacceptable;
credential-free alternatives materially increase build/toolchain burden.

Avalonia:
SOLE REMAINING CANDIDATE
→ must pass viability gates
→ otherwise NO_STACK_PASSES_CURRENT_CONTRACT
```

This is an operational/product constraint, not a benchmark score and not a claim that Qt failed canvas, table, rendering, packaging or desktop-performance gates.

## 2. Evidence that triggered the constraint

The frozen G0 Qt acquisition path was materially exercised before withdrawal:

- official Qt Online Installer `4.11.0` for Linux was downloaded from the Qt archive;
- installer SHA-256 matched the frozen value `40b76bdf74f6a396341efb70ae2e754fcd878474babb6cd9d7f07eff12a85c62`;
- exact-head run `32226051232` reached the explicit credential gate;
- `QT_INSTALLER_JWT_TOKEN` was not configured and the official unattended path requires a Qt Account credential;
- CMake `4.4.2`, Ninja `1.13.2` and Noto Sans `2.015` acquisition/provenance steps passed before that gate;
- the bounded run preserved acquisition evidence as artifact `9355567167`, digest `sha256:7b3c358093d4ff1f20ad65ca22c15f0fb6eba009fb3004d726d6b00802e76ada`.

The owner explicitly rejects introducing the vendor credential into the project baseline. The owner also rejects reopening the candidate through a credential-free acquisition approach when doing so materially increases build/toolchain burden.

No further Qt acquisition-path research is required by this work item unless the owner explicitly reopens the candidate.

## 3. Effect on the frozen G0 contract

`docs/development/PLATFORM_STACK_SPIKE.md` remains the historical frozen G0 research/measurement contract. This amendment supersedes only clauses that require an active two-candidate comparison after the owner withdrawal.

The following remain fully in force for Avalonia:

- exact-head reproducibility;
- frozen spike version pins;
- separation of spike pins from future Foundation/product pins;
- mandatory no-new-commercial-component/service baseline;
- deterministic fixtures and Russian typography requirements;
- raw machine-readable evidence;
- shared/canonical aggregation rules where percentile evidence is required;
- semantic-canvas SLOs;
- 100k Equipment Table + 10k Import Review gates;
- multi-window/multi-monitor/mixed-DPI requirements;
- Windows physical acceptance;
- reproducible Windows/Linux build/package requirements;
- licensing/dependency inventory;
- owner acceptance before final stack ADR.

The following comparative clauses are superseded:

- requirement to produce a second Qt implementation at each P-stage;
- requirement for Avalonia-vs-Qt paired environment fingerprints;
- requirement for side-by-side Qt screenshots/artifacts;
- requirement for head-to-head weighted scoring as a selection mechanism;
- `SELECT_QT` as an available exit state under the current contract.

Historical schemas may continue to allow `candidate=qt` so already-produced evidence remains valid and machine-readable. No new Qt evidence is expected while the withdrawal remains active.

## 4. Active candidate and decision topology

Active candidate set:

```text
Avalonia 12 / C# / .NET
```

The spike is now an **absolute viability proof**, not a comparative contest.

Allowed exit states:

```text
SELECT_AVALONIA
NO_STACK_PASSES_CURRENT_CONTRACT
```

Decision rule:

1. Avalonia must pass every mandatory gate applicable to the product baseline.
2. A failed mandatory gate cannot be compensated by weighted score, familiarity, lower implementation cost or owner preference.
3. If Avalonia fails an unresolved mandatory gate, the result is `NO_STACK_PASSES_CURRENT_CONTRACT`.
4. If Avalonia passes the full viability contract, `SELECT_AVALONIA` still requires owner manual acceptance and an explicit final decision/ADR.
5. The existence of a single remaining candidate is **not** itself stack selection.

## 5. Active implementation sequence

```text
P1 — finish Avalonia Professional Shell / UI Gallery acceptance
↓
P2 — Avalonia Semantic Scheme Canvas viability
↓
P3 — Avalonia Engineering Data Workspace viability
↓
P4 — Avalonia Multi-window / platform integration viability
↓
Windows + Linux packaging/evidence
↓
Owner physical Windows acceptance
↓
SELECT_AVALONIA or NO_STACK_PASSES_CURRENT_CONTRACT
```

No P-stage threshold is lowered because Qt was withdrawn.

## 6. Current P1 evidence at amendment entry

Exact head immediately before this amendment:

```text
8693e0f1868a7ebbb6c91b00c2bc91cc29347b90
```

Avalonia evidence on that head:

- Linux preflight: PASS;
- locked restore: PASS;
- Release build: PASS;
- deterministic presentation-behavior smoke: PASS;
- exact-head environment/dependency evidence: PASS;
- GitHub-hosted `windows-2022` x64 lane: PASS;
- self-contained Windows x64 preview published: PASS;
- Windows preview artifact `9355576296`, digest `sha256:460d74b1958b8ab41f8e2bc9fa44676f6a3462cfc33a86d4574df533c543e462`;
- Windows evidence artifact `9355576655`, digest `sha256:4ba321e5eda803082a20dd49d7a1d622498ef57bc1f8aa42411d54c8f176836f`.

Known P1 limitation remains explicit: Avalonia full-window `Avalonia.Headless 12.1.1` probing was not accepted as the native-window authority; Linux uses Release build + deterministic behavior evidence, while native visual/desktop behavior is validated through Windows evidence and owner physical acceptance.

## 7. Repository effect

Active Qt P1 implementation/workflow files are removed from the current PR net tree so a withdrawn candidate cannot continue to generate CI failures or look like an active acceptance dependency.

Git history and already-produced workflow/artifact evidence are intentionally preserved. The withdrawal is therefore auditable without carrying dead executable Qt code in the active spike baseline.

## 8. Reopening rule

Qt may be reconsidered only by a new explicit owner decision that states what hard constraint changed and what acquisition/toolchain baseline is now acceptable.

Do not silently substitute another Qt acquisition mechanism, introduce a vendor credential, add a private mirror, add a package manager, or create new paid/self-hosted infrastructure to bypass this amendment.
