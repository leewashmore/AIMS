/****** Object:  Table [dbo].[TARGETING_CALCULATION]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TARGETING_CALCULATION](
	[ID] [int] NOT NULL,
	[STATUS_CODE] [int] NOT NULL,
	[QUEUED_ON] [datetime] NOT NULL,
	[STARTED_ON] [datetime] NULL,
	[FINISHED_ON] [datetime] NULL,
	[LOG] [ntext] NULL,
 CONSTRAINT [PK_TARGETING_CALCULATION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
