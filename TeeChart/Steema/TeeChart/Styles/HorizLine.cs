namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(HorizLine), "SeriesIcons.HorizLine.bmp")]
    public class HorizLine : Line
    {
        public HorizLine() : this(null)
        {
        }

        public HorizLine(Chart c) : base(c)
        {
            base.SetHorizontal();
            base.Pointer.Visible = false;
            base.Pointer.defaultVisible = false;
            base.calcVisiblePoints = false;
            base.XValues.Order = ValueListOrder.None;
            base.YValues.Order = ValueListOrder.Ascending;
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            base.bBrush.Gradient.Direction = LinearGradientMode.Vertical;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryHorizLine;
            }
        }
    }
}

