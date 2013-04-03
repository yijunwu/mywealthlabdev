namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using WealthLab;

    public class WSDFilledOscillator : WSDrawingObject
    {
        private Brush brush_0;
        private Brush brush_1;
        private DataSeries dataSeries_0;
        private double double_0;
        private double double_1;

        public WSDFilledOscillator(DataSeries dataSeries_1, double overbought, double oversold, Brush brushOverbought, Brush brushOversold)
        {
            this.dataSeries_0 = dataSeries_1;
            this.double_1 = overbought;
            this.double_0 = oversold;
            this.brush_1 = brushOverbought;
            this.brush_0 = brushOversold;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            int num = 0;
            int num2 = 0;
            int num5 = (chartRenderer_0.LeftEdgeBar > this.dataSeries_0.FirstValidValue) ? chartRenderer_0.LeftEdgeBar : this.dataSeries_0.FirstValidValue;
            if (num5 < (chartRenderer_0.RightEdgeBar - 1))
            {
                int num6;
                int num7;
                GraphicsPath path = null;
                int num4 = 0;
                if (this.dataSeries_0[num5] >= this.double_1)
                {
                    path = new GraphicsPath();
                    num6 = chartRenderer_0.ConvertBarToX(num5);
                    num = num6;
                    num7 = pane.ConvertValueToY(this.double_1);
                    num2 = pane.ConvertValueToY(this.dataSeries_0[num5]);
                    path.AddLine(num6, num7, num, num2);
                    num4 = 1;
                }
                else if (this.dataSeries_0[num5] <= this.double_0)
                {
                    path = new GraphicsPath();
                    num6 = chartRenderer_0.ConvertBarToX(num5);
                    num = num6;
                    num7 = pane.ConvertValueToY(this.double_0);
                    num2 = pane.ConvertValueToY(this.dataSeries_0[num5]);
                    path.AddLine(num6, num7, num, num2);
                    num4 = -1;
                }
                num6 = chartRenderer_0.ConvertBarToX(num5);
                num7 = pane.ConvertValueToY(this.dataSeries_0[num5]);
                for (int i = num5 + 1; i <= chartRenderer_0.RightEdgeBar; i++)
                {
                    int num8;
                    num = chartRenderer_0.ConvertBarToX(i);
                    num2 = pane.ConvertValueToY(this.dataSeries_0[i]);
                    switch (num4)
                    {
                        case 1:
                            if (this.dataSeries_0[i] >= this.double_1)
                            {
                                path.AddLine(num6, num7, num, num2);
                                num6 = num;
                                num7 = num2;
                            }
                            else
                            {
                                num8 = pane.ConvertValueToY(this.double_1);
                                num = LineUtils.SolveForX(num6, num7, num, num2, num8);
                                num2 = num8;
                                path.AddLine(num6, num7, num, num2);
                                path.CloseFigure();
                                graphics_0.FillPath(this.brush_1, path);
                                path.Dispose();
                                num4 = 0;
                                num = chartRenderer_0.ConvertBarToX(i);
                                num2 = pane.ConvertValueToY(this.dataSeries_0[i]);
                            }
                            break;

                        case -1:
                            if (this.dataSeries_0[i] <= this.double_0)
                            {
                                path.AddLine(num6, num7, num, num2);
                                num6 = num;
                                num7 = num2;
                            }
                            else
                            {
                                num8 = pane.ConvertValueToY(this.double_0);
                                num = LineUtils.SolveForX(num6, num7, num, num2, num8);
                                num2 = num8;
                                path.AddLine(num6, num7, num, num2);
                                path.CloseFigure();
                                graphics_0.FillPath(this.brush_0, path);
                                path.Dispose();
                                num4 = 0;
                                num = chartRenderer_0.ConvertBarToX(i);
                                num2 = pane.ConvertValueToY(this.dataSeries_0[i]);
                            }
                            break;
                    }
                    if (num4 == 0)
                    {
                        if (this.dataSeries_0[i] >= this.double_1)
                        {
                            num6 = chartRenderer_0.ConvertBarToX(i - 1);
                            num7 = pane.ConvertValueToY(this.dataSeries_0[i - 1]);
                            num8 = pane.ConvertValueToY(this.double_1);
                            num6 = LineUtils.SolveForX(num6, num7, num, num2, num8);
                            num7 = num8;
                            path = new GraphicsPath();
                            path.AddLine(num6, num7, num, num2);
                            num4 = 1;
                            num5 = i;
                            num6 = num;
                            num7 = num2;
                        }
                        else if (this.dataSeries_0[i] <= this.double_0)
                        {
                            num6 = chartRenderer_0.ConvertBarToX(i - 1);
                            num7 = pane.ConvertValueToY(this.dataSeries_0[i - 1]);
                            num8 = pane.ConvertValueToY(this.double_0);
                            num6 = LineUtils.SolveForX(num6, num7, num, num2, num8);
                            num7 = num8;
                            path = new GraphicsPath();
                            path.AddLine(num6, num7, num, num2);
                            num4 = -1;
                            num5 = i;
                            num6 = num;
                            num7 = num2;
                        }
                    }
                }
                if (num4 != 0)
                {
                    num6 = num;
                    num7 = num2;
                    if (num4 == 1)
                    {
                        num2 = pane.ConvertValueToY(this.double_1);
                    }
                    else
                    {
                        num2 = pane.ConvertValueToY(this.double_0);
                    }
                    path.AddLine(num6, num7, num, num2);
                    num6 = num;
                    num7 = num2;
                    num = chartRenderer_0.ConvertBarToX(num5 - 1);
                    path.AddLine(num6, num7, num, num2);
                    if (num4 == 1)
                    {
                        graphics_0.FillPath(this.brush_1, path);
                    }
                    else
                    {
                        graphics_0.FillPath(this.brush_0, path);
                    }
                    path.Dispose();
                }
            }
        }
    }
}

