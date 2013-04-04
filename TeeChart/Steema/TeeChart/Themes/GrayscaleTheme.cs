namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class GrayscaleTheme : TeeChartTheme
    {
        public GrayscaleTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.PanelBevelOuter = "None";
            ThemeProperties.Instance.PanelPenVisible = true;
            ThemeProperties.Instance.PanelColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.LegendShadowHeight = 0;
            ThemeProperties.Instance.LegendShadowWidth = 0;
            ThemeProperties.Instance.LegendDividingLinesVisible = true;
            ThemeProperties.Instance.LegendFontSize = 10;
            ThemeProperties.Instance.LegendTransparent = true;
            ThemeProperties.Instance.HeaderGradientVisible = true;
            ThemeProperties.Instance.HeaderGradientEndColor = Utils.ColorToHex(Color.Gray);
            ThemeProperties.Instance.HeaderFontSize = 12;
            ThemeProperties.Instance.HeaderFontColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.HeaderColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.GrayscalePalette);
            ThemeProperties.Instance.WallApplyDark = false;
            ThemeProperties.Instance.WallSize = 8;
            ThemeProperties.Instance.WallsBackColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsBottomColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsLeftColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsRightColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsBackTransparent = false;
            ThemeProperties.Instance.AxisAxisPenWidth = 1;
            ThemeProperties.Instance.AxisGridStyle = "Solid";
            ThemeProperties.Instance.AxisTicksColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisMinorTicksVisible = false;
            ThemeProperties.Instance.AxisTicksInnerVisible = false;
            ThemeProperties.Instance.SeriesMarksTransparent = true;
            ThemeProperties.Instance.SeriesMarksFontSize = 10;
            ThemeProperties.Instance.SeriesMarksArrowColor = Utils.ColorToHex(Color.Black);
        }

        public override string ToString()
        {
            return Texts.GrayscaleTheme;
        }
    }
}

