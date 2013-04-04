namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;

    public class BaseTrend : Function
    {
        private int count;
        protected internal TrendStyles iTrendStyle;
        private double sumX;
        private double sumX2;
        private double sumXY;
        private double sumY;
        private double sumY2;

        public BaseTrend() : this(null)
        {
        }

        public BaseTrend(Chart c) : base(c)
        {
        }

        private void AddPoint(double m, double b, double val, Series s)
        {
            double y = (m * val) + b;
            double d = val * b;
            if (this.iTrendStyle == TrendStyles.Logarithmic)
            {
                y = (d > 0.0) ? (m * Math.Log(d)) : 0.0;
            }
            else if (this.iTrendStyle == TrendStyles.Exponential)
            {
                y = m * Math.Exp(d);
            }
            if (s.yMandatory)
            {
                base.Series.Add(val, y);
            }
            else
            {
                base.Series.Add(y, val);
            }
        }

        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            return 0.0;
        }

        protected override void CalculateAllPoints(Series source, Steema.TeeChart.Styles.ValueList notMandatorySource)
        {
            this.CalculatePeriod(source, 0.0, 0, source.Count - 1);
        }

        public override double CalculateMany(ArrayList sourceSeriesList, int valueIndex)
        {
            return 0.0;
        }

        protected override void CalculatePeriod(Series source, double tmpX, int firstIndex, int lastIndex)
        {
            double m = 0.0;
            double b = 0.0;
            if (this.CalculateTrend(source, firstIndex, lastIndex, ref m, ref b))
            {
                Steema.TeeChart.Styles.ValueList notMandatory = source.notMandatory;
                if (notMandatory.Order == ValueListOrder.None)
                {
                    this.AddPoint(m, b, notMandatory.Minimum, source);
                    this.AddPoint(m, b, notMandatory.Maximum, source);
                }
                else
                {
                    this.AddPoint(m, b, notMandatory[firstIndex], source);
                    this.AddPoint(m, b, notMandatory[lastIndex], source);
                }
            }
        }

        public bool CalculateTrend(Series s, int firstIndex, int lastIndex, ref double m, ref double b)
        {
            bool flag = this.CalculateValues(s, firstIndex, lastIndex);
            if (flag)
            {
                double num;
                if (this.iTrendStyle == TrendStyles.Normal)
                {
                    num = (this.count * this.sumX2) - (this.sumX * this.sumX);
                    if (num != 0.0)
                    {
                        m = ((this.count * this.sumXY) - (this.sumX * this.sumY)) / num;
                        b = ((this.sumY * this.sumX2) - (this.sumX * this.sumXY)) / num;
                        return flag;
                    }
                    m = 1.0;
                    b = 0.0;
                    return flag;
                }
                this.sumX /= (double) this.count;
                this.sumY /= (double) this.count;
                num = this.sumX2 - ((this.count * this.sumX) * this.sumX);
                b = (num != 0.0) ? ((this.sumXY - ((this.count * this.sumX) * this.sumY)) / num) : 1.0;
                if (this.iTrendStyle == TrendStyles.Logarithmic)
                {
                    m = this.sumY - (b * this.sumX);
                    return flag;
                }
                m = Math.Exp(this.sumY - (b * this.sumX));
            }
            return flag;
        }

        private bool CalculateValues(Series s, int firstIndex, int lastIndex)
        {
            if (firstIndex == -1)
            {
                firstIndex = 0;
                lastIndex = s.Count - 1;
            }
            this.count = (lastIndex - firstIndex) + 1;
            bool flag = this.count > 1;
            if (flag)
            {
                bool flag2 = (this.iTrendStyle == TrendStyles.Normal) && (this.count == s.Count);
                if (flag2)
                {
                    this.sumX = s.notMandatory.Total;
                    this.sumY = base.ValueList(s).Total;
                }
                else
                {
                    this.sumX = 0.0;
                    this.sumY = 0.0;
                }
                this.sumX2 = 0.0;
                this.sumY2 = 0.0;
                this.sumXY = 0.0;
                Steema.TeeChart.Styles.ValueList list = base.ValueList(s);
                for (int i = firstIndex; i <= lastIndex; i++)
                {
                    double num2;
                    double num = s.notMandatory[i];
                    if (this.iTrendStyle == TrendStyles.Normal)
                    {
                        num2 = list[i];
                    }
                    else if (list.Value[i] > 0.0)
                    {
                        num2 = Math.Log(list[i]);
                    }
                    else
                    {
                        num2 = 0.0;
                    }
                    this.sumXY += num * num2;
                    this.sumX2 += num * num;
                    this.sumY2 += num2 * num2;
                    if (!flag2)
                    {
                        this.sumX += num;
                        this.sumY += num2;
                    }
                }
            }
            return flag;
        }

        protected double Coefficient(Series s, int firstIndex, int lastIndex)
        {
            if (this.CalculateValues(s, firstIndex, lastIndex))
            {
                double num = this.sumXY - ((this.sumX * this.sumY) / ((double) this.count));
                double num2 = Math.Sqrt((this.sumX2 - ((this.sumX * this.sumX) / ((double) this.count))) * (this.sumY2 - ((this.sumY * this.sumY) / ((double) this.count))));
                if (num2 != 0.0)
                {
                    return (num / num2);
                }
            }
            return 1.0;
        }

        [Description("Defines different methods for calculating trendline."), DefaultValue(0)]
        public TrendStyles TrendStyle
        {
            get
            {
                return this.iTrendStyle;
            }
            set
            {
                if (this.iTrendStyle != value)
                {
                    this.iTrendStyle = value;
                    base.Recalculate();
                }
            }
        }
    }
}

