set noexec off

--declare  current and required version
declare @RequiredDBVersion as nvarchar(100) = '00087'
declare @CurrentScriptVersion as nvarchar(100) = '00088'
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

IF OBJECT_ID ('[dbo].[SetMeetingConfigSchedule]') IS NOT NULL
	DROP PROCEDURE [dbo].[SetMeetingConfigSchedule]
GO

CREATE PROCEDURE [dbo].[SetMeetingConfigSchedule]
	-- Add the parameters for the stored procedure here
	@userName VARCHAR(50),
	@PresentationDay VARCHAR(50),
	@PresentationTime DATETIME,
	@PresentationTimeZone VARCHAR(50),
	@PresentationDeadlineDay VARCHAR(50),
	@PresentationDeadlineTime DATETIME,
	@PreMeetingVotingDeadlineDay VARCHAR(50),
	@PreMeetingVotingDeadlineTime DATETIME    
AS
BEGIN		
	
	UPDATE MeetingConfigurationSchedule   
	SET [PresentationDay] = @PresentationDay,
		[PresentationTime] = @PresentationTime,
		[PresentationTimeZone] = @PresentationTimeZone,
		[PresentationDeadlineDay] = @PresentationDeadlineDay,
		[PresentationDeadlineTime] = @PresentationDeadlineTime,
		[PreMeetingVotingDeadlineDay] = @PreMeetingVotingDeadlineDay,
		[PreMeetingVotingDeadlineTime] = @PreMeetingVotingDeadlineTime,
		[ModifiedBy] = @userName,
		[ModifiedOn] = GETUTCDATE() 	
 	if @@ERROR <> 0
 	BEGIN 
 	 SELECT -1
 	END
 	
 	SELECT 0
END
GO

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00088'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())
