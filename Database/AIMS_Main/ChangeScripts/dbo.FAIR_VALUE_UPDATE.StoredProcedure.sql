SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
------------------------------------------------------------------------
-- Purpose:	This procedure updates the FAIR_VALUES table 
--
-- Author:	David Muench
-- Date:	August 12, 2012
------------------------------------------------------------------------
alter procedure [dbo].[FAIR_VALUE_UPDATE] (
	@ISSUER_ID			varchar(20) = NULL		-- The company identifier		
,	@VALUE_TYPE			varchar(20) = NULL		-- Default to update all datasources
,	@VERBOSE			char		= 'Y'	
)
as


--Modified 9/12/13 (JM) to clear current measure value and upside before re-calculating as numbers where remaining stale when a value for the measure was no longer in period_financials
update dbo.FAIR_VALUE
set CURRENT_MEASURE_VALUE = 0
	,  UPSIDE = 0
where SECURITY_ID in (select SECURITY_ID from GF_SECURITY_BASEVIEW where ISSUER_ID = @ISSUER_ID)



--Modified 6/25/13 (JM) to calculate Dividend Yield upside inversely and adjusted by 100(amount/sell...vs sell/amount)
	--Non-DY
	
	/*Modified 04/08/2014-Akhtar. The issuer information will be either in main table or stage table.. But I am just running the update by querying from both table.
	 Will think of adding if statement later based on whether the the issuer information is in stage or main table. 
	 
	 */
	
	--- Query from PERIOD_FINANCIALS_MAIN  and update
	update dbo.FAIR_VALUE
	   set CURRENT_MEASURE_VALUE = pf.AMOUNT
		,  UPSIDE = (fv.FV_SELL / pf.AMOUNT)-1
	  FROM dbo.FAIR_VALUE fv 
	 inner join dbo.PERIOD_FINANCIALS_SECURITY_MAIN pf on pf.SECURITY_ID = fv.SECURITY_ID and pf.DATA_ID = fv.FV_MEASURE
	 inner join dbo.GF_SECURITY_BASEVIEW sb on sb.SECURITY_ID = pf.SECURITY_ID
	 where (@ISSUER_ID is NULL or sb.ISSUER_ID = @ISSUER_ID)
	   and (@VALUE_TYPE is NULL or fv.VALUE_TYPE = @VALUE_TYPE)
	   and pf.CURRENCY = 'USD'
	   --and pf.PERIOD_TYPE = 'C'
	   and pf.PERIOD_TYPE = isnull(fv.period_type,'C')
	   and pf.PERIOD_YEAR = isnull(fv.period_year,0)
	   and pf.FISCAL_TYPE =  case when fv.period_type is null then '' when fv.PERIOD_TYPE = 'C' then '' else 'CALENDAR' end
	   and (   (fv.VALUE_TYPE <> 'INDUSTRY' and pf.DATA_SOURCE = 'PRIMARY')
		    or (fv.VALUE_TYPE =  'INDUSTRY' and pf.DATA_SOURCE = 'INDUSTRY') )
	   and isnull(pf.AMOUNT, 0.0) <> 0.0
	   and fv.fv_measure <> 236

	--DY
	update dbo.FAIR_VALUE
	   set CURRENT_MEASURE_VALUE = pf.AMOUNT
		,  UPSIDE = (pf.AMOUNT/(fv.FV_SELL/100))-1
	  FROM dbo.FAIR_VALUE fv 
	 inner join dbo.PERIOD_FINANCIALS_SECURITY_MAIN pf on pf.SECURITY_ID = fv.SECURITY_ID and pf.DATA_ID = fv.FV_MEASURE
	 inner join dbo.GF_SECURITY_BASEVIEW sb on sb.SECURITY_ID = pf.SECURITY_ID
	 where (@ISSUER_ID is NULL or sb.ISSUER_ID = @ISSUER_ID)
	   and (@VALUE_TYPE is NULL or fv.VALUE_TYPE = @VALUE_TYPE)
	   and pf.CURRENCY = 'USD'
	   --and pf.PERIOD_TYPE = 'C'
	   and pf.PERIOD_TYPE = isnull(fv.period_type,'C')
	   and pf.PERIOD_YEAR = isnull(fv.period_year,0)
	   and pf.FISCAL_TYPE =  case when fv.period_type is null then '' when fv.PERIOD_TYPE = 'C' then '' else 'CALENDAR' end
	   and (   (fv.VALUE_TYPE <> 'INDUSTRY' and pf.DATA_SOURCE = 'PRIMARY')
		    or (fv.VALUE_TYPE =  'INDUSTRY' and pf.DATA_SOURCE = 'INDUSTRY') )
	   and isnull(pf.AMOUNT, 0.0) <> 0.0
	   and fv.fv_measure = 236
	   
	   
	   
	   -- QUery From PERIOD_FINANCIALS_STAGE and update 
	   
	   	update dbo.FAIR_VALUE
	   set CURRENT_MEASURE_VALUE = pf.AMOUNT
		,  UPSIDE = (fv.FV_SELL / pf.AMOUNT)-1
	  FROM dbo.FAIR_VALUE fv 
	 inner join dbo.PERIOD_FINANCIALS_SECURITY_STAGE pf on pf.SECURITY_ID = fv.SECURITY_ID and pf.DATA_ID = fv.FV_MEASURE
	 inner join dbo.GF_SECURITY_BASEVIEW sb on sb.SECURITY_ID = pf.SECURITY_ID
	 where (@ISSUER_ID is NULL or sb.ISSUER_ID = @ISSUER_ID)
	   and (@VALUE_TYPE is NULL or fv.VALUE_TYPE = @VALUE_TYPE)
	   and pf.CURRENCY = 'USD'
	   --and pf.PERIOD_TYPE = 'C'
	   and pf.PERIOD_TYPE = isnull(fv.period_type,'C')
	   and pf.PERIOD_YEAR = isnull(fv.period_year,0)
	   and pf.FISCAL_TYPE =  case when fv.period_type is null then '' when fv.PERIOD_TYPE = 'C' then '' else 'CALENDAR' end
	   and (   (fv.VALUE_TYPE <> 'INDUSTRY' and pf.DATA_SOURCE = 'PRIMARY')
		    or (fv.VALUE_TYPE =  'INDUSTRY' and pf.DATA_SOURCE = 'INDUSTRY') )
	   and isnull(pf.AMOUNT, 0.0) <> 0.0
	   and fv.fv_measure <> 236

	--DY
	update dbo.FAIR_VALUE
	   set CURRENT_MEASURE_VALUE = pf.AMOUNT
		,  UPSIDE = (pf.AMOUNT/(fv.FV_SELL/100))-1
	  FROM dbo.FAIR_VALUE fv 
	 inner join dbo.PERIOD_FINANCIALS_SECURITY_STAGE pf on pf.SECURITY_ID = fv.SECURITY_ID and pf.DATA_ID = fv.FV_MEASURE
	 inner join dbo.GF_SECURITY_BASEVIEW sb on sb.SECURITY_ID = pf.SECURITY_ID
	 where (@ISSUER_ID is NULL or sb.ISSUER_ID = @ISSUER_ID)
	   and (@VALUE_TYPE is NULL or fv.VALUE_TYPE = @VALUE_TYPE)
	   and pf.CURRENCY = 'USD'
	   --and pf.PERIOD_TYPE = 'C'
	   and pf.PERIOD_TYPE = isnull(fv.period_type,'C')
	   and pf.PERIOD_YEAR = isnull(fv.period_year,0)
	   and pf.FISCAL_TYPE =  case when fv.period_type is null then '' when fv.PERIOD_TYPE = 'C' then '' else 'CALENDAR' end
	   and (   (fv.VALUE_TYPE <> 'INDUSTRY' and pf.DATA_SOURCE = 'PRIMARY')
		    or (fv.VALUE_TYPE =  'INDUSTRY' and pf.DATA_SOURCE = 'INDUSTRY') )
	   and isnull(pf.AMOUNT, 0.0) <> 0.0
	   and fv.fv_measure = 236


GO
