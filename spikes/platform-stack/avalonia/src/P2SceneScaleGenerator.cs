using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Eep.PlatformStack.P2.Semantics;

public sealed record P2SceneCounts(
    int EntityCount,
    int TerminalCount,
    int ConnectionCount,
    int SemanticElementCount,
    int PlacementCount,
    int ValidationMarkerCount);

public sealed record GeneratedP2Scene(
    string Tier,
    int Seed,
    P2SemanticScene Scene,
    P2SceneCounts Counts,
    string FingerprintSha256);

public sealed class P2SceneTierManifest
{
    public string Schema { get; init; } = string.Empty;
    public int Seed { get; init; }
    public required CountContractDto CountContract { get; init; }
    public required Dictionary<string, int> Tiers { get; init; }
    public required Dictionary<string, JsonElement> ViewportModes { get; init; }

    public static P2SceneTierManifest Load(string? path = null)
    {
        path ??= Path.Combine(AppContext.BaseDirectory, "Fixtures", "p2-scene-tiers.json");
        return JsonSerializer.Deserialize<P2SceneTierManifest>(File.ReadAllText(path), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("P2 scene tier manifest could not be deserialized.");
    }

    public sealed class CountContractDto
    {
        public string SemanticElementCount { get; init; } = string.Empty;
        public string[] ExcludedFromSemanticElementCount { get; init; } = [];
        public string Rationale { get; init; } = string.Empty;
    }
}

public static class P2SceneScaleGenerator
{
    private const int GridColumns = 125;
    private const double GridSpacingX = 112;
    private const double GridSpacingY = 92;

    public static GeneratedP2Scene Generate(string tier, P2SceneTierManifest manifest)
    {
        if (!manifest.Tiers.TryGetValue(tier, out var target))
        {
            throw new ArgumentOutOfRangeException(nameof(tier), tier, "Unknown P2 scene tier.");
        }
        return Generate(tier, target, manifest.Seed);
    }

    public static GeneratedP2Scene Generate(string tier, int semanticElementTarget, int seed)
    {
        if (semanticElementTarget <= 0 || semanticElementTarget % 4 != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(semanticElementTarget), "P2 benchmark target must be a positive multiple of four.");
        }

        // Count contract: E + T + C = N.
        // E=N/4; T=2E+1 (first busbar has one extra benchmark branch terminal); C=E-1.
        // Therefore E + (2E+1) + (E-1) = 4E = N exactly for every frozen tier.
        var entityCount = semanticElementTarget / 4;
        var terminalCount = entityCount * 2 + 1;
        var connectionCount = entityCount - 1;

        var entities = new Dictionary<string, SemanticEntity>(entityCount, StringComparer.Ordinal);
        var terminals = new Dictionary<string, SemanticTerminal>(terminalCount, StringComparer.Ordinal);
        var placements = new Dictionary<string, RepresentationPlacement>(entityCount, StringComparer.Ordinal);
        var connections = new Dictionary<string, SemanticConnection>(connectionCount, StringComparer.Ordinal);
        var markers = new List<ValidationMarker>(Math.Max(1, entityCount / 100));

        string? previousOutputTerminalId = null;
        for (var i = 0; i < entityCount; i++)
        {
            var kind = EntityKindFor(i);
            var entityId = $"{tier}-E{i:D6}";
            var terminalAId = $"{tier}-T{i:D6}-A";
            var terminalBId = $"{tier}-T{i:D6}-B";
            var terminalA = new SemanticTerminal(terminalAId, entityId, InputRole(kind));
            var terminalB = new SemanticTerminal(terminalBId, entityId, OutputRole(kind));
            var entityTerminals = new List<SemanticTerminal>(i == 0 ? 3 : 2) { terminalA, terminalB };

            if (i == 0)
            {
                var extra = new SemanticTerminal($"{tier}-T{i:D6}-X", entityId, "benchmark-branch");
                entityTerminals.Add(extra);
                terminals.Add(extra.Id, extra);
            }

            var entity = new SemanticEntity
            {
                Id = entityId,
                Designation = DesignationFor(kind, i),
                Kind = kind,
                State = StateFor(seed, i),
                Terminals = entityTerminals
            };
            entities.Add(entityId, entity);
            terminals.Add(terminalAId, terminalA);
            terminals.Add(terminalBId, terminalB);

            var row = i / GridColumns;
            var column = i % GridColumns;
            var jitterX = DeterministicOffset(seed, i, 17, 9);
            var jitterY = DeterministicOffset(seed, i, 29, 7);
            placements.Add(entityId, new RepresentationPlacement
            {
                EntityId = entityId,
                Position = new ScenePoint(100 + column * GridSpacingX + jitterX, 100 + row * GridSpacingY + jitterY)
            });

            if (previousOutputTerminalId is not null)
            {
                var connectionId = $"{tier}-C{i - 1:D6}";
                connections.Add(connectionId, new SemanticConnection
                {
                    Id = connectionId,
                    FromTerminalId = previousOutputTerminalId,
                    ToTerminalId = terminalAId
                });
            }

            // Earthing-switch earth terminal remains available to reconnect/hit-test;
            // the line terminal carries the deterministic benchmark backbone.
            previousOutputTerminalId = kind == SemanticEntityKind.EarthingSwitch ? terminalAId : terminalBId;

            if (i % 100 == 0)
            {
                markers.Add(new ValidationMarker(entityId, "LAYOUT_WARNING", "Deterministic P2 scale-fixture validation marker."));
            }
        }

        var scene = new P2SemanticScene
        {
            Entities = entities,
            Terminals = terminals,
            Connections = connections,
            Placements = placements,
            ValidationMarkers = markers
        };

        var counts = new P2SceneCounts(
            entities.Count,
            terminals.Count,
            connections.Count,
            entities.Count + terminals.Count + connections.Count,
            placements.Count,
            markers.Count);

        if (counts.SemanticElementCount != semanticElementTarget)
        {
            throw new InvalidOperationException($"Generated P2 tier {tier} count mismatch: expected {semanticElementTarget}, actual {counts.SemanticElementCount}.");
        }

        return new GeneratedP2Scene(tier, seed, scene, counts, Fingerprint(scene, counts, seed));
    }

    private static SemanticEntityKind EntityKindFor(int index) => index % 4 switch
    {
        0 => SemanticEntityKind.Busbar,
        1 => SemanticEntityKind.CircuitBreaker,
        2 => SemanticEntityKind.Disconnector,
        _ => SemanticEntityKind.EarthingSwitch
    };

    private static string DesignationFor(SemanticEntityKind kind, int index) => kind switch
    {
        SemanticEntityKind.Busbar => $"BUS-{index:D5}",
        SemanticEntityKind.CircuitBreaker => $"QF-{index:D5}",
        SemanticEntityKind.Disconnector => $"QS-{index:D5}",
        _ => $"QSG-{index:D5}"
    };

    private static string InputRole(SemanticEntityKind kind) => kind == SemanticEntityKind.Busbar ? "branch-a" : "line-a";

    private static string OutputRole(SemanticEntityKind kind) => kind switch
    {
        SemanticEntityKind.Busbar => "branch-b",
        SemanticEntityKind.EarthingSwitch => "earth",
        _ => "line-b"
    };

    private static SemanticState StateFor(int seed, int index)
    {
        var value = DeterministicWord(seed, index);
        return value % 20 switch
        {
            0 => SemanticState.Unknown,
            1 => SemanticState.Intermediate,
            2 => SemanticState.SimulatedClosed,
            < 11 => SemanticState.Open,
            _ => SemanticState.Closed
        };
    }

    private static double DeterministicOffset(int seed, int index, int salt, int amplitude)
    {
        var word = DeterministicWord(seed ^ salt, index);
        return (word % (uint)(amplitude * 2 + 1)) - amplitude;
    }

    private static uint DeterministicWord(int seed, int index)
    {
        unchecked
        {
            var x = (uint)seed ^ ((uint)index * 0x9E3779B9u);
            x ^= x >> 16;
            x *= 0x7FEB352Du;
            x ^= x >> 15;
            x *= 0x846CA68Bu;
            x ^= x >> 16;
            return x;
        }
    }

    private static string Fingerprint(P2SemanticScene scene, P2SceneCounts counts, int seed)
    {
        var builder = new StringBuilder();
        builder.Append(seed).Append('|')
            .Append(counts.EntityCount).Append('|')
            .Append(counts.TerminalCount).Append('|')
            .Append(counts.ConnectionCount).Append('|')
            .Append(counts.PlacementCount).Append('|');

        foreach (var entity in scene.Entities.Values.OrderBy(value => value.Id, StringComparer.Ordinal))
        {
            var placement = scene.Placements[entity.Id].Position;
            builder.Append(entity.Id).Append(':').Append(entity.Kind).Append(':').Append(entity.State).Append('@')
                .Append(placement.X.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)).Append(',')
                .Append(placement.Y.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)).Append(';');
        }
        builder.Append('|').Append(scene.TopologyFingerprint());

        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString()))).ToLowerInvariant();
    }
}
