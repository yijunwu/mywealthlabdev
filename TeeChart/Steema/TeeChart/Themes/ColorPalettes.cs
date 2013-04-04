namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    [Serializable]
    public sealed class ColorPalettes : List<string>
    {
        public static void ApplyPalette(Chart custom, int Index)
        {
            switch (Index)
            {
                case -1:
                    ApplyPalette(custom, Theme.OperaPalette);
                    break;

                case 0:
                    ApplyPalette(custom, Theme.TeeChartPalette);
                    break;

                case 1:
                    ApplyPalette(custom, Theme.ExcelPalette);
                    break;

                case 2:
                    ApplyPalette(custom, Theme.VictorianPalette);
                    break;

                case 3:
                    ApplyPalette(custom, Theme.PastelsPalette);
                    break;

                case 4:
                    ApplyPalette(custom, Theme.SolidPalette);
                    break;

                case 5:
                    ApplyPalette(custom, Theme.ClassicPalette);
                    break;

                case 6:
                    ApplyPalette(custom, Theme.WebPalette);
                    break;

                case 7:
                    ApplyPalette(custom, Theme.ModernPalette);
                    break;

                case 8:
                    ApplyPalette(custom, Theme.RainbowPalette);
                    break;

                case 9:
                    ApplyPalette(custom, Theme.WindowsXPPalette);
                    break;

                case 10:
                    ApplyPalette(custom, Theme.MacOSPalette);
                    break;

                case 11:
                    ApplyPalette(custom, Theme.WindowsVistaPalette);
                    break;

                case 12:
                    ApplyPalette(custom, Theme.GrayscalePalette);
                    break;

                case 13:
                    ApplyPalette(custom, Theme.OperaPalette);
                    break;

                case 14:
                    ApplyPalette(custom, Theme.WarmPalette);
                    break;

                case 15:
                    ApplyPalette(custom, Theme.CoolPalette);
                    break;

                case 0x10:
                    ApplyPalette(custom, Theme.OnBlackPalette);
                    break;

                default:
                    ApplyPalette(custom, (Color[]) null);
                    break;
            }
            if (custom.Aspect != null)
            {
                custom.Aspect.colorPaletteIdx = Index;
            }
            custom.Invalidate();
        }

        public static void ApplyPalette(Chart custom, Color[] Palette)
        {
            if (Palette != null)
            {
                Color[] palette = (Color[]) Graphics3D.ColorPalette.Clone();
                Graphics3D.ColorPalette = new Color[Palette.Length];
                for (int i = 0; i < Palette.Length; i++)
                {
                    Graphics3D.ColorPalette[i] = Palette[i];
                }
                if (custom.Series != null)
                {
                    ColorPaletteChanged(custom, palette);
                }
            }
        }

        public static void ColorPaletteChanged(Chart custom, Color[] Palette)
        {
            foreach (Series series in custom.Series)
            {
                if (!series.ColorEach)
                {
                    series.Color = Utils.EmptyColor;
                    series.Color = custom.FreeSeriesColor(true);
                }
            }
        }

        public static bool IsInPalette(Color color, Color[] Palette)
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                if (color == Palette[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}

