namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(Surface), "SeriesIcons.Surface.bmp")]
    public class Surface : Custom3DGrid
    {
        protected internal bool bWaterFall;
        private bool dotFrame;
        private bool hideCells;
        protected internal int iCalcX;
        protected internal int iCalcY;
        protected Point[] Points;
        private ChartBrush sideBrush;
        private ChartPen sideLines;
        private bool smoothPalette;
        private bool tmpBackCompare;
        private bool tmpBackRange;
        private int tmpYOrigin;
        private ChartPen waterLines;
        private bool wireFrame;

        public Surface() : this(null)
        {
        }

        public Surface(Chart c) : base(c)
        {
            this.Points = new Point[4];
            base.iNextXCell = -1;
            base.iNextZCell = -1;
            this.hideCells = false;
        }

        private int CalcOnePoint(int index, ref Point p0, ref Point p1)
        {
            int num = 0;
            if (index != -1)
            {
                p0 = this.CalcPointPos(index);
                p1.X = p0.X;
                p1.Y = this.tmpYOrigin;
                num = base.CalcZPos(index);
            }
            return num;
        }

        private int CalcOnePoint(int tmpRow, int t, ref Point p0, ref Point p1, int tmpYOrigin)
        {
            int index = base[tmpRow, t];
            p0 = this.CalcPointPos(index);
            p1.X = p0.X;
            p1.Y = tmpYOrigin;
            return base.CalcZPos(index);
        }

        private Point CalcPointPos(int index)
        {
            return new Point(base.GetHorizAxis.CalcXPosValue(base.vxValues[index]), base.GetVertAxis.CalcYPosValue(base.vyValues[index]));
        }

        public Steema.TeeChart.Styles.CellsOrientation CellsOrientation()
        {
            Steema.TeeChart.Styles.CellsOrientation orientation;
            int num = this.WaterFall ? 0 : 1;
            int num2 = Math.Min(base.iNumZValues, base.gridIndex.Capacity);
            int num3 = Math.Min(base.iNumXValues, base.gridIndex.Capacity);
            if (base.BackFaced())
            {
                base.iNextZCell = 1;
                orientation.InitZ = 1;
                orientation.EndZ = (num2 - num) + 1;
                orientation.IncZ = 1;
            }
            else
            {
                base.iNextZCell = -1;
                orientation.InitZ = num2;
                orientation.EndZ = (1 + num) - 1;
                orientation.IncZ = -1;
            }
            if (this.DrawValuesForward())
            {
                base.iNextXCell = -1;
                orientation.InitX = 2;
                orientation.EndX = num3 + 1;
                orientation.IncX = 1;
                return orientation;
            }
            base.iNextXCell = 1;
            orientation.InitX = num3 - 1;
            orientation.EndX = 0;
            orientation.IncX = -1;
            return orientation;
        }

        public override int Clicked(int x, int y)
        {
            Point[] p = new Point[4];
            if (base.Count > 0)
            {
                base.iNextXCell = -1;
                base.iNextZCell = -1;
                Point point = new Point(x, y);
                Steema.TeeChart.Styles.CellsOrientation orientation = this.CellsOrientation();
                base.iNextZCell = -base.iNextZCell;
                orientation.IncZ = -orientation.IncZ;
                for (int i = orientation.EndZ; i != orientation.InitZ; i += orientation.IncZ)
                {
                    for (int j = orientation.InitX; j != orientation.EndX; j += orientation.IncX)
                    {
                        if (this.FourGridIndex(j, i))
                        {
                            this.PointsTo2D(base.CalcZPos(base.valueIndex0), base.CalcZPos(base.valueIndex2), ref p);
                            if (Graphics3D.PointInPolygon(point, p))
                            {
                                return base.valueIndex0;
                            }
                        }
                    }
                }
            }
            return -1;
        }

        private int Compare(int a, int b, Point[] Cells)
        {
            int num;
            if ((Cells[a].X == -1) || (Cells[b].X == -1))
            {
                return 0;
            }
            double num2 = base.ZValues[base[Cells[a].X, Cells[a].Y]];
            double num3 = base.ZValues[base[Cells[b].X, Cells[b].Y]];
            if (num2 < num3)
            {
                num = 1;
            }
            else if (num2 > num3)
            {
                num = -1;
            }
            else
            {
                num = 0;
            }
            if (this.tmpBackCompare)
            {
                num = -num;
            }
            return num;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.WireFrame);
            AddSubChart(Texts.DotFrame);
            AddSubChart(Texts.Sides);
            AddSubChart(Texts.NoBorder);
        }

        public override void Draw()
        {
            if (base.Count > 0)
            {
                bool flag = this.ShouldDrawSides();
                if (this.ShouldDrawFast())
                {
                    this.FastDraw();
                }
                else
                {
                    if (flag)
                    {
                        this.DrawSides(true);
                    }
                    if (this.HideCells && this.InRangeRotation())
                    {
                        this.DrawAllSorted();
                    }
                    else
                    {
                        this.DrawAllCells();
                    }
                }
                if (flag)
                {
                    this.DrawSides(false);
                }
            }
        }

        private void DrawAllCells()
        {
            int z = 0;
            Steema.TeeChart.Styles.CellsOrientation orientation = this.CellsOrientation();
            if (this.PerspectiveCorrection())
            {
                int initX = orientation.InitX;
                int endX = orientation.EndX;
                int x = initX;
                bool flag = true;
                do
                {
                    for (z = orientation.InitZ; z != orientation.EndZ; z += orientation.IncZ)
                    {
                        this.DrawCell(x, z);
                    }
                    if (flag)
                    {
                        endX -= orientation.IncX;
                        x = endX;
                    }
                    else
                    {
                        initX += orientation.IncX;
                        x = initX;
                    }
                    flag = !flag;
                }
                while (initX != endX);
            }
            else
            {
                while (orientation.InitX != orientation.EndX)
                {
                    for (z = orientation.InitZ; z != orientation.EndZ; z += orientation.IncZ)
                    {
                        this.DrawCell(orientation.InitX, z);
                    }
                    orientation.InitX += orientation.IncX;
                }
            }
        }

        private void DrawAllSorted()
        {
            Point[] s = null;
            int index = 0;
            int newLength = (base.NumXValues - 1) * (base.NumZValues - 1);
            s = (Point[]) Utils.SetLength(s, newLength, typeof(Point));
            try
            {
                Steema.TeeChart.Styles.CellsOrientation orientation = this.CellsOrientation();
                while (orientation.InitX != orientation.EndX)
                {
                    int initZ = orientation.InitZ;
                    while (initZ != orientation.EndZ)
                    {
                        if (base.ExistFourGridIndex(orientation.InitX, initZ))
                        {
                            s[index].X = orientation.InitX;
                            s[index].Y = initZ;
                        }
                        else
                        {
                            s[index].X = -1;
                            s[index].Y = -1;
                        }
                        initZ += orientation.IncZ;
                        index++;
                    }
                    orientation.InitX += orientation.IncX;
                }
                if (base.chart.Axes.Depth.Inverted)
                {
                    this.tmpBackCompare = !this.tmpBackRange;
                }
                else
                {
                    this.tmpBackCompare = this.tmpBackRange;
                }
                this.SortCells(0, newLength - 1, ref s);
                for (int i = 0; i < newLength; i++)
                {
                    this.DrawCell(s[i].X, s[i].Y);
                }
            }
            finally
            {
                s = null;
            }
        }

        protected internal virtual void DrawCell(int x, int z)
        {
            int num = 0;
            bool flag = false;
            if (this.bWaterFall)
            {
                base.valueIndex0 = base[x, z];
                if (base.valueIndex0 > -1)
                {
                    base.valueIndex3 = base.valueIndex0;
                    base.valueIndex1 = base[x + base.iNextXCell, z];
                    if (base.valueIndex1 > -1)
                    {
                        base.valueIndex2 = base.valueIndex1;
                        this.Points[0] = this.CalcPointPos(base.valueIndex0);
                        this.Points[1] = this.CalcPointPos(base.valueIndex1);
                        flag = true;
                    }
                }
            }
            else
            {
                flag = this.FourGridIndex(x, z);
            }
            if (flag)
            {
                Color valueColorValue;
                if (base.iNextXCell == 1)
                {
                    if (base.iNextZCell == 1)
                    {
                        valueColorValue = this.ValueColor(base.valueIndex3);
                    }
                    else
                    {
                        valueColorValue = this.ValueColor(base.valueIndex0);
                    }
                }
                else if (base.iNextZCell == 1)
                {
                    valueColorValue = this.ValueColor(base.valueIndex2);
                }
                else
                {
                    valueColorValue = this.ValueColor(base.valueIndex1);
                }
                if (valueColorValue != Color.Transparent)
                {
                    num = base.CalcZPos(base.valueIndex0);
                    Color color2 = base.Pen.Color;
                    Color color3 = base.Brush.Color;
                    bool visible = base.Pen.Visible;
                    bool flag3 = base.Brush.Visible;
                    base.chart.graphics3D.Pen = base.Pen;
                    base.chart.graphics3D.Brush = base.Brush;
                    if (base.sameBrush)
                    {
                        if (this.smoothPalette)
                        {
                            valueColorValue = base.GetValueColorValue((((base.vyValues[base.valueIndex0] + base.vyValues[base.valueIndex1]) + base.vyValues[base.valueIndex2]) + base.vyValues[base.valueIndex3]) * 0.25);
                        }
                        if (this.wireFrame)
                        {
                            if (color2.A < 0xff)
                            {
                                valueColorValue = Color.FromArgb(color2.A, valueColorValue.R, valueColorValue.G, valueColorValue.B);
                            }
                            base.chart.graphics3D.Pen.Color = valueColorValue;
                        }
                        if (!this.wireFrame && !this.dotFrame)
                        {
                            if (color3.A < 0xff)
                            {
                                valueColorValue = Color.FromArgb(color3.A, valueColorValue.R, valueColorValue.G, valueColorValue.B);
                            }
                            base.chart.graphics3D.Brush.Color = valueColorValue;
                        }
                        else
                        {
                            base.chart.graphics3D.Brush.Visible = false;
                        }
                    }
                    else if (this.wireFrame)
                    {
                        base.chart.graphics3D.Brush.Visible = false;
                        if (color2.A < 0xff)
                        {
                            valueColorValue = Color.FromArgb(color2.A, valueColorValue.R, valueColorValue.G, valueColorValue.B);
                        }
                        base.chart.graphics3D.Pen.Color = valueColorValue;
                    }
                    this.DrawTheCell(num);
                    base.Pen.Color = color2;
                    base.Brush.Color = color3;
                    base.Pen.Visible = visible;
                    base.Brush.Visible = flag3;
                }
            }
        }

        private void DrawDot(int z0, int z1)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Pixel(this.Points[0].X, this.Points[0].Y, z0, this.ValueColor(base.valueIndex0));
            graphicsd.Pixel(this.Points[1].X, this.Points[1].Y, z0, this.ValueColor(base.valueIndex1));
            graphicsd.Pixel(this.Points[2].X, this.Points[2].Y, z1, this.ValueColor(base.valueIndex2));
            graphicsd.Pixel(this.Points[3].X, this.Points[3].Y, z1, this.ValueColor(base.valueIndex3));
        }

        private bool DrawFrontSideFirst(int ZPos)
        {
            Point[] p = new Point[4];
            Graphics3D graphicsd = base.chart.graphics3D;
            Rectangle chartRect = base.chart.ChartRect;
            p[0] = graphicsd.Calculate3DPosition(chartRect.Right, chartRect.Top, ZPos);
            int y = chartRect.Bottom + base.chart.Walls.CalcWallSize(base.chart.Axes.Bottom);
            p[1] = graphicsd.Calculate3DPosition(chartRect.Right, y, ZPos);
            p[2] = graphicsd.Calculate3DPosition(chartRect.Left, y, ZPos);
            return !graphicsd.TeeCull(p);
        }

        private void DrawSideCell(int a, int b, int c, int d, int tmpYOrigin, Graphics3D g)
        {
            Point[] p = new Point[4];
            int z = this.CalcOnePoint(a, b, ref p[0], ref p[1], tmpYOrigin);
            int num2 = this.CalcOnePoint(c, d, ref p[3], ref p[2], tmpYOrigin);
            p[0] = g.Calc3DPoint(p[0], z);
            p[1] = g.Calc3DPoint(p[1], z);
            p[2] = g.Calc3DPoint(p[2], num2);
            p[3] = g.Calc3DPoint(p[3], num2);
            if (Graphics3D.Cull(p))
            {
                g.Polygon(p);
            }
        }

        protected virtual void DrawSidePortion(Point[] P, int z0, int z1)
        {
            base.chart.graphics3D.PlaneFour3D(z0, z1, P);
        }

        private void DrawSides(bool BeforeCells)
        {
            int numXValues;
            Point[] pointArray = new Point[4];
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Brush = this.SideBrush;
            graphicsd.Pen = this.SideLines;
            if (base.GetVertAxis.Inverted)
            {
                this.tmpYOrigin = base.CalcYPosValue(base.YValues.Maximum);
            }
            else
            {
                this.tmpYOrigin = base.CalcYPosValue(base.YValues.Minimum);
            }
            bool flag = base.chart.DrawLeftWallFirst();
            bool flag3 = base.chart.DrawRightWallAfter();
            if ((flag && !BeforeCells) || (!flag && BeforeCells))
            {
                if (base.GetHorizAxis.Inverted)
                {
                    numXValues = base.NumXValues;
                }
                else
                {
                    numXValues = 1;
                }
                this.DrawZSide(numXValues);
            }
            if ((flag3 && !BeforeCells) || (!flag3 && BeforeCells))
            {
                if (base.GetHorizAxis.Inverted)
                {
                    numXValues = 1;
                }
                else
                {
                    numXValues = base.NumXValues;
                }
                this.DrawZSide(numXValues);
            }
            bool flag2 = !this.DrawFrontSideFirst(base.EndZ);
            if ((flag2 && BeforeCells) || (!flag2 && !BeforeCells))
            {
                if (base.chart.Axes.Depth.Inverted)
                {
                    numXValues = 1;
                }
                else
                {
                    numXValues = base.NumZValues;
                }
                this.DrawXSide(numXValues);
            }
            flag2 = !this.DrawFrontSideFirst(0);
            if ((flag2 && !BeforeCells) || (!flag2 && BeforeCells))
            {
                if (base.chart.Axes.Depth.Inverted)
                {
                    numXValues = base.NumZValues;
                }
                else
                {
                    numXValues = 1;
                }
                this.DrawXSide(numXValues);
                int z = this.CalcOnePoint(base[base.NumXValues, numXValues], ref pointArray[0], ref pointArray[1]);
                graphicsd.VerticalLine(pointArray[0].X, pointArray[0].Y, this.tmpYOrigin, z);
            }
        }

        private void DrawTheCell(int z0)
        {
            int num;
            Graphics3D graphicsd = base.chart.graphics3D;
            if (this.bWaterFall)
            {
                int iEndPos = base.GetVertAxis.IEndPos;
                if (!this.dotFrame)
                {
                    if (!this.wireFrame)
                    {
                        bool visible = graphicsd.Pen.Visible;
                        graphicsd.Pen.Visible = false;
                        graphicsd.Plane(this.Points[0], this.Points[1], new Point(this.Points[1].X, iEndPos), new Point(this.Points[0].X, iEndPos), z0);
                        graphicsd.Pen.Visible = visible;
                    }
                    graphicsd.Pen = base.Pen;
                    graphicsd.Line(this.Points[0].X, this.Points[0].Y, this.Points[1].X, this.Points[1].Y, z0);
                }
                if ((this.waterLines != null) && this.waterLines.Visible)
                {
                    graphicsd.Pen = this.waterLines;
                    graphicsd.VerticalLine(this.Points[0].X, this.Points[0].Y, iEndPos, z0);
                    graphicsd.VerticalLine(this.Points[1].X, this.Points[1].Y, iEndPos, z0);
                }
                if (this.dotFrame)
                {
                    num = base.CalcZPos(base.valueIndex2);
                    this.DrawDot(z0, num);
                }
            }
            else if (this.dotFrame)
            {
                num = base.CalcZPos(base.valueIndex2);
                this.DrawDot(z0, num);
            }
            else
            {
                num = base.CalcZPos(base.valueIndex2);
                graphicsd.PlaneFour3D(z0, num, this.Points);
            }
        }

        private void DrawXSide(int tmpRow)
        {
            Point[] p = new Point[4];
            for (int i = 2; i <= base.NumXValues; i++)
            {
                int num2 = this.CalcOnePoint(base[i, tmpRow], ref p[0], ref p[1]);
                int num3 = this.CalcOnePoint(base[i - 1, tmpRow], ref p[3], ref p[2]);
                this.DrawSidePortion(p, num2, num3);
            }
        }

        private void DrawZSide(int tmpRow)
        {
            Point[] p = new Point[4];
            for (int i = base.NumZValues; i >= 2; i--)
            {
                int num2 = this.CalcOnePoint(base[tmpRow, i], ref p[0], ref p[1]);
                int num3 = this.CalcOnePoint(base[tmpRow, i - 1], ref p[3], ref p[2]);
                this.DrawSidePortion(p, num2, num3);
            }
        }

        public bool FastCalcPoints(int x, int z, out Point3D p0, out Point3D p1, out Color color0, out Color color1)
        {
            p0.X = 0;
            p0.Y = 0;
            p0.Z = 0;
            p1.X = 0;
            p1.Y = 0;
            p1.Z = 0;
            color0 = Utils.EmptyColor;
            color1 = Utils.EmptyColor;
            bool flag = false;
            base.valueIndex0 = base.gridIndex[x - 1][z];
            if (base.valueIndex0 == -1)
            {
                return flag;
            }
            base.valueIndex1 = base.gridIndex[x][z];
            if (base.valueIndex1 == -1)
            {
                return flag;
            }
            Axis getHorizAxis = base.GetHorizAxis;
            p0.X = getHorizAxis.CalcXPosValue(base.XValues[base.valueIndex0]);
            p1.X = getHorizAxis.CalcXPosValue(base.XValues[base.valueIndex1]);
            p0.Z = base.CalcZPos(base.valueIndex0);
            p1.Z = base.CalcZPos(base.valueIndex1);
            double num = base.YValues[base.valueIndex0];
            double num2 = base.YValues[base.valueIndex1];
            getHorizAxis = base.GetVertAxis;
            p0.Y = getHorizAxis.CalcYPosValue(num);
            p1.Y = getHorizAxis.CalcYPosValue(num2);
            if (base.sameBrush)
            {
                color0 = this.ValueColor(base.valueIndex0);
                color1 = this.ValueColor(base.valueIndex1);
            }
            return true;
        }

        private void FastDraw()
        {
            SurfaceStyle wire;
            if (this.wireFrame)
            {
                wire = SurfaceStyle.Wire;
            }
            else if (this.dotFrame)
            {
                wire = SurfaceStyle.Dot;
            }
            else
            {
                wire = SurfaceStyle.Solid;
            }
            base.chart.Graphics3D.Pen = base.Pen;
            base.chart.Graphics3D.Brush = base.Brush;
            base.chart.Graphics3D.Surface3D(wire, !base.sameBrush, base.iNumXValues, base.iNumZValues, this);
        }

        protected bool FourGridIndex(int x, int z)
        {
            int num = x;
            int num2 = z;
            bool flag = base.ExistFourGridIndex(num, num2);
            if (flag)
            {
                this.Points[0] = this.CalcPointPos(base.valueIndex0);
                this.Points[1] = this.CalcPointPos(base.valueIndex1);
                this.Points[2] = this.CalcPointPos(base.valueIndex2);
                this.Points[3] = this.CalcPointPos(base.valueIndex3);
            }
            return flag;
        }

        private bool InRangeRotation()
        {
            this.tmpBackRange = (base.chart.Aspect.Rotation > 150) && (base.chart.Aspect.Rotation < 210);
            if (!this.tmpBackRange && (base.chart.Aspect.Rotation >= 30))
            {
                return (base.chart.Aspect.Rotation > 330);
            }
            return true;
        }

        private bool PerspectiveCorrection()
        {
            bool flag = !base.Chart.Graphics3D.SupportsFullRotation && (base.Chart.Aspect.Perspective > 0);
            if (flag)
            {
                int num = base.Chart.Aspect.Rotation % 360;
                flag = ((num == 360) || (num == 180)) || (num == 0);
            }
            return flag;
        }

        private void PointsTo2D(int Z0, int Z1, ref Point[] P)
        {
            P[0] = base.Chart.Graphics3D.Calculate3DPosition(this.Points[0], Z0);
            P[1] = base.Chart.Graphics3D.Calculate3DPosition(this.Points[1], Z0);
            P[2] = base.Chart.Graphics3D.Calculate3DPosition(this.Points[2], Z1);
            P[3] = base.Chart.Graphics3D.Calculate3DPosition(this.Points[3], Z1);
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.iInGallery = true;
            base.CreateValues(10, 10);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.sideBrush != null)
            {
                this.sideBrush.Chart = c;
            }
            if (this.waterLines != null)
            {
                this.waterLines.Chart = c;
            }
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 2:
                    this.WireFrame = true;
                    return;

                case 3:
                    this.DotFrame = true;
                    return;

                case 4:
                    this.SideBrush.Visible = true;
                    return;

                case 5:
                    base.Pen.Visible = false;
                    return;
            }
            base.SetSubGallery(index);
        }

        protected virtual bool ShouldDrawFast()
        {
            return base.chart.graphics3D.SupportsFullRotation;
        }

        protected virtual bool ShouldDrawSides()
        {
            return (((this.sideBrush != null) && this.sideBrush.visible) || ((this.sideLines != null) && this.sideLines.Visible));
        }

        private void SortCells(int l, int r, ref Point[] Cells)
        {
            int a = l;
            int b = r;
            int num3 = (a + b) >> 1;
            while (a < b)
            {
                while (this.Compare(a, num3, Cells) < 0)
                {
                    a++;
                }
                while (this.Compare(num3, b, Cells) < 0)
                {
                    b--;
                }
                if (a < b)
                {
                    Point point = Cells[a];
                    Cells[a] = Cells[b];
                    Cells[b] = point;
                    if (a == num3)
                    {
                        num3 = b;
                    }
                    else if (b == num3)
                    {
                        num3 = a;
                    }
                }
                if (a <= b)
                {
                    a++;
                    b--;
                }
            }
            if (l < b)
            {
                this.SortCells(l, b, ref Cells);
            }
            if (a < r)
            {
                this.SortCells(a, r, ref Cells);
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GallerySurface;
            }
        }

        [DefaultValue(false), Description("Sets SurfaceSeries as a grid of dots (pixels).")]
        public bool DotFrame
        {
            get
            {
                return this.dotFrame;
            }
            set
            {
                if (value)
                {
                    base.Pen.Visible = true;
                    this.wireFrame = false;
                }
                base.SetBooleanProperty(ref this.dotFrame, value);
            }
        }

        [DefaultValue(false)]
        public bool HideCells
        {
            get
            {
                return this.hideCells;
            }
            set
            {
                base.SetBooleanProperty(ref this.hideCells, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines the Brush to fill the sides of a Surface Series.")]
        public ChartBrush SideBrush
        {
            get
            {
                if (this.sideBrush == null)
                {
                    this.sideBrush = new ChartBrush(base.chart, Color.Black, false);
                }
                return this.sideBrush;
            }
            set
            {
                this.sideBrush = value;
            }
        }

        [Description(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen SideLines
        {
            get
            {
                if (this.sideLines == null)
                {
                    this.sideLines = new ChartPen(base.chart, Color.White, false);
                }
                return this.sideLines;
            }
            set
            {
                this.sideLines = value;
            }
        }

        [Description("Determine the cell Colors of a Surface Series."), DefaultValue(false)]
        public bool SmoothPalette
        {
            get
            {
                return this.smoothPalette;
            }
            set
            {
                base.SetBooleanProperty(ref this.smoothPalette, value);
            }
        }

        [DefaultValue(false), Description("Enables/disables the display as a waterfall.")]
        public bool WaterFall
        {
            get
            {
                return this.bWaterFall;
            }
            set
            {
                base.SetBooleanProperty(ref this.bWaterFall, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets Pen to draw valuelines.")]
        public ChartPen WaterLines
        {
            get
            {
                if (this.waterLines == null)
                {
                    this.waterLines = new ChartPen(base.chart, Color.Black);
                }
                return this.waterLines;
            }
        }

        [DefaultValue(false), Description("Shows Surface polygons as wire frame when true.")]
        public bool WireFrame
        {
            get
            {
                return this.wireFrame;
            }
            set
            {
                if (value)
                {
                    base.Pen.Visible = true;
                    this.dotFrame = false;
                }
                base.SetBooleanProperty(ref this.wireFrame, value);
            }
        }
    }
}

