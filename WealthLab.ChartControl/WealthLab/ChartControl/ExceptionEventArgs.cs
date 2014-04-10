namespace WealthLab.ChartControl
{
    using System;

    public class ExceptionEventArgs : EventArgs
    {
        private System.Exception exception;

        public ExceptionEventArgs(System.Exception exception_1)
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
    }
}

