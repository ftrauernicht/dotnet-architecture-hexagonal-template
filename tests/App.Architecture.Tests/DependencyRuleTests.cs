using Contoso.App.Application.UseCases;
using Contoso.App.Domain;

namespace Contoso.App.Architecture.Tests;

/// <summary>
/// Enforces the Dependency Rule at build time: inner layers must not reference outer ones. These
/// tests read each assembly's referenced-assembly list and fail if a forbidden edge appears.
/// </summary>
public sealed class DependencyRuleTests
{
    private static readonly string[] ForbiddenForDomain =
    [
        "Contoso.App.Application",
        "Contoso.App.Infrastructure",
        "Avalonia",
    ];

    [Fact]
    public void DomainReferencesNoInnerOrOuterProject()
    {
        var referenced = typeof(Item).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name ?? string.Empty)
            .ToArray();

        Assert.DoesNotContain(
            referenced,
            name => ForbiddenForDomain.Any(forbidden => name.StartsWith(forbidden, StringComparison.Ordinal)));
    }

    [Fact]
    public void ApplicationDoesNotReferenceInfrastructureOrUi()
    {
        var referenced = typeof(ImportItemsUseCase).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name ?? string.Empty)
            .ToArray();

        Assert.DoesNotContain(
            referenced,
            name => name.StartsWith("Contoso.App.Infrastructure", StringComparison.Ordinal)
                || name.StartsWith("Avalonia", StringComparison.Ordinal));
    }
}
