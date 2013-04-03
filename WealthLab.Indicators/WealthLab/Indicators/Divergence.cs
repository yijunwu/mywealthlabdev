namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class Divergence : DataSeries
    {
        private DataSeries dataSeries_1;
        private DataSeries dataSeries_2;
        private int int_1;
        private MAType matype_0;

        public Divergence(DataSeries source, MAType maType, int period, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = period;
            base.FirstValidValue = (period - 1) + source.FirstValidValue;
            this.matype_0 = maType;
            switch (maType)
            {
                case MAType.SMA:
                    this.dataSeries_2 = SMA.Series(source, period);
                    break;

                case MAType.EMAModern:
                    this.dataSeries_2 = EMAModern.Series(source, period);
                    break;

                case MAType.EMALegacy:
                    this.dataSeries_2 = EMALegacy.Series(source, period);
                    break;

                case MAType.WMA:
                    this.dataSeries_2 = WMA.Series(source, period);
                    break;

                case MAType.VMA:
                    this.dataSeries_2 = VMA.Series(source, period);
                    break;
            }
            if (period < base.Count)
            {
                for (int i = period; i < source.Count; i++)
                {
                    double num = source[i] - this.dataSeries_2[i];
                    base[i] = num;
                }
            }
        }

        public override void CalculatePartialValue()
        {
            if (((this.dataSeries_1.Count >= this.int_1) && (this.dataSeries_1.PartialValue != double.NaN)) && (this.dataSeries_2.PartialValue != double.NaN))
            {
                base.PartialValue = this.dataSeries_1.PartialValue - this.dataSeries_2.PartialValue;
            }
            else
            {
                base.PartialValue = double.NaN;
            }
        }

        public static Divergence Series(DataSeries source, MAType maType, int period)
        {
            string key = string.Concat(new object[] { "Divergence(", source.Description, ",", maType.ToString(), ",", period, ")" });
            if (source.Cache.ContainsKey(key))
            {
                return (Divergence) source.Cache[key];
            }
            Divergence divergence = new Divergence(source, maType, period, key);
            source.Cache[key] = divergence;
            return divergence;
        }
    }
}

