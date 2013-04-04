namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;

    public class CustomTheme : Theme
    {
        public CustomTheme(Chart c) : base(c)
        {
        }

        public override void Apply(Chart AChart)
        {
            this.SetDefaultValues();
            this.ChangeAspect(AChart.Aspect);
            this.ChangeAxes(AChart.Axes);
            this.ChangeHeader(AChart.Header);
            this.ChangeLegend(AChart.Legend);
            this.ChangePanel(AChart.Panel);
            this.ChangeAllSeries(AChart.Series);
            this.ChangeWalls(AChart.Walls);
            ColorPalettes.ApplyPalette(AChart, Utils.HexArrayToColorArray(ThemeProperties.Instance.Palette));
        }

        protected internal virtual void ChangeAllSeries(SeriesCollection ChartSeries)
        {
            for (int i = 0; i < ChartSeries.Count; i++)
            {
                this.ChangeSeries(ChartSeries[i]);
            }
        }

        protected internal virtual void ChangeAspect(Aspect ChartAspect)
        {
            if (ThemeProperties.Instance.AspectSmoothingMode != null)
            {
                ChartAspect.SmoothingMode = (SmoothingMode) Enum.Parse(typeof(SmoothingMode), ThemeProperties.Instance.AspectSmoothingMode, true);
            }
            if (ThemeProperties.Instance.AspectTextRenderingHint != null)
            {
                ChartAspect.TextRenderingHint = (TextRenderingHint) Enum.Parse(typeof(TextRenderingHint), ThemeProperties.Instance.AspectTextRenderingHint, true);
            }
        }

        protected internal virtual void ChangeAxes(Axes ChartAxes)
        {
            ChartAxes.Bottom.Grid.Centered = ThemeProperties.Instance.AxesBottomGridCentered;
            ChartAxes.Bottom.Grid.Visible = ThemeProperties.Instance.AxesBottomGridVisible;
            ChartAxes.Top.Grid.Visible = ThemeProperties.Instance.AxesTopGridVisible;
            for (int i = 0; i < ChartAxes.Count; i++)
            {
                this.ChangeAxis(ChartAxes[i]);
            }
        }

        protected internal virtual void ChangeAxis(Axis ChartAxis)
        {
            ChartAxis.AxisPen.Color = Utils.HexToColor(ThemeProperties.Instance.AxisAxisPenColor);
            ChartAxis.AxisPen.Width = ThemeProperties.Instance.AxisAxisPenWidth;
            ChartAxis.Grid.Centered = ThemeProperties.Instance.AxisGridCentered;
            ChartAxis.Grid.Color = Utils.HexToColor(ThemeProperties.Instance.AxisGridColor);
            ChartAxis.Grid.Style = (DashStyle) this.EnumParse(typeof(DashStyle), ThemeProperties.Instance.AxisGridStyle, true);
            ChartAxis.Grid.Visible = ThemeProperties.Instance.AxisGridVisible;
            ChartAxis.Grid.Width = ThemeProperties.Instance.AxisGridWidth;
            ChartAxis.Labels.Font.Bold = ThemeProperties.Instance.AxisLabelsFontBold;
            ChartAxis.Labels.Font.Color = Utils.HexToColor(ThemeProperties.Instance.AxisLabelsFontColor);
            ChartAxis.Labels.Font.Name = ThemeProperties.Instance.AxisLabelsFontName;
            ChartAxis.Labels.Font.Size = ThemeProperties.Instance.AxisLabelsFontSize;
            ChartAxis.MinorGrid.Color = Utils.HexToColor(ThemeProperties.Instance.AxisMinorGridColor);
            ChartAxis.MinorGrid.Visible = ThemeProperties.Instance.AxisMinorGridVisible;
            ChartAxis.MinorTickCount = ThemeProperties.Instance.AxisMinorTickCount;
            ChartAxis.MinorTicks.Length = ThemeProperties.Instance.AxisMinorTicksLength;
            ChartAxis.MinorTicks.Visible = ThemeProperties.Instance.AxisMinorTicksVisible;
            ChartAxis.Ticks.Color = Utils.HexToColor(ThemeProperties.Instance.AxisTicksColor);
            ChartAxis.TicksInner.Color = Utils.HexToColor(ThemeProperties.Instance.AxisTicksInnerColor);
            ChartAxis.TicksInner.Length = ThemeProperties.Instance.AxisTicksInnerLength;
            ChartAxis.TicksInner.Visible = ThemeProperties.Instance.AxisTicksInnerVisible;
            ChartAxis.Ticks.Length = ThemeProperties.Instance.AxisTicksLength;
            ChartAxis.Title.Font.Color = Utils.HexToColor(ThemeProperties.Instance.AxisTitleFontColor);
            ChartAxis.Title.Font.Name = ThemeProperties.Instance.AxisTitleFontName;
        }

        protected internal virtual void ChangeHeader(Header ChartHeader)
        {
            ChartHeader.Color = Utils.HexToColor(ThemeProperties.Instance.HeaderColor);
            ChartHeader.Font.Bold = ThemeProperties.Instance.HeaderFontBold;
            ChartHeader.Font.Color = Utils.HexToColor(ThemeProperties.Instance.HeaderFontColor);
            ChartHeader.Font.Name = ThemeProperties.Instance.HeaderFontName;
            ChartHeader.Font.Size = ThemeProperties.Instance.HeaderFontSize;
            ChartHeader.Pen.Color = Utils.HexToColor(ThemeProperties.Instance.HeaderPenColor);
            ChartHeader.Pen.Visible = ThemeProperties.Instance.HeaderPenVisible;
            ChartHeader.Pen.Width = ThemeProperties.Instance.HeaderPenWidth;
            ChartHeader.Shadow.Size = new Size(ThemeProperties.Instance.HeaderShadowSize, ThemeProperties.Instance.HeaderShadowSize);
            ChartHeader.Shadow.Transparency = ThemeProperties.Instance.HeaderShadowTransparency;
            ChartHeader.Transparency = ThemeProperties.Instance.HeaderTransparency;
            this.ResetHeaderGradient(ChartHeader.Gradient);
        }

        protected internal virtual void ChangeLegend(Legend ChartLegend)
        {
            ChartLegend.DividingLines.Color = Utils.HexToColor(ThemeProperties.Instance.LegendDividingLinesColor);
            ChartLegend.DividingLines.Visible = ThemeProperties.Instance.LegendDividingLinesVisible;
            ChartLegend.Font.Color = Utils.HexToColor(ThemeProperties.Instance.LegendFontColor);
            ChartLegend.Font.Name = ThemeProperties.Instance.LegendFontName;
            ChartLegend.Font.Size = ThemeProperties.Instance.LegendFontSize;
            ChartLegend.Pen.Color = Utils.HexToColor(ThemeProperties.Instance.LegendPenColor);
            ChartLegend.Pen.Style = (DashStyle) this.EnumParse(typeof(DashStyle), ThemeProperties.Instance.LegendPenStyle, true);
            ChartLegend.Pen.Visible = ThemeProperties.Instance.LegendPenVisible;
            ChartLegend.Pen.Width = ThemeProperties.Instance.LegendPenWidth;
            this.ResetLegendGradient(ChartLegend.Gradient);
            ChartLegend.Shadow.Color = Utils.HexToColor(ThemeProperties.Instance.LegendShadowColor);
            ChartLegend.Shadow.Height = ThemeProperties.Instance.LegendShadowHeight;
            ChartLegend.Shadow.Transparency = ThemeProperties.Instance.LegendShadowTransparency;
            ChartLegend.Shadow.Width = ThemeProperties.Instance.LegendShadowWidth;
            ChartLegend.Symbol.DefaultPen = ThemeProperties.Instance.LegendSymbolDefaultPen;
            ChartLegend.Symbol.Pen.Visible = ThemeProperties.Instance.LegendSymbolPenVisible;
            ChartLegend.Symbol.Squared = ThemeProperties.Instance.LegendSymbolSquared;
            ChartLegend.Transparent = ThemeProperties.Instance.LegendTransparent;
        }

        protected internal virtual void ChangePanel(Panel ChartPanel)
        {
            ChartPanel.Bevel.Inner = (BevelStyles) this.EnumParse(typeof(BevelStyles), ThemeProperties.Instance.PanelBevelInner, true);
            ChartPanel.Bevel.Outer = (BevelStyles) this.EnumParse(typeof(BevelStyles), ThemeProperties.Instance.PanelBevelOuter, true);
            ChartPanel.Bevel.Width = ThemeProperties.Instance.PanelBevelWidth;
            ChartPanel.BorderRound = ThemeProperties.Instance.PanelBorderRound;
            ChartPanel.Color = Utils.HexToColor(ThemeProperties.Instance.PanelColor);
            this.ResetPanelGradient(ChartPanel.Gradient);
            ChartPanel.Pen.Color = Utils.HexToColor(ThemeProperties.Instance.PanelPenColor);
            ChartPanel.Pen.Style = (DashStyle) this.EnumParse(typeof(DashStyle), ThemeProperties.Instance.PanelPenStyle, true);
            ChartPanel.Pen.Visible = ThemeProperties.Instance.PanelPenVisible;
            ChartPanel.Pen.Width = ThemeProperties.Instance.PanelPenWidth;
            ChartPanel.Shadow.Size = new Size(ThemeProperties.Instance.PanelShadowSize, ThemeProperties.Instance.PanelShadowSize);
            ChartPanel.Shadow.Visible = ThemeProperties.Instance.PanelShadowVisible;
        }

        protected internal virtual void ChangeSeries(Series ChartSeries)
        {
            ChartSeries.Marks.Arrow.Color = Utils.HexToColor(ThemeProperties.Instance.SeriesMarksArrowColor);
            ChartSeries.Marks.Font.Color = Utils.HexToColor(ThemeProperties.Instance.SeriesMarksFontColor);
            ChartSeries.Marks.Font.Name = ThemeProperties.Instance.SeriesMarksFontName;
            ChartSeries.Marks.Font.Size = ThemeProperties.Instance.SeriesMarksFontSize;
            ChartSeries.Marks.Transparent = ThemeProperties.Instance.SeriesMarksTransparent;
            this.ResetSeriesMarksGradient(ChartSeries.Marks.Gradient);
        }

        protected internal virtual void ChangeWall(Wall ChartWall, Color AColor)
        {
            ChartWall.ApplyDark = ThemeProperties.Instance.WallApplyDark;
            this.ResetWallGradient(ChartWall.Gradient);
            ChartWall.Pen.Color = Utils.HexToColor(ThemeProperties.Instance.WallPenColor);
            ChartWall.Pen.Style = (DashStyle) this.EnumParse(typeof(DashStyle), ThemeProperties.Instance.WallPenStyle, true);
            ChartWall.Pen.Visible = ThemeProperties.Instance.WallPenVisible;
            ChartWall.Pen.Width = ThemeProperties.Instance.WallPenWidth;
            ChartWall.Size = ThemeProperties.Instance.WallSize;
            ChartWall.Color = AColor;
        }

        protected internal virtual void ChangeWalls(Walls ChartWalls)
        {
            this.ChangeWall(ChartWalls.Back, Utils.HexToColor(ThemeProperties.Instance.WallsBackColor));
            ChartWalls.Back.Gradient.Direction = (LinearGradientMode) this.EnumParse(typeof(LinearGradientMode), ThemeProperties.Instance.WallsBackGradientDirection, true);
            ChartWalls.Back.Gradient.EndColor = Utils.HexToColor(ThemeProperties.Instance.WallsBackGradientEndColor);
            ChartWalls.Back.Gradient.MiddleColor = Utils.HexToColor(ThemeProperties.Instance.WallsBackGradientMiddleColor);
            ChartWalls.Back.Gradient.Sigma = ThemeProperties.Instance.WallsBackGradientSigma;
            ChartWalls.Back.Gradient.SigmaFocus = ThemeProperties.Instance.WallsBackGradientSigmaFocus;
            ChartWalls.Back.Gradient.SigmaScale = ThemeProperties.Instance.WallsBackGradientSigmaScale;
            ChartWalls.Back.Gradient.StartColor = Utils.HexToColor(ThemeProperties.Instance.WallsBackGradientStartColor);
            ChartWalls.Back.Gradient.Visible = ThemeProperties.Instance.WallsBackGradientVisible;
            ChartWalls.Back.Transparent = ThemeProperties.Instance.WallsBackTransparent;
            this.ChangeWall(ChartWalls.Bottom, Utils.HexToColor(ThemeProperties.Instance.WallsBottomColor));
            this.ChangeWall(ChartWalls.Left, Utils.HexToColor(ThemeProperties.Instance.WallsLeftColor));
            this.ChangeWall(ChartWalls.Right, Utils.HexToColor(ThemeProperties.Instance.WallsRightColor));
        }

        private object EnumParse(Type enumType, string value, bool ignoreCase)
        {
            return Enum.Parse(enumType, value, ignoreCase);
        }

        protected internal virtual void ResetHeaderGradient(Gradient ChartGradient)
        {
            ChartGradient.Direction = (LinearGradientMode) this.EnumParse(typeof(LinearGradientMode), ThemeProperties.Instance.HeaderGradientDirection, true);
            ChartGradient.EndColor = Utils.HexToColor(ThemeProperties.Instance.HeaderGradientEndColor);
            ChartGradient.MiddleColor = Utils.HexToColor(ThemeProperties.Instance.HeaderGradientMiddleColor);
            ChartGradient.Sigma = ThemeProperties.Instance.HeaderGradientSigma;
            ChartGradient.SigmaFocus = ThemeProperties.Instance.HeaderGradientSigmaFocus;
            ChartGradient.SigmaScale = ThemeProperties.Instance.HeaderGradientSigmaScale;
            ChartGradient.StartColor = Utils.HexToColor(ThemeProperties.Instance.HeaderGradientStartColor);
            ChartGradient.Visible = ThemeProperties.Instance.HeaderGradientVisible;
        }

        protected internal virtual void ResetLegendGradient(Gradient ChartGradient)
        {
            ChartGradient.Direction = (LinearGradientMode) this.EnumParse(typeof(LinearGradientMode), ThemeProperties.Instance.LegendGradientDirection, true);
            ChartGradient.EndColor = Utils.HexToColor(ThemeProperties.Instance.LegendGradientEndColor);
            ChartGradient.MiddleColor = Utils.HexToColor(ThemeProperties.Instance.LegendGradientMiddleColor);
            ChartGradient.Sigma = ThemeProperties.Instance.LegendGradientSigma;
            ChartGradient.SigmaFocus = ThemeProperties.Instance.LegendGradientSigmaFocus;
            ChartGradient.SigmaScale = ThemeProperties.Instance.LegendGradientSigmaScale;
            ChartGradient.StartColor = Utils.HexToColor(ThemeProperties.Instance.LegendGradientStartColor);
            ChartGradient.Visible = ThemeProperties.Instance.LegendGradientVisible;
        }

        protected internal virtual void ResetPanelGradient(Gradient ChartGradient)
        {
            ChartGradient.Direction = (LinearGradientMode) this.EnumParse(typeof(LinearGradientMode), ThemeProperties.Instance.PanelGradientDirection, true);
            ChartGradient.EndColor = Utils.HexToColor(ThemeProperties.Instance.PanelGradientEndColor);
            ChartGradient.MiddleColor = Utils.HexToColor(ThemeProperties.Instance.PanelGradientMiddleColor);
            ChartGradient.Sigma = ThemeProperties.Instance.PanelGradientSigma;
            ChartGradient.SigmaFocus = ThemeProperties.Instance.PanelGradientSigmaFocus;
            ChartGradient.SigmaScale = ThemeProperties.Instance.PanelGradientSigmaScale;
            ChartGradient.StartColor = Utils.HexToColor(ThemeProperties.Instance.PanelGradientStartColor);
            ChartGradient.Visible = ThemeProperties.Instance.PanelGradientVisible;
        }

        protected internal virtual void ResetSeriesMarksGradient(Gradient ChartGradient)
        {
            ChartGradient.Direction = (LinearGradientMode) this.EnumParse(typeof(LinearGradientMode), ThemeProperties.Instance.SeriesMarksGradientDirection, true);
            ChartGradient.EndColor = Utils.HexToColor(ThemeProperties.Instance.SeriesMarksGradientEndColor);
            ChartGradient.MiddleColor = Utils.HexToColor(ThemeProperties.Instance.SeriesMarksGradientMiddleColor);
            ChartGradient.Sigma = ThemeProperties.Instance.SeriesMarksGradientSigma;
            ChartGradient.SigmaFocus = ThemeProperties.Instance.SeriesMarksGradientSigmaFocus;
            ChartGradient.SigmaScale = ThemeProperties.Instance.SeriesMarksGradientSigmaScale;
            ChartGradient.StartColor = Utils.HexToColor(ThemeProperties.Instance.SeriesMarksGradientStartColor);
            ChartGradient.Visible = ThemeProperties.Instance.SeriesMarksGradientVisible;
        }

        protected internal virtual void ResetWallGradient(Gradient ChartGradient)
        {
            ChartGradient.Direction = (LinearGradientMode) this.EnumParse(typeof(LinearGradientMode), ThemeProperties.Instance.WallGradientDirection, true);
            ChartGradient.EndColor = Utils.HexToColor(ThemeProperties.Instance.WallGradientEndColor);
            ChartGradient.MiddleColor = Utils.HexToColor(ThemeProperties.Instance.WallGradientMiddleColor);
            ChartGradient.Sigma = ThemeProperties.Instance.WallGradientSigma;
            ChartGradient.SigmaFocus = ThemeProperties.Instance.WallGradientSigmaFocus;
            ChartGradient.SigmaScale = ThemeProperties.Instance.WallGradientSigmaScale;
            ChartGradient.StartColor = Utils.HexToColor(ThemeProperties.Instance.WallGradientStartColor);
            ChartGradient.Visible = ThemeProperties.Instance.WallGradientVisible;
        }

        protected internal virtual void SetDefaultValues()
        {
        }

        public override string ToString()
        {
            return Texts.TeeChartTheme;
        }
    }
}

