SELECT 
	UserId, 
	FirstName, 
	LastName, 
	BirthDate, 
	Gender, 
	-- Returns the total number of attachments in conversations
	jqsql.getcount(Conversations, 'Messages.Attachments') as NrOfAttachmets
FROM dbo.Users




