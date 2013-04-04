namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class RSIFunction : Moving
    {
        private ValueList Closes;
        private RSIStyle fStyle;
        private Series iSeries;
        private ValueList Opens;

        public RSIFunction() : this(null)
        {
        }

        public RSIFunction(Chart c) : base(c)
        {
            this.fStyle = RSIStyle.OpenClose;
            base.SingleSource = true;
            base.HideSourceList = true;
        }

        public override double Calculate(Series source, int first, int last)
        {
            double num2;
            if (this.iSeries != source)
            {
                this.Closes = source.GetYValueList(Texts.ValuesClose);
                this.Opens = source.GetYValueList(Texts.ValuesOpen);
                this.iSeries = source;
            }
            double num3 = 0.0;
            double num4 = 0.0;
            if (this.Style == RSIStyle.OpenClose)
            {
                for (int i = first; i <= last; i++)
                {
                    num2 = this.Closes[i];
                    if (this.Opens[i] > num2)
                    {
                        num4 += num2;
                    }
                    else
                    {
                        num3 += num2;
                    }
                }
            }
            else
            {
                for (int j = first + 1; j <= last; j++)
                {
                    num2 = this.Closes[j] - this.Closes[j - 1];
                    if (num2 < 0.0)
                    {
                        num4 -= num2;
                    }
                    else
                    {
                        num3 += num2;
                    }
                }
            }
            int num = (last - first) + 1;
            num4 /= (double) num;
            num3 /= (double) num;
            if (num4 != 0.0)
            {
                double num5 = 100.0 - (100.0 / (1.0 + Math.Abs((double) (num3 / num4))));
                if (num5 < 0.0)
                {
                    return 0.0;
                }
                if (num5 > 100.0)
                {
                    num5 = 100.0;
                }
                return num5;
            }
            return 100.0;
        }

        public override string Description()
        {
            return Texts.FunctionRSI;
        }

        internal override bool IsValidSource(Series Value)
        {
            return (Value is OHLC);
        }

        [DefaultValue(0)]
        public RSIStyle Style
        {
            get
            {
                return this.fStyle;
            }
            set
            {
                if (this.fStyle != value)
                {
                    this.fStyle = value;
                    base.Recalculate();
                }
            }
        }
    }
}

