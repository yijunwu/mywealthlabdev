namespace WealthLab.ChartControl
{
    using System;
    using WealthLab;

    public class DraggedIndicatorHelper
    {
        private IndicatorHelper indicatorHelper;

        public DraggedIndicatorHelper(IndicatorHelper helper)
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

