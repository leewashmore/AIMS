set noexec off

--declare  current and required version
declare @RequiredDBVersion as nvarchar(100) = '00113'
declare @CurrentScriptVersion as nvarchar(100) = '00114'
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

IF OBJECT_ID ('[dbo].[SetPresentationInfo]') IS NOT NULL
	DROP PROCEDURE [dbo].[SetPresentationInfo]
GO

CREATE PROCEDURE [dbo].[SetPresentationInfo] 
	-- Add the parameters for the stored procedure here
	  @userName VARCHAR(50),
	  @xmlScript NVARCHAR(MAX)
AS
BEGIN
	
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		DECLARE @PresentationID BIGINT	
		DECLARE @XML XML
		
		SELECT @XML = @xmlScript
		DECLARE @idoc int
		EXEC sp_xml_preparedocument @idoc OUTPUT, @XML
		
		IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -1
				RETURN
			END	
			
				
		DECLARE @InsertPresentationRecordCount INT
		SELECT @InsertPresentationRecordCount = COUNT(*) FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)			
		
		DECLARE @SecurityPFVMeasure VARCHAR(255)
		DECLARE @SecurityPFVMeasureValue DECIMAL
		DECLARE @SecurityBuyRange REAL
		DECLARE	@SecuritySellRange REAL
		DECLARE @SecurityRecommendation VARCHAR(50)

		SELECT Top(1) @SecurityPFVMeasure = [SecurityPFVMeasure] FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
				WITH ( [SecurityPFVMeasure] VARCHAR(50))
				
		SELECT Top(1) @SecurityPFVMeasureValue = [SecurityPFVMeasureValue] FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
		WITH ( [SecurityPFVMeasureValue] DECIMAL)
				
		SELECT Top(1) @SecurityBuyRange = [SecurityBuyRange] FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
				WITH ( [SecurityBuyRange] REAL)

		SELECT Top(1) @SecuritySellRange = [SecuritySellRange] FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
				WITH ( [SecuritySellRange] REAL)

		SELECT Top(1) @SecurityRecommendation = [SecurityRecommendation] FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
				WITH ( [SecurityRecommendation] VARCHAR(50))
				
				
		
		IF @InsertPresentationRecordCount <> 0
		BEGIN
			
			DECLARE @MeetingID BIGINT
			
			INSERT INTO PresentationInfo (
			[Presenter],
			[StatusType],
			[SecurityTicker],
			[SecurityName],
			[SecurityCountry],
			[SecurityCountryCode],
			[SecurityIndustry],
			[SecurityCashPosition],
			[SecurityPosition],
			[SecurityLastClosingPrice],
			[SecurityMarketCapitalization],
			[SecurityPFVMeasure],
			[SecurityPFVMeasureValue],
			[SecurityBuyRange],
			[SecuritySellRange],
			[SecurityRecommendation],
			[CommitteePFVMeasure],
			[CommitteePFVMeasureValue],
			[CommitteeBuyRange],
			[CommitteeSellRange],
			[CommitteeRecommendation],
			[CommitteeRangeEffectiveThrough],
			[AcceptWithoutDiscussionFlag],
			[AdminNotes],
			[CreatedBy],
			[CreatedOn],
			[ModifiedBy],
			[ModifiedOn],
			[Analyst],
			[Price],
			[FVCalc],
			[SecurityBuySellvsCrnt],
			[CurrentHoldings],
			[PercentEMIF],
			[SecurityBMWeight],
			[SecurityActiveWeight],
			[YTDRet_Absolute],
			[YTDRet_RELtoLOC],
			[YTDRet_RELtoEM],
			[PortfolioId])
			
			SELECT TOP(1) 
			[Presenter],
			[StatusType],
			[SecurityTicker],
			[SecurityName],
			[SecurityCountry],
			[SecurityCountryCode],
			[SecurityIndustry],
			[SecurityCashPosition],
			[SecurityPosition],
			[SecurityLastClosingPrice],
			[SecurityMarketCapitalization],
			[SecurityPFVMeasure],
			[SecurityPFVMeasureValue],
			[SecurityBuyRange],
			[SecuritySellRange],
			[SecurityRecommendation],
			[CommitteePFVMeasure],
			[CommitteePFVMeasureValue],
			[CommitteeBuyRange],
			[CommitteeSellRange],
			[CommitteeRecommendation],
			[CommitteeRangeEffectiveThrough],
			[AcceptWithoutDiscussionFlag],
			[AdminNotes],
			@userName,
			GETUTCDATE(),
			@userName,
			GETUTCDATE(),
			[Analyst],
			[Price],
			[FVCalc],
			[SecurityBuySellvsCrnt],
			[CurrentHoldings],
			[PercentEMIF],
			[SecurityBMWeight],
			[SecurityActiveWeight],
			[YTDRet_Absolute],
			[YTDRet_RELtoLOC],
			[YTDRet_RELtoEM],
			[PortfolioId]
			
			FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
			WITH (
			[Presenter] VARCHAR(50),
			[StatusType] VARCHAR(50),
			[SecurityTicker] VARCHAR(50),
			[SecurityName] VARCHAR(50),
			[SecurityCountry] VARCHAR(50),
			[SecurityCountryCode] VARCHAR(50),
			[SecurityIndustry] VARCHAR(50),
			[SecurityCashPosition] REAL,
			[SecurityPosition] BIGINT,
			[SecurityLastClosingPrice] REAL,
			[SecurityMarketCapitalization] REAL,
			[SecurityPFVMeasure] VARCHAR(50),
			[SecurityPFVMeasureValue] DECIMAL,
			[SecurityBuyRange] REAL,
			[SecuritySellRange] REAL,
			[SecurityRecommendation] VARCHAR(50),
			[CommitteePFVMeasure] VARCHAR(255),
			[CommitteePFVMeasureValue] DECIMAL,
			[CommitteeBuyRange] REAL,
			[CommitteeSellRange] REAL,
			[CommitteeRecommendation] VARCHAR(50),
			[CommitteeRangeEffectiveThrough] DATETIME,
			[AcceptWithoutDiscussionFlag] BIT,
			[AdminNotes] VARCHAR(255),			
			[Analyst] VARCHAR(50),
			[Price] VARCHAR(50),
			[FVCalc] VARCHAR(50),
			[SecurityBuySellvsCrnt] VARCHAR(50),
			[CurrentHoldings] VARCHAR(50),
			[PercentEMIF] VARCHAR(50),
			[SecurityBMWeight] VARCHAR(50),
			[SecurityActiveWeight] VARCHAR(50),
			[YTDRet_Absolute] VARCHAR(50),
			[YTDRet_RELtoLOC] VARCHAR(50),
			[YTDRet_RELtoEM] VARCHAR(50),
			[PortfolioId] VARCHAR(50))		
			
			
			SET @PresentationID = @@IDENTITY			
			
						
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -2
				RETURN
			END				
			
			DECLARE @MeetingDateTime DATETIME
			SELECT Top(1) @MeetingDateTime = [MeetingDateTime] FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
				WITH ( [MeetingDateTime] DATETIME)
				
			DECLARE @MeetingClosedDateTime DATETIME
			SELECT Top(1) @MeetingClosedDateTime = [MeetingClosedDateTime] FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
				WITH ( [MeetingClosedDateTime] DATETIME)
				
			DECLARE @MeetingVotingClosedDateTime DATETIME
			SELECT Top(1) @MeetingVotingClosedDateTime = [MeetingVotingClosedDateTime] FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
				WITH ( [MeetingVotingClosedDateTime] DATETIME)		
			
			SELECT Top(1) @MeetingID = MeetingInfo.MeetingID FROM MeetingInfo
			WHERE MeetingDateTime = @MeetingDateTime 
			AND MeetingClosedDateTime = @MeetingClosedDateTime
			AND MeetingVotingClosedDateTime = @MeetingVotingClosedDateTime
			
			IF @MeetingID IS NULL
			BEGIN
				INSERT INTO MeetingInfo (MeetingDateTime, MeetingClosedDateTime, MeetingVotingClosedDateTime, [MeetingDescription], CreatedBy,
				CreatedOn, ModifiedBy, ModifiedOn)
				VALUES (@MeetingDateTime, @MeetingClosedDateTime, @MeetingVotingClosedDateTime, NULL, @userName,
				GETUTCDATE(), @userName, GETUTCDATE())
				
				SET @MeetingID = @@IDENTITY
			END
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -3
				RETURN
			END	
			
			INSERT INTO MeetingPresentationMappingInfo (PresentationID, MeetingID, CreatedBy, CreatedOn, ModifedBy, ModifiedOn)
			VALUES ( @PresentationID, @MeetingID, @userName, GETUTCDATE(), @userName, GETUTCDATE())
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -4
				RETURN
			END	
	
	END			
		
		DECLARE @InsertVoterInfoRecordCount INT
		SELECT @InsertVoterInfoRecordCount = COUNT(*) FROM OPENXML(@idoc, '/Root/VotingUser', 1)			
		
		
		IF @InsertVoterInfoRecordCount <> 0
		BEGIN
						
			INSERT INTO VoterInfo 
			(			[PresentationID],
						[Name],
						[VoteType],
						[PostMeetingFlag],
						[DiscussionFlag],
						[VoterPFVMeasure],
						[VoterBuyRange],
						[VoterSellRange],
						[VoterRecommendation],
						[CreatedBy],
						[CreatedOn],
						[ModifiedBy],
						[ModifiedOn])
			SELECT		@PresentationID,						
						[text],						
						CASE WHEN CAST([text] AS VARCHAR(50)) = @userName THEN 'Agree' ELSE 'Abstain' END,
						'False',
						'False',
						CASE WHEN CAST([text] AS VARCHAR(50)) = @userName THEN @SecurityPFVMeasure ELSE null END,
						CASE WHEN CAST([text] AS VARCHAR(50)) = @userName THEN @SecurityBuyRange ELSE null END,
						CASE WHEN CAST([text] AS VARCHAR(50)) = @userName THEN @SecuritySellRange ELSE null END,
						CASE WHEN CAST([text] AS VARCHAR(50)) = @userName THEN @SecurityRecommendation ELSE null END,
						@userName,
						GETUTCDATE(),
						@userName,
						GETUTCDATE()			
			FROM OPENXML(@idoc, '/Root/VotingUser') 
			WHERE [text] IS NOT NULL
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -5
				RETURN
			END	
			
			INSERT INTO VoterInfo 
			(			[PresentationID],
						[Name],
						[PostMeetingFlag],
						[DiscussionFlag],
						[CreatedBy],
						[CreatedOn],
						[ModifiedBy],
						[ModifiedOn])
			SELECT		@PresentationID,						
						[text],						
						'True',
						'False',
						@userName,
						GETUTCDATE(),
						@userName,
						GETUTCDATE()			
			FROM OPENXML(@idoc, '/Root/VotingUser') 
			WHERE [text] IS NOT NULL
					

			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -6
				RETURN
			END	
	END
	SELECT @PresentationID
	COMMIT TRANSACTION
END
GO

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00114'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())
