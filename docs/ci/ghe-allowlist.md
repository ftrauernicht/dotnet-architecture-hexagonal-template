# GitHub Enterprise prerequisites (allowlist)

On github.com these workflows run out of the box. On a locked-down **GitHub Enterprise** instance
they usually need a few things allowlisted first. Hand this checklist to whoever administers the
instance and the runners.

## Runners

- [ ] A Linux runner labelled `ubuntu-latest` (build, test, security, format).
- [ ] A Windows runner labelled `windows-latest` (publish — the Avalonia app is `WinExe`).

## Actions allowlist

If the instance restricts Actions to an allowlist, add the pinned actions used here (SHA pins are
in the workflow files):

- [ ] `actions/checkout`
- [ ] `actions/setup-dotnet`
- [ ] `actions/setup-python`
- [ ] `actions/upload-artifact`
- [ ] `dorny/test-reporter`
- [ ] `aquasecurity/trivy-action`

## Outbound network access

The runners fetch tooling and data from the public internet. Allow (or mirror internally):

- [ ] **nuget.org** — package restore.
- [ ] **PyPI** — `diff-cover` install in the coverage job.
- [ ] **github.com releases** — the pinned `gitleaks` binary download (verified by SHA256).
- [ ] **Trivy vulnerability DB** (`ghcr.io` / `mirror.gcr.io` as configured by trivy-action).
- [ ] The **.NET SDK feed** used by `setup-dotnet` to install the pinned SDK.

## Secrets

- [ ] None required by default. (The gitleaks CLI is used directly to avoid needing a
      `GITLEAKS_LICENSE` org secret.)

## Then

- [ ] Trigger each workflow once via **Actions → Run workflow** (`workflow_dispatch`) or a first
      PR, confirm green, *then* mark the checks required in
      [`branch-protection.md`](branch-protection.md).

> The `workflow_dispatch` "Run workflow" button only appears once the workflow file exists on the
> **default branch** (`main`). Before then, exercise the workflows through a pull request.
