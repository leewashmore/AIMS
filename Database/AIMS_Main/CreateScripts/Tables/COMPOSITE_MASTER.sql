/****** Object:  Table [dbo].[COMPOSITE_MASTER]    Script Date: 04/18/2013 12:00:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COMPOSITE_MASTER](
	[COMPOSITE_ID] [nvarchar](255) NOT NULL,
	[NAME] [nvarchar](255) NULL,
	[BENCHMARK_ID] [nvarchar](255) NULL,
	[LOOK_THRU] [bit] NULL,
	[ACTIVE] [bit] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[COMPOSITE_MASTER] ([COMPOSITE_ID], [NAME], [BENCHMARK_ID], [LOOK_THRU], [ACTIVE]) VALUES (N'EQYALL', N'All Equity', N'MSCI EM IMI NET', NULL, 1)
INSERT [dbo].[COMPOSITE_MASTER] ([COMPOSITE_ID], [NAME], [BENCHMARK_ID], [LOOK_THRU], [ACTIVE]) VALUES (N'EQYBGA', N'All BGA', N'MSCI EM NET', 1, 1)
INSERT [dbo].[COMPOSITE_MASTER] ([COMPOSITE_ID], [NAME], [BENCHMARK_ID], [LOOK_THRU], [ACTIVE]) VALUES (N'EQYSMALL', N'All Small Cap', N'MSCI EM SC NET', NULL, 1)