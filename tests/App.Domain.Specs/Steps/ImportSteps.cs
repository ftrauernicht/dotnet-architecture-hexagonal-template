using Contoso.App.Application.Ports;
using Contoso.App.Application.UseCases;
using Contoso.App.Infrastructure.Inbound;
using Contoso.App.Infrastructure.Outbound;
using Contoso.App.Infrastructure.Persistence;

using Reqnroll;

namespace Contoso.App.Domain.Specs.Steps;

[Binding]
public sealed class ImportSteps
{
    private readonly InMemoryItemTarget _target = new();
    private readonly InMemoryLedger _ledger = new();
    private IItemSource _source = new SampleItemSource();
    private int _written;
    private int _writtenOnSecondRun;

    [Given("the sample source")]
    public void GivenTheSampleSource() => _source = new SampleItemSource();

    [When("the import runs")]
    public void WhenTheImportRuns() =>
        _written = new ImportItemsUseCase(_source, _target, _ledger).Execute();

    [When("the import runs again")]
    public void WhenTheImportRunsAgain() =>
        _writtenOnSecondRun = new ImportItemsUseCase(_source, _target, _ledger).Execute();

    [Then("{int} items are written")]
    public void ThenItemsAreWritten(int expected) => Assert.Equal(expected, _written);

    [Then("{int} items are written on the second run")]
    public void ThenItemsAreWrittenOnTheSecondRun(int expected) => Assert.Equal(expected, _writtenOnSecondRun);
}
