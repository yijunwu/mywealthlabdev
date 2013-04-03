namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.Indicators;

    public class RSIGeneral : IndexDefinition
    {
        private NumericParameter _paramUI = new NumericParameter("Period:", 14M, 0, 2147483647M, -2147483648M, 1M);
        private int _period = 14;

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
                    if (num4 >= RSI.Series(bars2.Close, this._period).FirstValidValue)
                    {
                        num++;
                        open += RSI.Series(bars2.Close, this._period)[num4];
                    }
                }
                if (num > 0)
                {
                    open /= (double) num;
                    indexToDate.Add(dt, open, open, open, open, open);
                }
            }
        }

        public override string Description
        {
            get
            {
                return "An indicator that returns an average of the Relative Strength Index (RSI) of a specific period for the symbols in the selected DataSet.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "RSI";
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
                return "RSI";
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

