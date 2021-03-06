﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GreenField.DataContracts;
using System.Configuration;
using GreenField.Web.Helpers;

namespace GreenField.Web.Helpers
{
    /// <summary>
    /// Calculations for Market Cap Grid
    /// </summary>
    public class MarketCapitalizationCalculations
    {

        /// <summary>
        /// Calculates Weighted Average for Portfolio
        /// </summary>
        /// <param name="marketCapDetails"></param>
        /// <returns>_portfolioWeightedAvg</returns>
        public static decimal CalculatePortfolioWeightedAvg(List<MarketCapitalizationData> marketCapDetails)
        {
            decimal? totalPortfolioMV = 0;
            decimal portfolioWtdAvg = 0;

            if (marketCapDetails == null)
                throw new ArgumentNullException(GreenfieldConstants.MARKET_CAPITALIZATION);

            //Calculate total Market Value(DirtyValue_PC) for portfolio
            totalPortfolioMV = marketCapDetails.Sum(a => a.PortfolioDirtyValuePC);

            foreach (MarketCapitalizationData _mktCapData in marketCapDetails)
            {
                //if (!(string.IsNullOrEmpty(_mktCapData.Portfolio_ID)) && (_mktCapData.MarketCapitalInUSD != 0 && _mktCapData.MarketCapitalInUSD != null))
                if (_mktCapData.MarketCapitalInUSD != 0 && _mktCapData.MarketCapitalInUSD != null)
                    portfolioWtdAvg = portfolioWtdAvg + Convert.ToDecimal(_mktCapData.PortfolioDirtyValuePC / totalPortfolioMV * _mktCapData.MarketCapitalInUSD);
            }

            return portfolioWtdAvg;
        }
        /// <summary>
        /// Calculates Weighted Average for Benchmark
        /// </summary>
        /// <param name="marketCapDetails"></param>
        /// <returns>_benchmarkWeightedAvg</returns>
        public static decimal CalculateBenchmarkWeightedAvg(List<MarketCapitalizationData> marketCapDetails, String filterType, String filterValue, bool isExCashSecurity)
        {
            decimal? totalBenchmarkWeight = 0;
            decimal benchmarkWtdAvg = 0;

            if (marketCapDetails == null)
                throw new ArgumentNullException(GreenfieldConstants.MARKET_CAPITALIZATION);

            //Calculate total benchmark weight for Benchmark
            if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                totalBenchmarkWeight = marketCapDetails.Sum(a => a.BenchmarkWeight);

            foreach (MarketCapitalizationData _mktCapData in marketCapDetails)
            {
                //if (!(string.IsNullOrEmpty(_mktCapData.Benchmark_ID)) && (_mktCapData.MarketCapitalInUSD != 0 && _mktCapData.MarketCapitalInUSD != null))
                if (_mktCapData.MarketCapitalInUSD != 0 && _mktCapData.MarketCapitalInUSD != null)
                {
                    if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                        benchmarkWtdAvg = benchmarkWtdAvg + Convert.ToDecimal(_mktCapData.BenchmarkWeight / totalBenchmarkWeight * _mktCapData.MarketCapitalInUSD);
                    else
                        benchmarkWtdAvg = benchmarkWtdAvg + Convert.ToDecimal(_mktCapData.BenchmarkWeight * _mktCapData.MarketCapitalInUSD);
                }
            }
            return benchmarkWtdAvg;
        }

        /// <summary>
        /// Calculates Weighted Median for Portfolio
        /// </summary>
        /// <param name="marketCapitalizationData"></param>
        /// <returns>_portfolioWeightdMedian</returns>

        public static decimal CalculatePortfolioWeightedMedian(List<MarketCapitalizationData> marketCapitalizationData)
        {
            List<MarketCapitalizationData> mktCapDetails = new List<MarketCapitalizationData>();
            decimal portfolioWeightedMedian = 0;
            decimal fiftyPercTotalMV = 0.50M;
            decimal totalPortfolioMV = 0;
            decimal sumPortfolioWeight = 0;

            if (marketCapitalizationData == null)
                throw new ArgumentNullException(GreenfieldConstants.MARKET_CAPITALIZATION);

            //Arrange list in ascending order by "MarketCapitalInUSD"
            mktCapDetails = marketCapitalizationData
                //.Where(list => list.SecurityThemeCode != GreenfieldConstants.CASH)
                                        .OrderBy(list => list.MarketCapitalInUSD).ToList();

            //Calculate total Market Value(DirtyValue_PC) for portfolio
            totalPortfolioMV = Convert.ToDecimal(mktCapDetails.Sum(a => a.PortfolioDirtyValuePC));

            //Calculate fifty percent of total market value(dirty value ex-cash)
            //foreach (MarketCapitalizationData _mktCap in mktCapDetails)
            //{
            //    //if (!string.IsNullOrEmpty(_mktCap.Portfolio_ID))
            //        fiftyPercTotalMV = fiftyPercTotalMV + Convert.ToDecimal(_mktCap.PortfolioDirtyValuePC / totalPortfolioMV);
            //}
            //fiftyPercTotalMV = fiftyPercTotalMV / 2;

            for (int _index = 0; _index < mktCapDetails.Count; _index++)
            {
                sumPortfolioWeight = sumPortfolioWeight + Convert.ToDecimal(mktCapDetails[_index].PortfolioDirtyValuePC / totalPortfolioMV);
                if (sumPortfolioWeight == fiftyPercTotalMV)
                {
                    portfolioWeightedMedian = Convert.ToDecimal(mktCapDetails[_index].MarketCapitalInUSD);
                    break;
                }
                else if (sumPortfolioWeight > fiftyPercTotalMV)
                {
                    if (_index > 0)
                        portfolioWeightedMedian = Convert.ToDecimal(mktCapDetails[_index - 1].MarketCapitalInUSD + mktCapDetails[_index].MarketCapitalInUSD) / 2;
                    else
                        portfolioWeightedMedian = Convert.ToDecimal(mktCapDetails[_index].MarketCapitalInUSD);
                    break;
                }
            }
            return portfolioWeightedMedian;
        }
        /// <summary>
        /// Calculates Weighted Median for Benchmark
        /// </summary>
        /// <param name="marketCapitalizationData"></param>
        /// <returns>_BenchmarkWeightdMedian</returns>

        public static decimal CalculateBenchmarkWeightedMedian(List<MarketCapitalizationData> marketCapitalizationData, String filterType, String filterValue, bool isExCashSecurity)
        {
            List<MarketCapitalizationData> mktCapDetails = new List<MarketCapitalizationData>();
            decimal fiftyPercTotalBenchWt = 0.50M;
            decimal benchmarkWtdMedian = 0;
            decimal sumBenchmarkWeight = 0;
            decimal totalBenchmarkWt = 0;
            if (marketCapitalizationData == null)
                throw new ArgumentNullException(GreenfieldConstants.MARKET_CAPITALIZATION);

            //Arrange list in ascending order by "MarketCapitalInUSD"
            mktCapDetails = marketCapitalizationData
                //.Where(list => list.SecurityThemeCode !=GreenfieldConstants.CASH)
                                        .OrderBy(list => list.MarketCapitalInUSD).ToList();

            //Calculate total Benchmark weight(Ex-Cash)
            if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                totalBenchmarkWt = Convert.ToDecimal(mktCapDetails.Sum(a => a.BenchmarkWeight));

            //fiftyPercTotalBenchWt = fiftyPercTotalBenchWt / 2;
            for (int _index = 0; _index < mktCapDetails.Count; _index++)
            {
                if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                    sumBenchmarkWeight = sumBenchmarkWeight + Convert.ToDecimal(mktCapDetails[_index].BenchmarkWeight / totalBenchmarkWt);
                else
                    sumBenchmarkWeight = sumBenchmarkWeight + Convert.ToDecimal(mktCapDetails[_index].BenchmarkWeight);
                if (sumBenchmarkWeight == fiftyPercTotalBenchWt)
                {
                    benchmarkWtdMedian = Convert.ToDecimal(mktCapDetails[_index].MarketCapitalInUSD);
                    break;
                }
                else if (sumBenchmarkWeight > fiftyPercTotalBenchWt)
                {
                    if (_index > 0)
                        benchmarkWtdMedian = Convert.ToDecimal(mktCapDetails[_index - 1].MarketCapitalInUSD + mktCapDetails[_index].MarketCapitalInUSD) / 2;
                    else
                        benchmarkWtdMedian = Convert.ToDecimal(mktCapDetails[_index].MarketCapitalInUSD);
                    break;
                }
            }
            return benchmarkWtdMedian;
        }

        /// <summary>
        /// To Calculate Sum of weights depending upon the value of MarketCapitalInUSD for Portfolio
        /// </summary>
        /// <param name="marketCapiltalizationData"></param>
        /// <param name="prtRangeForUndefinedMktCap"></param>
        /// <param name="prtRangeForMicroMktCap"></param>
        /// <param name="prtRangeForSmallMktCap"></param>
        /// <param name="prtRangeForMediumMktCap"></param>
        /// <param name="prtRangeForLargeMktCap"></param>
        /// <param name="prtRangeForMegaMktCap"></param>
        /// <returns>_portfolioSumRange</returns>
        public static List<MarketCapitalizationData> CalculateSumPortfolioRanges(List<MarketCapitalizationData> marketCapiltalizationData)
        {
            List<MarketCapitalizationData> portfolioSumRange = new List<MarketCapitalizationData>();
            List<MarketCapitalizationData> mktCapRanges = new List<MarketCapitalizationData>();
            MarketCapitalizationData mktCapData = new MarketCapitalizationData();

            //Calculate total Market Value(DirtyValue_PC) for portfolio
            decimal mktCapTotalDirtyValue = Convert.ToDecimal(marketCapiltalizationData.Sum(a => a.PortfolioDirtyValuePC));

            //Getting the lower and upper limit's values for all the Ranges.
            //These values are same for Portfolio and Benchmark. Hence, We are taking these values from Portfolio list not from Benchmark

            mktCapRanges = GetRangeLimit();
            mktCapData.LargeRange = mktCapRanges[0].LargeRange;
            mktCapData.MediumRange = mktCapRanges[0].MediumRange;
            mktCapData.SmallRange = mktCapRanges[0].SmallRange;
            mktCapData.MicroRange = mktCapRanges[0].MicroRange;
            mktCapData.UndefinedRange = mktCapRanges[0].UndefinedRange;
            portfolioSumRange.Add(mktCapData);

            //Calcualting sum for different ranges
            foreach (MarketCapitalizationData mktCap in marketCapiltalizationData)
            {
                ////if (!string.IsNullOrEmpty(mktCap.Portfolio_ID))
                ////{
                if (mktCap.MarketCapitalInUSD > mktCapRanges[0].LargeRange)
                    portfolioSumRange[0].PortfolioSumMegaRange = portfolioSumRange[0].PortfolioSumMegaRange + (Convert.ToDecimal(mktCap.PortfolioDirtyValuePC / mktCapTotalDirtyValue) * 100);

                else if (mktCap.MarketCapitalInUSD >= mktCapRanges[0].MediumRange && mktCap.MarketCapitalInUSD <= mktCapRanges[0].LargeRange)
                    portfolioSumRange[0].PortfolioSumLargeRange = portfolioSumRange[0].PortfolioSumLargeRange + (Convert.ToDecimal(mktCap.PortfolioDirtyValuePC / mktCapTotalDirtyValue) * 100);

                else if (mktCap.MarketCapitalInUSD >= mktCapRanges[0].SmallRange && mktCap.MarketCapitalInUSD < mktCapRanges[0].MediumRange)
                    portfolioSumRange[0].PortfolioSumMediumRange = portfolioSumRange[0].PortfolioSumMediumRange + (Convert.ToDecimal(mktCap.PortfolioDirtyValuePC / mktCapTotalDirtyValue) * 100);

                else if (mktCap.MarketCapitalInUSD >= mktCapRanges[0].MicroRange && mktCap.MarketCapitalInUSD < mktCapRanges[0].SmallRange)
                    portfolioSumRange[0].PortfolioSumSmallRange = portfolioSumRange[0].PortfolioSumSmallRange + (Convert.ToDecimal(mktCap.PortfolioDirtyValuePC / mktCapTotalDirtyValue) * 100);

                else if (mktCap.MarketCapitalInUSD > mktCapRanges[0].UndefinedRange && mktCap.MarketCapitalInUSD < mktCapRanges[0].MicroRange)
                    portfolioSumRange[0].PortfolioSumMicroRange = portfolioSumRange[0].PortfolioSumMicroRange + (Convert.ToDecimal(mktCap.PortfolioDirtyValuePC / mktCapTotalDirtyValue) * 100);

                else if (mktCap.MarketCapitalInUSD == mktCapRanges[0].UndefinedRange || mktCap.MarketCapitalInUSD == null)
                    portfolioSumRange[0].PortfolioSumUndefinedRange = portfolioSumRange[0].PortfolioSumUndefinedRange + (Convert.ToDecimal(mktCap.PortfolioDirtyValuePC / mktCapTotalDirtyValue) * 100);
                //}
            }
            return portfolioSumRange;
        }
        /// <summary>
        ///  To Calculate Sum of weights depending upon the value of MarketCapitalInUSD for Benchmark
        /// </summary>
        /// <param name="marketCapiltalizationData"></param>
        /// <returns>_benchmarkSumRange</returns>
        public static List<MarketCapitalizationData> CalculateSumBenchmarkRanges(List<MarketCapitalizationData> marketCapiltalizationData, String filterType, String filterValue, bool isExCashSecurity)
        {
            List<MarketCapitalizationData> benchmarkSumRange = new List<MarketCapitalizationData>();
            List<MarketCapitalizationData> mktCapRanges = new List<MarketCapitalizationData>();
            MarketCapitalizationData mktCap = new MarketCapitalizationData();
            decimal mktCapTotalBenchmarkWt = 0;

            //Calculate total Market Value(DirtyValue_PC) for portfolio
            if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                mktCapTotalBenchmarkWt = Convert.ToDecimal(marketCapiltalizationData.Sum(a => a.BenchmarkWeight));

            //Getting the lower and upper limit's values for all the Ranges

            mktCapRanges = GetRangeLimit();

            mktCap.LargeRange = mktCapRanges[0].LargeRange;
            mktCap.MediumRange = mktCapRanges[0].MediumRange;
            mktCap.SmallRange = mktCapRanges[0].SmallRange;
            mktCap.MicroRange = mktCapRanges[0].MicroRange;
            mktCap.UndefinedRange = mktCapRanges[0].UndefinedRange;
            benchmarkSumRange.Add(mktCap);


            //Calcualting sum for different ranges
            foreach (MarketCapitalizationData mktCapData in marketCapiltalizationData)
            {
                //if (!string.IsNullOrEmpty(mktCap.Benchmark_ID))
                //{
                if (mktCapData.MarketCapitalInUSD > mktCapRanges[0].LargeRange)
                {
                    if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                        benchmarkSumRange[0].BenchmarkSumMegaRange = benchmarkSumRange[0].BenchmarkSumMegaRange + ((Convert.ToDecimal(mktCapData.BenchmarkWeight) / mktCapTotalBenchmarkWt) * 100);
                    else
                        benchmarkSumRange[0].BenchmarkSumMegaRange = benchmarkSumRange[0].BenchmarkSumMegaRange + (Convert.ToDecimal(mktCapData.BenchmarkWeight) * 100);
                }
                else if (mktCapData.MarketCapitalInUSD >= mktCapRanges[0].MediumRange && mktCapData.MarketCapitalInUSD <= mktCapRanges[0].LargeRange)
                {
                    if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                        benchmarkSumRange[0].BenchmarkSumLargeRange = benchmarkSumRange[0].BenchmarkSumLargeRange + ((Convert.ToDecimal(mktCapData.BenchmarkWeight) / mktCapTotalBenchmarkWt) * 100);
                    else
                        benchmarkSumRange[0].BenchmarkSumLargeRange = benchmarkSumRange[0].BenchmarkSumLargeRange + (Convert.ToDecimal(mktCapData.BenchmarkWeight) * 100);
                }
                else if (mktCapData.MarketCapitalInUSD >= mktCapRanges[0].SmallRange && mktCapData.MarketCapitalInUSD < mktCapRanges[0].MediumRange)
                {
                    if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                        benchmarkSumRange[0].BenchmarkSumMediumRange = benchmarkSumRange[0].BenchmarkSumMediumRange + ((Convert.ToDecimal(mktCapData.BenchmarkWeight) / mktCapTotalBenchmarkWt) * 100);
                    else
                        benchmarkSumRange[0].BenchmarkSumMediumRange = benchmarkSumRange[0].BenchmarkSumMediumRange + (Convert.ToDecimal(mktCapData.BenchmarkWeight) * 100);// / mktCapTotalBenchmarkWt);
                }
                else if (mktCapData.MarketCapitalInUSD >= mktCapRanges[0].MicroRange && mktCapData.MarketCapitalInUSD < mktCapRanges[0].SmallRange)
                {
                    if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                        benchmarkSumRange[0].BenchmarkSumSmallRange = benchmarkSumRange[0].BenchmarkSumSmallRange + ((Convert.ToDecimal(mktCapData.BenchmarkWeight) / mktCapTotalBenchmarkWt) * 100);
                    else
                        benchmarkSumRange[0].BenchmarkSumSmallRange = benchmarkSumRange[0].BenchmarkSumSmallRange + (Convert.ToDecimal(mktCapData.BenchmarkWeight) * 100);// mktCapTotalBenchmarkWt);
                }
                else if (mktCapData.MarketCapitalInUSD > mktCapRanges[0].UndefinedRange && mktCapData.MarketCapitalInUSD < mktCapRanges[0].MicroRange)
                {
                    if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                        benchmarkSumRange[0].BenchmarkSumMicroRange = benchmarkSumRange[0].BenchmarkSumMicroRange + ((Convert.ToDecimal(mktCapData.BenchmarkWeight) / mktCapTotalBenchmarkWt) * 100);
                    else
                        benchmarkSumRange[0].BenchmarkSumMicroRange = benchmarkSumRange[0].BenchmarkSumMicroRange + (Convert.ToDecimal(mktCapData.BenchmarkWeight) * 100);  // / mktCapTotalBenchmarkWt);
                }
                else if (mktCapData.MarketCapitalInUSD == mktCapRanges[0].UndefinedRange || mktCap.MarketCapitalInUSD == null)
                {
                    if ((filterType == null || filterType == "Show Everything") && isExCashSecurity != true)
                        benchmarkSumRange[0].BenchmarkSumUndefinedRange = benchmarkSumRange[0].BenchmarkSumUndefinedRange + ((Convert.ToDecimal(mktCapData.BenchmarkWeight) / mktCapTotalBenchmarkWt) * 100);
                    else
                        benchmarkSumRange[0].BenchmarkSumUndefinedRange = benchmarkSumRange[0].BenchmarkSumUndefinedRange + (Convert.ToDecimal(mktCapData.BenchmarkWeight) * 100);// / mktCapTotalBenchmarkWt);
                }
            }
            return benchmarkSumRange;
        }

        /// <summary>
        /// Get limit values for all ranges from web.config
        /// </summary>
        /// <returns>_rangeLimit</returns>
        public static List<MarketCapitalizationData> GetRangeLimit()
        {

            List<MarketCapitalizationData> _rangeLimit = new List<MarketCapitalizationData>();
            MarketCapitalizationData _mktCapData = new MarketCapitalizationData();

            string[] _undefRanges = ConfigurationManager.AppSettings[GreenfieldConstants.UNDEFINED_RANGE].Split(',');

            //Check if range value is null or blank then consider it as UndefinedRange
            if (_undefRanges.Contains(ConfigurationManager.AppSettings[GreenfieldConstants.NULL_VAL]) || _undefRanges.Contains(ConfigurationManager.AppSettings[GreenfieldConstants.BLANK_VAL]) || _undefRanges.Contains(ConfigurationManager.AppSettings[GreenfieldConstants.DECIMAL_DEF_VAL]))
                _mktCapData.UndefinedRange = 0;// Convert.ToDecimal(GreenfieldConstants.DECIMAL_DEF_VAL);

            _mktCapData.LargeRange = Convert.ToDecimal(ConfigurationManager.AppSettings[GreenfieldConstants.LARGE_RANGE]);
            _mktCapData.MediumRange = Convert.ToDecimal(ConfigurationManager.AppSettings[GreenfieldConstants.MEDIUM_RANGE]);
            _mktCapData.SmallRange = Convert.ToDecimal(ConfigurationManager.AppSettings[GreenfieldConstants.SMALL_RANGE]);
            _mktCapData.MicroRange = Convert.ToDecimal(ConfigurationManager.AppSettings[GreenfieldConstants.MICRO_RANGE]);

            _rangeLimit.Add(_mktCapData);
            return _rangeLimit;
        }

    }
}