namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    public class XPTheme : BusinessTheme
    {
        public XPTheme(Chart c) : base(c)
        {
        }

        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();
            ThemeProperties.Instance.PanelPenWidth = 3;
            ThemeProperties.Instance.PanelPenColor = Utils.ColorToHex(Utils.FromArgb(0x29, 0x7a, 0xdf));
            ThemeProperties.Instance.PanelBorderRound = 0;
            ThemeProperties.Instance.PanelShadowVisible = true;
            ThemeProperties.Instance.PanelShadowSize = 5;
            ThemeProperties.Instance.PanelShadowColor = Utils.ColorToHex(Color.Black);
            ThemeProperties.Instance.PanelColor = Utils.ColorToHex(Color.White);
            ThemeProperties.Instance.PanelGradientEndColor = Utils.ColorToHex(Utils.FromArgb(0xb1, 0xb1, 0xb1));
            ThemeProperties.Instance.PanelGradientDirection = "BackwardDiagonal";
            ThemeProperties.Instance.WallGradientVisible = true;
            ThemeProperties.Instance.WallGradientStartColor = Utils.ColorToHex(Theme.WindowsXPPalette[2]);
            ThemeProperties.Instance.WallGradientEndColor = Utils.ColorToHex(Theme.WindowsXPPalette[1]);
            ThemeProperties.Instance.WallsBackTransparent = false;
            ThemeProperties.Instance.LegendShadowTransparency = 50;
            ThemeProperties.Instance.Palette = Utils.ColorArrayToHexArray(Theme.WindowsXPPalette);
        }

        public override string ToString()
        {
            return Texts.XPTheme;
        }
    }
}

