namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Funnel), "SeriesIcons.Funnel.bmp")]
    public class Funnel : Series
    {
        private Color aboveColor;
        private bool autoUpdate;
        private Color belowColor;
        private Point[] BoundingPoints;
        private double differenceLimit;
        private double IDiff;
        private double IMax;
        private double IMin;
        private int IPolyCount;
        private Point[] IPolyPoints;
        private double ISlope;
        private bool ISorted;
        private ChartPen linesPen;
        private ValueList opportunityValues;
        private ChartPen pen;
        private bool quotesSorted;
        private Color withinColor;

        public Funnel() : this(null)
        {
        }

        public Funnel(Chart c) : base(c)
        {
            this.autoUpdate = true;
            this.differenceLimit = 30.0;
            this.IPolyPoints = new Point[5];
            this.BoundingPoints = new Point[4];
            this.opportunityValues = new ValueList(this, Texts.OpportunityValues);
            this.opportunityValues.Order = ValueListOrder.None;
            base.vxValues.Order = ValueListOrder.None;
            base.PercentFormat = Texts.FunnelPercent;
            this.QuoteValues.Order = ValueListOrder.Descending;
            base.YValues.Name = Texts.QuoteValues;
            base.UseSeriesColor = false;
            base.bBrush.Transparency = 0;
            base.ColorEach = true;
            this.aboveColor = base.GetDefaultColor(0);
            this.belowColor = base.GetDefaultColor(1);
            this.withinColor = base.GetDefaultColor(2);
        }

        public int Add(double aQuote, double aOpportunity, string aLabel, Color aColor)
        {
            this.OpportunityValues.TempValue = aOpportunity;
            int num = base.Add(aQuote, aLabel, aColor);
            if (!this.quotesSorted)
            {
                base.YValues.Sort();
                base.XValues.FillSequence();
                this.ISorted = true;
            }
            return num;
        }

        protected override void AddSampleValues(int numValues)
        {
            Random random = new Random();
            for (int i = 0; i < numValues; i++)
            {
                this.Add((2.3 * numValues) * (i + 1), ((2.3 - random.Next(2)) * numValues) * (i + 2), Texts.FunnelSegment + Convert.ToString((int) (i + 1)), Utils.EmptyColor);
            }
        }

        protected internal override int CountLegendItems()
        {
            return 3;
        }

        private Color DefineFunnelRegion(int index)
        {
            if (this.QuoteValues[index] == 0.0)
            {
                this.IDiff = 0.0;
            }
            else
            {
                this.IDiff = this.OpportunityValues[index] / this.QuoteValues[index];
            }
            this.BoundingPoints[0].X = base.CalcXPosValue(index - 0.5);
            this.BoundingPoints[0].Y = base.CalcYPosValue(this.IMax - (this.ISlope * index));
            this.BoundingPoints[1].X = base.CalcXPosValue(index + 0.5);
            this.BoundingPoints[1].Y = base.CalcYPosValue(this.IMax - (this.ISlope * (index + 1)));
            this.BoundingPoints[2].X = this.BoundingPoints[1].X;
            this.BoundingPoints[2].Y = base.CalcYPosValue(this.ISlope * (index + 1));
            this.BoundingPoints[3].X = this.BoundingPoints[0].X;
            this.BoundingPoints[3].Y = base.CalcYPosValue(this.ISlope * index);
            int num = base.CalcYPosValue(((this.IMax - ((2.0 * this.ISlope) * index)) * this.IDiff) + (this.ISlope * index));
            if (num <= this.BoundingPoints[0].Y)
            {
                this.IPolyCount = 4;
                this.IPolyPoints[0] = this.BoundingPoints[0];
                this.IPolyPoints[1] = this.BoundingPoints[1];
                this.IPolyPoints[2] = this.BoundingPoints[2];
                this.IPolyPoints[3] = this.BoundingPoints[3];
            }
            else if ((num > this.BoundingPoints[0].Y) && (num <= this.BoundingPoints[1].Y))
            {
                this.IPolyCount = 5;
                this.IPolyPoints[0].X = this.BoundingPoints[0].X;
                this.IPolyPoints[0].Y = num;
                int num2 = base.CalcXPosValue(((((this.IMax - ((2.0 * this.ISlope) * index)) * (1.0 - this.IDiff)) / this.ISlope) + index) - 0.5);
                this.IPolyPoints[1].X = num2;
                this.IPolyPoints[1].Y = num;
                this.IPolyPoints[2] = this.BoundingPoints[1];
                this.IPolyPoints[3] = this.BoundingPoints[2];
                this.IPolyPoints[4] = this.BoundingPoints[3];
            }
            else if (num > this.BoundingPoints[2].Y)
            {
                this.IPolyCount = 3;
                this.IPolyPoints[0].X = this.BoundingPoints[0].X;
                this.IPolyPoints[0].Y = num;
                int num3 = base.CalcXPosValue(((((this.IMax - ((2.0 * this.ISlope) * index)) * this.IDiff) / this.ISlope) + index) - 0.5);
                this.IPolyPoints[1].X = num3;
                this.IPolyPoints[1].Y = num;
                this.IPolyPoints[2] = this.BoundingPoints[3];
            }
            else
            {
                this.IPolyCount = 4;
                this.IPolyPoints[0].X = this.BoundingPoints[0].X;
                this.IPolyPoints[0].Y = num;
                this.IPolyPoints[1].X = this.BoundingPoints[1].X;
                this.IPolyPoints[1].Y = num;
                this.IPolyPoints[2] = this.BoundingPoints[2];
                this.IPolyPoints[3] = this.BoundingPoints[3];
            }
            if (this.IDiff >= 1.0)
            {
                return this.aboveColor;
            }
            if (((1.0 - this.IDiff) * 100.0) > this.differenceLimit)
            {
                return this.belowColor;
            }
            return this.withinColor;
        }

        protected override void Dispose(bool disposing)
        {
            if ((disposing && base.Visible) && (base.GetVertAxis != null))
            {
                base.GetVertAxis.Visible = true;
            }
            base.Dispose(disposing);
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            if (base.Visible && (base.GetVertAxis != null))
            {
                base.GetVertAxis.Visible = false;
            }
        }

        public override void Draw()
        {
            if (this.autoUpdate)
            {
                this.Recalc();
            }
            base.Draw();
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Pen = this.Pen;
            bool visible = graphicsd.Brush.Visible;
            graphicsd.Brush.Visible = false;
            this.BoundingPoints[0].X = base.CalcXPosValue(-0.5);
            this.BoundingPoints[0].Y = base.CalcYPosValue(this.IMax);
            this.BoundingPoints[1].X = base.CalcXPosValue(base.Count - 0.5);
            this.BoundingPoints[1].Y = base.CalcYPosValue((this.IMax + this.IMin) * 0.5);
            this.BoundingPoints[2].X = this.BoundingPoints[1].X;
            this.BoundingPoints[2].Y = base.CalcYPosValue((this.IMax - this.IMin) * 0.5);
            this.BoundingPoints[3].X = this.BoundingPoints[0].X;
            this.BoundingPoints[3].Y = base.CalcYPosValue(0.0);
            graphicsd.Polygon(base.StartZ, this.BoundingPoints);
            graphicsd.Brush.Visible = visible;
            if ((this.linesPen != null) && this.linesPen.Visible)
            {
                graphicsd.Pen = this.linesPen;
                for (int i = base.firstVisible; i < base.lastVisible; i++)
                {
                    graphicsd.VerticalLine(base.CalcXPosValue(i + 0.5), base.CalcYPosValue(this.IMax - (this.ISlope * (i + 1))), base.CalcYPosValue(this.ISlope * (i + 1)), base.StartZ);
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            aPosition.LeftTop.X = base.CalcXPosValue((double) valueIndex) - (aPosition.Width / 2);
            aPosition.LeftTop.Y = base.CalcYPosValue(this.IMax * 0.5) - (aPosition.Height / 2);
            base.DrawMark(valueIndex, st, aPosition);
        }

        public override void DrawValue(int valueIndex)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Brush = this.Brush;
            graphicsd.Brush.Color = this.DefineFunnelRegion(valueIndex);
            Array iPolyPoints = this.IPolyPoints;
            if (base.chart.Aspect.View3D)
            {
                graphicsd.Polygon(base.StartZ, Graphics3D.SliceArray(ref iPolyPoints, this.IPolyCount));
            }
            else
            {
                graphicsd.Polygon(Graphics3D.SliceArray(ref iPolyPoints, this.IPolyCount));
            }
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        protected internal override string GetMarkText(int valueIndex)
        {
            return Utils.FormatFloat(base.PercentFormat, (100.0 * this.OpportunityValues[valueIndex]) / this.QuoteValues[valueIndex]);
        }

        protected internal override Color LegendItemColor(int LegendIndex)
        {
            switch (LegendIndex)
            {
                case 0:
                    return this.aboveColor;

                case 1:
                    return this.withinColor;
            }
            return this.belowColor;
        }

        public override string LegendString(int legendIndex, LegendTextStyles legendTextStyle)
        {
            switch (legendIndex)
            {
                case 0:
                    return Texts.FunnelExceed;

                case 1:
                {
                    double num2 = this.DifferenceLimit / 100.0;
                    return (num2.ToString(base.PercentFormat) + Texts.FunnelWithin);
                }
            }
            double num3 = this.DifferenceLimit / 100.0;
            return (num3.ToString(base.PercentFormat) + Texts.FunnelBelow);
        }

        public override double MaxXValue()
        {
            return (base.Count - 0.5);
        }

        public override double MinXValue()
        {
            return -0.5;
        }

        public override double MinYValue()
        {
            return 0.0;
        }

        public void Recalc()
        {
            if (!this.ISorted)
            {
                this.QuoteValues.Sort();
                base.vxValues.FillSequence();
                this.ISorted = true;
            }
            if (base.Count > 0)
            {
                this.IMax = this.QuoteValues.First;
                this.IMin = this.QuoteValues.Last;
                this.ISlope = (0.5 * (this.IMax - this.IMin)) / ((double) base.Count);
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.linesPen != null)
            {
                this.linesPen.Chart = base.chart;
            }
            if (this.pen != null)
            {
                this.pen.Chart = base.chart;
            }
        }

        [Description("Sets color if Opportunity value is greater than Quote value."), DefaultValue(typeof(Color), "Green")]
        public Color AboveColor
        {
            get
            {
                return this.aboveColor;
            }
            set
            {
                base.SetColorProperty(ref this.aboveColor, value);
            }
        }

        [DefaultValue(true), Description("Reconstructs FunnelSeries with every added point.")]
        public bool AutoUpdate
        {
            get
            {
                return this.autoUpdate;
            }
            set
            {
                this.autoUpdate = value;
                if (this.autoUpdate)
                {
                    this.Recalc();
                }
            }
        }

        [DefaultValue(typeof(Color), "Red"), Description("Sets color if Opportunity value is > DifferenceLimit % below the Quote value.")]
        public Color BelowColor
        {
            get
            {
                return this.belowColor;
            }
            set
            {
                base.SetColorProperty(ref this.belowColor, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Defines Brush to fill Funnel Series.")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.FunnelSeries;
            }
        }

        [DefaultValue((double) 30.0), Description("Sets the difference used to define the Funnel segment color.")]
        public double DifferenceLimit
        {
            get
            {
                return this.differenceLimit;
            }
            set
            {
                this.differenceLimit = value;
                if (this.autoUpdate)
                {
                    this.Recalc();
                }
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("Defines Pen to draw FunnelSeries bounding polygon."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen LinesPen
        {
            get
            {
                if (this.linesPen == null)
                {
                    this.linesPen = new ChartPen(base.chart, Color.Black);
                }
                return this.linesPen;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ValueList OpportunityValues
        {
            get
            {
                return this.opportunityValues;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Define Pen to draw the Funnel Chart.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart, Color.Black);
                }
                return this.pen;
            }
        }

        [Description("Sorts added segments by QuoteValues in descending order."), DefaultValue(false)]
        public bool QuotesSorted
        {
            get
            {
                return this.quotesSorted;
            }
            set
            {
                this.quotesSorted = value;
                this.ISorted = this.quotesSorted;
            }
        }

        [Description("Accesses the quote values of the FunnelSeries."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ValueList QuoteValues
        {
            get
            {
                return base.mandatory;
            }
        }

        [Description("Sets color if Opportunity value is within DifferenceLimit % below the Quote value."), DefaultValue(typeof(Color), "Yellow")]
        public Color WithinColor
        {
            get
            {
                return this.withinColor;
            }
            set
            {
                base.SetColorProperty(ref this.withinColor, value);
            }
        }
    }
}

