namespace WealthLab.ChartControl
{
    using System;

    public class DroppedIndicatorEventArgs : EventArgs
    {
        private IndicatorDescriptor indicatorDescriptor_0;

        public DroppedIndicatorEventArgs(IndicatorDescriptor indDesc)
        {
            this.indicatorDescriptor_0 = indDesc;
        }

        public IndicatorDescriptor IndDesc
        {
            get
            {
                return this.indicatorDescriptor_0;
            }
        }
    }
}

