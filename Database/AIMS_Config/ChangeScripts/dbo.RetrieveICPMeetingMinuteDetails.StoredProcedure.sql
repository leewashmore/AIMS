SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rahul Vig
-- Create date: 18-08-2012
-- Description:	Retrieve Meeting Minute Details
-- =============================================
alter procedure [dbo].[RetrieveICPMeetingMinuteDetails] 	
	@MeetingId BIGINT = 0
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	_PI.PresentationID,
	_PI.Presenter,	
	_PI.SecurityName,
	_PI.SecurityTicker,
	_PI.SecurityCountry,
	_PI.SecurityIndustry,
	_VI.VoterID,
	_VI.Name,
	_VI.AttendanceType
	FROM 
	PresentationInfo  _PI  
	LEFT JOIN MeetingPresentationMappingInfo _MPMI ON _MPMI.PresentationID = _PI.PresentationID
	LEFT JOIN MeetingInfo _MI ON _MPMI.MeetingID = _MI.MeetingID
	LEFT JOIN VoterInfo _VI ON _VI.PresentationID = _PI.PresentationID
	
	WHERE _MI.MeetingID = @MeetingId AND _PI.StatusType = 'Voting Closed'    
END
GO
