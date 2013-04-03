namespace WealthLab.ChartControl
{
    using System;

    public class IndicatorDescriptionString
    {
        private string string_0;

        public IndicatorDescriptionString(string desc)
        {
            this.string_0 = desc;
        }

        public override string ToString()
        {
            return this.Description;
        }

        public string Description
        {
            get
            {
                return this.string_0;
            }
            internal set
            {
                this.string_0 = value;
            }
        }
    }
}

