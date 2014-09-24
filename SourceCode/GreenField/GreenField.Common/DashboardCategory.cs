﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GreenField.Common
{
    public enum DashboardCategoryType
    {
        MARKETS_SNAPSHOT_SUMMARY,
        MARKETS_SNAPSHOT_MARKET_PERFORMANCE,
        MARKETS_SNAPSHOT_INTERNAL_MODEL_VALUATION,
        MARKETS_MACROECONOMIC_EM_SUMMARY,
        MARKETS_MACROECONOMIC_COUNTRY_SUMMARY,
        MARKETS_COMMODITIES_SUMMARY,
        PORTFOLIO_SNAPSHOT,
        PORTFOLIO_HOLDINGS,
        PORTFOLIO_PERFORMANCE_SUMMARY,
        PORTFOLIO_PERFORMANCE_ATTRIBUTION,
        PORTFOLIO_PERFORMANCE_RELATIVE_PERFORMANCE,
        PORTFOLIO_BENCHMARK_SUMMARY,
        PORTFOLIO_BENCHMARK_COMPOSITION,
        PORTFOLIO_TARGETING_ASSET_ALLOCATION,
        PORTFOLIO_TARGETING_STOCK_SELECTION,
        PORTFOLIO_TARGETING_BOTTOM_UP,
        PORTFOLIO_MODELS_DIRECT_OVERLAY,
        COMPANY_SNAPSHOT_SUMMARY,
        COMPANY_SNAPSHOT_COMPANY_PROFILE,
        COMPANY_SNAPSHOT_TEAR_SHEET,
        COMPANY_FINANCIALS_SUMMARY,
        COMPANY_FINANCIALS_INCOME_STATEMENT,
        COMPANY_FINANCIALS_BALANCE_SHEET,
        COMPANY_FINANCIALS_CASH_FLOW,
        COMPANY_FINANCIALS_FINSTAT,
        COMPANY_FINANCIALS_PEER_COMPARISON,
        COMPANY_FINANCIALS_INVESTMENT_CONTEXT,
        COMPANY_ESTIMATES_CONSENSUS,
        COMPANY_ESTIMATES_DETAILED,
        COMPANY_ESTIMATES_COMPARISON,
        COMPANY_VALUATION_FAIR_VALUE,
        COMPANY_VALUATION_DCF,
        COMPANY_DOCUMENTS,
        COMPANY_CHARTING_PRICE_COMPARISON,
        COMPANY_CHARTING_UNREALIZED_GAIN_LOSS,
        COMPANY_CHARTING_CONTEXT,
        COMPANY_CHARTING_VALUATION,
        COMPANY_ISSUERSHARES_ISSUER_SHARES_COMPOSITION,
        COMPANY_CORPORATE_GOVERNANCE_QUESTIONNAIRE,
        COMPANY_CORPORATE_GOVERNANCE_REPORT,
        SCREENING_STOCK,
        SCREENING_QUARTERLY_COMPARISON,
        INVESTMENT_COMMITTEE_CREATE_EDIT,
        INVESTMENT_COMMITTEE_NEW_PRESENTATION,
        INVESTMENT_COMMITTEE_IC_PRESENTATION,
        INVESTMENT_COMMITTEE_IC_VOTE_DECISION,
        INVESTMENT_COMMITTEE_EDIT_PRESENTATION,
        INVESTMENT_COMMITTEE_PRESENTATION_CHANGE_DATE,
        INVESTMENT_COMMITTEE_PRESENTATIONS,
        INVESTMENT_COMMITTEE_VOTE,
        INVESTMENT_COMMITTEE_PRE_MEETING_REPORT,
        INVESTMENT_COMMITTEE_MEETING_MINUTES,
        INVESTMENT_COMMITTEE_SUMMARY_REPORT,
        INVESTMENT_COMMITTEE_METRICS_REPORT,
        ADMIN_INVESTMENT_COMMITTEE_VIEW_AGENDA,
        ADMIN_INVESTMENT_COMMITTEE_EDIT_DATE,
        ADMIN_INVESTMENT_COMMITTEE_MEETING_DETAILS,
        ADMIN_BROKER_RESEARCH,
        USER_DASHBOARD,
        MKT_CAP,
        COMPANY_SNAPSHOT_BASICDATA_SUMMARY
        
    }
}
