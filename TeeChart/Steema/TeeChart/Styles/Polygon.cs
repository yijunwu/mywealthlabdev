namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class Polygon : TeeBase
    {
        private bool closed;
        protected internal PolygonSeries FPoints;
        private int index;
        private ChartPen iPen;
        private Point[] IPoints;
        private bool parentBrush;
        private bool parentPen;
        private Map parentSeries;
        private int transparency;

        public Polygon(Chart AChart) : this(null, AChart)
        {
        }

        public Polygon(PolygonList Collection, Chart AChart)
        {
            base.Chart = AChart;
            this.Closed = true;
            this.FPoints = new PolygonSeries();
            this.FPoints.iPolygon = this;
            this.FPoints.XValues.Order = ValueListOrder.None;
            this.FPoints.ShowInLegend = false;
            this.ParentPen = true;
            this.ParentBrush = true;
            if (Collection != null)
            {
                this.ParentSeries = Collection.Owner;
                this.ParentSeries.Add((double) 0.0, (double) 0.0);
            }
        }

        public int Add(Point Point)
        {
            return this.Points.Add((double) Point.X, (double) Point.Y);
        }

        public int Add(double X, double Y)
        {
            return this.Points.Add(X, Y);
        }

        public Rectangle Bounds()
        {
            return base.Chart.Graphics3D.PolygonBounds(this.GetPoints());
        }

        public void CalcPoints(int xoffset, int yoffset)
        {
            this.GetPoints();
            for (int i = 0; i < this.IPoints.Length; i++)
            {
                this.IPoints[i].X += xoffset;
                this.IPoints[i].Y += yoffset;
            }
        }

        private bool ContainsRect(Rectangle R1, Rectangle R2)
        {
            if (R2.Left > R1.Left)
            {
                R1.X = R2.Left;
            }
            if (R2.Right < R1.Right)
            {
                R1.Width = R2.Right - R2.Left;
            }
            if (R2.Top > R1.Top)
            {
                R1.Y = R2.Top;
            }
            if (R2.Bottom < R1.Bottom)
            {
                R1.Height = R2.Bottom - R2.Top;
            }
            return ((R1.Right >= R1.Left) && (R1.Bottom >= R1.Top));
        }

        public void Draw(Graphics3D g, int ValueIndex)
        {
            if ((this.Points.Active && (this.Points.Count > 1)) && this.Visible())
            {
                if (this.ParentPen)
                {
                    g.Pen = this.iPen;
                }
                else
                {
                    g.Pen = this.Pen;
                }
                System.Drawing.Color color = this.ParentSeries.ValueColor(ValueIndex);
                if (this.ParentBrush)
                {
                    g.Brush = this.ParentSeries.Brush;
                    g.Brush.Color = color;
                    this.Color = color;
                    if (this.Transparency > 0)
                    {
                        g.Brush.Transparency = this.Transparency;
                    }
                }
                else
                {
                    g.Brush = this.Brush;
                    this.Color = this.Brush.Color;
                }
                this.GetPoints();
                if (this.ParentSeries.Chart.Aspect.View3D)
                {
                    int z = this.ParentSeries.CalcZPos(ValueIndex);
                    if (this.Closed)
                    {
                        g.Polygon(z, this.IPoints);
                    }
                    else
                    {
                        g.Polyline(z, this.IPoints);
                    }
                }
                else if (this.Closed)
                {
                    g.Polygon(this.IPoints);
                }
                else
                {
                    g.Polyline(this.IPoints);
                }
            }
        }

        public Point[] GetPoints()
        {
            int count = this.Points.Count;
            Axis getHorizAxis = this.ParentSeries.GetHorizAxis;
            Axis getVertAxis = this.ParentSeries.GetVertAxis;
            ValueList xValues = this.Points.XValues;
            ValueList yValues = this.Points.YValues;
            this.IPoints = new Point[count];
            Point[] iPoints = this.IPoints;
            for (int i = 0; i < count; i++)
            {
                iPoints[i].X = getHorizAxis.CalcPosValue(xValues[i]);
                iPoints[i].Y = getVertAxis.CalcPosValue(yValues[i]);
            }
            return iPoints;
        }

        public bool Visible()
        {
            int left;
            int right;
            Rectangle empty = Rectangle.Empty;
            if ((base.Chart == null) && (this.ParentSeries != null))
            {
                base.Chart = this.ParentSeries.Chart;
            }
            bool flag = !base.Chart.Aspect.ClipPoints;
            if (flag)
            {
                return flag;
            }
            Axis getHorizAxis = this.ParentSeries.GetHorizAxis;
            empty.X = getHorizAxis.CalcPosValue(this.Points.XValues.Minimum);
            empty.Width = getHorizAxis.CalcPosValue(this.Points.XValues.Maximum) - empty.Left;
            if (getHorizAxis.Inverted)
            {
                left = empty.Left;
                right = empty.Right;
                empty.X = left;
                empty.Width = right - empty.Left;
            }
            Axis getVertAxis = this.ParentSeries.GetVertAxis;
            empty.Y = getVertAxis.CalcPosValue(this.Points.YValues.Maximum);
            empty.Height = getVertAxis.CalcPosValue(this.Points.YValues.Minimum) - empty.Top;
            if (getVertAxis.Inverted)
            {
                left = empty.Top;
                right = empty.Bottom;
                Utils.SwapInteger(ref left, ref right);
                empty.Y = left;
                empty.Height = right - empty.Top;
            }
            return this.ContainsRect(base.Chart.ChartRect, empty);
        }

        public ChartBrush Brush
        {
            get
            {
                return this.Points.bBrush;
            }
        }

        public bool Closed
        {
            get
            {
                return this.closed;
            }
            set
            {
                base.SetBooleanProperty(ref this.closed, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color Color
        {
            get
            {
                return this.ParentSeries.ValueColor(this.Index);
            }
            set
            {
                this.Points.Color = value;
            }
        }

        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Points.bBrush.Gradient;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        public bool ParentBrush
        {
            get
            {
                return this.parentBrush;
            }
            set
            {
                base.SetBooleanProperty(ref this.parentBrush, value);
            }
        }

        public bool ParentPen
        {
            get
            {
                return this.parentPen;
            }
            set
            {
                base.SetBooleanProperty(ref this.parentPen, value);
            }
        }

        public Map ParentSeries
        {
            get
            {
                return this.parentSeries;
            }
            set
            {
                if (this.iPen == null)
                {
                    this.iPen = value.Pen;
                }
                this.parentSeries = value;
            }
        }

        public ChartPen Pen
        {
            get
            {
                return this.Points.Pen;
            }
            set
            {
                this.Points.Pen = value;
            }
        }

        public PolygonSeries Points
        {
            get
            {
                return this.FPoints;
            }
            set
            {
                this.FPoints = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Text
        {
            get
            {
                return this.ParentSeries.Labels[this.Index];
            }
            set
            {
                this.ParentSeries.Labels[this.Index] = value;
                this.Points.Title = value;
            }
        }

        public int Transparency
        {
            get
            {
                return this.transparency;
            }
            set
            {
                this.transparency = value;
                this.Brush.Transparency = this.transparency;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Z
        {
            get
            {
                return this.ParentSeries.ZValues[this.Index];
            }
            set
            {
                this.ParentSeries.ZValues[this.Index] = value;
                this.ParentSeries.Repaint();
            }
        }
    }
}

