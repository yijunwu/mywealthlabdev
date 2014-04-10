namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class StrategyParameterEventArgs : EventArgs
    {
        [CompilerGenerated]
        private string symbol;
        [CompilerGenerated]
        private WealthLab.WealthScript wealthScript;

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
                return this.symbol;
            }
            [CompilerGenerated]
            internal set
            {
                this.symbol = value;
            }
        }

        public WealthLab.WealthScript WealthScript
        {
            [CompilerGenerated]
            get
            {
                return this.wealthScript;
            }
            [CompilerGenerated]
            internal set
            {
                this.wealthScript = value;
            }
        }
    }
}

