set noexec off

--declare  current and required version
declare @RequiredDBVersion as nvarchar(100) = '00111'
declare @CurrentScriptVersion as nvarchar(100) = '00112'
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

IF OBJECT_ID ('[dbo].[RetrieveICMeetingInfoByStatusType]') IS NOT NULL
	DROP PROCEDURE [dbo].[RetrieveICMeetingInfoByStatusType]
GO

CREATE PROCEDURE [dbo].[RetrieveICMeetingInfoByStatusType] 
	@PresentationStatus VARCHAR(50)	
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT _MI.* FROM
	MeetingInfo _MI
	LEFT JOIN MeetingPresentationMappingInfo _MPMI ON _MPMI.MeetingID = _MI.MeetingID
	LEFT JOIN PresentationInfo _PI ON _MPMI.PresentationID = _PI.PresentationID
	WHERE _PI.StatusType = @PresentationStatus 
	AND (_PI.CommitteeBuyRange <> NULL OR _PI.CommitteeSellRange <> NULL OR _PI.CommitteePFVMeasure <> NULL 
	OR _PI.CommitteePFVMeasureValue <> NULL OR _PI.CommitteeRecommendation <> NULL)
	ORDER BY _MI.MeetingDateTime DESC
    
END
GO

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00112'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())
