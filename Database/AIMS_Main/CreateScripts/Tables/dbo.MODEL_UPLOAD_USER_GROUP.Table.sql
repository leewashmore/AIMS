/****** Object:  Table [dbo].[MODEL_UPLOAD_USER_GROUP]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MODEL_UPLOAD_USER_GROUP](
	[MANAGER_NAME] [varchar](50) NOT NULL,
	[ANALYST_NAME] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MODEL_UPLOAD_USER_GROUP] PRIMARY KEY CLUSTERED 
(
	[MANAGER_NAME] ASC,
	[ANALYST_NAME] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
