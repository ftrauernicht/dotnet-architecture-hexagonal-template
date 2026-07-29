# Branch protection for `main`

The CI gates only *protect* `main` once branch protection actually requires them. Configure the
following on the `main` branch (GitHub / GHE → Settings → Branches → Add rule).

## Required status checks

Require these checks to pass before merging (names come from the `name:` of each job):

- `Build (Release)` — from `build.yml`
- `Tests + Report` — from `test.yml`
- `Diff Coverage` — from `test.yml` (PR-only; see note)
- `Secret scan` — from `security.yml`
- `Vulnerability scan` — from `security.yml`
- `dotnet format` — from `format.yml`

> **Note on `Diff Coverage`.** It runs only on pull requests. If your provider will not let a
> PR-only check be "required" (some treat a never-run check on direct pushes as pending), mark it
> required but keep in mind it is meaningful on PRs only — which is the only place changes should
> land anyway.

`Publish single-exe` is intentionally **not** a required gate — it produces a testing artifact,
it does not judge correctness.

## Other settings

- **Require a pull request before merging**, with **at least 1 approving review**.
- **Require review from Code Owners** (activates `.github/CODEOWNERS`).
- **Require branches to be up to date before merging** (so checks ran against the merge result).
- **Do not allow bypassing the above** (optionally exempt no one).
- **Require linear history** if you merge via squash/rebase; otherwise allow merge commits per
  your Git workflow.

## On GitHub Enterprise

The runners and outbound network access these workflows need may have to be allowlisted first —
see [`ghe-allowlist.md`](ghe-allowlist.md). Turn the checks *required* only once a first run has
gone green, or PRs will be blocked by checks that can never start.
