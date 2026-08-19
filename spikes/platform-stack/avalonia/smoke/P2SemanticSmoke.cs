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
        history.Undo(scene);

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

        var controller = new P2CanvasController(scene);
        var qfPosition = scene.Placements["QF-35-01"].Position;
        Check(controller.HitTestEntity(qfPosition) == "QF-35-01", "P2 canvas entity hit-test");

        var terminalPosition = controller.TerminalAnchor("T-QF-35-01-A");
        var terminalHit = controller.HitTestTerminal(terminalPosition, 1);
        Check(terminalHit?.TerminalId == "T-QF-35-01-A", "P2 semantic terminal hit-test");

        var screenBefore = controller.WorldToScreen(qfPosition);
        var roundTrip = controller.ScreenToWorld(screenBefore);
        Check(Close(roundTrip.X, qfPosition.X) && Close(roundTrip.Y, qfPosition.Y), "P2 world/screen transform round-trip");

        var anchorScreen = new ScenePoint(400, 260);
        var anchorWorldBefore = controller.ScreenToWorld(anchorScreen);
        controller.ZoomAt(anchorScreen, 1.5);
        var anchorWorldAfter = controller.ScreenToWorld(anchorScreen);
        Check(Close(anchorWorldBefore.X, anchorWorldAfter.X) && Close(anchorWorldBefore.Y, anchorWorldAfter.Y), "P2 zoom preserves pointer world anchor");

        var mappingBeforePan = controller.WorldToScreen(qfPosition);
        controller.PanBy(new ScenePoint(25, -10));
        var mappingAfterPan = controller.WorldToScreen(qfPosition);
        Check(Close(mappingAfterPan.X - mappingBeforePan.X, 25) && Close(mappingAfterPan.Y - mappingBeforePan.Y, -10), "P2 pan mapping");

        controller.SelectedConnectionId = "C-QF-QS";
        var controllerTopologyBefore = scene.TopologyFingerprint();
        Check(controller.ReconnectSelectedTo("T-QSG-35-01-A"), "P2 controller explicit reconnect command path");
        Check(scene.TopologyFingerprint() != controllerTopologyBefore, "P2 controller reconnect changes topology");
        Check(controller.Undo(), "P2 controller undo reconnect");
        Check(scene.TopologyFingerprint() == controllerTopologyBefore, "P2 controller undo restores topology");

        controller.SelectedEntityId = "QF-35-01";
        var stateBeforeCycle = scene.Entities["QF-35-01"].State;
        Check(controller.CycleSelectedState(), "P2 controller typed state command path");
        Check(scene.Entities["QF-35-01"].State != stateBeforeCycle, "P2 controller state changes");
        Check(controller.Undo(), "P2 controller undo state");
        Check(scene.Entities["QF-35-01"].State == stateBeforeCycle, "P2 controller undo restores state");

        Console.WriteLine("P2 neutral semantic model + canvas controller smoke: PASS");
    }

    private static bool Close(double a, double b) => Math.Abs(a - b) < 0.0001;

    private static void Check(bool condition, string name)
    {
        if (!condition)
        {
            throw new InvalidOperationException($"P2 semantic smoke failed: {name}");
        }
        Console.WriteLine($"PASS {name}");
    }
}
