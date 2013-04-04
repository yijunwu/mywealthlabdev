namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [ToolboxBitmap(typeof(Points3D), "SeriesIcons.Points3D.bmp")]
    public class Points3D : Custom3D
    {
        private ChartPen baselinePen;
        private double depthSize;
        private int IOldX;
        private int IOldY;
        private int IOldZ;
        private ChartPen linePen;
        private SeriesPointer pointer;
        private TreatNullsStyle treatnulls;

        public event GetPointerStyleEventHandler GetPointerStyle;

        public Points3D() : this(null)
        {
        }

        public Points3D(Chart c) : base(c)
        {
            this.pointer = new SeriesPointer(base.chart, this);
            this.linePen = new ChartPen(base.chart, Color.Black);
            this.baselinePen = new ChartPen(base.chart, Color.Black, false);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                base.Add((double) (100.0 * random.Random()), (double) (100.0 * random.Random()), (double) (100.0 * random.Random()));
            }
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            this.pointer.CalcHorizMargins(ref LeftMargin, ref RightMargin);
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            this.pointer.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
        }

        private void CalcZPositions(int valueIndex)
        {
            this.CalcZPositions(valueIndex, base.CalcZPos(valueIndex));
        }

        private void CalcZPositions(int valueIndex, int tmpMiddleZ)
        {
            base.middleZ = tmpMiddleZ;
            int num = Math.Max(1, base.chart.Axes.Depth.CalcSizeValue(this.depthSize) / 2);
            base.startZ = base.middleZ - num;
            base.endZ = base.middleZ + num;
        }

        public override int Clicked(int x, int y)
        {
            int num = x;
            int num2 = y;
            int num5 = base.Clicked(x, y);
            if ((num5 == -1) && this.pointer.Visible)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    int num3 = this.CalcXPos(i);
                    int num4 = this.CalcYPos(i);
                    x = num;
                    y = num2;
                    if (base.chart != null)
                    {
                        base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.CalcZPos(i));
                    }
                    if ((Math.Abs((int) (num3 - x)) < this.pointer.HorizSize) && (Math.Abs((int) (num4 - y)) < this.pointer.VertSize))
                    {
                        return i;
                    }
                }
            }
            return num5;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.NoPoint);
            AddSubChart(Texts.BaseLine);
            AddSubChart(Texts.NoLine);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.Point2D);
            AddSubChart(Texts.Triangle);
            AddSubChart(Texts.Star);
            AddSubChart(Texts.Circle);
            AddSubChart(Texts.DownTri);
            AddSubChart(Texts.Cross);
            AddSubChart(Texts.Diamond);
        }

        public override void Draw()
        {
            this.IOldX = -1;
            this.IOldY = -1;
            this.IOldZ = -1;
            base.Draw();
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle r)
        {
            if (this.pointer.Visible)
            {
                Color aColor = (valueIndex == -1) ? base.Color : this.ValueColor(valueIndex);
                PointerStyles style = this.pointer.Style;
                this.OnGetPointerStyle(valueIndex, ref style, ref aColor);
                this.pointer.Style = style;
                this.pointer.DrawLegendShape(g, aColor, r, this.LinePen.Visible);
            }
            else
            {
                base.DrawLegendShape(g, valueIndex, r);
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            this.CalcZPositions(valueIndex);
            base.Marks.ZPosition = this.pointer.Visible ? ((base.StartZ + base.EndZ) / 2) : base.StartZ;
            base.Marks.ApplyArrowLength(ref position);
            base.DrawMark(valueIndex, s, position);
        }

        private void DrawPointer(int valueIndex, int tmpX, int tmpY)
        {
            if (this.pointer.Visible)
            {
                Color aColor = this.ValueColor(valueIndex);
                PointerStyles style = this.pointer.Style;
                this.OnGetPointerStyle(valueIndex, ref style, ref aColor);
                this.pointer.Draw(base.chart.graphics3D, base.chart.Aspect.View3D, tmpX, tmpY, this.pointer.HorizSize, this.pointer.VertSize, aColor, style);
            }
        }

        public override void DrawValue(int valueIndex)
        {
            int tmpX = 0;
            int tmpY = 0;
            if (this.ShouldDrawPoint(valueIndex, ref tmpX, ref tmpY))
            {
                if (this.BaseLine.Visible)
                {
                    int z = base.CalcZPos(valueIndex);
                    base.chart.graphics3D.Pen = this.BaseLine;
                    if (((tmpY + this.pointer.VertSize) - base.GetVertAxis.IEndPos) < 0)
                    {
                        base.chart.graphics3D.MoveTo(tmpX, tmpY + this.pointer.VertSize, z);
                        base.chart.graphics3D.LineTo(tmpX, base.GetVertAxis.IEndPos, z);
                    }
                }
                if (((this.IOldX == -1) && (this.IOldY == -1)) && (this.IOldZ == -1))
                {
                    this.IOldX = tmpX;
                    this.IOldY = tmpY;
                    this.IOldZ = base.MiddleZ;
                }
                if ((valueIndex >= base.firstVisible) && this.linePen.Visible)
                {
                    base.chart.graphics3D.Pen = this.LinePen;
                    base.chart.graphics3D.MoveTo(this.IOldX, this.IOldY, this.IOldZ);
                    base.chart.graphics3D.LineTo(tmpX, tmpY, base.MiddleZ);
                }
                this.IOldX = tmpX;
                this.IOldY = tmpY;
                this.IOldZ = base.MiddleZ;
            }
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        public override double MaxZValue()
        {
            return (base.vzValues.Maximum + this.depthSize);
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            this.Pointer.Gradient.StartColor = color;
            if (!base.ColorEach)
            {
                Color color2 = Utils.DarkenColor(color, 60);
                this.Pointer.Pen.Color = color2;
            }
            if ((this.pointer != null) && (this.pointer.Color != color))
            {
                this.pointer.Color = color;
            }
            this.Invalidate();
        }

        protected void OnGetPointerStyle(int valueIndex, ref PointerStyles style, ref Color aColor)
        {
            if (this.GetPointerStyle != null)
            {
                GetPointerStyleEventArgs e = new GetPointerStyleEventArgs(valueIndex, style) {
                    Color = aColor
                };
                this.GetPointerStyle(this, e);
                style = e.Style;
                aColor = e.Color;
            }
        }

        public override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            this.linePen.Color = Color.Navy;
            base.chart.Aspect.Zoom = 60;
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (this.pointer != null)
            {
                this.pointer.Chart = value;
            }
            if (this.linePen != null)
            {
                this.linePen.Chart = value;
            }
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.Pointer.Visible = false;
                    return;

                case 2:
                    this.BaseLine.Visible = true;
                    return;

                case 3:
                    this.LinePen.Visible = false;
                    return;

                case 4:
                    base.ColorEach = true;
                    return;

                case 5:
                    base.Marks.Visible = true;
                    return;

                case 6:
                    this.Pointer.Brush.Visible = false;
                    return;

                case 7:
                    this.Pointer.Pen.Visible = false;
                    return;

                case 8:
                    this.Pointer.Draw3D = false;
                    return;

                case 9:
                    this.Pointer.Style = PointerStyles.Triangle;
                    return;

                case 10:
                    this.Pointer.Style = PointerStyles.Star;
                    return;

                case 11:
                    this.Pointer.Style = PointerStyles.Circle;
                    return;

                case 12:
                    this.Pointer.Style = PointerStyles.DownTriangle;
                    return;

                case 13:
                    this.Pointer.Style = PointerStyles.Cross;
                    return;

                case 14:
                    this.Pointer.Style = PointerStyles.Diamond;
                    return;
            }
            base.SetSubGallery(index);
        }

        private bool ShouldDrawPoint(int valueIndex, ref int tmpX, ref int tmpY)
        {
            bool flag = !base.IsNull(valueIndex);
            if (flag)
            {
                this.CalcZPositions(valueIndex);
                tmpX = this.CalcXPos(valueIndex);
                tmpY = this.CalcYPos(valueIndex);
                this.DrawPointer(valueIndex, tmpX, tmpY);
                if (((this.TreatNulls == TreatNullsStyle.DoNotPaint) && (valueIndex > 0)) && base.IsNull(valueIndex - 1))
                {
                    flag = false;
                    this.IOldX = tmpX;
                    this.IOldY = tmpY;
                    this.IOldZ = base.MiddleZ;
                }
                return flag;
            }
            if (this.TreatNulls == TreatNullsStyle.Ignore)
            {
                tmpX = base.CalcXPosValue((double) valueIndex);
                tmpY = base.CalcYPosValue(base.DefaultNullValue);
                this.CalcZPositions(valueIndex);
                return true;
            }
            if (this.TreatNulls == TreatNullsStyle.Skip)
            {
                if (valueIndex == 0)
                {
                    this.IOldX = base.CalcXPosValue((double) valueIndex);
                    this.IOldY = base.CalcYPosValue(base.DefaultNullValue);
                    this.CalcZPositions(valueIndex);
                }
                tmpX = this.IOldX;
                tmpY = this.IOldY;
                this.CalcZPositions(valueIndex, this.IOldZ);
            }
            return flag;
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Draws lines from every point to the base floor.")]
        public ChartPen BaseLine
        {
            get
            {
                return this.baselinePen;
            }
        }

        [DefaultValue((double) 0.0), Description("Sets the Depth of each 3DPoint to the value of DepthSize.")]
        public double DepthSize
        {
            get
            {
                return this.depthSize;
            }
            set
            {
                base.SetDoubleProperty(ref this.depthSize, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryPoint3D;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Sets the Pen for the Point3D connecting Lines.")]
        public ChartPen LinePen
        {
            get
            {
                return this.linePen;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue((string) null), Description("Each point in a PointSeries is drawn using the Pointer properties.")]
        public SeriesPointer Pointer
        {
            get
            {
                if (this.pointer == null)
                {
                    this.pointer = new SeriesPointer(base.chart, this);
                }
                if (base.Color != this.pointer.Color)
                {
                    this.pointer.Color = base.Color;
                }
                this.Invalidate();
                return this.pointer;
            }
        }

        [Description("Defines how null points are treated."), DefaultValue(0)]
        public TreatNullsStyle TreatNulls
        {
            get
            {
                return this.treatnulls;
            }
            set
            {
                if (this.treatnulls != value)
                {
                    this.treatnulls = value;
                    base.Repaint();
                }
            }
        }

        public delegate void GetPointerStyleEventHandler(Points3D series, GetPointerStyleEventArgs e);
    }
}

