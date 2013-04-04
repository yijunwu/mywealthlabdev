namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(Contour), "SeriesIcons.Contour.bmp")]
    public class Contour : Custom3DGrid
    {
        private bool automaticLevels;
        private ContourDraw cDraw;
        private ContourSeriesMarks contourmarks;
        private ContourConstruction drawingalgorithm;
        private bool fillLevels;
        private ChartPen framepen;
        protected internal bool iModifyingLevels;
        private List<ContourLevel> levels;
        public Color LinesColor;
        private int numLevels;
        private SeriesPointer pointer;
        private double yPosition;
        private bool yPositionLevel;

        public event DrawLevelEventHandler DrawLevel;

        public event GetLevelEventHandler GetLevel;

        public Contour() : this(null)
        {
        }

        public Contour(Chart c) : base(c)
        {
            this.automaticLevels = true;
            this.levels = new List<ContourLevel>();
            this.numLevels = 10;
            this.drawingalgorithm = ContourConstruction.Segments;
            this.LinesColor = Utils.EmptyColor;
            base.Marks.Transparent = true;
            this.LinesColor = Color.Black;
            this.Frame.Color = Color.Black;
            this.Frame.Visible = false;
            if (this.cDraw == null)
            {
                this.cDraw = new ContourDraw(this);
            }
        }

        protected override void AddSampleValues(int numValues)
        {
            base.AddSampleValues(numValues);
            this.yPosition = 0.5 * (base.vyValues.Maximum + base.vyValues.Minimum);
        }

        private void ClearAutoLevels()
        {
            this.iModifyingLevels = true;
            try
            {
                this.levels.Clear();
            }
            finally
            {
                this.iModifyingLevels = false;
            }
        }

        public override int Clicked(int x, int y)
        {
            for (int i = 0; i < this.levels.Count; i++)
            {
                int num;
                int num2;
                if (this.levels[i].Clicked(x, y, out num, out num2))
                {
                    return i;
                }
            }
            return -1;
        }

        protected internal override int CountLegendItems()
        {
            return this.levels.Count;
        }

        public void CreateAutoLevels()
        {
            if (this.automaticLevels && (this.numLevels > 0))
            {
                this.iModifyingLevels = true;
                if (this.levels.Count != this.numLevels)
                {
                    this.levels.Clear();
                }
                double num = base.YValues.Range / Math.Max((double) 1.0, (double) (this.numLevels - 1.0));
                double minimum = base.vyValues.Minimum;
                for (int i = 0; i < this.numLevels; i++)
                {
                    ContourLevel level;
                    if (this.levels.Count != this.numLevels)
                    {
                        level = new ContourLevel(this, i) {
                            UpToValue = minimum + (num * i)
                        };
                        this.levels.Add(level);
                    }
                    else
                    {
                        level = this.levels[i];
                        if (level.DefaultPen())
                        {
                            level.Color = base.ColorEach ? this.ValueColor(i) : base.GetValueColorValue(level.UpToValue);
                        }
                        else
                        {
                            level.Color = level.Pen.Color;
                        }
                    }
                    if (this.GetLevel != null)
                    {
                        GetLevelEventArgs e = new GetLevelEventArgs(i, level.UpToValue, level.Color);
                        this.GetLevel(this, e);
                        level.UpToValue = e.Value;
                        level.Color = e.Color;
                    }
                }
                this.iModifyingLevels = false;
            }
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Positions);
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            if ((!this.iModifyingLevels && this.automaticLevels) && (this.numLevels > 0))
            {
                this.CreateAutoLevels();
            }
        }

        public override void Draw()
        {
            if (base.Count > 0)
            {
                this.cDraw.Draw();
            }
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            g.Brush.Visible = true;
            if (valueIndex == -1)
            {
                g.Brush.Color = base.Color;
            }
            else
            {
                g.Brush.Color = this.LegendItemColor(valueIndex);
            }
            base.DrawLegendShape(g, valueIndex, rect);
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            if (!this.ContourMarks.AtSegments)
            {
                position.LeftTop.X = this.CalcXPos(valueIndex);
                position.LeftTop.Y = base.CalcYPosValue(base.ZValues[valueIndex]);
                base.DrawMark(valueIndex, s, position);
            }
        }

        protected override void DrawMarks()
        {
            if (!this.ContourMarks.AtSegments)
            {
                base.DrawMarks();
            }
        }

        private int GetNumLevels()
        {
            if (this.AutomaticLevels)
            {
                return this.numLevels;
            }
            return this.Levels.Count;
        }

        protected internal override Color LegendItemColor(int legendIndex)
        {
            return this.LegendLevel(legendIndex).InternalColor();
        }

        private ContourLevel LegendLevel(int index)
        {
            return this.Levels[(this.numLevels - index) - 1];
        }

        public override string LegendString(int legendIndex, LegendTextStyles legendTextStyle)
        {
            return this.LegendLevel(legendIndex).UpToValue.ToString(base.valueFormat);
        }

        public override double MaxYValue()
        {
            if (!base.chart.Aspect.View3D)
            {
                return base.vzValues.Maximum;
            }
            return base.MaxYValue();
        }

        public override double MinYValue()
        {
            if (!base.chart.Aspect.View3D)
            {
                return base.vzValues.Minimum;
            }
            return base.MinYValue();
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.framepen != null)
            {
                this.framepen.Chart = c;
            }
            if (this.pointer != null)
            {
                this.pointer.Chart = c;
            }
        }

        private void SetNumLevels(int value)
        {
            base.SetIntegerProperty(ref this.numLevels, value);
            if (this.AutomaticLevels)
            {
                this.levels.Clear();
                this.AutomaticLevels = true;
            }
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 2:
                    base.ColorEach = true;
                    break;

                case 3:
                    this.YPositionLevel = true;
                    break;

                default:
                    base.SetSubGallery(index);
                    break;
            }
            this.YPositionLevel = true;
        }

        [DefaultValue(true), Description("Sets ContourSeries Automatic Levels.")]
        public bool AutomaticLevels
        {
            get
            {
                return this.automaticLevels;
            }
            set
            {
                base.SetBooleanProperty(ref this.automaticLevels, value);
                if (this.automaticLevels)
                {
                    this.ClearAutoLevels();
                }
            }
        }

        public ContourSeriesMarks ContourMarks
        {
            get
            {
                if (this.contourmarks == null)
                {
                    this.contourmarks = new ContourSeriesMarks(this);
                }
                return this.contourmarks;
            }
            set
            {
                this.contourmarks = value;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryContour;
            }
        }

        [DefaultValue(1), Description("Contour series drawing algorithm.")]
        public ContourConstruction DrawingAlgorithm
        {
            get
            {
                return this.drawingalgorithm;
            }
            set
            {
                if (this.drawingalgorithm != value)
                {
                    this.drawingalgorithm = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("Fill contour levels.")]
        public bool FillLevels
        {
            get
            {
                return this.fillLevels;
            }
            set
            {
                base.SetBooleanProperty(ref this.fillLevels, value);
                base.bBrush.Visible = this.fillLevels;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines Series bounding frame characteristic.")]
        public ChartPen Frame
        {
            get
            {
                if (this.framepen == null)
                {
                    this.framepen = new ChartPen(base.chart, Color.Black, false);
                }
                return this.framepen;
            }
            set
            {
                this.framepen = value;
            }
        }

        public Axis GetZAxis
        {
            get
            {
                if (base.chart == null)
                {
                    return base.GetVertAxis;
                }
                if (!base.Chart.Aspect.view3D)
                {
                    return base.GetVertAxis;
                }
                return base.Chart.Axes.Depth;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Accesses ContourLevel characteristics by Level index.")]
        public List<ContourLevel> Levels
        {
            get
            {
                return this.levels;
            }
            set
            {
                this.levels = value;
            }
        }

        [DefaultValue(10), Description("Sets the number of levels for the ContourSeries.")]
        public int NumLevels
        {
            get
            {
                return this.numLevels;
            }
            set
            {
                this.SetNumLevels(value);
            }
        }

        [Description("Contour series Pointer characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public SeriesPointer Pointer
        {
            get
            {
                if (this.pointer == null)
                {
                    this.pointer = new SeriesPointer(base.Chart, this);
                    this.pointer.Pen.Visible = false;
                    this.pointer.VertSize = 2;
                    this.pointer.HorizSize = 2;
                    this.pointer.Visible = false;
                }
                return this.pointer;
            }
            set
            {
                this.pointer = value;
            }
        }

        [Description("Sets the Y-Axis height of the Contour Series.")]
        public double YPosition
        {
            get
            {
                return this.yPosition;
            }
            set
            {
                base.SetDoubleProperty(ref this.yPosition, value);
            }
        }

        [DefaultValue(false), Description("Enables/disables YPosition to be set.")]
        public bool YPositionLevel
        {
            get
            {
                return this.yPositionLevel;
            }
            set
            {
                base.SetBooleanProperty(ref this.yPositionLevel, value);
            }
        }

        public class ContourDraw : TeeBase
        {
            private ContourConstruction algo;
            private double[] cellX;
            private int[] cellXi;
            private double[] cellZ;
            private int[] cellZi;
            private Contour cs;
            private double[] difY;
            private TempLevel[] iLevels;
            private const double invtol = 10000.0;
            private Point[] P;
            private static byte[,,] Sides = new byte[,,] { { { 0, 0, 8 }, { 0, 2, 5 }, { 7, 6, 9 } }, { { 0, 3, 4 }, { 1, 3, 1 }, { 4, 3, 0 } }, { { 9, 6, 7 }, { 5, 2, 0 }, { 8, 0, 0 } } };
            private TempLevel tLevel;
            private bool tmpDrawMarks;
            private int tmpNumLevels;
            private string tmpSt;
            private int tmpY;
            private Axis tmpZAxis;
            private const double tol = 0.0001;

            public ContourDraw(Contour cSeries) : base(cSeries.chart)
            {
                this.algo = ContourConstruction.Segments;
                this.difY = new double[5];
                this.cellX = new double[5];
                this.cellZ = new double[5];
                this.cellXi = new int[5];
                this.cellZi = new int[5];
                this.cs = cSeries;
            }

            private void AddSegmentLine(double x1, double z1, double x2, double z2, int theLevel)
            {
                LevelSegment segment;
                bool flag = false;
                ContourLevel lvl = this.cs.Levels[theLevel];
                for (int i = 0; i < lvl.Segments.Count; i++)
                {
                    LevelPoint[] pointArray;
                    segment = lvl.Segments[i];
                    if (Utils.Round((double) (10000.0 * (Math.Abs((double) (x1 - segment.Points[0].X)) + Math.Abs((double) (z1 - segment.Points[0].Y))))) == 0)
                    {
                        flag = true;
                        segment.Count++;
                        pointArray = new LevelPoint[segment.Count - 1];
                        segment.Points.CopyTo(pointArray, 0);
                        segment.Points = new LevelPoint[segment.Count];
                        pointArray.CopyTo(segment.Points, 1);
                        segment.Points[0].X = x2;
                        segment.Points[0].Y = z2;
                        if (lvl.Segments.Count > 1)
                        {
                            this.CheckOtherSegments(i, 0, lvl);
                        }
                        break;
                    }
                    if (Utils.Round((double) (10000.0 * (Math.Abs((double) (x2 - segment.Points[0].X)) + Math.Abs((double) (z2 - segment.Points[0].Y))))) == 0)
                    {
                        flag = true;
                        segment.Count++;
                        pointArray = new LevelPoint[segment.Count - 1];
                        segment.Points.CopyTo(pointArray, 0);
                        segment.Points = new LevelPoint[segment.Count];
                        pointArray.CopyTo(segment.Points, 1);
                        segment.Points[0].X = x1;
                        segment.Points[0].Y = z1;
                        if (lvl.Segments.Count > 1)
                        {
                            this.CheckOtherSegments(i, 0, lvl);
                        }
                        break;
                    }
                    if (Utils.Round((double) (10000.0 * (Math.Abs((double) (x1 - segment.Points[segment.Count - 1].X)) + Math.Abs((double) (z1 - segment.Points[segment.Count - 1].Y))))) == 0)
                    {
                        flag = true;
                        segment.Count++;
                        segment.Points = Utils.SetLength(segment.Points, segment.Count, typeof(LevelPoint)) as LevelPoint[];
                        segment.Points[segment.Count - 1].X = x2;
                        segment.Points[segment.Count - 1].Y = z2;
                        if (lvl.Segments.Count > 1)
                        {
                            this.CheckOtherSegments(i, segment.Count - 1, lvl);
                        }
                        break;
                    }
                    if (Utils.Round((double) (10000.0 * (Math.Abs((double) (x2 - segment.Points[segment.Count - 1].X)) + Math.Abs((double) (z2 - segment.Points[segment.Count - 1].Y))))) == 0)
                    {
                        flag = true;
                        segment.Count++;
                        segment.Points = Utils.SetLength(segment.Points, segment.Count, typeof(LevelPoint)) as LevelPoint[];
                        segment.Points[segment.Count - 1].X = x1;
                        segment.Points[segment.Count - 1].Y = z1;
                        if (lvl.Segments.Count > 1)
                        {
                            this.CheckOtherSegments(i, segment.Count - 1, lvl);
                        }
                        break;
                    }
                }
                if (!flag)
                {
                    segment = new LevelSegment {
                        Count = 2,
                        Points = new LevelPoint[2]
                    };
                    segment.Points[0].X = x1;
                    segment.Points[0].Y = z1;
                    segment.Points[1].X = x2;
                    segment.Points[1].Y = z2;
                    lvl.Segments.Add(segment);
                }
            }

            private void CalcLevel(int theLevel)
            {
                int num;
                int[] numArray = new int[5];
                for (num = 0; num <= 4; num++)
                {
                    if (this.difY[num] > 0.0)
                    {
                        numArray[num] = 1;
                    }
                    else if (this.difY[num] < 0.0)
                    {
                        numArray[num] = -1;
                    }
                    else
                    {
                        numArray[num] = 0;
                    }
                }
                int num2 = numArray[0];
                for (num = 1; num <= 4; num++)
                {
                    int index = num;
                    int num4 = (num == 4) ? 1 : (num + 1);
                    int side = Sides[1 + numArray[index], 1 + num2, 1 + numArray[num4]];
                    if (side != 0)
                    {
                        if (this.algo == ContourConstruction.Segments)
                        {
                            this.CalcPointsSegment(index, num4, side, theLevel);
                        }
                        else
                        {
                            this.CalcLinePoints(index, num4, side, theLevel);
                        }
                    }
                }
            }

            private void CalcLinePoints(int m1, int m3, int side, int theLevel)
            {
                TempLevel level = this.iLevels[theLevel];
                if (level.Count >= level.Allocated)
                {
                    level.Allocated += 0x3e8;
                    level.Line = new LevelLine[level.Allocated];
                }
                LevelLine line = level.Line[level.Count];
                switch (side)
                {
                    case 1:
                        line.x1 = this.cellXi[m1];
                        line.z1 = this.cellZi[m1];
                        line.x2 = this.cellXi[0];
                        line.z2 = this.cellZi[0];
                        break;

                    case 2:
                        line.x1 = this.cellXi[0];
                        line.z1 = this.cellZi[0];
                        line.x2 = this.cellXi[m3];
                        line.z2 = this.cellZi[m3];
                        break;

                    case 3:
                        line.x1 = this.cellXi[m3];
                        line.z1 = this.cellZi[m3];
                        line.x2 = this.cellXi[m1];
                        line.z2 = this.cellZi[m1];
                        break;

                    case 4:
                        line.x1 = this.cellXi[m1];
                        line.z1 = this.cellZi[m1];
                        this.PointSect(0, m3, out line.x2, out line.z2);
                        break;

                    case 5:
                        line.x1 = this.cellXi[0];
                        line.z1 = this.cellZi[0];
                        this.PointSect(m3, m1, out line.x2, out line.z2);
                        break;

                    case 6:
                        line.x1 = this.cellXi[m3];
                        line.z1 = this.cellZi[m3];
                        this.PointSect(m1, 0, out line.x2, out line.z2);
                        break;

                    case 7:
                        this.PointSect(m1, 0, out line.x1, out line.z1);
                        this.PointSect(0, m3, out line.x2, out line.z2);
                        break;

                    case 8:
                        this.PointSect(0, m3, out line.x1, out line.z1);
                        this.PointSect(m3, m1, out line.x2, out line.z2);
                        break;

                    case 9:
                        this.PointSect(m3, m1, out line.x1, out line.z1);
                        this.PointSect(m1, 0, out line.x2, out line.z2);
                        break;
                }
                level.Line[level.Count] = line;
                level.Count++;
                this.iLevels[theLevel] = level;
            }

            private void CalcPointsSegment(int m1, int m3, int side, int theLevel)
            {
                double ax = 0.0;
                double az = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                switch (side)
                {
                    case 1:
                        ax = this.cellX[m1];
                        az = this.cellZ[m1];
                        num3 = this.cellX[0];
                        num4 = this.cellZ[0];
                        break;

                    case 2:
                        ax = this.cellX[0];
                        az = this.cellZ[0];
                        num3 = this.cellX[m3];
                        num4 = this.cellZ[m3];
                        break;

                    case 3:
                        ax = this.cellX[m3];
                        az = this.cellZ[m3];
                        num3 = this.cellX[m1];
                        num4 = this.cellZ[m1];
                        break;

                    case 4:
                        ax = this.cellX[m1];
                        az = this.cellZ[m1];
                        this.PointSect(0, m3, out num3, out num4);
                        break;

                    case 5:
                        ax = this.cellX[0];
                        az = this.cellZ[0];
                        this.PointSect(m3, m1, out num3, out num4);
                        break;

                    case 6:
                        ax = this.cellX[m3];
                        az = this.cellZ[m3];
                        this.PointSect(m1, 0, out num3, out num4);
                        break;

                    case 7:
                        this.PointSect(m1, 0, out ax, out az);
                        this.PointSect(0, m3, out num3, out num4);
                        break;

                    case 8:
                        this.PointSect(0, m3, out ax, out az);
                        this.PointSect(m3, m1, out num3, out num4);
                        break;

                    case 9:
                        this.PointSect(m3, m1, out ax, out az);
                        this.PointSect(m1, 0, out num3, out num4);
                        break;
                }
                this.AddSegmentLine(ax, az, num3, num4, theLevel);
            }

            private void CalculateLevels()
            {
                this.cs.iNextXCell = 1;
                this.cs.iNextZCell = 1;
                for (int i = 1; i < this.cs.iNumZValues; i++)
                {
                    for (int j = 1; j < this.cs.iNumXValues; j++)
                    {
                        if (this.cs.ExistFourGridIndex(j, i))
                        {
                            if (this.algo == ContourConstruction.Segments)
                            {
                                this.cellZ[1] = this.cs.vzValues[this.cs.valueIndex0];
                                this.cellZ[3] = this.cs.vzValues[this.cs.valueIndex3];
                                this.cellZ[2] = this.cellZ[1];
                                this.cellZ[4] = this.cellZ[3];
                                this.cellZ[0] = (this.cellZ[1] + this.cellZ[3]) * 0.5;
                                this.cellX[1] = this.cs.vxValues[this.cs.valueIndex0];
                                this.cellX[2] = this.cs.vxValues[this.cs.valueIndex1];
                                this.cellX[3] = this.cellX[2];
                                this.cellX[4] = this.cellX[1];
                                this.cellX[0] = (this.cellX[1] + this.cellX[2]) * 0.5;
                            }
                            else
                            {
                                this.cellZi[1] = this.tmpZAxis.CalcYPosValue(this.cs.vzValues[this.cs.valueIndex0]);
                                this.cellZi[3] = this.tmpZAxis.CalcYPosValue(this.cs.vzValues[this.cs.valueIndex3]);
                                this.cellZi[2] = this.cellZi[1];
                                this.cellZi[4] = this.cellZi[3];
                                this.cellZi[0] = (this.cellZi[1] + this.cellZi[3]) / 2;
                                Axis getHorizAxis = this.cs.GetHorizAxis;
                                this.cellXi[1] = getHorizAxis.CalcXPosValue(this.cs.XValues[this.cs.valueIndex0]);
                                this.cellXi[2] = getHorizAxis.CalcXPosValue(this.cs.XValues[this.cs.valueIndex1]);
                                this.cellXi[3] = this.cellXi[2];
                                this.cellXi[4] = this.cellXi[1];
                                this.cellXi[0] = (this.cellXi[1] + this.cellXi[2]) / 2;
                            }
                            ValueList yValues = this.cs.YValues;
                            double num6 = yValues[this.cs.valueIndex0];
                            double num7 = yValues[this.cs.valueIndex3];
                            double num8 = yValues[this.cs.valueIndex1];
                            double num9 = yValues[this.cs.valueIndex2];
                            double num10 = 0.25 * (((num6 + num7) + num8) + num9);
                            double num4 = num6;
                            if (num7 < num4)
                            {
                                num4 = num7;
                            }
                            if (num8 < num4)
                            {
                                num4 = num8;
                            }
                            if (num9 < num4)
                            {
                                num4 = num9;
                            }
                            if (num4 <= this.iLevels[this.tmpNumLevels - 1].UpToValue)
                            {
                                double num5 = num6;
                                if (num7 > num5)
                                {
                                    num5 = num7;
                                }
                                if (num8 > num5)
                                {
                                    num5 = num8;
                                }
                                if (num9 > num5)
                                {
                                    num5 = num9;
                                }
                                if (num5 >= this.iLevels[0].UpToValue)
                                {
                                    for (int k = 0; k < this.tmpNumLevels; k++)
                                    {
                                        this.tLevel = this.iLevels[k];
                                        if ((this.tLevel.UpToValue >= num4) && (this.tLevel.UpToValue <= num5))
                                        {
                                            this.difY[1] = num6 - this.tLevel.UpToValue;
                                            this.difY[2] = num8 - this.tLevel.UpToValue;
                                            this.difY[3] = num9 - this.tLevel.UpToValue;
                                            this.difY[4] = num7 - this.tLevel.UpToValue;
                                            this.difY[0] = num10 - this.tLevel.UpToValue;
                                            this.CalcLevel(k);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            private void CheckOtherSegments(int numsegment, int atpos, ContourLevel lvl)
            {
                LevelPoint point = lvl.Segments[numsegment].Points[atpos];
                int count = lvl.Segments[numsegment].Count;
                for (int i = 0; i < lvl.Segments.Count; i++)
                {
                    if (i != numsegment)
                    {
                        LevelSegment segment = lvl.Segments[i];
                        int num3 = segment.Count;
                        LevelPoint point2 = segment.Points[0];
                        if ((Math.Abs((double) (point.X - point2.X)) <= 0.0001) && (Math.Abs((double) (point.Y - point2.Y)) <= 0.0001))
                        {
                            LevelSegment segment2 = lvl.Segments[numsegment];
                            segment2.Count = (num3 + count) - 1;
                            segment2.Points = Utils.SetLength(segment2.Points, segment2.Count, typeof(LevelPoint)) as LevelPoint[];
                            if (atpos != 0)
                            {
                                for (int j = 1; j < num3; j++)
                                {
                                    segment2.Points[(count + j) - 1] = segment.Points[j];
                                }
                            }
                            else
                            {
                                for (int k = count - 1; k >= 0; k--)
                                {
                                    segment2.Points[(k + num3) - 1] = segment2.Points[k];
                                }
                                for (int m = 0; m < (num3 - 1); m++)
                                {
                                    segment2.Points[m] = segment.Points[(num3 - 1) - m];
                                }
                            }
                            this.DeleteSegment(lvl, i);
                            return;
                        }
                        point2 = segment.Points[num3 - 1];
                        if ((Math.Abs((double) (point.X - point2.X)) <= 0.0001) && (Math.Abs((double) (point.Y - point2.Y)) <= 0.0001))
                        {
                            LevelSegment segment3 = lvl.Segments[numsegment];
                            segment3.Count = (num3 + count) - 1;
                            segment3.Points = Utils.SetLength(segment3.Points, segment3.Count, typeof(LevelPoint)) as LevelPoint[];
                            if (atpos != 0)
                            {
                                for (int n = 0; n < (num3 - 1); n++)
                                {
                                    segment3.Points[count + n] = segment.Points[(num3 - 2) - n];
                                }
                            }
                            else
                            {
                                for (int num8 = count - 1; num8 >= 0; num8--)
                                {
                                    segment3.Points[(num8 + num3) - 1] = segment3.Points[num8];
                                }
                                for (int num9 = 0; num9 < (num3 - 1); num9++)
                                {
                                    segment3.Points[num9] = segment.Points[num9];
                                }
                            }
                            this.DeleteSegment(lvl, i);
                            return;
                        }
                    }
                }
            }

            private void DeleteSegment(ContourLevel lvl, int index)
            {
                lvl.Segments[index].Points = null;
                lvl.Segments.RemoveAt(index);
            }

            protected internal void Draw()
            {
                if ((this.cs.Count > 0) && (this.cs.NumLevels > 0))
                {
                    this.tmpNumLevels = this.cs.NumLevels;
                    this.iLevels = new TempLevel[this.tmpNumLevels];
                    this.algo = this.cs.drawingalgorithm;
                    this.PrepareLevels();
                    this.CalculateLevels();
                    this.DrawLevelLines();
                    if (this.cs.Frame.Visible)
                    {
                        this.DrawFrame();
                    }
                }
            }

            private void DrawFrame()
            {
                Graphics3D graphicsd = this.cs.Chart.Graphics3D;
                graphicsd.Brush.Visible = false;
                graphicsd.Pen = this.cs.framepen;
                int a = this.cs.GetHorizAxis.CalcPosValue(this.cs.MinXValue());
                int b = this.cs.GetHorizAxis.CalcPosValue(this.cs.MaxXValue());
                int num4 = this.cs.GetZAxis.CalcPosValue(this.cs.MinZValue());
                int num3 = this.cs.GetZAxis.CalcPosValue(this.cs.MaxZValue());
                if (a > b)
                {
                    Utils.SwapInteger(ref a, ref b);
                }
                if (num3 > num4)
                {
                    Utils.SwapInteger(ref num3, ref num4);
                }
                if (this.cs.Chart.Aspect.View3D)
                {
                    graphicsd.RectangleY(a, this.tmpY, b, num3, num4);
                }
                else
                {
                    graphicsd.Rectangle(Utils.FromLTRB(a, num3, b, num4));
                }
            }

            private void DrawLevelLines()
            {
                Graphics3D graphicsd = this.cs.chart.Graphics3D;
                Aspect aspect = this.cs.Chart.Aspect;
                SeriesMarks.Position position = new SeriesMarks.Position();
                this.P = null;
                this.tmpY = this.cs.GetVertAxis.CalcYPosValue(this.cs.yPosition);
                if (this.cs.FillLevels)
                {
                    this.FillLevels();
                }
                this.cs.Marks.Positions.Clear();
                for (int i = 0; i < this.tmpNumLevels; i++)
                {
                    Point point;
                    TempLevel level = this.iLevels[i];
                    if (this.cs.yPositionLevel)
                    {
                        this.tmpY = this.cs.GetVertAxis.CalcYPosValue(level.UpToValue);
                    }
                    this.tmpDrawMarks = (this.cs.Marks.Visible & this.cs.ContourMarks.AtSegments) & ((i % this.cs.Marks.DrawEvery) == 0);
                    if (this.tmpDrawMarks)
                    {
                        switch (this.cs.Marks.Style)
                        {
                            case MarksStyles.SeriesTitle:
                                this.tmpSt = this.cs.Title;
                                break;

                            case MarksStyles.PointIndex:
                                this.tmpSt = i.ToString();
                                break;

                            default:
                                this.tmpSt = level.UpToValue.ToString(this.cs.ValueFormat);
                                break;
                        }
                        this.cs.CalculateMarkPosition(this.cs.Marks, this.tmpSt, 0, 0, position);
                        point = new Point(position.Width, position.Height);
                    }
                    else
                    {
                        point = new Point(0, 0);
                    }
                    graphicsd.Pen = level.Pen.Clone() as ChartPen;
                    graphicsd.Pen.Color = level.LineColor;
                    graphicsd.Pen.Visible = level.Pen.Visible;
                    if (this.cs.DrawLevel != null)
                    {
                        this.cs.DrawLevel(this.cs, new Contour.DrawLevelEventArgs(i));
                    }
                    if (this.algo == ContourConstruction.Segments)
                    {
                        int num2 = level.Level.Segments.Count - 1;
                        if (((i == 0) || (i == (this.tmpNumLevels - 1))) && ((num2 == -1) && (this.cs.Pointer.Visible || this.tmpDrawMarks)))
                        {
                            this.DrawSingleMark(level.UpToValue, level.Color, position);
                        }
                        for (int j = 0; j <= num2; j++)
                        {
                            this.P = level.Level.GetSegmentPoints(j);
                            if (this.tmpDrawMarks)
                            {
                                int num4 = point.X + (2 * this.cs.ContourMarks.Margin);
                                if (aspect.View3D)
                                {
                                    graphicsd.MoveTo(this.P[0].X, this.tmpY, this.P[0].Y);
                                }
                                else
                                {
                                    graphicsd.MoveTo(this.P[0]);
                                }
                                int index = 1;
                                int num6 = 0;
                                int num7 = 0;
                                int length = this.P.Length;
                                while (index < length)
                                {
                                    int num9 = this.P[index].X - this.P[num7].X;
                                    int num10 = this.P[index].Y - this.P[num7].Y;
                                    double num11 = Math.Sqrt((double) ((num9 * num9) + (num10 * num10)));
                                    if (num11 > num4)
                                    {
                                        num9 = this.P[index].X - this.P[index - 1].X;
                                        num10 = this.P[index].Y - this.P[index - 1].Y;
                                        double num12 = Math.Sqrt((double) ((num9 * num9) + (num10 * num10)));
                                        num6++;
                                        if ((num6 % this.cs.ContourMarks.Density) == 0)
                                        {
                                            Point point2;
                                            if (num12 > num4)
                                            {
                                                for (int m = num7 + 1; m < index; m++)
                                                {
                                                    if (aspect.View3D)
                                                    {
                                                        graphicsd.LineTo(this.P[m].X, this.tmpY, this.P[m].Y);
                                                    }
                                                    else
                                                    {
                                                        graphicsd.LineTo(this.P[m].X, this.P[m].Y);
                                                    }
                                                }
                                                num7 = index - 1;
                                                num11 = num12;
                                            }
                                            if (num7 == (index - 1))
                                            {
                                                point2 = Graphics3D.PointAtDistance(this.P[index], this.P[index - 1], Utils.Round((double) (0.5 * (num12 - num4))));
                                                if (aspect.View3D)
                                                {
                                                    graphicsd.LineTo(point2.X, this.tmpY, point2.Y);
                                                }
                                                else
                                                {
                                                    graphicsd.LineTo(point2.X, point2.Y);
                                                }
                                            }
                                            point2 = Graphics3D.PointAtDistance(this.P[index], this.P[num7], Utils.Round((double) (num11 * 0.5)));
                                            double num13 = Math.Atan2((double) (this.P[index].X - this.P[num7].X), (double) (this.P[index].Y - this.P[num7].Y)) + 1.5707963267948966;
                                            int angle = Utils.Round((double) ((num13 * 180.0) / 3.1415926535897931));
                                            if (angle < 0)
                                            {
                                                angle += 360;
                                            }
                                            if ((angle > 90) && (angle < 270))
                                            {
                                                angle += 180;
                                                if (angle > 360)
                                                {
                                                    angle -= 360;
                                                }
                                            }
                                            this.DrawLevelMark(point2.X, point2.Y + (point.Y / 2), angle, this.tmpSt, level.Color, level.LineColor, position);
                                            if (num12 > num4)
                                            {
                                                point2 = Graphics3D.PointAtDistance(this.P[index], this.P[index - 1], Utils.Round((double) ((num12 + num4) * 0.5)));
                                            }
                                            else
                                            {
                                                point2 = this.P[index];
                                            }
                                            if (aspect.View3D)
                                            {
                                                graphicsd.MoveTo(point2.X, this.tmpY, point2.Y);
                                            }
                                            else
                                            {
                                                graphicsd.MoveTo(point2.X, point2.Y);
                                            }
                                        }
                                        else
                                        {
                                            for (int n = num7; n <= index; n++)
                                            {
                                                if (aspect.View3D)
                                                {
                                                    graphicsd.LineTo(this.P[n].X, this.tmpY, this.P[n].Y);
                                                }
                                                else
                                                {
                                                    graphicsd.LineTo(this.P[n].X, this.P[n].Y);
                                                }
                                            }
                                        }
                                        num7 = index;
                                    }
                                    index++;
                                }
                                for (int k = num7; k < length; k++)
                                {
                                    if (aspect.View3D)
                                    {
                                        graphicsd.LineTo(this.P[k].X, this.tmpY, this.P[k].Y);
                                    }
                                    else
                                    {
                                        graphicsd.LineTo(this.P[k].X, this.P[k].Y);
                                    }
                                }
                            }
                            else if (aspect.View3D)
                            {
                                graphicsd.MoveTo(this.P[0].X, this.tmpY, this.P[0].Y);
                                for (int num18 = 1; num18 < this.P.Length; num18++)
                                {
                                    graphicsd.LineTo(this.P[num18].X, this.tmpY, this.P[num18].Y);
                                }
                            }
                            else
                            {
                                graphicsd.Polyline(this.P);
                            }
                            if (this.cs.Pointer.Visible)
                            {
                                this.DrawPointers(level.Color, level.LineColor);
                            }
                            this.P = null;
                        }
                    }
                    else if (this.algo == ContourConstruction.Fast)
                    {
                        for (int num19 = 0; num19 < level.Count; num19++)
                        {
                            LevelLine line = level.Line[num19];
                            if (aspect.View3D)
                            {
                                graphicsd.MoveTo(line.x1, this.tmpY, line.z1);
                                graphicsd.LineTo(line.x2, this.tmpY, line.z2);
                            }
                            else
                            {
                                graphicsd.Line(line.x1, line.z1, line.x2, line.z2);
                            }
                        }
                    }
                }
            }

            private void DrawLevelMark(int x, int y, int angle, string text, Color color, Color linecolor, SeriesMarks.Position position)
            {
                if (this.cs.Chart.Aspect.View3D)
                {
                    this.cs.Marks.zPosition = y;
                    y = this.tmpY;
                }
                this.cs.Marks.Positions.MoveTo(ref position, x, y);
                if (!this.cs.ContourMarks.AntiOverlap || !this.MarkOverlaps(position))
                {
                    double num = this.cs.Marks.Angle;
                    Color color2 = this.cs.Marks.Font.Color;
                    this.cs.Marks.Angle = num + angle;
                    if (this.cs.ContourMarks.ColorLevel)
                    {
                        this.cs.Marks.Font.Color = color;
                    }
                    this.cs.Chart.Graphics3D.Font = this.cs.Marks.Font;
                    this.cs.Marks.InternalDraw(0, color, text, position);
                    if (this.cs.ContourMarks.AntiOverlap)
                    {
                        SeriesMarks.Position item = new SeriesMarks.Position();
                        item.Assign(position);
                        this.cs.Marks.Positions.Add(item);
                    }
                    this.cs.Marks.Angle = num;
                    if (this.cs.ContourMarks.ColorLevel)
                    {
                        this.cs.Marks.Font.Color = color2;
                    }
                    this.cs.Chart.graphics3D.Pen = this.cs.Pen.Clone() as ChartPen;
                    this.cs.Chart.graphics3D.Pen.Color = linecolor;
                    this.cs.Chart.graphics3D.Pen.Visible = this.cs.Pen.Visible;
                }
            }

            private void DrawPointers(Color color, Color linecolor)
            {
                if (this.cs.Chart.Aspect.View3D)
                {
                    for (int i = 0; i < this.P.Length; i++)
                    {
                        this.cs.startZ = this.P[i].Y;
                        this.cs.endZ = this.cs.startZ;
                        this.cs.middleZ = this.cs.EndZ;
                        this.cs.Pointer.Draw(this.P[i].X, this.tmpY, color);
                    }
                }
                else
                {
                    for (int j = 0; j < this.P.Length; j++)
                    {
                        this.cs.Pointer.Draw(this.P[j].X, this.P[j].Y, color);
                    }
                }
                this.cs.Chart.Graphics3D.Pen = this.cs.Pen.Clone() as ChartPen;
                this.cs.Chart.Graphics3D.Pen.Color = linecolor;
                this.cs.Chart.Graphics3D.Pen.Visible = this.cs.Pen.Visible;
            }

            private void DrawSingleMark(double value, Color color, SeriesMarks.Position position)
            {
                for (int i = 0; i < this.cs.Count; i++)
                {
                    if (this.SamaValue(this.cs.mandatory[i], value, 0.0))
                    {
                        this.P = new Point[1];
                        this.P[0].X = this.cs.GetHorizAxis.CalcXPosValue(this.cs.XValues[i]);
                        this.P[0].Y = this.tmpZAxis.CalcPosValue(this.cs.ZValues[i]);
                        if (this.cs.Pointer.Visible)
                        {
                            this.DrawPointers(color, color);
                        }
                        else if (this.cs.Chart.Aspect.View3D)
                        {
                            this.cs.Chart.Graphics3D.Pixel(this.P[0].X, this.tmpY, this.P[0].Y, color);
                        }
                        else
                        {
                            this.cs.Chart.Graphics3D.Pixel(this.P[0].X, this.P[0].Y, 0, color);
                        }
                        if (this.tmpDrawMarks)
                        {
                            this.DrawLevelMark(this.P[0].X, this.P[0].Y, 0, this.tmpSt, color, color, position);
                        }
                        this.P = null;
                    }
                }
            }

            private void FillLevels()
            {
                using (IsoSurface surface = new IsoSurface(this.cs.Chart))
                {
                    surface.InternalUse = true;
                    surface.Assign(this.cs);
                    surface.UseColorRange = false;
                    surface.PaletteSteps = this.cs.numLevels;
                    surface.Palette.Clear();
                    for (int i = 0; i < this.cs.numLevels; i++)
                    {
                        surface.Palette.Add(new GridPalette(this.iLevels[i].UpToValue, this.iLevels[i].Color));
                    }
                    surface.UseYPosition = !this.cs.yPositionLevel;
                    surface.YPosition = this.cs.yPosition;
                    surface.Pen.Visible = false;
                    surface.BandPen.Visible = false;
                    surface.CalcColorRange();
                    surface.Draw();
                }
            }

            private bool MarkOverlaps(SeriesMarks.Position position)
            {
                Rectangle bounds = position.Bounds;
                SeriesMarks.MarkPositions positions = this.cs.Marks.Positions;
                for (int i = 0; i < positions.Count; i++)
                {
                    if ((positions[i] != null) && !Rectangle.Intersect(positions[i].Bounds, bounds).IsEmpty)
                    {
                        return true;
                    }
                }
                return false;
            }

            private void PointSect(int p1, int p2, out double Ax, out double Az)
            {
                double num = this.difY[p2] - this.difY[p1];
                if (num != 0.0)
                {
                    num = 1.0 / num;
                    Ax = ((this.difY[p2] * this.cellX[p1]) - (this.difY[p1] * this.cellX[p2])) * num;
                    Az = ((this.difY[p2] * this.cellZ[p1]) - (this.difY[p1] * this.cellZ[p2])) * num;
                }
                else
                {
                    Ax = this.cellX[p2] - this.cellX[p1];
                    Az = this.cellZ[p2] - this.cellZ[p1];
                }
            }

            private void PointSect(int p1, int p2, out int Ax, out int Az)
            {
                double num = this.difY[p2] - this.difY[p1];
                if (num != 0.0)
                {
                    num = 1.0 / num;
                    Ax = Utils.Round((double) (((this.difY[p2] * this.cellXi[p1]) - (this.difY[p1] * this.cellXi[p2])) * num));
                    Az = Utils.Round((double) (((this.difY[p2] * this.cellZi[p1]) - (this.difY[p1] * this.cellZi[p2])) * num));
                }
                else
                {
                    Ax = Utils.Round((float) (this.cellXi[p2] - this.cellXi[p1]));
                    Az = Utils.Round((float) (this.cellZi[p2] - this.cellZi[p1]));
                }
            }

            private void PrepareLevels()
            {
                ContourLevel level = null;
                this.tmpZAxis = this.cs.GetZAxis;
                for (int i = 0; i < this.tmpNumLevels; i++)
                {
                    level = this.cs.Levels[i];
                    this.iLevels[i].Level = level;
                    this.iLevels[i].UpToValue = level.UpToValue;
                    this.iLevels[i].Color = level.InternalColor();
                    if (this.algo == ContourConstruction.Segments)
                    {
                        this.iLevels[i].Level.Segments.Clear();
                    }
                    else
                    {
                        this.iLevels[i].Count = 0;
                        this.iLevels[i].Allocated = 0;
                    }
                    this.iLevels[i].Pen = this.iLevels[i].Level.InternalPen();
                    if (!this.cs.FillLevels)
                    {
                        this.iLevels[i].LineColor = this.iLevels[i].Color;
                    }
                    else
                    {
                        this.iLevels[i].LineColor = this.cs.LinesColor;
                        if (this.iLevels[i].LineColor == Utils.EmptyColor)
                        {
                            this.iLevels[i].LineColor = this.iLevels[i].Color;
                        }
                    }
                }
            }

            private bool SamaValue(double a, double b, double epsilon)
            {
                if (epsilon == 0.0)
                {
                    epsilon = Math.Max((double) (Math.Min(Math.Abs(a), Math.Abs(b)) * 1E-12), (double) 1E-12);
                }
                if (a <= b)
                {
                    return ((b - a) <= epsilon);
                }
                return ((a - b) <= epsilon);
            }
        }

        public class DrawLevelEventArgs : EventArgs
        {
            private readonly int lIndex;

            public DrawLevelEventArgs(int LevelIndex)
            {
                this.lIndex = LevelIndex;
            }

            [Description("Accesses the  ContourLevel characteristics selecting Level by index.")]
            public int LevelIndex
            {
                get
                {
                    return this.lIndex;
                }
            }
        }

        public delegate void DrawLevelEventHandler(Contour sender, Contour.DrawLevelEventArgs e);

        public class GetLevelEventArgs : EventArgs
        {
            private System.Drawing.Color color;
            private readonly int lIndex;
            private double lValue;

            public GetLevelEventArgs(int LevelIndex, double Value, System.Drawing.Color Color)
            {
                this.lIndex = LevelIndex;
                this.lValue = Value;
                this.color = Color;
            }

            [Description("Sets the Colour of the ContourSeries Level.")]
            public System.Drawing.Color Color
            {
                get
                {
                    return this.color;
                }
                set
                {
                    this.color = value;
                }
            }

            [Description("Accesses the  ContourLevel characteristics selecting Level by index.")]
            public int LevelIndex
            {
                get
                {
                    return this.lIndex;
                }
            }

            [Description("Gets or Sets the values in the Value array.")]
            public double Value
            {
                get
                {
                    return this.lValue;
                }
                set
                {
                    this.lValue = value;
                }
            }
        }

        public delegate void GetLevelEventHandler(Contour sender, Contour.GetLevelEventArgs e);
    }
}

