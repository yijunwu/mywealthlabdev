namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class ClassicTheme : TeeChartTheme
    {
        public ClassicTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.PanelBevelOuter = "None";
            ThemeProperties.Instance.PanelColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.LegendShadowHeight = 0;
            ThemeProperties.Instance.LegendShadowWidth = 0;
            ThemeProperties.Instance.LegendFontName = "Times New Roman";
            ThemeProperties.Instance.LegendFontSize = 10;
            ThemeProperties.Instance.LegendTransparent = true;
            ThemeProperties.Instance.LegendPenVisible = false;
            ThemeProperties.Instance.LegendSymbolDefaultPen = false;
            ThemeProperties.Instance.WallsBackColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsLeftColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsRightColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsBottomColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.WallsBackTransparent = false;
            ThemeProperties.Instance.AxisAxisPenWidth = 1;
            ThemeProperties.Instance.AxisGridColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisGridStyle = "Solid";
            ThemeProperties.Instance.AxisTicksColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisMinorTicksVisible = false;
            ThemeProperties.Instance.AxisTicksInnerVisible = false;
            ThemeProperties.Instance.AxisLabelsFontName = "Times New Roman";
            ThemeProperties.Instance.AxisLabelsFontSize = 10;
            ThemeProperties.Instance.AxisTitleFontName = "Times New Roman";
            ThemeProperties.Instance.AxesBottomGridCentered = true;
            ThemeProperties.Instance.SeriesMarksTransparent = true;
            ThemeProperties.Instance.SeriesMarksFontName = "Times New Roman";
            ThemeProperties.Instance.SeriesMarksFontSize = 10;
            ThemeProperties.Instance.SeriesMarksArrowColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.HeaderFontName = "Times New Roman";
            ThemeProperties.Instance.HeaderFontSize = 12;
            ThemeProperties.Instance.HeaderFontColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.ClassicPalette);
        }

        public override string ToString()
        {
            return Texts.ClassicTheme;
        }
    }
}

