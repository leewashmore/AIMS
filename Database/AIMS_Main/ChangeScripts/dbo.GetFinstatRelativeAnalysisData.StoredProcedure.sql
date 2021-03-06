SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[GetFinstatRelativeAnalysisData] 
@issuerID varchar(20),
@securityId varchar(20),
@dataSource varchar(10),
@fiscalType char(8)
	
AS
BEGIN
DECLARE @countryCode nvarchar(255),
	    @benchmarkId nvarchar(255),
	    @gicsIndustry nvarchar(255)

---------------------------------STEP 1
Select p.DATA_ID,p.PERIOD_YEAR,p.AMOUNT,f.DECIMALS,f.MULTIPLIER,f.PERCENTAGE,'step1' as VALUE
INTO #A
From (Select * from PERIOD_FINANCIALS 
Where( ISSUER_ID = @issuerID  or SECURITY_ID = @securityId)
and DATA_SOURCE = 'PRIMARY'
and PERIOD_TYPE = 'A'
and FISCAL_TYPE = @fiscalType
and CURRENCY = 'USD'
AND DATA_ID IN(44,166,164,133))p

INNER JOIN
(Select *
from FINSTAT_DISPLAY
where COA_TYPE = (Select COA_TYPE from dbo.INTERNAL_ISSUER where ISSUER_ID = @issuerID)
AND DATA_ID IN (44,166,164,133)) f 
ON p.DATA_ID = f.DATA_ID


----------------------------------STEP 2
--Modified 06/18/13 (JM) to pull from PRIMARY and REUTERS instead of from raw consensus data
/*
--ACTUAL DATA
Select ESTIMATE_ID,PERIOD_YEAR,AMOUNT into #CCE_Actual 
From CURRENT_CONSENSUS_ESTIMATES 
Where( ISSUER_ID = @issuerID  or SECURITY_ID = @securityId) 
and DATA_SOURCE = 'REUTERS'
and PERIOD_TYPE = 'A'
and FISCAL_TYPE = @fiscalType
and CURRENCY = 'USD'
and AMOUNT_TYPE = 'ACTUAL'
and ESTIMATE_ID IN (11,166,164,19)
ORDER BY PERIOD_YEAR;

--ESTIMATE DATA
Select ESTIMATE_ID,PERIOD_YEAR,AMOUNT into #CCE_Estimate
From CURRENT_CONSENSUS_ESTIMATES 
Where( ISSUER_ID = @issuerID  or SECURITY_ID = @securityId) 
and DATA_SOURCE = 'REUTERS'
and PERIOD_TYPE = 'A'
and FISCAL_TYPE = @fiscalType
and CURRENCY = 'USD'
and AMOUNT_TYPE = 'ESTIMATE'
and ESTIMATE_ID IN (11,166,164,19)
and PERIOD_YEAR not in (select PERIOD_YEAR from #CCE_Actual) 
order by PERIOD_YEAR;

 --ACTUAL DATA AND ESTIMATE DATA COMBINED
 Select * into #CCEDATA 
 from #CCE_Estimate cce union (select * from #CCE_Actual cca)
 order by PERIOD_YEAR;
 
 --JOIN CCEDATA WITH COA SPECIFIC DATA
 Select cce.*,f.DECIMALS,f.MULTIPLIER,f.PERCENTAGE,'step2' as VALUE 
 INTO #B
 from #CCEDATA cce
 INNER JOIN
(Select * from FINSTAT_DISPLAY
where COA_TYPE = (Select COA_TYPE from dbo.INTERNAL_ISSUER where ISSUER_ID = @issuerID)
AND ESTIMATE_ID IN (11,166,164,19)) f 
ON cce.ESTIMATE_ID = f.ESTIMATE_ID;
*/

Select p.DATA_ID,p.PERIOD_YEAR,p.AMOUNT,f.DECIMALS,f.MULTIPLIER,f.PERCENTAGE,'step2' as VALUE
INTO #B
From (Select * from PERIOD_FINANCIALS 
Where( ISSUER_ID = @issuerID  or SECURITY_ID = @securityId)
and DATA_SOURCE = 'REUTERS'
and PERIOD_TYPE = 'A'
and FISCAL_TYPE = @fiscalType
and CURRENCY = 'USD'
AND DATA_ID IN(44,166,164,133))p

INNER JOIN
(Select *
from FINSTAT_DISPLAY
where COA_TYPE = (Select COA_TYPE from dbo.INTERNAL_ISSUER where ISSUER_ID = @issuerID)
AND DATA_ID IN (44,166,164,133)) f 
ON p.DATA_ID = f.DATA_ID

 
 -------------------------------------STEP 3
 --Modified 06/18/13 (JM) to disbale country and Industry data until bench node financial work complete
/* SET @countryCode = (Select DISTINCT ISO_COUNTRY_CODE from GF_SECURITY_BASEVIEW 
						where SECURITY_ID = @securityId);
			
Select b.*,f.DECIMALS,f.MULTIPLIER,f.PERCENTAGE,'step3' as VALUE
INTO #C
from 
(Select DATA_ID,PERIOD_YEAR,AMOUNT from BENCHMARK_NODE_FINANCIALS
where BENCHMARK_ID = 'MSCI EM NET'
AND NODE_NAME1 = 'COUNTRY'
and NODE_ID1 = @countryCode
and (NODE_NAME2 IS NULL OR NODE_NAME2 = '')
and DATA_ID IN (166,164,133)
and PERIOD_TYPE = 'A'
and CURRENCY = 'USD') b

INNER JOIN
(Select *
from FINSTAT_DISPLAY
where COA_TYPE = (Select COA_TYPE from dbo.INTERNAL_ISSUER where ISSUER_ID = @issuerID)
AND DATA_ID IN (44,166,164,133)) f 
ON b.DATA_ID = f.DATA_ID

---------------------------------------STEP 5
 SET @gicsIndustry = (Select DISTINCT GICS_INDUSTRY from GF_SECURITY_BASEVIEW 
						where SECURITY_ID = @securityId);

Select b.*,f.DECIMALS,f.MULTIPLIER,f.PERCENTAGE,'step5' as VALUE 
INTO #D
from 
(Select DATA_ID,PERIOD_YEAR,AMOUNT from BENCHMARK_NODE_FINANCIALS
where BENCHMARK_ID = 'MSCI EM NET'
AND NODE_NAME1 = 'INDUSTRY'
and NODE_ID1 = @gicsIndustry
and (NODE_NAME2 IS NULL OR NODE_NAME2 = '') 
and DATA_ID IN (166,164,133)
and PERIOD_TYPE = 'A'
and CURRENCY = 'USD') b

INNER JOIN
(Select *
from FINSTAT_DISPLAY
where COA_TYPE = (Select COA_TYPE from dbo.INTERNAL_ISSUER where ISSUER_ID = @issuerID)
AND DATA_ID IN (44,166,164,133)) f 
ON b.DATA_ID = f.DATA_ID
*/

--------------------------------UNION ALL DATA SETS
Select * from #A Union 
(Select * from #B) /*Union 
(Select * from #C) Union 
(Select * from #D)*/
order by VALUE
  
  -----------------------------DROP ALL TEMPORARY TABLES
 drop table /*#CCEDATA,#CCE_Actual,#CCE_Estimate,*/ #A,#B --,#C,#D; 

END
GO
