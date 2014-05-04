using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WealthLab;
using WealthLabPro;

namespace WLDIndicators
{
    //SMA Indicator class
    public class ExternalSymbolIndicator : DataSeries
    {
        //---*** Public interface ***---
        //Static Series method returns an instance of the indicator
        public static ExternalSymbolIndicator Series(string dsSymbol, DataSeries ds, int period)
        {
            //Build description
            string description = "ExternalSymbolIndicator("+ dsSymbol + "," + ds.Description + "," + period + ")";
            //See if it exists in the cache
            if (ds.Cache.ContainsKey(description))
                return (ExternalSymbolIndicator)ds.Cache[description];
            //Create SMA, cache it, return it
            ExternalSymbolIndicator sma = new ExternalSymbolIndicator(dsSymbol, ds, period, description);
            ds.Cache[description] = sma;

            return sma;
        }
        //This static method allows ad-hoc calculation of SMAs (single calc mode)
        public static double Value(int bar, DataSeries ds, int period)
        {
            if (ds.Count < period)
                return 0;
            else
            {
                double sum = 0;
                for (int i = bar; i > bar - period; i--)
                    sum += ds[i];
                return sum / period;
            }
        }
        //Constructor
        public ExternalSymbolIndicator(string dsSymbol, DataSeries ds, int period, string description)
            : base(ds, description)
        {
            WealthLab.BarsLoader barsLoader = new BarsLoader();

            string[] items = dsSymbol.Split(new string[]{"."}, StringSplitOptions.RemoveEmptyEntries);

            string dsName = items[0];
            string symbolName = items[1];

            DataSource datasource = MainModule.Instance.DataSources.FindDataSource(dsName);

            //barsLoader.Scale = datasource.Scale;
            //barsLoader.BarInterval = datasource.BarInterval;
            barsLoader.BarDataScale = datasource.BarDataScale;
            Bars symbolData = barsLoader.GetData(datasource, symbolName);
            DataSeries result = BarScaleConverter.Synchronize(symbolData.Close, ds);

            FirstValidValue = ds.FirstValidValue;

            for (int bar = 0; bar < ds.Count; bar++)
            {
                this[bar] = result[bar];
            }
            

            /*
            //Remember parameters
            _sourceSeries = ds;
            _period = period;
            //Assign first bar that contains indicator data
            FirstValidValue = period - 1 + ds.FirstValidValue;
            //Calculate moving average values
            
            for (int bar = period - 1; bar < ds.Count; bar++)
            {
                double sum = 0;
                for (int innerBar = bar; innerBar > bar - period; innerBar--)
                    sum += ds[innerBar];
                this[bar] = sum / period;
            }
             */
        }
        //Calculate a value for a partial bar
        public override void CalculatePartialValue()
        {
            if (_sourceSeries.Count < _period - 1 || _sourceSeries.PartialValue == Double.NaN)
                PartialValue = Double.NaN;
            else
            {
                double sum = 0;
                for (int bar = _sourceSeries.Count - _period + 1; bar < _sourceSeries.Count; bar++)
                    sum += _sourceSeries[bar];
                sum += _sourceSeries.PartialValue;
                PartialValue = sum / _period;
            }
        }
        //---*** Private members ***---
        DataSeries _sourceSeries;
        int _period;
    }


    //Helper class that allows SMA to work well in WL Pro
    public class ExternalSymbolIndicatorHelper : IndicatorHelper
    {
        //Return suggested default values
        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return _paramDefaults;
            }
        }
        //Return parameter descriptions
        public override IList<string> ParameterDescriptions
        {
            get
            {
                return _paramNames;
            }
        }
        //Return default color
        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }
        //Return a reference to supported indicator type
        public override Type IndicatorType
        {
            get
            {
                return typeof(ExternalSymbolIndicator);
            }
        }
        //Return a description of the indicator
        public override string Description
        {
            get
            {
                return @"The Simple Moving Average indicator calculates the average of a set of values over a specified period.";
            }
        }

        public override string TargetPane
        {
            get
            {
                return "ExternalSymbolIndicator";
            }
        }

        //Return a URL for more info
        public override string URL
        {
            get
            {
                return @"http://www.investopedia.com/terms/s/sma.asp";
            }
        }
        //Private members
        private static object[] _paramDefaults = { "ShiborVsYield_HS300.HS300_YIELD_SHIBOR_MINE", CoreDataSeries.Close, new RangeBoundInt32(20, 2, Int32.MaxValue) };
        private static string[] _paramNames = { "Symbol", "Source", "Period" };

    }
}
