namespace QWhale.Common
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class GdiPainter : Painter, IPainter
    {
        private IntPtr brush;
        private int[] buffer;
        private int bufferSize;
        private int formatFlags;
        private System.Drawing.Graphics graphics;
        private IntPtr hdc;
        private int oldBackColor;
        private IntPtr oldBrush;
        private IntPtr oldFont;
        private int oldMode = 1;
        private bool oldOpaque;
        private IntPtr oldPen;
        private int oldTextColor;
        private IntPtr pen;
        private bool safeTransformed;
        private Win32.XFORM safexForm;
        private bool transFormed;
        private bool useDrawText;
        private Win32.XFORM xForm;

        public virtual void BeginPaint(System.Drawing.Graphics graphics)
        {
            this.graphics = graphics;
            this.hdc = graphics.GetHdc();
            this.oldFont = (base.CurrentInfo != null) ? Win32.SelectObject(this.hdc, base.CurrentInfo.HFont) : IntPtr.Zero;
            this.oldTextColor = Win32.SetTextColor(this.hdc, Win32.ColorToGdiColor(this.TextColor));
            this.oldBackColor = Win32.SetBkColor(this.hdc, Win32.ColorToGdiColor(this.BackColor));
            this.oldOpaque = Win32.SetBkMode(this.hdc, this.Opaque ? 2 : 1) == 2;
            this.oldMode = Win32.GetGraphicsMode(this.hdc);
            this.SelectBrush(this.BackColor, true);
            this.SelectPen(this.ForeColor, false);
            this.oldPen = Win32.SelectObject(this.hdc, this.pen);
            this.oldBrush = Win32.SelectObject(this.hdc, this.brush);
        }

        private int Border3DSideToSide(Border3DSide sides)
        {
            int num = 0;
            if (sides == Border3DSide.All)
            {
                num |= 0x80f;
            }
            if ((sides & Border3DSide.Bottom) != 0)
            {
                num |= 8;
            }
            if ((sides & Border3DSide.Left) != 0)
            {
                num |= 1;
            }
            if ((sides & Border3DSide.Middle) != 0)
            {
                num |= 0x800;
            }
            if ((sides & Border3DSide.Right) != 0)
            {
                num |= 4;
            }
            if ((sides & Border3DSide.Top) != 0)
            {
                num |= 2;
            }
            return num;
        }

        private int Border3dStyleToBorder(Border3DStyle border)
        {
            switch (border)
            {
                case Border3DStyle.RaisedOuter:
                    return 1;

                case Border3DStyle.SunkenOuter:
                    return 2;

                case Border3DStyle.RaisedInner:
                    return 4;

                case Border3DStyle.Raised:
                    return 5;

                case Border3DStyle.Etched:
                    return 6;

                case Border3DStyle.SunkenInner:
                    return 8;

                case Border3DStyle.Bump:
                    return 9;

                case Border3DStyle.Sunken:
                    return 10;

                case Border3DStyle.Flat:
                    return 0x4000;
            }
            return 0;
        }

        public virtual int CharWidth(char ch, int count)
        {
            return this.CharWidth(base.CurrentInfo, ch, count);
        }

        private int CharWidth(FontInfo info, char ch, int count)
        {
            if (((GdiFontInfo) info).UseDrawText != this.useDrawText)
            {
                ((GdiFontInfo) info).UseDrawText = this.useDrawText;
            }
            if (count == 0x7fffffff)
            {
                return 0x7fffffff;
            }
            if (count <= 0)
            {
                return 0;
            }
            if (this.IsFontMonoSpaced())
            {
                return (count * info.FontWidth);
            }
            return (count * ((GdiFontInfo) info).CharWidth(ch));
        }

        public virtual int CharWidth(char ch, int width, out int count)
        {
            return this.CharWidth(base.CurrentInfo, ch, width, out count);
        }

        private int CharWidth(FontInfo info, char ch, int width, out int count)
        {
            if (((GdiFontInfo) info).UseDrawText != this.useDrawText)
            {
                ((GdiFontInfo) info).UseDrawText = this.useDrawText;
            }
            if (width == 0x7fffffff)
            {
                count = 0x7fffffff;
                return 0x7fffffff;
            }
            int fontWidth = 0;
            count = 0;
            if (width > 0)
            {
                if (this.IsFontMonoSpaced())
                {
                    fontWidth = info.FontWidth;
                }
                else
                {
                    fontWidth = ((GdiFontInfo) info).CharWidth(ch);
                }
                if (fontWidth > 0)
                {
                    count = width / fontWidth;
                    return (count * fontWidth);
                }
            }
            return 0;
        }

        public override void Clear()
        {
            this.FreeBuffer();
            this.pen = IntPtr.Zero;
            this.brush = IntPtr.Zero;
            base.Clear();
        }

        protected override void ClearBrushes()
        {
            foreach (DictionaryEntry entry in base.BrushTable)
            {
                Win32.DeleteObject((IntPtr) entry.Value);
            }
            base.ClearBrushes();
        }

        protected override void ClearPens()
        {
            foreach (DictionaryEntry entry in base.PenTable)
            {
                Win32.DeleteObject((IntPtr) entry.Value);
            }
            base.ClearPens();
        }

        protected override object CreateBrush(Color color)
        {
            return Win32.CreateSolidBrush(Win32.ColorToGdiColor(color));
        }

        protected override FontInfos CreateFontInfos(Font font)
        {
            return new GdiFontInfos(font, base.measureDC);
        }

        protected override FontInfos CreateFontInfos(IntPtr hFont)
        {
            return new GdiFontInfos(hFont, base.measureDC);
        }

        protected override object CreatePen(Color color)
        {
            return Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
        }

        internal int DashStyleToPenStyle(DashStyle style)
        {
            switch (style)
            {
                case DashStyle.Dash:
                    return 1;

                case DashStyle.Dot:
                    return 2;

                case DashStyle.DashDot:
                    return 3;

                case DashStyle.DashDotDot:
                    return 4;
            }
            return 0;
        }

        public virtual void DrawDotLine(int x1, int y1, int x2, int y2, Color color1, Color color2)
        {
            bool flag = color2 == Color.Empty;
            if (x1 == x2)
            {
                for (int i = 0; i < (y2 - y1); i++)
                {
                    if (((i + y1) % 2) == 0)
                    {
                        Win32.SetPixel(this.hdc, x1, i + y1, color1);
                    }
                    else if (!flag)
                    {
                        Win32.SetPixel(this.hdc, x1, i + y1, color2);
                    }
                }
            }
            else if (y1 == y2)
            {
                for (int j = 0; j < (x2 - x1); j++)
                {
                    if (((j + x1) % 2) == 0)
                    {
                        Win32.SetPixel(this.hdc, j + x1, y1, color1);
                    }
                    else if (!flag)
                    {
                        Win32.SetPixel(this.hdc, j + x1, y1, color2);
                    }
                }
            }
        }

        public virtual void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides)
        {
            this.DrawEdge(ref rect, border, sides, 0);
        }

        public virtual void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides, int flags)
        {
            Win32.DrawEdge(this.hdc, ref rect, this.Border3dStyleToBorder(border), this.Border3DSideToSide(sides) | flags);
        }

        public virtual void DrawFocusRect(Rectangle rect, Color color)
        {
            this.DrawFocusRect(rect.Left, rect.Top, rect.Width, rect.Height, color);
        }

        public virtual void DrawFocusRect(int x, int y, int width, int height, Color color)
        {
            this.DrawDotLine(x, y, x + width, y, color, Color.Empty);
            this.DrawDotLine(x, y, x, y + height, color, Color.Empty);
            this.DrawDotLine(x + width, y, x + width, y + height, color, Color.Empty);
            this.DrawDotLine(x, y + height, x + width, y + height, color, Color.Empty);
        }

        public virtual void DrawImage(ImageList images, int index, Rectangle rect)
        {
            if (images != null)
            {
                uint maxValue = uint.MaxValue;
                Win32.ImageList_DrawEx(images.Handle, index, this.hdc, rect.Left, rect.Top, rect.Width, rect.Height, (int) maxValue, (int) maxValue, 1);
            }
        }

        public virtual void DrawImage(ImageList images, int index, Rectangle rect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
        }

        public virtual void DrawLine(int x1, int y1, int x2, int y2)
        {
            Win32.MoveToEx(this.hdc, x1, y1, IntPtr.Zero);
            if (x1 == x2)
            {
                y2++;
            }
            if (y1 == y2)
            {
                x2++;
            }
            Win32.LineTo(this.hdc, x2, y2);
        }

        public virtual void DrawLine(int x1, int y1, int x2, int y2, Color color, int width, DashStyle style)
        {
            IntPtr ptr = Win32.CreatePen(this.DashStyleToPenStyle(style), width, Win32.ColorToGdiColor(color));
            IntPtr ptr2 = Win32.SelectObject(this.hdc, ptr);
            this.DrawLine(x1, y1, x2, y2);
            Win32.SelectObject(this.hdc, ptr2);
            Win32.DeleteObject(ptr);
        }

        public virtual void DrawPolygon(Point[] points, Color color)
        {
            if (points != null)
            {
                this.BackColor = color;
                IntPtr ptr = Win32.SelectObject(this.hdc, this.brush);
                IntPtr ptr2 = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
                IntPtr ptr3 = Win32.SelectObject(this.hdc, ptr2);
                Win32.Polygon(this.hdc, points, points.Length);
                Win32.SelectObject(this.hdc, ptr);
                Win32.SelectObject(this.hdc, ptr3);
                Win32.DeleteObject(ptr2);
            }
        }

        public virtual void DrawRectangle(Rectangle rect)
        {
            this.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public virtual void DrawRectangle(int x, int y, int width, int height)
        {
            Win32.FrameRect(this.hdc, x, y, width + 1, height + 1, this.brush);
        }

        public virtual void DrawRoundRectangle(int left, int top, int right, int bottom, int width, int height)
        {
            IntPtr ptr = Win32.SelectObject(this.hdc, Win32.GetStockObject(5));
            try
            {
                Win32.RoundRect(this.hdc, left, top, right + 1, bottom + 1, width + 1, height + 1);
            }
            finally
            {
                Win32.SelectObject(this.hdc, ptr);
            }
        }

        public virtual void DrawText(string text, int len, Rectangle rect)
        {
            if (text != null)
            {
                string s = (len == -1) ? text : text.Substring(0, len);
                Win32.DrawText(this.hdc, s, s.Length, ref rect, this.formatFlags);
            }
        }

        public virtual void DrawThemeBackground(IntPtr handle, int partID, int stateID, Rectangle rect)
        {
            Win32.GdiRect rect2 = new Win32.GdiRect(rect);
            Win32.DrawThemeBackground(handle, this.hdc, partID, stateID, ref rect2, IntPtr.Zero);
        }

        public virtual void DrawWave(Rectangle rect, Color color)
        {
            int num = rect.Left - (rect.Left % 6);
            int num2 = rect.Right % 6;
            int num3 = (num2 != 0) ? (rect.Right + (6 - num2)) : rect.Right;
            int count = (num3 - num) >> 1;
            if (count < 4)
            {
                count = 4;
            }
            else
            {
                num2 = (count - 4) / 3;
                if (((count - 4) % 3) != 0)
                {
                    num2++;
                }
                count = 4 + (num2 * 3);
            }
            Point[] points = new Point[count];
            for (int i = 0; i < count; i++)
            {
                points[i].X = num + (i * 2);
                points[i].Y = rect.Bottom - 1;
                switch ((i % 3))
                {
                    case 0:
                        points[i].Y -= 2;
                        break;

                    case 2:
                        points[i].Y += 2;
                        break;
                }
            }
            IntPtr ptr = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
            IntPtr ptr2 = Win32.SelectObject(this.hdc, ptr);
            Win32.PolyBezier(this.hdc, points, count);
            Win32.SelectObject(this.hdc, ptr2);
            Win32.DeleteObject(ptr);
        }

        public virtual void EndPaint()
        {
            if (this.oldFont != IntPtr.Zero)
            {
                Win32.SelectObject(this.hdc, this.oldFont);
            }
            Win32.SetTextColor(this.hdc, this.oldTextColor);
            Win32.SetBkColor(this.hdc, this.oldBackColor);
            Win32.SetBkMode(this.hdc, this.oldOpaque ? 2 : 1);
            Win32.SelectObject(this.hdc, this.oldPen);
            Win32.SelectObject(this.hdc, this.oldBrush);
            this.EndTransform();
            if (this.graphics == null)
            {
                this.graphics = null;
            }
            this.graphics.ReleaseHdc(this.hdc);
            this.graphics = null;
            this.hdc = IntPtr.Zero;
        }

        public virtual void EndTransform()
        {
            if (this.transFormed)
            {
                this.xForm = new Win32.XFORM(1f, 0f, 0f, 1f, 0f, 0f);
                Win32.SetWorldTransform(this.hdc, ref this.xForm);
                this.transFormed = false;
            }
        }

        public virtual void ExcludeClipRect(Rectangle rect)
        {
            this.ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public virtual void ExcludeClipRect(int x, int y, int width, int height)
        {
            Win32.ExcludeClipRect(this.hdc, x, y, x + width, y + height);
        }

        public virtual void FillGradient(Rectangle rect, Color beginColor, Color endColor, Point point1, Point point2)
        {
            this.FillGradient(rect.Left, rect.Top, rect.Width, rect.Height, beginColor, endColor, point1, point2);
        }

        public virtual void FillGradient(int x, int y, int width, int height, Color beginColor, Color endColor, Point point1, Point point2)
        {
            System.Drawing.Graphics gr = this.SafeEndPaint();
            try
            {
                Brush brush = new LinearGradientBrush(point1, point2, beginColor, endColor);
                gr.FillRectangle(brush, x, y, width, height);
                brush.Dispose();
            }
            finally
            {
                this.SafeBeginPaint(gr);
            }
        }

        public virtual void FillPolygon(Color color, Point[] points)
        {
            Color backColor = this.BackColor;
            Color foreColor = this.ForeColor;
            try
            {
                this.BackColor = color;
                this.ForeColor = color;
                Win32.Polygon(this.hdc, points, points.Length);
            }
            finally
            {
                this.BackColor = backColor;
                this.ForeColor = foreColor;
            }
        }

        public virtual void FillRectangle(Rectangle rect)
        {
            this.FillRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public virtual void FillRectangle(Color color, Rectangle rect)
        {
            Color backColor = this.BackColor;
            try
            {
                this.BackColor = color;
                this.FillRectangle(rect);
            }
            finally
            {
                this.BackColor = backColor;
            }
        }

        public virtual void FillRectangle(int x, int y, int width, int height)
        {
            Win32.FillRect(this.hdc, x, y, width, height, this.brush);
        }

        public virtual void FillRectangle(Color color, int x, int y, int width, int height)
        {
            Color backColor = this.BackColor;
            try
            {
                this.BackColor = color;
                this.FillRectangle(x, y, width, height);
            }
            finally
            {
                this.BackColor = backColor;
            }
        }

        private void FreeBuffer()
        {
            this.buffer = null;
            this.bufferSize = 0;
        }

        protected int[] GetBuffer(int len)
        {
            return this.GetBuffer(len, -1);
        }

        protected int[] GetBuffer(int len, int space)
        {
            if (!this.IsFontMonoSpaced() && (space < 0))
            {
                return null;
            }
            if (this.bufferSize < len)
            {
                this.FreeBuffer();
                this.bufferSize = len;
                this.buffer = new int[this.bufferSize];
                for (int i = 0; i < this.bufferSize; i++)
                {
                    this.buffer[i] = (space >= 0) ? space : this.FontWidth;
                }
            }
            return this.buffer;
        }

        private void InitDC(IntPtr hFont)
        {
            if (this.hdc != IntPtr.Zero)
            {
                Win32.SelectObject(this.hdc, hFont);
            }
        }

        public virtual void IntersectClipRect(Rectangle rect)
        {
            this.IntersectClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public virtual void IntersectClipRect(int x, int y, int width, int height)
        {
            Win32.IntersectClipRect(this.hdc, x, y, x + width, y + height);
        }

        protected virtual bool IsFontMonoSpaced()
        {
            return false;
        }

        protected virtual void OnUseDrawTextChanged()
        {
        }

        public virtual void RestoreClip(IntPtr rgn)
        {
            Win32.SelectClipRgn(this.hdc, rgn);
            Win32.DeleteObject(rgn);
        }

        protected void SafeBeginPaint(System.Drawing.Graphics gr)
        {
            if (this.safeTransformed)
            {
                gr.Transform = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
            }
            this.BeginPaint(gr);
            if (this.safeTransformed)
            {
                this.xForm = this.safexForm;
                Win32.SetWorldTransform(this.hdc, ref this.xForm);
                this.transFormed = true;
            }
        }

        protected System.Drawing.Graphics SafeEndPaint()
        {
            this.safexForm = this.xForm;
            this.safeTransformed = this.transFormed;
            System.Drawing.Graphics graphics = this.graphics;
            this.EndPaint();
            if (this.safeTransformed)
            {
                graphics.Transform = new Matrix(this.safexForm.eM11, this.safexForm.eM12, this.safexForm.eM21, this.safexForm.eM22, this.safexForm.eDx, this.safexForm.eDy);
            }
            return graphics;
        }

        public virtual IntPtr SaveClip(Rectangle rect)
        {
            IntPtr rgn = Win32.CreateRectRgnIndirect(rect);
            Win32.GetClipRgn(this.hdc, rgn);
            return rgn;
        }

        protected override void SelectBrush(Color color, bool select)
        {
            this.brush = (IntPtr) this.SelectBrush(color);
            if (select && (this.hdc != IntPtr.Zero))
            {
                Win32.SetBkColor(this.hdc, Win32.ColorToGdiColor(color));
            }
        }

        protected override void SelectFont(FontStyle fontStyle)
        {
            base.SelectFont(fontStyle);
            if (base.CurrentInfo != null)
            {
                this.InitDC(base.CurrentInfo.HFont);
            }
        }

        protected override void SelectFont(Font font, FontStyle fontStyle)
        {
            base.SelectFont(font, fontStyle);
            if (base.CurrentInfo != null)
            {
                this.InitDC(base.CurrentInfo.HFont);
            }
        }

        protected override void SelectOpaque(bool value)
        {
            if (this.hdc != IntPtr.Zero)
            {
                Win32.SetBkMode(this.hdc, value ? 2 : 1);
            }
        }

        protected override void SelectPen(Color color, bool select)
        {
            this.pen = (IntPtr) this.SelectPen(color);
            if (select && (this.hdc != IntPtr.Zero))
            {
                Win32.SelectObject(this.hdc, this.pen);
            }
        }

        protected override void SelectStringFormat(StringFormat format)
        {
            this.formatFlags = 0;
            switch (format.Alignment)
            {
                case StringAlignment.Near:
                    this.formatFlags = this.formatFlags;
                    break;

                case StringAlignment.Center:
                    this.formatFlags |= 1;
                    break;

                case StringAlignment.Far:
                    this.formatFlags |= 2;
                    break;
            }
            switch (format.LineAlignment)
            {
                case StringAlignment.Near:
                    this.formatFlags = this.formatFlags;
                    break;

                case StringAlignment.Center:
                    this.formatFlags |= 0x24;
                    break;

                case StringAlignment.Far:
                    this.formatFlags |= 40;
                    break;
            }
            switch (format.HotkeyPrefix)
            {
                case HotkeyPrefix.None:
                    this.formatFlags |= 0x800;
                    break;

                case HotkeyPrefix.Hide:
                    this.formatFlags |= 0x100000;
                    break;
            }
            switch (format.Trimming)
            {
                case StringTrimming.EllipsisCharacter:
                    this.formatFlags |= 0x8000;
                    break;

                case StringTrimming.EllipsisWord:
                    this.formatFlags |= 0x40000;
                    break;

                case StringTrimming.EllipsisPath:
                    this.formatFlags |= 0x4000;
                    break;
            }
            if ((format.FormatFlags & StringFormatFlags.DirectionRightToLeft) != 0)
            {
                this.formatFlags |= 0x20000;
            }
            if ((format.FormatFlags & StringFormatFlags.NoClip) != 0)
            {
                this.formatFlags |= 0x100;
            }
        }

        protected override void SelectTextColor(Color color)
        {
            if (this.hdc != IntPtr.Zero)
            {
                Win32.SetTextColor(this.hdc, Win32.ColorToGdiColor(color));
            }
        }

        public virtual void StretchDrawImage(Rectangle rect, Rectangle stretchRect, Rectangle imageRect, Bitmap image)
        {
        }

        public virtual int StringWidth(string text)
        {
            return this.StringWidth(text, 0, -1);
        }

        public virtual int StringWidth(string text, int pos, int len)
        {
            return this.StringWidth(base.CurrentInfo, text, pos, len);
        }

        protected virtual int StringWidth(FontInfo info, string text, int pos, int len)
        {
            if (len == 0x7fffffff)
            {
                return 0x7fffffff;
            }
            if ((text == null) || (info == null))
            {
                return 0;
            }
            if (len == -1)
            {
                len = text.Length;
            }
            if (this.IsFontMonoSpaced())
            {
                return (len * info.FontWidth);
            }
            int num = 0;
            for (int i = pos; i < (pos + len); i++)
            {
                num += ((GdiFontInfo) info).CharWidth(text[i]);
            }
            return num;
        }

        public virtual int StringWidth(string text, int width, out int count, bool exact)
        {
            return this.StringWidth(text, 0, -1, width, out count, exact);
        }

        public virtual int StringWidth(string text, int pos, int len, int width, out int count)
        {
            return this.StringWidth(text, pos, len, width, out count, true);
        }

        public virtual int StringWidth(string text, int pos, int len, int width, out int count, bool exact)
        {
            return this.StringWidth(base.CurrentInfo, text, pos, len, width, out count, exact);
        }

        protected virtual int StringWidth(FontInfo info, string text, int pos, int len, int width, out int count, bool exact)
        {
            count = 0;
            if (text == null)
            {
                return 0;
            }
            if (len == -1)
            {
                len = text.Length - pos;
            }
            if (width == 0x7fffffff)
            {
                count = len;
                return 0x7fffffff;
            }
            if (this.IsFontMonoSpaced())
            {
                int fontWidth = info.FontWidth;
                if (fontWidth == 0)
                {
                    count = len;
                }
                else
                {
                    if (exact)
                    {
                        count = width / fontWidth;
                    }
                    else
                    {
                        count = (int) Math.Round((double) (width / fontWidth));
                    }
                    count = Math.Min(count, len);
                }
                return (count * fontWidth);
            }
            for (int i = pos; i < (pos + len); i++)
            {
                int num2 = ((GdiFontInfo) info).CharWidth(text[i]);
                width -= num2;
                if (width < 0)
                {
                    if (!exact && (width > (-num2 / 2)))
                    {
                        count++;
                    }
                    break;
                }
                count++;
            }
            return this.StringWidth(info, text, pos, count);
        }

        public virtual void TextOut(string text, int len, Rectangle rect)
        {
            this.TextOut(text, len, rect, rect.Left, rect.Top, false, false, -1);
        }

        public virtual void TextOut(string text, int len, int x, int y)
        {
            this.TextOut(text, len, new Rectangle(x, y, 0, 0), x, y, false, false, -1);
        }

        public virtual void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque)
        {
            this.TextOut(text, len, rect, rect.Left, rect.Top, clipped, opaque, -1);
        }

        public virtual void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque, int space)
        {
            this.TextOut(text, len, rect, rect.Left, rect.Top, clipped, opaque, space);
        }

        public virtual void TextOut(string text, int len, int x, int y, bool clipped, bool opaque)
        {
            this.TextOut(text, len, new Rectangle(x, y, 0, 0), x, y, clipped, opaque, -1);
        }

        public virtual void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque)
        {
            this.TextOut(text, len, rect, x, y, clipped, opaque, -1);
        }

        public virtual void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque, int space)
        {
            if (text != null)
            {
                if (this.useDrawText)
                {
                    if (opaque)
                    {
                        Win32.GdiRect rect2 = new Win32.GdiRect(rect);
                        Win32.FillRect(this.hdc, ref rect2, this.brush);
                    }
                    Win32.DrawText(this.hdc, text, len, ref rect, !clipped ? 0x100 : 0);
                }
                else
                {
                    Win32.ExtTextOut(this.hdc, rect, x, y, (clipped ? 4 : 0) | (opaque ? 2 : 0), text, len, (space > 0) ? this.GetBuffer((len < 0) ? text.Length : 0, space) : null);
                }
            }
        }

        public virtual void Transform(int x, int y, float scaleX, float scaleY)
        {
            this.transFormed = true;
            this.oldMode = Win32.SetGraphicsMode(this.hdc, 2);
            this.xForm = new Win32.XFORM(scaleX, 0f, 0f, scaleY, x * scaleX, y * scaleY);
            Win32.SetWorldTransform(this.hdc, ref this.xForm);
        }

        public virtual System.Drawing.Graphics Graphics
        {
            get
            {
                return this.graphics;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return this.hdc;
            }
        }

        public virtual Matrix Transformation
        {
            get
            {
                if (this.graphics != null)
                {
                    return this.graphics.Transform;
                }
                return null;
            }
        }

        public virtual bool UseDrawText
        {
            get
            {
                return this.useDrawText;
            }
            set
            {
                if (this.useDrawText != value)
                {
                    this.useDrawText = value;
                    this.OnUseDrawTextChanged();
                }
            }
        }
    }
}

