set noexec off

--declare  current and required version
declare @RequiredDBVersion as nvarchar(100) = '00112'
declare @CurrentScriptVersion as nvarchar(100) = '00113'
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

IF OBJECT_ID ('[dbo].[PresentationInfo]') IS NOT NULL
	ALTER TABLE [PresentationInfo]
	ADD [PortfolioId] VARCHAR(50) NULL
GO



--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00113'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())
