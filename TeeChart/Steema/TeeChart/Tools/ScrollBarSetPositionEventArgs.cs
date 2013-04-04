namespace Steema.TeeChart.Tools
{
    using System;

    public class ScrollBarSetPositionEventArgs : EventArgs
    {
        private int position;

        public ScrollBarSetPositionEventArgs(int Position)
        {
            this.position = Position;
        }

        public int Position
        {
            get
            {
                return this.position;
            }
        }
    }
}

