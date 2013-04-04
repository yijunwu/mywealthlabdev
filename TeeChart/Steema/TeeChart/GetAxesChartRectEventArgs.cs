namespace Steema.TeeChart
{
    using System;
    using System.Drawing;

    public class GetAxesChartRectEventArgs : EventArgs
    {
        private Rectangle axesChartRect;

        public GetAxesChartRectEventArgs(Rectangle rect)
        {
            this.axesChartRect = rect;
        }

        public Rectangle AxesChartRect
        {
            get
            {
                return this.axesChartRect;
            }
            set
            {
                this.axesChartRect = value;
            }
        }
    }
}

