namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.Indicators;

    public class TsokakisMACDBull : IndexDefinition
    {
        private NumericParameter _paramUI = new NumericParameter("Period:", 9M, 0, 2147483647M, -2147483648M, 1M);
        private int _period = 9;

        public override void ClearUserInterface()
        {
            this._paramUI.ClearFields();
        }

        public override void SetIndexValues(List<Bars> bars, List<int> barNumbers, DateTime dt, Bars indexToDate)
        {
            if (bars.Count != 0)
            {
                int num = 0;
                double open = 0.0;
                for (int i = 0; i < bars.Count; i++)
                {
                    Bars bars2 = bars[i];
                    int num4 = barNumbers[i];
                    DataSeries series = EMA.Series(MACD.Series(bars2.Close), this._period, EMACalculation.Modern);
                    if (num4 >= series.FirstValidValue)
                    {
                        num++;
                        if (MACD.Series(bars2.Close)[num4] <= series[num4])
                        {
                            open++;
                        }
                    }
                }
                if (num > 0)
                {
                    open /= (double) num;
                    open *= 100.0;
                    indexToDate.Add(dt, open, open, open, open, open);
                }
            }
        }

        public override string Description
        {
            get
            {
                return "Tsokakis MACDBull is an Aggregate Indicator shows the percentage of symbols whose MACD is above the Signal Line (bullish).";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Tsokakis MACDBull";
            }
        }

        public override bool NeedsSeparateUIForParameters
        {
            get
            {
                return false;
            }
        }

        public override string ParameterString
        {
            get
            {
                this._period = (int) this._paramUI.Value;
                return ("Period:" + this._period.ToString());
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] strArray = value.Split(new char[] { ':' });
                    if ((strArray != null) && (strArray.Length == 2))
                    {
                        int num;
                        int.TryParse(strArray[1], out num);
                        this._period = num;
                    }
                }
            }
        }

        public override UserControl ParameterUserInterface
        {
            get
            {
                return this._paramUI;
            }
        }

        public override string Prefix
        {
            get
            {
                return "MACDBull";
            }
        }

        public override bool SupportsParameters
        {
            get
            {
                return true;
            }
        }
    }
}

