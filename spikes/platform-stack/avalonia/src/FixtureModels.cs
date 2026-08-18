using System.Text.Json;

namespace Eep.PlatformStack.P1.Avalonia;

public sealed class P1Fixture
{
    public string Schema { get; init; } = string.Empty;
    public string ApplicationTitle { get; init; } = string.Empty;
    public List<DocumentFixture> Documents { get; init; } = [];
    public EquipmentNodeFixture EquipmentTree { get; init; } = new();
    public string SelectedEquipmentId { get; init; } = string.Empty;
    public Dictionary<string, EquipmentFixture> Equipment { get; init; } = [];
    public GalleryFixture Gallery { get; init; } = new();
    public StatusFixture Status { get; init; } = new();
}

public sealed class DocumentFixture
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Kind { get; init; } = string.Empty;
}

public sealed class EquipmentNodeFixture
{
    public string Id { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public bool Expanded { get; init; }
    public List<EquipmentNodeFixture> Children { get; init; } = [];
}

public sealed class EquipmentFixture
{
    public string Designation { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public List<PropertyFixture> Properties { get; init; } = [];
}

public sealed class PropertyFixture
{
    public string Key { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string? DisplayValue { get; init; }
    public string State { get; init; } = "normal";
    public bool Editable { get; init; }
    public string? Message { get; init; }
}

public sealed class GalleryFixture
{
    public string TextInput { get; init; } = string.Empty;
    public decimal NumericInput { get; init; }
    public List<string> ComboOptions { get; init; } = [];
    public string ComboSelected { get; init; } = string.Empty;
    public bool Checkbox { get; init; }
    public List<string> RadioOptions { get; init; } = [];
    public string RadioSelected { get; init; } = string.Empty;
    public string LongLabel { get; init; } = string.Empty;
    public string MultilineError { get; init; } = string.Empty;
    public List<NotificationFixture> Notifications { get; init; } = [];
}

public sealed class NotificationFixture
{
    public string Severity { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
}

public sealed class StatusFixture
{
    public string Project { get; init; } = string.Empty;
    public string Diagnostics { get; init; } = string.Empty;
    public string Connection { get; init; } = string.Empty;
}

public static class P1FixtureLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static P1Fixture Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "p1-shell-fixture.json");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("P1 shared fixture was not copied to the application output.", path);
        }

        var fixture = JsonSerializer.Deserialize<P1Fixture>(File.ReadAllText(path), Options)
            ?? throw new InvalidDataException("P1 shared fixture deserialized to null.");

        if (fixture.Schema != "eep.p1-shell-fixture/v1")
        {
            throw new InvalidDataException($"Unexpected P1 fixture schema: {fixture.Schema}");
        }

        return fixture;
    }
}
