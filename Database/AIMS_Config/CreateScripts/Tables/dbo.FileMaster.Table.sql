/****** Object:  Table [dbo].[FileMaster]    Script Date: 03/08/2013 10:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FileMaster](
	[FileID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NULL,
	[SecurityName] [varchar](255) NULL,
	[SecurityTicker] [varchar](50) NULL,
	[Location] [varchar](255) NULL,
	[MetaTags] [varchar](255) NULL,
	[Type] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Category] [varchar](50) NULL,
	[IssuerName] [varchar](50) NULL,
 CONSTRAINT [PK_FileMaster] PRIMARY KEY CLUSTERED 
(
	[FileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
