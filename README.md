# .NET 10 Clean/Hexagonal Starter Template

An opinionated, **ready-to-clone** starter for a .NET 10 application with a clean/hexagonal
architecture, strict quality governance, an executable specification, and a full GitHub Actions
CI/CD pipeline. It builds, tests and publishes out of the box.

Everything here is deliberately generic. Replace the two placeholders and start building:

- `Contoso` → your organization (namespace / assembly prefix, in `Directory.Build.props`)
- `App` → your product (project names, solution file, UI title)

> This template distills a real, shipping setup into a reusable skeleton. The sample domain
> (`Item` + an idempotent import use case) exists only to make the wiring and the tests concrete —
> delete it and drop in your own domain.

---

## At a glance

| Layer | Choice |
|---|---|
| **Language / runtime** | C# on **.NET 10** (`net10.0`), `LangVersion=latest`, Nullable + ImplicitUsings |
| **Solution format** | `.slnx` (XML solution) |
| **Repo layout** | **flat** — solution, configs, `src/`, `tests/`, `docs/` all at the root |
| **Architecture** | Clean / hexagonal (ports & adapters); the Dependency Rule is **enforced by tests** |
| **Packages** | **Central Package Management** — every version in `Directory.Packages.props` |
| **SDK pin** | `global.json` (`rollForward: latestPatch`, `allowPrerelease: false`) |
| **Source pin** | `nuget.config` with `<clear/>` + `packageSourceMapping` (dependency-confusion safe) |
| **Build governance** | `Directory.Build.props`: `TreatWarningsAsErrors` + `EnforceCodeStyleInBuild` + analyzers |
| **UI** | Avalonia + MVVM (CommunityToolkit.Mvvm), DI via `Microsoft.Extensions.DependencyInjection` |
| **Tests** | xUnit **v3** (Microsoft.Testing.Platform), **Reqnroll** (Gherkin), architecture tests |
| **Versioning** | `Directory.Build.props` `<Version>` is authoritative; Nerdbank.GitVersioning **CLI-only** |
| **CI/CD** | GitHub Actions: build · test + diff-coverage · security · format · publish |
| **Supply chain** | Actions **SHA-pinned**, Dependabot, gitleaks (checksum-verified), Trivy (lock-file) |
| **Publish** | framework-dependent single-file EXE (runtime **not** embedded, ~30 MB) |

---

## Prerequisites

- **.NET 10 SDK** (the exact version is pinned in `global.json`; install it from
  <https://dot.net> or via `dotnet-install`).
- Run every command **from the repository root** — that is where `global.json`, `nuget.config`
  and the `Directory.*.props` files live.

## Build · Test · Run

```bash
dotnet restore App.slnx
dotnet build   App.slnx -c Release          # green build == style gate (warnings are errors)
dotnet test    App.slnx -c Release          # unit + architecture + Reqnroll specs
dotnet run --project src/App.Ui.Avalonia    # start the desktop app
dotnet format  App.slnx --verify-no-changes --severity warn   # what the format gate checks
```

There is no separate lint step — analyzers and code style run as part of the build.

---

## Structure

The repository is **flat**: no container folder around the code. Dependencies point **inwards**
only (`Ui → Application → Domain`, `Infrastructure.* → Application/Domain`), and the domain core
has **zero package references** — enforced, not just intended.

```
App.slnx                     XML solution
global.json                  SDK pin
nuget.config                 package-source pin (<clear/> + mapping)
Directory.Build.props        shared MSBuild settings, namespace convention, <Version>
Directory.Packages.props     ALL NuGet versions (Central Package Management)
version.json                 Nerdbank.GitVersioning
.editorconfig                code style + naming — enforced by the build
src/
  App.Domain                 BCL only, NO packages — the core
  App.Application            use cases + ports (IItemSource, IItemTarget, IMigrationLedger)
  App.Infrastructure.Inbound   inbound adapter (reads the source)
  App.Infrastructure.Outbound  outbound adapter (writes the target)
  App.Infrastructure.Persistence  the ledger (idempotency state)
  App.Ui.Avalonia            desktop UI (Avalonia + MVVM) and the composition root
tests/
  App.Domain.Tests           unit tests
  App.Domain.Specs           executable Gherkin specification (Reqnroll)
  App.Architecture.Tests     guards the Dependency Rule + the build settings
docs/
  adr/                       Architecture Decision Records — read these first
  ci/                        branch protection + (optional) GHE allowlist
.github/                     workflows, dependabot, CODEOWNERS
```

Assembly and namespace names carry the company prefix: project `App.Domain` produces
`Contoso.App.Domain`, set centrally in `Directory.Build.props`.

---

## What makes this template opinionated

### Architecture enforced by tests, not convention
`App.Architecture.Tests` fails the build if the domain gains an outward dependency, if the
application references infrastructure/UI, or if a shared quality gate (warnings-as-errors,
central package management, cleared NuGet sources, pinned SDK) is removed. Governance cannot rot
silently — removing a guardrail turns a test red.

### One version authority
`<Version>` in `Directory.Build.props` stamps every assembly. Nerdbank.GitVersioning is used in
CI **as a CLI only**, to label the run and the published artifact — the nbgv MSBuild package is
deliberately **not** added (it would set a per-project version and break the architecture test).

### A CI/CD pipeline that is a supply chain, not a script
Five small workflows, each least-privilege (`contents: read`):

| Workflow | Gate |
|---|---|
| `build.yml` | warning = error → green build |
| `test.yml` | all tests green · **diff coverage ≥ 80 % on changed lines** (PR-only) |
| `security.yml` | no committed secret (gitleaks) · no new CRITICAL/HIGH vuln (Trivy) |
| `format.yml` | formatted per `.editorconfig` |
| `publish.yml` | framework-dependent single-file EXE, attached to the PR for testing |

Cross-cutting: every action is **pinned to a commit SHA** (Dependabot bumps them), the coverage
gate scores **only the lines a PR changed** (not the whole codebase), gitleaks is downloaded and
**SHA256-verified** before running, and Trivy restores a **lock file** first so it actually sees
the .NET dependencies.

### Publish that stays small
`publish.yml` produces a **framework-dependent** single-file executable (~30 MB) — the .NET
runtime is not embedded (the target machine needs the matching .NET Desktop Runtime). Use
`-p:SelfContained=false`; the space-separated `--self-contained false` is mis-parsed and would
re-embed the whole runtime (~99 MB).

---

## Adopting the template for a new project

1. Copy the folder; initialize a fresh git repo (`git init`).
2. Find-and-replace `Contoso` → your org and `App` → your product (file names *and* contents),
   including the `.slnx`, the project folders/files, and namespaces.
3. Set real handles in `.github/CODEOWNERS`.
4. `dotnet restore && dotnet build && dotnet test` — confirm green.
5. Replace the sample domain (`Item`, the ports, the in-memory adapters, the feature file) with
   your own; keep the architecture tests pointed at your domain/application types.
6. Push to your host. If it is GitHub Enterprise, work through
   [`docs/ci/ghe-allowlist.md`](docs/ci/ghe-allowlist.md), then enable the required checks in
   [`docs/ci/branch-protection.md`](docs/ci/branch-protection.md).
7. Record your first real decisions as ADRs (see [`docs/adr/README.md`](docs/adr/README.md)).

---

## Gotchas baked in (so you don't rediscover them)

| Gotcha | Handled by |
|---|---|
| `--self-contained false` re-embeds the runtime | `publish.yml` uses `-p:SelfContained=false` |
| Native `.pdb` files bloat the artifact (~+100 MB) | `publish.yml` drops `*.pdb` after publish |
| Trivy silently scans nothing without a lock file | `security.yml` restores with `RestorePackagesWithLockFile=true` |
| `dotnet format --severity info` fails on clean code | `format.yml` uses `--severity warn` |
| Coverage over the whole codebase is meaningless | `test.yml` diff-scopes coverage to the PR |
| A moved action tag can change what runs | every action is pinned to a commit SHA |
| SDK pin drift | `setup-dotnet` uses `global-json-file` only, never a second hard-coded version |

---

## License

Add your own. This template ships without a license file on purpose — choose one before you
publish.
