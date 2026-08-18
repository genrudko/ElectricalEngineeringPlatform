# ADR 0004 — GitHub Control Plane + Existing VPS Execution Plane

Статус: **ACCEPTED по owner direction / зафиксирован в `UNIFIED-FOUNDATION-001`**  
Дата: 2026-08-18

## Контекст

Предыдущие workflows становились operationally expensive: local manual patching, routine SSH intervention, broad CI до visual acceptance и слишком много infrastructure steps для small changes.

У владельца уже есть GitHub, ChatGPT Plus и existing VPS; новый mandatory paid orchestration platform для normal development не требуется.

Во время Foundation был отдельно проверен прямой ChatGPT→VPS development bridge.

## Решение

Использовать два complementary contours.

### Интерактивный contour

```text
ChatGPT Plus Project chat
→ @Custom GPT Action
→ HTTPS/Bearer
→ EEP Development Bridge
→ existing VPS
```

Назначение: fast inspect/build/test/preview tasks без routine owner SSH.

### Формальный contour

```text
GitHub = canonical control plane
→ self-hosted runner on existing VPS
→ exact-head build/test/benchmark/package
→ GitHub checks/artifacts
→ owner acceptance
```

Owner workstation остаётся acceptance endpoint.

Local SSH — infrastructure/admin escape hatch, а не normal workflow.

CI risk-based; UI visual acceptance по возможности происходит до expensive unrelated gates.

## Доказанное evidence 2026-08-18

Практический test подтвердил:

- Custom GPT Action работает на текущем ChatGPT Plus;
- endpoint опубликован по HTTPS через Caddy;
- Let's Encrypt certificate получен успешно;
- bridge FastAPI слушает localhost;
- service работает под unprivileged `eepbridge` account;
- Bearer authentication работает (`401` without token / `200` with token);
- GPT Action реально дошёл до VPS: `GET /health ... 200 OK` зафиксирован server log;
- Custom GPT Action работает внутри existing ChatGPT Project chat;
- GitHub connector доступен в том же Project conversation;
- GitHub query и subsequent bridge query работают последовательно.

Следствие: Business/MCP/OpenAI API не являются required dependencies current baseline.

## Ограничение Bridge architecture

Development Bridge — bounded typed development API, а не universal remote shell.

Execution operations задаются explicit allowlist/profiles и должны иметь fixed workspace, timeout, audit и controlled output.

## Последствия

Положительные:

- не требуется новый mandatory paid control service;
- build toolchains/corpora остаются вне owner workstation;
- private NPT corpus может тестироваться на controlled VPS;
- GitHub остаётся auditable canonical state;
- interactive Bridge сокращает turnaround time;
- self-hosted runner даёт formal exact-head evidence;
- owner не должен routinely работать через SSH.

Costs/risks:

- bridge hardening/maintenance;
- one-time self-hosted runner setup;
- VPS capacity limits нужно измерить;
- private corpus/security isolation должна быть deliberate;
- token rotation/task isolation требуют explicit operational procedure.

## Отклонённые альтернативы

- **Owner workstation as mandatory build machine** — rejected из-за friction/environment drift.
- **New paid orchestration platform by default** — rejected до доказательства measurable value сверх GitHub+existing VPS+ChatGPT Plus.
- **Full CI after every trivial change** — rejected из-за poor feedback/risk ratio.
- **Use GitHub Actions as pseudo-terminal for every interactive step** — rejected после proof прямого bounded ChatGPT→VPS bridge.
- **Universal command-execution endpoint** — rejected из-за unnecessary security/operational risk.