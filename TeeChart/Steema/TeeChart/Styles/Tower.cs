namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [Serializable, ToolboxBitmap(typeof(Tower), "SeriesIcons.Tower.bmp")]
    public class Tower : Custom3DGrid
    {
        private bool dark3D;
        private double IOffD;
        private double IOffW;
        private double origin;
        private int percDepth;
        private int percWidth;
        private bool tmpChangeBrush;
        private TowerStyles towerStyle;
        private bool useOrigin;

        public Tower() : this(null)
        {
        }

        public Tower(Chart c) : base(c)
        {
            this.dark3D = true;
            this.percDepth = 100;
            this.percWidth = 100;
        }

        private Rectangle CalcCell(int AIndex, out int ATop, out int ABottom, out int z0, out int z1)
        {
            Rectangle rectangle = new Rectangle();
            double num = base.XValues[AIndex];
            rectangle.X = base.CalcXPosValue(num - this.IOffW);
            rectangle.Width = base.CalcXPosValue(num + this.IOffW) - rectangle.X;
            ATop = this.CalcYPos(AIndex);
            rectangle.Y = ATop;
            ABottom = this.useOrigin ? base.CalcYPosValue(this.Origin) : base.CalcYPosValue(base.YValues.Minimum);
            if (rectangle.Y > ABottom)
            {
                int y = rectangle.Y;
                rectangle.Y = ABottom;
                rectangle.Height = y - ABottom;
            }
            else
            {
                rectangle.Height = ABottom - rectangle.Y;
            }
            num = base.ZValues[AIndex];
            z0 = base.chart.Axes.Depth.CalcYPosValue(num - this.IOffD);
            z1 = base.chart.Axes.Depth.CalcYPosValue(num + this.IOffD);
            return rectangle;
        }

        public override int Clicked(int x, int y)
        {
            int res = -1;
            if ((base.Count > 0) && base.iGridIndexFilled)
            {
                int num;
                int numZValues;
                this.IOffW = this.percWidth * 0.005;
                this.IOffD = this.percDepth * 0.005;
                Point p = new Point(x, y);
                if (!base.BackFaced())
                {
                    if (!this.DrawValuesForward())
                    {
                        for (num = base.NumXValues; num >= 1; num--)
                        {
                            numZValues = 1;
                            while (numZValues <= base.NumZValues)
                            {
                                if (this.ClickedCell(ref res, num, numZValues, p))
                                {
                                    return res;
                                }
                                numZValues++;
                            }
                        }
                        return res;
                    }
                    num = 1;
                    while (num <= base.NumXValues)
                    {
                        numZValues = 1;
                        while (numZValues <= base.NumZValues)
                        {
                            if (this.ClickedCell(ref res, num, numZValues, p))
                            {
                                return res;
                            }
                            numZValues++;
                        }
                        num++;
                    }
                    return res;
                }
                if (!this.DrawValuesForward())
                {
                    for (num = base.NumXValues; num >= 1; num--)
                    {
                        numZValues = base.NumZValues;
                        while (numZValues >= 1)
                        {
                            if (this.ClickedCell(ref res, num, numZValues, p))
                            {
                                return res;
                            }
                            numZValues--;
                        }
                    }
                    return res;
                }
                for (num = 1; num <= base.NumXValues; num++)
                {
                    for (numZValues = base.NumZValues; numZValues >= 1; numZValues--)
                    {
                        if (this.ClickedCell(ref res, num, numZValues, p))
                        {
                            return res;
                        }
                    }
                }
            }
            return res;
        }

        private bool ClickedCell(ref int res, int tmpX, int tmpZ, Point P)
        {
            Point[] poly = new Point[4];
            int valueIndex = base.gridIndex[tmpX][tmpZ];
            if ((valueIndex != -1) && (this.ValueColor(valueIndex) != Color.Transparent))
            {
                int num2;
                int num3;
                int num4;
                int num5;
                Rectangle rectangle = this.CalcCell(valueIndex, out num4, out num5, out num2, out num3);
                Graphics3D graphicsd = base.chart.Graphics3D;
                if (this.TowerStyle == TowerStyles.Cover)
                {
                    poly[0] = graphicsd.Calculate3DPosition(rectangle.Left, num4, num2);
                    poly[1] = graphicsd.Calculate3DPosition(rectangle.Right, num4, num2);
                    poly[2] = graphicsd.Calculate3DPosition(rectangle.Right, num4, num3);
                    poly[3] = graphicsd.Calculate3DPosition(rectangle.Left, num4, num3);
                }
                else
                {
                    poly[0] = graphicsd.Calculate3DPosition(rectangle.Left, num4, num2);
                    poly[1] = graphicsd.Calculate3DPosition(rectangle.Right, num4, num2);
                    poly[2] = graphicsd.Calculate3DPosition(rectangle.Right, num5, num2);
                    poly[3] = graphicsd.Calculate3DPosition(rectangle.Left, num5, num2);
                }
                if (Graphics3D.PointInPolygon(P, poly))
                {
                    res = valueIndex;
                    return true;
                }
            }
            return false;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.SingleColor);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.Rectangle);
            AddSubChart(Texts.Cover);
            AddSubChart(Texts.Circle);
            AddSubChart(Texts.GalleryArrow);
            AddSubChart(Texts.Cone);
            AddSubChart(Texts.Pyramid);
        }

        public override void Draw()
        {
            this.tmpChangeBrush = base.Brush.Visible;
            base.chart.graphics3D.Pen = base.Pen;
            base.chart.graphics3D.Brush = base.Brush;
            this.DrawCells();
        }

        private void DrawCell(int x, int z)
        {
            int valueIndex = base.gridIndex[x][z];
            if (valueIndex != -1)
            {
                Color color = this.ValueColor(valueIndex);
                if (color != Utils.EmptyColor)
                {
                    int num2;
                    int num3;
                    int num4;
                    int num5;
                    if (this.tmpChangeBrush)
                    {
                        base.chart.graphics3D.Brush = base.Brush;
                        base.chart.graphics3D.Brush.Color = Graphics3D.TransparentColor(this.Transparency, color);
                    }
                    Rectangle r = this.CalcCell(valueIndex, out num2, out num3, out num4, out num5);
                    switch (this.towerStyle)
                    {
                        case TowerStyles.Cube:
                            base.chart.graphics3D.Cube(r, num4, num5, this.Dark3D);
                            return;

                        case TowerStyles.Rectangle:
                            base.chart.graphics3D.Rectangle(r, (num4 + num5) / 2);
                            return;

                        case TowerStyles.Cover:
                            base.chart.graphics3D.RectangleY(r.Left, num2, r.Right, num4, num5);
                            return;

                        case TowerStyles.Cylinder:
                            base.chart.graphics3D.Cylinder(true, r, num4, num5, this.Dark3D);
                            return;

                        case TowerStyles.Arrow:
                        {
                            int num6 = (r.Left + r.Right) / 2;
                            base.chart.graphics3D.Arrow(true, new Point(num6, num3), new Point(num6, num2), r.Right - r.Left, (r.Right - r.Left) / 2, (num4 + num5) / 2);
                            return;
                        }
                        case TowerStyles.Cone:
                            base.chart.graphics3D.Cone(true, r.Left, num2, r.Right, num3, num4, num5, this.dark3D);
                            return;

                        case TowerStyles.Pyramid:
                            base.chart.graphics3D.Pyramid(true, r.Left, num2, r.Right, num3, num4, num5, this.dark3D);
                            break;

                        default:
                            return;
                    }
                }
            }
        }

        private void DrawCells()
        {
            this.IOffW = this.percWidth * 0.005;
            this.IOffD = this.percDepth * 0.005;
            if (base.chart.axes.Depth.Inverted)
            {
                if (!this.DrawValuesForward())
                {
                    for (int i = base.NumXValues; i >= 1; i--)
                    {
                        for (int j = 1; j <= base.NumZValues; j++)
                        {
                            this.DrawCell(i, j);
                        }
                    }
                }
                else
                {
                    for (int k = 1; k <= base.NumXValues; k++)
                    {
                        for (int m = 1; m <= base.NumZValues; m++)
                        {
                            this.DrawCell(k, m);
                        }
                    }
                }
            }
            else if (!this.DrawValuesForward())
            {
                for (int n = base.NumXValues; n >= 1; n--)
                {
                    for (int num6 = base.NumZValues; num6 >= 1; num6--)
                    {
                        this.DrawCell(n, num6);
                    }
                }
            }
            else
            {
                for (int num7 = 1; num7 <= base.NumXValues; num7++)
                {
                    for (int num8 = base.NumZValues; num8 >= 1; num8--)
                    {
                        this.DrawCell(num7, num8);
                    }
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            base.Marks.ApplyArrowLength(ref position);
            base.DrawMark(valueIndex, s, position);
        }

        public override double MaxXValue()
        {
            return (base.XValues.Maximum + 0.5);
        }

        public override double MaxZValue()
        {
            return (base.ZValues.Maximum + 0.5);
        }

        public override double MinXValue()
        {
            return (base.XValues.Minimum - 0.5);
        }

        public override double MinZValue()
        {
            return (base.ZValues.Minimum - 0.5);
        }

        public override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            base.iInGallery = true;
            base.CreateValues(5, 5);
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 2:
                    base.Pen.Visible = false;
                    return;

                case 3:
                    base.UsePalette = false;
                    return;

                case 4:
                    base.Marks.Visible = true;
                    return;

                case 5:
                    base.Brush.Visible = false;
                    return;

                case 6:
                    this.TowerStyle = TowerStyles.Rectangle;
                    return;

                case 7:
                    this.TowerStyle = TowerStyles.Cover;
                    return;

                case 8:
                    this.TowerStyle = TowerStyles.Cylinder;
                    return;

                case 9:
                    this.TowerStyle = TowerStyles.Arrow;
                    return;

                case 10:
                    this.TowerStyle = TowerStyles.Cone;
                    return;

                case 11:
                    this.TowerStyle = TowerStyles.Pyramid;
                    return;
            }
            base.SetSubGallery(index);
        }

        [DefaultValue(true), Category("Appearance")]
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

        public override string Description
        {
            get
            {
                return Texts.GalleryTower;
            }
        }

        public double Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                base.SetDoubleProperty(ref this.origin, value);
            }
        }

        [DefaultValue(100)]
        public int PercentDepth
        {
            get
            {
                return this.percDepth;
            }
            set
            {
                base.SetIntegerProperty(ref this.percDepth, value);
            }
        }

        [DefaultValue(100)]
        public int PercentWidth
        {
            get
            {
                return this.percWidth;
            }
            set
            {
                base.SetIntegerProperty(ref this.percWidth, value);
            }
        }

        [DefaultValue(0)]
        public TowerStyles TowerStyle
        {
            get
            {
                return this.towerStyle;
            }
            set
            {
                if (this.towerStyle != value)
                {
                    this.towerStyle = value;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets Transparency level from 0 to 100%."), DefaultValue(0), Category("Appearance")]
        public int Transparency
        {
            get
            {
                return base.bBrush.Transparency;
            }
            set
            {
                base.bBrush.Transparency = value;
            }
        }

        [DefaultValue(false)]
        public bool UseOrigin
        {
            get
            {
                return this.useOrigin;
            }
            set
            {
                base.SetBooleanProperty(ref this.useOrigin, value);
            }
        }
    }
}

