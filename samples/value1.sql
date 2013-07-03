SELECT 
	UserId, 
	FirstName, 
	LastName, 
	BirthDate, 
	Gender, 
	-- Returns the titles of conversations
	jqsql.getvalue(Conversations, 'Title') as Titles
FROM dbo.Users