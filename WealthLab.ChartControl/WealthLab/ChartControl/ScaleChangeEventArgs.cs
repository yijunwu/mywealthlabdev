namespace WealthLab.ChartControl
{
    using System;
    using System.Runtime.CompilerServices;
    using WealthLab;

    public class ScaleChangeEventArgs : EventArgs
    {
        [CompilerGenerated]
        private BarDataScale barDataScale_0;

        public ScaleChangeEventArgs(BarDataScale barDataScale_1)
        {
            this.ChartScale = barDataScale_1;
        }

        public BarDataScale ChartScale
        {
            [CompilerGenerated]
            get
            {
                return this.barDataScale_0;
            }
            [CompilerGenerated]
            set
            {
                this.barDataScale_0 = value;
            }
        }
    }
}

