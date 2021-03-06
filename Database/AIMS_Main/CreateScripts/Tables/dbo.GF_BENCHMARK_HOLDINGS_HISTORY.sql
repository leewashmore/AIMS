

/****** Object:  Table [dbo].[GF_BENCHMARK_HOLDINGS_HISTORY]    Script Date: 05/01/2013 08:59:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [dbo].[GF_BENCHMARK_HOLDINGS_HISTORY](
	
	[EFFECTIVE_DATE] [datetime] NULL,
	[BENCHMARK_ID] [nvarchar](50) NULL,
	[ISSUER_ID] [nvarchar](16) NULL,
	[ASEC_SEC_SHORT_NAME] [nvarchar](16) NULL,
	[BENCHMARK_WEIGHT] [decimal](32, 6) NULL,
	[FLOAT_FACTOR] [decimal](32, 6) NULL,
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [GF_BENCHMARK_HOLDINGS_HISTORY_idx] ON [dbo].[GF_BENCHMARK_HOLDINGS_HISTORY] 
(
	[EFFECTIVE_DATE] ASC,
	[BENCHMARK_ID] ASC,
	[ISSUER_ID] ASC,
	[ASEC_SEC_SHORT_NAME] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

