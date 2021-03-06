SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[usp_RetrieveDCFAnalysisSummaryData](
                @ISSUER_ID                                      varchar(20)                    -- The company identifier
,               @DATA_SOURCE                             varchar(10)  = 'PRIMARY'              -- REUTERS, PRIMARY, INDUSTRY
,               @PERIOD_TYPE                                char(2) = 'C'                      -- A, Q
,               @FISCAL_TYPE                                char(8) = 'FISCAL'                 -- FISCAL, CALENDAR
,               @CURRENCY                                      char(3)  = 'USD'                -- USD or the currency of the country (local)
)
AS
BEGIN
                -- SET NOCOUNT ON added to prevent extra result sets from
                -- interfering with SELECT statements.
                SET NOCOUNT ON;

    -- Insert statements for procedure here
                SELECT *
                FROM PERIOD_FINANCIALS pf
                WHERE
                pf.ISSUER_ID=@ISSUER_ID AND
                pf.DATA_SOURCE=@DATA_SOURCE AND
                pf.PERIOD_TYPE=@PERIOD_TYPE AND
                pf.DATA_ID in (256,289) AND
                pf.CURRENCY=@CURRENCY
END
GO
