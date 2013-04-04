namespace Steema.TeeChart
{
    using Steema.TeeChart.Styles;
    using System;

    public class NotifySeriesEventArgs : EventArgs
    {
        private Steema.TeeChart.Styles.Series series;

        public NotifySeriesEventArgs(Steema.TeeChart.Styles.Series s)
        {
            this.series = s;
        }

        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
        }
    }
}

