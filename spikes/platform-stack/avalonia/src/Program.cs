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
            TraceStartup("program-main-enter");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            TraceStartup("program-main-exit");
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

    internal static void TraceStartup(string stage)
    {
        var explicitTrace = string.Equals(Environment.GetEnvironmentVariable("EEP_P2_STARTUP_TRACE"), "1", StringComparison.Ordinal);
        var ciTrace = string.Equals(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), "true", StringComparison.OrdinalIgnoreCase);
        if (!explicitTrace && !ciTrace)
        {
            return;
        }

        try
        {
            // Reuse the already-collected startup diagnostic channel on CI.
            // Physical owner runs do not create this trace unless explicitly requested.
            var target = Path.Combine(Path.GetTempPath(), "EEP.P1.Avalonia.startup-failure.log");
            File.AppendAllText(target, $"TRACE {DateTimeOffset.UtcNow:O} {stage}{Environment.NewLine}", Encoding.UTF8);
        }
        catch
        {
            // Diagnostics must never alter application behavior.
        }
    }

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
