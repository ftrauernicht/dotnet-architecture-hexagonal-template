# ADR-0003: Pin what the build depends on

- **Status**: Accepted
- **Date**: 2026-10-16

## Context

A build that produces different output on two machines is not a build, it is a negotiation. The
usual causes are well known and all of them are quiet: a package version resolved differently per
project, a NuGet source inherited from a machine-level config, an SDK one minor version ahead on
the runner, a warning that is a warning here and an error there.

Each of these is cheap to prevent on day one and expensive to diagnose on the day it matters,
which is usually a release day with someone waiting.

## Decision

We will pin the inputs of the build and assert in
`tests/App.Architecture.Tests/BuildSettingsTests.cs` that the pins are still there:

- **Package versions centrally**, through `Directory.Packages.props` with
  `ManagePackageVersionsCentrally`. A project names a package, never a version.
- **NuGet sources cleared** in `nuget.config` with `<clear />`, so what a machine happens to have
  configured cannot take part.
- **The SDK pinned** in `global.json`, with an explicit `rollForward` so the tolerance is a
  decision rather than a default.
- **Warnings are errors** and code style is enforced in the build, through
  `Directory.Build.props`.

The tests walk up from the test binary to the repository root and read these files, so removing a
gate fails the build that removed it.

## Consequences

- Two machines and the runner build the same thing, and an SDK the project has not been tried on
  fails immediately rather than subtly.
- Adding a package is two edits, not one: the reference and the central version. This is the
  cost, paid every time, and it is the reason the versions never drift apart.
- A warning stops the build. On a template whose job is to be copied, a tolerated warning
  propagates into every project started from it, which is why the setting is not negotiable here
  even though it is an irritation on a bad afternoon.
- Raising the SDK is now a visible commit to `global.json` instead of something that happened to
  a developer's machine.
