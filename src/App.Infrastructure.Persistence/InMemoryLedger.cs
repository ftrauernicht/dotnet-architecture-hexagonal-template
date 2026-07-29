using System.Collections.Concurrent;

using Contoso.App.Application.Ports;

namespace Contoso.App.Infrastructure.Persistence;

/// <summary>
/// Scaffolding ledger that keeps idempotency state in memory. Replace with a durable store
/// (for example a local SQLite file) so a re-run after a crash still skips written items.
/// </summary>
public sealed class InMemoryLedger : IMigrationLedger
{
    private readonly ConcurrentDictionary<string, string> _seen = new();

    /// <inheritdoc />
    public bool AlreadyProcessed(string externalId, string contentHash) =>
        _seen.TryGetValue(externalId, out var existing) && existing == contentHash;

    /// <inheritdoc />
    public void Record(string externalId, string contentHash) => _seen[externalId] = contentHash;
}
