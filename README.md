# DbCtl.Connectors
Core interfaces necessary for implementing connectors.

## Connector Implementation Guide

### Schema ChangeLog Table
Below is the `DbCtlChangeLog` table schema for SQL Server:

```
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
Icons made by srip[https://www.flaticon.com/authors/srip" title="srip">srip</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>
