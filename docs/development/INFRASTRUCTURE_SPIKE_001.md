# INFRASTRUCTURE-SPIKE-001 — GitHub ↔ development VPS

Статус: **ACTIVE — GitHub/VPS runner baseline PROVEN; Bridge hardening remains**  
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
- interactive repository workspace: `/home/eep-workspace/workspace/ElectricalEngineeringPlatform`.

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

Важно: `GITHUB_SHA` для `pull_request` может представлять synthetic merge ref, поэтому formal PR-head authority записывается отдельно как `github.event.pull_request.head.sha` и сравнивается с actual branch ref + checked-out `HEAD`.

## 6. Smoke / failure / artifact evidence — PROVEN

Первый baseline workflow доказал execution path. После уточнения exact-checkout contract current successful evidence:

- workflow: `Infrastructure Smoke`;
- run ID: `32172549233`;
- job ID: `95826882592`;
- runner: `eep-development-vps`;
- head: `73c50b47c3408d63e0455a63c15584dda661c324`;
- job conclusion: `success`.

Успешно выполнены:

- runner identity check;
- GitHub remote exact-head check;
- exact event-head checkout;
- checked-out `HEAD` verification;
- clean Git worktree verification;
- runner persistent credential isolation;
- `EEP Development Bridge` service health check;
- deliberate failure (`exit 17`) с ожидаемым readable diagnostic path;
- explicit assertion, что deliberate step действительно имел outcome `failure`;
- evidence file + SHA-256;
- GitHub artifact upload;
- GitHub job summary;
- explicit temporary evidence cleanup.

Artifact:

```text
name: infrastructure-smoke-evidence
artifact id: 9337793243
size: 667 bytes
digest: sha256:b9594875fc5022585ea7b460f7ce52191076ecf94c3730297ae83501235bf59f
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

## 10. Development Bridge compatibility

Existing `EEP Development Bridge` runner не заменяет.

```text
Interactive:
ChatGPT Project → bounded Bridge → VPS

Formal:
GitHub Actions → self-hosted runner → VPS
```

Infrastructure smoke проверяет, что `eep-dev-bridge.service` остаётся active после runner installation/jobs/restart.

Разделение identities/credentials обязательно сохраняется.

## 11. Что ещё осталось в полном `INFRASTRUCTURE-SPIKE-001`

GitHub↔VPS runner subphase доказана. Но canonical `NEXT_WORK_ITEMS.md` также требует production-quality hardening интерактивного Bridge.

Открытый Bridge scope:

- fixed allowed repository/workspace roots;
- typed/allowlisted read operations;
- bounded execution profiles вместо arbitrary shell;
- path normalization/escape prevention;
- task IDs/status/logs для long tasks;
- timeout/cancellation;
- output truncation/size limits;
- concurrency limit;
- audit log;
- token rotation/recovery procedure;
- deterministic task profiles, которые позже разделяются с runner через `./dev` contract.

Следовательно, **runner baseline не является автоматическим acceptance всего `INFRASTRUCTURE-SPIKE-001`**. PR #4 остаётся Draft до завершения Bridge hardening и owner acceptance либо до отдельного owner decision явно разделить work item.

## 12. Issue #3 acceptance matrix — GitHub/VPS part

| # | Criterion | State | Evidence |
|---|---|---|---|
| 1 | VPS fetch/checkout private repo | PASS | `eep-workspace` deploy-key clone/fetch |
| 2 | checked-out SHA vs expected | PASS | exact checkout + `git rev-parse HEAD` |
| 3 | runner Online | PASS | jobs received/executed |
| 4 | PR workflow picked by intended runner | PASS | `eep-development-vps` |
| 5 | exact head + runner identity recorded | PASS | workflow logs/artifact |
| 6 | deterministic smoke succeeds | PASS | run `32172549233` |
| 7 | deliberate failure diagnostic | PASS | controlled `exit 17` + asserted outcome |
| 8 | small artifact in GitHub | PASS | artifact `9337793243` |
| 9 | result inspectable without SSH | PASS | GitHub connector/actions evidence |
| 10 | runner returns usable | PASS | successful rerun after service restart |
| 11 | existing Bridge functional | PASS | service health check in job |
| 12 | no broad long-lived GitHub credential on runner | PASS | deploy key absent + read-only `GITHUB_TOKEN` |

## 13. Следующий шаг внутри work item

Перейти к bounded `EEP Development Bridge` hardening, не создавая новый issue/branch/PR.

После полного acceptance `INFRASTRUCTURE-SPIKE-001` следующий product work item:

`PLATFORM-STACK-SPIKE-001`.
