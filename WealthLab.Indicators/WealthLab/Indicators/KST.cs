namespace WealthLab.Indicators
{
    using System;
    using WealthLab;

    public class KST : DataSeries
    {
        private DataSeries dataSeries_1;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private int int_5;
        private int int_6;
        private int int_7;
        private int int_8;

        public KST(DataSeries source, int ROC1, int EMA1, int ROC2, int EMA2, int ROC3, int EMA3, int ROC4, int EMA4, string description) : base(source, description)
        {
            this.dataSeries_1 = source;
            this.int_1 = ROC1;
            this.int_2 = EMA1;
            this.int_3 = ROC2;
            this.int_4 = EMA2;
            this.int_5 = ROC3;
            this.int_6 = EMA3;
            this.int_7 = ROC4;
            this.int_8 = EMA4;
            DataSeries series = (DataSeries) (EMA.Series(ROC.Series(source, ROC1), EMA1, EMACalculation.Modern) + (EMA.Series(ROC.Series(source, ROC2), EMA2, EMACalculation.Modern) * 2.0));
            series += (DataSeries) (EMA.Series(ROC.Series(source, ROC3), EMA3, EMACalculation.Modern) * 3.0);
            series += (DataSeries) (EMA.Series(ROC.Series(source, ROC4), EMA4, EMACalculation.Modern) * 4.0);
            for (int i = 0; i < source.Count; i++)
            {
                base[i] = series[i];
            }
        }

        public override void CalculatePartialValue()
        {
            DataSeries series = (DataSeries) (EMA.Series(ROC.Series(this.dataSeries_1, this.int_1), this.int_2, EMACalculation.Modern) + (EMA.Series(ROC.Series(this.dataSeries_1, this.int_3), this.int_4, EMACalculation.Modern) * 2.0));
            series += (DataSeries) (EMA.Series(ROC.Series(this.dataSeries_1, this.int_5), this.int_6, EMACalculation.Modern) * 3.0);
            series += (DataSeries) (EMA.Series(ROC.Series(this.dataSeries_1, this.int_7), this.int_8, EMACalculation.Modern) * 4.0);
            base.PartialValue = series.PartialValue;
        }

        public static KST Series(DataSeries source, int ROC1, int EMA1, int ROC2, int EMA2, int ROC3, int EMA3, int ROC4, int EMA4)
        {
            string key = string.Concat(new object[] { 
                "KST(", source.Description, ",", ROC1, ",", EMA1, ",", ROC2, ",", EMA2, ",", ROC3, ",", EMA3, ",", ROC4, 
                ",", EMA4, ")"
             });
            if (source.Cache.ContainsKey(key))
            {
                return (KST) source.Cache[key];
            }
            KST kst = new KST(source, ROC1, EMA1, ROC2, EMA2, ROC3, EMA3, ROC4, EMA4, key);
            source.Cache[key] = kst;
            return kst;
        }
    }
}

