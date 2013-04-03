namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WealthLab;

    public class WSDPlottedFundamentalItem : WSDrawingObject
    {
        private Color color_0;
        private IList<FundamentalItem> ilist_0;
        private int int_0;
        private LineStyle lineStyle_0;

        public WSDPlottedFundamentalItem(IList<FundamentalItem> items, Color color, LineStyle style, int width)
        {
            this.ilist_0 = items;
            this.color_0 = color;
            this.lineStyle_0 = style;
            this.int_0 = width;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            DateTime time = chartRenderer_0.Bars.Date[chartRenderer_0.LeftEdgeBar];
            DateTime time2 = chartRenderer_0.Bars.Date[chartRenderer_0.RightEdgeBar];
            Color color = Color.FromArgb(30, this.color_0.R, this.color_0.G, this.color_0.B);
            for (int i = 0; i < this.ilist_0.Count; i++)
            {
                FundamentalItem item = this.ilist_0[i];
                bool flag = false;
                if ((item.Date >= time) && (item.Date <= time2))
                {
                    flag = true;
                }
                else if (((i < (this.ilist_0.Count - 1)) && (this.ilist_0[i + 1].Date > time)) && (item.Date <= time2))
                {
                    flag = true;
                }
                else if ((item.Date < time) && (i == (this.ilist_0.Count - 1)))
                {
                    flag = true;
                }
                if (flag)
                {
                    int num2;
                    int num3;
                    int num4;
                    int num6;
                    if (item.Date < time)
                    {
                        num6 = chartRenderer_0.ConvertBarToX(chartRenderer_0.LeftEdgeBar);
                    }
                    else
                    {
                        num6 = chartRenderer_0.ConvertBarToX(item.Bar);
                    }
                    if (i == (this.ilist_0.Count - 1))
                    {
                        num2 = chartRenderer_0.ConvertBarToX(chartRenderer_0.RightEdgeBar);
                    }
                    else if (this.ilist_0[i + 1].Date <= time2)
                    {
                        num2 = chartRenderer_0.ConvertBarToX(this.ilist_0[i + 1].Bar);
                    }
                    else
                    {
                        num2 = chartRenderer_0.ConvertBarToX(chartRenderer_0.RightEdgeBar);
                    }
                    if (item.Value > 0.0)
                    {
                        num4 = pane.ConvertValueToY(item.Value);
                        num3 = pane.ConvertValueToY(0.0);
                    }
                    else
                    {
                        num4 = pane.ConvertValueToY(0.0);
                        num3 = pane.ConvertValueToY(item.Value);
                    }
                    int height = num3 - num4;
                    if (height < this.int_0)
                    {
                        height = this.int_0;
                    }
                    Rectangle rect = new Rectangle(num6, num4, num2 - num6, height);
                    Brush brush = new SolidBrush(color);
                    using (brush)
                    {
                        graphics_0.FillRectangle(brush, rect);
                    }
                    if (this.lineStyle_0 != LineStyle.Invisible)
                    {
                        Pen pen = new Pen(this.color_0, (float) this.int_0);
                        ChartRenderer.SetPenStyle(pen, this.lineStyle_0);
                        using (pen)
                        {
                            graphics_0.DrawRectangle(pen, rect);
                        }
                    }
                }
            }
        }
    }
}

