set noexec off

--declare  current and required version
--also do it an the end of the script
declare @RequiredDBVersion as nvarchar(100) = '00283'
declare @CurrentScriptVersion as nvarchar(100) = '00284'

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
            ------------------------------------------------------------------------
-- Purpose:	This procedure calculates the value for DATA_ID:272 Trading Income as % of Revenue  
--
--			(SNII-NFAC)/(SNII+SIIB)
--
-- Author:	Anupriya
-- Date:	August 17, 2012

------------------------------------------------------------------------
IF OBJECT_ID ( 'AIMS_Calc_272', 'P' ) IS NOT NULL 
DROP PROCEDURE AIMS_Calc_272;
GO

CREATE procedure [dbo].[AIMS_Calc_272](
	@ISSUER_ID			varchar(20) = NULL			-- The company identifier		
,	@CALC_LOG			char		= 'Y'			-- write calculation errors to the log table
)
as

	-- Get the data
	select pf.* 
	  into #A
	  from dbo.PERIOD_FINANCIALS pf  
	  where DATA_ID = 17		-- ENII (Net Interest Income)
	   and pf.ISSUER_ID = @ISSUER_ID
	   and pf.PERIOD_TYPE = 'A'

	select pf.* 
	  into #B
	  from dbo.PERIOD_FINANCIALS  pf 
	 where DATA_ID = 264		-- NFAC (Fees & Commissions from Operations)
	   and pf.ISSUER_ID = @ISSUER_ID
	   and pf.PERIOD_TYPE = 'A'
	   
	 
	select pf.* 
	  into #C
	  from dbo.PERIOD_FINANCIALS  pf 
	 where DATA_ID = 33			-- SNII	(Non-Interest Income, Bank)   
	   and pf.ISSUER_ID = @ISSUER_ID
	   and pf.PERIOD_TYPE = 'A'  
	   
	   

	-- Add the data to the table
	insert into PERIOD_FINANCIALS(ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
		  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
		  , DATA_ID, AMOUNT, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE)
	select a.ISSUER_ID, a.SECURITY_ID, a.COA_TYPE, a.DATA_SOURCE, a.ROOT_SOURCE
		,  a.ROOT_SOURCE_DATE, a.PERIOD_TYPE, a.PERIOD_YEAR, a.PERIOD_END_DATE
		,  a.FISCAL_TYPE, a.CURRENCY
		,  272 as DATA_ID										-- DATA_ID:272 Trading Income as % of Revenue  
		,  (a.AMOUNT + b.AMOUNT)/(a.AMOUNT + c.AMOUNT) as AMOUNT			-- (17 + 264)/(17 + 33)
		,  '(ENII(' + CAST(a.AMOUNT as varchar(32)) + ') +  NFAC(' + CAST(b.AMOUNT as varchar(32)) + ')) / ( ENII(' + CAST(a.AMOUNT as varchar(32)) + ')' + 'SNII(' + CAST(c.AMOUNT as varchar(32)) + '))' as CALCULATION_DIAGRAM
		,  a.SOURCE_CURRENCY
		,  a.AMOUNT_TYPE
	  from #A a
	 inner join	#B b on b.ISSUER_ID = a.ISSUER_ID and b.DATA_SOURCE = a.DATA_SOURCE 
					and b.PERIOD_TYPE = a.PERIOD_TYPE and b.PERIOD_YEAR = a.PERIOD_YEAR 
					and b.FISCAL_TYPE = a.FISCAL_TYPE and b.CURRENCY = a.CURRENCY
	 inner join	#C c on c.ISSUER_ID = a.ISSUER_ID and c.DATA_SOURCE = a.DATA_SOURCE 
	                and c.PERIOD_TYPE = a.PERIOD_TYPE and c.PERIOD_YEAR = a.PERIOD_YEAR 
					and c.FISCAL_TYPE = a.FISCAL_TYPE and c.CURRENCY = a.CURRENCY	
	 where 1=1 
	   and isnull((a.AMOUNT + c.AMOUNT), 0.0) <> 0.0	-- Data validation
--	 order by a.ISSUER_ID, a.COA_TYPE, a.DATA_SOURCE, a.PERIOD_TYPE, a.PERIOD_YEAR,  a.FISCAL_TYPE, a.CURRENCY

	
	if @CALC_LOG = 'Y'
		BEGIN	
			-- Error conditions - NULL or Zero data 
			insert into dbo.CALC_LOG( LOG_DATE, DATA_ID, ISSUER_ID, PERIOD_TYPE, PERIOD_YEAR
							, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY, TXT )
			(
			select GETDATE() as LOG_DATE, 272 as DATA_ID, a.ISSUER_ID, a.PERIOD_TYPE
				,  a.PERIOD_YEAR, a.PERIOD_END_DATE, a.FISCAL_TYPE, a.CURRENCY
				, 'ERROR calculating 272 Trading Income as % of Revenue.  DATA_ID:17 ENII is NULL or ZERO'
			  from #A a
			  inner join #C c on c.ISSUER_ID = a.ISSUER_ID and c.DATA_SOURCE = a.DATA_SOURCE 
							and c.PERIOD_TYPE = a.PERIOD_TYPE and c.PERIOD_YEAR = a.PERIOD_YEAR 
							and c.FISCAL_TYPE = a.FISCAL_TYPE and c.CURRENCY = a.CURRENCY	
			 where isnull((a.AMOUNT + c.AMOUNT), 0.0) = 0.0	-- Data error
			 and a.PERIOD_TYPE = 'A'
			) union (

			-- Error conditions - missing data 
			select GETDATE() as LOG_DATE, 272 as DATA_ID, a.ISSUER_ID, a.PERIOD_TYPE
				,  a.PERIOD_YEAR,  a.PERIOD_END_DATE,  a.FISCAL_TYPE,  a.CURRENCY
				, 'ERROR calculating 272 Trading Income as % of Revenue  DATA_ID: 264 NFAC is missing' as TXT
			  from #B a
			  left join	#A b on b.ISSUER_ID = a.ISSUER_ID 
							and b.DATA_SOURCE = a.DATA_SOURCE and b.PERIOD_TYPE = a.PERIOD_TYPE
							and b.PERIOD_YEAR = a.PERIOD_YEAR and b.FISCAL_TYPE = a.FISCAL_TYPE
							and b.CURRENCY = a.CURRENCY
			 where 1=1 and b.ISSUER_ID is NULL
			   and a.PERIOD_TYPE = 'A'
			) union (	
			
			select GETDATE() as LOG_DATE, 272 as DATA_ID, a.ISSUER_ID, a.PERIOD_TYPE
				,  a.PERIOD_YEAR,  a.PERIOD_END_DATE,  a.FISCAL_TYPE,  a.CURRENCY
				, 'ERROR calculating 272 Trading Income as % of Revenue  DATA_ID:33 SNII is missing' as TXT
			  from #C a
			  left join	#A b on b.ISSUER_ID = a.ISSUER_ID 
							and b.DATA_SOURCE = a.DATA_SOURCE and b.PERIOD_TYPE = a.PERIOD_TYPE
							and b.PERIOD_YEAR = a.PERIOD_YEAR and b.FISCAL_TYPE = a.FISCAL_TYPE
							and b.CURRENCY = a.CURRENCY
			 where 1=1 and b.ISSUER_ID is NULL
			   and a.PERIOD_TYPE = 'A'
			) union (	
			
			-- Error conditions - missing data 
			select GETDATE() as LOG_DATE, 272 as DATA_ID, a.ISSUER_ID, a.PERIOD_TYPE
				,  a.PERIOD_YEAR, a.PERIOD_END_DATE, a.FISCAL_TYPE, a.CURRENCY
				, 'ERROR calculating 272 Trading Income as % of Revenue.  DATA_ID:17 ENII is missing' as TXT
			  from #A a
			  left join	#B b on b.ISSUER_ID = a.ISSUER_ID  
							and b.DATA_SOURCE = a.DATA_SOURCE and b.PERIOD_TYPE = a.PERIOD_TYPE
							and b.PERIOD_YEAR = a.PERIOD_YEAR and b.FISCAL_TYPE = a.FISCAL_TYPE
							and b.CURRENCY = a.CURRENCY
			 where 1=1 and b.ISSUER_ID is NULL
			   and a.PERIOD_TYPE = 'A'
			) union (	
			
			 
			 
			-- ERROR - No data at all available
			select GETDATE() as LOG_DATE, 272 as DATA_ID, isnull(@ISSUER_ID, ' ') as ISSUER_ID, ' ' as PERIOD_TYPE
				,  0 as PERIOD_YEAR,  '1/1/1900' as PERIOD_END_DATE,  ' ' as FISCAL_TYPE,  ' ' as CURRENCY
				, 'ERROR calculating 272 Trading Income as % of Revenue.  DATA_ID:17 ENII no data' as TXT
			  from (select COUNT(*) CNT from #A having COUNT(*) = 0) z
			) union (	

			select GETDATE() as LOG_DATE, 272 as DATA_ID, isnull(@ISSUER_ID, ' ') as ISSUER_ID, ' ' as PERIOD_TYPE
				,  0 as PERIOD_YEAR,  '1/1/1900' as PERIOD_END_DATE,  ' ' as FISCAL_TYPE,  ' ' as CURRENCY
				, 'ERROR calculating 272 Trading Income as % of Revenue.  DATA_ID:264 NFAC no data' as TXT
			  from (select COUNT(*) CNT from #B having COUNT(*) = 0) z
			)
			 union (	

			select GETDATE() as LOG_DATE, 272 as DATA_ID, isnull(@ISSUER_ID, ' ') as ISSUER_ID, ' ' as PERIOD_TYPE
				,  0 as PERIOD_YEAR,  '1/1/1900' as PERIOD_END_DATE,  ' ' as FISCAL_TYPE,  ' ' as CURRENCY
				, 'ERROR calculating 272 Trading Income as % of Revenue  DATA_ID:33 SNII no data' as TXT
			  from (select COUNT(*) CNT from #C having COUNT(*) = 0) z
			)
		END

	-- Clean up
	drop table #A
	drop table #B
	drop table #C

GO



--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00284'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())