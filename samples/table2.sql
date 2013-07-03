DECLARE @JsonData NVARCHAR(MAX)

SELECT @JsonData = Conversations
FROM dbo.Users
WHERE UserId = 2

-- Returns attachments in messages as table
EXEC [jqsql].[totable] @JsonData, 'Messages.Attachments'


