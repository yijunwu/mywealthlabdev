namespace QWhale.Common
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    internal class Win32
    {
        public const int BDR_INNER = 12;
        public const int BDR_OUTER = 3;
        public const int BDR_RAISED = 5;
        public const int BDR_RAISEDINNER = 4;
        public const int BDR_RAISEDOUTER = 1;
        public const int BDR_SUNKEN = 10;
        public const int BDR_SUNKENINNER = 8;
        public const int BDR_SUNKENOUTER = 2;
        public const int BF_ADJUST = 0x2000;
        public const int BF_BOTTOM = 8;
        public const int BF_FLAT = 0x4000;
        public const int BF_LEFT = 1;
        public const int BF_MIDDLE = 0x800;
        public const int BF_MONO = 0x8000;
        public const int BF_RIGHT = 4;
        public const int BF_SOFT = 0x1000;
        public const int BF_TOP = 2;
        public const int BS_PATTERN = 3;
        public const int BS_SOLID = 0;
        public const int CFS_POINT = 2;
        public const int CLIP_DEFAULT_PRECIS = 0;
        public const int DEFAULT_CHARSET = 1;
        public const int DEFAULT_PITCH = 0;
        public const int DEFAULT_QUALITY = 0;
        public const int DT_BOTTOM = 8;
        public const int DT_CALCRECT = 0x400;
        public const int DT_CENTER = 1;
        public const int DT_END_ELLIPSIS = 0x8000;
        public const int DT_HIDEPREFIX = 0x100000;
        public const int DT_LEFT = 0;
        public const int DT_NOCLIP = 0x100;
        public const int DT_NOPREFIX = 0x800;
        public const int DT_PATH_ELLIPSIS = 0x4000;
        public const int DT_RIGHT = 2;
        public const int DT_RTLREADING = 0x20000;
        public const int DT_SINGLELINE = 0x20;
        public const int DT_TOP = 0;
        public const int DT_VCENTER = 4;
        public const int DT_WORD_ELLIPSIS = 0x40000;
        public const int EDGE_BUMP = 9;
        public const int EDGE_ETCHED = 6;
        public const int EDGE_RAISED = 5;
        public const int EDGE_SUNKEN = 10;
        public const int ETO_CLIPPED = 4;
        public const int ETO_OPAQUE = 2;
        public const int FSB_FLAT_MODE = 2;
        public const int FW_BOLD = 700;
        public const int FW_NORMAL = 400;
        public const int GCS_RESULTSTR = 0x800;
        public const int GM_ADVANCED = 2;
        public const int GM_COMPATIBLE = 1;
        public const int GWL_WNDPROC = -4;
        public const int HOLLOW_BRUSH = 5;
        public const int HWND_TOPMOST = -1;
        public const int IMC_GETCOMPOSITIONWINDOW = 11;
        public const int IMN_SETCOMPOSITIONWINDOW = 11;
        public const int IMN_SETOPENSTATUS = 8;
        public const int LOGPIXELSX = 0x58;
        public const int LOGPIXELSY = 90;
        public const int MB_ICONHAND = 0x10;
        public const int OPAQUE = 2;
        public const int OUT_DEFAULT_PRECIS = 0;
        public const int PS_DASH = 1;
        public const int PS_DASHDOT = 3;
        public const int PS_DASHDOTDOT = 4;
        public const int PS_DOT = 2;
        public const int PS_SOLID = 0;
        public const int RGN_AND = 1;
        public const int RGN_COPY = 5;
        public const int RGN_DIFF = 4;
        public const int RGN_MAX = 5;
        public const int RGN_MIN = 1;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;
        public const int SB_BOTTOM = 7;
        public const int SB_ENDSCROLL = 8;
        public const int SB_HORZ = 0;
        public const int SB_LEFT = 6;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINELEFT = 0;
        public const int SB_LINERIGHT = 1;
        public const int SB_LINEUP = 0;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGERIGHT = 3;
        public const int SB_PAGEUP = 2;
        public const int SB_RIGHT = 7;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_VERT = 1;
        public const int Scroll_Size = 0x1c;
        public const int SIF_DISABLENOSCROLL = 8;
        public const int SIF_PAGE = 2;
        public const int SIF_POS = 4;
        public const int SIF_RANGE = 1;
        public const int SIF_TRACKPOS = 0x10;
        public const int SM_CXHSCROLL = 0x15;
        public const int SM_CYVSCROLL = 20;
        public const int SM_DBCSENABLED = 0x2a;
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        public const int SWP_NOACTIVATE = 0x10;
        public const int SWP_NOMOVE = 2;
        public const int SWP_NOSIZE = 1;
        public const int SWP_NOZORDER = 4;
        public const int SWP_SHOWWINDOW = 0x40;
        public const int TMPF_DEVICE = 8;
        public const int TMPF_FIXED_PITCH = 1;
        public const int TMPF_TRUETYPE = 4;
        public const int TMPF_VECTOR = 2;
        public const int TRANSPARENT = 1;
        public const int WH_CALLWNDPROC = 4;
        public const int WH_GETMESSAGE = 3;
        public const int WH_MOUSE = 7;
        public const int WM_CANUNDO = 0xc6;
        public const int WM_CHAR = 0x102;
        public const int WM_CLEAR = 0x303;
        public const int WM_CLOSE = 0x10;
        public const int WM_CLOSEDROPPED = 0x401;
        public const int WM_COPY = 0x301;
        public const int WM_CUT = 0x300;
        public const int WM_DESTROY = 2;
        public const int WM_GETTEXT = 13;
        public const int WM_HSCROLL = 0x114;
        public const int WM_IME_COMPOSITION = 0x10f;
        public const int WM_IME_CONTROL = 0x283;
        public const int WM_IME_NOTIFY = 0x282;
        public const int WM_IME_STARTCOMPOSITION = 0x10d;
        public const int WM_INITDIALOG = 0x110;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_MAXCLICK = 520;
        public const int WM_MAXNCCLICK = 0xa9;
        public const int WM_MINCLICK = 0x201;
        public const int WM_MINNCCLICK = 0xa1;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_NCACTIVATE = 0x86;
        public const int WM_NCDESTROY = 130;
        public const int WM_NCLBUTTONDOWN = 0xa1;
        public const int WM_NCPAINT = 0x85;
        public const int WM_PASTE = 770;
        public const int WM_SETCURSOR = 0x20;
        public const int WM_SETTEXT = 12;
        public const int WM_SIZE = 5;
        public const int WM_SYSKEYDOWN = 260;
        public const int WM_UNDO = 0x304;
        public const int WM_VSCROLL = 0x115;
        public const int WS_BORDER = 0x800000;
        public const int WS_EX_CLIENTEDGE = 0x200;
        public const int WS_EX_DLGMODALFRAME = 1;
        public const int WS_EX_WINDOWEDGE = 0x100;
        public const int WS_HSCROLL = 0x100000;
        public const int WS_VSCROLL = 0x200000;
        public const int WSB_PROP_HSTYLE = 0x200;
        public const int WSB_PROP_VSTYLE = 0x100;

        private Win32()
        {
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wparam, IntPtr lparam);
        [DllImport("uxtheme.dll")]
        public static extern int CloseThemeData(IntPtr htheme);
        public static int ColorToGdiColor(Color color)
        {
            int num = color.ToArgb() & 0xffffff;
            return (((num >> 0x10) + (num & 0xff00)) + ((num & 0xff) << 0x10));
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool CreateCaret(IntPtr handle, IntPtr hBitmap, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        private static extern IntPtr CreateFont(int nHeight, int nWidth, int nEscapement, int nOrientaion, int fnWeight, uint fdwItalic, uint fdwUnderline, uint fdwStrikeOut, uint fdwCharSet, uint fdwOutputPrecision, uint fdwClipPrecision, uint fdwQuality, uint fdwPitchAndFamily, string lpszFace);
        public static IntPtr CreateFontIndirect(string fontName, FontStyle fontStyle, int fontHeight)
        {
            return CreateFont(fontHeight, 0, 0, 0, (((fontStyle & FontStyle.Bold) != FontStyle.Regular) ? 700 : 400), (uint)(((fontStyle & FontStyle.Italic) != FontStyle.Regular) ? 1 : 0), (uint)(((fontStyle & FontStyle.Underline) != FontStyle.Regular) ? 1 : 0), (uint)(((fontStyle & FontStyle.Strikeout) != FontStyle.Regular) ? 1 : 0), 1, 0, 0, 0, 0, fontName);
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int style, int width, int color);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgnIndirect(ref GdiRect rect);
        public static IntPtr CreateRectRgnIndirect(Rectangle rect)
        {
            GdiRect rect2 = new GdiRect(rect.Left, rect.Top, rect.Right, rect.Bottom);
            return CreateRectRgnIndirect(ref rect2);
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int color);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr p1);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool DestroyCaret();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        private static extern bool DrawEdge(IntPtr hdc, ref GdiRect rect, int edge, int flags);
        public static bool DrawEdge(IntPtr hdc, ref Rectangle rect, int edge, int flags)
        {
            GdiRect rect2 = new GdiRect(rect);
            bool flag = DrawEdge(hdc, ref rect2, edge, flags);
            rect = new Rectangle(rect2.left, rect2.top, rect2.right - rect2.left, rect2.bottom - rect2.top);
            return flag;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool DrawFocusRect(IntPtr hdc, ref GdiRect rect);
        public static bool DrawFocusRect(IntPtr hdc, int x, int y, int w, int h)
        {
            GdiRect rect = new GdiRect(x, y, x + w, y + h);
            return DrawFocusRect(hdc, ref rect);
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        private static extern int DrawText(IntPtr hdc, string s, int len, ref GdiRect rect, int format);
        public static int DrawText(IntPtr hdc, string s, int len, ref Rectangle rect, int format)
        {
            GdiRect rect2 = new GdiRect(rect);
            int num = DrawText(hdc, s, len, ref rect2, format);
            rect = rect2.ToRectangle();
            return num;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("uxtheme.dll")]
        public static extern bool DrawThemeBackground(IntPtr theme, IntPtr hdc, int partID, int stateID, ref GdiRect rect, IntPtr clipRect);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr hwndParent, EnumChildProc lpEnumFunc, IntPtr lparam);
        [DllImport("gdi32.dll")]
        public static extern int ExcludeClipRect(IntPtr hdc, int l, int t, int r, int b);
        public static int ExtTextOut(IntPtr hdc, Rectangle rect, int x, int y, int options, string s, int len, int[] spacings)
        {
            int num3;
            GdiRect rect2 = new GdiRect(rect);
            if (len < 0)
            {
                len = s.Length;
            }
            IntPtr ptr = (spacings != null) ? Marshal.AllocHGlobal((int) (len * 4)) : IntPtr.Zero;
            try
            {
                if (ptr != IntPtr.Zero)
                {
                    int length = spacings.Length;
                    for (int i = 0; i < len; i++)
                    {
                        Marshal.WriteInt32(ptr, i * 4, (i < length) ? spacings[i] : 0);
                    }
                }
                num3 = ExtTextOut(hdc, x, y, options, ref rect2, s, len, ptr);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            return num3;
        }

        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true), DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        private static extern int ExtTextOut(IntPtr hdc, int x, int y, int options, ref GdiRect rect, string s, int len, IntPtr buffer);
        [DllImport("user32.dll")]
        public static extern int FillRect(IntPtr hdc, ref GdiRect rect, IntPtr brush);
        public static int FillRect(IntPtr hdc, int x, int y, int w, int h, IntPtr brush)
        {
            GdiRect rect = new GdiRect(x, y, x + w, y + h);
            return FillRect(hdc, ref rect, brush);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        private static extern bool FlatSB_GetScrollInfo(IntPtr hwnd, int barFlag, ref ScrollInfo scrollInfo);
        public static int FlatSB_GetScrollPos(IntPtr handle, int code)
        {
            ScrollInfo info;
            info.cbSize = 0x1c;
            info.fMask = 0x10;
            info.nMin = 0;
            info.nMax = 0;
            info.nPage = 0;
            info.nPos = 0;
            info.nTrackPos = 0;
            FlatSB_GetScrollInfo(handle, code, ref info);
            return info.nTrackPos;
        }

        public static int FlatSB_GetScrollSize(IntPtr handle, int code)
        {
            ScrollInfo info;
            info.cbSize = 0x1c;
            info.fMask = 3;
            info.nMin = 0;
            info.nMax = 0;
            info.nPage = 0;
            info.nPos = 0;
            info.nTrackPos = 0;
            FlatSB_GetScrollInfo(handle, code, ref info);
            if (info.nPage <= info.nMax)
            {
                return (int) info.nPage;
            }
            return -1;
        }

        public static void FlatSB_SetScrollBarInfo(IntPtr handle, int size, int pageSize, int code)
        {
            if (size < pageSize)
            {
                size = pageSize;
            }
            FlatSB_SetScrollSize(handle, code, 0, size, pageSize);
        }

        [DllImport("comctl32.dll")]
        private static extern int FlatSB_SetScrollInfo(IntPtr hwnd, int barFlag, ref ScrollInfo scrollInfo, [MarshalAs(UnmanagedType.Bool)] bool redraw);
        public static void FlatSB_SetScrollPos(IntPtr handle, int code, int pos)
        {
            ScrollInfo info;
            info.cbSize = 0x1c;
            info.fMask = 4;
            info.nMin = 0;
            info.nMax = 0;
            info.nPage = 0;
            info.nPos = pos;
            info.nTrackPos = 0;
            FlatSB_SetScrollInfo(handle, code, ref info, true);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        public static extern bool FlatSB_SetScrollProp(IntPtr hwnd, int index, IntPtr newValue, [MarshalAs(UnmanagedType.Bool)] bool setValue);
        public static void FlatSB_SetScrollSize(IntPtr handle, int code, int minPos, int maxPos, int pageSize)
        {
            ScrollInfo info;
            info.cbSize = 0x1c;
            info.fMask = 3;
            info.nMin = minPos;
            info.nMax = maxPos;
            info.nPage = (uint) pageSize;
            info.nPos = 0;
            info.nTrackPos = 0;
            FlatSB_SetScrollInfo(handle, code, ref info, true);
        }

        [DllImport("user32.dll")]
        public static extern int FrameRect(IntPtr hdc, ref GdiRect rect, IntPtr brush);
        public static int FrameRect(IntPtr hdc, int x, int y, int w, int h, IntPtr brush)
        {
            GdiRect rect = new GdiRect(x, y, x + w, y + h);
            return FrameRect(hdc, ref rect, brush);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true), DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        private static extern bool GetCharABCWidths(IntPtr hdc, int iFirstChar, int iLastChar, IntPtr buffer);
        public static bool GetCharABCWidths(IntPtr hdc, int iFirstChar, int iLastChar, ref int[] buffer)
        {
            bool flag = true;
            IntPtr ptr = Marshal.AllocHGlobal((int) (buffer.Length * 12));
            try
            {
                flag = GetCharABCWidths(hdc, iFirstChar, iLastChar, ptr);
                if (!flag)
                {
                    return flag;
                }
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = (Marshal.ReadInt32(ptr, i * 12) + Marshal.ReadInt32(ptr, (i * 12) + 4)) + Marshal.ReadInt32(ptr, (i * 12) + 8);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return flag;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true), DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        private static extern bool GetCharWidth32(IntPtr hdc, int iFirstChar, int iLastChar, IntPtr buffer);
        public static bool GetCharWidth32(IntPtr hdc, int iFirstChar, int iLastChar, ref int[] buffer)
        {
            bool flag = true;
            IntPtr ptr = Marshal.AllocHGlobal((int) (buffer.Length * 4));
            try
            {
                flag = GetCharWidth32(hdc, iFirstChar, iLastChar, ptr);
                if (!flag)
                {
                    return flag;
                }
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Marshal.ReadInt32(ptr, i * 4);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return flag;
        }

        public static string GetClassName(IntPtr hwnd)
        {
            string str;
            int nMaxCount = 0xff;
            IntPtr lpClassName = Marshal.AllocHGlobal((int) (nMaxCount + 1));
            try
            {
                int len = GetClassName(hwnd, lpClassName, nMaxCount);
                str = (len > 0) ? Marshal.PtrToStringAnsi(lpClassName, len) : string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(lpClassName);
            }
            return str;
        }

        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true), DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hwnd, IntPtr lpClassName, int nMaxCount);
        [DllImport("gdi32.dll")]
        public static extern int GetClipRgn(IntPtr handle, IntPtr rgn);
        [DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        public static extern int GetCurrentThemeName(IntPtr themeFileName, int maxNameChars, IntPtr colorName, int maxColorName, IntPtr dummy1, IntPtr dummy2);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int index);
        [DllImport("user32.dll")]
        public static extern int GetDoubleClickTime();
        [DllImport("gdi32.dll")]
        public static extern int GetGraphicsMode(IntPtr hdc);
        [DllImport("user32.dll")]
        private static extern int GetScrollInfo(IntPtr hwnd, int barFlag, ref ScrollInfo scrollInfo);
        public static int GetScrollPos(IntPtr handle, int code)
        {
            ScrollInfo info;
            info.cbSize = 0x1c;
            info.fMask = 0x10;
            info.nMin = 0;
            info.nMax = 0;
            info.nPage = 0;
            info.nPos = 0;
            info.nTrackPos = 0;
            GetScrollInfo(handle, code, ref info);
            return info.nTrackPos;
        }

        public static int GetScrollSize(IntPtr handle, int code)
        {
            ScrollInfo info;
            info.cbSize = 0x1c;
            info.fMask = 3;
            info.nMin = 0;
            info.nMax = 0;
            info.nPage = 0;
            info.nPos = 0;
            info.nTrackPos = 0;
            GetScrollInfo(handle, code, ref info);
            if (info.nPage <= info.nMax)
            {
                return (int) info.nPage;
            }
            return -1;
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetStockObject(int index);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        public static string GetText(IntPtr hwnd)
        {
            string str;
            int num = 0xff;
            IntPtr lparam = Marshal.AllocHGlobal((int) (num + 1));
            try
            {
                int len = (int) SendMessage(hwnd, 13, (IntPtr) num, lparam);
                str = (len > 0) ? Marshal.PtrToStringAnsi(lparam, len) : string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(lparam);
            }
            return str;
        }

        public static bool GetTextExtentExPoint(IntPtr hdc, string s, int len, ref int[] buffer, ref Size size)
        {
            bool flag = true;
            IntPtr alpDx = Marshal.AllocHGlobal((int) (buffer.Length * 4));
            try
            {
                GdiSize size2 = new GdiSize();
                flag = GetTextExtentExPoint(hdc, s, len, 0, IntPtr.Zero, alpDx, ref size2);
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Marshal.ReadInt32(alpDx, i * 4);
                }
                size = size2.ToSize();
            }
            finally
            {
                Marshal.FreeHGlobal(alpDx);
            }
            return flag;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true), DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        private static extern bool GetTextExtentExPoint(IntPtr hdc, string s, int len, int maxExtent, IntPtr lpnFit, IntPtr alpDx, ref GdiSize size);
        [return: MarshalAs(UnmanagedType.Bool)]
        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true), DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetTextExtentPoint32(IntPtr hdc, string s, int count, ref GdiSize size);
        public static bool GetTextExtentPoint32(IntPtr hdc, string s, int count, ref Size size)
        {
            GdiSize size2 = new GdiSize(0, 0);
            bool flag = GetTextExtentPoint32(hdc, s, count, ref size2);
            size.Width = size2.cx;
            size.Height = size2.cy;
            return flag;
        }

        [DllImport("gdi32.dll")]
        public static extern int GetTextMetrics(IntPtr hdc, ref TextMetrics metrics);
        [DllImport("uxtheme.dll")]
        public static extern int GetThemeAppProperties();
        [DllImport("uxtheme.dll")]
        public static extern int GetThemeColor(IntPtr htheme, int partID, int stateID, int propID, out ColorRef colorRef);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DrawEx(IntPtr handle, int index, IntPtr hdc, int x, int y, int dx, int dy, int bk, int fr, int style);
        public static void ImeComposition(IntPtr hWnd)
        {
            string lpBuf = string.Empty;
            IntPtr hImc = ImmGetContext(hWnd);
            try
            {
                if (hImc != IntPtr.Zero)
                {
                    int count = ImmGetCompositionString(hImc, 0x800, ref lpBuf, 0);
                    if (count > 0)
                    {
                        lpBuf = new string(' ', count);
                        ImmGetCompositionString(hImc, 0x800, ref lpBuf, count);
                    }
                }
            }
            finally
            {
                ImmReleaseContext(hWnd, hImc);
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Imm32.dll")]
        public static extern bool ImmGetCandidateWindow(IntPtr hIMC, int dwIndex, CANDIDATEFORM lpCandidate);
        [return: MarshalAs(UnmanagedType.I4)]
        [DllImport("Imm32.dll", CharSet=CharSet.Auto)]
        public static extern int ImmGetCompositionString(IntPtr hImc, int dwIndex, ref string lpBuf, int dwLen);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Imm32.dll")]
        public static extern bool ImmGetCompositionWindow(IntPtr hIMC, COMPOSITIONFORM lpCompForm);
        [DllImport("Imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Imm32.dll")]
        public static extern bool ImmGetOpenStatus(IntPtr hIMC);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Imm32.dll")]
        public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Imm32.dll")]
        public static extern bool ImmSetCandidateWindow(IntPtr hIMC, int dwIndex, CANDIDATEFORM lpCandidate);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Imm32.dll")]
        public static extern bool ImmSetCompositionFont(IntPtr hIMC, LOGFONT logFont);
        public static void ImmSetCompositionFont(IntPtr hWnd, Font font)
        {
            IntPtr hIMC = ImmGetContext(hWnd);
            try
            {
                if (ImmGetOpenStatus(hIMC))
                {
                    LOGFONT logFont = new LOGFONT();
                    font.ToLogFont(logFont);
                    ImmSetCompositionFont(hIMC, logFont);
                }
            }
            finally
            {
                ImmReleaseContext(hWnd, hIMC);
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Imm32.dll")]
        public static extern bool ImmSetCompositionWindow(IntPtr hIMC, ref COMPOSITIONFORM lpCompForm);
        public static void ImmSetCompositionWindow(IntPtr hWnd, Point pos)
        {
            GdiPoint point = new GdiPoint(pos.X, pos.Y);
            COMPOSITIONFORM lpCompForm = new COMPOSITIONFORM();
            IntPtr hIMC = ImmGetContext(hWnd);
            try
            {
                if (ImmGetOpenStatus(hIMC))
                {
                    lpCompForm.dwStyle = 2;
                    lpCompForm.ptCurrentPos = point;
                    lpCompForm.rcArea = new GdiRect(0, 0, 0, 0);
                    ImmSetCompositionWindow(hIMC, ref lpCompForm);
                }
            }
            finally
            {
                ImmReleaseContext(hWnd, hIMC);
            }
        }

        public static bool InitCommonControls(int icc)
        {
            INITCOMMONCONTROLSEX iic = new INITCOMMONCONTROLSEX {
                dwSize = 0x10,
                dwICC = icc
            };
            return InitCommonControlsEx(ref iic);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        private static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX iic);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        public static extern bool InitializeFlatSB(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        public static extern int IntersectClipRect(IntPtr hdc, int l, int t, int r, int b);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("uxtheme.dll")]
        public static extern bool IsAppThemed();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("uxtheme.dll")]
        public static extern bool IsThemeActive();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool LineTo(IntPtr hdc, int x, int y);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool MessageBeep(int type);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool MoveToEx(IntPtr hdc, int x, int y, IntPtr p);
        [DllImport("uxtheme.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr OpenThemeData(IntPtr hwnd, string classList);
        [return: MarshalAs(UnmanagedType.Bool)]
        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true), DllImport("gdi32.dll")]
        public static extern bool PolyBezier(IntPtr hdc, IntPtr points, int count);
        public static bool PolyBezier(IntPtr hdc, Point[] points, int count)
        {
            bool flag;
            IntPtr ptr = Marshal.AllocHGlobal((int) (count * 8));
            try
            {
                for (int i = 0; i < count; i++)
                {
                    Marshal.WriteInt32(ptr, i * 8, points[i].X);
                    Marshal.WriteInt32(ptr, (i * 8) + 4, points[i].Y);
                }
                flag = PolyBezier(hdc, ptr, count);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return flag;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true), DllImport("gdi32.dll")]
        public static extern bool Polygon(IntPtr hdc, IntPtr points, int count);
        public static bool Polygon(IntPtr hdc, Point[] points, int count)
        {
            bool flag;
            IntPtr ptr = Marshal.AllocHGlobal((int) (count * 8));
            try
            {
                for (int i = 0; i < count; i++)
                {
                    Marshal.WriteInt32(ptr, i * 8, points[i].X);
                    Marshal.WriteInt32(ptr, (i * 8) + 4, points[i].Y);
                }
                flag = Polygon(hdc, ptr, count);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return flag;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool RoundRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidth, int nHeight);
        public static void ScrollWindow(IntPtr handle, int x, int y, Rectangle rect)
        {
            GdiRect r = new GdiRect(rect);
            ScrollWindow(handle, x, y, ref r, IntPtr.Zero);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        private static extern bool ScrollWindow(IntPtr handle, int x, int y, ref GdiRect r, IntPtr clipR);
        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr handle, IntPtr rgn);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("gdi32.dll")]
        public static extern int SetBkColor(IntPtr hdc, int color);
        [DllImport("gdi32.dll")]
        public static extern int SetBkMode(IntPtr hdc, int bkMode);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool SetCaretPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr handle);
        [DllImport("gdi32.dll")]
        public static extern int SetGraphicsMode(IntPtr hdc, int mode);
        public static int SetPixel(IntPtr hdc, int x, int y, Color color)
        {
            return SetPixel(hdc, x, y, ColorToGdiColor(color));
        }

        [DllImport("gdi32.dll")]
        public static extern int SetPixel(IntPtr hdc, int x, int y, int color);
        public static void SetScrollBarInfo(IntPtr handle, int size, int pageSize, int code)
        {
            if (size < pageSize)
            {
                size = pageSize;
            }
            SetScrollSize(handle, code, 0, size, pageSize);
        }

        [DllImport("user32.dll")]
        private static extern int SetScrollInfo(IntPtr hwnd, int barFlag, ref ScrollInfo scrollInfo, [MarshalAs(UnmanagedType.Bool)] bool redraw);
        public static void SetScrollPos(IntPtr handle, int code, int pos)
        {
            ScrollInfo info;
            info.cbSize = 0x1c;
            info.fMask = 4;
            info.nMin = 0;
            info.nMax = 0;
            info.nPage = 0;
            info.nPos = pos;
            info.nTrackPos = 0;
            SetScrollInfo(handle, code, ref info, true);
        }

        public static void SetScrollSize(IntPtr handle, int code, int minPos, int maxPos, int pageSize)
        {
            ScrollInfo info;
            info.cbSize = 0x1c;
            info.fMask = 3;
            info.nMin = minPos;
            info.nMax = maxPos;
            info.nPage = (uint) pageSize;
            info.nPos = 0;
            info.nTrackPos = 0;
            SetScrollInfo(handle, code, ref info, true);
        }

        public static void SetText(IntPtr hwnd, string text)
        {
            SendMessage(hwnd, 12, IntPtr.Zero, Marshal.StringToHGlobalAnsi(text));
        }

        [DllImport("gdi32.dll")]
        public static extern int SetTextColor(IntPtr hdc, int color);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookHandler lpfn, IntPtr hMod, int dwThreadId);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool SetWorldTransform(IntPtr hdc, ref XFORM p2);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool ShowCaret(IntPtr handle);
        [DllImport("kernel32.dll")]
        public static extern void Sleep(int milliSeconds);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        public static extern bool UninitializeFlatSB(IntPtr hwnd);
        public static void UpdateCompositionWindow(IntPtr hWnd, Point pos, IntPtr wndPos)
        {
            object obj2 = Marshal.PtrToStructure(wndPos, typeof(COMPOSITIONFORM));
            if (obj2 != null)
            {
                COMPOSITIONFORM compositionform1 = (COMPOSITIONFORM) obj2;
                new GdiPoint(pos.X, pos.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ABC
        {
            private int abcA;
            private uint abcB;
            private int abcC;
            public ABC(int a, uint b, int c)
            {
                this.abcA = a;
                this.abcB = b;
                this.abcC = c;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CANDIDATEFORM
        {
            public int dwIndex;
            public int dwStyle;
            public Win32.GdiPoint ptCurrentPos;
            public Win32.GdiRect rcArea;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ColorRef
        {
            public byte rgbRed;
            public byte rgbGreen;
            public byte rgbBlue;
            public byte rgbReserved;
            public System.Drawing.Color Color
            {
                get
                {
                    return System.Drawing.Color.FromArgb(this.rgbRed, this.rgbGreen, this.rgbBlue);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COMPOSITIONFORM
        {
            public uint dwStyle;
            public Win32.GdiPoint ptCurrentPos;
            public Win32.GdiRect rcArea;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdiPoint
        {
            public int X;
            public int Y;
            public GdiPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdiRect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public GdiRect(int l, int t, int r, int b)
            {
                this.left = l;
                this.top = t;
                this.right = r;
                this.bottom = b;
            }

            public GdiRect(Rectangle rect)
            {
                this.left = rect.Left;
                this.top = rect.Top;
                this.right = rect.Right;
                this.bottom = rect.Bottom;
            }

            public Rectangle ToRectangle()
            {
                return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdiSize
        {
            public int cx;
            public int cy;
            public GdiSize(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }

            public GdiSize(Size size)
            {
                this.cx = size.Width;
                this.cy = size.Height;
            }

            public Size ToSize()
            {
                return new Size(this.cx, this.cy);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INITCOMMONCONTROLSEX
        {
            [FieldOffset(8)]
            public int dwICC;
            [FieldOffset(0)]
            public int dwSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LogBrush
        {
            public int style;
            public int color;
            public IntPtr hatch;
            public LogBrush(int style, int color, IntPtr hatch)
            {
                this.style = style;
                this.color = color;
                this.hatch = hatch;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public class LOGFONT
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
            public string lfFaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT
        {
            public Win32.GdiPoint Pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wparam;
            public IntPtr lparam;
            public uint time;
            public Win32.GdiPoint Pt;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ScrollInfo
        {
            [FieldOffset(0)]
            public uint cbSize;
            [FieldOffset(4)]
            public uint fMask;
            [FieldOffset(12)]
            public int nMax;
            [FieldOffset(8)]
            public int nMin;
            [FieldOffset(0x10)]
            public uint nPage;
            [FieldOffset(20)]
            public int nPos;
            [FieldOffset(0x18)]
            public int nTrackPos;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct TextMetrics
        {
            [FieldOffset(4)]
            public int Accent;
            [FieldOffset(20)]
            public int AveCharWidth;
            [FieldOffset(0x39)]
            public byte BreakChar;
            [FieldOffset(0x34)]
            public byte CharSet;
            [FieldOffset(0x2e)]
            public byte DefaultChar;
            [FieldOffset(8)]
            public int Descent;
            [FieldOffset(0x24)]
            public int DigitizedAspectX;
            [FieldOffset(40)]
            public int DigitizedAspectY;
            [FieldOffset(0x10)]
            public int ExternalLeading;
            [FieldOffset(0x2c)]
            public byte FirstChar;
            [FieldOffset(0)]
            public int Height;
            [FieldOffset(0x3a)]
            public byte Italic;
            [FieldOffset(0x2d)]
            public byte LastChar;
            [FieldOffset(0x18)]
            public int MaxCharWidth;
            [FieldOffset(0x20)]
            public int Overhang;
            [FieldOffset(0x33)]
            public byte PitchAndFamily;
            [FieldOffset(12)]
            public int publicLeading;
            [FieldOffset(0x34)]
            private int Reserved;
            [FieldOffset(50)]
            public byte StruckOut;
            [FieldOffset(0x31)]
            public byte Underlined;
            [FieldOffset(0x1c)]
            public int Weight;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct XFORM
        {
            [FieldOffset(0x10)]
            public float eDx;
            [FieldOffset(20)]
            public float eDy;
            [FieldOffset(0)]
            public float eM11;
            [FieldOffset(4)]
            public float eM12;
            [FieldOffset(8)]
            public float eM21;
            [FieldOffset(12)]
            public float eM22;

            public XFORM(float m11, float m12, float m21, float m22, float dx, float dy)
            {
                this.eM11 = m11;
                this.eM12 = m12;
                this.eM21 = m21;
                this.eM22 = m22;
                this.eDx = dx;
                this.eDy = dy;
            }
        }
    }
}

