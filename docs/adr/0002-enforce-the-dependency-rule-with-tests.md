# ADR-0002: Enforce the Dependency Rule with tests, not convention

- **Status**: Accepted
- **Date**: 2026-10-16

## Context

Hexagonal architecture lives or dies on one rule: dependencies point inwards. The domain knows
nothing of the application layer, and neither knows anything of infrastructure or UI. Folder
names and a diagram in a wiki do not enforce that. A single `ProjectReference` added under
deadline pressure is enough to invert an edge, and it will be reviewed by someone who is reading
the feature, not the dependency graph.

The failure is quiet. Nothing breaks on the day the edge is added. What breaks is six months
later, when the domain can no longer be tested without a database, and nobody can say when that
happened.

## Decision

We will assert the Dependency Rule in `tests/App.Architecture.Tests/DependencyRuleTests.cs` and
let the build fail on a violation:

- `DomainReferencesNoInnerOrOuterProject` asserts the domain assembly references neither the
  application layer, nor any infrastructure assembly, nor Avalonia.
- `ApplicationDoesNotReferenceInfrastructureOrUi` asserts the same for the application layer.

Both read the compiled assembly's referenced-assembly list rather than the project file.

## Consequences

- A forbidden edge fails a test run, including the one on a pull request. The reviewer does not
  have to notice it.
- Adding a layer or renaming an assembly means updating the forbidden-prefix lists in that file.
  That is the maintenance cost, and it is small and obvious.
- **A known limitation worth stating:** the tests read what the assembly actually references, and
  the C# compiler omits a project reference nothing is used from. A `ProjectReference` that is
  declared but never touched will therefore pass. The test catches a real dependency, not a
  careless line in a `.csproj`. In practice the careless line arrives together with the use of
  it, so this has not cost anything yet — but it is the gap, and anyone relying on these tests
  should know where it is.
- The rule is now a decision somebody has to supersede rather than something a merge can erode.
