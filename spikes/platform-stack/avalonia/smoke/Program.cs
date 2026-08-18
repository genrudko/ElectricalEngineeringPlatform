using Avalonia.Headless;
using Eep.PlatformStack.P1.Avalonia;

Console.WriteLine("P1 Avalonia smoke: starting headless session");
using var session = HeadlessUnitTestSession.StartNew(typeof(App));
using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(20));

Console.WriteLine("P1 Avalonia smoke: dispatching shell checks");
await session.Dispatch(() =>
{
    Console.WriteLine("P1 Avalonia smoke: constructing MainWindow");
    var window = new MainWindow();
    Console.WriteLine("P1 Avalonia smoke: showing MainWindow");
    window.Show();

    Check(window.FixtureSchema == "eep.p1-shell-fixture/v1", "fixture schema");
    Check(window.DocumentTitles.SequenceEqual(new[]
    {
        "Однолинейная схема КРУ 35 кВ",
        "Схема собственных нужд 0,4 кВ",
        "Импорт оборудования — предварительный просмотр",
        "UI Gallery"
    }), "document tabs");
    Check(window.EquipmentIds.Contains("QF-35-01"), "equipment tree contains QF-35-01");
    Check(window.SelectEquipmentForTest("QF-35-01"), "equipment selection");
    Check(window.SelectedEquipmentId == "QF-35-01", "properties selection routing");
    Check(window.RenderedPropertyStates.Contains("warning"), "warning state");
    Check(window.RenderedPropertyStates.Contains("error"), "error state");
    Check(window.RenderedPropertyStates.Contains("unknown"), "UNKNOWN state");
    Check(window.RenderedPropertyModes.Contains("read-only"), "read-only property state");
    Check(window.Shortcuts["open"] == "Ctrl+O", "Ctrl+O mapping");
    Check(window.Shortcuts["save"] == "Ctrl+S", "Ctrl+S mapping");
    Check(window.Shortcuts["undo"] == "Ctrl+Z", "Ctrl+Z mapping");
    Check(window.Shortcuts["redo"] == "Ctrl+Y", "Ctrl+Y mapping");

    window.SelectDocumentForTest(3);
    Check(window.IsUiGallerySelected, "UI Gallery tab switching");
    Check(window.Content is not null, "window content rendered");

    window.Close();
    Console.WriteLine("P1 Avalonia smoke: shell checks complete");
}, timeout.Token);

Console.WriteLine("P1 Avalonia headless smoke: PASS");

static void Check(bool condition, string name)
{
    if (!condition)
    {
        throw new InvalidOperationException($"P1 Avalonia smoke failed: {name}");
    }
    Console.WriteLine($"PASS {name}");
}
