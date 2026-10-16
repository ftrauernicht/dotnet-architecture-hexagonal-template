using CommunityToolkit.Mvvm.ComponentModel;

using Contoso.App.Application.UseCases;

namespace Contoso.App.Ui.Avalonia;

/// <summary>
/// Demonstrates MVVM with CommunityToolkit source generators: <c>[ObservableProperty]</c> turns
/// the <c>_status</c> field into an observable <c>Status</c> property. On construction it runs the
/// import use case once and reports what the run did.
/// </summary>
public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _status;

    public MainWindowViewModel(ImportItemsUseCase importItems)
    {
        var summary = importItems.Execute();
        _status = $"Read {summary.Read} item(s): {summary.Written} written, "
            + $"{summary.SkippedAsUnchanged} already up to date.";
    }
}
