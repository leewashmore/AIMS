/****** Object:  Table [dbo].[tblMarketSnapshotGroupPreference]    Script Date: 03/08/2013 10:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMarketSnapshotGroupPreference](
	[GroupPreferenceId] [int] IDENTITY(1,1) NOT NULL,
	[SnapshotPreferenceId] [int] NOT NULL,
	[GroupName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tblUserGroupPreference] PRIMARY KEY CLUSTERED 
(
	[GroupPreferenceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblMarketSnapshotGroupPreference]  WITH CHECK ADD  CONSTRAINT [FK_tblMarketSnapshotGroupPreference_tblMarketSnapshotPreference] FOREIGN KEY([SnapshotPreferenceId])
REFERENCES [dbo].[tblMarketSnapshotPreference] ([SnapshotPreferenceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblMarketSnapshotGroupPreference] CHECK CONSTRAINT [FK_tblMarketSnapshotGroupPreference_tblMarketSnapshotPreference]
GO
