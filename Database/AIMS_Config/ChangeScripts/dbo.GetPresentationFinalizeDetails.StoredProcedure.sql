SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[GetPresentationFinalizeDetails] 	
@ScheduleMinutes INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @CURRENT_UTC DATETIME = GETUTCDATE()
	DECLARE @MARGINAL_UTC DATETIME = DATEADD(MI, -1 * @ScheduleMinutes, @CURRENT_UTC)
	
	SELECT 
		_MI.MeetingID, _MI.MeetingDateTime, _MI.MeetingClosedDateTime, _MI.MeetingDescription, _MI.MeetingVotingClosedDateTime,
		
		_PI.PresentationID, _PI.AdminNotes, _PI.CommitteeBuyRange, _PI.CommitteeSellRange ,_PI.CommitteePFVMeasure, _PI.CommitteeRecommendation,
		_PI.Presenter, _PI.SecurityName, _PI.SecurityTicker, _PI.SecurityCountry, _PI.SecurityIndustry,
		_PI.SecurityBuyRange, _PI.SecuritySellRange, _PI.SecurityPFVMeasure, _PI.SecurityRecommendation,
		
		_VI.VoterID, _VI.Name, _VI.VoteType,_VI.VoterPFVMeasure, _VI.VoterBuyRange, _VI.VoterSellRange, _VI.DiscussionFlag, _VI.Notes, _VI.AttendanceType,
		
		_FM.FileID, _FM.[Type], _FM.Category, _FM.Name as [FileName], _FM.Location
		
	FROM MeetingPresentationMappingInfo _MPMI
	LEFT JOIN MeetingInfo _MI ON _MI.MeetingID = _MPMI.MeetingID
	LEFT JOIN PresentationInfo _PI ON _PI.PresentationID = _MPMI.PresentationID
	LEFT JOIN MeetingAttachedFileInfo _MAFI ON _MAFI.MeetingID = _MI.MeetingID	
	LEFT JOIN FileMaster _FM ON _FM.FileID = _MAFI.FileID
	LEFT JOIN VoterInfo _VI ON _VI.PresentationID = _PI.PresentationID
	WHERE _PI.StatusType = 'Final'
	AND _PI.ModifiedOn >= @MARGINAL_UTC AND _PI.ModifiedOn <= @CURRENT_UTC	
	AND PostMeetingFlag = 'True'
	
END
GO
