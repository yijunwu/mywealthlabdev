namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(Steema.TeeChart.Styles.Calendar), "SeriesIcons.Calendar.bmp")]
    public class Calendar : Series
    {
        private DateTime date;
        public int DayOneColumn;
        public int DayOneRow;
        private CalendarCell days;
        private int FontH;
        private int IColumns;
        private int IFirstDay;
        private int IRows;
        private CalendarCellUpper months;
        private TextShape nextMonth;
        private ChartPen pen;
        private ContextMenu popupMenu;
        private TextShape previousMonth;
        private CalendarCell sunday;
        private int tmpFrameWidth;
        private int tmpXPosTitle;
        private CalendarCell today;
        private CalendarCell trailing;
        private CalendarCellUpper weekDays;

        public event CalendarChangeEventHandler Change;

        public Calendar() : this(null)
        {
        }

        public Calendar(Chart c) : base(c)
        {
            this.date = DateTime.MinValue;
            this.IColumns = 7;
            this.IRows = 8;
            base.UseAxis = false;
            this.IFirstDay = ((int) DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek) - 2;
            base.calcVisiblePoints = false;
            base.ShowInLegend = false;
            this.date = DateTime.Today;
            this.weekDays = new CalendarCellUpper(this);
            this.weekDays.Pen.Visible = false;
            this.weekDays.Format = "ddd";
            this.months = new CalendarCellUpper(this);
            this.months.Transparent = true;
            this.months.Format = "MMMM, yyyy";
            this.days = new CalendarCell(this);
            this.days.Transparent = true;
            this.today = new CalendarCell(this);
            this.today.Shadow.Visible = false;
            this.today.Font.Color = Color.White;
            this.today.Color = Color.Blue;
            this.sunday = new CalendarCell(this);
            this.sunday.Shadow.Visible = false;
            this.sunday.Color = Color.Red;
            this.sunday.Font.Color = Color.White;
            this.trailing = new CalendarCell(this);
            this.trailing.Transparent = true;
            this.trailing.Font.Color = Color.DarkGray;
            this.nextMonth = new TextShape();
            this.previousMonth = new TextShape();
            this.SetTextShape(this.nextMonth, ">", 30);
            this.SetTextShape(this.previousMonth, "<", 6);
            base.Marks.Brush.Color = Color.Gold;
            base.UseSeriesColor = false;
            if (base.chart != null)
            {
                base.Add();
            }
        }

        internal DateTime CellDate(int ACol, int ARow)
        {
            int num;
            DateTime date = this.Date;
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            if (ARow == this.DayOneRow)
            {
                if (ACol >= this.DayOneColumn)
                {
                    return new DateTime(year, month, (ACol - this.DayOneColumn) + 1);
                }
                if (this.Trailing.Visible)
                {
                    month--;
                    if (month == 0)
                    {
                        month = 12;
                        year--;
                    }
                    num = ((1 + DateTime.DaysInMonth(year, month)) - this.DayOneColumn) + ACol;
                    date = new DateTime(year, month, num);
                }
                return date;
            }
            if (ARow <= this.DayOneRow)
            {
                return date;
            }
            num = (((7 * (ARow - this.DayOneRow)) + ACol) - this.DayOneColumn) + 1;
            if (num > DateTime.DaysInMonth(year, month))
            {
                if (!this.Trailing.Visible)
                {
                    return date;
                }
                num -= DateTime.DaysInMonth(year, month);
                month++;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }
            }
            return new DateTime(year, month, num);
        }

        private void ChangeMonthMenu(object sender, EventArgs e)
        {
            int index = ((MenuItem) sender).Index;
            int year = this.Date.Year;
            int day = this.Date.Day;
            int month = index + 1;
            int num5 = DateTime.DaysInMonth(year, month);
            if (day > num5)
            {
                day = num5;
            }
            this.Date = new DateTime(year, month, day);
        }

        internal bool CheckClick(int x, int y)
        {
            bool flag = false;
            if (this.Months.ShapeBounds.Contains(x, y))
            {
                if (base.chart.parent.GetControl() != null)
                {
                    this.PopupMenu.Show(base.chart.parent.GetControl(), new Point(x, y));
                }
                return flag;
            }
            if (this.Clicked(x, y) == 0)
            {
                double num = (x - base.GetHorizAxis.IStartPos) / (base.GetHorizAxis.IAxisSize / this.Columns);
                int aCol = 1 + Utils.Round(num);
                num = (y - base.GetVertAxis.IStartPos) / (base.GetVertAxis.IAxisSize / this.Rows);
                int aRow = 1 + Utils.Round(num);
                this.Date = this.CellDate(aCol, aRow);
                return true;
            }
            if (this.SetNextPrevious(x, y))
            {
                flag = true;
            }
            return flag;
        }

        internal void CheckMove(int x, int y)
        {
            this.SetMouseOverTextShape(this.nextMonth, x, y);
            this.SetMouseOverTextShape(this.previousMonth, x, y);
        }

        public override int Clicked(int x, int y)
        {
            if (base.Active && this.SeriesRect().Contains(x, y))
            {
                return 0;
            }
            return -1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.nextMonth.Dispose();
                this.previousMonth.Dispose();
            }
            base.Dispose(disposing);
        }

        public override void Draw()
        {
            int num;
            string str;
            base.Draw();
            Graphics3D graphicsd = base.chart.graphics3D;
            if (this.WeekDays.Visible)
            {
                graphicsd.Font = this.WeekDays.Font;
                graphicsd.TextAlign = StringAlignment.Center;
                num = 1;
                if (this.Months.Visible)
                {
                    num++;
                }
                for (int i = 1; i <= this.IColumns; i++)
                {
                    this.DrawBack(this.WeekDays, this.RectCell(i, num));
                    str = new DateTime(0x76b, 1, (2 + i) + this.IFirstDay).ToString(this.WeekDays.Format);
                    if (this.WeekDays.UpperCase)
                    {
                        str = str.ToUpper();
                    }
                    this.DrawCell(this.WeekDays, i, num, str);
                }
            }
            if (this.Months.Visible)
            {
                graphicsd.Font = this.Months.Font;
                graphicsd.TextAlign = StringAlignment.Center;
                Rectangle rect = this.SeriesRect();
                rect.Y = this.RectCell(4, 0).Bottom;
                rect.Height = this.RectCell(4, 1).Bottom - rect.Y;
                this.DrawBack(this.Months, rect);
                str = this.Date.ToString(this.Months.Format);
                if (this.Months.UpperCase)
                {
                    str = str.ToUpper();
                }
                this.DrawCell(this.Months, 4, 1, str);
            }
            int year = this.Date.Year;
            int month = this.Date.Month;
            int day = this.Date.Day;
            int num7 = 1;
            DateTime time = new DateTime(year, month, num7);
            int c = ((int) time.DayOfWeek) - 1;
            if (c == 0)
            {
                c = 7;
            }
            c = 1 + (((c + (7 - this.IFirstDay)) - 1) % this.IColumns);
            num = 3;
            if (!this.WeekDays.visible)
            {
                num--;
            }
            if (!this.Months.visible)
            {
                num--;
            }
            this.DayOneRow = num;
            this.DayOneColumn = c;
            int num4 = DateTime.DaysInMonth(year, month);
            if (this.Trailing.Visible && (c > 1))
            {
                int num6 = month - 1;
                int num5 = year;
                if (num6 == 0)
                {
                    num6 = 12;
                    num5--;
                }
                int num3 = DateTime.DaysInMonth(num5, num6);
                for (int j = 1; j < c; j++)
                {
                    this.DrawDay(this.Trailing, j, num, (num3 - (c - j)) + 1);
                }
            }
            do
            {
                if ((num7 == day) && this.Today.Visible)
                {
                    this.DrawDay(this.Today, c, num, num7);
                }
                else
                {
                    DateTime time7 = new DateTime(year, month, num7);
                    if (time7.DayOfWeek == DayOfWeek.Sunday)
                    {
                        this.DrawDay(this.Sunday, c, num, num7);
                    }
                    else
                    {
                        this.DrawDay(this.Days, c, num, num7);
                    }
                }
                this.NextDay(ref num7, ref c, ref num);
            }
            while (num7 <= num4);
            if (this.Trailing.Visible && ((c <= this.IColumns) || (num < this.Rows)))
            {
                month++;
                num7 = 1;
                while ((c <= this.IColumns) && (num <= this.Rows))
                {
                    this.DrawDay(this.Trailing, c, num, num7);
                    this.NextDay(ref num7, ref c, ref num);
                }
            }
            if (this.nextMonth.Visible)
            {
                this.DrawTextShape(this.nextMonth);
            }
            if (this.previousMonth.Visible)
            {
                this.DrawTextShape(this.previousMonth);
            }
            if (this.Pen.Visible)
            {
                this.DrawGrid();
            }
        }

        private void DrawBack(CalendarCell ACell, Rectangle Rect)
        {
            if (ACell.Shadow.Visible)
            {
                Rect.Height -= ACell.Shadow.Height;
                Rect.Width -= ACell.Shadow.Width;
            }
            ACell.DrawRectRotated(base.chart.graphics3D, Rect, 0, base.StartZ);
            ACell.ShapeBounds = Rect;
        }

        protected void DrawCell(CalendarCell cell, int Column, int Row, string text)
        {
            int num = Utils.Round((float) base.chart.graphics3D.FontHeight);
            int x = this.XCell(Column - 0.5);
            int y = this.YCell(Row - 0.5) - (num / 2);
            if (cell.Shadow.Visible)
            {
                x -= cell.Shadow.Width / 2;
                y -= cell.Shadow.Height / 2;
            }
            base.chart.graphics3D.TextOut(x, y, text);
        }

        private void DrawDay(CalendarCell ACell, int c, int r, int d)
        {
            if (ACell.Visible)
            {
                this.DrawBack(ACell, this.RectCell(c, r));
                base.chart.graphics3D.Font = ACell.Font;
                base.chart.graphics3D.TextAlign = StringAlignment.Center;
                this.DrawCell(ACell, c, r, d.ToString());
            }
        }

        private void DrawGrid()
        {
            if (this.Pen.Visible)
            {
                int num = this.WeekDays.Visible ? 1 : 0;
                if (this.Months.Visible)
                {
                    num++;
                }
                Graphics3D graphicsd = base.chart.graphics3D;
                graphicsd.Pen = this.pen;
                for (int i = 0; i <= this.IColumns; i++)
                {
                    graphicsd.VerticalLine(this.XCell((double) i), this.YCell((double) num), base.GetVertAxis.IEndPos, base.StartZ);
                }
                for (int j = num; j <= this.Rows; j++)
                {
                    graphicsd.HorizontalLine(base.GetHorizAxis.IStartPos, base.GetHorizAxis.IEndPos, this.YCell((double) j), base.StartZ);
                }
            }
        }

        private void DrawLineTitle(TextShape Shape, int AIndex)
        {
            string text = Shape.Lines[AIndex];
            int y = Shape.ShapeBounds.Top + (Utils.Round((float) ((Shape.ShapeBounds.Bottom - Shape.ShapeBounds.Top) / 2)) - Utils.Round((float) (this.FontH / 2)));
            y += AIndex * this.FontH;
            this.tmpXPosTitle = Shape.ShapeBounds.Left;
            this.tmpXPosTitle += Utils.Round((float) ((Shape.ShapeBounds.Right - Shape.ShapeBounds.Left) / 2));
            base.Chart.Graphics3D.Font = Shape.Font;
            base.Chart.Graphics3D.TextOut(this.tmpXPosTitle, y, text);
        }

        private void DrawText(TextShape Shape)
        {
            if (Shape.Pen.Visible)
            {
                this.tmpFrameWidth = Shape.Pen.Width;
            }
            else
            {
                this.tmpFrameWidth = 1;
            }
            this.FontH = Utils.Round(base.Chart.Graphics3D.TextHeight("W"));
            for (int i = 0; i < Shape.Lines.Length; i++)
            {
                this.DrawLineTitle(Shape, i);
            }
        }

        private void DrawTextShape(TextShape Shape)
        {
            Shape.DrawRectRotated(base.Chart.Graphics3D, Shape.ShapeBounds, 0, 0);
            this.DrawText(Shape);
        }

        public void MonthClick(object sender, EventArgs e)
        {
            if (sender == this.nextMonth)
            {
                this.NextMonth();
            }
            else
            {
                this.PreviousMonth();
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (((kind == MouseEventKinds.Down) && (Utils.GetMouseButton(e) == MouseButtons.Left)) && (e.Clicks == 1))
            {
                this.CheckClick(e.X, e.Y);
            }
            else if (kind == MouseEventKinds.Move)
            {
                this.CheckMove(e.X, e.Y);
            }
        }

        private void NextDay(ref int d, ref int c, ref int r)
        {
            d++;
            c++;
            if (c > this.IColumns)
            {
                c = 1;
                r++;
            }
        }

        public void NextMonth()
        {
            this.Date = this.Date.AddMonths(1);
        }

        protected internal override int NumSampleValues()
        {
            return 1;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.Pen.Visible = false;
            this.Days.Font.Size = 6;
            this.Months.Visible = false;
            this.WeekDays.Font.Size = 5;
            this.WeekDays.Shadow.Visible = false;
            this.Sunday.Font.Size = 6;
            this.Today.Font.Size = 6;
            this.Trailing.Visible = false;
            this.NextMonthButton.Visible = false;
            this.PreviousMonthButton.Visible = false;
        }

        public void PreviousMonth()
        {
            this.Date = this.Date.AddMonths(-1);
        }

        public Rectangle RectCell(int Column, int Row)
        {
            return Utils.FromLTRB(this.XCell((double) (Column - 1)) + 1, this.YCell((double) (Row - 1)) + 1, this.XCell((double) Column), this.YCell((double) Row));
        }

        private Rectangle SeriesRect()
        {
            Rectangle empty = Rectangle.Empty;
            empty.X = base.GetHorizAxis.IStartPos;
            empty.Width = base.GetHorizAxis.IEndPos - empty.X;
            empty.Y = base.GetVertAxis.IStartPos;
            empty.Height = base.GetVertAxis.IEndPos - empty.Y;
            int num = Utils.Round((float) (base.GetVertAxis.IAxisSize / this.IRows));
            if (this.WeekDays.Visible)
            {
                empty.Y += num;
            }
            if (this.Months.Visible)
            {
                empty.Y += num;
            }
            return empty;
        }

        protected override void SetActive(bool value)
        {
            base.SetActive(value);
            this.nextMonth.Visible = base.Visible;
            this.previousMonth.Visible = base.Visible;
        }

        public bool SetCell(int x, int y)
        {
            return this.CheckClick(x, y);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.days != null)
            {
                this.days.Chart = base.Chart;
            }
            if (this.weekDays != null)
            {
                this.weekDays.Chart = base.Chart;
            }
            if (this.months != null)
            {
                this.months.Chart = base.Chart;
            }
            if (this.today != null)
            {
                this.today.Chart = base.Chart;
            }
            if (this.sunday != null)
            {
                this.sunday.Chart = base.Chart;
            }
            if (this.trailing != null)
            {
                this.trailing.Chart = base.Chart;
            }
            if (this.nextMonth != null)
            {
                this.nextMonth.Chart = base.Chart;
            }
            if (this.previousMonth != null)
            {
                this.previousMonth.Chart = base.Chart;
            }
            if (this.nextMonth != null)
            {
                this.SetTextShape(this.nextMonth, ">", 30);
            }
            if (this.previousMonth != null)
            {
                this.SetTextShape(this.previousMonth, "<", 6);
            }
        }

        private void SetMouseOverTextShape(TextShape Shape, int x, int y)
        {
            if (Shape.ShapeBounds.Contains(x, y))
            {
                Shape.Brush.Color = SystemColors.ControlDark;
            }
            else
            {
                Shape.Brush.Color = SystemColors.Control;
            }
        }

        public bool SetNextPrevious(int x, int y)
        {
            bool flag = false;
            if (this.nextMonth.ShapeBounds.Contains(x, y))
            {
                this.NextMonth();
                return true;
            }
            if (this.previousMonth.ShapeBounds.Contains(x, y))
            {
                this.PreviousMonth();
                flag = true;
            }
            return flag;
        }

        private void SetTextShape(TextShape b, string text, int ALeftPos)
        {
            b.Chart = base.chart;
            b.ShapeBounds = new Rectangle(ALeftPos, 6, 20, 0x17);
            b.Brush.Color = SystemColors.Control;
            b.Text = text;
            b.Font.Size = 8;
            b.Font.Name = "Microsoft Sans Serif";
        }

        private int XCell(double Column)
        {
            return (base.GetHorizAxis.IStartPos + Utils.Round((double) ((Column * base.GetHorizAxis.IAxisSize) / ((double) this.IColumns))));
        }

        private int YCell(double Row)
        {
            return (base.GetVertAxis.IStartPos + Utils.Round((double) ((Row * base.GetVertAxis.IAxisSize) / ((double) this.Rows))));
        }

        [Description("Returns number of Columns present in Calender Series.")]
        public int Columns
        {
            get
            {
                return this.IColumns;
            }
        }

        [Description("Defines a day and causes Calender Series to display the associated month.")]
        public DateTime Date
        {
            get
            {
                if (this.date == DateTime.MinValue)
                {
                    this.date = DateTime.Today;
                }
                return this.date;
            }
            set
            {
                if ((this.Change != null) && (value != this.date))
                {
                    CalendarChangeEventArgs e = new CalendarChangeEventArgs(value);
                    this.Change(this, e);
                    value = e.Date;
                }
                if (value != this.date)
                {
                    this.date = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Defines Calender Cell appearance for calender days.")]
        public CalendarCell Days
        {
            get
            {
                return this.days;
            }
        }

        [Description("Gets description text.")]
        public override string Description
        {
            get
            {
                return Texts.CalendarSeries;
            }
        }

        [Description("")]
        public int Month
        {
            get
            {
                return this.Date.Month;
            }
        }

        [Description("Defines Calender Cell appearance for the month name.")]
        public CalendarCellUpper Months
        {
            get
            {
                return this.months;
            }
        }

        [Description("Gets the month after the present one.")]
        public TextShape NextMonthButton
        {
            get
            {
                return this.nextMonth;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Pen Characteristics."), Category("Appearance")]
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

        [Description("")]
        public ContextMenu PopupMenu
        {
            get
            {
                if (this.popupMenu == null)
                {
                    this.popupMenu = new ContextMenu();
                    for (int i = 1; i <= 12; i++)
                    {
                        MenuItem item = new MenuItem(DateTimeFormatInfo.CurrentInfo.MonthNames[i - 1]);
                        item.Click += new EventHandler(this.ChangeMonthMenu);
                        this.popupMenu.MenuItems.Add(item);
                    }
                }
                return this.popupMenu;
            }
        }

        [Description("Gets the month before the present one.")]
        public TextShape PreviousMonthButton
        {
            get
            {
                return this.previousMonth;
            }
        }

        [Description("Returns number of Rows present in Calender Series.")]
        public int Rows
        {
            get
            {
                int iRows = this.IRows;
                if (!this.weekDays.Visible)
                {
                    iRows--;
                }
                if (!this.months.Visible)
                {
                    iRows--;
                }
                return iRows;
            }
        }

        [Description("Defines Calender Cell appearance for all Sundays.")]
        public CalendarCell Sunday
        {
            get
            {
                return this.sunday;
            }
        }

        [Description("Defines Calender Cell appearance for the highlighted day.")]
        public CalendarCell Today
        {
            get
            {
                return this.today;
            }
        }

        [Description("Defines Calender Cell appearance for the trailing days.")]
        public CalendarCell Trailing
        {
            get
            {
                return this.trailing;
            }
        }

        [Description("Defines Calender Cell appearance for weekday titles.")]
        public CalendarCellUpper WeekDays
        {
            get
            {
                return this.weekDays;
            }
        }

        public class CalendarCell : TextShape
        {
            public Steema.TeeChart.Styles.Calendar Series;

            public CalendarCell() : base(null)
            {
            }

            public CalendarCell(Steema.TeeChart.Styles.Calendar s) : base(s.chart)
            {
                this.Series = s;
            }
        }

        public sealed class CalendarCellUpper : Steema.TeeChart.Styles.Calendar.CalendarCell
        {
            private string format;
            private bool upper;

            public CalendarCellUpper()
            {
            }

            public CalendarCellUpper(Steema.TeeChart.Styles.Calendar s) : base(s)
            {
                base.Series = s;
            }

            [Description("Defines the DateValue format for the months and weekdays."), DefaultValue("")]
            public string Format
            {
                get
                {
                    return this.format;
                }
                set
                {
                    base.SetStringProperty(ref this.format, value);
                }
            }

            [DefaultValue(false), Description("Enables/disables showing months and weekdays in upper case.")]
            public bool UpperCase
            {
                get
                {
                    return this.upper;
                }
                set
                {
                    base.SetBooleanProperty(ref this.upper, value);
                }
            }
        }

        public class CalendarChangeEventArgs : EventArgs
        {
            private DateTime date;

            public CalendarChangeEventArgs(DateTime dateTime)
            {
                this.date = dateTime;
            }

            public DateTime Date
            {
                get
                {
                    return this.date;
                }
                set
                {
                    this.date = value;
                }
            }
        }

        public delegate void CalendarChangeEventHandler(Steema.TeeChart.Styles.Calendar sender, Steema.TeeChart.Styles.Calendar.CalendarChangeEventArgs e);
    }
}

