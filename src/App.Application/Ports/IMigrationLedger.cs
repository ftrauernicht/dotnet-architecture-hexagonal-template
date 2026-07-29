namespace Contoso.App.Application.Ports;

/// <summary>
/// Outbound port for idempotency: records which items have already been written, keyed by the
/// source <c>externalId</c> plus a content hash, so a re-run skips unchanged items.
/// </summary>
public interface IMigrationLedger
{
    /// <summary>Returns true if this exact content has already been written for this id.</summary>
    bool AlreadyProcessed(string externalId, string contentHash);

    /// <summary>Records that this content has been written for this id.</summary>
    void Record(string externalId, string contentHash);
}
