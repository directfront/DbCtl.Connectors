![Nuget](https://img.shields.io/nuget/dt/DbCtl.Connectors?style=plastic)

# DbCtl.Connectors
Core interfaces necessary for implementing connectors.

## Connector Implementation Guide

### Schema ChangeLog Table
Below is the `DbCtlChangeLog` table schema for SQL Server. The table name `DbCtlChangeLog` is not prescribed but is the suggested name. Each connector can decide on the table name, and must use it consistently with, at least, the columns in the schema below.

```sql
CREATE TABLE DbCtlChangeLog (
    MigrationType VARCHAR(15) NOT NULL,
    Version VARCHAR(10),
    Description VARCHAR(255),
    Filename VARCHAR(255) NOT NULL,
    Hash VARCHAR(64) NOT NULL,
    AppliedBy VARCHAR(50) NOT NULL,
    ChangeDateTime DATETIME NOT NULL,
    CONSTRAINT PK_DbCtlChangeLog PRIMARY KEY (ChangeDateTime DESC, Version DESC)
)
```
