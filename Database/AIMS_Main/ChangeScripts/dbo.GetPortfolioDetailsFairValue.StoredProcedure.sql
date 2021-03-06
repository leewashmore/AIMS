SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[GetPortfolioDetailsFairValue] 
	(
	@SECURITY_IDS varchar(max)
	)
AS
BEGIN
	
	declare @TempData Table
	(
	[VALUE_TYPE] [varchar](20) NOT NULL,
	[SECURITY_ID] [varchar](20) NOT NULL,
	[FV_MEASURE] [int] NOT NULL,
	[FV_BUY] [decimal](32, 6) NOT NULL,
	[FV_SELL] [decimal](32, 6) NOT NULL,
	[CURRENT_MEASURE_VALUE] [decimal](32, 6) NOT NULL,
	[UPSIDE] [decimal](32, 6) NOT NULL,
	[UPDATED] [datetime] NULL,
	[PERIOD_TYPE] [CHAR](2) NULL,
	[PERIOD_YEAR] [INT] NULL
	)
	declare @sqlquery varchar(max)
	SET NOCOUNT ON;

if @SECURITY_IDS is not null
begin
set @sqlquery ='SELECT * 
    FROM FAIR_VALUE 
    WHERE VALUE_TYPE=''PRIMARY'' 
    AND SECURITY_ID IN ('+ @SECURITY_IDS+')'
END
INSERT INTO @TempData  EXECUTE(@sqlquery)
    
    Select * from @TempData
    
END
GO
