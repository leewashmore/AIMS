/****** Object:  Table [dbo].[MeetingConfigurationSchedule]    Script Date: 03/08/2013 10:53:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MeetingConfigurationSchedule](
	[PresentationDay] [varchar](50) NULL,
	[PresentationTime] [datetime] NOT NULL,
	[PresentationTimeZone] [varchar](50) NOT NULL,
	[PresentationDeadlineDay] [varchar](50) NULL,
	[PresentationDeadlineTime] [datetime] NOT NULL,
	[PreMeetingVotingDeadlineDay] [varchar](50) NULL,
	[PreMeetingVotingDeadlineTime] [datetime] NOT NULL,
	[ConfigurablePresentationDeadline] [decimal](18, 2) NULL,
	[ConfigurablePreMeetingVotingDeadline] [decimal](18, 2) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
