namespace Steema.TeeChart
{
    using System;

    internal class GetLegendSizeEventArgs : EventArgs
    {
        private int size;

        public GetLegendSizeEventArgs(int Size)
        {
            this.size = Size;
        }

        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }
    }
}

