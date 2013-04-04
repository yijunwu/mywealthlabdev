namespace QWhale.Common
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class XPThemes
    {
        private static XPThemeName currentTheme = XPThemeName.None;
        private static Point offset;
        private static XPTheme theme = new XPTheme();

        static XPThemes()
        {
            InitTheme();
        }

        private static void DetectCurrentTheme()
        {
            string getCurrentThemeName = theme.GetCurrentThemeName;
            if ((getCurrentThemeName == null) || (getCurrentThemeName.Length == 0))
            {
                currentTheme = XPThemeName.Blue;
            }
            else if (getCurrentThemeName == "NormalColor")
            {
                currentTheme = XPThemeName.Blue;
            }
            else if (getCurrentThemeName == "HomeStead")
            {
                currentTheme = XPThemeName.HomeStead;
            }
            else if (getCurrentThemeName == "Metallic")
            {
                currentTheme = XPThemeName.Metallic;
            }
            else
            {
                currentTheme = XPThemeName.Custom;
            }
        }

        public static void DoneTheme()
        {
            theme.Close();
        }

        public static void DrawBackground(IPainter painter, Rectangle rect, Color startColor, Color endColor)
        {
            if (painter != null)
            {
                if (currentTheme == XPThemeName.None)
                {
                    painter.FillGradient(rect, startColor, endColor, rect.Location, new Point(0, rect.Height));
                }
                else
                {
                    theme.DrawOutbarBackground(painter, DrawRect(rect));
                }
            }
        }

        public static void DrawCheckBox(IPainter painter, Rectangle rect, bool pressed, bool hot)
        {
            theme.DrawCheckBox(painter, rect, pressed, hot);
        }

        public static void DrawEditBorder(IPainter painter, Rectangle rect)
        {
            theme.DrawEditBorder(painter, rect);
        }

        public static void DrawEditBorder(IntPtr dc, Rectangle rect)
        {
            theme.DrawEditBorder(dc, rect);
        }

        public static void DrawGroupBackground(IPainter painter, Rectangle rect)
        {
            if (painter != null)
            {
                switch (currentTheme)
                {
                    case XPThemeName.None:
                        painter.FillRectangle(Consts.BlueItemsAreaColor, rect);
                        return;

                    case XPThemeName.HomeStead:
                        painter.FillRectangle(Consts.HomeSteadItemsAreaColor, rect);
                        return;

                    case XPThemeName.Metallic:
                        painter.FillRectangle(Consts.MetallicItemsAreaColor, rect);
                        return;
                }
                theme.DrawGroupBackground(painter, DrawRect(rect));
            }
        }

        public static void DrawPushButton(IPainter painter, Rectangle rect, bool pressed, bool hot)
        {
            theme.DrawPushButton(painter, rect, pressed, hot);
        }

        public static Rectangle DrawRect(Rectangle sourceRect)
        {
            return new Rectangle(sourceRect.X + offset.X, sourceRect.Y + offset.Y, sourceRect.Width, sourceRect.Height);
        }

        public static Color GetGroupCaptionColor(bool hot)
        {
            switch (currentTheme)
            {
                case XPThemeName.None:
                case XPThemeName.Blue:
                    if (!hot)
                    {
                        return Consts.DefaultXPHeaderForeColor;
                    }
                    return Consts.DefaultXPItemsForeColor;

                case XPThemeName.HomeStead:
                    if (!hot)
                    {
                        return Color.FromArgb(0x56, 0x66, 0x2d);
                    }
                    return Color.FromArgb(0x72, 0x92, 0x1d);

                case XPThemeName.Metallic:
                    if (!hot)
                    {
                        return Color.FromArgb(0x3f, 0x3d, 0x3d);
                    }
                    return Color.FromArgb(0x7e, 0x7c, 0x7c);
            }
            return theme.GetGroupCaptionColor(hot);
        }

        public static Color GetGroupItemCaptionColor(bool hot)
        {
            return GetGroupCaptionColor(hot);
        }

        public static void InitTheme()
        {
            theme.Open(string.Empty);
            if (theme.themesAvailable)
            {
                DetectCurrentTheme();
            }
            else
            {
                currentTheme = XPThemeName.None;
            }
        }

        public static XPThemeName CurrentTheme
        {
            get
            {
                return currentTheme;
            }
        }

        public static Point Offset
        {
            get
            {
                return offset;
            }
            set
            {
                offset = value;
            }
        }

        private class XPTheme
        {
            private const int BP_CHECKBOX = 3;
            private const int BP_PUSHBUTTON = 1;
            private string buttonTheme = "Button";
            private const int CBS_CHECKEDNORMAL = 5;
            private const int CBS_UNCHECKEDHOT = 2;
            private const int CBS_UNCHECKEDNORMAL = 1;
            private string currentThemeName = string.Empty;
            private const int EBNG_HOT = 2;
            private const int EBNG_NORMAL = 1;
            private const int EBP_BACKGROUND = 0;
            private const int EBP_NORMALGROUP = 8;
            private const int EBP_NORMALGROUPBACKGROUND = 5;
            private const int EBP_NORMALGROUPCOLLAPSE = 6;
            private const int EBP_NORMALGROUPEXPAND = 7;
            private string editTheme = "edit";
            private const int EP_EDITTEXT = 1;
            private const int ETS_NORMAL = 1;
            private string explorerBarTheme = "explorerbar";
            private IntPtr handle = IntPtr.Zero;
            private const int PBS_HOT = 2;
            private const int PBS_NORMAL = 1;
            private const int PBS_PRESSED = 3;
            private Hashtable themeNames = new Hashtable();
            internal bool themesAvailable;
            private const int TMT_TEXTCOLOR = 0xedb;
            private const int TS_DRAW = 2;

            private void CheckThemes()
            {
                this.themesAvailable = (((OSFeature.Feature.GetVersionPresent(OSFeature.Themes) != null) && Win32.IsAppThemed()) && Win32.IsThemeActive()) && ((Win32.GetThemeAppProperties() & 2) == 2);
            }

            public void Close()
            {
                this.currentThemeName = string.Empty;
                this.themeNames.Clear();
                if (this.handle != IntPtr.Zero)
                {
                    Win32.CloseThemeData(this.handle);
                }
            }

            public void DrawCheckBox(IPainter painter, Rectangle rect, bool pressed, bool hot)
            {
                if (this.themesAvailable)
                {
                    int stateID = pressed ? 5 : (hot ? 2 : 1);
                    this.OpenTheme(this.buttonTheme);
                    this.DrawThemeBackground(painter, 3, stateID, rect);
                }
            }

            public void DrawEditBorder(IPainter painter, Rectangle rect)
            {
                if (this.themesAvailable)
                {
                    this.OpenTheme(this.editTheme);
                    this.DrawThemeBackground(painter, 1, 1, rect);
                }
            }

            public void DrawEditBorder(IntPtr dc, Rectangle rect)
            {
                if (this.themesAvailable)
                {
                    this.OpenTheme(this.editTheme);
                    this.DrawThemeBackground(dc, 1, 1, rect);
                }
            }

            public void DrawGroupBackground(IPainter painter, Rectangle rect)
            {
                this.OpenTheme(this.explorerBarTheme);
                this.DrawThemeBackground(painter, 5, 0, rect);
            }

            public void DrawGroupHeaderSymbol(IPainter painter, Rectangle rect, bool collapse, bool hot)
            {
                int num;
                int num2;
                if (collapse)
                {
                    num = 6;
                }
                else
                {
                    num = 7;
                }
                if (hot)
                {
                    num2 = 2;
                }
                else
                {
                    num2 = 1;
                }
                this.OpenTheme(this.explorerBarTheme);
                this.DrawThemeBackground(painter, num, num2, rect);
            }

            public void DrawHeaderBackground(IPainter painter, Rectangle rect)
            {
                this.OpenTheme(this.explorerBarTheme);
                this.DrawThemeBackground(painter, 8, 0, rect);
            }

            public void DrawOutbarBackground(IPainter painter, Rectangle rect)
            {
                this.OpenTheme(this.explorerBarTheme);
                this.DrawThemeBackground(painter, 0, 0, rect);
            }

            public void DrawPushButton(IPainter painter, Rectangle rect, bool pressed, bool hot)
            {
                if (this.themesAvailable)
                {
                    int stateID = pressed ? 3 : (hot ? 2 : 1);
                    this.OpenTheme(this.buttonTheme);
                    this.DrawThemeBackground(painter, 1, stateID, rect);
                }
            }

            public void DrawThemeBackground(IPainter painter, int partID, int stateID, Rectangle rect)
            {
                painter.DrawThemeBackground(this.handle, partID, stateID, rect);
            }

            public void DrawThemeBackground(IntPtr dc, int partID, int stateID, Rectangle rect)
            {
                Win32.GdiRect rect2 = new Win32.GdiRect(rect);
                Win32.DrawThemeBackground(this.handle, dc, partID, stateID, ref rect2, IntPtr.Zero);
            }

            public Color GetGroupCaptionColor(bool hot)
            {
                Win32.ColorRef ref2;
                Win32.GetThemeColor(this.handle, 8, 0, 0xedb, out ref2);
                return ref2.Color;
            }

            public void Open(string name)
            {
                this.CheckThemes();
                if (this.themesAvailable && (name != string.Empty))
                {
                    this.handle = Win32.OpenThemeData(IntPtr.Zero, name);
                }
            }

            public void OpenTheme(string name)
            {
                if (this.currentThemeName != name)
                {
                    object obj2 = this.themeNames[name];
                    if (obj2 != null)
                    {
                        this.handle = (IntPtr) obj2;
                    }
                    else
                    {
                        this.handle = Win32.OpenThemeData(IntPtr.Zero, name);
                        this.themeNames.Add(name, this.handle);
                    }
                    this.currentThemeName = name;
                }
            }

            public string GetCurrentThemeName
            {
                get
                {
                    string str;
                    int maxNameChars = 260;
                    IntPtr themeFileName = Marshal.AllocHGlobal((int) (maxNameChars + 1));
                    IntPtr colorName = Marshal.AllocHGlobal((int) (maxNameChars + 1));
                    try
                    {
                        Win32.GetCurrentThemeName(themeFileName, maxNameChars, colorName, maxNameChars, IntPtr.Zero, IntPtr.Zero);
                        str = Marshal.PtrToStringAuto(colorName);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(themeFileName);
                        Marshal.FreeHGlobal(colorName);
                    }
                    return str;
                }
            }
        }
    }
}

