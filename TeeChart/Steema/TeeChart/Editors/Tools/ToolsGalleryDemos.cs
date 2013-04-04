namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class ToolsGalleryDemos : System.Windows.Forms.Panel
    {
        private TChart chart;

        public ToolsGalleryDemos()
        {
            this.BackColor = Color.FromArgb(0xc4, 0xc4, 0xc4);
            base.BorderStyle = BorderStyle.FixedSingle;
        }

        private Button AddButton(string AText, EventHandler AEvent)
        {
            Button button = this.AddControl(typeof(Button), AText) as Button;
            button.Click += AEvent;
            return button;
        }

        private CheckBox AddCheck(string AText, EventHandler AEvent)
        {
            CheckBox box = this.AddControl(typeof(CheckBox), AText) as CheckBox;
            box.Click += AEvent;
            return box;
        }

        private Control AddControl(System.Type AClass, string AText)
        {
            Control control = Activator.CreateInstance(AClass) as Control;
            control.Left = 20;
            control.Top = this.chart.Height - 50;
            control.Text = AText;
            control.Width = Math.Max(control.Width, AText.Length * 6);
            control.Tag = this.chart.Tools[0];
            this.chart.Controls.Add(control);
            return control;
        }

        private void AddSeries(TChart chart, System.Type seriesType)
        {
            chart.Series.Add(seriesType).FillSampleValues();
        }

        private void Animation(object sender, EventArgs e)
        {
            ((sender as Button).Tag as SeriesAnimation).Execute();
        }

        private void BannerBlinkChecked(object sender, EventArgs e)
        {
            ((sender as CheckBox).Tag as BannerTool).Blink = (sender as CheckBox).Checked;
        }

        private void BannerScrollChecked(object sender, EventArgs e)
        {
            ((sender as CheckBox).Tag as BannerTool).Scroll = (sender as CheckBox).Checked;
        }

        public void chart_DoubleClick(object sender, EventArgs e)
        {
        }

        public void CreateChart(System.Type ATool)
        {
            this.CreateChart(ATool, "", null);
        }

        public void CreateChart(System.Type ATool, string ATitle)
        {
            this.CreateChart(ATool, ATitle, null);
        }

        public void CreateChart(System.Type ATool, string ATitle, System.Type ASeries)
        {
            this.chart = new TChart();
            this.chart.Dock = DockStyle.Fill;
            this.chart.DoubleClick += new EventHandler(this.chart_DoubleClick);
            this.chart.Legend.Visible = false;
            this.chart.Header.Text = "";
            base.Controls.Clear();
            base.Controls.Add(this.chart);
            if (ASeries == null)
            {
                this.chart.Series.Add(new Line());
            }
            else
            {
                this.chart.Series.Add(ASeries);
            }
            this.chart[0].FillSampleValues();
            this.chart.Header.Text = ATitle;
            if (ATool != null)
            {
                this.chart.Tools.Add(ATool);
            }
            new WebTheme(this.chart.Chart).Apply();
            base.BorderStyle = BorderStyle.None;
        }

        public void CreateGallery(System.Type tool)
        {
            double minimum;
            int num2;
            switch (Utils.ToolTypeIndex(tool))
            {
                case 0:
                    this.CreateChart(tool);
                    (this.chart.Tools[0] as Annotation).Text = "This is an Annotation tool";
                    this.chart.Tools.Add(new Annotation());
                    (this.chart.Tools[1] as Annotation).Text = "Another Annotation";
                    (this.chart.Tools[1] as Annotation).Left = 150;
                    (this.chart.Tools[1] as Annotation).Top = 80;
                    (this.chart.Tools[1] as Annotation).Shape.Font.Size = 0x12;
                    (this.chart.Tools[1] as Annotation).Shape.Gradient.Visible = true;
                    return;

                case 1:
                {
                    this.CreateChart(tool, "Drag chart to scroll axes and image");
                    this.chart.Aspect.View3D = false;
                    this.chart[0].Color = Color.Green;
                    (this.chart.Tools[0] as ChartImage).Series = this.chart[0];
                    using (BrushEditor editor = new BrushEditor())
                    {
                        Image image = editor.brushImages.Images[1];
                        (this.chart.Tools[0] as ChartImage).Image = image;
                        break;
                    }
                }
                case 2:
                    this.CreateChart(tool, "Show additional Legend panels", typeof(Bar));
                    this.chart.Series.Add(typeof(Bar));
                    this.chart[1].FillSampleValues();
                    this.chart.Legend.Visible = true;
                    this.chart.Legend.LegendStyle = LegendStyles.Values;
                    this.chart.Legend.Title.Visible = true;
                    this.chart.Legend.Title.Font = this.chart.Legend.Font.Clone() as ChartFont;
                    this.chart.Legend.Title.Font.Bold = true;
                    this.chart.Legend.Title.Text = Texts.Legend;
                    this.chart.Legend.Title.Transparent = true;
                    (this.chart.Tools[0] as ExtraLegend).Series = this.chart[1];
                    (this.chart.Tools[0] as ExtraLegend).Legend.Title.Visible = true;
                    (this.chart.Tools[0] as ExtraLegend).Legend.Title.Text = "Extra Legend";
                    (this.chart.Tools[0] as ExtraLegend).Legend.Left = this.chart.Width - 200;
                    (this.chart.Tools[0] as ExtraLegend).Legend.Top = 100;
                    return;

                case 3:
                    this.CreateChart(tool, "Two bands fill axes grid lines space");
                    (this.chart.Tools[0] as GridBand).Axis = this.chart.Axes.Left;
                    (this.chart.Tools[0] as GridBand).Band1.Color = RandomTheme.RandomColor;
                    (this.chart.Tools[0] as GridBand).Band2.Color = RandomTheme.RandomColor;
                    return;

                case 4:
                    this.CreateChart(tool, "Swaps 3D Series data, rows by columns", typeof(Surface));
                    (this.chart[0] as Surface).HideCells = true;
                    (this.chart.Tools[0] as GridTranspose).Series = this.chart[0] as Surface;
                    this.chart.Aspect.Orthogonal = false;
                    this.chart.Aspect.Perspective = 100;
                    this.chart.Aspect.Chart3DPercent = 0x4b;
                    this.chart.Aspect.Zoom = 0x4b;
                    this.chart.Axes.Depth.Visible = true;
                    this.chart.Tools.Add(typeof(Rotate));
                    this.AddButton("&Transpose", new EventHandler(this.Transpose3DSeries));
                    return;

                case 5:
                    this.CreateChart(tool, "Move mouse over Series points to display hints");
                    this.chart[0].FillSampleValues(5);
                    (this.chart[0] as Line).Pointer.Visible = true;
                    (this.chart.Tools[0] as MarksTip).Series = this.chart[0];
                    (this.chart.Tools[0] as MarksTip).MouseAction = MarksTipMouseAction.Move;
                    this.chart[0].Cursor = Cursors.Hand;
                    this.chart.Axes.Bottom.MaximumOffset = 20;
                    this.chart.Axes.Bottom.MinimumOffset = 20;
                    return;

                case 6:
                    this.CreateChart(tool, "Move the mouse over Series points");
                    this.chart[0].FillSampleValues(8);
                    (this.chart[0] as Line).Pointer.Visible = true;
                    (this.chart.Tools[0] as NearestPoint).Series = this.chart[0];
                    (this.chart.Tools[0] as NearestPoint).Pen.Width = 2;
                    (this.chart.Tools[0] as NearestPoint).Pen.Color = Color.Blue;
                    return;

                case 7:
                    this.CreateChart(tool, "Shows Page numbering", typeof(Bar));
                    this.chart[0].FillSampleValues(0x19);
                    this.chart.Page.MaxPointsPerPage = 5;
                    return;

                case 8:
                    this.CreateChart(tool, "Move mouse over Pie slices", typeof(Pie));
                    this.chart.Aspect.Chart3DPercent = 70;
                    (this.chart.Tools[0] as PieTool).Series = this.chart[0];
                    (this.chart[0] as Pie).Pen.Color = Color.DarkGray;
                    return;

                case 9:
                    this.CreateChart(tool, "Click and drag to rotate");
                    this.chart.Aspect.Orthogonal = false;
                    this.chart.Aspect.Perspective = 100;
                    this.chart.Aspect.Chart3DPercent = 0x4b;
                    this.AddCheck("OutLine", new EventHandler(this.OutlineChecked)).Left = 4;
                    return;

                case 10:
                    this.CreateChart(tool, "Displays scrollbar at Legend");
                    this.chart.Legend.Visible = true;
                    this.chart[0].FillSampleValues(50);
                    (this.chart.Tools[0] as LegendScrollBar).Gradient.Visible = true;
                    (this.chart.Tools[0] as LegendScrollBar).ThumbBrush.Gradient.Visible = true;
                    (this.chart.Tools[0] as LegendScrollBar).ThumbBrush.Gradient.StartColor = Color.Red;
                    (this.chart.Tools[0] as LegendScrollBar).ThumbBrush.Gradient.EndColor = Color.Green;
                    return;

                case 11:
                    this.CreateChart(tool, "Series points display animated", typeof(Bar));
                    this.chart[0].ColorEach = true;
                    (this.chart.Tools[0] as SeriesAnimation).Series = this.chart[0];
                    (this.chart.Tools[0] as SeriesAnimation).StartValue = 0.0;
                    (this.chart.Tools[0] as SeriesAnimation).StartAtMin = false;
                    (this.chart.Tools[0] as SeriesAnimation).Steps = 100;
                    (this.chart.Tools[0] as SeriesAnimation).DrawEvery = 1;
                    this.AddButton("&Animate!", new EventHandler(this.Animation));
                    return;

                case 12:
                    this.CreateChart(tool, "Move mouse over Surface to highlight cells", typeof(Surface));
                    this.chart.Aspect.Orthogonal = false;
                    this.chart.Aspect.Perspective = 100;
                    this.chart.Aspect.Chart3DPercent = 0x4b;
                    this.chart.Aspect.Zoom = 0x4b;
                    (this.chart[0] as Surface).HideCells = true;
                    this.chart[0].FillSampleValues(20);
                    (this.chart.Tools[0] as SurfaceNearestTool).Series = this.chart[0];
                    this.chart.Tools.Add(typeof(Rotate));
                    this.chart.Walls.Left.Visible = false;
                    this.chart.Walls.Back.Visible = false;
                    return;

                case 13:
                    this.CreateChart(tool, "Click and drag cursor lines");
                    return;

                case 14:
                    this.CreateChart(tool, "Click Series Marks to drag");
                    this.chart[0].FillSampleValues(5);
                    this.chart[0].Marks.Visible = true;
                    this.chart[0].Marks.Font.Size = 14;
                    this.chart[0].Marks.Callout.Visible = true;
                    this.chart[0].Marks.Callout.Arrow.Color = Color.Black;
                    (this.chart.Tools[0] as DragMarks).Series = this.chart[0];
                    this.chart.Axes.Left.MaximumOffset = 20;
                    this.chart.Axes.Left.MinimumOffset = 20;
                    this.chart.Axes.Bottom.MaximumOffset = 20;
                    this.chart.Axes.Bottom.MinimumOffset = 20;
                    return;

                case 15:
                    this.CreateChart(tool, "Click axes arrow to scroll");
                    this.chart.Aspect.View3D = false;
                    (this.chart.Tools[0] as AxisArrow).Length = 60;
                    (this.chart.Tools[0] as AxisArrow).Axis = this.chart.Axes.Bottom;
                    return;

                case 0x10:
                    this.CreateChart(tool, "Several Color lines displayed\nat random positions");
                    minimum = this.chart[0].mandatory.Minimum;
                    num2 = Utils.Round(this.chart[0].mandatory.Range);
                    this.RandomColorLine(this.chart.Tools[0] as ColorLine, ref minimum, num2);
                    this.chart.Tools.Add(typeof(ColorLine));
                    this.RandomColorLine(this.chart.Tools[1] as ColorLine, ref minimum, num2);
                    this.chart.Tools.Add(typeof(ColorLine));
                    this.RandomColorLine(this.chart.Tools[2] as ColorLine, ref minimum, num2);
                    return;

                case 0x11:
                {
                    this.CreateChart(tool, "Color bands to fill axes background");
                    this.chart.Aspect.View3D = false;
                    minimum = this.chart[0].mandatory.Minimum;
                    num2 = Utils.Round(this.chart[0].mandatory.Range);
                    this.RandomColorBand(this.chart.Tools[0] as ColorBand, ref minimum, num2);
                    int num3 = this.chart.Tools.Add(typeof(ColorBand));
                    this.RandomColorBand(this.chart.Tools[num3] as ColorBand, ref minimum, num2);
                    num3 = this.chart.Tools.Add(typeof(ColorBand));
                    this.RandomColorBand(this.chart.Tools[num3] as ColorBand, ref minimum, num2);
                    return;
                }
                case 0x12:
                {
                    this.CreateChart(tool, "Click and drag to draw lines");
                    minimum = (this.chart[0].mandatory.minimum + this.chart[0].mandatory.maximum) * 0.5;
                    DrawLineItem item = new DrawLineItem(this.chart.Tools[0] as DrawLine) {
                        StartPos = new PointDouble(1.5, minimum),
                        EndPos = new PointDouble(2.8, minimum - (minimum * 0.5))
                    };
                    (this.chart.Tools[0] as DrawLine).Selected = item;
                    return;
                }
                case 0x13:
                    this.CreateChart(tool, "Click and drag Series points", typeof(Points));
                    this.chart[0].FillSampleValues(10);
                    this.chart[0].ColorEach = true;
                    this.chart[0].Cursor = Cursors.Hand;
                    (this.chart.Tools[0] as DragPoint).Series = this.chart[0];
                    return;

                case 20:
                    this.CreateChart(tool, "Click Gantt bars to drag and resize", typeof(Gantt));
                    this.chart.Aspect.View3D = false;
                    this.chart.Zoom.Allow = false;
                    this.chart[0].FillSampleValues(10);
                    (this.chart[0] as Gantt).Pointer.VertSize = 10;
                    (this.chart.Tools[0] as GanttTool).Series = this.chart[0];
                    return;

                case 0x15:
                    this.CreateChart(tool, "Drag axes to scroll");
                    this.chart.Aspect.View3D = false;
                    (this.chart.Tools[0] as AxisScroll).Axis = this.chart.Axes.Left;
                    this.chart.Tools.Add(typeof(AxisScroll));
                    (this.chart.Tools[1] as AxisScroll).Axis = this.chart.Axes.Bottom;
                    return;

                case 0x16:
                    this.CreateChart(typeof(Annotation), "Series Hotspot");
                    (this.chart.Tools[0] as Annotation).Text = "No preview is currently available";
                    (this.chart.Tools[0] as Annotation).Left = 150;
                    (this.chart.Tools[0] as Annotation).Top = 80;
                    this.chart.Series.Clear(true);
                    return;

                case 0x17:
                    this.CreateChart(typeof(Annotation), "Zoom Tool");
                    (this.chart.Tools[0] as Annotation).Text = "No preview is currently available";
                    (this.chart.Tools[0] as Annotation).Left = 150;
                    (this.chart.Tools[0] as Annotation).Top = 80;
                    this.chart.Series.Clear(true);
                    return;

                case 0x18:
                    this.CreateChart(typeof(Annotation), "Scroll Tool");
                    (this.chart.Tools[0] as Annotation).Text = "No preview is currently available";
                    (this.chart.Tools[0] as Annotation).Left = 150;
                    (this.chart.Tools[0] as Annotation).Top = 80;
                    this.chart.Series.Clear(true);
                    return;

                case 0x19:
                    this.CreateChart(tool, "2D Lighting");
                    (this.chart.Tools[0] as LightTool).FollowMouse = true;
                    (this.chart.Tools[0] as LightTool).Style = LightStyle.SpotLight;
                    this.chart.Tools.Add(typeof(Rotate));
                    return;

                case 0x1a:
                    this.CreateChart(tool, "Financial Data Analysis", typeof(Candle));
                    this.chart[0].FillSampleValues(40);
                    (this.chart.Tools[0] as FibonacciTool).Series = this.chart[0];
                    (this.chart.Tools[0] as FibonacciTool).StartX = (this.chart[0] as Candle).DateValues[0];
                    (this.chart.Tools[0] as FibonacciTool).StartY = (this.chart[0] as Candle).CloseValues[0];
                    (this.chart.Tools[0] as FibonacciTool).EndX = (this.chart[0] as Candle).DateValues[10];
                    (this.chart.Tools[0] as FibonacciTool).EndY = (this.chart[0] as Candle).CloseValues[10];
                    return;

                case 0x1b:
                    this.CreateChart(tool, "Multiple Charts inside a Chart");
                    this.chart[0].Clear();
                    this.chart.Axes.Left.Grid.Visible = false;
                    this.chart.Walls.Visible = false;
                    this.AddSeries((this.chart.Tools[0] as SubChartTool).Charts.AddChart("Chart1"), typeof(Line));
                    this.AddSeries((this.chart.Tools[0] as SubChartTool).Charts.AddChart("Chart2"), typeof(Bar));
                    (this.chart.Tools[0] as SubChartTool).Charts[1].Left = 250;
                    (this.chart.Tools[0] as SubChartTool).Charts[1].Chart[0].Color = Color.Green;
                    ((this.chart.Tools[0] as SubChartTool).Charts[1].Chart[0] as Bar).Pen.Color = Color.Green;
                    this.AddSeries((this.chart.Tools[0] as SubChartTool).Charts.AddChart("Chart3"), typeof(Pie));
                    (this.chart.Tools[0] as SubChartTool).Charts[2].Top = 200;
                    (this.chart.Tools[0] as SubChartTool).Charts[2].Chart[0].Marks.Visible = false;
                    this.AddSeries((this.chart.Tools[0] as SubChartTool).Charts.AddChart("Chart4"), typeof(Area));
                    (this.chart.Tools[0] as SubChartTool).Charts[3].Top = 200;
                    (this.chart.Tools[0] as SubChartTool).Charts[3].Left = 250;
                    (this.chart.Tools[0] as SubChartTool).Charts[3].Chart[0].Color = Color.Blue;
                    return;

                case 0x1c:
                    break;

                case 0x1d:
                    this.CreateChart(tool, "Fade in/out a Chart.");
                    (this.chart.Tools[0] as FaderTool).Color = this.chart.Panel.Color;
                    (this.chart.Tools[0] as FaderTool).Speed = 2.0;
                    this.AddButton("&Fade", new EventHandler(this.FadeChart));
                    return;

                case 30:
                    this.CreateChart(tool, "Annotations that can be\ndragged and resized");
                    (this.chart.Tools[0] as RectangleTool).AutoSize = false;
                    (this.chart.Tools[0] as RectangleTool).Width = 120;
                    (this.chart.Tools[0] as RectangleTool).Height = 60;
                    (this.chart.Tools[0] as RectangleTool).Text = "Drag and\nresize me";
                    (this.chart.Tools[0] as RectangleTool).Shape.Color = Color.Red;
                    (this.chart.Tools[0] as RectangleTool).Shape.Font.Size = 0x10;
                    (this.chart.Tools[0] as RectangleTool).Shape.Transparency = 30;
                    (this.chart.Tools[0] as RectangleTool).Shape.Left = 80;
                    (this.chart.Tools[0] as RectangleTool).Shape.Top = 90;
                    return;

                case 0x1f:
                {
                    this.CreateChart(tool, "Click to select Chart items", typeof(Bar));
                    this.chart.Legend.Visible = true;
                    this.chart[0].FillSampleValues(4);
                    this.chart.Series.Add(typeof(Bar)).FillSampleValues(4);
                    this.chart.Tools.Add(typeof(Annotation));
                    (this.chart.Tools[1] as Annotation).Text = "Click me!";
                    (this.chart.Tools[1] as Annotation).Position = AnnotationPositions.LeftBottom;
                    this.chart.Header.Transparent = false;
                    this.chart.Footer.Visible = true;
                    this.chart.Footer.Text = "Footer text";
                    this.chart.Footer.Transparent = false;
                    ChartClickedPart part = (this.chart.Tools[0] as Selector).Part;
                    return;
                }
                case 0x20:
                    this.CreateChart(tool, "Fills region between series and value.", typeof(Line));
                    this.chart[0].FillSampleValues(10);
                    (this.chart.Tools[0] as SeriesRegionTool).Series = this.chart[0];
                    (this.chart.Tools[0] as SeriesRegionTool).Transparency = 30;
                    (this.chart.Tools[0] as SeriesRegionTool).Brush.Color = Color.BurlyWood;
                    (this.chart.Tools[0] as SeriesRegionTool).AutoBound = false;
                    (this.chart.Tools[0] as SeriesRegionTool).LowerBound = this.chart[0].XValues[3];
                    (this.chart.Tools[0] as SeriesRegionTool).UpperBound = this.chart[0].XValues[7];
                    (this.chart.Tools[0] as SeriesRegionTool).UseOrigin = false;
                    this.AddCheck("Draw behind", new EventHandler(this.DrawBehindSeriesChecked)).Checked = true;
                    return;

                case 0x21:
                    this.CreateChart(tool, "Displays legend of Series color palettes.", typeof(Surface));
                    this.chart.Aspect.Orthogonal = false;
                    this.chart.Aspect.Perspective = 100;
                    this.chart.Aspect.Chart3DPercent = 0x4b;
                    this.chart.Aspect.Zoom = 0x4b;
                    this.chart.Panel.MarginLeft = 20.0;
                    (this.chart[0] as Surface).HideCells = true;
                    (this.chart[0] as Surface).PaletteStyle = PaletteStyles.Rainbow;
                    this.chart[0].FillSampleValues(20);
                    (this.chart.Tools[0] as LegendPalette).Series = this.chart[0];
                    (this.chart.Tools[0] as LegendPalette).Top = 100;
                    (this.chart.Tools[0] as LegendPalette).Smooth = true;
                    this.chart.Axes.Left.PositionUnits = PositionUnits.Pixels;
                    this.chart.Axes.Left.RelativePosition = 2.0;
                    this.chart.Axes.Right.PositionUnits = PositionUnits.Pixels;
                    this.chart.Axes.Right.RelativePosition = -2.0;
                    return;

                case 0x22:
                {
                    this.CreateChart(tool, "Calculates series statistics", typeof(Line));
                    this.chart[0].FillSampleValues(10);
                    (this.chart.Tools[0] as SeriesStats).Series = this.chart[0];
                    this.chart.Tools.Add(typeof(Annotation));
                    Annotation annotation = this.chart.Tools[1] as Annotation;
                    annotation.Shape.Transparency = 10;
                    annotation.Left = 80;
                    annotation.Top = 50;
                    annotation.Text = (this.chart.Tools[0] as SeriesStats).Statistics;
                    return;
                }
                case 0x23:
                    this.CreateChart(tool, "Swap series rows and columns.", typeof(Bar));
                    this.chart.Aspect.Chart3DPercent = 0x4b;
                    this.chart[0].FillSampleValues();
                    (this.chart[0] as Bar).MultiBar = MultiBars.None;
                    this.AddButton("Transpose", new EventHandler(this.SeriesTranspose));
                    return;

                case 0x24:
                    this.CreateChart(tool, "Displays a grid with Series data.", typeof(Bar));
                    this.chart.Series.Add(typeof(Bar));
                    this.chart[0].FillSampleValues(4);
                    this.chart[1].FillSampleValues(4);
                    this.chart[0].ValueFormat = "#.00";
                    this.chart[1].ValueFormat = this.chart[0].ValueFormat;
                    return;

                case 0x25:
                    this.CreateChart(tool, "Drag chart to show series\r\nthat does not display outside axes", typeof(Line));
                    this.chart.Aspect.view3D = false;
                    this.chart.Panning.Allow = ScrollModes.Both;
                    this.chart.Axes.Left.StartPosition = 20.0;
                    this.chart.Axes.Left.EndPosition = 80.0;
                    this.chart.Axes.Bottom.StartPosition = 25.0;
                    this.chart.Axes.Bottom.EndPosition = 70.0;
                    (this.chart.Tools[0] as ClipSeries).Series = this.chart[0];
                    return;

                case 0x26:
                {
                    this.CreateChart(tool, "Text scrolling and/or\r\n blinking.");
                    (this.chart.Tools[0] as BannerTool).Text = "Scrolling text";
                    this.AddCheck("Blinking", new EventHandler(this.BannerBlinkChecked));
                    CheckBox box = this.AddCheck("Scrolling", new EventHandler(this.BannerScrollChecked));
                    box.Top -= 20;
                    box.Checked = true;
                    return;
                }
                case 0x27:
                    this.CreateChart(tool, "Magnify a Chart portion under the mouse.");
                    (this.chart.Tools[0] as Magnify).Width = 100;
                    (this.chart.Tools[0] as Magnify).Height = 100;
                    return;

                case 40:
                    this.CreateChart(tool, "Fill the region between two Series.");
                    this.chart.Aspect.View3D = false;
                    this.chart[0].FillSampleValues();
                    this.chart.Series.Add(typeof(Line));
                    for (int i = 0; i < this.chart[0].Count; i++)
                    {
                        this.chart[1].Add(this.chart[0].XValues[i], (double) (this.chart[0].YValues[i] / 2.0));
                    }
                    (this.chart[0] as Line).LinePen.Width = 3;
                    (this.chart[1] as Line).LinePen.Width = 3;
                    (this.chart.Tools[0] as SeriesBandTool).Series = this.chart[0];
                    (this.chart.Tools[0] as SeriesBandTool).Series2 = this.chart[1];
                    (this.chart.Tools[0] as SeriesBandTool).Gradient.Visible = true;
                    (this.chart.Tools[0] as SeriesBandTool).Gradient.StartColor = Color.Silver;
                    (this.chart.Tools[0] as SeriesBandTool).Pen.Visible = true;
                    (this.chart.Tools[0] as SeriesBandTool).DrawBehindSeries = false;
                    break;

                default:
                    return;
            }
        }

        private void DrawBehindSeriesChecked(object sender, EventArgs e)
        {
            ((sender as CheckBox).Tag as SeriesRegionTool).DrawBehindSeries = (sender as CheckBox).Checked;
        }

        private void FadeChart(object sender, EventArgs e)
        {
            ((sender as Button).Tag as FaderTool).Start();
        }

        private void OutlineChecked(object sender, EventArgs e)
        {
            ((sender as CheckBox).Tag as Rotate).Pen.Visible = (sender as CheckBox).Checked;
        }

        private void RandomColorBand(ColorBand tool, ref double tmpY, int tmpRange)
        {
            Random random = new Random();
            tool.Axis = this.chart.Axes.Left;
            tool.Start = tmpY;
            tool.End = tool.Start + random.Next(tmpRange);
            tmpY = tool.End;
            tool.Color = RandomTheme.RandomColor;
            tool.ResizeEnd = true;
            tool.ResizeStart = true;
        }

        private void RandomColorLine(ColorLine tool, ref double tmpY, int tmpRange)
        {
            Random random = new Random();
            tool.Axis = this.chart.Axes.Left;
            tool.Value = tmpY + random.Next(tmpRange);
            tmpY = tool.Value;
            tool.Pen.Color = RandomTheme.RandomColor;
            tool.Pen.Width = 2;
        }

        private void SeriesTranspose(object sender, EventArgs e)
        {
            ((sender as Button).Tag as Steema.TeeChart.Tools.SeriesTranspose).Transpose();
        }

        private void Transpose3DSeries(object sender, EventArgs e)
        {
            ((sender as Button).Tag as GridTranspose).Transpose();
        }

        public bool View3D
        {
            get
            {
                return this.chart.Aspect.View3D;
            }
            set
            {
                this.chart.Aspect.View3D = value;
            }
        }
    }
}

