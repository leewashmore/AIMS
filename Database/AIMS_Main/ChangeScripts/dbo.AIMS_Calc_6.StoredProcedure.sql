IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AIMS_Calc_6]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AIMS_Calc_6]
GO

/****** Object:  StoredProcedure [dbo].[AIMS_Calc_6]    Script Date: 05/01/2013 14:06:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

create procedure [dbo].[AIMS_Calc_6](

	@ISSUER_ID			varchar(20) = NULL			-- The company identifier		
,	@CALC_LOG			char		= 'Y'			-- write calculation errors to the log table
,	@stage				char		= 'N'
)
as

	-- Get the data
	
	select pf.* 
	  into #A
	  from dbo.PERIOD_FINANCIALS_ISSUER_STAGE pf with (nolock)
	  where 1=0
	  
	select pf.* 
	  into #B
	  from dbo.PERIOD_FINANCIALS_ISSUER_STAGE pf with (nolock)
	  where 1=0
	
	if @stage = 'Y'
	begin
			insert into #A
		select pf.* 
		  
		  from dbo.PERIOD_FINANCIALS_ISSUER_STAGE pf with (nolock)
		 where DATA_ID = 4				-- SPRE
		   and pf.ISSUER_ID = @ISSUER_ID
		   and pf.PERIOD_TYPE = 'A'
		
		insert into #B
		select pf.* 
		  
		  from dbo.PERIOD_FINANCIALS_ISSUER_STAGE pf with (nolock)
		 where DATA_ID = 288					-- RPRW
		   and pf.ISSUER_ID = @ISSUER_ID
		   and pf.PERIOD_TYPE = 'A'
	end
	else
	begin
	
			insert into #A
			select pf.* 
			  
			  from dbo.PERIOD_FINANCIALS_ISSUER_MAIN pf with (nolock)
			 where DATA_ID = 4				-- SPRE
			   and pf.ISSUER_ID = @ISSUER_ID
			   and pf.PERIOD_TYPE = 'A'

			insert into #B
			select pf.* 
			  
			  from dbo.PERIOD_FINANCIALS_ISSUER_MAIN pf with (nolock)
			 where DATA_ID = 288					-- RPRW
			   and pf.ISSUER_ID = @ISSUER_ID
			   and pf.PERIOD_TYPE = 'A'
  end
	-- Add the data to the table
	BEGIN TRAN T1
	if @stage = 'Y'
	begin
		insert into PERIOD_FINANCIALS_ISSUER_STAGE(ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
		  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
		  , DATA_ID, AMOUNT, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE)
	select a.ISSUER_ID, a.SECURITY_ID, a.COA_TYPE, a.DATA_SOURCE, a.ROOT_SOURCE
		,  a.ROOT_SOURCE_DATE, a.PERIOD_TYPE, a.PERIOD_YEAR, a.PERIOD_END_DATE
		,  a.FISCAL_TYPE, a.CURRENCY
		,  6 as DATA_ID										-- DATA_ID:6 Retention Ratio 
		,  a.AMOUNT /  b.AMOUNT as AMOUNT			            -- SPRE/RPRW
		,  'SPRE(' + CAST(a.AMOUNT as varchar(32)) + ') /  RPRW(' + CAST(b.AMOUNT as varchar(32)) + ') ' as CALCULATION_DIAGRAM
		,  a.SOURCE_CURRENCY
		,  a.AMOUNT_TYPE
	  from #A a
	 inner join	#B b on b.ISSUER_ID = a.ISSUER_ID 
					and b.DATA_SOURCE = a.DATA_SOURCE and b.PERIOD_TYPE = a.PERIOD_TYPE
					and b.PERIOD_YEAR = a.PERIOD_YEAR and b.FISCAL_TYPE = a.FISCAL_TYPE
					and b.CURRENCY = a.CURRENCY
	 where 1=1 
	   and a.PERIOD_TYPE = 'A'
	   and isnull(b.AMOUNT, 0.0) <> 0.0
	end
	else
	begin
			insert into PERIOD_FINANCIALS_ISSUER_MAIN(ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
				  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
				  , DATA_ID, AMOUNT, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE)
			select a.ISSUER_ID, a.SECURITY_ID, a.COA_TYPE, a.DATA_SOURCE, a.ROOT_SOURCE
				,  a.ROOT_SOURCE_DATE, a.PERIOD_TYPE, a.PERIOD_YEAR, a.PERIOD_END_DATE
				,  a.FISCAL_TYPE, a.CURRENCY
				,  6 as DATA_ID										-- DATA_ID:6 Retention Ratio 
				,  a.AMOUNT /  b.AMOUNT as AMOUNT			            -- SPRE/RPRW
				,  'SPRE(' + CAST(a.AMOUNT as varchar(32)) + ') /  RPRW(' + CAST(b.AMOUNT as varchar(32)) + ') ' as CALCULATION_DIAGRAM
				,  a.SOURCE_CURRENCY
				,  a.AMOUNT_TYPE
			  from #A a
			 inner join	#B b on b.ISSUER_ID = a.ISSUER_ID 
							and b.DATA_SOURCE = a.DATA_SOURCE and b.PERIOD_TYPE = a.PERIOD_TYPE
							and b.PERIOD_YEAR = a.PERIOD_YEAR and b.FISCAL_TYPE = a.FISCAL_TYPE
							and b.CURRENCY = a.CURRENCY
			 where 1=1 
			   and a.PERIOD_TYPE = 'A'
			   and isnull(b.AMOUNT, 0.0) <> 0.0	-- Data validation
--	 order by a.ISSUER_ID, a.COA_TYPE, a.DATA_SOURCE, a.PERIOD_TYPE, a.PERIOD_YEAR,  a.FISCAL_TYPE, a.CURRENCY
end
	COMMIT TRAN T1

	if @CALC_LOG = 'Y'
		BEGIN	
			-- Error conditions - NULL or ZERO data
			insert into CALC_LOG( LOG_DATE, DATA_ID, ISSUER_ID, PERIOD_TYPE, PERIOD_YEAR
								, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY, TXT)
			(
			select GETDATE() as LOG_DATE, 6 as DATA_ID, a.ISSUER_ID, a.PERIOD_TYPE
				,  a.PERIOD_YEAR, a.PERIOD_END_DATE, a.FISCAL_TYPE, a.CURRENCY
				, 'ERROR calculating 6:Retention Ratio .  DATA_ID:288 RPRW is NULL or ZERO'
			  from #B a
			 where isnull(a.AMOUNT, 0.0) = 0.0	-- Data error
			  and a.PERIOD_TYPE = 'A'
			) union (
			
			-- Error conditions - missing data 
			select GETDATE() as LOG_DATE, 6 as DATA_ID, a.ISSUER_ID, a.PERIOD_TYPE
				,  a.PERIOD_YEAR, a.PERIOD_END_DATE, a.FISCAL_TYPE, a.CURRENCY
				, 'ERROR calculating 6:Retention Ratio .  DATA_ID:4 SPRE is missing'
			  from #A a
			  left join	#B b on b.ISSUER_ID = a.ISSUER_ID 
							and b.DATA_SOURCE = a.DATA_SOURCE and b.PERIOD_TYPE = a.PERIOD_TYPE
							and b.PERIOD_YEAR = a.PERIOD_YEAR and b.FISCAL_TYPE = a.FISCAL_TYPE
							and b.CURRENCY = a.CURRENCY
			 where 1=1 and b.ISSUER_ID is NULL
			  and a.PERIOD_TYPE = 'A'
			) union (

			-- Error conditions - missing data 
			select GETDATE() as LOG_DATE, 6 as DATA_ID, a.ISSUER_ID, a.PERIOD_TYPE
				,  a.PERIOD_YEAR,  a.PERIOD_END_DATE,  a.FISCAL_TYPE,  a.CURRENCY
				, 'ERROR calculating 6:Retention Ratio .  DATA_ID:288 RPRW is missing' as TXT
			  from #B a
			  left join	#A b on b.ISSUER_ID = a.ISSUER_ID 
							and b.DATA_SOURCE = a.DATA_SOURCE and b.PERIOD_TYPE = a.PERIOD_TYPE
							and b.PERIOD_YEAR = a.PERIOD_YEAR and b.FISCAL_TYPE = a.FISCAL_TYPE
							and b.CURRENCY = a.CURRENCY
			 where 1=1 and b.ISSUER_ID is NULL
			  and a.PERIOD_TYPE = 'A'
			) union (
			 
				-- ERROR - No data at all available
			select GETDATE() as LOG_DATE, 6 as DATA_ID, isnull(@ISSUER_ID, ' ') as ISSUER_ID, ' ' as PERIOD_TYPE
				,  0 as PERIOD_YEAR,  '1/1/1900' as PERIOD_END_DATE,  ' ' as FISCAL_TYPE,  ' ' as CURRENCY
				, 'ERROR calculating 6:Retention Ratio .  DATA_ID:4 SPRE no data' as TXT
			  from (select COUNT(*) CNT from #A having COUNT(*) = 0) z
			) union (
			select GETDATE() as LOG_DATE, 6 as DATA_ID, isnull(@ISSUER_ID, ' ') as ISSUER_ID, ' ' as PERIOD_TYPE
				,  0 as PERIOD_YEAR,  '1/1/1900' as PERIOD_END_DATE,  ' ' as FISCAL_TYPE,  ' ' as CURRENCY
				, 'ERROR calculating 6:Retention Ratio .  DATA_ID:288 RPRW no data' as TXT
			  from (select COUNT(*) CNT from #B having COUNT(*) = 0) z
			) 
		END
		
	-- Clean up
	drop table #A
	drop table #B
GO
