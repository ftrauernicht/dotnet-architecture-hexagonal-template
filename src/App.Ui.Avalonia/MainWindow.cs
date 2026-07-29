using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;

namespace Contoso.App.Ui.Avalonia;

/// <summary>
/// The main window, built in code (no XAML). It binds a single TextBlock to the view model's
/// <c>Status</c> property to prove the DI + MVVM wiring end to end.
/// </summary>
public sealed class MainWindow : Window
{
    public MainWindow()
    {
        Title = "Contoso.App";
        Width = 480;
        Height = 240;

        var status = new TextBlock
        {
            Margin = new Thickness(24),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        status.Bind(TextBlock.TextProperty, new Binding(nameof(MainWindowViewModel.Status)));

        Content = status;
    }
}
