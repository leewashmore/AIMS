USE [AIMS_Main]
GO
/****** Object:  StoredProcedure [dbo].[RetrieveAnalystModelUpdateStatus]    Script Date: 10/31/2014 13:37:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


ALTER procedure [dbo].[RetrieveAnalystModelUpdateStatus]
(
@portfolioId varchar(50),
@analyst varchar(50)
)
as
declare @pordate		date;	

if @portfolioId <> 'All'  and @analyst = 'All'
begin
select @pordate = max(portfolio_date) from gf_portfolio_ltholdings where portfolio_id = @portfolioId

select distinct  issuer_name,username,max(load_time) as LastUpdate from internal_model_load i
inner join gf_portfolio_ltholdings  p on p.issuer_id = i.issuer_id
inner join gf_Security_baseview s on s.issuer_id = i.issuer_id
where p.portfolio_id = @portfolioId and p.portfolio_Date = @pordate
and p.securitythemecode = 'EQUITY'
group by issuer_name,username

union
select distinct issuer_name,s.ASHMOREEMM_PRIMARY_ANALYST,null as LastUpdate from gf_portfolio_ltholdings  p
inner join gf_Security_baseview s on s.issuer_id = p.issuer_id
where portfolio_id = @portfolioId and portfolio_Date = @pordate
and securitythemecode = 'EQUITY' 
and p.issuer_id not in 
(
select distinct p.issuer_id from internal_model_load i
inner join gf_portfolio_ltholdings  p on p.issuer_id = i.issuer_id
inner join gf_Security_baseview s on s.issuer_id = i.issuer_id
where p.portfolio_id = @portfolioId and p.portfolio_Date =  @pordate	
and p.securitythemecode = 'EQUITY'
 )
end				
else if @portfolioId =  'All'  and @analyst <> 'All'

select distinct  issuer_name,username,max(load_time) as LastUpdate from internal_model_load i
inner join gf_Security_baseview s on s.issuer_id = i.issuer_id
inner join gf_portfolio_ltholdings  p on p.issuer_id = s.issuer_id
where 
 s.ASHMOREEMM_PRIMARY_ANALYST = @analyst
group by issuer_name,username
union

select distinct issuer_name,s.ASHMOREEMM_PRIMARY_ANALYST,null as LastUpdate from gf_portfolio_ltholdings  p
inner join gf_Security_baseview s on s.issuer_id = p.issuer_id
where portfolio_Date = (select max(portfolio_date) from gf_portfolio_ltholdings)
and securitythemecode = 'EQUITY' 
and s.ASHMOREEMM_PRIMARY_ANALYST=@analyst
and p.issuer_id not in 
(
select distinct p.issuer_id from internal_model_load i
inner join gf_portfolio_ltholdings  p on p.issuer_id = i.issuer_id
inner join gf_Security_baseview s on s.issuer_id = i.issuer_id
where p.portfolio_Date = (select max(portfolio_date) from gf_portfolio_ltholdings)
and p.securitythemecode = 'EQUITY'
 )




else -- ALL ALL
	select distinct  issuer_name,username,max(load_time) as LastUpdate from internal_model_load i
inner join gf_portfolio_ltholdings  p on p.issuer_id = i.issuer_id
inner join gf_Security_baseview s on s.issuer_id = i.issuer_id
where /*p.portfolio_id = @portfolioId and*/ p.portfolio_Date = (select max(portfolio_date) from gf_portfolio_ltholdings)
/*and s.ASHMOREEMM_PRIMARY_ANALYST=@analyst*/
and p.securitythemecode = 'EQUITY'
group by issuer_name,username

union
select distinct issuer_name,s.ASHMOREEMM_PRIMARY_ANALYST,null as LastUpdate from gf_portfolio_ltholdings  p
inner join gf_Security_baseview s on s.issuer_id = p.issuer_id
where /*portfolio_id = @portfolioId and*/ portfolio_Date = (select max(portfolio_date) from gf_portfolio_ltholdings)
and securitythemecode = 'EQUITY' 
/*and s.ASHMOREEMM_PRIMARY_ANALYST=@analyst*/
and p.issuer_id not in 
(
select distinct p.issuer_id from internal_model_load i
inner join gf_portfolio_ltholdings  p on p.issuer_id = i.issuer_id
inner join gf_Security_baseview s on s.issuer_id = i.issuer_id
where /*p.portfolio_id = @portfolioId and*/ p.portfolio_Date = (select max(portfolio_date) from gf_portfolio_ltholdings)
and p.securitythemecode = 'EQUITY'
 )

				
