namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using Steema.TeeChart.Editors.Tools;
    using Steema.TeeChart.Editors.Series;

    public sealed class EditorUtils
    {
        private static System.Type[] aSeriesEditorsOf = new System.Type[] { 
            typeof(CustomSeries), typeof(PointSeries), typeof(AreaSeries), typeof(FastLineSeries), typeof(CustomSeries), typeof(BarSeries), typeof(BarSeries), typeof(PieSeries), typeof(ShapeSeries), typeof(ArrowSeries), typeof(PointSeries), typeof(GanttSeries), typeof(CandleSeries), typeof(DonutSeries), typeof(VolumeSeries), typeof(BarSeries), 
            typeof(Point3DSeries), typeof(PolarSeries), typeof(PolarSeries), typeof(PolarSeries), typeof(ClockSeries), typeof(PolarSeries), typeof(PyramidSeries), typeof(SurfaceSeries), typeof(PointSeries), typeof(BarSeries), typeof(ColorGridSeries), typeof(WaterfallSeries), typeof(HistogramSeries), typeof(ErrorSeries), typeof(ErrorSeries), typeof(ContourSeries), 
            typeof(SmithSeries), typeof(BezierSeries), typeof(CalendarSeries), typeof(HighLow), typeof(TriSurfaceSeries), typeof(FunnelSeries), typeof(BoxSeriesEditor), typeof(BoxSeriesEditor), typeof(AreaSeries), typeof(TowerSeries), typeof(PointFigure), typeof(GaugesSeries), typeof(Vector3DSeries), typeof(HistogramSeries), typeof(MapSeries), typeof(ImageBarSeries), 
            typeof(KagiSeries), typeof(RenkoEditor), typeof(IsoSurfaceEditor), typeof(DarvasEditor), typeof(VolumePipeEditor), typeof(PointSeries), typeof(CircularGaugeSeries), typeof(LinearGaugeSeries), typeof(LinearGaugeSeries), typeof(NumericGaugeSeries), typeof(OrgChartEditor), typeof(TagCloudSeries), typeof(PolarGridEditor)
         };
        private static System.Type[] aToolEditorsOf = new System.Type[] { 
            typeof(AnnotationEditor), typeof(ImageEditor), typeof(ExtraLegendEditor), typeof(GridBandEditor), typeof(GridTransposeEditor), typeof(MarksTipEditor), typeof(NearestPointEditor), typeof(AnnotationEditor), typeof(PieToolEditor), typeof(RotateEditor), typeof(ScrollBarEditor), typeof(SeriesAnimationEditor), typeof(SurfaceNearestToolEditor), typeof(CursorEditor), typeof(DragMarksEditor), typeof(AxisArrowEditor), 
            typeof(ColorLineEditor), typeof(ColorBandEditor), typeof(DrawLineEditor), typeof(DragPointEditor), typeof(GanttToolEditor), typeof(AxisScrollEditor), typeof(HotspotEditor), typeof(ZoomToolEditor), typeof(ScrollToolEditor), typeof(LightToolEditor), typeof(FibonacciToolEditor), typeof(SubChartEditor), typeof(AnnotationEditor), typeof(FaderToolEditor), typeof(AnnotationEditor), typeof(SelectorEditor), 
            typeof(SeriesRegionToolEditor), typeof(LegendPaletteEditor), typeof(SeriesStatsEditor), typeof(SeriesTransposeEditor), typeof(DataTableEditor), typeof(ClipSeriesEditor), typeof(AnnotationEditor), typeof(MagnifyEditor), typeof(SeriesBandToolEditor)
         };
        public static List<System.Type> SeriesEditorsOf = new List<System.Type>(aSeriesEditorsOf);
        public static ArrayList ToolEditorsOf = new ArrayList(aToolEditorsOf);

        private EditorUtils()
        {
        }

        internal static void AddDefaultValueFormats(IList aItems)
        {
            aItems.Add("#,##0.###");
            aItems.Add("0");
            aItems.Add("0.0");
            aItems.Add("0.#");
            aItems.Add("#.#");
            aItems.Add("#,##0.00;(#,##0.00)");
            aItems.Add("00e-0");
            aItems.Add("#.0 \"x10\" E+0");
            aItems.Add("#.# x10E-#");
        }

        public static void EditChartPart(Chart chart, ChartClickedPart part, bool showDefault)
        {
            chart.CancelMouse = true;
            switch (part.Part)
            {
                case ChartClickedPartStyle.Legend:
                {
                    Steema.TeeChart.Editors.LegendEditor c = new Steema.TeeChart.Editors.LegendEditor(chart, chart.Legend, null) {
                        Text = string.Format(Texts.Editing, Texts.Legend)
                    };
                    Translate(c);
                    ShowFormModal(c);
                    return;
                }
                case ChartClickedPartStyle.Axis:
                {
                    AxisEditor editor4 = new AxisEditor(part.AAxis, null);
                    Translate(editor4);
                    ShowFormModal(editor4);
                    return;
                }
                case ChartClickedPartStyle.Series:
                {
                    SeriesEditor editor5 = new SeriesEditor(part.ASeries, null) {
                        Text = string.Format(Texts.Editing, part.ASeries.Title)
                    };
                    Translate(editor5);
                    ShowFormModal(editor5);
                    return;
                }
                case ChartClickedPartStyle.Header:
                {
                    TitleEditor editor6 = new TitleEditor(chart.Header, null);
                    Translate(editor6);
                    ShowFormModal(editor6);
                    return;
                }
                case ChartClickedPartStyle.Foot:
                {
                    TitleEditor editor7 = new TitleEditor(chart.Footer, null);
                    Translate(editor7);
                    ShowFormModal(editor7);
                    return;
                }
                case ChartClickedPartStyle.ChartRect:
                {
                    Steema.TeeChart.Editors.WallEditor editor2 = new Steema.TeeChart.Editors.WallEditor(chart.Walls.Back, null);
                    Translate(editor2);
                    ShowFormModal(editor2);
                    return;
                }
                case ChartClickedPartStyle.SeriesMarks:
                {
                    SeriesMarksEditor editor8 = new SeriesMarksEditor(part.ASeries.Marks, null);
                    Translate(editor8);
                    ShowFormModal(editor8);
                    return;
                }
                case ChartClickedPartStyle.SubHeader:
                {
                    TitleEditor editor9 = new TitleEditor(chart.SubHeader, null);
                    Translate(editor9);
                    ShowFormModal(editor9);
                    return;
                }
                case ChartClickedPartStyle.SubFoot:
                {
                    TitleEditor editor10 = new TitleEditor(chart.SubFooter, null);
                    Translate(editor10);
                    ShowFormModal(editor10);
                    return;
                }
                case ChartClickedPartStyle.AxisTitle:
                {
                    TextEditor editor = new TextEditor(part.AAxis.Title.Font, null) {
                        Text = string.Format(Texts.Editing, Texts.AxisTitle)
                    };
                    Translate(editor);
                    ShowFormModal(editor);
                    return;
                }
            }
            if (showDefault)
            {
                ChartEditor.ShowModal(chart);
            }
        }

        public static bool EditFont(ChartFont font)
        {
            using (PrivateFontDialog dialog = new PrivateFontDialog(font))
            {
                bool flag = dialog.ShowDialog() == DialogResult.OK;
                if (flag)
                {
                    font.Name = dialog.Font.Name;
                    font.Color = dialog.Color;
                    font.Size = (int) dialog.Font.Size;
                    font.Bold = dialog.Font.Bold;
                    font.Italic = dialog.Font.Italic;
                    font.Underline = dialog.Font.Underline;
                    font.Strikeout = dialog.Font.Strikeout;
                }
                return flag;
            }
        }

        public static void FillCursors(ComboBox combo, Cursor cursor)
        {
            if (combo.Items.Count == 0)
            {
                foreach (PropertyInfo info in typeof(Cursors).GetProperties())
                {
                    if (info.PropertyType.Equals(typeof(Cursor)))
                    {
                        combo.Items.Add(info.Name);
                    }
                }
            }
            string str = cursor.ToString();
            int index = str.IndexOf(":");
            if (index > 0)
            {
                str = str.Remove(0, index + 2);
                index = str.IndexOf("]");
                if (index > 0)
                {
                    str = str.Substring(0, index);
                }
                combo.SelectedIndex = combo.Items.IndexOf(str);
            }
            else
            {
                combo.SelectedIndex = -1;
            }
        }

        internal static void GetUpDown(Button bUp, Button bDown)
        {
            string str = "Steema.TeeChart.Editors.Images.";
            bUp.Image = Utils.GetBitmapResource(str + "Up.bmp");
            bDown.Image = Utils.GetBitmapResource(str + "Down.bmp");
        }

        internal static int ImageModeToIndex(ImageMode mode)
        {
            if (mode == ImageMode.Stretch)
            {
                return 0;
            }
            if (mode == ImageMode.Tile)
            {
                return 1;
            }
            if (mode == ImageMode.Center)
            {
                return 2;
            }
            return 3;
        }

        internal static ImageMode IndexToImageMode(int index)
        {
            if (index == 0)
            {
                return ImageMode.Stretch;
            }
            if (index == 1)
            {
                return ImageMode.Tile;
            }
            if (index == 2)
            {
                return ImageMode.Center;
            }
            return ImageMode.Normal;
        }

        public static void InsertForm(Form f, Control c)
        {
            if (c != null)
            {
                f.TopLevel = false;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ControlBox = false;
                c.Controls.Add(f);
                f.Show();
            }
        }

        public static MouseButtons MouseButtonFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return MouseButtons.Left;

                case 1:
                    return MouseButtons.Middle;

                case 2:
                    return MouseButtons.Right;

                case 3:
                    return MouseButtons.XButton1;

                case 4:
                    return MouseButtons.XButton2;
            }
            return MouseButtons.None;
        }

        public static int MouseButtonIndex(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return 0;

                case MouseButtons.Right:
                    return 2;

                case MouseButtons.Middle:
                    return 1;

                case MouseButtons.XButton1:
                    return 3;

                case MouseButtons.XButton2:
                    return 4;
            }
            return -1;
        }

        public static bool ShowFormModal(Form f)
        {
            return ShowFormModal(f, null);
        }

        public static bool ShowFormModal(Form f, IWin32Window w)
        {
            using (DialogEditor editor = new DialogEditor())
            {
                editor.InsertForm(f);
                DialogResult result = editor.ShowDialog(w);
                f.Dispose();
                return (result == DialogResult.OK);
            }
        }

        public static Cursor StringToCursor(string s)
        {
            foreach (PropertyInfo info in typeof(Cursors).GetProperties())
            {
                if (info.PropertyType.Equals(typeof(Cursor)) && (info.Name == s))
                {
                    return (Cursor) info.GetValue(null, null);
                }
            }
            return Cursors.Default;
        }

        internal static Icon TChartIcon()
        {
            Bitmap bitmapResource = (Bitmap) Utils.GetBitmapResource("Steema.TeeChart.Images.TChart.ico");
            if (bitmapResource != null)
            {
                return Icon.FromHandle(bitmapResource.GetHicon());
            }
            return null;
        }

        public static void Translate(Control c)
        {
            if (Texts.Translator != null)
            {
                if (c is IStopComboBoxTranslate)
                {
                    ArrayList excludechildren = new ArrayList();
                    excludechildren.AddRange((c as IStopComboBoxTranslate).GetComboBoxes());
                    Texts.Translator.Translate(c, excludechildren);
                }
                else
                {
                    Texts.Translator.Translate(c);
                }
            }
        }

        public static void Translate(Control c, ArrayList ExcludeChildren)
        {
            if (Texts.Translator != null)
            {
                Texts.Translator.Translate(c, ExcludeChildren);
            }
        }

        [DesignTimeVisible(false), ToolboxItem(false)]
        public class PrivateFontDialog : FontDialog
        {
            private bool isPrivateFont;

            public PrivateFontDialog(ChartFont font)
            {
                this.isPrivateFont = font.UsePrivateFont;
                base.Font = font.DrawingFont;
                base.ShowColor = true;
                base.Color = font.Color;
            }

            protected override bool RunDialog(IntPtr hWndOwner)
            {
                bool flag = base.RunDialog(hWndOwner);
                if (flag && !Utils.IsPrivateFont(base.Font.Name))
                {
                    this.isPrivateFont = false;
                }
                return flag;
            }

            public bool IsPrivateFont
            {
                get
                {
                    return this.isPrivateFont;
                }
                set
                {
                    this.isPrivateFont = value;
                }
            }
        }
    }
}

