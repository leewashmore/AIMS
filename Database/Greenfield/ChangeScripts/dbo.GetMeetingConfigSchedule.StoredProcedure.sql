SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sneha Sharma
-- Create date: 14-08-2012
-- Description:	Fetch the System Configuration Settings
-- =============================================
alter procedure [dbo].[GetMeetingConfigSchedule]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	
	SELECT 
	config.PresentationDateTime,
	config.PresentationTimeZone,
	config.PreMeetingVotingDeadline,
	config.PresentationDeadline,
	config.ConfigurablePresentationDeadline,
	config.ConfigurablePreMeetingVotingDeadline
	
	FROM 
	  MeetingConfigurationSchedule  config	
END
GO
