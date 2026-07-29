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

        Assert.Equal(2, useCase.Execute());
    }

    [Fact]
    public void IsIdempotentOnASecondRun()
    {
        var ledger = new InMemoryLedger();
        var useCase = new ImportItemsUseCase(new SampleItemSource(), new InMemoryItemTarget(), ledger);

        useCase.Execute();

        Assert.Equal(0, useCase.Execute());
    }
}
