/****** Object:  Table [dbo].[tblBenchmarkPreference]    Script Date: 03/08/2013 10:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblBenchmarkPreference](
	[UserId] [varchar](50) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[BenchmarkName] [nvarchar](max) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
