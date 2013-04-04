namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class DownSampling : Function
    {
        private int displayedPointCount;
        private DownSamplingMethod method;
        private int reducedsize;
        private double tolerance;

        public DownSampling() : this(null)
        {
        }

        public DownSampling(Chart c) : base(c)
        {
            base.CanUsePeriod = false;
            base.SingleSource = true;
            base.dPeriod = 1.0;
            this.tolerance = 1.0;
            this.method = DownSamplingMethod.Average;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series s = (Series) source.GetValue(0);
                base.Series.Clear();
                int count = s.Count;
                int lbound = 0;
                if ((base.Chart != null) && (base.Chart.ChartRect != Rectangle.Empty))
                {
                    base.Chart.Bitmap();
                    s.CalcFirstLastVisibleIndex();
                    count = s.LastVisibleIndex - s.FirstVisibleIndex;
                    lbound = s.FirstVisibleIndex;
                }
                if (count > 0)
                {
                    if (s.yMandatory == base.Series.yMandatory)
                    {
                        base.Series.notMandatory.Order = ValueListOrder.Ascending;
                        base.Series.mandatory.Order = ValueListOrder.None;
                    }
                    else
                    {
                        base.Series.notMandatory.Order = ValueListOrder.None;
                        base.Series.mandatory.Order = ValueListOrder.Ascending;
                    }
                    double[] rx = new double[count];
                    double[] ry = new double[count];
                    ColorList colors = new ColorList(count);
                    double tolerance = this.tolerance;
                    if (this.DisplayedPointCount > 0)
                    {
                        tolerance = ((double) this.DisplayedPointCount) / 4.0;
                        tolerance = ((double) count) / tolerance;
                    }
                    this.reducedsize = this.Reduce(this.method, tolerance, lbound, lbound + count, s, ref rx, ref ry, ref colors, base.Series.Color);
                    base.Series.notMandatory.Count = this.reducedsize;
                    base.Series.mandatory.Count = this.reducedsize;
                    if (s.yMandatory)
                    {
                        base.Series.notMandatory.Value = rx;
                        base.Series.mandatory.Value = ry;
                    }
                    else
                    {
                        base.Series.notMandatory.Value = ry;
                        base.Series.mandatory.Value = rx;
                    }
                    base.Series.Colors = colors;
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionDownSampling;
        }

        public int Reduce(DownSamplingMethod algorithm, double tol, int lbound, int ubound, Series s, ref double[] rx, ref double[] ry, ref ColorList colors, Color color)
        {
            int index = 0;
            int num2 = lbound;
            int num3 = num2;
            int num4 = 0;
            int num5 = Utils.Round(tol);
            double[] numArray = s.XValues.Value;
            double[] numArray2 = s.YValues.Value;
            double num6 = 0.0;
            double num7 = 0.0;
            double num8 = 0.0;
            double num9 = 0.0;
            double num10 = 0.0;
            double num11 = 0.0;
            double num12 = 0.0;
            double num13 = 0.0;
            double num14 = 0.0;
            while (num2 < ubound)
            {
                num3 = num2;
                if (algorithm == DownSamplingMethod.MinMaxFirstLastNull)
                {
                    num4 = 0;
                    if (s.IsNull(num2))
                    {
                        num4++;
                    }
                }
                num6 = numArray2[num2];
                num7 = numArray2[num2];
                num8 = numArray2[num2];
                num9 = numArray2[num2];
                num11 = numArray[num2];
                num12 = numArray[num2];
                num13 = numArray[num2];
                num10 = num9;
                num14 = num11;
                if (this.DisplayedPointCount <= 0)
                {
                    goto Label_0238;
                }
                for (int i = 1; ((num3 + 1) < ubound) && (i < num5); i++)
                {
                    num3++;
                    if (algorithm == DownSamplingMethod.MinMaxFirstLastNull)
                    {
                        if (s.IsNull(num3))
                        {
                            num4++;
                        }
                        if (num4 <= 1)
                        {
                            if (!s.IsNull(num3))
                            {
                                if (num6 != s.DefaultNullValue)
                                {
                                    num6 += numArray2[num3];
                                }
                                else
                                {
                                    num6 = numArray2[num3];
                                }
                                if (numArray2[num3] > num7)
                                {
                                    num7 = numArray2[num3];
                                    num12 = numArray[num3];
                                }
                                if (numArray2[num3] < num8)
                                {
                                    num8 = numArray2[num3];
                                    num13 = numArray[num3];
                                }
                                num10 = numArray2[num3];
                                num14 = numArray[num3];
                            }
                            else
                            {
                                if (numArray2[num3] > num7)
                                {
                                    num7 = numArray2[num3];
                                    num12 = numArray[num3];
                                }
                                if (numArray2[num3] < num8)
                                {
                                    num8 = numArray2[num3];
                                    num13 = numArray[num3];
                                }
                                num10 = numArray2[num3];
                                num14 = numArray[num3];
                            }
                        }
                    }
                    else
                    {
                        num6 += numArray2[num3];
                        if (numArray2[num3] > num7)
                        {
                            num7 = numArray2[num3];
                            num12 = numArray[num3];
                        }
                        if (numArray2[num3] < num8)
                        {
                            num8 = numArray2[num3];
                            num13 = numArray[num3];
                        }
                        num10 = numArray2[num3];
                        num14 = numArray[num3];
                    }
                }
                goto Label_0252;
            Label_01F7:
                num3++;
                num6 += numArray2[num3];
                if (numArray2[num3] > num7)
                {
                    num7 = numArray2[num3];
                    num12 = numArray[num3];
                }
                if (numArray2[num3] < num8)
                {
                    num8 = numArray2[num3];
                    num13 = numArray[num3];
                }
                num10 = numArray2[num3];
                num14 = numArray[num3];
            Label_0238:
                if (((num3 + 1) < ubound) && (Math.Abs((double) (numArray[num3 + 1] - numArray[num2])) < tol))
                {
                    goto Label_01F7;
                }
            Label_0252:
                if (((algorithm != DownSamplingMethod.MinMax) && (algorithm != DownSamplingMethod.MinMaxFirstLast)) && (algorithm != DownSamplingMethod.MinMaxFirstLastNull))
                {
                    rx[index] = (numArray[num3] + numArray[num2]) * 0.5;
                    if (algorithm == DownSamplingMethod.Average)
                    {
                        ry[index] = num6 / ((double) ((num3 - num2) + 1));
                    }
                    else if (algorithm == DownSamplingMethod.Max)
                    {
                        ry[index] = num7;
                    }
                    else if (algorithm == DownSamplingMethod.Min)
                    {
                        ry[index] = num8;
                    }
                    index++;
                }
                else if (index <= rx.GetUpperBound(0))
                {
                    if ((num3 - num2) == 0)
                    {
                        rx[index] = numArray[num2];
                        ry[index] = num8;
                        if (algorithm == DownSamplingMethod.MinMaxFirstLastNull)
                        {
                            if (num8 == s.DefaultNullValue)
                            {
                                colors.Add(Color.Transparent);
                            }
                            else
                            {
                                colors.Add(color);
                            }
                        }
                        index++;
                    }
                    else if (algorithm == DownSamplingMethod.MinMax)
                    {
                        rx[index] = numArray[num2];
                        rx[index + 1] = numArray[num3];
                        ry[index] = num8;
                        ry[index + 1] = num7;
                        index += 2;
                    }
                    else if ((num3 - num2) > 2)
                    {
                        rx[index] = num11;
                        ry[index] = num9;
                        if (algorithm == DownSamplingMethod.MinMaxFirstLastNull)
                        {
                            if (num9 == s.DefaultNullValue)
                            {
                                colors.Add(Color.Transparent);
                            }
                            else
                            {
                                colors.Add(color);
                            }
                            index++;
                            if (num12 <= num13)
                            {
                                if (num11 != num12)
                                {
                                    rx[index] = num12;
                                    ry[index] = num7;
                                    if (num7 == s.DefaultNullValue)
                                    {
                                        colors.Add(Color.Transparent);
                                    }
                                    else
                                    {
                                        colors.Add(color);
                                    }
                                    index++;
                                }
                                if ((num11 != num13) && (num13 != num12))
                                {
                                    rx[index] = num13;
                                    ry[index] = num8;
                                    if (num8 == s.DefaultNullValue)
                                    {
                                        colors.Add(Color.Transparent);
                                    }
                                    else
                                    {
                                        colors.Add(color);
                                    }
                                    index++;
                                }
                            }
                            else
                            {
                                if (num11 != num13)
                                {
                                    rx[index] = num13;
                                    ry[index] = num8;
                                    if (num8 == s.DefaultNullValue)
                                    {
                                        colors.Add(Color.Transparent);
                                    }
                                    else
                                    {
                                        colors.Add(color);
                                    }
                                    index++;
                                }
                                if ((num11 != num12) && (num13 != num12))
                                {
                                    rx[index] = num12;
                                    ry[index] = num7;
                                    if (num7 == s.DefaultNullValue)
                                    {
                                        colors.Add(Color.Transparent);
                                    }
                                    else
                                    {
                                        colors.Add(color);
                                    }
                                    index++;
                                }
                            }
                            if (((num14 != num11) && (num14 != num13)) && (num14 != num12))
                            {
                                rx[index] = num14;
                                ry[index] = num10;
                                if (num10 == s.DefaultNullValue)
                                {
                                    colors.Add(Color.Transparent);
                                }
                                else
                                {
                                    colors.Add(color);
                                }
                                index++;
                            }
                        }
                        else
                        {
                            if (num12 <= num13)
                            {
                                rx[index + 1] = num12;
                                rx[index + 2] = num13;
                                ry[index + 1] = num7;
                                ry[index + 2] = num8;
                            }
                            else
                            {
                                rx[index + 2] = num12;
                                rx[index + 1] = num13;
                                ry[index + 2] = num7;
                                ry[index + 1] = num8;
                            }
                            rx[index + 3] = num14;
                            ry[index + 3] = num10;
                            index += 4;
                        }
                    }
                    else if ((num3 - num2) < 2)
                    {
                        rx[index] = numArray[num2];
                        rx[index + 1] = numArray[num3];
                        ry[index] = numArray2[num2];
                        ry[index + 1] = numArray2[num3];
                        index += 2;
                    }
                    else
                    {
                        double num16 = 0.0;
                        double num17 = 0.0;
                        double num18 = 0.0;
                        double num19 = 0.0;
                        double num20 = 0.0;
                        double num21 = 0.0;
                        if ((num9 == num8) || (num10 == num8))
                        {
                            num16 = num9;
                            num19 = num11;
                            num17 = num7;
                            num20 = num12;
                            num18 = num10;
                            num21 = num14;
                        }
                        else if ((num9 == num7) || (num10 == num7))
                        {
                            num16 = num9;
                            num19 = num11;
                            num17 = num8;
                            num20 = num13;
                            num18 = num10;
                            num21 = num14;
                        }
                        rx[index] = num19;
                        rx[index + 1] = num20;
                        rx[index + 2] = num21;
                        ry[index] = num16;
                        ry[index + 1] = num17;
                        ry[index + 2] = num18;
                        index += 3;
                    }
                }
                num2 = num3 + 1;
            }
            return index;
        }

        public int DisplayedPointCount
        {
            get
            {
                return this.displayedPointCount;
            }
            set
            {
                this.displayedPointCount = value;
            }
        }

        [Description("Defines reduction/downsampling method.")]
        public DownSamplingMethod Method
        {
            get
            {
                return this.method;
            }
            set
            {
                if (this.method != value)
                {
                    this.method = value;
                    base.Recalculate();
                }
            }
        }

        public int ReducedSize
        {
            get
            {
                return this.reducedsize;
            }
        }

        public double Tolerance
        {
            get
            {
                return this.tolerance;
            }
            set
            {
                if (this.tolerance != value)
                {
                    this.tolerance = Math.Max(0.0, value);
                    base.Recalculate();
                }
            }
        }
    }
}

