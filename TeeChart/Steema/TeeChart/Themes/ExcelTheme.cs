namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class ExcelTheme : TeeChartTheme
    {
        public ExcelTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.PanelBevelOuter = "None";
            ThemeProperties.Instance.PanelPenVisible = true;
            ThemeProperties.Instance.PanelPenWidth = 1;
            ThemeProperties.Instance.PanelColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.LegendShadowHeight = 0;
            ThemeProperties.Instance.LegendShadowWidth = 0;
            ThemeProperties.Instance.LegendFontSize = 10;
            ThemeProperties.Instance.WallsBottomColor = Utils.ColorToHex(Color.DarkGray);
            ThemeProperties.Instance.WallsLeftColor = Utils.ColorToHex(Color.Silver);
            ThemeProperties.Instance.WallsBackTransparent = false;
            ThemeProperties.Instance.AxisGridColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisGridStyle = "Solid";
            ThemeProperties.Instance.AxisTicksColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.AxisMinorTicksVisible = false;
            ThemeProperties.Instance.AxisTicksInnerVisible = false;
            ThemeProperties.Instance.AxisLabelsFontSize = 10;
            ThemeProperties.Instance.AxesBottomGridCentered = true;
            ThemeProperties.Instance.SeriesMarksTransparent = true;
            ThemeProperties.Instance.SeriesMarksFontSize = 10;
            ThemeProperties.Instance.HeaderFontSize = 10;
            ThemeProperties.Instance.HeaderFontColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.ExcelPalette);
        }

        public override string ToString()
        {
            return Texts.ExcelTheme;
        }
    }
}

