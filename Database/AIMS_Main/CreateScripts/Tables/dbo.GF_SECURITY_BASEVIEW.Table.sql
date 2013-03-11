/****** Object:  Table [dbo].[GF_SECURITY_BASEVIEW]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[GF_SECURITY_BASEVIEW](
	[ID] [int] NOT NULL,
	[GF_ID] [smallint] NULL,
	[SECURITY_ID] [varchar](20) NULL,
	[ASEC_SEC_SHORT_NAME] [nvarchar](255) NULL,
	[ISSUE_NAME] [nvarchar](255) NULL,
	[ISIN] [nvarchar](255) NULL,
	[SEDOL] [nvarchar](255) NULL,
	[SECS_INSTYPE] [nvarchar](255) NULL,
	[ASEC_INSTR_TYPE] [nvarchar](255) NULL,
	[SECURITY_TYPE] [nvarchar](255) NULL,
	[ASEC_FC_SEC_REF] [int] NULL,
	[LOOK_THRU_FUND] [nvarchar](255) NULL,
	[FIFTYTWO_WEEK_LOW] [float] NULL,
	[FIFTYTWO_WEEK_HIGH] [float] NULL,
	[SECURITY_VOLUME_AVG_90D] [nvarchar](255) NULL,
	[SECURITY_VOLUME_AVG_6M] [float] NULL,
	[SECURITY_VOLUME_AVG_30D] [float] NULL,
	[GREENFIELD_FLAG] [nvarchar](255) NULL,
	[WACC_COST_EQUITY] [nvarchar](255) NULL,
	[WACC_COST_PFD] [nvarchar](255) NULL,
	[WACC_COST_DEBT] [nvarchar](255) NULL,
	[FLOAT_AMOUNT] [nvarchar](255) NULL,
	[CUSIP] [nvarchar](255) NULL,
	[STOCK_EXCHANGE_ID] [nvarchar](255) NULL,
	[ASEC_ISSUED_VOLUME] [float] NULL,
	[ISSUER_ID] [varchar](20) NULL,
	[TRADING_CURRENCY] [nvarchar](255) NULL,
	[SHARES_OUTSTANDING] [float] NULL,
	[BETA] [nvarchar](100) NULL,
	[BARRA_BETA] [float] NULL,
	[TICKER] [nvarchar](255) NULL,
	[MSCI] [nvarchar](255) NULL,
	[BARRA] [nvarchar](255) NULL,
	[ISO_COUNTRY_CODE] [nvarchar](255) NULL,
	[ASEC_SEC_COUNTRY_NAME] [nvarchar](255) NULL,
	[ASHEMM_PROPRIETARY_REGION_CODE] [nvarchar](255) NULL,
	[ASEC_SEC_COUNTRY_ZONE_NAME] [nvarchar](255) NULL,
	[ISSUER_NAME] [nvarchar](255) NULL,
	[ASHEMM_ONE_LINER_DESCRIPTION] [nvarchar](255) NULL,
	[BLOOMBERG_DESCRIPTION] [ntext] NULL,
	[ASHMOREEMM_INDUSTRY_ANALYST] [nvarchar](255) NULL,
	[ASHMOREEMM_PRIMARY_ANALYST] [nvarchar](255) NULL,
	[ASHMOREEMM_PORTFOLIO_MANAGER] [nvarchar](255) NULL,
	[WEBSITE] [nvarchar](255) NULL,
	[FISCAL_YEAR_END] [nvarchar](255) NULL,
	[XREF] [nvarchar](12) NULL,
	[REPORTNUMBER] [nvarchar](5) NULL,
	[GICS_SECTOR] [nvarchar](255) NULL,
	[GICS_SECTOR_NAME] [nvarchar](255) NULL,
	[GICS_INDUSTRY] [nvarchar](255) NULL,
	[GICS_INDUSTRY_NAME] [nvarchar](255) NULL,
	[GICS_SUB_INDUSTRY] [nvarchar](255) NULL,
	[GICS_SUB_INDUSTRY_NAME] [nvarchar](255) NULL,
	[SHARES_PER_ADR] [float] NULL,
	[ADR_UNDERLYING_TICKER] [nvarchar](255) NULL,
	[MARKET_CAP_IN_TRADING_CURRENCY] [float] NULL,
	[CLOSING_PRICE] [float] NULL,
	[LAST_CLOSE_FX_QUO_CURR_TO_USD] [float] NULL,
	[LAST_CLOSE_DATE] [datetime] NULL,
	[TOT_CURR_SHRS_OUTST_ALL_CLASS] [float] NULL,
	[CHG_PCT_MTD] [nvarchar](100) NULL,
	[CHG_PCT_QTD] [nvarchar](100) NULL,
	[CHG_PCT_YTD] [nvarchar](100) NULL,
	[CHG_PCT_1YR] [nvarchar](100) NULL,
 CONSTRAINT [PK_GF_SECURITY_BASEVIEW] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [GF_SECURITY_BASEVIEW_idx] ON [dbo].[GF_SECURITY_BASEVIEW] 
(
	[ISSUER_ID] ASC,
	[SECURITY_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [GF_SECURITY_BASEVIEW_idx2] ON [dbo].[GF_SECURITY_BASEVIEW] 
(
	[SECURITY_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [GF_SECURITY_BASEVIEW_idx3] ON [dbo].[GF_SECURITY_BASEVIEW] 
(
	[ISSUER_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [GF_SECURITY_BASEVIEW_idx4] ON [dbo].[GF_SECURITY_BASEVIEW] 
(
	[REPORTNUMBER] ASC
)
INCLUDE ( [ISSUER_ID]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [GF_SECURITY_BASEVIEW_idx5] ON [dbo].[GF_SECURITY_BASEVIEW] 
(
	[ISSUER_ID] ASC
)
INCLUDE ( [XREF]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
