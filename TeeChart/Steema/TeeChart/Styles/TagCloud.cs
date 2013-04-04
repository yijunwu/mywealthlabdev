namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class TagCloud : Custom3DPalette
    {
        private string filter;
        private ChartFont font;
        private Steema.TeeChart.Drawing.Gradient gradient;
        public Rectangle[] Positions;
        private Color TagEndColor;
        private int tagSeparation;
        private Color TagStartColor;
        private ChartFont tmpFont;

        public event TagCloudDrawTagEventHandler DrawTag;

        public TagCloud() : this(null)
        {
        }

        public TagCloud(Chart c) : base(c)
        {
            this.TagStartColor = Graphics3D.ColorPalette[0];
            this.TagEndColor = Graphics3D.ColorPalette[1];
            this.tagSeparation = 5;
            this.filter = "";
            base.Brush.Visible = false;
            base.Brush.defaultVisible = false;
            base.Pen.Visible = false;
            base.Pen.defaultVisible = false;
            base.UseAxis = false;
            base.ShowInLegend = false;
            base.StartColor = this.TagStartColor;
            base.EndColor = this.TagEndColor;
        }

        protected override void AddSampleValues(int numValues)
        {
            Random random = new Random();
            Series.SeriesRandom random2 = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                Type type = Utils.SeriesTypesOf[random.Next(Utils.SeriesTypesCount)];
                if (!base.Labels.Contains(type.Name))
                {
                    this.AddTag(type.Name, random2.tmpY + (random2.DifY * random.Next(0x3e8)));
                }
            }
            this.Sort(TagCloudOrder.Text);
        }

        public int AddTag(string Text, double Value)
        {
            return base.Add((double) 0.0, Value, (double) 0.0, Text);
        }

        public override int CalcXPos(int index)
        {
            if (this.Positions.Length <= index)
            {
                return 0;
            }
            return Utils.Round((float) (this.Positions[index].Width / 2));
        }

        public override int CalcYPos(int index)
        {
            if (this.Positions.Length <= index)
            {
                return 0;
            }
            return Utils.Round((float) (this.Positions[index].Height / 2));
        }

        public override int Clicked(int x, int y)
        {
            for (int i = 0; i < this.Positions.Length; i++)
            {
                if (this.Positions[i].Contains(x, y))
                {
                    return i;
                }
            }
            return -1;
        }

        public override void Draw()
        {
            int num3;
            base.Draw();
            Graphics3D g = base.Chart.Graphics3D;
            Rectangle chartRect = base.Chart.ChartRect;
            g.Brush = base.Brush;
            g.Pen = base.Pen;
            this.tmpFont = this.Font.Clone() as ChartFont;
            g.Font = this.tmpFont;
            if (base.Chart.Aspect.ClipPoints)
            {
                g.ClipRectangle(chartRect);
            }
            if (this.Gradient.Visible)
            {
                this.Gradient.Draw(g, chartRect);
            }
            if (base.Pen.Visible || base.Brush.Visible)
            {
                g.Rectangle(chartRect);
            }
            this.Positions = (Rectangle[]) Utils.SetLength(this.Positions, base.Count, typeof(Rectangle));
            int top = chartRect.Top;
            int num10 = 0;
            int valueIndex = 0;
        Label_00E0:
            num3 = 0;
            int num4 = 0;
            while (valueIndex < base.Count)
            {
                if (this.ShouldDraw(valueIndex))
                {
                    if (this.DrawTag != null)
                    {
                        g.Font = this.tmpFont;
                    }
                    this.tmpFont.Size = this.GetTagFontSize(valueIndex);
                    TagCloudDrawTagEventArgs e = new TagCloudDrawTagEventArgs(valueIndex);
                    this.OnDrawTag(e);
                    int num2 = Utils.Round(g.TextWidth(base.Labels[valueIndex]));
                    if (num4 > 0)
                    {
                        num2 += this.TagSpacing();
                    }
                    if ((num4 + num2) >= chartRect.Width)
                    {
                        if (num4 == 0)
                        {
                            num3 = Math.Max(num3, Utils.Round(g.TextHeight(base.Labels[valueIndex])));
                            valueIndex++;
                        }
                        break;
                    }
                    num4 += num2;
                    num3 = Math.Max(num3, Utils.Round(g.TextHeight(base.Labels[valueIndex])));
                    valueIndex++;
                }
                else
                {
                    valueIndex++;
                }
            }
            int left = chartRect.Left;
            if ((top + num3) < chartRect.Bottom)
            {
                for (int i = num10; i < valueIndex; i++)
                {
                    if (this.ShouldDraw(i))
                    {
                        if (this.DrawTag != null)
                        {
                            g.Font = this.tmpFont;
                        }
                        this.tmpFont.Size = this.GetTagFontSize(i);
                        this.tmpFont.Color = this.ValueColor(i);
                        TagCloudDrawTagEventArgs args2 = new TagCloudDrawTagEventArgs(i);
                        this.OnDrawTag(args2);
                        string text = base.Labels[i];
                        int num6 = left;
                        int num7 = top;
                        int right = num6 + Utils.Round(g.TextWidth(text));
                        num7 += Utils.Round((float) (num3 - g.TextHeight(text)));
                        int bottom = num7 + num3;
                        this.Positions[i] = Utils.FromLTRB(num6, num7, right, bottom);
                        if (base.Chart.Aspect.View3D)
                        {
                            g.TextOut(this.Positions[i].Left, this.Positions[i].Top, 0, text);
                        }
                        else
                        {
                            g.TextOut(this.Positions[i].Left, this.Positions[i].Top, text);
                        }
                        left += this.TagSpacing() + this.Positions[i].Width;
                    }
                }
                num10 = valueIndex;
                top += num3;
                if (num10 < base.Count)
                {
                    goto Label_00E0;
                }
            }
            base.firstVisible = 0;
            base.lastVisible = valueIndex;
            if (base.Chart.Aspect.ClipPoints)
            {
                g.UnClip();
            }
        }

        public override void GalleryChanged3D(bool Is3D)
        {
            base.GalleryChanged3D(Is3D);
            if (base.chart != null)
            {
                base.chart.Aspect.View3D = false;
            }
        }

        public int GetTagFontSize(int ValueIndex)
        {
            int size = this.Font.Size;
            double num2 = base.mandatory.Maximum - base.mandatory[ValueIndex];
            size -= Utils.Round((double) (((0.75 * num2) * this.Font.Size) / base.mandatory.Range));
            if (size < 1)
            {
                size = 1;
            }
            return size;
        }

        protected virtual void OnDrawTag(TagCloudDrawTagEventArgs e)
        {
            if (this.DrawTag != null)
            {
                this.DrawTag(this, e);
            }
        }

        public override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            this.Font.Size = 0x10;
            base.Chart.Header.Visible = true;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (base.Chart != null)
            {
                this.font = new ChartFont(base.Chart);
                this.font.Size = 0x20;
                this.gradient = new Steema.TeeChart.Drawing.Gradient(base.Chart);
            }
        }

        private bool ShouldDraw(int ValueIndex)
        {
            if (!Utils.IsNullOrEmpty(this.filter))
            {
                return (base.Labels[ValueIndex].ToUpper().IndexOf(this.filter.ToUpper()) > -1);
            }
            return true;
        }

        public void Sort(TagCloudOrder SortBy)
        {
            this.Sort(SortBy, ValueListOrder.Ascending);
        }

        public void Sort(TagCloudOrder SortBy, ValueListOrder Order)
        {
            switch (SortBy)
            {
                case TagCloudOrder.Text:
                    base.SortByLabels(Order);
                    break;

                case TagCloudOrder.Value:
                    base.YValues.Order = Order;
                    base.YValues.Sort();
                    break;
            }
            this.Invalidate();
        }

        private int TagSpacing()
        {
            return Utils.Round((double) (((this.tagSeparation * 5) * base.Chart.Graphics3D.TextWidth("W")) * 0.01));
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryTagCloud;
            }
        }

        [DefaultValue("")]
        public string Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;
                this.Invalidate();
            }
        }

        public ChartFont Font
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.gradient;
            }
            set
            {
                this.gradient = value;
            }
        }

        [DefaultValue(5)]
        public int TagSeparation
        {
            get
            {
                return this.tagSeparation;
            }
            set
            {
                this.tagSeparation = value;
                this.Invalidate();
            }
        }
    }
}

