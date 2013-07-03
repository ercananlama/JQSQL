## JQSQL

JQSQL is a handy library which is designed to provide flexible and simple query features over JSON data in MS SQL Server.

With JQSQL, you can do the followings:

	Query JSON data and retrieve value(s) or portion of JSON data
	Sum values in JSON data
	Take average of values in JSON data
	Retrieve count of items in JSON data
	Find maximum/minimum of values in JSON data
	Tansform JSON data to table

## Installation

To be able to use JQSQL with MS SQL Server, you need to install it first. 

For this step, you have 2 alternatives

1) 
	You can do it manually. You find install.sql under built folder in root of the project. 
	You need to change this sql as instructed in the sql file. 
	This is required for more advanced scenarios.

2) 
	In built folder, you find a small application named JQSQL.Management.exe. 
	With this tool, you can install JQSQL in your MS SQL Server databases.
	This is the recommended way in most cases.

## Technical requirements and specification

JQSQL is built to work with MS SQL Server with version 2005 and higher. It uses .net framework 3.5. 
SampleDb is a database backup which was created on MS SQL Server 2008 R2.

## Folder structure

	built: The output of the project which includes files used to install and run JQSQL in MS SQL Server
	doc: Includes documentation and help
	lib: Contains 3rd party libraries
	samples: Sample files (sql scripts) are designed to run over sample database (located in the same directory)
	source: Contains project source files
