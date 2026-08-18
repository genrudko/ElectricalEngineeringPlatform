# INFRASTRUCTURE-SPIKE-001 — GitHub ↔ development VPS

Статус: **ACTIVE — GitHub/VPS runner baseline PROVEN; Bridge v0.2 DEPLOYED; ChatGPT Action E2E remains**  
Issue: #3  
Branch: `infrastructure/infrastructure-spike-001`  
Draft PR: #4

## 1. Цель

Связать существующий development VPS с `genrudko/ElectricalEngineeringPlatform` так, чтобы formal development evidence проходил по цепочке:

```text
GitHub
→ exact repository state
→ self-hosted runner / exact checkout
→ VPS build/test execution
→ GitHub checks/artifacts
→ owner acceptance без routine SSH
```

GitHub остаётся canonical control plane. VPS остаётся execution plane.

Одновременно `INFRASTRUCTURE-SPIKE-001` сохраняет отдельный интерактивный contour через `EEP Development Bridge`; его production-quality hardening выполняется в том же work item после доказанного runner baseline.

## 2. Доказанный runtime layout — 2026-08-18

На existing VPS используются раздельные identities:

```text
eep-workspace
→ persistent fixed interactive checkout
→ read-only repository deploy key

eep-runner
→ GitHub Actions self-hosted runner
→ no persistent repository deploy key / PAT

eepbridge
→ EEP Development Bridge service
→ separate bearer credential / service boundary
```

Фактически подтверждено:

- host: `eod-development-vps`;
- Ubuntu 24.04.4 LTS;
- `eep-workspace`: UID 1003;
- `eep-runner`: UID 1002;
- `eepbridge`: UID 995;
- runner application: `2.336.0`;
- runner name: `eep-development-vps`;
- runner labels: `self-hosted`, `linux`, `x64`, `eep-vps`;
- runner work root: `/home/eep-runner/actions-runner`;
- interactive repository workspace: `/home/eep-workspace/workspace/ElectricalEngineeringPlatform`;
- Bridge application root: `/opt/eep-dev-bridge`;
- Bridge state root: `/var/lib/eep-dev-bridge`;
- Bridge bind: `127.0.0.1:8788` behind existing Caddy TLS endpoint.

## 3. Phase A — безопасный repository access с VPS — PROVEN

Для интерактивного workspace используется repository-scoped read-only deploy key без generic owner PAT.

Фактически доказано:

```text
read-only deploy key
→ private genrudko/ElectricalEngineeringPlatform
→ eep-workspace
→ git ls-remote / clone / fetch
→ fixed VPS workspace
```

Во время первичной проверки:

- GitHub `main`: `c62a67064145610cc41711c21b79d0cc0eecb5b9`;
- remote `refs/heads/main`: тот же SHA;
- local checkout HEAD: тот же SHA;
- workspace status: clean `main...origin/main`.

Deploy key после проверки удалён из `eep-runner` и остаётся только у `eep-workspace`.

Правила baseline:

- deploy key отдельный для этого repository;
- write access для deploy key выключен;
- VPS не используется для routine push/merge;
- expected/current SHA проверяются явно;
- canonical edits проходят через GitHub work-item branch/PR.

## 4. Phase B — repository-level self-hosted runner — PROVEN

Runner зарегистрирован как repository-level self-hosted runner private repository и установлен как `systemd` service:

```text
actions.runner.genrudko-ElectricalEngineeringPlatform.eep-development-vps.service
```

Подтверждено:

- service установлен и enabled;
- service работает от `eep-runner`, не root;
- registration token использован только при регистрации;
- постоянный deploy key/PAT у `eep-runner` отсутствует;
- workflow `GITHUB_TOKEN` имеет только `contents: read` + metadata read;
- `needrestart` настроен не перезапускать runner service посреди job;
- ручной `systemctl restart` завершился состоянием `active`;
- после restart GitHub повторно отправил job, runner его принял и успешно завершил.

Actual full host reboot специально не выполнялся: VPS shared с другими development services. `systemd enabled` + service restart/re-acquisition job доказаны; реальный reboot recovery проверяется при ближайшем естественном maintenance reboot, а не создаётся искусственно без product value.

## 5. Formal exact-head contract — PROVEN

Canonical infrastructure smoke workflow:

`.github/workflows/infrastructure-smoke.yml`

Для `pull_request` workflow выполняет две независимые проверки:

1. GitHub API branch head должен совпасть с `github.event.pull_request.head.sha`;
2. repository checkout выполняется на exact PR head, после чего `git rev-parse HEAD` обязан совпасть с expected head.

`actions/checkout` использует:

- exact event head;
- `fetch-depth: 1`;
- `persist-credentials: false`;
- clean workspace preparation.

External Actions в persistent self-hosted workflow pinned на immutable commit SHA, а не на floating tags.

Важно: `GITHUB_SHA` для `pull_request` может представлять synthetic merge ref, поэтому formal PR-head authority записывается отдельно как `github.event.pull_request.head.sha` и сравнивается с actual branch ref + checked-out `HEAD`.

## 6. Smoke / failure / artifact evidence — PROVEN

Current successful evidence для Bridge deployment source commit:

- workflow: `Infrastructure Smoke`;
- run ID: `32175626537`;
- job ID: `95836766611`;
- runner: `eep-development-vps`;
- exact head: `45047e86974bd46fe55a3209bd5a3667375c15c3`;
- job conclusion: `success`.

Успешно выполнены:

- runner identity check;
- GitHub remote exact-head check;
- exact event-head checkout;
- checked-out `HEAD` verification;
- clean Git worktree verification;
- Bridge installer syntax validation;
- Bridge Python compile + 9 bounded-policy unit tests;
- static rejection of arbitrary-command primitives;
- runner persistent credential isolation;
- `EEP Development Bridge` service health check;
- deliberate failure (`exit 17`) с ожидаемым readable diagnostic path;
- explicit assertion, что deliberate step действительно имел outcome `failure`;
- evidence file + SHA-256;
- GitHub artifact upload;
- GitHub job summary;
- explicit temporary evidence cleanup.

Current artifact:

```text
name: infrastructure-smoke-evidence
artifact id: 9338897252
size: 683 bytes
digest: sha256:e87113cbe4c68b4a155f316e41503e162cda71afb9282f48e9c81221817273da
retention: 3 days
```

Artifact содержит только infrastructure metadata/evidence; private corpus/secrets туда не попадают.

## 7. Restart/recovery evidence — PROVEN для service restart

После owner-triggered:

```text
systemctl restart actions.runner.genrudko-ElectricalEngineeringPlatform.eep-development-vps.service
```

service вернулся в `active`.

После этого через GitHub connector был запрошен re-run уже существующего job. Новый attempt снова выполнился на `eep-development-vps` и завершился `success`, подтвердив, что runner после restart:

- reconnects к GitHub;
- получает новые jobs;
- сохраняет identity/isolation;
- продолжает exact-head verification;
- публикует artifact.

## 8. Runner lifecycle / operator procedure

Normal owner workflow не требует SSH. SSH остаётся infrastructure escape hatch.

Read-only status:

```bash
sudo systemctl status actions.runner.genrudko-ElectricalEngineeringPlatform.eep-development-vps.service
```

Controlled restart:

```bash
sudo systemctl restart actions.runner.genrudko-ElectricalEngineeringPlatform.eep-development-vps.service
```

Stop/start:

```bash
sudo systemctl stop actions.runner.genrudko-ElectricalEngineeringPlatform.eep-development-vps.service
sudo systemctl start actions.runner.genrudko-ElectricalEngineeringPlatform.eep-development-vps.service
```

Runner application root:

```text
/home/eep-runner/actions-runner
```

При decommission/re-registration сначала runner удаляется из repository Settings → Actions → Runners, затем выполняется штатное `config.sh remove`/`svc.sh uninstall` с новым short-lived removal token, после чего local runner directory может быть удалён. Broad PAT для этого не вводится.

## 9. Execution hygiene

Baseline rules:

- `eep-runner` не хранит deploy key/PAT;
- `eep-workspace` имеет только read-only repository key;
- job получает short-lived workflow `GITHUB_TOKEN` с минимальными permissions;
- persistent runner machine считается privileged development execution surface;
- public/fork-untrusted workload на runner не допускается;
- private NPT/reference corpus остаётся вне Git;
- secrets/private corpus не публикуются как artifacts;
- runner temp evidence явно очищается smoke workflow;
- repository checkout перед formal job очищается;
- fixed persistent workspace не считается canonical state.

## 10. Development Bridge v0.2 — DEPLOYED

Bridge и runner остаются разными contours:

```text
Interactive:
ChatGPT Project → bounded Bridge → VPS

Formal:
GitHub Actions → self-hosted runner → VPS
```

Bridge v0.2 source находится в:

- `tools/development_bridge/app.py`;
- `tools/development_bridge/bridge_core.py`;
- `tools/development_bridge/tests/test_bridge_core.py`;
- `tools/development_bridge/openapi-action.yaml`;
- `scripts/install_development_bridge.sh`.

Runtime deployed из exact commit:

```text
45047e86974bd46fe55a3209bd5a3667375c15c3
```

Owner deployment evidence:

```text
PASS: EEP Development Bridge v0.2 deployed
Backup: /var/backups/eep-dev-bridge/20260818T192334Z
Bridge: active, authenticated, OpenAPI/docs hidden
Bridge state: writable only at /var/lib/eep-dev-bridge
Repo sync: timer enabled, read-only deploy key remains isolated
```

После deployment дополнительно подтверждено:

- `eep-dev-bridge.service`: `active`;
- GitHub runner service: `active`;
- `eep-workspace-sync.timer`: `active`;
- unauthenticated `/health` и `/status` требуют bearer auth;
- `/openapi.json`, `/docs`, `/redoc` не публикуются runtime service;
- `eepbridge` имеет read-only доступ к fixed repository root;
- `eepbridge` не может писать repository root или `.git`;
- `eepbridge` не может читать deploy key `eep-workspace`;
- Bridge state writable только в dedicated `/var/lib/eep-dev-bridge` через systemd hardening exception;
- audit `service_start` запись создаётся после старта;
- bounded task concurrency = 1;
- task timeout и bounded output реализованы;
- arbitrary shell endpoint отсутствует;
- allowed execution profiles ограничены `bridge_compile` и `bridge_selftest`.

## 11. Failure/rollback evidence Bridge deployment

Hardening deployment специально не считался успешным до реального runtime start.

Во время доводки были обнаружены и исправлены два инфраструктурных дефекта:

1. **Git cross-owner safety** — `eepbridge` получил `dubious ownership` при работе с checkout владельца `eep-workspace`. Исправлено точечным `git -c safe.directory=<fixed-root>`; глобальный wildcard `safe.directory=*` не используется.
2. **systemd read-only namespace** — при `ProtectSystem=strict` `/var/lib/eep-dev-bridge/audit.jsonl` был read-only, несмотря на корректного Linux owner. Исправлено через dedicated `StateDirectory=eep-dev-bridge` + `ReadWritePaths=/var/lib/eep-dev-bridge`, а не отключением systemd sandbox.

В обоих случаях installer остановил rollout и восстановил предыдущий working Bridge. После второго исправления deployment завершился PASS.

Installer теперь выполняет preflight Python compile до замены working runtime и содержит rollback source/systemd override при ошибке deployment.

## 12. Bounded Bridge API contract

Runtime OpenAPI intentionally hidden; canonical ChatGPT Action schema хранится отдельно в Git:

`tools/development_bridge/openapi-action.yaml`

Allowlisted surface:

```text
GET  /health
GET  /status
GET  /workspace/status
GET  /repository/status
GET  /git/status
GET  /git/diff
GET  /files
GET  /search
POST /tasks/run
GET  /tasks/{task_id}
GET  /tasks/{task_id}/log
POST /tasks/{task_id}/cancel
```

Execution API принимает только typed profile + validated ref. Допустимый ref — full lowercase 40-char SHA либо bounded `origin/<branch>` без Git revision operators.

File read API ограничен fixed repository root, нормализует path, блокирует absolute/path traversal/symlink escape и возвращает только bounded UTF-8 output.

## 13. Issue #3 acceptance matrix

| # | Criterion | State | Evidence |
|---|---|---|---|
| 1 | VPS fetch/checkout private repo | PASS | `eep-workspace` deploy-key clone/fetch |
| 2 | checked-out SHA vs expected | PASS | exact checkout + `git rev-parse HEAD` |
| 3 | runner Online | PASS | jobs received/executed |
| 4 | PR workflow picked by intended runner | PASS | `eep-development-vps` |
| 5 | exact head + runner identity recorded | PASS | workflow logs/artifact |
| 6 | deterministic smoke succeeds | PASS | run `32175626537` |
| 7 | deliberate failure diagnostic | PASS | controlled `exit 17` + asserted outcome |
| 8 | small artifact in GitHub | PASS | artifact `9338897252` |
| 9 | result inspectable without SSH | PASS | GitHub connector/actions evidence |
| 10 | runner returns usable | PASS | successful rerun after service restart |
| 11 | existing Bridge functional | PASS | Bridge v0.2 deployment PASS + service active |
| 12 | no broad long-lived GitHub credential on runner | PASS | deploy key absent + read-only `GITHUB_TOKEN` |

Additional Bridge-hardening acceptance:

| Criterion | State | Evidence |
|---|---|---|
| fixed repository/workspace root | PASS | runtime `/home/eep-workspace/workspace/ElectricalEngineeringPlatform` |
| typed/allowlisted read operations | PASS | v0.2 API source + tests |
| no arbitrary shell | PASS | fixed task profiles + static workflow check |
| path escape prevention | PASS | unit tests for absolute/parent/symlink escape |
| task ID/status/log/cancel | PASS | v0.2 API contract |
| timeout/output bounds/concurrency 1 | PASS | v0.2 implementation |
| audit state writable under hardened systemd | PASS | deployment audit-write verification |
| repo credential isolation | PASS | owner deployment checks |
| authenticated ChatGPT Action end-to-end | **PENDING** | requires Action schema update + Project-chat invocation |
| token rotation/recovery procedure | **PENDING DOC/EVIDENCE** | do not rotate working token solely for ceremony |

## 14. Следующий шаг внутри work item

Следующий шаг — не SSH и не новый server patch.

Нужно обновить ChatGPT Action на schema:

`tools/development_bridge/openapi-action.yaml`

После этого из Project-чата выполнить authenticated E2E:

```text
getBridgeHealth
→ getServerStatus
→ getWorkspaceStatus
→ getRepositoryStatus
→ readWorkspaceFile
→ searchWorkspace
→ runBridgeTask(profile=bridge_selftest, ref=<exact SHA/origin branch>)
→ poll task
→ read bounded log
```

Если этот contour проходит, остаётся зафиксировать token rotation/recovery procedure и провести owner acceptance всего `INFRASTRUCTURE-SPIKE-001`.

PR #4 остаётся **DRAFT / DO NOT MERGE** до явной owner acceptance.

После полного acceptance `INFRASTRUCTURE-SPIKE-001` следующий product work item:

`PLATFORM-STACK-SPIKE-001`.
