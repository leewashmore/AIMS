set noexec off

--declare  current and required version
declare @RequiredDBVersion as nvarchar(100) = '00082'
declare @CurrentScriptVersion as nvarchar(100) = '00083'
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

IF OBJECT_ID ('[dbo].[RetrieveICPresentationAttachedFileDetails]') IS NOT NULL
	DROP PROCEDURE [dbo].[RetrieveICPresentationAttachedFileDetails]
GO

CREATE PROCEDURE [dbo].[RetrieveICPresentationAttachedFileDetails]
	@PresentationId BIGINT = 0
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM
    FileMaster _FM
    WHERE _FM.[FileID] IN
    ( SELECT [FileID] FROM PresentationAttachedFileInfo WHERE [PresentationID] = @PresentationId )    
    ORDER BY _FM.[Name]
END
GO

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00083'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())
