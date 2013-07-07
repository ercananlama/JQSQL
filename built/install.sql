-- Description: 
		-- This sql file installs JQSQL in your database. Before executing this file, you need to modify it by the following instructions
-- Instructions:
		-- Replace JQSQLOutputFolder with the absolute path of JQSQL built folder in your drive
		-- Replace DbName with the database name in which you install JQSQL
		-- Validate the location of System.Core.dll if it points correct location in your drive  

PRINT 'Enable clr'

EXEC sp_configure 'clr enabled', 1;
GO
RECONFIGURE;
GO

PRINT 'Generate assembly'

GO
-- SQL 2005
IF (CHARINDEX('9.00', CAST(SERVERPROPERTY('productversion') AS NVARCHAR(20))) = 1)
BEGIN

	ALTER DATABASE [DbName] SET TRUSTWORTHY ON
	
	-- Check if system.core is already installed
	IF (ASSEMBLYPROPERTY('System.Core', 'SimpleName') IS NULL)
	BEGIN
		CREATE ASSEMBLY [System.Core]
		AUTHORIZATION [dbo]
		FROM 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.5\System.Core.dll'
		WITH PERMISSION_SET = UNSAFE
	END
	
	CREATE ASSEMBLY [jqsql] FROM 'JQSQLOutputFolder\JQSQL.dll'	
	WITH PERMISSION_SET = UNSAFE
END
ELSE
BEGIN -- SQL 2008 and higher

	CREATE ASSEMBLY [jqsql] FROM 'JQSQLOutputFolder\JQSQL.dll'

END
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

