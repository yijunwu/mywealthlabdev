namespace WealthLab.DataProviders.MarketManagerService
{
    using System;

    public class MarketInfoSettings
    {
        private bool bool_0;
        public bool HolidaysReadOnly;
        public bool NameReadOnly;
        public bool SpecialHoursReadOnly;
        private string string_0;
        private Symbols symbols_0;

        public MarketInfoSettings()
        {
            this.symbols_0 = new Symbols();
        }

        public MarketInfoSettings(string name)
        {
            this.symbols_0 = new Symbols();
            this.string_0 = name;
        }

        public Symbols CheckForDuplicates(Symbols symbols)
        {
            Symbols symbols2 = new Symbols();
            foreach (string str in symbols.Items)
            {
                if (this.symbols_0.Items.Contains(str))
                {
                    symbols2.Items.Add(str);
                }
            }
            return symbols2;
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

        public Symbols SymbolList
        {
            get
            {
                return this.symbols_0;
            }
            set
            {
                this.symbols_0 = value;
            }
        }

        public bool UseMarketByDefault
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
    }
}

