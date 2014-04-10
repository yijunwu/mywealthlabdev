namespace WealthLab.ChartControl
{
    using System;
    using System.Runtime.CompilerServices;
    using WealthLab;

    public class ScaleChangeEventArgs : EventArgs
    {
        [CompilerGenerated]
        private BarDataScale barDataScale;

        public ScaleChangeEventArgs(BarDataScale barDataScale_1)
        {
            this.ChartScale = barDataScale_1;
        }

        public BarDataScale ChartScale
        {
            [CompilerGenerated]
            get
            {
                return this.barDataScale;
            }
            [CompilerGenerated]
            set
            {
                this.barDataScale = value;
            }
        }
    }
}

