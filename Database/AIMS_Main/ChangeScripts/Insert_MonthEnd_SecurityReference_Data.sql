IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Insert_MonthEnd_SecurityReference_Data]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Insert_MonthEnd_SecurityReference_Data]
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-----------------------------------------------------------------------------------
-- Purpose:	Called every month and insert monthend security reference data
--

--			EffDate - Effective Date
--
-- Author:	Akhtar Nazirali 
-- Date:	April 30, 2013
-----------------------------------------------------------------------------------


create procedure [dbo].[Insert_MonthEnd_SecurityReference_Data](
	

	@EffDate datetime

		
)
as
print @EffDate

	begin tran
		Merge dbo.gf_Security_baseview_history as target
		using (select @EffDate as EffDate, gsb.* from dbo.gf_Security_baseview gsb) as source
		on (	cast(target.effective_date as date) = cast(source.EffDate as date)
			and isnull(target.issuer_id,'')=isnull(source.issuer_id,'')
			and isnull(target.security_id,'') = isnull(source.security_id,'')
			)
		when matched then
			update set 
			target.ASEC_SEC_SHORT_NAME 		= source.ASEC_SEC_SHORT_NAME ,
			target.ISSUE_NAME 				= source.ISSUE_NAME ,
			target.ISIN 					= source.ISIN,
			target.SEDOL 					= source.SEDOL ,
			target.SECS_INSTYPE 			= source.SECS_INSTYPE ,
			target.ASEC_INSTR_TYPE 			= source.ASEC_INSTR_TYPE ,
			target.SECURITY_TYPE 			= source.SECURITY_TYPE ,
			target.ASEC_FC_SEC_REF 			= source.ASEC_FC_SEC_REF ,
			target.LOOK_THRU_FUND 			= source.LOOK_THRU_FUND ,
			target.FIFTYTWO_WEEK_LOW 		= source.FIFTYTWO_WEEK_LOW ,
			target.FIFTYTWO_WEEK_HIGH 		= source.FIFTYTWO_WEEK_HIGH ,
			target.SECURITY_VOLUME_AVG_90D 	= source.SECURITY_VOLUME_AVG_90D ,
			target.SECURITY_VOLUME_AVG_6M 	= source.SECURITY_VOLUME_AVG_6M ,
			target.SECURITY_VOLUME_AVG_30D 	= source.SECURITY_VOLUME_AVG_30D ,
			target.GREENFIELD_FLAG 			= source.GREENFIELD_FLAG ,
			target.WACC_COST_EQUITY 		= source.WACC_COST_EQUITY ,
			target.WACC_COST_PFD 			= source.WACC_COST_PFD ,
			target.WACC_COST_DEBT 			= source.WACC_COST_DEBT ,
			target.FLOAT_AMOUNT 			= source.FLOAT_AMOUNT ,
			target.CUSIP 					= source.CUSIP ,
			target.STOCK_EXCHANGE_ID 		= source.STOCK_EXCHANGE_ID ,
			target.ASEC_ISSUED_VOLUME 		= source.ASEC_ISSUED_VOLUME ,
			target.TRADING_CURRENCY 		= source.TRADING_CURRENCY ,
			target.SHARES_OUTSTANDING 		= source.SHARES_OUTSTANDING ,
			target.BETA 					= source.BETA,
			target.BARRA_BETA 				= source.BARRA_BETA ,
			target.TICKER 					= source.TICKER ,
			target.MSCI 					= source.MSCI,
			target.BARRA 					= source.BARRA ,
			target.ISO_COUNTRY_CODE 		= source.ISO_COUNTRY_CODE ,
			target.ASEC_SEC_COUNTRY_NAME 	= source.ASEC_SEC_COUNTRY_NAME ,
			target.ASHEMM_PROPRIETARY_REGION_CODE 	= source.ASHEMM_PROPRIETARY_REGION_CODE,
			target.ASEC_SEC_COUNTRY_ZONE_NAME 		= source.ASEC_SEC_COUNTRY_ZONE_NAME ,
			target.ISSUER_NAME 						= source.ISSUER_NAME ,
			target.ASHEMM_ONE_LINER_DESCRIPTION 	= source.ASHEMM_ONE_LINER_DESCRIPTION ,
			target.BLOOMBERG_DESCRIPTION 			= source.BLOOMBERG_DESCRIPTION ,
			target.ASHMOREEMM_INDUSTRY_ANALYST 		= source.ASHMOREEMM_INDUSTRY_ANALYST ,
			target.ASHMOREEMM_PRIMARY_ANALYST 		= source.ASHMOREEMM_PRIMARY_ANALYST ,
			target.ASHMOREEMM_PORTFOLIO_MANAGER 	= source.ASHMOREEMM_PORTFOLIO_MANAGER ,
			target.WEBSITE 							= source.WEBSITE ,
			target.FISCAL_YEAR_END 					= source.FISCAL_YEAR_END ,
			target.XREF 							= source.XREF,
			target.REPORTNUMBER 					= source.REPORTNUMBER ,
			target.GICS_SECTOR 						= source.GICS_SECTOR ,
			target.GICS_SECTOR_NAME 				= source.GICS_SECTOR_NAME ,
			target.GICS_INDUSTRY 					= source.GICS_INDUSTRY ,
			target.GICS_INDUSTRY_NAME 				= source.GICS_INDUSTRY_NAME ,
			target.GICS_SUB_INDUSTRY 				= source.GICS_SUB_INDUSTRY ,
			target.GICS_SUB_INDUSTRY_NAME 			= source.GICS_SUB_INDUSTRY_NAME ,
			target.SHARES_PER_ADR 					= source.SHARES_PER_ADR ,
			target.ADR_UNDERLYING_TICKER 			= source.ADR_UNDERLYING_TICKER ,
			target.MARKET_CAP_IN_TRADING_CURRENCY 	= source.MARKET_CAP_IN_TRADING_CURRENCY,
			target.CLOSING_PRICE 					= source.CLOSING_PRICE ,
			target.LAST_CLOSE_FX_QUO_CURR_TO_USD 	= source.LAST_CLOSE_FX_QUO_CURR_TO_USD ,
			target.LAST_CLOSE_DATE 					= source.LAST_CLOSE_DATE ,
			target.TOT_CURR_SHRS_OUTST_ALL_CLASS 	= source.TOT_CURR_SHRS_OUTST_ALL_CLASS ,
			target.CHG_PCT_MTD 						= source.CHG_PCT_MTD ,
			target.CHG_PCT_QTD 						= source.CHG_PCT_QTD ,
			target.CHG_PCT_YTD 						= source.CHG_PCT_YTD ,
			target.CHG_PCT_1YR 						= source.CHG_PCT_1YR ,
			target.issuer_proxy						= source.issuer_proxy ,
			target.UPDATE_BB_STATUS 				= source.UPDATE_BB_STATUS 
		
		when not matched  then
		
			INSERT(EFFECTIVE_DATE, 
			SECURITY_ID,
			ASEC_SEC_SHORT_NAME,
			ISSUE_NAME,
			ISIN,
			SEDOL,
			SECS_INSTYPE,
			ASEC_INSTR_TYPE,
			SECURITY_TYPE,
			ASEC_FC_SEC_REF,
			LOOK_THRU_FUND,
			FIFTYTWO_WEEK_LOW,
			FIFTYTWO_WEEK_HIGH,
			SECURITY_VOLUME_AVG_90D,
			SECURITY_VOLUME_AVG_6M,
			SECURITY_VOLUME_AVG_30D,
			GREENFIELD_FLAG,
			WACC_COST_EQUITY,
			WACC_COST_PFD,
			WACC_COST_DEBT,
			FLOAT_AMOUNT,
			CUSIP,
			STOCK_EXCHANGE_ID,
			ASEC_ISSUED_VOLUME,
			ISSUER_ID,
			TRADING_CURRENCY,
			SHARES_OUTSTANDING,
			BETA,
			BARRA_BETA,
			TICKER,
			MSCI,
			BARRA,
			ISO_COUNTRY_CODE,
			ASEC_SEC_COUNTRY_NAME,
			ASHEMM_PROPRIETARY_REGION_CODE ,
			ASEC_SEC_COUNTRY_ZONE_NAME ,
			ISSUER_NAME,
			ASHEMM_ONE_LINER_DESCRIPTION ,
			BLOOMBERG_DESCRIPTION,
			ASHMOREEMM_INDUSTRY_ANALYST ,
			ASHMOREEMM_PRIMARY_ANALYST ,
			ASHMOREEMM_PORTFOLIO_MANAGER ,
			WEBSITE,
			FISCAL_YEAR_END,
			XREF,
			REPORTNUMBER,
			GICS_SECTOR,
			GICS_SECTOR_NAME,
			GICS_INDUSTRY,
			GICS_INDUSTRY_NAME,
			GICS_SUB_INDUSTRY,
			GICS_SUB_INDUSTRY_NAME,
			SHARES_PER_ADR,
			ADR_UNDERLYING_TICKER,
			MARKET_CAP_IN_TRADING_CURRENCY ,
			CLOSING_PRICE,
			LAST_CLOSE_FX_QUO_CURR_TO_USD ,
			LAST_CLOSE_DATE,
			TOT_CURR_SHRS_OUTST_ALL_CLASS ,
			CHG_PCT_MTD,
			CHG_PCT_QTD,
			CHG_PCT_YTD,
			CHG_PCT_1YR,
			issuer_proxy,
			UPDATE_BB_STATUS)
			VALUES
			(	cast(source.EFFDate as date),
				source.SECURITY_ID ,
				source.ASEC_SEC_SHORT_NAME ,
				source.ISSUE_NAME ,
				source.ISIN,
				source.SEDOL ,
				source.SECS_INSTYPE ,
				source.ASEC_INSTR_TYPE ,
				source.SECURITY_TYPE ,
				source.ASEC_FC_SEC_REF ,
				source.LOOK_THRU_FUND ,
				source.FIFTYTWO_WEEK_LOW ,
				source.FIFTYTWO_WEEK_HIGH ,
				source.SECURITY_VOLUME_AVG_90D ,
				source.SECURITY_VOLUME_AVG_6M ,
				source.SECURITY_VOLUME_AVG_30D ,
				source.GREENFIELD_FLAG ,
				source.WACC_COST_EQUITY ,
				source.WACC_COST_PFD ,
				source.WACC_COST_DEBT ,
				source.FLOAT_AMOUNT ,
				source.CUSIP ,
				source.STOCK_EXCHANGE_ID ,
				source.ASEC_ISSUED_VOLUME ,
				source.ISSUER_ID ,
				source.TRADING_CURRENCY ,
				source.SHARES_OUTSTANDING ,
				source.BETA,
				source.BARRA_BETA ,
				source.TICKER ,
				source.MSCI,
				source.BARRA ,
				source.ISO_COUNTRY_CODE ,
				source.ASEC_SEC_COUNTRY_NAME ,
				source.ASHEMM_PROPRIETARY_REGION_CODE,
				source.ASEC_SEC_COUNTRY_ZONE_NAME ,
				source.ISSUER_NAME ,
				source.ASHEMM_ONE_LINER_DESCRIPTION ,
				source.BLOOMBERG_DESCRIPTION ,
				source.ASHMOREEMM_INDUSTRY_ANALYST ,
				source.ASHMOREEMM_PRIMARY_ANALYST ,
				source.ASHMOREEMM_PORTFOLIO_MANAGER ,
				source.WEBSITE ,
				source.FISCAL_YEAR_END ,
				source.XREF,
				source.REPORTNUMBER ,
				source.GICS_SECTOR ,
				source.GICS_SECTOR_NAME ,
				source.GICS_INDUSTRY ,
				source.GICS_INDUSTRY_NAME ,
				source.GICS_SUB_INDUSTRY ,
				source.GICS_SUB_INDUSTRY_NAME ,
				source.SHARES_PER_ADR ,
				source.ADR_UNDERLYING_TICKER ,
				source.MARKET_CAP_IN_TRADING_CURRENCY,
				source.CLOSING_PRICE ,
				source.LAST_CLOSE_FX_QUO_CURR_TO_USD ,
				source.LAST_CLOSE_DATE ,
				source.TOT_CURR_SHRS_OUTST_ALL_CLASS ,
				source.CHG_PCT_MTD ,
				source.CHG_PCT_QTD ,
				source.CHG_PCT_YTD ,
				source.CHG_PCT_1YR ,
				source.issuer_proxy ,
				source.UPDATE_BB_STATUS 
			)
		output $action, Inserted.*, Deleted.*;
	commit tran;
	
-- exec dbo.[Insert_MonthEnd_SecurityReference_Data] '04/30/2013' 

