namespace WealthLab.ChartStyles.Trending
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartStyles.Trending.Properties;

    public class PnFChartStyle : ChartStyle, ICustomSettings
    {
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private double double_0 = 1.0;
        private double double_1;
        private int int_0 = 3;
        private int int_1 = 6;
        private int int_2;
        private string string_0 = "Close";
        private TPnF tpnF_0;
        private uxPnFChartStyle uxPnFChartStyle_0;

        public void ChangeSettings(UserControl userControl_0)
        {
            double num = this.double_0;
            int num2 = this.int_0;
            bool flag = this.bool_1;
            bool flag2 = this.bool_0;
            this.bool_0 = this.uxPnFChartStyle_0.TraditionalSettings;
            this.bool_1 = this.uxPnFChartStyle_0.LogMethod;
            this.double_0 = this.uxPnFChartStyle_0.BoxSize;
            this.int_0 = this.uxPnFChartStyle_0.ReversalBoxes;
            this.bool_3 = this.uxPnFChartStyle_0.DrawTrendLines;
            this.bool_4 = this.uxPnFChartStyle_0.DrawTargets;
            this.bool_5 = this.uxPnFChartStyle_0.DrawSettings;
            this.bool_6 = this.uxPnFChartStyle_0.DrawGrid;
            this.string_0 = this.uxPnFChartStyle_0.PriceField;
            if ((((num != this.double_0) | (num2 != this.int_0)) | (flag != this.bool_1)) | (flag2 != this.bool_0))
            {
                this.Initialize();
            }
        }

        public UserControl GetSettingsUI()
        {
            if (this.uxPnFChartStyle_0 == null)
            {
                this.uxPnFChartStyle_0 = new uxPnFChartStyle();
                this.uxPnFChartStyle_0.TraditionalSettings = this.bool_0;
                this.uxPnFChartStyle_0.LogMethod = this.bool_1;
                this.uxPnFChartStyle_0.BoxSize = this.double_0;
                this.uxPnFChartStyle_0.ReversalBoxes = this.int_0;
                this.uxPnFChartStyle_0.DrawTrendLines = this.bool_3;
                this.uxPnFChartStyle_0.DrawSettings = this.bool_5;
                this.uxPnFChartStyle_0.DrawGrid = this.bool_6;
                this.uxPnFChartStyle_0.DrawTargets = this.bool_4;
                this.uxPnFChartStyle_0.PriceField = this.string_0;
            }
            return this.uxPnFChartStyle_0;
        }

        public override void Initialize()
        {
            this.int_2 = base.Bars.Count;
            this.bool_2 = this.bool_1;
            if (base.Bars.Count >= 2)
            {
                ControlPrice close;
                double num = base.Bars.High[base.Bars.Count - 1];
                if (this.bool_0)
                {
                    if (num < 0.25)
                    {
                        this.double_0 = 0.0625;
                    }
                    else if (num <= 1.0)
                    {
                        this.double_0 = 0.125;
                    }
                    else if (num <= 5.0)
                    {
                        this.double_0 = 0.25;
                    }
                    else if (num <= 20.0)
                    {
                        this.double_0 = 0.5;
                    }
                    else if (num <= 100.0)
                    {
                        this.double_0 = 1.0;
                    }
                    else if (num <= 200.0)
                    {
                        this.double_0 = 2.0;
                    }
                    else if (num <= 500.0)
                    {
                        this.double_0 = 4.0;
                    }
                    else if (num <= 1000.0)
                    {
                        this.double_0 = 5.0;
                    }
                    else if (num <= 25000.0)
                    {
                        this.double_0 = 50.0;
                    }
                    else
                    {
                        this.double_0 = 500.0;
                    }
                    this.bool_2 = false;
                }
                if (this.string_0 == "Close")
                {
                    close = ControlPrice.Close;
                }
                else
                {
                    close = ControlPrice.HighLow;
                }
                this.tpnF_0 = new TPnF(base.Bars, close, this.double_0, this.int_0, this.bool_2);
                if (this.bool_2)
                {
                    this.double_1 = Math.Log(1.0 + (this.double_0 / 100.0));
                }
                else
                {
                    this.double_1 = this.double_0;
                }
            }
        }

        protected override void InitializeBarWidths()
        {
            if (this.tpnF_0 != null)
            {
                this.int_1 = Math.Max(base.BarSpacing, 8);
                for (int i = 0; i < this.int_2; i++)
                {
                    if (this.tpnF_0.Columns[i].Plotted)
                    {
                        base.SetBarWidth(i, this.int_1);
                    }
                    else
                    {
                        base.SetBarWidth(i, 0);
                    }
                }
            }
        }

        private int method_1(double double_2)
        {
            if (!this.bool_2)
            {
                return base.PricePane.ConvertValueToY(double_2);
            }
            return base.PricePane.ConvertValueToY(Math.Pow(2.7182818284590451, double_2));
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.int_0 = host.Get("uxPnFChartStyle.ReversalBoxes", 3);
            this.double_0 = host.Get("uxPnFChartStyle.BoxSize", (double) 1.0);
            this.bool_0 = host.Get("uxPnFChartStyle.TraditionalSettings", false);
            this.bool_1 = host.Get("uxPnFChartStyle.LogMethod", false);
            this.bool_3 = host.Get("uxPnFChartStyle.ShowTrendlines", false);
            this.bool_6 = host.Get("uxPnFChartStyle.ShowGrid", true);
            this.bool_5 = host.Get("uxPnFChartStyle.ShowSettings", false);
            this.bool_4 = host.Get("uxPnFChartStyle.ShowTargets", false);
            this.string_0 = host.Get("uxPnFChartStyle.PriceField", "Close");
        }

        public override void RenderBars(Graphics graphics_0)
        {
            double low;
            Pen pen;
            double num12;
            int num14;
            int num16;
            double num20;
            int bar;
            int num = 0;
            int top = 0;
            if (this.tpnF_0 == null)
            {
                return;
            }
            if (this.tpnF_0.Columns.Count < 1)
            {
                return;
            }
            base.PricePane.LogScale = this.bool_2;
            if (!this.bool_2)
            {
                low = this.tpnF_0.RoundToBox(base.Bars.Close[base.LeftEdgeBar]);
                top = base.PricePane.ConvertValueToY(low);
                low -= this.double_1;
                top = base.PricePane.ConvertValueToY(low) - top;
            }
            else
            {
                low = this.tpnF_0.RoundToBox(Math.Log(base.Bars.Close[base.LeftEdgeBar]));
                top = base.PricePane.ConvertValueToY(Math.Pow(2.7182818284590451, low));
                low -= this.double_1;
                top = base.PricePane.ConvertValueToY(Math.Pow(2.7182818284590451, low)) - top;
            }
            ColorFinder finder = new ColorFinder(this);
            pen = new Pen(Color.Black) {
                Width = 1f,
                StartCap = LineCap.Round,
                EndCap = pen.StartCap
            };
            int num8 = Convert.ToInt32(Math.Ceiling((double) (((double) this.int_1) / 2.0)));
            int num5 = Convert.ToInt32(Math.Ceiling((double) (((double) top) / 2.0)));
            int height = top - 1;
            SolidBrush brush = new SolidBrush(finder.ColorDown);
            int num11 = Math.Min(base.RightEdgeBar, this.int_2 - 1);
            if (!this.bool_6)
            {
                goto Label_03FF;
            }
            base.HorizontalGridlines = false;
            base.VerticalGridlines = false;
            pen.Color = Color.LightGray;
            int num19 = base.ConvertBarToX(base.LeftEdgeBar);
            int num15 = base.ConvertBarToX(num11);
            double d = 0.0;
            double num22 = 1E+15;
            for (int i = base.LeftEdgeBar; i <= num11; i++)
            {
                if (base.Bars.High[i] > d)
                {
                    d = base.Bars.High[i];
                }
                if (base.Bars.Low[i] < num22)
                {
                    num22 = base.Bars.Low[i];
                }
            }
            bool flag2 = true;
            double num18 = this.double_1;
            Font font = new Font("System", 7f);
            brush.Color = Color.Black;
            if (!this.bool_2)
            {
                num20 = d + num18;
                num12 = Math.Floor((double) (num22 / num18)) * num18;
            }
            else
            {
                num20 = Math.Log(d) + num18;
                num12 = (Math.Floor((double) (Math.Log(num22) / num18)) * num18) + num18;
                while (true)
                {
                    double num13 = Math.Pow(2.7182818284590451, num12);
                    top = base.PricePane.ConvertValueToY(num13);
                    num14 = top + num5;
                    if (flag2)
                    {
                        graphics_0.DrawString(num13.ToString("#,##0.####"), font, brush, (float) (num15 + this.int_1), (float) (top - 6));
                    }
                    graphics_0.DrawLine(pen, num19, num14, num15 + this.int_1, num14);
                    num12 += num18;
                    flag2 = !flag2;
                    if (num12 >= num20)
                    {
                        goto Label_0396;
                    }
                }
            }
            do
            {
                top = base.PricePane.ConvertValueToY(num12);
                num14 = top - num5;
                if (flag2)
                {
                    graphics_0.DrawString(num12.ToString("#,##0.####"), font, brush, (float) (num15 + this.int_1), (float) (top - 6));
                }
                graphics_0.DrawLine(pen, num19, num14, num15 + num8, num14);
                num12 += num18;
                flag2 = !flag2;
            }
            while (num12 < num20);
        Label_0396:
            num16 = base.PricePane.Top + base.PricePane.Height;
            for (int j = base.LeftEdgeBar; j <= num11; j++)
            {
                PnF nf2 = this.tpnF_0.Columns[j];
                if (nf2.Plotted)
                {
                    num = base.ConvertBarToX(j);
                    graphics_0.DrawLine(pen, num - num8, num14, num - num8, num16);
                }
            }
        Label_03FF:
            bar = num11;
            int col = this.tpnF_0.Columns.Count - 1;
            for (int k = base.LeftEdgeBar; k <= num11; k++)
            {
                int num35;
                PnF nf = this.tpnF_0.Columns[k];
                if (!nf.Plotted)
                {
                    continue;
                }
                bar = nf.Bar;
                col = nf.Col;
                bool flag = true;
                num = base.ConvertBarToX(k);
                if (!nf.DirectionUp)
                {
                    goto Label_0544;
                }
                pen.Color = finder.ColorUp;
                low = nf.Low;
                goto Label_0534;
            Label_0488:
                low = this.tpnF_0.RoundToBox(low + this.double_1);
                if (flag && nf.OneStepBack)
                {
                    pen.Color = finder.ColorDown;
                    top = this.method_1(low);
                    graphics_0.DrawEllipse(pen, num - num8, top + num5, this.int_1, height);
                    pen.Color = finder.ColorUp;
                }
                flag = false;
                if (low <= nf.High)
                {
                    goto Label_0534;
                }
                continue;
            Label_04F5:
                num35 = (top + num5) - 1;
                int num36 = top - num5;
                int num37 = num - num8;
                int num38 = num + num8;
                graphics_0.DrawLine(pen, num37, num36, num38, num35);
                graphics_0.DrawLine(pen, num38, num36, num37, num35);
                goto Label_0488;
            Label_0534:
                top = this.method_1(low);
                if (flag)
                {
                    goto Label_0488;
                }
                goto Label_04F5;
            Label_0544:
                pen.Color = finder.ColorDown;
                low = nf.High;
                goto Label_060F;
            Label_055F:
                graphics_0.DrawEllipse(pen, num - num8, top + num5, this.int_1, height);
                flag = false;
                low = this.tpnF_0.RoundToBox(low - this.double_1);
                if (low >= this.tpnF_0.RoundToBox(nf.Low + this.double_1))
                {
                    goto Label_060F;
                }
                continue;
            Label_05AB:
                if (nf.OneStepBack)
                {
                    int num6 = (top + num5) - 1;
                    int num7 = top - num5;
                    int num9 = num - num8;
                    int num10 = num + num8;
                    pen.Color = finder.ColorUp;
                    graphics_0.DrawLine(pen, num9, num7, num10, num6);
                    graphics_0.DrawLine(pen, num10, num7, num9, num6);
                    pen.Color = finder.ColorDown;
                }
                goto Label_055F;
            Label_060F:
                top = this.method_1(low);
                if (!flag)
                {
                    goto Label_055F;
                }
                goto Label_05AB;
            }
            if (this.bool_3)
            {
                foreach (PnFTrendLine line in this.tpnF_0.TrendLines)
                {
                    int endBar = line.EndBar;
                    if (endBar == -1)
                    {
                        endBar = bar;
                    }
                    endBar = Math.Min(endBar, bar);
                    int endColumn = line.EndColumn;
                    if (endColumn == -1)
                    {
                        endColumn = col;
                    }
                    int num32 = Math.Min(endColumn, col) - line.StartColumn;
                    if ((line.Bar1 < num11) && (endBar > base.LeftEdgeBar))
                    {
                        if (line.Accel == 0)
                        {
                            pen.Width = 2f;
                        }
                        else
                        {
                            pen.Width = 1f;
                        }
                        if ((num32 > 1) || (endBar == bar))
                        {
                            int num34;
                            num = base.ConvertBarToX(line.Bar1);
                            int num31 = base.ConvertBarToX(endBar);
                            double num33 = this.double_1 * num32;
                            if (!line.IsRising)
                            {
                                num33 = -num33;
                                pen.Color = Color.Red;
                            }
                            else
                            {
                                pen.Color = Color.Blue;
                            }
                            if (!this.bool_2)
                            {
                                top = base.PricePane.ConvertValueToY(line.Price1);
                                num34 = base.PricePane.ConvertValueToY(line.Price1 + num33);
                            }
                            else
                            {
                                top = base.PricePane.ConvertValueToY(Math.Pow(2.7182818284590451, line.Price1));
                                num34 = base.PricePane.ConvertValueToY(Math.Pow(2.7182818284590451, line.Price1 + num33));
                            }
                            graphics_0.DrawLine(pen, num, top, num31, num34);
                        }
                    }
                }
            }
            if (this.bool_4)
            {
                foreach (PFTarget target in this.tpnF_0.PFTargets)
                {
                    if ((target.Bar < num11) && (target.Bar > base.LeftEdgeBar))
                    {
                        int num24;
                        pen.Width = 1f;
                        pen.Color = Color.Black;
                        num = (base.ConvertBarToX(target.Bar) - num8) + 1;
                        if (!this.bool_2)
                        {
                            top = base.PricePane.ConvertValueToY(target.TargetPrice);
                            num24 = base.PricePane.ConvertValueToY(target.ActivatePrice) - num5;
                        }
                        else
                        {
                            top = base.PricePane.ConvertValueToY(Math.Pow(2.7182818284590451, target.TargetPrice));
                            num24 = base.PricePane.ConvertValueToY(Math.Pow(2.7182818284590451, target.ActivatePrice)) - num5;
                        }
                        graphics_0.DrawLine(pen, num, top, num, num24);
                        int num25 = (base.ConvertBarToX(target.EntryColumnBar) - num8) - 1;
                        graphics_0.DrawLine(pen, num, num24, num25, num24);
                    }
                }
            }
            if (this.bool_5 && (num > 50))
            {
                num = (int) Math.Round((double) (((double) base.ConvertBarToX(num11)) / 2.0));
                top = base.PricePane.Top;
                string str = " x ";
                if (this.bool_2)
                {
                    str = "% x ";
                }
                string s = this.double_0.ToString("#,##0.0##") + str + this.int_0.ToString();
                if (this.bool_2)
                {
                    s = this.double_0.ToString("#,##0.0##") + str + this.int_0.ToString();
                }
                Font font2 = new Font("System", 8f);
                brush.Color = Color.Black;
                graphics_0.DrawString(s, font2, brush, (float) num, (float) top);
            }
            pen.Brush.Dispose();
            pen.Dispose();
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("uxPnFChartStyle.ReversalBoxes", this.int_0);
            host.Set("uxPnFChartStyle.BoxSize", this.double_0);
            host.Set("uxPnFChartStyle.TraditionalSettings", this.bool_0);
            host.Set("uxPnFChartStyle.LogMethod", this.bool_1);
            host.Set("uxPnFChartStyle.ShowTrendlines", this.bool_3);
            host.Set("uxPnFChartStyle.ShowGrid", this.bool_6);
            host.Set("uxPnFChartStyle.ShowSettings", this.bool_5);
            host.Set("uxPnFChartStyle.ShowTargets", this.bool_4);
            host.Set("uxPnFChartStyle.PriceField", this.string_0);
        }

        public override string FriendlyName
        {
            get
            {
                return "Point & Figure";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.ChartStylePnF;
            }
        }
    }
}

