# ADR 0004 — GitHub как Control Plane + existing VPS как Execution Plane

Статус: **ACCEPTED по решению владельца / зафиксирован в `UNIFIED-FOUNDATION-001`**  
Дата: 2026-08-18

## Контекст

Предыдущие процессы разработки становились слишком дорогими по времени: ручная работа на локальной машине, регулярные SSH-сессии, тяжёлый CI до визуальной приёмки и слишком много инфраструктурных действий для небольших изменений.

У владельца уже есть GitHub, ChatGPT Plus и existing VPS. Новый обязательный платный orchestration platform для нормальной разработки не нужен.

Во время Foundation отдельно проверен прямой ChatGPT→VPS development bridge.

## Решение

Использовать два взаимодополняющих контура.

### Интерактивный контур

```text
ChatGPT Plus Project chat
→ @Custom GPT Action
→ HTTPS/Bearer
→ EEP Development Bridge
→ existing VPS
```

Назначение: быстрые inspect/build/test/preview tasks без регулярного SSH со стороны владельца.

### Формальный контур проверки

```text
GitHub = canonical control plane
→ self-hosted runner on existing VPS
→ exact-head build/test/benchmark/package
→ GitHub checks/artifacts
→ owner acceptance
```

Рабочая станция владельца остаётся acceptance endpoint.

Local SSH — инфраструктурный/admin escape hatch, а не normal workflow.

CI остаётся risk-based; визуальная приёмка UI по возможности выполняется до дорогих unrelated gates.

## Доказательства от 2026-08-18

Практический тест подтвердил:

- Custom GPT Action работает на текущем ChatGPT Plus;
- endpoint опубликован по HTTPS через Caddy;
- сертификат Let's Encrypt получен успешно;
- bridge FastAPI слушает только localhost;
- service работает под непривилегированным `eepbridge` account;
- Bearer authentication работает: `401` без token и `200` с корректным token;
- GPT Action реально дошёл до VPS: `GET /health ... 200 OK` зафиксирован server log;
- Custom GPT Action работает внутри существующего ChatGPT Project chat;
- GitHub connector доступен в том же Project conversation;
- GitHub query и последующий bridge query работают последовательно.

Следствие: Business/MCP/OpenAI API не являются обязательными dependencies текущего baseline.

## Ограничение архитектуры Bridge

Development Bridge — bounded typed development API, а не universal remote shell.

Execution operations задаются explicit allowlist/profiles и должны иметь fixed workspace, timeout, audit и controlled output.

## Последствия

Положительные:

- новый обязательный платный control service не нужен;
- build toolchains/corpora остаются вне owner workstation;
- private NPT corpus может тестироваться на controlled VPS;
- GitHub остаётся auditable canonical state;
- interactive Bridge сокращает turnaround time;
- self-hosted runner даёт formal exact-head evidence;
- владелец не должен routinely работать через SSH.

Затраты/риски:

- hardening и maintenance bridge;
- одноразовый setup self-hosted runner;
- VPS capacity limits нужно измерить;
- isolation private corpus/security должна быть deliberate;
- token rotation/task isolation требуют explicit operational procedure.

## Отклонённые альтернативы

- **Рабочая станция владельца как обязательная build machine** — отклонено из-за friction/environment drift.
- **Новая платная orchestration platform по умолчанию** — отклонено до доказательства measurable value сверх GitHub + existing VPS + ChatGPT Plus.
- **Full CI после каждого trivial change** — отклонено из-за плохого feedback/risk ratio.
- **GitHub Actions как pseudo-terminal для каждого interactive step** — отклонено после proof прямого bounded ChatGPT→VPS bridge.
- **Universal command-execution endpoint** — отклонено из-за unnecessary security/operational risk.