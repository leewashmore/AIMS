SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[CLAIM_IDS](@sequenceName varchar(100), @howMany int) AS
BEGIN
	DECLARE @result int
	update SEQUENCE
		set
			@result = NEXT_AVAILABLE_ID,
			NEXT_AVAILABLE_ID = NEXT_AVAILABLE_ID + @howMany
		where Name = @sequenceName
	Select @result as AVAILABLE_ID
END
GO
