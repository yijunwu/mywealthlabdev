namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    internal sealed class PaletteSeries : Series
    {
        internal LegendPalette iTool;
        private ChartPen pen;

        public PaletteSeries() : this(null)
        {
        }

        public PaletteSeries(Chart c) : base(c)
        {
            base.calcVisiblePoints = false;
        }

        public override void DrawValue(int valueIndex)
        {
            if (valueIndex > 0)
            {
                int iStartPos;
                int iEndPos;
                int num3;
                int num4;
                if (this.iTool.Vertical)
                {
                    iStartPos = base.GetHorizAxis.IStartPos;
                    if (base.VertAxis != VerticalAxis.Right)
                    {
                        iStartPos += base.Chart.Axes.Left.AxisPen.Width;
                    }
                    iEndPos = base.GetHorizAxis.IEndPos;
                    num3 = this.CalcYPos(valueIndex);
                    num4 = this.CalcYPos(valueIndex - 1);
                    if (valueIndex == (base.Count - 1))
                    {
                        if (base.GetVertAxis.Inverted)
                        {
                            num4++;
                        }
                        else
                        {
                            num3--;
                        }
                    }
                    if ((valueIndex == 1) && base.GetVertAxis.Inverted)
                    {
                        num4--;
                    }
                    if (base.GetVertAxis.Inverted)
                    {
                        Utils.SwapInteger(ref num3, ref num4);
                    }
                    if (!this.Pen.Visible)
                    {
                        num3--;
                        num4++;
                    }
                }
                else
                {
                    num3 = base.GetVertAxis.IStartPos;
                    if (base.HorizAxis != HorizontalAxis.Bottom)
                    {
                        num3 += base.Chart.Axes.Bottom.AxisPen.Width;
                    }
                    num4 = base.GetVertAxis.IEndPos;
                    iStartPos = this.CalcXPos(valueIndex - 1);
                    iEndPos = this.CalcXPos(valueIndex);
                    if (base.GetHorizAxis.Inverted)
                    {
                        Utils.SwapInteger(ref iStartPos, ref iEndPos);
                    }
                    if (!this.Pen.Visible)
                    {
                        iStartPos--;
                        iEndPos++;
                    }
                }
                Graphics3D g = base.Chart.Graphics3D;
                if (this.iTool.Smooth)
                {
                    LinearGradientMode horizontal;
                    Color color;
                    Color color2;
                    if (!this.iTool.Vertical)
                    {
                        horizontal = LinearGradientMode.Horizontal;
                        color2 = this.ValueColor(valueIndex);
                        if (valueIndex > 0)
                        {
                            color = this.ValueColor(valueIndex - 1);
                        }
                        else
                        {
                            color = color2;
                        }
                    }
                    else
                    {
                        horizontal = LinearGradientMode.Vertical;
                        color = this.ValueColor(valueIndex);
                        if (valueIndex > 0)
                        {
                            color2 = this.ValueColor(valueIndex - 1);
                        }
                        else
                        {
                            color2 = color;
                        }
                    }
                    if (this.iTool.Inverted)
                    {
                        this.SwapColors(ref color, ref color2);
                    }
                    Gradient gradient = new Gradient {
                        Visible = true,
                        StartColor = color,
                        EndColor = color2,
                        Direction = horizontal
                    };
                    Gradient gradient2 = g.Brush.Gradient;
                    bool visible = g.Pen.Visible;
                    try
                    {
                        g.Pen.Visible = false;
                        g.Brush.Gradient = gradient;
                        g.Brush.WrapMode = WrapMode.Clamp;
                        gradient.Draw(g, iStartPos, num3, iEndPos, num4);
                    }
                    finally
                    {
                        g.Brush.Gradient = gradient2;
                        g.Pen.Visible = visible;
                    }
                }
                else
                {
                    Color color3 = g.Brush.Color;
                    bool flag2 = g.Pen.Visible;
                    try
                    {
                        g.Pen.Visible = false;
                        g.Brush.Color = this.ValueColor(valueIndex);
                        g.Brush.Solid = true;
                        g.Rectangle(iStartPos, num3, iEndPos - iStartPos, num4 - num3);
                    }
                    finally
                    {
                        g.Pen.Visible = flag2;
                        g.Brush.Color = color3;
                    }
                }
                if (this.Pen.Visible)
                {
                    g.Pen = this.Pen;
                    if (this.iTool.Vertical)
                    {
                        g.HorizontalLine(iStartPos, iEndPos, num3);
                        if (valueIndex == 1)
                        {
                            g.HorizontalLine(iStartPos, iEndPos, num4);
                        }
                        if (base.VertAxis == VerticalAxis.Left)
                        {
                            g.VerticalLine(iEndPos, num3, num4);
                        }
                        else if (base.VertAxis == VerticalAxis.Right)
                        {
                            g.VerticalLine(iStartPos, num3, num4);
                        }
                    }
                    else
                    {
                        g.VerticalLine(iEndPos, num3, num4);
                        if (valueIndex == 1)
                        {
                            g.VerticalLine(iStartPos, num3, num4);
                        }
                        if (base.HorizAxis == HorizontalAxis.Top)
                        {
                            g.HorizontalLine(iStartPos, iEndPos, num3);
                        }
                        else if (base.HorizAxis == HorizontalAxis.Bottom)
                        {
                            g.HorizontalLine(iStartPos, iEndPos, num4);
                        }
                    }
                }
            }
        }

        private void SwapColors(ref Color a, ref Color b)
        {
            a = b;
            b = a;
        }

        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.Chart);
                }
                return this.pen;
            }
            set
            {
                this.pen = value;
            }
        }
    }
}

