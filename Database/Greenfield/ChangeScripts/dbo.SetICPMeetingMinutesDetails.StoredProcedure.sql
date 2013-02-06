SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[SetICPMeetingMinutesDetails] 
	@UserName VARCHAR(50),
	@xmlScript NVARCHAR(MAX)	
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		
		DECLARE @XML XML
		DECLARE @UpdationICPMeetingAttachedFileRecordCount INT
		
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
		-- MeetingInfo - Updated MeetingDescription in MeetingInfo table
		--#############################################################################################################
		
		DECLARE @UpdationICPMeetingMinutesDescriptionRecordCount INT
		SELECT @UpdationICPMeetingMinutesDescriptionRecordCount = COUNT(*) FROM OPENXML(@idoc, '/Root/MeetingInfo', 1)			
			
		IF @UpdationICPMeetingMinutesDescriptionRecordCount <> 0
		BEGIN
			SELECT * INTO #ICPMeetingMinutesDescriptionData FROM OPENXML(@idoc, '/Root/MeetingInfo', 1)
				WITH ( 
					[MeetingID]				BIGINT, 
					[MeetingDescription]	VARCHAR(255))			
			UPDATE MeetingInfo 
			SET [MeetingDescription]	= _IMMD.[MeetingDescription],
				[ModifiedBy]			= @UserName,
				[ModifiedOn]			= GETUTCDATE()
			FROM #ICPMeetingMinutesDescriptionData _IMMD
			WHERE MeetingInfo.[MeetingID] = _IMMD.[MeetingID]
						
			DROP TABLE #ICPMeetingMinutesDescriptionData
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -2
				RETURN
			END		
		END
			
		--#############################################################################################################
		-- MeetingMinutesVoterInfo
		--#############################################################################################################
		DECLARE @UpdationMeetingMinutesVoterInfoRecordCount INT
		SELECT @UpdationMeetingMinutesVoterInfoRecordCount = COUNT(*) FROM OPENXML(@idoc, '/Root/MeetingMinuteData', 1)		
			
		IF @UpdationMeetingMinutesVoterInfoRecordCount <> 0
		BEGIN
			SELECT _MMVI.[PresentationID],
				   _MMVI.[VoterID],
				   _MMVI.[Name],
				   _MMVI.[AttendanceType]				   
			INTO #ICPMeetingMinutesVoterData 
			FROM OPENXML(@idoc, '/Root/MeetingMinuteData', 1)
				WITH ( 
					[PresentationID]	BIGINT, 
					[VoterID]			BIGINT, 
					[Name]				VARCHAR(50),
					[AttendanceType]	VARCHAR(50)) _MMVI
					
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -3
				RETURN
			END	
					
			MERGE INTO VoterInfo _VI
			USING #ICPMeetingMinutesVoterData _MMVD
			ON _VI.[VoterID] = _MMVD.[VoterID]
			WHEN MATCHED THEN
			UPDATE SET
				_VI.[AttendanceType] = _MMVD.[AttendanceType],
				_VI.[ModifiedBy] = @UserName,
				_VI.[ModifiedOn] = GETUTCDATE()
			WHEN NOT MATCHED THEN
			INSERT ([PresentationID], [Name], [AttendanceType], [PostMeetingFlag], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
			VALUES (_MMVD.[PresentationID], _MMVD.[Name], _MMVD.[AttendanceType], 'True', @UserName, GETUTCDATE()
			, @UserName, GETUTCDATE());
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -4
				RETURN
			END	
				
			DROP TABLE #ICPMeetingMinutesVoterData
			
			IF @@ERROR <> 0
			BEGIN
				ROLLBACK TRANSACTION
				SELECT -5
				RETURN
			END		
		END	
		
	COMMIT TRANSACTION	
	SELECT 0
    
END
GO
