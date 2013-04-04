namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Serializable, Editor(typeof(Steema.TeeChart.Styles.SeriesPointer.PointerEditor), typeof(UITypeEditor)), Description("Series Pointer.")]
    public class SeriesPointer : TeeBase
    {
        protected internal bool AllowChangeSize;
        protected Steema.TeeChart.Styles.Series aSeries;
        private ChartBrush bBrush;
        protected internal bool bVisible;
        private bool dark3D;
        internal bool defaultVisible;
        private bool draw3D;
        private int horizSize;
        private bool inflate;
        private ChartPen pen;
        protected int PXMinus;
        protected int PXPlus;
        protected int PYMinus;
        protected int PYPlus;
        private PointerStyles style;
        private int vertSize;

        public SeriesPointer(Chart c, Steema.TeeChart.Styles.Series s) : base(c)
        {
            this.dark3D = true;
            this.draw3D = true;
            this.inflate = true;
            this.horizSize = 4;
            this.vertSize = 4;
            this.bVisible = true;
            this.defaultVisible = true;
            this.aSeries = s;
        }

        internal void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            if (this.bVisible && this.inflate)
            {
                LeftMargin = Math.Max(LeftMargin, this.horizSize + 1);
                RightMargin = Math.Max(RightMargin, this.horizSize + 1);
            }
        }

        internal void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            if (this.bVisible && this.inflate)
            {
                TopMargin = Math.Max(TopMargin, this.vertSize + 1);
                BottomMargin = Math.Max(BottomMargin, this.vertSize + 1);
            }
        }

        public object Clone()
        {
            Steema.TeeChart.Styles.SeriesPointer pointer = new Steema.TeeChart.Styles.SeriesPointer(base.Chart, this.Series);
            if (this.pen != null)
            {
                pointer.pen = this.pen.Clone() as ChartPen;
            }
            if (this.bBrush != null)
            {
                pointer.Brush = this.bBrush.Clone() as ChartBrush;
            }
            pointer.Visible = this.Visible;
            pointer.Color = this.Color;
            pointer.Dark3D = this.dark3D;
            pointer.HorizSize = this.horizSize;
            pointer.VertSize = this.vertSize;
            pointer.Transparency = this.Transparency;
            pointer.InflateMargins = this.InflateMargins;
            pointer.Style = this.style;
            return pointer;
        }

        private void DoHorizTriangle3D(Graphics3D g, int DeltaX, int px, int py)
        {
            if (this.draw3D)
            {
                g.Pyramid(false, px + DeltaX, this.PYMinus, px - DeltaX, this.PYPlus, this.StartZ, this.EndZ, this.dark3D);
            }
            else
            {
                g.Triangle(new Point(px + DeltaX, this.PYMinus), new Point(px + DeltaX, this.PYPlus), new Point(px - DeltaX, py), this.StartZ);
            }
        }

        private void DoTriangle3D(Graphics3D g, int DeltaY, int px, int py)
        {
            if (this.draw3D)
            {
                g.Pyramid(true, this.PXMinus, py - DeltaY, this.PXPlus, py + DeltaY, this.StartZ, this.EndZ, this.dark3D);
            }
            else
            {
                g.Triangle(new Point(this.PXMinus, py + DeltaY), new Point(this.PXPlus, py + DeltaY), new Point(px, py - DeltaY), this.StartZ);
            }
        }

        public void Draw(int px, int py, System.Drawing.Color colorValue)
        {
            this.Draw(base.chart.graphics3D, base.chart.Aspect.View3D, px, py, this.horizSize, this.vertSize, colorValue, this.style);
        }

        public void Draw(int px, int py, System.Drawing.Color colorValue, PointerStyles aStyle)
        {
            this.Draw(base.chart.graphics3D, base.chart.Aspect.View3D, px, py, this.horizSize, this.vertSize, colorValue, aStyle);
        }

        public void Draw(Graphics3D g, bool is3D, int px, int py, int tmpHoriz, int tmpVert, System.Drawing.Color colorValue, PointerStyles aStyle)
        {
            this.PXMinus = px - tmpHoriz;
            this.PXPlus = px + tmpHoriz;
            this.PYMinus = py - tmpVert;
            this.PYPlus = py + tmpVert;
            System.Drawing.Color color = this.Pen.Color;
            System.Drawing.Color color2 = this.Brush.Color;
            Steema.TeeChart.Drawing.Gradient gradient = this.Brush.Gradient;
            this.PrepareCanvas(g, colorValue);
            if (!is3D)
            {
                switch (aStyle)
                {
                    case PointerStyles.Rectangle:
                        g.Rectangle(this.PXMinus, this.PYMinus, this.PXPlus + 1, this.PYPlus + 1);
                        break;

                    case PointerStyles.Circle:
                        g.Ellipse(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus);
                        break;

                    case PointerStyles.Triangle:
                    {
                        Point[] p = new Point[] { new Point(this.PXMinus, this.PYPlus), new Point(this.PXPlus, this.PYPlus), new Point(px, this.PYMinus) };
                        g.Polygon(p);
                        break;
                    }
                    case PointerStyles.DownTriangle:
                    {
                        Point[] pointArray2 = new Point[] { new Point(this.PXMinus, this.PYMinus), new Point(this.PXPlus, this.PYMinus), new Point(px, this.PYPlus) };
                        g.Polygon(pointArray2);
                        break;
                    }
                    case PointerStyles.Cross:
                        this.DrawCross(g, false, px, py, colorValue);
                        break;

                    case PointerStyles.DiagCross:
                        this.DrawDiagonalCross(g, false, colorValue);
                        break;

                    case PointerStyles.Star:
                        this.DrawCross(g, false, px, py, colorValue);
                        this.DrawDiagonalCross(g, false, colorValue);
                        break;

                    case PointerStyles.Diamond:
                    {
                        Point[] pointArray5 = new Point[] { new Point(this.PXMinus, py), new Point(px, this.PYMinus), new Point(this.PXPlus, py), new Point(px, this.PYPlus) };
                        g.Polygon(pointArray5);
                        break;
                    }
                    case PointerStyles.SmallDot:
                        g.Pixel(px, py, this.MiddleZ, colorValue);
                        break;

                    case PointerStyles.LeftTriangle:
                    {
                        Point[] pointArray3 = new Point[] { new Point(this.PXMinus, py), new Point(this.PXPlus, this.PYMinus), new Point(this.PXPlus, this.PYPlus) };
                        g.Polygon(pointArray3);
                        break;
                    }
                    case PointerStyles.RightTriangle:
                    {
                        Point[] pointArray4 = new Point[] { new Point(this.PXMinus, this.PYMinus), new Point(this.PXMinus, this.PYPlus), new Point(this.PXPlus, py) };
                        g.Polygon(pointArray4);
                        break;
                    }
                    case PointerStyles.Sphere:
                        g.SphereEnh(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus);
                        break;

                    case PointerStyles.PolishedSphere:
                        g.EllipseEnh(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus);
                        break;

                    case PointerStyles.Hexagon:
                        this.DrawHexagon(g, false, px, py, tmpVert, colorValue);
                        break;
                }
            }
            else
            {
                switch (aStyle)
                {
                    case PointerStyles.Rectangle:
                        if (!this.draw3D)
                        {
                            g.Rectangle(this.PXMinus, this.PYMinus, this.PXPlus + 1, this.PYPlus + 1, this.StartZ);
                            break;
                        }
                        g.Cube(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus, this.StartZ, this.EndZ, this.dark3D);
                        break;

                    case PointerStyles.Circle:
                        if (!this.draw3D || !g.SupportsFullRotation)
                        {
                            g.Ellipse(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus, this.StartZ);
                            break;
                        }
                        g.Sphere(px, py, this.MiddleZ, (double) tmpHoriz);
                        break;

                    case PointerStyles.Triangle:
                        this.DoTriangle3D(g, tmpVert, px, py);
                        break;

                    case PointerStyles.DownTriangle:
                        this.DoTriangle3D(g, -tmpVert, px, py);
                        break;

                    case PointerStyles.Cross:
                        this.DrawCross(g, true, px, py, colorValue);
                        break;

                    case PointerStyles.DiagCross:
                        this.DrawDiagonalCross(g, true, colorValue);
                        break;

                    case PointerStyles.Star:
                        this.DrawCross(g, true, px, py, colorValue);
                        this.DrawDiagonalCross(g, true, colorValue);
                        break;

                    case PointerStyles.Diamond:
                        g.Plane(new Point(this.PXMinus, py), new Point(px, this.PYMinus), new Point(this.PXPlus, py), new Point(px, this.PYPlus), this.StartZ);
                        break;

                    case PointerStyles.SmallDot:
                        g.Pixel(px, py, this.MiddleZ, colorValue);
                        break;

                    case PointerStyles.LeftTriangle:
                        this.DoHorizTriangle3D(g, tmpHoriz, px, py);
                        break;

                    case PointerStyles.RightTriangle:
                        this.DoHorizTriangle3D(g, -tmpHoriz, px, py);
                        break;

                    case PointerStyles.Sphere:
                        g.SphereEnh(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus, this.MiddleZ);
                        break;

                    case PointerStyles.PolishedSphere:
                        g.EllipseEnh(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus, this.MiddleZ);
                        break;

                    case PointerStyles.Hexagon:
                        this.DrawHexagon(g, true, px, py, tmpVert, colorValue);
                        break;
                }
            }
            this.Brush.Color = color2;
            this.Brush.Gradient = gradient;
            this.Pen.Color = color;
        }

        private void DrawCross(Graphics3D g, bool is3D, int px, int py, System.Drawing.Color ColorValue)
        {
            if (is3D)
            {
                g.VerticalLine(px, this.PYMinus, this.PYPlus, this.StartZ);
                g.HorizontalLine(this.PXMinus, this.PXPlus, py, this.StartZ);
            }
            else
            {
                g.VerticalLine(px, this.PYMinus, this.PYPlus);
                g.HorizontalLine(this.PXMinus, this.PXPlus, py);
            }
        }

        private void DrawDiagonalCross(Graphics3D g, bool is3D, System.Drawing.Color ColorValue)
        {
            if (is3D)
            {
                g.Line(this.PXMinus, this.PYMinus, this.PXPlus + 1, this.PYPlus + 1, this.StartZ);
                g.Line(this.PXPlus, this.PYMinus, this.PXMinus - 1, this.PYPlus + 1, this.StartZ);
            }
            else
            {
                g.Line(this.PXMinus, this.PYMinus, this.PXPlus + 1, this.PYPlus + 1);
                g.Line(this.PXPlus, this.PYMinus, this.PXMinus - 1, this.PYPlus + 1);
            }
        }

        private void DrawHexagon(Graphics3D g, bool is3D, int px, int py, int tmpVert, System.Drawing.Color ColorValue)
        {
            int num = tmpVert / 2;
            if (is3D)
            {
                Point[] p = new Point[] { new Point(px, this.PYMinus), new Point(this.PXPlus, py - num), new Point(this.PXPlus, py + num), new Point(px, this.PYPlus), new Point(this.PXMinus, py + num), new Point(this.PXMinus, py - num) };
                g.Polygon(this.StartZ, p);
            }
            else
            {
                Point[] pointArray2 = new Point[] { new Point(px, this.PYMinus), new Point(this.PXPlus, py - num), new Point(this.PXPlus, py + num), new Point(px, this.PYPlus), new Point(this.PXMinus, py + num), new Point(this.PXMinus, py - num) };
                g.Polygon(pointArray2);
            }
        }

        internal void DrawLegendShape(System.Drawing.Color color, Rectangle rect, bool drawPen)
        {
            this.DrawLegendShape(base.chart.graphics3D, color, rect, drawPen);
        }

        internal void DrawLegendShape(Graphics3D g, System.Drawing.Color color, Rectangle rect, bool drawPen)
        {
            int num;
            int num2;
            if (drawPen)
            {
                num = rect.Width / 3;
                num2 = rect.Height / 3;
            }
            else
            {
                num = 1 + (rect.Width / 2);
                num2 = 1 + (rect.Height / 2);
            }
            this.Draw(g, false, (rect.X + rect.Right) / 2, (rect.Y + rect.Bottom) / 2, Math.Min(this.horizSize, num), Math.Min(this.vertSize, num2), color, this.style);
        }

        protected internal int[] GetBounds(int index, ref PolygonStyle p)
        {
            int count = base.chart.Series.Count;
            int num2 = base.chart.Series.IndexOf(this.aSeries);
            int num3 = base.chart.aspect.Height3D;
            int num4 = base.chart.aspect.Width3D;
            int num5 = num3 / count;
            int num6 = num4 / count;
            int num7 = (count - (num2 + 1)) * (num3 / count);
            int num8 = (count - (num2 + 1)) * (num4 / count);
            int num9 = this.aSeries.CalcXPos(index);
            int num10 = this.aSeries.CalcYPos(index);
            p = PolygonStyle.Poly;
            switch (this.Style)
            {
                case PointerStyles.Rectangle:
                    if (base.Chart.Aspect.View3D && !base.Chart.Aspect.Orthogonal)
                    {
                        int x = base.Chart.Series[num2].CalcXPos(index);
                        int y = base.Chart.Series[num2].CalcYPos(index);
                        int num13 = x;
                        int num14 = y;
                        base.Chart.Graphics3D.Calc3DPos(ref x, ref y, base.Chart.Series[num2].StartZ);
                        base.Chart.Graphics3D.Calc3DPos(ref num13, ref num14, base.Chart.Series[num2].EndZ);
                        return new int[] { (x - this.HorizSize), (y - this.VertSize), (num13 - this.HorizSize), (num14 - this.VertSize), (num13 + this.HorizSize), (num14 - this.VertSize), (num13 + this.HorizSize), (num14 + this.VertSize), (x + this.HorizSize), (y + this.VertSize), (x - this.HorizSize), (y + this.VertSize) };
                    }
                    return new int[] { ((num9 - this.HorizSize) + num8), ((num10 - this.VertSize) - num7), (((num9 - this.HorizSize) + num6) + num8), (((num10 - this.VertSize) - num5) - num7), (((num9 + this.HorizSize) + num6) + num8), (((num10 - this.VertSize) - num5) - num7), (((num9 + this.HorizSize) + num6) + num8), (((num10 + this.VertSize) - num5) - num7), ((num9 + this.HorizSize) + num8), ((num10 + this.VertSize) - num7), ((num9 - this.HorizSize) + num8), ((num10 + this.VertSize) - num7) };

                case PointerStyles.Circle:
                {
                    int[] numArray = new int[] { num9 + num8, num10 - num7, this.HorizSize };
                    p = PolygonStyle.Circle;
                    return numArray;
                }
            }
            return null;
        }

        internal void PrepareCanvas(Graphics3D g, System.Drawing.Color colorValue)
        {
            g.Pen = this.Pen;
            g.Brush = this.Brush;
            if (this.aSeries != null)
            {
                if (this.Brush.Transparency > 0)
                {
                    g.Brush.Transparency = this.Brush.Transparency;
                    g.Brush.Color = Graphics3D.TransparentColor(g.Brush.Transparency, colorValue);
                }
                else
                {
                    g.Brush.Color = colorValue;
                }
                if (this.aSeries.ColorEach)
                {
                    g.Pen.Color = Utils.DarkenColor(colorValue, 60);
                    if (this.Brush.Gradient.Visible)
                    {
                        g.Brush.Gradient.StartColor = colorValue;
                        g.Brush.Gradient.MiddleColor = Utils.EmptyColor;
                        g.Brush.Gradient.EndColor = Utils.CalcColorBlend(colorValue, System.Drawing.Color.White, 60);
                    }
                }
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pen != null)
            {
                this.pen.Chart = c;
            }
            if (this.bBrush != null)
            {
                this.bBrush.Chart = c;
            }
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.bVisible != this.defaultVisible);
        }

        [Description("Brush used to fill Series Pointers."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                if (this.bBrush == null)
                {
                    this.bBrush = new ChartBrush(base.chart);
                    this.bBrush.Transparency = 0;
                    if (this.aSeries != null)
                    {
                        this.bBrush.defaultColor = this.aSeries.Color;
                        this.bBrush.Color = this.aSeries.Color;
                    }
                }
                return this.bBrush;
            }
            set
            {
                this.bBrush = value;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets the pointer color.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.Brush.Color;
            }
            set
            {
                this.Brush.Color = value;
            }
        }

        [Description("Fills pointer sides in 3D mode with darker color."), Category("Appearance"), DefaultValue(true)]
        public bool Dark3D
        {
            get
            {
                return this.dark3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.dark3D, value);
            }
        }

        [Description("Draws pointer in 3D mode."), DefaultValue(true)]
        public bool Draw3D
        {
            get
            {
                return this.draw3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.draw3D, value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("")]
        public int EndZ
        {
            get
            {
                if (this.aSeries != null)
                {
                    return this.aSeries.EndZ;
                }
                return 0;
            }
        }

        [Description("Configures Gradient filling attributes."), DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
        }

        [DefaultValue(4), Description("Horizontal size of pointer in pixels.")]
        public int HorizSize
        {
            get
            {
                return this.horizSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.horizSize, value);
            }
        }

        [DefaultValue(true), Description("Expands axes to fit pointers.")]
        public bool InflateMargins
        {
            get
            {
                return this.inflate;
            }
            set
            {
                base.SetBooleanProperty(ref this.inflate, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("")]
        public int MiddleZ
        {
            get
            {
                if (this.aSeries != null)
                {
                    return this.aSeries.MiddleZ;
                }
                return 0;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen used to draw a frame around Series Pointers.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart, System.Drawing.Color.Black);
                }
                return this.pen;
            }
            set
            {
                this.pen = value;
            }
        }

        [Browsable(false)]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.aSeries;
            }
        }

        [Description(""), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int StartZ
        {
            get
            {
                if (this.aSeries != null)
                {
                    return this.aSeries.StartZ;
                }
                return 0;
            }
        }

        [Description("Pointer style.")]
        public PointerStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.Invalidate();
                }
            }
        }

        [Category("Appearance"), Description("Sets Transparency level from 0 to 100%."), DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Transparency
        {
            get
            {
                return this.Brush.Transparency;
            }
            set
            {
                this.Brush.Transparency = value;
            }
        }

        [DefaultValue(4), Description("Horizontal size of pointer in pixels.")]
        public int VertSize
        {
            get
            {
                return this.vertSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.vertSize, value);
            }
        }

        [Description("Shows or hides the pointer.")]
        public bool Visible
        {
            get
            {
                return this.bVisible;
            }
            set
            {
                base.SetBooleanProperty(ref this.bVisible, value);
            }
        }

        internal sealed class PointerEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                bool flag = EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.SeriesPointer((Steema.TeeChart.Styles.SeriesPointer) value));
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                Chart chart;
                base.PaintValue(e);
                Steema.TeeChart.Styles.SeriesPointer pointer = (Steema.TeeChart.Styles.SeriesPointer) e.Value;
                if (pointer.chart == null)
                {
                    chart = new Chart {
                        AutoRepaint = false
                    };
                    pointer.Chart = chart;
                }
                else
                {
                    chart = null;
                }
                Graphics3DGdiPlus g = new Graphics3DGdiPlus(pointer.chart) {
                    g = e.Graphics
                };
                int px = e.Bounds.X + (e.Bounds.Width / 2);
                int py = e.Bounds.Y + (e.Bounds.Height / 2);
                int tmpHoriz = Math.Min(e.Bounds.Width, e.Bounds.Height) / 2;
                pointer.Draw(g, pointer.draw3D, px, py, tmpHoriz, tmpHoriz, pointer.Color, pointer.style);
                if (pointer.chart == chart)
                {
                    pointer.Chart = null;
                    chart.Dispose();
                }
            }
        }
    }
}

