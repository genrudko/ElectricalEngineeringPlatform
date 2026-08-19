using Avalonia;
using System.Text;

namespace Eep.PlatformStack.P1.Avalonia;

internal static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            return 0;
        }
        catch (Exception ex)
        {
            WriteStartupFailure(ex);
            return 70;
        }
    }

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect();

    private static void WriteStartupFailure(Exception ex)
    {
        var report = new StringBuilder()
            .AppendLine("EEP.P1.Avalonia startup failure")
            .AppendLine($"timestamp_utc={DateTimeOffset.UtcNow:O}")
            .AppendLine($"os={Environment.OSVersion}")
            .AppendLine($"framework={Environment.Version}")
            .AppendLine($"base_directory={AppContext.BaseDirectory}")
            .AppendLine()
            .AppendLine(ex.ToString())
            .ToString();

        var targets = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "EEP.P1.Avalonia.startup-failure.log"),
            Path.Combine(Path.GetTempPath(), "EEP.P1.Avalonia.startup-failure.log")
        };

        foreach (var target in targets.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            try
            {
                File.WriteAllText(target, report, Encoding.UTF8);
            }
            catch
            {
                // Best-effort diagnostics must never hide the original startup failure.
            }
        }
    }
}
