namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class WebTheme : TeeChartTheme
    {
        private Color darkSilver;

        public WebTheme(Chart c) : base(c)
        {
            this.darkSilver = Utils.FromArgb(0xc4, 0xc4, 0xc4);
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.PanelBevelOuter = "None";
            ThemeProperties.Instance.PanelPenVisible = true;
            ThemeProperties.Instance.PanelColor = Utils.ColorToHex(this.darkSilver);
            ThemeProperties.Instance.LegendShadowColor = Utils.ColorToHex(Color.DarkGray);
            ThemeProperties.Instance.LegendFontName = "Lucida Console";
            ThemeProperties.Instance.LegendFontSize = 9;
            ThemeProperties.Instance.LegendTransparent = true;
            ThemeProperties.Instance.WallsBackColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsBottomColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsLeftColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsRightColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsBackTransparent = false;
            ThemeProperties.Instance.HeaderFontName = "Lucida Console";
            ThemeProperties.Instance.HeaderFontSize = 10;
            ThemeProperties.Instance.HeaderFontColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.HeaderFontBold = true;
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.WebPalette);
            ThemeProperties.Instance.AxisGridColor = Utils.ColorToHex(this.darkSilver);
            ThemeProperties.Instance.AxisGridStyle = "Solid";
            ThemeProperties.Instance.AxisGridVisible = true;
            ThemeProperties.Instance.AxisTicksColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisMinorTicksLength = -3;
            ThemeProperties.Instance.AxisTicksLength = 0;
            ThemeProperties.Instance.AxisTicksInnerLength = 6;
            ThemeProperties.Instance.AxisLabelsFontName = "Lucida Console";
            ThemeProperties.Instance.AxisLabelsFontSize = 10;
            ThemeProperties.Instance.SeriesMarksFontName = "Lucida Console";
            ThemeProperties.Instance.SeriesMarksGradientVisible = true;
            ThemeProperties.Instance.SeriesMarksGradientStartColor = Utils.ColorToHex(Color.Silver);
        }

        public override string ToString()
        {
            return Texts.WEBTheme;
        }
    }
}

