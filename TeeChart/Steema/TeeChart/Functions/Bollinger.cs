namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class Bollinger : Function
    {
        private ValueList AList;
        private double deviation;
        private bool exponential;
        private FastLine iOther;

        public Bollinger() : this((Chart) null)
        {
        }

        public Bollinger(Chart c) : base(c)
        {
            base.SingleSource = true;
            this.Exponential = true;
            this.Deviation = 2.0;
            base.dPeriod = 10.0;
        }

        public Bollinger(FastLine lowBand) : this((Chart) null)
        {
            this.LowBand = lowBand;
            this.LowBand.Title = "LowBand";
            this.HideSeries(this.LowBand);
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                if (series.Count > 0)
                {
                    base.Series.Clear();
                    this.PrepareSeries(this.LowBand);
                    if (base.Period > 0.0)
                    {
                        this.InternalAddPoints(base.Series, this.Deviation, series);
                        this.InternalAddPoints(this.iOther, -this.Deviation, series);
                    }
                }
            }
        }

        public override void Clear()
        {
            if (this.iOther != null)
            {
                this.iOther.Clear();
            }
            base.Clear();
        }

        public override string Description()
        {
            return Texts.FunctionBollinger;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.iOther != null))
            {
                this.iOther.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InternalAddPoints(Series ASeries, double ADeviation, Series Source)
        {
            Function function;
            if (this.Exponential)
            {
                function = new ExpMovAverage();
            }
            else
            {
                function = new MovingAverage();
            }
            function.Period = base.Period;
            Series series = new Series(null) {
                DataSource = Source
            };
            string valueSource = base.Series.mandatory.valueSource;
            if (valueSource == "")
            {
                valueSource = Source.mandatory.Name;
            }
            series.mandatory.valueSource = valueSource;
            series.Function = function;
            ASeries.Clear();
            this.AList = Source.GetYValueList(valueSource);
            int num2 = Utils.Round(base.Period);
            for (int i = num2; i <= Source.Count; i++)
            {
                double y = ADeviation * this.StdDev(i - num2, i - 1);
                if (this.Exponential)
                {
                    y = series.YValues[i - 1] + y;
                }
                else
                {
                    y = series.YValues[i - num2] + y;
                }
                ASeries.Add(Source.XValues[i - 1], y);
            }
            series.DataSource = null;
        }

        private double StdDev(int First, int Last)
        {
            double num = 0.0;
            double num2 = 0.0;
            for (int i = First; i <= Last; i++)
            {
                double num3 = this.AList[i];
                num += num3;
                num2 += Utils.Sqr(num3);
            }
            num = ((base.Period * num2) - Utils.Sqr(num)) / Utils.Sqr(base.Period);
            if (num > 0.0)
            {
                return Math.Sqrt(num);
            }
            return 0.0;
        }

        public double Deviation
        {
            get
            {
                return this.deviation;
            }
            set
            {
                base.SetDoubleProperty(ref this.deviation, value);
                base.Recalculate();
            }
        }

        [DefaultValue(true)]
        public bool Exponential
        {
            get
            {
                return this.exponential;
            }
            set
            {
                base.SetBooleanProperty(ref this.exponential, value);
                base.Recalculate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FastLine LowBand
        {
            get
            {
                if (this.iOther == null)
                {
                    this.iOther = new FastLine();
                    this.HideSeries(this.iOther);
                }
                return this.iOther;
            }
            set
            {
                this.iOther = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartPen LowBandPen
        {
            get
            {
                return this.LowBand.LinePen;
            }
            set
            {
                this.LowBand.LinePen = value;
                if (this.LowBand.Visible != value.Visible)
                {
                    this.LowBand.Visible = value.Visible;
                }
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen UpperBandPen
        {
            get
            {
                if (base.Series is BaseLine)
                {
                    return (base.Series as BaseLine).LinePen;
                }
                return null;
            }
            set
            {
                if (base.Series is BaseLine)
                {
                    (base.Series as BaseLine).LinePen = value;
                }
                if (base.Series.Visible != value.Visible)
                {
                    base.Series.Visible = value.Visible;
                }
                this.Invalidate();
            }
        }
    }
}

