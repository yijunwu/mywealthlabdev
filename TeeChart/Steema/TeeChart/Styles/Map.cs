namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;

    [ToolboxBitmap(typeof(Map), "SeriesIcons.Map.bmp")]
    public class Map : Custom3DPalette
    {
        private Polygon[] i3DList;
        private Random rnd;
        private PolygonList shapes;
        private bool tmpClip;
        private Rectangle tmpRect;
        private int transparency;

        public Map() : this(null)
        {
        }

        public Map(Chart c) : base(c)
        {
            this.rnd = new Random();
            this.shapes = new PolygonList(this);
            base.calcVisiblePoints = false;
            base.yMandatory = false;
            base.mandatory = base.ZValues;
            this.transparency = 0;
        }

        protected override void AddSampleValues(int numValues)
        {
            int[] x = new int[] { 1, 3, 4, 4, 5, 5, 6, 6, 4, 3, 2, 1, 2, 2 };
            int[] y = new int[] { 7, 5, 5, 7, 8, 9, 10, 11, 11, 12, 12, 11, 10, 8 };
            int[] numArray3 = new int[] { 5, 7, 8, 8, 7, 6, 5, 4, 4 };
            int[] numArray4 = new int[] { 4, 4, 5, 6, 7, 7, 8, 7, 5 };
            int[] numArray5 = new int[] { 9, 10, 11, 11, 12, 9, 8, 7, 6, 6, 5, 5, 6, 7, 8, 8 };
            int[] numArray6 = new int[] { 5, 6, 6, 7, 8, 11, 11, 12, 11, 10, 9, 8, 7, 7, 6, 5 };
            int[] numArray7 = new int[] { 12, 14, 15, 14, 13, 12, 11, 11 };
            int[] numArray8 = new int[] { 5, 5, 6, 7, 7, 8, 7, 6 };
            int[] numArray9 = new int[] { 4, 6, 7, 7, 6, 6, 5, 4, 3, 3, 2 };
            int[] numArray10 = new int[] { 11, 11, 12, 13, 14, 15, 0x10, 0x10, 15, 14, 13 };
            int[] numArray11 = new int[] { 7, 8, 9, 11, 10, 8, 7, 6, 5, 5, 6, 6 };
            int[] numArray12 = new int[] { 13, 14, 14, 0x10, 0x11, 0x11, 0x12, 0x12, 0x11, 0x10, 15, 14 };
            int[] numArray13 = new int[] { 10, 12, 12, 14, 13, 11, 9, 8, 7, 7, 8, 9 };
            int[] numArray14 = new int[] { 10, 12, 13, 15, 0x10, 0x10, 14, 14, 13, 12, 11, 11 };
            int[] numArray15 = new int[] { 0x11, 0x13, 0x12, 0x12, 0x11, 15, 14, 13, 15, 0x10 };
            int[] numArray16 = new int[] { 11, 13, 14, 0x10, 0x11, 15, 15, 14, 12, 12 };
            int[] numArray17 = new int[] { 15, 0x10, 0x11, 0x10, 15, 14, 14, 13, 12, 11, 10, 11, 12, 13, 14 };
            int[] numArray18 = new int[] { 6, 6, 7, 8, 8, 9, 10, 11, 12, 11, 10, 9, 8, 7, 7 };
            int[] numArray19 = new int[] { 15, 0x10, 0x10, 0x11, 0x11, 0x10, 15, 13, 12, 12, 14, 14 };
            int[] numArray20 = new int[] { 8, 8, 9, 10, 11, 12, 12, 14, 13, 12, 10, 9 };
            int[] numArray21 = new int[] { 0x11, 0x13, 20, 20, 0x13, 0x11, 0x10, 0x10, 0x11, 0x10 };
            int[] numArray22 = new int[] { 5, 5, 6, 8, 8, 10, 9, 8, 7, 6 };
            int[] numArray23 = new int[] { 0x13, 20, 0x15, 0x15, 0x13, 0x11, 0x11 };
            int[] numArray24 = new int[] { 8, 8, 9, 11, 13, 11, 10 };
            for (int i = 0; i < numValues; i++)
            {
                switch ((i % this.NumSampleValues()))
                {
                    case 0:
                        this.AddShape(x, y, "A");
                        break;

                    case 1:
                        this.AddShape(numArray3, numArray4, "B");
                        break;

                    case 2:
                        this.AddShape(numArray5, numArray6, "C");
                        break;

                    case 3:
                        this.AddShape(numArray7, numArray8, "D");
                        break;

                    case 4:
                        this.AddShape(numArray9, numArray10, "E");
                        break;

                    case 5:
                        this.AddShape(numArray11, numArray12, "F");
                        break;

                    case 6:
                        this.AddShape(numArray13, numArray14, "G");
                        break;

                    case 7:
                        this.AddShape(numArray15, numArray16, "H");
                        break;

                    case 8:
                        this.AddShape(numArray17, numArray18, "I");
                        break;

                    case 9:
                        this.AddShape(numArray19, numArray20, "J");
                        break;

                    case 10:
                        this.AddShape(numArray21, numArray22, "K");
                        break;

                    case 11:
                        this.AddShape(numArray23, numArray24, "L");
                        break;
                }
                if (base.GetVertAxis.Labels.Style == AxisLabelStyle.Auto)
                {
                    base.GetVertAxis.Labels.Style = AxisLabelStyle.Value;
                }
            }
        }

        public void AddShape(int[] X, int[] Y, string Text)
        {
            int num2;
            int num3;
            if (base.Count > this.NumSampleValues())
            {
                num2 = this.rnd.Next(this.NumSampleValues());
                num3 = this.rnd.Next(this.NumSampleValues());
            }
            else
            {
                num2 = 0;
                num3 = 0;
            }
            Polygon polygon = new Polygon(this.shapes, base.Chart);
            for (int i = X.GetLowerBound(0); i <= X.GetUpperBound(0); i++)
            {
                polygon.Add((double) (num2 + X[i]), (double) (num3 + Y[i]));
            }
            int num4 = this.Shapes.Add(polygon);
            if (Text != "")
            {
                this.Shapes[num4].Text = Text;
            }
            else
            {
                this.Shapes[num4].Text = num4.ToString();
            }
            this.Shapes[num4].Z = ((double) this.rnd.Next(0x3e8)) / 1000.0;
        }

        protected internal override void CalcHorizMargins(ref int leftMargin, ref int rightMargin)
        {
            base.CalcHorizMargins(ref leftMargin, ref rightMargin);
            if (base.Pen.Visible)
            {
                leftMargin += base.Pen.Width;
                rightMargin += base.Pen.Width;
            }
        }

        protected internal override void CalcVerticalMargins(ref int topMargin, ref int bottomMargin)
        {
            base.CalcVerticalMargins(ref topMargin, ref bottomMargin);
            bottomMargin++;
            if (base.Pen.Visible)
            {
                topMargin += base.Pen.Width;
                bottomMargin += base.Pen.Width;
            }
        }

        public override void Clear()
        {
            base.Clear();
            if (this.Shapes != null)
            {
                this.Shapes.Clear();
            }
        }

        public override int Clicked(int x, int y)
        {
            if (base.Chart != null)
            {
                this.tmpClip = base.Chart.Aspect.ClipPoints;
                this.tmpRect = base.Chart.ChartRect;
                for (int i = this.Shapes.Count - 1; i >= 0; i--)
                {
                    Polygon polygon = this.Shapes[i];
                    if (this.IsShapeVisible(polygon.Points))
                    {
                        int num = x;
                        int num2 = y;
                        base.Chart.Graphics3D.Calculate2DPosition(ref num, ref num2, base.CalcZPos(i));
                        if (Graphics3D.PointInPolygon(new Point(num, num2), polygon.GetPoints()))
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        private int CompareOrder(int a, int b)
        {
            double z = this.i3DList[a].Z;
            double num2 = this.i3DList[b].Z;
            if (z > num2)
            {
                return 1;
            }
            if (z < num2)
            {
                return -1;
            }
            return 0;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Colors);
        }

        public override void Delete(int index)
        {
            base.Delete(index);
            if (this.shapes != null)
            {
                this.shapes.RemoveAt(index);
            }
        }

        public override void Delete(int index, int count, bool removeGap)
        {
            base.Delete(index, count, removeGap);
            if (this.shapes != null)
            {
                this.shapes.RemoveRange(index, count);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if ((base.GetVertAxis != null) && (base.GetVertAxis.Labels.Style != AxisLabelStyle.Auto))
            {
                base.GetVertAxis.Labels.Style = AxisLabelStyle.Auto;
            }
            this.Shapes.Clear();
            base.Dispose(disposing);
        }

        public override void Draw()
        {
            if (base.Chart.Aspect.View3D)
            {
                this.DrawAllSorted();
            }
            else
            {
                base.Draw();
            }
        }

        private void DrawAllSorted()
        {
            int count = this.Shapes.Count;
            if (count > 0)
            {
                this.i3DList = new Polygon[count];
                try
                {
                    int num;
                    for (num = 0; num < count; num++)
                    {
                        this.i3DList[num] = this.Shapes[num];
                    }
                    Utils.Sort(0, count - 1, new Utils.CompareEventHandler(this.CompareOrder), new Utils.SwapEventHandler(this.SwapPolygon));
                    for (num = count - 1; num >= 0; num--)
                    {
                        this.i3DList[num].Draw(base.Chart.Graphics3D, this.i3DList[num].Index);
                        if (this.i3DList[num].ParentBrush)
                        {
                            this.i3DList[num].Color = this.ValueColor(this.i3DList[num].Index);
                        }
                        else
                        {
                            this.i3DList[num].Color = this.i3DList[num].Brush.Color;
                        }
                    }
                }
                finally
                {
                    this.i3DList = null;
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            if (this.Shapes.Count > valueIndex)
            {
                Rectangle rectangle = this.Shapes[valueIndex].Bounds();
                aPosition.LeftTop.X = ((rectangle.Right + rectangle.Left) / 2) - (aPosition.Width / 2);
                aPosition.LeftTop.Y = ((rectangle.Top + rectangle.Bottom) / 2) - (aPosition.Height / 2);
            }
            base.DrawMark(valueIndex, st, aPosition);
        }

        public override void DrawValue(int index)
        {
            if (this.Shapes.Count > index)
            {
                this.Shapes[index].Draw(base.Chart.Graphics3D, index);
            }
        }

        public override void GalleryChanged3D(bool Is3D)
        {
            if (Is3D)
            {
                base.GalleryChanged3D(Is3D);
            }
            else
            {
                base.Chart.Aspect.View3D = false;
            }
        }

        protected internal override int[] GetBounds(int index, ref PolygonStyle p)
        {
            this.DrawValue(index);
            p = (base.chart.graphics3D as Graphics3DHotSpot).PolygonStyle;
            return (base.chart.graphics3D as Graphics3DHotSpot).GetBounds();
        }

        private Polygon GetPolygon(int Index)
        {
            return this.shapes[Index];
        }

        private bool IsShapeVisible(PolygonSeries Shape)
        {
            if (this.tmpClip)
            {
                int num2;
                Axis getHorizAxis = base.GetHorizAxis;
                int num = getHorizAxis.CalcPosValue(Shape.XValues.Minimum);
                if ((num < this.tmpRect.Left) || (num > this.tmpRect.Right))
                {
                    num2 = getHorizAxis.CalcPosValue(Shape.XValues.Maximum);
                    if ((num < this.tmpRect.Left) || ((num2 > this.tmpRect.Right) && (num > this.tmpRect.Right)))
                    {
                    }
                }
                Axis getVertAxis = base.GetVertAxis;
                num = getVertAxis.CalcPosValue(Shape.YValues.Maximum);
                if ((num < this.tmpRect.Top) || (num > this.tmpRect.Bottom))
                {
                    num2 = getVertAxis.CalcPosValue(Shape.YValues.Minimum);
                    return (((num2 >= this.tmpRect.Top) && (num2 <= this.tmpRect.Bottom)) || ((num2 > this.tmpRect.Bottom) && (num2 < this.tmpRect.Top)));
                }
                return true;
            }
            return true;
        }

        public override double MaxXValue()
        {
            if (this.Shapes.Count == 0)
            {
                return 0.0;
            }
            double num = this.Shapes[0].Points.MaxXValue();
            for (int i = 1; i < this.Shapes.Count; i++)
            {
                num = Math.Max(num, this.Shapes[i].Points.MaxXValue());
            }
            return num;
        }

        public override double MaxYValue()
        {
            if (this.Shapes.Count == 0)
            {
                return 0.0;
            }
            double num = this.Shapes[0].Points.MaxYValue();
            for (int i = 1; i < this.Shapes.Count; i++)
            {
                num = Math.Max(num, this.Shapes[i].Points.MaxYValue());
            }
            return num;
        }

        public override double MinXValue()
        {
            if (this.Shapes.Count == 0)
            {
                return 0.0;
            }
            double num = this.Shapes[0].Points.MinXValue();
            for (int i = 1; i < this.Shapes.Count; i++)
            {
                num = Math.Min(num, this.Shapes[i].Points.MinXValue());
            }
            return num;
        }

        public override double MinYValue()
        {
            if (this.Shapes.Count == 0)
            {
                return 0.0;
            }
            double num = this.Shapes[0].Points.MinYValue();
            for (int i = 1; i < this.Shapes.Count; i++)
            {
                num = Math.Min(num, this.Shapes[i].Points.MinYValue());
            }
            return num;
        }

        protected internal override int NumSampleValues()
        {
            return 12;
        }

        public override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            if (!IsEnabled)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    this.Shapes[i].Color = Color.Silver;
                }
            }
        }

        private void SetShapes(PolygonList Value)
        {
            this.shapes.Assign(Value);
        }

        public override void SetSubGallery(int index)
        {
            if (index == 2)
            {
                base.ColorEach = true;
            }
            else
            {
                base.SetSubGallery(index);
            }
        }

        protected virtual bool ShouldSerializeShapes()
        {
            return true;
        }

        private void SwapPolygon(int a, int b)
        {
            Polygon polygon = this.i3DList[a];
            this.i3DList[a] = this.i3DList[b];
            this.i3DList[b] = polygon;
        }

        internal override void SwapValueIndex(int a, int b)
        {
            base.SwapValueIndex(a, b);
            Polygon polygon = this.Shapes[a];
            Polygon polygon2 = this.Shapes[b];
            int index = polygon.Index;
            polygon.Index = polygon2.Index;
            polygon2.Index = index;
            this.Shapes[a] = polygon2;
            this.Shapes[b] = polygon;
        }

        public override string Description
        {
            get
            {
                return Texts.MapSeries;
            }
        }

        public Polygon this[int index]
        {
            get
            {
                return this.shapes[index];
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LabelMember
        {
            get
            {
                return base.LabelMember;
            }
            set
            {
                base.LabelMember = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PolygonList Shapes
        {
            get
            {
                return this.shapes;
            }
            set
            {
                this.shapes.Assign(value);
            }
        }

        [DefaultValue(0)]
        public int Transparency
        {
            get
            {
                return this.transparency;
            }
            set
            {
                if (this.transparency != value)
                {
                    this.transparency = value;
                    for (int i = 0; i < this.Shapes.Count; i++)
                    {
                        this.Shapes[i].Transparency = this.transparency;
                    }
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ValueList XValues
        {
            get
            {
                return base.XValues;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ValueList YValues
        {
            get
            {
                return base.YValues;
            }
        }
    }
}

