using Eep.PlatformStack.P1.Avalonia;

var fixture = P1FixtureLoader.Load();

Check(fixture.Schema == "eep.p1-shell-fixture/v1", "fixture schema");
Check(fixture.ApplicationTitle == "Electrical Engineering Platform", "application title");
Check(fixture.Documents.Select(d => d.Title).SequenceEqual(new[]
{
    "Однолинейная схема КРУ 35 кВ",
    "Схема собственных нужд 0,4 кВ",
    "Импорт оборудования — предварительный просмотр",
    "UI Gallery"
}), "document surface");

var equipmentIds = new HashSet<string>(StringComparer.Ordinal);
Collect(fixture.EquipmentTree, equipmentIds);
Check(equipmentIds.Contains("QF-35-01"), "equipment tree contains QF-35-01");
Check(equipmentIds.Contains("QS-35-01"), "equipment tree contains QS-35-01");
Check(equipmentIds.Contains("QSG-35-01"), "equipment tree contains QSG-35-01");

foreach (var leafId in new[] { "QF-35-01", "QS-35-01", "QSG-35-01" })
{
    Check(fixture.Equipment.TryGetValue(leafId, out var leafEquipment), $"equipment leaf {leafId} resolves inspector data");
    Check(leafEquipment!.Designation == leafId, $"equipment leaf {leafId} inspector designation matches selection");
    Check(leafEquipment.Properties.Count > 0, $"equipment leaf {leafId} inspector has properties");
}

Check(fixture.Equipment.TryGetValue(fixture.SelectedEquipmentId, out var selected), "selected equipment resolves");
Check(selected!.Designation == "QF-35-01", "selected designation");
Check(selected.Name == "Выключатель 35 кВ", "selected equipment name");
var states = selected.Properties.Select(p => p.State).ToHashSet(StringComparer.Ordinal);
Check(states.Contains("normal"), "normal state");
Check(states.Contains("warning"), "warning state");
Check(states.Contains("error"), "error state");
Check(states.Contains("unknown"), "UNKNOWN state");
Check(selected.Properties.Any(p => !p.Editable), "read-only property state");
Check(selected.Properties.Any(p => p.Editable), "editable property state");

Check(P1Commands.Shortcuts["open"] == "Ctrl+O", "Ctrl+O mapping");
Check(P1Commands.Shortcuts["save"] == "Ctrl+S", "Ctrl+S mapping");
Check(P1Commands.Shortcuts["undo"] == "Ctrl+Z", "Ctrl+Z mapping");
Check(P1Commands.Shortcuts["redo"] == "Ctrl+Y", "Ctrl+Y mapping");

Check(fixture.Gallery.Notifications.Select(n => n.Severity).SequenceEqual(new[] { "info", "warning", "error" }), "notification states");
Check(fixture.Gallery.LongLabel.Contains("Диспетчерское наименование", StringComparison.Ordinal), "long Russian label");
Check(fixture.Gallery.MultilineError.Contains('\n'), "multiline error text");
Check(fixture.EquipmentTree.Expanded, "expanded root state");
Check(fixture.EquipmentTree.Children.Any(n => !n.Expanded), "collapsed tree state");

var typographyPath = Path.Combine(AppContext.BaseDirectory, "Fixtures", "typography.txt");
var typography = File.ReadAllText(typographyPath);
foreach (var expected in new[]
{
    "Ё", "ё", "КРУ 35 кВ", "Выключатель QF-35-01", "2500 А", "52,4 МВт", "−4,8 Мвар", "№ 12", "ΔP = 1,5 %", "Состояние неизвестно"
})
{
    Check(typography.Contains(expected, StringComparison.Ordinal), $"typography corpus: {expected}");
}

Console.WriteLine("P1 Avalonia presentation-behavior smoke: PASS");
P2SemanticSmoke.Run();

static void Collect(EquipmentNodeFixture node, ISet<string> ids)
{
    ids.Add(node.Id);
    foreach (var child in node.Children)
    {
        Collect(child, ids);
    }
}

static void Check(bool condition, string name)
{
    if (!condition)
    {
        throw new InvalidOperationException($"P1 Avalonia smoke failed: {name}");
    }
    Console.WriteLine($"PASS {name}");
}
