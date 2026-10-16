using Contoso.App.Application.Ports;

namespace Contoso.App.Application.UseCases;

/// <summary>
/// Reads every item from the source and writes those not yet recorded in the ledger. Running it
/// again is a no-op, which is what makes a re-runnable, idempotent migration possible.
/// </summary>
public sealed class ImportItemsUseCase(IItemSource source, IItemTarget target, IMigrationLedger ledger)
{
    /// <summary>Runs the import and reports what it wrote and what it left alone.</summary>
    public ImportSummary Execute()
    {
        var written = 0;
        var skipped = 0;

        foreach (var item in source.Read())
        {
            var contentHash = $"{item.ExternalId}:{item.Name}";
            if (ledger.AlreadyProcessed(item.ExternalId, contentHash))
            {
                skipped++;
                continue;
            }

            target.Write(item);
            ledger.Record(item.ExternalId, contentHash);
            written++;
        }

        return new ImportSummary(written, skipped);
    }
}
