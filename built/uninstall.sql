IF (OBJECT_ID('[jqsql].[getvalue]') IS NOT NULL)
BEGIN
	DROP FUNCTION [jqsql].[getvalue]
END

IF (OBJECT_ID('[jqsql].[getsum]') IS NOT NULL)
BEGIN
	DROP FUNCTION [jqsql].[getsum]
END

IF (OBJECT_ID('[jqsql].[getcount]') IS NOT NULL)
BEGIN
	DROP FUNCTION [jqsql].[getcount]
END

IF (OBJECT_ID('[jqsql].[getmax]') IS NOT NULL)
BEGIN
	DROP FUNCTION [jqsql].[getmax]
END

IF (OBJECT_ID('[jqsql].[getmin]') IS NOT NULL)
BEGIN
	DROP FUNCTION [jqsql].[getmin]
END

IF (OBJECT_ID('[jqsql].[getavg]') IS NOT NULL)
BEGIN
	DROP FUNCTION [jqsql].[getavg]
END

IF (OBJECT_ID('[jqsql].[totable]') IS NOT NULL)
BEGIN
	DROP PROCEDURE [jqsql].[totable]
END

IF (ASSEMBLYPROPERTY('jqsql', 'SimpleName') IS NOT NULL)
BEGIN
	DROP ASSEMBLY [jqsql]
END

IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'jqsql')
BEGIN
	DROP SCHEMA [jqsql]
END