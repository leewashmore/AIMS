IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Update_Current_Cap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].Update_Current_Cap
GO

/****** Object:  StoredProcedure [dbo].[Update_Current_Cap]    Script Date: 05/01/2013 14:06:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

create procedure [dbo].[Update_Current_Cap]

as

	begin tran
		Merge dbo.MONITORING_DAILY_CAP_CHANGE as target
		using (select pf.SECURITY_ID, pf.AMOUNT, pf.CALCULATION_DIAGRAM 
			from .dbo.PERIOD_FINANCIALS pf
			where pf.DATA_ID = 185
			and pf.DATA_SOURCE = 'PRIMARY'
			and pf.CURRENCY = 'USD'
			and pf.PERIOD_TYPE = 'C') as source
		on (target.security_id = source.security_id)
		when matched then
			update set 
			target.CURR_CAP 		= source.amount ,
			target.CURR_DIAGRAM 		= source.CALCULATION_DIAGRAM					
		when not matched  then
		
			INSERT(SECURITY_ID,
			CURR_DATE,
			PRIOR_CAP,
			PRIOR_DIAGRAM,
			CURR_CAP,
			CURR_DIAGRAM
			)
			VALUES
			(	source.SECURITY_ID ,
				getdate() ,
				0 ,
				"",
				source.amount ,
				source.calculation_diagram
				);
	commit tran;	
GO
