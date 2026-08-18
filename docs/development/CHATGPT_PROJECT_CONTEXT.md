# ChatGPT Project Context Workflow

Статус: канонический Foundation-документ  
Дата проверки product capabilities: **2026-08-18**

## 1. Назначение

Для длительной координации Electrical Engineering Platform используется не один «бесконечный чат» и не цепочка больших handoff-файлов, а сочетание:

```text
ChatGPT Project
├── chats               # рабочая история и параллельные ветки
├── project sources     # короткие принятые checkpoints / decision notes
└── project instructions# устойчивые правила работы

+ GitHub                # каноническое состояние code/docs/issues/PR
+ Development Bridge    # фактическое runtime/development состояние VPS
```

Главный принцип:

> Project Memory помогает восстановить контекст и причины решений, но не заменяет проверку фактического текущего состояния в GitHub/runtime.

## 2. Что подтверждено текущими возможностями ChatGPT

По официальной документации OpenAI на 2026-08-18:

- Plus/Pro могут использовать предыдущие chats внутри Project для более релевантных ответов;
- при вопросе внутри Project ChatGPT приоритизирует project chats и files;
- для project memory на non-Enterprise subscriptions должны быть включены `Reference saved memories` и `Reference chat history` в Personalization settings;
- Project Memory не предоставляет отдельного полного списка «всего, что проект помнит»;
- важный chat response можно сохранить как Project Source через `Save to project` / `Add to project sources`;
- branching позволяет исследовать альтернативное направление, сохраняя исходную conversation;
- Custom GPT можно использовать для сообщений в уже существующем Project chat.

Product behavior ChatGPT может меняться. Эти capability assumptions должны повторно проверяться по official OpenAI documentation, если UI/plan behavior изменился.

## 3. Ограничение Project Memory

Project Memory — retrieval/context layer, а не transactional database.

Нельзя считать, что новый chat автоматически получит:

- каждый прошлый message;
- каждый exact SHA;
- каждый intermediate experiment;
- все детали старого troubleshooting;
- последнюю runtime-конфигурацию без проверки.

Поэтому критичные technical facts не должны существовать только в chat history.

## 4. Authority model

Для разработки действует разделение ответственности:

```text
WHY / historical reasoning
    → Project chats + selected Project Sources

STABLE WORKING RULES
    → Project Instructions + AGENTS.md

WHAT IS TRUE NOW
    → GitHub + actual runtime/VPS evidence

ACCEPTED ENGINEERING DECISIONS
    → accepted ADRs + canonical docs in GitHub
```

Если Project Memory/checkpoint противоречит GitHub или фактическому runtime evidence, chat memory/checkpoint считается устаревшим.

## 5. Project Instructions — только устойчивые правила

В Project Instructions разрешены правила, которые редко меняются, например:

- GitHub — canonical source of truth;
- issue → branch → Draft PR workflow;
- no Ready/Merge без explicit owner command;
- русский язык product/canonical docs;
- English internal code/domain terminology;
- risk-based CI;
- visual-first UI acceptance;
- GitHub/runtime state re-verify перед implementation/acceptance;
- Bridge не является arbitrary remote shell.

Не помещать туда volatile state:

- exact SHA;
- current PR head;
- current branch compare;
- текущий runtime version;
- последний эксперимент;
- временный blocker;
- конкретный task result.

Иначе Project Instructions превращаются в источник устаревшей информации.

## 6. Project Sources — контрольные точки, а не handoff

`Save to project` используется для коротких durable checkpoints после действительно значимого изменения состояния.

Подходящие события:

- owner acceptance архитектурного решения;
- завершение spike;
- выбор platform stack;
- accepted module baseline;
- аппаратное/runtime доказательство, которое трудно восстановить повторно;
- закрытие значимого troubleshooting contour;
- accepted release/pilot state.

Checkpoint должен быть коротким и содержать только:

```text
Checkpoint title + date
Status
Accepted facts
Evidence / canonical references
Frozen decisions / constraints
Open questions
Next intended work item
```

Он не должен пересказывать весь chat.

### Пример

```text
Electrical Engineering Platform — Foundation accepted — YYYY-MM-DD

Status: ACCEPTED
Repository: genrudko/ElectricalEngineeringPlatform
Foundation: merged PR #...
Accepted: modular monolith, ElectricalProject source of truth,
          RU UI/docs + EN internal technical layer,
          bounded ChatGPT→VPS Bridge
Pending: platform stack, native project format, EOD feasibility
Next: INFRASTRUCTURE-SPIKE-001
```

Exact volatile GitHub state из старого checkpoint всё равно перепроверяется при новом work session.

## 7. Новый chat — retrieval-first, не handoff-first

Новый chat внутри того же Project начинается с нескольких уникальных semantic anchors, по которым Project Memory может найти relevant previous conversations.

Recommended prompt pattern:

```text
Продолжаем Electrical Engineering Platform.
Восстанови из предыдущих chats этого Project контекст по якорям:
UNIFIED-FOUNDATION-001, ElectricalProject, EEP Development Bridge,
Avalonia vs Qt, Import-to-Scheme.

Сначала дай:
1. последние доказанные факты;
2. последнее принятое решение;
3. последний выполненный шаг;
4. незакрытые вопросы.

Ничего пока не меняй.
После восстановления отдельно проверь factual current state в GitHub/runtime.
```

Это не гарантирует perfect recall; это retrieval hint.

## 8. Recovery sequence нового coordinator/chat

Нормальная последовательность:

```text
1. Project Memory retrieval по anchors
        ↓
2. Project Sources / accepted checkpoints
        ↓
3. docs/INDEX.md + AGENTS.md
        ↓
4. GitHub factual verification
   main / issue / branch / Draft PR / exact head / diff / workflows
        ↓
5. runtime/VPS verification, если relevant
        ↓
6. только после этого implementation/acceptance
```

Нельзя использовать старый chat summary как замену шагам 4–5.

## 9. Handoff policy

Большой handoff-файл **не является default способом перехода между chats внутри одного Project**.

Handoff допустим, когда:

- context переносится в другой ChatGPT Project;
- context должен уйти во внешнюю систему/другому человеку;
- Project Memory/Project Sources недоступны;
- требуется standalone archival record;
- есть конкретная compliance/audit причина.

Во всех обычных intra-project переходах предпочтительны retrieval + checkpoint + canonical verification.

Избегать цепочки:

```text
handoff.md
handoff_final.md
handoff_final_v2.md
handoff_final_really_final.md
```

## 10. Branch chats

Branching используется для реальных альтернативных гипотез, например:

```text
Branch A → Avalonia canvas approach
Branch B → Qt/QML canvas approach
```

или:

```text
Branch A → NPT topology mapping hypothesis 1
Branch B → mapping hypothesis 2
```

Branch не является специальным механизмом «обнуления слишком длинного контекста» и не заменяет canonical decision record.

После выбора направления accepted conclusion фиксируется в GitHub/ADR и, при необходимости, коротком Project Source checkpoint.

## 11. Tasks/reminders

Project rule:

> Tasks/reminders используются для времени, recurrence и external-condition follow-up, но не как контейнер engineering state.

Task может напомнить «продолжить Platform Stack Spike», но current head, benchmark result, accepted decision и runtime evidence должны находиться в canonical/project sources, а не только в task text.

## 12. GitHub остаётся каноническим state layer

Project context strategy не меняет GitHub-first discipline.

Перед любым существенным implementation/acceptance coordinator обязан фактически получить из GitHub:

- current `main`;
- active issue/work item;
- branch / Draft PR;
- exact head;
- `behind_by`/compare state;
- changed files;
- relevant workflow/check state;
- current canonical docs.

ChatGPT Project нужен для continuity, а не для замены repository truth.

## 13. Runtime truth

Если вопрос зависит от фактического development runtime, дополнительно используется bounded EEP Development Bridge или formal runner evidence.

Пример:

```text
GitHub говорит, какой code должен быть запущен.
Bridge/runner доказывает, что реально произошло на VPS.
Project Memory объясняет, почему мы выбрали такой путь.
```

## 14. Checkpoint hygiene

Project Sources должны оставаться небольшим curated набором.

Не сохранять каждый хороший ответ.

Сохранять только ответы, которые являются:

- acceptance note;
- decision note;
- stable baseline;
- completed spike result;
- difficult-to-reproduce evidence summary.

Если checkpoint superseded, новый checkpoint должен явно указывать это. Старый можно сохранить для history, но не считать current state.

## 15. Anti-goals

Не строить workflow, где:

- один chat должен жить бесконечно;
- каждый новый chat требует огромный handoff;
- Project Instructions содержат volatile SHA/status;
- Project Memory считается гарантированным полным архивом;
- Task/reminder считается state database;
- branch chat заменяет ADR;
- chat summary переопределяет GitHub/runtime evidence.

## 16. Текущий baseline Electrical Engineering Platform

Для этого Project принят следующий coordination pattern:

```text
Project chats
    → рабочая история / retrieval

Project Sources
    → редкие accepted checkpoints

Project Instructions
    → устойчивые operating rules

GitHub
    → current canonical product/development state

EEP Development Bridge / runner
    → фактическое execution/runtime evidence
```

Это является Foundation direction для дальнейшей работы.