namespace WealthLab.ChartStyles
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.ChartStyles.Properties;

    public class EquivolumeChartStyle : ChartStyle
    {
        public override void Initialize()
        {
        }

        protected override void InitializeBarWidths()
        {
            Class4.smethod_0(this);
        }

        public override void RenderBars(Graphics graphics_0)
        {
            for (int i = base.LeftEdgeBar; i <= base.RightEdgeBar; i++)
            {
                Pen pen = new Pen(base.GetBarColor(i));
                int num6 = base.ConvertBarToX(i);
                int num7 = base.PricePane.ConvertValueToY(base.Bars.Low[i]);
                int num3 = base.PricePane.ConvertValueToY(base.Bars.High[i]);
                int height = num7 - num3;
                int barWidth = base.GetBarWidth(i);
                int num8 = barWidth / 2;
                int num2 = num6 - num8;
                if ((barWidth % 2) > 0)
                {
                    num2--;
                }
                if (height == 0)
                {
                    graphics_0.DrawLine(pen, num2, num3, num2 + barWidth, num3);
                }
                else
                {
                    graphics_0.DrawRectangle(pen, num2, num3, barWidth, height);
                }
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "EquiVolume";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.EquivolumeChartStyle;
            }
        }
    }
}

