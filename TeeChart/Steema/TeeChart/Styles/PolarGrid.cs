namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(PolarGrid), "SeriesIcons.PolarGrid.bmp")]
    public class PolarGrid : CustomPolar
    {
        private ValueList cellValues;
        private bool centered;
        internal Custom3DPalette i3D;
        private SpecialChart iChart;
        private int numSectors;
        private int numTracks;
        private GridColorPalette palette;

        public PolarGrid() : this(null)
        {
        }

        public PolarGrid(Chart c) : base(c)
        {
            this.centered = true;
            this.numSectors = 10;
            this.numTracks = 10;
            this.cellValues = new ValueList(this, "Values");
            base.RotationAngle = 90;
            base.calcVisiblePoints = false;
            base.vxValues.Name = "Sectors";
            base.vxValues.Order = ValueListOrder.None;
            base.vyValues.Name = "Tracks";
            this.i3D = new Custom3DPalette();
            this.iChart = new SpecialChart();
            this.iChart.iLink = this;
            this.iChart.Series.Add(this.i3D);
            this.palette = new GridColorPalette(this.i3D);
            base.Pen.Color = Color.Black;
            base.Pen.Visible = true;
            base.Brush.Visible = true;
            base.Pointer.Visible = false;
        }

        public int AddCell(int sector, int track, double value)
        {
            this.cellValues.TempValue = value;
            return base.Add((double) sector, (double) track);
        }

        protected override void AddSampleValues(int numValues)
        {
            this.numSectors = numValues;
            this.numTracks = numValues;
            base.BeginUpdate();
            for (int i = 0; i < this.NumSectors; i++)
            {
                for (int j = 0; j < this.numTracks; j++)
                {
                    double num = ((0.5 * Math.Pow(Math.Cos(((double) i) / (this.NumSectors * 0.2)), 2.0)) + Math.Pow(Math.Cos(((double) j) / (this.NumTracks * 0.2)), 2.0)) - Math.Cos(((double) j) / (this.NumTracks * 0.5));
                    this.AddCell(i, j, num);
                }
            }
            base.EndUpdate();
        }

        public override void AssignFormat(Series source)
        {
            base.AssignFormat(source);
            if (source is PolarGrid)
            {
                this.centered = (source as PolarGrid).Centered;
                this.numTracks = (source as PolarGrid).NumTracks;
                this.numSectors = (source as PolarGrid).NumSectors;
                this.palette.EndColor = (source as PolarGrid).Palette.EndColor;
                this.palette.MidColor = (source as PolarGrid).Palette.MidColor;
                this.palette.PaletteMin = (source as PolarGrid).Palette.PaletteMin;
                this.palette.PaletteStep = (source as PolarGrid).Palette.PaletteStep;
                this.palette.PaletteSteps = (source as PolarGrid).Palette.PaletteSteps;
                this.palette.PaletteStyle = (source as PolarGrid).Palette.PaletteStyle;
                this.palette.StartColor = (source as PolarGrid).Palette.StartColor;
                this.palette.UseColorRange = (source as PolarGrid).Palette.UseColorRange;
                this.palette.UsePalette = (source as PolarGrid).Palette.UsePalette;
                this.palette.UsePaletteMin = (source as PolarGrid).Palette.UsePaletteMin;
            }
        }

        public override int CalcXPos(int valueIndex)
        {
            int num;
            int num2;
            base.CalcXYPos(this.InternalX(valueIndex), base.vyValues[valueIndex], (double) base.XRadius, out num, out num2);
            return num;
        }

        public override int CalcYPos(int valueIndex)
        {
            int num;
            int num2;
            base.CalcXYPos(this.InternalX(valueIndex), base.vyValues[valueIndex], (double) base.YRadius, out num2, out num);
            return num;
        }

        protected void CellPolygon(int valueindex, ref Point[] Points)
        {
            int num4;
            int num5;
            Points[0].X = this.CalcXPos(valueindex);
            Points[0].Y = this.CalcYPos(valueindex);
            double num = 360.0 / ((double) this.numSectors);
            double yvalue = base.vyValues[valueindex];
            double num3 = this.centered ? 0.5 : 0.0;
            double xvalue = ((base.vxValues[valueindex] - num3) + 1.0) * num;
            if (base.ClockWiseLabels)
            {
                xvalue = 360.0 - xvalue;
            }
            base.CalcXYPos(xvalue, yvalue, (double) base.XRadius, out num4, out num5);
            Points[1].X = num4;
            base.CalcXYPos(xvalue, yvalue, (double) base.YRadius, out num4, out num5);
            Points[1].Y = num5;
            if (yvalue > 0.0)
            {
                yvalue--;
                xvalue = (base.vxValues[valueindex] - num3) * num;
                if (base.ClockWiseLabels)
                {
                    xvalue = 360.0 - xvalue;
                }
                base.CalcXYPos(xvalue, yvalue, (double) base.XRadius, out num4, out num5);
                Points[3].X = num4;
                base.CalcXYPos(xvalue, yvalue, (double) base.YRadius, out num4, out num5);
                Points[3].Y = num5;
                xvalue = ((base.vxValues[valueindex] - num3) + 1.0) * num;
                if (base.ClockWiseLabels)
                {
                    xvalue = 360.0 - xvalue;
                }
                base.CalcXYPos(xvalue, yvalue, (double) base.XRadius, out num4, out num5);
                Points[2].X = num4;
                base.CalcXYPos(xvalue, yvalue, (double) base.YRadius, out num4, out num5);
                Points[2].Y = num5;
            }
            else
            {
                Points[3].X = base.CircleXCenter;
                Points[3].Y = base.CircleYCenter;
                Points[2].X = Points[3].X;
                Points[2].Y = Points[3].Y;
            }
        }

        public override int Clicked(int x, int y)
        {
            if (base.Count > 0)
            {
                if (base.chart != null)
                {
                    base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
                }
                Point p = new Point(x, y);
                Point[] points = new Point[4];
                for (int i = 0; i < base.Count; i++)
                {
                    if (!base.IsNull(i))
                    {
                        this.CellPolygon(i, ref points);
                        if (Graphics3D.PointInPolygon(p, points))
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        protected internal override int CountLegendItems()
        {
            return this.i3D.CountLegendItems();
        }

        protected internal override void DoAfterDrawValues()
        {
            base.Chart.graphics3D.Pen = base.CirclePen;
            this.DrawPolarCircle(base.CircleWidth / 2, base.CircleHeight / 2, base.EndZ);
            base.DoAfterDrawValues();
        }

        protected internal override void DoBeforeDrawChart()
        {
            this.i3D.BeginUpdate();
            this.i3D.Clear();
            this.i3D.Add(0.0, this.cellValues.Minimum, 0.0);
            this.i3D.Add(0.0, this.cellValues.Maximum, 0.0);
            this.i3D.Color = base.Color;
            this.i3D.DoBeforeDrawChart();
            this.i3D.EndUpdate();
            base.AngleIncrement = 360.0 / ((double) this.numSectors);
            base.DoBeforeDrawChart();
        }

        protected override void DrawPolarCircle(int HalfWidth, int HalfHeight, int Z)
        {
            int num3;
            int num4;
            double num = this.centered ? 0.5 : 0.0;
            double num2 = 0.017453292519943295 * base.AngleIncrement;
            this.AngleToPos(num * num2, (double) HalfWidth, (double) HalfHeight, out num3, out num4);
            base.chart.graphics3D.MoveTo(num3, num4, Z);
            for (int i = 0; i < this.numSectors; i++)
            {
                this.AngleToPos((i + num) * num2, (double) HalfWidth, (double) HalfHeight, out num3, out num4);
                base.chart.graphics3D.LineTo(num3, num4, Z);
            }
        }

        public override void DrawValue(int valueIndex)
        {
            Point[] points = new Point[4];
            Color cellColor = base.InternalColor(valueIndex);
            if ((cellColor != Color.Transparent) || (base.TreatNulls == TreatNullsStyle.Ignore))
            {
                if (cellColor == Utils.EmptyColor)
                {
                    if (!base.ColorEach && (this.i3D.UseColorRange || this.i3D.UsePalette))
                    {
                        cellColor = this.GetCellColor(this.cellValues[valueIndex]);
                    }
                    else
                    {
                        cellColor = this.ValueColor(valueIndex);
                    }
                }
                this.CellPolygon(valueIndex, ref points);
                Graphics3D graphicsd = base.chart.graphics3D;
                graphicsd.Pen = base.Pen;
                bool visible = graphicsd.Brush.Gradient.Visible;
                bool flag2 = graphicsd.Brush.Visible;
                int transparency = graphicsd.Brush.Transparency;
                Color color2 = graphicsd.Brush.Color;
                try
                {
                    graphicsd.Brush.Gradient.Visible = false;
                    graphicsd.Brush.Color = cellColor;
                    graphicsd.Brush.Transparency = base.Transparency;
                    if (base.chart.Aspect.View3D)
                    {
                        graphicsd.Polygon(base.MiddleZ, points);
                    }
                    else
                    {
                        graphicsd.Polygon(points);
                    }
                }
                finally
                {
                    graphicsd.Brush.Visible = flag2;
                    graphicsd.Brush.Transparency = transparency;
                    graphicsd.Brush.Gradient.Visible = visible;
                    graphicsd.Brush.Color = color2;
                }
            }
        }

        private Color GetCellColor(double value)
        {
            return this.i3D.GetValueColorValue(value);
        }

        protected override string GetCircleLabel(double angle, int index)
        {
            if ((index > 0) && base.ClockWiseLabels)
            {
                index = this.numSectors - index;
            }
            return index.ToString();
        }

        private double InternalX(int valueIndex)
        {
            double num = this.centered ? (base.vxValues[valueIndex] - 0.5) : base.vxValues[valueIndex];
            num *= 360.0 / ((double) this.numSectors);
            if (base.ClockWiseLabels)
            {
                num = 360.0 - num;
            }
            return num;
        }

        public override bool IsValidSourceOf(Series value)
        {
            return (value is PolarGrid);
        }

        protected internal override Color LegendItemColor(int index)
        {
            return this.i3D.LegendItemColor(index);
        }

        public override string LegendString(int legendIndex, LegendTextStyles legendTextStyle)
        {
            return this.i3D.LegendString(legendIndex, legendTextStyle);
        }

        protected internal override int NumSampleValues()
        {
            return 10;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.FillSampleValues(8);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
        }

        public ValueList CellValues
        {
            get
            {
                return this.cellValues;
            }
            set
            {
                base.SetValueList(this.cellValues, value);
            }
        }

        [DefaultValue(true)]
        public bool Centered
        {
            get
            {
                return this.centered;
            }
            set
            {
                base.SetBooleanProperty(ref this.centered, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.PolarGridSeries;
            }
        }

        [DefaultValue(10)]
        public int NumSectors
        {
            get
            {
                return this.numSectors;
            }
            set
            {
                base.SetIntegerProperty(ref this.numSectors, value);
            }
        }

        [DefaultValue(10)]
        public int NumTracks
        {
            get
            {
                return this.numTracks;
            }
            set
            {
                base.SetIntegerProperty(ref this.numTracks, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GridColorPalette Palette
        {
            get
            {
                return this.palette;
            }
            set
            {
                this.palette = value;
                this.Invalidate();
            }
        }

        internal class SpecialChart : TChart
        {
            internal Series iLink;

            public override void DoInvalidate()
            {
                if (this.iLink != null)
                {
                    this.iLink.Repaint();
                }
            }
        }
    }
}

