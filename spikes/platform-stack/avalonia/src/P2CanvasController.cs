namespace Eep.PlatformStack.P2.Semantics;

public readonly record struct SceneRect(double X, double Y, double Width, double Height)
{
    public bool Contains(ScenePoint point, double tolerance = 0) =>
        point.X >= X - tolerance && point.X <= X + Width + tolerance &&
        point.Y >= Y - tolerance && point.Y <= Y + Height + tolerance;
}

public readonly record struct TerminalHit(string TerminalId, ScenePoint Position);

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
        var target = Math.Clamp(Zoom * factor, 0.25, 4.0);
        Zoom = target;
        Pan = new ScenePoint(
            screenAnchor.X - worldAnchor.X * Zoom,
            screenAnchor.Y - worldAnchor.Y * Zoom);
    }

    public SceneRect EntityBounds(string entityId)
    {
        var entity = Scene.Entities[entityId];
        var position = Scene.Placements[entityId].Position;
        return entity.Kind == SemanticEntityKind.Busbar
            ? new SceneRect(position.X, position.Y, BusbarWidth, BusbarHeight)
            : new SceneRect(position.X - DeviceWidth / 2, position.Y - DeviceHeight / 2, DeviceWidth, DeviceHeight);
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
