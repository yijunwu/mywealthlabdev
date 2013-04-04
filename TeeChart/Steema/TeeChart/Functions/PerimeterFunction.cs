namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;

    public class PerimeterFunction : Function
    {
        public PerimeterFunction() : this(null)
        {
        }

        public PerimeterFunction(Chart c) : base(c)
        {
            base.CanUsePeriod = false;
            base.SingleSource = true;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                if (series.Count > 0)
                {
                    int num4;
                    base.Series.BeginUpdate();
                    base.Series.Clear();
                    base.Series.XValues.order = ValueListOrder.None;
                    Point[] p = new Point[series.Count];
                    if ((series.GetVertAxis.IAxisSize == 0) || (series.GetHorizAxis.IAxisSize == 0))
                    {
                        IChart parent = series.chart.parent;
                        if (parent != null)
                        {
                            int height = parent.GetControl().Height;
                            int width = parent.GetControl().Width;
                            series.Chart.Bitmap(width, height);
                        }
                    }
                    for (int i = 0; i < series.Count; i++)
                    {
                        series.chart.Graphics3D.Calc3DPos(ref p[i], series.CalcXPos(i), series.CalcYPos(i), series.MiddleZ);
                    }
                    series.chart.Graphics3D.ConvexHull(ref p, out num4);
                    for (int j = 0; j < num4; j++)
                    {
                        base.Series.Add(base.Series.XScreenToValue(p[j].X), base.Series.YScreenToValue(p[j].Y));
                    }
                    if (num4 > 0)
                    {
                        base.Series.Add(base.Series.XValues[0], base.Series.YValues[0]);
                    }
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionPerimeter;
        }
    }
}

