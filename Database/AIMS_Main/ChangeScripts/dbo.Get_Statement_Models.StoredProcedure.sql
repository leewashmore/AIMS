SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[Get_Statement_Models](
	@ISSUER_ID			varchar(20),				-- The company identifier		
	@DATA_SOURCE		varchar(10)  = 'REUTERS',	-- REUTERS, PRIMARY, INDUSTRY
	@PERIOD_TYPE		char(2)      = 'A',			-- A, Q
	@FISCAL_TYPE		char(8)      = 'FISCAL',	-- FISCAL, CALENDAR
	@STATEMENT_TYPE		char(3)      = 'BAL',		-- Type of statement to get: BAL, CAS, INC
	@CURRENCY			char(3)	     = 'USD'		-- USD or the currency of the country (local)
)
AS
--------------------------------------------------------------------------------------
-- Select the data for the statement
--------------------------------------------------------------------------------------
DECLARE @COA_TYPE varchar(10)

SET @COA_TYPE= (Select TOP 1 [COA_TYPE] from INTERNAL_ISSUER where ISSUER_ID=@ISSUER_ID);

SELECT 	*							
INTO #PERIOD_FINANCIALS_DATA
FROM dbo.PERIOD_FINANCIALS_ISSUER pf
WHERE pf.ISSUER_ID = @ISSUER_ID
	AND pf.DATA_SOURCE = @DATA_SOURCE
	AND LEFT(pf.PERIOD_TYPE,1) = @PERIOD_TYPE
	AND pf.FISCAL_TYPE = @FISCAL_TYPE
	AND pf.CURRENCY = @CURRENCY
	and pf.AMOUNT_TYPE='ACTUAL'
	
SELECT pfd.GROUP_NAME						AS [GroupName]
	,  pfd.DATA_ID							AS [DataId]
	,  pfd.BOLD_FONT						AS [BoldFont]
	,  pfd.DECIMALS							AS [Decimals]
	,  pf.ROOT_SOURCE						AS [RootSource]
	,  pf.ROOT_SOURCE_DATE					AS [RootSourceDate]
	,  pf.CALCULATION_DIAGRAM				AS [CalculationDiagram]
	,  pfd.DATA_DESC						AS [Description]
	,  pfd.SORT_ORDER						AS [SortOrder]
	,  pf.PERIOD_YEAR						AS [PeriodYear]
	,  ISNULL(pf.PERIOD_TYPE,'E')			AS [PeriodType]
	,  pf.AMOUNT * CASE 
	   WHEN (pfd.MULTIPLIER = 0.0) THEN 1.0 
	   ELSE pfd.MULTIPLIER END				AS [Amount]
	,  ISNULL(pf.AMOUNT_TYPE,'ESTIMATE')	AS [AmountType]
	,  'N'									AS [IsConsensus]
		from PERIOD_FINANCIALS_DISPLAY pfd
	
LEFT JOIN #PERIOD_FINANCIALS_DATA pf 
	ON pf.DATA_ID = pfd.DATA_ID 

WHERE pfd.STATEMENT_TYPE = @STATEMENT_TYPE
AND pfd.COA_TYPE=@COA_TYPE
ORDER BY pfd.SORT_ORDER, PERIOD_YEAR

DROP TABLE #PERIOD_FINANCIALS_DATA
GO
