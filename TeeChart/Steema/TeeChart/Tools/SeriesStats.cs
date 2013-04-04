namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(SeriesStats), "ToolsIcons.SeriesStats.bmp"), Description("Statistics")]
    public class SeriesStats : ToolSeries
    {
        public SeriesStats() : this(null)
        {
        }

        public SeriesStats(Chart c) : base(c)
        {
        }

        private string AddFunction(Type f, string desc)
        {
            double num = (Activator.CreateInstance(f) as Function).Calculate(base.Series, -1, -1);
            return (desc + num.ToString(base.Series.ValueFormat));
        }

        private string AtPosition(double value)
        {
            int index = base.Series.mandatory.IndexOf(value);
            if (index != -1)
            {
                return (" at position " + index.ToString());
            }
            return "";
        }

        private string CalcStats()
        {
            string str = "";
            if (base.Series != null)
            {
                string valueFormat = base.Series.ValueFormat;
                string str3 = str + "Number of values:\t" + base.Series.Count.ToString() + "\r\n";
                string str4 = str3 + "Maximum value:\t" + base.Series.mandatory.Maximum.ToString(valueFormat) + this.AtPosition(base.Series.mandatory.Maximum) + "\r\n";
                str = (str4 + "Minimum value:\t" + base.Series.mandatory.Minimum.ToString(valueFormat) + this.AtPosition(base.Series.mandatory.Minimum) + "\r\n") + "Range:\t" + base.Series.mandatory.Range.ToString(valueFormat) + "\r\n";
                str = ((((((str + "Average:\t" + ((base.Series.Count > 0) ? (base.Series.mandatory.Total / ((double) base.Series.Count)) : 0.0).ToString(valueFormat) + "\r\n\r\n") + this.AddFunction(typeof(MedianFunction), "Median:\t") + "\r\n") + this.AddFunction(typeof(ModeFunction), "Mode:\t") + "\r\n") + this.AddFunction(typeof(StdDeviation), "Std. deviation:\t") + "\r\n") + this.AddFunction(typeof(VarianceFunction), "Variance:\t") + "\r\n") + this.AddFunction(typeof(RootMeanSquare), "Root mean square:\t") + "\r\n") + this.AddFunction(typeof(CorrelationFunction), "Correlation:\t") + "\r\n";
            }
            return str;
        }

        internal Function StatFunction(Type ftype)
        {
            Function function = null;
            foreach (Series series in base.Chart.Series)
            {
                function = series.Function;
                if (((function != null) && ftype.Equals(function.GetType())) && (function.Series.DataSource == base.Series))
                {
                    return function;
                }
            }
            return null;
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.SeriesStatsTool;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Statistics
        {
            get
            {
                return this.CalcStats();
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.SeriesStatsToolSummary;
            }
        }
    }
}

