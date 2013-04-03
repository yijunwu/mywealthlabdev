namespace WealthLab.ChartControl
{
    using System;
    using System.Drawing;
    using WealthLab;

    public class ChartDrawingObjectHandle
    {
        private bool bool_0;
        private bool bool_1 = true;
        private bool bool_2 = true;
        private static Brush brush_0 = new SolidBrush(Color.Gainsboro);
        private static Brush brush_1 = new SolidBrush(Color.Red);
        private ChartDrawingObject chartDrawingObject_0;
        private ChartDrawingObjectHandleType chartDrawingObjectHandleType_0;
        private DateTime dateTime_0;
        private double double_0;
        private int int_0;
        private int int_1;
        private static Pen pen_0 = new Pen(Color.Black);

        public ChartDrawingObjectHandle(ChartDrawingObject owner)
        {
            this.chartDrawingObject_0 = owner;
        }

        internal void method_0(int int_2, ref double double_1)
        {
            if (this.Pane.IsPricePane)
            {
                if (double_1 >= this.Bars.High[int_2])
                {
                    double_1 = this.Bars.High[int_2];
                }
                else if (double_1 <= this.Bars.Low[int_2])
                {
                    double_1 = this.Bars.Low[int_2];
                }
                else
                {
                    double num3 = Math.Abs((double) (double_1 - this.Bars.Close[int_2]));
                    double num4 = Math.Abs((double) (double_1 - this.Bars.Open[int_2]));
                    if (num3 < num4)
                    {
                        double_1 = this.Bars.Close[int_2];
                    }
                    else
                    {
                        double_1 = this.Bars.Open[int_2];
                    }
                }
            }
            else
            {
                double maxValue = double.MaxValue;
                DataSeries series = null;
                foreach (PlottedIndicator indicator in this.Pane.PlottedIndicators)
                {
                    double num2 = Math.Abs((double) (double_1 - indicator.Series[int_2]));
                    if (num2 < maxValue)
                    {
                        maxValue = num2;
                        series = indicator.Series;
                    }
                    if (series != null)
                    {
                        double_1 = series[int_2];
                    }
                }
            }
        }

        internal void method_1(Graphics graphics_0)
        {
            if ((this.Bar != -1) && graphics_0.ClipBounds.IntersectsWith(this.Bounds))
            {
                if (this.HandleType == ChartDrawingObjectHandleType.Mover)
                {
                    graphics_0.FillRectangle(brush_1, this.Bounds);
                    if (this.Owner.ShowToolTip)
                    {
                        string toolTipText = this.Owner.ToolTipText;
                        SizeF ef4 = graphics_0.MeasureString(toolTipText, this.Owner.HandleFont);
                        graphics_0.DrawRectangle(new Pen(Color.Black), (float) (this.X - (ef4.Width / 2f)), (float) ((this.Y - (ef4.Height / 2f)) + 10f), (float) (ef4.Width + 5f), (float) (ef4.Height + 3f));
                        RectangleF rect = new RectangleF((this.X - (ef4.Width / 2f)) + 1f, (this.Y - (ef4.Height / 2f)) + 11f, ef4.Width + 3f, ef4.Height + 1f);
                        graphics_0.FillRectangle(new SolidBrush(SystemColors.Info), rect);
                        graphics_0.DrawString(toolTipText, this.Owner.HandleFont, brush_1, rect);
                    }
                }
                else
                {
                    graphics_0.FillRectangle(brush_0, this.Bounds);
                }
                graphics_0.DrawRectangle(pen_0, this.Bounds);
                if (this.bool_2)
                {
                    string text = this.Pane.FormatChartValue(this.Value);
                    SizeF ef2 = graphics_0.MeasureString(text, this.Owner.HandleFont);
                    RectangleF ef3 = new RectangleF((this.X - (ef2.Width / 2f)) - 1f, (this.Y - (ef2.Height / 2f)) + 12f, ef2.Width + 2f, ef2.Height - 3f);
                    graphics_0.FillRectangle(brush_0, ef3);
                    graphics_0.DrawString(text, this.Owner.HandleFont, Brushes.Black, (float) (this.X - (ef2.Width / 2f)), (float) ((this.Y - (ef2.Height / 2f)) + 11f));
                }
            }
        }

        internal void method_2()
        {
            this.int_0 = this.X;
            this.int_1 = this.Y;
        }

        internal Point method_3()
        {
            return new Point(this.X - this.int_0, this.Y - this.int_1);
        }

        internal bool method_4(Point point_0)
        {
            return (this.Owner.ConvertXToBar(this.int_0 + point_0.X) >= 0);
        }

        internal void method_5(Point point_0)
        {
            this.X = this.int_0 + point_0.X;
            this.Y = this.int_1 + point_0.Y;
        }

        public int Bar
        {
            get
            {
                return this.Bars.ConvertDateToBar(this.Date, false);
            }
            set
            {
                int num = this.Bars.Date.Count - 1;
                if ((value > -1) && (value <= num))
                {
                    this.Date = this.Bars.Date[value];
                }
                else
                {
                    this.Date = this.Bars.Date[((num - 1) >= 0) ? (num - 1) : num];
                }
            }
        }

        public WealthLab.Bars Bars
        {
            get
            {
                return this.Owner.Bars;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(this.X - 4, this.Y - 4, 9, 9);
            }
        }

        public DateTime Date
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
            }
        }

        public ChartDrawingObjectHandleType HandleType
        {
            get
            {
                return this.chartDrawingObjectHandleType_0;
            }
            set
            {
                if (value == ChartDrawingObjectHandleType.Mover)
                {
                    this.bool_2 = false;
                }
                this.chartDrawingObjectHandleType_0 = value;
            }
        }

        public ChartDrawingObject Owner
        {
            get
            {
                return this.chartDrawingObject_0;
            }
            set
            {
                this.chartDrawingObject_0 = value;
            }
        }

        public ChartPane Pane
        {
            get
            {
                return this.Owner.Pane;
            }
        }

        public bool ShowPriceValue
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public bool SnapToValue
        {
            get
            {
                if (this.chartDrawingObjectHandleType_0 == ChartDrawingObjectHandleType.Mover)
                {
                    return false;
                }
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public double Value
        {
            get
            {
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public int X
        {
            get
            {
                return this.Owner.ConvertBarToX(this.Bar);
            }
            set
            {
                this.Bar = this.Owner.ConvertXToBar(value);
            }
        }

        public int Y
        {
            get
            {
                return this.Pane.ConvertValueToY(this.Value);
            }
            set
            {
                this.Value = this.Pane.ConvertYToValue(value);
            }
        }
    }
}

