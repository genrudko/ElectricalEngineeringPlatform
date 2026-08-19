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
        GeneratedP2Scene? xl = null;
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
            if (tier == "XL")
            {
                xl = generated;
            }

            Console.WriteLine($"P2_TIER {tier} semantic={generated.Counts.SemanticElementCount} entities={generated.Counts.EntityCount} terminals={generated.Counts.TerminalCount} connections={generated.Counts.ConnectionCount} placements={generated.Counts.PlacementCount} markers={generated.Counts.ValidationMarkerCount} sha256={generated.FingerprintSha256}");
        }

        var repeatS = P2SceneScaleGenerator.Generate("S", manifest);
        Check(repeatS.FingerprintSha256 == sFingerprint, "P2 generator deterministic repeat for same seed/tier");

        Check(xl is not null, "P2 XL retained for viewport smoke");
        var controller = new P2CanvasController(xl!.Scene);
        const double viewportWidth = 900;
        const double viewportHeight = 700;

        controller.ApplyViewportMode(P2ViewportMode.Normal, viewportWidth, viewportHeight);
        var normalVisible = controller.VisibleSemanticElementCount(viewportWidth, viewportHeight);
        Check(normalVisible >= 350 && normalVisible <= 700, "P2 NORMAL approximately 500 visible semantic elements");

        controller.ApplyViewportMode(P2ViewportMode.Dense, viewportWidth, viewportHeight);
        var denseVisible = controller.VisibleSemanticElementCount(viewportWidth, viewportHeight);
        Check(denseVisible >= 1_500 && denseVisible <= 2_500, "P2 DENSE approximately 2000 visible semantic elements");
        Check(denseVisible > normalVisible, "P2 DENSE exposes more semantic elements than NORMAL");

        controller.ApplyViewportMode(P2ViewportMode.ZoomToFit, viewportWidth, viewportHeight);
        var fitVisible = controller.VisibleSemanticElementCount(viewportWidth, viewportHeight);
        Check(fitVisible >= 45_000, "P2 ZOOM_TO_FIT exposes the overwhelming majority of XL scene");
        Check(controller.Zoom >= 0.05, "P2 zoom-to-fit respects bounded minimum zoom");

        Console.WriteLine($"P2_VIEWPORT NORMAL={normalVisible} DENSE={denseVisible} ZOOM_TO_FIT={fitVisible} fit_zoom={controller.Zoom:F4}");
        Console.WriteLine("P2 deterministic scale + viewport smoke: PASS");
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
