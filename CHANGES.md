# Changelog

## 1.1.0

1. Added initial `IDbConnector` interface with corresponding metadata interface for MEF exports. 
2. Added `ChangeLogEntry` with parsing functionality from filename to schema change log record.

## 2.0.0
1. Renamed `User` attribute to `AppliedBy`.
2. Added documentation to each type and attribute.
3. Changed `MigrationType` attribute to `char` instead of `string`.
4. Removed all public `Parse` methods and replaced them with constructors.
5. Improved unit tests.

## 2.0.1
Adds a parameterless internal constructor for ORMs to materialize the change log entry.
