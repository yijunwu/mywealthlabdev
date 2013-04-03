namespace WealthLab.ChartControl
{
    using System;

    public class ExceptionEventArgs : EventArgs
    {
        private System.Exception exception_0;

        public ExceptionEventArgs(System.Exception exception_1)
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
    }
}

