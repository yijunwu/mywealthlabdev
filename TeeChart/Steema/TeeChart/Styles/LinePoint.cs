namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(LinePoint), "SeriesIcons.LinePoint.bmp")]
    public class LinePoint : Points
    {
        public LinePoint() : this(null)
        {
        }

        public LinePoint(Chart c) : base(c)
        {
            base.Pointer.Draw3D = false;
            base.Pointer.Style = PointerStyles.Diamond;
            base.LinePen.Color = Color.Red;
        }

        public override void DrawValue(int valueIndex)
        {
            int x = this.CalcXPos(valueIndex);
            int y = this.CalcYPos(valueIndex);
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Pen = base.LinePen;
            graphicsd.MoveTo(base.GetVertAxis.Position, y, base.StartZ);
            graphicsd.LineTo(x, y, base.StartZ);
            graphicsd.LineTo(x, base.GetHorizAxis.Position, base.StartZ);
            base.DrawValue(valueIndex);
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            base.Pointer.Color = color;
            base.LinePen.Color = Color.Red;
            if (!base.ColorEach)
            {
                Color color2 = Utils.DarkenColor(color, 60);
                base.Pointer.Pen.Color = color2;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryLinePoint;
            }
        }
    }
}

