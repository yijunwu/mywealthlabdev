namespace QWhale.Common
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class GdiPlusPainter : Painter, IPainter
    {
        private System.Drawing.Brush brush;
        private System.Drawing.Graphics graphics;
        private System.Drawing.Graphics measureGraphics;
        private System.Drawing.Pen pen;
        private System.Drawing.Brush textBrush;
        private bool transFormed;
        private bool useDrawText;

        public GdiPlusPainter()
        {
            this.measureGraphics = System.Drawing.Graphics.FromHdc(base.measureDC);
        }

        public virtual void BeginPaint(System.Drawing.Graphics graphics)
        {
            this.graphics = graphics;
        }

        public virtual int CharWidth(char ch, int count)
        {
            return this.StringWidth(new string(ch, (count <= 0) ? 1 : count));
        }

        public virtual int CharWidth(char ch, int width, out int count)
        {
            int num = this.StringWidth(new string(ch, 1));
            count = (num != 0) ? (width / num) : 0;
            return (count * num);
        }

        protected override void ClearBrushes()
        {
            foreach (DictionaryEntry entry in base.BrushTable)
            {
                ((System.Drawing.Brush) entry.Value).Dispose();
            }
            base.ClearBrushes();
        }

        protected override void ClearPens()
        {
            foreach (DictionaryEntry entry in base.PenTable)
            {
                ((System.Drawing.Pen) entry.Value).Dispose();
            }
            base.ClearPens();
        }

        protected override object CreateBrush(Color color)
        {
            return new SolidBrush(color);
        }

        protected override object CreatePen(Color color)
        {
            return new System.Drawing.Pen(color);
        }

        public virtual void DrawDotLine(int x1, int y1, int x2, int y2, Color color1, Color color2)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(color1, 1f) {
                DashStyle = DashStyle.Dot
            };
            if (color2 != Color.Empty)
            {
                pen.Brush = new SolidBrush(color2);
            }
            this.graphics.DrawLine(pen, x1, y1, x2, y2);
            pen.Dispose();
        }

        public virtual void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides)
        {
            this.DrawEdge(ref rect, border, sides, 0);
        }

        public virtual void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides, int flags)
        {
            ControlPaint.DrawBorder3D(this.graphics, rect, border, sides);
        }

        public virtual void DrawFocusRect(Rectangle rect, Color color)
        {
            this.DrawFocusRect(rect.Left, rect.Top, rect.Width, rect.Height, color);
        }

        public virtual void DrawFocusRect(int x, int y, int width, int height, Color color)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(color, 1f) {
                DashStyle = DashStyle.Dot
            };
            this.graphics.DrawRectangle(pen, x, y, width, height);
            pen.Dispose();
        }

        public virtual void DrawImage(ImageList images, int index, Rectangle rect)
        {
            if ((images != null) && ((index >= 0) && (index < images.Images.Count)))
            {
                this.graphics.DrawImage(images.Images[index], rect.Left, rect.Top);
            }
        }

        public virtual void DrawImage(ImageList images, int index, Rectangle rect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            if ((images != null) && ((index >= 0) && (index < images.Images.Count)))
            {
                this.graphics.DrawImage(images.Images[index], rect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr);
            }
        }

        public virtual void DrawLine(int x1, int y1, int x2, int y2)
        {
            this.graphics.DrawLine(this.pen, x1, y1, x2, y2);
        }

        public virtual void DrawLine(int x1, int y1, int x2, int y2, Color color, int width, DashStyle penStyle)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(color, (float) width) {
                DashStyle = penStyle
            };
            this.graphics.DrawLine(pen, x1, y1, x2, y2);
            pen.Dispose();
        }

        public virtual void DrawPolygon(Point[] points, Color color)
        {
            this.graphics.DrawPolygon((System.Drawing.Pen) this.SelectPen(color), points);
        }

        public virtual void DrawRectangle(Rectangle rect)
        {
            this.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public virtual void DrawRectangle(int x, int y, int width, int height)
        {
            this.graphics.DrawRectangle(this.pen, x, y, width, height);
        }

        public virtual void DrawRoundRectangle(int left, int top, int right, int bottom, int width, int height)
        {
            this.graphics.DrawArc(this.pen, left, top, right - left, bottom - top, 0, 360);
        }

        public virtual void DrawText(string text, int len, Rectangle rect)
        {
            if (text != null)
            {
                string s = (len == -1) ? text : text.Substring(0, len);
                this.graphics.DrawString(s, this.Font, this.textBrush, rect, this.StringFormat);
            }
        }

        public virtual void DrawThemeBackground(IntPtr handle, int partID, int stateID, Rectangle rect)
        {
            IntPtr hdc = this.graphics.GetHdc();
            try
            {
                Win32.GdiRect rect2 = new Win32.GdiRect(rect);
                Win32.DrawThemeBackground(handle, hdc, partID, stateID, ref rect2, IntPtr.Zero);
            }
            finally
            {
                this.graphics.ReleaseHdc(hdc);
            }
        }

        public virtual void DrawWave(Rectangle rect, Color color)
        {
            int num = rect.Left - (rect.Left % 6);
            int num2 = rect.Right % 6;
            int num3 = (num2 != 0) ? (rect.Right + (6 - num2)) : rect.Right;
            int num4 = (num3 - num) >> 1;
            if (num4 < 4)
            {
                num4 = 4;
            }
            else
            {
                num2 = (num4 - 4) / 3;
                if (((num4 - 4) % 3) != 0)
                {
                    num2++;
                }
                num4 = 4 + (num2 * 3);
            }
            Point[] points = new Point[num4];
            for (int i = 0; i < num4; i++)
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
            this.graphics.DrawBeziers((System.Drawing.Pen) this.SelectPen(color), points);
        }

        public virtual void EndPaint()
        {
            this.graphics = null;
        }

        public virtual void EndTransform()
        {
            if (this.transFormed)
            {
                this.graphics.Transform = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
                this.transFormed = false;
            }
        }

        public virtual void ExcludeClipRect(Rectangle rect)
        {
            this.graphics.ExcludeClip(rect);
        }

        public virtual void ExcludeClipRect(int x, int y, int width, int height)
        {
            this.graphics.ExcludeClip(new Rectangle(x, y, width, height));
        }

        public virtual void FillGradient(Rectangle rect, Color beginColor, Color endColor, Point point1, Point point2)
        {
            this.FillGradient(rect.X, rect.Y, rect.Width, rect.Height, beginColor, endColor, point1, point2);
        }

        public virtual void FillGradient(int x, int y, int width, int height, Color beginColor, Color endColor, Point point1, Point point2)
        {
            this.graphics.FillRectangle(new LinearGradientBrush(point1, point2, beginColor, endColor), x, y, width, height);
        }

        public virtual void FillPolygon(Color color, Point[] points)
        {
            Color backColor = this.BackColor;
            try
            {
                this.BackColor = color;
                this.graphics.FillPolygon(this.brush, points);
            }
            finally
            {
                this.BackColor = backColor;
            }
        }

        public virtual void FillRectangle(Rectangle rect)
        {
            this.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height);
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
            this.graphics.FillRectangle(this.brush, x, y, width, height);
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

        ~GdiPlusPainter()
        {
            this.measureGraphics.ReleaseHdc(base.measureDC);
            this.measureGraphics.Dispose();
        }

        public virtual void IntersectClipRect(Rectangle rect)
        {
            this.graphics.IntersectClip(rect);
        }

        public virtual void IntersectClipRect(int x, int y, int width, int height)
        {
            this.graphics.IntersectClip(new Rectangle(x, y, width, height));
        }

        protected virtual bool IsFontMonoSpaced()
        {
            return false;
        }

        protected virtual void OnBrushChanged()
        {
        }

        protected virtual void OnPenChanged()
        {
        }

        protected virtual void OnUseDrawTextChanged()
        {
        }

        public virtual void RestoreClip(IntPtr rgn)
        {
            if (rgn != IntPtr.Zero)
            {
                Region region = Region.FromHrgn(rgn);
                this.graphics.SetClip(region, CombineMode.Complement);
                region.Dispose();
            }
        }

        public virtual IntPtr SaveClip(Rectangle rect)
        {
            Region clip = this.graphics.Clip;
            IntPtr hrgn = clip.GetHrgn(this.graphics);
            clip.Dispose();
            return hrgn;
        }

        protected override void SelectBrush(Color color, bool select)
        {
            this.brush = (System.Drawing.Brush) this.SelectBrush(color);
        }

        protected override void SelectPen(Color color, bool select)
        {
            this.pen = (System.Drawing.Pen) this.SelectPen(color);
        }

        protected override void SelectTextColor(Color color)
        {
            this.textBrush = (System.Drawing.Brush) this.SelectBrush(color);
        }

        public virtual void StretchDrawImage(Rectangle rect, Rectangle stretchRect, Rectangle imageRect, Bitmap image)
        {
            if (image != null)
            {
                int width = imageRect.Width - stretchRect.Right;
                int height = imageRect.Height - stretchRect.Bottom;
                this.graphics.DrawImage(image, rect.Left, rect.Top, new Rectangle(imageRect.Left, imageRect.Top, stretchRect.Left, stretchRect.Top), GraphicsUnit.Pixel);
                this.graphics.DrawImage(image, new Rectangle(rect.Left, rect.Top + stretchRect.Top, stretchRect.Left, (rect.Height - height) - stretchRect.Top), imageRect.Left, imageRect.Top + stretchRect.Top, stretchRect.Left, stretchRect.Height, GraphicsUnit.Pixel);
                this.graphics.DrawImage(image, rect.Left, rect.Bottom - height, new Rectangle(imageRect.Left, imageRect.Top + stretchRect.Bottom, stretchRect.Left, height), GraphicsUnit.Pixel);
                this.graphics.DrawImage(image, new Rectangle(rect.Left + stretchRect.Left, rect.Top, (rect.Width - stretchRect.Left) - width, stretchRect.Top), imageRect.Left + stretchRect.Left, imageRect.Top, stretchRect.Width, stretchRect.Top, GraphicsUnit.Pixel);
                this.graphics.DrawImage(image, new Rectangle(rect.Left + stretchRect.Left, rect.Top + stretchRect.Top, (rect.Width - stretchRect.Left) - width, (rect.Height - stretchRect.Top) - height), imageRect.Left + stretchRect.Left, imageRect.Top + stretchRect.Top, stretchRect.Width, stretchRect.Height, GraphicsUnit.Pixel);
                this.graphics.DrawImage(image, new Rectangle(rect.Left + stretchRect.Left, rect.Bottom - height, (rect.Width - stretchRect.Left) - width, height), imageRect.Left + stretchRect.Left, imageRect.Top + stretchRect.Bottom, stretchRect.Width, height, GraphicsUnit.Pixel);
                this.graphics.DrawImage(image, rect.Right - width, rect.Top, new Rectangle(imageRect.Right - width, imageRect.Top, width, stretchRect.Top), GraphicsUnit.Pixel);
                this.graphics.DrawImage(image, new Rectangle(rect.Right - width, rect.Top + stretchRect.Top, width, (rect.Height - height) - stretchRect.Top), imageRect.Left + stretchRect.Right, imageRect.Top + stretchRect.Top, width, stretchRect.Height, GraphicsUnit.Pixel);
                this.graphics.DrawImage(image, rect.Right - width, rect.Bottom - height, new Rectangle(imageRect.Left + stretchRect.Right, imageRect.Top + stretchRect.Bottom, width, height), GraphicsUnit.Pixel);
            }
        }

        public virtual int StringWidth(string text)
        {
            return this.StringWidth(text, 0, -1);
        }

        public virtual int StringWidth(string text, int pos, int len)
        {
            if (len == 0x7fffffff)
            {
                return 0x7fffffff;
            }
            if (text == null)
            {
                return 0;
            }
            if (len == -1)
            {
                len = text.Length;
            }
            if (this.IsFontMonoSpaced() && (base.CurrentInfo != null))
            {
                return (len * base.CurrentInfo.FontWidth);
            }
            StringFormatFlags formatFlags = this.StringFormat.FormatFlags;
            string str = text.Substring(pos, len);
            StringFormat stringFormat = this.StringFormat;
            stringFormat.FormatFlags &= ~StringFormatFlags.NoClip;
            int width = (int) this.measureGraphics.MeasureString(str, this.Font, new PointF(0f, 0f), this.StringFormat).Width;
            this.StringFormat.FormatFlags = formatFlags;
            return width;
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
            count = 0;
            if ((text == null) || (width <= 0))
            {
                return 0;
            }
            for (int i = pos; i < (pos + len); i++)
            {
                int num = this.CharWidth(text[i], 1);
                width -= num;
                if (width < 0)
                {
                    if (!exact && (width > (-num / 2)))
                    {
                        count++;
                    }
                    break;
                }
                count++;
            }
            return this.StringWidth(text, pos, count);
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
                string s = (len == -1) ? text : text.Substring(0, len);
                StringFormatFlags formatFlags = this.StringFormat.FormatFlags;
                if (clipped)
                {
                    StringFormat stringFormat = this.StringFormat;
                    stringFormat.FormatFlags &= ~StringFormatFlags.NoClip;
                }
                else
                {
                    StringFormat format2 = this.StringFormat;
                    format2.FormatFlags |= StringFormatFlags.NoClip;
                }
                if (opaque)
                {
                    this.graphics.FillRectangle(this.brush, rect);
                }
                rect.X = x;
                rect.Y = y;
                this.graphics.DrawString(s, this.Font, this.textBrush, rect, this.StringFormat);
                this.StringFormat.FormatFlags = formatFlags;
            }
        }

        public virtual void Transform(int x, int y, float scaleX, float scaleY)
        {
            this.transFormed = true;
            this.graphics.Transform = new Matrix(scaleX, 0f, 0f, scaleY, x * scaleX, y * scaleY);
        }

        public System.Drawing.Brush Brush
        {
            get
            {
                return this.brush;
            }
            set
            {
                if (this.brush != value)
                {
                    this.brush = value;
                    this.OnBrushChanged();
                }
            }
        }

        public virtual System.Drawing.Graphics Graphics
        {
            get
            {
                if (this.graphics != null)
                {
                    return this.graphics;
                }
                return null;
            }
        }

        public System.Drawing.Pen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                if (this.pen != value)
                {
                    this.pen = value;
                    this.OnPenChanged();
                }
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

