namespace WealthLab.ChartStyles.Trending
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartStyles.Trending.Properties;

    public class RenkoChartStyle : ChartStyle, ICustomSettings
    {
        private bool bool_0;
        private double double_0 = 1.0;
        private int int_0 = 6;
        private TRenko trenko_0;
        private uxRenkoChartStyle uxRenkoChartStyle_0;

        public void ChangeSettings(UserControl userControl_0)
        {
            double num = this.double_0;
            bool flag = this.bool_0;
            this.double_0 = this.uxRenkoChartStyle_0.RenkoPriceUnits;
            this.bool_0 = this.uxRenkoChartStyle_0.DrawHLC;
            if ((num != this.double_0) | (flag != this.bool_0))
            {
                this.Initialize();
            }
        }

        public UserControl GetSettingsUI()
        {
            if (this.uxRenkoChartStyle_0 == null)
            {
                this.uxRenkoChartStyle_0 = new uxRenkoChartStyle();
                this.uxRenkoChartStyle_0.RenkoPriceUnits = this.double_0;
                this.uxRenkoChartStyle_0.DrawHLC = this.bool_0;
            }
            return this.uxRenkoChartStyle_0;
        }

        public override void Initialize()
        {
            this.trenko_0 = new TRenko(base.Bars, this.double_0);
        }

        protected override void InitializeBarWidths()
        {
            this.int_0 = base.BarSpacing;
            if (this.bool_0)
            {
                for (int i = 0; i < base.Bars.Count; i++)
                {
                    base.SetBarWidth(i, this.int_0);
                }
            }
            else
            {
                if (base.BarSpacing > 2)
                {
                    this.int_0 = base.BarSpacing;
                }
                else
                {
                    this.int_0 = 3;
                }
                for (int j = 0; j < base.Bars.Count; j++)
                {
                    if (this.trenko_0.int_0[j] > 0)
                    {
                        base.SetBarWidth(j, this.int_0 * this.trenko_0.int_0[j]);
                    }
                    else
                    {
                        base.SetBarWidth(j, 0);
                    }
                }
                if (this.trenko_0.int_0[base.Bars.Count - 1] == 0)
                {
                    base.SetBarWidth(base.Bars.Count - 1, this.int_0);
                }
            }
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.double_0 = host.Get("uxRenkoChartStyle.RenkoPriceUnits", (double) 1.0);
            this.bool_0 = host.Get("uxRenkoChartStyle.DrawHLC", false);
        }

        public override void RenderBars(Graphics graphics_0)
        {
            Pen pen = new Pen(Color.Black) {
                Width = 1f
            };
            try
            {
                if (base.Bars.Count >= 1)
                {
                    int height = base.PricePane.ConvertValueToY(base.Bars.Close[base.LeftEdgeBar]) - base.PricePane.ConvertValueToY(base.Bars.Close[base.LeftEdgeBar] + this.double_0);
                    int ghostBarXPosition = base.ConvertBarToX(base.LeftEdgeBar);
                    double high = 0.0;
                    ColorFinder finder = new ColorFinder(this);
                    pen.StartCap = LineCap.Square;
                    pen.EndCap = pen.StartCap;
                    SolidBrush brush = new SolidBrush(Color.Red);
                    if (this.bool_0)
                    {
                        int num8 = 0;
                        if (this.int_0 > 4)
                        {
                            num8 = this.int_0 / 2;
                        }
                        else if (this.int_0 > 1)
                        {
                            num8 = 1;
                        }
                        int leftEdgeBar = base.LeftEdgeBar;
                        SolidBrush brush2 = new SolidBrush(finder.ColorDown);
                        for (int i = base.LeftEdgeBar + 1; i <= base.RightEdgeBar; i++)
                        {
                            pen.Color = Color.Black;
                            ghostBarXPosition = base.ConvertBarToX(i);
                            int num6 = base.PricePane.ConvertValueToY(base.Bars.High[i]);
                            int num7 = base.PricePane.ConvertValueToY(base.Bars.Low[i]);
                            graphics_0.DrawLine(pen, ghostBarXPosition, num6, ghostBarXPosition, num7);
                            num7 = base.PricePane.ConvertValueToY(base.Bars.Close[i]);
                            graphics_0.DrawLine(pen, ghostBarXPosition, num7, ghostBarXPosition + num8, num7);
                            Renko renko = this.trenko_0.Columns[i];
                            if (renko.Plotted)
                            {
                                ghostBarXPosition = base.ConvertBarToX(leftEdgeBar);
                                int num10 = base.ConvertBarToX(i);
                                num6 = base.PricePane.ConvertValueToY(renko.High);
                                int num11 = base.PricePane.ConvertValueToY(base.Bars.Close[base.LeftEdgeBar]) - base.PricePane.ConvertValueToY(base.Bars.Close[base.LeftEdgeBar] + (this.trenko_0.int_0[i] * this.double_0));
                                Rectangle rect = new Rectangle(ghostBarXPosition, num6, num10 - ghostBarXPosition, num11);
                                if (renko.DirectionUp)
                                {
                                    pen.Color = finder.ColorUp;
                                    if (height < 2)
                                    {
                                        graphics_0.DrawLine(pen, ghostBarXPosition, num6, num10, num6);
                                    }
                                    else
                                    {
                                        brush2.Color = Color.FromArgb(50, finder.ColorUp);
                                        graphics_0.FillRectangle(brush2, rect);
                                    }
                                }
                                else
                                {
                                    pen.Color = finder.ColorDown;
                                    if (height < 2)
                                    {
                                        graphics_0.DrawLine(pen, ghostBarXPosition, num6, num10, num6);
                                    }
                                    else
                                    {
                                        brush2.Color = Color.FromArgb(50, finder.ColorDown);
                                        graphics_0.FillRectangle(brush2, rect);
                                    }
                                }
                                leftEdgeBar = i;
                            }
                        }
                        if (base.ShouldDrawGhostBar)
                        {
                            pen.Color = Color.Black;
                            ghostBarXPosition = base.GhostBarXPosition;
                            int num14 = base.PricePane.ConvertValueToY(base.Bars.High.PartialValue);
                            int num15 = base.PricePane.ConvertValueToY(base.Bars.Low.PartialValue);
                            graphics_0.DrawLine(pen, ghostBarXPosition, num14, ghostBarXPosition, num15);
                            num15 = base.PricePane.ConvertValueToY(base.Bars.Close.PartialValue);
                            graphics_0.DrawLine(pen, ghostBarXPosition, num15, ghostBarXPosition + num8, num15);
                        }
                    }
                    else
                    {
                        ghostBarXPosition = base.ConvertBarToX(base.RightEdgeBar) - (this.int_0 / 2);
                        for (int j = base.RightEdgeBar; j > base.LeftEdgeBar; j--)
                        {
                            Renko renko2 = this.trenko_0.Columns[j];
                            if (this.trenko_0.int_0[j] > 0)
                            {
                                if (renko2.DirectionUp)
                                {
                                    pen.Color = finder.ColorUp;
                                    high = renko2.High;
                                    for (int k = 1; k <= this.trenko_0.int_0[j]; k++)
                                    {
                                        graphics_0.DrawRectangle(pen, ghostBarXPosition, base.PricePane.ConvertValueToY(high), this.int_0, height);
                                        ghostBarXPosition -= this.int_0;
                                        high -= this.double_0;
                                    }
                                }
                                else
                                {
                                    pen.Color = finder.ColorDown;
                                    brush.Color = pen.Color;
                                    high = renko2.Low + this.double_0;
                                    for (int m = 1; m <= this.trenko_0.int_0[j]; m++)
                                    {
                                        Rectangle rectangle = new Rectangle(ghostBarXPosition, base.PricePane.ConvertValueToY(high), this.int_0, height);
                                        graphics_0.DrawRectangle(pen, rectangle);
                                        graphics_0.FillRectangle(brush, rectangle);
                                        ghostBarXPosition -= this.int_0;
                                        high += this.double_0;
                                    }
                                }
                            }
                        }
                        if (base.ShouldDrawGhostBar)
                        {
                            int num17 = base.Bars.Count - 1;
                            double partialValue = base.Bars.High.PartialValue;
                            double num19 = base.Bars.Low.PartialValue;
                            double num20 = base.Bars.Close[num17];
                            if (base.Bars.Close.PartialValue > num20)
                            {
                                pen.Color = finder.ColorUp;
                            }
                            else
                            {
                                pen.Color = finder.ColorDown;
                            }
                            brush.Color = pen.Color;
                            int num12 = (base.BarSpacing / 2) - 3;
                            if (num12 < 1)
                            {
                                num12 = 1;
                            }
                            ghostBarXPosition = base.GhostBarXPosition;
                            int num21 = base.PricePane.ConvertValueToY(base.Bars.Close.PartialValue);
                            int num22 = base.PricePane.ConvertValueToY(num20);
                            int num23 = base.PricePane.ConvertValueToY(partialValue);
                            int num24 = base.PricePane.ConvertValueToY(num19);
                            Rectangle rectangle2 = new Rectangle {
                                X = ghostBarXPosition - num12,
                                Y = num22,
                                Width = (num12 * 2) + 1,
                                Height = Math.Abs((int) (num21 - num22))
                            };
                            graphics_0.DrawLine(pen, ghostBarXPosition, num24, ghostBarXPosition, num23);
                            if (rectangle2.Height == 0)
                            {
                                rectangle2.Height = 1;
                            }
                            rectangle2.Height++;
                            graphics_0.FillRectangle(brush, rectangle2);
                        }
                    }
                }
            }
            finally
            {
                pen.Brush.Dispose();
                pen.Dispose();
            }
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("uxRenkoChartStyle.RenkoPriceUnits", this.double_0);
            host.Set("uxRenkoChartStyle.DrawHLC", this.bool_0);
        }

        public override string FriendlyName
        {
            get
            {
                return "Renko";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.ChartStyleRenko;
            }
        }
    }
}

