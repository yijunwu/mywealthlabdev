namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class BlueSkyTheme : TeeChartTheme
    {
        public BlueSkyTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.PanelShadowVisible = true;
            ThemeProperties.Instance.PanelShadowColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.PanelBevelInner = "Lowered";
            ThemeProperties.Instance.PanelBevelOuter = "None";
            ThemeProperties.Instance.PanelBevelWidth = 2;
            ThemeProperties.Instance.WallsBackColor = "FFFCECCF";
            ThemeProperties.Instance.WallsBottomColor = "FF038CFC";
            ThemeProperties.Instance.WallsLeftColor = "FF8080FF";
            ThemeProperties.Instance.WallPenVisible = false;
            ThemeProperties.Instance.WallSize = 5;
            ThemeProperties.Instance.PanelGradientEndColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.PanelGradientMiddleColor = "FF80FFFF";
            ThemeProperties.Instance.PanelGradientStartColor = "FF2003A5";
            ThemeProperties.Instance.PanelGradientVisible = true;
            ThemeProperties.Instance.LegendDividingLinesColor = Utils.ColorToHex(Color.Silver);
            ThemeProperties.Instance.LegendDividingLinesVisible = true;
            ThemeProperties.Instance.LegendFontColor = "FF000064";
            ThemeProperties.Instance.LegendGradientDirection = "Horizontal";
            ThemeProperties.Instance.LegendGradientEndColor = "FFFFDBCE";
            ThemeProperties.Instance.LegendGradientMiddleColor = "FFE9E6E0";
            ThemeProperties.Instance.LegendGradientStartColor = "FFEAF3FF";
            ThemeProperties.Instance.LegendGradientVisible = true;
            ThemeProperties.Instance.LegendShadowHeight = 5;
            ThemeProperties.Instance.LegendShadowWidth = 4;
            ThemeProperties.Instance.LegendShadowTransparency = 50;
            ThemeProperties.Instance.LegendSymbolSquared = true;
            ThemeProperties.Instance.HeaderColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.HeaderTransparency = 70;
            ThemeProperties.Instance.HeaderFontColor = Utils.ColorToHex(Color.Navy);
            ThemeProperties.Instance.HeaderFontSize = 12;
            ThemeProperties.Instance.HeaderPenVisible = true;
            ThemeProperties.Instance.HeaderPenWidth = 2;
            ThemeProperties.Instance.HeaderPenColor = "FFFBDD99";
            ThemeProperties.Instance.HeaderGradientEndColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.HeaderGradientMiddleColor = "FF400080";
            ThemeProperties.Instance.HeaderGradientStartColor = Utils.ColorToHex(Color.Gray);
            ThemeProperties.Instance.HeaderGradientVisible = true;
            ThemeProperties.Instance.HeaderShadowSize = 4;
            ThemeProperties.Instance.HeaderShadowTransparency = 70;
            ThemeProperties.Instance.AxisAxisPenWidth = 1;
            ThemeProperties.Instance.AxisGridColor = "FFB9B9FF";
            ThemeProperties.Instance.AxisGridStyle = "Dot";
            ThemeProperties.Instance.AxisLabelsFontColor = Utils.ColorToHex(Color.Navy);
            ThemeProperties.Instance.AxisLabelsFontName = "Tahoma";
            ThemeProperties.Instance.AxisLabelsFontBold = true;
            ThemeProperties.Instance.AxisMinorGridVisible = false;
            ThemeProperties.Instance.AxisMinorGridColor = "FFE5E5E5";
            ThemeProperties.Instance.AxisMinorTickCount = 7;
            ThemeProperties.Instance.AxisTicksLength = 5;
            ThemeProperties.Instance.AxisAxisPenColor = Utils.ColorToHex(Color.Navy);
            ThemeProperties.Instance.AxisGridColor = Utils.ColorToHex(Color.Blue);
            ThemeProperties.Instance.AxisGridStyle = "Dot";
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.PastelsPalette);
        }

        public override string ToString()
        {
            return Texts.BlueSkyTheme;
        }
    }
}

