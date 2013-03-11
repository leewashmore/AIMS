/****** Object:  Table [dbo].[SUMMARY_EM_DISPLAY]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SUMMARY_EM_DISPLAY](
	[SUMMARY_ID] [int] NOT NULL,
	[COUNTRY_CODE] [char](4) NULL,
	[GROUP_CODE] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [SUMMARY_EM_DISPLAY_idx] ON [dbo].[SUMMARY_EM_DISPLAY] 
(
	[SUMMARY_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
