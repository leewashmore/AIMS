﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using GreenField.Web.DimensionEntitiesService;
using System.Configuration;
using System.Resources;
using GreenField.Web.Helpers.Service_Faults;
using GreenField.Web.DataContracts;
using GreenField.Web.Helpers;
using System.Data;
using System.Data.SqlClient;
using GreenField.DAL;
using System.Collections;
using System.Data.Common;

namespace GreenField.Web.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModelFXOperations" in code, svc and config file together.
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ModelFXOperations 
    {
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

        public ResourceManager ServiceFaultResourceManager
        {
            get
            {
                return new ResourceManager(typeof(FaultDescriptions));
            }
        }

        /// <summary>
        /// Retrives data for Macro database key annual report
        /// </summary>
        /// <param name="CountryName"></param>
        /// <param name="regionName"></param>
        /// <returns>report data</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<MacroDatabaseKeyAnnualReportData> RetrieveMacroDatabaseKeyAnnualReportData(String countryNameVal)
        {
            try
            {
                //bool isServiceUp;
                //isServiceUp = CheckServiceAvailability.ServiceAvailability();

                //if (!isServiceUp)
                //    throw new Exception();

                List<MacroDatabaseKeyAnnualReportData> result = new List<MacroDatabaseKeyAnnualReportData>();
                //MacroDatabaseKeyAnnualReportData entry = new MacroDatabaseKeyAnnualReportData();
                DimensionEntitiesService.Entities entity = DimensionEntity;
                ResearchEntities research = new ResearchEntities();
                //IList macroDatalist =  research.RetrieveCTYSUMMARYDataReport("AR").ToList();
                result = research.ExecuteStoreQuery<MacroDatabaseKeyAnnualReportData>("exec RetrieveCTYSUMMARYDataReportPerCountry @country={0}", "AR").ToList();
                //foreach (var i in myList)
                //{
                //    MacroDatabaseKeyAnnualReportData entry = new MacroDatabaseKeyAnnualReportData();
                //    entry.CATEGORY_NAME = i.CATEGORY_NAME;
                //    entry.CountryName = i.CountryName;
                //    entry.Description = i.Description;
                //    entry.DisplayType = i.DisplayType;
                //    entry.FiveYEAR_Avg = i.FiveYEAR_Avg;
                //    entry.YEAR_1987 = i.YEAR_1987;
                //    entry.YEAR_1988 = i.YEAR_1988;
                //    entry.YEAR_1989 = i.YEAR_1989;
                //    entry.YEAR_1990 = i.YEAR_1990;
                //    entry.YEAR_1991 = i.YEAR_1991;
                //    entry.YEAR_1992 = i.YEAR_1992;
                //    entry.YEAR_1993 = i.YEAR_1993;
                //    entry.YEAR_1994 = i.YEAR_1994;
                //    entry.YEAR_1995 = i.YEAR_1995;
                //    entry.YEAR_1996 = i.YEAR_1996;
                //    entry.YEAR_1997 = i.YEAR_1997;
                //    entry.YEAR_1998 = i.YEAR_1998;
                //    entry.YEAR_1999 = i.YEAR_1999;
                //    entry.YEAR_2000 = i.YEAR_2000;
                //    entry.YEAR_2001 = i.YEAR_2001;
                //    entry.YEAR_2002 = i.YEAR_2002;
                //    entry.YEAR_2003 = i.YEAR_2003;
                //    entry.YEAR_2004 = i.YEAR_2004;
                //    entry.YEAR_2005 = i.YEAR_2005;
                //    entry.YEAR_2006 = i.YEAR_2006;
                //    entry.YEAR_2007 = i.YEAR_2007;
                //    entry.YEAR_2008 = i.YEAR_2008;
                //    entry.YEAR_2009 = i.YEAR_2009;
                //    entry.YEAR_2010 = i.YEAR_2010;
                //    entry.YEAR_2011 = i.YEAR_2011;
                //    entry.YEAR_2012 = i.YEAR_2012;
                //    entry.YEAR_2013 = i.YEAR_2013;
                //    entry.YEAR_2014 = i.YEAR_2014;
                //    entry.YEAR_2015 = i.YEAR_2015;
                //    entry.YEAR_2016 = i.YEAR_2016;
                //    entry.YEAR_2017 = i.YEAR_2017;
                //    entry.YEAR_2018 = i.YEAR_2018;
                //    entry.YEAR_2019 = i.YEAR_2019;
                //    entry.YEAR_2020 = i.YEAR_2020;
                //    entry.YEAR_2021 = i.YEAR_2021;
                //    entry.YEAR_2022 = i.YEAR_2022;
                //    entry.YEAR_2023 = i.YEAR_2023;
                //    entry.YEAR_2024 = i.YEAR_2024;
                //    entry.YEAR_2025 = i.YEAR_2025;                  
                //    result.Add(entry);
                //}
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
        public List<CountrySelectionData> RetrieveCountrySelectionData()
        {
            List<CountrySelectionData> result = new List<CountrySelectionData>();
            ResearchEntities research = new ResearchEntities();
            List<Country_Master> countryData = new List<Country_Master>();
            countryData = research.Country_Master.ToList();
            for (int i = 0; i < countryData.Count; i++)
            {
                CountrySelectionData entry = new CountrySelectionData();
                entry.CountryCode = countryData[i].COUNTRY_CODE;
                entry.CountryName = countryData[i].COUNTRY_NAME;
                result.Add(entry);
            }
            return result;
        }

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<MacroDatabaseKeyAnnualReportData> RetrieveMacroDatabaseKeyAnnualReportDataEMSummary(String countryNameVal)
        {
            try
            {
                //bool isServiceUp;
                //isServiceUp = CheckServiceAvailability.ServiceAvailability();

                //if (!isServiceUp)
                //    throw new Exception();

                List<MacroDatabaseKeyAnnualReportData> result = new List<MacroDatabaseKeyAnnualReportData>();
                //MacroDatabaseKeyAnnualReportData entry = new MacroDatabaseKeyAnnualReportData();
                DimensionEntitiesService.Entities entity = DimensionEntity;              
                ResearchEntities research = new ResearchEntities();
                //IList macroDatalist =  research.RetrieveCTYSUMMARYDataReport("AR").ToList();
                result = research.ExecuteStoreQuery<MacroDatabaseKeyAnnualReportData>("exec RetrieveEMSummaryDataReportPerCountry @country={0}", "AR").ToList();
                //foreach (var i in myList)
                //{
                //    MacroDatabaseKeyAnnualReportData entry = new MacroDatabaseKeyAnnualReportData();
                //    entry.CATEGORY_NAME = i.CATEGORY_NAME;
                //    entry.CountryName = i.CountryName;
                //    entry.Description = i.Description;
                //    entry.DisplayType = i.DisplayType;
                //    entry.FiveYEAR_Avg = i.FiveYEAR_Avg;
                //    entry.YEAR_1987 = i.YEAR_1987;
                //    entry.YEAR_1988 = i.YEAR_1988;
                //    entry.YEAR_1989 = i.YEAR_1989;
                //    entry.YEAR_1990 = i.YEAR_1990;
                //    entry.YEAR_1991 = i.YEAR_1991;
                //    entry.YEAR_1992 = i.YEAR_1992;
                //    entry.YEAR_1993 = i.YEAR_1993;
                //    entry.YEAR_1994 = i.YEAR_1994;
                //    entry.YEAR_1995 = i.YEAR_1995;
                //    entry.YEAR_1996 = i.YEAR_1996;
                //    entry.YEAR_1997 = i.YEAR_1997;
                //    entry.YEAR_1998 = i.YEAR_1998;
                //    entry.YEAR_1999 = i.YEAR_1999;
                //    entry.YEAR_2000 = i.YEAR_2000;
                //    entry.YEAR_2001 = i.YEAR_2001;
                //    entry.YEAR_2002 = i.YEAR_2002;
                //    entry.YEAR_2003 = i.YEAR_2003;
                //    entry.YEAR_2004 = i.YEAR_2004;
                //    entry.YEAR_2005 = i.YEAR_2005;
                //    entry.YEAR_2006 = i.YEAR_2006;
                //    entry.YEAR_2007 = i.YEAR_2007;
                //    entry.YEAR_2008 = i.YEAR_2008;
                //    entry.YEAR_2009 = i.YEAR_2009;
                //    entry.YEAR_2010 = i.YEAR_2010;
                //    entry.YEAR_2011 = i.YEAR_2011;
                //    entry.YEAR_2012 = i.YEAR_2012;
                //    entry.YEAR_2013 = i.YEAR_2013;
                //    entry.YEAR_2014 = i.YEAR_2014;
                //    entry.YEAR_2015 = i.YEAR_2015;
                //    entry.YEAR_2016 = i.YEAR_2016;
                //    entry.YEAR_2017 = i.YEAR_2017;
                //    entry.YEAR_2018 = i.YEAR_2018;
                //    entry.YEAR_2019 = i.YEAR_2019;
                //    entry.YEAR_2020 = i.YEAR_2020;
                //    entry.YEAR_2021 = i.YEAR_2021;
                //    entry.YEAR_2022 = i.YEAR_2022;
                //    entry.YEAR_2023 = i.YEAR_2023;
                //    entry.YEAR_2024 = i.YEAR_2024;
                //    entry.YEAR_2025 = i.YEAR_2025;                  
                //    result.Add(entry);
                //}
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
        /// Gets Commodity Data
        /// </summary>
        /// <param name="countryNameVal"></param>
        /// <returns>Commodity Result</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        public List<CommodityResult> RetrieveCommodityData()
        {
            try
            {
                //bool isServiceUp;
                //isServiceUp = CheckServiceAvailability.ServiceAvailability();

                //if (!isServiceUp)
                //    throw new Exception();

                List<CommodityResult> result = new List<CommodityResult>();
                //MacroDatabaseKeyAnnualReportData entry = new MacroDatabaseKeyAnnualReportData();
                DimensionEntitiesService.Entities entity = DimensionEntity;
                ResearchEntities research = new ResearchEntities();
                //IList macroDatalist =  research.RetrieveCTYSUMMARYDataReport("AR").ToList();
                result = research.ExecuteStoreQuery<CommodityResult>("exec GetCOMMODITY_FORECASTS").ToList();
               
                return result;
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