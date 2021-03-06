SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[GetUserBenchmarkPreference] 
	-- Add the parameters for the stored procedure here
	  @userid varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 
	tblUserGroupPreference.GroupName,
	tblUserGroupPreference.GroupOrder,
	tblUserGroupPreference.GroupPreferenceID,
    tblUserBenchmarkPreference.BenchmarkName,
    tblUserBenchmarkPreference.BenchmarkReturnType,
    tblUserBenchmarkPreference.BenchmarkOrder
    
 from 
 tblUserGroupPreference LEFT OUTER JOIN tblUserBenchmarkPreference
 
 ON
  tblUserGroupPreference.GroupPreferenceID = tblUserBenchmarkPreference.GroupPreferenceID
  
  where 
  tblUserGroupPreference.UserId = @userid
 
END
GO
