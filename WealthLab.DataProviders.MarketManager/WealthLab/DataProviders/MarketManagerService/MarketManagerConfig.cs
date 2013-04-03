namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.Collections.Generic;

    public class MarketManagerConfig
    {
        private List<string> list_0 = new List<string>();

        public List<string> DisabledProviders
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }
    }
}

