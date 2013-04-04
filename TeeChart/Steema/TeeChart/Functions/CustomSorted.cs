namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;

    public class CustomSorted : Function
    {
        protected internal int iCount;
        private bool includeNulls;
        protected internal double[] tmp;

        public CustomSorted() : this(null)
        {
        }

        public CustomSorted(Chart c) : base(c)
        {
            this.includeNulls = true;
        }

        private void AddValue(double Value, int Index)
        {
            bool flag = false;
            for (int i = 0; i <= Index; i++)
            {
                if (this.tmp[i] > Value)
                {
                    for (int j = Index; j > i; j--)
                    {
                        this.tmp[j] = this.tmp[j - 1];
                    }
                    this.tmp[i] = Value;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                this.tmp[Index] = Value;
            }
        }

        protected internal virtual double CalcResult()
        {
            return 0.0;
        }

        public override double Calculate(Series source, int first, int last)
        {
            double num = 0.0;
            if (first == -1)
            {
                first = 0;
            }
            if (last == -1)
            {
                last = source.Count - 1;
            }
            this.iCount = (last - first) + 1;
            if (this.iCount > 0)
            {
                this.tmp = new double[this.iCount];
                for (int i = first; i <= last; i++)
                {
                    if (this.IncludeNulls || !source.IsNull(i))
                    {
                        this.AddValue(source.mandatory[i], i);
                    }
                    num = this.CalcResult();
                }
                return num;
            }
            return 0.0;
        }

        public override double CalculateMany(ArrayList sourceSeries, int valueIndex)
        {
            double num = 0.0;
            this.iCount = sourceSeries.Count;
            if (this.iCount > 0)
            {
                this.tmp = new double[this.iCount];
                for (int i = 0; i < this.iCount; i++)
                {
                    if ((this.IncludeNulls || !(sourceSeries[i] as Series).IsNull(valueIndex)) && (valueIndex < this.iCount))
                    {
                        this.AddValue((sourceSeries[i] as Series).mandatory[valueIndex], valueIndex);
                    }
                    num = this.CalcResult();
                }
                return num;
            }
            return 0.0;
        }

        [DefaultValue(true)]
        public bool IncludeNulls
        {
            get
            {
                return this.includeNulls;
            }
            set
            {
                if (this.includeNulls != value)
                {
                    this.includeNulls = value;
                    base.Recalculate();
                }
            }
        }
    }
}

