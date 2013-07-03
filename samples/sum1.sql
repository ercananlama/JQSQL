SELECT 
	UserId, 
	FirstName, 
	LastName, 
	BirthDate, 
	Gender, 
	-- Returns the total sizes of attachments in conversations
	jqsql.getsum(Conversations, 'Messages.Attachments.ItemSize') as TotalSizeOfAttachments
FROM dbo.Users




