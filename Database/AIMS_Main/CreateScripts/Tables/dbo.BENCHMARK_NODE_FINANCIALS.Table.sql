/****** Object:  Table [dbo].[BENCHMARK_NODE_FINANCIALS]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BENCHMARK_NODE_FINANCIALS](
	[BENCHMARK_ID] [varchar](50) NOT NULL,
	[NODE_NAME1] [varchar](50) NULL,
	[NODE_ID1] [varchar](50) NULL,
	[NODE_NAME2] [varchar](50) NULL,
	[NODE_ID2] [varchar](50) NULL,
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
CREATE NONCLUSTERED INDEX [BENCHMARK_NODE_FINANCIALS_idx2] ON [dbo].[BENCHMARK_NODE_FINANCIALS] 
(
	[BENCHMARK_ID] ASC,
	[NODE_NAME1] ASC,
	[NODE_ID1] ASC,
	[DATA_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
