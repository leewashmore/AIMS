﻿CREATE TABLE [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE](
	[ID] [int] NOT NULL,
	[CHANGESET_ID] [int] NOT NULL,
	[TARGETING_TYPE_ID] [int] NOT NULL,
	[BASKET_ID] [int] NOT NULL,
	[SECURITY_ID] [varchar](20) NOT NULL,
	[BASE_VALUE_BEFORE] [decimal](32, 6) NULL,
	[BASE_VALUE_AFTER] [decimal](32, 6) NULL,
	[COMMENT] [ntext] NOT NULL,
 CONSTRAINT [PK_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE_BASKET] FOREIGN KEY([BASKET_ID])
REFERENCES [dbo].[BASKET] ([ID])
GO

ALTER TABLE [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] CHECK CONSTRAINT [FK_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE_BASKET]
GO

ALTER TABLE [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE_TARGETING_TYPE_GROUP] FOREIGN KEY([TARGETING_TYPE_ID])
REFERENCES [dbo].[TARGETING_TYPE_GROUP] ([ID])
GO

ALTER TABLE [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] CHECK CONSTRAINT [FK_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE_TARGETING_TYPE_GROUP]
GO

ALTER TABLE [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE]  WITH CHECK ADD  CONSTRAINT [FK_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGESET] FOREIGN KEY([CHANGESET_ID])
REFERENCES [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGESET] ([ID])
GO

ALTER TABLE [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] CHECK CONSTRAINT [FK_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE_TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGESET]
GO

insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (000, 0, 0, 5, '17227', 0.1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (001, 0, 0, 5, '17264', 0.052, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (002, 0, 0, 5, '144801', 0.063, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (003, 0, 0, 5, '44625', 0.079, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (004, 0, 0, 5, '145175', 0.069, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (005, 0, 0, 5, '40594', 0.172, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (006, 0, 0, 5, '146426', 0.044, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (007, 0, 0, 23, '2285', 0.06, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (008, 0, 0, 23, '2330', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (009, 0, 0, 23, '2308', 0.06, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (010, 0, 0, 23, '145119', 0.04, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (011, 0, 0, 23, '17357', 0.125, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (012, 0, 0, 23, '109291', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (013, 0, 0, 23, '146934', 0.05, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (014, 0, 0, 23, '2287', 0.105, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (015, 0, 0, 23, '17340', 0.12, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (016, 0, 0, 23, '163612', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (017, 0, 0, 10, '45603', 0.4, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (018, 0, 0, 10, '17414', 0.6, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (019, 0, 0, 15, '2323', 0.12, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (020, 0, 0, 15, '2301', 0.15, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (021, 0, 0, 15, '2340', 0.16, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (022, 0, 0, 20, '152464', 1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (023, 0, 0, 3, '17643', 0.06, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (024, 0, 0, 3, '17669', 0.05, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (025, 0, 0, 3, '17621', 0.06, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (026, 0, 0, 3, '17622', 0.202, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (027, 0, 0, 3, '6964', 0.268, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (028, 0, 0, 3, '17672', 0.05, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (029, 0, 0, 3, '17656', 0.06, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (030, 0, 0, 3, '2283', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (031, 0, 0, 17, '17807', 0.75, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (032, 0, 0, 6, '33924', 0.1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (033, 0, 0, 6, '75690', 0.21, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (034, 0, 0, 6, '17769', 0.11, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (035, 0, 0, 6, '17788', 0.1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (036, 0, 0, 18, '17902', 1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (037, 0, 0, 11, '2365', 0.24, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (038, 0, 0, 11, '2362', 0.12, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (039, 0, 0, 11, '158637', 0.43, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (040, 0, 0, 14, '8020', 0.28, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (041, 0, 0, 14, '18260', 0.18, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (042, 0, 0, 14, '18279', 0.18, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (043, 0, 0, 14, '18259', 0.18, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (044, 0, 0, 4, '18187', 0.033, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (045, 0, 0, 4, '18119', 0.044, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (046, 0, 0, 4, '18140', 0.056, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (047, 0, 0, 4, '2291', 0.144, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (048, 0, 0, 4, '18206', 0.022, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (049, 0, 0, 4, '18192', 0.238, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (050, 0, 0, 4, '20616', 0.167, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (051, 0, 0, 4, '18127', 0.078, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (052, 0, 0, 16, '145117', 0.3, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (053, 0, 0, 16, '167751', 0.4, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (054, 0, 0, 16, '18064', 0.3, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (055, 0, 0, 13, '18913', 1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (056, 0, 0, 8, '32026', 1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (057, 0, 0, 22, '152461', 1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (058, 0, 0, 21, '152462', 1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (059, 0, 0, 3, '17602', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (060, 0, 0, 6, '104524', 0.12, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (061, 0, 0, 6, '145184', 0.12, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (062, 0, 0, 6, '25257', 0.13, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (063, 0, 0, 5, '17286', 0.106, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (064, 0, 0, 3, '2282', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (065, 0, 0, 5, '86173', 0.038, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (066, 0, 0, 23, '69802', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (067, 0, 0, 5, '2996', 0.038, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (068, 0, 0, 12, '178533', 0.13, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (069, 0, 0, 7, '17316', 0.2, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (070, 0, 0, 3, '144034', 0.04, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (071, 0, 0, 7, '177457', 0.8, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (072, 0, 0, 15, '225484', 0.29, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (073, 0, 0, 6, '231374', 0.11, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (074, 0, 0, 11, '21545', 0.08, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (075, 0, 0, 15, '17473', 0.12, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (076, 0, 0, 23, '17405', 0.045, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (077, 0, 0, 23, '240592', 0.04, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (078, 0, 0, 12, '18097', 0.42, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (079, 0, 0, 12, '65047', 0.3, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (080, 0, 0, 12, '18102', 0.15, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (081, 0, 0, 11, '2995', 0.05, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (082, 0, 0, 3, '17630', 0.06, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (083, 0, 0, 4, '178666', 0.028, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (084, 0, 0, 23, '16383', 0.05, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (085, 0, 0, 15, '9242', 0.16, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (086, 0, 0, 23, '59744', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (087, 0, 0, 23, '21812', 0.04, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (088, 0, 0, 4, '18109', 0.134, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (089, 0, 0, 4, '50333', 0.056, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (090, 0, 0, 3, '17605', 0.06, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (091, 0, 0, 19, '20779', 1, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (092, 0, 0, 23, '17365', 0.035, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (093, 0, 0, 5, '24696', 0.038, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (094, 0, 0, 5, '17266', 0.036, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (095, 0, 0, 5, '17285', 0.071, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (096, 0, 0, 5, '5684', 0.094, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (097, 0, 0, 23, '17396', 0.05, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (098, 0, 0, 23, '17325', 0.03, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (099, 0, 0, 14, '18252', 0.18, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (100, 0, 0, 11, '23001', 0.08, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (101, 0, 0, 17, '142535', 0.25, '')
insert into [dbo].[TARGETING_TYPE_GROUP_BASKET_SECURITY_BASE_VALUE_CHANGE] ([ID], [CHANGESET_ID], [TARGETING_TYPE_ID], [BASKET_ID], [SECURITY_ID], [BASE_VALUE_AFTER], [COMMENT]) values (102, 0, 0, 9, '136740', 1, '')
