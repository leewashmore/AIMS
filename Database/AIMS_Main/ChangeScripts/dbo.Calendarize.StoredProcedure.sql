SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-----------------------------------------------------------------------------------
-- Purpose:	To take the Fiscal data and make a calendarized version of it.
--
-- Author:	David Muench
-- Date:	Oct. 15, 2012
-----------------------------------------------------------------------------------

alter procedure [dbo].[Calendarize](
	@ISSUER_ID			varchar(20) = NULL					-- The company identifier		
)
as
			-- Create a temp table that mirrors PERIOD_FINAINCIALS
			select * 
			  into #PF
			  from PERIOD_FINANCIALS
			 where 1=0


			print 'Calendarize PRIMARY ANNUAL'
			-- Calendarize
			print '>> ' + CONVERT(varchar(40), getdate(), 121) + ' - before Calendarize'
			insert into PERIOD_FINANCIALS
			select pfc.ISSUER_ID
				,  pfc.SECURITY_ID
				,  pfc.COA_TYPE
				,  pfc.DATA_SOURCE
				,  pfc.ROOT_SOURCE
				,  pfc.ROOT_SOURCE_DATE
				,  pfc.PERIOD_TYPE
				,  pfc.PERIOD_YEAR
				,  pfc.PERIOD_END_DATE
				,  'CALENDAR' as FISCAL_TYPE
				,  pfc.CURRENCY
				,  pfc.DATA_ID
				,  (pfc.AMOUNT  * datepart(month, pfc.PERIOD_END_DATE)/12)
					+  case when pfn.DATA_ID is not null 
								then (pfn.AMOUNT * (12-datepart(month, pfc.PERIOD_END_DATE)/12))
							else (pfc.AMOUNT * (12-datepart(month, pfc.PERIOD_END_DATE)/12)) end as AMOUNT
				,  pfc.CALCULATION_DIAGRAM
				,  pfc.SOURCE_CURRENCY
				,  pfc.AMOUNT_TYPE
			  from dbo.PERIOD_FINANCIALS pfc
			  left join dbo.PERIOD_FINANCIALS pfn on pfn.ISSUER_ID = pfc.ISSUER_ID     and pfn.AMOUNT_TYPE = pfc.AMOUNT_TYPE
												 and pfn.DATA_ID = pfc.DATA_ID         and pfn.CURRENCY = pfc.CURRENCY
												 and pfn.DATA_SOURCE = pfc.DATA_SOURCE and pfn.FISCAL_TYPE = pfc.FISCAL_TYPE
												 and pfn.PERIOD_TYPE = pfc.PERIOD_TYPE and pfn.PERIOD_YEAR = pfc.PERIOD_YEAR
												 and pfn.SECURITY_ID = pfc.SECURITY_ID 
			 inner join dbo.DATA_MASTER dm on dm.DATA_ID = pfc.DATA_ID
			 where dm.CALENDARIZE = 'Y'
			   and pfc.ISSUER_ID = @ISSUER_ID
			   and pfc.DATA_SOURCE = 'PRIMARY'							-- Only the newly created values for PRIMARY
			   and pfc.PERIOD_TYPE = 'A'								-- Only Annual, not Quarterly
			   and pfc.FISCAL_TYPE = 'FISCAL'							-- Only FISCAL
--			 order by pfc.ISSUER_ID, pfc.AMOUNT_TYPE, pfc.DATA_SOURCE, pfc.CURRENCY, pfc.PERIOD_YEAR, pfc.PERIOD_TYPE, pfc.DATA_ID

			BEGIN TRANSACTION;

			insert into dbo.PERIOD_FINANCIALS
			select * from #PF;

			commit;
GO
