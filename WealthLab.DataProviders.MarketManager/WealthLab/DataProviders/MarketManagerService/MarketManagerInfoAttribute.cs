namespace WealthLab.DataProviders.MarketManagerService
{
    using System;

    public class MarketManagerInfoAttribute
    {
        private bool bool_0;
        private string string_0;

        internal MarketManagerInfoAttribute()
        {
            this.bool_0 = true;
        }

        public MarketManagerInfoAttribute(string name)
        {
            this.bool_0 = true;
            this.string_0 = name;
        }

        public override string ToString()
        {
            return this.string_0;
        }

        public bool Enabled
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public string Name
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}

