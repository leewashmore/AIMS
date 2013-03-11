/****** Object:  Table [dbo].[PORTFOLIO_FINANCIALS]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PORTFOLIO_FINANCIALS](
	[PORTFOLIO_ID] [varchar](50) NOT NULL,
	[PORTFOLIO_DATE] [datetime] NOT NULL,
	[DATA_ID] [int] NOT NULL,
	[PERIOD_TYPE] [char](2) NOT NULL,
	[PERIOD_YEAR] [int] NOT NULL,
	[CURRENCY] [char](3) NOT NULL,
	[AMOUNT] [decimal](32, 6) NOT NULL,
	[UPDATE_DATE] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [PORTFOLIO_FINANCIALS_idx] ON [dbo].[PORTFOLIO_FINANCIALS] 
(
	[PORTFOLIO_ID] ASC,
	[PORTFOLIO_DATE] ASC,
	[DATA_ID] ASC,
	[PERIOD_TYPE] ASC,
	[PERIOD_YEAR] ASC,
	[CURRENCY] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
