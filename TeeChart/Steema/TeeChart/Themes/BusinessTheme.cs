namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class BusinessTheme : TeeChartTheme
    {
        public BusinessTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.PanelBevelOuter = "None";
            ThemeProperties.Instance.PanelPenVisible = true;
            ThemeProperties.Instance.PanelPenWidth = 6;
            ThemeProperties.Instance.PanelBorderRound = 10;
            ThemeProperties.Instance.PanelPenColor = Utils.ColorToHex(Color.Navy);
            ThemeProperties.Instance.PanelGradientVisible = true;
            ThemeProperties.Instance.PanelGradientEndColor = Utils.ColorToHex(Color.DarkGray);
            ThemeProperties.Instance.LegendShadowHeight = 3;
            ThemeProperties.Instance.LegendShadowWidth = 3;
            ThemeProperties.Instance.LegendGradientVisible = true;
            ThemeProperties.Instance.WallsLeftColor = Utils.ColorToHex(Utils.FromArgb(0xff, 0xff, 0x80));
            ThemeProperties.Instance.WallsRightColor = Utils.ColorToHex(Color.Silver);
            ThemeProperties.Instance.WallsBackColor = Utils.ColorToHex(Color.Silver);
            ThemeProperties.Instance.WallsBottomColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.SeriesMarksGradientVisible = true;
            ThemeProperties.Instance.SeriesMarksGradientStartColor = Utils.ColorToHex(Color.Silver);
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.VictorianPalette);
        }

        public override string ToString()
        {
            return Texts.BusinessTheme;
        }
    }
}

