USE [AIMS_Main_Dev]
GO

/****** Object:  StoredProcedure [dbo].[Get_Data_Process_Thread]    Script Date: 03/21/2013 12:20:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO




Alter procedure [dbo].[RebuildIndex]

as
					DBCC INDEXDEFRAG (0, PERIOD_FINANCIALS, PERIOD_FINANCIALS_idx ,0 )
					DBCC INDEXDEFRAG (0, PERIOD_FINANCIALS, PERIOD_FINANCIALS_idx2 ,0 )
					DBCC INDEXDEFRAG (0, PERIOD_FINANCIALS, PERIOD_FINANCIALS_idx3 ,0 )
					DBCC INDEXDEFRAG (0, PERIOD_FINANCIALS, PERIOD_FINANCIALS_idx4 ,0 )
					DBCC INDEXDEFRAG (0, PERIOD_FINANCIALS, PERIOD_FINANCIALS_idx5 ,0 )
					DBCC INDEXDEFRAG (0, PERIOD_FINANCIALS, PERIOD_FINANCIALS_idx6 ,0 )
					DBCC INDEXDEFRAG (0, FX_RATES, FX_RATES_idx ,0 )
					DBCC INDEXDEFRAG (0, PRICES, PRICES_idx ,0 )
					DBCC INDEXDEFRAG (0, COUNTRY_MASTER, COUNTRY_MASTER_idx2 ,0 )
					DBCC INDEXDEFRAG (0, DATA_MASTER, DATA_MASTER_idx ,0 )
					DBCC INDEXDEFRAG (0, CONSENSUS_MASTER, CONSENSUS_MASTER_idx ,0 )
					DBCC INDEXDEFRAG (0, ISSUER_SHARES, ISSUER_SHARES_inx ,0 )
					DBCC INDEXDEFRAG (0, ISSUER_SHARES, ISSUER_SHARES_indx2 ,0 )
					DBCC INDEXDEFRAG (0, CALC_LIST, CALC_LIST_idx ,0 )
					DBCC INDEXDEFRAG (0, CALC_LIST, CALC_LIST_idx2 ,0 )
					-- Make sure the indexes I need are up to date
					UPDATE STATISTICS PERIOD_FINANCIALS (PERIOD_FINANCIALS_idx ,PERIOD_FINANCIALS_idx2,PERIOD_FINANCIALS_idx3,PERIOD_FINANCIALS_idx4,PERIOD_FINANCIALS_idx5,PERIOD_FINANCIALS_idx6 ) WITH FULLSCAN
					UPDATE STATISTICS GF_SECURITY_BASEVIEW (GF_SECURITY_BASEVIEW_idx, GF_SECURITY_BASEVIEW_idx2
										, GF_SECURITY_BASEVIEW_idx3, GF_SECURITY_BASEVIEW_idx4, GF_SECURITY_BASEVIEW_idx5) WITH FULLSCAN
					UPDATE STATISTICS FX_RATES (FX_RATES_idx) WITH FULLSCAN
					UPDATE STATISTICS PRICES (PRICES_idx) WITH FULLSCAN
					UPDATE STATISTICS COUNTRY_MASTER (COUNTRY_MASTER_idx, COUNTRY_MASTER_idx2) WITH FULLSCAN
					UPDATE STATISTICS DATA_MASTER (DATA_MASTER_idx) WITH FULLSCAN
					UPDATE STATISTICS CONSENSUS_MASTER (CONSENSUS_MASTER_idx) WITH FULLSCAN
					UPDATE STATISTICS CALC_LIST (CALC_LIST_idx) WITH FULLSCAN
					UPDATE STATISTICS CALC_LIST (CALC_LIST_idx2) WITH FULLSCAN
					UPDATE STATISTICS ISSUER_SHARES (ISSUER_SHARES_inx) WITH FULLSCAN
					UPDATE STATISTICS ISSUER_SHARES (ISSUER_SHARES_indx2) WITH FULLSCAN