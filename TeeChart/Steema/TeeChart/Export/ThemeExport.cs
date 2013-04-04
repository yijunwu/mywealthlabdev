namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using System;
    using System.ComponentModel;
    using System.IO;

    public class ThemeExport : ExportFormat
    {
        private Chart chart;
        private bool saveWithBase64;

        public ThemeExport(Chart c)
        {
            this.chart = c;
            base.FileExtension = "xml";
            this.saveWithBase64 = false;
        }

        internal static string FileFilter()
        {
            return (Texts.XMLFile + " (" + Texts.XMLFilter + ")|*." + Texts.XMLFilter);
        }

        internal override string FilterFiles()
        {
            return (Texts.XMLFile + " (" + Texts.XMLFilter + ")|*." + Texts.XMLFilter);
        }

        public void Save(Stream stream)
        {
            this.Serialize(stream);
        }

        public void Save(string FileName)
        {
            using (FileStream stream = new FileStream(FileName, FileMode.Create))
            {
                this.Save(stream);
            }
        }

        private void Serialize(Stream stream)
        {
            ThemeProperties instance = ThemeProperties.Instance;
            instance.AspectSmoothingMode = this.chart.Aspect.SmoothingMode.ToString();
            instance.AspectTextRenderingHint = this.chart.Aspect.TextRenderingHint.ToString();
            instance.AxesBottomGridCentered = this.chart.Axes.Bottom.Grid.Centered;
            instance.AxesBottomGridVisible = this.chart.Axes.Bottom.Grid.Visible;
            instance.AxesTopGridVisible = this.chart.Axes.Top.Grid.Visible;
            Axis left = this.chart.Axes.Left;
            instance.AxisAxisPenColor = Utils.ColorToHex(left.AxisPen.Color);
            instance.AxisAxisPenWidth = left.AxisPen.Width;
            instance.AxisGridCentered = left.Grid.Centered;
            instance.AxisGridColor = Utils.ColorToHex(left.Grid.Color);
            instance.AxisGridStyle = left.Grid.Style.ToString();
            instance.AxisGridVisible = left.Grid.Visible;
            instance.AxisGridWidth = left.Grid.Width;
            ChartFont font = left.Labels.Font;
            instance.AxisLabelsFontBold = font.Bold;
            instance.AxisLabelsFontColor = Utils.ColorToHex(font.Color);
            instance.AxisLabelsFontName = font.Name;
            instance.AxisLabelsFontSize = font.Size;
            ChartPen minorGrid = left.MinorGrid;
            instance.AxisMinorGridColor = Utils.ColorToHex(minorGrid.Color);
            instance.AxisMinorGridVisible = minorGrid.Visible;
            instance.AxisMinorTickCount = left.MinorTickCount;
            instance.AxisMinorTicksLength = left.MinorTicks.Length;
            instance.AxisMinorTicksVisible = left.MinorTicks.Visible;
            instance.AxisTicksColor = Utils.ColorToHex(left.Ticks.Color);
            instance.AxisTicksInnerColor = Utils.ColorToHex(left.TicksInner.Color);
            instance.AxisTicksInnerLength = left.TicksInner.Length;
            instance.AxisTicksInnerVisible = left.TicksInner.Visible;
            instance.AxisTicksLength = left.Ticks.Length;
            instance.AxisTitleFontColor = Utils.ColorToHex(left.Title.Font.Color);
            instance.AxisTitleFontName = left.Title.Font.Name;
            Header header = this.chart.Header;
            instance.HeaderColor = Utils.ColorToHex(header.Color);
            instance.HeaderFontBold = header.Font.Bold;
            instance.HeaderFontColor = Utils.ColorToHex(header.Font.Color);
            instance.HeaderFontName = header.Font.Name;
            instance.HeaderFontSize = header.Font.Size;
            Gradient gradient = header.Gradient;
            instance.HeaderGradientDirection = gradient.Direction.ToString();
            instance.HeaderGradientEndColor = Utils.ColorToHex(gradient.EndColor);
            instance.HeaderGradientMiddleColor = Utils.ColorToHex(gradient.MiddleColor);
            instance.HeaderGradientSigma = gradient.Sigma;
            instance.HeaderGradientSigmaFocus = gradient.SigmaFocus;
            instance.HeaderGradientSigmaScale = gradient.SigmaScale;
            instance.HeaderGradientStartColor = Utils.ColorToHex(gradient.StartColor);
            instance.HeaderGradientVisible = gradient.Visible;
            instance.HeaderPenColor = Utils.ColorToHex(header.Pen.Color);
            instance.HeaderPenVisible = header.Pen.Visible;
            instance.HeaderPenWidth = header.Pen.Width;
            instance.HeaderShadowSize = header.Shadow.Size.Height;
            instance.HeaderShadowTransparency = header.Shadow.Transparency;
            instance.HeaderTransparency = header.Transparency;
            Legend legend = this.chart.Legend;
            instance.LegendDividingLinesColor = Utils.ColorToHex(legend.DividingLines.Color);
            instance.LegendDividingLinesVisible = legend.DividingLines.Visible;
            instance.LegendFontColor = Utils.ColorToHex(legend.Font.Color);
            instance.LegendFontName = legend.Font.Name;
            instance.LegendFontSize = legend.Font.Size;
            gradient = legend.Gradient;
            instance.LegendGradientDirection = gradient.Direction.ToString();
            instance.LegendGradientEndColor = Utils.ColorToHex(gradient.EndColor);
            instance.LegendGradientMiddleColor = Utils.ColorToHex(gradient.MiddleColor);
            instance.LegendGradientSigma = gradient.Sigma;
            instance.LegendGradientSigmaFocus = gradient.SigmaFocus;
            instance.LegendGradientSigmaScale = gradient.SigmaScale;
            instance.LegendGradientStartColor = Utils.ColorToHex(gradient.StartColor);
            instance.LegendGradientVisible = gradient.Visible;
            minorGrid = legend.Pen;
            instance.LegendPenColor = Utils.ColorToHex(minorGrid.Color);
            instance.LegendPenStyle = minorGrid.Style.ToString();
            instance.LegendPenVisible = minorGrid.Visible;
            instance.LegendPenWidth = minorGrid.Width;
            instance.LegendShadowColor = Utils.ColorToHex(legend.Shadow.Color);
            instance.LegendShadowHeight = legend.Shadow.Height;
            instance.LegendShadowTransparency = legend.Shadow.Transparency;
            instance.LegendShadowWidth = legend.Shadow.Width;
            instance.LegendSymbolDefaultPen = legend.Symbol.DefaultPen;
            instance.LegendSymbolPenVisible = legend.Symbol.Pen.Visible;
            instance.LegendSymbolSquared = legend.Symbol.Squared;
            instance.LegendTransparent = legend.Transparent;
            instance.Palette = Utils.ColorArrayToHexArray(Graphics3D.ColorPalette);
            Panel panel = this.chart.Panel;
            instance.PanelBevelInner = panel.Bevel.Inner.ToString();
            instance.PanelBevelOuter = panel.Bevel.Outer.ToString();
            instance.PanelBevelWidth = panel.Bevel.Width;
            instance.PanelBorderRound = panel.BorderRound;
            instance.PanelColor = Utils.ColorToHex(panel.Color);
            instance.PanelGradientDirection = panel.Gradient.Direction.ToString();
            instance.PanelGradientEndColor = Utils.ColorToHex(panel.Gradient.EndColor);
            instance.PanelGradientMiddleColor = Utils.ColorToHex(panel.Gradient.MiddleColor);
            instance.PanelGradientStartColor = Utils.ColorToHex(panel.Gradient.StartColor);
            instance.PanelGradientSigma = panel.Gradient.Sigma;
            instance.PanelGradientSigmaFocus = panel.Gradient.SigmaFocus;
            instance.PanelGradientSigmaScale = panel.Gradient.SigmaScale;
            instance.PanelGradientVisible = panel.Gradient.Visible;
            instance.PanelPenColor = Utils.ColorToHex(panel.Pen.Color);
            instance.PanelPenStyle = panel.Pen.Style.ToString();
            instance.PanelPenVisible = panel.Pen.Visible;
            instance.PanelPenWidth = panel.Pen.Width;
            instance.PanelShadowColor = Utils.ColorToHex(panel.Shadow.Color);
            instance.PanelShadowSize = panel.Shadow.Size.Height;
            instance.PanelShadowVisible = panel.Shadow.Visible;
            if (this.chart.Series.Count > 0)
            {
                Series series = this.chart[0];
                instance.SeriesMarksArrowColor = Utils.ColorToHex(series.Marks.Arrow.Color);
                instance.SeriesMarksFontColor = Utils.ColorToHex(series.Marks.Font.Color);
                instance.SeriesMarksFontName = series.Marks.Font.Name;
                instance.SeriesMarksFontSize = series.Marks.Font.Size;
                instance.SeriesMarksGradientDirection = series.Marks.Gradient.Direction.ToString();
                instance.SeriesMarksGradientEndColor = Utils.ColorToHex(series.Marks.Gradient.EndColor);
                instance.SeriesMarksGradientMiddleColor = Utils.ColorToHex(series.Marks.Gradient.MiddleColor);
                instance.SeriesMarksGradientSigma = series.Marks.Gradient.Sigma;
                instance.SeriesMarksGradientSigmaFocus = series.Marks.Gradient.SigmaFocus;
                instance.SeriesMarksGradientSigmaScale = series.Marks.Gradient.SigmaScale;
                instance.SeriesMarksGradientStartColor = Utils.ColorToHex(series.Marks.Gradient.StartColor);
                instance.SeriesMarksGradientVisible = series.Marks.Gradient.Visible;
                instance.SeriesMarksTransparent = series.Marks.Transparent;
            }
            Wall wall = this.chart.Walls.Left;
            instance.WallApplyDark = wall.ApplyDark;
            instance.WallGradientDirection = wall.Gradient.Direction.ToString();
            instance.WallGradientEndColor = Utils.ColorToHex(wall.Gradient.EndColor);
            instance.WallGradientMiddleColor = Utils.ColorToHex(wall.Gradient.MiddleColor);
            instance.WallGradientSigma = wall.Gradient.Sigma;
            instance.WallGradientSigmaFocus = wall.Gradient.SigmaFocus;
            instance.WallGradientSigmaScale = wall.Gradient.SigmaScale;
            instance.WallGradientStartColor = Utils.ColorToHex(wall.Gradient.StartColor);
            instance.WallGradientVisible = wall.Gradient.Visible;
            instance.WallPenColor = Utils.ColorToHex(wall.Pen.Color);
            instance.WallPenStyle = wall.Pen.Style.ToString();
            instance.WallPenVisible = wall.Pen.Visible;
            instance.WallPenWidth = wall.Pen.Width;
            instance.WallsBackColor = Utils.ColorToHex(this.chart.Walls.Back.Color);
            instance.WallsBackGradientDirection = this.chart.Walls.Back.Gradient.Direction.ToString();
            instance.WallsBackGradientEndColor = Utils.ColorToHex(this.chart.Walls.Back.Gradient.EndColor);
            instance.WallsBackGradientMiddleColor = Utils.ColorToHex(this.chart.Walls.Back.Gradient.MiddleColor);
            instance.WallsBackGradientSigma = this.chart.Walls.Back.Gradient.Sigma;
            instance.WallsBackGradientSigmaFocus = this.chart.Walls.Back.Gradient.SigmaFocus;
            instance.WallsBackGradientSigmaScale = this.chart.Walls.Back.Gradient.SigmaScale;
            instance.WallsBackGradientStartColor = Utils.ColorToHex(this.chart.Walls.Back.Gradient.StartColor);
            instance.WallsBackGradientVisible = this.chart.Walls.Back.Gradient.Visible;
            instance.WallsBackTransparent = this.chart.Walls.Back.Transparent;
            instance.WallsBottomColor = Utils.ColorToHex(this.chart.Walls.Bottom.Color);
            instance.WallSize = wall.Size;
            instance.WallsLeftColor = Utils.ColorToHex(this.chart.Walls.Left.Color);
            instance.WallsRightColor = Utils.ColorToHex(this.chart.Walls.Right.Color);
            if (this.saveWithBase64)
            {
                instance.Base64 = this.chart.Export.GetBase64((MemoryStream) this.chart.Export.InternalSaveViewState(this.chart));
            }
            else
            {
                instance.Base64 = "";
            }
            instance.Write(stream);
        }

        [DefaultValue(false)]
        public bool SaveWithBase64
        {
            get
            {
                return this.saveWithBase64;
            }
            set
            {
                this.saveWithBase64 = value;
            }
        }
    }
}

