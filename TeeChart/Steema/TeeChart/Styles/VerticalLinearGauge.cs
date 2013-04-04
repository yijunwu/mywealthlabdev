namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(VerticalLinearGauge), "SeriesIcons.VerticalLinearGauge.bmp")]
    public class VerticalLinearGauge : LinearGauge
    {
        public VerticalLinearGauge() : this(null)
        {
        }

        public VerticalLinearGauge(Chart c) : base(c)
        {
            base.Horizontal = false;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryVerticalLinearGauge;
            }
        }
    }
}

