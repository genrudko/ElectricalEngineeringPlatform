namespace Eep.PlatformStack.P2.Semantics;

public sealed class P2SpatialIndex
{
    private const double CellSize = 256.0;

    private readonly P2SemanticScene _scene;
    private readonly Func<string, SceneRect> _entityBounds;
    private readonly Func<string, ScenePoint> _terminalAnchor;
    private readonly Func<string, SceneRect> _connectionBounds;

    private readonly Dictionary<(int X, int Y), HashSet<string>> _entityCells = new();
    private readonly Dictionary<(int X, int Y), HashSet<string>> _terminalCells = new();
    private readonly Dictionary<(int X, int Y), HashSet<string>> _connectionCells = new();
    private readonly Dictionary<string, List<(int X, int Y)>> _entityMembership = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<(int X, int Y)>> _terminalMembership = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<(int X, int Y)>> _connectionMembership = new(StringComparer.Ordinal);

    public P2SpatialIndex(
        P2SemanticScene scene,
        Func<string, SceneRect> entityBounds,
        Func<string, ScenePoint> terminalAnchor,
        Func<string, SceneRect> connectionBounds)
    {
        _scene = scene;
        _entityBounds = entityBounds;
        _terminalAnchor = terminalAnchor;
        _connectionBounds = connectionBounds;
        Rebuild();
    }

    public void Rebuild()
    {
        _entityCells.Clear();
        _terminalCells.Clear();
        _connectionCells.Clear();
        _entityMembership.Clear();
        _terminalMembership.Clear();
        _connectionMembership.Clear();

        foreach (var entityId in _scene.Entities.Keys)
        {
            Add(_entityCells, _entityMembership, entityId, _entityBounds(entityId));
        }
        foreach (var terminalId in _scene.Terminals.Keys)
        {
            Add(_terminalCells, _terminalMembership, terminalId, Around(_terminalAnchor(terminalId), 2));
        }
        foreach (var connectionId in _scene.Connections.Keys)
        {
            Add(_connectionCells, _connectionMembership, connectionId, _connectionBounds(connectionId));
        }
    }

    public IReadOnlyList<string> QueryEntities(SceneRect region) => Query(_entityCells, region);
    public IReadOnlyList<string> QueryTerminals(SceneRect region) => Query(_terminalCells, region);
    public IReadOnlyList<string> QueryConnections(SceneRect region) => Query(_connectionCells, region);

    public void ReindexEntityGeometry(string entityId)
    {
        Remove(_entityCells, _entityMembership, entityId);
        Add(_entityCells, _entityMembership, entityId, _entityBounds(entityId));

        foreach (var terminal in _scene.Entities[entityId].Terminals)
        {
            Remove(_terminalCells, _terminalMembership, terminal.Id);
            Add(_terminalCells, _terminalMembership, terminal.Id, Around(_terminalAnchor(terminal.Id), 2));
        }

        foreach (var connectionId in ConnectionsForEntity(entityId))
        {
            ReindexConnection(connectionId);
        }
    }

    public void ReindexConnection(string connectionId)
    {
        Remove(_connectionCells, _connectionMembership, connectionId);
        Add(_connectionCells, _connectionMembership, connectionId, _connectionBounds(connectionId));
    }

    private IEnumerable<string> ConnectionsForEntity(string entityId)
    {
        var terminalIds = _scene.Entities[entityId].Terminals.Select(terminal => terminal.Id).ToHashSet(StringComparer.Ordinal);
        foreach (var connection in _scene.Connections.Values)
        {
            if (terminalIds.Contains(connection.FromTerminalId) || terminalIds.Contains(connection.ToTerminalId))
            {
                yield return connection.Id;
            }
        }
    }

    private static SceneRect Around(ScenePoint point, double radius) =>
        new(point.X - radius, point.Y - radius, radius * 2, radius * 2);

    private static void Add(
        Dictionary<(int X, int Y), HashSet<string>> cells,
        Dictionary<string, List<(int X, int Y)>> membership,
        string id,
        SceneRect bounds)
    {
        var occupied = Cells(bounds).ToList();
        membership[id] = occupied;
        foreach (var cell in occupied)
        {
            if (!cells.TryGetValue(cell, out var ids))
            {
                ids = new HashSet<string>(StringComparer.Ordinal);
                cells[cell] = ids;
            }
            ids.Add(id);
        }
    }

    private static void Remove(
        Dictionary<(int X, int Y), HashSet<string>> cells,
        Dictionary<string, List<(int X, int Y)>> membership,
        string id)
    {
        if (!membership.Remove(id, out var occupied))
        {
            return;
        }

        foreach (var cell in occupied)
        {
            if (!cells.TryGetValue(cell, out var ids))
            {
                continue;
            }
            ids.Remove(id);
            if (ids.Count == 0)
            {
                cells.Remove(cell);
            }
        }
    }

    private static IReadOnlyList<string> Query(Dictionary<(int X, int Y), HashSet<string>> cells, SceneRect region)
    {
        var result = new HashSet<string>(StringComparer.Ordinal);
        foreach (var cell in Cells(region))
        {
            if (!cells.TryGetValue(cell, out var ids))
            {
                continue;
            }
            result.UnionWith(ids);
        }
        return result.OrderBy(id => id, StringComparer.Ordinal).ToArray();
    }

    private static IEnumerable<(int X, int Y)> Cells(SceneRect bounds)
    {
        var minX = Cell(bounds.X);
        var maxX = Cell(bounds.Right);
        var minY = Cell(bounds.Y);
        var maxY = Cell(bounds.Bottom);
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                yield return (x, y);
            }
        }
    }

    private static int Cell(double coordinate) => (int)Math.Floor(coordinate / CellSize);
}
