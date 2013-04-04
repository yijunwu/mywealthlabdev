namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;

    [ToolboxBitmap(typeof(Gantt), "SeriesIcons.Gantt.bmp")]
    public class Gantt : Points
    {
        private ChartPen connectingPen;
        private ValueList endValues;
        private ValueList nextTask;

        public Gantt() : this(null)
        {
        }

        public Gantt(Chart c) : base(c)
        {
            base.bColorEach = true;
            base.SetHorizontal();
            base.calcVisiblePoints = false;
            base.vxValues.Name = Texts.ValuesGanttStart;
            base.vxValues.DateTime = true;
            base.vxValues.Order = ValueListOrder.None;
            this.endValues = new ValueList(this, Texts.ValuesGanttEnd);
            this.endValues.DateTime = true;
            this.nextTask = new ValueList(this, Texts.ValuesGanttNextTask);
            base.Pointer.Style = PointerStyles.Rectangle;
        }

        public override void Add(DataView view)
        {
            int index = -1;
            int num2 = -1;
            double tempValue = -1.0;
            Color emptyColor = Utils.EmptyColor;
            string text = "";
            int[] numArray = new int[base.ValuesLists.Count];
            int num4 = 0;
            foreach (ValueList list in base.ValuesLists)
            {
                if (list.DataMember.Length != 0)
                {
                    numArray[base.ValuesLists.IndexOf(list)] = view.Table.Columns.IndexOf(list.DataMember);
                    num4++;
                }
            }
            if (base.labelMember.Length != 0)
            {
                index = view.Table.Columns.IndexOf(base.labelMember);
            }
            if (base.colorMember.Length != 0)
            {
                num2 = view.Table.Columns.IndexOf(base.colorMember);
            }
            if (num4 == base.ValuesLists.Count)
            {
                foreach (DataRowView view2 in view)
                {
                    DataRow row = view2.Row;
                    if (num2 != -1)
                    {
                        emptyColor = (Color) row[num2];
                    }
                    if (index != -1)
                    {
                        text = Convert.ToString(row[index]);
                    }
                    foreach (ValueList list2 in base.ValuesLists)
                    {
                        int num5 = numArray[base.ValuesLists.IndexOf(list2)];
                        if (num5 > -1)
                        {
                            if (row[num5] is DateTime)
                            {
                                list2.TempValue = Utils.DateTime((DateTime) row[num5]);
                            }
                            else
                            {
                                list2.TempValue = Convert.ToDouble(row[num5]);
                            }
                        }
                    }
                    tempValue = this.NextTasks.TempValue;
                    int num6 = this.Add(base.XValues.TempValue, this.endValues.TempValue, base.YValues.TempValue, text, emptyColor);
                    if (tempValue > -1.0)
                    {
                        this.NextTasks[num6] = tempValue;
                    }
                }
            }
        }

        public int Add(double start, double endDate, double y)
        {
            return this.Add(start, endDate, y, "", Utils.EmptyColor);
        }

        public int Add(DateTime start, DateTime endDate, double y, Color color)
        {
            return this.Add(start, endDate, y, "", color);
        }

        public int Add(DateTime start, DateTime endDate, double y, string text)
        {
            return this.Add(start, endDate, y, text, Utils.EmptyColor);
        }

        public int Add(double start, double endDate, double Y, Color color)
        {
            return this.Add(start, endDate, Y, "", color);
        }

        public int Add(double start, double endDate, double y, string text)
        {
            return this.Add(start, endDate, y, text, Utils.EmptyColor);
        }

        public int Add(DateTime start, DateTime endDate, double y, string text, Color color)
        {
            return this.Add(Utils.DateTime(start), Utils.DateTime(endDate), y, text, color);
        }

        public int Add(double start, double endDate, double y, string text, Color color)
        {
            this.endValues.TempValue = endDate;
            this.nextTask.TempValue = -1.0;
            return base.Add(start, y, text, color);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 0; i <= Math.Min(numValues, 10 + Utils.Round((double) (20.0 * random.Random()))); i++)
            {
                double start = (Utils.DateTime(DateTime.Today) + (i * 3)) + (5.0 * random.Random());
                double endDate = (start + 9.0) + (16.0 * random.Random());
                int index = i % 10;
                int num5 = this.Add(start, endDate, (double) index, this.GanttSampleStr(index));
                for (int j = 0; j < num5; j++)
                {
                    if ((this.nextTask[j] == -1.0) && (start > this.endValues[j]))
                    {
                        this.nextTask[j] = num5;
                        break;
                    }
                }
            }
        }

        public override bool ClickedPointer(int valueIndex, int tmpX, int tmpY, int x, int y)
        {
            return (((x >= tmpX) && (x <= base.CalcXPosValue(this.EndValues[valueIndex]))) && (Math.Abs((int) (tmpY - y)) < base.Pointer.VertSize));
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position aPosition)
        {
            aPosition.LeftTop.X += (base.CalcXPosValue(this.endValues[valueIndex]) - aPosition.ArrowFrom.X) / 2;
            aPosition.LeftTop.Y += aPosition.Height / 2;
            base.DrawMark(valueIndex, s, aPosition);
        }

        public override void DrawValue(int valueIndex)
        {
            if (base.point.Visible)
            {
                Color aColor = this.ValueColor(valueIndex);
                int num = this.CalcXPos(valueIndex);
                int num2 = base.CalcXPosValue(this.endValues[valueIndex]);
                int tmpHoriz = (num2 - num) / 2;
                int py = this.CalcYPos(valueIndex);
                PointerStyles style = base.point.Style;
                base.OnGetPointerStyle(valueIndex, ref style, ref aColor);
                Graphics3D g = base.chart.graphics3D;
                base.point.Draw(g, base.chart.Aspect.View3D, num + tmpHoriz, py, tmpHoriz, base.point.VertSize, aColor, style);
                if (this.ConnectingPen.Visible)
                {
                    int index = Utils.Round(this.nextTask[valueIndex]);
                    if ((index >= 0) && (index < base.Count))
                    {
                        g.Pen = this.ConnectingPen;
                        int x = this.CalcXPos(index);
                        int num7 = num2 + ((x - num2) / 2);
                        int y = this.CalcYPos(index);
                        g.Line(num2, py, num7, py, base.MiddleZ);
                        g.LineTo(num7, y, base.MiddleZ);
                        g.LineTo(x, y, base.MiddleZ);
                    }
                }
            }
        }

        private string GanttSampleStr(int Index)
        {
            switch (Index)
            {
                case 0:
                    return Texts.GanttSample1;

                case 1:
                    return Texts.GanttSample2;

                case 2:
                    return Texts.GanttSample3;

                case 3:
                    return Texts.GanttSample4;

                case 4:
                    return Texts.GanttSample5;

                case 5:
                    return Texts.GanttSample6;

                case 6:
                    return Texts.GanttSample7;

                case 7:
                    return Texts.GanttSample8;

                case 8:
                    return Texts.GanttSample9;
            }
            return Texts.GanttSample10;
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        public override bool IsValidSourceOf(Series value)
        {
            return (value is Gantt);
        }

        public override double MaxXValue()
        {
            return Math.Max(base.MaxXValue(), this.endValues.Maximum);
        }

        public override void PrepareForGallery(bool isEnabled)
        {
            base.ColorEach = isEnabled;
            base.PrepareForGallery(isEnabled);
            base.point.VertSize = 3;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("")]
        public ChartPen ConnectingPen
        {
            get
            {
                if (this.connectingPen == null)
                {
                    this.connectingPen = new ChartPen(base.Chart, Color.Black, true);
                }
                return this.connectingPen;
            }
            set
            {
                this.connectingPen = value;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryGantt;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gantt End values.")]
        public ValueList EndValues
        {
            get
            {
                return this.endValues;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gantt Next task values.")]
        public ValueList NextTasks
        {
            get
            {
                return this.nextTask;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gantt Start values.")]
        public ValueList StartValues
        {
            get
            {
                return base.vxValues;
            }
        }
    }
}

