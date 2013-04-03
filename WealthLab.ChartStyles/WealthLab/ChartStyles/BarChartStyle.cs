namespace WealthLab.ChartStyles
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.ChartStyles.Properties;

    public class BarChartStyle : ChartStyle
    {
        private int int_0;

        public override void Initialize()
        {
        }

        protected override void InitializeBarWidths()
        {
            this.int_0 = (base.BarSpacing / 2) - 1;
            if (this.int_0 < 1)
            {
                this.int_0 = 1;
            }
        }

        public override void RenderBars(Graphics graphics_0)
        {
            int ghostBarXPosition;
            int num3;
            int num4;
            int num5;
            int num6;
            Pen pen = new Pen(Color.Black);
            ChartPane pricePane = base.PricePane;
            for (int i = base.LeftEdgeBar; i <= base.RightEdgeBar; i++)
            {
                ghostBarXPosition = base.ConvertBarToX(i);
                num3 = pricePane.ConvertValueToY(base.Bars.Open[i]);
                num4 = pricePane.ConvertValueToY(base.Bars.High[i]);
                num5 = pricePane.ConvertValueToY(base.Bars.Low[i]);
                num6 = pricePane.ConvertValueToY(base.Bars.Close[i]);
                pen.Color = base.GetBarColor(i);
                graphics_0.DrawLine(pen, ghostBarXPosition, num4, ghostBarXPosition, num5);
                graphics_0.DrawLine(pen, ghostBarXPosition - this.int_0, num3, ghostBarXPosition, num3);
                graphics_0.DrawLine(pen, ghostBarXPosition, num6, ghostBarXPosition + this.int_0, num6);
            }
            if (base.ShouldDrawGhostBar)
            {
                ghostBarXPosition = base.GhostBarXPosition;
                num3 = pricePane.ConvertValueToY(base.Bars.Open.PartialValue);
                num4 = pricePane.ConvertValueToY(base.Bars.High.PartialValue);
                num5 = pricePane.ConvertValueToY(base.Bars.Low.PartialValue);
                num6 = pricePane.ConvertValueToY(base.Bars.Close.PartialValue);
                pen.Color = base.GhostBarColor;
                graphics_0.DrawLine(pen, ghostBarXPosition, num4, ghostBarXPosition, num5);
                graphics_0.DrawLine(pen, ghostBarXPosition - this.int_0, num3, ghostBarXPosition, num3);
                graphics_0.DrawLine(pen, ghostBarXPosition, num6, ghostBarXPosition + this.int_0, num6);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Bar";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.BarChartStyle;
            }
        }
    }
}

