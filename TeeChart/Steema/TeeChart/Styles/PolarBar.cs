namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(PolarBar), "SeriesIcons.PolarBar.bmp")]
    public class PolarBar : Polar
    {
        public PolarBar() : this(null)
        {
        }

        public PolarBar(Chart c) : base(c)
        {
        }

        protected override void InternalDrawValue(int index, int X, int Y)
        {
            if ((base.TreatNulls == TreatNullsStyle.Ignore) || !base.IsNull(index))
            {
                base.chart.graphics3D.Pen = base.Pen;
                base.chart.graphics3D.Pen.Color = this.ValueColor(index);
                base.chart.graphics3D.Line(base.CircleXCenter, base.CircleYCenter, X, Y, base.startZ);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryPolarBar;
            }
        }
    }
}

