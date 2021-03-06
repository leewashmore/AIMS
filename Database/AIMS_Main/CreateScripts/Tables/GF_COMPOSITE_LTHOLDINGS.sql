/****** Object:  Table [dbo].[GF_COMPOSITE_LTHOLDINGS]    Script Date: 04/18/2013 11:54:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GF_COMPOSITE_LTHOLDINGS](
	[GF_ID] [decimal](38, 0) NOT NULL,
	[PORTFOLIO_DATE] [datetime] NULL,
	[PORTFOLIO_ID] [varchar](10) NULL,
	[A_PFCHOLDINGS_PORLT] [varchar](10) NULL,
	[PORPATH] [varchar](500) NULL,
	[PORTFOLIO_THEME_SUBGROUP_CODE] [varchar](50) NULL,
	[PORTFOLIO_CURRENCY] [varchar](50) NULL,
	[BENCHMARK_ID] [varchar](50) NULL,
	[ISSUER_ID] [varchar](16) NULL,
	[ASEC_SEC_SHORT_NAME] [varchar](16) NULL,
	[ISSUE_NAME] [varchar](50) NULL,
	[TICKER] [varchar](30) NULL,
	[SECURITYTHEMECODE] [varchar](10) NULL,
	[A_SEC_INSTR_TYPE] [varchar](2000) NULL,
	[SECURITY_TYPE] [varchar](10) NULL,
	[BALANCE_NOMINAL] [decimal](22, 8) NULL,
	[DIRTY_PRICE] [decimal](22, 8) NULL,
	[TRADING_CURRENCY] [varchar](3) NULL,
	[DIRTY_VALUE_PC] [decimal](22, 8) NULL,
	[BENCHMARK_WEIGHT] [decimal](22, 8) NULL,
	[ASH_EMM_MODEL_WEIGHT] [decimal](22, 8) NULL,
	[MARKET_CAP_IN_USD] [decimal](22, 8) NULL,
	[ASHEMM_PROP_REGION_CODE] [varchar](10) NULL,
	[ASHEMM_PROP_REGION_NAME] [varchar](50) NULL,
	[ISO_COUNTRY_CODE] [varchar](3) NULL,
	[COUNTRYNAME] [varchar](50) NULL,
	[GICS_SECTOR] [varchar](50) NULL,
	[GICS_SECTOR_NAME] [varchar](60) NULL,
	[GICS_INDUSTRY] [varchar](50) NULL,
	[GICS_INDUSTRY_NAME] [varchar](60) NULL,
	[GICS_SUB_INDUSTRY] [varchar](50) NULL,
	[GICS_SUB_INDUSTRY_NAME] [varchar](60) NULL,
	[LOOK_THRU_FUND] [varchar](10) NULL,
 CONSTRAINT [PK_GF_COMPOSITE_LTHOLDINGS] PRIMARY KEY CLUSTERED 
(
	[GF_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


