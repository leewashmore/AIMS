set noexec off

--declare  current and required version
--also do it an the end of the script
declare @RequiredDBVersion as nvarchar(100) = '00037'
declare @CurrentScriptVersion as nvarchar(100) = '00038'

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

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserBenchmarkPreference] 
	-- Add the parameters for the stored procedure here
	  @userid varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 
	tblUserGroupPreference.GroupName,
	tblUserGroupPreference.GroupOrder,
	tblUserGroupPreference.GroupPreferenceID,
    tblUserBenchmarkPreference.BenchmarkName,
    tblUserBenchmarkPreference.BenchmarkReturnType,
    tblUserBenchmarkPreference.BenchmarkOrder
    
 from 
 tblUserGroupPreference LEFT OUTER JOIN tblUserBenchmarkPreference
 
 ON
  tblUserGroupPreference.GroupPreferenceID = tblUserBenchmarkPreference.GroupPreferenceID
  
  where 
  tblUserGroupPreference.UserId = @userid
 
END
GO


--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00038'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())






