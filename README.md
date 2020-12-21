![GitHub Workflow Status](https://img.shields.io/github/workflow/status/directfront/DbCtl.Connectors/DbCtl%20Connectors%20-%20CI?style=plastic)
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
	Hash VARCHAR(64) NOT NULL CONSTRAINT UQ_DbCtlChangeLog_Hash UNIQUE,
    AppliedBy VARCHAR(50) NOT NULL,
	ChangeDateTime DATETIME NOT NULL,
    CONSTRAINT PK_DbCtlChangeLog PRIMARY KEY (ChangeDateTime DESC, Version DESC)
)
```

# Credits

Icons made by [srip](https://www.flaticon.com/authors/srip) from [Flaticon](www.flaticon.com)

