using Contoso.App.Application.UseCases;
using Contoso.App.Infrastructure.Inbound;
using Contoso.App.Infrastructure.Outbound;
using Contoso.App.Infrastructure.Persistence;

namespace Contoso.App.Domain.Tests;

public sealed class ImportItemsUseCaseTests
{
    [Fact]
    public void WritesEachSampleItemOnce()
    {
        var useCase = new ImportItemsUseCase(new SampleItemSource(), new InMemoryItemTarget(), new InMemoryLedger());

        Assert.Equal(new ImportSummary(Written: 2, SkippedAsUnchanged: 0), useCase.Execute());
    }

    [Fact]
    public void IsIdempotentOnASecondRun()
    {
        var ledger = new InMemoryLedger();
        var useCase = new ImportItemsUseCase(new SampleItemSource(), new InMemoryItemTarget(), ledger);

        useCase.Execute();

        var second = useCase.Execute();

        Assert.Equal(0, second.Written);
        Assert.Equal(2, second.SkippedAsUnchanged);
        Assert.Equal(2, second.Read);
    }
}
