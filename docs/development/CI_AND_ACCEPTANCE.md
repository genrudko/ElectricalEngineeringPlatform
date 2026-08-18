# CI and Acceptance Strategy

Статус: canonical foundation document

## 1. Principle

Verification depth follows risk and changed ownership.

A two-button UI repair must not wait for full NPT corpus, switching scenarios and release packaging before the owner can see it. Conversely, a topology/state/normative change must not be accepted because a screenshot looks good.

## 2. Lane model

```text
L0 FAST STATIC
L1 TARGETED OWNER/MODULE TESTS
L2 VISUAL/PREVIEW ACCEPTANCE
L3 INTEGRATION/CORPUS/SCENARIO
L4 CROSS-PLATFORM/PACKAGING
L5 FULL/NIGHTLY/RELEASE
```

Workflow selection is based on changed paths plus explicit work-item risk classification; path filters are assistance, not the sole safety mechanism.

## 3. UI-only lane

Typical scope: design tokens, spacing/icons, shared-control layout and non-domain presentation.

Before visual acceptance:

- compile/type/build affected UI;
- targeted UI/component tests;
- UI Gallery screenshot/headless render;
- preview artifact.

Then owner visual acceptance.

Do not run private NPT corpus/switching rule suites merely because a button margin changed.

## 4. Domain lane

Changes to neutral project/equipment/terminal/connection/state/persistence require core unit tests, affected module tests, invariants, serialization round-trip/migration and relevant import/switching integration where contracts changed.

Add visual evidence if visible behavior changes.

## 5. Import lane

Requires source parser/mapping tests, staging/ambiguity tests, reconciliation diff tests, destructive-change safety, provenance, atomic transaction/undo, auto-layout preservation of constraints and representative vertical-slice fixture.

## 6. Scheme/canvas lane

Depending on change:

- geometry/layout tests;
- topology/view separation invariant;
- selection/hit-test interaction tests;
- deterministic render/visual snapshot;
- performance budget checks for core canvas changes;
- print/export evidence;
- owner visual review.

## 7. NPT compatibility lane

Public/normal lane:

- synthetic/cleared XSDE/XTABL fixtures;
- parser/writer tests;
- preservation tests;
- typed property tests;
- safe-save validation.

Controlled private corpus lane when format handling changes:

- full XSDE parse/unchanged round-trip;
- representative save/edit cases;
- XTABL corpus round-trip;
- signal catalog checks where affected;
- renderer comparison corpus when established;
- no proprietary corpus upload as artifact.

NPT lane is not required for unrelated UI Core fixes.

## 8. Switching/topology/compliance lane

Highest logical risk short of real-equipment control.

Requires:

- rule-ID indexed unit tests;
- positive/negative/UNKNOWN scenarios;
- topology/state transition tests;
- sequence revalidation tests;
- local-policy non-weakening tests;
- applicable source/profile version checks;
- regression scenarios from accepted operational examples;
- deterministic explanation/result tests where practical.

Normative semantic change requires source/provenance review, not only green CI.

## 9. Project format/migration lane

Changing native schema requires old-version load fixtures, migration, semantic comparison, extension preservation, save/reopen, recovery behavior and representative real-project migration before release.

## 10. Cross-platform lane

Run when framework/platform/native integration is touched or before release:

- Windows build/test/package;
- Linux build/test/package;
- native adapter tests where automatable;
- artifact restore/start;
- platform-specific manual gates as defined.

Not every platform-neutral domain commit needs full installer matrix before code review.

## 11. Performance lane

Run on canvas/spatial-index/layout architecture changes, large table/tree infrastructure, topology algorithms, large import reconciliation and release/nightly.

Store raw measurements/environment metadata and use regression thresholds with tolerance rather than flaky exact milliseconds.

## 12. Visual-first acceptance

For user-visible UI repairs/features:

```text
implement
→ fast targeted build/test
→ screenshot/preview artifact
→ owner review
→ repair if needed
→ only then expensive relevant gates
```

Automated visual tests complement, not replace, owner acceptance for major UX changes.

## 13. Failure retention

For important platform/compatibility defects retain concise evidence of observed failure, root cause, repair head, regression test and accepted retest.

## 14. Full suite

Appropriate for release candidate, nightly/periodic health check, broad architecture refactor, dependency/toolchain update, native schema migration and cross-cutting Domain Core contract change.

## 15. Preview artifacts

UI work should produce Gallery screenshots/archive, portable app preview, targeted recording only when needed or PDF/export artifact for output changes.

The owner should not need SSH/build tools to inspect normal UI work.

## 16. Acceptance evidence by class

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

## 17. Flakiness policy

Do not normalize flaky tests by blind reruns. Isolate cause, reduce nondeterminism, define tolerance where measurement varies and quarantine only with explicit issue/expiry condition.

## 18. CI budget

Track runner wall-clock and feedback latency.

Target principles:

- fast UI preview lane: minutes, not tens of minutes/hours;
- targeted core/module lane: bounded to changed subsystem;
- full private corpus/release lane: heavier but not on every trivial commit.

Exact thresholds are set after Infrastructure/Platform spikes provide measurements.
