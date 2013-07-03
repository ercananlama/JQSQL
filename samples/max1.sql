SELECT 
	UserId, 
	FirstName, 
	LastName, 
	BirthDate, 
	Gender, 
	-- Returns the biggest send date in messages
	jqsql.getmax(Conversations, 'Messages.SendDate') as LastMsgSendDate
FROM dbo.Users







