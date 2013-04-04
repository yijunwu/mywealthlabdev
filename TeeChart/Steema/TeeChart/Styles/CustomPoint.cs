namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class CustomPoint : BaseLine
    {
        protected CustomStack iStacked;
        protected internal SeriesPointer point;

        public event ClickPointerEventHandler ClickPointer;

        public event GetPointerStyleEventHandler GetPointerStyle;

        public CustomPoint() : this(null)
        {
        }

        public CustomPoint(Chart c) : base(c)
        {
        }

        public override void AssignFormat(Series source)
        {
            base.AssignFormat(source);
            if (source is CustomPoint)
            {
                this.point = (source as CustomPoint).Pointer.Clone() as SeriesPointer;
                this.iStacked = (source as CustomPoint).Stacked;
            }
        }

        private int AxisPosition()
        {
            if (base.yMandatory)
            {
                return base.GetVertAxis.IEndPos;
            }
            return base.GetHorizAxis.IEndPos;
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            this.Pointer.CalcHorizMargins(ref LeftMargin, ref RightMargin);
        }

        private int CalcStackedPos(int valueIndex, double value)
        {
            value += this.PointOrigin(valueIndex, false);
            if (this.iStacked == CustomStack.Stack)
            {
                return Math.Min(this.AxisPosition(), base.CalcPosValue(value));
            }
            double num = this.PointOrigin(valueIndex, true);
            if (num == 0.0)
            {
                return this.AxisPosition();
            }
            return base.CalcPosValue((value * 100.0) / num);
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            this.Pointer.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
        }

        public override int CalcXPos(int valueIndex)
        {
            if ((!base.yMandatory && (this.iStacked != CustomStack.None)) && (this.iStacked != CustomStack.Overlap))
            {
                return this.CalcStackedPos(valueIndex, base.vxValues[valueIndex]);
            }
            return base.CalcXPos(valueIndex);
        }

        public override int CalcYPos(int valueIndex)
        {
            if ((base.yMandatory && (this.iStacked != CustomStack.None)) && (this.iStacked != CustomStack.Overlap))
            {
                return this.CalcStackedPos(valueIndex, base.vyValues[valueIndex]);
            }
            return base.CalcYPos(valueIndex);
        }

        protected internal override void CalcZOrder()
        {
            if (this.iStacked == CustomStack.None)
            {
                base.CalcZOrder();
            }
            else
            {
                base.iZOrder = base.chart.maxZOrder;
            }
        }

        public override int Clicked(int x, int y)
        {
            if (base.chart != null)
            {
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
            }
            int num = base.Clicked(x, y);
            if (((num == -1) && (base.firstVisible > -1)) && (base.lastVisible > -1))
            {
                for (int i = base.lastVisible; i >= base.firstVisible; i--)
                {
                    if (this.ClickedPointer(i, this.CalcXPos(i), this.CalcYPos(i), x, y))
                    {
                        this.OnClickPointer(i, x, y);
                        return i;
                    }
                }
            }
            return num;
        }

        public virtual bool ClickedPointer(int valueIndex, int tmpX, int tmpY, int x, int y)
        {
            return ((Math.Abs((int) (tmpX - x)) < this.point.HorizSize) && (Math.Abs((int) (tmpY - y)) < this.point.VertSize));
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            if (this.Pointer.Visible)
            {
                Color color = (valueIndex == -1) ? base.Color : this.ValueColor(valueIndex);
                this.point.DrawLegendShape(g, color, rect, false);
            }
            else
            {
                base.DrawLegendShape(g, valueIndex, rect);
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            base.Marks.ZPosition = base.StartZ;
            if (base.yMandatory)
            {
                base.Marks.ApplyArrowLength(ref position);
            }
            base.DrawMark(valueIndex, s, position);
        }

        public void DrawPointer(int aX, int aY, Color aColor, int valueIndex)
        {
            PointerStyles style = this.point.Style;
            if (this.point.Color != base.Color)
            {
                aColor = this.point.Color;
            }
            Color transparent = Color.Transparent;
            if (aColor.ToArgb() == Color.Transparent.ToArgb())
            {
                transparent = this.point.Pen.Color;
                this.point.Pen.Color = Color.Transparent;
            }
            this.OnGetPointerStyle(valueIndex, ref style, ref aColor);
            this.point.Draw(aX, aY, aColor, style);
            if (aColor.ToArgb() == Color.Transparent.ToArgb())
            {
                this.point.Pen.Color = transparent;
            }
        }

        public override void DrawValue(int valueIndex)
        {
            if (!base.IsNull(valueIndex) || (base.TreatNulls == TreatNullsStyle.Ignore))
            {
                this.DrawPointer(this.CalcXPos(valueIndex), this.CalcYPos(valueIndex), this.ValueColor(valueIndex), valueIndex);
            }
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            int count = base.chart.Series.Count;
            int num2 = base.chart.Series.IndexOf(this);
            int num3 = base.chart.aspect.Height3D;
            int num4 = base.chart.aspect.Width3D;
            int num5 = num3 / count;
            int num6 = num4 / count;
            int num7 = (count - (num2 + 1)) * (num3 / count);
            int num8 = (count - (num2 + 1)) * (num4 / count);
            int num9 = this.CalcXPos(index);
            int num10 = this.CalcYPos(index);
            p = PolygonStyle.Poly;
            switch (this.point.Style)
            {
                case PointerStyles.Rectangle:
                    if (base.Chart.Aspect.View3D && !base.Chart.Aspect.Orthogonal)
                    {
                        int x = base.Chart.Series[num2].CalcXPos(index);
                        int y = base.Chart.Series[num2].CalcYPos(index);
                        int num13 = x;
                        int num14 = y;
                        base.Chart.Graphics3D.Calc3DPos(ref x, ref y, base.Chart.Series[num2].StartZ);
                        base.Chart.Graphics3D.Calc3DPos(ref num13, ref num14, base.Chart.Series[num2].EndZ);
                        return new int[] { (x - this.point.HorizSize), (y - this.point.VertSize), (num13 - this.point.HorizSize), (num14 - this.point.VertSize), (num13 + this.point.HorizSize), (num14 - this.point.VertSize), (num13 + this.point.HorizSize), (num14 + this.point.VertSize), (x + this.point.HorizSize), (y + this.point.VertSize), (x - this.point.HorizSize), (y + this.point.VertSize) };
                    }
                    return new int[] { ((num9 - this.point.HorizSize) + num8), ((num10 - this.point.VertSize) - num7), (((num9 - this.point.HorizSize) + num6) + num8), (((num10 - this.point.VertSize) - num5) - num7), (((num9 + this.point.HorizSize) + num6) + num8), (((num10 - this.point.VertSize) - num5) - num7), (((num9 + this.point.HorizSize) + num6) + num8), (((num10 + this.point.VertSize) - num5) - num7), ((num9 + this.point.HorizSize) + num8), ((num10 + this.point.VertSize) - num7), ((num9 - this.point.HorizSize) + num8), ((num10 + this.point.VertSize) - num7) };

                case PointerStyles.Circle:
                {
                    int[] numArray = new int[] { num9 + num8, num10 - num7, this.point.HorizSize };
                    p = PolygonStyle.Circle;
                    return numArray;
                }
            }
            return null;
        }

        protected virtual int GetOriginPos(int valueIndex)
        {
            if ((this.iStacked != CustomStack.None) && (this.iStacked != CustomStack.Overlap))
            {
                return this.CalcStackedPos(valueIndex, 0.0);
            }
            if (base.yMandatory)
            {
                if (!base.GetVertAxis.Inverted)
                {
                    return base.GetVertAxis.IEndPos;
                }
                return base.GetVertAxis.IStartPos;
            }
            if (!base.GetHorizAxis.Inverted)
            {
                return base.GetHorizAxis.IStartPos;
            }
            return base.GetHorizAxis.IEndPos;
        }

        public override double MaxXValue()
        {
            double num = 0.0;
            if (base.yMandatory)
            {
                return base.MaxXValue();
            }
            if (this.iStacked == CustomStack.Stack100)
            {
                return 100.0;
            }
            num = base.CalcMinMaxValue(false);
            if (this.iStacked == CustomStack.Stack)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    num = Math.Max(num, this.PointOrigin(i, false) + base.XValues[i]);
                }
            }
            return num;
        }

        public override double MaxYValue()
        {
            if (!base.yMandatory)
            {
                return base.MaxYValue();
            }
            if (this.iStacked == CustomStack.Stack100)
            {
                return 100.0;
            }
            double num = base.CalcMinMaxValue(false);
            if (this.iStacked == CustomStack.Stack)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    num = Math.Max(num, this.PointOrigin(i, false) + base.vyValues[i]);
                }
            }
            return num;
        }

        public override double MinXValue()
        {
            if (base.yMandatory)
            {
                return base.MinXValue();
            }
            if (this.iStacked == CustomStack.Stack100)
            {
                return 0.0;
            }
            return base.CalcMinMaxValue(true);
        }

        public override double MinYValue()
        {
            if (base.yMandatory && (this.iStacked == CustomStack.Stack100))
            {
                return 0.0;
            }
            double num = base.yMandatory ? base.CalcMinMaxValue(true) : base.MinYValue();
            if (this.iStacked == CustomStack.Stack)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    num = Math.Min(num, this.PointOrigin(i, false) + base.vyValues[i]);
                }
            }
            return num;
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            base.LinePen.Color = Utils.DarkenColor(color, 60);
        }

        protected void OnClickPointer(int valueIndex, int x, int y)
        {
            if (this.ClickPointer != null)
            {
                this.ClickPointer(this, valueIndex, x, y);
            }
        }

        protected void OnGetPointerStyle(int valueIndex, ref PointerStyles style)
        {
            Color aColor = this.Pointer.Color;
            this.OnGetPointerStyle(valueIndex, ref style, ref aColor);
        }

        protected void OnGetPointerStyle(int valueIndex, ref PointerStyles style, ref Color aColor)
        {
            if (this.GetPointerStyle != null)
            {
                GetPointerStyleEventArgs e = new GetPointerStyleEventArgs(valueIndex, style) {
                    Color = aColor
                };
                this.GetPointerStyle(this, e);
                style = e.Style;
                aColor = e.Color;
            }
        }

        private double PointOrigin(int valueIndex, bool sumAll)
        {
            double num = 0.0;
            double num2 = base.mandatory[valueIndex];
            foreach (Series series in base.chart.Series)
            {
                if (!sumAll && (series == this))
                {
                    return num;
                }
                if ((series.Active && this.SameClassOrigin(series)) && (series.Count > valueIndex))
                {
                    double originValue = series.GetOriginValue(valueIndex);
                    if (num2 < 0.0)
                    {
                        if (originValue < 0.0)
                        {
                            num += originValue;
                        }
                    }
                    else if (originValue > 0.0)
                    {
                        num += originValue;
                    }
                }
            }
            return num;
        }

        protected virtual bool SameClassOrigin(Series s)
        {
            return base.SameClass(s);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.point != null)
            {
                this.point.Chart = base.chart;
            }
        }

        private void SetOtherStacked()
        {
            if (base.chart != null)
            {
                foreach (Series series in base.chart.Series)
                {
                    if (base.GetType().Equals(series.GetType()))
                    {
                        ((CustomPoint) series).iStacked = this.iStacked;
                    }
                }
            }
        }

        [Description("Defines all necessary properties of the Series Pointer."), DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SeriesPointer Pointer
        {
            get
            {
                if (this.point == null)
                {
                    this.point = new SeriesPointer(base.chart, this);
                }
                return this.point;
            }
        }

        [DefaultValue(0), Description("Defines how multiple series will be displayed.")]
        public CustomStack Stacked
        {
            get
            {
                return this.iStacked;
            }
            set
            {
                if (this.iStacked != value)
                {
                    this.iStacked = value;
                    this.SetOtherStacked();
                    this.Invalidate();
                }
            }
        }

        public delegate void ClickPointerEventHandler(CustomPoint series, int valueIndex, int x, int y);

        public delegate void GetPointerStyleEventHandler(CustomPoint series, GetPointerStyleEventArgs e);
    }
}

