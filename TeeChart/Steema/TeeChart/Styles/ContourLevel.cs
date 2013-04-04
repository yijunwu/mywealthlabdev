namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class ContourLevel
    {
        private System.Drawing.Color color;
        private int index;
        private Contour iSeries;
        private ChartPen pen;
        private List<LevelSegment> segments = new List<LevelSegment>();
        private double upTo;

        public ContourLevel(Contour cs, int levelindex)
        {
            this.iSeries = cs;
            this.index = levelindex;
        }

        private void CheckAuto()
        {
            if (!this.iSeries.iModifyingLevels)
            {
                this.iSeries.AutomaticLevels = false;
            }
        }

        public bool Clicked(int x, int y, out int segmentindex, out int pointindex)
        {
            for (int i = 0; i < this.segments.Count; i++)
            {
                if (this.ClickedSegment(x, y, i, out pointindex))
                {
                    segmentindex = i;
                    return true;
                }
            }
            segmentindex = -1;
            pointindex = -1;
            return false;
        }

        public bool ClickedSegment(int x, int y, int segmentindex, out int pointindex)
        {
            if (this.iSeries.Chart != null)
            {
                this.iSeries.Chart.Graphics3D.Calculate2DPosition(ref x, ref y, this.iSeries.middleZ);
            }
            Point[] segmentPoints = this.GetSegmentPoints(segmentindex);
            Point p = new Point(x, y);
            int length = segmentPoints.Length;
            for (int i = 0; i < (segmentPoints.Length - 1); i++)
            {
                if (Graphics3D.PointInLine(p, segmentPoints[i], segmentPoints[i + 1]))
                {
                    pointindex = i;
                    return true;
                }
            }
            pointindex = -1;
            return false;
        }

        public bool DefaultPen()
        {
            return (this.pen == null);
        }

        public Point[] GetSegmentPoints(int segmentindex)
        {
            int count = this.segments[segmentindex].Count;
            Point[] pointArray = new Point[count];
            Axis getHorizAxis = this.iSeries.GetHorizAxis;
            Axis getZAxis = this.iSeries.GetZAxis;
            for (int i = 0; i < count; i++)
            {
                pointArray[i].X = getHorizAxis.CalcXPosValue(this.segments[segmentindex].Points[i].X);
                pointArray[i].Y = getZAxis.CalcPosValue(this.segments[segmentindex].Points[i].Y);
            }
            return pointArray;
        }

        internal System.Drawing.Color InternalColor()
        {
            if (!(this.Color == Utils.EmptyColor))
            {
                return this.Color;
            }
            return this.iSeries.GetValueColorValue(this.UpToValue);
        }

        internal ChartPen InternalPen()
        {
            if (this.pen == null)
            {
                return this.iSeries.Pen;
            }
            return this.pen;
        }

        private void SetColor(System.Drawing.Color value)
        {
            this.color = value;
            if (this.iSeries != null)
            {
                this.iSeries.Invalidate();
            }
            if (this.pen != null)
            {
                this.pen.Color = value;
            }
            this.CheckAuto();
        }

        private void SetUpTo(double value)
        {
            this.upTo = value;
            this.CheckAuto();
        }

        public bool ShouldSerializePen()
        {
            return (this.pen != null);
        }

        [Category("Appearance"), Description("Colour of TContourSeries Level.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                if (this.color != value)
                {
                    this.SetColor(value);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen characteristics of the ContourLevel."), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(this.iSeries.Chart);
                }
                return this.pen;
            }
            set
            {
                this.pen = value;
                this.iSeries.Invalidate();
            }
        }

        public List<LevelSegment> Segments
        {
            get
            {
                return this.segments;
            }
        }

        [Description("Sets range value for ContourLevel.")]
        public double UpToValue
        {
            get
            {
                return this.upTo;
            }
            set
            {
                this.SetUpTo(value);
            }
        }
    }
}

