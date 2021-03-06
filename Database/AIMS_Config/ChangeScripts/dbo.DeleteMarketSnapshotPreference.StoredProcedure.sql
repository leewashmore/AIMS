SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[DeleteMarketSnapshotPreference] 
	-- Add the parameters for the stored procedure here
	  @userId nVARCHAR(100),
	  @snapshotname NVARCHAR(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Delete from tblMarketSnapshotPreference	
	where UserId = @userId AND SnapshotName = @snapshotname	
	Select @@ROWCOUNT
	
END
GO
