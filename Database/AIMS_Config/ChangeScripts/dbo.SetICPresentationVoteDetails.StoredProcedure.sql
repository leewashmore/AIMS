SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===================================================
-- Author:		Rahul Vig
-- Create date: 25-08-2012
-- Description:	Update Decision Entry Details
-- ===================================================
alter procedure [dbo].[SetICPresentationVoteDetails] 
	@UserName VARCHAR(50),
	@xmlScript NVARCHAR(MAX)	
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		
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
		
		--#############################################################################################################
		-- VoterInfo - Updated VoterInfo table
		--#############################################################################################################
		DECLARE @UpdationVoterInfoRecordCount INT
		SELECT @UpdationVoterInfoRecordCount = COUNT(*) FROM OPENXML(@idoc, '/Root/VoterInfo', 1)		
			
		IF @UpdationVoterInfoRecordCount <> 0
		BEGIN
			SELECT _MMVI.[VoterID],
				   _MMVI.[VoterPFVMeasure],
				   _MMVI.[VoterBuyRange],
				   _MMVI.[VoterSellRange],
				   _MMVI.[VoteType],
				   _MMVI.[Notes],
				   _MMVI.[DiscussionFlag]
			INTO #ICVoterInfo
			FROM OPENXML(@idoc, '/Root/VoterInfo', 1)
				WITH ( 
					[VoterID]			BIGINT, 
					[VoterPFVMeasure]	VARCHAR(255), 
					[VoterBuyRange]		REAL,
					[VoterSellRange]	REAL,
					[VoteType]			VARCHAR(50),
					[Notes]				VARCHAR(255),
					[DiscussionFlag]	BIT) _MMVI

			UPDATE VoterInfo
			SET
				VoterInfo.[VoterPFVMeasure] = _ICVI.[VoterPFVMeasure],
				VoterInfo.[VoterBuyRange] = _ICVI.[VoterBuyRange],
				VoterInfo.[VoterSellRange] = _ICVI.[VoterSellRange],
				VoterInfo.[VoteType] = _ICVI.[VoteType],
				VoterInfo.[Notes] = _ICVI.[Notes],
				VoterInfo.[DiscussionFlag] = _ICVI.[DiscussionFlag],
				VoterInfo.[ModifiedBy] = @UserName,
				VoterInfo.[ModifiedOn] = GETUTCDATE()
			FROM #ICVoterInfo _ICVI
			WHERE VoterInfo.[VoterID] = _ICVI.[VoterID]	
											
			DROP TABLE #ICVoterInfo
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -3
				RETURN
			END	
		END
		
	SELECT 0
	COMMIT TRANSACTION	
    
END
GO
