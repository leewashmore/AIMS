SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
alter procedure [dbo].[gh_Test]
	( 
	@ISSUER_ID			varchar(20)					-- The company identifier	
	)
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select id.*,ints.PERIOD_END_DATE, dm.DATA_ID, sb.REPORTNUMBER
	from INTERNAL_DATA id, INTERNAL_STATEMENT ints, GF_SECURITY_BASEVIEW sb, DATA_MASTER dm
	where id.ISSUER_ID = @issuer_id
	and id.COA IN ('VRUQ')
	and id.REF_NO = ints.REF_NO
	and id.ISSUER_ID = sb.ISSUER_ID
	and id.COA = dm.COA
	
END
GO
