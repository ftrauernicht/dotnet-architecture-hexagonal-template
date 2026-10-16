# ADR-0004: Pin GitHub Actions to commit SHAs

- **Status**: Accepted
- **Date**: 2026-10-16

## Context

`uses: some/action@v4` is not a version. It is a tag, and a tag is a pointer the publisher can
move. Whoever controls that repository can change what runs inside our pipeline, retroactively,
without a commit on our side. The pipeline holds a token, reads the whole checkout and uploads
artefacts, so "what runs there" is not a detail.

The counter-argument is real: a SHA is unreadable, and a wall of forty-character hex strings is
worse documentation than a tag.

## Decision

We will pin every action to a full commit SHA and write the human-readable tag in a trailing
comment on the same line:

```yaml
uses: actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1  # v7.0.1
```

Dependabot's `github-actions` ecosystem moves the SHA and the comment together, so the pin stays
maintainable rather than rotting into a version from two years ago.

## Consequences

- What runs in the pipeline is fixed by this repository and cannot be changed from outside it.
- The comment carries the readability the SHA costs. A comment that disagrees with its SHA is
  worse than no comment, so it is Dependabot that updates both, not a person editing by hand.
- SHAs are never guessed or copied from memory. They come from the API or from the Dependabot
  pull request that proposes them.
- Patch and minor bumps arrive grouped and merge themselves once the checks pass, which keeps the
  pins current without a weekly chore. A major version belongs to no group and waits for a
  person, because a major in an action is usually the runtime underneath it changing.
- On GitHub Enterprise an action also has to be allowlisted; see
  [`../ci/ghe-allowlist.md`](../ci/ghe-allowlist.md).
