namespace Steema.TeeChart
{
    using Steema.TeeChart.Styles;
    using System;

    public class ChangeOrderEventArgs : EventArgs
    {
        private Series series1;
        private Series series2;

        public ChangeOrderEventArgs(Series s1, Series s2)
        {
            this.series1 = s1;
            this.series2 = s2;
        }

        public Series Series1
        {
            get
            {
                return this.series1;
            }
        }

        public Series Series2
        {
            get
            {
                return this.series2;
            }
        }
    }
}

