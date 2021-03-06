--ADD calc 301
delete from .dbo.CALC_LIST where CALC_NUM = 301
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(301, 1, 'Book Value BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 301
insert into .dbo.DATA_MASTER
values (301,'','NONE','N','Y','Y','Y','Y','N','Book Value BF12','','N','Y','','','Y',NULL,NULL)

--ADD calc 302
delete from .dbo.CALC_LIST where CALC_NUM = 302
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(302, 2, 'Dividend Yield BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 302
insert into .dbo.DATA_MASTER
values (302,'','NONE','N','Y','Y','Y','Y','N','Dividend Yield BF12','','N','Y','','','Y',NULL,NULL)

--ADD calc 303
delete from .dbo.CALC_LIST where CALC_NUM = 303
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(303, 1, 'Dividends BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 303
insert into .dbo.DATA_MASTER
values (303,'','NONE','N','Y','Y','Y','Y','N','Dividends BF12','','N','Y','','','Y',NULL,NULL)

--ADD calc 304
delete from .dbo.CALC_LIST where CALC_NUM = 304
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(304, 1, 'Earnings BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 304
insert into .dbo.DATA_MASTER
values (304,'','NONE','N','Y','Y','Y','Y','N','Earnings BF12','','N','Y','','','Y',NULL,NULL)

--ADD calc 305
delete from .dbo.CALC_LIST where CALC_NUM = 305
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(305, 3, 'EV/EBITDA BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 305
insert into .dbo.DATA_MASTER
values (305,'','NONE','N','Y','N','N','Y','N','EV/EBITDA BF12','','N','Y','','','Y',NULL,NULL)

--ADD calc 306
delete from .dbo.CALC_LIST where CALC_NUM = 306
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(306, 2, 'P/BV BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 306
insert into .dbo.DATA_MASTER
values (306,'','NONE','N','Y','Y','Y','Y','N','P/BV BF12','','N','Y','','','Y',0,10)

--ADD calc 307
delete from .dbo.CALC_LIST where CALC_NUM = 307
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(307, 2, 'P/CE BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 307
insert into .dbo.DATA_MASTER
values (307,'','NONE','N','Y','Y','Y','Y','N','P/CE BF12','','N','Y','','','Y',NULL,NULL)

--ADD calc 308
delete from .dbo.CALC_LIST where CALC_NUM = 308
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(308, 2, 'P/E BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 308
insert into .dbo.DATA_MASTER
values (308,'','NONE','N','Y','Y','Y','Y','N','P/E BF12','','N','Y','','','Y',0,60)

--ADD calc 309
delete from .dbo.CALC_LIST where CALC_NUM = 309
insert into .dbo.calc_list(CALC_NUM,CALC_SEQ,CALC_NAME,ACTIVE,PRICEBASED)
values(309, 2, 'ROE BF12', 'Y' , 'Y')

delete from .dbo.DATA_MASTER where DATA_ID = 309
insert into .dbo.DATA_MASTER
values (309,'','NONE','N','Y','Y','Y','Y','N','ROE BF12','','N','Y','','','Y',NULL,NULL)



--Update description for existing forward calculations to read BF24
update .dbo.CALC_LIST 
set calc_name = 'Book Value BF24' where calc_num = 280

update .dbo.CALC_LIST 
set calc_name ='Dividend Yield BF24' where calc_num = 236

update .dbo.CALC_LIST 
set calc_name ='Dividends BF24' where calc_num = 300

update .dbo.CALC_LIST 
set calc_name ='Earnings BF24' where calc_num = 279

update .dbo.CALC_LIST 
set calc_name ='EV/EBITDA BF24' where calc_num =198

update .dbo.CALC_LIST 
set calc_name ='P/BV BF24' where calc_num =188

update .dbo.CALC_LIST 
set calc_name ='P/CE BF24' where calc_num =189

update .dbo.CALC_LIST 
set calc_name ='P/E BF24' where calc_num =187

update .dbo.CALC_LIST 
set calc_name ='ROE BF24' where calc_num =200


update .dbo.DATA_MASTER 
set DATA_DESC = 'Book Value BF24' where DATA_ID = 280

update .dbo.DATA_MASTER	
set DATA_DESC ='Dividend Yield BF24' where DATA_ID = 236

update .dbo.DATA_MASTER
set DATA_DESC ='Dividends BF24' where DATA_ID = 300

update .dbo.DATA_MASTER
set DATA_DESC ='Earnings BF24' where DATA_ID = 279

update .dbo.DATA_MASTER
set DATA_DESC ='EV/EBITDA BF24' where DATA_ID =198

update .dbo.DATA_MASTER
set DATA_DESC ='P/BV BF24' where DATA_ID =188

update .dbo.DATA_MASTER
set DATA_DESC ='P/CE BF24' where DATA_ID =189

update .dbo.DATA_MASTER
set DATA_DESC ='P/E BF24' where DATA_ID =187

update .dbo.DATA_MASTER
set DATA_DESC ='ROE BF24' where DATA_ID =200




--Update seq for 24M 
update .dbo.CALC_LIST 
set CALC_SEQ = 2 where CALC_NUM in (236,188,187,200,189) 

update .dbo.CALC_LIST 
set PRICEBASED = 'Y' where CALC_NUM in (280,236,300,279,198,188,189,187,200)


