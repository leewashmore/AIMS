
set noexec off

--declare  current and required version
--also do it an the end of the script
declare @RequiredDBVersion as nvarchar(100) = '00013'
declare @CurrentScriptVersion as nvarchar(100) = '00014'

--if current version already in DB, just skip
if exists(select 1 from ChangeScripts  where ScriptVersion = @CurrentScriptVersion)
 set noexec on 

--check that current DB version is Ok
declare @DBCurrentVersion as nvarchar(100) = (select top 1 ScriptVersion from ChangeScripts order by DateExecuted desc)
if (@DBCurrentVersion != @RequiredDBVersion)
begin
	RAISERROR(N'DB version is "%s", required "%s".', 16, 1, @DBCurrentVersion, @RequiredDBVersion)
	set noexec on
end

GO

IF OBJECT_ID ('[dbo].[GetBasicData]') IS NOT NULL
	DROP PROCEDURE [dbo].[GetBasicData]
GO
CREATE PROCEDURE [dbo].[GetBasicData] (
@SecurityID varchar(20)
 )	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
declare @MarketCapitalization decimal 
declare @EnterpriseValue decimal


    -- Insert statements for procedure here
	SELECT @MarketCapitalization = AMOUNT 
	from dbo.PERIOD_FINANCIALS 
	where
	SECURITY_ID = @SecurityID and 
	DATA_ID = 185  and
	CURRENCY = 'USD' and
	PERIOD_TYPE = 'C'
	
	SELECT  @EnterpriseValue = AMOUNT 
	from dbo.PERIOD_FINANCIALS 
	where
	SECURITY_ID = @SecurityID and 
	DATA_ID = 186  and
	CURRENCY = 'USD' and
	PERIOD_TYPE = 'C'
	
	SELECT  @MarketCapitalization as MARKET_CAPITALIZATION ,@EnterpriseValue as ENTERPRISE_VALUE
END


GO
--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00014'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())