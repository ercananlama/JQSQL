PRINT 'Enable clr'

EXEC sp_configure 'clr enabled', 1;
GO
RECONFIGURE;
GO

PRINT 'Generate assembly'

GO
-- Modify the following line as replacing JQSQLOutputFolder with the absolute path of JQSQL built folder in your drive
CREATE ASSEMBLY [jqsql] FROM 'JQSQLOutputFolder\JQSQL.dll'
GO

PRINT 'Create schema'

GO
CREATE SCHEMA [jqsql]
GO

PRINT 'Create value function'

GO
CREATE FUNCTION [jqsql].[getvalue](@jsonData NVARCHAR(MAX), @expression NVARCHAR(500)) RETURNS NVARCHAR(4000)
AS EXTERNAL NAME JQSQL.Functions.Value; 
GO

PRINT 'Create sum function'

GO
CREATE FUNCTION [jqsql].[getsum](@jsonData NVARCHAR(MAX), @expression NVARCHAR(500)) RETURNS FLOAT
AS EXTERNAL NAME JQSQL.Functions.Sum; 
GO

PRINT 'Create count function'

GO
CREATE FUNCTION [jqsql].[getcount](@jsonData NVARCHAR(MAX), @expression NVARCHAR(500)) RETURNS INT
AS EXTERNAL NAME JQSQL.Functions.Count; 
GO

PRINT 'Create max function'

GO
CREATE FUNCTION [jqsql].[getmax](@jsonData NVARCHAR(MAX), @expression NVARCHAR(500)) RETURNS NVARCHAR(20)
AS EXTERNAL NAME JQSQL.Functions.Max; 
GO

PRINT 'Create min function'

GO
CREATE FUNCTION [jqsql].[getmin](@jsonData NVARCHAR(MAX), @expression NVARCHAR(500)) RETURNS NVARCHAR(20)
AS EXTERNAL NAME JQSQL.Functions.Min; 
GO

PRINT 'Create average function'

GO
CREATE FUNCTION [jqsql].[getavg](@jsonData NVARCHAR(MAX), @expression NVARCHAR(500)) RETURNS FLOAT
AS EXTERNAL NAME JQSQL.Functions.Average; 
GO

PRINT 'Create table procedure'

GO
CREATE PROCEDURE [jqsql].[totable] (
	@jsonData NVARCHAR(MAX), 
	@expression NVARCHAR(500)
)
AS EXTERNAL NAME JQSQL.Functions.ToTable;

