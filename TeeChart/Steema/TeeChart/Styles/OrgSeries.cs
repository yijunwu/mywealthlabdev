namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;

    [ToolboxBitmap(typeof(OrgSeries), "SeriesIcons.OrgSeries.bmp")]
    public class OrgSeries : Series
    {
        private OrgShape fDefault;
        private OrgLineStyle linestyle;
        private OrgItems nodes;
        private OrgDraw oDraw;
        private ChartPen pPen;
        private Spacing spacing;

        public OrgSeries() : this(null)
        {
        }

        public OrgSeries(Chart c) : base(c)
        {
            base.calcVisiblePoints = false;
            base.ShowInLegend = false;
            base.manualData = true;
            base.UseAxis = false;
            if (this.oDraw == null)
            {
                this.oDraw = new OrgDraw(this);
            }
            if (this.nodes == null)
            {
                this.nodes = new OrgItems(this);
            }
            this.spacing = new Spacing(this);
            this.fDefault = new OrgShape();
            this.fDefault.TextAlign = StringAlignment.Center;
        }

        public int Add(string aText)
        {
            return this.Add(aText, -1);
        }

        public int Add(string aText, int aSuperior)
        {
            int num = base.Add((double) aSuperior, aText);
            OrgShape format = this.nodes.Add().Format;
            format = this.fDefault.Clone() as OrgShape;
            format.Text = aText;
            return num;
        }

        protected override void AddSampleValues(int numValues)
        {
            Random r = new Random();
            int aSuperior = this.Add(this.RandomName(r) + "President");
            this.Add(this.RandomName(r) + "Sales director", aSuperior);
            int num2 = this.Add(this.RandomName(r) + "Sales director\nUSA", aSuperior);
            this.Add(this.RandomName(r) + "Asian Sales", num2);
            num2 = this.Add(this.RandomName(r) + "Human Relations", aSuperior);
            this.Add(this.RandomName(r) + "Assistant", num2);
        }

        public override void AssignFormat(Series source)
        {
            base.AssignFormat(source);
            if (source is OrgSeries)
            {
                this.nodes.Assign((source as OrgSeries).Items);
                this.linestyle = (source as OrgSeries).LineStyle;
                this.spacing.Vertical = (source as OrgSeries).ItemSpacing.Vertical;
                this.spacing.Horizontal = (source as OrgSeries).ItemSpacing.Horizontal;
            }
        }

        public override int CalcXPos(int index)
        {
            Rectangle shapeBounds = this.nodes[index].Format.ShapeBounds;
            return ((shapeBounds.Left + shapeBounds.Right) / 2);
        }

        public override int CalcYPos(int index)
        {
            return this.nodes[index].Format.ShapeBounds.Bottom;
        }

        protected override void ClearLists()
        {
            base.ClearLists();
            if (this.nodes != null)
            {
                this.nodes.Clear();
            }
        }

        public override int Clicked(int x, int y)
        {
            for (int i = 0; i < Math.Min(this.nodes.Count, base.Count); i++)
            {
                if (this.nodes[i].Format.ShapeBounds.Contains(x, y))
                {
                    return i;
                }
            }
            return -1;
        }

        public override void Delete(int index)
        {
            int num = 0;
            do
            {
                num = this.FirstChild(index);
                if (num != -1)
                {
                    this.Delete(num);
                }
            }
            while (num != -1);
            base.Delete(index);
            if (this.nodes.Count > index)
            {
                this.nodes.RemoveAt(index);
            }
            for (int i = 0; i < base.Count; i++)
            {
                this.Items[i].Index = i;
                if (base.mandatory[i] > index)
                {
                    ValueList list;
                    int num3;
                    (list = base.mandatory)[num3 = i] = list[num3] - 1.0;
                }
            }
        }

        public override void Draw()
        {
            if (base.Count > 0)
            {
                this.oDraw.Draw();
            }
        }

        public int FirstChild(int valueindex)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base.mandatory[i] == valueindex)
                {
                    return i;
                }
            }
            return -1;
        }

        public override bool IsValidSourceOf(Series value)
        {
            return (value is OrgSeries);
        }

        private string RandomName(Random r)
        {
            int num = 9;
            string[] strArray = new string[] { "John", "Anne", "Mary", "Paul", "Bob", "Mike", "Lisa", "Brad", "Peter" };
            string[] strArray2 = new string[] { "Smith", "Shane", "Wizard", "Smart", "Best", "Patson", "Hood", "Dale", "Scarlet" };
            return (strArray[r.Next(num - 1)] + " " + strArray2[r.Next(num - 1)] + "\n");
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.nodes != null)
            {
                foreach (OrgItem item in this.nodes)
                {
                    item.Format.Chart = c;
                }
            }
        }

        internal override void SwapValueIndex(int a, int b)
        {
            base.SwapValueIndex(a, b);
            OrgItem item = this.nodes[b];
            this.nodes[a].Index = b;
            item.Index = a;
        }

        public Rectangle Bounds
        {
            get
            {
                return this.oDraw.Bounds;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryOrgChart;
            }
        }

        public OrgShape Format
        {
            get
            {
                return this.fDefault;
            }
            set
            {
                this.fDefault = value;
            }
        }

        public OrgItem this[int index]
        {
            get
            {
                return this.nodes[index];
            }
            set
            {
                if (this.nodes[index] != null)
                {
                    OrgItem item = value;
                }
            }
        }

        public OrgItems Items
        {
            get
            {
                return this.nodes;
            }
            set
            {
                this.nodes = value;
                this.Invalidate();
            }
        }

        public Spacing ItemSpacing
        {
            get
            {
                return this.spacing;
            }
            set
            {
                this.spacing.Vertical = value.Vertical;
                this.spacing.Horizontal = value.Horizontal;
            }
        }

        public OrgLineStyle LineStyle
        {
            get
            {
                return this.linestyle;
            }
            set
            {
                this.linestyle = value;
                this.Invalidate();
            }
        }

        [Description("Pen used to draw the connecting lines."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (this.pPen == null)
                {
                    this.pPen = new ChartPen(base.chart, base.Color, true, LineCap.Square);
                }
                return this.pPen;
            }
        }

        public class OrgDraw : TeeBase
        {
            private int ibb = -2147483647;
            private int ibl = 0x7fffffff;
            private int ibr = -2147483647;
            private int ibt = 0x7fffffff;
            private OrgSeries ser;

            public OrgDraw(OrgSeries s)
            {
                this.ser = s;
            }

            private void AutoSizeNodes()
            {
                for (int i = 0; i < Math.Min(this.ser.nodes.Count, this.ser.Count); i++)
                {
                    TextShapePosition format = this.ser.nodes[i].Format;
                    format.Text = this.ser.Labels[i];
                    if (format.AutoSize)
                    {
                        format.CalcBounds();
                    }
                }
            }

            private int ChildCount(int index)
            {
                int num = 0;
                for (int i = 0; i < this.ser.Count; i++)
                {
                    if (this.ser.mandatory[i] == index)
                    {
                        num++;
                    }
                }
                return num;
            }

            public void Draw()
            {
                if (this.ser.Count > 0)
                {
                    this.ibt = 0x7fffffff;
                    this.ibl = 0x7fffffff;
                    this.ibr = -2147483647;
                    this.ibb = -2147483647;
                    this.AutoSizeNodes();
                    this.DrawNodes(-1, this.ser.Chart.Graphics3D.ChartXCenter, this.ser.Chart.ChartRectTop);
                }
            }

            private void DrawChilds(OrgItemList l, int xPos, int yPos)
            {
                TextShapePosition format;
                int height;
                int treeWidth = this.GetTreeWidth(l);
                this.ibt = Math.Min(this.ibt, yPos);
                for (int i = 0; i < l.Count; i++)
                {
                    format = this.ser.nodes[this.ser.nodes.IndexOf(l[i])].Format;
                    height = format.Height;
                    format.Top = yPos + this.ser.Chart.Aspect.VertOffset;
                    format.Height = height;
                }
                int num4 = (xPos - (treeWidth / 2)) + this.ser.Chart.Aspect.HorizOffset;
                this.ibl = Math.Min(this.ibl, num4);
                int num5 = 0;
                int left = 0;
                int right = 0;
                int top = 0;
                int num9 = 0;
                for (int j = 0; j < l.Count; j++)
                {
                    format = this.ser.nodes[this.ser.nodes.IndexOf(l[j])].Format;
                    height = format.Width;
                    format.Left = num4;
                    format.Width = height;
                    Rectangle shapeBounds = format.ShapeBounds;
                    if (j == 0)
                    {
                        left = num4 + (shapeBounds.Width / 2);
                    }
                    else if (j == (l.Count - 1))
                    {
                        right = num4 + (shapeBounds.Width / 2);
                    }
                    int index = Utils.Round(this.ser.mandatory[this.ser.nodes.IndexOf(l[j])]);
                    if (index != -1)
                    {
                        this.ser.Chart.Graphics3D.Pen = this.ser.Pen;
                        if (this.ser.LineStyle == OrgLineStyle.lsSquared)
                        {
                            if (this.ChildCount(index) == 1)
                            {
                                top = this.ser.CalcYPos(index);
                            }
                            else
                            {
                                top = format.Top - (this.ser.ItemSpacing.Vertical / 2);
                            }
                            this.ser.Chart.Graphics3D.VerticalLine(num4 + (shapeBounds.Width / 2), top, format.Top);
                        }
                        else
                        {
                            if (this.ChildCount(index) == 1)
                            {
                                num9 = this.ser.CalcXPos(index);
                            }
                            else
                            {
                                num9 = num4 + (shapeBounds.Width / 2);
                            }
                            this.ser.Chart.Graphics3D.Line(num9, format.Top, this.ser.CalcXPos(index), this.ser.CalcYPos(index));
                        }
                    }
                    format.DrawText();
                    num5 = Math.Max(num5, shapeBounds.Height + 1);
                    num4 += shapeBounds.Width + 1;
                    if (j < (l.Count - 1))
                    {
                        num4 += this.ser.ItemSpacing.Horizontal;
                    }
                    else if (format.Shadow.Visible && (format.Shadow.Width > 0))
                    {
                        num4 += format.Shadow.Width;
                    }
                    if (format.Shadow.Visible && (format.Shadow.Height > 0))
                    {
                        this.ibb = Math.Max(this.ibb, shapeBounds.Bottom + format.Shadow.Height);
                    }
                    else
                    {
                        this.ibb = Math.Max(this.ibb, shapeBounds.Bottom);
                    }
                }
                this.ibr = Math.Max(this.ibr, num4);
                if ((this.ser.LineStyle == OrgLineStyle.lsSquared) && (l.Count > 1))
                {
                    this.ser.Chart.Graphics3D.Pen = this.ser.Pen;
                    this.ser.Chart.Graphics3D.HorizontalLine(left, right, top);
                }
                OrgItemList list = new OrgItemList();
                for (int k = 0; k < l.Count; k++)
                {
                    format = this.ser.nodes[this.ser.nodes.IndexOf(l[k])].Format;
                    xPos = (format.ShapeBounds.Left + format.ShapeBounds.Right) / 2;
                    this.GetChilds(list, this.ser.nodes.IndexOf(l[k]), true);
                    if (list.Count > 0)
                    {
                        if ((list.Count > 1) && (this.ser.LineStyle == OrgLineStyle.lsSquared))
                        {
                            this.ser.Chart.Graphics3D.Pen = this.ser.Pen;
                            top = format.ShapeBounds.Bottom;
                            this.ser.Chart.Graphics3D.VerticalLine(xPos, top, (yPos + num5) + (this.ser.ItemSpacing.Vertical / 2));
                        }
                        this.DrawChilds(list, xPos, (yPos + num5) + this.ser.ItemSpacing.Vertical);
                    }
                }
            }

            private void DrawNodes(int parent, int xPos, int yPos)
            {
                OrgItemList l = new OrgItemList();
                this.GetChilds(l, parent, true);
                this.DrawChilds(l, xPos, yPos);
            }

            private void GetChilds(OrgItemList l, int superior, bool visibleOnly)
            {
                l.Clear();
                for (int i = 0; i < Math.Min(this.ser.nodes.Count, this.ser.Count); i++)
                {
                    if ((this.ser.mandatory[i] == superior) && (!visibleOnly || this.ser.nodes[i].Format.Visible))
                    {
                        l.Add(this.ser.nodes[i]);
                    }
                }
            }

            private int GetTreeWidth(OrgItemList l)
            {
                int num = this.ser.ItemSpacing.Horizontal * (l.Count - 1);
                int num2 = 0;
                for (int i = 0; i < l.Count; i++)
                {
                    Rectangle shapeBounds = this.ser.nodes[this.ser.nodes.IndexOf(l[i])].Format.ShapeBounds;
                    num += Math.Max(num2, shapeBounds.Width + 1);
                }
                return num;
            }

            public Rectangle Bounds
            {
                get
                {
                    return Utils.FromLTRB(this.ibl, this.ibt, this.ibr, this.ibb);
                }
            }
        }
    }
}

