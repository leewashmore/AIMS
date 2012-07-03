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
using GreenField.Gadgets.Models;
using Telerik.Windows.Controls;
using System.Collections.Generic;
using GreenField.ServiceCaller.ExternalResearchDefinitions;
using System.Linq;
using GreenField.Common;
using GreenField.DataContracts;

namespace GreenField.Gadgets.Helpers
{
    public static class PeriodColumns
    {
        #region Period Column Update Event
        public delegate void PeriodColumnUpdateEventHandler(PeriodColumnUpdateEventArg e);

        public class PeriodColumnUpdateEventArg
        {
            public String PeriodColumnNamespace { get; set; }
            public List<String> PeriodColumnHeader { get; set; }
            public EntitySelectionData EntitySelectionData { get; set; }
            public PeriodRecord PeriodRecord { get; set; }
            public bool PeriodIsYearly { get; set; }
        }

        public static event PeriodColumnUpdateEventHandler PeriodColumnUpdate = delegate { };

        public static void RaisePeriodColumnUpdateCompleted(PeriodColumnUpdateEventArg e)
        {
            PeriodColumnUpdateEventHandler periodColumnUpdationEvent = PeriodColumnUpdate;
            if (periodColumnUpdationEvent != null)
            {
                periodColumnUpdationEvent(e);
            }
        }
        #endregion

        #region Period Column Navigation Event
        public delegate void PeriodColumnNavigationEventHandler(PeriodColumnNavigationEventArg e);

        public enum NavigationDirection
        {
            LEFT,
            RIGHT
        }

        public class PeriodColumnNavigationEventArg
        {
            public String PeriodColumnNamespace { get; set; }
            public NavigationDirection PeriodColumnNavigationDirection { get; set; }
        }

        public static event PeriodColumnNavigationEventHandler PeriodColumnNavigate = delegate { };

        public static void RaisePeriodColumnNavigationCompleted(PeriodColumnNavigationEventArg e)
        {
            PeriodColumnNavigationEventHandler periodColumnNavigationEvent = PeriodColumnNavigate;
            if (periodColumnNavigationEvent != null)
            {
                periodColumnNavigationEvent(e);
            }
        }
        #endregion

        /// <summary>
        /// Set the period record consisting of six years and quarters based on iteration
        /// </summary>
        /// <param name="incrementFactor">increment factor of iteration in period column year quarter calculation</param>
        /// <returns>PeriodRecord object</returns>
        public static PeriodRecord SetPeriodRecord(int incrementFactor = 0)
        {
            PeriodRecord periodRecord = new PeriodRecord();

            int presentYear = DateTime.Today.Year;
            int presentMonth = DateTime.Today.Month;
            int presentQuarter = GetQuarter(presentMonth);

            periodRecord.YearOne = presentYear - 3 + incrementFactor;
            periodRecord.YearOneIsHistorical = periodRecord.YearOne < presentYear;

            periodRecord.YearTwo = periodRecord.YearOne + 1;
            periodRecord.YearTwoIsHistorical = periodRecord.YearTwo < presentYear;

            periodRecord.YearThree = periodRecord.YearTwo + 1;
            periodRecord.YearThreeIsHistorical = periodRecord.YearThree < presentYear;

            periodRecord.YearFour = periodRecord.YearThree + 1;
            periodRecord.YearFourIsHistorical = periodRecord.YearFour < presentYear;

            periodRecord.YearFive = periodRecord.YearFour + 1;
            periodRecord.YearFiveIsHistorical = periodRecord.YearFive < presentYear;

            periodRecord.YearSix = periodRecord.YearFive + 1;
            periodRecord.YearSixIsHistorical = periodRecord.YearSix < presentYear;

            periodRecord.YearSeven = periodRecord.YearSix + 1;
            periodRecord.YearSevenIsHistorical = periodRecord.YearSeven < presentYear;

            periodRecord.QuarterOneYear = (new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor - 4) * 3).Year;
            periodRecord.QuarterOneQuarter = GetQuarter((new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor - 4) * 3).Month);
            periodRecord.QuarterOneIsHistorical = periodRecord.QuarterOneYear < presentYear
                ? true : (periodRecord.QuarterOneYear == presentYear ? periodRecord.QuarterOneQuarter < presentQuarter : false);

            periodRecord.QuarterTwoYear = (new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor - 3) * 3).Year;
            periodRecord.QuarterTwoQuarter = GetQuarter((new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor - 3) * 3).Month);
            periodRecord.QuarterTwoIsHistorical = periodRecord.QuarterTwoYear < presentYear
                ? true : (periodRecord.QuarterTwoYear == presentYear ? periodRecord.QuarterTwoQuarter < presentQuarter : false);


            periodRecord.QuarterThreeYear = (new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor - 2) * 3).Year;
            periodRecord.QuarterThreeQuarter = GetQuarter((new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor - 2) * 3).Month);
            periodRecord.QuarterThreeIsHistorical = periodRecord.QuarterThreeYear < presentYear
                ? true : (periodRecord.QuarterThreeYear == presentYear ? periodRecord.QuarterThreeQuarter < presentQuarter : false);

            periodRecord.QuarterFourYear = (new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor - 1) * 3).Year;
            periodRecord.QuarterFourQuarter = GetQuarter((new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor - 1) * 3).Month);
            periodRecord.QuarterFourIsHistorical = periodRecord.QuarterFourYear < presentYear
                ? true : (periodRecord.QuarterFourYear == presentYear ? periodRecord.QuarterFourQuarter < presentQuarter : false);

            periodRecord.QuarterFiveYear = (new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor) * 3).Year;
            periodRecord.QuarterFiveQuarter = GetQuarter((new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor) * 3).Month);
            periodRecord.QuarterFiveIsHistorical = periodRecord.QuarterFiveYear < presentYear
                ? true : (periodRecord.QuarterFiveYear == presentYear ? periodRecord.QuarterFiveQuarter < presentQuarter : false);

            periodRecord.QuarterSixYear = (new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor + 1) * 3).Year;
            periodRecord.QuarterSixQuarter = GetQuarter((new DateTime(presentYear, presentMonth, 1)).AddMonths((incrementFactor + 1) * 3).Month);
            periodRecord.QuarterSixIsHistorical = periodRecord.QuarterSixYear < presentYear
                ? true : (periodRecord.QuarterSixYear == presentYear ? periodRecord.QuarterSixQuarter < presentQuarter : false);

            return periodRecord;
        }

        public static List<String> SetColumnHeaders(PeriodRecord periodRecord = null, bool showHistorical = true)
        {
            if (periodRecord == null)
                periodRecord = SetPeriodRecord();

            List<String> periodColumnHeader = new List<string>();

            periodColumnHeader.Add(periodRecord.YearOne.ToString() + (showHistorical ? " " + (periodRecord.YearOneIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.YearTwo.ToString() + (showHistorical ? " " + (periodRecord.YearTwoIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.YearThree.ToString() + (showHistorical ? " " + (periodRecord.YearThreeIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.YearFour.ToString() + (showHistorical ? " " + (periodRecord.YearFourIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.YearFive.ToString() + (showHistorical ? " " + (periodRecord.YearFiveIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.YearSix.ToString() + (showHistorical ? " " + (periodRecord.YearSixIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.QuarterOneYear.ToString() + " Q" + periodRecord.QuarterOneQuarter.ToString() + (showHistorical ? " " + (periodRecord.QuarterOneIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.QuarterTwoYear.ToString() + " Q" + periodRecord.QuarterTwoQuarter.ToString() + (showHistorical ? " " + (periodRecord.QuarterTwoIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.QuarterThreeYear.ToString() + " Q" + periodRecord.QuarterThreeQuarter.ToString() + (showHistorical ? " " + (periodRecord.QuarterThreeIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.QuarterFourYear.ToString() + " Q" + periodRecord.QuarterFourQuarter.ToString() + (showHistorical ? " " + (periodRecord.QuarterFourIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.QuarterFiveYear.ToString() + " Q" + periodRecord.QuarterFiveQuarter.ToString() + (showHistorical ? " " + (periodRecord.QuarterFiveIsHistorical ? "A" : "E") : ""));
            periodColumnHeader.Add(periodRecord.QuarterSixYear.ToString() + " Q" + periodRecord.QuarterSixQuarter.ToString() + (showHistorical ? " " + (periodRecord.QuarterSixIsHistorical ? "A" : "E") : ""));

            return periodColumnHeader;
        }

        public static List<PeriodColumnDisplayData> SetPeriodColumnDisplayInfo(object periodColumnInfo, out PeriodRecord periodRecord, PeriodRecord periodRecordInfo)
        {
            List<PeriodColumnDisplayData> result = new List<PeriodColumnDisplayData>();
            PeriodRecord period = periodRecordInfo;

            #region Financial Statements
            if (periodColumnInfo is List<FinancialStatementData>)
            {
                List<FinancialStatementData> financialStatementInfo = (periodColumnInfo as List<FinancialStatementData>)
                    .OrderBy(record => record.SORT_ORDER)
                    .ThenBy(record => record.PERIOD_TYPE)
                    .ThenBy(record => record.PERIOD)
                    .ToList();

                List<String> distinctDataDescriptors = financialStatementInfo.Select(record => record.DATA_DESC).Distinct().ToList();

                foreach (string dataDesc in distinctDataDescriptors)
                {
                    FinancialStatementData recordData = financialStatementInfo.Where(record => record.DATA_DESC == dataDesc).FirstOrDefault();

                    Int32 dataId = recordData == null ? -1 : recordData.Data_ID;

                    #region Annual
                    #region Year One
                    FinancialStatementData yearOneData = financialStatementInfo
                                    .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.YearOne.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearOneIsHistorical ? "A" : "E") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A")
                                    .FirstOrDefault();

                    if (yearOneData == null)
                    {
                        yearOneData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD.ToUpper().Trim() == period.YearOne.ToString().ToUpper().Trim() &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearOneIsHistorical ? "E" : "A") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A")
                            .FirstOrDefault();
                        if (yearOneData != null)
                        {
                            period.YearOneIsHistorical = !(period.YearOneIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Two
                    FinancialStatementData yearTwoData = financialStatementInfo
                                    .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.YearTwo.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearTwoIsHistorical ? "A" : "E") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearTwoData == null)
                    {
                        yearTwoData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD.ToUpper().Trim() == period.YearTwo.ToString().ToUpper().Trim() &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearTwoIsHistorical ? "E" : "A") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearTwoData != null)
                        {
                            period.YearTwoIsHistorical = !(period.YearTwoIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Three
                    FinancialStatementData yearThreeData = financialStatementInfo
                                    .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.YearThree.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearThreeIsHistorical ? "A" : "E") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearThreeData == null)
                    {
                        yearThreeData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD.ToUpper().Trim() == period.YearThree.ToString().ToUpper().Trim() &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearThreeIsHistorical ? "E" : "A") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearThreeData != null)
                        {
                            period.YearThreeIsHistorical = !(period.YearThreeIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Four
                    FinancialStatementData yearFourData = financialStatementInfo
                                    .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.YearFour.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearFourIsHistorical ? "A" : "E") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearFourData == null)
                    {
                        yearFourData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD.ToUpper().Trim() == period.YearFour.ToString().ToUpper().Trim() &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearFourIsHistorical ? "E" : "A") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearFourData != null)
                        {
                            period.YearFourIsHistorical = !(period.YearFourIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Five
                    FinancialStatementData yearFiveData = financialStatementInfo
                                    .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.YearFive.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearFiveIsHistorical ? "A" : "E") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearFiveData == null)
                    {
                        yearFiveData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD.ToUpper().Trim() == period.YearFive.ToString().ToUpper().Trim() &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearFiveIsHistorical ? "E" : "A") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearFiveData != null)
                        {
                            period.YearFiveIsHistorical = !(period.YearFiveIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Six
                    FinancialStatementData yearSixData = financialStatementInfo
                                    .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.YearSix.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearSixIsHistorical ? "A" : "E") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearSixData == null)
                    {
                        yearSixData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD.ToUpper().Trim() == period.YearSix.ToString().ToUpper().Trim() &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearSixIsHistorical ? "E" : "A") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearSixData != null)
                        {
                            period.YearSixIsHistorical = !(period.YearSixIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Seven
                    FinancialStatementData yearSevenData = financialStatementInfo
                                    .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.YearSeven.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearSevenIsHistorical ? "A" : "E") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearSevenData == null)
                    {
                        yearSevenData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD.ToUpper().Trim() == period.YearSeven.ToString().ToUpper().Trim() &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearSevenIsHistorical ? "E" : "A") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearSevenData != null)
                        {
                            period.YearSevenIsHistorical = !(period.YearSevenIsHistorical);
                        }
                    }
                    #endregion
                    #endregion

                    #region Quarterly
                    #region Quarter One
                    FinancialStatementData quarterOneData = financialStatementInfo
                                                .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD.ToUpper().Trim() == period.QuarterOneYear.ToString().ToUpper().Trim() &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterOneIsHistorical ? "A" : "E") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterOneQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterOneData == null)
                    {
                        quarterOneData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.QuarterOneYear.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterOneIsHistorical ? "E" : "A") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterOneQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterOneData != null)
                        {
                            period.QuarterOneIsHistorical = !(period.QuarterOneIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Two
                    FinancialStatementData quarterTwoData = financialStatementInfo
                                                .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD.ToUpper().Trim() == period.QuarterTwoYear.ToString().ToUpper().Trim() &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterTwoIsHistorical ? "A" : "E") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterTwoQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterTwoData == null)
                    {
                        quarterTwoData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.QuarterTwoYear.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterTwoIsHistorical ? "E" : "A") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterTwoQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterTwoData != null)
                        {
                            period.QuarterTwoIsHistorical = !(period.QuarterTwoIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Three
                    FinancialStatementData quarterThreeData = financialStatementInfo
                                                .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD.ToUpper().Trim() == period.QuarterThreeYear.ToString().ToUpper().Trim() &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterThreeIsHistorical ? "A" : "E") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterThreeQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterThreeData == null)
                    {
                        quarterThreeData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.QuarterThreeYear.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterThreeIsHistorical ? "E" : "A") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterThreeQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterThreeData != null)
                        {
                            period.QuarterThreeIsHistorical = !(period.QuarterThreeIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Four
                    FinancialStatementData quarterFourData = financialStatementInfo
                                                .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD.ToUpper().Trim() == period.QuarterFourYear.ToString().ToUpper().Trim() &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterFourIsHistorical ? "A" : "E") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterFourQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterFourData == null)
                    {
                        quarterFourData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.QuarterFourYear.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterFourIsHistorical ? "E" : "A") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterFourQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterFourData != null)
                        {
                            period.QuarterFourIsHistorical = !(period.QuarterFourIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Five
                    FinancialStatementData quarterFiveData = financialStatementInfo
                                                .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD.ToUpper().Trim() == period.QuarterFiveYear.ToString().ToUpper().Trim() &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterFiveIsHistorical ? "A" : "E") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterFiveQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterFiveData == null)
                    {
                        quarterFiveData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.QuarterFiveYear.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterFiveIsHistorical ? "E" : "A") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterFiveQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterFiveData != null)
                        {
                            period.QuarterFiveIsHistorical = !(period.QuarterFiveIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Six
                    FinancialStatementData quarterSixData = financialStatementInfo
                                                .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD.ToUpper().Trim() == period.QuarterSixYear.ToString().ToUpper().Trim() &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterSixIsHistorical ? "A" : "E") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterSixQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterSixData == null)
                    {
                        quarterSixData = financialStatementInfo
                            .Where(record => record.DATA_DESC.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD.ToUpper().Trim() == period.QuarterSixYear.ToString().ToUpper().Trim() &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterSixIsHistorical ? "E" : "A") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterSixQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterSixData != null)
                        {
                            period.QuarterSixIsHistorical = !(period.QuarterSixIsHistorical);
                        }
                    }
                    #endregion
                    #endregion

                    result.Add(new PeriodColumnDisplayData()
                    {
                        DATA_ID = dataId,
                        DATA_DESC = dataDesc,
                        YEAR_ONE = yearOneData != null ? yearOneData.AMOUNT : null,
                        YEAR_TWO = yearTwoData != null ? yearTwoData.AMOUNT : null,
                        YEAR_THREE = yearThreeData != null ? yearThreeData.AMOUNT : null,
                        YEAR_FOUR = yearFourData != null ? yearFourData.AMOUNT : null,
                        YEAR_FIVE = yearFiveData != null ? yearFiveData.AMOUNT : null,
                        YEAR_SIX = yearSixData != null ? yearSixData.AMOUNT : null,
                        YEAR_SEVEN = yearSevenData != null ? yearSevenData.AMOUNT : null,
                        QUARTER_ONE = quarterOneData != null ? quarterOneData.AMOUNT : null,
                        QUARTER_TWO = quarterTwoData != null ? quarterTwoData.AMOUNT : null,
                        QUARTER_THREE = quarterThreeData != null ? quarterThreeData.AMOUNT : null,
                        QUARTER_FOUR = quarterFourData != null ? quarterFourData.AMOUNT : null,
                        QUARTER_FIVE = quarterFiveData != null ? quarterFiveData.AMOUNT : null,
                        QUARTER_SIX = quarterSixData != null ? quarterSixData.AMOUNT : null,
                    });
                }
            }
            #endregion

            #region Consensus Estimate Detail
            if (periodColumnInfo is List<ConsensusEstimateDetailedData>)
            {
                List<ConsensusEstimateDetailedData> consensusEstimateDetailedData = (periodColumnInfo as List<ConsensusEstimateDetailedData>);

                List<String> distinctDataDescriptors = consensusEstimateDetailedData.Select(record => record.ESTIMATE_TYPE).Distinct().ToList();

                foreach (string dataDesc in distinctDataDescriptors)
                {
                    //ConsensusEstimateDetailedData recordData = consensusEstimateDetailedData.Where(record => record.ESTIMATE_TYPE == dataDesc).FirstOrDefault();

                    #region Annual
                    #region Year One

                    //ConsensusEstimateDetailedData yearOneData = consensusEstimateDetailedData
                    //                .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                    //                    record.PERIOD_YEAR.ToUpper().Trim() == period.YearOne.ToString().ToUpper().Trim() &&
                    //                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearOneIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                    //                    record.PERIOD_TYPE.ToUpper().Trim() == "A")
                    //                .FirstOrDefault();

                    //if (yearOneData == null)
                    //{
                    //    yearOneData = consensusEstimateDetailedData
                    //        .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                    //            record.PERIOD_YEAR.ToUpper().Trim() == period.YearOne.ToString().ToUpper().Trim() &&
                    //            record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearOneIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                    //            record.PERIOD_TYPE.ToUpper().Trim() == "A")
                    //        .FirstOrDefault();
                    //    if (yearOneData != null)
                    //    {
                    //        period.YearOneIsHistorical = !(period.YearOneIsHistorical);
                    //    }
                    //}
                    #endregion

                    #region Year Two
                    ConsensusEstimateDetailedData yearTwoData = consensusEstimateDetailedData
                                    .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.YearTwo &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearTwoIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearTwoData == null)
                    {
                        yearTwoData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD_YEAR == period.YearTwo &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearTwoIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearTwoData != null)
                        {
                            period.YearTwoIsHistorical = !(period.YearTwoIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Three
                    ConsensusEstimateDetailedData yearThreeData = consensusEstimateDetailedData
                                    .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.YearThree &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearThreeIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearThreeData == null)
                    {
                        yearThreeData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD_YEAR == period.YearThree &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearThreeIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearThreeData != null)
                        {
                            period.YearThreeIsHistorical = !(period.YearThreeIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Four
                    ConsensusEstimateDetailedData yearFourData = consensusEstimateDetailedData
                                    .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.YearFour &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearFourIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearFourData == null)
                    {
                        yearFourData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD_YEAR == period.YearFour &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearFourIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearFourData != null)
                        {
                            period.YearFourIsHistorical = !(period.YearFourIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Five
                    ConsensusEstimateDetailedData yearFiveData = consensusEstimateDetailedData
                                    .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.YearFive &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearFiveIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearFiveData == null)
                    {
                        yearFiveData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD_YEAR == period.YearFive &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearFiveIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearFiveData != null)
                        {
                            period.YearFiveIsHistorical = !(period.YearFiveIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Six
                    ConsensusEstimateDetailedData yearSixData = consensusEstimateDetailedData
                                    .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.YearSix &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearSixIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearSixData == null)
                    {
                        yearSixData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PERIOD_YEAR == period.YearSix &&
                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearSixIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PERIOD_TYPE.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearSixData != null)
                        {
                            period.YearSixIsHistorical = !(period.YearSixIsHistorical);
                        }
                    }
                    #endregion

                    #endregion

                    #region Quarterly
                    #region Quarter One
                    //ConsensusEstimateDetailedData quarterOneData = consensusEstimateDetailedData
                    //                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                    //                                record.PERIOD_YEAR.ToUpper().Trim() == period.QuarterOneYear.ToString().ToUpper().Trim() &&
                    //                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterOneIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                    //                                record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterOneQuarter.ToString().ToUpper().Trim()
                    //                                )
                    //                            .FirstOrDefault();

                    //if (quarterOneData == null)
                    //{
                    //    quarterOneData = consensusEstimateDetailedData
                    //        .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                    //                    record.PERIOD_YEAR.ToUpper().Trim() == period.QuarterOneYear.ToString().ToUpper().Trim() &&
                    //                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterOneIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                    //                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterOneQuarter.ToString().ToUpper().Trim()
                    //                    )
                    //                .FirstOrDefault();
                    //    if (quarterOneData != null)
                    //    {
                    //        period.QuarterOneIsHistorical = !(period.QuarterOneIsHistorical);
                    //    }
                    //}
                    #endregion

                    #region Quarter Two
                    ConsensusEstimateDetailedData quarterTwoData = consensusEstimateDetailedData
                                                .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD_YEAR == period.QuarterTwoYear &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterTwoIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterTwoQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterTwoData == null)
                    {
                        quarterTwoData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.QuarterTwoYear &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterTwoIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterTwoQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterTwoData != null)
                        {
                            period.QuarterTwoIsHistorical = !(period.QuarterTwoIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Three
                    ConsensusEstimateDetailedData quarterThreeData = consensusEstimateDetailedData
                                                .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD_YEAR == period.QuarterThreeYear &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterThreeIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterThreeQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterThreeData == null)
                    {
                        quarterThreeData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.QuarterThreeYear &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterThreeIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterThreeQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterThreeData != null)
                        {
                            period.QuarterThreeIsHistorical = !(period.QuarterThreeIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Four
                    ConsensusEstimateDetailedData quarterFourData = consensusEstimateDetailedData
                                                .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD_YEAR == period.QuarterFourYear &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterFourIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterFourQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterFourData == null)
                    {
                        quarterFourData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.QuarterFourYear &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterFourIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterFourQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterFourData != null)
                        {
                            period.QuarterFourIsHistorical = !(period.QuarterFourIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Five
                    ConsensusEstimateDetailedData quarterFiveData = consensusEstimateDetailedData
                                                .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD_YEAR == period.QuarterFiveYear &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterFiveIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterFiveQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterFiveData == null)
                    {
                        quarterFiveData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.QuarterFiveYear &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterFiveIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterFiveQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterFiveData != null)
                        {
                            period.QuarterFiveIsHistorical = !(period.QuarterFiveIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Six
                    ConsensusEstimateDetailedData quarterSixData = consensusEstimateDetailedData
                                                .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PERIOD_YEAR == period.QuarterSixYear &&
                                                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterSixIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterSixQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterSixData == null)
                    {
                        quarterSixData = consensusEstimateDetailedData
                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PERIOD_YEAR == period.QuarterSixYear &&
                                        record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterSixIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterSixQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterSixData != null)
                        {
                            period.QuarterSixIsHistorical = !(period.QuarterSixIsHistorical);
                        }
                    }
                    #endregion
                    #endregion

                    #region Result Addition

                    #region #Estimates
                    PeriodColumnDisplayData estimatePeriodColumnData = new PeriodColumnDisplayData()
                                {
                                    DATA_DESC = dataDesc,
                                    SUB_DATA_DESC = "# Of Estimates",
                                };


                    if (yearTwoData != null)
                        estimatePeriodColumnData.YEAR_TWO = yearTwoData.NUMBER_OF_ESTIMATES;
                    if (yearThreeData != null)
                        estimatePeriodColumnData.YEAR_THREE = yearThreeData.NUMBER_OF_ESTIMATES;
                    if (yearFourData != null)
                        estimatePeriodColumnData.YEAR_FOUR = yearFourData.NUMBER_OF_ESTIMATES;
                    if (yearFiveData != null)
                        estimatePeriodColumnData.YEAR_FIVE = yearFiveData.NUMBER_OF_ESTIMATES;
                    if (yearSixData != null)
                        estimatePeriodColumnData.YEAR_SIX = yearSixData.NUMBER_OF_ESTIMATES;
                    if (quarterTwoData != null)
                        estimatePeriodColumnData.QUARTER_TWO = quarterTwoData.NUMBER_OF_ESTIMATES;
                    if (quarterThreeData != null)
                        estimatePeriodColumnData.QUARTER_THREE = quarterThreeData.NUMBER_OF_ESTIMATES;
                    if (quarterFourData != null)
                        estimatePeriodColumnData.QUARTER_FOUR = quarterFourData.NUMBER_OF_ESTIMATES;
                    if (quarterFiveData != null)
                        estimatePeriodColumnData.QUARTER_FIVE = quarterFiveData.NUMBER_OF_ESTIMATES;
                    if (quarterSixData != null)
                        estimatePeriodColumnData.QUARTER_SIX = quarterSixData.NUMBER_OF_ESTIMATES;

                    result.Add(estimatePeriodColumnData);
                    #endregion

                    #region High
                    PeriodColumnDisplayData highPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "High",
                    };

                    if (yearTwoData != null)
                        highPeriodColumnData.YEAR_TWO = yearTwoData.HIGH;
                    if (yearThreeData != null)
                        highPeriodColumnData.YEAR_THREE = yearThreeData.HIGH;
                    if (yearFourData != null)
                        highPeriodColumnData.YEAR_FOUR = yearFourData.HIGH;
                    if (yearFiveData != null)
                        highPeriodColumnData.YEAR_FIVE = yearFiveData.HIGH;
                    if (yearSixData != null)
                        highPeriodColumnData.YEAR_SIX = yearSixData.HIGH;
                    if (quarterTwoData != null)
                        highPeriodColumnData.QUARTER_TWO = quarterTwoData.HIGH;
                    if (quarterThreeData != null)
                        highPeriodColumnData.QUARTER_THREE = quarterThreeData.HIGH;
                    if (quarterFourData != null)
                        highPeriodColumnData.QUARTER_FOUR = quarterFourData.HIGH;
                    if (quarterFiveData != null)
                        highPeriodColumnData.QUARTER_FIVE = quarterFiveData.HIGH;
                    if (quarterSixData != null)
                        highPeriodColumnData.QUARTER_SIX = quarterSixData.HIGH;

                    result.Add(highPeriodColumnData);
                    #endregion

                    #region Low
                    PeriodColumnDisplayData lowPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Low",
                    };

                    if (yearTwoData != null)
                        lowPeriodColumnData.YEAR_TWO = yearTwoData.LOW;
                    if (yearThreeData != null)
                        lowPeriodColumnData.YEAR_THREE = yearThreeData.LOW;
                    if (yearFourData != null)
                        lowPeriodColumnData.YEAR_FOUR = yearFourData.LOW;
                    if (yearFiveData != null)
                        lowPeriodColumnData.YEAR_FIVE = yearFiveData.LOW;
                    if (yearSixData != null)
                        lowPeriodColumnData.YEAR_SIX = yearSixData.LOW;
                    if (quarterTwoData != null)
                        lowPeriodColumnData.QUARTER_TWO = quarterTwoData.LOW;
                    if (quarterThreeData != null)
                        lowPeriodColumnData.QUARTER_THREE = quarterThreeData.LOW;
                    if (quarterFourData != null)
                        lowPeriodColumnData.QUARTER_FOUR = quarterFourData.LOW;
                    if (quarterFiveData != null)
                        lowPeriodColumnData.QUARTER_FIVE = quarterFiveData.LOW;
                    if (quarterSixData != null)
                        lowPeriodColumnData.QUARTER_SIX = quarterSixData.LOW;

                    result.Add(lowPeriodColumnData);
                    #endregion

                    #region Std. Dev
                    PeriodColumnDisplayData stdDevPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Std Dev",
                    };

                    if (yearTwoData != null)
                        stdDevPeriodColumnData.YEAR_TWO = yearTwoData.STANDARD_DEVIATION;
                    if (yearThreeData != null)
                        stdDevPeriodColumnData.YEAR_THREE = yearThreeData.STANDARD_DEVIATION;
                    if (yearFourData != null)
                        stdDevPeriodColumnData.YEAR_FOUR = yearFourData.STANDARD_DEVIATION;
                    if (yearFiveData != null)
                        stdDevPeriodColumnData.YEAR_FIVE = yearFiveData.STANDARD_DEVIATION;
                    if (yearSixData != null)
                        stdDevPeriodColumnData.YEAR_SIX = yearSixData.STANDARD_DEVIATION;
                    if (quarterTwoData != null)
                        stdDevPeriodColumnData.QUARTER_TWO = quarterTwoData.STANDARD_DEVIATION;
                    if (quarterThreeData != null)
                        stdDevPeriodColumnData.QUARTER_THREE = quarterThreeData.STANDARD_DEVIATION;
                    if (quarterFourData != null)
                        stdDevPeriodColumnData.QUARTER_FOUR = quarterFourData.STANDARD_DEVIATION;
                    if (quarterFiveData != null)
                        stdDevPeriodColumnData.QUARTER_FIVE = quarterFiveData.STANDARD_DEVIATION;
                    if (quarterSixData != null)
                        stdDevPeriodColumnData.QUARTER_SIX = quarterSixData.STANDARD_DEVIATION;

                    result.Add(stdDevPeriodColumnData);
                    #endregion

                    #region Consensus Median
                    PeriodColumnDisplayData consensusMedianPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Consensus Median",
                    };

                    if (yearTwoData != null)
                        consensusMedianPeriodColumnData.YEAR_TWO = yearTwoData.AMOUNT;
                    if (yearThreeData != null)
                        consensusMedianPeriodColumnData.YEAR_THREE = yearThreeData.AMOUNT;
                    if (yearFourData != null)
                        consensusMedianPeriodColumnData.YEAR_FOUR = yearFourData.AMOUNT;
                    if (yearFiveData != null)
                        consensusMedianPeriodColumnData.YEAR_FIVE = yearFiveData.AMOUNT;
                    if (yearSixData != null)
                        consensusMedianPeriodColumnData.YEAR_SIX = yearSixData.AMOUNT;
                    if (quarterTwoData != null)
                        consensusMedianPeriodColumnData.QUARTER_TWO = quarterTwoData.AMOUNT;
                    if (quarterThreeData != null)
                        consensusMedianPeriodColumnData.QUARTER_THREE = quarterThreeData.AMOUNT;
                    if (quarterFourData != null)
                        consensusMedianPeriodColumnData.QUARTER_FOUR = quarterFourData.AMOUNT;
                    if (quarterFiveData != null)
                        consensusMedianPeriodColumnData.QUARTER_FIVE = quarterFiveData.AMOUNT;
                    if (quarterSixData != null)
                        consensusMedianPeriodColumnData.QUARTER_SIX = quarterSixData.AMOUNT;

                    result.Add(consensusMedianPeriodColumnData);
                    #endregion

                    #endregion
                }
            }
            #endregion

            #region Consensus Estimate Median
            if (periodColumnInfo is List<ConsensusEstimateMedian>)
            {
                List<ConsensusEstimateMedian> consensusEstimateDetailedData = (periodColumnInfo as List<ConsensusEstimateMedian>);

                List<String> distinctDataDescriptors = consensusEstimateDetailedData.Select(record => record.EstimateType).Distinct().ToList();

                foreach (string dataDesc in distinctDataDescriptors)
                {
                    //ConsensusEstimateDetailedData recordData = consensusEstimateDetailedData.Where(record => record.ESTIMATE_TYPE == dataDesc).FirstOrDefault();

                    #region Annual
                    #region Year One

                    //ConsensusEstimateDetailedData yearOneData = consensusEstimateDetailedData
                    //                .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                    //                    record.PERIOD_YEAR.ToUpper().Trim() == period.YearOne.ToString().ToUpper().Trim() &&
                    //                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearOneIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                    //                    record.PERIOD_TYPE.ToUpper().Trim() == "A")
                    //                .FirstOrDefault();

                    //if (yearOneData == null)
                    //{
                    //    yearOneData = consensusEstimateDetailedData
                    //        .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                    //            record.PERIOD_YEAR.ToUpper().Trim() == period.YearOne.ToString().ToUpper().Trim() &&
                    //            record.AMOUNT_TYPE.ToUpper().Trim() == (period.YearOneIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                    //            record.PERIOD_TYPE.ToUpper().Trim() == "A")
                    //        .FirstOrDefault();
                    //    if (yearOneData != null)
                    //    {
                    //        period.YearOneIsHistorical = !(period.YearOneIsHistorical);
                    //    }
                    //}
                    #endregion

                    #region Year Two
                    ConsensusEstimateMedian yearTwoData = consensusEstimateDetailedData
                                    .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.YearTwo &&
                                        record.AmountType.ToUpper().Trim() == (period.YearTwoIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PeriodType.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearTwoData == null)
                    {
                        yearTwoData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PeriodYear == period.YearTwo &&
                                record.AmountType.ToUpper().Trim() == (period.YearTwoIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PeriodType.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearTwoData != null)
                        {
                            period.YearTwoIsHistorical = !(period.YearTwoIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Three
                    ConsensusEstimateMedian yearThreeData = consensusEstimateDetailedData
                                    .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.YearThree &&
                                        record.AmountType.ToUpper().Trim() == (period.YearThreeIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PeriodType.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearThreeData == null)
                    {
                        yearThreeData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PeriodYear == period.YearThree &&
                                record.AmountType.ToUpper().Trim() == (period.YearThreeIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PeriodType.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearThreeData != null)
                        {
                            period.YearThreeIsHistorical = !(period.YearThreeIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Four
                    ConsensusEstimateMedian yearFourData = consensusEstimateDetailedData
                                    .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.YearFour &&
                                        record.AmountType.ToUpper().Trim() == (period.YearFourIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PeriodType.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearFourData == null)
                    {
                        yearFourData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PeriodYear == period.YearFour &&
                                record.AmountType.ToUpper().Trim() == (period.YearFourIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PeriodType.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearFourData != null)
                        {
                            period.YearFourIsHistorical = !(period.YearFourIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Five
                    ConsensusEstimateMedian yearFiveData = consensusEstimateDetailedData
                                    .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.YearFive &&
                                        record.AmountType.ToUpper().Trim() == (period.YearFiveIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PeriodType.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearFiveData == null)
                    {
                        yearFiveData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PeriodYear == period.YearFive &&
                                record.AmountType.ToUpper().Trim() == (period.YearFiveIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PeriodType.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearFiveData != null)
                        {
                            period.YearFiveIsHistorical = !(period.YearFiveIsHistorical);
                        }
                    }
                    #endregion

                    #region Year Six
                    ConsensusEstimateMedian yearSixData = consensusEstimateDetailedData
                                    .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.YearSix &&
                                        record.AmountType.ToUpper().Trim() == (period.YearSixIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                        record.PeriodType.ToUpper().Trim() == "A"
                                        )
                                    .FirstOrDefault();

                    if (yearSixData == null)
                    {
                        yearSixData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                record.PeriodYear == period.YearSix &&
                                record.AmountType.ToUpper().Trim() == (period.YearSixIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                record.PeriodType.ToUpper().Trim() == "A"
                                )
                            .FirstOrDefault();
                        if (yearSixData != null)
                        {
                            period.YearSixIsHistorical = !(period.YearSixIsHistorical);
                        }
                    }
                    #endregion

                    #endregion

                    #region Quarterly
                    #region Quarter One
                    //ConsensusEstimateDetailedData quarterOneData = consensusEstimateDetailedData
                    //                            .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                    //                                record.PERIOD_YEAR.ToUpper().Trim() == period.QuarterOneYear.ToString().ToUpper().Trim() &&
                    //                                record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterOneIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                    //                                record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterOneQuarter.ToString().ToUpper().Trim()
                    //                                )
                    //                            .FirstOrDefault();

                    //if (quarterOneData == null)
                    //{
                    //    quarterOneData = consensusEstimateDetailedData
                    //        .Where(record => record.ESTIMATE_TYPE.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                    //                    record.PERIOD_YEAR.ToUpper().Trim() == period.QuarterOneYear.ToString().ToUpper().Trim() &&
                    //                    record.AMOUNT_TYPE.ToUpper().Trim() == (period.QuarterOneIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                    //                    record.PERIOD_TYPE.ToUpper().Trim() == "Q" + period.QuarterOneQuarter.ToString().ToUpper().Trim()
                    //                    )
                    //                .FirstOrDefault();
                    //    if (quarterOneData != null)
                    //    {
                    //        period.QuarterOneIsHistorical = !(period.QuarterOneIsHistorical);
                    //    }
                    //}
                    #endregion

                    #region Quarter Two
                    ConsensusEstimateMedian quarterTwoData = consensusEstimateDetailedData
                                                .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PeriodYear == period.QuarterTwoYear &&
                                                    record.AmountType.ToUpper().Trim() == (period.QuarterTwoIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterTwoQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterTwoData == null)
                    {
                        quarterTwoData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.QuarterTwoYear &&
                                        record.AmountType.ToUpper().Trim() == (period.QuarterTwoIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterTwoQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterTwoData != null)
                        {
                            period.QuarterTwoIsHistorical = !(period.QuarterTwoIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Three
                    ConsensusEstimateMedian quarterThreeData = consensusEstimateDetailedData
                                                .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PeriodYear == period.QuarterThreeYear &&
                                                    record.AmountType.ToUpper().Trim() == (period.QuarterThreeIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterThreeQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterThreeData == null)
                    {
                        quarterThreeData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.QuarterThreeYear &&
                                        record.AmountType.ToUpper().Trim() == (period.QuarterThreeIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterThreeQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterThreeData != null)
                        {
                            period.QuarterThreeIsHistorical = !(period.QuarterThreeIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Four
                    ConsensusEstimateMedian quarterFourData = consensusEstimateDetailedData
                                                .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PeriodYear == period.QuarterFourYear &&
                                                    record.AmountType.ToUpper().Trim() == (period.QuarterFourIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterFourQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterFourData == null)
                    {
                        quarterFourData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.QuarterFourYear &&
                                        record.AmountType.ToUpper().Trim() == (period.QuarterFourIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterFourQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterFourData != null)
                        {
                            period.QuarterFourIsHistorical = !(period.QuarterFourIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Five
                    ConsensusEstimateMedian quarterFiveData = consensusEstimateDetailedData
                                                .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PeriodYear == period.QuarterFiveYear &&
                                                    record.AmountType.ToUpper().Trim() == (period.QuarterFiveIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterFiveQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterFiveData == null)
                    {
                        quarterFiveData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.QuarterFiveYear &&
                                        record.AmountType.ToUpper().Trim() == (period.QuarterFiveIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterFiveQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterFiveData != null)
                        {
                            period.QuarterFiveIsHistorical = !(period.QuarterFiveIsHistorical);
                        }
                    }
                    #endregion

                    #region Quarter Six
                    ConsensusEstimateMedian quarterSixData = consensusEstimateDetailedData
                                                .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                                    record.PeriodYear == period.QuarterSixYear &&
                                                    record.AmountType.ToUpper().Trim() == (period.QuarterSixIsHistorical ? "ACTUAL" : "ESTIMATE") &&
                                                    record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterSixQuarter.ToString().ToUpper().Trim()
                                                    )
                                                .FirstOrDefault();

                    if (quarterSixData == null)
                    {
                        quarterSixData = consensusEstimateDetailedData
                            .Where(record => record.EstimateType.ToUpper().Trim() == dataDesc.ToUpper().Trim() &&
                                        record.PeriodYear == period.QuarterSixYear &&
                                        record.AmountType.ToUpper().Trim() == (period.QuarterSixIsHistorical ? "ESTIMATE" : "ACTUAL") &&
                                        record.PeriodType.ToUpper().Trim() == "Q" + period.QuarterSixQuarter.ToString().ToUpper().Trim()
                                        )
                                    .FirstOrDefault();
                        if (quarterSixData != null)
                        {
                            period.QuarterSixIsHistorical = !(period.QuarterSixIsHistorical);
                        }
                    }
                    #endregion
                    #endregion

                    #region Result Addition

                    #region #Estimates
                    PeriodColumnDisplayData estimatePeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "# Of Estimates",
                    };


                    if (yearTwoData != null)
                        estimatePeriodColumnData.YEAR_TWO = yearTwoData.NumberOfEstimates;
                    if (yearThreeData != null)
                        estimatePeriodColumnData.YEAR_THREE = yearThreeData.NumberOfEstimates;
                    if (yearFourData != null)
                        estimatePeriodColumnData.YEAR_FOUR = yearFourData.NumberOfEstimates;
                    if (yearFiveData != null)
                        estimatePeriodColumnData.YEAR_FIVE = yearFiveData.NumberOfEstimates;
                    if (yearSixData != null)
                        estimatePeriodColumnData.YEAR_SIX = yearSixData.NumberOfEstimates;
                    if (quarterTwoData != null)
                        estimatePeriodColumnData.QUARTER_TWO = quarterTwoData.NumberOfEstimates;
                    if (quarterThreeData != null)
                        estimatePeriodColumnData.QUARTER_THREE = quarterThreeData.NumberOfEstimates;
                    if (quarterFourData != null)
                        estimatePeriodColumnData.QUARTER_FOUR = quarterFourData.NumberOfEstimates;
                    if (quarterFiveData != null)
                        estimatePeriodColumnData.QUARTER_FIVE = quarterFiveData.NumberOfEstimates;
                    if (quarterSixData != null)
                        estimatePeriodColumnData.QUARTER_SIX = quarterSixData.NumberOfEstimates;

                    result.Add(estimatePeriodColumnData);
                    #endregion

                    #region High
                    PeriodColumnDisplayData highPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "High",
                    };


                    if (yearTwoData != null)
                        highPeriodColumnData.YEAR_TWO = yearTwoData.High;
                    if (yearThreeData != null)
                        highPeriodColumnData.YEAR_THREE = yearThreeData.High;
                    if (yearFourData != null)
                        highPeriodColumnData.YEAR_FOUR = yearFourData.High;
                    if (yearFiveData != null)
                        highPeriodColumnData.YEAR_FIVE = yearFiveData.High;
                    if (yearSixData != null)
                        highPeriodColumnData.YEAR_SIX = yearSixData.High;
                    if (quarterTwoData != null)
                        highPeriodColumnData.QUARTER_TWO = quarterTwoData.High;
                    if (quarterThreeData != null)
                        highPeriodColumnData.QUARTER_THREE = quarterThreeData.High;
                    if (quarterFourData != null)
                        highPeriodColumnData.QUARTER_FOUR = quarterFourData.High;
                    if (quarterFiveData != null)
                        highPeriodColumnData.QUARTER_FIVE = quarterFiveData.High;
                    if (quarterSixData != null)
                        highPeriodColumnData.QUARTER_SIX = quarterSixData.High;

                    result.Add(highPeriodColumnData);
                    #endregion

                    #region Net Income
                    PeriodColumnDisplayData netIncomePeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Net Income",
                    };


                    if (yearTwoData != null)
                        netIncomePeriodColumnData.YEAR_TWO = yearTwoData.Amount;
                    if (yearThreeData != null)
                        netIncomePeriodColumnData.YEAR_THREE = yearThreeData.Amount;
                    if (yearFourData != null)
                        netIncomePeriodColumnData.YEAR_FOUR = yearFourData.Amount;
                    if (yearFiveData != null)
                        netIncomePeriodColumnData.YEAR_FIVE = yearFiveData.Amount;
                    if (yearSixData != null)
                        netIncomePeriodColumnData.YEAR_SIX = yearSixData.Amount;
                    if (quarterTwoData != null)
                        netIncomePeriodColumnData.QUARTER_TWO = quarterTwoData.Amount;
                    if (quarterThreeData != null)
                        netIncomePeriodColumnData.QUARTER_THREE = quarterThreeData.Amount;
                    if (quarterFourData != null)
                        netIncomePeriodColumnData.QUARTER_FOUR = quarterFourData.Amount;
                    if (quarterFiveData != null)
                        netIncomePeriodColumnData.QUARTER_FIVE = quarterFiveData.Amount;
                    if (quarterSixData != null)
                        netIncomePeriodColumnData.QUARTER_SIX = quarterSixData.Amount;

                    result.Add(netIncomePeriodColumnData);
                    #endregion

                    #region Actual
                    PeriodColumnDisplayData actualPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Actual",
                    };


                    if (yearTwoData != null)
                        actualPeriodColumnData.YEAR_TWO = yearTwoData.Amount;
                    if (yearThreeData != null)
                        actualPeriodColumnData.YEAR_THREE = yearThreeData.Amount;
                    if (yearFourData != null)
                        actualPeriodColumnData.YEAR_FOUR = yearFourData.Amount;
                    if (yearFiveData != null)
                        actualPeriodColumnData.YEAR_FIVE = yearFiveData.Amount;
                    if (yearSixData != null)
                        actualPeriodColumnData.YEAR_SIX = yearSixData.Amount;
                    if (quarterTwoData != null)
                        actualPeriodColumnData.QUARTER_TWO = quarterTwoData.Amount;
                    if (quarterThreeData != null)
                        actualPeriodColumnData.QUARTER_THREE = quarterThreeData.Amount;
                    if (quarterFourData != null)
                        actualPeriodColumnData.QUARTER_FOUR = quarterFourData.Amount;
                    if (quarterFiveData != null)
                        actualPeriodColumnData.QUARTER_FIVE = quarterFiveData.Amount;
                    if (quarterSixData != null)
                        actualPeriodColumnData.QUARTER_SIX = quarterSixData.Amount;

                    result.Add(actualPeriodColumnData);
                    #endregion

                    #region Low
                    PeriodColumnDisplayData lowPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Actual",
                    };


                    if (yearTwoData != null)
                        lowPeriodColumnData.YEAR_TWO = yearTwoData.Low;
                    if (yearThreeData != null)
                        lowPeriodColumnData.YEAR_THREE = yearThreeData.Low;
                    if (yearFourData != null)
                        lowPeriodColumnData.YEAR_FOUR = yearFourData.Low;
                    if (yearFiveData != null)
                        lowPeriodColumnData.YEAR_FIVE = yearFiveData.Low;
                    if (yearSixData != null)
                        lowPeriodColumnData.YEAR_SIX = yearSixData.Low;
                    if (quarterTwoData != null)
                        lowPeriodColumnData.QUARTER_TWO = quarterTwoData.Low;
                    if (quarterThreeData != null)
                        lowPeriodColumnData.QUARTER_THREE = quarterThreeData.Low;
                    if (quarterFourData != null)
                        lowPeriodColumnData.QUARTER_FOUR = quarterFourData.Low;
                    if (quarterFiveData != null)
                        lowPeriodColumnData.QUARTER_FIVE = quarterFiveData.Low;
                    if (quarterSixData != null)
                        lowPeriodColumnData.QUARTER_SIX = quarterSixData.Low;

                    result.Add(lowPeriodColumnData);
                    #endregion

                    #region Standard Deviation
                    PeriodColumnDisplayData standardDeviationPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Actual",
                    };


                    if (yearTwoData != null)
                        standardDeviationPeriodColumnData.YEAR_TWO = yearTwoData.StandardDeviation;
                    if (yearThreeData != null)
                        standardDeviationPeriodColumnData.YEAR_THREE = yearThreeData.StandardDeviation;
                    if (yearFourData != null)
                        standardDeviationPeriodColumnData.YEAR_FOUR = yearFourData.StandardDeviation;
                    if (yearFiveData != null)
                        standardDeviationPeriodColumnData.YEAR_FIVE = yearFiveData.StandardDeviation;
                    if (yearSixData != null)
                        standardDeviationPeriodColumnData.YEAR_SIX = yearSixData.StandardDeviation;
                    if (quarterTwoData != null)
                        standardDeviationPeriodColumnData.QUARTER_TWO = quarterTwoData.StandardDeviation;
                    if (quarterThreeData != null)
                        standardDeviationPeriodColumnData.QUARTER_THREE = quarterThreeData.StandardDeviation;
                    if (quarterFourData != null)
                        standardDeviationPeriodColumnData.QUARTER_FOUR = quarterFourData.StandardDeviation;
                    if (quarterFiveData != null)
                        standardDeviationPeriodColumnData.QUARTER_FIVE = quarterFiveData.StandardDeviation;
                    if (quarterSixData != null)
                        standardDeviationPeriodColumnData.QUARTER_SIX = quarterSixData.StandardDeviation;

                    result.Add(standardDeviationPeriodColumnData);
                    #endregion

                    #region Last Update

                    PeriodColumnDisplayData lastUpdatePeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Source Date",
                    };


                    if (yearTwoData != null)
                        lastUpdatePeriodColumnData.YEAR_TWO = yearTwoData.DataSourceDate;
                    if (yearThreeData != null)
                        lastUpdatePeriodColumnData.YEAR_THREE = yearThreeData.DataSourceDate;
                    if (yearFourData != null)
                        lastUpdatePeriodColumnData.YEAR_FOUR = yearFourData.DataSourceDate;
                    if (yearFiveData != null)
                        lastUpdatePeriodColumnData.YEAR_FIVE = yearFiveData.DataSourceDate;
                    if (yearSixData != null)
                        lastUpdatePeriodColumnData.YEAR_SIX = yearSixData.DataSourceDate;
                    if (quarterTwoData != null)
                        lastUpdatePeriodColumnData.QUARTER_TWO = quarterTwoData.DataSourceDate;
                    if (quarterThreeData != null)
                        lastUpdatePeriodColumnData.QUARTER_THREE = quarterThreeData.DataSourceDate;
                    if (quarterFourData != null)
                        lastUpdatePeriodColumnData.QUARTER_FOUR = quarterFourData.DataSourceDate;
                    if (quarterFiveData != null)
                        lastUpdatePeriodColumnData.QUARTER_FIVE = quarterFiveData.DataSourceDate;
                    if (quarterSixData != null)
                        lastUpdatePeriodColumnData.QUARTER_SIX = quarterSixData.DataSourceDate;

                    result.Add(lastUpdatePeriodColumnData);
                    #endregion

                    #region Reported Currency

                    PeriodColumnDisplayData reportedCurrencyPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "Reported Currency",
                    };


                    if (yearTwoData != null)
                        reportedCurrencyPeriodColumnData.YEAR_TWO = yearTwoData.SourceCurrency;
                    if (yearThreeData != null)
                        reportedCurrencyPeriodColumnData.YEAR_THREE = yearThreeData.SourceCurrency;
                    if (yearFourData != null)
                        reportedCurrencyPeriodColumnData.YEAR_FOUR = yearFourData.SourceCurrency;
                    if (yearFiveData != null)
                        reportedCurrencyPeriodColumnData.YEAR_FIVE = yearFiveData.SourceCurrency;
                    if (yearSixData != null)
                        reportedCurrencyPeriodColumnData.YEAR_SIX = yearSixData.SourceCurrency;
                    if (quarterTwoData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_TWO = quarterTwoData.SourceCurrency;
                    if (quarterThreeData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_THREE = quarterThreeData.SourceCurrency;
                    if (quarterFourData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_FOUR = quarterFourData.SourceCurrency;
                    if (quarterFiveData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_FIVE = quarterFiveData.SourceCurrency;
                    if (quarterSixData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_SIX = quarterSixData.SourceCurrency;

                    result.Add(reportedCurrencyPeriodColumnData);
                    #endregion

                    #region YOY Growth

                    PeriodColumnDisplayData YOYPeriodColumnData = new PeriodColumnDisplayData()
                    {
                        DATA_DESC = dataDesc,
                        SUB_DATA_DESC = "YOY",
                    };

                    if (yearTwoData != null)
                        reportedCurrencyPeriodColumnData.YEAR_TWO = 0.ToString();
                    if (yearThreeData != null)
                        reportedCurrencyPeriodColumnData.YEAR_THREE = ((yearThreeData.Amount - Convert.ToDecimal(yearTwoData.Amount)) - 1).ToString() + "%";
                    if (yearFourData != null)
                        reportedCurrencyPeriodColumnData.YEAR_FOUR = ((yearFourData.Amount - Convert.ToDecimal(yearThreeData.Amount)) - 1).ToString() + "%";
                    if (yearFiveData != null)
                        reportedCurrencyPeriodColumnData.YEAR_FIVE = ((yearFiveData.Amount - Convert.ToDecimal(yearFourData.Amount)) - 1).ToString() + "%";
                    if (yearSixData != null)
                        reportedCurrencyPeriodColumnData.YEAR_SIX = ((yearSixData.Amount - Convert.ToDecimal(yearFiveData.Amount)) - 1).ToString() + "%";
                    if (quarterTwoData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_TWO = 0.ToString();
                    if (quarterThreeData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_THREE = ((quarterThreeData.Amount - Convert.ToDecimal(quarterTwoData.Amount)) - 1).ToString() + "%";
                    if (quarterFourData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_FOUR = ((quarterFourData.Amount - Convert.ToDecimal(quarterThreeData.Amount)) - 1).ToString() + "%";
                    if (quarterFiveData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_FIVE = ((quarterFiveData.Amount - Convert.ToDecimal(quarterFourData.Amount)) - 1).ToString() + "%";
                    if (quarterSixData != null)
                        reportedCurrencyPeriodColumnData.QUARTER_SIX = ((quarterSixData.Amount - Convert.ToDecimal(quarterFiveData.Amount)) - 1).ToString() + "%";


                    #endregion

                    #endregion
                }
            }
            #endregion

            periodRecord = period;
            return result;
        }

        public static void UpdateColumnInformation(RadGridView gridView, PeriodColumnUpdateEventArg e, bool IsFinancial = true)
        {
            if (IsFinancial)
            {
                for (int i = 0; i < 12; i++)
                {
                    gridView.Columns[i + 2].Header = e.PeriodColumnHeader[i];
                    gridView.Columns[i + 2].IsVisible = i < 6 ? e.PeriodIsYearly : !(e.PeriodIsYearly);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    gridView.Columns[i + 2].Header = e.PeriodColumnHeader[i + (i < 5 ? 1 : 2)];
                    gridView.Columns[i + 2].IsVisible = i < 5 ? e.PeriodIsYearly : !(e.PeriodIsYearly);
                }
            }

        }

        private static int GetQuarter(int month)
        {
            return month < 4 ? 1 : (month < 7 ? 2 : (month < 10 ? 3 : 4));
        }
    }
}
