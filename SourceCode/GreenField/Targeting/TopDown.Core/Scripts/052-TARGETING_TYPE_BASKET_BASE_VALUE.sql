﻿CREATE TABLE [dbo].[TARGETING_TYPE_BASKET_BASE_VALUE](
	[TARGETING_TYPE_ID] [int] NOT NULL,
	[BASKET_ID] [int] NOT NULL,
	[BASE_VALUE] [decimal](32, 6) NOT NULL,
	[CHANGE_ID] [int] NOT NULL,
 CONSTRAINT [PK_TARGETING_TYPE_BASKET_BASE_VALUE] PRIMARY KEY CLUSTERED 
(
	[TARGETING_TYPE_ID] ASC,
	[BASKET_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_BASE_VALUE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_BASKET_BASE_VALUE_BASKET] FOREIGN KEY([BASKET_ID])
REFERENCES [dbo].[BASKET] ([ID])
GO

ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_BASE_VALUE] CHECK CONSTRAINT [FK_TARGETING_TYPE_BASKET_BASE_VALUE_BASKET]
GO

ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_BASE_VALUE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_BASKET_BASE_VALUE_TARGETING_TYPE] FOREIGN KEY([TARGETING_TYPE_ID])
REFERENCES [dbo].[TARGETING_TYPE] ([ID])
GO

ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_BASE_VALUE] CHECK CONSTRAINT [FK_TARGETING_TYPE_BASKET_BASE_VALUE_TARGETING_TYPE]
GO

ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_BASE_VALUE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_BASKET_BASE_VALUE_TARGETING_TYPE_BASKET_BASE_VALUE_CHANGE] FOREIGN KEY([CHANGE_ID])
REFERENCES [dbo].[TARGETING_TYPE_BASKET_BASE_VALUE_CHANGE] ([ID])
GO

ALTER TABLE [dbo].[TARGETING_TYPE_BASKET_BASE_VALUE] CHECK CONSTRAINT [FK_TARGETING_TYPE_BASKET_BASE_VALUE_TARGETING_TYPE_BASKET_BASE_VALUE_CHANGE]
GO
