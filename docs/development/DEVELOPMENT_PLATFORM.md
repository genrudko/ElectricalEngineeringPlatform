# Development Platform

Статус: канонический Foundation-документ

## 1. Цель

Development workflow должен подходить для owner/coordinator + automated coding agents и не превращать local workstation в permanent build/CI administration machine.

Оптимизируются одновременно:

- correctness;
- auditability;
- iteration speed;
- минимальная routine SSH/manual terminal work владельца.

## 2. Два development contours

После Foundation используются два взаимодополняющих контура.

### 2.1. Интерактивный контур

```text
Owner / ChatGPT Project chat
          │
          ▼
@EEP Development Bridge
Custom GPT Action
          │ HTTPS + Bearer
          ▼
EEP Development Bridge
          │
          ▼
Existing VPS
inspect / build / test / benchmark / preview
```

Этот контур предназначен для fast interactive work: проверить environment, прочитать bounded workspace state, запустить targeted task, получить log/result без routine owner SSH.

### 2.2. Формальный verification contour

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
GitHub checks + artifacts
          │
          ▼
Owner acceptance
```

GitHub остаётся source of truth. Development Bridge не заменяет formal exact-head CI.

## 3. Доказанный Bridge baseline — 2026-08-18

Фактически проверено на текущем ChatGPT Plus и existing VPS:

1. Custom GPT Action вызывает внешний HTTPS API.
2. Bearer authentication работает: no token → `401`, valid token → `200`.
3. Caddy получил valid Let's Encrypt certificate.
4. FastAPI/Uvicorn слушает только localhost `127.0.0.1:8788`.
5. Service работает под непривилегированным user `eepbridge`.
6. GPT Action `/health` реально дошёл до VPS; server log зафиксировал `GET /health ... 200 OK`.
7. Custom GPT работает внутри existing ChatGPT Project chat.
8. В том же Project chat работает GitHub connector.
9. GitHub query и subsequent bridge query работают последовательно в одном conversation context.

Следствие: ChatGPT Business, MCP или отдельно оплачиваемый OpenAI API **не являются required dependencies** текущей Development Platform.

## 4. Canonical state

GitHub владеет:

- source code;
- canonical docs;
- issues/work items;
- branches/PRs;
- accepted ADRs;
- CI definitions;
- cleared fixtures;
- build/test evidence metadata.

VPS владеет reproducible execution state и private reference data, которым не место в Git.

Local workstation не содержит unique canonical development state.

## 5. Роли VPS

Existing VPS может host:

```text
EEP Development Bridge
self-hosted runner service account
workspace/checkouts
build caches
private NPT/reference corpus
logs/artifact staging
optional local coding-agent runtime
```

Internal enterprise/site instructions не входят в shared VPS corpus.

## 6. Development Bridge security/contract baseline

Bridge должен оставаться bounded development API, а не remote shell.

Обязательные свойства следующей версии:

- dedicated unprivileged service account;
- fixed allowed repository/workspace roots;
- typed/allowlisted operations;
- path normalization/escape prevention;
- explicit task profiles для build/tests/benchmark/preview;
- timeout/cancellation;
- task ID/status/log;
- bounded output size;
- audit log;
- concurrency limits;
- token rotation;
- no privileged production credentials.

Недопустимый design:

```text
runCommand(arbitraryString)
```

Предпочтительный contract:

```text
getWorkspaceStatus()
readWorkspaceFile(path)
searchWorkspace(query)
getGitDiff()
runBuild(profile)
runTests(suite)
runBenchmark(profile)
buildPreview(profile)
getTaskStatus(id)
getTaskLog(id)
listArtifacts()
```

Exact operation names определяются implementation work item.

## 7. Runner security baseline

`INFRASTRUCTURE-SPIKE-001` настраивает отдельный unprivileged runner account.

Requirements:

- no production SCADA credentials;
- no blanket root requirement для normal jobs;
- repository-specific runner scope where practical;
- writable paths limited to workspace/cache/artifact areas;
- secrets only for jobs that require them;
- private corpus permissions explicitly controlled;
- untrusted fork workflow changes не получают private corpora/secrets;
- cleanup/retention policy.

## 8. One-command development launcher

После platform selection repository должен иметь stable entry point, conceptually:

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

Windows wrapper может существовать отдельно, но command semantics сохраняются.

Bridge и GitHub runner вызывают тот же underlying deterministic launcher, а не две независимые build systems.

## 9. Environment pinning

После stack selection фиксируются:

- SDK/compiler/runtime versions;
- package-manager/lock files;
- build OS/image baseline;
- framework dependencies;
- tooling versions;
- packaging dependencies;
- private-corpus schema/version fingerprints where practical.

`works on VPS today` само по себе не является reproducibility evidence.

## 10. Local machine role

Owner workstation обычно требует только:

- ChatGPT/GitHub access;
- возможность скачать/run preview artifact;
- manual acceptance.

Не требовать routine:

- SSH для каждого task;
- manual patch copy;
- terminal Git coordination;
- full compiler stack на owner PC;
- сложный manual CI trigger.

SSH остаётся infrastructure/admin escape hatch.

## 11. Development feedback tiers

### Tier 0 — static/fast checks

Formatting/lint/type/unit checks where cheap.

### Tier 1 — targeted task

Affected component lane через Bridge или runner.

### Tier 2 — preview/visual acceptance

Runnable Gallery/app artifact или screenshots для owner review.

### Tier 3 — module/integration gates

Relevant domain/corpus/scenario suites после acceptance visible direction.

### Tier 4 — full/nightly/release

Cross-module, packaging, broad corpus, performance и release gates.

Небольшой visible UI repair обычно должен достигать Tier 2 до Tier 4.

## 12. Private NPT corpus lane

Repository CI использует synthetic/cleared fixtures.

Controlled private lane может выполнять full NPT checks:

- XSDE parse/round-trip;
- XTABL round-trip;
- renderer comparison fixtures;
- signal catalog consistency;
- topology extraction experiments.

Proprietary files не публикуются как artifacts.

## 13. Artifacts

Useful artifacts:

- UI Gallery preview;
- portable app preview;
- screenshot/visual report;
- benchmark report;
- test summaries;
- format/corpus diagnostics;
- packaging archives/checksums.

Retention определяется risk/value.

## 14. Coding-agent role

Agents могут менять active branch/work item within scope и использовать Bridge/runner checks.

Они не должны:

- создавать duplicate work items;
- silently broaden scope;
- считать VPS workspace canonical;
- merge/mark Ready без owner command;
- access unrelated credentials/data;
- rewrite normative semantics без source/evidence review.

## 15. `INFRASTRUCTURE-SPIKE-001` acceptance

Spike complete, когда доказано:

```text
ChatGPT Project
→ bounded Bridge operation
→ deterministic task/result/log

и

branch push
→ self-hosted runner verifies exact head
→ deterministic test/build
→ GitHub check + artifact
→ owner obtains/reviews result without SSH
```

Также должны быть доказаны failure path, cleanup и isolation.

## 16. Portability

Initial preview/package по возможности self-contained.

Exact installer/update strategy deferred до stack selection и packaging evidence.

Portable preview приоритетен, потому что сокращает owner acceptance loop.

## 17. Cost constraint

Baseline использует уже имеющиеся ресурсы:

- ChatGPT Plus;
- GitHub;
- existing VPS.

Новый mandatory paid service принимается только при доказанной measurable value сверх этого baseline.