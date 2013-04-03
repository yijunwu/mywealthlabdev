namespace WealthLab.IndexDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using WealthLab;

    public class AggregateIndicatorIndex : IndexDefinition
    {
        private IndicatorGenerator _indicatorGenerator = new IndicatorGenerator();
        private AggregateIndicatorIndexParameter _paramUI;

        public override void SetIndexValues(List<Bars> bars, List<int> barNumbers, DateTime dt, Bars indexToDate)
        {
            double open = 0.0;
            double high = 0.0;
            double num3 = 0.0;
            double close = 0.0;
            double volume = 0.0;
            if (bars.Count != 0)
            {
                int num6 = 0;
                for (int i = 0; i < bars.Count; i++)
                {
                    Bars symBars = bars[i];
                    int num8 = barNumbers[i];
                    DataSeries indicator = this._indicatorGenerator.GetIndicator(symBars);
                    if (num8 >= indicator.FirstValidValue)
                    {
                        num6++;
                        open += indicator[num8];
                        high += indicator[num8];
                        num3 += indicator[num8];
                        close += indicator[num8];
                        volume += indicator[num8];
                    }
                }
                if (num6 != 0)
                {
                    open /= (double) num6;
                    high /= (double) num6;
                    num3 /= (double) num6;
                    close /= (double) num6;
                    volume /= (double) num6;
                    indexToDate.Add(dt, open, high, num3, close, volume);
                }
            }
        }

        public override bool ValidateUserInput(ref string errMsg)
        {
            if (string.IsNullOrEmpty(this.ParameterString))
            {
                errMsg = "Please select an indicator.";
                return false;
            }
            return true;
        }

        public override string Description
        {
            get
            {
                return "A brand new indicator that averages the results of any technical indicator over the symbols in the selected DataSet.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Aggregate Indicator";
            }
        }

        public override string ParameterString
        {
            get
            {
                return this._paramUI.IndicatorString;
            }
            set
            {
                this._indicatorGenerator.Code = value;
            }
        }

        public override UserControl ParameterUserInterface
        {
            get
            {
                if (this._paramUI == null)
                {
                    this._paramUI = new AggregateIndicatorIndexParameter();
                }
                return this._paramUI;
            }
        }

        public override string Prefix
        {
            get
            {
                if ((this._paramUI != null) && (this._paramUI.IndicatorHelper != null))
                {
                    return this._paramUI.IndicatorHelper.IndicatorType.Name;
                }
                return "AII";
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

