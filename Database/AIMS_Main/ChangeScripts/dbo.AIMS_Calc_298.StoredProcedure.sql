IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AIMS_Calc_298]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AIMS_Calc_298]
GO

/****** Object:  StoredProcedure [dbo].[AIMS_Calc_298]    Script Date: 05/01/2013 14:06:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

create procedure [dbo].[AIMS_Calc_298](
	@ISSUER_ID			varchar(20) = NULL			-- The company identifier		
,	@CALC_LOG			char		= 'Y'			-- write calculation errors to the log table
,	@STAGE				char		= 'N'
)
as

	select  ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
		  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
		  , DATA_ID, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE
		  ,  AMOUNT, AMOUNT as AMOUNT100
	  into #A
	  from dbo.PERIOD_FINANCIALS_ISSUER_STAGE pf  with (nolock)
	  where 1=0
	  
	  select  ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
		  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
		  , DATA_ID, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE
		  , AMOUNT
	  into #B
	  from dbo.PERIOD_FINANCIALS_ISSUER_STAGE pf  with (nolock)
	  where 1=0

	--Trailing Book Value (298)

	-- Get the data

	-- Calculate the percentage of the amount to use.
	declare @PERCENTAGE decimal(32,6)
	declare @PERIOD_END_DATE datetime
	declare @PERIOD_YEAR integer
	iF @stage = 'Y'
	BEGIN
		select @PERCENTAGE = cast(datediff(day, MAX(period_end_date),getdate()) as decimal(32,6)) / 365.0
		   ,   @PERIOD_END_DATE = MAX(period_end_date)
		  from PERIOD_FINANCIALS_ISSUER_STAGE with (nolock)
		 where ISSUER_ID = @ISSUER_ID
		   and DATA_ID = 104
		   and PERIOD_END_DATE < GETDATE()
		   and PERIOD_TYPE = 'A'


		select @PERIOD_YEAR = PERIOD_YEAR
		  from PERIOD_FINANCIALS_ISSUER_STAGE with (nolock)
		 where ISSUER_ID = @ISSUER_ID
		   and DATA_ID = 104
		   and PERIOD_END_DATE = @PERIOD_END_DATE
		   and PERIOD_TYPE = 'A'
		   
	   insert  into #A
		select distinct ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
			  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
			  , DATA_ID, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE
			  , (AMOUNT * (1-@PERCENTAGE)) as AMOUNT, AMOUNT as AMOUNT100
		 
		  from dbo.PERIOD_FINANCIALS_ISSUER_STAGE pf  with (nolock)
		 where DATA_ID = 104		-- Book Value
		   and pf.ISSUER_ID = @ISSUER_ID
		   and pf.PERIOD_TYPE = 'A'
		   and FISCAL_TYPE = 'FISCAL'
		   and pf.PERIOD_END_DATE = @PERIOD_END_DATE
		
		insert   into #B
		select distinct ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
			  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
			  , DATA_ID, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE
			  , (AMOUNT * @PERCENTAGE) as AMOUNT
		
		  from dbo.PERIOD_FINANCIALS_ISSUER_STAGE pf  with (nolock)
		 where DATA_ID = 104		-- Book Value
		   and pf.ISSUER_ID = @ISSUER_ID
		   and pf.PERIOD_TYPE = 'A'
		   and FISCAL_TYPE = 'FISCAL'
		   and pf.PERIOD_YEAR = @PERIOD_YEAR + 1
	END
	ELSE
	BEGIN
		select @PERCENTAGE = cast(datediff(day, MAX(period_end_date),getdate()) as decimal(32,6)) / 365.0
		   ,   @PERIOD_END_DATE = MAX(period_end_date)
		  from PERIOD_FINANCIALS_ISSUER_MAIN with (nolock)
		 where ISSUER_ID = @ISSUER_ID
		   and DATA_ID = 104
		   and PERIOD_END_DATE < GETDATE()
		   and PERIOD_TYPE = 'A'


		select @PERIOD_YEAR = PERIOD_YEAR
		  from PERIOD_FINANCIALS_ISSUER_MAIN with (nolock)
		 where ISSUER_ID = @ISSUER_ID
		   and DATA_ID = 104
		   and PERIOD_END_DATE = @PERIOD_END_DATE
		   and PERIOD_TYPE = 'A'
	   
	   insert 		  into #A
		select distinct ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
			  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
			  , DATA_ID, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE
			  , (AMOUNT * (1-@PERCENTAGE)) as AMOUNT, AMOUNT as AMOUNT100

		  from dbo.PERIOD_FINANCIALS_ISSUER_MAIN pf  with (nolock)
		 where DATA_ID = 104		-- Book Value
		   and pf.ISSUER_ID = @ISSUER_ID
		   and pf.PERIOD_TYPE = 'A'
		   and FISCAL_TYPE = 'FISCAL'
		   and pf.PERIOD_END_DATE = @PERIOD_END_DATE
		
		insert 		  into #B
		select distinct ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
			  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
			  , DATA_ID, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE
			  , (AMOUNT * @PERCENTAGE) as AMOUNT

		  from dbo.PERIOD_FINANCIALS_ISSUER_MAIN pf  with (nolock)
		 where DATA_ID = 104		-- Book Value
		   and pf.ISSUER_ID = @ISSUER_ID
		   and pf.PERIOD_TYPE = 'A'
		   and FISCAL_TYPE = 'FISCAL'
		   and pf.PERIOD_YEAR = @PERIOD_YEAR + 1	
	END
	-- Add the data to the table
	-- When dealing with 'C'urrent PERIOD_TYPE there should be only one value... the current one.  
	-- No PERIOD_YEAR not PERIOD_END_DATE is needed.
	BEGIN TRAN T1
	IF @STAGE = 'Y'
	BEGIN
	insert into PERIOD_FINANCIALS_ISSUER_STAGE(ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
		  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
		  , DATA_ID, AMOUNT, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE)
	select distinct a.ISSUER_ID, a.SECURITY_ID, a.COA_TYPE, a.DATA_SOURCE, a.ROOT_SOURCE
		,  a.ROOT_SOURCE_DATE, 'C', 0, '01/01/1900'				-- These are specific for PERIOD_TYPE = 'C'
		,  a.FISCAL_TYPE, a.CURRENCY
		,  298 as DATA_ID										-- DATA_ID: 298 Trailing Book Value
		,  case when b.AMOUNT is NULL then a.AMOUNT100 else (a.AMOUNT +b.AMOUNT) end as AMOUNT						
		,  case when b.AMOUNT is NULL then 'Book Value(' + CAST(a.AMOUNT100 as varchar(32)) + ')' 
				else 'Book Value(' + CAST(a.AMOUNT as varchar(32)) + ' + ' +CAST(b.AMOUNT as varchar(32)) + ')' end as CALCULATION_DIAGRAM
		,  a.SOURCE_CURRENCY
		,  a.AMOUNT_TYPE
	  from #A a
	  left join	#B b on b.ISSUER_ID = a.ISSUER_ID 					
					and b.CURRENCY = a.CURRENCY
					and b.DATA_SOURCE = a.DATA_SOURCE
	 where 1=1 	  
	   and isnull(a.AMOUNT, 0.0) <> 0.0	-- Data validation
--	   and isnull(c.AMOUNT, 0.0) <> 0.0	-- Data validation
	-- order by a.ISSUER_ID, a.COA_TYPE, a.DATA_SOURCE, a.PERIOD_TYPE, a.PERIOD_YEAR,  a.FISCAL_TYPE, a.CURRENCY
	END
	ELSE
	BEGIN
		insert into PERIOD_FINANCIALS_ISSUER_MAIN(ISSUER_ID, SECURITY_ID, COA_TYPE, DATA_SOURCE, ROOT_SOURCE
		  , ROOT_SOURCE_DATE, PERIOD_TYPE, PERIOD_YEAR, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY
		  , DATA_ID, AMOUNT, CALCULATION_DIAGRAM, SOURCE_CURRENCY, AMOUNT_TYPE)
	select distinct a.ISSUER_ID, a.SECURITY_ID, a.COA_TYPE, a.DATA_SOURCE, a.ROOT_SOURCE
		,  a.ROOT_SOURCE_DATE, 'C', 0, '01/01/1900'				-- These are specific for PERIOD_TYPE = 'C'
		,  a.FISCAL_TYPE, a.CURRENCY
		,  298 as DATA_ID										-- DATA_ID: 298 Trailing Book Value
		,  case when b.AMOUNT is NULL then a.AMOUNT100 else (a.AMOUNT +b.AMOUNT) end as AMOUNT						
		,  case when b.AMOUNT is NULL then 'Book Value(' + CAST(a.AMOUNT100 as varchar(32)) + ')' 
				else 'Book Value(' + CAST(a.AMOUNT as varchar(32)) + ' + ' +CAST(b.AMOUNT as varchar(32)) + ')' end as CALCULATION_DIAGRAM
		,  a.SOURCE_CURRENCY
		,  a.AMOUNT_TYPE
	  from #A a
	  left join	#B b on b.ISSUER_ID = a.ISSUER_ID 					
					and b.CURRENCY = a.CURRENCY
					and b.DATA_SOURCE = a.DATA_SOURCE
	 where 1=1 	  
	   and isnull(a.AMOUNT, 0.0) <> 0.0	-- Data validation
--	   and isnull(c.AMOUNT, 0.0) <> 0.0	-- Data validation
	-- order by a.ISSUER_ID, a.COA_TYPE, a.DATA_SOURCE, a.PERIOD_TYPE, a.PERIOD_YEAR,  a.FISCAL_TYPE, a.CURRENCY
	END

	COMMIT TRAN T1
	
	if @CALC_LOG = 'Y'
		BEGIN
			-- Error conditions - NULL or Zero data 
			insert into CALC_LOG( LOG_DATE, DATA_ID, ISSUER_ID, PERIOD_TYPE, PERIOD_YEAR
							, PERIOD_END_DATE, FISCAL_TYPE, CURRENCY, TXT )
			(
			select GETDATE() as LOG_DATE, 298 as DATA_ID, a.ISSUER_ID, 'C'
				,  0, '01/01/1900', a.FISCAL_TYPE, a.CURRENCY
				, 'ERROR calculating 298 Trailing Book Value. DATA_ID:104 is NULL or ZERO'
			  from #A a
			 where isnull(a.AMOUNT, 0.0) = 0.0	 -- Data error	 
			) union (	
			
				-- ERROR - No data at all available
			select GETDATE() as LOG_DATE, 298 as DATA_ID, isnull(@ISSUER_ID, ' ') as ISSUER_ID, ' ' as PERIOD_TYPE
				,  0 as PERIOD_YEAR,  '1/1/1900' as PERIOD_END_DATE,  ' ' as FISCAL_TYPE,  ' ' as CURRENCY
				, 'ERROR calculating 298 Trailing Book Value.  DATA_ID:104 No data or missing quarters' as TXT
			  from (select COUNT(*) CNT from #A having COUNT(*) = 0) z
			)
		END


	-- Clean up
	drop table #A
	drop table #B
GO
