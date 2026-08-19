namespace Eep.PlatformStack.P2.Semantics;

public interface IP2SemanticCommand
{
    void Execute(P2SemanticScene scene);
    void Undo(P2SemanticScene scene);
}

public sealed class MoveRepresentationCommand(string entityId, ScenePoint target) : IP2SemanticCommand
{
    private ScenePoint _before;
    private bool _captured;

    public void Execute(P2SemanticScene scene)
    {
        var placement = scene.Placements[entityId];
        if (!_captured)
        {
            _before = placement.Position;
            _captured = true;
        }
        placement.Position = target;
    }

    public void Undo(P2SemanticScene scene) => scene.Placements[entityId].Position = _before;
}

public sealed class ReconnectTerminalCommand(string connectionId, bool reconnectFrom, string targetTerminalId) : IP2SemanticCommand
{
    private string _before = string.Empty;
    private bool _captured;

    public void Execute(P2SemanticScene scene)
    {
        if (!scene.Terminals.ContainsKey(targetTerminalId))
        {
            throw new InvalidOperationException($"Reconnect target terminal does not exist: {targetTerminalId}");
        }

        var connection = scene.Connections[connectionId];
        if (!_captured)
        {
            _before = reconnectFrom ? connection.FromTerminalId : connection.ToTerminalId;
            _captured = true;
        }

        if (reconnectFrom)
        {
            connection.FromTerminalId = targetTerminalId;
        }
        else
        {
            connection.ToTerminalId = targetTerminalId;
        }
    }

    public void Undo(P2SemanticScene scene)
    {
        var connection = scene.Connections[connectionId];
        if (reconnectFrom)
        {
            connection.FromTerminalId = _before;
        }
        else
        {
            connection.ToTerminalId = _before;
        }
    }
}

public sealed class UpdateEntityStateCommand(string entityId, SemanticState targetState) : IP2SemanticCommand
{
    private SemanticState _before;
    private bool _captured;

    public void Execute(P2SemanticScene scene)
    {
        var entity = scene.Entities[entityId];
        if (!_captured)
        {
            _before = entity.State;
            _captured = true;
        }
        entity.State = targetState;
    }

    public void Undo(P2SemanticScene scene) => scene.Entities[entityId].State = _before;
}

public sealed class P2CommandHistory
{
    private readonly Stack<IP2SemanticCommand> _undo = new();
    private readonly Stack<IP2SemanticCommand> _redo = new();

    public int UndoCount => _undo.Count;
    public int RedoCount => _redo.Count;

    public void Execute(P2SemanticScene scene, IP2SemanticCommand command)
    {
        command.Execute(scene);
        _undo.Push(command);
        _redo.Clear();
    }

    public bool Undo(P2SemanticScene scene)
    {
        if (!_undo.TryPop(out var command))
        {
            return false;
        }

        command.Undo(scene);
        _redo.Push(command);
        return true;
    }

    public bool Redo(P2SemanticScene scene)
    {
        if (!_redo.TryPop(out var command))
        {
            return false;
        }

        command.Execute(scene);
        _undo.Push(command);
        return true;
    }
}
