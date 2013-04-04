namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Calendar), "SeriesIcons.Renko.bmp")]
    public class Renko : Custom
    {
        private double boxsize;
        private Color downswingcolor;
        private double imax;
        private double imin;
        private Color upswingcolor;

        public Renko() : this(null)
        {
        }

        public Renko(Chart c) : base(c)
        {
            this.boxsize = 1.0;
            this.upswingcolor = Color.White;
            this.downswingcolor = Color.Black;
            base.vxValues.DateTime = false;
            base.AllowSinglePoint = false;
            base.Marks.Visible = false;
            base.Pointer.Visible = false;
            base.Pointer.defaultVisible = false;
            base.Brush.Visible = true;
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                base.Add(random.tmpX, (double) ((random.Random() + 1.0) * (numValues + 2)));
                random.tmpX += random.StepX;
            }
        }

        private int CalcBricks(bool draw)
        {
            int startindex = 0;
            this.imin = this.CloseValues.minimum;
            this.imax = this.CloseValues.maximum;
            if (base.Count > 1)
            {
                int num2 = 1;
                while ((this.NumBricks(this.CloseValues[num2], this.CloseValues[0]) == 0) && (num2 < base.Count))
                {
                    num2++;
                }
                if (num2 >= base.Count)
                {
                    return startindex;
                }
                this.imin = this.imax = this.CloseValues[0];
                bool uptrend = (this.CloseValues[num2] - this.CloseValues[0]) > 0.0;
                bool flag2 = false;
                double cur = this.CloseValues[0];
                int numbricks = this.NumBricks(cur, this.CloseValues[num2]);
                if (draw)
                {
                    this.DrawBricks(cur, startindex, numbricks, uptrend);
                }
                cur = uptrend ? (cur + (numbricks * this.boxsize)) : (cur - (numbricks * this.boxsize));
                this.CompareMinMax(cur);
                startindex += numbricks;
                for (int i = num2 + 1; i < base.Count; i++)
                {
                    flag2 = (this.CloseValues.Value[i] > cur) != uptrend;
                    uptrend = this.CloseValues.Value[i] > cur;
                    if (flag2)
                    {
                        cur = uptrend ? (cur += this.boxsize) : (cur -= this.boxsize);
                    }
                    numbricks = this.NumBricks(cur, this.CloseValues[i]);
                    if (numbricks > 0)
                    {
                        if (draw)
                        {
                            this.DrawBricks(cur, startindex, numbricks, uptrend);
                        }
                        cur = uptrend ? (cur + (numbricks * this.boxsize)) : (cur - (numbricks * this.boxsize));
                        this.CompareMinMax(cur);
                        startindex += numbricks;
                    }
                }
            }
            return startindex;
        }

        internal override void CalcFirstLastVisibleIndex()
        {
            base.CalcFirstLastVisibleIndex();
            base.firstVisible = 0;
            base.lastVisible = this.CalcBricks(false) - 1;
        }

        private void CompareMinMax(double val)
        {
            this.imin = Math.Min(this.imin, val);
            this.imax = Math.Max(this.imax, val);
        }

        protected internal override int CountLegendItems()
        {
            return 2;
        }

        public override void Draw()
        {
            this.CalcBricks(true);
        }

        private void DrawBrick(double low, double high, int index)
        {
            Graphics3D graphicsd = base.Chart.Graphics3D;
            int left = base.CalcXPosValue(index - 0.5);
            int right = base.CalcXPosValue(index + 0.5);
            int top = base.CalcYPosValue(high);
            int bottom = base.CalcYPosValue(low);
            if (graphicsd.aspect.view3D && base.Pointer.Draw3D)
            {
                graphicsd.Cube(left, top, right, bottom, base.StartZ, base.endZ, false);
            }
            else
            {
                graphicsd.Rectangle(left, top, right, bottom, base.MiddleZ);
            }
        }

        private void DrawBricks(double start, int startindex, int numbricks, bool uptrend)
        {
            Graphics3D graphicsd = base.Chart.Graphics3D;
            graphicsd.Brush = base.Brush;
            graphicsd.Brush.Color = uptrend ? this.upswingcolor : this.downswingcolor;
            if (base.Transparency > 0)
            {
                graphicsd.Brush.Transparency = base.Transparency;
            }
            graphicsd.Pen = base.LinePen;
            graphicsd.Pen.Color = base.LinePen.Color;
            for (int i = 0; i < numbricks; i++)
            {
                if (uptrend)
                {
                    this.DrawBrick(start + (i * this.boxsize), start + ((i + 1) * this.boxsize), i + startindex);
                }
                else
                {
                    this.DrawBrick(start - ((i + 1) * this.boxsize), start - (i * this.boxsize), i + startindex);
                }
            }
        }

        protected internal override Color LegendItemColor(int index)
        {
            if (index != 0)
            {
                return this.downswingcolor;
            }
            return this.upswingcolor;
        }

        public override string LegendString(int legendIndex, LegendTextStyles legendTextStyle)
        {
            if (legendIndex != 0)
            {
                return Texts.Down;
            }
            return Texts.Up;
        }

        public override double MaxXValue()
        {
            return (this.CalcBricks(false) - 0.5);
        }

        public override double MaxYValue()
        {
            this.CalcBricks(false);
            return this.imax;
        }

        public override double MinXValue()
        {
            return -0.5;
        }

        public override double MinYValue()
        {
            this.CalcBricks(false);
            return this.imin;
        }

        private int NumBricks(double cur, double pre)
        {
            return (int) (Math.Abs((double) (cur - pre)) / this.boxsize);
        }

        protected internal override int NumSampleValues()
        {
            return 8;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.FillSampleValues(5);
        }

        [Description("Renko chart box size."), DefaultValue((double) 1.0)]
        public double BoxSize
        {
            get
            {
                return this.boxsize;
            }
            set
            {
                base.SetDoubleProperty(ref this.boxsize, value);
            }
        }

        [Description("Gets and sets all Stock market closing values.")]
        public ValueList CloseValues
        {
            get
            {
                return base.vyValues;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryRenko;
            }
        }

        [Category("Appearance"), Description("Color for downward trend (current close lower than previous close) bricks."), DefaultValue(typeof(Color), "Black")]
        public Color DownSwingColor
        {
            get
            {
                return this.downswingcolor;
            }
            set
            {
                base.SetColorProperty(ref this.downswingcolor, value);
            }
        }

        [Category("Appearance"), Description("Color for upward trend (current close higher than previous close) bricks."), DefaultValue(typeof(Color), "White")]
        public Color UpSwingColor
        {
            get
            {
                return this.upswingcolor;
            }
            set
            {
                base.SetColorProperty(ref this.upswingcolor, value);
            }
        }
    }
}

