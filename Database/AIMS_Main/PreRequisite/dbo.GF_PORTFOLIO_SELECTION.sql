
/****** Object:  Table [dbo].[GFQ_PORTFOLIO_SELECTION]    Script Date: 11/26/2013 16:40:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GF_PORTFOLIO_SELECTION](
	[GF_ID] [float] NOT NULL,
	[PORTFOLIO_ID] [nvarchar](10) NULL,
	[PORTFOLIO_THEME_SUBGROUP_CODE] [nvarchar](10) NULL,
	[PORTFOLIO_THEME_SUBGROUP_NAME] [nvarchar](50) NULL
) ON [PRIMARY]

GO

--drop table [GF_PORTFOLIO_SELECTION]

ALTER TABLE [dbo].[GF_PORTFOLIO_SELECTION] ADD  CONSTRAINT [PK_GF_PORTFOLIO_SELECTION] PRIMARY KEY CLUSTERED 
(
	[GF_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO