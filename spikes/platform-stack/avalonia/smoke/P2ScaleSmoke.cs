using Eep.PlatformStack.P2.Semantics;

internal static class P2ScaleSmoke
{
    public static void Run()
    {
        var manifest = P2SceneTierManifest.Load();
        Check(manifest.Schema == "eep.p2-scene-tiers/v1", "P2 tier manifest schema");
        Check(manifest.CountContract.SemanticElementCount == "entities + terminals + connections", "P2 explicit semantic count contract");
        Check(manifest.Tiers.Count == 4, "P2 four frozen scene tiers");
        Check(manifest.Tiers["S"] == 2_000, "P2 S target");
        Check(manifest.Tiers["M"] == 10_000, "P2 M target");
        Check(manifest.Tiers["L"] == 25_000, "P2 L target");
        Check(manifest.Tiers["XL"] == 50_000, "P2 XL target");

        string? sFingerprint = null;
        foreach (var tier in new[] { "S", "M", "L", "XL" })
        {
            var generated = P2SceneScaleGenerator.Generate(tier, manifest);
            var target = manifest.Tiers[tier];
            var expectedEntities = target / 4;

            Check(generated.Counts.SemanticElementCount == target, $"P2 {tier} exact semantic element count");
            Check(generated.Counts.EntityCount == expectedEntities, $"P2 {tier} entity count");
            Check(generated.Counts.TerminalCount == expectedEntities * 2 + 1, $"P2 {tier} terminal count");
            Check(generated.Counts.ConnectionCount == expectedEntities - 1, $"P2 {tier} connection count");
            Check(generated.Counts.PlacementCount == expectedEntities, $"P2 {tier} placement count separate from semantic denominator");
            Check(generated.Scene.Entities.Values.Select(e => e.Kind).Distinct().Count() == 4, $"P2 {tier} all mandatory entity kinds represented");
            Check(generated.Scene.ValidationMarkers.Count > 0, $"P2 {tier} validation overlay represented");
            Check(generated.FingerprintSha256.Length == 64, $"P2 {tier} deterministic fingerprint format");

            if (tier == "S")
            {
                sFingerprint = generated.FingerprintSha256;
            }

            Console.WriteLine($"P2_TIER {tier} semantic={generated.Counts.SemanticElementCount} entities={generated.Counts.EntityCount} terminals={generated.Counts.TerminalCount} connections={generated.Counts.ConnectionCount} placements={generated.Counts.PlacementCount} markers={generated.Counts.ValidationMarkerCount} sha256={generated.FingerprintSha256}");
        }

        var repeatS = P2SceneScaleGenerator.Generate("S", manifest);
        Check(repeatS.FingerprintSha256 == sFingerprint, "P2 generator deterministic repeat for same seed/tier");

        Console.WriteLine("P2 deterministic scale generator smoke: PASS");
    }

    private static void Check(bool condition, string name)
    {
        if (!condition)
        {
            throw new InvalidOperationException($"P2 scale smoke failed: {name}");
        }
        Console.WriteLine($"PASS {name}");
    }
}
