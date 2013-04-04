namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart.Styles;
    using System;

    public class SeriesHotspotEventArgs : EventArgs
    {
        private Steema.TeeChart.Styles.PointPolygon pointPolygon;
        private Steema.TeeChart.Styles.Series series;

        public SeriesHotspotEventArgs(Steema.TeeChart.Styles.Series s, Steema.TeeChart.Styles.PointPolygon p)
        {
            this.series = s;
            this.pointPolygon = p;
        }

        public Steema.TeeChart.Styles.PointPolygon PointPolygon
        {
            get
            {
                return this.pointPolygon;
            }
            set
            {
                this.pointPolygon = value;
            }
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

