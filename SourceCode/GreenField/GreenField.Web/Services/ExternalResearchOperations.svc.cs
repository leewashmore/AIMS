﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.Resources;
using GreenField.Web.Helpers.Service_Faults;
using GreenField.Web.Helpers;
using GreenField.DataContracts;
using GreenField.DAL;
using System.Data.Objects;
using GreenField.Web.DimensionEntitiesService;
using System.Configuration;
using GreenField.Web.DataContracts;
using System.Data;
using GreenField.DataContracts.DataContracts;

namespace GreenField.Web.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExternalResearchOperations
    {
        public ResourceManager ServiceFaultResourceManager
        {
            get
            {
                return new ResourceManager(typeof(FaultDescriptions));
            }
        }

        private Entities dimensionEntity;
        public Entities DimensionEntity
        {
            get
            {
                if (null == dimensionEntity)
                    dimensionEntity = new Entities(new Uri(ConfigurationManager.AppSettings["DimensionWebService"]));

                return dimensionEntity;
            }
        }

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public string RetrieveIssuerId(EntitySelectionData entitySelectionData)
        {
            try
            {
                string result = DimensionEntity.GF_PORTFOLIO_HOLDINGS.Where(record =>
                        record.ASEC_SEC_SHORT_NAME == entitySelectionData.InstrumentID &&
                        record.ISSUE_NAME == entitySelectionData.LongName &&
                        record.TICKER == entitySelectionData.ShortName)
                    .Select(record => record.ISSUER_ID).FirstOrDefault();

                return result == null ? String.Empty : result;
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
        public IssuerReferenceData RetrieveIssuerReferenceData(EntitySelectionData entitySelectionData)
        {
            try
            {
                ExternalResearchEntities entity = new ExternalResearchEntities();

                GF_SECURITY_BASEVIEW securityDetails = DimensionEntity.GF_SECURITY_BASEVIEW
                    .Where(record => record.ASEC_SEC_SHORT_NAME == entitySelectionData.InstrumentID &&
                        record.ISSUE_NAME == entitySelectionData.LongName &&
                        record.TICKER == entitySelectionData.ShortName).FirstOrDefault();

                if (securityDetails == null)
                    return new IssuerReferenceData();

                String issuerId = securityDetails.ISSUER_ID;
                String countryCode = securityDetails.ISO_COUNTRY_CODE;
                String countryName = securityDetails.ASEC_SEC_COUNTRY_NAME;
                String regionCode = securityDetails.ASHEMM_PROPRIETARY_REGION_CODE;
                String sectorCode = securityDetails.GICS_SECTOR;
                String sectorName = securityDetails.GICS_SECTOR_NAME;
                String industryCode = securityDetails.GICS_INDUSTRY;
                String industryName = securityDetails.GICS_SUB_INDUSTRY_NAME;
                int? securityID = securityDetails.SECURITY_ID;
                String currencyCode = null;
                String currencyName = null;

                External_Country_Master countryDetails = entity.External_Country_Master
                    .Where(record => record.COUNTRY_CODE == countryCode &&
                        record.COUNTRY_NAME == countryName)
                    .FirstOrDefault();

                if (countryDetails != null)
                {
                    currencyCode = countryDetails.CURRENCY_CODE;
                    currencyName = countryDetails.CURRENCY_NAME;
                }

                IssuerReferenceData result = new IssuerReferenceData()
                {
                    IssuerId = issuerId,
                    CountryCode = countryCode,
                    CountryName = countryName,
                    CurrencyCode = currencyCode,
                    CurrencyName = currencyName,
                    RegionCode = regionCode,
                    SectorCode = sectorCode,
                    SectorName = sectorName,
                    IndustryCode = industryCode,
                    IndustryName = industryName,
                    SecurityId = securityID
                };

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
        public List<FinancialStatementData> RetrieveFinancialStatement(string issuerID, FinancialStatementDataSource dataSource, FinancialStatementPeriodType periodType
            , FinancialStatementFiscalType fiscalType, FinancialStatementType statementType, String currency)
        {
            try
            {
                string _dataSource = EnumUtils.ToString(dataSource);
                string _periodType = EnumUtils.ToString(periodType).Substring(0, 1);
                string _fiscalType = EnumUtils.ToString(fiscalType);
                string _statementType = EnumUtils.ToString(statementType);

                ExternalResearchEntities entity = new ExternalResearchEntities();

                List<FinancialStatementData> result = null;

               result = entity.Get_Statement(issuerID, _dataSource, _periodType, _fiscalType, _statementType, currency).ToList();

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
        /// Get data for Consensus Estimate Detailed gadget
        /// </summary>
        /// <param name="issuerId"></param>
        /// <param name="periodType"></param>
        /// <param name="currency"></param>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<ConsensusEstimateDetail> RetrieveConsensusEstimateDetailedData(string issuerId, FinancialStatementPeriodType periodType, String currency)
        {
            try
            {
                string _periodType = EnumUtils.ToString(periodType).Substring(0, 1);

                ExternalResearchEntities entity = new ExternalResearchEntities();

                List<ConsensusEstimateDetailData> data = new List<ConsensusEstimateDetailData>();
                List<ConsensusEstimateDetail> result = new List<ConsensusEstimateDetail>();

                data = entity.GetConsensusDetail(issuerId, "REUTERS", _periodType, "FISCAL", currency).ToList();

                if (data == null)
                    return result;

                data = data.OrderBy(record => record.ESTIMATE_DESC).ThenByDescending(record => record.PERIOD_YEAR).ToList();
                List<string> dataDescriptors = data.Select(r => r.ESTIMATE_DESC).Distinct().ToList();

                List<BrokerDetail> brokerDetailsList = new List<BrokerDetail>();
                foreach (string item in dataDescriptors)
	            {
                    List<BrokerDetail> temp = new List<BrokerDetail>();
                    temp = entity.GetBrokerDetail(issuerId,item,_periodType,currency).ToList();
                    if(temp != null)
                    foreach (BrokerDetail value in temp)
	                {
                          brokerDetailsList.Add(value);		 
	                }		 
	            }

                for (int i = 0; i < data.Count; i++)
                {
                    //if (requiredDescriptors.Contains(data[i].ESTIMATE_DESC))
                    //{
                        ConsensusEstimateDetail temp = new ConsensusEstimateDetail();
                        temp.IssuerId = data[i].ISSUER_ID;
                        temp.EstimateId = data[i].ESTIMATE_ID;
                        temp.Description = data[i].ESTIMATE_DESC;
                        temp.Period = data[i].Period;
                        temp.AmountType = data[i].AMOUNT_TYPE;
                        temp.PeriodYear = data[i].PERIOD_YEAR;
                        temp.PeriodType = data[i].PERIOD_TYPE;
                        temp.Amount = data[i].AMOUNT;
                        temp.AshmoreEmmAmount = data[i].ASHMOREEMM_AMOUNT;
                        temp.NumberOfEstimates = data[i].NUMBER_OF_ESTIMATES;
                        temp.High = data[i].HIGH;
                        temp.Low = data[i].LOW;
                        temp.StandardDeviation = data[i].STANDARD_DEVIATION;
                        temp.SourceCurrency = data[i].SOURCE_CURRENCY;
                        temp.DataSource = data[i].DATA_SOURCE;
                        temp.DataSourceDate = data[i].DATA_SOURCE_DATE;
                        temp.Actual = data[i].ACTUAL;
                        temp.ConsensusMedian = data[i].AMOUNT;
                        temp.YOYGrowth = data[i].AMOUNT;
                        temp.Variance = data[i].AMOUNT == 0 ? null : ((data[i].ASHMOREEMM_AMOUNT / data[i].AMOUNT) - 1) * 100;
                        if (i != data.Count - 1)
                        {
                            if (data[i].ESTIMATE_DESC == data[i + 1].ESTIMATE_DESC &&
                                data[i].PERIOD_YEAR == data[i + 1].PERIOD_YEAR + 1)
                            {
                                if(data[i+1].AMOUNT != 0)
                                temp.YOYGrowth = (temp.YOYGrowth / data[i + 1].AMOUNT) - 1;
                            }
                        }

                        result.Add(temp);
                   // }                 
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

        //[OperationContract]
        //[FaultContract(typeof(ServiceFault))]
        //public List<FinstatData> RetrieveFinstatData(string issuerId,string securityId, FinancialStatementDataSource dataSource, String currency


        /// <summary>
        /// Gets Basic Data
        /// </summary>
        /// <param name="securityId"></param>
        /// <returns>Basic data</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<BasicData> RetrieveBasicData(EntitySelectionData entitySelectionData)
        {
            try
            {
                List<BasicData> result = new List<BasicData>();
                List<GetBasicData_Result> resultDB = new List<GetBasicData_Result>();
                ExternalResearchEntities extResearch = new ExternalResearchEntities();
                if (entitySelectionData == null)
                    return null;

                DimensionEntitiesService.Entities entity = DimensionEntity;

                bool isServiceUp;
                isServiceUp = CheckServiceAvailability.ServiceAvailability();

                if (!isServiceUp)
                    throw new Exception("Services are not available");

                //Retrieving data from security view
                DimensionEntitiesService.GF_SECURITY_BASEVIEW data = entity.GF_SECURITY_BASEVIEW
                    .Where(record => record.TICKER == entitySelectionData.ShortName
                        && record.ISSUE_NAME == entitySelectionData.LongName
                        && record.ASEC_SEC_SHORT_NAME == entitySelectionData.InstrumentID
                        && record.SECURITY_TYPE == entitySelectionData.SecurityType)
                    .FirstOrDefault();

                if (data == null)
                    return null;

                BasicData basicData = new BasicData();
                basicData.WeekRange52 = data.FIFTYTWO_WEEK_LOW - data.FIFTYTWO_WEEK_HIGH;
                basicData.AverageVolume = data.SECURITY_VOLUME_AVG_6M;
                basicData.SharesOutstanding = data.SHARES_OUTSTANDING;
                if (data.BARRA_BETA != null)
                {
                    basicData.Beta = data.BARRA_BETA;
                    basicData.BetaSource = "BARRA";
                }
                else
                {
                    decimal convertedString;
                    basicData.BetaSource = "BLOOMBERG";
                    //if (data.BETA != null && data.BETA != string.Empty)
                    //{
                    if (decimal.TryParse(data.BETA, out convertedString))
                        basicData.Beta = convertedString;

                    else
                    {
                        basicData.Beta = null;
                        LoggingOperations _logging = new LoggingOperations();
                        string userName = null;

                        if (System.Web.HttpContext.Current.Session["Session"] != null)
                        {
                            userName = (System.Web.HttpContext.Current.Session["Session"] as Session).UserName;
                        }
                        //Calling method to log error in text file
                        _logging.LogToFile("|User[(" + userName.Replace(Environment.NewLine, " ")
                         + ")]|Type[(Exception"
                         + ")]|Message[(" + "Conversion from string to decimal failed."
                         + ")]", "Exception", "Medium");
                    }
                }

                ////Retrieving data from Period Financials table
                resultDB = extResearch.ExecuteStoreQuery<GetBasicData_Result>("exec GetBasicData @SecurityID={0}", Convert.ToString(data.SECURITY_ID)).ToList();



                basicData.MarketCapitalization = resultDB[0].MARKET_CAPITALIZATION;
                basicData.EnterpriseValue = resultDB[0].ENTERPRISE_VALUE;
                result.Add(basicData);

                return result;
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

        #region ConsensusEstimatesGadgets

        /// <summary>
        /// Service Method for ConsensusEstimatesGadget-TargetPrice
        /// </summary>
        /// <returns>Collection of TargetPriceCEData</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<TargetPriceCEData> RetrieveTargetPriceData(EntitySelectionData entitySelectionData)
        {
            List<TargetPriceCEData> result = new List<TargetPriceCEData>();
            TargetPriceCEData data = new TargetPriceCEData();

            if (entitySelectionData == null)
                return new List<TargetPriceCEData>();
            DimensionEntitiesService.Entities dimensionEntity = DimensionEntity;

            List<GF_SECURITY_BASEVIEW> securityData = (dimensionEntity.GF_SECURITY_BASEVIEW.
                Where(a => a.ISSUE_NAME.ToUpper().Trim() == entitySelectionData.LongName.ToUpper().Trim()).ToList());
            if (securityData == null)
                return result;

            string XRef = securityData.Select(a => a.XREF).FirstOrDefault();

            if (XRef == null)
                return result;

            List<GetTargetPrice_Result> dbResult = new List<GetTargetPrice_Result>();
            ExternalResearchEntities entity = new ExternalResearchEntities();
            dbResult = entity.GetTargetPrice(XRef).ToList();

            if (dbResult == null)
                return result;
            if (dbResult.Count == 0)
                return result;

            foreach (GetTargetPrice_Result item in dbResult)
            {
                data = new TargetPriceCEData();
                data.Ticker = (item.Ticker == null) ? "N/A" : item.Ticker;
                data.ConsensusRecommendation = item.MeanLabel;
                data.CurrentPrice = ((item.CurrentPrice == null) ? "N/A" : item.CurrentPrice.ToString()).ToString() +
                    "( " + ((item.Currency == null) ? "N/A" : (item.Currency.ToString())).ToString() + " )";
                data.MedianTargetPrice = ((item.Median == null) ? "N/A" : item.Median.ToString()) +
                    " ( " + ((item.TargetCurrency == null) ? "N/A" : item.TargetCurrency.ToString()) + " )";
                data.LastUpdate = Convert.ToDateTime(item.StartDate);
                data.NoOfEstimates = (item.NumOfEsts == null) ? "N/A" : (Convert.ToString(item.NumOfEsts));
                data.High = (item.High == null) ? "N/A" : (Convert.ToString(item.High));
                data.Low = (item.Low == null) ? "N/A" : (Convert.ToString(item.Low));
                data.StandardDeviation = (item.StdDev == null) ? "N/A" : (Convert.ToString(item.StdDev));
                result.Add(data);
            }

            return result;
        }

        /// <summary>
        /// Service Method for ConsensusEstimateGadget - Median
        /// </summary>
        /// <param name="issuerId">Issuer ID</param>
        /// <param name="periodType">Period Type: A/Q</param>
        /// <param name="currency">Selected Currency</param>
        /// <returns>Collection of ConsensusEstimateMedianData</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<ConsensusEstimateMedian> RetrieveConsensusEstimatesMedianData(string issuerId, FinancialStatementPeriodType periodType, string currency)
        {
            List<ConsensusEstimateMedian> result = new List<ConsensusEstimateMedian>();
            List<ConsensusEstimateMedianData> data = new List<ConsensusEstimateMedianData>();
            try
            {
                string _periodType = EnumUtils.ToString(periodType).Substring(0, 1);

                ExternalResearchEntities entity = new ExternalResearchEntities();

                data = entity.GetConsensusEstimateData(issuerId, "REUTERS", _periodType, "FISCAL", currency).ToList();
                List<int> dataDesc = new List<int>() { 17, 7, 11, 13, 12, 8, 9, 5, 18, 19 };
                data = data.OrderBy(record => record.ESTIMATE_DESC).ThenByDescending(record => record.PERIOD_YEAR).ToList();

                for (int i = 0; i < data.Count; i++)
                {

                    if (dataDesc.Contains(data[i].ESTIMATE_ID))
                    {
                        ConsensusEstimateMedian temp = new ConsensusEstimateMedian();
                        temp.IssuerId = data[i].ISSUER_ID;
                        temp.EstimateId = data[i].ESTIMATE_ID;
                        temp.Description = data[i].ESTIMATE_DESC;
                        temp.Period = data[i].Period;
                        temp.AmountType = data[i].AMOUNT_TYPE;
                        temp.PeriodYear = data[i].PERIOD_YEAR;
                        temp.PeriodType = data[i].PERIOD_TYPE;
                        temp.Amount = data[i].AMOUNT;
                        temp.AshmoreEmmAmount = data[i].ASHMOREEMM_AMOUNT;
                        temp.NumberOfEstimates = data[i].NUMBER_OF_ESTIMATES;
                        temp.High = data[i].HIGH;
                        temp.Low = data[i].LOW;
                        temp.StandardDeviation = data[i].STANDARD_DEVIATION;
                        temp.SourceCurrency = data[i].SOURCE_CURRENCY;
                        temp.DataSource = data[i].DATA_SOURCE;
                        temp.DataSourceDate = data[i].DATA_SOURCE_DATE;
                        temp.Actual = data[i].ACTUAL;

                        temp.YOYGrowth = data[i].AMOUNT;
                        temp.Variance = data[i].AMOUNT == 0 ? null : ((data[i].ASHMOREEMM_AMOUNT / data[i].AMOUNT) - 1) * 100;
                        if (i != data.Count - 1)
                        {
                            if (data[i].ESTIMATE_DESC == data[i + 1].ESTIMATE_DESC &&
                                data[i].PERIOD_YEAR == data[i + 1].PERIOD_YEAR + 1)
                            {
                                if (data[i + 1].AMOUNT != 0)
                                {
                                    temp.YOYGrowth = ((temp.YOYGrowth / data[i + 1].AMOUNT) - 1) * 100;
                                }
                            }
                            else
                            {
                                temp.YOYGrowth = 0;
                            }
                        }

                        result.Add(temp);
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
        /// Service Method for ConsensusEstimateGadget- Valuations
        /// </summary>
        /// <param name="issuerId">Issuer Id for a Security</param>
        /// <param name="periodType">Period Type: A/Q</param>
        /// <param name="currency">Selected Currency</param>
        /// <returns>Collection of ConsensusEstimatesValuations Data</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<ConsensusEstimatesValuations> RetrieveConsensusEstimatesValuationData(string issuerId, FinancialStatementPeriodType periodType, string currency)
        {
            List<ConsensusEstimatesValuations> result = new List<ConsensusEstimatesValuations>();
            List<ConsensusEstimateValuation> data = new List<ConsensusEstimateValuation>();
            try
            {
                string _periodType = EnumUtils.ToString(periodType).Substring(0, 1);

                ExternalResearchEntities entity = new ExternalResearchEntities();

                data = entity.GetConsensusEstimatesValuation(issuerId, "REUTERS", _periodType, "FISCAL", currency, null, null).ToList();
                List<int> dataDesc = new List<int>() { 166, 170, 171, 164, 192, 172 };
                data = data.OrderBy(record => record.ESTIMATE_DESC).ThenByDescending(record => record.PERIOD_YEAR).ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (dataDesc.Contains(data[i].ESTIMATE_ID))
                    {
                        
                        ConsensusEstimatesValuations temp = new ConsensusEstimatesValuations();
                        temp.IssuerId = data[i].ISSUER_ID;
                        temp.EstimateId = data[i].ESTIMATE_ID;
                        temp.Description = data[i].ESTIMATE_DESC;
                        temp.Period = data[i].Period;
                        temp.AmountType = data[i].AMOUNT_TYPE;
                        temp.PeriodYear = data[i].PERIOD_YEAR;
                        temp.PeriodType = data[i].PERIOD_TYPE;
                        temp.Amount = data[i].AMOUNT;
                        temp.AshmoreEmmAmount = data[i].ASHMOREEMM_AMOUNT;
                        temp.NumberOfEstimates = data[i].NUMBER_OF_ESTIMATES;
                        temp.High = data[i].HIGH;
                        temp.Low = data[i].LOW;
                        temp.StandardDeviation = data[i].STANDARD_DEVIATION;
                        temp.SourceCurrency = data[i].SOURCE_CURRENCY;
                        temp.DataSource = data[i].DATA_SOURCE;
                        temp.DataSourceDate = data[i].DATA_SOURCE_DATE;
                        temp.Actual = data[i].AMOUNT;

                        temp.YOYGrowth = data[i].AMOUNT;
                        temp.Variance = data[i].AMOUNT == 0 ? null : ((data[i].ASHMOREEMM_AMOUNT / data[i].AMOUNT) - 1) * 100;
                        if (i != data.Count - 1)
                        {
                            if (data[i].ESTIMATE_DESC == data[i + 1].ESTIMATE_DESC &&
                                data[i].PERIOD_YEAR == data[i + 1].PERIOD_YEAR + 1)
                            {
                                if (data[i + 1].AMOUNT != 0)
                                {
                                    temp.YOYGrowth = ((temp.YOYGrowth / data[i + 1].AMOUNT) - 1) * 100;
                                }
                                else
                                {
                                    temp.YOYGrowth = 0;
                                }
                            }
                        }

                        result.Add(temp);
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


        #endregion

        /// <summary>
        /// Retrieve data for consensus Estimates Summary Gadget
        /// </summary>
        /// <param name="entityIdentifier">Security identifier selected by the user</param>
        /// <returns>Returns data in the list of type ConsensusEstimatesSummaryData</returns>
        /// 
        #region Consensus Estimates Summary Gadget
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<GreenField.DataContracts.DataContracts.ConsensusEstimatesSummaryData> RetrieveConsensusEstimatesSummaryData(EntitySelectionData entityIdentifier)
        {
            try
            {
                List<GreenField.DataContracts.DataContracts.ConsensusEstimatesSummaryData> result = new List<GreenField.DataContracts.DataContracts.ConsensusEstimatesSummaryData>();
                DimensionEntitiesService.Entities entity = DimensionEntity;
                ExternalResearchEntities research = new ExternalResearchEntities();
                result = research.ExecuteStoreQuery<GreenField.DataContracts.DataContracts.ConsensusEstimatesSummaryData>("exec GetConsensusEstimatesSummaryData @Security={0}", entityIdentifier.LongName).ToList();
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

        #region Quarterly Comparision Results
        /// <summary>
        /// Retrieves Data for Quarterly Comparison
        /// </summary>
        /// <param name="fieldValue">field as selected by the user</param>
        /// <param name="yearValue">year as selected by the user</param>
        /// <returns>Returns data in list of type QuarterlyResultsData </returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<QuarterlyResultsData> RetrieveQuarterlyResultsData(String fieldValue, int yearValue)
        {
            try
            {
                int dataID;
                List<QuarterlyResultsData> result = new List<QuarterlyResultsData>();
                DimensionEntitiesService.Entities entity = DimensionEntity;
                ExternalResearchEntities research = new ExternalResearchEntities();
                if (fieldValue == "Revenue")
                    dataID = 11;
                else
                    dataID = 44;
                result = research.ExecuteStoreQuery<QuarterlyResultsData>("exec usp_GetQuarterlyResults @DataId={0}, @PeriodYear = {1}", dataID, yearValue).ToList();

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

        #region Historical Valuation Multiples Gadget
        /// <summary>
        /// Gets P/Revenue Data
        /// </summary>
        /// <param name="entitySelectionData"></param>
        /// <returns>P/Revenue Data</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<PRevenueData> RetrievePRevenueData(EntitySelectionData entitySelectionData, string chartTitle)
        {
            try
            {
                List<PRevenueData> result = new List<PRevenueData>();
                List<GetPRevenueData_Result> resultDB = new List<GetPRevenueData_Result>();
                List<GetEV_EBITDAData_Result> resultDB_EV_EBITDA = new List<GetEV_EBITDAData_Result>();
                ExternalResearchEntities extResearch = new ExternalResearchEntities();

                if (entitySelectionData == null)
                    return null;

                DimensionEntitiesService.Entities entity = DimensionEntity;

                bool isServiceUp;
                isServiceUp = CheckServiceAvailability.ServiceAvailability();

                if (!isServiceUp)
                    throw new Exception("Services are not available");

                //Retrieving data from security view
                DimensionEntitiesService.GF_SECURITY_BASEVIEW svcData = entity.GF_SECURITY_BASEVIEW
                    .Where(record => record.TICKER == entitySelectionData.ShortName
                        && record.ISSUE_NAME == entitySelectionData.LongName
                        && record.ASEC_SEC_SHORT_NAME == entitySelectionData.InstrumentID
                        && record.SECURITY_TYPE == entitySelectionData.SecurityType)
                    .FirstOrDefault();

                if (svcData == null)
                    return null;
                //execute store proc giving securityId as an input parameter
                int? securityId = svcData.SECURITY_ID;

                if (chartTitle == "EV/EBITDA")
                    resultDB_EV_EBITDA = extResearch.ExecuteStoreQuery<GetEV_EBITDAData_Result>("exec Get_EV_EBITDA @SecurityID={0},@issuerId={1},@chartTitle={2}", "157240", "8233223", chartTitle).ToList();//, Convert.ToString(data.SECURITY_ID)).ToList();
                else
                    ////Retrieving data from Period Financials table
                    resultDB = extResearch.ExecuteStoreQuery<GetPRevenueData_Result>("exec Get_PRevenue @SecurityID={0},@issuerId={1},@chartTitle={2}", "157240", "8233223", chartTitle).ToList();//, Convert.ToString(data.SECURITY_ID)).ToList();

                #region Dummy Data
                //TODO SEEMA:DELETE DUMMY DATA START
                //GetPRevenueData_Result dummyData = null;
                //resultDB = null;
                //resultDB = new List<GetPRevenueData_Result>();

                //dummyData = new GetPRevenueData_Result();
                //dummyData.PeriodLabel = "Q1 2011";
                //dummyData.Amount = 250;
                //dummyData.USDPrice = 20;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result();
                //dummyData.PeriodLabel = "Q2 2011";
                //dummyData.Amount = 300;
                //dummyData.USDPrice = 25;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q3 2011";
                //dummyData.Amount = 360;
                //dummyData.USDPrice = 30;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q4 2011";
                //dummyData.Amount = 450;
                //dummyData.USDPrice = 28;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q2 2012";
                //dummyData.Amount = 225;
                //dummyData.USDPrice = 21;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q3 2012";
                //dummyData.Amount = 200;
                //dummyData.USDPrice = 18;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);


                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q4 2012";
                //dummyData.Amount = 200;
                //dummyData.USDPrice = 17;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q1 2013";
                //dummyData.Amount = 200;
                //dummyData.USDPrice = 22;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q2 2013";
                //dummyData.Amount = null;
                //dummyData.USDPrice = 22;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q3 2013";
                //dummyData.Amount = 240;
                //dummyData.USDPrice = 22;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q4 2013";
                //dummyData.Amount = 310;
                //dummyData.USDPrice = 22;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); 
                //dummyData.PeriodLabel = "Q1 2014";
                //dummyData.Amount = 330;
                //dummyData.USDPrice = 22;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result(); dummyData.PeriodLabel = "Q2 2014";
                //dummyData.Amount = 400;
                //dummyData.USDPrice = 22;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                //dummyData = new GetPRevenueData_Result();
                //dummyData.PeriodLabel = "Q3 2014";
                //dummyData.Amount = 425;
                //dummyData.USDPrice = 22;
                //dummyData.Shares_Outstanding = 100;
                //resultDB.Add(dummyData);

                ////DUMMY DATA END

                #endregion

                for (int _index = 0; _index < resultDB.Count; _index++)
                {
                    PRevenueData data = new PRevenueData();
                    decimal? sumAmount = null;
                    decimal? sumNetDebt = null;
                    decimal? sumEBITDA = null;
                    data.PeriodLabel = resultDB[_index].PeriodLabel;

                    if ((resultDB[_index].USDPrice == null || resultDB[_index].USDPrice == 0) || (resultDB[_index].Shares_Outstanding == null || resultDB[_index].Shares_Outstanding == 0))
                    {
                        data.PRevenueVal = null;

                    }
                    else
                    {
                        if (chartTitle == "EV/EBITDA")
                        {
                            //TODO SEEMA Delete below line
                            //return null;
                            if (_index + 1 < resultDB_EV_EBITDA.Count && _index + 2 < resultDB_EV_EBITDA.Count && _index + 3 < resultDB_EV_EBITDA.Count)
                            {
                                if ((resultDB_EV_EBITDA[_index].NetDebt != null && resultDB_EV_EBITDA[_index + 1].NetDebt != null && resultDB_EV_EBITDA[_index + 2].NetDebt != null && resultDB_EV_EBITDA[_index + 3].NetDebt != null)
                                    || (resultDB_EV_EBITDA[_index].EBITDA != null && resultDB_EV_EBITDA[_index + 1].EBITDA != null && resultDB_EV_EBITDA[_index + 2].EBITDA != null && resultDB_EV_EBITDA[_index + 3].EBITDA != null))
                                {
                                    sumNetDebt = resultDB_EV_EBITDA[_index].NetDebt + resultDB_EV_EBITDA[_index + 1].NetDebt + resultDB_EV_EBITDA[_index + 2].NetDebt + resultDB_EV_EBITDA[_index + 3].NetDebt;
                                    sumEBITDA = resultDB_EV_EBITDA[_index].EBITDA + resultDB_EV_EBITDA[_index + 1].EBITDA + resultDB_EV_EBITDA[_index + 2].EBITDA + resultDB_EV_EBITDA[_index + 3].EBITDA;
                                }
                                else
                                {
                                    sumNetDebt = null;
                                    sumEBITDA = null;
                                }
                                if ((sumNetDebt == null || sumNetDebt == 0) || (sumEBITDA == null || sumEBITDA == 0))
                                {
                                    data.PRevenueVal = null;
                                }
                                else
                                {
                                    decimal? EV = null;
                                    decimal? EBITDA = null;
                                    EV = (resultDB_EV_EBITDA[_index].USDPrice * resultDB_EV_EBITDA[_index].Shares_Outstanding) / sumNetDebt;
                                    EBITDA = sumEBITDA;
                                    data.PRevenueVal = EV / EBITDA;
                                }
                            }
                        }
                        else
                        {
                            //Sum of Amount if 4 quarters exist
                            if (_index + 1 < resultDB.Count && _index + 2 < resultDB.Count && _index + 3 < resultDB.Count)
                            {
                                if (resultDB[_index].Amount != null && resultDB[_index + 1].Amount != null && resultDB[_index + 2].Amount != null && resultDB[_index + 3].Amount != null)
                                    sumAmount = resultDB[_index].Amount + resultDB[_index + 1].Amount + resultDB[_index + 2].Amount + resultDB[_index + 3].Amount;
                            }
                            else
                            {
                                sumAmount = null;
                            }

                            if (sumAmount == null || sumAmount == 0)
                                data.PRevenueVal = null;
                            else
                            {
                                if (chartTitle == "FCF Yield" || chartTitle == "Dividend Yield")
                                    data.PRevenueVal = sumAmount / (resultDB[_index].USDPrice * resultDB[_index].Shares_Outstanding);
                                else
                                    data.PRevenueVal = (resultDB[_index].USDPrice * resultDB[_index].Shares_Outstanding) / sumAmount;
                            }
                        }
                    }
                    result.Add(data);
                }

                result = HistoricalValuationCalculations.CalculateAvg(result);
                result = HistoricalValuationCalculations.CalculateStdDev(result);

                for (int _index = result.Count; _index > 0; _index--)
                {
                    if (result[_index - 1].PRevenueVal == null)
                    {
                        result[_index - 1].Average = null;
                        result[_index - 1].StdDevMinus = null;
                        result[_index - 1].StdDevPlus = null;
                    }
                    else
                        break;
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

        #region Scatter Graph Gadget
        /// <summary>
        /// Gets Ratio Comparison Data
        /// </summary>
        /// <param name="contextSecurityXML">xml script for security list for a particular context</param>
        /// <returns>RatioComparisonData</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<RatioComparisonData> RetrieveRatioComparisonData(String contextSecurityXML)
        {
            try
            {
                ExternalResearchEntities entity = new ExternalResearchEntities();
                List<RatioComparisonData> result = entity.usp_RetrieveRatioComparisonData(contextSecurityXML).ToList();
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
        public List<GF_SECURITY_BASEVIEW> RetrieveRatioSecurityReferenceData(ScatterGraphContext context, IssuerReferenceData issuerDetails)
        {
            try
            {
                switch (context)
                {
                    case ScatterGraphContext.REGION:
                        return DimensionEntity.GF_SECURITY_BASEVIEW.Where(record => record.ASHEMM_PROPRIETARY_REGION_CODE == issuerDetails.RegionCode).ToList();
                    case ScatterGraphContext.COUNTRY:
                        return DimensionEntity.GF_SECURITY_BASEVIEW.Where(record => record.ISO_COUNTRY_CODE == issuerDetails.CountryCode).ToList();
                    case ScatterGraphContext.SECTOR:
                        return DimensionEntity.GF_SECURITY_BASEVIEW.Where(record => record.GICS_SECTOR == issuerDetails.SectorCode).ToList();
                    case ScatterGraphContext.INDUSTRY:
                        return DimensionEntity.GF_SECURITY_BASEVIEW.Where(record => record.GICS_INDUSTRY == issuerDetails.IndustryCode).ToList();
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }
        #endregion

        #region Gadget With Period Columns
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<GreenField.DataContracts.COASpecificData> RetrieveCOASpecificData(String issuerId, int? securityId, FinancialStatementDataSource cSource, FinancialStatementFiscalType cFiscalType, String cCurrency)
        {
            try
            {
                string _dataSource = EnumUtils.ToString(cSource);
                string _fiscalType = EnumUtils.ToString(cFiscalType);                
                List<GreenField.DAL.COASpecificData> result = new List<GreenField.DAL.COASpecificData>();
                 List<GreenField.DataContracts.COASpecificData> mainResult = new List<GreenField.DataContracts.COASpecificData>();
                
                DimensionEntitiesService.Entities entity = DimensionEntity;
                ExternalResearchEntities research = new ExternalResearchEntities();
                result = research.GetDataForPeriodGadgets(_dataSource, _fiscalType, cCurrency, issuerId, securityId.ToString()).ToList();
                foreach (GreenField.DAL.COASpecificData item in result)
                {
                    GreenField.DataContracts.COASpecificData  entry = new GreenField.DataContracts.COASpecificData();
                    entry.Amount = item.Amount;
                    entry.AmountType = item.AmountType;
                    entry.DataSource = item.DataSource;
                    entry.Decimals = item.Decimals;
                    entry.Description = item.Description;
                    entry.GridDesc = item.GridDesc;
                    entry.GridId = item.GridId;
                    entry.IsPercentage = item.IsPercentage;
                    entry.PeriodType = item.Period_Type;
                    entry.PeriodYear = item.PeriodYear;
                    entry.RootSource = item.RootSource;
                    entry.ShowGrid = item.ShowGrid;
                    entry.SortOrder = item.SortOrder;
                    mainResult.Add(entry);
                }
                return mainResult;

            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }
        #endregion


        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<DocumentCategoricalData> RetrieveDocumentsData(String searchString)
        {
            try
            {
                List<DocumentCategoricalData> result = new List<DocumentCategoricalData>();
                result.Add(new DocumentCategoricalData()
                {
                    DocumentCategoryType = DocumentCategoryType.COMPANY_MEETING_NOTES,
                    DocumentCompanyName = "Company1",
                    DocumentCompanyTicker = "CompanyTicker1",
                    DocumentCatalogData = new DocumentCatalogData()
                    {
                        FileId = 1,
                        FileMetaTags = "Finance, specific catalog",
                        FileName = "Financial Statement 27-07-2012.docx",
                        FilePath = @"http://sharepointLocalSite/Documents/Financial Statement 27-07-2012.docx",
                        FileUploadedBy = "Rahul Vig",
                        FileUploadedOn = DateTime.Now.AddDays(-5)
                    },
                    CommentDetails = new List<CommentDetails>
                    {
                        new CommentDetails() { Comment = "Comment1", CommentBy = "Neeraj Jindal", CommentOn = DateTime.Now.AddDays(-1) },
                        new CommentDetails() { Comment = "Comment2", CommentBy = "Neeraj Jindal", CommentOn = DateTime.Now.AddDays(-2) },
                        new CommentDetails() { Comment = "Comment3", CommentBy = "Neeraj Jindal", CommentOn = DateTime.Now.AddDays(-3) }
                    }
                });
                result.Add(new DocumentCategoricalData()
                {
                    DocumentCategoryType = DocumentCategoryType.COMPANY_MEETING_NOTES,
                    DocumentCompanyName = "Company2",
                    DocumentCompanyTicker = "CompanyTicker2",
                    DocumentCatalogData = new DocumentCatalogData()
                    {
                        FileId = 1,
                        FileMetaTags = "Finance, specific catalog 2",
                        FileName = "Financial Statement 30-07-2012.docx",
                        FilePath = @"http://sharepointLocalSite/Documents/Financial Statement 27-07-2012.docx",
                        FileUploadedBy = "Rahul Vig",
                        FileUploadedOn = DateTime.Now.AddDays(-2)
                    },
                    CommentDetails = new List<CommentDetails>
                    {
                        new CommentDetails() { Comment = "Comment1", CommentBy = "Neeraj Jindal", CommentOn = DateTime.Now.AddDays(-1) },
                        new CommentDetails() { Comment = "Comment2", CommentBy = "Neeraj Jindal", CommentOn = DateTime.Now.AddDays(-1) },
                        new CommentDetails() { Comment = "Comment3", CommentBy = "Neeraj Jindal", CommentOn = DateTime.Now.AddDays(-2) }
                    }
                });

                result.Add(new DocumentCategoricalData()
                {
                    DocumentCategoryType = DocumentCategoryType.BLOG,
                    DocumentCompanyName = "Company1",
                    DocumentCompanyTicker = "CompanyTicker1",
                    DocumentCatalogData = null,
                    CommentDetails = new List<CommentDetails>
                    {
                        new CommentDetails() { Comment = "Comment1", CommentBy = "Abhinav Singh", CommentOn = DateTime.Now.AddDays(-1) },
                        new CommentDetails() { Comment = "Comment2", CommentBy = "Abhinav Singh", CommentOn = DateTime.Now.AddDays(-22) },
                        new CommentDetails() { Comment = "Comment3", CommentBy = "Abhinav Singh", CommentOn = DateTime.Now.AddDays(-31) }
                    }
                });


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
        public List<tblCompanyInfo> RetrieveCompanyData()
        {
            try
            {                
                ReutersEntities entity = new ReutersEntities();
                return entity.tblCompanyInfoes.ToList();
            }
            catch (Exception ex)
            {
                ExceptionTrace.LogException(ex);
                string networkFaultMessage = ServiceFaultResourceManager.GetString("NetworkFault").ToString();
                throw new FaultException<ServiceFault>(new ServiceFault(networkFaultMessage), new FaultReason(ex.Message));
            }
        }

    }
}
