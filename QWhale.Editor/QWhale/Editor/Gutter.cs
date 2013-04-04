namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class Gutter : IGutter, IUpdate
    {
        private int bookMarkImageIndex;
        private System.Drawing.Brush brush;
        private IDrawInfo drawInfo;
        private bool drawLineBookmarks;
        private ImageList images;
        private ImageList internalImages;
        private Color lineBookmarksColor;
        private Color lineModificatorChangedColor;
        private Color lineModificatorSavedColor;
        private int lineNumberLength;
        private StringAlignment lineNumbersAlignment;
        private Color lineNumbersBackColor;
        private Color lineNumbersForeColor;
        private int lineNumbersLeftIndent;
        private int lineNumbersRightIndent;
        private int lineNumbersStart;
        private int lineNumberWidth;
        private int maxLineNumberLength;
        private GutterOptions options;
        private int outliningLeftIndent;
        private int outliningRightIndent;
        private ISyntaxEdit owner;
        private System.Drawing.Pen pen;
        private IList<IRange> ranges;
        private bool showBookmarkHints;
        private int updateCount;
        private Color userMarginBackColor;
        private Color userMarginForeColor;
        private Regex userMarginRegex;
        private string userMarginText;
        private int userMarginWidth;
        private bool visible;
        private int width;
        private int wrapImageIndex;

        [Browsable(false)]
        public event EventHandler Click;

        [Browsable(false)]
        public event EventHandler DoubleClick;

        [Browsable(false)]
        public event DrawUserMarginEvent DrawUserMargin;

        public Gutter()
        {
            this.visible = true;
            this.width = EditConsts.DefaultGutterWidth;
            this.lineNumbersStart = EditConsts.DefaultLineNumbersStart;
            this.lineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;
            this.lineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;
            this.outliningLeftIndent = EditConsts.DefaultOutliningIndent;
            this.outliningRightIndent = EditConsts.DefaultOutliningIndent;
            this.bookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;
            this.wrapImageIndex = EditConsts.DefaultWrapImageIndex;
            this.showBookmarkHints = true;
            this.lineBookmarksColor = EditConsts.DefaultLineBookmarksColor;
            this.userMarginWidth = EditConsts.DefaultUserMarginWidth;
            this.userMarginText = EditConsts.DefaultUserMarginText;
            this.drawInfo = new DrawInfo();
            this.brush = new SolidBrush(EditConsts.DefaultGutterBackColor);
            this.pen = new System.Drawing.Pen(EditConsts.DefaultGutterForeColor, 1f);
            this.internalImages = new ImageList();
            this.internalImages.ImageSize = new Size(15, 15);
            this.ranges = new List<IRange>();
            this.options = EditConsts.DefaultGutterOptions;
            this.lineNumbersForeColor = EditConsts.DefaultLineNumbersForeColor;
            this.lineNumbersBackColor = EditConsts.DefaultLineNumbersBackColor;
            this.lineModificatorChangedColor = EditConsts.DefaultLineModificatorChangedColor;
            this.lineModificatorSavedColor = EditConsts.DefaultLineModificatorSavedColor;
            this.userMarginForeColor = EditConsts.DefaultUserMarginForeColor;
            this.userMarginBackColor = EditConsts.DefaultUserMarginBackColor;
            try
            {
                ResourceManager manager = new ResourceManager(typeof(Resources));
                this.internalImages.ImageStream = (ImageListStreamer) manager.GetObject("SyntaxEdit.Gutter.Images.ImageStream");
            }
            catch
            {
            }
            this.internalImages.TransparentColor = EditConsts.DefaultTransparentColor;
        }

        public Gutter(ISyntaxEdit owner) : this()
        {
            this.owner = owner;
        }

        private bool AllowOutlining()
        {
            return ((this.owner != null) && this.owner.Outlining.AllowOutlining);
        }

        public virtual void Assign(IGutter source)
        {
            this.BeginUpdate();
            try
            {
                this.width = source.Width;
                if (source.Brush is SolidBrush)
                {
                    ((SolidBrush) this.brush).Color = ((SolidBrush) source.Brush).Color;
                }
                this.Pen.Color = source.Pen.Color;
                this.Pen.Width = source.Pen.Width;
                this.Visible = source.Visible;
                this.LineNumbersStart = source.LineNumbersStart;
                this.LineNumbersLeftIndent = source.LineNumbersLeftIndent;
                this.LineNumbersRightIndent = source.LineNumbersRightIndent;
                this.LineNumbersForeColor = source.LineNumbersForeColor;
                this.LineNumbersBackColor = source.LineNumbersBackColor;
                this.LineNumbersAlignment = source.LineNumbersAlignment;
                this.UserMarginForeColor = source.UserMarginForeColor;
                this.UserMarginBackColor = source.UserMarginBackColor;
                this.UserMarginText = source.UserMarginText;
                this.UserMarginWidth = source.UserMarginWidth;
                this.OutliningLeftIndent = source.OutliningLeftIndent;
                this.OutliningRightIndent = source.OutliningRightIndent;
                this.Options = source.Options;
                this.BookMarkImageIndex = source.BookMarkImageIndex;
                this.WrapImageIndex = source.WrapImageIndex;
                if (this.images == null)
                {
                    this.Images.ImageStream = source.Images.ImageStream;
                }
                else
                {
                    this.Images = source.Images;
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        protected bool CanDrawImage(int index)
        {
            return ((index >= 0) && (index < this.Images.Images.Count));
        }

        protected bool CanDrawImage(int index, int left, int right)
        {
            return (((index >= 0) && (index < this.Images.Images.Count)) && ((left + this.Images.ImageSize.Width) <= right));
        }

        private void CenterOutlineRect(ref int l, ref int t, int w)
        {
            int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
            int num2 = (w - defaultCollasedImageWidth) >> 1;
            if (num2 > 0)
            {
                l += num2;
                t += num2;
            }
        }

        private int CompareRange(Point pt, Point sPt, Point ePt)
        {
            if ((pt.Y < sPt.Y) || ((pt.Y == sPt.Y) && (pt.X < sPt.X)))
            {
                return 1;
            }
            if ((pt.Y <= ePt.Y) && (((pt.Y != ePt.Y) || (pt.X < ePt.X)) || (ePt.X == 0x7fffffff)))
            {
                return 0;
            }
            return -1;
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        private void DrawGutter(IPainter painter, Rectangle rect, int startLine)
        {
            int imageIndex = -1;
            bool bookMark = false;
            this.DrawGutter(painter, rect, false, startLine, 0, ref imageIndex, ref bookMark);
        }

        private void DrawGutter(IPainter painter, Rectangle rect, bool calcIndex, int line, int imageX, ref int imageIndex, ref bool bookMark)
        {
            int top = rect.Top;
            int fontHeight = painter.FontHeight;
            bool drawLines = (this.owner.Outlining.OutlineOptions & OutlineOptions.DrawLines) != OutlineOptions.None;
            bool allowOutlining = this.owner.Outlining.AllowOutlining;
            bool flag3 = (this.options & GutterOptions.PaintLinesBeyondEof) != GutterOptions.None;
            bool flag4 = this.NeedLineNumbers();
            bool flag5 = (this.options & GutterOptions.PaintBookMarks) != GutterOptions.None;
            bool flag6 = this.GetUserMarginWidth() > 1;
            bool drawOnGutter = flag4 && (this.visible & ((this.options & GutterOptions.PaintLinesOnGutter) != GutterOptions.None));
            bool flag8 = (this.options & GutterOptions.PaintLineModificators) != GutterOptions.None;
            int lineNumbersLeft = this.GetLineNumbersLeft(true);
            int lineNumbersRight = this.GetLineNumbersRight(true);
            int outlineLeft = this.GetOutlineLeft(true);
            int lineModificatorLeft = this.GetLineModificatorLeft();
            int right = rect.Left + this.GetPaintWidth(false);
            int left = this.GetPaintWidth() - this.GetUserMarginWidth();
            int displayCount = this.owner.DisplayLines.DisplayCount;
            IList<IBookMark> list = new List<IBookMark>();
            IList<ILineStyle> list2 = new List<ILineStyle>();
            IList<int> list3 = new List<int>();
            NavigateOptions navigateOptions = this.owner.NavigateOptions;
            try
            {
                this.owner.SetNavigateOptions(navigateOptions | NavigateOptions.BeyondEof);
                while ((top < rect.Bottom) || calcIndex)
                {
                    Point startPoint = this.owner.DisplayLines.DisplayPointToPoint(0, line);
                    if ((line >= displayCount) && (!flag4 || !flag3))
                    {
                        break;
                    }
                    if (flag8 && !calcIndex)
                    {
                        this.DrawLineModificator(painter, startPoint.Y, lineModificatorLeft, top);
                    }
                    if (flag4 && !calcIndex)
                    {
                        if (startPoint.X == 0)
                        {
                            this.DrawLineNumber(painter, startPoint.Y, lineNumbersLeft, top, drawOnGutter);
                        }
                        else if (!this.Transparent)
                        {
                            Color backColor = painter.BackColor;
                            try
                            {
                                painter.BackColor = drawOnGutter ? this.BrushColor : this.lineNumbersBackColor;
                                painter.FillRectangle(lineNumbersLeft, top, lineNumbersRight - lineNumbersLeft, fontHeight);
                            }
                            finally
                            {
                                painter.BackColor = backColor;
                            }
                        }
                    }
                    if (flag6 && !calcIndex)
                    {
                        this.DrawLineOnUserMargin(painter, startPoint.X, startPoint.Y, left, top);
                    }
                    if (line < displayCount)
                    {
                        if (this.Visible)
                        {
                            list3.Clear();
                            int num11 = -1;
                            if ((this.owner.WordWrap && (startPoint.X != 0)) && ((this.wrapImageIndex >= 0) && this.CanDrawImage(this.wrapImageIndex)))
                            {
                                list3.Add(this.wrapImageIndex);
                            }
                            if (flag5)
                            {
                                Point endPoint = this.owner.DisplayLines.DisplayPointToPoint(0x7fffffff, line, true, false, false);
                                this.owner.Source.BookMarks.GetBookMarks(startPoint, endPoint, list);
                                foreach (IBookMark mark in list)
                                {
                                    if (!this.owner.Outlining.AllowOutlining || this.owner.Outlining.IsVisible(new Point(mark.Pos, mark.Line)))
                                    {
                                        int index = mark.Index;
                                        if (index == 0x7fffffff)
                                        {
                                            index = this.bookMarkImageIndex;
                                        }
                                        if (this.CanDrawImage(index))
                                        {
                                            list3.Add(index);
                                            num11 = index;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (((top >= (rect.Top - this.Images.ImageSize.Height)) && (startPoint.X == 0)) && (!this.owner.Outlining.AllowOutlining || this.owner.Outlining.IsVisible(startPoint.Y)))
                            {
                                this.owner.Source.LineStyles.GetLineStyles(startPoint.Y, list2);
                                if (list2.Count > 0)
                                {
                                    for (int i = list2.Count - 1; i >= 0; i--)
                                    {
                                        IEditLineStyle style = this.owner.LineStyles[list2[i].Index];
                                        if (style != null)
                                        {
                                            int num14 = style.ImageIndex;
                                            if (this.CanDrawImage(num14))
                                            {
                                                list3.Add(num14);
                                            }
                                        }
                                    }
                                }
                            }
                            if (list3.Count > 0)
                            {
                                int x = 0;
                                Size imageSize = this.Images.ImageSize;
                                Rectangle empty = Rectangle.Empty;
                                if (!this.Transparent && (imageSize.Width != 0))
                                {
                                    int introduced51 = Math.Min(list3.Count, right / imageSize.Width);
                                    empty = new Rectangle(x, top, introduced51 * imageSize.Width, imageSize.Height);
                                    painter.FillRectangle(empty);
                                }
                                foreach (int num15 in list3)
                                {
                                    if ((calcIndex && (imageX >= x)) && (imageX < (x + imageSize.Width)))
                                    {
                                        imageIndex = num15;
                                        bookMark = this.IsBookmarkImageIndex(num15);
                                        return;
                                    }
                                    if (!calcIndex)
                                    {
                                        this.DrawImage(painter, num15, startPoint.Y, x, top, right, num11 == num15);
                                    }
                                    if (x <= (right - (imageSize.Width * 2)))
                                    {
                                        x += imageSize.Width;
                                    }
                                }
                                if (!this.Transparent && !empty.IsEmpty)
                                {
                                    painter.ExcludeClipRect(empty.Left, empty.Top, empty.Width, empty.Height);
                                }
                            }
                        }
                        if (allowOutlining && !calcIndex)
                        {
                            this.owner.Outlining.GetOutlineRanges(this.ranges, startPoint.Y);
                            Point ePt = this.owner.DisplayLines.DisplayPointToPoint(0x7fffffff, line, true, false, false);
                            bool flag9 = false;
                            bool visible = true;
                            foreach (IOutlineRange range in this.ranges)
                            {
                                if (this.CompareRange(range.StartPoint, startPoint, ePt) == 0)
                                {
                                    visible &= range.Visible;
                                    flag9 = true;
                                }
                            }
                            if (flag9)
                            {
                                IRange range2 = this.ranges[0];
                                this.DrawOutlineImage(painter, startPoint.Y, outlineLeft, top, fontHeight, drawLines, visible, this.CompareRange(range2.StartPoint, startPoint, ePt) > 0, this.CompareRange(range2.EndPoint, startPoint, ePt) < 0);
                            }
                            if (!flag9 && drawLines)
                            {
                                bool flag11 = false;
                                bool end = false;
                                foreach (IOutlineRange range3 in this.ranges)
                                {
                                    int num16 = this.CompareRange(range3.StartPoint, startPoint, ePt);
                                    int num17 = this.CompareRange(range3.EndPoint, startPoint, ePt);
                                    flag11 |= (num16 > 0) && (num17 <= 0);
                                    end |= num17 == 0;
                                }
                                if (flag11)
                                {
                                    this.DrawOutline(painter, startPoint.Y, outlineLeft, top, fontHeight, end);
                                }
                            }
                        }
                    }
                    if (calcIndex)
                    {
                        break;
                    }
                    top += fontHeight;
                    line++;
                }
                if (!calcIndex)
                {
                    if ((flag4 && !this.Transparent) && ((top > rect.Top) && (lineNumbersRight > lineNumbersLeft)))
                    {
                        painter.ExcludeClipRect(lineNumbersLeft, rect.Top, lineNumbersRight - lineNumbersLeft, top - rect.Top);
                    }
                    if ((flag6 && !this.Transparent) && (top > rect.Top))
                    {
                        painter.ExcludeClipRect(left, rect.Top, this.GetUserMarginWidth() - 1, top - rect.Top);
                    }
                }
            }
            finally
            {
                this.owner.SetNavigateOptions(navigateOptions);
            }
        }

        private void DrawImage(IPainter painter, int imageIndex, int line, int left, int top, int right, bool bookmark)
        {
            if (this.CanDrawImage(imageIndex, left, right))
            {
                this.drawInfo.Reset();
                this.drawInfo.Line = line;
                this.drawInfo.GutterImage = imageIndex;
                Rectangle rect = new Rectangle(left, top, this.Images.ImageSize.Width, this.Images.ImageSize.Height);
                if (!this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.Before, (DrawState.GutterImage | DrawState.Gutter) | (bookmark ? DrawState.BookMark : DrawState.None), this.drawInfo))
                {
                    painter.DrawImage(this.Images, imageIndex, rect);
                }
                this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.After, DrawState.GutterImage | DrawState.Gutter, this.drawInfo);
            }
        }

        private void DrawLine(IPainter painter, int x1, int y1, int x2, int y2)
        {
            painter.DrawLine(x1, y1, x2, y2);
            if (!this.Transparent)
            {
                if (x1 == x2)
                {
                    painter.ExcludeClipRect(x1, y1, 1, y2 - y1);
                }
                else
                {
                    painter.ExcludeClipRect(x1, y1, x2 - x1, 1);
                }
            }
        }

        private void DrawLineModificator(IPainter painter, int line, int left, int top)
        {
            if (this.owner != null)
            {
                Rectangle rect = new Rectangle(left, top, this.GetLineModificatorWidth(false), painter.FontHeight);
                this.drawInfo.Reset();
                this.drawInfo.Line = line;
                Color backColor = painter.BackColor;
                try
                {
                    bool flag;
                    bool flag2 = this.owner.Source.LineIsModified(line, out flag);
                    if (flag2)
                    {
                        painter.BackColor = flag ? this.LineModificatorSavedColor : this.LineModificatorChangedColor;
                    }
                    if (!this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.LineModificator | DrawState.Gutter, this.drawInfo) && flag2)
                    {
                        painter.FillRectangle(rect);
                        painter.ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
                    }
                    this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.After, DrawState.LineModificator | DrawState.Gutter, this.drawInfo);
                }
                finally
                {
                    painter.BackColor = backColor;
                }
            }
        }

        private void DrawLineNumber(IPainter painter, int line, int left, int top, bool drawOnGutter)
        {
            if (this.owner != null)
            {
                Rectangle rect = new Rectangle(left, top, this.lineNumberWidth - (this.lineNumbersLeftIndent + this.lineNumbersRightIndent), painter.FontHeight);
                this.drawInfo.Reset();
                this.drawInfo.Line = line;
                Color textColor = painter.TextColor;
                Color backColor = painter.BackColor;
                bool transparent = this.Transparent;
                try
                {
                    painter.TextColor = this.lineNumbersForeColor;
                    painter.BackColor = drawOnGutter ? this.BrushColor : this.lineNumbersBackColor;
                    if (transparent)
                    {
                        painter.Opaque = false;
                    }
                    this.drawInfo.Text = (line + this.lineNumbersStart).ToString();
                    if (!this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.LineNumber | DrawState.Gutter, this.drawInfo))
                    {
                        switch (this.lineNumbersAlignment)
                        {
                            case StringAlignment.Near:
                                painter.TextOut(this.drawInfo.Text, -1, rect, false, !transparent);
                                break;

                            case StringAlignment.Center:
                            {
                                int num2 = painter.StringWidth(this.drawInfo.Text);
                                painter.TextOut(this.drawInfo.Text, -1, rect, rect.Left + ((rect.Width - num2) / 2), rect.Top, false, !transparent);
                                break;
                            }
                            case StringAlignment.Far:
                            {
                                int num = painter.StringWidth(this.drawInfo.Text);
                                painter.TextOut(this.drawInfo.Text, -1, rect, (rect.Left + rect.Width) - num, rect.Top, false, !transparent);
                                break;
                            }
                        }
                    }
                    this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.After, DrawState.LineNumber | DrawState.Gutter, this.drawInfo);
                }
                finally
                {
                    if (transparent)
                    {
                        painter.Opaque = true;
                    }
                    painter.TextColor = textColor;
                    painter.BackColor = backColor;
                }
            }
        }

        private void DrawLineOnUserMargin(IPainter painter, int pos, int line, int left, int top)
        {
            if (this.owner != null)
            {
                Rectangle rect = new Rectangle(left, top, this.userMarginWidth, painter.FontHeight);
                this.drawInfo.Reset();
                this.drawInfo.Line = line;
                Color textColor = painter.TextColor;
                Color backColor = painter.BackColor;
                bool transparent = this.Transparent;
                try
                {
                    painter.TextColor = this.userMarginForeColor;
                    painter.BackColor = this.userMarginBackColor;
                    if (transparent)
                    {
                        painter.Opaque = false;
                    }
                    this.drawInfo.Text = (pos == 0) ? this.GetUserMarginText(this.userMarginText, line, this.owner.Lines.GetLength(line)) : string.Empty;
                    if (!this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.UserMargin | DrawState.Gutter, this.drawInfo))
                    {
                        painter.TextOut(this.drawInfo.Text, -1, rect, true, !transparent);
                    }
                    this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.After, DrawState.UserMargin | DrawState.Gutter, this.drawInfo);
                }
                finally
                {
                    if (transparent)
                    {
                        painter.Opaque = true;
                    }
                    painter.TextColor = textColor;
                    painter.BackColor = backColor;
                }
            }
        }

        private void DrawOutline(IPainter painter, int line, int l, int t, int w, bool end)
        {
            int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
            int num2 = t;
            this.CenterOutlineRect(ref l, ref t, w);
            Rectangle rect = new Rectangle(l, t, defaultCollasedImageWidth, w);
            this.drawInfo.Reset();
            this.drawInfo.Line = line;
            Color foreColor = painter.ForeColor;
            try
            {
                painter.ForeColor = this.owner.Outlining.OutlineColor;
                if (!this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.OutlineArea | DrawState.Gutter, this.drawInfo))
                {
                    this.DrawLine(painter, l + (defaultCollasedImageWidth >> 1), num2, l + (defaultCollasedImageWidth >> 1), num2 + w);
                    if (end)
                    {
                        this.DrawLine(painter, l + (defaultCollasedImageWidth >> 1), (num2 + w) - 1, (l + defaultCollasedImageWidth) + 1, (num2 + w) - 1);
                    }
                }
            }
            finally
            {
                painter.ForeColor = foreColor;
            }
            this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.After, DrawState.OutlineArea | DrawState.Gutter, this.drawInfo);
        }

        private void DrawOutlineImage(IPainter painter, int line, int l, int t, int w, bool drawLines, bool visible, bool before, bool after)
        {
            int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
            int num2 = t;
            this.CenterOutlineRect(ref l, ref t, w);
            Rectangle rect = new Rectangle(l, t, defaultCollasedImageWidth, defaultCollasedImageWidth);
            this.drawInfo.Reset();
            this.drawInfo.Line = line;
            IOutlining outlining = this.owner.Outlining;
            Color backColor = painter.BackColor;
            Color foreColor = painter.ForeColor;
            try
            {
                painter.BackColor = outlining.OutlineColor;
                painter.ForeColor = outlining.OutlineColor;
                if (!this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.OutlineImage | DrawState.Gutter, this.drawInfo))
                {
                    this.DrawRectangle(painter, rect, !visible);
                    if (drawLines)
                    {
                        if (before)
                        {
                            this.DrawLine(painter, l + (defaultCollasedImageWidth >> 1), num2, l + (defaultCollasedImageWidth >> 1), t);
                        }
                        if (after)
                        {
                            this.DrawLine(painter, l + (defaultCollasedImageWidth >> 1), t + defaultCollasedImageWidth, l + (defaultCollasedImageWidth >> 1), num2 + w);
                        }
                    }
                    this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.After, DrawState.OutlineImage | DrawState.Gutter, this.drawInfo);
                }
            }
            finally
            {
                painter.BackColor = backColor;
                painter.ForeColor = foreColor;
            }
        }

        private void DrawRectangle(IPainter painter, Rectangle rect, bool drawPlus)
        {
            Rectangle rectangle = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 1, rect.Height - 1);
            if (!this.Transparent)
            {
                Color backColor = painter.BackColor;
                try
                {
                    painter.BackColor = (this.visible && ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != OutlineOptions.None)) ? this.BrushColor : this.owner.SyntaxPaint.GetBackColor(false);
                    painter.FillRectangle(rectangle);
                }
                finally
                {
                    painter.BackColor = backColor;
                }
            }
            int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
            painter.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
            painter.DrawLine(rect.Left + 2, rect.Top + (defaultCollasedImageWidth >> 1), (rect.Left + defaultCollasedImageWidth) - 2, rect.Top + (defaultCollasedImageWidth >> 1));
            if (drawPlus)
            {
                painter.DrawLine(rect.Left + (defaultCollasedImageWidth >> 1), rect.Top + 2, rect.Left + (defaultCollasedImageWidth >> 1), (rect.Top + defaultCollasedImageWidth) - 2);
            }
            if (!this.Transparent)
            {
                painter.ExcludeClipRect(rect.Left, rect.Top, rect.Width + 1, rect.Height + 1);
            }
        }

        public virtual int EnableUpdate()
        {
            this.updateCount--;
            return this.updateCount;
        }

        public virtual int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
            return this.updateCount;
        }

        ~Gutter()
        {
            this.ranges.Clear();
            this.brush.Dispose();
            this.pen.Dispose();
            this.internalImages.ImageStream = null;
            this.internalImages.Dispose();
        }

        protected virtual int GetDisplayWidth(bool charWidth)
        {
            int num = this.visible ? this.Width : 0;
            int lineNumbersWidth = this.GetLineNumbersWidth();
            if ((this.options & GutterOptions.PaintLinesOnGutter) == GutterOptions.None)
            {
                num += lineNumbersWidth;
            }
            else
            {
                num = Math.Max(num, lineNumbersWidth);
            }
            return (((num + this.GetLineModificatorWidth(true, charWidth)) + this.GetOutlineWidth()) + this.GetUserMarginWidth());
        }

        public virtual void GetHitTest(int x, int y, IHitTestInfo hitTestInfo)
        {
            IOutlineRange range;
            int num;
            bool flag;
            if (this.IsMouseOnGutter(x, y, false))
            {
                hitTestInfo.HitTest |= HitTest.Gutter;
            }
            if (this.IsMouseOnOutlineArea(x, y))
            {
                hitTestInfo.HitTest |= HitTest.OutlineArea;
            }
            Point outPt = new Point(0, 0);
            if (this.IsMouseOnOutlineImage(x, y, ref outPt))
            {
                hitTestInfo.HitTest |= HitTest.OutlineImage;
                hitTestInfo.OutlineIndex = outPt.Y;
            }
            if (this.IsMouseOnOutlineButton(x, y, out range))
            {
                hitTestInfo.HitTest |= HitTest.OutlineButton;
                hitTestInfo.OutlineRange = range;
            }
            if (this.IsMouseOnLineModificatorArea(x, y))
            {
                hitTestInfo.HitTest |= HitTest.LineModificator;
            }
            if (this.IsMouseOnGutterImage(x, y, out num, out flag))
            {
                hitTestInfo.HitTest |= HitTest.GutterImage;
                if (flag)
                {
                    hitTestInfo.HitTest |= HitTest.BookMark;
                }
                hitTestInfo.GutterImage = num;
            }
            if (this.IsMouseOnLineNumberArea(x, y))
            {
                hitTestInfo.HitTest |= HitTest.LineNumber;
            }
        }

        private int GetLineModificatorLeft()
        {
            return (this.DisplayWidth - (((this.AllowOutlining() && (this.owner != null)) && (!this.Visible || ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) == OutlineOptions.None))) ? this.GetOutlineWidth() : this.GetLineModificatorWidth(false)));
        }

        private int GetLineModificatorWidth(bool checkOutlining)
        {
            return this.GetLineModificatorWidth(checkOutlining, false);
        }

        private int GetLineModificatorWidth(bool checkOutlining, bool charWidth)
        {
            int num = ((this.options & GutterOptions.PaintLineModificators) != GutterOptions.None) ? EditConsts.DefaultLineModificatorWidth : 0;
            if (((charWidth || (num == 0)) || (!checkOutlining || !this.AllowOutlining())) || ((this.owner == null) || (this.Visible && ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != OutlineOptions.None))))
            {
                return num;
            }
            return 0;
        }

        private int GetLineNumbersLeft(bool useIndent)
        {
            if (!this.NeedLineNumbers())
            {
                return 0;
            }
            return (((this.visible && ((this.options & GutterOptions.PaintLinesOnGutter) == GutterOptions.None)) ? this.GetPaintWidth() : 0) + (useIndent ? this.lineNumbersLeftIndent : 0));
        }

        private int GetLineNumbersRight(bool useIndent)
        {
            if (!this.NeedLineNumbers())
            {
                return 0;
            }
            return ((this.GetLineNumbersLeft(useIndent) + this.lineNumberWidth) - (useIndent ? (this.lineNumbersLeftIndent + this.lineNumbersRightIndent) : 0));
        }

        private int GetLineNumbersWidth()
        {
            if ((this.options & GutterOptions.PaintLineNumbers) == GutterOptions.None)
            {
                return 0;
            }
            return this.lineNumberWidth;
        }

        private int GetOutlineLeft(bool useIndent)
        {
            if (((this.owner != null) && ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != OutlineOptions.None)) && this.Visible)
            {
                return (((this.GetPaintWidth() - this.GetUserMarginWidth()) - this.GetOutlineWidth()) + (useIndent ? this.outliningLeftIndent : 0));
            }
            return ((this.DisplayWidth - this.GetOutlineWidth()) + (useIndent ? this.outliningLeftIndent : 0));
        }

        private int GetOutlineWidth()
        {
            if (!this.AllowOutlining())
            {
                return 0;
            }
            return ((this.owner.Painter.FontHeight + this.outliningLeftIndent) + this.outliningRightIndent);
        }

        private int GetPaintWidth()
        {
            return this.GetPaintWidth(true);
        }

        private int GetPaintWidth(bool CheckOutlining)
        {
            int num = this.visible ? this.Width : 0;
            if ((this.options & GutterOptions.PaintLinesOnGutter) != GutterOptions.None)
            {
                num = Math.Max(num, this.GetLineNumbersWidth());
            }
            num += this.GetUserMarginWidth();
            if ((CheckOutlining && (this.owner != null)) && ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != OutlineOptions.None))
            {
                num += this.GetOutlineWidth();
            }
            return num;
        }

        private string GetUserMarginText(string text, int line, int chars)
        {
            MatchCollection matchs = this.UserMarginRegex.Matches(text);
            for (int i = matchs.Count - 1; i >= 0; i--)
            {
                Match match = matchs[i];
                if (match.Success)
                {
                    string strA = text.Substring(match.Index, match.Length);
                    if (string.Compare(strA, EditConsts.LineTag) == 0)
                    {
                        strA = (line + 1).ToString();
                    }
                    if (string.Compare(strA, EditConsts.CharsTag) == 0)
                    {
                        strA = chars.ToString();
                    }
                    else
                    {
                        this.OnDrawUserMargin(ref strA, line);
                    }
                    text = text.Remove(match.Index, match.Length);
                    text = text.Insert(match.Index, strA);
                }
            }
            return text;
        }

        private int GetUserMarginWidth()
        {
            if ((this.options & GutterOptions.PaintUserMargin) == GutterOptions.None)
            {
                return 0;
            }
            return this.userMarginWidth;
        }

        public virtual bool InvalidateLineNumberArea()
        {
            bool flag = this.NeedLineNumbers();
            if (flag)
            {
                flag = this.UpdateLineNumberLength();
                if (flag)
                {
                    if (this.owner != null)
                    {
                        if (this.owner.WordWrap)
                        {
                            this.owner.UpdateWordWrap();
                        }
                        this.owner.Invalidate();
                        this.owner.UpdateCaret();
                    }
                    return flag;
                }
                if (((this.options & GutterOptions.PaintLineNumbers) == GutterOptions.None) || (((this.options & GutterOptions.PaintLinesOnGutter) != GutterOptions.None) && this.Visible))
                {
                    return flag;
                }
                if ((this.owner != null) && (this.owner.Pages.PageType != PageType.PageLayout))
                {
                    int lineNumbersRight = this.GetLineNumbersRight(false);
                    this.owner.Invalidate(new Rectangle(lineNumbersRight - 1, 0, 1, this.owner.ClientHeight));
                }
            }
            return flag;
        }

        private bool IsBookmarkImageIndex(int index)
        {
            return (((index >= 0) && (index <= 9)) || (index == this.bookMarkImageIndex));
        }

        protected virtual bool IsMouseOnGutter(int x, int y, bool strictCheck)
        {
            x = this.ScreenToClient(x, y).X;
            return ((x >= this.Rect.Left) && (x <= (this.Rect.Left + (strictCheck ? this.GetPaintWidth() : this.DisplayWidth))));
        }

        protected virtual bool IsMouseOnGutterImage(int x, int y, out int imageIndex, out bool bookMark)
        {
            imageIndex = -1;
            bookMark = false;
            if (this.Visible && this.IsMouseOnGutter(x, y, true))
            {
                x = this.ScreenToClient(x, y).X;
                Point point = this.owner.ScreenToDisplay(x, y);
                Rectangle rect = this.Rect;
                this.DrawGutter(this.owner.Painter, new Rectangle(rect.Left, rect.Top, this.Width, 0), true, point.Y, x, ref imageIndex, ref bookMark);
            }
            return (imageIndex >= 0);
        }

        protected virtual bool IsMouseOnLineModificatorArea(int x, int y)
        {
            if ((this.options & GutterOptions.PaintLineModificators) == GutterOptions.None)
            {
                return false;
            }
            x = this.ScreenToClient(x, y).X;
            int lineModificatorLeft = this.GetLineModificatorLeft();
            return ((x >= lineModificatorLeft) && (x <= (lineModificatorLeft + this.GetLineModificatorWidth(false))));
        }

        protected virtual bool IsMouseOnLineNumberArea(int x, int y)
        {
            if ((this.options & GutterOptions.PaintLineNumbers) == GutterOptions.None)
            {
                return false;
            }
            x = this.ScreenToClient(x, y).X;
            return ((x >= this.GetLineNumbersLeft(false)) && (x <= this.GetLineNumbersRight(false)));
        }

        protected virtual bool IsMouseOnOutlineArea(int x, int y)
        {
            if (!this.AllowOutlining())
            {
                return false;
            }
            x = this.ScreenToClient(x, y).X;
            int num = this.GetOutlineLeft(false) + this.Rect.Left;
            return ((x >= num) && (x <= (num + this.GetOutlineWidth())));
        }

        public virtual bool IsMouseOnOutlineButton(int x, int y, out IOutlineRange range)
        {
            Rectangle clientRect;
            range = null;
            if (((this.owner == null) || !this.owner.Outlining.AllowOutlining) || ((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawButtons) == OutlineOptions.None))
            {
                return false;
            }
            if (this.owner.Pages.PageType == PageType.PageLayout)
            {
                clientRect = this.owner.Pages.GetPageAtPoint(x, y).ClientRect;
            }
            else
            {
                clientRect = this.owner.ClientRect;
            }
            int displayWidth = this.DisplayWidth;
            clientRect.X += displayWidth;
            clientRect.Width -= displayWidth;
            if (!clientRect.Contains(x, y))
            {
                return false;
            }
            Point position = this.owner.ScreenToDisplay(x, y);
            position = this.owner.DisplayLines.DisplayPointToPoint(position.X, position.Y, false, true, false);
            range = this.owner.DisplayLines.GetOutlineRange(position);
            return ((range != null) && !range.Visible);
        }

        protected virtual bool IsMouseOnOutlineImage(int x, int y, ref Point outPt)
        {
            if (this.AllowOutlining())
            {
                IDisplayStrings displayLines = this.owner.DisplayLines;
                outPt = this.owner.ScreenToDisplay(x, y);
                outPt.X = 0;
                int num = outPt.Y;
                outPt = displayLines.DisplayPointToPoint(outPt);
                this.owner.Outlining.GetOutlineRanges(this.ranges, outPt.Y);
                Point ePt = displayLines.DisplayPointToPoint(0x7fffffff, num, true, false, false);
                bool flag = false;
                foreach (IOutlineRange range in this.ranges)
                {
                    if (this.CompareRange(range.StartPoint, outPt, ePt) == 0)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    int l = this.GetOutlineLeft(true) + this.Rect.Left;
                    int fontHeight = this.owner.Painter.FontHeight;
                    Point point2 = this.ScreenToClient(x, y);
                    int top = this.Rect.Top;
                    int t = (fontHeight == 0) ? point2.Y : (top + (((point2.Y - top) / fontHeight) * fontHeight));
                    int defaultCollasedImageWidth = EditConsts.DefaultCollasedImageWidth;
                    this.CenterOutlineRect(ref l, ref t, fontHeight);
                    return ((((point2.X >= (l - 1)) && (point2.X <= ((l + defaultCollasedImageWidth) + 1))) && (point2.Y >= (t - 1))) && (point2.Y <= ((t + defaultCollasedImageWidth) + 1)));
                }
            }
            return false;
        }

        private bool NeedLineNumbers()
        {
            return ((this.options & GutterOptions.PaintLineNumbers) != GutterOptions.None);
        }

        private bool NeedPaint(ref Rectangle rect)
        {
            if ((this.owner == null) || rect.IsEmpty)
            {
                return false;
            }
            int fontHeight = this.owner.Painter.FontHeight;
            if (fontHeight > 0)
            {
                int bottom = rect.Bottom;
                rect.Y = (rect.Top / fontHeight) * fontHeight;
                rect.Height = bottom - rect.Y;
            }
            return true;
        }

        protected virtual void OnBookMarkImageIndexChanged()
        {
            this.Update();
        }

        protected virtual void OnBrushChanged()
        {
            this.Update();
        }

        protected virtual void OnBrushColorChanged()
        {
            this.Update();
        }

        public virtual void OnClick(EventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        public virtual void OnDoubleClick(EventArgs e)
        {
            if (this.DoubleClick != null)
            {
                this.DoubleClick(this, e);
            }
        }

        protected virtual void OnDrawLineBookmarksChanged()
        {
            if (this.owner != null)
            {
                this.owner.Invalidate();
            }
        }

        protected virtual void OnDrawUserMargin(ref string text, int line)
        {
            if (this.DrawUserMargin != null)
            {
                DrawUserMarginEventArgs e = new DrawUserMarginEventArgs(text, line);
                this.DrawUserMargin(this, e);
                if (e.Handled)
                {
                    text = e.Text;
                }
            }
        }

        protected virtual void OnImagesChanged()
        {
            this.Update();
        }

        protected virtual void OnLineBookmarksColorChanged()
        {
            if (this.drawLineBookmarks && (this.owner != null))
            {
                this.owner.Invalidate();
            }
        }

        protected virtual void OnLineModificatorChangedColorChanged()
        {
            this.Update((this.options & GutterOptions.PaintLineModificators) != GutterOptions.None);
        }

        protected virtual void OnLineModificatorSavedColorChanged()
        {
            this.Update((this.options & GutterOptions.PaintLineModificators) != GutterOptions.None);
        }

        protected virtual void OnLineNumbersAlignmentChanged()
        {
            this.Update(this.NeedLineNumbers());
        }

        protected virtual void OnLineNumbersBackColorChanged()
        {
            this.Update(this.NeedLineNumbers());
        }

        protected virtual void OnLineNumbersForeColorChanged()
        {
            this.Update(this.NeedLineNumbers());
        }

        protected virtual void OnLineNumbersLeftIndentChanged()
        {
            this.InvalidateLineNumberArea();
        }

        protected virtual void OnLineNumbersRightIndentChanged()
        {
            this.InvalidateLineNumberArea();
        }

        protected virtual void OnLineNumbersStartChanged()
        {
            this.InvalidateLineNumberArea();
            this.Update();
        }

        protected virtual void OnMaxLineNumberLengthChanged()
        {
            this.InvalidateLineNumberArea();
            this.Update();
        }

        protected virtual void OnOptionsChanged()
        {
            this.InvalidateLineNumberArea();
            this.Update();
        }

        protected virtual void OnOutliningLeftIndentChanged()
        {
            this.Update();
        }

        protected virtual void OnOutliningRightIndentChanged()
        {
            this.Update();
        }

        protected virtual void OnPenChanged()
        {
            this.Update();
        }

        protected virtual void OnPenColorChanged()
        {
            this.Update();
        }

        protected virtual void OnShowBookmarkHintsChanged()
        {
        }

        protected virtual void OnUserMarginBackColorChanged()
        {
            if ((this.options & GutterOptions.PaintUserMargin) != GutterOptions.None)
            {
                this.Update();
            }
        }

        protected virtual void OnUserMarginForeColorChanged()
        {
            if ((this.options & GutterOptions.PaintUserMargin) != GutterOptions.None)
            {
                this.Update();
            }
        }

        protected virtual void OnUserMarginTextChanged()
        {
            if ((this.options & GutterOptions.PaintUserMargin) != GutterOptions.None)
            {
                this.Update();
            }
        }

        protected virtual void OnUserMarginWidthChanged()
        {
            if ((this.options & GutterOptions.PaintUserMargin) != GutterOptions.None)
            {
                this.Update();
            }
        }

        protected virtual void OnVisibleChanged()
        {
            if ((this.owner != null) && this.owner.WordWrap)
            {
                this.owner.UpdateWordWrap();
            }
            this.Update();
        }

        protected virtual void OnWidthChanged()
        {
            if (this.visible)
            {
                this.Update();
            }
        }

        protected virtual void OnWrapImageIndexChanged()
        {
            this.Update();
        }

        public virtual void Paint(IPainter painter, Rectangle rect)
        {
            this.Paint(painter, rect, (this.owner != null) ? this.owner.Scrolling.WindowOriginY : 0);
        }

        public virtual void Paint(IPainter painter, Rectangle rect, int startLine)
        {
            if (this.NeedPaint(ref rect))
            {
                Color backColor = painter.BackColor;
                try
                {
                    if (painter.FontHeight > 0)
                    {
                        startLine += rect.Top / painter.FontHeight;
                    }
                    painter.BackColor = (this.brush is SolidBrush) ? ((SolidBrush) this.brush).Color : EditConsts.DefaultGutterBackColor;
                    this.drawInfo.Reset();
                    if (!this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Gutter, this.drawInfo))
                    {
                        this.DrawGutter(painter, new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height), startLine);
                        int width = this.GetPaintWidth() - this.GetUserMarginWidth();
                        if (this.Visible && (width != 0))
                        {
                            if (!this.Transparent)
                            {
                                painter.FillRectangle(0, rect.Top, width - 1, rect.Height);
                            }
                            painter.DrawLine(width - 1, 0, width - 1, rect.Bottom - 1, this.Pen.Color, (int) this.Pen.Width, this.Pen.DashStyle);
                        }
                        width = this.GetUserMarginWidth();
                        if (width != 0)
                        {
                            int x = this.GetPaintWidth() - width;
                            if (!this.Transparent)
                            {
                                painter.BackColor = this.userMarginBackColor;
                                painter.FillRectangle(x, rect.Top, width - 1, rect.Height);
                            }
                            painter.DrawDotLine((x + width) - 1, 0, (x + width) - 1, rect.Bottom, this.userMarginForeColor, !this.Transparent ? this.userMarginBackColor : Color.Empty);
                        }
                        if (((this.options & GutterOptions.PaintLineNumbers) != GutterOptions.None) && (((this.options & GutterOptions.PaintLinesOnGutter) == GutterOptions.None) || !this.Visible))
                        {
                            int lineNumbersLeft = this.GetLineNumbersLeft(false);
                            int lineNumbersRight = this.GetLineNumbersRight(false);
                            if (lineNumbersLeft < lineNumbersRight)
                            {
                                if (!this.Transparent)
                                {
                                    painter.BackColor = this.lineNumbersBackColor;
                                    painter.FillRectangle(lineNumbersLeft, rect.Top, (lineNumbersRight - lineNumbersLeft) - 1, rect.Height);
                                }
                                painter.DrawDotLine(lineNumbersRight - 1, 0, lineNumbersRight - 1, rect.Bottom, this.lineNumbersForeColor, !this.Transparent ? this.lineNumbersBackColor : Color.Empty);
                            }
                        }
                        if (!this.Transparent)
                        {
                            if (((this.owner.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) == OutlineOptions.None) || !this.Visible)
                            {
                                width = this.GetOutlineWidth();
                                if (width != 0)
                                {
                                    painter.BackColor = this.owner.SyntaxPaint.GetBackColor(false);
                                    painter.FillRectangle(this.GetOutlineLeft(false), rect.Top, width, rect.Height);
                                }
                            }
                            width = this.GetLineModificatorWidth(true);
                            if (width != 0)
                            {
                                painter.BackColor = this.owner.SyntaxPaint.GetBackColor(false);
                                painter.FillRectangle(this.GetLineModificatorLeft(), rect.Top, width, rect.Height);
                            }
                        }
                        this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.After, DrawState.Gutter, this.drawInfo);
                    }
                }
                finally
                {
                    painter.BackColor = backColor;
                }
            }
        }

        public virtual void ResetBookMarkImageIndex()
        {
            this.BookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;
        }

        public virtual void ResetBrushColor()
        {
            this.BrushColor = EditConsts.DefaultGutterBackColor;
        }

        public virtual void ResetDrawLineBookmarks()
        {
            this.DrawLineBookmarks = false;
        }

        public virtual void ResetLineBookmarksColor()
        {
            this.LineBookmarksColor = EditConsts.DefaultLineBookmarksColor;
        }

        public virtual void ResetLineModificatorChangedColor()
        {
            this.LineModificatorChangedColor = EditConsts.DefaultLineModificatorChangedColor;
        }

        public virtual void ResetLineModificatorSavedColor()
        {
            this.LineModificatorSavedColor = EditConsts.DefaultLineModificatorSavedColor;
        }

        public virtual void ResetLineNumbersAlignment()
        {
            this.LineNumbersAlignment = StringAlignment.Near;
        }

        public virtual void ResetLineNumbersBackColor()
        {
            this.LineNumbersBackColor = EditConsts.DefaultLineNumbersBackColor;
        }

        public virtual void ResetLineNumbersForeColor()
        {
            this.LineNumbersForeColor = EditConsts.DefaultLineNumbersForeColor;
        }

        public virtual void ResetLineNumbersLeftIndent()
        {
            this.LineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;
        }

        public virtual void ResetLineNumbersRightIndent()
        {
            this.LineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;
        }

        public virtual void ResetLineNumbersStart()
        {
            this.LineNumbersStart = EditConsts.DefaultLineNumbersStart;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultGutterOptions;
        }

        public virtual void ResetOutliningLeftIndent()
        {
            this.OutliningLeftIndent = EditConsts.DefaultOutliningIndent;
        }

        public virtual void ResetOutliningRightIndent()
        {
            this.OutliningRightIndent = EditConsts.DefaultOutliningIndent;
        }

        public virtual void ResetPenColor()
        {
            this.PenColor = EditConsts.DefaultGutterForeColor;
        }

        public virtual void ResetShowBookmarkHints()
        {
            this.ShowBookmarkHints = true;
        }

        public virtual void ResetUserMarginBackColor()
        {
            this.UserMarginForeColor = EditConsts.DefaultUserMarginBackColor;
        }

        public virtual void ResetUserMarginForeColor()
        {
            this.UserMarginForeColor = EditConsts.DefaultUserMarginForeColor;
        }

        public virtual void ResetUserMarginText()
        {
            this.UserMarginText = EditConsts.DefaultUserMarginText;
        }

        public virtual void ResetUserMarginWidth()
        {
            this.UserMarginWidth = EditConsts.DefaultUserMarginWidth;
        }

        public virtual void ResetVisible()
        {
            this.Visible = true;
        }

        public virtual void ResetWidth()
        {
            this.Width = EditConsts.DefaultGutterWidth;
        }

        public virtual void ResetWrapImageIndex()
        {
            this.WrapImageIndex = EditConsts.DefaultWrapImageIndex;
        }

        private Point ScreenToClient(int x, int y)
        {
            if ((this.owner != null) && (this.owner.Pages.PageType == PageType.PageLayout))
            {
                Rectangle clientRect = this.owner.Pages.GetPageAtPoint(x, y).ClientRect;
                x -= clientRect.Left;
                y -= clientRect.Top;
            }
            return new Point(x, y);
        }

        public bool ShouldSerializeBookMarkImageIndex()
        {
            return (this.bookMarkImageIndex != EditConsts.DefaultBookMarkImageIndex);
        }

        public bool ShouldSerializeBrushColor()
        {
            return (this.BrushColor != EditConsts.DefaultGutterBackColor);
        }

        public bool ShouldSerializeLineBookmarksColor()
        {
            return (this.lineBookmarksColor != EditConsts.DefaultLineBookmarksColor);
        }

        public bool ShouldSerializeLineModificatorChangedColor()
        {
            return (this.lineModificatorChangedColor != EditConsts.DefaultLineModificatorChangedColor);
        }

        public virtual bool ShouldSerializeLineModificatorSavedColor()
        {
            return (this.lineModificatorSavedColor != EditConsts.DefaultLineModificatorSavedColor);
        }

        public bool ShouldSerializeLineNumbersBackColor()
        {
            return (this.lineNumbersBackColor != EditConsts.DefaultLineNumbersBackColor);
        }

        public bool ShouldSerializeLineNumbersForeColor()
        {
            return (this.lineNumbersForeColor != EditConsts.DefaultLineNumbersForeColor);
        }

        public bool ShouldSerializeLineNumbersLeftIndent()
        {
            return (this.lineNumbersLeftIndent != EditConsts.DefaultLineNumbersIndent);
        }

        public bool ShouldSerializeLineNumbersRightIndent()
        {
            return (this.lineNumbersRightIndent != EditConsts.DefaultLineNumbersIndent);
        }

        public bool ShouldSerializeLineNumbersStart()
        {
            return (this.lineNumbersStart != EditConsts.DefaultLineNumbersStart);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultGutterOptions);
        }

        public bool ShouldSerializeOutliningLeftIndent()
        {
            return (this.outliningLeftIndent != EditConsts.DefaultOutliningIndent);
        }

        public bool ShouldSerializeOutliningRightIndent()
        {
            return (this.outliningRightIndent != EditConsts.DefaultOutliningIndent);
        }

        public bool ShouldSerializePenColor()
        {
            return (this.PenColor != EditConsts.DefaultGutterForeColor);
        }

        public bool ShouldSerializeUserMarginBackColor()
        {
            return (this.userMarginBackColor != EditConsts.DefaultUserMarginBackColor);
        }

        public bool ShouldSerializeUserMarginForeColor()
        {
            return (this.userMarginForeColor != EditConsts.DefaultUserMarginForeColor);
        }

        public bool ShouldSerializeUserMarginText()
        {
            return (this.userMarginText != EditConsts.DefaultUserMarginText);
        }

        public bool ShouldSerializeUserMarginWidth()
        {
            return (this.userMarginWidth != EditConsts.DefaultUserMarginWidth);
        }

        public bool ShouldSerializeWidth()
        {
            return (this.width != EditConsts.DefaultGutterWidth);
        }

        public bool ShouldSerializeWrapImageIndex()
        {
            return (this.wrapImageIndex != EditConsts.DefaultWrapImageIndex);
        }

        public void Update()
        {
            this.Update(true);
        }

        public void Update(bool needChange)
        {
            if (((this.updateCount == 0) && (this.owner != null)) && needChange)
            {
                this.owner.UpdateCaret();
                this.owner.Invalidate();
            }
        }

        private bool UpdateLineNumberLength()
        {
            bool flag = false;
            if (this.owner != null)
            {
                int num = Math.Max((Math.Max(this.owner.DisplayLines.DisplayCount, this.owner.Lines.Count) + this.lineNumbersStart) - 1, 0);
                if ((this.options & GutterOptions.PaintLinesBeyondEof) != GutterOptions.None)
                {
                    int y;
                    if (this.owner.Pages.PageType == PageType.PageLayout)
                    {
                        IEditPage page = (this.owner.Pages.Count > 0) ? this.owner.Pages[this.owner.Pages.Count - 1] : this.owner.Pages.DefaultPage;
                        y = page.EndLine;
                    }
                    else
                    {
                        y = this.owner.Scrolling.WindowOriginY;
                        int fontHeight = this.owner.Painter.FontHeight;
                        if (fontHeight != 0)
                        {
                            int clientHeight = this.owner.ClientHeight;
                            y += (clientHeight / fontHeight) + (((clientHeight % fontHeight) != 0) ? 1 : 0);
                        }
                    }
                    y = this.owner.DisplayLines.DisplayPointToPoint(0, y).Y;
                    num = Math.Max(num, y);
                }
                int num5 = Math.Max(num.ToString().Length, this.maxLineNumberLength);
                flag = this.lineNumberLength != num5;
                this.lineNumberLength = num5;
                if (flag)
                {
                    this.owner.Painter.FontStyle = this.owner.Font.Style;
                    this.lineNumberWidth = ((this.lineNumberLength * this.owner.Painter.StringWidth("9")) + this.lineNumbersLeftIndent) + this.lineNumbersRightIndent;
                }
            }
            return flag;
        }

        [Description("Gets or sets a value that specifies index of item in the image collection used to paint bookmark.")]
        public virtual int BookMarkImageIndex
        {
            get
            {
                return this.bookMarkImageIndex;
            }
            set
            {
                if (this.bookMarkImageIndex != value)
                {
                    this.bookMarkImageIndex = value;
                    this.OnBookMarkImageIndexChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual System.Drawing.Brush Brush
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

        [Description("Gets or sets background color of the gutter.")]
        public virtual Color BrushColor
        {
            get
            {
                if (this.brush is SolidBrush)
                {
                    return ((SolidBrush) this.brush).Color;
                }
                return EditConsts.DefaultGutterBackColor;
            }
            set
            {
                if ((this.brush is SolidBrush) && (((SolidBrush) this.brush).Color != value))
                {
                    ((SolidBrush) this.brush).Color = value;
                    this.OnBrushColorChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int DisplayArea
        {
            get
            {
                return this.GetDisplayWidth(true);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int DisplayWidth
        {
            get
            {
                return this.GetDisplayWidth(false);
            }
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether Edit control should draw triangle at bookmark position inside line.")]
        public virtual bool DrawLineBookmarks
        {
            get
            {
                return this.drawLineBookmarks;
            }
            set
            {
                if (this.drawLineBookmarks != value)
                {
                    this.drawLineBookmarks = value;
                    this.OnDrawLineBookmarksChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ImageList Images
        {
            get
            {
                if (this.images == null)
                {
                    return this.internalImages;
                }
                return this.images;
            }
            set
            {
                if (this.images != value)
                {
                    this.images = value;
                    this.OnImagesChanged();
                }
            }
        }

        [Description("Gets or sets a color of the line bookmarks.")]
        public virtual Color LineBookmarksColor
        {
            get
            {
                return this.lineBookmarksColor;
            }
            set
            {
                if (this.lineBookmarksColor != value)
                {
                    this.lineBookmarksColor = value;
                    this.OnLineBookmarksColorChanged();
                }
            }
        }

        [Description("Gets or sets a color of the line modificators(color stitch that indicates that the line content is unmodified, modified or saved) in the modified state.")]
        public virtual Color LineModificatorChangedColor
        {
            get
            {
                return this.lineModificatorChangedColor;
            }
            set
            {
                if (this.lineModificatorChangedColor != value)
                {
                    this.lineModificatorChangedColor = value;
                    this.OnLineModificatorChangedColorChanged();
                }
            }
        }

        [Description("Gets or sets a color of the line modificators(color stitch that indicates that the line content is unmodified, modified or saved) in the saved state.")]
        public virtual Color LineModificatorSavedColor
        {
            get
            {
                return this.lineModificatorSavedColor;
            }
            set
            {
                if (this.lineModificatorSavedColor != value)
                {
                    this.lineModificatorSavedColor = value;
                    this.OnLineModificatorSavedColorChanged();
                }
            }
        }

        [DefaultValue(0), Description("Gets or sets line numbers alignment information.")]
        public virtual StringAlignment LineNumbersAlignment
        {
            get
            {
                return this.lineNumbersAlignment;
            }
            set
            {
                if (this.lineNumbersAlignment != value)
                {
                    this.lineNumbersAlignment = value;
                    this.OnLineNumbersAlignmentChanged();
                }
            }
        }

        [Description("Gets or sets background color for the line numbers.")]
        public virtual Color LineNumbersBackColor
        {
            get
            {
                return this.lineNumbersBackColor;
            }
            set
            {
                if (this.lineNumbersBackColor != value)
                {
                    this.lineNumbersBackColor = value;
                    this.OnLineNumbersBackColorChanged();
                }
            }
        }

        [Description("Gets or sets foreground color for the line numbers.")]
        public virtual Color LineNumbersForeColor
        {
            get
            {
                return this.lineNumbersForeColor;
            }
            set
            {
                if (this.lineNumbersForeColor != value)
                {
                    this.lineNumbersForeColor = value;
                    this.OnLineNumbersForeColorChanged();
                }
            }
        }

        [Description("Gets or sets line numbers indentation from the left gutter border.")]
        public virtual int LineNumbersLeftIndent
        {
            get
            {
                return this.lineNumbersLeftIndent;
            }
            set
            {
                if (this.lineNumbersLeftIndent != value)
                {
                    this.lineNumbersLeftIndent = value;
                    this.OnLineNumbersLeftIndentChanged();
                }
            }
        }

        [Description("Gets or sets line numbers indentation from the right gutter border.")]
        public virtual int LineNumbersRightIndent
        {
            get
            {
                return this.lineNumbersRightIndent;
            }
            set
            {
                if (this.lineNumbersRightIndent != value)
                {
                    this.lineNumbersRightIndent = value;
                    this.OnLineNumbersRightIndentChanged();
                }
            }
        }

        [Description("Gets or sets number of the first line being painted on the gutter.")]
        public virtual int LineNumbersStart
        {
            get
            {
                return this.lineNumbersStart;
            }
            set
            {
                if (this.lineNumbersStart != value)
                {
                    this.lineNumbersStart = value;
                    this.OnLineNumbersStartChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int MaxLineNumberLength
        {
            get
            {
                return this.maxLineNumberLength;
            }
            set
            {
                if (this.maxLineNumberLength != value)
                {
                    this.maxLineNumberLength = value;
                    this.OnMaxLineNumberLengthChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets a \"GutterOptions\" that determine gutter appearance and behaviour.")]
        public virtual GutterOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.OnOptionsChanged();
                }
            }
        }

        [Description("Gets or sets outlining indentation from the left gutter border.")]
        public virtual int OutliningLeftIndent
        {
            get
            {
                return this.outliningLeftIndent;
            }
            set
            {
                if (this.outliningLeftIndent != value)
                {
                    this.outliningLeftIndent = value;
                    this.OnOutliningLeftIndentChanged();
                }
            }
        }

        [Description("Gets or sets outlining indentation from the right gutter border.")]
        public virtual int OutliningRightIndent
        {
            get
            {
                return this.outliningRightIndent;
            }
            set
            {
                if (this.outliningRightIndent != value)
                {
                    this.outliningRightIndent = value;
                    this.OnOutliningRightIndentChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual System.Drawing.Pen Pen
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

        [Description("Gets or sets color of the gutter line.")]
        public virtual Color PenColor
        {
            get
            {
                return this.pen.Color;
            }
            set
            {
                if (this.pen.Color != value)
                {
                    this.pen.Color = value;
                    this.OnPenColorChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle Rect
        {
            get
            {
                Rectangle rectangle = (this.owner != null) ? this.owner.ClientArea : new Rectangle(0, 0, 0, 0);
                rectangle.Width = this.DisplayWidth;
                return rectangle;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlGutterInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets or sets a value indicating whether Edit control should display text describing bookmark in form of tooltip window when mouse pointer is over the gutter bookmark."), DefaultValue(true)]
        public virtual bool ShowBookmarkHints
        {
            get
            {
                return this.showBookmarkHints;
            }
            set
            {
                if (this.showBookmarkHints != value)
                {
                    this.showBookmarkHints = value;
                    this.OnShowBookmarkHintsChanged();
                }
            }
        }

        protected bool Transparent
        {
            get
            {
                return ((this.owner != null) && this.owner.Transparent);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [Description("Gets or sets background color of the user margin.")]
        public virtual Color UserMarginBackColor
        {
            get
            {
                return this.userMarginBackColor;
            }
            set
            {
                if (this.userMarginBackColor != value)
                {
                    this.userMarginBackColor = value;
                    this.OnUserMarginBackColorChanged();
                }
            }
        }

        [Description("Gets or sets foreground color of the user margin.")]
        public virtual Color UserMarginForeColor
        {
            get
            {
                return this.userMarginForeColor;
            }
            set
            {
                if (this.userMarginForeColor != value)
                {
                    this.userMarginForeColor = value;
                    this.OnUserMarginForeColorChanged();
                }
            }
        }

        protected Regex UserMarginRegex
        {
            get
            {
                if (this.userMarginRegex == null)
                {
                    this.userMarginRegex = new Regex(@"\\\[[a-zA-Z_0-9]+\]", RegexOptions.Singleline);
                }
                return this.userMarginRegex;
            }
        }

        [Description("Gets or sets text of the user margin.")]
        public virtual string UserMarginText
        {
            get
            {
                return this.userMarginText;
            }
            set
            {
                if (this.userMarginText != value)
                {
                    this.userMarginText = value;
                    this.OnUserMarginTextChanged();
                }
            }
        }

        public virtual int UserMarginWidth
        {
            get
            {
                return this.userMarginWidth;
            }
            set
            {
                if (this.userMarginWidth != value)
                {
                    this.userMarginWidth = value;
                    this.OnUserMarginWidthChanged();
                }
            }
        }

        [DefaultValue(true), Description("Gets or sets a value indicating whether the gutter area is visible.")]
        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnVisibleChanged();
                }
            }
        }

        [Description("Gets or sets the width of the gutter.")]
        public virtual int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                if (this.width != value)
                {
                    this.width = value;
                    this.OnWidthChanged();
                }
            }
        }

        [Description("Gets or sets a value that specifies index of item in the image collection used to paint special mark indicating the wrapped line.")]
        public virtual int WrapImageIndex
        {
            get
            {
                return this.wrapImageIndex;
            }
            set
            {
                if (this.wrapImageIndex != value)
                {
                    this.wrapImageIndex = value;
                    this.OnWrapImageIndexChanged();
                }
            }
        }
    }
}

