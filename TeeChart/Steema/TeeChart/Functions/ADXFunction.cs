namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class ADXFunction : Function
    {
        private FastLine iDMDown;
        private FastLine iDMUp;

        public ADXFunction() : this(null)
        {
        }

        public ADXFunction(Chart c) : base(c)
        {
            base.dPeriod = 14.0;
            base.SingleSource = true;
            base.HideSourceList = true;
        }

        public ADXFunction(FastLine dmDown, FastLine dmUp) : this(null)
        {
            this.DMDown = dmDown;
            this.DMDown.Title = "DMDown";
            this.DMUp = dmUp;
            this.DMUp.Title = "DMUp";
            this.HideSeries(this.DMDown);
            this.HideSeries(this.DMUp);
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                if ((series.Count > 0) && (base.Period >= 2.0))
                {
                    base.Series.Clear();
                    this.PrepareSeries(this.DMUp);
                    this.PrepareSeries(this.DMDown);
                    if (series.Count >= (2.0 * base.Period))
                    {
                        ValueList closeValues = (series as OHLC).CloseValues;
                        ValueList highValues = (series as OHLC).HighValues;
                        ValueList lowValues = (series as OHLC).LowValues;
                        double[] numArray = new double[series.Count];
                        double[] numArray2 = new double[series.Count];
                        double[] numArray3 = new double[series.Count];
                        for (int i = 1; i < series.Count; i++)
                        {
                            double num = closeValues[i - 1];
                            numArray[i] = highValues[i] - lowValues[i];
                            numArray[i] = Math.Max(numArray[i], Math.Abs((double) (highValues[i] - num)));
                            numArray[i] = Math.Max(numArray[i], Math.Abs((double) (lowValues[i] - num)));
                            if ((highValues[i] - highValues[i - 1]) > (lowValues[i - 1] - lowValues[i]))
                            {
                                numArray2[i] = Math.Max((double) 0.0, (double) (highValues[i] - highValues[i - 1]));
                            }
                            else
                            {
                                numArray2[i] = 0.0;
                            }
                            if ((lowValues.Value[i - 1] - lowValues.Value[i]) > (highValues.Value[i] - highValues.Value[i - 1]))
                            {
                                numArray3[i] = Math.Max((double) 0.0, (double) (lowValues[i - 1] - lowValues[i]));
                            }
                            else
                            {
                                numArray3[i] = 0.0;
                            }
                        }
                        double num4 = 0.0;
                        double num5 = 0.0;
                        double num6 = 0.0;
                        int num7 = Utils.Round(base.Period);
                        for (int j = num7; j < series.Count; j++)
                        {
                            if (j == num7)
                            {
                                for (int m = 1; m <= Utils.Round(base.Period); m++)
                                {
                                    num4 += numArray[m];
                                    num5 += numArray2[m];
                                    num6 += numArray3[m];
                                }
                            }
                            else
                            {
                                num4 = (num4 - (num4 / base.Period)) + numArray[j];
                                num5 = (num5 - (num5 / base.Period)) + numArray2[j];
                                num6 = (num6 - (num6 / base.Period)) + numArray3[j];
                            }
                            double x = series.XValues[j];
                            this.DMUp.Add(x, (double) (100.0 * (num5 / num4)));
                            this.DMDown.Add(x, (double) (100.0 * (num6 / num4)));
                        }
                        numArray = null;
                        numArray2 = null;
                        numArray3 = null;
                        double y = 0.0;
                        num7 = Utils.Round((double) ((2.0 * base.Period) - 2.0));
                        for (int k = num7; k < series.Count; k++)
                        {
                            if (k == num7)
                            {
                                y = 0.0;
                                for (int n = Utils.Round(base.Period); n <= num7; n++)
                                {
                                    y += this.CalcADX(n);
                                }
                                y /= base.Period - 1.0;
                            }
                            else
                            {
                                y = ((y * (base.Period - 1.0)) + this.CalcADX(k)) / base.Period;
                            }
                            base.Series.Add(series.XValues[k], y);
                        }
                    }
                }
            }
        }

        private double CalcADX(int Index)
        {
            int num2 = Index - Utils.Round(base.Period);
            return ((100.0 * Math.Abs((double) (this.DMUp.YValues[num2] - this.DMDown.YValues[num2]))) / (this.DMUp.YValues[num2] + this.DMDown.YValues[num2]));
        }

        public override void Clear()
        {
            if (this.iDMDown != null)
            {
                this.iDMDown.Clear();
            }
            if (this.iDMUp != null)
            {
                this.iDMUp.Clear();
            }
            base.Clear();
        }

        public override string Description()
        {
            return Texts.FunctionADX;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.iDMDown != null)
                {
                    this.iDMDown.Dispose();
                    this.iDMDown = null;
                }
                if (this.iDMUp != null)
                {
                    this.iDMUp.Dispose();
                    this.iDMUp = null;
                }
            }
            base.Dispose(disposing);
        }

        private FastLine GetDMDown()
        {
            if (this.iDMDown == null)
            {
                this.iDMDown = new FastLine();
                this.iDMDown.Title = "DMDown";
                this.HideSeries(this.iDMDown);
            }
            return this.iDMDown;
        }

        private FastLine GetDMUp()
        {
            if (this.iDMUp == null)
            {
                this.iDMUp = new FastLine();
                this.iDMUp.Title = "DMUp";
                this.HideSeries(this.iDMUp);
            }
            return this.iDMUp;
        }

        internal override bool IsValidSource(Series Value)
        {
            return (Value is OHLC);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FastLine DMDown
        {
            get
            {
                return this.GetDMDown();
            }
            set
            {
                this.iDMDown = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FastLine DMUp
        {
            get
            {
                return this.GetDMUp();
            }
            set
            {
                this.iDMUp = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartPen DownLinePen
        {
            get
            {
                return this.DMDown.LinePen;
            }
            set
            {
                this.DMDown.LinePen = value;
                if (this.DMDown.Visible != value.Visible)
                {
                    this.DMDown.Visible = value.Visible;
                }
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartPen UpLinePen
        {
            get
            {
                return this.DMUp.LinePen;
            }
            set
            {
                this.DMUp.LinePen = value;
                if (this.DMUp.Visible != value.Visible)
                {
                    this.DMUp.Visible = value.Visible;
                }
                this.Invalidate();
            }
        }
    }
}

