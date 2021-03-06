SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[GetCustomScreeningMarketCap]
(
@securityIdsList varchar(max)
)
AS

SET FMTONLY OFF

BEGIN
DECLARE @tempTable TABLE
(
SecurityId varchar(20),
Amount decimal(32,6) not null
)
DECLARE @sqlquery varchar(max);
if @securityIdsList is not null
begin
Set @sqlquery = 'Select SECURITY_ID,AMOUNT
from PERIOD_FINANCIALS
where SECURITY_ID IN ('+@securityIdsList+')
AND DATA_SOURCE = ''PRIMARY''
AND	DATA_ID = 185
AND CURRENCY = ''USD''
AND PERIOD_TYPE = ''C'''
end
Print @sqlquery
INSERT INTO @tempTable  EXECUTE(@sqlquery)

Select * from @tempTable;
END
GO
