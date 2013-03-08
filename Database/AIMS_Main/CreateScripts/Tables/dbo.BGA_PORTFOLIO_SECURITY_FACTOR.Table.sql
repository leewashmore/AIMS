/****** Object:  Table [dbo].[BGA_PORTFOLIO_SECURITY_FACTOR]    Script Date: 03/08/2013 11:10:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[BGA_PORTFOLIO_SECURITY_FACTOR](
	[PORTFOLIO_ID] [varchar](20) NOT NULL,
	[SECURITY_ID] [varchar](20) NOT NULL,
	[FACTOR] [decimal](32, 6) NOT NULL,
	[CHANGE_ID] [int] NOT NULL,
 CONSTRAINT [PK_BGA_PORTFOLIO_SECURITY_FACTOR] PRIMARY KEY CLUSTERED 
(
	[PORTFOLIO_ID] ASC,
	[SECURITY_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[BGA_PORTFOLIO_SECURITY_FACTOR]  WITH CHECK ADD  CONSTRAINT [FK_BGA_PORTFOLIO_SECURITY_FACTOR_BGA_PORTFOLIO_SECURITY_FACTOR_CHANGE] FOREIGN KEY([CHANGE_ID])
REFERENCES [dbo].[BGA_PORTFOLIO_SECURITY_FACTOR_CHANGE] ([ID])
GO
ALTER TABLE [dbo].[BGA_PORTFOLIO_SECURITY_FACTOR] CHECK CONSTRAINT [FK_BGA_PORTFOLIO_SECURITY_FACTOR_BGA_PORTFOLIO_SECURITY_FACTOR_CHANGE]
GO
