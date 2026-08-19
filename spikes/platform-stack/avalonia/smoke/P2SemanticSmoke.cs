using Eep.PlatformStack.P2.Semantics;

internal static class P2SemanticSmoke
{
    public static void Run()
    {
        var scene = P2SemanticFixtureLoader.Load();

        Check(scene.Entities.Values.Any(entity => entity.Kind == SemanticEntityKind.CircuitBreaker), "P2 CircuitBreaker entity");
        Check(scene.Entities.Values.Any(entity => entity.Kind == SemanticEntityKind.Disconnector), "P2 Disconnector entity");
        Check(scene.Entities.Values.Any(entity => entity.Kind == SemanticEntityKind.EarthingSwitch), "P2 EarthingSwitch entity");
        Check(scene.Entities.Values.Any(entity => entity.Kind == SemanticEntityKind.Busbar), "P2 Busbar entity");
        Check(scene.Terminals.Count >= 8, "P2 semantic terminals");
        Check(scene.Connections.Count >= 3, "P2 routed semantic connection baseline");
        Check(scene.ValidationMarkers.Any(marker => marker.Layer == "LAYOUT_WARNING"), "P2 validation marker layer");

        var history = new P2CommandHistory();
        var topologyBeforeMove = scene.TopologyFingerprint();
        var qfBefore = scene.Placements["QF-35-01"].Position;
        var qfMoved = new ScenePoint(qfBefore.X + 40, qfBefore.Y + 20);

        history.Execute(scene, new MoveRepresentationCommand("QF-35-01", qfMoved));
        Check(scene.Placements["QF-35-01"].Position == qfMoved, "P2 representation drag updates placement");
        Check(scene.TopologyFingerprint() == topologyBeforeMove, "P2 representation drag preserves topology");
        Check(history.Undo(scene), "P2 undo move");
        Check(scene.Placements["QF-35-01"].Position == qfBefore, "P2 undo restores placement");
        Check(history.Redo(scene), "P2 redo move");
        Check(scene.Placements["QF-35-01"].Position == qfMoved, "P2 redo restores moved placement");

        var topologyBeforeReconnect = scene.TopologyFingerprint();
        history.Execute(scene, new ReconnectTerminalCommand("C-QF-QS", reconnectFrom: false, targetTerminalId: "T-QSG-35-01-A"));
        Check(scene.Connections["C-QF-QS"].ToTerminalId == "T-QSG-35-01-A", "P2 explicit semantic reconnect");
        Check(scene.TopologyFingerprint() != topologyBeforeReconnect, "P2 reconnect mutates topology");
        Check(history.Undo(scene), "P2 undo reconnect");
        Check(scene.TopologyFingerprint() == topologyBeforeReconnect, "P2 undo reconnect restores topology");

        var qfStateBefore = scene.Entities["QF-35-01"].State;
        history.Execute(scene, new UpdateEntityStateCommand("QF-35-01", SemanticState.Open));
        Check(scene.Entities["QF-35-01"].State == SemanticState.Open, "P2 typed state update");
        Check(history.Undo(scene), "P2 undo state update");
        Check(scene.Entities["QF-35-01"].State == qfStateBefore, "P2 undo restores typed state");

        Console.WriteLine("P2 neutral semantic model smoke: PASS");
    }

    private static void Check(bool condition, string name)
    {
        if (!condition)
        {
            throw new InvalidOperationException($"P2 semantic smoke failed: {name}");
        }
        Console.WriteLine($"PASS {name}");
    }
}
