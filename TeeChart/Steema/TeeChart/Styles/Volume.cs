namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [Serializable, ToolboxBitmap(typeof(Volume), "SeriesIcons.Volume.bmp")]
    public class Volume : Custom
    {
        private Color IColor;
        private bool inflate;
        private double origin;
        private bool useYOrigin;

        public Volume() : this(null)
        {
        }

        public Volume(Chart c) : base(c)
        {
            this.inflate = true;
            this.IColor = Utils.EmptyColor;
            base.drawArea = false;
            base.DrawBetweenPoints = false;
            base.bClickableLine = false;
            base.Pointer.Visible = false;
            base.Pointer.defaultVisible = false;
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                base.Add(random.tmpX, (double) (Utils.Round((double) (random.DifY / 15.0)) * random.Random()));
                random.tmpX += random.StepX;
            }
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            if (this.inflate)
            {
                LeftMargin = 5;
                RightMargin = 5;
            }
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Dotted);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Origin);
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle r)
        {
            this.PrepareCanvas(this.ValueColor(valueIndex));
            g.HorizontalLine(r.X, r.Right, (r.Y + r.Bottom) / 2);
        }

        public override void DrawValue(int valueIndex)
        {
            int iStartPos;
            Graphics3D graphicsd = base.chart.graphics3D;
            this.PrepareCanvas(this.ValueColor(valueIndex));
            if (this.useYOrigin)
            {
                iStartPos = base.CalcYPosValue(this.origin);
            }
            else if (base.GetVertAxis.Inverted)
            {
                iStartPos = base.GetVertAxis.IStartPos;
            }
            else
            {
                iStartPos = base.GetVertAxis.IEndPos;
            }
            if (base.chart.Aspect.View3D)
            {
                graphicsd.VerticalLine(this.CalcXPos(valueIndex), this.CalcYPos(valueIndex), iStartPos, base.MiddleZ);
            }
            else
            {
                graphicsd.VerticalLine(this.CalcXPos(valueIndex), iStartPos, this.CalcYPos(valueIndex));
            }
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            base.LinePen.Color = color;
            this.IColor = color;
        }

        protected internal override int NumSampleValues()
        {
            return 40;
        }

        private void PrepareCanvas(Color AColor)
        {
            this.PrepareCanvas(base.chart.graphics3D, AColor);
        }

        private void PrepareCanvas(Graphics3D g, Color AColor)
        {
            g.Pen = base.LinePen;
            if (AColor != this.IColor)
            {
                g.Pen.Color = AColor;
                this.IColor = AColor;
            }
        }

        public override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            this.FillSampleValues(0x1a);
            base.point.InflateMargins = true;
        }

        public override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    base.LinePen.Style = DashStyle.Dot;
                    return;

                case 2:
                    base.ColorEach = true;
                    return;

                case 3:
                    this.UseOrigin = true;
                    return;
            }
            base.SetSubGallery(index);
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryVolume;
            }
        }

        [DefaultValue(true), Description("Expands axes to fit volume lines.")]
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

        [DefaultValue((double) 0.0), Description("Defines the YValue used as the origin for the specified Volume Series.")]
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

        [DefaultValue(false), Description("Enables/Disables the Y value that defines the bottom position for Volume points.")]
        public bool UseOrigin
        {
            get
            {
                return this.useYOrigin;
            }
            set
            {
                base.SetBooleanProperty(ref this.useYOrigin, value);
            }
        }
    }
}

