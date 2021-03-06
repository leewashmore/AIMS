SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[UpdateDocumentsData]
	@fileId BIGINT,
	@userName VARCHAR(50),
	@metaTags VARCHAR(255),
	@companyName VARCHAR(50),
	@categoryType VARCHAR(50),
	@comment VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;
	SET FMTONLY OFF;

	UPDATE FileMaster 
	SET
	SecurityName = NULL,
	SecurityTicker = NULL,
	IssuerName = @companyName,
	MetaTags = @metaTags,
	Type = @categoryType,
	ModifiedBy = @userName,
	ModifiedOn = GETUTCDATE()
	WHERE [FileID] = @fileId
	
	IF @comment IS NOT NULL
	BEGIN
		INSERT INTO CommentInfo ( [Comment], [CommentBy], [CommentOn], [FileID] )
		VALUES ( @comment, @userName, GETUTCDATE(), @fileId )
	END
    
END
GO
