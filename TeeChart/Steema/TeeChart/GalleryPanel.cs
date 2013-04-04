namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class GalleryPanel : System.Windows.Forms.Panel
    {
        public GalleryChartCollection Charts = new GalleryChartCollection();
        public static bool CheckMaximize = true;
        private bool checkSeries = true;
        private static int colorPaletteIdx;
        private int colWidth;
        public static Cursor CursorDisabled = Cursors.No;
        private bool displaySub = true;
        public bool FunctionsVisible = true;
        public static int GalleryNumCols = 4;
        public static int GalleryNumRows = 3;
        private ArrayList items = new ArrayList();
        private IWorldMaps iWorldMaps;
        public GalleryChart LastSelectedChart;
        private int numCols = GalleryNumCols;
        private int numRows = GalleryNumRows;
        private int rowHeight;
        public GalleryChart SelectedChart;
        public Series SelectedSeries;
        private SmoothingMode smooth = SmoothingMode.HighQuality;
        private GalleryPanel tmpG;
        private Series tmpSeries;
        private Form tmpSub;
        private bool view3D = true;

        public event EventHandler OnChangeChart;

        public event EventHandler OnSelectedChart;

        public event EventHandler OnSubSelected;

        private void AddSubCharts(GalleryPanel AGallery)
        {
            AGallery.Charts.Clear();
            if (this.tmpSeries != null)
            {
                this.tmpSeries.CreateSubGallery(new Series.SubGalleryEventHandler(this.CreateSubChart));
                AGallery.CalcChartWidthHeight();
            }
        }

        internal void AddSubCharts(Series s)
        {
            this.tmpG = this;
            this.tmpSeries = s;
            this.AddSubCharts(this);
        }

        public void ApplyPalette(int galleryPaletteIndex)
        {
            foreach (GalleryChart chart in this.Charts)
            {
                ColorPalettes.ApplyPalette(chart.Chart, galleryPaletteIndex);
                colorPaletteIdx = galleryPaletteIndex;
            }
        }

        private void CalcChartWidthHeight()
        {
            if ((this.NumRows > 0) && (this.NumCols > 0))
            {
                int num = 1;
                this.rowHeight = (base.ClientRectangle.Height - num) / this.NumRows;
                this.colWidth = (base.ClientRectangle.Width - num) / this.NumCols;
            }
            else
            {
                this.rowHeight = 0;
                this.colWidth = 0;
            }
        }

        private int CalcCount(string PageName, bool Functions)
        {
            int length = 0;
            if (Functions)
            {
                for (int i = 0; i < Utils.FunctionTypesCount; i++)
                {
                    if (this.ValidFunction(i, PageName))
                    {
                        length++;
                    }
                }
            }
            else
            {
                for (int j = 0; j < Utils.SeriesTypesCount; j++)
                {
                    if (this.ValidSeries(j, PageName))
                    {
                        length++;
                    }
                }
            }
            if (PageName == Texts.GalleryWorldMaps)
            {
                length = this.iWorldMaps.GetWorldMapTypeNames().GetLength(0);
            }
            return length;
        }

        private void ChartEvent(object sender, EventArgs e)
        {
            if (sender is GalleryChart)
            {
                this.SelectChart((GalleryChart) sender);
            }
        }

        private void ChartOnDblClick(object sender, EventArgs e)
        {
            this.SelectedChart = (GalleryChart) sender;
            if (this.OnSelectedChart != null)
            {
                this.OnSelectedChart(this, e);
            }
        }

        private GalleryChart CreateChart(System.Type seriesType)
        {
            return this.CreateChart(Utils.SeriesTypesIndex(seriesType), "", null);
        }

        private GalleryChart CreateChart(int index, System.Type function)
        {
            return this.CreateChart(index, this.FunctionDescription(function), function);
        }

        private GalleryChart CreateChart(int index, string title, System.Type function)
        {
            return this.CreateChart(index, title, function, null);
        }

        private GalleryChart CreateChart(int index, string title, System.Type function, object tag)
        {
            GalleryChart item = new GalleryChart(this.Smooth);
            this.Charts.Add(item);
            item.Parent = this;
            this.ResizeChart(item);
            if (function == null)
            {
                this.CreateSeries(item, index, new object[] { tag });
            }
            else
            {
                for (int i = 1; i <= 2; i++)
                {
                    if (function == typeof(CompressOHLC))
                    {
                        Series.CreateNewSeries(item.Chart, typeof(Candle), function);
                    }
                    else
                    {
                        Series.CreateNewSeries(item.Chart, typeof(Line), function);
                    }
                }
            }
            if (tag != null)
            {
                item.Tag = tag;
            }
            item.Header.Lines = ((title.Length == 0) ? item[0].Description : title).Split(new char[] { '\n' }, 5);
            bool flag = (this.checkSeries && (this.SelectedSeries != null)) && !item[0].IsValidSourceOf(this.SelectedSeries);
            if (flag)
            {
                item.Cursor = Cursors.No;
                item.Click -= new EventHandler(this.ChartEvent);
                item.Enter -= new EventHandler(this.ChartEvent);
                item.DoubleClick -= new EventHandler(this.ChartOnDblClick);
                item.Header.Font.Color = Color.Gray;
                item.Walls.Left.Pen.Color = Color.Gray;
                item.Walls.Bottom.Pen.Color = Color.Gray;
                item.Axes.Left.AxisPen.Width = 1;
                item.Axes.Left.AxisPen.Color = Color.White;
                item.Axes.Bottom.AxisPen.Width = 1;
                item.Axes.Bottom.AxisPen.Color = Color.White;
            }
            else
            {
                item.Cursor = Cursors.Hand;
                item.Click += new EventHandler(this.ChartEvent);
                item.DoubleClick += new EventHandler(this.ChartOnDblClick);
                item.Enter += new EventHandler(this.ChartEvent);
            }
            item.Series[0].GalleryChanged3D(this.view3D);
            foreach (Series series in item.Series)
            {
                series.PrepareForGallery(!flag);
            }
            item.SetMargins();
            item.CheckShowLabels();
            return item;
        }

        public void CreateGalleryPage(string pageName)
        {
            this.CreateGalleryPage(pageName, false);
        }

        public void CreateGalleryPage(string pageName, bool functions)
        {
            this.SetIWorldMaps();
            int num = this.CalcCount(pageName, functions);
            if (num > (this.NumRows * this.NumCols))
            {
                this.numRows = 1 + ((num - 1) / this.NumCols);
            }
            else
            {
                this.numRows = GalleryNumRows;
            }
            this.CalcChartWidthHeight();
            this.Charts.Clear();
            if (functions)
            {
                for (int i = 0; i < Utils.FunctionTypesCount; i++)
                {
                    if (this.ValidFunction(i, pageName))
                    {
                        this.items.Add(Utils.FunctionTypesOf[i].ToString());
                        this.CreateChart(i, (System.Type) Utils.FunctionTypesOf[i]);
                    }
                }
            }
            else
            {
                for (int j = 0; j < Utils.SeriesTypesCount; j++)
                {
                    if (this.ValidSeries(j, pageName))
                    {
                        if (Utils.SeriesTypesOf[j].GetInterface("IWorldMaps") != null)
                        {
                            this.CreateWorldMapChart(Utils.SeriesTypesOf[j], j);
                        }
                        else
                        {
                            this.items.Add(Utils.SeriesTypesOf[j].ToString());
                            this.CreateChart(Utils.SeriesTypesOf[j]);
                        }
                    }
                }
            }
            this.iWorldMaps = null;
            this.FindSelectedChart();
            this.ShowSelectedChart();
        }

        public void CreateGallerySeries(System.Type seriesType)
        {
            this.items.Add(seriesType.ToString());
            this.CreateChart(seriesType);
        }

        private void CreateSeries(GalleryChart c, int s, params object[] tag)
        {
            int num = Utils.SeriesGalleryCount[s];
            for (int i = 1; i <= Math.Max(1, num); i++)
            {
                Series.CreateNewSeries(c.Chart, Utils.SeriesTypesOf[s], null, 0, tag);
            }
        }

        private Chart CreateSubChart(string Name)
        {
            int count = this.tmpG.Charts.Count;
            GalleryChart chart = this.tmpG.CreateChart(Utils.SeriesTypesIndex(this.tmpSeries), Name, null);
            for (int i = 0; i < chart.Series.Count; i++)
            {
                chart[i].SetSubGallery(count);
            }
            return chart.Chart;
        }

        private void CreateSubGallery(GalleryPanel AGallery, System.Type type)
        {
            this.tmpSeries = Series.CreateNewSeries(null, type, null);
            this.AddSubCharts(AGallery);
            int num = AGallery.Charts.Count / AGallery.NumCols;
            if ((AGallery.Charts.Count % AGallery.NumCols) > 0)
            {
                num++;
            }
            AGallery.Parent.Height = 0x26 + (AGallery.rowHeight * num);
            AGallery.NumRows = num;
            this.tmpSeries.Dispose();
        }

        private void CreateWorldMapChart(System.Type seriesType, int t)
        {
            this.items.Add(Utils.SeriesTypesOf[t].ToString());
            foreach (string str in this.iWorldMaps.GetWorldMapTypeNames())
            {
                object tag = this.iWorldMaps.ParseWorldMapTypeName(str);
                this.CreateChart(Utils.SeriesTypesIndex(seriesType), this.GetTranslatedTitle(str), null, tag);
            }
        }

        private GalleryChart FindChartXY(int x, int y)
        {
            foreach (GalleryChart chart in this.Charts)
            {
                int num;
                int num2;
                this.GetChartXY(chart, out num, out num2);
                if ((x == num) && (y == num2))
                {
                    return chart;
                }
            }
            return null;
        }

        private void FindSelectedChart()
        {
            this.SelectedChart = null;
            if (this.SelectedSeries != null)
            {
                for (int i = 0; i < this.Charts.Count; i++)
                {
                    if ((this.items[i] == this.SelectedSeries.GetType()) && (this.Charts[i].Series[0].Function == null))
                    {
                        this.SelectedChart = this.Charts[i];
                        return;
                    }
                }
            }
        }

        private string FunctionDescription(System.Type f)
        {
            return Function.NewInstance(f).Description();
        }

        public static string GalleryPages(int index)
        {
            switch (index)
            {
                case 0:
                    return Texts.GalleryStandard;

                case 1:
                    return Texts.GalleryFinancial;

                case 2:
                    return Texts.GalleryStats;

                case 3:
                    return Texts.GalleryExtended;

                case 4:
                    return Texts.Gallery3D;

                case 5:
                    return Texts.GalleryGauges;

                case 6:
                    return Texts.GalleryWorldMaps;
            }
            return Texts.GallerySamples;
        }

        private void GetChartXY(GalleryChart c, out int x, out int y)
        {
            int index = this.Charts.IndexOf(c);
            y = index / this.numCols;
            x = index % this.numCols;
        }

        private string GetTranslatedTitle(string name)
        {
            switch (name)
            {
                case "World":
                    return Texts.MappingWorld;

                case "Africa":
                    return Texts.MappingAfrica;

                case "Asia":
                    return Texts.MappingAsia;

                case "Australia":
                    return Texts.MappingAustralia;

                case "CentralAmerica":
                    return Texts.MappingCentralAmerica;

                case "Europe":
                    return Texts.MappingEurope;

                case "Europe15":
                    return Texts.MappingEurope15;

                case "Europe27":
                    return Texts.MappingEurope27;

                case "Spain":
                    return Texts.MappingSpain;

                case "MiddleEast":
                    return Texts.MappingMiddleEast;

                case "NorthAmerica":
                    return Texts.MappingNorthAmerica;

                case "SouthAmerica":
                    return Texts.MappingSouthAmerica;

                case "USA":
                    return Texts.MappingUSA;

                case "USAHawaiiAlaska":
                    return Texts.MappingUSAHawaiiAlaska;
            }
            return Texts.MappingWorld;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            int num;
            int num2;
            base.OnKeyDown(e);
            this.GetChartXY(this.SelectedChart, out num, out num2);
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (num > 0)
                    {
                        num--;
                    }
                    break;

                case Keys.Up:
                    if (num2 > 0)
                    {
                        num2--;
                    }
                    break;

                case Keys.Right:
                    if (num < this.NumCols)
                    {
                        num++;
                    }
                    break;

                case Keys.Down:
                    if (!e.Alt)
                    {
                        if (num2 < this.NumRows)
                        {
                            num2++;
                        }
                        break;
                    }
                    if (this.DisplaySub)
                    {
                        this.ShowSubGallery();
                    }
                    break;

                case Keys.Enter:
                    this.ChartOnDblClick(this.SelectedChart, new EventArgs());
                    break;
            }
            GalleryChart c = this.FindChartXY(num, num2);
            if ((c != null) && (c.Cursor == Cursors.Hand))
            {
                this.SelectChart(c);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.ResizeCharts();
        }

        private void ResizeChart(GalleryChart c)
        {
            if ((this.NumCols > 0) && (this.NumRows > 0))
            {
                int num;
                int num2;
                this.GetChartXY(c, out num, out num2);
                int num3 = 1;
                c.Left = num3 + (num * this.colWidth);
                c.Top = num3 + (num2 * this.rowHeight);
                c.Width = Math.Min(this.colWidth, base.Width - num3) - 1;
                c.Height = Math.Min(this.rowHeight, base.Height - num3) - 1;
                c.CheckShowLabels();
            }
        }

        private void ResizeCharts()
        {
            this.CalcChartWidthHeight();
            foreach (GalleryChart chart in this.Charts)
            {
                this.ResizeChart(chart);
            }
        }

        private void SelectChart(GalleryChart c)
        {
            if (c != this.SelectedChart)
            {
                this.SelectedChart = c;
                this.ShowSelectedChart();
            }
            if (this.displaySub)
            {
                Rectangle rectangle = new Rectangle(0, Utils.Round((float) this.SelectedChart.Height) - 12, 12, 12);
                if (rectangle.Contains(c.PointToClient(Control.MousePosition)))
                {
                    this.ShowSubGallery();
                }
            }
        }

        private void SetIWorldMaps()
        {
            for (int i = 0; i < Utils.SeriesTypesCount; i++)
            {
                if (Utils.SeriesTypesOf[i].GetInterface("IWorldMaps") != null)
                {
                    this.iWorldMaps = (IWorldMaps) Activator.CreateInstance(Utils.SeriesTypesOf[i]);
                }
            }
        }

        public void SetSubSelected(Series s, int index)
        {
            s.SetSubGallery(index);
        }

        private void ShowSelectedChart()
        {
            if ((this.SelectedChart == null) && (this.Charts.Count > 0))
            {
                this.SelectedChart = this.Charts[0];
            }
            if (this.SelectedChart != null)
            {
                this.SelectedChart.DoFocus(this.view3D);
                this.SelectedChart.Invalidate();
                if (this.OnChangeChart != null)
                {
                    this.OnChangeChart(this, EventArgs.Empty);
                }
            }
            if (this.LastSelectedChart != null)
            {
                this.LastSelectedChart.UnFocus(this.view3D);
                this.LastSelectedChart.Invalidate();
            }
            this.LastSelectedChart = this.SelectedChart;
        }

        private void ShowSubGallery()
        {
            if (this.tmpSub != null)
            {
                this.tmpSub.Dispose();
            }
            this.SelectedChart.UnFocus(this.view3D);
            this.tmpSub = new Form();
            this.tmpSub.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.tmpSub.WindowState = FormWindowState.Normal;
            this.tmpSub.StartPosition = FormStartPosition.Manual;
            this.tmpSub.MinimizeBox = false;
            this.tmpSub.MaximizeBox = false;
            Point empty = Point.Empty;
            empty = base.PointToScreen(this.SelectedChart.Location);
            this.tmpSub.Top = empty.Y + Utils.Round((float) this.SelectedChart.Height);
            this.tmpSub.Left = empty.X;
            this.tmpSub.Size = new Size(300, 200);
            this.tmpSub.KeyDown += new KeyEventHandler(this.tmpSub_KeyDown);
            this.tmpSub.KeyPreview = true;
            this.tmpG = new GalleryPanel();
            this.tmpG.View3D = this.view3D;
            this.tmpG.OnSelectedChart = (EventHandler) Delegate.Combine(this.tmpG.OnSelectedChart, new EventHandler(this.SubSelected));
            this.tmpG.DisplaySub = false;
            this.tmpG.Dock = DockStyle.Fill;
            this.tmpG.Parent = this.tmpSub;
            this.tmpG.Height = this.tmpSub.Height;
            this.CreateSubGallery(this.tmpG, this.SelectedChart[0].GetType());
            this.tmpG.ShowSelectedChart();
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            if (this.tmpSub.Bounds.Right > bounds.Width)
            {
                this.tmpSub.Left = bounds.Width - this.tmpSub.Width;
            }
            if ((this.tmpSub.ShowDialog() == DialogResult.OK) && (this.OnSubSelected != null))
            {
                this.OnSubSelected(this, EventArgs.Empty);
            }
            this.tmpSub.Dispose();
            this.ShowSelectedChart();
        }

        internal void SubSelected(object sender, EventArgs e)
        {
            this.SelectedChart.Tag = this.tmpG.Charts.IndexOf(this.tmpG.SelectedChart);
            if (this.SubIndex == 0)
            {
                System.Type type = this.SelectedChart[0].GetType();
                int count = this.SelectedChart.Series.Count;
                this.SelectedChart.Series.Clear();
                for (int i = 1; i <= count; i++)
                {
                    Series.CreateNewSeries(this.SelectedChart.Chart, type, null);
                }
                this.SelectedChart[0].GalleryChanged3D(this.tmpG.View3D);
                for (int j = 0; j < this.SelectedChart.Series.Count; j++)
                {
                    this.SelectedChart[j].PrepareForGallery(true);
                }
            }
            else
            {
                foreach (Series series in this.SelectedChart.Series)
                {
                    this.SetSubSelected(series, this.SubIndex);
                }
            }
            if (this.tmpSub != null)
            {
                this.tmpSub.DialogResult = DialogResult.OK;
            }
        }

        private void tmpSub_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.tmpSub.DialogResult = DialogResult.Cancel;
            }
        }

        private bool ValidFunction(int AFunctionType, string APage)
        {
            return (GalleryPages((int) Utils.FunctionGalleryPage[AFunctionType]) == APage);
        }

        private bool ValidSeries(int ASeriesType, string APage)
        {
            return (GalleryPages(Utils.SeriesGalleryPage[ASeriesType]) == APage);
        }

        public static int ColorPaletteIndex
        {
            get
            {
                return colorPaletteIdx;
            }
            set
            {
                colorPaletteIdx = value;
            }
        }

        [DefaultValue(true), Description("Gets or sets if GalleryPanel will display a sub-gallery when clicking left-bottom corner small arrow.")]
        public bool DisplaySub
        {
            get
            {
                return this.displaySub;
            }
            set
            {
                this.displaySub = value;
            }
        }

        [Browsable(false)]
        public System.Type FunctionType
        {
            get
            {
                if ((this.SelectedChart != null) && (this.SelectedChart[0].Function != null))
                {
                    return this.SelectedChart[0].Function.GetType();
                }
                return null;
            }
        }

        public object[] InitArgs
        {
            get
            {
                return new object[] { this.SelectedChart.Tag };
            }
        }

        [DefaultValue(4)]
        public int NumCols
        {
            get
            {
                return this.numCols;
            }
            set
            {
                if (this.numCols != value)
                {
                    this.numCols = value;
                    this.ResizeCharts();
                }
            }
        }

        [DefaultValue(3)]
        public int NumRows
        {
            get
            {
                return this.numRows;
            }
            set
            {
                if (this.numRows != value)
                {
                    this.numRows = value;
                    this.ResizeCharts();
                }
            }
        }

        [Browsable(false)]
        public System.Type SeriesType
        {
            get
            {
                if (this.SelectedChart == null)
                {
                    return null;
                }
                return this.SelectedChart[0].GetType();
            }
        }

        [DefaultValue(2)]
        public SmoothingMode Smooth
        {
            get
            {
                return this.smooth;
            }
            set
            {
                if (this.smooth != value)
                {
                    this.smooth = value;
                    foreach (GalleryChart chart in this.Charts)
                    {
                        chart.Graphics3D.SmoothingMode = this.smooth;
                        chart.Invalidate();
                    }
                }
            }
        }

        public int SubIndex
        {
            get
            {
                if ((this.SelectedChart != null) && (this.SelectedChart.Tag != null))
                {
                    return (int) this.SelectedChart.Tag;
                }
                return 0;
            }
        }

        [DefaultValue(true)]
        public bool View3D
        {
            get
            {
                return this.view3D;
            }
            set
            {
                if (this.view3D != value)
                {
                    this.view3D = value;
                    foreach (GalleryChart chart in this.Charts)
                    {
                        chart[0].GalleryChanged3D(this.view3D);
                        chart.SetMargins();
                        chart.Invalidate();
                    }
                }
            }
        }

        [ToolboxItem(false)]
        public class GalleryChart : TChart
        {
            internal bool canDrawFrame;
            internal bool frameDisplayed;
            public static Color GalleryColor = Color.White;

            public GalleryChart(SmoothingMode smooth)
            {
                base.Legend.Visible = false;
                base.Axes.Left.Labels.Visible = false;
                base.Axes.Bottom.Labels.Visible = false;
                base.Zoom.Animated = true;
                base.Header.Font.Color = Color.Navy;
                base.Header.Bevel.Inner = BevelStyles.None;
                base.Header.Bevel.Outer = BevelStyles.None;
                base.Header.Transparent = true;
                base.Header.Pen.Visible = false;
                base.Aspect.Orthogonal = false;
                base.Aspect.Zoom = 90;
                base.Aspect.Chart3DPercent = 100;
                base.Aspect.ClipPoints = false;
                base.Graphics3D.SmoothingMode = smooth;
                base.Panel.Bevel.Width = 2;
                base.Panel.Bevel.Outer = BevelStyles.None;
                base.Walls.Left.Color = Color.White;
                base.Walls.Left.Size = 4;
                base.Walls.Left.Pen.Color = Color.DarkGray;
                base.Walls.Bottom.Color = Color.White;
                base.Walls.Bottom.Size = 4;
                base.Walls.Bottom.Pen.Color = Color.DarkGray;
                base.Walls.Back.Pen.Visible = false;
                base.Walls.Back.Visible = false;
                base.AfterDraw += new PaintChartEventHandler(this.DoAfterDraw);
            }

            internal void CheckShowLabels()
            {
                if (GalleryPanel.CheckMaximize && base.Axes.Visible)
                {
                    Form form = base.FindForm();
                    if (form != null)
                    {
                        base.Axes.Left.Labels.Visible = form.WindowState == FormWindowState.Maximized;
                        base.Axes.Bottom.Labels.Visible = base.Axes.Left.Labels.Visible;
                    }
                }
            }

            public void DoAfterDraw(object sender, Graphics3D g)
            {
                if (base.Panel.Gradient.Visible && ((GalleryPanel) base.Parent).DisplaySub)
                {
                    g.Brush.Solid = true;
                    g.Brush.Color = Color.Black;
                    g.Brush.Visible = true;
                    g.Brush.Gradient.Visible = false;
                    g.Pen.Visible = false;
                    Point[] p = new Point[] { new Point(4, (base.Height - 6) - 2), new Point(11, (base.Height - 6) - 2), new Point(7, base.Height - 4) };
                    g.Polygon(p);
                    g.Pen.Style = DashStyle.Solid;
                }
                this.canDrawFrame = true;
            }

            internal void DoFocus(bool Is3D)
            {
                base.Panel.Gradient.Visible = true;
                base.Panel.Gradient.Direction = LinearGradientMode.Vertical;
                base.Panel.Gradient.StartColor = Color.Silver;
                base.Panel.Gradient.EndColor = GalleryColor;
                base.Aspect.Rotation = 0x159;
                if (base.Series.Count > 0)
                {
                    base.Series[0].GalleryChanged3D(Is3D);
                }
                base.Header.Font.Bold = true;
                base.Header.Font.Color = Color.Black;
                base.Header.Font.Size = 9;
                base.Panel.Bevel.Outer = BevelStyles.Raised;
                base.Focus();
            }

            private void DrawFrame3D(bool erase)
            {
                if (((this.Cursor != Cursors.No) && this.canDrawFrame) && (!erase || this.frameDisplayed))
                {
                    ControlPaint.DrawReversibleFrame(base.RectangleToScreen(base.ClientRectangle), SystemColors.Control, FrameStyle.Thick);
                    this.frameDisplayed = !erase;
                }
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                base.OnKeyDown(e);
                ((GalleryPanel) base.Parent).OnKeyDown(e);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                base.OnMouseEnter(e);
                this.DrawFrame3D(false);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                this.DrawFrame3D(true);
            }

            internal void SetMargins()
            {
                int num = base.Aspect.View3D ? 4 : 5;
                base.Panel.MarginTop = num;
                base.Panel.MarginBottom = num;
                base.Panel.MarginLeft = num;
                base.Panel.MarginRight = num;
            }

            internal void UnFocus(bool Is3D)
            {
                if (base.Panel.Gradient.Visible)
                {
                    base.Panel.Gradient.Visible = false;
                    base.Panel.Bevel.Outer = BevelStyles.None;
                    base.Header.Font.Bold = false;
                    base.Header.Font.Color = Color.Navy;
                    base.Header.Font.Size = 8;
                }
                base.Aspect.Rotation = 0x14f;
                if (base.Series.Count > 0)
                {
                    base.Series[0].GalleryChanged3D(Is3D);
                }
            }
        }

        public class GalleryChartCollection : List<GalleryPanel.GalleryChart>
        {
            public void Clear()
            {
                foreach (GalleryPanel.GalleryChart chart in this)
                {
                    chart.Dispose();
                }
                base.Clear();
            }
        }
    }
}

