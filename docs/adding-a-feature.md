# Adding a feature, and what the rules cost

An architecture is easy to admire in a diagram. What it is worth shows in the second week, when
somebody has to change something and the structure either helps or bills them for it. This is one
real change to this repository, written down with the bill attached.

The change: the import used to return the number of items it wrote. Now it reports what it wrote
**and** what it skipped because the ledger already had it, so a run that writes nothing can be
told apart from a run that found nothing.

## The six files, in the order you touch them

| # | File | What changes |
|---|---|---|
| 1 | `src/App.Application/UseCases/ImportSummary.cs` | New. A `readonly record struct` with `Written`, `SkippedAsUnchanged` and a derived `Read`. |
| 2 | `src/App.Application/UseCases/ImportItemsUseCase.cs` | Counts the skips and returns the summary instead of an `int`. |
| 3 | `src/App.Ui.Avalonia/MainWindowViewModel.cs` | Reports all three numbers instead of one. |
| 4 | `tests/App.Domain.Tests/ImportItemsUseCaseTests.cs` | Asserts the whole summary, and the second run now proves two items were skipped rather than merely that none were written. |
| 5 | `tests/App.Domain.Specs/Steps/ImportSteps.cs` | Holds two summaries instead of two ints, and gains one step. |
| 6 | `tests/App.Domain.Specs/Features/Import.feature` | One more `And` line on the idempotency scenario. |

Three of the six are tests. That ratio is not an accident of this example; it is what the setup is
for.

## The decision the rule did not make for you

`ImportSummary` could have gone in the domain. Nothing would have stopped it: the domain is the
innermost ring, and a type that sits there is visible to everyone.

It belongs in the application layer anyway, because it describes a run of a use case rather than
anything a business expert would recognise. Put it in the domain and the domain starts carrying
vocabulary about importing, which is the slow way a domain model turns into a report of whatever
the last project needed.

This is worth saying plainly: **the Dependency Rule tells you where a type may not go, never where
it belongs.** It rules out the inverted edge and leaves the judgement to you. An architecture that
claimed to make that call for you would be lying.

## What it cost, honestly

**The compiler found every call site**, because the return type changed. Six places, no searching,
nothing missed. A feature that only added a field to a record would have cost less and taught less.

**The Reqnroll step had to grow.** A new `Then` phrase means a step definition and a line in the
feature file, in two different places, and they have to agree on the wording. That is the price of
executable specification, paid every time, and it is the reason the scenario reads the way a
person would say it.

**The architecture tests did nothing**, and that is the correct outcome. No ring was crossed, so
nothing fired. They are a smoke detector, not a guide. The day they do fire is the day someone
reaches from the domain into infrastructure, and on that day they will cost an afternoon and save
a year.

**The SDK pin got in the way, exactly as designed.** `global.json` asks for 10.0.110 with
`rollForward: latestPatch`. On a machine carrying 10.0.402 the build refuses to start rather than
quietly compiling against a different toolchain. That is the behaviour [ADR-0003](adr/0003-pin-what-the-build-depends-on.md)
buys, and it is irritating in precisely the way it is supposed to be.

## What this says about the template

Six files for one counter is not a small number, and pretending otherwise would be dishonest. What
you get for it is that each of the six had an obvious home, the compiler found the rest, and the
test suite carried the change rather than being rewritten around it.

Compare the alternative honestly before copying this layout. A single-assembly application with a
service class would have done the same change in two files. If your system is small, stays small,
and is operated by the people who wrote it, that is the better trade and you should take it. The
structure here pays off when the system outlives the team that built it, which is the case this
template was extracted from.
