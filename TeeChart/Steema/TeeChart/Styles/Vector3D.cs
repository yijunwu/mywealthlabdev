namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Vector3D), "SeriesIcons.Vector3D.bmp"), Description("Vector3D Series.")]
    public class Vector3D : Custom3DPalette
    {
        private int FArrowHeight;
        private int FArrowWidth;
        private ChartPen FEndArrow;
        private ValueList FEndXValues;
        private ValueList FEndYValues;
        private ValueList FEndZValues;
        private ChartPen FStartArrow;
        private Color tmpColor;
        private double tmpCos;
        private double tmpSin;

        public Vector3D() : this(null)
        {
        }

        public Vector3D(Chart c) : base(c)
        {
            this.FArrowHeight = 4;
            this.FArrowWidth = 4;
            this.FEndXValues = new ValueList(this, "EndXValues");
            this.FEndYValues = new ValueList(this, "EndYValues");
            this.FEndZValues = new ValueList(this, "EndZValues");
        }

        public int Add(double x0, double y0, double z0, double x1, double y1, double z1)
        {
            this.EndXValues.TempValue = x1;
            this.EndYValues.TempValue = y1;
            this.EndZValues.TempValue = z1;
            return base.Add(x0, y0, z0);
        }

        public int Add(double x0, double y0, double z0, double x1, double y1, double z1, string text, Color color)
        {
            this.EndXValues.TempValue = x1;
            this.EndYValues.TempValue = y1;
            this.EndZValues.TempValue = z1;
            return base.Add(x0, y0, z0, text, color);
        }

        protected override void AddSampleValues(int numValues)
        {
            double num10 = 0.0;
            double a = 0.0;
            double num9 = numValues * 0.6;
            double num8 = 25.132741228718345 / ((double) numValues);
            for (int i = 1; i <= numValues; i++)
            {
                double num2 = num10 + Math.Sin(a);
                double num3 = num10;
                double num4 = Math.Cos(a);
                double num5 = num2 + Math.Sin(a);
                double num6 = num3 + ((num9 * Math.Cos(a)) * Math.Sin(a));
                double num7 = num4 + (Math.Cos(a) * num10);
                this.Add(num2, num3, num4, num5, num6, num7);
                num10++;
                a += num8;
            }
        }

        private void DrawArrow(ChartPen APen, Point P, int Z)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            Color color = APen.Color;
            graphicsd.Pen = APen;
            if (color == Utils.EmptyColor)
            {
                graphicsd.Pen.Color = this.tmpColor;
            }
            graphicsd.MoveTo(P.X + Utils.Round((double) ((-this.FArrowWidth * this.tmpCos) - (this.FArrowHeight * this.tmpSin))), P.Y + Utils.Round((double) ((this.FArrowWidth * this.tmpSin) - (this.FArrowHeight * this.tmpCos))), Z);
            graphicsd.LineTo(P.X, P.Y, Z);
            graphicsd.LineTo(P.X + Utils.Round((double) ((-this.FArrowWidth * this.tmpCos) + (this.FArrowHeight * this.tmpSin))), P.Y + Utils.Round((double) ((this.FArrowWidth * this.tmpSin) + (this.FArrowHeight * this.tmpCos))), Z);
            if (color == Utils.EmptyColor)
            {
                APen.Color = color;
            }
        }

        public override void DrawValue(int valueIndex)
        {
            Point p = new Point(0, 0);
            new Point(0, 0);
            Point point2 = new Point(0, 0);
            Graphics3D graphicsd = base.chart.Graphics3D;
            this.tmpColor = this.ValueColor(valueIndex);
            base.Pen.Color = this.tmpColor;
            graphicsd.Pen = base.Pen;
            p.X = this.CalcXPos(valueIndex);
            p.Y = this.CalcYPos(valueIndex);
            int z = base.CalcZPos(valueIndex);
            graphicsd.MoveTo(p.X, p.Y, z);
            point2.X = base.CalcXPosValue(this.EndXValues[valueIndex]);
            point2.Y = base.CalcYPosValue(this.EndYValues[valueIndex]);
            int num2 = base.chart.axes.depth.CalcYPosValue(this.EndZValues[valueIndex]);
            if (base.Pen.Visible)
            {
                graphicsd.LineTo(point2.X, point2.Y, num2);
            }
            if (this.StartArrow.Visible || this.EndArrow.Visible)
            {
                Point point3 = p;
                Point point4 = point2;
                graphicsd.Calc3DPos(ref point3, point3.X, point3.Y, z);
                graphicsd.Calc3DPos(ref point4, point4.X, point4.Y, num2);
                Utils.SinCos(Math.Atan2((double) (point3.Y - point4.Y), (double) (point4.X - point3.X)), out this.tmpSin, out this.tmpCos);
                if (this.StartArrow.Visible)
                {
                    this.DrawArrow(this.StartArrow, p, z);
                }
                if (this.EndArrow.Visible)
                {
                    this.DrawArrow(this.EndArrow, point2, num2);
                }
            }
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        public override bool IsValidSourceOf(Series value)
        {
            return (value is Vector3D);
        }

        public override double MaxXValue()
        {
            return Math.Max(base.MaxXValue(), this.FEndXValues.Maximum);
        }

        public override double MaxYValue()
        {
            return Math.Max(base.MaxYValue(), this.FEndYValues.Maximum);
        }

        public override double MaxZValue()
        {
            return Math.Max(base.MaxZValue(), this.FEndZValues.Maximum);
        }

        public override double MinXValue()
        {
            return Math.Min(base.MinXValue(), this.FEndXValues.Minimum);
        }

        public override double MinYValue()
        {
            return Math.Min(base.MinYValue(), this.FEndYValues.Minimum);
        }

        public override double MinZValue()
        {
            return Math.Min(base.MinZValue(), this.FEndZValues.Minimum);
        }

        protected internal override int NumSampleValues()
        {
            return 250;
        }

        [Description("The arrow height in pixels."), DefaultValue(4)]
        public int ArrowHeight
        {
            get
            {
                return this.FArrowHeight;
            }
            set
            {
                base.SetIntegerProperty(ref this.FArrowHeight, value);
            }
        }

        [DefaultValue(4), Description("The arrow width in pixels.")]
        public int ArrowWidth
        {
            get
            {
                return this.FArrowWidth;
            }
            set
            {
                base.SetIntegerProperty(ref this.FArrowWidth, value);
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryVector3D;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("When Visible, this pen is used to display arrows at the end coordinate of vector lines.")]
        public ChartPen EndArrow
        {
            get
            {
                if (this.FEndArrow == null)
                {
                    this.FEndArrow = new ChartPen(base.chart, true);
                }
                return this.FEndArrow;
            }
        }

        [Description("List of values representing the end X coordinates of vector lines."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Data")]
        public ValueList EndXValues
        {
            get
            {
                return this.FEndXValues;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Data"), Description("List of values representing the end Y coordinates of vector lines.")]
        public ValueList EndYValues
        {
            get
            {
                return this.FEndYValues;
            }
        }

        [Category("Data"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("List of values representing the end Z coordinates of vector lines.")]
        public ValueList EndZValues
        {
            get
            {
                return this.FEndZValues;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("When Visible, this pen is used to display arrows at the start coordinate of vector lines.")]
        public ChartPen StartArrow
        {
            get
            {
                if (this.FStartArrow == null)
                {
                    this.FStartArrow = new ChartPen(base.chart, false);
                }
                return this.FStartArrow;
            }
        }
    }
}

