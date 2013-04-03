namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class WSExceptionEventArgs : EventArgs
    {
        private System.Exception exception_0;
        [CompilerGenerated]
        private WealthLab.Strategy strategy_0;

        public WSExceptionEventArgs(System.Exception exception_1)
        {
            this.exception_0 = exception_1;
        }

        public System.Exception Exception
        {
            get
            {
                return this.exception_0;
            }
        }

        public WealthLab.Strategy Strategy
        {
            [CompilerGenerated]
            get
            {
                return this.strategy_0;
            }
            [CompilerGenerated]
            set
            {
                this.strategy_0 = value;
            }
        }
    }
}

