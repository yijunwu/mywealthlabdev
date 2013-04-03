namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class QStick : DataSeries
    {
        private Bars bars_1;
        private int int_1;

        public QStick(Bars bars, int period, string description) : base(bars, description)
        {
            this.bars_1 = bars;
            this.int_1 = period;
            for (int i = period; i < bars.Count; i++)
            {
                double num2 = 0.0;
                for (int j = i; j > (i - period); j--)
                {
                    num2 += bars.Close[j] - bars.Open[j];
                }
                base[i] = num2 / ((double) period);
            }
        }

        public override void CalculatePartialValue()
        {
            double num = 0.0;
            for (int i = (int) base.PartialValue; i > (base.PartialValue - this.int_1); i--)
            {
                num += this.bars_1.Close[i] - this.bars_1.Open[i];
            }
            base.PartialValue = num / ((double) this.int_1);
        }

        public static QStick Series(Bars bars, int period)
        {
            string key = "QStick(" + period + ")";
            if (bars.Cache.ContainsKey(key))
            {
                return (QStick) bars.Cache[key];
            }
            QStick stick = new QStick(bars, period, key);
            bars.Cache[key] = stick;
            return stick;
        }
    }
}

