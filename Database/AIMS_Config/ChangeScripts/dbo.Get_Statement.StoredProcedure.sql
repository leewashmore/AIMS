SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[Get_Statement](
	@ISSUER_ID			varchar(20),				-- The company identifier		
	@DATA_SOURCE		varchar(10)  = 'REUTERS',	-- REUTERS, PRIMARY, INDUSTRY
	@PERIOD_TYPE		char(2) = 'A',				-- A, Q
	@FISCAL_TYPE		char(8) = 'FISCAL',			-- FISCAL, CALENDAR
	@STATEMENT_TYPE		char(3) = 'BAL',			-- Type of statement to get: BAL, CAS, INC
	@CURRENCY			char(3)	= 'USD'				-- USD or the currency of the country (local)
)
as

-- Select the data for the statement
select pfd.GROUP_NAME
	,  pf.Data_ID
	,  pfd.BOLD_FONT
	,  pfd.SORT_ORDER
	,  pfd.DATA_DESC
	,  substring(AMOUNT_TYPE,1,1) as AMOUNT_TYPE
	,  cast(pf.PERIOD_YEAR as CHAR(4)) as PERIOD
	,  pf.PERIOD_TYPE
	,  pf.AMOUNT * case when (pfd.MULTIPLIER = 0.0) then 1.0 else pfd.MULTIPLIER end as AMOUNT
	,  pfd.DECIMALS
	,  pf.ROOT_SOURCE
	,  pf.ROOT_SOURCE_DATE
	,  pf.CALCULATION_DIAGRAM
--  into #statement
  from dbo.PERIOD_FINANCIALS pf
 inner join dbo.PERIOD_FINANCIALS_DISPLAY pfd on pfd.DATA_ID = pf.DATA_ID and pfd.COA_TYPE = pf.COA_TYPE
 inner join dbo.DATA_MASTER dm on dm.DATA_ID = pf.DATA_ID 
 where 1=1
   and pf.ISSUER_ID = @ISSUER_ID
   and pf.DATA_SOURCE = @DATA_SOURCE
   and pf.PERIOD_TYPE = @PERIOD_TYPE
   and pf.FISCAL_TYPE = @FISCAL_TYPE
   and pf.CURRENCY = @CURRENCY
   and pfd.STATEMENT_TYPE = @STATEMENT_TYPE
 order by pfd.SORT_ORDER, Period
 ;

/* No longer needed, Application will pivot.
-- Pivot the data so the years are columns 
DECLARE @cols NVARCHAR(3000)
SELECT @cols = COALESCE(@cols + '],[', '') + 
   CAST( period AS varchar(7))
FROM (SELECT DISTINCT Period FROM #statement) a

DECLARE @query NVARCHAR(4000)
SET @query = 'SELECT GROUP_NAME, BOLD_FONT, SORT_ORDER, DATA_DESC, [' + @cols + 
'] FROM( SELECT GROUP_NAME, BOLD_FONT, SORT_ORDER, DATA_DESC, Period, Amount
  from #statement ) p
 PIVOT (MAX(Amount) FOR [Period] IN (['+ @cols +'])) AS pvt
 order by SORT_ORDER'

EXECUTE(@query)
*/

--drop table #statement
GO
