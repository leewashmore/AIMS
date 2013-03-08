/****** Object:  Table [dbo].[DATA_MASTER]    Script Date: 03/08/2013 10:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DATA_MASTER](
	[DATA_ID] [int] NOT NULL,
	[COA] [char](8) NULL,
	[FX_CONV_TYPE] [char](6) NOT NULL,
	[CALENDARIZE] [char](1) NOT NULL,
	[INDUSTRIAL] [char](1) NOT NULL,
	[BANK] [char](1) NOT NULL,
	[INSURANCE] [char](1) NOT NULL,
	[UTILITY] [char](1) NOT NULL,
	[QUARTERLY] [char](1) NOT NULL,
	[DATA_DESC] [varchar](100) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
