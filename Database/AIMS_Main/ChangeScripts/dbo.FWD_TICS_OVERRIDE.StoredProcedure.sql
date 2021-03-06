IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FWD_TICS_OVERRIDE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FWD_TICS_OVERRIDE]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

create procedure [dbo].[FWD_TICS_OVERRIDE]
as
------------------------------------------------------------------------
-- Purpose:	This procedure updates the SpecialCurrenciesFXRates table for 
--		currencies that need special FWD_RATE_TICS values to override what
--		is coming from SCD
--
-- Author:	Justin M (built by Lane)
-- Date:	June 21, 2013
------------------------------------------------------------------------

declare @FXDate varchar(11)
declare @FwdRate decimal(18,6) 

set @FXDate = cast(GETDATE()-1 as varchar(11))


--TZS in TICS table as 1, pull prior date value and carry forward in special processing FX rates table
select @FwdRate = fx.FX_RATE
from dbo.FX_RATES fx 
where fx.CURRENCY  = 'TZS'
and fx.FX_DATE =  cast(@FXDate as datetime)

--print cast(@FXDate as datetime)
--print @FwdRate

delete from SpecialCurrenciesFXRates
where CURRENCY_CROSS = 'TZS/USD'

insert into SpecialCurrenciesFXRates
values (1,
'TZS/USD',
'Yes',
@FXDate,
@FwdRate, 
Null,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate, 
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate,
@FwdRate)