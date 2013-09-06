## JQSQL

JQSQL is a handy library which is designed to provide flexible and simple query features over JSON data in MS SQL Server.

With JQSQL, you can do the followings:

	Query JSON data and retrieve value(s) or portion of JSON data
	Sum values in JSON data
	Take average of values in JSON data
	Retrieve count of items in JSON data
	Find maximum/minimum of values in JSON data
	Transform JSON data to table

## First look

Below, you see a sample JSON which is stored in a database table.

	
	[
	  {
	    "Participants": [4,6],
	    "Title": "Happy birthday",
	    "Messages": [
	      {
	        "Sender": 4,
	        "SendDate": "2013-05-16T22:00:00+03:00",
	        "Content": "Wish you happy birthday :)",
	        "Attachments": [
	          {
	            "Title": "Cake",
	            "ItemLink": "url here",
	            "ItemSize": 90.0
	          }
	        ]
	      },
	      {
	        "Sender": 6,
	        "SendDate": "2013-05-17T00:00:00+03:00",
	        "Content": "Oh what a nice surprise for me! Thank you dear"
	      }
	    ]
	  }
	]

And, in the following image, you see samples illustrating how JQSQL works with the table containing the sample JSON.
	
![JQSQL count sample](https://github.com/ercananlama/JQSQL/raw/master/doc/CountSampleScreen.png)

## Technical requirements and specification

JQSQL is built to work with MS SQL Server with version 2005 and higher. It uses .net framework 3.5.
It uses JSON query framework called JQDotNet. In order to use query expressions properly and learn more about JSON query, 
please visit http://jqdotnet.com.

There is a database backup named SampleDb which was created on MS SQL Server 2008 R2 with the 
intention of demonstration of JSON query in MS SQL Server environment.

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

## Folder structure

	built: The output of the project which includes files used to install and run JQSQL in MS SQL Server
	lib: Contains 3rd party libraries
	samples: Sample files (sql scripts) are designed to run over sample database (located in the same directory)
	source: Contains project source files	
	
For the documentation, news and samples, please visit the [official site](http://jqsql.com)
