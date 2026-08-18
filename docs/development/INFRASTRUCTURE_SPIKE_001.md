# INFRASTRUCTURE-SPIKE-001 — GitHub ↔ development VPS

Статус: **ACCEPTANCE READY — GitHub/VPS runner PROVEN; Bridge v0.2 DEPLOYED; authenticated ChatGPT Action E2E PASS**  
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

Последний exact-head smoke перед final acceptance:

- workflow: `Infrastructure Smoke`;
- run ID: `32178488664`;
- job ID: `95845779180`;
- runner: `eep-development-vps`;
- exact head: `bd442d3ee4c0de39dd2c8c73a11aab90b6119ded`;
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
artifact id: 9339903901
size: 681 bytes
digest: sha256:ba7944eb76567aa09a21132dd40537cd3ef8a3df7303d6b08efe8f8620a5ea71
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

## 10. Development Bridge v0.2 — DEPLOYED / E2E PROVEN

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

Runtime application source deployed из exact commit:

```text
bd442d3ee4c0de39dd2c8c73a11aab90b6119ded
```

Owner deployment evidence:

```text
PASS: EEP Development Bridge v0.2 deployed
Bridge: active, authenticated, OpenAPI/docs hidden
Bridge state: writable only at /var/lib/eep-dev-bridge
Bridge repo view: explicit read-only bind inside ProtectHome=tmpfs
Repo sync: timer enabled, read-only deploy key remains isolated
```

После deployment подтверждено:

- `eep-dev-bridge.service`: `active`;
- GitHub runner service: `active`;
- `eep-workspace-sync.timer`: `active`;
- unauthenticated `/health` и `/status` требуют bearer auth;
- `/openapi.json`, `/docs`, `/redoc` не публикуются runtime service;
- `eepbridge` имеет read-only доступ к fixed repository root внутри service namespace;
- `eepbridge` не может писать repository root или `.git`;
- `eepbridge` не может читать deploy key `eep-workspace`;
- Bridge state writable только в dedicated `/var/lib/eep-dev-bridge`;
- audit `service_start` запись создаётся после старта;
- bounded task concurrency = 1;
- task timeout и bounded output реализованы;
- arbitrary shell endpoint отсутствует;
- allowed execution profiles ограничены `bridge_compile` и `bridge_selftest`.

Authenticated ChatGPT Action E2E после final permission repair доказал:

```text
health/status                    → PASS
workspace/status                 → PASS
repository/status                → PASS
repository read / clean tree     → PASS
origin/main resolution           → PASS
bridge_selftest exact ref        → PASS
requested_ref                    = bd442d3ee4c0de39dd2c8c73a11aab90b6119ded
resolved_sha                     = bd442d3ee4c0de39dd2c8c73a11aab90b6119ded
state                            = success
exit_code                        = 0
unit tests                       = 9 / OK
```

## 11. Failure/rollback evidence Bridge deployment

Hardening deployment специально не считался успешным до реального runtime start и authenticated E2E.

Во время доводки были обнаружены и исправлены три инфраструктурных дефекта:

1. **Git cross-owner safety** — `eepbridge` получил `dubious ownership` при работе с checkout владельца `eep-workspace`. Исправлено точечным `git -c safe.directory=<fixed-root>`; глобальный wildcard `safe.directory=*` не используется.
2. **systemd read-only namespace** — при `ProtectSystem=strict` `/var/lib/eep-dev-bridge/audit.jsonl` был read-only, несмотря на корректного Linux owner. Исправлено через dedicated `StateDirectory=eep-dev-bridge` + `ReadWritePaths=/var/lib/eep-dev-bridge`, а не отключением systemd sandbox.
3. **systemd home isolation** — host-side доступ `eepbridge` к repo проходил, но внутри service namespace `ProtectHome` блокировал `/home/eep-workspace/...`. Исправлено сохранением home isolation через `ProtectHome=tmpfs` и точечным read-only bind fixed repository path, без раскрытия deploy-key каталога.

Первые два failure path доказали rollback: installer остановил rollout и восстановил предыдущий working Bridge. После третьего repair authenticated repository checks прошли внутри реально работающего service namespace.

Installer выполняет preflight Python compile до замены working runtime и содержит rollback source/systemd override при ошибке deployment.

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

POST task/cancel operations в canonical Action schema явно помечены как low-impact/non-consequential для bounded development contour, чтобы UI мог сохранять persistent approval; это не расширяет API permissions и не добавляет arbitrary execution capability.

## 13. Issue #3 acceptance matrix

| # | Criterion | State | Evidence |
|---|---|---|---|
| 1 | VPS fetch/checkout private repo | PASS | `eep-workspace` deploy-key clone/fetch |
| 2 | checked-out SHA vs expected | PASS | exact checkout + `git rev-parse HEAD` |
| 3 | runner Online | PASS | jobs received/executed |
| 4 | PR workflow picked by intended runner | PASS | `eep-development-vps` |
| 5 | exact head + runner identity recorded | PASS | workflow logs/artifact |
| 6 | deterministic smoke succeeds | PASS | run `32178488664` |
| 7 | deliberate failure diagnostic | PASS | controlled `exit 17` + asserted outcome |
| 8 | small artifact in GitHub | PASS | artifact `9339903901` |
| 9 | result inspectable without SSH | PASS | GitHub + ChatGPT Action evidence |
| 10 | runner returns usable | PASS | successful rerun after service restart |
| 11 | existing Bridge functional | PASS | authenticated ChatGPT Action E2E + `bridge_selftest` success |
| 12 | no broad long-lived GitHub credential on runner | PASS | deploy key absent + read-only `GITHUB_TOKEN` |

Additional Bridge-hardening acceptance:

| Criterion | State | Evidence |
|---|---|---|
| fixed repository/workspace root | PASS | `/home/eep-workspace/workspace/ElectricalEngineeringPlatform` |
| typed/allowlisted read operations | PASS | v0.2 API source + E2E |
| no arbitrary shell | PASS | fixed task profiles + static workflow check |
| path escape prevention | PASS | unit tests for absolute/parent/symlink escape |
| task ID/status/log/cancel | PASS | v0.2 API + Action E2E |
| timeout/output bounds/concurrency 1 | PASS | v0.2 implementation |
| audit state writable under hardened systemd | PASS | deployment audit-write verification |
| repo credential isolation | PASS | owner deployment checks |
| service-namespace repository access | PASS | authenticated workspace/repository E2E |
| authenticated ChatGPT Action end-to-end | PASS | exact-ref `bridge_selftest`, 9 tests OK |
| token rotation/recovery procedure | PASS | `DEVELOPMENT_BRIDGE_OPERATIONS.md` |

## 14. Final acceptance state

`INFRASTRUCTURE-SPIKE-001` technical acceptance criteria выполнены. Финальный merge остаётся отдельным owner-controlled действием.

Формальный contour доказан:

```text
GitHub PR exact head
→ repository-level self-hosted runner
→ eod-development-vps
→ deterministic checks/failure diagnostics/artifact
→ GitHub evidence
```

Интерактивный contour доказан:

```text
ChatGPT Action
→ bearer-authenticated bounded Bridge
→ fixed read-only repository view
→ allowlisted exact-ref task
→ bounded task status/log
→ bridge_selftest success
```

PR #4 остаётся Draft до явного owner merge command. После merge `Closes #3` закрывает issue автоматически.

Следующий product work item после merge:

`PLATFORM-STACK-SPIKE-001`.
