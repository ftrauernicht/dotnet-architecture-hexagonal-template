using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;

using Microsoft.Extensions.DependencyInjection;

namespace Contoso.App.Ui.Avalonia;

/// <summary>
/// The Avalonia application. Built in code (no XAML) to keep the template minimal. It wires the
/// Fluent theme and, once the desktop lifetime is ready, resolves the main window from DI.
/// </summary>
/// <remarks>
/// The base type is written out as <c>global::Avalonia.Application</c> on purpose. Two collisions
/// make the short forms fail inside this namespace: an unqualified <c>Application</c> binds to the
/// sibling namespace <c>Contoso.App.Application</c> (CS0118), and even <c>Avalonia.Application</c>
/// binds <c>Avalonia</c> to the enclosing <c>...Ui.Avalonia</c> namespace (CS0234). The
/// <c>global::</c> qualifier forces the real Avalonia root.
/// </remarks>
public sealed class App : global::Avalonia.Application
{
    /// <inheritdoc />
    public override void Initialize() => Styles.Add(new FluentTheme());

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = CompositionRoot.Build();
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
