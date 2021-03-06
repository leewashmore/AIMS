SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[Get_EV_EBITDA](@securityId varchar(20),@issuerId varchar(20),@chartTitle varchar(20))	
AS
BEGIN
	SET NOCOUNT ON;
	
	------Prices/Shares/FX Rates----------
	Select a.SECURITY_ID, a.PRICE_DATE
		, (a.price/case when a.ADR_CONV = 0.0 then 1.0 else a.ADR_CONV end)/ fx.FX_RATE as USDPrice
		, fx.CURRENCY
		, b.SHARES_OUTSTANDING 
into #SupportData
from PRICES a
		inner join GF_SECURITY_BASEVIEW sb			on sb.SECURITY_ID = a.SECURITY_ID
		inner join FX_RATES fx						on fx.CURRENCY = sb.TRADING_CURRENCY and fx.FX_DATE = a.PRICE_DATE
		inner join ISSUER_SHARES b					on b.SHARES_DATE = fx.FX_DATE and b.ISSUER_ID = sb.ISSUER_ID
where sb.SECURITY_ID = @securityId
	
	
	-----EXTRACT QUARTER VALUES FROM PRIMARY ANALYST SOURCE FIRST---
	
Select * 
into #PrimaryFinancials
from PERIOD_FINANCIALS with (nolock)
where (@issuerId is null or ISSUER_ID = @issuerId)
		and PERIOD_END_DATE > dateadd (year, -10, getdate())
		and PERIOD_END_DATE < dateadd (year, 6, getdate())
		-- and DATA_SOURCE = 'PRIMARY' -- currently no data for Primary analyst
		and FISCAL_TYPE = 'FISCAL'
		and Currency = 'USD'
		and PERIOD_TYPE in ('Q1','Q2','Q3','Q4')
order by ISSUER_ID, DATA_ID, PERIOD_YEAR

-- *next pull quarter estimated values for periods after the oldest PeriodEndDate in the select above

Select * 
into #ConsensusFinancials
from CURRENT_CONSENSUS_ESTIMATES cce with (nolock)
where (@issuerId is null or ISSUER_ID = @issuerId)
		and PERIOD_END_DATE > (select max(PERIOD_END_DATE) as PERIOD_END_DATE from #PrimaryFinancials)
		and DATA_SOURCE = 'REUTERS' 
		and FISCAL_TYPE = 'FISCAL'
		and Currency = 'USD'
		and PERIOD_TYPE in ('Q1','Q2','Q3','Q4')
order by ISSUER_ID, estimate_id, PERIOD_YEAR


   --Joining tables depending upon the chart title
     
 

 IF(@chartTitle = 'EV/EBITDA')
			BEGIN
				
					
					--JOIN ABOVE TWO EXTRACTS ORDERED BY PERIODENDDATES INTO “REVENUEVALUES”
				  SELECT *
				  INTO #RevenueValuesEV_EBITDA
				  FROM 
						   (Select PERIOD_TYPE + ' ' + CAST(PERIOD_YEAR AS VARCHAR(4)) as PeriodLabel
									,  PERIOD_END_DATE
									,  AMOUNT
									,  PERIOD_TYPE
									,  PERIOD_YEAR
									,  DATA_ID
							from #PrimaryFinancials
							where DATA_ID in (90,51,92,29,24)
							)ev_ebitda
							
							
					--*From above extract, group by PeriodType and PeriodYear calculating two values (Net Debt and EBITDA) for each Period by aggregating DataID’s per the following into datset EVEBITDA
					SELECT  PERIOD_TYPE + ' ' + CAST(PERIOD_YEAR AS VARCHAR(4)) as PeriodLabel
							,PERIOD_END_DATE							
							,[90] 
							,[92]
							,[51]
					into #PIVOT_EVBTIDA_NetDebt
					FROM    
					( 
						Select o.PERIOD_TYPE 
								,o.PERIOD_YEAR 
								,a.period_end_date
								,a.Data_id
								,a.AMOUNT 
						from #RevenueValuesEV_EBITDA a
							Inner join(
							  SELECT     PERIOD_TYPE 
										,PERIOD_YEAR
							  FROM      #RevenueValuesEV_EBITDA
							  group by  PERIOD_TYPE, PERIOD_YEAR
							  ) o
							  ON a.PERIOD_TYPE = o.PERIOD_TYPE
								and a.PERIOD_YEAR = o.PERIOD_YEAR
					) p 
					PIVOT ( SUM(Amount)
								FOR Data_id   IN ([90],[92],[51])
							  ) AS pvt 
                  
                                   
                  SELECT  PERIOD_TYPE
						  ,PERIOD_YEAR
						  ,PERIOD_TYPE + ' ' + CAST(PERIOD_YEAR AS VARCHAR(4)) as PeriodLabel
						  ,PERIOD_END_DATE
						  ,[29] 
						  ,[24]
						into #PIVOT_EVBTIDA_EBITDA
							FROM    
							( 
								Select o.PERIOD_TYPE 
										,o.PERIOD_YEAR 
										,period_end_date
										,Data_id
										,Amount 
								from #RevenueValuesEV_EBITDA a
									Inner join(
									  SELECT     PERIOD_TYPE 
												,PERIOD_YEAR
									  FROM      #RevenueValuesEV_EBITDA
									  group by PERIOD_TYPE, PERIOD_YEAR
									  ) o
									  ON a.PERIOD_TYPE = o.PERIOD_TYPE
										and a.PERIOD_YEAR = o.PERIOD_YEAR
							) p 
							PIVOT ( SUM(Amount)
										FOR Data_id   IN ([29],[24])
									  ) AS pvt 
							
                  
                  --CALCULATING NetDebt and EBITDA
                  Select e.PERIOD_TYPE
						  ,e.PERIOD_YEAR
						  ,e.PERIOD_TYPE + ' ' + CAST(e.PERIOD_YEAR AS VARCHAR(4)) as PeriodLabel--e.PeriodLabel
						  ,e.PERIOD_END_DATE
						  ,e.[29]+e.[24] As EBITDA
						  ,n.[90]+n.[92]-n.[51] as NetDebt
                  into #EVEBITDA 
                  from #PIVOT_EVBTIDA_EBITDA e 
                  inner join #PIVOT_EVBTIDA_NetDebt n
                  on
                  e.PeriodLabel = n.PeriodLabel
                  order by e.PERIOD_YEAR, e.PERIOD_TYPE
                                    
					--Join EVEBITDA dataset  to SupportData on PeriodEndDates
					Select a.*, b.USDPrice, b.SHARES_OUTSTANDING 
					into #EVEBITDASupport
					from #EVEBITDA a					
					left join #SupportData b 
					on a.PERIOD_END_DATE = b.PRICE_DATE
					order by a.PERIOD_YEAR, a.PERIOD_TYPE

					
					--SELECTING CALCULATED DATA
					
					SELECT * FROM #EVEBITDASupport
					
					--Dropping above tables
					DROP TABLE #RevenueValuesEV_EBITDA
					DROP TABLE #PIVOT_EVBTIDA_NetDebt	
					DROP TABLE #PIVOT_EVBTIDA_EBITDA	
					DROP TABLE #EVEBITDA
			END	 
	
	--Dropping temporary tables
	drop table #SupportData
	drop table #PrimaryFinancials
	drop table #ConsensusFinancials	

END
GO
