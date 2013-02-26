SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[GetConsensusEstimatesValuation](
	@ISSUER_ID			varchar(20)					-- The company identifier		
,	@DATA_SOURCE		varchar(10)  = 'REUTERS'	-- REUTERS, PRIMARY, INDUSTRY
,	@PERIOD_TYPE		char(2) = 'A'				-- A, Q
,	@FISCAL_TYPE		char(8) = 'FISCAL'			-- FISCAL, CALENDAR
,	@CURRENCY			char(3)	= 'USD'				-- USD or the currency of the country (local)
,	@ESTIMATE_ID		integer = NULL				-- A specific ID or NULL for all.
,	@PERIOD_YEAR		integer = NULL				-- A specific year if required	
,	@Security_ID		varchar(20)					-- Security-ID of selected Security
)
as

-- Select the actual data
	select cce.ISSUER_ID
		,  cce.ESTIMATE_ID
		,  null as ESTIMATE_DESC
		,  substring(ISNULL(AMOUNT_TYPE,'ESTIMATE'),1,1)+cast(ISNULL(cce.PERIOD_YEAR,2300) as CHAR(4))+ISNULL(cce.PERIOD_TYPE,'E') as Period
		,  ISNULL(cce.AMOUNT_TYPE, 'ESTIMATE') AS AMOUNT_TYPE
		,  ISNULL(cce.PERIOD_YEAR,2300) AS PERIOD_YEAR
		,  ISNULL(cce.PERIOD_TYPE,'E') AS PERIOD_TYPE
		,  ISNULL(cce.AMOUNT,0) AS AMOUNT
		,  0.0 as ASHMOREEMM_AMOUNT
		,  cce.NUMBER_OF_ESTIMATES
		,  cce.HIGH
		,  cce.LOW
		,  cce.STANDARD_DEVIATION
		,  cce.SOURCE_CURRENCY
		,  cce.DATA_SOURCE
		,  cce.DATA_SOURCE_DATE
		,  cce.SECURITY_ID 
		,  case cce.Estimate_id 
			when 170 then 'P_Revenue'		--P/Revenue				 
			when 171 then 'P_CE'			--P/CE						 		
			when 166 then 'P_E'				--P/E								
			when 172 then 'P_E_To_Growth'	--P/E To Growth							
			when 164 then 'P_BV'			--P/BV						
			when 192 then 'DIV_YIELD'		--DividendYield
		   End as dp_desc	
	--into #ConsensusActualData
	into #Actual
	from CURRENT_CONSENSUS_ESTIMATES cce
	where cce.ISSUER_ID = @ISSUER_ID or cce.SECURITY_ID=@Security_ID
	   and cce.DATA_SOURCE = @DATA_SOURCE
	   and substring(cce.PERIOD_TYPE,1,1) = @PERIOD_TYPE
	   and cce.FISCAL_TYPE = @FISCAL_TYPE
	   and cce.CURRENCY = @CURRENCY
	   and cce.AMOUNT_TYPE = 'ACTUAL'
	   and cce.estimate_id in (170,		--P/Revenue
								171,	--P/CE
								166,	--P/E
								172,	--P/E To Growth
								164,	--P/BV
								192		--DividendYield
								)	
	 order by cce.PERIOD_YEAR
	 ;

	/*-- Select the Actual data 
	select ISNULL(cce.ISSUER_ID,@ISSUER_ID) AS ISSUER_ID
		,  cm.ESTIMATE_ID
		,  cm.ESTIMATE_DESC
		,  substring(ISNULL(AMOUNT_TYPE,'ESTIMATE'),1,1)+cast(ISNULL(cce.PERIOD_YEAR,2300) as CHAR(4))+ISNULL(cce.PERIOD_TYPE,'E') as Period
		,  ISNULL(cce.AMOUNT_TYPE,'ESTIMATE') AS AMOUNT_TYPE
		,  ISNULL(cce.PERIOD_YEAR,2300) AS PERIOD_YEAR
		,  ISNULL(cce.PERIOD_TYPE,'E') AS PERIOD_TYPE
		,  ISNULL(cce.AMOUNT,0) AS AMOUNT
		,  0.0 as ASHMOREEMM_AMOUNT
		,  cce.NUMBER_OF_ESTIMATES
		,  cce.HIGH
		,  cce.LOW
		,  cce.STANDARD_DEVIATION
		,  ISNULL(cce.SOURCE_CURRENCY,@CURRENCY) AS SOURCE_CURRENCY
		,  ISNULL(cce.DATA_SOURCE,@DATA_SOURCE) AS DATA_SOURCE
		,  cce.DATA_SOURCE_DATE
		,  ISNULL(cce.SECURITY_ID,@Security_ID) AS SECURITY_ID
	  into #Actual
	  from CONSENSUS_MASTER cm
	 Left join #ConsensusActualData cce on cm.ESTIMATE_ID =cce.ESTIMATE_ID
	 order by cce.PERIOD_YEAR;
*/
	-- Select the Estimated data
	
	select cce.ISSUER_ID
		,  cce.ESTIMATE_ID
		,  null as ESTIMATE_DESC
		,  substring(ISNULL(AMOUNT_TYPE,'ESTIMATE'),1,1)+cast(ISNULL(cce.PERIOD_YEAR,2300) as CHAR(4))+ISNULL(cce.PERIOD_TYPE,'E') as Period
		,  ISNULL(cce.AMOUNT_TYPE, 'ESTIMATE') AS AMOUNT_TYPE
		,  ISNULL(cce.PERIOD_YEAR,2300)AS PERIOD_YEAR
		,  ISNULL(cce.PERIOD_TYPE,'E') AS PERIOD_TYPE
		,  ISNULL(cce.AMOUNT,0) AS AMOUNT
		,  0.0 as ASHMOREEMM_AMOUNT
		,  cce.NUMBER_OF_ESTIMATES
		,  cce.HIGH
		,  cce.LOW
		,  cce.STANDARD_DEVIATION
		,  cce.SOURCE_CURRENCY
		,  cce.DATA_SOURCE
		,  cce.DATA_SOURCE_DATE
		,  cce.SECURITY_ID
		,  case cce.Estimate_id 
			when 170 then 'P_Revenue'		--P/Revenue				 
			when 171 then 'P_CE'			--P/CE						 		
			when 166 then 'P_E'				--P/E								
			when 172 then 'P_E_To_Growth'	--P/E To Growth							
			when 164 then 'P_BV'			--P/BV						
			when 192 then 'DIV_YIELD'		--DividendYield
		 End as dp_desc	
	  into #Estimate
	  from dbo.CURRENT_CONSENSUS_ESTIMATES cce
		where (cce.ISSUER_ID = @ISSUER_ID or cce.SECURITY_ID=@Security_ID)
	   and cce.DATA_SOURCE = @DATA_SOURCE
	   and substring(cce.PERIOD_TYPE,1,1) = @PERIOD_TYPE
	   and cce.FISCAL_TYPE = @FISCAL_TYPE
	   and cce.CURRENCY = @CURRENCY
	   and cce.AMOUNT_TYPE = 'ESTIMATE'
	   and cce.estimate_id in ( 170,		--P/Revenue
								171,	--P/CE
								166,	--P/E
								172,	--P/E To Growth
								164,	--P/BV
								192		--DividendYield
							)
	   --and (cce.ESTIMATE_ID =cce.ESTIMATE_ID)
	 order by cce.PERIOD_YEAR
	 ;
	
	
	/*
		
	select ISNULL(cce.ISSUER_ID,@ISSUER_ID) AS ISSUER_ID
		,  cm.ESTIMATE_ID
		,  cm.ESTIMATE_DESC
		,  substring(ISNULL(AMOUNT_TYPE,'ESTIMATE'),1,1)+cast(ISNULL(cce.PERIOD_YEAR,2300) as CHAR(4))+ISNULL(cce.PERIOD_TYPE,'E') as Period
		,  ISNULL(cce.AMOUNT_TYPE, 'ESTIMATE') AS AMOUNT_TYPE
		,  ISNULL(cce.PERIOD_YEAR,2300)AS PERIOD_YEAR
		,  ISNULL(cce.PERIOD_TYPE,'E') AS PERIOD_TYPE
		,  ISNULL(cce.AMOUNT,0) AS AMOUNT
		,  0.0 as ASHMOREEMM_AMOUNT
		,  cce.NUMBER_OF_ESTIMATES
		,  cce.HIGH
		,  cce.LOW
		,  cce.STANDARD_DEVIATION
		,  ISNULL(cce.SOURCE_CURRENCY,@CURRENCY) AS SOURCE_CURRENCY
		,  ISNULL(cce.DATA_SOURCE,@DATA_SOURCE) AS DATA_SOURCE
		,  cce.DATA_SOURCE_DATE
		,  ISNULL(cce.SECURITY_ID,@Security_ID) AS SECURITY_ID
	  into #Estimate
	  from #ConsensusEstimateData cce
	 Right join dbo.CONSENSUS_MASTER cm on cm.ESTIMATE_ID =cce.ESTIMATE_ID
	 order by cce.PERIOD_YEAR
	 ;
	 */
	 
	 Select pf.ISSUER_ID, pf.AMOUNT, pf.AMOUNT_TYPE, pf.DATA_ID
				,pf.FISCAL_TYPE, pf.CURRENCY, pf.PERIOD_YEAR, pf.PERIOD_TYPE,
	 		case pf.data_id 
				when 170 then 'P_Revenue'		--P/Revenue				 
				when 171 then 'P_CE'			--P/CE						 		
				when 166 then 'P_E'				--P/E								
				when 172 then 'P_E_To_Growth'	--P/E To Growth							
				when 164 then 'P_BV'			--P/BV						
				when 192 then 'DIV_YIELD'		--DividendYield
			 End as dp_desc	
		 into #pf1
		 from PERIOD_FINANCIALS pf
				  where 1=1
				    and pf.DATA_SOURCE = 'PRIMARY'
				    and substring(pf.PERIOD_TYPE,1,1) = @PERIOD_TYPE
				    and pf.FISCAL_TYPE = @FISCAL_TYPE
				    and pf.CURRENCY = @CURRENCY
				    and (pf.ISSUER_ID = @ISSUER_ID or pf.SECURITY_ID=@Security_ID)
					and pf.DATA_ID in( 170, --P/Revenue			
										171, --P/CE	
										166, --P/E
										172,  --P/E To Growth
										164,  --P/BV
										192   --DividendYield
									  )
	 

	-- Combine Actual with Estimated so that Actual is always used in case both are present.
	select isnull(e.ISSUER_ID, a.ISSUER_ID) as ISSUER_ID
		,  isnull(e.ESTIMATE_ID, a.ESTIMATE_ID) as ESTIMATE_ID
		,  isnull(e.ESTIMATE_DESC, a.ESTIMATE_DESC) as [ESTIMATE_DESC]
		,  isnull(e.Period, a.Period) as Period
		,  isnull(e.AMOUNT_TYPE, a.AMOUNT_TYPE) as AMOUNT_TYPE
		,  isnull(e.PERIOD_YEAR, a.PERIOD_YEAR) as PERIOD_YEAR
		,  isnull(e.PERIOD_TYPE, a.PERIOD_TYPE) as PERIOD_TYPE
		,  isnull(e.AMOUNT, a.AMOUNT) as AMOUNT
		--,  b.AMOUNT as ASHMOREEMM_AMOUNT
		,(select amount from #pf1 b where b.issuer_id = ae.issuer_id and b.period_year = ae.period_year
				and b.dp_desc = ae.dp_desc
		  ) as ASHMOREEMM_AMOUNT
		,  isnull(e.NUMBER_OF_ESTIMATES, a.NUMBER_OF_ESTIMATES) as NUMBER_OF_ESTIMATES
		,  isnull(e.HIGH, a.HIGH) as HIGH
		,  isnull(e.LOW, a.LOW) as LOW
		,  isnull(e.STANDARD_DEVIATION, a.STANDARD_DEVIATION) as STANDARD_DEVIATION
		,  isnull(e.SOURCE_CURRENCY, a.SOURCE_CURRENCY) as SOURCE_CURRENCY
		,  isnull(e.DATA_SOURCE, a.DATA_SOURCE) as DATA_SOURCE
		,  isnull(e.DATA_SOURCE_DATE, a.DATA_SOURCE_DATE) as DATA_SOURCE_DATE
		,  a.AMOUNT  as ACTUAL
	-- Need to get all the possible rows
	  from (select distinct * 
			 from (		  select ISSUER_ID, DATA_SOURCE, ESTIMATE_ID, PERIOD_YEAR, SOURCE_CURRENCY,SECURITY_ID , dp_desc
							from #Actual 
					union 
						  select ISSUER_ID, DATA_SOURCE, ESTIMATE_ID, PERIOD_YEAR, SOURCE_CURRENCY,SECURITY_ID, dp_desc
							from #Estimate
				  ) u
			) ae
	  left join #Estimate e on (e.ISSUER_ID = ae.ISSUER_ID or e.SECURITY_ID=ae.SECURITY_ID) 
				and e.DATA_SOURCE = ae.DATA_SOURCE 
				and e.ESTIMATE_ID = ae.ESTIMATE_ID
				and e.PERIOD_YEAR = ae.PERIOD_YEAR and e.SOURCE_CURRENCY = ae.SOURCE_CURRENCY
	  left join #Actual a on (a.ISSUER_ID = ae.ISSUER_ID or  a.SECURITY_ID=ae.SECURITY_ID)  
				and a.DATA_SOURCE = ae.DATA_SOURCE 
				and a.ESTIMATE_ID = ae.ESTIMATE_ID
				and a.PERIOD_YEAR = ae.PERIOD_YEAR and a.SOURCE_CURRENCY = ae.SOURCE_CURRENCY
/*				
	  left join (Select pf.ISSUER_ID, pf.AMOUNT, pf.AMOUNT_TYPE, pf.DATA_ID,pf.SECURITY_ID
				   from PERIOD_FINANCIALS pf
				  where 1=1
				    and pf.DATA_SOURCE = 'PRIMARY'
				    and substring(pf.PERIOD_TYPE,1,1) = @PERIOD_TYPE
				    and pf.FISCAL_TYPE = @FISCAL_TYPE
				    and pf.CURRENCY = @CURRENCY
				    and (pf.ISSUER_ID = @ISSUER_ID or pf.SECURITY_ID=@Security_ID)
				 ) b on (b.ISSUER_ID = a.ISSUER_ID or b.SECURITY_ID=@Security_ID)
					and (   (b.DATA_ID =  170 and a.ESTIMATE_ID = 170)		-- P/Revenue
						 or (b.DATA_ID = 171 and a.ESTIMATE_ID = 171)			-- P/CE
						 or (b.DATA_ID =  166 and a.ESTIMATE_ID =166)-- P/E
						 or (b.DATA_ID = 172 and a.ESTIMATE_ID =172)	-- P/E To Growth
						 or (b.DATA_ID = 164 and a.ESTIMATE_ID = 164)		-- P/BV
						 or (b.DATA_ID = 192 and a.ESTIMATE_ID = 192)		-- DividendYield
						)
		;
*/		
	-- Clean up
	drop table #Actual;
	drop table #Estimate;
	drop table #ConsensusActualData;
	drop table #ConsensusEstimateData;
GO
