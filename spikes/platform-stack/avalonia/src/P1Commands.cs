namespace Eep.PlatformStack.P1.Avalonia;

public static class P1Commands
{
    public static IReadOnlyDictionary<string, string> Shortcuts { get; } = new Dictionary<string, string>
    {
        ["open"] = "Ctrl+O",
        ["save"] = "Ctrl+S",
        ["undo"] = "Ctrl+Z",
        ["redo"] = "Ctrl+Y"
    };
}
