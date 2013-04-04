namespace Steema.TeeChart
{
    using System;

    public class DblClickSeriesEventArgs : EventArgs
    {
        private int index;

        public DblClickSeriesEventArgs(int i)
        {
            this.index = i;
        }

        public int Index
        {
            get
            {
                return this.index;
            }
        }
    }
}

