namespace WealthLab.WSDrawingObjects
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using WealthLab;

    public class WSDFilledBand : WSDrawingObject
    {
        private Brush brush_0;
        private DataSeries dataSeries_0;
        private DataSeries dataSeries_1;

        public WSDFilledBand(DataSeries ser1, DataSeries ser2, Brush brush)
        {
            this.dataSeries_0 = ser1;
            this.dataSeries_1 = ser2;
            this.brush_0 = brush;
        }

        internal override void Render(Graphics graphics_0, ChartPane pane, ChartRenderer chartRenderer_0)
        {
            int num = (chartRenderer_0.LeftEdgeBar > this.dataSeries_0.FirstValidValue) ? chartRenderer_0.LeftEdgeBar : this.dataSeries_0.FirstValidValue;
            if (chartRenderer_0.RightEdgeBar > (num + 1))
            {
                GraphicsPath path = new GraphicsPath();
                using (path)
                {
                    int num2 = chartRenderer_0.ConvertBarToX(num);
                    int num3 = pane.ConvertValueToY(this.dataSeries_0[num]);
                    int num4 = 0;
                    int num5 = 0;
                    for (int i = num + 1; i <= chartRenderer_0.RightEdgeBar; i++)
                    {
                        num4 = chartRenderer_0.ConvertBarToX(i);
                        num5 = pane.ConvertValueToY(this.dataSeries_0[i]);
                        path.AddLine(num2, num3, num4, num5);
                        num2 = num4;
                        num3 = num5;
                    }
                    num2 = num4;
                    num3 = num5;
                    num5 = pane.ConvertValueToY(this.dataSeries_1[chartRenderer_0.RightEdgeBar]);
                    path.AddLine(num2, num3, num4, num5);
                    num3 = num5;
                    for (int j = chartRenderer_0.RightEdgeBar - 1; j >= num; j--)
                    {
                        num4 = chartRenderer_0.ConvertBarToX(j);
                        num5 = pane.ConvertValueToY(this.dataSeries_1[j]);
                        path.AddLine(num2, num3, num4, num5);
                        num2 = num4;
                        num3 = num5;
                    }
                    num2 = num4;
                    num3 = num5;
                    num5 = pane.ConvertValueToY(this.dataSeries_0[num]);
                    path.AddLine(num2, num3, num4, num5);
                    graphics_0.FillPath(this.brush_0, path);
                }
            }
        }
    }
}

