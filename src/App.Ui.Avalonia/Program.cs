using Avalonia;

namespace Contoso.App.Ui.Avalonia;

/// <summary>Process entry point. Builds the Avalonia app and starts the desktop lifetime.</summary>
internal static class Program
{
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    /// <summary>Avalonia configuration — also used by the visual designer.</summary>
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
