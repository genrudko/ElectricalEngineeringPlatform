# Development Platform

Статус: canonical foundation document

## 1. Objective

Build a development workflow suitable for one owner/coordinator and automated coding agents without making the local workstation a permanent build/CI administration machine.

The platform must optimize both correctness and **iteration speed**.

## 2. Control/execution plane

```text
Owner / ChatGPT coordinator
          │
          ▼
GitHub — canonical control plane
(issue / branch / Draft PR / diff / checks / artifacts)
          │
          ▼
Self-hosted GitHub runner
          │
          ▼
Existing VPS — execution plane
(checkout / build / tests / corpus / packaging / benchmarks)
          │
          ▼
GitHub logs + artifacts
          │
          ▼
Owner workstation — acceptance endpoint
```

No new paid Business workspace, MCP control plane or second VPS is required by Foundation.

## 3. Canonical state

GitHub owns source code, canonical docs, issues/work items, branches/PRs, accepted ADRs, CI definitions, cleared fixtures and build/test evidence metadata.

VPS owns reproducible execution state and private reference data that must not enter Git.

Local workstation owns no unique canonical development state.

## 4. VPS roles

Existing VPS may host:

```text
runner service account
workspace/checkouts
build caches
private NPT/reference corpus
private site/policy fixtures
logs/artifact staging
optional local coding-agent runtime
```

Private corpora are referenced through configured paths/secrets and never committed accidentally.

## 5. Runner security baseline

Infrastructure Spike configures an unprivileged dedicated runner account.

Requirements:

- no production SCADA credentials;
- no blanket root requirement for normal jobs;
- repository-specific runner scope where practical;
- writable paths limited to workspace/cache/artifact areas;
- secrets only for jobs that require them;
- private corpus permissions explicitly controlled;
- untrusted fork workflow changes must not gain private corpora/secrets;
- cleanup/retention policy.

## 6. One-command development launcher

After platform selection, repository should expose one stable entry point, conceptually:

```text
./dev doctor
./dev build
./dev test core
./dev test ui
./dev test npt
./dev test switching
./dev gallery
./dev preview
./dev package
./dev full
```

A Windows wrapper may exist, but command semantics remain the same.

## 7. Environment pinning

Once stack is selected, record SDK/compiler/runtime versions, package-manager/lock files, build image/OS baseline, framework dependencies, tooling versions, packaging dependencies and private-corpus schema/version fingerprints where practical.

`works on VPS today` is not reproducibility evidence by itself.

## 8. Local machine role

The owner workstation should normally need only GitHub/ChatGPT access and ability to run/download a preview artifact.

Do not require the owner to SSH for each normal task, manually copy patches, coordinate Git from terminal, install full compiler stacks for acceptance or trigger complex CI manually.

SSH remains an infrastructure/admin escape hatch.

## 9. Development feedback tiers

### Tier 0 — static/local agent check

Fast formatting/lint/type/unit checks where cheap.

### Tier 1 — targeted PR check

Runs affected component lanes and produces fast feedback.

### Tier 2 — preview/visual acceptance

Builds runnable Gallery/app artifact or screenshots for owner review.

### Tier 3 — module/integration gates

Runs relevant domain/corpus/scenario suites after visible direction is accepted.

### Tier 4 — full/nightly/release

Cross-module, packaging, broad corpus, performance and release gates.

A small visible UI repair should normally reach Tier 2 before Tier 4.

## 10. Private NPT corpus lane

Repository CI uses synthetic/cleared fixtures.

Controlled private lane may run full NPT reference checks:

- XSDE parse/round-trip;
- XTABL round-trip;
- renderer comparison fixtures;
- signal catalog consistency;
- topology extraction experiments.

Do not publish proprietary files as artifacts.

## 11. Artifacts

Useful artifacts include UI Gallery preview, portable app preview, screenshot/visual report, benchmark report, test summaries, format/corpus diagnostics and packaging archives/checksums.

Retention is risk/value based.

## 12. Coding-agent role

Agents may modify the active branch/work item within scope and use runner checks.

They must not create duplicate work items, broaden scope silently, treat VPS workspace as canonical, merge/mark Ready without owner command, access unrelated credentials/data or rewrite normative rules without source/evidence review.

## 13. Infrastructure Spike acceptance

`INFRASTRUCTURE-SPIKE-001` is complete when:

```text
branch push
→ self-hosted runner checks exact head
→ deterministic test/build command
→ result/log + small artifact
→ coordinator reads status through GitHub
→ owner obtains/runs artifact without SSH
```

Also prove failure path and runner cleanup.

## 14. Portability

Initial preview/package should be self-contained as far as practical. Exact installer/update strategy is deferred until stack selection and packaging evidence.

Portable preview is prioritized because it shortens owner acceptance loops.

## 15. Cost constraint

Baseline assumes existing resources only:

- GitHub;
- existing VPS;
- current ChatGPT/coordination tools.

Any new paid service must demonstrate measurable value over this baseline before adoption.
