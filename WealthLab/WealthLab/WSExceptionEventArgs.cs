namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class WSExceptionEventArgs : EventArgs
    {
        private System.Exception exception;
        [CompilerGenerated]
        private WealthLab.Strategy strategy;

        public WSExceptionEventArgs(System.Exception exception_1)
        {
            this.exception = exception_1;
        }

        public System.Exception Exception
        {
            get
            {
                return this.exception;
            }
        }

        public WealthLab.Strategy Strategy
        {
            [CompilerGenerated]
            get
            {
                return this.strategy;
            }
            [CompilerGenerated]
            set
            {
                this.strategy = value;
            }
        }
    }
}

