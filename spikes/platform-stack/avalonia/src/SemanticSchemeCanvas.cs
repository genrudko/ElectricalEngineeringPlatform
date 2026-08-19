using System.Globalization;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Eep.PlatformStack.P2.Semantics;

namespace Eep.PlatformStack.P1.Avalonia;

public sealed class SemanticSchemeCanvas : Control
{
    private static readonly IBrush BackgroundBrush = new SolidColorBrush(Color.Parse("#FFFFFF"));
    private static readonly IBrush GridBrush = new SolidColorBrush(Color.Parse("#EEF1F4"));
    private static readonly IBrush NormalBrush = new SolidColorBrush(Color.Parse("#26323F"));
    private static readonly IBrush ClosedBrush = new SolidColorBrush(Color.Parse("#176B3A"));
    private static readonly IBrush OpenBrush = new SolidColorBrush(Color.Parse("#355F8A"));
    private static readonly IBrush UnknownBrush = new SolidColorBrush(Color.Parse("#697582"));
    private static readonly IBrush SelectedBrush = new SolidColorBrush(Color.Parse("#1769AA"));
    private static readonly IBrush HoverBrush = new SolidColorBrush(Color.Parse("#8AA7C0"));
    private static readonly IBrush WarningBrush = new SolidColorBrush(Color.Parse("#B78300"));
    private static readonly IBrush TerminalFillBrush = new SolidColorBrush(Color.Parse("#FFFFFF"));
    private static readonly IBrush SimulatedBrush = new SolidColorBrush(Color.Parse("#7A4D96"));
    private static readonly Typeface CanvasTypeface = new(new FontFamily("Noto Sans"));

    private readonly P2CanvasController _controller;
    private readonly string _tierLabel;
    private bool _panning;
    private ScenePoint _lastPanScreen;
    private string? _dragEntityId;
    private ScenePoint _dragStartWorld;
    private ScenePoint _dragOriginal;
    private string _dragTopology = string.Empty;
    private P2ViewportMode? _requestedViewportMode;
    private bool _applyViewportOnNextRender;

    public SemanticSchemeCanvas()
        : this(P2SemanticFixtureLoader.Load(), "DEMO")
    {
    }

    public SemanticSchemeCanvas(P2SemanticScene scene, string tierLabel)
    {
        _controller = new P2CanvasController(scene);
        _tierLabel = tierLabel;
        Focusable = true;
        ClipToBounds = true;
    }

    public P2CanvasController Controller => _controller;
    public string TierLabel => _tierLabel;
    public event Action<string>? StatusChanged;
    public event Action<string>? EntitySelected;

    public void ApplyViewportMode(P2ViewportMode mode)
    {
        _requestedViewportMode = mode;
        _applyViewportOnNextRender = true;
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        var width = Math.Max(1, Bounds.Width);
        var height = Math.Max(1, Bounds.Height);
        if (_applyViewportOnNextRender && _requestedViewportMode is { } mode)
        {
            _controller.ApplyViewportMode(mode, width, height);
            _applyViewportOnNextRender = false;
        }

        context.FillRectangle(BackgroundBrush, new Rect(Bounds.Size));
        if (_controller.Zoom >= 0.20)
        {
            DrawGrid(context);
        }

        var visibleEntityIds = _controller.VisibleEntityIds(width, height);
        var visibleConnectionIds = _controller.VisibleConnectionIds(width, height);
        var markerIds = _controller.Scene.ValidationMarkers.Select(marker => marker.EntityId).ToHashSet(StringComparer.Ordinal);

        DrawConnections(context, visibleConnectionIds);
        DrawEntities(context, visibleEntityIds, markerIds);
        DrawOverlay(context, visibleEntityIds, visibleConnectionIds);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        Focus();

        var screen = ToScenePoint(e.GetPosition(this));
        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsMiddleButtonPressed || point.Properties.IsRightButtonPressed)
        {
            _panning = true;
            _requestedViewportMode = null;
            _lastPanScreen = screen;
            e.Pointer.Capture(this);
            e.Handled = true;
            return;
        }

        if (!point.Properties.IsLeftButtonPressed)
        {
            return;
        }

        var world = _controller.ScreenToWorld(screen);
        var terminal = _controller.HitTestTerminal(world, 10 / _controller.Zoom);
        if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && terminal is not null && _controller.SelectedConnectionId is not null)
        {
            var before = _controller.Scene.TopologyFingerprint();
            _controller.ReconnectSelectedTo(terminal.Value.TerminalId);
            StatusChanged?.Invoke($"P2 reconnect: {_controller.SelectedConnectionId} → {terminal.Value.TerminalId}; topology changed={before != _controller.Scene.TopologyFingerprint()}");
            InvalidateVisual();
            e.Handled = true;
            return;
        }

        var entityId = _controller.HitTestEntity(world, 5 / _controller.Zoom);
        if (entityId is not null)
        {
            _controller.SelectedEntityId = entityId;
            _controller.SelectedConnectionId = null;
            EntitySelected?.Invoke(entityId);
            _dragEntityId = entityId;
            _dragStartWorld = world;
            _dragOriginal = _controller.Scene.Placements[entityId].Position;
            _dragTopology = _controller.Scene.TopologyFingerprint();
            e.Pointer.Capture(this);
            StatusChanged?.Invoke($"P2 selected: {entityId}; drag moves representation only");
            InvalidateVisual();
            e.Handled = true;
            return;
        }

        var connectionId = _controller.HitTestConnection(world, 8 / _controller.Zoom);
        _controller.SelectedConnectionId = connectionId;
        if (connectionId is not null)
        {
            StatusChanged?.Invoke($"P2 connection selected: {connectionId}; Ctrl+click a terminal to reconnect TO endpoint");
        }
        InvalidateVisual();
        e.Handled = connectionId is not null;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        var screen = ToScenePoint(e.GetPosition(this));

        if (_panning)
        {
            _controller.PanBy(new ScenePoint(screen.X - _lastPanScreen.X, screen.Y - _lastPanScreen.Y));
            _lastPanScreen = screen;
            InvalidateVisual();
            e.Handled = true;
            return;
        }

        var world = _controller.ScreenToWorld(screen);
        if (_dragEntityId is not null)
        {
            var target = new ScenePoint(
                _dragOriginal.X + world.X - _dragStartWorld.X,
                _dragOriginal.Y + world.Y - _dragStartWorld.Y);
            _controller.Scene.Placements[_dragEntityId].Position = target;
            InvalidateVisual();
            e.Handled = true;
            return;
        }

        var hovered = _controller.HitTestEntity(world, 4 / _controller.Zoom);
        if (!string.Equals(hovered, _controller.HoveredEntityId, StringComparison.Ordinal))
        {
            _controller.HoveredEntityId = hovered;
            InvalidateVisual();
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_dragEntityId is not null)
        {
            var entityId = _dragEntityId;
            var target = _controller.Scene.Placements[entityId].Position;
            _controller.Scene.Placements[entityId].Position = _dragOriginal;
            _controller.MoveEntity(entityId, target);
            var topologyPreserved = _dragTopology == _controller.Scene.TopologyFingerprint();
            StatusChanged?.Invoke($"P2 move committed: {entityId}; topology preserved={topologyPreserved}");
            _dragEntityId = null;
        }

        _panning = false;
        e.Pointer.Capture(null);
        InvalidateVisual();
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        _requestedViewportMode = null;
        var screen = ToScenePoint(e.GetPosition(this));
        _controller.ZoomAt(screen, e.Delta.Y > 0 ? 1.12 : 1 / 1.12);
        StatusChanged?.Invoke($"P2 {_tierLabel} zoom: {_controller.Zoom:P0}");
        InvalidateVisual();
        e.Handled = true;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.Key == Key.Z)
        {
            if (_controller.Undo())
            {
                StatusChanged?.Invoke("P2 undo");
                InvalidateVisual();
            }
            e.Handled = true;
            return;
        }

        if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.Key == Key.Y)
        {
            if (_controller.Redo())
            {
                StatusChanged?.Invoke("P2 redo");
                InvalidateVisual();
            }
            e.Handled = true;
            return;
        }

        if (!e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.Key == Key.S && _controller.CycleSelectedState())
        {
            var selected = _controller.SelectedEntityId!;
            StatusChanged?.Invoke($"P2 typed state update: {selected} → {_controller.Scene.Entities[selected].State}");
            InvalidateVisual();
            e.Handled = true;
        }
    }

    private void DrawGrid(DrawingContext context)
    {
        const double step = 40;
        var topLeft = _controller.ScreenToWorld(new ScenePoint(0, 0));
        var bottomRight = _controller.ScreenToWorld(new ScenePoint(Bounds.Width, Bounds.Height));
        var pen = new Pen(GridBrush, 1);

        var startX = Math.Floor(topLeft.X / step) * step;
        for (var x = startX; x <= bottomRight.X; x += step)
        {
            context.DrawLine(pen,
                ToPoint(_controller.WorldToScreen(new ScenePoint(x, topLeft.Y))),
                ToPoint(_controller.WorldToScreen(new ScenePoint(x, bottomRight.Y))));
        }

        var startY = Math.Floor(topLeft.Y / step) * step;
        for (var y = startY; y <= bottomRight.Y; y += step)
        {
            context.DrawLine(pen,
                ToPoint(_controller.WorldToScreen(new ScenePoint(topLeft.X, y))),
                ToPoint(_controller.WorldToScreen(new ScenePoint(bottomRight.X, y))));
        }
    }

    private void DrawConnections(DrawingContext context, IReadOnlyList<string> connectionIds)
    {
        foreach (var connectionId in connectionIds)
        {
            var selected = connectionId == _controller.SelectedConnectionId;
            var pen = new Pen(selected ? SelectedBrush : NormalBrush, selected ? 3 : 1.5);
            var route = _controller.ConnectionRoute(connectionId);
            for (var i = 0; i < route.Count - 1; i++)
            {
                context.DrawLine(pen, ToPoint(_controller.WorldToScreen(route[i])), ToPoint(_controller.WorldToScreen(route[i + 1])));
            }
        }
    }

    private void DrawEntities(DrawingContext context, IReadOnlyList<string> entityIds, ISet<string> markerIds)
    {
        var lowDetail = _controller.Zoom < 0.18;
        var showLabels = _controller.Zoom >= 0.35;
        var showTerminals = _controller.Zoom >= 0.45;

        foreach (var entityId in entityIds)
        {
            var entity = _controller.Scene.Entities[entityId];
            var bounds = ToRect(_controller.EntityBounds(entity.Id));
            var selected = entity.Id == _controller.SelectedEntityId;
            var hovered = entity.Id == _controller.HoveredEntityId;
            var stateBrush = StateBrush(entity.State);

            if (lowDetail)
            {
                if (entity.Kind == SemanticEntityKind.Busbar)
                {
                    context.DrawLine(new Pen(stateBrush, 2), new Point(bounds.Left, bounds.Center.Y), new Point(bounds.Right, bounds.Center.Y));
                }
                else
                {
                    context.FillRectangle(stateBrush, bounds);
                }
            }
            else
            {
                var statePen = new Pen(stateBrush, Math.Max(1.5, 2 * _controller.Zoom));
                switch (entity.Kind)
                {
                    case SemanticEntityKind.Busbar:
                        context.DrawLine(new Pen(stateBrush, Math.Max(3, 5 * _controller.Zoom)),
                            new Point(bounds.Left, bounds.Center.Y), new Point(bounds.Right, bounds.Center.Y));
                        break;
                    case SemanticEntityKind.CircuitBreaker:
                        DrawCircuitBreaker(context, bounds, entity.State, statePen);
                        break;
                    case SemanticEntityKind.Disconnector:
                        DrawDisconnector(context, bounds, entity.State, statePen);
                        break;
                    case SemanticEntityKind.EarthingSwitch:
                        DrawEarthingSwitch(context, bounds, entity.State, statePen);
                        break;
                }
            }

            if (selected || hovered)
            {
                context.DrawRectangle(null, new Pen(selected ? SelectedBrush : HoverBrush, selected ? 2 : 1), bounds.Inflate(5));
            }

            if (showLabels)
            {
                DrawLabel(context, entity.Designation, new Point(bounds.Left, bounds.Bottom + 7), 12, NormalBrush);
            }

            if (showTerminals)
            {
                foreach (var terminal in entity.Terminals)
                {
                    var point = ToPoint(_controller.WorldToScreen(_controller.TerminalAnchor(terminal.Id)));
                    context.DrawEllipse(TerminalFillBrush, new Pen(NormalBrush, 1.2), point, 3.5, 3.5);
                }
            }

            if (markerIds.Contains(entity.Id) && _controller.Zoom >= 0.12)
            {
                context.DrawEllipse(WarningBrush, null, new Point(bounds.Right + 4, bounds.Top - 4), 4, 4);
            }
        }
    }

    private static void DrawCircuitBreaker(DrawingContext context, Rect bounds, SemanticState state, Pen pen)
    {
        var centerX = bounds.Center.X;
        context.DrawRectangle(null, pen, new Rect(centerX - 18, bounds.Center.Y - 16, 36, 32));
        context.DrawLine(pen, new Point(centerX, bounds.Top), new Point(centerX, bounds.Center.Y - 16));
        context.DrawLine(pen, new Point(centerX, bounds.Center.Y + 16), new Point(centerX, bounds.Bottom));
        context.DrawLine(pen,
            state is SemanticState.Closed or SemanticState.SimulatedClosed ? new Point(centerX, bounds.Center.Y - 12) : new Point(centerX - 8, bounds.Center.Y + 10),
            state is SemanticState.Closed or SemanticState.SimulatedClosed ? new Point(centerX, bounds.Center.Y + 12) : new Point(centerX + 9, bounds.Center.Y - 8));
    }

    private static void DrawDisconnector(DrawingContext context, Rect bounds, SemanticState state, Pen pen)
    {
        var x = bounds.Center.X;
        var upper = new Point(x, bounds.Center.Y - 15);
        var lower = new Point(x, bounds.Center.Y + 15);
        context.DrawLine(pen, new Point(x, bounds.Top), upper);
        context.DrawLine(pen, lower, new Point(x, bounds.Bottom));
        context.DrawEllipse(TerminalFillBrush, pen, upper, 3, 3);
        context.DrawEllipse(TerminalFillBrush, pen, lower, 3, 3);
        context.DrawLine(pen, lower, state == SemanticState.Closed ? upper : new Point(x + 22, bounds.Center.Y - 8));
    }

    private static void DrawEarthingSwitch(DrawingContext context, Rect bounds, SemanticState state, Pen pen)
    {
        var left = new Point(bounds.Left, bounds.Center.Y);
        var pivot = new Point(bounds.Center.X - 8, bounds.Center.Y);
        var earth = new Point(bounds.Center.X, bounds.Bottom);
        context.DrawLine(pen, left, pivot);
        context.DrawLine(pen, pivot, state == SemanticState.Closed ? new Point(bounds.Center.X, bounds.Center.Y + 18) : new Point(bounds.Center.X + 18, bounds.Center.Y - 12));
        context.DrawLine(pen, new Point(bounds.Center.X, bounds.Center.Y + 18), earth);
        context.DrawLine(pen, new Point(bounds.Center.X - 14, bounds.Bottom), new Point(bounds.Center.X + 14, bounds.Bottom));
        context.DrawLine(pen, new Point(bounds.Center.X - 9, bounds.Bottom + 5), new Point(bounds.Center.X + 9, bounds.Bottom + 5));
        context.DrawLine(pen, new Point(bounds.Center.X - 4, bounds.Bottom + 10), new Point(bounds.Center.X + 4, bounds.Bottom + 10));
    }

    private void DrawOverlay(DrawingContext context, IReadOnlyList<string> entityIds, IReadOnlyList<string> connectionIds)
    {
        var selected = _controller.SelectedEntityId ?? "—";
        var connection = _controller.SelectedConnectionId ?? "—";
        var terminalCount = entityIds.Sum(id => _controller.Scene.Entities[id].Terminals.Count);
        var visibleSemantic = entityIds.Count + terminalCount + connectionIds.Count;
        var mode = _requestedViewportMode?.ToString().ToUpperInvariant() ?? "CUSTOM";
        DrawLabel(context,
            $"P2 {_tierLabel} · {mode} · zoom {_controller.Zoom:P1} · visible semantic {visibleSemantic} · entity {selected} · connection {connection}",
            new Point(12, 10), 12, NormalBrush);
        DrawLabel(context,
            "ЛКМ select/drag · ПКМ/СКМ pan · wheel zoom · connection + Ctrl+click terminal reconnect · S state · Ctrl+Z/Y",
            new Point(12, 29), 11, UnknownBrush);
    }

    private void DrawLabel(DrawingContext context, string text, Point origin, double fontSize, IBrush brush)
    {
        var formatted = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, CanvasTypeface, fontSize, brush);
        context.DrawText(formatted, origin);
    }

    private static IBrush StateBrush(SemanticState state) => state switch
    {
        SemanticState.Closed => ClosedBrush,
        SemanticState.Open => OpenBrush,
        SemanticState.Intermediate => WarningBrush,
        SemanticState.Unknown => UnknownBrush,
        SemanticState.SimulatedClosed => SimulatedBrush,
        _ => NormalBrush
    };

    private Rect ToRect(SceneRect world)
    {
        var topLeft = _controller.WorldToScreen(new ScenePoint(world.X, world.Y));
        return new Rect(topLeft.X, topLeft.Y, world.Width * _controller.Zoom, world.Height * _controller.Zoom);
    }

    private static ScenePoint ToScenePoint(Point point) => new(point.X, point.Y);
    private static Point ToPoint(ScenePoint point) => new(point.X, point.Y);
}
