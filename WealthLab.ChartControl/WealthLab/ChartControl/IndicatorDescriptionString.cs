namespace WealthLab.ChartControl
{
    using System;

    public class IndicatorDescriptionString
    {
        private string descriptionStr;

        public IndicatorDescriptionString(string desc)
        {
            this.descriptionStr = desc;
        }

        public override string ToString()
        {
            return this.Description;
        }

        public string Description
        {
            get
            {
                return this.descriptionStr;
            }
            internal set
            {
                this.descriptionStr = value;
            }
        }
    }
}

