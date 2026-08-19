using System.Text.Json;

namespace Eep.PlatformStack.P2.Semantics;

public enum SemanticEntityKind
{
    CircuitBreaker,
    Disconnector,
    EarthingSwitch,
    Busbar
}

public enum SemanticState
{
    Open,
    Closed,
    Intermediate,
    Unknown,
    SimulatedClosed
}

public readonly record struct ScenePoint(double X, double Y);

public sealed record SemanticTerminal(string Id, string EntityId, string Role);

public sealed class SemanticEntity
{
    public required string Id { get; init; }
    public required string Designation { get; init; }
    public required SemanticEntityKind Kind { get; init; }
    public required SemanticState State { get; set; }
    public required IReadOnlyList<SemanticTerminal> Terminals { get; init; }
}

public sealed class SemanticConnection
{
    public required string Id { get; init; }
    public required string FromTerminalId { get; set; }
    public required string ToTerminalId { get; set; }
}

public sealed class RepresentationPlacement
{
    public required string EntityId { get; init; }
    public required ScenePoint Position { get; set; }
}

public sealed record ValidationMarker(string EntityId, string Layer, string Message);

public sealed class P2SemanticScene
{
    public required Dictionary<string, SemanticEntity> Entities { get; init; }
    public required Dictionary<string, SemanticTerminal> Terminals { get; init; }
    public required Dictionary<string, SemanticConnection> Connections { get; init; }
    public required Dictionary<string, RepresentationPlacement> Placements { get; init; }
    public required IReadOnlyList<ValidationMarker> ValidationMarkers { get; init; }

    public string TopologyFingerprint() => string.Join(
        "|",
        Connections.Values
            .OrderBy(connection => connection.Id, StringComparer.Ordinal)
            .Select(connection => $"{connection.Id}:{connection.FromTerminalId}>{connection.ToTerminalId}"));
}

public static class P2SemanticFixtureLoader
{
    public static P2SemanticScene Load(string? path = null)
    {
        path ??= Path.Combine(AppContext.BaseDirectory, "Fixtures", "p2-semantic-scene.json");
        var json = File.ReadAllText(path);
        var dto = JsonSerializer.Deserialize<SceneDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("P2 semantic fixture could not be deserialized.");

        var entities = new Dictionary<string, SemanticEntity>(StringComparer.Ordinal);
        var terminals = new Dictionary<string, SemanticTerminal>(StringComparer.Ordinal);

        foreach (var entityDto in dto.Entities)
        {
            var entityTerminals = entityDto.Terminals
                .Select(terminal => new SemanticTerminal(terminal.Id, entityDto.Id, terminal.Role))
                .ToArray();

            var entity = new SemanticEntity
            {
                Id = entityDto.Id,
                Designation = entityDto.Designation,
                Kind = Enum.Parse<SemanticEntityKind>(entityDto.Kind, ignoreCase: true),
                State = Enum.Parse<SemanticState>(entityDto.State, ignoreCase: true),
                Terminals = entityTerminals
            };

            if (!entities.TryAdd(entity.Id, entity))
            {
                throw new InvalidOperationException($"Duplicate P2 entity id: {entity.Id}");
            }

            foreach (var terminal in entityTerminals)
            {
                if (!terminals.TryAdd(terminal.Id, terminal))
                {
                    throw new InvalidOperationException($"Duplicate P2 terminal id: {terminal.Id}");
                }
            }
        }

        var connections = dto.Connections.ToDictionary(
            connection => connection.Id,
            connection => new SemanticConnection
            {
                Id = connection.Id,
                FromTerminalId = connection.FromTerminalId,
                ToTerminalId = connection.ToTerminalId
            },
            StringComparer.Ordinal);

        foreach (var connection in connections.Values)
        {
            RequireTerminal(terminals, connection.FromTerminalId, connection.Id);
            RequireTerminal(terminals, connection.ToTerminalId, connection.Id);
        }

        var placements = dto.Placements.ToDictionary(
            placement => placement.EntityId,
            placement => new RepresentationPlacement
            {
                EntityId = placement.EntityId,
                Position = new ScenePoint(placement.X, placement.Y)
            },
            StringComparer.Ordinal);

        foreach (var placement in placements.Values)
        {
            if (!entities.ContainsKey(placement.EntityId))
            {
                throw new InvalidOperationException($"Placement references unknown entity: {placement.EntityId}");
            }
        }

        var markers = dto.ValidationMarkers
            .Select(marker => new ValidationMarker(marker.EntityId, marker.Layer, marker.Message))
            .ToArray();

        return new P2SemanticScene
        {
            Entities = entities,
            Terminals = terminals,
            Connections = connections,
            Placements = placements,
            ValidationMarkers = markers
        };
    }

    private static void RequireTerminal(IReadOnlyDictionary<string, SemanticTerminal> terminals, string terminalId, string connectionId)
    {
        if (!terminals.ContainsKey(terminalId))
        {
            throw new InvalidOperationException($"Connection {connectionId} references unknown terminal {terminalId}.");
        }
    }

    private sealed class SceneDto
    {
        public List<EntityDto> Entities { get; set; } = [];
        public List<ConnectionDto> Connections { get; set; } = [];
        public List<PlacementDto> Placements { get; set; } = [];
        public List<ValidationMarkerDto> ValidationMarkers { get; set; } = [];
    }

    private sealed class EntityDto
    {
        public string Id { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public List<TerminalDto> Terminals { get; set; } = [];
    }

    private sealed class TerminalDto
    {
        public string Id { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    private sealed class ConnectionDto
    {
        public string Id { get; set; } = string.Empty;
        public string FromTerminalId { get; set; } = string.Empty;
        public string ToTerminalId { get; set; } = string.Empty;
    }

    private sealed class PlacementDto
    {
        public string EntityId { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
    }

    private sealed class ValidationMarkerDto
    {
        public string EntityId { get; set; } = string.Empty;
        public string Layer { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
