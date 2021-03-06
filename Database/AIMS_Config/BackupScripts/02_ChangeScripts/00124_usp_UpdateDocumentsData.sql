set noexec off

--declare  current and required version
declare @RequiredDBVersion as nvarchar(100) = '00123'
declare @CurrentScriptVersion as nvarchar(100) = '00124'
--if current version already in DB, just skip
if exists(select 1 from ChangeScripts  where ScriptVersion = @CurrentScriptVersion)
 set noexec on 

--check that current DB version is Ok
declare @DBCurrentVersion as nvarchar(100) = (select top 1 ScriptVersion from ChangeScripts order by DateExecuted desc)
if (@DBCurrentVersion != @RequiredDBVersion)
begin
	RAISERROR(N'DB version is "%s", required "%s".', 16, 1, @DBCurrentVersion, @RequiredDBVersion)
	set noexec on
end

GO

IF OBJECT_ID ('[dbo].[UpdateDocumentsData]') IS NOT NULL
	DROP PROCEDURE [dbo].[UpdateDocumentsData]
GO

CREATE PROCEDURE [dbo].[UpdateDocumentsData]
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
	MetaTags = @metaTags,
	Type = @categoryType,
	ModifiedBy = @userName,
	ModifiedOn = GETUTCDATE()
	WHERE [FileID] = @fileId
	
	IF @comment <> NULL
	BEGIN
		INSERT INTO CommentInfo ( [Comment], [CommentBy], [CommentOn], [FileID] )
		VALUES ( @comment, @userName, GETUTCDATE(), @fileId )
	END
    
END


GO

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00124'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())
