namespace QWhale.Common
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class OSUtils
    {
        public const int BorderFlags_Adjust = 0x2000;
        public const int BorderFlags_Flat = 0x4000;
        public const int BorderFlags_Mono = 0x8000;
        public const int BorderFlags_Soft = 0x1000;
        public const int GCS_RESULTSTR = 0x800;
        public const int IMC_GETCOMPOSITIONWINDOW = 11;
        public const int IMN_SETCOMPOSITIONWINDOW = 11;
        public const int IMN_SETOPENSTATUS = 8;
        public const int Message_CanUndo = 0xc6;
        public const int Message_CAPTURECHANGED = 0x215;
        public const int Message_Char = 0x102;
        public const int Message_Clear = 0x303;
        public const int Message_CloseDropped = 0x401;
        public const int Message_Copy = 0x301;
        public const int Message_Cut = 0x300;
        public const int Message_HScroll = 0x114;
        public const int Message_IME_COMPOSITION = 0x10f;
        public const int Message_IME_CONTROL = 0x283;
        public const int Message_IME_NOTIFY = 0x282;
        public const int Message_IME_STARTCOMPOSITION = 0x10d;
        public const int Message_InitDialog = 0x110;
        public const int Message_KeyDown = 0x100;
        public const int Message_LButtonDown = 0x201;
        public const int Message_LButtonUp = 0x202;
        public const int Message_MouseMove = 0x200;
        public const int Message_NCActivate = 0x86;
        public const int Message_NCPaint = 0x85;
        public const int Message_Paste = 770;
        public const int Message_SetCursor = 0x20;
        public const int Message_SysKeyDown = 260;
        public const int Message_THEMECHANGED = 0x31a;
        public const int Message_Undo = 0x304;
        public const int Message_VScroll = 0x115;
        public const int WindowStyle_Border = 0x800000;
        public const int WindowStyle_ClientEdge = 0x200;

        public static void Beep(int freq, int duration)
        {
            Win32.Beep(freq, duration);
        }

        public static IntPtr CallNextHook(IntPtr hhk, int nCode, IntPtr wparam, IntPtr lparam)
        {
            return Win32.CallNextHookEx(hhk, nCode, wparam, lparam);
        }

        public static bool CreateCaret(IntPtr handle, int nWidth, int nHeight)
        {
            return Win32.CreateCaret(handle, IntPtr.Zero, nWidth, nHeight);
        }

        public static bool DestroyCaret()
        {
            return Win32.DestroyCaret();
        }

        public static bool EnumChildWindows(IntPtr hwndParent, EnumChildProc lpEnumFunc, IntPtr lparam)
        {
            return Win32.EnumChildWindows(hwndParent, lpEnumFunc, lparam);
        }

        public static int ExcludeClipRect(IntPtr hdc, int l, int t, int r, int b)
        {
            return Win32.ExcludeClipRect(hdc, l, t, r, b);
        }

        public static string GetClassName(IntPtr hwnd)
        {
            return Win32.GetClassName(hwnd);
        }

        public static int GetDoubleClickTime()
        {
            return Win32.GetDoubleClickTime();
        }

        public static IntPtr GetMouseHookHandle(IntPtr lparam, out Point pt)
        {
            Win32.MOUSEHOOKSTRUCT mousehookstruct = (Win32.MOUSEHOOKSTRUCT) Marshal.PtrToStructure(lparam, typeof(Win32.MOUSEHOOKSTRUCT));
            pt = new Point(mousehookstruct.Pt.X, mousehookstruct.Pt.Y);
            return mousehookstruct.hwnd;
        }

        public static Size GetScreenCaps()
        {
            Size size;
            IntPtr dC = Win32.GetDC(IntPtr.Zero);
            try
            {
                size = new Size(Win32.GetDeviceCaps(dC, 0x58), Win32.GetDeviceCaps(dC, 90));
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, dC);
            }
            return size;
        }

        public static int GetScrollPos(IntPtr handle, bool flat, bool isVertical)
        {
            if (flat)
            {
                return Win32.FlatSB_GetScrollPos(handle, isVertical ? 1 : 0);
            }
            return Win32.GetScrollPos(handle, isVertical ? 1 : 0);
        }

        public static int GetScrollSize(bool isVertical)
        {
            return Win32.GetSystemMetrics(isVertical ? 20 : 0x15);
        }

        public static int GetScrollSize(IntPtr handle, bool flat, bool isVertical)
        {
            if (flat)
            {
                return Win32.FlatSB_GetScrollSize(handle, isVertical ? 1 : 0);
            }
            return Win32.GetScrollSize(handle, isVertical ? 1 : 0);
        }

        public static ScrollEventType GetScrollType(int code)
        {
            switch (code)
            {
                case 0:
                    return ScrollEventType.SmallDecrement;

                case 1:
                    return ScrollEventType.SmallIncrement;

                case 2:
                    return ScrollEventType.LargeDecrement;

                case 3:
                    return ScrollEventType.LargeIncrement;

                case 4:
                    return ScrollEventType.ThumbPosition;

                case 5:
                    return ScrollEventType.ThumbTrack;

                case 6:
                    return ScrollEventType.First;

                case 7:
                    return ScrollEventType.Last;
            }
            return ScrollEventType.EndScroll;
        }

        public static string GetText(IntPtr hwnd)
        {
            return Win32.GetText(hwnd);
        }

        public static IntPtr GetWindowDC(IntPtr hwnd)
        {
            return Win32.GetWindowDC(hwnd);
        }

        public static short HiWord(IntPtr value)
        {
            return (short) (value.ToInt32() >> 0x10);
        }

        public static void ImeComposition(IntPtr hWnd)
        {
            Win32.ImeComposition(hWnd);
        }

        public static void ImmSetCompositionFont(IntPtr hWnd, Font font)
        {
            Win32.ImmSetCompositionFont(hWnd, font);
        }

        public static void ImmSetCompositionWindow(IntPtr hWnd, Point pos)
        {
            Win32.ImmSetCompositionWindow(hWnd, pos);
        }

        public static bool InitCommonControls()
        {
            return Win32.InitCommonControls(8);
        }

        public static bool InitializeFlatSB(IntPtr handle)
        {
            bool flag = Win32.InitializeFlatSB(handle);
            if (flag)
            {
                Win32.FlatSB_SetScrollProp(handle, 0x200, new IntPtr(2), true);
                Win32.FlatSB_SetScrollProp(handle, 0x100, new IntPtr(2), true);
            }
            return flag;
        }

        public static bool IsMouseMsg(int msg)
        {
            return (((msg >= 0x201) && (msg <= 520)) || ((msg >= 0xa1) && (msg <= 0xa9)));
        }

        public static short LoWord(IntPtr value)
        {
            return (short) value.ToInt32();
        }

        public static void MessageBeep()
        {
            Win32.MessageBeep(0x10);
        }

        public static bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            return Win32.PostMessage(hwnd, msg, wparam, lparam);
        }

        public static int ReleaseDC(IntPtr hwnd, IntPtr hdc)
        {
            return Win32.ReleaseDC(hwnd, hdc);
        }

        public static bool ReleaseHook(IntPtr hhk)
        {
            return Win32.UnhookWindowsHookEx(hhk);
        }

        public static void ScrollWindow(IntPtr handle, int x, int y, Rectangle rect)
        {
            Win32.ScrollWindow(handle, x, y, rect);
        }

        public static IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            return Win32.SendMessage(hwnd, msg, wparam, lparam);
        }

        public static bool SetCaretPos(int x, int y)
        {
            return Win32.SetCaretPos(x, y);
        }

        public static IntPtr SetCursor(IntPtr handle)
        {
            return Win32.SetCursor(handle);
        }

        public static IntPtr SetMouseHook(HookHandler lpfn)
        {
            return Win32.SetWindowsHookEx(7, lpfn, IntPtr.Zero, Thread.CurrentThread.ManagedThreadId);
        }

        public static void SetScrollBar(IntPtr handle, bool flat, int size, int pageSize, bool isVertical)
        {
            if (flat)
            {
                Win32.FlatSB_SetScrollBarInfo(handle, size, pageSize, isVertical ? 1 : 0);
            }
            else
            {
                Win32.SetScrollBarInfo(handle, size, pageSize, isVertical ? 1 : 0);
            }
        }

        public static void SetScrollPos(IntPtr handle, bool flat, int pos, bool isVertical)
        {
            if (flat)
            {
                Win32.FlatSB_SetScrollPos(handle, isVertical ? 1 : 0, pos);
            }
            else
            {
                Win32.SetScrollPos(handle, isVertical ? 1 : 0, pos);
            }
        }

        public static void SetText(IntPtr hwnd, string text)
        {
            Win32.SetText(hwnd, text);
        }

        public static IntPtr SetWndProcHook(HookHandler lpfn)
        {
            return Win32.SetWindowsHookEx(4, lpfn, IntPtr.Zero, Thread.CurrentThread.ManagedThreadId);
        }

        public static bool ShowCaret(IntPtr handle)
        {
            return Win32.ShowCaret(handle);
        }

        public static bool ShowWindowTopMost(IntPtr hwnd, int x, int y, int cx, int cy)
        {
            return Win32.SetWindowPos(hwnd, (IntPtr) (-1), x, y, cx, cy, 80);
        }

        public static void Sleep(int milliSeconds)
        {
            Win32.Sleep(milliSeconds);
        }

        public static bool UninitializeFlatSB(IntPtr handle)
        {
            return Win32.UninitializeFlatSB(handle);
        }

        public static void UpdateCompositionWindow(IntPtr hWnd, Point pos, IntPtr wndPos)
        {
            Win32.UpdateCompositionWindow(hWnd, pos, wndPos);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPSTRUCT
        {
            public IntPtr lParam;
            public IntPtr wParam;
            public int message;
            public IntPtr hwnd;
        }
    }
}

