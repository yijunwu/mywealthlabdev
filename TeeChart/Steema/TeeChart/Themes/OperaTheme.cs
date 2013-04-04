namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;

    public class OperaTheme : TeeChartTheme
    {
        public OperaTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.AspectSmoothingMode = "HighQuality";
            ThemeProperties.Instance.AspectTextRenderingHint = "ClearTypeGridFit";
            ThemeProperties.Instance.AxisAxisPenColor = "FF404040";
            ThemeProperties.Instance.AxisGridColor = "FFA9A9A9";
            ThemeProperties.Instance.AxisGridStyle = "Dash";
            ThemeProperties.Instance.AxisLabelsFontName = "Verdana";
            ThemeProperties.Instance.AxisTicksInnerColor = "FFA9A9A9";
            ThemeProperties.Instance.AxisTicksLength = 4;
            ThemeProperties.Instance.AxisTitleFontName = "Verdana";
            ThemeProperties.Instance.HeaderFontColor = "FF000080";
            ThemeProperties.Instance.HeaderFontName = "Verdana";
            ThemeProperties.Instance.HeaderPenVisible = true;
            ThemeProperties.Instance.LegendFontName = "Verdana";
            ThemeProperties.Instance.LegendSymbolPenVisible = true;
            ThemeProperties.Instance.PanelGradientEndColor = "FFFFFFFF";
            ThemeProperties.Instance.PanelGradientMiddleColor = "FFEAEAEA";
            ThemeProperties.Instance.PanelGradientStartColor = "FFEAEAEA";
            ThemeProperties.Instance.PanelGradientVisible = true;
            ThemeProperties.Instance.SeriesMarksFontName = "Verdana";
            ThemeProperties.Instance.WallsBackTransparent = false;
            ThemeProperties.Instance.WallsBackGradientDirection = "Vertical";
            ThemeProperties.Instance.WallsBackGradientEndColor = "FFFFFFFF";
            ThemeProperties.Instance.WallsBackGradientMiddleColor = Utils.ColorToHex(Utils.EmptyColor);
            ThemeProperties.Instance.WallsBackGradientSigma = false;
            ThemeProperties.Instance.WallsBackGradientSigmaFocus = 0.5f;
            ThemeProperties.Instance.WallsBackGradientSigmaScale = 1f;
            ThemeProperties.Instance.WallsBackGradientStartColor = "FFEAEAEA";
            ThemeProperties.Instance.WallsBackGradientVisible = true;
            ThemeProperties.Instance.WallsRightColor = "FFC0C0C0";
        }

        public override string ToString()
        {
            return Texts.OperaTheme;
        }
    }
}

