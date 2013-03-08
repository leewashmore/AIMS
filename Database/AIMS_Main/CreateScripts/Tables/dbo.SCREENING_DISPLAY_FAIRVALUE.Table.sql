/****** Object:  Table [dbo].[SCREENING_DISPLAY_FAIRVALUE]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SCREENING_DISPLAY_FAIRVALUE](
	[SCREENING_ID] [varchar](50) NOT NULL,
	[DATA_DESC] [varchar](100) NOT NULL,
	[SHORT_COLUMN_DESC] [varchar](255) NOT NULL,
	[LONG_DESC] [varchar](max) NOT NULL,
	[TABLE_COLUMN] [varchar](50) NOT NULL,
	[DATA_TYPE] [varchar](50) NOT NULL,
	[MULTIPLIER] [decimal](32, 6) NULL,
	[DECIMAL] [int] NULL,
	[PERCENTAGE] [char](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
