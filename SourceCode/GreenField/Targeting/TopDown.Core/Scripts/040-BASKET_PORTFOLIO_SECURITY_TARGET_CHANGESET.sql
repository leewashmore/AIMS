﻿CREATE TABLE [dbo].[BASKET_PORTFOLIO_SECURITY_TARGET_CHANGESET](
	[ID] [int] NOT NULL,
	[USERNAME] [varchar](50) NOT NULL,
	[TIMESTAMP] [datetime] NOT NULL,
	[CALCULATION_ID] [int] NOT NULL
 CONSTRAINT [PK_BASKET_PORTFOLIO_SECURITY_TARGET_CHANGESET] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[BASKET_PORTFOLIO_SECURITY_TARGET_CHANGESET]
	WITH CHECK ADD  CONSTRAINT [FK_BASKET_PORTFOLIO_SECURITY_TARGET_CHANGESET_TARGETING_CALCULATION]
	FOREIGN KEY([CALCULATION_ID])
REFERENCES [dbo].[TARGETING_CALCULATION] ([ID])
GO

ALTER TABLE [dbo].[BASKET_PORTFOLIO_SECURITY_TARGET_CHANGESET] CHECK CONSTRAINT [FK_BASKET_PORTFOLIO_SECURITY_TARGET_CHANGESET_TARGETING_CALCULATION]
GO

INSERT INTO [dbo].[BASKET_PORTFOLIO_SECURITY_TARGET_CHANGESET]
           ([ID],[USERNAME],[TIMESTAMP],[CALCULATION_ID])
     VALUES (0,'bykova',GETDATE(), 0)
GO

