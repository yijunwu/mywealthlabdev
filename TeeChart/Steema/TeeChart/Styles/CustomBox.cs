namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class CustomBox : Points
    {
        private double adjacentPoint1;
        private double adjacentPoint3;
        protected double dPosition;
        private SeriesPointer extrOut;
        private double innerFence1;
        private double innerFence3;
        protected bool IVertical;
        private double median;
        private ChartPen medianPen;
        private SeriesPointer mildOut;
        private double outerFence1;
        private double outerFence3;
        private double quartile1;
        private double quartile3;
        private bool useCustomValues;
        private double whiskerLength;
        private ChartPen whiskerPen;

        public CustomBox() : this(null)
        {
        }

        public CustomBox(Chart c) : base(c)
        {
            this.whiskerLength = 1.5;
            this.IVertical = true;
            base.AllowSinglePoint = false;
            base.calcVisiblePoints = false;
            base.vxValues.Name = "";
            base.vyValues.Name = "Samples";
            base.Marks.Visible = false;
            base.Marks.defaultVisible = false;
            base.Marks.ArrowLength = 0;
            base.Marks.defaultArrowLength = 0;
            base.Pointer.Draw3D = false;
            base.Pointer.Pen.Width = 1;
            base.Pointer.VertSize = 15;
            base.Pointer.HorizSize = 15;
            base.Pointer.Brush.Color = Color.White;
            base.Pointer.Gradient.StartColor = Color.White;
            this.mildOut = new SeriesPointer(base.chart, this);
            this.mildOut.Style = PointerStyles.Circle;
            this.extrOut = new SeriesPointer(base.chart, this);
            this.extrOut.Style = PointerStyles.Diamond;
        }

        public void Add(double aPosition, double[] values)
        {
            this.Position = aPosition;
            base.Add(values);
        }

        protected override void AddSampleValues(int numValues)
        {
            int num = base.chart.Series.Count + 1;
            Random random = new Random();
            int num2 = num * (3 + random.Next(10));
            base.Add(-num2);
            for (int i = 2; i < (numValues - 1); i++)
            {
                base.Add((int) ((num2 * i) / numValues));
            }
            base.Add((int) (2 * num2));
        }

        private double CalcPctile(ValueList sortedvalues, int n, int k, double d)
        {
            if (k < 1)
            {
                return sortedvalues[0];
            }
            if (k > (n - 1))
            {
                return sortedvalues[n - 1];
            }
            return (((1.0 - d) * sortedvalues[k - 1]) + (d * sortedvalues[k]));
        }

        private int CalcPos(double value)
        {
            if (!this.IVertical)
            {
                return base.CalcXPosValue(value);
            }
            return base.CalcYPosValue(value);
        }

        protected internal override void DoBeforeDrawValues()
        {
            base.DoBeforeDrawValues();
            if (!this.useCustomValues)
            {
                this.ReconstructFromData();
            }
        }

        public override void Draw()
        {
            int num;
            int num2;
            int num3;
            int num4;
            int num6;
            int num7;
            int num8;
            int num10;
            base.Draw();
            int horizSize = base.Pointer.HorizSize;
            if (this.IVertical)
            {
                num = base.CalcXPosValue(this.dPosition) - horizSize;
                num3 = base.CalcXPosValue(this.dPosition) + horizSize;
                num2 = base.CalcYPosValue(this.quartile3);
                num4 = base.CalcYPosValue(this.quartile1);
                num7 = num4;
                num8 = num2;
            }
            else
            {
                num2 = base.CalcYPosValue(this.dPosition) - horizSize;
                num4 = base.CalcYPosValue(this.dPosition) + horizSize;
                num3 = base.CalcXPosValue(this.quartile3);
                num = base.CalcXPosValue(this.quartile1);
                num7 = num;
                num8 = num3;
            }
            if (base.GetHorizAxis.Inverted)
            {
                num10 = num;
                num = num3;
                num3 = num10;
            }
            if (base.GetVertAxis.Inverted)
            {
                num10 = num2;
                num2 = num4;
                num4 = num10;
            }
            Graphics3D g = base.chart.graphics3D;
            if (base.Pointer.Visible)
            {
                if (this.IVertical)
                {
                    num6 = (num4 - num2) / 2;
                    base.Pointer.Draw(g, base.chart.aspect.view3D, (num + horizSize) - 1, num2 + num6, base.Pointer.HorizSize - 1, num6 - 1, base.Pointer.Brush.Color, base.Pointer.Style);
                }
                else
                {
                    int num5 = (num3 - num) / 2;
                    base.Pointer.Draw(g, base.chart.aspect.view3D, num + num5, (num2 + horizSize) - 1, num5 - 1, base.Pointer.VertSize - 1, base.Pointer.Brush.Color, base.Pointer.Style);
                }
            }
            if (this.MedianPen.Visible)
            {
                g.Pen = this.medianPen;
                num6 = this.CalcPos(this.median);
                if (this.IVertical)
                {
                    if (base.chart.aspect.view3D)
                    {
                        g.HorizontalLine(num, num3, num6, base.StartZ);
                    }
                    else
                    {
                        g.HorizontalLine(num, num3, num6);
                    }
                }
                else if (base.chart.Aspect.View3D)
                {
                    g.VerticalLine(num6, num2, num4, base.StartZ);
                }
                else
                {
                    g.VerticalLine(num6, num2, num4);
                }
            }
            if (this.WhiskerPen.Visible)
            {
                int tmpZ = (base.Pointer.Visible && base.Pointer.Draw3D) ? base.MiddleZ : base.StartZ;
                g.Pen = this.whiskerPen;
                int num12 = (this.IVertical ? (num + num3) : (num2 + num4)) / 2;
                this.DrawWhisker(this.adjacentPoint1, num7, tmpZ, horizSize, num12);
                this.DrawWhisker(this.adjacentPoint3, num8, tmpZ, horizSize, num12);
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, Steema.TeeChart.Styles.SeriesMarks.Position position)
        {
            if (this.IVertical)
            {
                position.ArrowTo.X = base.CalcXPosValue(this.dPosition);
                position.ArrowFrom.X = position.ArrowTo.X;
                position.LeftTop.X = position.ArrowTo.X - (position.Width / 2);
            }
            else
            {
                position.ArrowTo.Y = base.CalcYPosValue(this.dPosition);
                position.ArrowFrom.Y = position.ArrowTo.Y;
                position.LeftTop.Y = position.ArrowTo.Y - (position.Height / 2);
            }
            base.DrawMark(valueIndex, s, position);
        }

        public override void DrawValue(int index)
        {
            SeriesPointer mildOut = null;
            double num = this.SampleValues[index];
            if ((num >= this.innerFence1) && (num <= this.innerFence3))
            {
                mildOut = null;
            }
            else if (((num >= this.innerFence3) && (num <= this.outerFence3)) || ((num <= this.innerFence1) && (num >= this.outerFence1)))
            {
                mildOut = this.mildOut;
            }
            else
            {
                mildOut = this.extrOut;
            }
            if ((mildOut != null) && mildOut.Visible)
            {
                Color colorValue = this.ValueColor(index);
                if (this.IVertical)
                {
                    mildOut.Draw(base.CalcXPosValue(this.dPosition), this.CalcYPos(index), colorValue);
                }
                else
                {
                    mildOut.Draw(this.CalcXPos(index), base.CalcYPosValue(this.dPosition), colorValue);
                }
            }
        }

        private void DrawWhisker(double adjPos, int Pos, int tmpZ, int tmp, int tmp1)
        {
            int bottom = this.CalcPos(adjPos);
            Graphics3D graphicsd = base.chart.graphics3D;
            if (base.chart.aspect.view3D)
            {
                if (this.IVertical)
                {
                    graphicsd.VerticalLine(tmp1, Pos, bottom, tmpZ);
                    graphicsd.HorizontalLine(tmp1 - tmp, tmp1 + tmp, bottom, tmpZ);
                }
                else
                {
                    graphicsd.HorizontalLine(Pos, bottom, tmp1, tmpZ);
                    graphicsd.VerticalLine(bottom, tmp1 - tmp, tmp1 + tmp, tmpZ);
                }
            }
            else if (this.IVertical)
            {
                graphicsd.VerticalLine(tmp1, Pos, bottom);
                graphicsd.HorizontalLine(tmp1 - tmp, tmp1 + tmp, bottom);
            }
            else
            {
                graphicsd.HorizontalLine(Pos, bottom, tmp1);
                graphicsd.VerticalLine(bottom, tmp1 - tmp, tmp1 + tmp);
            }
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        protected internal override object GetDataSource()
        {
            DataSet set = new DataSet();
            set.Tables.Add("TeeDataTable");
            return set;
        }

        private void InternalGallery()
        {
            this.dPosition = base.chart.Series.IndexOf(this) + 1;
            base.Pointer.HorizSize = 12;
            this.MildOut.HorizSize = 3;
            this.ExtrOut.VertSize = 3;
            this.FillSampleValues(Utils.Round((double) (10.0 * this.dPosition)));
        }

        private double Percentile(ValueList values, double p)
        {
            int count = values.Count;
            double num2 = p * (count + 1);
            double d = (num2 != 0.0) ? (num2 - ((int) num2)) : 0.0;
            int k = (num2 != 0.0) ? ((int) num2) : 0;
            return this.CalcPctile(values, count, k, d);
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            foreach (Series series in base.chart.Series)
            {
                if (series is CustomBox)
                {
                    ((CustomBox) series).InternalGallery();
                }
            }
        }

        public void ReconstructFromData()
        {
            int count = this.SampleValues.Count;
            if (count > 0)
            {
                double num1 = 1.0 / ((double) count);
                int num2 = count / 2;
                if ((count % 2) == 0)
                {
                    this.median = 0.5 * (this.SampleValues[num2 - 1] + this.SampleValues[num2]);
                }
                else
                {
                    this.median = this.SampleValues[num2];
                }
                this.quartile1 = (count > 1) ? this.Percentile(this.SampleValues, 0.25) : this.SampleValues[0];
                this.quartile3 = (count > 1) ? this.Percentile(this.SampleValues, 0.75) : this.SampleValues[0];
                double num3 = this.quartile3 - this.quartile1;
                this.innerFence1 = this.quartile1 - (this.whiskerLength * num3);
                this.innerFence3 = this.quartile3 + (this.whiskerLength * num3);
                int num4 = 0;
                while (num4 <= num2)
                {
                    if (this.SampleValues[num4] > this.innerFence1)
                    {
                        break;
                    }
                    num4++;
                }
                this.adjacentPoint1 = this.SampleValues[num4];
                num4 = num2;
                while (num4 < count)
                {
                    if (this.SampleValues[num4] > this.innerFence3)
                    {
                        break;
                    }
                    num4++;
                }
                this.adjacentPoint3 = this.SampleValues[num4 - 1];
                this.outerFence1 = this.quartile1 - ((2.0 * this.whiskerLength) * num3);
                this.outerFence3 = this.quartile3 + ((2.0 * this.whiskerLength) * num3);
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.extrOut != null)
            {
                this.extrOut.Chart = base.chart;
            }
            if (this.mildOut != null)
            {
                this.mildOut.Chart = base.chart;
            }
            if (this.medianPen != null)
            {
                this.medianPen.Chart = base.chart;
            }
            if (this.whiskerPen != null)
            {
                this.whiskerPen.Chart = base.chart;
            }
        }

        [Description(""), DefaultValue((double) 0.0)]
        public double AdjacentPoint1
        {
            get
            {
                return this.adjacentPoint1;
            }
            set
            {
                base.SetDoubleProperty(ref this.adjacentPoint1, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double AdjacentPoint3
        {
            get
            {
                return this.adjacentPoint3;
            }
            set
            {
                base.SetDoubleProperty(ref this.adjacentPoint3, value);
            }
        }

        [Browsable(false), Description("Controls the appearance of CustomBoxSeries box.")]
        public SeriesPointer Box
        {
            get
            {
                return base.Pointer;
            }
        }

        [Description("Controls the appearance of the extreme range of outer points."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SeriesPointer ExtrOut
        {
            get
            {
                return this.extrOut;
            }
        }

        [Description(""), DefaultValue((double) 0.0)]
        public double InnerFence1
        {
            get
            {
                return this.innerFence1;
            }
            set
            {
                base.SetDoubleProperty(ref this.innerFence1, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double InnerFence3
        {
            get
            {
                return this.innerFence3;
            }
            set
            {
                base.SetDoubleProperty(ref this.innerFence3, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double Median
        {
            get
            {
                return this.median;
            }
            set
            {
                base.SetDoubleProperty(ref this.median, value);
            }
        }

        [Description("Defines the Pen to draw the median line."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen MedianPen
        {
            get
            {
                if (this.medianPen == null)
                {
                    this.medianPen = new ChartPen(base.chart, Color.Black, true);
                    this.medianPen.Width = 1;
                    this.medianPen.Style = DashStyle.Dot;
                }
                return this.medianPen;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Controls the appearance of the mid range of outer points.")]
        public SeriesPointer MildOut
        {
            get
            {
                return this.mildOut;
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double OuterFence1
        {
            get
            {
                return this.outerFence1;
            }
            set
            {
                base.SetDoubleProperty(ref this.outerFence1, value);
            }
        }

        [Description(""), DefaultValue((double) 0.0)]
        public double OuterFence3
        {
            get
            {
                return this.outerFence3;
            }
            set
            {
                base.SetDoubleProperty(ref this.outerFence3, value);
            }
        }

        [DefaultValue((double) 0.0), Description("Specifies the position of box series.")]
        public double Position
        {
            get
            {
                return this.dPosition;
            }
            set
            {
                base.SetDoubleProperty(ref this.dPosition, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double Quartile1
        {
            get
            {
                return this.quartile1;
            }
            set
            {
                base.SetDoubleProperty(ref this.quartile1, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double Quartile3
        {
            get
            {
                return this.quartile3;
            }
            set
            {
                base.SetDoubleProperty(ref this.quartile3, value);
            }
        }

        [Browsable(false), Description(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ValueList SampleValues
        {
            get
            {
                return base.mandatory;
            }
        }

        [DefaultValue(false), Description("")]
        public bool UseCustomValues
        {
            get
            {
                return this.useCustomValues;
            }
            set
            {
                base.SetBooleanProperty(ref this.useCustomValues, value);
            }
        }

        [Description("Defines the whisker length as a function of the IQR."), DefaultValue((double) 1.5)]
        public double WhiskerLength
        {
            get
            {
                return this.whiskerLength;
            }
            set
            {
                base.SetDoubleProperty(ref this.whiskerLength, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the Pen to draw the whisker lines.")]
        public ChartPen WhiskerPen
        {
            get
            {
                if (this.whiskerPen == null)
                {
                    this.whiskerPen = new ChartPen(base.chart, Color.Black, true);
                }
                return this.whiskerPen;
            }
        }
    }
}

