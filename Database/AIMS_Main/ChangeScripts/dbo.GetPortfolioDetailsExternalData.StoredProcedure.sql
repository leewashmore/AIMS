SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[GetPortfolioDetailsExternalData] 
	(
		@issuerIds varchar(max),
		@securityIds varchar(max)
	)
AS
BEGIN
		
	SET NOCOUNT ON;

DECLARE @tempData TABLE
(
IssuerId varchar(20),
SecurityId varchar(20),
DataSource varchar(10),
PeriodType varchar(5),
Currency varchar(5),
DataId int,
Amount Decimal(32,6),
PeriodYear int
)

declare @sqlquery varchar(max)
declare @sqlqueryIssuer varchar(max)
declare @sqlfwdroeIssuer varchar(max)

declare @currentYear int =Year(getdate())
declare @nextYear int =Year(getdate())+1

if @securityIds is not null
begin
set @sqlquery = 'Select ISSUER_ID,SECURITY_ID,DATA_SOURCE,PERIOD_TYPE,CURRENCY,
 DATA_ID,AMOUNT,PERIOD_YEAR 
from PERIOD_FINANCIALS 
where CURRENCY = ''USD''
and PERIOD_TYPE = ''C''
and DATA_SOURCE = ''PRIMARY''
and (SECURITY_ID IN ('+@securityIds+'))
and DATA_ID IN (185,187,198,188)'
end

if @issuerIds is not null
    begin
    set @sqlqueryIssuer = 'Select ISSUER_ID,SECURITY_ID,DATA_SOURCE,PERIOD_TYPE,CURRENCY,
 DATA_ID,AMOUNT,PERIOD_YEAR 
from PERIOD_FINANCIALS 
where CURRENCY = ''USD''
and PERIOD_TYPE = ''A''
and FISCAL_TYPE=''CALENDAR''
and DATA_SOURCE = ''PRIMARY''
and (ISSUER_ID IN ('+@issuerIds+'))
and DATA_ID IN (178,177,133,149,146)'
--forward roe
set @sqlfwdroeIssuer = 'Select ISSUER_ID,SECURITY_ID,DATA_SOURCE,PERIOD_TYPE,CURRENCY,
 DATA_ID,AMOUNT,PERIOD_YEAR 
from PERIOD_FINANCIALS 
where CURRENCY = ''USD''
and PERIOD_TYPE = ''C''
and DATA_SOURCE = ''PRIMARY''
and (ISSUER_ID IN ('+@issuerIds+'))
and DATA_ID IN (200)'
end
    
    INSERT INTO @tempData  EXECUTE(@sqlquery)
    INSERT INTO @tempData  EXECUTE(@sqlqueryIssuer)   
    INSERT INTO @tempData  EXECUTE(@sqlfwdroeIssuer)   
    select * from @tempData
    
    
    
    
    
END
GO
-- exec [dbo].[GetPortfolioDetailsExternalData] '10106947',null