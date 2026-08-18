# Стратегия CI и acceptance

Статус: канонический Foundation-документ

## 1. Принцип

Глубина verification определяется risk и changed ownership.

Небольшой UI repair не должен ждать full NPT corpus, Switching scenarios и release packaging до первой owner visual review. И наоборот, topology/state/normative change не может быть accepted только потому, что screenshot выглядит хорошо.

## 2. Lane model

```text
L0 FAST STATIC
L1 TARGETED OWNER/MODULE TESTS
L2 VISUAL/PREVIEW ACCEPTANCE
L3 INTEGRATION/CORPUS/SCENARIO
L4 CROSS-PLATFORM/PACKAGING
L5 FULL/NIGHTLY/RELEASE
```

Workflow selection основывается на changed paths + explicit work-item risk classification. Path filters помогают, но не являются единственным safety mechanism.

## 3. UI-only lane

Typical scope: design tokens, spacing/icons, shared-control layout и non-domain presentation.

До visual acceptance:

- compile/type/build affected UI;
- targeted UI/component tests;
- UI Gallery screenshot/headless render;
- preview artifact.

Затем owner visual acceptance.

Private NPT corpus/Sharp switching rule suites не запускаются только потому, что изменён button margin.

## 4. Domain lane

Changes neutral project/equipment/terminal/connection/state/persistence требуют:

- core unit tests;
- affected module tests;
- invariants;
- serialization round-trip/migration;
- relevant import/switching integration, если изменился contract.

Добавлять visual evidence, если меняется visible behavior.

## 5. Import lane

Требует:

- source parser/mapping tests;
- staging/ambiguity tests;
- reconciliation diff tests;
- destructive-change safety;
- provenance;
- atomic transaction/undo;
- auto-layout preservation of constraints;
- representative vertical-slice fixture.

## 6. Scheme/canvas lane

В зависимости от change:

- geometry/layout tests;
- topology/view separation invariant;
- selection/hit-test interaction tests;
- deterministic render/visual snapshot;
- performance budget checks для core canvas changes;
- print/export evidence;
- owner visual review.

## 7. NPT compatibility lane

Public/normal lane:

- synthetic/cleared XSDE/XTABL fixtures;
- parser/writer tests;
- preservation tests;
- typed property tests;
- safe-save validation.

Controlled private corpus lane при format handling changes:

- full XSDE parse/unchanged round-trip;
- representative save/edit cases;
- XTABL corpus round-trip;
- signal catalog checks where affected;
- renderer comparison corpus when established;
- no proprietary corpus upload as artifact.

NPT lane не required для unrelated UI Core fixes.

## 8. Switching/topology/compliance lane

Это highest logical risk до real-equipment control.

Требуется:

- rule-ID indexed unit tests;
- positive/negative/UNKNOWN scenarios;
- topology/state transition tests;
- sequence revalidation tests;
- local-policy non-weakening tests;
- applicable source/profile version checks;
- synthetic/cleared regression scenarios;
- deterministic explanation/result tests where practical.

Normative semantic change требует source/provenance review, а не только green CI.

## 9. Project format/migration lane

Change native schema требует:

- old-version load fixtures;
- migration;
- semantic comparison;
- extension preservation;
- save/reopen;
- recovery behavior;
- representative real-project migration before release.

## 10. Cross-platform lane

Запускается при changes framework/platform/native integration или перед release:

- Windows build/test/package;
- Linux build/test/package;
- native adapter tests where automatable;
- artifact restore/start;
- platform-specific manual gates as defined.

Не каждый platform-neutral domain commit требует full installer matrix до code review.

## 11. Performance lane

Запускается для:

- canvas/spatial-index/layout architecture changes;
- large table/tree infrastructure;
- topology algorithms;
- large import reconciliation;
- release/nightly.

Сохранять raw measurements/environment metadata и использовать regression thresholds с tolerance вместо flaky exact milliseconds.

## 12. Visual-first acceptance

Для user-visible UI repair/feature:

```text
implement
→ fast targeted build/test
→ screenshot/preview artifact
→ owner review
→ repair if needed
→ only then expensive relevant gates
```

Automated visual tests дополняют, а не заменяют owner acceptance для major UX changes.

## 13. Interactive Bridge vs formal CI

EEP Development Bridge предназначен для fast interactive tasks и не является formal acceptance evidence сам по себе.

```text
Bridge
→ быстрый inspect/build/test/preview loop

GitHub self-hosted runner
→ exact-head check/status/artifact evidence
```

Если результат должен участвовать в formal PR gate, он воспроизводится через repository-defined command на exact PR head и фиксируется GitHub check/artifact.

## 14. Failure retention

Для важных platform/compatibility defects сохраняется concise evidence:

- observed failure;
- root cause;
- repair head;
- regression test;
- accepted retest.

## 15. Full suite

Уместен для:

- release candidate;
- nightly/periodic health check;
- broad architecture refactor;
- dependency/toolchain update;
- native schema migration;
- cross-cutting Domain Core contract change.

## 16. Preview artifacts

UI work должен produce:

- Gallery screenshots/archive;
- portable app preview;
- targeted recording only when needed;
- PDF/export artifact для output changes.

Owner не должен нуждаться в SSH/build tools для normal UI inspection.

## 17. Acceptance evidence by class

| Change | Minimum evidence before acceptance |
|---|---|
| text/docs | doc validation/review |
| UI visual | targeted tests + screenshot/preview + owner visual |
| shared UI infrastructure | above + relevant window/HiDPI/component tests |
| Domain Core | unit/invariant/round-trip + affected integration |
| Import | reconciliation/ambiguity/undo + representative vertical slice |
| NPT format | preservation + private corpus when relevant |
| topology/switching | scenario/invariant/UNKNOWN + normative provenance |
| packaging/platform | Windows/Linux package/start + manual native evidence |
| normative rule | source review + rule tests + coverage update |

## 18. Flakiness policy

Не нормализовать flaky tests blind reruns.

Нужно isolate cause, reduce nondeterminism, define tolerance where measurement varies и quarantine только с explicit issue/expiry condition.

## 19. CI budget

Track runner wall-clock и feedback latency.

Target principles:

- fast UI preview lane: минуты, не десятки минут/часы;
- targeted core/module lane: bounded к changed subsystem;
- full private corpus/release lane: heavier, но не на каждый trivial commit.

Exact thresholds задаются после Infrastructure/Platform spikes.