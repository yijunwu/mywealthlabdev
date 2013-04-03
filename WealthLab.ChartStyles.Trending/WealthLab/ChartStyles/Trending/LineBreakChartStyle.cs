namespace WealthLab.ChartStyles.Trending
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartStyles.Trending.Properties;

    public class LineBreakChartStyle : ChartStyle, ICustomSettings
    {
        private bool bool_0;
        private int int_0 = 3;
        private int int_1 = 6;
        private TLineBreak tlineBreak_0;
        private uxLineBreakChartStyle uxLineBreakChartStyle_0;

        public void ChangeSettings(UserControl userControl_0)
        {
            int num = this.int_0;
            bool flag = this.bool_0;
            this.int_0 = this.uxLineBreakChartStyle_0.LinesToBreak;
            this.bool_0 = this.uxLineBreakChartStyle_0.DrawHLC;
            if ((num != this.int_0) | (flag != this.bool_0))
            {
                this.Initialize();
            }
        }

        public UserControl GetSettingsUI()
        {
            if (this.uxLineBreakChartStyle_0 == null)
            {
                this.uxLineBreakChartStyle_0 = new uxLineBreakChartStyle();
                this.uxLineBreakChartStyle_0.LinesToBreak = this.int_0;
                this.uxLineBreakChartStyle_0.DrawHLC = this.bool_0;
            }
            return this.uxLineBreakChartStyle_0;
        }

        public override void Initialize()
        {
            this.tlineBreak_0 = new TLineBreak(base.Bars, this.int_0);
        }

        protected override void InitializeBarWidths()
        {
            this.int_1 = base.BarSpacing;
            if (this.bool_0)
            {
                for (int i = 0; i < base.Bars.Count; i++)
                {
                    base.SetBarWidth(i, this.int_1);
                }
            }
            else
            {
                for (int j = 0; j < base.Bars.Count; j++)
                {
                    if (this.tlineBreak_0.int_0[j] > 0)
                    {
                        base.SetBarWidth(j, this.int_1);
                    }
                    else
                    {
                        base.SetBarWidth(j, 0);
                    }
                }
            }
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.int_0 = host.Get("uxLineBreakChartStyle.LinesToBreak", 3);
            this.bool_0 = host.Get("uxLineBreakChartStyle.DrawHLC", false);
        }

        public override void RenderBars(Graphics graphics_0)
        {
            if (base.Bars.Count >= 1)
            {
                Pen pen;
                int height = 0;
                int num5 = 0;
                int num6 = 0;
                ColorFinder finder = new ColorFinder(this);
                pen = new Pen(Color.Black) {
                    Width = 1f,
                    StartCap = LineCap.Square,
                    EndCap = pen.StartCap
                };
                if (this.bool_0)
                {
                    int num8 = 0;
                    if (this.int_1 > 4)
                    {
                        num8 = this.int_1 / 2;
                    }
                    else if (this.int_1 > 1)
                    {
                        num8 = 1;
                    }
                    int leftEdgeBar = base.LeftEdgeBar;
                    SolidBrush brush = new SolidBrush(finder.ColorDown);
                    for (int i = base.LeftEdgeBar; i <= base.RightEdgeBar; i++)
                    {
                        pen.Color = Color.Black;
                        num5 = base.ConvertBarToX(i);
                        num6 = base.PricePane.ConvertValueToY(base.Bars.High[i]);
                        int num7 = base.PricePane.ConvertValueToY(base.Bars.Low[i]);
                        graphics_0.DrawLine(pen, num5, num6, num5, num7);
                        num7 = base.PricePane.ConvertValueToY(base.Bars.Close[i]);
                        graphics_0.DrawLine(pen, num5, num7, num5 + num8, num7);
                        LineBreak break2 = this.tlineBreak_0.Columns[i];
                        if (break2.Plotted)
                        {
                            num5 = base.ConvertBarToX(leftEdgeBar);
                            int num9 = base.ConvertBarToX(i);
                            num6 = base.PricePane.ConvertValueToY(break2.High);
                            height = base.PricePane.ConvertValueToY(break2.Low) - num6;
                            Rectangle rect = new Rectangle(num5, num6, num9 - num5, height);
                            if (break2.DirectionUp)
                            {
                                pen.Color = finder.ColorUp;
                                if (height < 2)
                                {
                                    graphics_0.DrawLine(pen, num5, num6, num9, num6);
                                }
                                else
                                {
                                    brush.Color = Color.FromArgb(50, finder.ColorUp);
                                    graphics_0.FillRectangle(brush, rect);
                                }
                            }
                            else
                            {
                                pen.Color = finder.ColorDown;
                                if (height < 2)
                                {
                                    graphics_0.DrawLine(pen, num5, num6, num9, num6);
                                }
                                else
                                {
                                    brush.Color = Color.FromArgb(50, finder.ColorDown);
                                    graphics_0.FillRectangle(brush, rect);
                                }
                            }
                            leftEdgeBar = i;
                        }
                    }
                }
                else
                {
                    SolidBrush brush2 = new SolidBrush(finder.ColorDown);
                    for (int j = base.LeftEdgeBar; j <= base.RightEdgeBar; j++)
                    {
                        LineBreak @break = this.tlineBreak_0.Columns[j];
                        if (@break.Plotted)
                        {
                            num5 = base.ConvertBarToX(j);
                            num6 = base.PricePane.ConvertValueToY(@break.High);
                            height = base.PricePane.ConvertValueToY(@break.Low) - num6;
                            Rectangle rectangle = new Rectangle(num5, num6, this.int_1, height);
                            if (@break.DirectionUp)
                            {
                                pen.Color = finder.ColorUp;
                                if (height < 2)
                                {
                                    graphics_0.DrawLine(pen, num5, num6, num5 + this.int_1, num6);
                                }
                                else
                                {
                                    graphics_0.DrawRectangle(pen, rectangle);
                                }
                            }
                            else
                            {
                                pen.Color = finder.ColorDown;
                                if (height < 2)
                                {
                                    graphics_0.DrawLine(pen, num5, num6, num5 + this.int_1, num6);
                                }
                                else
                                {
                                    graphics_0.DrawRectangle(pen, rectangle);
                                    graphics_0.FillRectangle(brush2, rectangle);
                                }
                            }
                        }
                    }
                }
                pen.Brush.Dispose();
                pen.Dispose();
            }
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("uxLineBreakChartStyle.LinesToBreak", this.int_0);
            host.Set("uxLineBreakChartStyle.DrawHLC", this.bool_0);
        }

        public override string FriendlyName
        {
            get
            {
                return "Line Break";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.ChartStyleLineBreak;
            }
        }
    }
}

