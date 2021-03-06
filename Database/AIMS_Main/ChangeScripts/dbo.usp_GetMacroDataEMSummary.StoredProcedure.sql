SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[usp_GetMacroDataEMSummary]
(
@countryCodes varchar(max)
)
AS

SET FMTONLY OFF

BEGIN
DECLARE @tempMacroData TABLE
(
CountryCode varchar(8),
Field varchar(30),
Year1 int,
Value Decimal(32,6)
)
declare @sqlquery varchar(max),
@previousYear INT,
@currentYear INT,
@nextYear INT
if @countryCodes is not null
begin
SET @currentYear = (select datepart(yyyy,getdate()))
set @previousYear = @currentYear - 1
set @nextYear = @currentYear + 1
 set @sqlquery = 'Select COUNTRY_CODE,FIELD,YEAR1,VALUE from dbo.Macroeconomic_Data 
where ( COUNTRY_CODE in  ('+@countryCodes+')) and FIELD in (''REAL_GDP_GROWTH_RATE'',''INFLATION_PCT'',''ST_INTEREST_RATE'',''CURRENT_ACCOUNT_PCT_GDP'')
and YEAR1 in ('+CAST(@previousYear AS VARCHAR(10))+','+CAST(@currentYear AS VARCHAR(10))+','+CAST(@nextYear AS VARCHAR(10))+')'
end
INSERT INTO @tempMacroData  EXECUTE(@sqlquery)
select * from @tempMacroData
END
GO
