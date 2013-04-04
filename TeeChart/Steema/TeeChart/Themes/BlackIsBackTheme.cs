namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;

    public class BlackIsBackTheme : OperaTheme
    {
        public BlackIsBackTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.LegendFontColor = "FFFFFFFF";
            ThemeProperties.Instance.LegendGradientVisible = true;
            ThemeProperties.Instance.LegendGradientStartColor = "FF464646";
            ThemeProperties.Instance.LegendGradientMiddleColor = "00000000";
            ThemeProperties.Instance.LegendGradientEndColor = "FF787878";
            ThemeProperties.Instance.LegendShadowWidth = 0;
            ThemeProperties.Instance.LegendPenVisible = false;
            ThemeProperties.Instance.HeaderFontColor = "FFFFFFFF";
            ThemeProperties.Instance.WallPenVisible = false;
            ThemeProperties.Instance.WallsBackGradientVisible = true;
            ThemeProperties.Instance.WallsBackGradientStartColor = "FF464646";
            ThemeProperties.Instance.WallsBackGradientMiddleColor = "00000000";
            ThemeProperties.Instance.WallsBackGradientEndColor = "FF787878";
            ThemeProperties.Instance.PanelGradientVisible = true;
            ThemeProperties.Instance.PanelGradientStartColor = "FF464646";
            ThemeProperties.Instance.PanelGradientMiddleColor = "00000000";
            ThemeProperties.Instance.PanelGradientEndColor = "FF464646";
            ThemeProperties.Instance.AxisMinorTicksVisible = false;
            ThemeProperties.Instance.AxisLabelsFontColor = "FFFFFFFF";
            ThemeProperties.Instance.AxisTicksColor = "FF828282";
            ThemeProperties.Instance.AxisAxisPenColor = "FF828282";
            ThemeProperties.Instance.AxisGridStyle = "Solid";
            ThemeProperties.Instance.AxisGridColor = "FF828282";
            ThemeProperties.Instance.SeriesMarksFontColor = "FFFFFFFF";
            ThemeProperties.Instance.SeriesMarksTransparent = true;
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.OnBlackPalette);
        }

        public override string ToString()
        {
            return Texts.BlackIsBackTheme;
        }
    }
}

