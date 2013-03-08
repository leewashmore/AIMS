/****** Object:  Table [dbo].[SCREENING_DISPLAY_CURRENT]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SCREENING_DISPLAY_CURRENT](
	[SCREENING_ID] [varchar](50) NOT NULL,
	[DATA_ID] [int] NOT NULL,
	[ESTIMATE_ID] [int] NULL,
	[MULTIPLIER] [decimal](32, 6) NULL,
	[DECIMAL] [int] NULL,
	[PERCENTAGE] [char](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
