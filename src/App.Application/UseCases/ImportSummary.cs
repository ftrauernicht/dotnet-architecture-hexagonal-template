namespace Contoso.App.Application.UseCases;

/// <summary>
/// What one import run did. It lives in the application layer and not in the domain on purpose:
/// it describes a run of a use case, not anything a business expert would recognise. The
/// Dependency Rule would have allowed it in either place, which is the point worth noticing —
/// the rule says where a type may not go, never where it belongs.
/// </summary>
/// <param name="Written">Items written to the target on this run.</param>
/// <param name="SkippedAsUnchanged">Items the ledger had already recorded with the same content.</param>
public readonly record struct ImportSummary(int Written, int SkippedAsUnchanged)
{
    /// <summary>Items the source offered, which is everything written plus everything skipped.</summary>
    public int Read => Written + SkippedAsUnchanged;
}
