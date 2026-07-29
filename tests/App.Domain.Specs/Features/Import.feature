Feature: Import items

  The import reads every item from the source and writes it once. Running it again writes
  nothing, because the ledger has already recorded each item — the migration is idempotent.

  Scenario: Importing the sample source writes every item once
    Given the sample source
    When the import runs
    Then 2 items are written

  Scenario: A second run writes nothing
    Given the sample source
    When the import runs
    And the import runs again
    Then 0 items are written on the second run
