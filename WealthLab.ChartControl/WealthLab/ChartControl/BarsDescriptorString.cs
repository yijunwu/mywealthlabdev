namespace WealthLab.ChartControl
{
    using System;

    public class BarsDescriptorString
    {
        private string descriptorStr;

        public BarsDescriptorString(string desc)
        {
            this.descriptorStr = desc;
        }

        public override string ToString()
        {
            return this.Description;
        }

        public string Description
        {
            get
            {
                return this.descriptorStr;
            }
        }
    }
}

