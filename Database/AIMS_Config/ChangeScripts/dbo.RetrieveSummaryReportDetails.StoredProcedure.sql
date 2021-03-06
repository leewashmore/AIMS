SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[RetrieveSummaryReportDetails] 
	-- Add the parameters for the stored procedure here
	@startDate DateTime, 
	@endDate DateTime
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @ReportCurrentInfo TABLE
	(
		PresentationID BIGINT NULL,
		CurrentCashPosition REAL NULL,
		CurrentPosition BIGINT NULL,
		CurrentPFVMeasure VARCHAR(50) NULL,
		CurrentBuyRange REAL NULL,
		CurrentSellRange REAL NULL,
		CurrentPFVMeasureValue DECIMAL NULL, 
		CurrentUpside REAL NULL
	)	

    SELECT
    ROW_NUMBER() OVER (ORDER BY _MI.MeetingDateTime) AS [Sno],
    _PI.SecurityTicker,
    _PI.SecurityName,
    _PI.SecurityCountry,    
    _MI.MeetingDateTime,
    _PI.Analyst,
    _PI.SecurityCashPosition,
    _PI.SecurityPosition,
    _PI.SecurityPFVMeasure,
    CAST(_PI.SecurityBuyRange AS VARCHAR(50)) + ' - ' + CAST(_PI.SecuritySellRange AS VARCHAR(50)) AS [SecurityBuySellRange],
    _PI.SecurityPFVMeasureValue,
    _PI.SecurityRecommendation,
    _PI.CommitteePFVMeasure,
    CAST(_PI.CommitteeBuyRange AS VARCHAR(50)) + ' - ' + CAST(_PI.CommitteeSellRange AS VARCHAR(50)) AS [CommitteeBuySellRange],
    _PI.CommitteePFVMeasureValue,
    _PI.CommitteeRecommendation,
    _RCI.CurrentCashPosition,
    _RCI.CurrentPosition,
    _RCI.CurrentPFVMeasure,
    CAST(_RCI.CurrentBuyRange AS VARCHAR(50)) + ' - ' + CAST(_RCI.CurrentSellRange AS VARCHAR(50)) AS [CurrentBuySellRange],
    _RCI.CurrentPFVMeasureValue,
    _RCI.CurrentUpside,
    _F.Location,
    _PI.Security_Id
    
    FROM PresentationInfo _PI
    LEFT JOIN @ReportCurrentInfo _RCI ON _RCI.PresentationID = _PI.PresentationID
    LEFT JOIN MeetingPresentationMappingInfo _MPMI ON _MPMI.PresentationID = _PI.PresentationID
    LEFT JOIN MeetingInfo _MI ON _MI.MeetingID = _MPMI.MeetingID 
    LEFT JOIN dbo.PresentationAttachedFileInfo  _PAI ON _PAI.PresentationID = _PI.PresentationID
    LEFT JOIN dbo.FileMaster _F  on _F.FileID = _PAI.FileID
    WHERE cast(_MI.MeetingDateTime as date) >= @startDate AND  cast(_MI.MeetingDateTime as date) <= @endDate    
    and _F.Category = 'Investment Committee Packet'
END
GO


--exec [dbo].[RetrieveSummaryReportDetails] '11/03/2014', '11/06/2014'