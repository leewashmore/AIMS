SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[RetrieveICMeetingInfoByMeetingDateTime] 
	@MeetingDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT * FROM
	MeetingInfo _MI
	WHERE _MI.[MeetingDateTime] = @MeetingDateTime	
    
END
GO
