namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Area), "SeriesIcons.Area.bmp")]
    public class Area : Custom
    {
        private double origin;
        private int stackgroup;
        private bool useOrigin;

        public Area() : this(null)
        {
        }

        public Area(Chart c) : base(c)
        {
            base.drawArea = true;
            base.AllowSinglePoint = false;
            base.Pointer.Visible = false;
            base.Pointer.defaultVisible = false;
        }

        protected internal override void CalcZOrder()
        {
            if (this.MultiArea == MultiAreas.None)
            {
                base.CalcZOrder();
            }
            else if (base.chart != null)
            {
                int zOrder = -1;
                foreach (Series series in base.chart.Series)
                {
                    if (series.Active)
                    {
                        if (series == this)
                        {
                            break;
                        }
                        if (this.SameClassOrigin(series))
                        {
                            zOrder = series.ZOrder;
                            break;
                        }
                    }
                }
                if (zOrder == -1)
                {
                    CustomStack iStacked = base.iStacked;
                    base.iStacked = CustomStack.None;
                    try
                    {
                        base.CalcZOrder();
                    }
                    finally
                    {
                        base.iStacked = iStacked;
                    }
                }
                else
                {
                    base.iZOrder = zOrder;
                }
            }
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Stairs);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.NoLines);
            AddSubChart(Texts.Stack);
            AddSubChart(Texts.Stack100);
            AddSubChart(Texts.Points);
            AddSubChart(Texts.Gradient);
        }

        protected override int GetOriginPos(int valueIndex)
        {
            if (this.useOrigin)
            {
                return base.CalcPosValue(this.origin);
            }
            return base.GetOriginPos(valueIndex);
        }

        public override double MaxXValue()
        {
            double origin = base.MaxXValue();
            if ((!base.yMandatory && this.useOrigin) && (origin < this.origin))
            {
                origin = this.origin;
            }
            return origin;
        }

        public override double MaxYValue()
        {
            double origin = base.MaxYValue();
            if ((base.yMandatory && this.useOrigin) && (origin < this.origin))
            {
                origin = this.origin;
            }
            return origin;
        }

        public override double MinXValue()
        {
            double origin = base.MinXValue();
            if ((!base.yMandatory && this.useOrigin) && (origin > this.origin))
            {
                origin = this.origin;
            }
            return origin;
        }

        public override double MinYValue()
        {
            double origin = base.MinYValue();
            if ((base.yMandatory && this.useOrigin) && (origin > this.origin))
            {
                origin = this.origin;
            }
            return origin;
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            this.AreaBrush.Color = color;
            this.AreaBrush.Gradient.StartColor = color;
            if (!base.ColorEach)
            {
                Color color2 = Utils.DarkenColor(color, 60);
                base.LinePen.Color = color2;
                this.AreaLines.Color = color2;
            }
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            backColor = base.GetAreaBrushColor(g.Brush.ForegroundColor);
            using (ChartPen pen = this.AreaLines.Clone() as ChartPen)
            {
                g.Pen = pen;
            }
            aBrush = this.AreaBrush;
        }

        protected override bool SameClassOrigin(Series s)
        {
            bool flag = base.SameClassOrigin(s);
            if (s is Area)
            {
                flag &= (s as Area).StackGroup == this.stackgroup;
            }
            return flag;
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    base.Stairs = true;
                    return;

                case 2:
                    base.Marks.Visible = true;
                    return;

                case 3:
                    base.ColorEach = true;
                    return;

                case 4:
                    this.AreaBrush.Visible = false;
                    return;

                case 5:
                    this.AreaLines.Visible = false;
                    return;

                case 6:
                    this.MultiArea = MultiAreas.Stacked;
                    return;

                case 7:
                    this.MultiArea = MultiAreas.Stacked100;
                    return;

                case 8:
                    base.Pointer.Visible = true;
                    return;

                case 9:
                    this.Gradient.Visible = true;
                    return;
            }
        }

        [Description("Determines Brush to fill the background Area region."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush AreaBrush
        {
            get
            {
                if (base.bAreaBrush == null)
                {
                    base.bAreaBrush = new ChartBrush(base.chart, false);
                }
                return base.bAreaBrush;
            }
        }

        [DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines Pen to draw AreaLines.")]
        public ChartPen AreaLines
        {
            get
            {
                if (base.pAreaLines == null)
                {
                    base.pAreaLines = new ChartPen(base.chart, Color.Black);
                }
                return base.pAreaLines;
            }
        }

        [Obsolete("Please use AreaLines property."), DefaultValue((string) null), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartPen AreaLinesPen
        {
            get
            {
                return this.AreaLines;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryArea;
            }
        }

        [Category("Appearance"), Description("Determines Gradient to fill the background Area region."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.AreaBrush.Gradient;
            }
        }

        [Description("Determines how Multi-AreaSeries are displayed."), DefaultValue(0)]
        public MultiAreas MultiArea
        {
            get
            {
                if (base.iStacked == CustomStack.Stack)
                {
                    return MultiAreas.Stacked;
                }
                if (base.iStacked == CustomStack.Stack100)
                {
                    return MultiAreas.Stacked100;
                }
                return MultiAreas.None;
            }
            set
            {
                if (value != this.MultiArea)
                {
                    if (value == MultiAreas.None)
                    {
                        base.Stacked = CustomStack.None;
                    }
                    else if (value == MultiAreas.Stacked)
                    {
                        base.Stacked = CustomStack.Stack;
                    }
                    else if (value == MultiAreas.Stacked100)
                    {
                        base.Stacked = CustomStack.Stack100;
                    }
                }
            }
        }

        [DefaultValue((double) 0.0), Description("Sets axis value as a common bottom for all AreaSeries points.")]
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

        [DefaultValue(0), Description("Area series stack group.")]
        public int StackGroup
        {
            get
            {
                return this.stackgroup;
            }
            set
            {
                base.SetIntegerProperty(ref this.stackgroup, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines to fill the top 3D Area region."), Category("Appearance")]
        public Steema.TeeChart.Drawing.Gradient TopGradient
        {
            get
            {
                return base.bBrush.Gradient;
            }
        }

        [DefaultValue(false), Description("Aligns bottom of AreaSeries to the Origin property value.")]
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

