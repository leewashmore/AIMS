SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[UpdatePresentationInfo] 
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
				
		DECLARE @UpdatePresentationRecordCount INT
		SELECT @UpdatePresentationRecordCount = COUNT(*) FROM OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)			
		
		IF @UpdatePresentationRecordCount <> 0
		BEGIN
			
			DECLARE @MeetingID BIGINT
			
			UPDATE PresentationInfo SET
			[Presenter] = _IN.[Presenter],
			[StatusType] = _IN.[StatusType],
			[SecurityTicker] = _IN.[SecurityTicker],
			[SecurityName] = _IN.[SecurityName],
			[SecurityCountry] = _IN.[SecurityCountry],
			[SecurityCountryCode] = _IN.[SecurityCountryCode],
			[SecurityIndustry] = _IN.[SecurityIndustry],
			[SecurityCashPosition] = _IN.[SecurityCashPosition],
			[SecurityPosition] = _IN.[SecurityPosition],
			[SecurityLastClosingPrice] = _IN.[SecurityLastClosingPrice],
			[SecurityMarketCapitalization] = _IN.[SecurityMarketCapitalization],
			[SecurityPFVMeasure] = _IN.[SecurityPFVMeasure],
			[SecurityPFVMeasureValue] = _IN.[SecurityPFVMeasureValue],
			[SecurityBuyRange] = _IN.[SecurityBuyRange],
			[SecuritySellRange] = _IN.[SecuritySellRange],
			[SecurityRecommendation] = _IN.[SecurityRecommendation],
			[CommitteePFVMeasure] = _IN.[CommitteePFVMeasure],
			[CommitteePFVMeasureValue] = _IN.[CommitteePFVMeasureValue],
			[CommitteeBuyRange] = _IN.[CommitteeBuyRange],
			[CommitteeSellRange] = _IN.[CommitteeSellRange],
			[CommitteeRecommendation] = _IN.[CommitteeRecommendation],
			[CommitteeRangeEffectiveThrough] = _IN.[CommitteeRangeEffectiveThrough],
			[AcceptWithoutDiscussionFlag] = _IN.[AcceptWithoutDiscussionFlag],
			[AdminNotes] = _IN.[AdminNotes],
			[ModifiedBy] = @userName,
			[ModifiedOn] = GETUTCDATE(),
			[Analyst] = _IN.[Analyst],
			[Price] = _IN.[Price],
			[FVCalc] = _IN.[FVCalc],
			[SecurityBuySellvsCrnt] = _IN.[SecurityBuySellvsCrnt],
			[CurrentHoldings] = _IN.[CurrentHoldings],
			[PercentEMIF] = _IN.[PercentEMIF],
			[SecurityBMWeight] = _IN.[SecurityBMWeight],
			[SecurityActiveWeight] = _IN.[SecurityActiveWeight],
			[YTDRet_Absolute] = _IN.[YTDRet_Absolute],
			[YTDRet_RELtoLOC] = _IN.[YTDRet_RELtoLOC],
			[YTDRet_RELtoEM] = _IN.[YTDRet_RELtoEM],
			[PortfolioId] = _IN.[PortfolioId]
			FROM PresentationInfo 
			INNER JOIN 
			OPENXML(@idoc, '/Root/ICPresentationOverviewData', 1)
			WITH (
			[PresentationID] BIGINT,
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
			[PortfolioId] VARCHAR(50)) _IN	
			ON PresentationInfo.[PresentationID] = _IN.[PresentationID]
						
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -2
				RETURN
			END				
		END	
	SELECT 0	
	COMMIT TRANSACTION
END
GO
