SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[GetFinancialTabDataDescriptions]
	@tabName nvarchar(10)
AS
SET FMTONLY OFF
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 if @tabName = 'Period'
 begin
	Select a.SCREENING_ID, b.DATA_DESC, b.LONG_DESC, b.QUARTERLY, b.ANNUAL,a.DATA_ID,a.ESTIMATE_ID
	INTO #periodTempTable
	from SCREENING_DISPLAY_PERIOD a 
	left join 
	DATA_MASTER b  
	on a.DATA_ID = b.DATA_ID
	order by data_desc;
	
	Select * from #periodTempTable
	WHERE DATA_DESC IS NOT NULL;
	Drop table #periodTempTable;
end

else if @tabName = 'Current'
begin
	Select a.SCREENING_ID, b.DATA_DESC, b.LONG_DESC, null AS QUARTERLY,null AS ANNUAL,a.DATA_ID, a.ESTIMATE_ID
	INTO #currentTempTable
	from SCREENING_DISPLAY_CURRENT a 
	left join 
	DATA_MASTER b  
	on a.DATA_ID = b.DATA_ID
	order by data_desc;
	
	Select * from #currentTempTable
	WHERE DATA_DESC IS NOT NULL;
	Drop table #currentTempTable;
end

END
GO
