/****** Object:  Default [DF__aspnet_Ap__Appli__08EA5793]    Script Date: 03/08/2013 11:18:58 ******/
ALTER TABLE [dbo].[aspnet_Applications] ADD  DEFAULT (newid()) FOR [ApplicationId]
GO
