namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class MACDFunction : Moving
    {
        private Volume iHisto;
        private ExpMovAverage iMoving1;
        private ExpMovAverage iMoving2;
        private ExpMovAverage iMoving3;
        private FastLine iOther;
        private Line iSeries1;
        private Line iSeries2;

        public MACDFunction() : this(null)
        {
            this.SetInternalSeries(true);
        }

        public MACDFunction(Chart c) : base(c)
        {
            base.SingleSource = true;
            this.SetInternalSeries(c != null);
        }

        public MACDFunction(FastLine macdExp, Volume histogram) : this(null)
        {
            this.MACDExp = macdExp;
            this.MACDExp.Title = "MACDExp";
            this.Histogram = histogram;
            this.Histogram.Title = "Histogram";
            this.HideSeries(this.MACDExp);
            this.HideSeries(this.Histogram);
            this.SetInternalSeries(true);
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                base.Series.Clear();
                Series series = (Series) source.GetValue(0);
                if (series.Count > 0)
                {
                    this.iMoving1.AddPoints(source);
                    this.iMoving2.Period = base.Period;
                    this.iMoving2.AddPoints(source);
                    this.iSeries1.CheckDataSource();
                    this.iSeries2.CheckDataSource();
                    for (int i = 0; i < series.Count; i++)
                    {
                        base.Series.Add(series.XValues.Value[i], (double) (this.iSeries1.YValues.Value[i] - this.iSeries2.YValues.Value[i]));
                    }
                    this.iSeries1.Clear();
                    this.iSeries2.Clear();
                    if (this.MACDExp.Function.Series == null)
                    {
                        this.MACDExp.Function = this.iMoving3;
                    }
                    this.PrepareSeries(this.MACDExp);
                    if (base.Series.Color == Color.White)
                    {
                        base.Series.Color = Color.Blue;
                    }
                    this.MACDExp.DataSource = base.Series;
                    this.MACDExp.CheckDataSource();
                    if (this.Histogram.Active)
                    {
                        this.PrepareSeries(this.Histogram);
                        this.Histogram.BeginUpdate();
                        this.Histogram.Clear();
                        for (int j = 0; j < this.MACDExp.Count; j++)
                        {
                            this.Histogram.Add(this.MACDExp.XValues[j], (double) (base.Series.YValues[j] - this.MACDExp.YValues[j]));
                        }
                        this.Histogram.EndUpdate();
                    }
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
            if (this.iOther != null)
            {
                this.iOther.Clear();
            }
            if (this.iHisto != null)
            {
                this.iHisto.Clear();
            }
        }

        public override string Description()
        {
            return Texts.FunctionMACD;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.iOther != null)
                {
                    this.iOther.Dispose();
                }
                if (this.iHisto != null)
                {
                    this.iHisto.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void SetInternalSeries(bool set)
        {
            if (set)
            {
                this.iMoving1 = new ExpMovAverage();
                this.iMoving1.InternalUse = true;
                this.iMoving1.Period = 12.0;
                this.iSeries1 = new Line();
                this.iSeries1.Function = this.iMoving1;
                this.iMoving2 = new ExpMovAverage();
                this.iMoving2.InternalUse = true;
                this.iMoving2.Period = 26.0;
                this.iSeries2 = new Line();
                this.iSeries2.Function = this.iMoving2;
                base.Period = this.iMoving2.Period;
                this.iMoving3 = new ExpMovAverage();
                this.iMoving3.InternalUse = true;
                this.iMoving3.Period = 9.0;
                this.MACDExp.Function = this.iMoving3;
                this.Histogram.UseOrigin = true;
                this.Histogram.Origin = 0.0;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Volume Histogram
        {
            get
            {
                if (this.iHisto == null)
                {
                    this.iHisto = new Volume();
                    this.iHisto.Title = "Histogram";
                    this.HideSeries(this.iHisto);
                }
                return this.iHisto;
            }
            set
            {
                this.iHisto = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartPen HistogramPen
        {
            get
            {
                return this.Histogram.LinePen;
            }
            set
            {
                this.Histogram.LinePen = value;
                if (this.Histogram.Visible != value.Visible)
                {
                    this.Histogram.Visible = value.Visible;
                }
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FastLine MACDExp
        {
            get
            {
                if (this.iOther == null)
                {
                    this.iOther = new FastLine();
                    this.iOther.Title = "MACDExp";
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
        public ChartPen MACDExpPen
        {
            get
            {
                return this.MACDExp.LinePen;
            }
            set
            {
                this.MACDExp.LinePen = value;
                if (this.MACDExp.Visible != value.Visible)
                {
                    this.MACDExp.Visible = value.Visible;
                }
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartPen MACDPen
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

        public double Period2
        {
            get
            {
                return this.iMoving1.Period;
            }
            set
            {
                if (this.iMoving1.Period != value)
                {
                    this.iMoving1.Period = value;
                    if (this.iMoving1.Period < 1.0)
                    {
                        this.iMoving1.Period = 1.0;
                    }
                    base.Recalculate();
                }
            }
        }

        public double Period3
        {
            get
            {
                return (double) Utils.Round(this.iOther.Function.Period);
            }
            set
            {
                this.iOther.Function.Period = value;
            }
        }
    }
}

