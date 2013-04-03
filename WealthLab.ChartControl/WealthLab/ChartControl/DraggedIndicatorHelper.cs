namespace WealthLab.ChartControl
{
    using System;
    using WealthLab;

    public class DraggedIndicatorHelper
    {
        private IndicatorHelper indicatorHelper_0;

        public DraggedIndicatorHelper(IndicatorHelper helper)
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

