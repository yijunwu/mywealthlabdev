namespace WealthLab
{
    using System;

    public class IndicatorEventArgs : EventArgs
    {
        private IndicatorHelper indicatorHelper_0;

        public IndicatorEventArgs(IndicatorHelper helper)
        {
            this.indicatorHelper_0 = helper;
        }

        public IndicatorHelper Helper
        {
            get
            {
                return this.indicatorHelper_0;
            }
        }
    }
}

