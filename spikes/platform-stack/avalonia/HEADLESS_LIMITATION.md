# P1 Avalonia full-window headless limitation evidence

`Avalonia.Headless 12.1.1` was evaluated as a practical Linux visual/behavior smoke backend before P1 Windows evidence.

The application and smoke projects compile in Release, but repeated bounded full-window probes timed out while constructing the production-like `MainWindow` in a headless session. The probes were intentionally bounded and did not mutate host configuration.

Representative exact-head workflow evidence:

- run `32194424745`: both Release builds passed; full-window headless smoke timed out;
- run `32194811558`: constructor tracing localized the hang after fixture load / during shell control construction;
- run `32195085242`: explicit early `SelectionChanged` wiring was shown to participate in the hang;
- run `32195406066`: idiomatic XAML event wiring compiled, full-window probe still timed out;
- run `32195746934`: data-bound `TreeView` replacement compiled, full-window probe still timed out;
- run `32196146577`: early programmatic tree selection removed, full-window probe still timed out.

P1 therefore does **not** deform the real shell to satisfy this test backend. Linux evidence uses exact-head Release compilation plus deterministic presentation-behavior smoke over the same shared fixture. Native window startup/rendering and the four required visual surfaces are validated in the Windows P1 lane.

This is P1 tooling/testability evidence only. It is not a stack-selection conclusion and does not waive native desktop acceptance.
