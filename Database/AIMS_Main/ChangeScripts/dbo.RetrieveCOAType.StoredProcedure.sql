SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[RetrieveCOAType]
(
@ISSUER_ID VARCHAR(50)
)	
AS
BEGIN
	
	Select Top 1 * 
	from INTERNAL_ISSUER 
	where ISSUER_ID=@ISSUER_ID
END
GO
