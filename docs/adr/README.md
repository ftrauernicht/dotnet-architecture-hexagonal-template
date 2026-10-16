# Architecture Decision Records (ADR)

Every load-bearing decision in this repo is recorded here as a short, numbered, immutable file.
An ADR captures the **context**, the **decision**, and its **consequences** — so that six months
later the *why* is still on record, not just the *what*.

## Rules

- **Read the relevant ADR before changing anything it governs.** If a change would violate an ADR,
  say so and propose superseding it — do not work around it silently.
- ADRs are **append-only**. You do not edit a decision; you write a new ADR that **supersedes** it
  and mark the old one `Superseded by ADR-NNNN`.
- Numbering is sequential (`0001`, `0002`, …). Copy [`_template.md`](_template.md) to start one.
- Several ADRs are **enforced by tests** in `tests/App.Architecture.Tests` — the build fails if
  the code drifts from the decision.

## Index

| ADR | Decision | Status |
|---|---|---|
| [0001](0001-record-architecture-decisions.md) | Record architecture decisions | Accepted |
| [0002](0002-enforce-the-dependency-rule-with-tests.md) | Enforce the Dependency Rule with tests, not convention | Accepted |
| [0003](0003-pin-what-the-build-depends-on.md) | Pin what the build depends on | Accepted |
| [0004](0004-pin-actions-to-commit-shas.md) | Pin GitHub Actions to commit SHAs | Accepted |

## Suggested first decisions to record for a new project

The template records the decisions it makes about itself (0001 to 0004). When you adopt it,
record your own versions of the decisions it leaves to you, so they are on the record for your
team:

- Target framework and solution format (`.slnx`).
- UI stack (Avalonia + MVVM) — or your replacement.
- Central Package Management and the pinned SDK / NuGet sources.
- Versioning authority (`Directory.Build.props` `<Version>`; Nerdbank.GitVersioning CLI-only).
- Idempotency strategy (ExternalId + content hash + ledger), if your domain is a migration/import.
