set noexec off

--declare  current and required version
declare @RequiredDBVersion as nvarchar(100) = '00093'
declare @CurrentScriptVersion as nvarchar(100) = '00094'
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

IF OBJECT_ID ('[dbo].[GetPreMeetingVotingReportDetails]') IS NOT NULL
	DROP PROCEDURE [dbo].[GetPreMeetingVotingReportDetails]
GO

CREATE PROCEDURE [dbo].[GetPreMeetingVotingReportDetails] 	
@PresentationId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT _MI.MeetingID, _MI.MeetingDateTime, _MI.MeetingClosedDateTime, 
		_PI.PresentationID, _PI.SecurityName, _PI.SecurityTicker, _PI.SecurityCountry, _PI.SecurityBuyRange, _PI.SecuritySellRange, _PI.SecurityPFVMeasure, _PI.SecurityRecommendation,
		_VI.VoterID, _VI.Name, _VI.VoteType, _VI.VoterBuyRange, _VI.VoterSellRange, _VI.DiscussionFlag, _VI.Notes,
		_FM.FileID, _FM.[Type], _FM.Category, _FM.Name as [FileName], _FM.Location
	FROM MeetingPresentationMappingInfo _MPMI
	LEFT JOIN MeetingInfo _MI ON _MI.MeetingID = _MPMI.MeetingID
	LEFT JOIN PresentationInfo _PI ON _PI.PresentationID = _MPMI.PresentationID
	LEFT JOIN PresentationAttachedFileInfo _PAFI ON _PAFI.PresentationID = _PI.PresentationID
	LEFT JOIN FileMaster _FM ON _FM.FileID = _PAFI.FileID
	LEFT JOIN VoterInfo _VI ON _VI.PresentationID = _PI.PresentationID
	WHERE _PI.PresentationID = @PresentationId
	AND _VI.PostMeetingFlag = 'False'
	
END
GO

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00094'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())
