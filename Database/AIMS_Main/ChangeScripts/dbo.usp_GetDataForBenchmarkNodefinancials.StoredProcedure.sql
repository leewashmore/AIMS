SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[usp_GetDataForBenchmarkNodefinancials]
(
@issuerIds varchar(max),
@securityIds varchar(max)
)
AS

SET FMTONLY OFF

BEGIN

DECLARE @tempPorfolio TABLE
(
IssuerId varchar(20),
SecurityId varchar(20),
DataId int,
Amount Decimal(32,6)
)

declare @sqlquery varchar(max)

if @issuerIds is not null and @securityIds is not null
begin
 set @sqlquery = 'Select ISSUER_ID,SECURITY_ID,DATA_ID,AMOUNT
from PERIOD_FINANCIALS 
where CURRENCY = ''USD''
and PERIOD_TYPE = ''C''
and DATA_SOURCE = ''PRIMARY''
and (ISSUER_ID IN ('+@issuerIds+')or SECURITY_ID IN ('+@securityIds+'))
and DATA_ID IN (197, 198, 187, 189, 188, 200, 236, 201, 202)'
end

else if @issuerIds is not null
begin
set @sqlquery  = 'Select ISSUER_ID,SECURITY_ID,DATA_ID,AMOUNT 
from PERIOD_FINANCIALS 
where CURRENCY = ''USD''
and PERIOD_TYPE = ''C''
and DATA_SOURCE = ''PRIMARY''
and (ISSUER_ID IN ('+@issuerIds+'))
and DATA_ID IN (197, 198, 187, 189, 188, 200, 236, 201, 202)'
end

else if @securityIds is not null
begin 
set @sqlquery = 'Select ISSUER_ID,SECURITY_ID,DATA_ID,AMOUNT 
from PERIOD_FINANCIALS 
where CURRENCY = ''USD''
and PERIOD_TYPE = ''C''
and DATA_SOURCE = ''PRIMARY''
and (SECURITY_ID IN ('+@securityIds+'))
and DATA_ID IN (197, 198, 187, 189, 188, 200, 236, 201, 202)'
end

INSERT INTO @tempPorfolio  EXECUTE(@sqlquery)
SELECT * FROM @tempPorfolio
END
GO
