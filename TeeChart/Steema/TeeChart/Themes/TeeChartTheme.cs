namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class TeeChartTheme : CustomTheme
    {
        public TeeChartTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            ThemeProperties.Instance.AspectSmoothingMode = "HighSpeed";
            ThemeProperties.Instance.AspectTextRenderingHint = "SystemDefault";
            ThemeProperties.Instance.AxesBottomGridCentered = false;
            ThemeProperties.Instance.AxesBottomGridVisible = false;
            ThemeProperties.Instance.AxesTopGridVisible = false;
            ThemeProperties.Instance.AxisAxisPenColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisAxisPenWidth = 2;
            ThemeProperties.Instance.AxisGridCentered = false;
            ThemeProperties.Instance.AxisGridColor = Utils.ColorToHex(Color.Gray);
            ThemeProperties.Instance.AxisGridStyle = "Dot";
            ThemeProperties.Instance.AxisGridVisible = true;
            ThemeProperties.Instance.AxisGridWidth = 1;
            ThemeProperties.Instance.AxisLabelsFontBold = false;
            ThemeProperties.Instance.AxisLabelsFontColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisLabelsFontName = "Arial";
            ThemeProperties.Instance.AxisLabelsFontSize = 8;
            ThemeProperties.Instance.AxisMinorGridColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisMinorGridVisible = false;
            ThemeProperties.Instance.AxisMinorTickCount = 3;
            ThemeProperties.Instance.AxisMinorTicksLength = 2;
            ThemeProperties.Instance.AxisMinorTicksVisible = true;
            ThemeProperties.Instance.AxisTicksColor = Utils.ColorToHex(Color.DarkGray);
            ThemeProperties.Instance.AxisTicksInnerColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisTicksInnerLength = 0;
            ThemeProperties.Instance.AxisTicksInnerVisible = true;
            ThemeProperties.Instance.AxisTicksLength = 2;
            ThemeProperties.Instance.AxisTitleFontColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisTitleFontName = "Arial";
            ThemeProperties.Instance.HeaderColor = Utils.ColorToHex(Color.Silver);
            ThemeProperties.Instance.HeaderFontBold = false;
            ThemeProperties.Instance.HeaderFontColor = Utils.ColorToHex(Color.Blue);
            ThemeProperties.Instance.HeaderFontName = "Arial";
            ThemeProperties.Instance.HeaderFontSize = 8;
            ThemeProperties.Instance.HeaderGradientDirection = "Vertical";
            ThemeProperties.Instance.HeaderGradientEndColor = Utils.ColorToHex(Color.Yellow);
            ThemeProperties.Instance.HeaderGradientMiddleColor = Utils.ColorToHex(Utils.EmptyColor);
            ThemeProperties.Instance.HeaderGradientSigma = false;
            ThemeProperties.Instance.HeaderGradientSigmaFocus = 0f;
            ThemeProperties.Instance.HeaderGradientSigmaScale = 0f;
            ThemeProperties.Instance.HeaderGradientStartColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.HeaderGradientVisible = false;
            ThemeProperties.Instance.HeaderPenColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.HeaderPenVisible = false;
            ThemeProperties.Instance.HeaderPenWidth = 1;
            ThemeProperties.Instance.HeaderShadowSize = 0;
            ThemeProperties.Instance.HeaderShadowTransparency = 0;
            ThemeProperties.Instance.HeaderTransparency = 0;
            ThemeProperties.Instance.LegendDividingLinesColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.LegendDividingLinesVisible = false;
            ThemeProperties.Instance.LegendFontColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.LegendFontName = "Arial";
            ThemeProperties.Instance.LegendFontSize = 8;
            ThemeProperties.Instance.LegendGradientDirection = "Vertical";
            ThemeProperties.Instance.LegendGradientEndColor = Utils.ColorToHex(Color.Yellow);
            ThemeProperties.Instance.LegendGradientMiddleColor = Utils.ColorToHex(Utils.EmptyColor);
            ThemeProperties.Instance.LegendGradientSigma = false;
            ThemeProperties.Instance.LegendGradientSigmaFocus = 0f;
            ThemeProperties.Instance.LegendGradientSigmaScale = 0f;
            ThemeProperties.Instance.LegendGradientStartColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.LegendGradientVisible = false;
            ThemeProperties.Instance.LegendPenColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.LegendPenStyle = "Solid";
            ThemeProperties.Instance.LegendPenVisible = true;
            ThemeProperties.Instance.LegendPenWidth = 1;
            ThemeProperties.Instance.LegendShadowColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.LegendShadowHeight = 3;
            ThemeProperties.Instance.LegendShadowTransparency = 0;
            ThemeProperties.Instance.LegendShadowWidth = 3;
            ThemeProperties.Instance.LegendSymbolDefaultPen = true;
            ThemeProperties.Instance.LegendSymbolPenVisible = false;
            ThemeProperties.Instance.LegendSymbolSquared = false;
            ThemeProperties.Instance.LegendTransparent = false;
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.OperaPalette);
            ThemeProperties.Instance.PanelBevelInner = "None";
            ThemeProperties.Instance.PanelBevelOuter = "Raised";
            ThemeProperties.Instance.PanelBevelWidth = 1;
            ThemeProperties.Instance.PanelBorderRound = 0;
            ThemeProperties.Instance.PanelColor = Utils.ColorToHex(SystemColors.Control);
            ThemeProperties.Instance.PanelGradientDirection = "Vertical";
            ThemeProperties.Instance.PanelGradientEndColor = Utils.ColorToHex(Color.Yellow);
            ThemeProperties.Instance.PanelGradientMiddleColor = Utils.ColorToHex(Utils.EmptyColor);
            ThemeProperties.Instance.PanelGradientSigma = false;
            ThemeProperties.Instance.PanelGradientSigmaFocus = 0f;
            ThemeProperties.Instance.PanelGradientSigmaScale = 0f;
            ThemeProperties.Instance.PanelGradientStartColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.PanelGradientSigma = false;
            ThemeProperties.Instance.PanelGradientSigmaFocus = 0f;
            ThemeProperties.Instance.PanelGradientSigmaScale = 0f;
            ThemeProperties.Instance.PanelGradientVisible = false;
            ThemeProperties.Instance.PanelPenColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.PanelPenStyle = "Solid";
            ThemeProperties.Instance.PanelPenVisible = false;
            ThemeProperties.Instance.PanelPenWidth = 1;
            ThemeProperties.Instance.PanelShadowColor = Utils.ColorToHex(Color.DarkGray);
            ThemeProperties.Instance.PanelShadowSize = 0;
            ThemeProperties.Instance.PanelShadowVisible = false;
            ThemeProperties.Instance.SeriesMarksArrowColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.SeriesMarksFontColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.SeriesMarksFontName = "Arial";
            ThemeProperties.Instance.SeriesMarksFontSize = 8;
            ThemeProperties.Instance.SeriesMarksGradientDirection = "Vertical";
            ThemeProperties.Instance.SeriesMarksGradientEndColor = Utils.ColorToHex(Color.Yellow);
            ThemeProperties.Instance.SeriesMarksGradientMiddleColor = Utils.ColorToHex(Utils.EmptyColor);
            ThemeProperties.Instance.SeriesMarksGradientSigma = false;
            ThemeProperties.Instance.SeriesMarksGradientSigmaFocus = 0f;
            ThemeProperties.Instance.SeriesMarksGradientSigmaScale = 0f;
            ThemeProperties.Instance.SeriesMarksGradientStartColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.SeriesMarksGradientVisible = false;
            ThemeProperties.Instance.SeriesMarksTransparent = false;
            ThemeProperties.Instance.WallApplyDark = true;
            ThemeProperties.Instance.WallGradientDirection = "Vertical";
            ThemeProperties.Instance.WallGradientEndColor = Utils.ColorToHex(Color.Yellow);
            ThemeProperties.Instance.WallGradientMiddleColor = Utils.ColorToHex(Utils.EmptyColor);
            ThemeProperties.Instance.WallGradientSigma = false;
            ThemeProperties.Instance.WallGradientSigmaFocus = 0f;
            ThemeProperties.Instance.WallGradientSigmaScale = 0f;
            ThemeProperties.Instance.WallGradientStartColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallGradientVisible = false;
            ThemeProperties.Instance.WallPenColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.WallPenStyle = "Solid";
            ThemeProperties.Instance.WallPenVisible = true;
            ThemeProperties.Instance.WallPenWidth = 1;
            ThemeProperties.Instance.WallsBackColor = Utils.ColorToHex(Color.Silver);
            ThemeProperties.Instance.WallsBackGradientDirection = "Vertical";
            ThemeProperties.Instance.WallsBackGradientEndColor = Utils.ColorToHex(Color.Yellow);
            ThemeProperties.Instance.WallsBackGradientMiddleColor = Utils.ColorToHex(Utils.EmptyColor);
            ThemeProperties.Instance.WallsBackGradientSigma = false;
            ThemeProperties.Instance.WallsBackGradientSigmaFocus = 0f;
            ThemeProperties.Instance.WallsBackGradientSigmaScale = 0f;
            ThemeProperties.Instance.WallsBackGradientStartColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsBackGradientVisible = false;
            ThemeProperties.Instance.WallsBackTransparent = true;
            ThemeProperties.Instance.WallsBottomColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallSize = 0;
            ThemeProperties.Instance.WallsLeftColor = Utils.ColorToHex(Color.LightYellow);
            ThemeProperties.Instance.WallsRightColor = Utils.ColorToHex(Color.Silver);
        }

        public override string ToString()
        {
            return Texts.TeeChartTheme;
        }
    }
}

