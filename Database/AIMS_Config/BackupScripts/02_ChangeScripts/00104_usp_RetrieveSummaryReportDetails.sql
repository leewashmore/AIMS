set noexec off

--declare  current and required version
declare @RequiredDBVersion as nvarchar(100) = '00103'
declare @CurrentScriptVersion as nvarchar(100) = '00104'
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

IF OBJECT_ID ('[dbo].[RetrieveSummaryReportDetails]') IS NOT NULL
	DROP PROCEDURE [dbo].[RetrieveSummaryReportDetails]
GO

CREATE PROCEDURE [dbo].[RetrieveSummaryReportDetails] 
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
    _RCI.CurrentUpside
    FROM PresentationInfo _PI
    LEFT JOIN @ReportCurrentInfo _RCI ON _RCI.PresentationID = _PI.PresentationID
    LEFT JOIN MeetingPresentationMappingInfo _MPMI ON _MPMI.PresentationID = _PI.PresentationID
    LEFT JOIN MeetingInfo _MI ON _MI.MeetingID = _MPMI.MeetingID    
    WHERE _MI.MeetingDateTime >= @startDate AND _MI.MeetingDateTime <= @endDate    
    
END

GO

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00104'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())
