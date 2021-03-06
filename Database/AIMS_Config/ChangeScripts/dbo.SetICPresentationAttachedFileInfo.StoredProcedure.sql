SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[SetICPresentationAttachedFileInfo]
	@UserName VARCHAR(50),
	@PresentationId BIGINT,
	@Name VARCHAR(255),
	@IssuerName VARCHAR(50), 
	@SecurityName VARCHAR(255),
	@SecurityTicker VARCHAR(50),
	@Location VARCHAR(255),
	@MetaTags VARCHAR(255),
	@Category VARCHAR(50),
	@Type  VARCHAR(50),
	@FileId BIGINT = 0,
	@DeletionFlag BIT = 'False'
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION	
					
		IF @FileId = 0
		BEGIN	
			INSERT INTO FileMaster ([Name], [IssuerName], [SecurityName], [SecurityTicker], [Location], [MetaTags], [Category], [Type]
			, [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
			VALUES (@Name, @IssuerName, @SecurityName, @SecurityTicker, @Location, @MetaTags, @Category, @Type
			, @UserName, GETUTCDATE(), @UserName, GETUTCDATE())
			
			SET @FileId = @@IDENTITY
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -1
				RETURN
			END	
		
			INSERT INTO dbo.PresentationAttachedFileInfo ( [FileID], [PresentationID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn] )
			VALUES ( @FileId, @PresentationId, @UserName, GETUTCDATE(), @UserName, GETUTCDATE() )
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -2
				RETURN
			END	
		END
		ELSE
		BEGIN
			IF @DeletionFlag = 'True'
			BEGIN			
				DELETE FROM FileMaster WHERE [FileID] = @FileId
				IF @@ERROR <> 0
				BEGIN
					ROLLBACK TRANSACTION
					SELECT -3
					RETURN
				END	
			END
			ELSE
			BEGIN
				UPDATE FileMaster SET
				[Name] = @Name, 
				[SecurityName] = @SecurityName,
				[SecurityTicker] = @SecurityTicker,
				[Location] = @Location,
				[MetaTags] = @MetaTags,
				[Category] = @Category,
				[Type] = @Type,
				[ModifiedBy] = @UserName,
				[ModifiedOn] = GETUTCDATE()
				WHERE [FileID] = @FileId
				
				IF @@ERROR <> 0
				BEGIN
					ROLLBACK TRANSACTION
					SELECT -4
					RETURN
				END	
			END
		END							
		
	COMMIT TRANSACTION	
	SELECT 0
    
END
GO
