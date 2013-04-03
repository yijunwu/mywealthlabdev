namespace WealthLab.ChartStyles
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartStyles.Properties;

    public class CandleChartStyle : ChartStyle, ICustomSettings
    {
        private bool bool_0;
        private bool bool_1;
        private CandleChartStyleSettings candleChartStyleSettings_0;
        private Graphics graphics_0;
        private int int_0;
        private Pen pen_0 = new Pen(Color.Black);

        public void ChangeSettings(UserControl userControl_0)
        {
            CandleChartStyleSettings settings = (CandleChartStyleSettings) userControl_0;
            this.bool_0 = settings.cbFillCandleSticks.Checked;
            this.bool_1 = settings.cbDrawOutline.Checked;
        }

        public UserControl GetSettingsUI()
        {
            if (this.candleChartStyleSettings_0 == null)
            {
                this.candleChartStyleSettings_0 = new CandleChartStyleSettings();
            }
            this.candleChartStyleSettings_0.cbFillCandleSticks.Checked = this.bool_0;
            this.candleChartStyleSettings_0.cbDrawOutline.Checked = this.bool_1;
            return this.candleChartStyleSettings_0;
        }

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

        private void method_1(int int_1, int int_2, int int_3, int int_4, int int_5, Color color_0)
        {
            int num;
            int num2;
            if (this.bool_1 & (base.BarSpacing > 1))
            {
                this.pen_0.Color = Color.Black;
            }
            else
            {
                this.pen_0.Color = color_0;
            }
            Brush brush = new SolidBrush(color_0);
            Rectangle rect = new Rectangle();
            if (int_2 < int_5)
            {
                num = int_2;
                num2 = int_5;
            }
            else
            {
                num = int_5;
                num2 = int_2;
            }
            rect.X = int_1 - this.int_0;
            rect.Y = num;
            rect.Width = (this.int_0 * 2) + 1;
            rect.Height = num2 - num;
            bool flag = false;
            if (rect.Height == 0)
            {
                rect.Height = 1;
                flag = true;
            }
            if (!this.bool_1)
            {
                if (int_5 < int_2)
                {
                    if (this.bool_0)
                    {
                        if (!flag)
                        {
                            rect.Height++;
                        }
                        this.graphics_0.FillRectangle(brush, rect);
                        this.graphics_0.DrawLine(this.pen_0, int_1, int_3, int_1, int_4);
                    }
                    else
                    {
                        if (rect.Height == 1)
                        {
                            this.graphics_0.FillRectangle(brush, rect);
                        }
                        else
                        {
                            rect.Width--;
                            this.graphics_0.DrawRectangle(this.pen_0, rect);
                        }
                        this.graphics_0.DrawLine(this.pen_0, int_1, int_3, int_1, rect.Top);
                        this.graphics_0.DrawLine(this.pen_0, int_1, int_4, int_1, rect.Bottom);
                    }
                }
                else
                {
                    if (!flag)
                    {
                        rect.Height++;
                    }
                    this.graphics_0.FillRectangle(brush, rect);
                    this.graphics_0.DrawLine(this.pen_0, int_1, int_3, int_1, int_4);
                }
            }
            else if (int_5 < int_2)
            {
                if (this.bool_0)
                {
                    this.graphics_0.DrawLine(this.pen_0, int_1, int_3, int_1, int_4);
                    if (rect.Height == 1)
                    {
                        brush = new SolidBrush(Color.Black);
                        this.graphics_0.FillRectangle(brush, rect);
                    }
                    else
                    {
                        rect.Width--;
                        this.graphics_0.FillRectangle(brush, rect);
                        this.graphics_0.DrawRectangle(this.pen_0, rect);
                    }
                }
                else
                {
                    if (rect.Height == 1)
                    {
                        brush = new SolidBrush(Color.Black);
                        this.graphics_0.FillRectangle(brush, rect);
                    }
                    else
                    {
                        rect.Width--;
                        this.graphics_0.DrawRectangle(this.pen_0, rect);
                    }
                    this.graphics_0.DrawLine(this.pen_0, int_1, int_3, int_1, rect.Top);
                    this.graphics_0.DrawLine(this.pen_0, int_1, int_4, int_1, rect.Bottom);
                }
            }
            else
            {
                this.graphics_0.DrawLine(this.pen_0, int_1, int_3, int_1, int_4);
                if (rect.Height == 1)
                {
                    brush = new SolidBrush(Color.Black);
                    this.graphics_0.FillRectangle(brush, rect);
                }
                else
                {
                    rect.Width--;
                    this.graphics_0.FillRectangle(brush, rect);
                    this.graphics_0.DrawRectangle(this.pen_0, rect);
                }
            }
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.bool_0 = host.Get("CandleChartStyle.FillCandles", false);
            this.bool_1 = host.Get("CandleChartStyle.Outline", false);
        }

        public override void RenderBars(Graphics graphics_1)
        {
            int ghostBarXPosition;
            int num3;
            int num4;
            int num5;
            int num6;
            ChartPane pricePane = base.PricePane;
            this.graphics_0 = graphics_1;
            for (int i = base.LeftEdgeBar; i <= base.RightEdgeBar; i++)
            {
                ghostBarXPosition = base.ConvertBarToX(i);
                num3 = pricePane.ConvertValueToY(base.Bars.Open[i]);
                num4 = pricePane.ConvertValueToY(base.Bars.High[i]);
                num5 = pricePane.ConvertValueToY(base.Bars.Low[i]);
                num6 = pricePane.ConvertValueToY(base.Bars.Close[i]);
                this.method_1(ghostBarXPosition, num3, num4, num5, num6, base.GetBarColor(i));
            }
            if (base.ShouldDrawGhostBar)
            {
                ghostBarXPosition = base.GhostBarXPosition;
                num3 = pricePane.ConvertValueToY(base.Bars.Open.PartialValue);
                num4 = pricePane.ConvertValueToY(base.Bars.High.PartialValue);
                num5 = pricePane.ConvertValueToY(base.Bars.Low.PartialValue);
                num6 = pricePane.ConvertValueToY(base.Bars.Close.PartialValue);
                this.method_1(ghostBarXPosition, num3, num4, num5, num6, base.GhostBarColor);
            }
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("CandleChartStyle.FillCandles", this.bool_0);
            host.Set("CandleChartStyle.Outline", this.bool_1);
        }

        public override string FriendlyName
        {
            get
            {
                return "CandleStick";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.CandleChartStyle;
            }
        }
    }
}

