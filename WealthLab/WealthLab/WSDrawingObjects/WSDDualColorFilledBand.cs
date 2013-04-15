namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using WealthLab;

    public class WSDDualColorFilledBand : WSDrawingObject
    {
        private Brush brush_0;
        private Brush brush_1;
        private DataSeries dataSeries_0;
        private DataSeries dataSeries_1;

        public WSDDualColorFilledBand(DataSeries ser1, DataSeries ser2, Brush brush1, Brush brush2)
        {
            this.dataSeries_0 = ser1;
            this.dataSeries_1 = ser2;
            this.brush_0 = brush1;
            this.brush_1 = brush2;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            int leftEdgeBar = chartRenderer_0.LeftEdgeBar;
            if (this.dataSeries_0.FirstValidValue > leftEdgeBar)
            {
                leftEdgeBar = this.dataSeries_0.FirstValidValue;
            }
            if (this.dataSeries_1.FirstValidValue > leftEdgeBar)
            {
                leftEdgeBar = this.dataSeries_1.FirstValidValue;
            }
            int num10 = leftEdgeBar;
            int rightEdgeBar = chartRenderer_0.RightEdgeBar;
            while (leftEdgeBar < chartRenderer_0.RightEdgeBar)
            {
                int num7;
                Brush brush;
                rightEdgeBar = chartRenderer_0.RightEdgeBar;
                if (this.dataSeries_0[leftEdgeBar] > this.dataSeries_1[leftEdgeBar])
                {
                    brush = this.brush_0;
                    num7 = 1;
                }
                else
                {
                    num7 = -1;
                    brush = this.brush_1;
                }
                GraphicsPath path = new GraphicsPath();
                using (path)
                {
                    int num2 = chartRenderer_0.ConvertBarToX(leftEdgeBar);
                    int num3 = pane.ConvertValueToY(this.dataSeries_0[leftEdgeBar]);
                    int num4 = 0;
                    int num5 = 0;
                    int num6 = leftEdgeBar + 1;
                    while (num6 <= chartRenderer_0.RightEdgeBar)
                    {
                        num4 = chartRenderer_0.ConvertBarToX(num6);
                        num5 = pane.ConvertValueToY(this.dataSeries_0[num6]);
                        path.AddLine(num2, num3, num4, num5);
                        num2 = num4;
                        num3 = num5;
                        if ((this.dataSeries_0[num6] * num7) <= this.dataSeries_1[num6])
                        {
                            ///goto  Label_0118;  ///WYJ fix, simplify the flow
                            rightEdgeBar = num6;
                            break;
                        }
                        num6++;
                    }
                    if (rightEdgeBar == chartRenderer_0.RightEdgeBar)
                    {
                        num2 = num4;
                        num3 = num5;
                        num5 = pane.ConvertValueToY(this.dataSeries_1[chartRenderer_0.RightEdgeBar]);
                        path.AddLine(num2, num3, num4, num5);
                        num3 = num5;
                    }
                    else
                    {
                        num2 = num4;
                        num3 = pane.ConvertValueToY(this.dataSeries_1[rightEdgeBar]);
                    }
                    for (int i = rightEdgeBar - 1; i >= leftEdgeBar; i--)
                    {
                        num4 = chartRenderer_0.ConvertBarToX(i);
                        num5 = pane.ConvertValueToY(this.dataSeries_1[i]);
                        path.AddLine(num2, num3, num4, num5);
                        num2 = num4;
                        num3 = num5;
                    }
                    if (leftEdgeBar == num10)
                    {
                        num2 = num4;
                        num3 = num5;
                        num5 = pane.ConvertValueToY(this.dataSeries_0[leftEdgeBar]);
                        path.AddLine(num2, num3, num4, num5);
                    }
                    graphics_0.FillPath(brush, path);
                }
                leftEdgeBar = rightEdgeBar;
            }
        }
    }
}

