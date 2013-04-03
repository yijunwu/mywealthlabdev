namespace WealthLab.ChartControl
{
    using System;

    public class BarsDescriptorString
    {
        private string string_0;

        public BarsDescriptorString(string desc)
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
        }
    }
}

