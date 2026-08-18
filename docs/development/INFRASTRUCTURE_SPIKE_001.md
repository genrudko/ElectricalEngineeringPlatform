# INFRASTRUCTURE-SPIKE-001 — GitHub ↔ development VPS

Статус: ACTIVE work-item contract  
Issue: #3  
Branch: `infrastructure/infrastructure-spike-001`

## 1. Цель

Связать существующий development VPS с `genrudko/ElectricalEngineeringPlatform` так, чтобы:

```text
GitHub
→ exact repository state
→ self-hosted runner / deterministic checkout
→ VPS build/test execution
→ GitHub checks/artifacts
→ owner acceptance без routine SSH
```

GitHub остаётся canonical control plane. VPS остаётся execution plane.

## 2. Phase A — безопасный repository access с VPS

Для интерактивного workspace требуется repository-scoped read access без generic owner PAT.

Предпочтительный baseline:

```text
read-only deploy key
→ private repository
→ fixed VPS workspace
→ fetch/checkout
```

Правила:

- ключ отдельный для этого repository;
- write access для deploy key выключен;
- private key доступен только выделенному service user;
- VPS не используется для routine push/merge;
- expected/current SHA проверяются явно;
- canonical edits по-прежнему проходят через GitHub work-item branch/PR.

## 3. Phase B — repository-level self-hosted runner

Runner разворачивается на том же VPS, но отдельным непривилегированным service user.

Target labels:

```text
self-hosted
linux
x64
eep-vps
```

Требования:

- repository-level runner только для private `ElectricalEngineeringPlatform`;
- systemd service;
- automatic restart/start after reboot;
- GitHub registration token используется только при регистрации и не хранится как постоянный credential;
- workflow `GITHUB_TOKEN` получает минимальные permissions;
- no broad PAT в runner environment;
- runner не получает unrelated production/SCADA secrets;
- private NPT/reference corpus остаётся вне Git и доступен только controlled jobs при явной необходимости.

## 4. Security model

Self-hosted runner исполняет workflow code на persistent machine и поэтому считается privileged development execution surface.

Не допускается:

- public/fork-untrusted workload на этом runner;
- arbitrary repository write token на VPS;
- запуск runner от root;
- shared credentials с `eepbridge`;
- публикация private corpus/secrets как artifacts;
- implicit trust к workspace после failed/untrusted job.

## 5. Exact-head contract

Каждый formal job должен записывать как минимум:

```text
repository
ref
GITHUB_SHA
checked-out HEAD
runner name
runner labels/platform
workflow run id
```

Job падает, если checked-out `HEAD` не совпадает с expected `GITHUB_SHA`.

## 6. Initial smoke workflow

Первый workflow должен доказать только инфраструктуру:

1. job действительно попал на `eep-vps`;
2. checkout exact head;
3. записаны environment metadata;
4. deterministic smoke task успешно выполнен;
5. создан маленький artifact;
6. deliberate failure variant даёт читаемую диагностику.

До выбора Avalonia/Qt product build в smoke workflow не нужен.

## 7. Runner lifecycle

Нужно доказать:

- install;
- start;
- status;
- restart;
- reboot recovery;
- job cleanup;
- safe removal/re-registration;
- runner app update behavior;
- Ubuntu `needrestart` не должен рестартовать runner посреди job.

## 8. Development Bridge compatibility

Уже работающий `EEP Development Bridge` не заменяется runner'ом.

```text
Interactive:
ChatGPT Project → bounded Bridge → VPS

Formal:
GitHub Actions → self-hosted runner → VPS
```

Runner и Bridge используют отдельные service identities/credentials и по мере появления общего `./dev` launcher вызывают одинаковые deterministic task profiles.

## 9. Acceptance

Spike принят, когда выполнены все пункты issue #3, включая:

- read-only repository fetch с VPS;
- runner `Online`;
- exact-head workflow;
- success + deliberate failure evidence;
- artifact доступен через GitHub;
- owner/coordinator читает результат без SSH;
- existing Bridge остаётся healthy;
- broad long-lived GitHub credential отсутствует на VPS.

## 10. Следующий work item

После acceptance:

`PLATFORM-STACK-SPIKE-001`.