/****** Object:  Table [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE](
	[ID] [int] NOT NULL,
	[TARGETING_TYPE_ID] [int] NOT NULL,
	[BASKET_ID] [int] NOT NULL,
	[PORTFOLIO_ID] [varchar](20) NOT NULL,
	[TARGET_BEFORE] [decimal](32, 6) NULL,
	[TARGET_AFTER] [decimal](32, 6) NULL,
	[CHANGESET_ID] [int] NOT NULL,
	[COMMENT] [ntext] NOT NULL,
 CONSTRAINT [PK_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE_BASKET] FOREIGN KEY([BASKET_ID])
REFERENCES [dbo].[BASKET] ([ID])
GO
ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE] CHECK CONSTRAINT [FK_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE_BASKET]
GO
ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE_TARGETING_TYPE] FOREIGN KEY([TARGETING_TYPE_ID])
REFERENCES [dbo].[TARGETING_TYPE] ([ID])
GO
ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE] CHECK CONSTRAINT [FK_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE_TARGETING_TYPE]
GO
ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGESET] FOREIGN KEY([CHANGESET_ID])
REFERENCES [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGESET] ([ID])
GO
ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE] CHECK CONSTRAINT [FK_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGE_TARGETING_TYPE_BASKET_PORTFOLIO_TARGET_CHANGESET]
GO
