using System.Collections.Concurrent;

using Contoso.App.Application.Ports;
using Contoso.App.Domain;

namespace Contoso.App.Infrastructure.Outbound;

/// <summary>
/// Scaffolding target that collects written items in memory. Replace with the real outbound
/// adapter (the API or package the target system ingests).
/// </summary>
public sealed class InMemoryItemTarget : IItemTarget
{
    private readonly ConcurrentBag<Item> _written = [];

    /// <summary>The items written so far — exposed for tests and diagnostics.</summary>
    public IReadOnlyCollection<Item> Written => _written;

    /// <inheritdoc />
    public void Write(Item item) => _written.Add(item);
}
