/****** Object:  Default [DF__aspnet_Us__UserI__0EA330E9]    Script Date: 03/08/2013 11:18:58 ******/
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (newid()) FOR [UserId]
GO
