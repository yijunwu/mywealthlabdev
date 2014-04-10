namespace WealthLab.ChartControl
{
    using System;

    public class DroppedIndicatorEventArgs : EventArgs
    {
        private IndicatorDescriptor indicatorDescriptor;

        public DroppedIndicatorEventArgs(IndicatorDescriptor indDesc)
        {
            this.indicatorDescriptor = indDesc;
        }

        public IndicatorDescriptor IndDesc
        {
            get
            {
                return this.indicatorDescriptor;
            }
        }
    }
}

