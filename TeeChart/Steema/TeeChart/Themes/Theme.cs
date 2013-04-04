namespace Steema.TeeChart.Themes
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    [Serializable]
    public abstract class Theme : TeeBase
    {
        private static List<Type> chartThemes = null;
        public static Color[] ClassicPalette = new Color[] { Utils.FromArgb(0, 0, 0xff), Utils.FromArgb(0, 0xff, 0), Utils.FromArgb(0, 0xff, 0xff), Utils.FromArgb(0xff, 0, 0), Utils.FromArgb(0xff, 0, 0xff), Utils.FromArgb(0xff, 0xff, 0), Utils.FromArgb(0, 0, 0x80), Utils.FromArgb(0, 0x80, 0), Utils.FromArgb(0, 0x80, 0x80), Utils.FromArgb(0x80, 0, 0), Utils.FromArgb(0x80, 0x80, 0), Utils.FromArgb(0x80, 0x80, 0x80) };
        private static Steema.TeeChart.Themes.ColorPalettes colorPalettes = null;
        public static Color[] CoolPalette = new Color[] { Utils.FromArgb(0x2b, 0x40, 0x6b), Utils.FromArgb(0x3b, 0x54, 140), Utils.FromArgb(0x44, 0x66, 0xa3), Utils.FromArgb(0x4e, 0x97, 0xa8), Utils.FromArgb(0x5d, 0xb7, 0x9e), Utils.FromArgb(0x41, 160, 0x8a), Utils.FromArgb(0x2b, 0x92, 0x7d), Utils.FromArgb(0x1d, 0x7b, 0x63) };
        public static Color[] ExcelPalette = new Color[] { 
            Utils.FromArgb(0x99, 0x99, 0xff), Utils.FromArgb(0x99, 0x33, 0x66), Utils.FromArgb(0xff, 0xff, 0xcc), Utils.FromArgb(0xcc, 0xff, 0xff), Utils.FromArgb(0x66, 0, 0x66), Utils.FromArgb(0xff, 0x80, 0x80), Utils.FromArgb(0, 0x66, 0xcc), Utils.FromArgb(0xcc, 0xcc, 0xff), Utils.FromArgb(0, 0, 0x80), Utils.FromArgb(0xff, 0, 0xff), Utils.FromArgb(0xff, 0xff, 0), Utils.FromArgb(0, 0xff, 0xff), Utils.FromArgb(0x80, 0, 0), Utils.FromArgb(0, 0x80, 0), Utils.FromArgb(0, 0, 0x80), Utils.FromArgb(0, 0, 0xff), 
            Utils.FromArgb(0, 0xcc, 0xff), Utils.FromArgb(0xcc, 0xff, 0xff), Utils.FromArgb(0xcc, 0xff, 0xcc), Utils.FromArgb(0xff, 0xff, 0), Utils.FromArgb(0x99, 0xcc, 0xff), Utils.FromArgb(0xff, 0x99, 0xcc)
         };
        public static Color[] GrayscalePalette = new Color[] { Utils.FromArgb(240, 240, 240), Utils.FromArgb(0xe0, 0xe0, 0xe0), Utils.FromArgb(0xd0, 0xd0, 0xd0), Utils.FromArgb(0xc0, 0xc0, 0xc0), Utils.FromArgb(0xb0, 0xb0, 0xb0), Utils.FromArgb(160, 160, 160), Utils.FromArgb(0x90, 0x90, 0x90), Utils.FromArgb(0x80, 0x80, 0x80), Utils.FromArgb(0x70, 0x70, 0x70), Utils.FromArgb(0x60, 0x60, 0x60), Utils.FromArgb(80, 80, 80), Utils.FromArgb(0x40, 0x40, 0x40), Utils.FromArgb(0x30, 0x30, 0x30), Utils.FromArgb(0x20, 0x20, 0x20), Utils.FromArgb(0x10, 0x10, 0x10) };
        public static Color[] MacOSPalette = new Color[] { Utils.FromArgb(0xff, 0xff, 0xff), Utils.FromArgb(0xfc, 0xf3, 5), Utils.FromArgb(0xff, 100, 2), Utils.FromArgb(0xdd, 8, 6), Utils.FromArgb(0xf2, 8, 0x84), Utils.FromArgb(70, 0, 0xa5), Utils.FromArgb(0, 0, 0xd4), Utils.FromArgb(2, 0xab, 0xea), Utils.FromArgb(0x1f, 0xb7, 20), Utils.FromArgb(0, 100, 0x11), Utils.FromArgb(0x56, 0x2c, 5), Utils.FromArgb(0x90, 0x71, 0x3a), Utils.FromArgb(0xc0, 0xc0, 0xc0), Utils.FromArgb(0x80, 0x80, 0x80), Utils.FromArgb(0x40, 0x40, 0x40), Utils.FromArgb(0, 0, 0) };
        public static Color[] ModernPalette = new Color[] { Utils.FromArgb(0xff, 0x99, 0x66), Utils.FromArgb(0xff, 0x66, 0x66), Utils.FromArgb(0x99, 0xcc, 0xff), Utils.FromArgb(0x66, 0x99, 0x66), Utils.FromArgb(0xcc, 0xcc, 0x99), Utils.FromArgb(0x99, 0x66, 0xcc), Utils.FromArgb(0xcc, 0x66, 0x66), Utils.FromArgb(0xff, 0xcc, 0x99), Utils.FromArgb(0x99, 0x66, 0xff), Utils.FromArgb(0xcc, 0xcc, 0xcc), Utils.FromArgb(0x66, 0xff, 0xcc), Utils.FromArgb(0x66, 0x99, 0xff), Utils.FromArgb(0x99, 0x66, 0x99), Utils.FromArgb(0xcc, 0xcc, 0xff) };
        public static Color[] OnBlackPalette = new Color[] { Utils.FromArgb(200, 230, 90), Utils.FromArgb(90, 150, 220), Utils.FromArgb(230, 90, 40), Utils.FromArgb(230, 160, 15) };
        public static Color[] OperaPalette = new Color[] { Utils.FromArgb(0x44, 0x66, 0xa3), Utils.FromArgb(0xf3, 0x9c, 0x35), Utils.FromArgb(0xf1, 0x4c, 20), Utils.FromArgb(0x4e, 0x97, 0xa8), Utils.FromArgb(0x2b, 0x40, 0x6b), Utils.FromArgb(0x1d, 0x7b, 0x63), Utils.FromArgb(0xb3, 8, 14), Utils.FromArgb(0xf2, 0xc0, 0x5d), Utils.FromArgb(0x5d, 0xb7, 0x9e), Utils.FromArgb(0x70, 0x70, 0x70), Utils.FromArgb(0xf3, 0xea, 0x8d), Utils.FromArgb(180, 180, 180) };
        public static Color[] PastelsPalette = new Color[] { Utils.FromArgb(0xcc, 0xff, 0xff), Utils.FromArgb(0xff, 0xff, 0xcc), Utils.FromArgb(0xcc, 0xcc, 0xff), Utils.FromArgb(0, 0xcc, 0xcc), Utils.FromArgb(0xcc, 0xcc, 0xcc), Utils.FromArgb(0, 0x99, 0x99), Utils.FromArgb(0x99, 0x99, 0x99), Utils.FromArgb(0xdd, 0xcc, 0xcc), Utils.FromArgb(0xff, 0xcc, 0x66), Utils.FromArgb(0xcc, 0xcc, 0xff), Utils.FromArgb(0xff, 0x99, 0x99), Utils.FromArgb(0xff, 0xff, 0x99), Utils.FromArgb(0x99, 0xcc, 0xff), Utils.FromArgb(0xcc, 0xff, 0xcc) };
        public static Color[] RainbowPalette = new Color[] { 
            Utils.FromArgb(0x99, 0, 0), Utils.FromArgb(0xc3, 0, 0), Utils.FromArgb(0xee, 0, 0), Utils.FromArgb(0xff, 0x1a, 0), Utils.FromArgb(0xff, 70, 0), Utils.FromArgb(0xff, 0x73, 0), Utils.FromArgb(0xff, 0x9f, 0), Utils.FromArgb(0xff, 0xcb, 0), Utils.FromArgb(0xff, 0xf7, 0), Utils.FromArgb(0xe3, 0xf4, 8), Utils.FromArgb(0xc3, 0xe7, 0x11), Utils.FromArgb(0xa3, 0xda, 0x1b), Utils.FromArgb(0x83, 0xcd, 0x25), Utils.FromArgb(0x63, 0xc0, 0x2e), Utils.FromArgb(0x42, 0xb3, 0x38), Utils.FromArgb(0x22, 0xa6, 0x42), 
            Utils.FromArgb(2, 0x9a, 0x4b), Utils.FromArgb(12, 0x87, 0x6a), Utils.FromArgb(0x1a, 0x75, 0x8a), Utils.FromArgb(40, 0x63, 170), Utils.FromArgb(0x36, 80, 0xcb), Utils.FromArgb(0x44, 0x3e, 0xeb), Utils.FromArgb(0x61, 0x2a, 0xff), Utils.FromArgb(150, 0x15, 0xff), Utils.FromArgb(0xcc, 0, 0xff)
         };
        public static Color[] SolidPalette = new Color[] { Utils.FromArgb(0, 0, 0xff), Utils.FromArgb(0xff, 0, 0), Utils.FromArgb(0, 0xff, 0), Utils.FromArgb(0xff, 0xcc, 0), Utils.FromArgb(0x40, 0x40, 0x40), Utils.FromArgb(0xff, 0xff, 0), Utils.FromArgb(0xff, 0, 0xc0), Utils.FromArgb(0xff, 0xff, 0xff) };
        public static Color[] TeeChartPalette = new Color[] { 
            Color.Red, Color.Green, Color.Yellow, Color.Blue, Color.White, Color.Gray, Color.Fuchsia, Color.Teal, Color.Navy, Color.Maroon, Color.Lime, Color.Olive, Color.Purple, Color.Silver, Color.Aqua, Color.Black, 
            Color.GreenYellow, Color.SkyBlue, Color.Bisque, Color.Indigo
         };
        public static Color[] VictorianPalette = new Color[] { Utils.FromArgb(0x5d, 0xa5, 0xa1), Utils.FromArgb(0xc4, 0x53, 0x31), Utils.FromArgb(0xe7, 150, 9), Utils.FromArgb(0xf6, 0xe8, 0x4a), Utils.FromArgb(0xb1, 0xa2, 0xa7), Utils.FromArgb(0xc9, 0xa7, 0x84), Utils.FromArgb(140, 0x79, 0x51), Utils.FromArgb(0xd8, 0xcd, 0xb7), Utils.FromArgb(8, 0x65, 0x53), Utils.FromArgb(0xf7, 0xd8, 0x7b), Utils.FromArgb(1, 100, 0x84) };
        public static Color[] WarmPalette = new Color[] { Utils.FromArgb(0xf3, 0xea, 0x8d), Utils.FromArgb(0xf2, 0xc0, 0x5d), Utils.FromArgb(0xf3, 0x9c, 0x35), Utils.FromArgb(0xf5, 0x81, 0x1c), Utils.FromArgb(0xf3, 0x6b, 0x15), Utils.FromArgb(0xf1, 0x4c, 20), Utils.FromArgb(230, 0x18, 10), Utils.FromArgb(0xb3, 8, 14) };
        public static Color[] WebPalette = new Color[] { Utils.FromArgb(0xff, 0xa5, 0), Utils.FromArgb(0, 0, 0xce), Utils.FromArgb(0, 0xce, 0), Utils.FromArgb(0xff, 0xff, 0x40), Utils.FromArgb(0x40, 0xff, 0xff), Utils.FromArgb(0xff, 0x40, 0xff), Utils.FromArgb(0xff, 0x40, 0), Utils.FromArgb(0x80, 0x80, 0xa5), Utils.FromArgb(0x80, 0x80, 0x40) };
        public static Color[] WindowsVistaPalette = new Color[] { Utils.FromArgb(0, 0x1f, 210), Utils.FromArgb(0xe0, 2, 1), Utils.FromArgb(30, 0x66, 2), Utils.FromArgb(0xe8, 0xcd, 0x7e), Utils.FromArgb(0xaf, 0xab, 0xac), Utils.FromArgb(0xa4, 0xd0, 0xd9), Utils.FromArgb(0x3d, 0x3b, 60), Utils.FromArgb(0x95, 0xdd, 0x31), Utils.FromArgb(0x9e, 0, 1), Utils.FromArgb(220, 0xf7, 0x74), Utils.FromArgb(0x45, 0xfd, 0xfd), Utils.FromArgb(0xd1, 0x8e, 0x74), Utils.FromArgb(160, 0xd8, 0x91), Utils.FromArgb(0xd5, 0x7a, 0x65), Utils.FromArgb(150, 0x95, 0xd9) };
        public static Color[] WindowsXPPalette = new Color[] { Utils.FromArgb(130, 0x9b, 0xfe), Utils.FromArgb(0xfc, 0xd1, 0x24), Utils.FromArgb(0x7c, 0xbc, 13), Utils.FromArgb(0xfd, 0x85, 0x2f), Utils.FromArgb(0xfd, 0xfe, 0xfc), Utils.FromArgb(0xe2, 0x4e, 0x21), Utils.FromArgb(0x29, 0x38, 0xd6), Utils.FromArgb(0xb7, 0x94, 0), Utils.FromArgb(90, 0x86, 0), Utils.FromArgb(210, 70, 0), Utils.FromArgb(0xd3, 0xe5, 250), Utils.FromArgb(0xd8, 0xd8, 0xd8), Utils.FromArgb(0x5f, 0x71, 0x7b) };

        public Theme(Chart c) : base(c)
        {
        }

        public void Apply()
        {
            this.Apply(base.Chart);
        }

        public abstract void Apply(Chart AChart);
        public static void ApplyChartTheme(Theme ThemeClass, Chart CustomChart)
        {
            ApplyChartTheme(ThemeClass, CustomChart, -1);
        }

        public static void ApplyChartTheme(Stream ThemeStream, Chart CustomChart)
        {
            CustomTheme theme = new CustomTheme(CustomChart);
            ReadFromXML(ThemeStream);
            theme.Apply();
        }

        public static void ApplyChartTheme(string ThemeFile, Chart CustomChart)
        {
            CustomTheme theme = new CustomTheme(CustomChart);
            ReadFromXML(ThemeFile);
            theme.Apply();
        }

        public static void ApplyChartTheme(Theme ThemeClass, Chart CustomChart, int PaletteIndex)
        {
            ApplyChartTheme(ThemeClass, CustomChart, PaletteIndex);
        }

        public static void ApplyChartTheme(Stream ThemeStream, Chart CustomChart, int PaletteIndex)
        {
            ApplyChartTheme(ThemeStream, CustomChart, PaletteIndex);
        }

        private static void ApplyChartTheme(object ThemeClassOrStream, Chart CustomChart, int PaletteIndex)
        {
            if (ThemeClassOrStream is Theme)
            {
                (ThemeClassOrStream as Theme).Apply(CustomChart);
            }
            else if (ThemeClassOrStream is Stream)
            {
                ApplyChartTheme(ThemeClassOrStream as Stream, CustomChart);
            }
            Steema.TeeChart.Themes.ColorPalettes.ApplyPalette(CustomChart, PaletteIndex);
        }

        public static void ReadFromXML(Stream stream)
        {
            ThemeProperties.Instance.Read(stream);
        }

        public static void ReadFromXML(string fileName)
        {
            ThemeProperties.Instance.Read(fileName);
        }

        public static void RegisterChartThemes()
        {
            if (chartThemes == null)
            {
                chartThemes = new List<Type>();
                chartThemes.Add(typeof(BlackIsBackTheme));
                chartThemes.Add(typeof(OperaTheme));
                chartThemes.Add(typeof(TeeChartTheme));
                chartThemes.Add(typeof(ExcelTheme));
                chartThemes.Add(typeof(ClassicTheme));
                chartThemes.Add(typeof(XPTheme));
                chartThemes.Add(typeof(WebTheme));
                chartThemes.Add(typeof(BusinessTheme));
                chartThemes.Add(typeof(BlueSkyTheme));
                chartThemes.Add(typeof(GrayscaleTheme));
            }
        }

        public override string ToString()
        {
            return Texts.DefaultTheme;
        }

        public void WriteToXML(Stream stream)
        {
            ThemeProperties.Instance.Write(stream);
        }

        public void WriteToXML(string fileName)
        {
            ThemeProperties.Instance.Write(fileName);
        }

        public static List<Type> ChartThemes
        {
            get
            {
                RegisterChartThemes();
                return chartThemes;
            }
        }

        public static Steema.TeeChart.Themes.ColorPalettes ColorPalettes
        {
            get
            {
                if (colorPalettes == null)
                {
                    colorPalettes = new Steema.TeeChart.Themes.ColorPalettes();
                    colorPalettes.Add("TeeChart");
                    colorPalettes.Add("Excel");
                    colorPalettes.Add("Victorian");
                    colorPalettes.Add("Pastels");
                    colorPalettes.Add("Solid");
                    colorPalettes.Add("Classic");
                    colorPalettes.Add("Web");
                    colorPalettes.Add("Modern");
                    colorPalettes.Add("Rainbow");
                    colorPalettes.Add("Win. XP");
                    colorPalettes.Add("MacOS");
                    colorPalettes.Add("Windows Vista");
                    colorPalettes.Add("Grayscale");
                    colorPalettes.Add("Opera");
                    colorPalettes.Add("Warm");
                    colorPalettes.Add("Cool");
                    colorPalettes.Add("OnBlack");
                }
                return colorPalettes;
            }
        }
    }
}

