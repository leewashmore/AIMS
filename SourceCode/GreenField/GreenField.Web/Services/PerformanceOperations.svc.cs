﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Resources;
using System.ServiceModel;
using System.ServiceModel.Activation;
using GreenField.DAL;
using GreenField.DataContracts;
using GreenField.Web.DataContracts;
using GreenField.Web.DimensionEntitiesService;
using GreenField.Web.Helpers;
using GreenField.Web.Helpers.Service_Faults;

namespace GreenField.Web.Services
{
    /// <summary>
    /// Service class for Performance Operations
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PerformanceOperations
    {
        #region PropertyDeclaration

        /*private Entities dimensionEntity;
        public Entities DimensionEntity
        {
            get
            {
                if (null == dimensionEntity)
                    dimensionEntity = new Entities(new Uri(ConfigurationManager.AppSettings["DimensionWebService"]));

                return dimensionEntity;
            }
        }*/

        private DimensionEntities dimensionEntity;
        public DimensionEntities DimensionEntity
        {
            get
            {
                if (null == dimensionEntity)
                {
                    dimensionEntity = new GreenField.DAL.DimensionEntities();
                }
                return dimensionEntity;
            }
        }

        #endregion

        #region FaultResourceManager

        public ResourceManager ServiceFaultResourceManager
        {
            get
            {
                return new ResourceManager(typeof(FaultDescriptions));
            }
        }

        #endregion

        #region PerformanceServices

        /// <summary>
        /// Service Method for RelativePerformanceUI gadget (Relative Performance)
        ///     *Results will be empty if WCF does not have data for the previous day
        /// </summary>
        /// <param name="objSelectedEntity">details of selected Portfolio & Security</param>
        /// <param name="objEffectiveDate">selected effective Date</param>
        /// <returns>List of RelativePerformanceUIData</returns>
        [OperationContract]
        public List<RelativePerformanceUIData> RetrieveRelativePerformanceUIData(Dictionary<string, string> objSelectedEntity, DateTime objEffectiveDate)
        {
            //objEffectiveDate = Convert.ToDateTime("7/5/2013"); //For Debugging puposes only. When running against older WCF, the results will likely be empty.  This is because the WCF will likely not contain previous business days data.  Can reset date here to debug.
            try
            {
                bool isServiceUp = false;
                if ((objSelectedEntity == null) || (objEffectiveDate == null) || (objSelectedEntity.Count == 0))
                {
                    return new List<RelativePerformanceUIData>();
                }
                //if dictionary object doesn't contains security/portfolio data, return empty set
                if (!objSelectedEntity.ContainsKey("SECURITY") || !objSelectedEntity.ContainsKey("PORTFOLIO"))
                {
                    return new List<RelativePerformanceUIData>();
                }

                List<RelativePerformanceUIData> result = new List<RelativePerformanceUIData>();
                //create new Entity for service
                DimensionEntities entity = DimensionEntity;
                List<string> countryName = new List<string>();
                List<string> benchmarkName = new List<string>();
                List<string> sectorName = new List<string>();
                string securityName = objSelectedEntity.Where(a => a.Key == "SECURITY").FirstOrDefault().Value;
                string portfolioName = objSelectedEntity.Where(a => a.Key == "PORTFOLIO").FirstOrDefault().Value;
                List<GreenField.DAL.GF_SECURITY_BASEVIEW> securityBaseData = (entity.GF_SECURITY_BASEVIEW.Where(a => a.ISSUE_NAME.ToUpper().Trim() == securityName.ToUpper().Trim()).ToList());
              /*  #region ServiceAvailabilityChecker

                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                {
                    throw new Exception("Data Services are not available");
                }
                #endregion*/

                //Reset the objEffectiveDate to the maxdate available to avoid days like weekends when no performace data is available 
                objEffectiveDate = (entity.GF_PERF_DAILY_ATTRIBUTION.Where(a => a.NODE_NAME == "GICS Level 1" && a.PORTFOLIO == "EMIF").Select(g => new { g.TO_DATE }).ToList()
                        .Select(x => x.TO_DATE.Value)).Distinct().Max();

                countryName = securityBaseData.Select(a => a.ASEC_SEC_COUNTRY_NAME).ToList();
                sectorName = securityBaseData.Select(a => a.GICS_SECTOR_NAME).ToList();
                benchmarkName = (entity.GF_PERF_DAILY_ATTRIBUTION.Where(a => a.PORTFOLIO.ToUpper().Trim() == portfolioName.ToUpper().Trim()
                    && a.BMNAME != null).Take(1).ToList()).Select(a => a.BMNAME).Distinct().ToList();
                if (benchmarkName == null)
                {
                    return result;
                }
                if (benchmarkName.Count != 1)
                {
                    return result;
                }
                if (countryName == null)
                {
                    return result;
                }
                if (countryName.Count != 1)
                {
                    return result;
                }
                if (sectorName == null)
                {
                    return result;
                }
                if (sectorName.Count != 1)
                {
                    return result;
                }
                /*#region ServiceAvailabilityChecker

                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                {
                    throw new Exception("Data Services are not available");
                }
                #endregion */

                List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> dimensionDailyPerfData = entity.GF_PERF_DAILY_ATTRIBUTION.Where(a =>
                    ((a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == securityName.ToUpper().Trim() && a.PORTFOLIO.ToUpper().Trim() == portfolioName.ToUpper().Trim())
                    || (a.NODE_NAME.ToUpper().Trim() == "COUNTRY" && a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == countryName.FirstOrDefault().ToUpper().Trim() && a.PORTFOLIO.ToUpper().Trim() == portfolioName.ToUpper().Trim())
                    || (a.PORTFOLIO.ToUpper().Trim() == portfolioName.ToUpper().Trim() && a.NODE_NAME.ToUpper().Trim() == "GICS LEVEL 1" && a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == sectorName.FirstOrDefault().ToUpper().Trim())) && a.TO_DATE == objEffectiveDate.Date).ToList().Distinct().ToList();

                #region Comparator
                IEqualityComparer<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> customComparer = new GF_PERF_DAILY_ATTRIBUTION_Comparer();
                dimensionDailyPerfData = dimensionDailyPerfData.Distinct(customComparer).ToList();
                #endregion

                GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION dimensionBenchmarkReturnData = (entity.GF_PERF_DAILY_ATTRIBUTION.
                    Where(a => a.BMNAME == benchmarkName.FirstOrDefault() && a.TO_DATE == objEffectiveDate.Date).FirstOrDefault());

                if (dimensionDailyPerfData != null || dimensionBenchmarkReturnData != null)
                {
                    result = RelativePerformanceUICalculations.CalculateRelativePerformanceUIData(dimensionDailyPerfData, dimensionBenchmarkReturnData);
                }
                if (result == null)
                {
                    throw new InvalidOperationException
                          ("Method Name: CalculateRelativePerformanceUIData, Class: GreenField.Web.Helpers.RelativePerformanceUICalculations, Result Null Exception");
                }
                return result.OrderBy(a => a.SortId).ToList();
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// Method to retrieve data in Benchmark Chart
        /// </summary>
        /// <param name="objBenchmarkIdentifier"></param>
        /// <param name="objStartDate"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<BenchmarkChartReturnData> RetrieveBenchmarkChartReturnData(Dictionary<string, string> objSelectedEntities)
        {
            try
            {
                bool isServiceUp = false;
                List<BenchmarkChartReturnData> result = new List<BenchmarkChartReturnData>();

                //Arguement null Exception
                if (objSelectedEntities == null)
                {
                    return result;
                }
                if (!objSelectedEntities.ContainsKey("PORTFOLIO"))
                {
                    return result;
                }
                DimensionEntities entity = DimensionEntity;
                string portfolioId = "";
                DateTime startDate = DateTime.Today.AddYears(-1);
                string countryName = "";
                string sectorName = "";
                List<string> benchmarkName;

                if (objSelectedEntities.ContainsKey("PORTFOLIO"))
                {
                    portfolioId = objSelectedEntities.Where(a => a.Key == "PORTFOLIO").First().Value;
                }
                /*#region ServiceAvailabilityChecker

                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                {
                    throw new Exception("Data Services are not available");
                }
                #endregion*/

                if (objSelectedEntities.ContainsKey("COUNTRY"))
                {
                    countryName = objSelectedEntities.Where(a => a.Key == "COUNTRY").First().Value;
                }
                else if (objSelectedEntities.ContainsKey("SECTOR"))
                {
                    sectorName = objSelectedEntities.Where(a => a.Key == "SECTOR").First().Value;
                }
                benchmarkName = (entity.GF_PORTFOLIO_HOLDINGS.
                    Where(a => a.PORTFOLIO_ID.ToUpper().Trim() == portfolioId.ToUpper().Trim()).ToList()).Select(a => a.BENCHMARK_ID).Distinct().ToList();

                if (benchmarkName == null)
                {
                    return result;
                }
                if (benchmarkName.Count != 1)
                {
                    return result;
                }
                List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> dimensionDailyPerfData = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> dimensionBenchmarkReturnData = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> dimensionBenchmarkReturns = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                List<DateTime> endDates = MultiLineBenchmarkUICalculations.CalculateEndDates();

                foreach (DateTime item in endDates)
                {
                    dimensionBenchmarkReturns = entity.GF_PERF_DAILY_ATTRIBUTION.
                        Where(a => a.PORTFOLIO.ToUpper().Trim() == portfolioId.ToUpper().Trim() && a.BMNAME.ToUpper().Trim() == benchmarkName.First().ToUpper().Trim()
                            && a.TO_DATE == item.Date).Take(1).ToList();
                    if (dimensionBenchmarkReturns != null)
                    {
                        if (dimensionBenchmarkReturns.Count != 0)
                        {
                            dimensionBenchmarkReturnData.Add(dimensionBenchmarkReturns.First());
                        }
                    }
                    dimensionBenchmarkReturns = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                }
                if (countryName != "")
                {
                    dimensionDailyPerfData = (entity.GF_PERF_DAILY_ATTRIBUTION.Where(a => a.PORTFOLIO.ToUpper().Trim() == portfolioId.ToUpper().Trim() && a.BMNAME.ToUpper().Trim() == benchmarkName.First().ToUpper().Trim() && a.NODE_NAME.ToUpper().Trim() == "COUNTRY" && a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == countryName.ToUpper().Trim() && a.TO_DATE > startDate.Date)).ToList();
                }
                else if (sectorName != "")
                {
                    dimensionDailyPerfData = (entity.GF_PERF_DAILY_ATTRIBUTION.Where(a => a.PORTFOLIO.ToUpper().Trim() == portfolioId.ToUpper().Trim() && a.BMNAME == benchmarkName.First() && a.NODE_NAME.ToUpper().Trim() == "GICS LEVEL 1" && a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == sectorName.ToUpper().Trim() && a.TO_DATE > startDate.Date)).ToList();
                }
                if (dimensionBenchmarkReturnData == null || dimensionBenchmarkReturnData.Count == 0)
                {
                    return result;
                }
                result = MultiLineBenchmarkUICalculations.RetrieveBenchmarkChartData(dimensionDailyPerfData, dimensionBenchmarkReturnData);
                if (result == null)
                {
                    throw new InvalidOperationException();
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// Method to retrieve data in Multi-Line Benchmark UI Grid
        /// </summary>
        /// <param name="objSelectedEntities"> Selected Security & Portfolio</param>
        /// <returns>List of BenchmarkGridReturnData</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<BenchmarkGridReturnData> RetrieveBenchmarkGridReturnData(Dictionary<string, string> objSelectedEntities)
        {
            bool isServiceUp = false;
            List<BenchmarkGridReturnData> result = new List<BenchmarkGridReturnData>();
            try
            {
                DimensionEntities entity = DimensionEntity;

                if (objSelectedEntities == null)
                {
                    return result;
                }
                if ((!objSelectedEntities.ContainsKey("PORTFOLIO")))
                {
                    return result;
                }

                #region CalculatingStartDate

                DateTime lastDayPreviousMonth;
                DateTime currentDate = DateTime.Today;
                List<DateTime> endDates = new List<DateTime>();
                int numberOfDays = 0;
                endDates.Add(new DateTime(currentDate.Year - 1, 12, 31));
                endDates.Add(new DateTime(currentDate.Year - 2, 12, 31));
                endDates.Add(new DateTime(currentDate.Year - 3, 12, 31));
                if (currentDate.Month == 1)
                {
                    lastDayPreviousMonth = new DateTime(currentDate.Year - 1, 12, 1);
                }
                else
                {
                    numberOfDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month - 1);
                    lastDayPreviousMonth = new DateTime(currentDate.Year, currentDate.Month - 1, numberOfDays);
                    endDates.Add(lastDayPreviousMonth);
                }

                #endregion

                string portfolioId = "";
                string countryName = "";
                string sectorName = "";
                List<string> benchmarkName;
                DateTime startDate = DateTime.Today.AddYears(-1);

                if (objSelectedEntities.ContainsKey("PORTFOLIO"))
                {
                    portfolioId = (objSelectedEntities.Where(a => a.Key == "PORTFOLIO").FirstOrDefault().Value);
                }
                if (objSelectedEntities.ContainsKey("COUNTRY"))
                {
                    countryName = objSelectedEntities.Where(a => a.Key == "COUNTRY").FirstOrDefault().Value;
                }
                else if (objSelectedEntities.ContainsKey("SECTOR"))
                {
                    sectorName = objSelectedEntities.Where(a => a.Key == "SECTOR").FirstOrDefault().Value;
                }
                /*#region ServiceAvailabilityChecker

                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                {
                    throw new Exception("Data Services are not available");
                }
                #endregion*/

                benchmarkName = (entity.GF_PORTFOLIO_HOLDINGS.Where(a => a.PORTFOLIO_ID == portfolioId).ToList()).Select(a => a.BENCHMARK_ID).ToList().Distinct().ToList();
                if (benchmarkName == null)
                {
                    return result;
                }
                if (benchmarkName.Count != 1)
                {
                    return result;
                }
                List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> sectorCountryReturn = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> dimensionBenchmarkSingleReturn = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> dimensionBenchmarkReturnData = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                foreach (DateTime item in endDates)
                {
                    dimensionBenchmarkSingleReturn = entity.GF_PERF_DAILY_ATTRIBUTION.
                        Where(a => a.PORTFOLIO.ToUpper().Trim() == portfolioId.ToUpper().Trim() && a.BMNAME.ToUpper().Trim() == benchmarkName.First().ToUpper().Trim()
                            && a.TO_DATE == item.Date).Take(1).ToList();
                    if (dimensionBenchmarkSingleReturn != null)
                    {
                        if (dimensionBenchmarkSingleReturn.Count != 0)
                        {
                            dimensionBenchmarkReturnData.Add(dimensionBenchmarkSingleReturn.First());
                        }
                    }
                    dimensionBenchmarkSingleReturn = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                }
                if (countryName != "")
                {
                    sectorCountryReturn = (entity.GF_PERF_DAILY_ATTRIBUTION.Where(a => a.PORTFOLIO.ToUpper().Trim() == portfolioId.ToUpper().Trim() && a.BMNAME.ToUpper().Trim() == benchmarkName.First().ToUpper().Trim() && a.NODE_NAME.ToUpper().Trim() == "COUNTRY" && a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == countryName.ToUpper().Trim() && a.TO_DATE > startDate.Date)).ToList();
                }
                else if (sectorName != "")
                {
                    sectorCountryReturn = (entity.GF_PERF_DAILY_ATTRIBUTION.Where(a => a.PORTFOLIO.ToUpper().Trim() == portfolioId.ToUpper().Trim() && a.BMNAME == benchmarkName.First() && a.NODE_NAME.ToUpper().Trim() == "GICS LEVEL 1" && a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == sectorName.ToUpper().Trim() && a.TO_DATE > startDate.Date)).ToList();
                }

                if (dimensionBenchmarkReturnData == null)
                {
                    return new List<BenchmarkGridReturnData>();
                }
                if (dimensionBenchmarkReturnData.Count() != 0)
                {
                    result = MultiLineBenchmarkUICalculations.RetrieveBenchmarkGridData(sectorCountryReturn, dimensionBenchmarkReturnData);
                }
                if (result == null)
                {
                    throw new InvalidOperationException();
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// Service for Chart Extension Data
        /// </summary>
        /// <param name="objSelectedSecurity">Selected Security</param>
        /// <param name="objSelectedPortfolio">Selected Portfolio</param>
        /// <param name="objStartDate">start Date for the Chart</param>
        /// <returns>Collection of Chart Extension Data</returns>
        [OperationContract]
        public List<ChartExtensionData> RetrieveChartExtensionData(Dictionary<string, string> objSelectedEntities, DateTime objStartDate)
        {
            try
            {
                if (objSelectedEntities == null || objStartDate == null)
                {
                    return new List<ChartExtensionData>();
                }
                List<ChartExtensionData> result = new List<ChartExtensionData>();
                bool isServiceUp;
                string securityLongName = "";
                string portfolioId = "";
                List<string> countryName;
                List<string> benchmarkName = new List<string>();
                List<string> sectorName = new List<string>();
                DimensionEntities  entity = DimensionEntity;

                if (objSelectedEntities.ContainsKey("SECURITY"))
                {
                    securityLongName = objSelectedEntities.Where(a => a.Key == "SECURITY").FirstOrDefault().Value;
                }
                else
                {
                    return new List<ChartExtensionData>();
                }
                if (objSelectedEntities.ContainsKey("PORTFOLIO"))
                {
                    portfolioId = objSelectedEntities.Where(a => a.Key == "PORTFOLIO").FirstOrDefault().Value;
                }
                List<GreenField.DAL.GF_SECURITY_BASEVIEW> securityBaseData = (entity.GF_SECURITY_BASEVIEW.Where(a => a.ISSUE_NAME.ToUpper().Trim() == securityLongName.ToUpper().Trim()).ToList());
                countryName = securityBaseData.Select(a => a.ASEC_SEC_COUNTRY_NAME).Distinct().ToList();
                sectorName = securityBaseData.Select(a => a.GICS_SECTOR_NAME).Distinct().ToList();
                benchmarkName = (entity.GF_PORTFOLIO_HOLDINGS.Where(a => a.PORTFOLIO_ID == portfolioId).ToList()).Select(a => a.BENCHMARK_ID).ToList().Distinct().ToList();

                if (benchmarkName == null)
                {
                    return result;
                }
                if (benchmarkName.Count != 1)
                {
                    return result;
                }
                if (countryName == null)
                {
                    return result;
                }
                if (countryName.Count != 1)
                {
                    return result;
                }
                if (sectorName == null)
                {
                    return result;
                }
                if (sectorName.Count != 1)
                {
                    return result;
                }
                if (securityLongName != null && securityLongName != "")
                {
                    /*#region ServiceAvailabilityChecker

                    isServiceUp = CheckServiceAvailability.ServiceAvailability();
                    if (!isServiceUp)
                    {
                        throw new Exception();
                    }
                    #endregion*/
                    List<GreenField.DAL.GF_PRICING_BASEVIEW> dimensionSecurityPrice = entity.GF_PRICING_BASEVIEW.
                        Where(a => (a.ISSUE_NAME == securityLongName) && (a.FROMDATE >= objStartDate.Date) && (a.DAILY_SPOT_FX != 0)).OrderByDescending(a => a.FROMDATE).ToList();
                    result = ChartExtensionCalculations.CalculateSecurityPricing(dimensionSecurityPrice);
                }
                //transaction data
                if (portfolioId != null && portfolioId != "")
                {
                    List<GreenField.DAL.GF_TRANSACTIONS> dimensionTransactionData = entity.GF_TRANSACTIONS.
                        Where(a => ((a.TRANSACTION_CODE.ToUpper().Trim() == "BUY") || (a.TRANSACTION_CODE.ToUpper().Trim() == "SELL"))
                            && (a.PORTFOLIO_ID.ToUpper().Trim() == portfolioId.ToUpper().Trim())
                            && (a.SEC_NAME.ToUpper().Trim() == securityLongName.ToUpper().Trim()) && (a.TRADE_DATE >= objStartDate.Date)).ToList();
                    result = ChartExtensionCalculations.CalculateTransactionValues(dimensionTransactionData, result);
                }

                //Sector & Country Return Data
                List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> dimensionSectorCountryReturnData = entity.GF_PERF_DAILY_ATTRIBUTION.
                    Where(a => a.PORTFOLIO.ToUpper().Trim() == portfolioId.ToUpper().Trim() && a.TO_DATE > objStartDate.Date
                        && ((a.NODE_NAME.ToUpper().Trim() == "GICS LEVEL 1" && a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == sectorName.FirstOrDefault().ToUpper().Trim())
                        || (a.NODE_NAME.ToUpper().Trim() == "COUNTRY" && a.AGG_LVL_1_LONG_NAME.ToUpper().Trim() == countryName.FirstOrDefault().ToUpper().Trim()))).ToList();

                #region Comparator
                IEqualityComparer<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> customComparer = new GF_PERF_DAILY_ATTRIBUTION_Comparer();
                dimensionSectorCountryReturnData = dimensionSectorCountryReturnData.Distinct(customComparer).ToList();
                #endregion

                if (dimensionSectorCountryReturnData == null)
                {
                    return result;
                }
                if (dimensionSectorCountryReturnData.Count == 0)
                {
                    return result;
                }
                List<ChartExtensionData> sectorCountryReturnData = ChartExtensionCalculations.CalculateSectorCountryReturnValues(dimensionSectorCountryReturnData);
                if (sectorCountryReturnData != null || sectorCountryReturnData.Count != 0)
                {
                    result.AddRange(sectorCountryReturnData);
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        #endregion

        #region Market Performance Snapshot Operation Contracts
        /// <summary>
        /// Retrieve Benchmark Filter Selection Data for the prefered filterType
        /// </summary>
        /// <param name="benchmarkCode">Benchmark Code</param>
        /// <param name="benchmarkName">Benchmark Name</param>
        /// <param name="filterType">Filter type: Country / Sector / null</param>
        /// <returns>List of BenchmarkFilterSelectionData</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<BenchmarkFilterSelectionData> RetrieveBenchmarkFilterSelectionData(String benchmarkCode, String benchmarkName, String filterType)
        {
            List<BenchmarkFilterSelectionData> result = new List<BenchmarkFilterSelectionData>();

            GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION lastBusinessDateRecord = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION
                .Where(record => record.NODE_NAME == (filterType == "Country" ? "Country" : "GICS Level 1")
                    && record.BMNAME == benchmarkName
                    && record.BM == benchmarkCode)
                .OrderByDescending(g => g.TO_DATE).FirstOrDefault();

            DateTime? lastBusinessDate = lastBusinessDateRecord.TO_DATE;

            List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> benchmarkCountryPerformanceData = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION
                .Where(record => record.NODE_NAME == (filterType == "Country" ? "Country" : "GICS Level 1")
                    && record.BMNAME == benchmarkName
                    && record.BM == benchmarkCode
                    && record.TO_DATE == lastBusinessDate)
                .ToList();

            foreach (GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION item in benchmarkCountryPerformanceData)
            {
                if (result.Where(record => record.FilterCode == item.AGG_LVL_1 && record.FilterName == item.AGG_LVL_1_LONG_NAME)
                    .FirstOrDefault() == null)
                {
                    result.Add(new BenchmarkFilterSelectionData()
                    {
                        FilterCode = item.AGG_LVL_1,
                        FilterName = item.AGG_LVL_1_LONG_NAME
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// retrieving list of market performance snapshots for particular user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>returns list of market performance snapshots</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<MarketSnapshotSelectionData> RetrieveMarketSnapshotSelectionData(string userName)
        {
            try
            {
                /*TODO
                // use cache if available
                var fromCache = (List<MarketSnapshotSelectionData>)new DefaultCacheProvider().Get(CacheKeyNames.MarketSnapshotSelectionDataCache);
                if (fromCache != null)
                    return fromCache;
                 */

                // otherwise fetch the data and cache it
                if (String.IsNullOrEmpty(userName))
                    return new List<MarketSnapshotSelectionData>();

                ResearchEntities entity = new ResearchEntities();
                List<MarketSnapshotSelectionData> userPreference = (entity.GetMarketSnapshotSelectionData(userName))
                    .OrderBy(record => record.SnapshotName)
                    .ToList<MarketSnapshotSelectionData>();

                //new DefaultCacheProvider().Set(CacheKeyNames.MarketSnapshotSelectionDataCache, userPreference, Int32.Parse(ConfigurationManager.AppSettings["PerformanceCacheTime"]));
                return userPreference;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// retrieving user preference for market performance snapshot gadget
        /// </summary>
        /// <param name="userName">user name/id</param>
        /// <param name="snapshotName">snapshot name</param>
        /// <returns>list of user preference of entities in market performance snapshot</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<MarketSnapshotPreference> RetrieveMarketSnapshotPreference(int snapshotPreferenceId)
        {
            try
            {
                ResearchEntities entity = new ResearchEntities();
                List<MarketSnapshotPreference> userPreference = (entity.GetMarketSnapshotPreference(snapshotPreferenceId)).ToList<MarketSnapshotPreference>();
                return userPreference.OrderBy(record => record.GroupPreferenceID).ThenBy(record => record.EntityOrder).ToList();
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        private Decimal? GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(string instrumentId, string returnType, DateTime recordDate, out DateTime returnDate)
        {
            DimensionEntities entity = DimensionEntity;
            GreenField.DAL.GF_PRICING_BASEVIEW pricingRecord = null;
            int iteration = 0;
            while (pricingRecord == null)
            {
                iteration++;
                if (iteration > 4)
                    break;
                recordDate = recordDate.AddDays(-1);
                pricingRecord = entity.GF_PRICING_BASEVIEW
                            .Where(record => record.INSTRUMENT_ID == instrumentId
                                && record.FROMDATE == Convert.ToDateTime(recordDate.ToString())).FirstOrDefault();
            }
            returnDate = recordDate;

            Decimal? pricingRecordValue = null;
            if (pricingRecord != null)
            {
                switch (returnType)
                {
                    case "Price":
                        pricingRecordValue = pricingRecord.DAILY_PRICE_RETURN;
                        break;
                    case "Total":
                        pricingRecordValue = pricingRecord.DAILY_GROSS_RETURN;
                        break;
                    case null:
                        pricingRecordValue = pricingRecord.DAILY_CLOSING_PRICE;
                        break;
                    default:
                        pricingRecordValue = null;
                        break;
                }
            }

            return pricingRecordValue;
        }

        private DateTime GetWeekBeginDate(DateTime date)
        {
            int weekDay = -1;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    weekDay = 5;
                    break;
                case DayOfWeek.Monday:
                    weekDay = 1;
                    break;
                case DayOfWeek.Saturday:
                    weekDay = 6;
                    break;
                case DayOfWeek.Sunday:
                    weekDay = 7;
                    break;
                case DayOfWeek.Thursday:
                    weekDay = 4;
                    break;
                case DayOfWeek.Tuesday:
                    weekDay = 2;
                    break;
                case DayOfWeek.Wednesday:
                    weekDay = 3;
                    break;
                default:
                    weekDay = -1;
                    break;
            }

            if (weekDay >= 6)
                return date.AddDays(-1 * (weekDay - 6));
            else
                return date.AddDays(-1 * (weekDay + 1));

        }

        private DateTime GetQuarterBeginDate(DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 3)
                return new DateTime(date.Year, 1, 1);
            if (date.Month >= 4 && date.Month <= 6)
                return new DateTime(date.Year, 4, 1);
            if (date.Month >= 7 && date.Month <= 9)
                return new DateTime(date.Year, 7, 1);
            else
                return new DateTime(date.Year, 10, 1);
        }

        /// <summary>
        /// retrieving entity data for market performance snapshot gadget based on user preference
        /// </summary>
        /// <param name="marketSnapshotPreference"></param>
        /// <returns>list of entity data for market performance snapshot</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<MarketSnapshotPerformanceData> RetrieveMarketSnapshotPerformanceData(List<MarketSnapshotPreference> marketSnapshotPreference)
        {
            try
            {
                List<MarketSnapshotPerformanceData> result = new List<MarketSnapshotPerformanceData>();
                DimensionEntities entity = DimensionEntity;

                if (marketSnapshotPreference == null)
                    return result;

                foreach (MarketSnapshotPreference preference in marketSnapshotPreference)
                {
                    MarketSnapshotPerformanceData entityPerformanceData = new MarketSnapshotPerformanceData();
                    if (preference.EntityType == "BENCHMARK")
                    {
                        entityPerformanceData = MarketPerformanceSnapshotDataCalculations.GetBenchmarkPerformanceData(entity, preference);
                    }
                    else
                    {
                        entityPerformanceData = MarketPerformanceSnapshotDataCalculations.GetSecurityCommodityIndexPerformanceData(entity, preference);
                    }

                    //if (preference.EntityType == "SECURITY" || preference.EntityType == "COMMODITY" || preference.EntityType == "INDEX")
                    //{
                    //    GF_SELECTION_BASEVIEW entityRecord = entity.GF_SELECTION_BASEVIEW
                    //        .Where(record => record.LONG_NAME == preference.EntityName
                    //            && record.TYPE == preference.EntityType)
                    //        .FirstOrDefault();

                    //    if (entityRecord == null)
                    //    {
                    //        result.Add(new MarketPerformanceSnapshotData());
                    //        continue;
                    //    }


                    //    string entityInstrumentId = entityRecord.INSTRUMENT_ID;

                    //    #region Last Date Pricing Data
                    //    DateTime lastBusinessDate = DateTime.Today;
                    //    Decimal? lastBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, lastBusinessDate, out lastBusinessDate);
                    //    #endregion

                    //    #region Second Last Date Pricing Data
                    //    DateTime secondLastBusinessDate = lastBusinessDate;
                    //    Decimal? secondLastBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, secondLastBusinessDate, out secondLastBusinessDate);
                    //    #endregion

                    //    #region Last Week Date Pricing Data
                    //    DateTime lastWeekBusinessDate = GetWeekBeginDate(DateTime.Today);
                    //    Decimal? lastWeekBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, lastWeekBusinessDate, out lastWeekBusinessDate);
                    //    #endregion

                    //    #region Last Month Date Pricing Data
                    //    DateTime lastMonthBusinessDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    //    Decimal? lastMonthBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, lastMonthBusinessDate, out lastMonthBusinessDate);
                    //    #endregion

                    //    #region Last Quarter Date Pricing Data
                    //    DateTime lastQuarterBusinessDate = GetQuarterBeginDate(DateTime.Today);
                    //    Decimal? lastQuarterBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, lastQuarterBusinessDate, out lastQuarterBusinessDate);
                    //    #endregion

                    //    #region Last Year Date Pricing Data
                    //    DateTime lastYearBusinessDate = new DateTime(DateTime.Today.Year, 1, 1);
                    //    Decimal? lastYearBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, lastYearBusinessDate, out lastYearBusinessDate);
                    //    #endregion

                    //    #region Second Last Year Date Pricing Data
                    //    DateTime secondLastYearBusinessDate = new DateTime(DateTime.Today.Year - 1, 1, 1);
                    //    Decimal? secondLastYearBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, secondLastYearBusinessDate, out secondLastYearBusinessDate);
                    //    #endregion

                    //    #region Third Last Year Date Pricing Data
                    //    DateTime thirdLastYearBusinessDate = new DateTime(DateTime.Today.Year - 2, 1, 1);
                    //    Decimal? thirdLastYearBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, thirdLastYearBusinessDate, out thirdLastYearBusinessDate);
                    //    #endregion

                    //    #region Fourth Last Year Date Pricing Data
                    //    DateTime fourthLastYearBusinessDate = new DateTime(DateTime.Today.Year - 3, 1, 1);
                    //    Decimal? fourthLastYearBusinessDatePrice = GetPerformanceDataDataByInstrumentIdReturnTypeAndFromDate(entityInstrumentId
                    //        , preference.EntityReturnType, fourthLastYearBusinessDate, out fourthLastYearBusinessDate);
                    //    #endregion


                    //    Decimal? dateToDateReturn = secondLastBusinessDatePrice == 0 ? 0 : ((lastBusinessDatePrice - secondLastBusinessDatePrice) / (secondLastBusinessDatePrice)) * 100;
                    //    Decimal? weekToDateReturn = lastWeekBusinessDatePrice == 0 ? 0 : ((lastBusinessDatePrice - lastWeekBusinessDatePrice) / (lastWeekBusinessDatePrice)) * 100;
                    //    Decimal? monthToDateReturn = lastMonthBusinessDatePrice == 0 ? 0 : ((lastBusinessDatePrice - lastMonthBusinessDatePrice) / (lastMonthBusinessDatePrice)) * 100;
                    //    Decimal? quarterToDateReturn = lastQuarterBusinessDatePrice == 0 ? 0 : ((lastBusinessDatePrice - lastQuarterBusinessDatePrice) / (lastQuarterBusinessDatePrice)) * 100;
                    //    Decimal? yearToDateReturn = lastYearBusinessDatePrice == 0 ? 0 : ((lastBusinessDatePrice - lastYearBusinessDatePrice) / (lastYearBusinessDatePrice)) * 100;
                    //    Decimal? lastYearReturn = secondLastYearBusinessDatePrice == 0 ? 0 : ((lastYearBusinessDatePrice - secondLastYearBusinessDatePrice) / (secondLastYearBusinessDatePrice)) * 100;
                    //    Decimal? secondLastYearReturn = thirdLastYearBusinessDatePrice == 0 ? 0 : ((secondLastYearBusinessDatePrice - thirdLastYearBusinessDatePrice) / (thirdLastYearBusinessDatePrice)) * 100;
                    //    Decimal? thirdLastYearReturn = fourthLastYearBusinessDatePrice == 0 ? 0 : ((thirdLastYearBusinessDatePrice - fourthLastYearBusinessDatePrice) / (fourthLastYearBusinessDatePrice)) * 100;

                    //result.Add(new MarketPerformanceSnapshotData()
                    //{
                    //    MarketSnapshotPreferenceInfo = preference,
                    //    DateToDateReturn = dateToDateReturn,
                    //    WeekToDateReturn = weekToDateReturn,
                    //    MonthToDateReturn = monthToDateReturn,
                    //    QuarterToDateReturn = quarterToDateReturn,
                    //    YearToDateReturn = yearToDateReturn,
                    //    LastYearReturn = lastYearReturn,
                    //    SecondLastYearReturn = secondLastYearReturn,
                    //    ThirdLastYearReturn = thirdLastYearReturn
                    //});

                    entityPerformanceData.MarketSnapshotPreferenceInfo = preference;
                    result.Add(entityPerformanceData);
                    //}
                    //else if (preference.EntityType == "BENCHMARK")
                    //{
                    //    GF_PERF_DAILY_ATTRIB_DIST_BM benchmarkDetails = entity.GF_PERF_DAILY_ATTRIB_DIST_BM.
                    //        Where(record => record.BMNAME == preference.EntityName).FirstOrDefault();

                    //    if (benchmarkDetails == null)
                    //        continue;


                    //    GF_PERF_DAILY_ATTRIBUTION benchmarkPerfRecord = entity.GF_PERF_DAILY_ATTRIBUTION
                    //        .Where(g => g.NODE_NAME == "GICS Level 5" && g.AGG_LVL_1 == "Undefined" &&
                    //            g.BM == benchmarkDetails.BM && g.BMNAME == benchmarkDetails.BMNAME).FirstOrDefault();

                    //    if (benchmarkPerfRecord == null)
                    //        continue;

                    //    Decimal? dateToDateReturn = benchmarkPerfRecord.BM1_TOP_QC_TWR_1D * 100;
                    //    Decimal? weekToDateReturn = benchmarkPerfRecord.BM1_TOP_QC_TWR_1W * 100;
                    //    Decimal? monthToDateReturn = benchmarkPerfRecord.BM1_TOP_QC_TWR_MTD * 100;
                    //    Decimal? quarterToDateReturn = benchmarkPerfRecord.BM1_TOP_RC_TWR_QTD * 100;
                    //    Decimal? yearToDateReturn = benchmarkPerfRecord.BM1_TOP_RC_TWR_YTD * 100;
                    //    Decimal? lastYearReturn = benchmarkPerfRecord.BM1_TOP_RC_TWR_1Y * 100;
                    //    Decimal? secondLastYearReturn = null;
                    //    Decimal? thirdLastYearReturn = null;

                    //    result.Add(new MarketPerformanceSnapshotData()
                    //    {
                    //        MarketSnapshotPreferenceInfo = preference,
                    //        DateToDateReturn = dateToDateReturn,
                    //        WeekToDateReturn = weekToDateReturn,
                    //        MonthToDateReturn = monthToDateReturn,
                    //        QuarterToDateReturn = quarterToDateReturn,
                    //        YearToDateReturn = yearToDateReturn,
                    //        LastYearReturn = lastYearReturn,
                    //        SecondLastYearReturn = secondLastYearReturn,
                    //        ThirdLastYearReturn = thirdLastYearReturn
                    //    });
                    //}
                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        ///// <summary>
        ///// adding new market performance snapshot created by user
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="snapshotName"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //public bool AddMarketSnapshotPerformance(string userId, string snapshotName)
        //{
        //    try
        //    {
        //        ResearchEntities entity = new ResearchEntities();
        //        entity.SetMarketSnapshotPreference(userId, snapshotName);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionTrace.LogException(ex);
        //        string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
        //        throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
        //    }
        //}

        ///// <summary>
        ///// updating the market performance snapshot name for a particular user
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="snapshotName"></param>
        ///// <param name="snapshotPreferenceId"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //public bool UpdateMarketSnapshotPerformance(string userId, string snapshotName, int snapshotPreferenceId)
        //{
        //    try
        //    {
        //        ResearchEntities entity = new ResearchEntities();
        //        entity.UpdateMarketSnapshotPreference(userId, snapshotName, snapshotPreferenceId);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionTrace.LogException(ex);
        //        string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
        //        throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
        //    }
        //}

        ///// <summary>
        ///// adding user preferred groups in market performance snapshot gadget
        ///// </summary>
        ///// <param name="snapshotPreferenceId"></param>
        ///// <param name="groupName"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //public bool AddMarketSnapshotGroupPreference(int snapshotPreferenceId, string groupName)
        //{
        //    ResearchEntities entity = new ResearchEntities();
        //    try
        //    {
        //        entity.SetMarketSnapshotGroupPreference(snapshotPreferenceId, groupName);
        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        ExceptionTrace.LogException(ex);
        //        string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
        //        throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
        //    }
        //}

        ///// <summary>
        ///// removing user preferred groups from market performance snapshot gadget
        ///// </summary>
        ///// <param name="grouppreferenceId"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //public bool RemoveMarketSnapshotGroupPreference(int groupPreferenceId)
        //{
        //    try
        //    {
        //        ResearchEntities entity = new ResearchEntities();
        //        entity.DeleteMarketSnapshotGroupPreference(groupPreferenceId);
        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        ExceptionTrace.LogException(ex);
        //        string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
        //        throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
        //    }
        //}

        ///// <summary>
        ///// adding user preferred entities in groups in market performance snapshot gadget
        ///// </summary>
        ///// <param name="marketSnapshotPreference"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //public bool AddMarketSnapshotEntityPreference(MarketSnapshotPreference marketSnapshotPreference)
        //{
        //    ResearchEntities entity = new ResearchEntities();
        //    try
        //    {
        //        entity.SetMarketSnapshotEntityPreference(marketSnapshotPreference.GroupPreferenceID,
        //                                                    marketSnapshotPreference.EntityName,
        //                                                        marketSnapshotPreference.EntityReturnType,
        //                                                            marketSnapshotPreference.EntityType,
        //                                                                marketSnapshotPreference.EntityOrder);
        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        ExceptionTrace.LogException(ex);
        //        string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
        //        throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
        //    }
        //}

        ///// <summary>
        /////  removing user preferred entities from groups in market performance snapshot gadget
        ///// </summary>
        ///// <param name="marketSnapshotPreference"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //public bool RemoveMarketSnapshotEntityPreference(MarketSnapshotPreference marketSnapshotPreference)
        //{
        //    ResearchEntities entity = new ResearchEntities();
        //    try
        //    {
        //        int? affectedRows = entity.DeleteMarketSnapshotEntityPreference(marketSnapshotPreference.EntityPreferenceId).FirstOrDefault();
        //        if (affectedRows == null || affectedRows == 0)
        //            return false;
        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        ExceptionTrace.LogException(ex);
        //        string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
        //        throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
        //    }
        //}

        /// <summary>
        ///  save user preference in market performance snapshot gadget
        /// </summary>
        /// <param name="marketSnapshotPreference"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<MarketSnapshotPreference> SaveMarketSnapshotPreference(string updateXML)
        {
            ResearchEntities entity = new ResearchEntities();
            try
            {
                int? status = entity.UpdateMarketPerformanceSnapshot(updateXML).FirstOrDefault();

                //switch (status)
                //{
                //    case -1:
                //        throw new NotImplementedException("specified snapshot does not exists in database");
                //    case -2:
                //        throw new NotImplementedException("An error occured while creating groups within the specified snapshot");
                //    case -3:
                //        throw new NotImplementedException("An error occured while creating entities for inserted groups within the specified snapshot");
                //    case -4:
                //        throw new NotImplementedException("An error occured while deleting groups within the specified snapshot");
                //    case -5:
                //        throw new NotImplementedException("An error occured while creating entities within the specified snapshot");
                //    case -6:
                //        throw new NotImplementedException("An error occured while deleting entities within the specified snapshot");
                //    case -7:
                //        throw new NotImplementedException("An error occured while updating entities within the specified snapshot");
                //    default:
                //        break;
                //}

                if (status <= -10)
                    throw new NotImplementedException("Error[" + status.ToString() + "]: Snapshot Updation Failed");

                List<MarketSnapshotPreference> userPreference = (entity.GetMarketSnapshotPreference(status))
                    .ToList<MarketSnapshotPreference>();

                return userPreference.OrderBy(record => record.GroupPreferenceID)
                    .ThenBy(record => record.EntityOrder)
                    .ToList();
            }

            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        ///// <summary>
        /////  save new user preference in market performance snapshot gadget
        ///// </summary>
        ///// <param name="marketSnapshotPreference"></param>
        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //public PopulatedMarketPerformanceSnapshotData SaveAsMarketSnapshotPreference(string updateXML)
        //{
        //    ResearchEntities entity = new ResearchEntities();
        //    try
        //    {
        //        PopulatedMarketPerformanceSnapshotData result = new PopulatedMarketPerformanceSnapshotData();

        //        int? status = entity.UpdateMarketPerformanceSnapshot(updateXML).FirstOrDefault();

        //        if (status <= -10)
        //            throw new NotImplementedException("Error[" + status.ToString() + "]: Snapshot Creation Failed");

        //        tblMarketSnapshotPreference snapshotRecord = entity.tblMarketSnapshotPreferences.Where(record => record.SnapshotPreferenceId == status).FirstOrDefault();

        //        if (snapshotRecord == null)
        //            return null;

        //        MarketSnapshotSelectionData marketSnapshotSelectionData = new MarketSnapshotSelectionData() { SnapshotName = snapshotRecord.SnapshotName, SnapshotPreferenceId = snapshotRecord.SnapshotPreferenceId };

        //        List<MarketSnapshotPreference> marketSnapshotPreference = RetrieveMarketSnapshotPreference(Convert.ToInt32(status));
        //        List<MarketPerformanceSnapshotData> marketPerformanceSnapshotData = RetrieveMarketPerformanceSnapshotData(marketSnapshotPreference);

        //        result.MarketSnapshotSelectionInfo = marketSnapshotSelectionData;
        //        result.MarketPerformanceSnapshotInfo = marketPerformanceSnapshotData.OrderBy(record => record.MarketSnapshotPreferenceInfo.GroupPreferenceID)
        //                                                .ThenBy(record => record.MarketSnapshotPreferenceInfo.EntityOrder)
        //                                                .ToList();
        //        return result;
        //    }

        //    catch (Exception ex)
        //    {
        //        ExceptionTrace.LogException(ex);
        //        string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
        //        throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
        //    }
        //}
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public PopulatedMarketSnapshotPerformanceData SaveAsMarketSnapshotPreference(string updateXML)
        {
            ResearchEntities entity = new ResearchEntities();
            try
            {
                PopulatedMarketSnapshotPerformanceData result = new PopulatedMarketSnapshotPerformanceData();

                int? status = entity.UpdateMarketPerformanceSnapshot(updateXML).FirstOrDefault();

                if (status <= -10)
                    throw new NotImplementedException("Error[" + status.ToString() + "]: Snapshot Creation Failed");

                tblMarketSnapshotPreference snapshotRecord = entity.tblMarketSnapshotPreferences.Where(record => record.SnapshotPreferenceId == status).FirstOrDefault();

                if (snapshotRecord == null)
                    return null;

                MarketSnapshotSelectionData marketSnapshotSelectionData = new MarketSnapshotSelectionData() { SnapshotName = snapshotRecord.SnapshotName, SnapshotPreferenceId = snapshotRecord.SnapshotPreferenceId };

                List<MarketSnapshotPreference> marketSnapshotPreference = RetrieveMarketSnapshotPreference(Convert.ToInt32(status));
                List<MarketSnapshotPerformanceData> marketPerformanceSnapshotData = RetrieveMarketSnapshotPerformanceData(marketSnapshotPreference);

                result.MarketSnapshotSelectionInfo = marketSnapshotSelectionData;
                result.MarketPerformanceSnapshotInfo = marketPerformanceSnapshotData.OrderBy(record => record.MarketSnapshotPreferenceInfo.GroupPreferenceID)
                                                        .ThenBy(record => record.MarketSnapshotPreferenceInfo.EntityOrder)
                                                        .ToList();
                return result;
            }

            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public bool RemoveMarketSnapshotPreference(string userName, string snapshotName)
        {
            ResearchEntities entity = new ResearchEntities();
            try
            {
                entity.DeleteMarketSnapshotPreference(userName, snapshotName);
                return true;
            }

            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }
        #endregion

        #region Relative Performance Gadgets
        /// <summary>
        /// Retrieves list of sector information for a particular composite/fund and effective date.
        /// </summary>
        /// <param name="portfolioSelectionData">PortfolioSelectionData object</param>
        /// <param name="effectiveDate">Effective date</param>
        /// <returns>List of RelativePerformanceSectorData</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<RelativePerformanceSectorData> RetrieveRelativePerformanceSectorData(PortfolioSelectionData portfolioSelectionData, DateTime effectiveDate)
        {
            try
            {
                List<RelativePerformanceSectorData> result = new List<RelativePerformanceSectorData>();
                if (portfolioSelectionData == null || effectiveDate == null)
                {
                    return result;
                }

                //checking if the service is down
                /*bool isServiceUp;
                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                { throw new Exception(); }*/

                DimensionEntities entity = DimensionEntity;

                result = entity.GF_PERF_DAILY_ATTRIBUTION
                    .Where(t => t.PORTFOLIO == portfolioSelectionData.PortfolioId &&
                        t.TO_DATE == effectiveDate &&
                        t.NODE_NAME == "Security ID" &&
                        t.COUNTRY != null &&
                        t.GICS_LVL1 != null &&
                        t.SEC_INV_THEME == "EQUITY")
                    .Select(t => new RelativePerformanceSectorData
                    {
                        SectorId = t.GICS_LVL1,
                        SectorName = t.GICS_LVL1
                    })
                    .ToList();

                result = ListUtils.DistinctBy(result , g=>g.SectorId)
                    .OrderBy(r => r.SectorName).ToList();

                return result;

                //List<GF_PERF_DAILY_ATTRIBUTION> data = entity.GF_PERF_DAILY_ATTRIBUTION.Where(t =>
                //                                                                        t.PORTFOLIO == portfolioSelectionData.PortfolioId &&
                //                                                                        t.TO_DATE == Convert.ToDateTime(effectiveDate) &&
                //                                                                        t.NODE_NAME == "Security ID" &&
                //                                                                        t.COUNTRY != null &&
                //                                                                        t.GICS_LVL1 != null &&
                //                                                                        t.SEC_INV_THEME == "EQUITY").ToList();
                //if (data.Count.Equals(0))
                //{ return result; }

                //foreach (GF_PERF_DAILY_ATTRIBUTION record in data)
                //{
                //    result.Add(new RelativePerformanceSectorData()
                //    {
                //        SectorId = record.GICS_LVL1,
                //        SectorName = record.GICS_LVL1
                //    });
                //}
                //result = result.Distinct().OrderBy(r => r.SectorName).ToList();
                //return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// Retrieves relative performance data for a particular composite/fund, effective date and period.
        /// </summary>
        /// <param name="portfolioSelectionData">PortfolioSelectionData object</param>
        /// <param name="effectiveDate">Effective date</param>
        /// <param name="period">Period</param>
        /// <returns>List of RelativePerformanceData</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<RelativePerformanceData> RetrieveRelativePerformanceData(PortfolioSelectionData portfolioSelectionData, DateTime effectiveDate, string period)
        {
            try
            {
                List<RelativePerformanceData> result = new List<RelativePerformanceData>();
                if (portfolioSelectionData == null || effectiveDate == null || period == null)
                {
                    return result; 
                }

                DimensionEntities entity = DimensionEntity;

                IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> resultQuery = entity.GF_PERF_DAILY_ATTRIBUTION
                    .Where(t => t.PORTFOLIO == portfolioSelectionData.PortfolioId &&
                        t.TO_DATE ==effectiveDate &&
                        t.NODE_NAME == "Security ID" &&
                        t.COUNTRY != null &&
                        t.GICS_LVL1 != null &&
                        t.SEC_INV_THEME == "EQUITY");


                #region Inception Date Check
                Boolean isInceptionDateValid = InceptionDateCheck(resultQuery, period, effectiveDate.Date);
                if (!isInceptionDateValid)
                {
                    return result;
                }
                #endregion

                #region Query Execution
                List<RelativePerformanceQueryData> data = new List<RelativePerformanceQueryData>();
                switch (period)
                {
                    case "1D":
                        data = resultQuery.Select(t => new RelativePerformanceQueryData
                        {
                            SecurityName = t.SEC_NAME,
                            CountryCode = t.COUNTRY,
                            SectorCode = t.GICS_LVL1,
                            SectorName = t.GICS_LVL1,
                            FundWeight = t.POR_RC_AVG_WGT_1D * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1D * 100,
                            //Alpha = t.BM1_RC_ASSET_ALLOC_1D + t.BM1_RC_SEC_SELEC_1D
                            Alpha = t.F_BM1_ASH_ASSET_ALLOC_1D + t.F_BM1_ASH_SEC_SELEC_1D
                        }).ToList();
                        break;
                    case "1W":
                        data = resultQuery.Select(t => new RelativePerformanceQueryData
                        {
                            SecurityName = t.SEC_NAME,
                            CountryCode = t.COUNTRY,
                            SectorCode = t.GICS_LVL1,
                            SectorName = t.GICS_LVL1,
                            FundWeight = t.POR_RC_AVG_WGT_1W * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1W * 100,
                            //Alpha = t.BM1_RC_ASSET_ALLOC_1W + t.BM1_RC_SEC_SELEC_1W
                            Alpha = t.F_BM1_ASH_ASSET_ALLOC_1W + t.F_BM1_ASH_SEC_SELEC_1W
                        }).ToList();
                        break;
                    case "MTD":
                        data = resultQuery.Select(t => new RelativePerformanceQueryData
                        {
                            SecurityName = t.SEC_NAME,
                            CountryCode = t.COUNTRY,
                            SectorCode = t.GICS_LVL1,
                            SectorName = t.GICS_LVL1,
                            FundWeight = t.POR_RC_AVG_WGT_MTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_MTD * 100,
                            //Alpha = t.BM1_RC_ASSET_ALLOC_MTD + t.BM1_RC_SEC_SELEC_MTD
                            Alpha = t.F_BM1_ASH_ASSET_ALLOC_MTD + t.F_BM1_ASH_SEC_SELEC_MTD
                        }).ToList();
                        break;
                    case "QTD":
                        data = resultQuery.Select(t => new RelativePerformanceQueryData
                        {
                            SecurityName = t.SEC_NAME,
                            CountryCode = t.COUNTRY,
                            SectorCode = t.GICS_LVL1,
                            SectorName = t.GICS_LVL1,
                            FundWeight = t.POR_RC_AVG_WGT_QTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_QTD * 100,
                           // Alpha = t.BM1_RC_ASSET_ALLOC_QTD + t.BM1_RC_SEC_SELEC_QTD
                            Alpha = t.F_BM1_ASH_ASSET_ALLOC_QTD + t.F_BM1_ASH_SEC_SELEC_QTD
                        }).ToList();
                        break;
                    case "1Y":
                        data = resultQuery.Select(t => new RelativePerformanceQueryData
                        {
                            SecurityName = t.SEC_NAME,
                            CountryCode = t.COUNTRY,
                            SectorCode = t.GICS_LVL1,
                            SectorName = t.GICS_LVL1,
                            FundWeight = t.POR_RC_AVG_WGT_1Y * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1Y * 100,
                            //Alpha = t.BM1_RC_ASSET_ALLOC_1Y + t.BM1_RC_SEC_SELEC_1Y
                            Alpha = t.F_BM1_ASH_ASSET_ALLOC_1Y + t.F_BM1_ASH_SEC_SELEC_1Y
                        }).ToList();
                        break;
                    default:
                        data = resultQuery.Select(t => new RelativePerformanceQueryData
                        {
                            SecurityName = t.SEC_NAME,
                            CountryCode = t.COUNTRY,
                            SectorCode = t.GICS_LVL1,
                            SectorName = t.GICS_LVL1,
                            FundWeight = t.POR_RC_AVG_WGT_YTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_YTD * 100,
                            //Alpha = t.BM1_RC_ASSET_ALLOC_YTD + t.BM1_RC_SEC_SELEC_YTD
                            Alpha = t.F_BM1_ASH_ASSET_ALLOC_YTD + t.F_BM1_ASH_SEC_SELEC_YTD
                        }).ToList();
                        break;
                }
                #endregion

                #region Distinct country and sector codes
                List<string> countryCodes = data.Select(t => t.CountryCode).ToList();
                countryCodes = countryCodes.Distinct().OrderBy(a => a).ToList();


                List<RelativePerformanceSectorData> sectors = data
                    .Select(t => new RelativePerformanceSectorData
                    {
                        SectorId = t.SectorCode,
                        SectorName = t.SectorName,
                    }).ToList();
                sectors = sectors.Distinct().OrderBy(r => r.SectorName).ToList();
                #endregion

                foreach (string countryCode in countryCodes)
                {
                    decimal? aggcsAlpha = 0;
                    decimal? aggcsPortfolioShare = 0;
                    decimal? aggcsBenchmarkShare = 0;
                    List<RelativePerformanceCountrySpecificData> sectorSpecificData = new List<RelativePerformanceCountrySpecificData>();
                    foreach (RelativePerformanceSectorData sectorData in sectors)
                    {
                        decimal? aggssAlpha = 0;
                        decimal? aggssPortfolioShare = 0;
                        decimal? aggssBenchmarkShare = 0;

                        List<RelativePerformanceQueryData> specificData = data.Where(t => t.CountryCode == countryCode 
                            && t.SectorCode == sectorData.SectorName).ToList();

                        foreach (RelativePerformanceQueryData row in specificData)
                        {
                            aggssPortfolioShare += row.FundWeight;
                            aggssBenchmarkShare += row.BenchmarkWeight;
                            aggssAlpha += row.Alpha;
                        }

                        sectorSpecificData.Add(new RelativePerformanceCountrySpecificData()
                        {
                            SectorId = sectorData.SectorId,
                            SectorName = sectorData.SectorName,
                            Alpha = aggssAlpha,
                            PortfolioShare = aggssPortfolioShare,
                            BenchmarkShare = aggssBenchmarkShare,
                            ActivePosition = Convert.ToDecimal(aggssPortfolioShare - aggssBenchmarkShare),
                        });

                        aggcsAlpha += aggssAlpha;
                        aggcsPortfolioShare += aggssPortfolioShare;
                        aggcsBenchmarkShare += aggssBenchmarkShare;
                    }

                    if (sectorSpecificData.Count > 0)
                    {
                        result.Add(new RelativePerformanceData()
                        {
                            CountryId = countryCode,
                            RelativePerformanceCountrySpecificInfo = sectorSpecificData,
                            AggregateCountryAlpha = aggcsAlpha,
                            AggregateCountryPortfolioShare = aggcsPortfolioShare,
                            AggregateCountryBenchmarkShare = aggcsBenchmarkShare,
                            AggregateCountryActivePosition = aggcsPortfolioShare - aggcsBenchmarkShare,
                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        ///  Retrieves Country Level Active Position Data for a particular composite/fund, effective date and period.
        /// </summary>
        /// <param name="portfolioSelectionData">PortfolioSelectionData object</param>
        /// <param name="effectiveDate">Effective date</param>
        /// <param name="period">Period</param>
        /// <param name="countryID">(optional) COUNTRY; By default Null</param>
        /// <param name="sectorID">(optional) GICS_SECTOR; By default Null</param>
        /// <returns>List of RelativePerformanceActivePositionData objects</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<RelativePerformanceActivePositionData> RetrieveRelativePerformanceCountryActivePositionData(PortfolioSelectionData portfolioSelectionData,
            DateTime effectiveDate, string period, string countryID = null, string sectorID = null)
        {
            try
            {
                List<RelativePerformanceActivePositionData> result = new List<RelativePerformanceActivePositionData>();
                if (portfolioSelectionData == null || effectiveDate == null || period == null)
                { return result; }

                //checking if the service is down
                /*bool isServiceUp;
                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                { throw new Exception(); }*/

                DimensionEntities entity = DimensionEntity;

                IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> resultQuery = GetRelativePerformanceCountryDataQuery(portfolioSelectionData
                    , effectiveDate.Date, countryID, sectorID);

                #region Inception Date Check
                Boolean isInceptionDateValid = InceptionDateCheck(resultQuery, period, effectiveDate.Date);
                if (!isInceptionDateValid)
                {
                    return result;
                }
                #endregion

                #region Query Execution
                List<RelativePerformanceActivePositionData> data = new List<RelativePerformanceActivePositionData>();
                switch (period)
                {
                    case "1D":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_1D != 0 || g.POR_RC_TWR_1D != 0 || g.BM1_RC_AVG_WGT_1D != 0 || g.BM1_RC_TWR_1D != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1D * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1D * 100
                        }).ToList();
                        break;
                    case "1W":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_1W != 0 || g.POR_RC_TWR_1W != 0 || g.BM1_RC_AVG_WGT_1W != 0 || g.BM1_RC_TWR_1W != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1W * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1W * 100
                        }).ToList();
                        break;
                    case "MTD":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_MTD != 0 || g.POR_RC_TWR_MTD != 0 || g.BM1_RC_AVG_WGT_MTD != 0 || g.BM1_RC_TWR_MTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_MTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_MTD * 100
                        }).ToList();
                        break;
                    case "QTD":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_QTD != 0 || g.POR_RC_TWR_QTD != 0 || g.BM1_RC_AVG_WGT_QTD != 0 || g.BM1_RC_TWR_QTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_QTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_QTD * 100
                        }).ToList();
                        break;
                    case "1Y":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_1Y != 0 || g.POR_RC_TWR_1Y != 0 || g.BM1_RC_AVG_WGT_1Y != 0 || g.BM1_RC_TWR_1Y != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1Y * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1Y * 100
                        }).ToList();
                        break;
                    default:
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_YTD != 0 || g.POR_RC_TWR_YTD != 0 || g.BM1_RC_AVG_WGT_YTD != 0 || g.BM1_RC_TWR_YTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_YTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_YTD * 100
                        }).ToList();
                        break;
                }
                #endregion

                //data = ListUtils.DistinctBy(data, g => new { g.Entity, g.EntityGroup, g.MarketValue, g.FundWeight, g.BenchmarkWeight }).ToList();

                List<String> countryCodes = data
                    .Select(t => t.EntityGroup ).ToList();

                countryCodes = countryCodes.Distinct().ToList();

                foreach (string countryCode in countryCodes)
                {
                    if (countryID != null)
                    {
                        if (!countryCode.Equals(countryID.ToString()))
                        {
                            continue;
                        }
                    }

                    RelativePerformanceActivePositionData record = new RelativePerformanceActivePositionData();
                    decimal? MarketValue = 0;
                    decimal? FundWeight = 0;
                    decimal? BenchmarkWeight = 0;

                    record.Entity = countryCode.ToString();


                    List<RelativePerformanceActivePositionData> sectorSpecificData = data.Where(row => row.EntityGroup == countryCode).ToList();

                    foreach (RelativePerformanceActivePositionData row in sectorSpecificData)
                    {
                        MarketValue = MarketValue + ((row.MarketValue) == null ? 0 : row.MarketValue);
                        FundWeight = FundWeight + ((row.FundWeight) == null ? 0 : row.FundWeight);
                        BenchmarkWeight = BenchmarkWeight + ((row.BenchmarkWeight) == null ? 0 : row.BenchmarkWeight);
                    }

                    record.MarketValue = MarketValue;
                    record.FundWeight = FundWeight;
                    record.BenchmarkWeight = BenchmarkWeight;
                    record.ActivePosition = Convert.ToDecimal(FundWeight - BenchmarkWeight);

                    result.Add(record);
                }

                return result.OrderByDescending(t => t.ActivePosition).ToList();

                
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        ///  Retrieves Sector Level Active Position Data for a particular composite/fund, effective date and period.
        /// </summary>
        /// <param name="portfolioSelectionData">PortfolioSelectionData object</param>
        /// <param name="effectiveDate">Effective date</param>
        /// <param name="period">Period</param>
        /// <param name="countryID">(optional) COUNTRY; By default Null</param>
        /// <param name="sectorID">(optional) GICS_SECTOR; By default Null</param>
        /// <returns>List of RelativePerformanceActivePositionData objects</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<RelativePerformanceActivePositionData> RetrieveRelativePerformanceSectorActivePositionData(PortfolioSelectionData portfolioSelectionData,
            DateTime effectiveDate, string period, string countryID = null, string sectorID = null)
        {
            try
            {
                List<RelativePerformanceActivePositionData> result = new List<RelativePerformanceActivePositionData>();
                if (portfolioSelectionData == null || effectiveDate == null || period == null)
                { 
                    return result; 
                }

                //checking if the service is down
                /*bool isServiceUp;
                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                { throw new Exception(); }*/

                DimensionEntities entity = DimensionEntity;

                IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> resultQuery = GetRelativePerformanceSectorDataQuery(portfolioSelectionData
                    , effectiveDate.Date, countryID, sectorID);

                #region Inception Date Check
                Boolean isInceptionDateValid = InceptionDateCheck(resultQuery, period, effectiveDate.Date);
                if (!isInceptionDateValid)
                {
                    return result;
                }
                #endregion

                #region Query Execution
                List<RelativePerformanceActivePositionData> data = new List<RelativePerformanceActivePositionData>();
                switch (period)
                {
                    case "1D":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_1D != 0 || g.POR_RC_TWR_1D != 0 || g.BM1_RC_AVG_WGT_1D != 0 || g.BM1_RC_TWR_1D != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1_LONG_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1D * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1D * 100
                        }).ToList();
                        break;
                    case "1W":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_1W != 0 || g.POR_RC_TWR_1W != 0 || g.BM1_RC_AVG_WGT_1W != 0 || g.BM1_RC_TWR_1W != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1_LONG_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1W * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1W * 100
                        }).ToList();
                        break;
                    case "MTD":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_MTD != 0 || g.POR_RC_TWR_MTD != 0 || g.BM1_RC_AVG_WGT_MTD != 0 || g.BM1_RC_TWR_MTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1_LONG_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_MTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_MTD * 100
                        }).ToList();
                        break;
                    case "QTD":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_QTD != 0 || g.POR_RC_TWR_QTD != 0 || g.BM1_RC_AVG_WGT_QTD != 0 || g.BM1_RC_TWR_QTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1_LONG_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_QTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_QTD * 100
                        }).ToList();
                        break;
                    case "1Y":
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_1Y != 0 || g.POR_RC_TWR_1Y != 0 || g.BM1_RC_AVG_WGT_1Y != 0 || g.BM1_RC_TWR_1Y != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1_LONG_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1Y * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1Y * 100
                        }).ToList();
                        break;
                    default:
                        data = resultQuery.Where(g => g.POR_RC_AVG_WGT_YTD != 0 || g.POR_RC_TWR_YTD != 0 || g.BM1_RC_AVG_WGT_YTD != 0 || g.BM1_RC_TWR_YTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.SEC_NAME,
                            EntityGroup = t.AGG_LVL_1_LONG_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_YTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_YTD * 100
                        }).ToList();
                        break;
                }
                #endregion

                //data = ListUtils.DistinctBy(data, g => new { g.Entity, g.EntityGroup, g.MarketValue, g.FundWeight, g.BenchmarkWeight }).ToList();
                
                List<RelativePerformanceSectorData> sectorCodes = data
                    .Select(t => new RelativePerformanceSectorData
                    {
                        SectorId = t.EntityGroup,
                        SectorName = t.EntityGroup
                    })
                    .ToList();

                sectorCodes = ListUtils.DistinctBy(sectorCodes, g => g.SectorId)
                    .OrderBy(r => r.SectorName).ToList();

                foreach (RelativePerformanceSectorData sector in sectorCodes)
                {
                    if (sectorID != null)
                    {
                        if (!sector.SectorId.Equals(sectorID))
                        {
                            continue;
                        }
                    }

                    RelativePerformanceActivePositionData record = new RelativePerformanceActivePositionData();
                    decimal? MarketValue = 0;
                    decimal? FundWeight = 0;
                    decimal? BenchmarkWeight = 0;

                    record.Entity = sector.SectorName.ToString();


                    List<RelativePerformanceActivePositionData> sectorSpecificData = data.Where(row => row.EntityGroup == sector.SectorId).ToList();

                    foreach (RelativePerformanceActivePositionData row in sectorSpecificData)
                    {
                        MarketValue = MarketValue + ((row.MarketValue) == null ? 0 : row.MarketValue);
                        FundWeight = FundWeight + ((row.FundWeight) == null ? 0 : row.FundWeight);
                        BenchmarkWeight = BenchmarkWeight + ((row.BenchmarkWeight) == null ? 0 : row.BenchmarkWeight);
                    }

                    record.MarketValue = MarketValue;
                    record.FundWeight = FundWeight;
                    record.BenchmarkWeight = BenchmarkWeight;
                    record.ActivePosition = Convert.ToDecimal(FundWeight - BenchmarkWeight);

                    result.Add(record);
                }

                return result.OrderByDescending(t => t.ActivePosition).ToList();
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        ///  Retrieves Security Level Active Position Data for a particular composite/fund, effective date and period.
        /// </summary>
        /// <param name="portfolioSelectionData">PortfolioSelectionData object</param>
        /// <param name="effectiveDate">Effective date</param>
        /// <param name="period">Period</param>
        /// <param name="countryID">(optional) COUNTRY; By default Null</param>
        /// <param name="sectorID">(optional) GICS_SECTOR; By default Null</param>
        /// <returns>List of RelativePerformanceActivePositionData objects</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<RelativePerformanceActivePositionData> RetrieveRelativePerformanceSecurityActivePositionData(PortfolioSelectionData portfolioSelectionData,
            DateTime effectiveDate, string period, string countryID = null, string sectorID = null)
        {
            try
            {
                List<RelativePerformanceActivePositionData> result = new List<RelativePerformanceActivePositionData>();
                if (portfolioSelectionData == null || effectiveDate == null || period == null)
                { return result; }

                //checking if the service is down
                /*bool isServiceUp;
                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                { throw new Exception(); }*/

                DimensionEntities entity = DimensionEntity;

                IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> resultQuery = GetRelativePerformanceSecurityDataQuery(portfolioSelectionData
                    , effectiveDate.Date, countryID, sectorID);

                #region Inception Date Check
                Boolean isInceptionDateValid = InceptionDateCheck(resultQuery, period, effectiveDate.Date);
                if (!isInceptionDateValid)
                {
                    return result;
                }
                #endregion

                #region Query Execution
                switch (period)
                {
                    case "1D":
                        result = resultQuery.Where(g => g.POR_RC_AVG_WGT_1D != 0 || g.POR_RC_TWR_1D != 0 || g.BM1_RC_AVG_WGT_1D != 0 || g.BM1_RC_TWR_1D != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.AGG_LVL_1_LONG_NAME,
                            EntityGroup = t.ISSUER_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1D * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1D * 100,
                           // ActivePosition = t.BM1_RC_ASSET_ALLOC_1D + t.BM1_RC_SEC_SELEC_1D
                            ActivePosition = t.POR_RC_AVG_WGT_1D * 100 - t.BM1_RC_AVG_WGT_1D * 100
                        }).ToList();
                        break;
                    case "1W":
                        result = resultQuery.Where(g => g.POR_RC_AVG_WGT_1W != 0 || g.POR_RC_TWR_1W != 0 || g.BM1_RC_AVG_WGT_1W != 0 || g.BM1_RC_TWR_1W != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.AGG_LVL_1_LONG_NAME,
                            EntityGroup = t.ISSUER_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1W * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1W * 100,
                           // ActivePosition = t.BM1_RC_ASSET_ALLOC_1W + t.BM1_RC_SEC_SELEC_1W
                            ActivePosition = t.POR_RC_AVG_WGT_1W * 100 - t.BM1_RC_AVG_WGT_1W * 100

                        }).ToList();
                        break;
                    case "MTD":
                        result = resultQuery.Where(g => g.POR_RC_AVG_WGT_MTD != 0 || g.POR_RC_TWR_MTD != 0 || g.BM1_RC_AVG_WGT_MTD != 0 || g.BM1_RC_TWR_MTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.AGG_LVL_1_LONG_NAME,
                            EntityGroup = t.ISSUER_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_MTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_MTD * 100,
                            //ActivePosition = t.BM1_RC_ASSET_ALLOC_MTD + t.BM1_RC_SEC_SELEC_MTD
                            ActivePosition = t.POR_RC_AVG_WGT_MTD * 100 - t.BM1_RC_AVG_WGT_MTD * 100
                        }).ToList();
                        break;
                    case "QTD":
                        result = resultQuery.Where(g => g.POR_RC_AVG_WGT_QTD != 0 || g.POR_RC_TWR_QTD != 0 || g.BM1_RC_AVG_WGT_QTD != 0 || g.BM1_RC_TWR_QTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.AGG_LVL_1_LONG_NAME,
                            EntityGroup = t.ISSUER_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_QTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_QTD * 100,
                            //ActivePosition = t.BM1_RC_ASSET_ALLOC_QTD + t.BM1_RC_SEC_SELEC_QTD
                            ActivePosition = t.POR_RC_AVG_WGT_QTD * 100 - t.BM1_RC_AVG_WGT_QTD * 100
                        }).ToList();
                        break;
                    case "1Y":
                        result = resultQuery.Where(g => g.POR_RC_AVG_WGT_1Y != 0 || g.POR_RC_TWR_1Y != 0 || g.BM1_RC_AVG_WGT_1Y != 0 || g.BM1_RC_TWR_1Y != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.AGG_LVL_1_LONG_NAME,
                            EntityGroup = t.ISSUER_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_1Y * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_1Y * 100,
                            //ActivePosition = t.BM1_RC_ASSET_ALLOC_1Y + t.BM1_RC_SEC_SELEC_1Y
                            ActivePosition = t.POR_RC_AVG_WGT_1Y * 100 - t.BM1_RC_AVG_WGT_1Y * 100
                        }).ToList();
                        break;
                    default:
                        result = resultQuery.Where(g => g.POR_RC_AVG_WGT_YTD != 0 || g.POR_RC_TWR_YTD != 0 || g.BM1_RC_AVG_WGT_YTD != 0 || g.BM1_RC_TWR_YTD != 0).Select(t => new RelativePerformanceActivePositionData
                        {
                            Entity = t.AGG_LVL_1_LONG_NAME,
                            EntityGroup = t.ISSUER_NAME,
                            MarketValue = t.POR_RC_MARKET_VALUE,
                            FundWeight = t.POR_RC_AVG_WGT_YTD * 100,
                            BenchmarkWeight = t.BM1_RC_AVG_WGT_YTD * 100,
                            //ActivePosition = t.BM1_RC_ASSET_ALLOC_YTD + t.BM1_RC_SEC_SELEC_YTD
                            ActivePosition = t.POR_RC_AVG_WGT_YTD * 100 - t.BM1_RC_AVG_WGT_YTD * 100
                        }).ToList();
                        break;
                }
                #endregion

                result = result.OrderByDescending(t => t.ActivePosition).ToList();
                //result = ListUtils.DistinctBy(result, g => new { g.Entity, g.EntityGroup, g.MarketValue, g.FundWeight, g.BenchmarkWeight, g.ActivePosition })
                //    .OrderByDescending(t => t.ActivePosition).ToList();
                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// Retrieves Security Level Relative Performance Data for a particular composite/fund, benchmark and efective date.
        /// Filtering data filtering based on ISO_COUNTRY_CODE, GICS_SECTOR and record restriction handled through optional arguments
        /// </summary>
        /// <param name="fundSelectionData">PortfolioSelectionData object</param>
        /// <param name="effectiveDate">Effective date</param>
        /// <param name="countryID">(optional) ISO_COUNTRY_CODE; By default Null</param>
        /// <param name="sectorID">(optional) GICS_SECTOR; By default Null</param>
        /// <returns>List of RetrieveRelativePerformanceSecurityData objects</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<RelativePerformanceSecurityData> RetrieveRelativePerformanceSecurityData(PortfolioSelectionData portfolioSelectionData,
            DateTime effectiveDate, string period, string countryID = null, string sectorID = null)
        {
            try
            {
                List<RelativePerformanceSecurityData> result = new List<RelativePerformanceSecurityData>();
                if (portfolioSelectionData == null || effectiveDate == null || period == null)
                { return result; }

                //checking if the service is down
              /*  bool isServiceUp;
                isServiceUp = CheckServiceAvailability.ServiceAvailability();
                if (!isServiceUp)
                { throw new Exception(); }*/

                DimensionEntities entity = DimensionEntity;

                IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> resultQuery = GetRelativePerformanceSecurityDataQuery(portfolioSelectionData
                    , effectiveDate.Date, countryID, sectorID);

                #region Inception Date Check
                Boolean isInceptionDateValid = InceptionDateCheck(resultQuery, period, effectiveDate.Date);
                if (!isInceptionDateValid)
                {
                    return result;
                }
                #endregion

                #region Query Execution
                switch (period)
                {
                    case "1D":
                        result = resultQuery.Select(t => new RelativePerformanceSecurityData
                        {
                            SecurityName = t.SEC_NAME,
                            SecurityCountryId = t.COUNTRY,
                            SecuritySectorName = t.GICS_LVL1,
                            SecurityMarketValue = t.POR_RC_MARKET_VALUE,
                            //SecurityAlpha = t.BM1_RC_ASSET_ALLOC_1D + t.BM1_RC_SEC_SELEC_1D
                            SecurityAlpha = t.F_BM1_ASH_ASSET_ALLOC_1D + t.F_BM1_ASH_SEC_SELEC_1D
                        }).ToList();
                        break;
                    case "1W":
                        result = resultQuery.Select(t => new RelativePerformanceSecurityData
                        {
                            SecurityName = t.SEC_NAME,
                            SecurityCountryId = t.COUNTRY,
                            SecuritySectorName = t.GICS_LVL1,
                            SecurityMarketValue = t.POR_RC_MARKET_VALUE,
                            //SecurityAlpha = t.BM1_RC_ASSET_ALLOC_1W + t.BM1_RC_SEC_SELEC_1W
                            SecurityAlpha = t.F_BM1_ASH_ASSET_ALLOC_1W + t.F_BM1_ASH_SEC_SELEC_1W
                        }).ToList();
                        break;
                    case "MTD":
                        result = resultQuery.Select(t => new RelativePerformanceSecurityData
                        {
                            SecurityName = t.SEC_NAME,
                            SecurityCountryId = t.COUNTRY,
                            SecuritySectorName = t.GICS_LVL1,
                            SecurityMarketValue = t.POR_RC_MARKET_VALUE,
                            //SecurityAlpha = t.BM1_RC_ASSET_ALLOC_MTD + t.BM1_RC_SEC_SELEC_MTD
                            SecurityAlpha = t.F_BM1_ASH_ASSET_ALLOC_MTD + t.F_BM1_ASH_SEC_SELEC_MTD
                        }).ToList();
                        break;
                    case "QTD":
                        result = resultQuery.Select(t => new RelativePerformanceSecurityData
                        {
                            SecurityName = t.SEC_NAME,
                            SecurityCountryId = t.COUNTRY,
                            SecuritySectorName = t.GICS_LVL1,
                            SecurityMarketValue = t.POR_RC_MARKET_VALUE,
                            //SecurityAlpha = t.BM1_RC_ASSET_ALLOC_QTD + t.BM1_RC_SEC_SELEC_QTD
                            SecurityAlpha = t.F_BM1_ASH_ASSET_ALLOC_QTD + t.F_BM1_ASH_SEC_SELEC_QTD
                        }).ToList();
                        break;
                    case "1Y":
                        result = resultQuery.Select(t => new RelativePerformanceSecurityData
                        {
                            SecurityName = t.SEC_NAME,
                            SecurityCountryId = t.COUNTRY,
                            SecuritySectorName = t.GICS_LVL1,
                            SecurityMarketValue = t.POR_RC_MARKET_VALUE,
                            //SecurityAlpha = t.BM1_RC_ASSET_ALLOC_1Y + t.BM1_RC_SEC_SELEC_1Y
                            SecurityAlpha = t.F_BM1_ASH_ASSET_ALLOC_1Y + t.F_BM1_ASH_SEC_SELEC_1Y
                        }).ToList();
                        break;
                    default:
                        result = resultQuery.Select(t => new RelativePerformanceSecurityData
                        {
                            SecurityName = t.SEC_NAME,
                            SecurityCountryId = t.COUNTRY,
                            SecuritySectorName = t.GICS_LVL1,
                            SecurityMarketValue = t.POR_RC_MARKET_VALUE,
                            //SecurityAlpha = t.BM1_RC_ASSET_ALLOC_YTD + t.BM1_RC_SEC_SELEC_YTD
                            SecurityAlpha = t.F_BM1_ASH_ASSET_ALLOC_YTD + t.F_BM1_ASH_SEC_SELEC_YTD
                        }).ToList();
                        break;
                } 
                #endregion

                result = result.OrderByDescending(t => t.SecurityAlpha).ToList();
                //result = ListUtils.DistinctBy(result, g => new { g.SecurityName, g.SecurityCountryId, g.SecurityAlpha, g.SecurityMarketValue, g.SecuritySectorName})
                //    .OrderByDescending(t => t.SecurityAlpha).ToList();
                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }        

        #region Relative Performance Helper Methods
        /// <summary>
        /// retrieving data from GF_PERF_DAILY_ATTRIBUTION view for relative performance gadgets
        /// </summary>
        /// <param name="portfolioSelectionData"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="country"></param>
        /// <param name="sector"></param>
        /// <returns>GF_PERF_DAILY_ATTRIBUTION Collection</returns>
        private IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> GetRelativePerformanceSecurityDataQuery(PortfolioSelectionData portfolioSelectionData,
            DateTime effectiveDate, string countryID = null, string sectorID = null)
        {
            DimensionEntities entity = DimensionEntity;
            IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> resultQuery = entity.GF_PERF_DAILY_ATTRIBUTION
                    .Where(t => t.PORTFOLIO == portfolioSelectionData.PortfolioId &&
                        t.TO_DATE ==effectiveDate &&
                        t.NODE_NAME == "Security ID" &&
                        t.AGG_LVL_1_LONG_NAME != null &&
                        t.SEC_INV_THEME == "EQUITY");
            if (sectorID != null)
            {
                resultQuery = resultQuery.Where(t => t.GICS_LVL1 == sectorID);
            }

            if (countryID != null)
            {
                resultQuery = resultQuery.Where(t => t.COUNTRY == countryID);
            }

            return resultQuery;
        }

        private IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> GetRelativePerformanceSectorDataQuery(PortfolioSelectionData portfolioSelectionData,
            DateTime effectiveDate, string countryID = null, string sectorID = null)
        {
            DimensionEntities entity = DimensionEntity;
            IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> resultQuery = entity.GF_PERF_DAILY_ATTRIBUTION
                    .Where(t => t.PORTFOLIO == portfolioSelectionData.PortfolioId &&
                        t.TO_DATE ==effectiveDate &&
                        t.NODE_NAME == "GICS Level 1" &&
                        t.AGG_LVL_1_LONG_NAME != "-" &&
                        t.AGG_LVL_1_LONG_NAME != null);
            if (sectorID != null)
            {
                resultQuery = resultQuery.Where(t => t.AGG_LVL_1_LONG_NAME == sectorID);
            }

            if (countryID != null)
            {
                resultQuery = resultQuery.Where(t => t.COUNTRY == countryID);
            }

            return resultQuery;
        }

        private IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> GetRelativePerformanceCountryDataQuery(PortfolioSelectionData portfolioSelectionData,
            DateTime effectiveDate, string countryID = null, string sectorID = null)
        {
            DimensionEntities entity = DimensionEntity;
            IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> resultQuery = entity.GF_PERF_DAILY_ATTRIBUTION
                    .Where(t => t.PORTFOLIO == portfolioSelectionData.PortfolioId &&
                        t.TO_DATE == effectiveDate &&
                        t.NODE_NAME == "Country" &&
                        t.AGG_LVL_1 != null);
            if (sectorID != null)
            {
                resultQuery = resultQuery.Where(t => t.GICS_LVL1 == sectorID);
            }

            if (countryID != null)
            {
                resultQuery = resultQuery.Where(t => t.AGG_LVL_1 == countryID);
            }

            return resultQuery;
        }

        private Boolean InceptionDateCheck(IQueryable<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> query, String period, DateTime effectiveDate)
        {
            Boolean result = true;
            if (period != "1D" && period != "1W")
            {
                System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                dateInfo.ShortDatePattern = "dd/MM/yyyy";
                int recordCount = query.Select(t => new { t.POR_INCEPTION_DATE }).Count();
                if (recordCount == 0)
                {
                    return false;
                }

                var defaultInceptionDate = query.Select(t => new { t.POR_INCEPTION_DATE }).FirstOrDefault();
                DateTime portfolioInceptionDate = Convert.ToDateTime(defaultInceptionDate.POR_INCEPTION_DATE.ToString(), dateInfo);

                bool isValid = InceptionDateChecker.ValidateInceptionDate(period, portfolioInceptionDate, effectiveDate);
                if (!isValid)
                {
                    return false;
                }
            }

            return result;
        }

        

        ///// <summary>
        ///// retrieving alpha values based on period selected for relative performance gadget
        ///// </summary>
        ///// <param name="row"></param>
        ///// <param name="period"></param>
        ///// <returns></returns>
        //private decimal? RetrieveRelativePerformanceAlphaValue(GF_PERF_DAILY_ATTRIBUTION row, string period)
        //{
        //    decimal? alpha = 0;
        //    switch (period)
        //    {
        //        case "1D":
        //            alpha = row.BM1_RC_ASSET_ALLOC_1D + row.BM1_RC_SEC_SELEC_1D;
        //            break;

        //        case "1W":
        //            alpha = row.BM1_RC_ASSET_ALLOC_1W + row.BM1_RC_SEC_SELEC_1W;
        //            break;

        //        case "MTD":
        //            alpha = row.BM1_RC_ASSET_ALLOC_MTD + row.BM1_RC_SEC_SELEC_MTD;
        //            break;

        //        case "QTD":
        //            alpha = row.BM1_RC_ASSET_ALLOC_QTD + row.BM1_RC_SEC_SELEC_QTD;
        //            break;

        //        case "1Y":
        //            alpha = row.BM1_RC_ASSET_ALLOC_1Y + row.BM1_RC_SEC_SELEC_1Y;
        //            break;

        //        case "YTD":
        //            alpha = row.BM1_RC_ASSET_ALLOC_YTD + row.BM1_RC_SEC_SELEC_YTD;
        //            break;
        //        default:
        //            break;
        //    }
        //    return alpha;
        //}

        ///// <summary>
        ///// retrieving benchmark weights based on period selected for relative performance gadget
        ///// </summary>
        ///// <param name="row"></param>
        ///// <param name="period"></param>
        ///// <returns></returns>
        //private decimal? RetrieveRelativePerformanceBenchmarkWeight(GF_PERF_DAILY_ATTRIBUTION row, string period)
        //{
        //    decimal? benchmarkWeight = 0;
        //    switch (period)
        //    {
        //        case "1D":
        //            benchmarkWeight = (row.BM1_RC_AVG_WGT_1D) * 100;
        //            break;

        //        case "1W":
        //            benchmarkWeight = (row.BM1_RC_AVG_WGT_1W) * 100;
        //            break;

        //        case "MTD":
        //            benchmarkWeight = (row.BM1_RC_AVG_WGT_MTD) * 100;
        //            break;

        //        case "YTD":
        //            benchmarkWeight = (row.BM1_RC_AVG_WGT_YTD) * 100;
        //            break;

        //        case "QTD":
        //            benchmarkWeight = (row.BM1_RC_AVG_WGT_QTD) * 100;
        //            break;

        //        case "1Y":
        //            benchmarkWeight = (row.BM1_RC_AVG_WGT_1Y) * 100;
        //            break;

        //        default:
        //            break;
        //    }
        //    return benchmarkWeight;
        //}

        ///// <summary>
        ///// retrieving portfolio weights based on period selected for relative performance gadget
        ///// </summary>
        ///// <param name="row"></param>
        ///// <param name="period"></param>
        ///// <returns></returns>
        //private decimal? RetrieveRelativePerformancePortfolioWeight(GF_PERF_DAILY_ATTRIBUTION row, string period)
        //{
        //    decimal? portfolioWeight = 0;
        //    switch (period)
        //    {
        //        case "1D":
        //            portfolioWeight = (row.POR_RC_AVG_WGT_1D) * 100;
        //            break;

        //        case "1W":
        //            portfolioWeight = (row.POR_RC_AVG_WGT_1W) * 100;
        //            break;

        //        case "MTD":
        //            portfolioWeight = (row.POR_RC_AVG_WGT_MTD) * 100;
        //            break;

        //        case "YTD":
        //            portfolioWeight = (row.POR_RC_AVG_WGT_YTD) * 100;
        //            break;

        //        case "QTD":
        //            portfolioWeight = (row.POR_RC_AVG_WGT_QTD) * 100;
        //            break;

        //        case "1Y":
        //            portfolioWeight = (row.POR_RC_AVG_WGT_1Y) * 100;
        //            break;

        //        default:
        //            break;
        //    }
        //    return portfolioWeight;
        //}
        #endregion

        #endregion

        #region Market Capitalization Methods
        /// <summary>
        /// Retrieves consolidated data for portfolio and benchmark
        /// </summary>
        /// <param name="selPortfolioID">Contains Selected Portfolio ID</param>
        /// <param name="selPortfolioDate">Effective Date selected by user</param>
        /// <returns>Consolidated list of portfolio and benchmark</returns>        
        private List<MarketCapitalizationData> RetrievePortfolioMktCapData(PortfolioSelectionData portfolio_ID, DateTime effectiveDate, String filterType, String filterValue, bool isExCashSecurity, bool lookThruEnabled)
        {

            List<MarketCapitalizationData> result = new List<MarketCapitalizationData>();
            //List<DimensionEntitiesService.GF_PORTFOLIO_HOLDINGS> filteredResult = new List<GF_PORTFOLIO_HOLDINGS>();
            DimensionEntities entity = DimensionEntity;
            List<GreenField.DAL.GF_PORTFOLIO_HOLDINGS> dimensionServicePortfolioData = null;
            List<GreenField.DAL.GF_PORTFOLIO_LTHOLDINGS> dimensionServicePortfolioLTData = null;

            if (entity.GF_PORTFOLIO_HOLDINGS == null && entity.GF_BENCHMARK_HOLDINGS.Count() == 0)
                return null;

            if (lookThruEnabled)
            {
                if (isExCashSecurity)
                {
                    dimensionServicePortfolioLTData = entity.GF_PORTFOLIO_LTHOLDINGS
                            .Where(portfolioList => (portfolioList.PORTFOLIO_ID == portfolio_ID.PortfolioId)
                                && (portfolioList.PORTFOLIO_DATE == effectiveDate)
                                && (portfolioList.SECURITYTHEMECODE.ToUpper() != GreenfieldConstants.CASH)).ToList();
                }
                else
                {
                    dimensionServicePortfolioLTData = entity.GF_PORTFOLIO_LTHOLDINGS
                            .Where(portfolioList => (portfolioList.PORTFOLIO_ID == portfolio_ID.PortfolioId)
                                && (portfolioList.PORTFOLIO_DATE == effectiveDate)).ToList();
                }
            }
            else
            {
                if (isExCashSecurity)
                {
                    dimensionServicePortfolioData = entity.GF_PORTFOLIO_HOLDINGS
                            .Where(portfolioList => (portfolioList.PORTFOLIO_ID == portfolio_ID.PortfolioId)
                                && (portfolioList.PORTFOLIO_DATE == effectiveDate)
                                && (portfolioList.SECURITYTHEMECODE.ToUpper() != GreenfieldConstants.CASH)).ToList();
                }
                else
                {
                    dimensionServicePortfolioData = entity.GF_PORTFOLIO_HOLDINGS
                            .Where(portfolioList => (portfolioList.PORTFOLIO_ID == portfolio_ID.PortfolioId)
                                && (portfolioList.PORTFOLIO_DATE == effectiveDate)).ToList();
                }
            }
            if ((dimensionServicePortfolioData == null || dimensionServicePortfolioData.Count == 0) && (dimensionServicePortfolioLTData == null || dimensionServicePortfolioLTData.Count == 0))
                return result;


            //Applying filters
            if (filterType != null)//&& filterValue != null)
            {
                switch (filterType)
                {
                    case GreenfieldConstants.REGION:
                        if (lookThruEnabled)
                            dimensionServicePortfolioLTData = dimensionServicePortfolioLTData.Where(list => (list.ASHEMM_PROP_REGION_CODE == filterValue)).ToList();
                        else
                            dimensionServicePortfolioData = dimensionServicePortfolioData.Where(list => (list.ASHEMM_PROP_REGION_CODE == filterValue)).ToList();

                        break;
                    case GreenfieldConstants.COUNTRY:
                        if (lookThruEnabled)
                            dimensionServicePortfolioLTData = dimensionServicePortfolioLTData.Where(list => (list.ISO_COUNTRY_CODE == filterValue)).ToList();
                        else
                            dimensionServicePortfolioData = dimensionServicePortfolioData.Where(list => (list.ISO_COUNTRY_CODE == filterValue)).ToList();

                        break;
                    case GreenfieldConstants.INDUSTRY:
                        if (lookThruEnabled)
                            dimensionServicePortfolioLTData = dimensionServicePortfolioLTData.Where(list => (list.GICS_INDUSTRY_NAME == filterValue)).ToList();
                        else
                            dimensionServicePortfolioData = dimensionServicePortfolioData.Where(list => (list.GICS_INDUSTRY_NAME == filterValue)).ToList();

                        break;
                    case GreenfieldConstants.SECTOR:
                        if (lookThruEnabled)
                            dimensionServicePortfolioLTData = dimensionServicePortfolioLTData.Where(list => (list.GICS_SECTOR_NAME == filterValue)).ToList();
                        else
                            dimensionServicePortfolioData = dimensionServicePortfolioData.Where(list => (list.GICS_SECTOR_NAME == filterValue)).ToList();

                        break;
                    case GreenfieldConstants.SHOW_EVERYTHING:
                        if (lookThruEnabled)
                            dimensionServicePortfolioLTData = entity.GF_PORTFOLIO_LTHOLDINGS
                            .Where(portfolioList => (portfolioList.PORTFOLIO_ID == portfolio_ID.PortfolioId)
                                && (portfolioList.PORTFOLIO_DATE == effectiveDate)).ToList();
                        else
                            dimensionServicePortfolioData = entity.GF_PORTFOLIO_HOLDINGS
                            .Where(portfolioList => (portfolioList.PORTFOLIO_ID == portfolio_ID.PortfolioId)
                                && (portfolioList.PORTFOLIO_DATE == effectiveDate)).ToList();

                        break;
                    //default:
                    //    break;
                }
            }
            if (lookThruEnabled)
            {
                for (int _index = 0; _index < dimensionServicePortfolioLTData.Count; _index++)
                {
                    MarketCapitalizationData mktCapData = new MarketCapitalizationData();
                    mktCapData.PortfolioDirtyValuePC = dimensionServicePortfolioLTData[_index].DIRTY_VALUE_PC;
                    mktCapData.MarketCapitalInUSD = dimensionServicePortfolioLTData[_index].MARKET_CAP_IN_USD;
                    mktCapData.SecurityThemeCode = dimensionServicePortfolioLTData[_index].SECURITYTHEMECODE;
                    mktCapData.Benchmark_ID = dimensionServicePortfolioLTData[_index].BENCHMARK_ID;
                    mktCapData.Portfolio_ID = dimensionServicePortfolioLTData[_index].PORTFOLIO_ID;
                    mktCapData.AsecSecShortName = dimensionServicePortfolioLTData[_index].ASEC_SEC_SHORT_NAME;
                    mktCapData.BenchmarkWeight = 0;

                    result.Add(mktCapData);
                }
            }
            else
            {
                for (int _index = 0; _index < dimensionServicePortfolioData.Count; _index++)
                {
                    MarketCapitalizationData mktCapData = new MarketCapitalizationData();
                    mktCapData.PortfolioDirtyValuePC = dimensionServicePortfolioData[_index].DIRTY_VALUE_PC;
                    mktCapData.MarketCapitalInUSD = dimensionServicePortfolioData[_index].MARKET_CAP_IN_USD;
                    mktCapData.SecurityThemeCode = dimensionServicePortfolioData[_index].SECURITYTHEMECODE;
                    mktCapData.Benchmark_ID = dimensionServicePortfolioData[_index].BENCHMARK_ID;
                    mktCapData.Portfolio_ID = dimensionServicePortfolioData[_index].PORTFOLIO_ID;
                    mktCapData.AsecSecShortName = dimensionServicePortfolioData[_index].ASEC_SEC_SHORT_NAME;
                    mktCapData.BenchmarkWeight = 0;

                    result.Add(mktCapData);
                }
            }
            //List<MarketCapitalizationData> _portfolioBenchmarkData = RetrieveBenchmarkMktCapData(result, effectiveDate, filterType, filterValue, isExCashSecurity);
            return result;

        }

        /// <summary>
        /// Retrieves bechmark data
        /// </summary>
        /// <param name="portfolioData"></param>
        /// <param name="effectiveDate"></param>
        /// <returns>bechmark data</returns>
        private List<MarketCapitalizationData> RetrieveBenchmarkMktCapData(List<MarketCapitalizationData> portfolioData, DateTime effectiveDate, String filterType, String filterValue, bool isExCashSecurity)
        {

            //List<DimensionEntitiesService.GF_BENCHMARK_HOLDINGS> filteredResult = new List<GF_BENCHMARK_HOLDINGS>();
            DimensionEntities entity = DimensionEntity;
            List<GreenField.DAL.GF_BENCHMARK_HOLDINGS> dimensionServiceBenchmarkData = null;
            List<MarketCapitalizationData> result = new List<MarketCapitalizationData>();

            //Retrieve the Id of benchmark associated with the Portfolio
            List<string> benchmarkId = portfolioData.Select(a => a.Benchmark_ID).Distinct().ToList();

            //If the DataBase doesn't return a single Benchmark for a Portfolio
            if (benchmarkId == null || benchmarkId.Count != 1)
                throw new InvalidOperationException();

            if (isExCashSecurity)
            {
                dimensionServiceBenchmarkData = entity.GF_BENCHMARK_HOLDINGS.
                    Where(benchMarklist => (benchMarklist.BENCHMARK_ID == benchmarkId.First())
                    && (benchMarklist.PORTFOLIO_DATE == effectiveDate.Date)
                    && (benchMarklist.SECURITYTHEMECODE.ToUpper() != GreenfieldConstants.CASH)).ToList();
            }
            else
            {
                dimensionServiceBenchmarkData = entity.GF_BENCHMARK_HOLDINGS.
                    Where(benchMarklist => (benchMarklist.BENCHMARK_ID == benchmarkId.First())
                    && (benchMarklist.PORTFOLIO_DATE == effectiveDate.Date)).ToList();
            }

            if (dimensionServiceBenchmarkData.Count < 1)
                return null;

            //Applying filters
            if (filterType != null)//&& filterValue != null)
            {
                switch (filterType)
                {
                    case GreenfieldConstants.REGION:
                        dimensionServiceBenchmarkData = dimensionServiceBenchmarkData.Where(list => (list.ASHEMM_PROP_REGION_CODE == filterValue)).ToList();

                        break;
                    case GreenfieldConstants.COUNTRY:
                        dimensionServiceBenchmarkData = dimensionServiceBenchmarkData.Where(list => (list.ISO_COUNTRY_CODE == filterValue)).ToList();

                        break;
                    case GreenfieldConstants.INDUSTRY:
                        dimensionServiceBenchmarkData = dimensionServiceBenchmarkData.Where(list => (list.GICS_INDUSTRY_NAME == filterValue)).ToList();

                        break;
                    case GreenfieldConstants.SECTOR:
                        dimensionServiceBenchmarkData = dimensionServiceBenchmarkData.Where(list => (list.GICS_SECTOR_NAME == filterValue)).ToList();

                        break;
                    case GreenfieldConstants.SHOW_EVERYTHING:
                        dimensionServiceBenchmarkData = entity.GF_BENCHMARK_HOLDINGS.
                            Where(benchMarklist => (benchMarklist.BENCHMARK_ID == benchmarkId.First())
                            && (benchMarklist.PORTFOLIO_DATE == effectiveDate.Date)).ToList();

                        break;

                }
            }
            for (int _index = 0; _index < dimensionServiceBenchmarkData.Count; _index++)
            {
                MarketCapitalizationData mktCapData = new MarketCapitalizationData();
                mktCapData.MarketCapitalInUSD = dimensionServiceBenchmarkData[_index].MARKET_CAP_IN_USD;
                mktCapData.SecurityThemeCode = dimensionServiceBenchmarkData[_index].SECURITYTHEMECODE;
                mktCapData.BenchmarkWeight = dimensionServiceBenchmarkData[_index].BENCHMARK_WEIGHT;
                mktCapData.AsecSecShortName = dimensionServiceBenchmarkData[_index].ASEC_SEC_SHORT_NAME;
                result.Add(mktCapData);
            }

            //Add benchmark wieghts if ASEC_SEC_SHORT_NAME does not exist in portfolio list
            //foreach (GF_BENCHMARK_HOLDINGS benchmarkData in dimensionServiceBenchmarkData)
            //{
            //    var existingPortfolio = from p in portfolioData
            //                            where p.AsecSecShortName.ToLower() == benchmarkData.ASEC_SEC_SHORT_NAME.ToLower()
            //                            select p;

            //    if (existingPortfolio.Count() == 0)
            //    {
            //        MarketCapitalizationData mktCapData = new MarketCapitalizationData();

            //        mktCapData.MarketCapitalInUSD = benchmarkData.MARKET_CAP_IN_USD;
            //        mktCapData.SecurityThemeCode = benchmarkData.SECURITYTHEMECODE;
            //        mktCapData.BenchmarkWeight = benchmarkData.BENCHMARK_WEIGHT;
            //        mktCapData.AsecSecShortName = benchmarkData.ASEC_SEC_SHORT_NAME;

            //        portfolioData.Add(mktCapData);
            //    }           
            //}

            return result;

        }

        /// <summary>
        /// Retrieves portfolio and benchmark data for market capitalization grid
        /// </summary>
        /// <param name="portfolioSelectionData">Contains Selected Fund Data</param>
        /// <param name="effectiveDate">Effectice date as selected by the user</param>
        /// <param name="filterType">The Filter type selected by the user</param>
        /// <param name="filterValue">The Filter value selected by the user</param>
        /// <returns>List of MarketCapitalizationData </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<MarketCapitalizationData> RetrieveMarketCapitalizationData(PortfolioSelectionData portfolioSelectionData, DateTime effectiveDate, String filterType, String filterValue, bool isExCashSecurity, bool lookThruEnabled)
        {
            try
            {
                if (portfolioSelectionData == null || effectiveDate == null)
                    throw new ArgumentNullException(ServiceFaultResourceManager.GetString(GreenfieldConstants.SERVICE_NULL_ARG_EXC_MSG).ToString());

                List<MarketCapitalizationData> portfollioData = new List<MarketCapitalizationData>();
                List<MarketCapitalizationData> benchmarkData = new List<MarketCapitalizationData>();
                List<MarketCapitalizationData> result = new List<MarketCapitalizationData>();
                MarketCapitalizationData mktCapData = new MarketCapitalizationData();


                portfollioData = RetrievePortfolioMktCapData(portfolioSelectionData, effectiveDate, filterType, filterValue, isExCashSecurity, lookThruEnabled);

                if (portfollioData == null || portfollioData.Count == 0)
                    return portfollioData;

                benchmarkData = RetrieveBenchmarkMktCapData(portfollioData, effectiveDate, filterType, filterValue, isExCashSecurity);

                //****************** PORTFOLIO CALCULATIONS ***********************//

                //weighted avg for portfolio                 
                mktCapData.PortfolioWtdAvg = MarketCapitalizationCalculations.CalculatePortfolioWeightedAvg(portfollioData);

                //weighted median for portfolio
                mktCapData.PortfolioWtdMedian = MarketCapitalizationCalculations.CalculatePortfolioWeightedMedian(portfollioData);

                //ranges for portfolio
                List<MarketCapitalizationData> lstmktCapPortfolio = MarketCapitalizationCalculations.CalculateSumPortfolioRanges(portfollioData);
                mktCapData.PortfolioSumMegaRange = lstmktCapPortfolio[0].PortfolioSumMegaRange;
                mktCapData.PortfolioSumLargeRange = lstmktCapPortfolio[0].PortfolioSumLargeRange;
                mktCapData.PortfolioSumMediumRange = lstmktCapPortfolio[0].PortfolioSumMediumRange;
                mktCapData.PortfolioSumSmallRange = lstmktCapPortfolio[0].PortfolioSumSmallRange;
                mktCapData.PortfolioSumMicroRange = lstmktCapPortfolio[0].PortfolioSumMicroRange;
                mktCapData.PortfolioSumUndefinedRange = lstmktCapPortfolio[0].PortfolioSumUndefinedRange;

                //Lower and upper limits range values are coming from portfolio list
                mktCapData.LargeRange = lstmktCapPortfolio[0].LargeRange;
                mktCapData.MediumRange = lstmktCapPortfolio[0].MediumRange;
                mktCapData.SmallRange = lstmktCapPortfolio[0].SmallRange;
                mktCapData.MicroRange = lstmktCapPortfolio[0].MicroRange;
                mktCapData.UndefinedRange = lstmktCapPortfolio[0].UndefinedRange;

                //****************** BENCHMARK CALCULATIONS ***********************//

                if (benchmarkData != null || benchmarkData.Count > 0)
                {
                    //weighted avg for benchmark
                    mktCapData.BenchmarkWtdAvg = MarketCapitalizationCalculations.CalculateBenchmarkWeightedAvg(benchmarkData, filterType, filterValue, isExCashSecurity);

                    //weighted median for benchmark
                    mktCapData.BenchmarkWtdMedian = MarketCapitalizationCalculations.CalculateBenchmarkWeightedMedian(benchmarkData, filterType, filterValue, isExCashSecurity);

                    //ranges for benchmark
                    List<MarketCapitalizationData> lstmktCapBenchmark = MarketCapitalizationCalculations.CalculateSumBenchmarkRanges(benchmarkData, filterType, filterValue, isExCashSecurity);
                    mktCapData.BenchmarkSumMegaRange = lstmktCapBenchmark[0].BenchmarkSumMegaRange;
                    mktCapData.BenchmarkSumLargeRange = lstmktCapBenchmark[0].BenchmarkSumLargeRange;
                    mktCapData.BenchmarkSumMediumRange = lstmktCapBenchmark[0].BenchmarkSumMediumRange;
                    mktCapData.BenchmarkSumSmallRange = lstmktCapBenchmark[0].BenchmarkSumSmallRange;
                    mktCapData.BenchmarkSumMicroRange = lstmktCapBenchmark[0].BenchmarkSumMicroRange;
                    mktCapData.BenchmarkSumUndefinedRange = lstmktCapBenchmark[0].BenchmarkSumUndefinedRange;
                }

                result.Add(mktCapData);
                return result;

            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString(GreenfieldConstants.NETWORK_FAULT_ECX_MSG).ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }


        }
        #endregion

        #region Performance Summary Gadgets

        /// <summary>
        /// Retrieves Performance grid data for a particular composite/fund.
        /// Filtering data based on the fund name and Effective date.
        /// </summary>
        /// <param name="portfolioSelectionData">Portfolio Data that contains the name of the selected portfolio</param>
        /// <param name="effectiveDate">Selected Effective Date</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<PerformanceGridData> RetrievePerformanceGridData(PortfolioSelectionData portfolioSelectionData, DateTime effectiveDate, String country)
        {
            List<PerformanceGridData> result = new List<PerformanceGridData>();
            if (portfolioSelectionData == null || effectiveDate == null)
            {
                return result;
            }
            //checking if the service is down
          /*  bool isServiceUp;
            isServiceUp = CheckServiceAvailability.ServiceAvailability();
            if (!isServiceUp)
            {
                throw new Exception();
            }*/
            GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION performanceData;
            if (country == "NoFiltering")
            {
                performanceData = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == portfolioSelectionData.PortfolioId && t.TO_DATE == effectiveDate).FirstOrDefault();
            }
            else
            {
                performanceData = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == portfolioSelectionData.PortfolioId && t.TO_DATE == effectiveDate && t.AGG_LVL_1 == country).FirstOrDefault();
            }
            if (performanceData == null)
            {
                return result;
            }

            #region  Inception check
            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";
            DateTime portfolioInceptionDate = Convert.ToDateTime((performanceData.POR_INCEPTION_DATE), dateInfo);
            bool isValidMTD = InceptionDateChecker.ValidateInceptionDate("MTD", portfolioInceptionDate, effectiveDate);
            if (!isValidMTD)
            {
                performanceData.POR_TOP_RC_TWR_MTD = null;
                performanceData.BM1_TOP_RC_TWR_MTD = null;
            }
            bool isValidQTD = InceptionDateChecker.ValidateInceptionDate("QTD", portfolioInceptionDate, effectiveDate);
            if (!isValidQTD)
            {
                performanceData.POR_TOP_RC_TWR_QTD = null;
                performanceData.BM1_TOP_RC_TWR_QTD = null;
            }
            bool isValidYTD = InceptionDateChecker.ValidateInceptionDate("YTD", portfolioInceptionDate, effectiveDate);
            if (!isValidYTD)
            {
                performanceData.POR_TOP_RC_TWR_YTD = null;
                performanceData.BM1_TOP_RC_TWR_YTD = null;
            }
            bool isValid1Y = InceptionDateChecker.ValidateInceptionDate("1Y", portfolioInceptionDate, effectiveDate);
            if (!isValid1Y)
            {
                performanceData.POR_TOP_RC_TWR_1Y = null;
                performanceData.BM1_TOP_RC_TWR_1Y = null;
            }
            #endregion

            String benchmarkID;
            String portfolioID = performanceData.PORTFOLIO;
            List<GreenField.DAL.GF_PORTFOLIO_HOLDINGS> holdingsData = DimensionEntity.GF_PORTFOLIO_HOLDINGS.Where(t => t.PORTFOLIO_ID == portfolioID).ToList();
            if (holdingsData != null && holdingsData.Count != 0)
            {
                benchmarkID = holdingsData.FirstOrDefault().BENCHMARK_ID;
            }
            else
            {
                benchmarkID = null;
            }
            try
            {
                {
                    PerformanceGridData entry = new PerformanceGridData();
                    entry.Name = portfolioID;
                    entry.TopRcTwr1D = performanceData.POR_TOP_RC_TWR_1D;
                    entry.TopRcTwr1W = performanceData.POR_TOP_RC_TWR_1W;
                    entry.TopRcTwrMtd = performanceData.POR_TOP_RC_TWR_MTD;
                    entry.TopRcTwrQtd = performanceData.POR_TOP_RC_TWR_QTD;
                    entry.TopRcTwrYtd = performanceData.POR_TOP_RC_TWR_YTD;
                    entry.TopRcTwr1Y = performanceData.POR_TOP_RC_TWR_1Y;
                    result.Add(entry);
                    entry = new PerformanceGridData();
                    entry.Name = benchmarkID;
                    entry.TopRcTwr1D = performanceData.BM1_TOP_RC_TWR_1D;
                    entry.TopRcTwr1W = performanceData.BM1_TOP_RC_TWR_1W;
                    entry.TopRcTwrMtd = performanceData.BM1_TOP_RC_TWR_MTD;
                    entry.TopRcTwrQtd = performanceData.BM1_TOP_RC_TWR_QTD;
                    entry.TopRcTwrYtd = performanceData.BM1_TOP_RC_TWR_YTD;
                    entry.TopRcTwr1Y = performanceData.BM1_TOP_RC_TWR_1Y;
                    result.Add(entry);
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// Retrieves Performance graph data for a particular composite/fund.
        /// Filtering data based on the fund name.
        /// </summary>
        /// <param name="nameOfFund">Name of the selected fund</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<PerformanceGraphData> RetrievePerformanceGraphData(PortfolioSelectionData fundSelectionData, DateTime effectiveDate, 
            String period, String country)
        {
            List<PerformanceGraphData> result = new List<PerformanceGraphData>();
            PerformanceGraphData entry = new PerformanceGraphData();
            if (fundSelectionData == null || effectiveDate == null)
            {
                return result;
            }
            //checking if the service is down
            /*bool isServiceUp;*/
            EqualityComparer<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> customComparer = new GreenField.Web.Services.PerformanceOperations.GF_PERF_DAILY_ATTRIBUTION_Comparer();
            /*isServiceUp = CheckServiceAvailability.ServiceAvailability();*/
            String benchmarkID;
            List<GreenField.DAL.GF_PORTFOLIO_HOLDINGS> holdingsData = DimensionEntity.GF_PORTFOLIO_HOLDINGS.Where(t => t.PORTFOLIO_ID == fundSelectionData.PortfolioId).ToList();
            if (holdingsData != null && holdingsData.Count != 0)
            {
                benchmarkID = holdingsData.FirstOrDefault().BENCHMARK_ID;
            }
            else
            {
                benchmarkID = null;
            }
          /*  if (!isServiceUp)
            {
                throw new Exception();
            }*/
            try
            {
                switch (period)
                {
                    case "1D":
                        #region When Period Selected is "1D"
                        List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionDatafor1D;
                        if (country == "NoFiltering")
                        {
                            attributionDatafor1D = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == fundSelectionData.PortfolioId && t.TO_DATE == effectiveDate &&
                                t.NODE_NAME == "Country").ToList();
                            attributionDatafor1D = attributionDatafor1D.Distinct(customComparer).ToList();
                        }
                        else
                        {
                            attributionDatafor1D = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == fundSelectionData.PortfolioId && t.TO_DATE == effectiveDate &&
                                t.AGG_LVL_1 == country).ToList();
                            attributionDatafor1D = attributionDatafor1D.Distinct(customComparer).ToList();
                        }
                        if (attributionDatafor1D.Count == 0 || attributionDatafor1D == null)
                        {
                            return result;
                        }
                        entry = new PerformanceGraphData();
                        entry.PortfolioID = fundSelectionData.PortfolioId;
                        entry.BenchmarkID = benchmarkID;
                        Decimal? sumPerformanceWeight = 0;
                        Decimal? sumBenchmarkWeight = 0;
                        for (int i = 0; i <= attributionDatafor1D.Count - 1; i++)
                        {
                            sumPerformanceWeight = sumPerformanceWeight + attributionDatafor1D[i].ADJ_RTN_POR_RC_TWR_1D;
                            sumBenchmarkWeight = sumBenchmarkWeight + attributionDatafor1D[i].ADJ_BM1_RC_EXRTN_1D;
                        }
                        entry.BenchmarkPerformance = sumBenchmarkWeight;
                        entry.PortfolioPerformance = sumPerformanceWeight;
                        entry.EffectiveDate = effectiveDate;
                        result.Add(entry);
                        break;
                        #endregion
                    case "1W":
                        #region When Period selected is "1W"
                        List<DateTime> listOfEffectiveDates1W = new List<DateTime>();
                        for (int i = 0; i < 4; i++)
                        {
                            DateTime newDate = new DateTime();
                            newDate = effectiveDate.AddDays(-i);
                            listOfEffectiveDates1W.Add(newDate);
                        }
                        List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionDatafor1W = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                        FetchDataPerformanceGraph(country, attributionDatafor1W, ref result, benchmarkID, fundSelectionData, listOfEffectiveDates1W, "1W");
                        break;
                        #endregion
                    case "MTD":
                        #region When Period Selected is "MTD"
                        DateTime now = effectiveDate;
                        DateTime lastDayLastMonth = new DateTime(now.Year, now.Month, 1);
                        lastDayLastMonth = lastDayLastMonth.AddDays(-1);
                        List<DateTime> listOfEffectiveDatesMTD = new List<DateTime>();
                        listOfEffectiveDatesMTD.Add(lastDayLastMonth);
                        for (int i = 1; i <= effectiveDate.Day; i++)
                        {
                            DateTime newDate = new DateTime(effectiveDate.Year, effectiveDate.Month, i);
                            listOfEffectiveDatesMTD.Add(newDate);
                        }
                        List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionDataforMTD = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                        FetchDataPerformanceGraph(country, attributionDataforMTD, ref result, benchmarkID, fundSelectionData, listOfEffectiveDatesMTD, "MTD");
                        break;
                        #endregion
                    case "QTD":
                        #region When Period Selected is "QTD"
                        int tQtr = (effectiveDate.Month - 1) / 3;
                        int differenceInMonths = 0;
                        DateTime lastDayOfQuarter = new DateTime(effectiveDate.Year, (tQtr * 3) + 1, 1).AddDays(-1);
                        List<DateTime> listOfEffectiveDatesQTD = new List<DateTime>();
                        listOfEffectiveDatesQTD.Add(lastDayOfQuarter);
                        if (lastDayOfQuarter.Month == 12)
                        {
                            differenceInMonths = effectiveDate.Month - 0;
                        }
                        else
                        {
                            differenceInMonths = effectiveDate.Month - lastDayOfQuarter.Month;
                        }
                        switch (differenceInMonths)
                        {
                            case 1:
                                for (int i = 1; i <= effectiveDate.Day; i++)
                                {
                                    DateTime newDate = new DateTime(effectiveDate.Year, effectiveDate.Month, i);
                                    listOfEffectiveDatesQTD.Add(newDate);
                                }
                                break;
                            case 2:
                                int previousMonth = effectiveDate.Month - 1;
                                for (int i = 1; i <= DateTime.DaysInMonth(effectiveDate.Year, previousMonth); i++)
                                {
                                    DateTime newDate = new DateTime(effectiveDate.Year, previousMonth, i);
                                    listOfEffectiveDatesQTD.Add(newDate);
                                }
                                for (int i = 1; i <= effectiveDate.Day; i++)
                                {
                                    DateTime newDate = new DateTime(effectiveDate.Year, effectiveDate.Month, i);
                                    listOfEffectiveDatesQTD.Add(newDate);
                                }
                                break;
                            case 3:
                                int previous2Month = effectiveDate.Month - 2;
                                for (int i = 1; i <= DateTime.DaysInMonth(effectiveDate.Year, previous2Month); i++)
                                {
                                    DateTime newDate = new DateTime(effectiveDate.Year, previous2Month, i);
                                    listOfEffectiveDatesQTD.Add(newDate);
                                }
                                int previous1Month = effectiveDate.Month - 1;
                                for (int i = 1; i <= DateTime.DaysInMonth(effectiveDate.Year, previous1Month); i++)
                                {
                                    DateTime newDate = new DateTime(effectiveDate.Year, previous1Month, i);
                                    listOfEffectiveDatesQTD.Add(newDate);
                                }
                                for (int i = 1; i <= effectiveDate.Day; i++)
                                {
                                    DateTime newDate = new DateTime(effectiveDate.Year, effectiveDate.Month, i);
                                    listOfEffectiveDatesQTD.Add(newDate);
                                }
                                break;
                            default:
                                break;
                        }
                        List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionDataforQTD = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                        FetchDataPerformanceGraph(country, attributionDataforQTD, ref result, benchmarkID, fundSelectionData, listOfEffectiveDatesQTD, "QTD");
                        break;
                        #endregion

                    case "YTD":
                        #region When Period Selected is "YTD"
                        DateTime previousYearEndDate = new DateTime(effectiveDate.Year - 1, 12, 31);
                        List<DateTime> listOfEffectiveDatesYTD = new List<DateTime>();
                        listOfEffectiveDatesYTD.Add(previousYearEndDate);
                        int noOfMonths = effectiveDate.Month;
                        for (int i = 1; i < noOfMonths; i++)
                        {
                            DateTime newDate = new DateTime(effectiveDate.Year, i, DateTime.DaysInMonth(effectiveDate.Year, i));
                            listOfEffectiveDatesYTD.Add(newDate);
                        }
                        if (!listOfEffectiveDatesYTD.Contains(effectiveDate))
                        {
                            listOfEffectiveDatesYTD.Add(effectiveDate);
                        }
                        List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionDataforYTD = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                        FetchDataPerformanceGraphforMTDVaules(country, attributionDataforYTD, ref result, benchmarkID, fundSelectionData, listOfEffectiveDatesYTD, "YTD");
                        break;
                        #endregion

                    case "1Y":
                        #region When Period Selected is "1Y"
                        DateTime previousYearDate = effectiveDate.AddYears(-1);
                        int noOfMonth = previousYearDate.Month;
                        List<DateTime> startingStubDates = new List<DateTime>();
                        List<DateTime> listOfEffectiveDates1YMTD = new List<DateTime>();
                        List<DateTime> endingStubDates = new List<DateTime>();

                        // partial period returns for starting stub
                        DateTime nextMonthEnd = new DateTime(previousYearDate.Year, noOfMonth, DateTime.DaysInMonth(previousYearDate.Year, noOfMonth));
                        for (int i = previousYearDate.Day; i <= nextMonthEnd.Day; i++)
                        {
                            DateTime newDate = new DateTime(previousYearDate.Year, nextMonthEnd.Month, i);
                            startingStubDates.Add(newDate);
                        }
                        List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionDatafor1Y = new List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>();
                        List<Decimal?> portfolioReturns = new List<decimal?>();
                        List<Decimal?> benchmarkReturns = new List<decimal?>();
                        FetchDataPerformanceGraph1YValues(country, attributionDatafor1Y, ref result, benchmarkID, fundSelectionData, startingStubDates, portfolioReturns,
                            benchmarkReturns, "1Y");

                        //MTD for month ends between beginning stub and ending stub
                        for (int i = nextMonthEnd.Month + 1; i <= 12; i++)
                        {
                            DateTime newDate = new DateTime(previousYearDate.Year, i, DateTime.DaysInMonth(previousYearDate.Year, i));
                            listOfEffectiveDates1YMTD.Add(newDate);
                        }
                        for (int i = 1; i < effectiveDate.Month; i++)
                        {
                            DateTime newDate = new DateTime(effectiveDate.Year, i, DateTime.DaysInMonth(effectiveDate.Year, i));
                            listOfEffectiveDates1YMTD.Add(newDate);
                        }
                        FetchDataPerformanceGraphforMTDVaules(country, attributionDatafor1Y, ref result, benchmarkID, fundSelectionData, listOfEffectiveDates1YMTD, "1Y");

                        // partial period returns for ending stub

                        List<Decimal?> eportfolioReturns = new List<decimal?>();
                        List<Decimal?> ebenchmarkReturns = new List<decimal?>();
                        for (int i = 1; i <= effectiveDate.Day; i++)
                        {
                            DateTime newDate = new DateTime(effectiveDate.Year, effectiveDate.Month, i);
                            endingStubDates.Add(newDate);
                        }
                        FetchDataPerformanceGraph1YValues(country, attributionDatafor1Y, ref result, benchmarkID, fundSelectionData, endingStubDates, eportfolioReturns,
                            ebenchmarkReturns, "1Y");
                        break;
                    default:
                        List<PerformanceGraphData> resultForDefault = new List<PerformanceGraphData>();
                        break;
                        #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        #region Private Methods for Performance Graph
        /// <summary>
        /// Fetch data for MTD,QTD
        /// </summary>
        /// <param name="country"></param>
        /// <param name="attributionData"></param>
        /// <param name="result"></param>
        /// <param name="benchmarkID"></param>
        /// <param name="fundSelectionData"></param>
        /// <param name="listOfEffectiveDates"></param>
        /// <param name="period"></param>
        private void FetchDataPerformanceGraph(String country, List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionData, ref List<PerformanceGraphData> result,
            String benchmarkID, PortfolioSelectionData fundSelectionData, List<DateTime> listOfEffectiveDates, String period)
        {
            EqualityComparer<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> customComparer = new GreenField.Web.Services.PerformanceOperations.GF_PERF_DAILY_ATTRIBUTION_Comparer();

            foreach (DateTime d in listOfEffectiveDates)
            {
                Decimal? sumPerformanceWeight = 0;
                Decimal? sumBenchmarkWeight = 0;
                if (country == "NoFiltering")
                {
                    attributionData = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == fundSelectionData.PortfolioId && t.TO_DATE == d && t.NODE_NAME == "Country")
                        .ToList();
                    attributionData = attributionData.Distinct(customComparer).ToList();
                }
                else
                {
                    attributionData = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == fundSelectionData.PortfolioId && t.TO_DATE == d && t.AGG_LVL_1 == country)
                        .ToList();
                    attributionData = attributionData.Distinct(customComparer).ToList();
                }
                if (attributionData.Count == 0 || attributionData == null)
                    continue;
                #region Inception Check
                if (period != "1W" && period != "1D")
                {
                    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    dateInfo.ShortDatePattern = "dd/MM/yyyy";
                    DateTime portfolioInceptionDate = Convert.ToDateTime((attributionData.Select(t => t.POR_INCEPTION_DATE).FirstOrDefault()), dateInfo);
                    bool isValid = InceptionDateChecker.ValidateInceptionDate(period, portfolioInceptionDate, d);
                    if (!isValid)
                    {
                        continue;
                    }
                }
                #endregion
                PerformanceGraphData entry = new PerformanceGraphData();
                for (int i = 0; i <= attributionData.Count - 1; i++)
                {
                    sumPerformanceWeight = sumPerformanceWeight + attributionData[i].ADJ_RTN_POR_RC_TWR_1D;
                    sumBenchmarkWeight = sumBenchmarkWeight + attributionData[i].ADJ_BM1_RC_EXRTN_1D;
                }
                entry.PortfolioID = fundSelectionData.PortfolioId;
                entry.BenchmarkID = benchmarkID;
                entry.BenchmarkPerformance = sumBenchmarkWeight;
                entry.PortfolioPerformance = sumPerformanceWeight;
                entry.EffectiveDate = d;
                result.Add(entry);
            }
        }

        /// <summary>
        /// Fetch data for YTD
        /// </summary>
        /// <param name="country"></param>
        /// <param name="attributionData"></param>
        /// <param name="result"></param>
        /// <param name="benchmarkID"></param>
        /// <param name="fundSelectionData"></param>
        /// <param name="listOfEffectiveDates"></param>
        /// <param name="period"></param>
        private void FetchDataPerformanceGraphforMTDVaules(String country, List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionData, ref List<PerformanceGraphData> result,
            String benchmarkID, PortfolioSelectionData fundSelectionData, List<DateTime> listOfEffectiveDates, String period)
        {
            EqualityComparer<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> customComparer = new GreenField.Web.Services.PerformanceOperations.GF_PERF_DAILY_ATTRIBUTION_Comparer();
            foreach (DateTime d in listOfEffectiveDates)
            {
                Decimal? sumPerformanceWeight = 0;
                Decimal? sumBenchmarkWeight = 0;
                if (country == "NoFiltering")
                {
                    attributionData = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == fundSelectionData.PortfolioId && t.TO_DATE == d && t.NODE_NAME == "Country")
                        .ToList();
                    attributionData = attributionData.Distinct(customComparer).ToList();
                }
                else
                {
                    attributionData = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == fundSelectionData.PortfolioId && t.TO_DATE == d && t.AGG_LVL_1 == country)
                        .ToList();
                    attributionData = attributionData.Distinct(customComparer).ToList();
                }
                if (attributionData.Count == 0 || attributionData == null)
                {
                    continue;
                }
                #region Inception Check
                if (period != "1W" && period != "1D")
                {
                    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    dateInfo.ShortDatePattern = "dd/MM/yyyy";
                    DateTime portfolioInceptionDate = Convert.ToDateTime((attributionData.Select(t => t.POR_INCEPTION_DATE).FirstOrDefault()), dateInfo);
                    bool isValid = InceptionDateChecker.ValidateInceptionDate(period, portfolioInceptionDate, d);
                    if (!isValid)
                    {
                        continue;
                    }
                }
                #endregion
                PerformanceGraphData entry = new PerformanceGraphData();
                for (int i = 0; i <= attributionData.Count - 1; i++)
                {
                    sumPerformanceWeight = sumPerformanceWeight + attributionData[i].ADJ_RTN_POR_RC_TWR_MTD;
                    sumBenchmarkWeight = sumBenchmarkWeight + attributionData[i].ADJ_BM1_RC_EXRTN_MTD;
                }
                entry.PortfolioID = fundSelectionData.PortfolioId;
                entry.BenchmarkID = benchmarkID;
                entry.BenchmarkPerformance = sumBenchmarkWeight;
                entry.PortfolioPerformance = sumPerformanceWeight;
                entry.EffectiveDate = d;
                result.Add(entry);
            }
        }

        /// <summary>
        /// Fetch Data for 1Y values
        /// </summary>
        /// <param name="country"></param>
        /// <param name="attributionDatafor1Y"></param>
        /// <param name="result"></param>
        /// <param name="benchmarkID"></param>
        /// <param name="fundSelectionData"></param>
        /// <param name="StubDates"></param>
        /// <param name="eportfolioReturns"></param>
        /// <param name="ebenchmarkReturns"></param>
        /// <param name="period"></param>
        private void FetchDataPerformanceGraph1YValues(String country, List<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> attributionDatafor1Y, ref List<PerformanceGraphData> result,
            String benchmarkID, PortfolioSelectionData fundSelectionData, List<DateTime> StubDates, List<Decimal?> eportfolioReturns, List<Decimal?> ebenchmarkReturns, String period)
        {
            EqualityComparer<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION> customComparer = new GreenField.Web.Services.PerformanceOperations.GF_PERF_DAILY_ATTRIBUTION_Comparer();
            foreach (DateTime d in StubDates)
            {
                if (country == "NoFiltering")
                {
                    attributionDatafor1Y = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == fundSelectionData.PortfolioId && t.TO_DATE == d && t.NODE_NAME == "Country")
                        .ToList();
                    attributionDatafor1Y = attributionDatafor1Y.Distinct(customComparer).ToList();
                }
                else
                {
                    attributionDatafor1Y = DimensionEntity.GF_PERF_DAILY_ATTRIBUTION.Where(t => t.PORTFOLIO == fundSelectionData.PortfolioId && t.TO_DATE == d && t.AGG_LVL_1 == country)
                        .ToList();
                    attributionDatafor1Y = attributionDatafor1Y.Distinct(customComparer).ToList();
                }
                if (attributionDatafor1Y.Count == 0 || attributionDatafor1Y == null)
                {
                    continue;
                }
                #region Inception Check
                if (period != "1W" && period != "1D")
                {
                    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    dateInfo.ShortDatePattern = "dd/MM/yyyy";
                    DateTime portfolioInceptionDate = Convert.ToDateTime((attributionDatafor1Y.Select(t => t.POR_INCEPTION_DATE).FirstOrDefault()), dateInfo);
                    bool isValid = InceptionDateChecker.ValidateInceptionDate(period, portfolioInceptionDate, d);
                    if (!isValid)
                    {
                        continue;
                    }
                }
                #endregion
                Decimal? portfolioReturn = 0;
                for (int i = 0; i <= attributionDatafor1Y.Count - 1; i++)
                {
                    portfolioReturn = portfolioReturn + attributionDatafor1Y[i].ADJ_RTN_POR_RC_TWR_1D;
                }
                portfolioReturn = (portfolioReturn / 100) + 1;
                eportfolioReturns.Add(portfolioReturn);
                Decimal? mul = 1;
                foreach (Decimal? ret in eportfolioReturns)
                {
                    mul = ret * mul;
                }
                mul = (mul - 1) * 100;

                Decimal? benchmarkReturn = 0;
                for (int i = 0; i <= attributionDatafor1Y.Count - 1; i++)
                {
                    benchmarkReturn = benchmarkReturn + attributionDatafor1Y[i].ADJ_BM1_RC_EXRTN_1D;
                }
                benchmarkReturn = (benchmarkReturn / 100) + 1;
                ebenchmarkReturns.Add(benchmarkReturn);
                Decimal? mulBenchmark = 1;
                foreach (Decimal? ret in ebenchmarkReturns)
                {
                    mulBenchmark = ret * mulBenchmark;
                }
                mulBenchmark = (mulBenchmark - 1) * 100;
                PerformanceGraphData entry = new PerformanceGraphData();
                entry.PortfolioID = fundSelectionData.PortfolioId;
                entry.BenchmarkID = benchmarkID;
                entry.BenchmarkPerformance = mulBenchmark;
                entry.PortfolioPerformance = mul;
                entry.EffectiveDate = d;
                result.Add(entry);
            }
        }
        #endregion
        #endregion

        #region Comparator-GF_PERF_DAILY_ATTRIBUTION

        /// <summary>
        /// Comparator to remove duplicates from GF_PERF_DAILY_ATTRIBUTION
        /// </summary>
        public class GF_PERF_DAILY_ATTRIBUTION_Comparer : EqualityComparer<GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION>
        {
            public override bool Equals(GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION row1, GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION row2)
            {
                if ((row1 == null) && (row2 == null))
                    return true;
                if ((row2 == null) && (row1 != null))
                    return false;
                if ((row1 != null) && (row2 == null))
                    return false;
                if (((row1.PORTFOLIO == null || row2.PORTFOLIO == null)) && (row1.NODE_NAME == row2.NODE_NAME)
                    && (row1.TO_DATE == row2.TO_DATE) && (row2.AGG_LVL_1_LONG_NAME == row1.AGG_LVL_1_LONG_NAME))
                    return true;
                if (((row1.AGG_LVL_1_LONG_NAME == null || row2.AGG_LVL_1_LONG_NAME == null)) && (row1.PORTFOLIO == row2.PORTFOLIO)
                    && (row1.TO_DATE == row2.TO_DATE) && (row2.NODE_NAME == row1.NODE_NAME) && (row2.AGG_LVL_1 == row1.AGG_LVL_1))
                    return true;

                if (((row1.AGG_LVL_1_LONG_NAME == null || row2.AGG_LVL_1_LONG_NAME == null)) && (row1.PORTFOLIO == row2.PORTFOLIO)
                   && (row1.TO_DATE == row2.TO_DATE) && (row2.NODE_NAME == row1.NODE_NAME) && (row2.AGG_LVL_1 != row1.AGG_LVL_1))
                    return false;

                if (((row1.TO_DATE == null || row2.TO_DATE == null)) && (row1.PORTFOLIO == row2.PORTFOLIO)
                    && (row1.NODE_NAME == row2.NODE_NAME) && (row2.AGG_LVL_1_LONG_NAME == row1.AGG_LVL_1_LONG_NAME))
                    return true;

                if (((row1.NODE_NAME == null || row2.NODE_NAME == null)) && (row1.PORTFOLIO == row2.PORTFOLIO)
                    && (row1.TO_DATE == row2.TO_DATE) && (row2.AGG_LVL_1_LONG_NAME == row1.AGG_LVL_1_LONG_NAME))
                    return true;


                return (row1.AGG_LVL_1_LONG_NAME.ToUpper().Trim().Equals(row2.AGG_LVL_1_LONG_NAME.ToUpper().Trim()))
                    && (row1.TO_DATE.Equals(row2.TO_DATE))
                    && (row1.NODE_NAME.ToUpper().Trim().Equals(row2.NODE_NAME.ToUpper().Trim()) && (row1.PORTFOLIO.ToUpper().Trim() == row2.PORTFOLIO.ToUpper().Trim()));
            }

            public override int GetHashCode(GreenField.DAL.GF_PERF_DAILY_ATTRIBUTION data)
            {
                if (data.AGG_LVL_1_LONG_NAME == null || data.NODE_NAME == null || data.TO_DATE == null || data.PORTFOLIO == null)
                    return 0;
                int hCodeName = data.AGG_LVL_1_LONG_NAME.GetHashCode();
                int hCodeDate = data.TO_DATE.GetHashCode();
                int hCodeNode = data.NODE_NAME.GetHashCode();
                int hCodePortfolio = data.PORTFOLIO.GetHashCode();
                return hCodeDate ^ hCodeName ^ hCodeNode ^ hCodePortfolio;
            }
        }


        #endregion

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<DateTime> GetLastDayOfMonths()
        {
            // use cache if available
            var fromCache = (List<DateTime>)new DefaultCacheProvider().Get(CacheKeyNames.LastDayOfMonthsCache);
            if (fromCache != null)
                return fromCache;

            // otherwise fetch the data and cache it
            DimensionEntities entity = DimensionEntity;
    
           /* var q =
                (entity.GF_PERF_TOPLEVELMONTH.Select(g => new { g.TO_DATE })).ToList()
                    .Select(x => DateTime.ParseExact(x.TO_DATE, "dd-MM-yyyy", System.Globalization.CultureInfo.GetCultureInfo("en-us")))
                    .Distinct()
                    .OrderByDescending(x => x)
                    .Take(12);
            */
            var q =
                (entity.GF_PERF_DAILY_ATTRIBUTION.Where (a => a.NODE_NAME == "GICS Level 1" && a.PORTFOLIO=="EMIF").Select(g => new { g.TO_DATE }).ToList()
					.Select(x => x.TO_DATE.Value))
					.Distinct()
					.OrderByDescending(x => x);
					//.Take(13);

            new DefaultCacheProvider().Set(CacheKeyNames.LastDayOfMonthsCache, q.ToList(), Int32.Parse(ConfigurationManager.AppSettings["PerformanceCacheTime"]));
            return q.ToList();
        }
    }
}