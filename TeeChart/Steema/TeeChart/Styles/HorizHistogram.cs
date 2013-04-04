namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(HorizHistogram), "SeriesIcons.HorizHistogram.bmp")]
    public class HorizHistogram : Histogram
    {
        public HorizHistogram() : this(null)
        {
        }

        public HorizHistogram(Chart c) : base(c)
        {
            base.SetHorizontal();
            base.XValues.Order = ValueListOrder.None;
            base.YValues.Order = ValueListOrder.Ascending;
        }

        protected internal override RectangleF CalcRectangle(int valueIndex)
        {
            float num;
            RectangleF ef = new RectangleF();
            if (base.VisiblePoints() <= 0)
            {
                return new Rectangle(0, 0, 0, 0);
            }
            if (base.VisiblePoints() > 1)
            {
                if (valueIndex == base.FirstDisplayedIndex())
                {
                    num = ((float) (this.CalcYPos(valueIndex) - this.CalcYPos(valueIndex + 1))) / 2f;
                }
                else
                {
                    num = ((float) (this.CalcYPos(valueIndex - 1) - this.CalcYPos(valueIndex))) / 2f;
                }
            }
            else
            {
                num = base.GetVertAxis.IAxisSize / base.VisiblePoints();
            }
            if (valueIndex == base.FirstDisplayedIndex())
            {
                float a = this.CalcYPos(valueIndex) - num;
                float b = 2f * num;
                if (!this.DrawValuesForward())
                {
                    Utils.SwapFloat(ref a, ref b);
                }
                ef.Y = a;
                ef.Height = b;
            }
            else
            {
                if (this.DrawValuesForward())
                {
                    ef.Y = this.CalcYPos(valueIndex) - num;
                }
                else
                {
                    ef.Y = this.CalcYPos(valueIndex) + num;
                }
                ef.Height = base.previous - ef.Y;
            }
            base.previous = ef.Top;
            ef.X = base.GetHorizAxis.Inverted ? ((float) base.GetHorizAxis.IEndPos) : ((float) base.GetHorizAxis.IStartPos);
            ef.Width = this.CalcXPos(valueIndex) - ef.X;
            return ef;
        }

        protected internal override void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            int num = aPosition.Height / 2;
            int num2 = base.Marks.Callout.Length + base.Marks.Callout.Distance;
            aPosition.LeftTop.Y = aPosition.ArrowTo.Y - num;
            aPosition.LeftTop.X += num2 + (aPosition.Width / 2);
            aPosition.ArrowTo.X += num2;
            aPosition.ArrowFrom.Y = aPosition.ArrowTo.Y;
            aPosition.ArrowFrom.X += base.Marks.Callout.Distance;
            base.DrawMark(valueIndex, st, aPosition);
        }

        protected override void ModifySeriesColors(Color color)
        {
            base.ModifySeriesColors(color);
            base.bBrush.Gradient.Direction = LinearGradientMode.Vertical;
        }

        public override string Description
        {
            get
            {
                return Texts.HorizHistogramSeries;
            }
        }
    }
}

