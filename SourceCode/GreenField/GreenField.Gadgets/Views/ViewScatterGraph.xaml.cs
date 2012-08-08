﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GreenField.Gadgets.ViewModels;
using GreenField.Gadgets.Helpers;
using GreenField.ServiceCaller.ExternalResearchDefinitions;
using GreenField.Common;
using GreenField.DataContracts;
using Telerik.Windows.Controls.Charting;

namespace GreenField.Gadgets.Views
{
    public partial class ViewScatterGraph : ViewBaseUserControl
    {
        ViewModelScatterGraph _dataContextSource;

        public ViewScatterGraph(ViewModelScatterGraph dataContextSource)
        {
            InitializeComponent();
            this.DataContext = dataContextSource;
            _dataContextSource = dataContextSource;
        }

        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFlip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgScatterGraph_ElementExporting(object sender, Telerik.Windows.Controls.GridViewElementExportingEventArgs e)
        {

        }

        private void dgScatterGraph_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {

        }

        private void ChartArea_ItemToolTipOpening(ItemToolTip2D tooltip, ItemToolTipEventArgs e)
        {
            RatioComparisonData dataPointContext = e.DataPoint.DataItem as RatioComparisonData;
            if(dataPointContext == null)
            {
                tooltip.Content = null;
                return;
            }            

            tooltip.Content = dataPointContext.ISSUE_NAME + " (" 
                + EnumUtils.GetDescriptionFromEnumValue<ScatterGraphValuationRatio>(_dataContextSource.SelectedValuationRatio) + ":" + dataPointContext.VALUATION + ", "
                + EnumUtils.GetDescriptionFromEnumValue<ScatterGraphFinancialRatio>(_dataContextSource.SelectedFinancialRatio) + ":" + dataPointContext.FINANCIAL + ")";

        }

        private void chScatter_Loaded(object sender, RoutedEventArgs e)
        {
            //this.chScatter.DefaultView.ChartArea.AxisX.la = EnumUtils.GetDescriptionFromEnumValue<ScatterGraphValuationRatio>(_dataContextSource.SelectedValuationRatio);
            //this.chScatter.DefaultView.ChartArea.AxisY.AxisName = EnumUtils.GetDescriptionFromEnumValue<ScatterGraphFinancialRatio>(_dataContextSource.SelectedFinancialRatio);
        }

        private void chScatter_DataBound(object sender, ChartDataBoundEventArgs e)
        {
            if (_dataContextSource.RatioComparisonInfo != null)
            {
                this.chScatter.DefaultView.ChartArea.Annotations.Clear();

                if (_dataContextSource.RatioComparisonInfo.Count() != 0)
                {
                    Decimal? financialRatioTotal = _dataContextSource.RatioComparisonInfo.Sum(record => record.FINANCIAL);
                    Decimal? financialRatioAverage = financialRatioTotal / _dataContextSource.RatioComparisonInfo.Count();

                    Decimal? valuationRatioTotal = _dataContextSource.RatioComparisonInfo.Sum(record => record.VALUATION);
                    Decimal? valuationRatioAverage = valuationRatioTotal / _dataContextSource.RatioComparisonInfo.Count();

                    this.chaScatter.Annotations.Add(new CustomGridLine() { XIntercept = Convert.ToDouble(valuationRatioAverage), StrokeThickness = 1 });
                    this.chaScatter.Annotations.Add(new CustomGridLine() { YIntercept = Convert.ToDouble(financialRatioAverage), StrokeThickness = 1 });

                    //RatioComparisonData issuerRecord = _dataContextSource.RatioComparisonInfo.Where(record => record.ISSUE_NAME == _dataContextSource.EntitySelectionInfo.LongName
                    //    && record.ISSUER_ID == _dataContextSource.IssuerReferenceInfo.IssuerId).FirstOrDefault();
                    

                }
            }

            

            //RatioComparisonData issuerRecord = _dataContextSource.RatioComparisonInfo.Where(record => record.ISSUE_NAME == "SampleIssueName15").FirstOrDefault();
            //DataPoint point = new DataPoint() 
            //{
            //    XValue = Convert.ToDouble(issuerRecord.VALUATION), 
            //    YValue = Convert.ToDouble(issuerRecord.FINANCIAL),                
            //};
            
            //this.chScatter.DefaultView.ChartArea.Annotations.Add(new CustomLine()
            //{
            //    MinX = Convert.ToDouble(issuerRecord.VALUATION) - 0.5,
            //    MinY = Convert.ToDouble(issuerRecord.FINANCIAL) - 0.5,
            //    MaxX = Convert.ToDouble(issuerRecord.VALUATION) + 0.5,
            //    MaxY = Convert.ToDouble(issuerRecord.FINANCIAL) + 0.5,
            //    Stroke = new SolidColorBrush(Color.FromArgb(255, 159, 29, 33)),
            //    StrokeThickness = 1,
            //    Slope = Convert.ToDouble(issuerRecord.FINANCIAL) / Convert.ToDouble(issuerRecord.VALUATION),
            //    YIntercept = 0
            //});


            //ScatterSeriesDefinition scatterSeriesDefinition = new ScatterSeriesDefinition() { ShowItemLabels = false, ShowItemToolTips = false, LegendDisplayMode = LegendDisplayMode.None };
            //ItemMapping xItemMapping = new ItemMapping() { FieldName = "FINANCIAL", DataPointMember = DataPointMember.XValue };
            //ItemMapping yItemMapping = new ItemMapping() { FieldName = "VALUATION", DataPointMember = DataPointMember.YValue };
            //SeriesMapping selectedItemSeriesMapping = new SeriesMapping();
            //selectedItemSeriesMapping.SeriesDefinition = scatterSeriesDefinition;
            //selectedItemSeriesMapping.ItemMappings.Add(xItemMapping);
            //selectedItemSeriesMapping.ItemMappings.Add(yItemMapping);
            //selectedItemSeriesMapping.ItemsSource = issuerRecord;

            //this.chScatter.SeriesMappings.Add(selectedItemSeriesMapping);

        }

        

        
    }
}
