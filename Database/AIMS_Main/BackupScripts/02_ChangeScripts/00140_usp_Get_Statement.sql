set noexec off

--declare  current and required version
--also do it an the end of the script
declare @RequiredDBVersion as nvarchar(100) = '00139'
declare @CurrentScriptVersion as nvarchar(100) = '00140'

--if current version already in DB, just skip
if exists(select 1 from ChangeScripts  where ScriptVersion = @CurrentScriptVersion)
 set noexec on 

--check that current DB version is Ok
declare @DBCurrentVersion as nvarchar(100) = (select top 1 ScriptVersion from ChangeScripts order by DateExecuted desc)
if (@DBCurrentVersion != @RequiredDBVersion)
begin
	RAISERROR(N'DB version is "%s", required "%s".', 16, 1, @DBCurrentVersion, @RequiredDBVersion)
	set noexec on
end

GO

IF OBJECT_ID ('[dbo].[Get_Statement]') IS NOT NULL
	DROP PROCEDURE [dbo].[Get_Statement]
GO

/****** Object:  StoredProcedure [dbo].[DeleteDCFFairValueData]    Script Date: 08/07/2012 17:24:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_Statement](
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
FROM dbo.PERIOD_FINANCIALS pf
WHERE pf.ISSUER_ID = @ISSUER_ID
	AND pf.DATA_SOURCE = @DATA_SOURCE
	AND LEFT(pf.PERIOD_TYPE,1) = @PERIOD_TYPE
	AND pf.FISCAL_TYPE = @FISCAL_TYPE
	AND pf.CURRENCY = @CURRENCY
	

SELECT pfd.GROUP_NAME						AS [GroupName]
	,  pfd.DATA_ID							AS [DataId]
	,  pfd.BOLD_FONT						AS [BoldFont]
	,  pfd.DECIMALS							AS [Decimals]
	,  pf.ROOT_SOURCE						AS [RootSource]
	,  pf.ROOT_SOURCE_DATE					AS [RootSourceDate]
	,  pf.CALCULATION_DIAGRAM				AS [CalculationDiagram]
	,  pfd.DATA_DESC						AS [Description]
	,  pfd.SORT_ORDER						AS [SortOrder]
	,  ISNULL(pf.PERIOD_YEAR,2300)			AS [PeriodYear]
	,  ISNULL(pf.PERIOD_TYPE,'E')			AS [PeriodType]
	,  pf.AMOUNT * CASE 
	   WHEN (pfd.MULTIPLIER = 0.0) THEN 1.0 
	   ELSE pfd.MULTIPLIER END				AS [Amount]
	,  ISNULL(pf.AMOUNT_TYPE,'ESTIMATE')	AS [AmountType]
	,  'N'									AS [IsConsensus]
	
	INTO #STATEMENTS_DATA
	from PERIOD_FINANCIALS_DISPLAY pfd
	
Left Outer JOIN #PERIOD_FINANCIALS_DATA pf 
	ON pf.DATA_ID = pfd.DATA_ID

WHERE pfd.STATEMENT_TYPE = @STATEMENT_TYPE
AND pfd.COA_TYPE=@COA_TYPE
ORDER BY pfd.SORT_ORDER, PERIOD_YEAR

--------------------------------------------------------------------------------------
-- Select the data for the external research grid
--------------------------------------------------------------------------------------
DECLARE @EARNINGS VARCHAR(10)

SELECT @EARNINGS = CI.Earnings
FROM [Reuters].[dbo].[tblCompanyInfo] CI
INNER JOIN [dbo].[GF_SECURITY_BASEVIEW] GSB
	ON GSB.XREF = CI.XRef
WHERE GSB.ISSUER_ID = @ISSUER_ID

select *
INTO #EXTERNAL_ACTUAL_DATA
from CURRENT_CONSENSUS_ESTIMATES CCE
where  CCE.ISSUER_ID = @ISSUER_ID
	   AND CCE.DATA_SOURCE = 'REUTERS'
	   AND LEFT(CCE.PERIOD_TYPE, 1) = @PERIOD_TYPE
	   AND CCE.FISCAL_TYPE = @FISCAL_TYPE
	   AND CCE.CURRENCY = @CURRENCY
	   AND CCE.AMOUNT_TYPE = 'ACTUAL'



SELECT ''							AS [GroupName]
	, CCE.ESTIMATE_ID				AS [DataId]
	, 'N'							AS [BoldFont]
	, 0								AS [Decimals]
	, CCE.DATA_SOURCE   			AS [RootSource]
	, CCE.DATA_SOURCE_DATE			AS [RootSourceDate]
	, ''							AS [CalculationDiagram]
	, SCM.[DESCRIPTION]				AS [Description]
	, SCM.SORT_ORDER				AS [SortOrder]
	, ISNULL(CCE.PERIOD_YEAR,2300)	AS [PeriodYear]
	, ISNULL(CCE.PERIOD_TYPE,'A')	AS [PeriodType]
	, CCE.AMOUNT			AS [Amount]
	, CCE.AMOUNT_TYPE		AS [AmountType]
	, 'Y'					AS [IsConsensus]
INTO #CONSENSUS_ACTUAL_DATA
FROM dbo.STATEMENT_CONSENSUS_MAPPING SCM
LEFT JOIN #EXTERNAL_ACTUAL_DATA CCE 
	ON CCE.ESTIMATE_ID = SCM.ESTIMATE_ID
WHERE SCM.STATEMENT_TYPE = @STATEMENT_TYPE
	  AND (SCM.EARNINGS = @EARNINGS OR SCM.EARNINGS IS NULL)


SELECT * 
INTO #EXTERNAL_ESTIMATE_DATA 
FROM CURRENT_CONSENSUS_ESTIMATES CCE
WHERE CCE.ISSUER_ID = @ISSUER_ID
	  AND CCE.DATA_SOURCE = 'REUTERS'
	  AND LEFT(CCE.PERIOD_TYPE, 1) = @PERIOD_TYPE
	  AND CCE.FISCAL_TYPE = @FISCAL_TYPE
	  AND CCE.CURRENCY = @CURRENCY
	  AND CCE.AMOUNT_TYPE = 'ESTIMATE'



SELECT ''							AS [GroupName]
	, CCE.ESTIMATE_ID				AS [DataId]
	, 'N'							AS [BoldFont]
	, 0								AS [Decimals]
	, CCE.DATA_SOURCE   			AS [RootSource]
	, CCE.DATA_SOURCE_DATE			AS [RootSourceDate]
	, ''							AS [CalculationDiagram]
	, SCM.[DESCRIPTION]				AS [Description]
	, SCM.SORT_ORDER				AS [SortOrder]
	, ISNULL(CCE.PERIOD_YEAR,2300)	AS [PeriodYear]
	, ISNULL(CCE.PERIOD_TYPE,'E')	AS [PeriodType]
	, CCE.AMOUNT					AS [Amount]
	, CCE.AMOUNT_TYPE				AS [AmountType]
	, 'Y'							AS [IsConsensus]
INTO #CONSENSUS_ESTIMATE_DATA
FROM dbo.STATEMENT_CONSENSUS_MAPPING SCM
LEFT JOIN #EXTERNAL_ESTIMATE_DATA CCE 
	ON SCM.ESTIMATE_ID = CCE.ESTIMATE_ID

WHERE SCM.STATEMENT_TYPE = @STATEMENT_TYPE
	  AND (SCM.EARNINGS = @EARNINGS OR SCM.EARNINGS IS NULL)
	  AND CCE.PERIOD_YEAR NOT IN (SELECT DISTINCT [PeriodYear] FROM #CONSENSUS_ACTUAL_DATA)	

SELECT * FROM #STATEMENTS_DATA
UNION
SELECT * FROM #CONSENSUS_ACTUAL_DATA
UNION
SELECT * FROM #CONSENSUS_ESTIMATE_DATA
ORDER BY SortOrder, PeriodYear


DROP TABLE #PERIOD_FINANCIALS_DATA
DROP TABLE #STATEMENTS_DATA
DROP TABLE #CONSENSUS_ACTUAL_DATA
DROP TABLE #CONSENSUS_ESTIMATE_DATA
DROP TABLE #EXTERNAL_ESTIMATE_DATA
DROP TABLE #EXTERNAL_ACTUAL_DATA
Go

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00140'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())

