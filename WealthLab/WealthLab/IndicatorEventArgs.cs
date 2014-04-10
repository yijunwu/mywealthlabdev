namespace WealthLab
{
    using System;

    public class IndicatorEventArgs : EventArgs
    {
        private IndicatorHelper indicatorHelper;

        public IndicatorEventArgs(IndicatorHelper helper)
        {
            this.indicatorHelper = helper;
        }

        public IndicatorHelper Helper
        {
            get
            {
                return this.indicatorHelper;
            }
        }
    }
}

