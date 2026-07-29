using Contoso.App.Application.Ports;
using Contoso.App.Application.UseCases;
using Contoso.App.Infrastructure.Inbound;
using Contoso.App.Infrastructure.Outbound;
using Contoso.App.Infrastructure.Persistence;

using Microsoft.Extensions.DependencyInjection;

namespace Contoso.App.Ui.Avalonia;

/// <summary>
/// The composition root: the ONE place that knows every concrete adapter. Swap an in-memory
/// scaffold for a real adapter by changing a single registration here.
/// </summary>
internal static class CompositionRoot
{
    public static IServiceProvider Build() =>
        new ServiceCollection()
            .AddSingleton<IItemSource, SampleItemSource>()
            .AddSingleton<IItemTarget, InMemoryItemTarget>()
            .AddSingleton<IMigrationLedger, InMemoryLedger>()
            .AddSingleton<ImportItemsUseCase>()
            .AddSingleton<MainWindowViewModel>()
            .BuildServiceProvider();
}
