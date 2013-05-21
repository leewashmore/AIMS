

/****** Object:  Table [dbo].[GF_PORTFOLIO_LTHOLDINGS_HISTORY]    Script Date: 05/01/2013 11:32:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GF_PORTFOLIO_LTHOLDINGS_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[GF_PORTFOLIO_LTHOLDINGS_HISTORY]
GO



/****** Object:  Table [dbo].[GF_PORTFOLIO_LTHOLDINGS_HISTORY]    Script Date: 05/01/2013 11:32:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GF_PORTFOLIO_LTHOLDINGS_HISTORY](
	[ID] BIGINT IDENTITY,
	[PORTFOLIO_DATE] [datetime] NULL,
	[PORTFOLIO_ID] [varchar](10) NULL,
	[PORTFOLIO_CURRENCY] [varchar](50) NULL,
	[BENCHMARK_ID] [varchar](50) NULL,
	[ISSUER_ID] [varchar](16) NULL,
	[ASEC_SEC_SHORT_NAME] [varchar](16) NULL,
	[SECURITYTHEMECODE] [varchar](10) NULL,
	[BALANCE_NOMINAL] [decimal](22, 8) NULL,
	[DIRTY_PRICE] [decimal](22, 8) NULL,
	[TRADING_CURRENCY] [varchar](3) NULL,
	[DIRTY_VALUE_PC] [decimal](22, 8) NULL,
) 

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Index [GF_PORTFOLIO_LTHOLDINGS_HISTORY_idx]    Script Date: 05/01/2013 12:14:47 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[GF_PORTFOLIO_LTHOLDINGS_HISTORY]') AND name = N'GF_PORTFOLIO_LTHOLDINGS_HISTORY_idx')
DROP INDEX [GF_PORTFOLIO_LTHOLDINGS_HISTORY_idx] ON [dbo].[GF_PORTFOLIO_LTHOLDINGS_HISTORY] WITH ( ONLINE = OFF )
GO


/****** Object:  Index [GF_PORTFOLIO_LTHOLDINGS_HISTORY_idx]    Script Date: 05/01/2013 12:14:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [GF_PORTFOLIO_LTHOLDINGS_HISTORY_idx] ON [dbo].[GF_PORTFOLIO_LTHOLDINGS_HISTORY] 
(
	[PORTFOLIO_DATE] ASC,
	[PORTFOLIO_ID] ASC,
	[ISSUER_ID] ASC,
	[ASEC_SEC_SHORT_NAME] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

