# ADR-0001: Record architecture decisions

- **Status**: Accepted
- **Date**: 2026-07-29

## Context

Load-bearing decisions get made once and then lived with for a long time. When the reasoning is
not written down, it is reconstructed — badly — from the code months later, and changes get made
that quietly undo a decision nobody remembered was deliberate.

## Decision

We will record every architecturally significant decision as an **Architecture Decision Record**
in `docs/adr/`, using the format in [`_template.md`](_template.md): Context, Decision,
Consequences. ADRs are append-only and immutable; a decision is changed by writing a new ADR that
supersedes the old one.

## Consequences

- The *why* behind the structure is on the record and survives team turnover.
- There is a small, deliberate cost to changing a governed decision: you must supersede an ADR
  rather than silently work around it. That friction is the point.
- Where practical, an ADR is backed by a test in `tests/App.Architecture.Tests`, so the build —
  not a reviewer's memory — catches drift.
