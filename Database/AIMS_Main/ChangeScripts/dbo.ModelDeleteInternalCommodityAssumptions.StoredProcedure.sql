SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[ModelDeleteInternalCommodityAssumptions] 
	(
		@ISSUER_ID VARCHAR(20),
		@REF_NO VARCHAR(50)
	)
AS
BEGIN

	SET NOCOUNT ON;

DELETE FROM INTERNAL_COMMODITY_ASSUMPTIONS 
WHERE ISSUER_ID=@ISSUER_ID AND REF_NO=@REF_NO
    
END
GO
