set noexec off

--declare  current and required version
--also do it an the end of the script
declare @RequiredDBVersion as nvarchar(100) = '00129'
declare @CurrentScriptVersion as nvarchar(100) = '00130'

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

IF OBJECT_ID ('[dbo].[ModelInsertInternalCommodityAssumptions]') IS NOT NULL
	DROP PROCEDURE [dbo].[ModelInsertInternalCommodityAssumptions]
GO

/****** Object:  StoredProcedure [dbo].[ModelInsertInternalCommodityAssumptions]    Script Date: 08/07/2012 17:24:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ModelInsertInternalCommodityAssumptions] 
	(
		@ISSUER_ID VARCHAR(20),
		@REF_NO VARCHAR(20),
		@COMMODITY_ID VARCHAR(50),
		@AMOUNT DECIMAL(32,6)
	)
AS
BEGIN

	SET NOCOUNT ON;

INSERT INTO INTERNAL_COMMODITY_ASSUMPTIONS VALUES
(		@ISSUER_ID ,
		@REF_NO ,
		@COMMODITY_ID,
		@AMOUNT 
)
END
Go

--indicate thet current script is executed
declare @CurrentScriptVersion as nvarchar(100) = '00130'
insert into ChangeScripts (ScriptVersion, DateExecuted ) values (@CurrentScriptVersion, GETDATE())

