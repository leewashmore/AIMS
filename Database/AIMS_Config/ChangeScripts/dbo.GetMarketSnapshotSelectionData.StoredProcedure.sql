SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[GetMarketSnapshotSelectionData] 
	-- Add the parameters for the stored procedure here
	  @userId nVARCHAR(100)	  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select SnapshotPreferenceId,SnapshotName from tblMarketSnapshotPreference
	WHERE UserId = @userId	
END
GO
