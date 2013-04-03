namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class StrategyParameterEventArgs : EventArgs
    {
        [CompilerGenerated]
        private string string_0;
        [CompilerGenerated]
        private WealthLab.WealthScript wealthScript_0;

        public StrategyParameterEventArgs(WealthLab.WealthScript wealthScript_1, string symbol)
        {
            this.WealthScript = wealthScript_1;
            this.Symbol = symbol;
        }

        public string Symbol
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            internal set
            {
                this.string_0 = value;
            }
        }

        public WealthLab.WealthScript WealthScript
        {
            [CompilerGenerated]
            get
            {
                return this.wealthScript_0;
            }
            [CompilerGenerated]
            internal set
            {
                this.wealthScript_0 = value;
            }
        }
    }
}

