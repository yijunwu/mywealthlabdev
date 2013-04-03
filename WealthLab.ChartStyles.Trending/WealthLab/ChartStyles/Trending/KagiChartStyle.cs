namespace WealthLab.ChartStyles.Trending
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartStyles.Trending.Properties;

    public class KagiChartStyle : ChartStyle, ICustomSettings
    {
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private double double_0 = 3.0;
        private Graphics graphics_0;
        private int int_0 = 1;
        private int int_1 = 3;
        private int int_2 = 6;
        private int int_3 = 5;
        private KagiReverseType kagiReverseType_0 = KagiReverseType.Percent;
        private Pen pen_0 = new Pen(Color.Black);
        private TKagi tkagi_0;
        private uxKagiChartStyle uxKagiChartStyle_0;

        public void ChangeSettings(UserControl userControl_0)
        {
            this.LineWidth = this.uxKagiChartStyle_0.KagiLineWidth;
            this.double_0 = this.uxKagiChartStyle_0.KagiReversalAmount;
            this.kagiReverseType_0 = this.uxKagiChartStyle_0.KagiRevType;
            this.bool_0 = this.uxKagiChartStyle_0.KagiOneColor;
            this.bool_1 = this.uxKagiChartStyle_0.DrawHLC;
            this.bool_2 = this.uxKagiChartStyle_0.ShowArrows;
            this.kagiReverseType_0 = this.uxKagiChartStyle_0.KagiRevType;
            this.int_3 = this.uxKagiChartStyle_0.KagiATRPeriod;
        }

        public UserControl GetSettingsUI()
        {
            if (this.uxKagiChartStyle_0 == null)
            {
                this.uxKagiChartStyle_0 = new uxKagiChartStyle();
                this.uxKagiChartStyle_0.KagiLineWidth = this.int_0;
                this.uxKagiChartStyle_0.KagiReversalAmount = this.double_0;
                this.uxKagiChartStyle_0.KagiOneColor = this.bool_0;
                this.uxKagiChartStyle_0.DrawHLC = this.bool_1;
                this.uxKagiChartStyle_0.KagiRevType = this.kagiReverseType_0;
                this.uxKagiChartStyle_0.KagiATRPeriod = this.int_3;
                this.uxKagiChartStyle_0.ShowArrows = this.bool_2;
            }
            return this.uxKagiChartStyle_0;
        }

        public override void Initialize()
        {
            this.tkagi_0 = new TKagi(base.Bars, this.kagiReverseType_0, this.double_0, this.int_3);
        }

        protected override void InitializeBarWidths()
        {
            this.int_2 = base.BarSpacing;
            if (this.bool_1)
            {
                for (int i = 0; i < base.Bars.Count; i++)
                {
                    base.SetBarWidth(i, this.int_2);
                }
            }
            else
            {
                this.int_2 = 2 * this.LineWidthYang;
                if (this.int_2 < base.BarSpacing)
                {
                    this.int_2 = base.BarSpacing;
                }
                if ((((double) this.LineWidth) % 2.0) != (((double) this.int_2) % 2.0))
                {
                    this.int_2++;
                }
                for (int j = 0; j < base.Bars.Count; j++)
                {
                    if (this.tkagi_0.int_1[j] > 0)
                    {
                        base.SetBarWidth(j, this.int_2);
                    }
                    else
                    {
                        base.SetBarWidth(j, 0);
                    }
                }
            }
        }

        private void method_1(Kagi kagi_0, int int_4, out int int_5, out int int_6)
        {
            if (kagi_0.DirectionUp)
            {
                int_5 = base.PricePane.ConvertValueToY(this.tkagi_0.Columns[int_4].Low);
                int_6 = base.PricePane.ConvertValueToY(this.tkagi_0.Columns[int_4].High);
            }
            else
            {
                int_5 = base.PricePane.ConvertValueToY(this.tkagi_0.Columns[int_4].High);
                int_6 = base.PricePane.ConvertValueToY(this.tkagi_0.Columns[int_4].Low);
            }
        }

        private void method_2(int int_4, int int_5, int int_6, int int_7, int int_8)
        {
            this.graphics_0.DrawLine(this.pen_0, int_5, int_6, int_5, int_7);
            this.graphics_0.DrawLine(this.pen_0, int_5, int_8, int_5 + int_4, int_8);
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.LineWidth = host.Get("uxKagiChartStyle.KagiLineWidth", 1);
            this.double_0 = host.Get("uxKagiChartStyle.KagiReversalAmount", (double) 3.0);
            this.bool_0 = host.Get("uxKagiChartStyle.KagiSingleColor", false);
            this.int_3 = host.Get("uxKagiChartStyle.KagiATRPeriod", 5);
            this.bool_1 = host.Get("uxKagiChartStyle.DrawHLC", false);
            this.bool_2 = host.Get("uxKagiChartStyle.ShowArrows", false);
            if (host.ContainsKey("uxKagiChartStyle.KagiReverseType"))
            {
                switch (host.Get("uxKagiChartStyle.KagiReverseType", KagiReverseType.Percent.ToString()))
                {
                    case "Percent":
                        this.kagiReverseType_0 = KagiReverseType.Percent;
                        return;

                    case "Points":
                        this.kagiReverseType_0 = KagiReverseType.Points;
                        break;

                    case "ATR":
                        this.kagiReverseType_0 = KagiReverseType.ATR;
                        break;
                }
            }
            else if (host.Get("uxKagiChartStyle.KagiReverseIsPercent", true))
            {
                this.kagiReverseType_0 = KagiReverseType.Percent;
            }
            else
            {
                this.kagiReverseType_0 = KagiReverseType.Points;
            }
        }

        public override void RenderBars(Graphics graphics_1)
        {
            if (base.Bars.Count >= 1)
            {
                int num6;
                this.graphics_0 = graphics_1;
                Kagi kagi = this.tkagi_0.Columns[base.LeftEdgeBar];
                int num4 = 0;
                int num8 = base.ConvertBarToX(base.LeftEdgeBar);
                int num7 = base.PricePane.ConvertValueToY(base.Bars.Close[base.LeftEdgeBar]);
                int num12 = 0;
                bool isBullish = kagi.IsBullish;
                Pen pen = new Pen(Color.Black);
                this.pen_0.Color = Color.Black;
                this.pen_0.Width = 1f;
                ColorFinder finder = new ColorFinder(this);
                if (this.bool_0)
                {
                    pen.Color = finder.ColorUp;
                }
                if (isBullish)
                {
                    pen.Width = this.int_1;
                }
                else
                {
                    pen.Width = this.int_0;
                }
                pen.StartCap = LineCap.Square;
                pen.EndCap = pen.StartCap;
                int num = 0;
                if (!this.bool_1)
                {
                    this.method_1(kagi, base.LeftEdgeBar, out num6, out num7);
                    graphics_1.DrawLine(pen, num8, num6, num8, num7);
                }
                else
                {
                    if (this.int_2 > 4)
                    {
                        num = this.int_2 / 2;
                    }
                    else if (this.int_2 > 1)
                    {
                        num = 1;
                    }
                    if (kagi.Plotted)
                    {
                        this.method_1(kagi, base.LeftEdgeBar, out num6, out num7);
                        graphics_1.DrawLine(pen, num8, num6, num8, num7);
                    }
                    else
                    {
                        for (int j = base.LeftEdgeBar - 1; j >= 0; j--)
                        {
                            kagi = this.tkagi_0.Columns[j];
                            if (kagi.Plotted)
                            {
                                this.method_1(kagi, j, out num6, out num7);
                                break;
                            }
                        }
                    }
                }
                int num3 = -1;
                int num2 = -1;
                Font font = new Font("Wingdings", 8f, FontStyle.Bold);
                SolidBrush brush2 = new SolidBrush(finder.ColorDown);
                SolidBrush brush = new SolidBrush(finder.ColorUp);
                string s = Convert.ToChar(0xea).ToString();
                string str = Convert.ToChar(0xe9).ToString();
                int num13 = 3 + (this.int_2 / 2);
                if (this.int_2 > 8)
                {
                    num13 = 4;
                }
                for (int i = base.LeftEdgeBar; i <= base.RightEdgeBar; i++)
                {
                    kagi = this.tkagi_0.Columns[i];
                    num4 = base.ConvertBarToX(i);
                    if (this.bool_1)
                    {
                        int num10 = base.PricePane.ConvertValueToY(base.Bars.High[i]);
                        int num11 = base.PricePane.ConvertValueToY(base.Bars.Low[i]);
                        this.method_2(num, num4, num10, num11, base.PricePane.ConvertValueToY(base.Bars.Close[i]));
                        if (this.bool_2 && kagi.Reversed)
                        {
                            if (kagi.DirectionUp)
                            {
                                graphics_1.DrawString(str, font, brush, (float) (num4 - num13), (float) (num11 + 5));
                            }
                            else
                            {
                                graphics_1.DrawString(s, font, brush2, (float) (num4 - num13), (float) (num10 - 14));
                            }
                        }
                    }
                    if ((this.tkagi_0.int_1[i] > 0) || (i == base.RightEdgeBar))
                    {
                        if (i == base.RightEdgeBar)
                        {
                            if (num3 > num2)
                            {
                                graphics_1.DrawLine(pen, num8, num7, num4, num7);
                                break;
                            }
                            if (num2 > num3)
                            {
                                num4 = base.ConvertBarToX(num2);
                            }
                        }
                        graphics_1.DrawLine(pen, num8, num7, num4, num7);
                        this.method_1(kagi, i, out num6, out num7);
                        if (kagi.DirectionUp)
                        {
                            if (!this.bool_0)
                            {
                                pen.Color = finder.ColorUp;
                            }
                            if ((!isBullish && kagi.IsBullish) && (kagi.YangLevel < kagi.High))
                            {
                                num12 = base.PricePane.ConvertValueToY(kagi.YangLevel);
                                graphics_1.DrawLine(pen, num4, num6, num4, num12);
                                pen.Width = this.int_1;
                                graphics_1.DrawLine(pen, num4, num12, num4, num7);
                            }
                            else
                            {
                                graphics_1.DrawLine(pen, num4, num6, num4, num7);
                            }
                        }
                        else
                        {
                            if (!this.bool_0)
                            {
                                pen.Color = finder.ColorDown;
                            }
                            if ((isBullish && !kagi.IsBullish) && (kagi.YinLevel > kagi.Low))
                            {
                                num12 = base.PricePane.ConvertValueToY(kagi.YinLevel);
                                graphics_1.DrawLine(pen, num4, num6, num4, num12);
                                pen.Width = this.int_0;
                                graphics_1.DrawLine(pen, num4, num12, num4, num7);
                            }
                            else
                            {
                                graphics_1.DrawLine(pen, num4, num6, num4, num7);
                            }
                        }
                        num8 = num4;
                        isBullish = kagi.IsBullish;
                        if (kagi.Plotted)
                        {
                            num3 = i;
                        }
                        if (kagi.Reversed)
                        {
                            num2 = i;
                        }
                    }
                }
                if (base.ShouldDrawGhostBar && this.bool_1)
                {
                    this.pen_0.Color = Color.Fuchsia;
                    this.method_2(num, base.GhostBarXPosition, base.PricePane.ConvertValueToY(base.Bars.High.PartialValue), base.PricePane.ConvertValueToY(base.Bars.Low.PartialValue), base.PricePane.ConvertValueToY(base.Bars.Close.PartialValue));
                }
                pen.Dispose();
            }
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("uxKagiChartStyle.KagiLineWidth", this.LineWidth);
            host.Set("uxKagiChartStyle.KagiReversalAmount", this.double_0);
            host.Set("uxKagiChartStyle.KagiReverseType", this.kagiReverseType_0.ToString());
            host.Set("uxKagiChartStyle.KagiATRPeriod", this.int_3);
            host.Set("uxKagiChartStyle.KagiSingleColor", this.bool_0);
            host.Set("uxKagiChartStyle.DrawHLC", this.bool_1);
            host.Set("uxKagiChartStyle.ShowArrows", this.bool_2);
        }

        public override string FriendlyName
        {
            get
            {
                return "Kagi";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.ChartStyleKagi;
            }
        }

        public int LineWidth
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
                if (this.int_0 < 4)
                {
                    this.int_1 = this.int_0 + 2;
                }
                else
                {
                    this.int_1 = this.int_0 + 4;
                }
            }
        }

        public int LineWidthYang
        {
            get
            {
                return this.int_1;
            }
        }
    }
}

