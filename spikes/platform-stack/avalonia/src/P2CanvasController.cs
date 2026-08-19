namespace Eep.PlatformStack.P2.Semantics;

public readonly record struct SceneRect(double X, double Y, double Width, double Height)
{
    public double Right => X + Width;
    public double Bottom => Y + Height;

    public bool Contains(ScenePoint point, double tolerance = 0) =>
        point.X >= X - tolerance && point.X <= Right + tolerance &&
        point.Y >= Y - tolerance && point.Y <= Bottom + tolerance;

    public bool Intersects(SceneRect other) =>
        X <= other.Right && Right >= other.X && Y <= other.Bottom && Bottom >= other.Y;
}

public readonly record struct TerminalHit(string TerminalId, ScenePoint Position);

public enum P2ViewportMode
{
    Normal,
    Dense,
    ZoomToFit
}

public sealed class P2CanvasController
{
    private const double DeviceWidth = 84;
    private const double DeviceHeight = 64;
    private const double BusbarWidth = 320;
    private const double BusbarHeight = 24;

    public P2CanvasController(P2SemanticScene scene)
    {
        Scene = scene;
    }

    public P2SemanticScene Scene { get; }
    public P2CommandHistory History { get; } = new();
    public double Zoom { get; private set; } = 1.0;
    public ScenePoint Pan { get; private set; } = new(60, 60);
    public string? SelectedEntityId { get; set; }
    public string? HoveredEntityId { get; set; }
    public string? SelectedConnectionId { get; set; }

    public ScenePoint WorldToScreen(ScenePoint world) =>
        new(world.X * Zoom + Pan.X, world.Y * Zoom + Pan.Y);

    public ScenePoint ScreenToWorld(ScenePoint screen) =>
        new((screen.X - Pan.X) / Zoom, (screen.Y - Pan.Y) / Zoom);

    public void PanBy(ScenePoint screenDelta) =>
        Pan = new ScenePoint(Pan.X + screenDelta.X, Pan.Y + screenDelta.Y);

    public void ZoomAt(ScenePoint screenAnchor, double factor)
    {
        var worldAnchor = ScreenToWorld(screenAnchor);
        var target = Math.Clamp(Zoom * factor, 0.05, 8.0);
        Zoom = target;
        Pan = new ScenePoint(
            screenAnchor.X - worldAnchor.X * Zoom,
            screenAnchor.Y - worldAnchor.Y * Zoom);
    }

    public void ApplyViewportMode(P2ViewportMode mode, double viewportWidth, double viewportHeight)
    {
        switch (mode)
        {
            case P2ViewportMode.Normal:
                TargetVisibleSemanticElements(500, viewportWidth, viewportHeight);
                break;
            case P2ViewportMode.Dense:
                TargetVisibleSemanticElements(2_000, viewportWidth, viewportHeight);
                break;
            case P2ViewportMode.ZoomToFit:
                FitScene(viewportWidth, viewportHeight, 32);
                break;
        }
    }

    public SceneRect VisibleWorldRect(double viewportWidth, double viewportHeight)
    {
        var topLeft = ScreenToWorld(new ScenePoint(0, 0));
        var bottomRight = ScreenToWorld(new ScenePoint(viewportWidth, viewportHeight));
        return new SceneRect(
            Math.Min(topLeft.X, bottomRight.X),
            Math.Min(topLeft.Y, bottomRight.Y),
            Math.Abs(bottomRight.X - topLeft.X),
            Math.Abs(bottomRight.Y - topLeft.Y));
    }

    public SceneRect SceneBounds()
    {
        if (Scene.Entities.Count == 0)
        {
            return new SceneRect(0, 0, 1, 1);
        }

        var first = true;
        double left = 0, top = 0, right = 0, bottom = 0;
        foreach (var entityId in Scene.Entities.Keys)
        {
            var bounds = EntityBounds(entityId);
            if (first)
            {
                left = bounds.X;
                top = bounds.Y;
                right = bounds.Right;
                bottom = bounds.Bottom;
                first = false;
            }
            else
            {
                left = Math.Min(left, bounds.X);
                top = Math.Min(top, bounds.Y);
                right = Math.Max(right, bounds.Right);
                bottom = Math.Max(bottom, bounds.Bottom);
            }
        }
        return new SceneRect(left, top, right - left, bottom - top);
    }

    public IReadOnlyList<string> VisibleEntityIds(double viewportWidth, double viewportHeight)
    {
        var visible = VisibleWorldRect(viewportWidth, viewportHeight);
        var ids = new List<string>();
        foreach (var entityId in Scene.Entities.Keys)
        {
            if (EntityBounds(entityId).Intersects(visible))
            {
                ids.Add(entityId);
            }
        }
        return ids;
    }

    public IReadOnlyList<string> VisibleConnectionIds(double viewportWidth, double viewportHeight)
    {
        var visible = VisibleWorldRect(viewportWidth, viewportHeight);
        var ids = new List<string>();
        foreach (var connectionId in Scene.Connections.Keys)
        {
            if (ConnectionBounds(connectionId).Intersects(visible))
            {
                ids.Add(connectionId);
            }
        }
        return ids;
    }

    public int VisibleSemanticElementCount(double viewportWidth, double viewportHeight)
    {
        var entityIds = VisibleEntityIds(viewportWidth, viewportHeight);
        var terminalCount = 0;
        foreach (var entityId in entityIds)
        {
            terminalCount += Scene.Entities[entityId].Terminals.Count;
        }
        return entityIds.Count + terminalCount + VisibleConnectionIds(viewportWidth, viewportHeight).Count;
    }

    public SceneRect EntityBounds(string entityId)
    {
        var entity = Scene.Entities[entityId];
        var position = Scene.Placements[entityId].Position;
        return entity.Kind == SemanticEntityKind.Busbar
            ? new SceneRect(position.X, position.Y, BusbarWidth, BusbarHeight)
            : new SceneRect(position.X - DeviceWidth / 2, position.Y - DeviceHeight / 2, DeviceWidth, DeviceHeight);
    }

    public SceneRect ConnectionBounds(string connectionId)
    {
        var route = ConnectionRoute(connectionId);
        var left = route.Min(point => point.X);
        var top = route.Min(point => point.Y);
        var right = route.Max(point => point.X);
        var bottom = route.Max(point => point.Y);
        return new SceneRect(left, top, Math.Max(1, right - left), Math.Max(1, bottom - top));
    }

    public string? HitTestEntity(ScenePoint world, double tolerance = 4)
    {
        foreach (var entityId in Scene.Entities.Keys.Reverse())
        {
            if (EntityBounds(entityId).Contains(world, tolerance))
            {
                return entityId;
            }
        }
        return null;
    }

    public TerminalHit? HitTestTerminal(ScenePoint world, double tolerance = 10)
    {
        TerminalHit? best = null;
        var bestDistance = double.MaxValue;
        foreach (var terminal in Scene.Terminals.Values)
        {
            var position = TerminalAnchor(terminal.Id);
            var distance = Distance(world, position);
            if (distance <= tolerance && distance < bestDistance)
            {
                bestDistance = distance;
                best = new TerminalHit(terminal.Id, position);
            }
        }
        return best;
    }

    public string? HitTestConnection(ScenePoint world, double tolerance = 7)
    {
        string? bestId = null;
        var bestDistance = double.MaxValue;
        foreach (var connection in Scene.Connections.Values)
        {
            var route = ConnectionRoute(connection.Id);
            for (var i = 0; i < route.Count - 1; i++)
            {
                var distance = DistanceToSegment(world, route[i], route[i + 1]);
                if (distance <= tolerance && distance < bestDistance)
                {
                    bestDistance = distance;
                    bestId = connection.Id;
                }
            }
        }
        return bestId;
    }

    public ScenePoint TerminalAnchor(string terminalId)
    {
        var terminal = Scene.Terminals[terminalId];
        var entity = Scene.Entities[terminal.EntityId];
        var bounds = EntityBounds(entity.Id);

        if (entity.Kind == SemanticEntityKind.Busbar)
        {
            var index = 0;
            for (var i = 0; i < entity.Terminals.Count; i++)
            {
                if (entity.Terminals[i].Id == terminalId)
                {
                    index = i;
                    break;
                }
            }
            var x = bounds.X + bounds.Width * (index + 1) / (entity.Terminals.Count + 1);
            return new ScenePoint(x, bounds.Y + bounds.Height / 2);
        }

        if (entity.Kind == SemanticEntityKind.EarthingSwitch)
        {
            return terminal.Role.Equals("earth", StringComparison.OrdinalIgnoreCase)
                ? new ScenePoint(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height)
                : new ScenePoint(bounds.X, bounds.Y + bounds.Height / 2);
        }

        return terminal.Role.Contains("a", StringComparison.OrdinalIgnoreCase)
            ? new ScenePoint(bounds.X + bounds.Width / 2, bounds.Y)
            : new ScenePoint(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height);
    }

    public IReadOnlyList<ScenePoint> ConnectionRoute(string connectionId)
    {
        var connection = Scene.Connections[connectionId];
        var from = TerminalAnchor(connection.FromTerminalId);
        var to = TerminalAnchor(connection.ToTerminalId);

        if (Math.Abs(to.Y - from.Y) >= Math.Abs(to.X - from.X))
        {
            var midY = (from.Y + to.Y) / 2;
            return new[] { from, new ScenePoint(from.X, midY), new ScenePoint(to.X, midY), to };
        }

        var midX = (from.X + to.X) / 2;
        return new[] { from, new ScenePoint(midX, from.Y), new ScenePoint(midX, to.Y), to };
    }

    public void MoveEntity(string entityId, ScenePoint target) =>
        History.Execute(Scene, new MoveRepresentationCommand(entityId, target));

    public bool ReconnectSelectedTo(string targetTerminalId)
    {
        if (SelectedConnectionId is null)
        {
            return false;
        }
        History.Execute(Scene, new ReconnectTerminalCommand(SelectedConnectionId, reconnectFrom: false, targetTerminalId));
        return true;
    }

    public bool CycleSelectedState()
    {
        if (SelectedEntityId is null || !Scene.Entities.TryGetValue(SelectedEntityId, out var entity))
        {
            return false;
        }

        var target = entity.State switch
        {
            SemanticState.Closed => SemanticState.Open,
            SemanticState.Open => SemanticState.Intermediate,
            SemanticState.Intermediate => SemanticState.Unknown,
            SemanticState.Unknown => SemanticState.SimulatedClosed,
            _ => SemanticState.Closed
        };
        History.Execute(Scene, new UpdateEntityStateCommand(entity.Id, target));
        return true;
    }

    public bool Undo() => History.Undo(Scene);
    public bool Redo() => History.Redo(Scene);

    public void SetViewport(double zoom, ScenePoint pan)
    {
        Zoom = Math.Clamp(zoom, 0.05, 8.0);
        Pan = pan;
    }

    private void TargetVisibleSemanticElements(int target, double viewportWidth, double viewportHeight)
    {
        if (Scene.Entities.Count == 0)
        {
            return;
        }

        if (target >= Scene.Entities.Count + Scene.Terminals.Count + Scene.Connections.Count)
        {
            FitScene(viewportWidth, viewportHeight, 32);
            return;
        }

        var bounds = SceneBounds();
        var center = new ScenePoint(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
        var low = 0.05;
        var high = 8.0;
        for (var i = 0; i < 18; i++)
        {
            var zoom = (low + high) / 2;
            CenterViewport(center, zoom, viewportWidth, viewportHeight);
            var visible = VisibleSemanticElementCount(viewportWidth, viewportHeight);
            if (visible > target)
            {
                low = zoom;
            }
            else
            {
                high = zoom;
            }
        }
        CenterViewport(center, (low + high) / 2, viewportWidth, viewportHeight);
    }

    private void CenterViewport(ScenePoint center, double zoom, double viewportWidth, double viewportHeight)
    {
        SetViewport(zoom, new ScenePoint(
            viewportWidth / 2 - center.X * zoom,
            viewportHeight / 2 - center.Y * zoom));
    }

    private void FitScene(double viewportWidth, double viewportHeight, double margin)
    {
        var bounds = SceneBounds();
        var usableWidth = Math.Max(1, viewportWidth - margin * 2);
        var usableHeight = Math.Max(1, viewportHeight - margin * 2);
        var zoom = Math.Clamp(Math.Min(usableWidth / Math.Max(1, bounds.Width), usableHeight / Math.Max(1, bounds.Height)), 0.05, 8.0);
        var panX = margin - bounds.X * zoom + (usableWidth - bounds.Width * zoom) / 2;
        var panY = margin - bounds.Y * zoom + (usableHeight - bounds.Height * zoom) / 2;
        SetViewport(zoom, new ScenePoint(panX, panY));
    }

    private static double Distance(ScenePoint a, ScenePoint b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double DistanceToSegment(ScenePoint point, ScenePoint start, ScenePoint end)
    {
        var dx = end.X - start.X;
        var dy = end.Y - start.Y;
        if (Math.Abs(dx) < 0.0001 && Math.Abs(dy) < 0.0001)
        {
            return Distance(point, start);
        }

        var t = ((point.X - start.X) * dx + (point.Y - start.Y) * dy) / (dx * dx + dy * dy);
        t = Math.Clamp(t, 0, 1);
        return Distance(point, new ScenePoint(start.X + t * dx, start.Y + t * dy));
    }
}
