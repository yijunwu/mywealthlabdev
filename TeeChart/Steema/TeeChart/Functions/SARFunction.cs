namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class SARFunction : Function
    {
        private double AF;
        private double MS;

        public SARFunction() : this(null)
        {
        }

        public SARFunction(Chart c) : base(c)
        {
            this.AF = 0.019999999552965164;
            this.MS = 0.30000001192092896;
            base.CanUsePeriod = false;
            base.SingleSource = true;
        }

        public override void AddPoints(Array source)
        {
            if (!base.updating && (source != null))
            {
                base.Series.Clear();
                OHLC ohlc = source.GetValue(0) as OHLC;
                if (ohlc.Count > 2)
                {
                    double[] numArray = new double[ohlc.Count];
                    numArray[0] = ohlc.CloseValues[0];
                    numArray[1] = ohlc.HighValues[1];
                    bool flag = true;
                    double aF = this.AF;
                    double num3 = ohlc.LowValues[0];
                    double num4 = ohlc.HighValues[0];
                    double num5 = num3;
                    for (int i = 2; i < ohlc.Count; i++)
                    {
                        numArray[i] = flag ? (numArray[i - 1] + (aF * (num4 - numArray[i - 1]))) : (numArray[i - 1] + (aF * (num5 - numArray[i - 1])));
                        int num6 = 0;
                        if (flag)
                        {
                            if (ohlc.LowValues[i] < numArray[i])
                            {
                                flag = false;
                                num6 = 1;
                                numArray[i] = num4;
                                num5 = ohlc.LowValues[i];
                                aF = this.AF;
                            }
                        }
                        else if (ohlc.HighValues[i] > numArray[i])
                        {
                            flag = true;
                            num6 = 1;
                            numArray[i] = num5;
                            num4 = ohlc.HighValues[i];
                            aF = this.AF;
                        }
                        if (num6 == 0)
                        {
                            if (flag)
                            {
                                if (ohlc.HighValues[i] > num4)
                                {
                                    num4 = ohlc.HighValues[i];
                                    aF = this.Min(aF + this.AF, this.MS, this.MS);
                                }
                                numArray[i] = this.Min(numArray[i], ohlc.LowValues[i - 1], ohlc.LowValues[i - 2]);
                            }
                            else
                            {
                                if (ohlc.LowValues[i] < num5)
                                {
                                    num5 = ohlc.LowValues[i];
                                    aF = this.Min(aF + this.AF, this.MS, this.MS);
                                }
                                numArray[i] = this.Max(numArray[i], ohlc.HighValues[i - 1], ohlc.HighValues[i - 2]);
                            }
                        }
                    }
                    for (int j = 2; j < ohlc.Count; j++)
                    {
                        base.AddFunctionXY(ohlc.yMandatory, ohlc.notMandatory[j], numArray[j]);
                    }
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionSAR;
        }

        internal override bool IsValidSource(Series Value)
        {
            return (Value is OHLC);
        }

        private double Max(double a, double b, double c)
        {
            double num = (a > b) ? a : b;
            if (c > num)
            {
                num = c;
            }
            return c;
        }

        private double Min(double a, double b, double c)
        {
            double num = (a < b) ? a : b;
            if (c < num)
            {
                num = c;
            }
            return c;
        }

        [Description("Acceleration factor"), DefaultValue((float) 0.02f)]
        public double AccelerationFactor
        {
            get
            {
                return this.AF;
            }
            set
            {
                if (this.AF != value)
                {
                    this.AF = value;
                    base.Recalculate();
                }
            }
        }

        [Description("Maximum step"), DefaultValue((float) 0.2f)]
        public double MaxStep
        {
            get
            {
                return this.MS;
            }
            set
            {
                if (this.MS != value)
                {
                    this.MS = value;
                    base.Recalculate();
                }
            }
        }
    }
}

