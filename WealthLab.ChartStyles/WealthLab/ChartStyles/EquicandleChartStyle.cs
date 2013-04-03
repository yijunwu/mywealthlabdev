namespace WealthLab.ChartStyles
{
    using System;
    using System.Drawing;
    using WealthLab;
    using WealthLab.ChartStyles.Properties;

    public class EquicandleChartStyle : ChartStyle
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
                int num3;
                int num6;
                Pen pen = new Pen(base.GetBarColor(i));
                int num7 = base.ConvertBarToX(i);
                if (base.Bars.Open[i] > base.Bars.Close[i])
                {
                    num6 = base.PricePane.ConvertValueToY(base.Bars.Close[i]);
                    num3 = base.PricePane.ConvertValueToY(base.Bars.Open[i]);
                }
                else
                {
                    num6 = base.PricePane.ConvertValueToY(base.Bars.Open[i]);
                    num3 = base.PricePane.ConvertValueToY(base.Bars.Close[i]);
                }
                int height = num6 - num3;
                int barWidth = base.GetBarWidth(i);
                int num8 = barWidth / 2;
                int num2 = num7 - num8;
                if ((barWidth % 2) > 0)
                {
                    num2--;
                }
                if (num3 == num6)
                {
                    graphics_0.DrawLine(pen, num2, num3, num2 + barWidth, num3);
                }
                else
                {
                    graphics_0.DrawRectangle(pen, num2, num3, barWidth, height);
                }
                height = base.PricePane.ConvertValueToY(base.Bars.High[i]);
                graphics_0.DrawLine(pen, num7, height, num7, num3);
                height = base.PricePane.ConvertValueToY(base.Bars.Low[i]);
                graphics_0.DrawLine(pen, num7, num6, num7, height);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "EquiCandle";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.EquicandleChartStyle;
            }
        }
    }
}

