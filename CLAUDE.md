# CLAUDE.md — guide for coding agents

This is the central guide for coding agents (and humans) working in this repository. It is a
**.NET 10 clean/hexagonal starter**. Replace `Contoso` (org) and `App` (product) with your own
before you build on it.

`AGENTS.md` points non-Claude agents here.

## Architecture

**Clean / hexagonal architecture (ports and adapters).** Dependencies point **inwards only**:
`Ui → Application → Domain`, `Infrastructure.* → Application/Domain`. `App.Domain` has **zero
package references** — that is load-bearing, not incidental, and is enforced by
`tests/App.Architecture.Tests` (the build fails on a forbidden project or package reference).

Decisions and their reasoning live in [`docs/adr/`](docs/adr/README.md). **Read the relevant ADR
before changing anything it governs.** If a change would violate an ADR, say so and propose
superseding it — do not work around it silently.

## Build / Run / Test

Requires the **.NET 10 SDK** (pinned in `global.json`). Run from the repository root.

```bash
dotnet build App.slnx -c Release        # build everything (green build == style gate)
dotnet test  App.slnx -c Release        # unit tests + Gherkin specs + architecture tests
dotnet run --project src/App.Ui.Avalonia
```

There is no separate lint step; analyzers and code style run as part of the build
(`TreatWarningsAsErrors` + `EnforceCodeStyleInBuild`). Before pushing, `dotnet format App.slnx`
so the format gate stays green.

## Language convention

**English for all code, documentation, commit messages, branch names and Gherkin scenarios.** If
your domain vocabulary is in another language, keep those terms **verbatim as domain nouns** and
do not translate them — but keep the surrounding prose and all identifiers English.

## Repo structure

The repository is **flat**: the solution, its configuration, `src/`, `tests/` and `docs/` all sit
at the root. Assembly and namespace names carry the company prefix (project `App.Domain` →
`Contoso.App.Domain`), set centrally in `Directory.Build.props`. See the README for the full tree.

- `src/App.Domain` — the core; **BCL only, no packages**.
- `src/App.Application` — use cases and ports (interfaces).
- `src/App.Infrastructure.*` — adapters that implement the ports.
- `src/App.Ui.Avalonia` — desktop UI and the **composition root** (the one place that knows the
  concrete adapters).
- `tests/App.Architecture.Tests` — guards the Dependency Rule and the shared build settings.

## Conventions

- **Never commit secrets or sensitive/customer data.** No database snapshots, connection strings,
  export packages, or ledger databases — `.gitignore` blocks the common cases, but think before
  you add a file. A database snapshot is typically personal data under GDPR.
- **Do not commit without being asked.** Commit deliberately, on request.
- **Central Package Management is on**: put NuGet versions in `Directory.Packages.props`, never a
  `Version=` on a `<PackageReference>`.
- **Version authority is `<Version>` in `Directory.Build.props`.** Nerdbank.GitVersioning is a
  CLI-only labeling tool in CI; do not add its MSBuild package.

## Git workflow

This template assumes **GitHub Flow**: there is only `main`; `feature/*` and `hotfix/*` branches
are cut from `main` and merged back via pull request. **Direct commits to `main` are forbidden.**

Suggested branch naming (adjust to your tracker), machine-validatable:

```
feature/<WorkItemID>_<Short_English_Name>
hotfix/<BugOrIssueID>_<Short_English_Name>
```

Regex: `^(feature|hotfix)/[0-9]+_[A-Za-z0-9_]+$` (wire it into a `pre-push` hook if you use a
work-item tracker).

### Commits — Conventional Commits

```
<type>: <subject>
```

- `<type>` is one of `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`, `build`,
  `perf` — lowercase, English.
- Subject: imperative present tense, lowercase, ≤ 50 characters, no trailing period.
- Body (optional): *what/why*, not *how*; wrapped at ≤ 72 characters.
- If you use a work-item tracker, append its reference to the subject (e.g. Azure Boards
  `AB#<id>`, GitHub `#<id>`).

### Pull requests

- One PR per unit of work, into `main`, with a **Description** and **Testing** steps.
- At least one reviewer; `.github/CODEOWNERS` + branch protection enforce the reviewer pool (see
  [`docs/ci/branch-protection.md`](docs/ci/branch-protection.md)).
- CI must be green (build, tests, diff-coverage, security, format) before merge.
