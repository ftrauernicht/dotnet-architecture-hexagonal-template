using Contoso.App.Application.Ports;
using Contoso.App.Domain;

namespace Contoso.App.Infrastructure.Inbound;

/// <summary>
/// Scaffolding source that returns a fixed in-memory sample. Replace with the real inbound
/// adapter (database, API or file reader) for your source system.
/// </summary>
public sealed class SampleItemSource : IItemSource
{
    /// <inheritdoc />
    public IReadOnlyCollection<Item> Read() =>
    [
        new Item("1001", "Alpha"),
        new Item("1002", "Beta"),
    ];
}
