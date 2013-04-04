namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Design;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Scrolling : IScrolling, IUpdate
    {
        private int defaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;
        private bool flatBars;
        private IScrollingButtons horzButtons;
        private IScrollingButton horzSplitButton;
        private ScrollBar hScrollBar;
        private ScrollingOptions options = EditConsts.DefaultScrollingOptions;
        private ISyntaxEdit owner;
        private RichTextBoxScrollBars scrollBars = RichTextBoxScrollBars.Both;
        private int scrollUpdateCount;
        private Timer smoothTimer;
        private int updateCount;
        private IScrollingButtons vertButtons;
        private IScrollingButton vertSplitButton;
        private ScrollBar vScrollBar;
        private int windowOriginX;
        private int windowOriginY;

        [Browsable(false)]
        public event EventHandler HorizontalScroll;

        [Browsable(false)]
        public event EventHandler ScrollButtonClick;

        [Browsable(false)]
        public event EventHandler VerticalScroll;

        public Scrolling(ISyntaxEdit owner)
        {
            this.owner = owner;
            this.horzButtons = new ScrollingButtons(this, owner);
            this.vertButtons = new ScrollingButtons(this, owner);
        }

        public virtual void Assign(IScrolling source)
        {
            this.BeginUpdate();
            try
            {
                this.ScrollBars = source.ScrollBars;
                this.DefaultHorzScrollSize = source.DefaultHorzScrollSize;
                this.WindowOriginX = source.WindowOriginX;
                this.WindowOriginY = source.WindowOriginY;
                this.Options = source.Options;
                this.horzButtons.Assign(source.HorzButtons);
                this.vertButtons.Assign(source.VertButtons);
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

        protected bool CanSplitHorz()
        {
            return (((this.options & ScrollingOptions.AllowSplitHorz) != ScrollingOptions.None) && this.owner.CanSplitHorz());
        }

        protected bool CanSplitVert()
        {
            return (((this.options & ScrollingOptions.AllowSplitVert) != ScrollingOptions.None) && this.owner.CanSplitVert());
        }

        protected virtual IScrollingButton CreateSplitterButton(bool vert)
        {
            IScrollingButton button = new ScrollingSplitter {
                Scrolling = this,
                Visible = false
            };
            SpeedButton button2 = button.Button;
            if (vert)
            {
                button2.Cursor = Cursors.VSplit;
                button2.Width = EditConsts.DefaltScrollSplitterSize;
            }
            else
            {
                button2.Cursor = Cursors.HSplit;
                button2.Height = EditConsts.DefaltScrollSplitterSize;
            }
            button2.Parent = (Control) this.owner;
            button2.BringToFront();
            button2.Dock = DockStyle.None;
            button2.MouseDown += new MouseEventHandler(this.SplitterMouseDown);
            return button;
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        protected void DoHorizontalScroll(int pos)
        {
            this.DoHorizontalScroll(ScrollEventType.ThumbPosition, pos);
        }

        protected void DoHorizontalScroll(ScrollEventType code, int pos)
        {
            if ((this.scrollUpdateCount <= 0) && (this.owner != null))
            {
                switch (code)
                {
                    case ScrollEventType.SmallDecrement:
                        this.WindowOriginX -= this.ScrollByPixels ? this.owner.Painter.FontWidth : 1;
                        return;

                    case ScrollEventType.SmallIncrement:
                        this.WindowOriginX += this.ScrollByPixels ? this.owner.Painter.FontWidth : 1;
                        return;

                    case ScrollEventType.LargeDecrement:
                        this.WindowOriginX -= this.ScrollByPixels ? this.owner.ClientRect.Width : this.owner.CharsInWidth;
                        return;

                    case ScrollEventType.LargeIncrement:
                        this.WindowOriginX += this.ScrollByPixels ? this.owner.ClientRect.Width : this.owner.CharsInWidth;
                        return;

                    case ScrollEventType.ThumbPosition:
                        this.WindowOriginX = pos;
                        return;

                    case ScrollEventType.ThumbTrack:
                        if ((this.options & ScrollingOptions.SmoothScroll) != ScrollingOptions.None)
                        {
                            this.WindowOriginX = pos;
                        }
                        return;

                    case ScrollEventType.First:
                        this.WindowOriginX = 0;
                        return;

                    case ScrollEventType.Last:
                        this.WindowOriginX = this.ScrollWidth();
                        return;
                }
            }
        }

        protected void DoVerticalScroll(int pos)
        {
            this.DoVerticalScroll(ScrollEventType.ThumbPosition, pos);
        }

        protected void DoVerticalScroll(ScrollEventType code, int pos)
        {
            if ((this.scrollUpdateCount <= 0) && (this.owner != null))
            {
                switch (code)
                {
                    case ScrollEventType.SmallDecrement:
                        this.WindowOriginY -= this.ScrollByPixels ? this.owner.Painter.FontHeight : 1;
                        return;

                    case ScrollEventType.SmallIncrement:
                        this.WindowOriginY += this.ScrollByPixels ? this.owner.Painter.FontHeight : 1;
                        return;

                    case ScrollEventType.LargeDecrement:
                        this.WindowOriginY -= this.ScrollByPixels ? this.owner.ClientRect.Height : this.owner.LinesInHeight;
                        return;

                    case ScrollEventType.LargeIncrement:
                        this.WindowOriginY += this.ScrollByPixels ? this.owner.ClientRect.Height : this.owner.LinesInHeight;
                        return;

                    case ScrollEventType.ThumbPosition:
                        this.WindowOriginY = pos;
                        return;

                    case ScrollEventType.ThumbTrack:
                        if ((this.options & ScrollingOptions.SmoothScroll) != ScrollingOptions.None)
                        {
                            this.WindowOriginY = pos;
                        }
                        if ((this.options & ScrollingOptions.ShowScrollHint) != ScrollingOptions.None)
                        {
                            if ((this.options & ScrollingOptions.SmoothScroll) != ScrollingOptions.None)
                            {
                                pos = this.WindowOriginY;
                            }
                            else
                            {
                                pos = this.VScrollBar.Value;
                            }
                            this.owner.ShowScrollHint(pos);
                        }
                        return;

                    case ScrollEventType.First:
                        this.WindowOriginY = 0;
                        return;

                    case ScrollEventType.Last:
                        this.WindowOriginY = this.ScrollHeight();
                        return;

                    case ScrollEventType.EndScroll:
                        if (this.owner.WordWrap)
                        {
                            this.UpdateScroll();
                        }
                        if ((this.options & ScrollingOptions.ShowScrollHint) != ScrollingOptions.None)
                        {
                            this.owner.HideScrollHint();
                        }
                        return;
                }
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

        ~Scrolling()
        {
            this.horzButtons.Clear();
            this.vertButtons.Clear();
            if (this.smoothTimer != null)
            {
                this.smoothTimer.Dispose();
            }
        }

        private void GetScrollCodes(out bool vert, out bool horz, out bool forced)
        {
            vert = false;
            horz = false;
            forced = false;
            switch (this.scrollBars)
            {
                case RichTextBoxScrollBars.Horizontal:
                    horz = true;
                    return;

                case RichTextBoxScrollBars.Vertical:
                    vert = true;
                    return;

                case RichTextBoxScrollBars.Both:
                    vert = true;
                    horz = true;
                    return;

                case RichTextBoxScrollBars.ForcedHorizontal:
                    horz = true;
                    forced = true;
                    return;

                case RichTextBoxScrollBars.ForcedVertical:
                    vert = true;
                    forced = true;
                    return;

                case RichTextBoxScrollBars.ForcedBoth:
                    horz = true;
                    vert = true;
                    forced = true;
                    return;
            }
        }

        protected void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.DoHorizontalScroll(e.Type, e.NewValue);
        }

        private bool InvalidateLines(int ch, int line)
        {
            bool flag = false;
            if (this.owner.IsHandleCreated && ((line != this.WindowOriginY) || (ch != this.WindowOriginX)))
            {
                if ((((ch != this.WindowOriginX) && (line != this.WindowOriginY)) || ((this.owner.Selection.SelectionType != SelectionType.None) || this.owner.Transparent)) || (this.owner.Pages.Transparent || this.owner.LineSeparator.NeedHighlight()))
                {
                    this.owner.Invalidate();
                    return flag;
                }
                int num = 0;
                int num2 = 0;
                Rectangle clientRect = this.owner.ClientRect;
                int width = clientRect.Width;
                int height = clientRect.Height;
                if (ch == this.WindowOriginX)
                {
                    if (this.ScrollByPixels)
                    {
                        num2 = line - this.WindowOriginY;
                    }
                    else
                    {
                        num2 = (line - this.WindowOriginY) * this.owner.Painter.FontHeight;
                    }
                }
                else if (line == this.WindowOriginY)
                {
                    if (this.ScrollByPixels)
                    {
                        num = ch - this.WindowOriginX;
                    }
                    else
                    {
                        num = (ch - this.WindowOriginX) * this.owner.Painter.FontWidth;
                    }
                }
                if ((Math.Abs(num) < (width / 2)) && (Math.Abs(num2) < (height / 2)))
                {
                    Rectangle rectangle2;
                    Rectangle rectangle3;
                    int x = clientRect.Left + ((this.owner.Pages.PageType == PageType.PageLayout) ? 0 : this.owner.Gutter.DisplayWidth);
                    if (num2 == 0)
                    {
                        if (num > 0)
                        {
                            rectangle2 = new Rectangle(x, clientRect.Top, width - num, height);
                            rectangle3 = new Rectangle(x, clientRect.Top, num, height);
                        }
                        else
                        {
                            rectangle2 = new Rectangle(x - num, clientRect.Top, width + num, height);
                            rectangle3 = new Rectangle(width + num, clientRect.Top, -num, height);
                        }
                    }
                    else if (num2 > 0)
                    {
                        rectangle2 = new Rectangle(clientRect.Left, clientRect.Top, width, height - num2);
                        rectangle3 = new Rectangle(clientRect.Left, clientRect.Top, width, num2 - clientRect.Top);
                    }
                    else
                    {
                        rectangle2 = new Rectangle(clientRect.Left, clientRect.Top - num2, width, height + num2);
                        rectangle3 = new Rectangle(clientRect.Left, height + num2, width, -num2);
                    }
                    OSUtils.ScrollWindow(this.owner.Handle, num, num2, rectangle2);
                    this.owner.Invalidate(rectangle3);
                    return true;
                }
                this.owner.Invalidate();
            }
            return flag;
        }

        public virtual void MouseScroll(int delta)
        {
            delta = -delta / 120;
            if (delta != 0)
            {
                if (this.ScrollByPixels)
                {
                    this.WindowOriginY += (delta * SystemInformation.MouseWheelScrollLines) * this.owner.Painter.FontHeight;
                }
                else
                {
                    this.WindowOriginY += delta * SystemInformation.MouseWheelScrollLines;
                }
            }
        }

        protected bool NeedSmoothScroll()
        {
            if ((this.options & ScrollingOptions.ScrollByPixels) != ScrollingOptions.None)
            {
                if (!this.SmoothTimer.Enabled)
                {
                    this.SmoothTimer.Enabled = true;
                    return true;
                }
                this.SmoothTimer.Enabled = false;
                this.SmoothTimer.Enabled = true;
            }
            return false;
        }

        protected virtual void OnDefaultHorzScrollSizeChanged()
        {
            this.UpdateScroll(false);
        }

        protected void OnHorizontalScroll()
        {
            if (this.HorizontalScroll != null)
            {
                this.HorizontalScroll(this.owner, EventArgs.Empty);
            }
        }

        protected virtual void OnScrollBarsChanged()
        {
            this.UpdateScroll(true);
        }

        public virtual void OnScrollButtonClick(object sender, EventArgs e)
        {
            if (this.ScrollButtonClick != null)
            {
                foreach (IScrollingButton button in this.vertButtons)
                {
                    if (sender == button.Button)
                    {
                        this.ScrollButtonClick(button, e);
                        return;
                    }
                }
                foreach (ScrollingButton button2 in this.horzButtons)
                {
                    if (sender == button2.Button)
                    {
                        this.ScrollButtonClick(button2, e);
                        break;
                    }
                }
            }
        }

        protected void OnSmoothTimer(object sender, EventArgs e)
        {
            this.smoothTimer.Enabled = false;
        }

        protected void OnVerticalScroll()
        {
            if (this.VerticalScroll != null)
            {
                this.VerticalScroll(this.owner, EventArgs.Empty);
            }
        }

        protected void RemoveScrollbars(bool system)
        {
            if (system)
            {
                if (this.owner.IsHandleCreated)
                {
                    OSUtils.SetScrollBar(this.owner.Handle, this.flatBars, 0, -1, false);
                    OSUtils.SetScrollBar(this.owner.Handle, this.flatBars, 0, -1, true);
                }
            }
            else
            {
                if (this.vScrollBar != null)
                {
                    this.vScrollBar.Dispose();
                    this.vScrollBar = null;
                }
                if (this.hScrollBar != null)
                {
                    this.hScrollBar.Dispose();
                    this.hScrollBar = null;
                }
                Rectangle empty = Rectangle.Empty;
                this.UpdateScrollButtons(this.vertButtons, true, ref empty);
                this.UpdateScrollButtons(this.horzButtons, false, ref empty);
            }
        }

        protected void RepositionScrollBars()
        {
            Rectangle clientRectangle = this.owner.ClientRectangle;
            if (this.HasVScrollBar)
            {
                clientRectangle.Width -= this.VScrollBar.Width;
            }
            if (this.HasHScrollBar)
            {
                clientRectangle.Height -= this.HScrollBar.Height;
            }
            Rectangle rectangle2 = clientRectangle;
            this.UpdateScrollButtons(this.vertButtons, true, ref clientRectangle);
            if (this.HasVScrollBar)
            {
                this.VScrollBar.Bounds = new Rectangle(rectangle2.Right, clientRectangle.Top, this.VScrollBar.Width, clientRectangle.Height);
            }
            clientRectangle = rectangle2;
            this.UpdateScrollButtons(this.horzButtons, false, ref clientRectangle);
            if (this.HasHScrollBar)
            {
                this.HScrollBar.Bounds = new Rectangle(clientRectangle.Left, rectangle2.Bottom, clientRectangle.Width, this.HScrollBar.Height);
            }
        }

        public virtual void ResetDefaultHorzScrollSize()
        {
            this.DefaultHorzScrollSize = EditConsts.DefaultHorzScrollSize;
        }

        public virtual void ResetOptions()
        {
            this.Options = EditConsts.DefaultScrollingOptions;
        }

        public virtual void ResetScrollBars()
        {
            this.ScrollBars = RichTextBoxScrollBars.Both;
        }

        private int ScrollHeight()
        {
            if (this.owner == null)
            {
                return 0;
            }
            if (this.owner.Pages.PageType == PageType.PageLayout)
            {
                return this.owner.Pages.Height;
            }
            if (this.ScrollByPixels)
            {
                return (this.owner.DisplayLines.DisplayCount * this.owner.Painter.FontHeight);
            }
            return this.owner.DisplayLines.DisplayCount;
        }

        private int ScrollWidth()
        {
            if (this.owner == null)
            {
                return 0;
            }
            switch (this.owner.Pages.PageType)
            {
                case PageType.PageBreaks:
                    return this.owner.GetCharsInWidth(this.owner.Pages.Width);

                case PageType.PageLayout:
                    return this.owner.Pages.Width;
            }
            bool flag = (this.scrollBars == RichTextBoxScrollBars.ForcedBoth) || (this.scrollBars == RichTextBoxScrollBars.ForcedHorizontal);
            if (flag && (this.defaultHorzScrollSize > 0))
            {
                if (!this.ScrollByPixels)
                {
                    return this.defaultHorzScrollSize;
                }
                return (this.defaultHorzScrollSize * this.owner.Painter.FontWidth);
            }
            if (this.owner.WordWrap)
            {
                if (this.owner.WrapAtMargin)
                {
                    if (!this.ScrollByPixels)
                    {
                        return this.owner.EditMargin.Position;
                    }
                    return (this.owner.EditMargin.Position * this.owner.Painter.FontWidth);
                }
                if (!this.ScrollByPixels)
                {
                    return this.owner.CharsInWidth;
                }
                return this.owner.ClientWidth;
            }
            int maxLineWidth = this.owner.DisplayLines.MaxLineWidth;
            if (flag)
            {
                maxLineWidth = Math.Max((int) (this.owner.ClientWidth * (1f + (1f / ((float) EditConsts.DefaultHorzScrollDelta)))), maxLineWidth);
            }
            if (this.ScrollByPixels)
            {
                return maxLineWidth;
            }
            if (this.owner.Painter.FontWidth == 0)
            {
                return this.defaultHorzScrollSize;
            }
            int num2 = maxLineWidth / this.owner.Painter.FontWidth;
            if ((maxLineWidth % this.owner.Painter.FontWidth) != 0)
            {
                num2++;
            }
            return num2;
        }

        private void SetScrollBar(int size, int pageSize, bool vert)
        {
            if ((this.options & ScrollingOptions.SystemScrollbars) != ScrollingOptions.None)
            {
                OSUtils.SetScrollBar(this.owner.Handle, this.flatBars, Math.Max(size - 1, 0), pageSize, vert);
            }
            else
            {
                ScrollBar bar = vert ? this.vScrollBar : this.hScrollBar;
                if (pageSize < 0)
                {
                    if (bar != null)
                    {
                        bar.Visible = false;
                        if (vert)
                        {
                            bar.Height = 0;
                        }
                        else
                        {
                            bar.Width = 0;
                        }
                    }
                }
                else
                {
                    bar = vert ? this.VScrollBar : this.HScrollBar;
                    bar.Visible = true;
                    bar.Maximum = size;
                    bar.SmallChange = 1;
                    bar.LargeChange = pageSize;
                    bar.Enabled = size >= pageSize;
                }
            }
        }

        private void SetScrollPos(bool vert, int value)
        {
            if ((this.options & ScrollingOptions.SystemScrollbars) != ScrollingOptions.None)
            {
                OSUtils.SetScrollPos(this.owner.Handle, this.flatBars, value, vert);
            }
            else
            {
                ScrollBar bar = vert ? this.vScrollBar : this.hScrollBar;
                if (bar != null)
                {
                    bar.Value = Math.Min(value, bar.Maximum);
                }
            }
        }

        public bool ShouldSerializeDefaultHorzScrollSize()
        {
            return (this.defaultHorzScrollSize != EditConsts.DefaultHorzScrollSize);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.options != EditConsts.DefaultScrollingOptions);
        }

        protected void SmoothScroll(int start, int end)
        {
            this.windowOriginY = start;
            int defaultSmoothScrollSteps = EditConsts.DefaultSmoothScrollSteps;
            float num2 = (end - start) / defaultSmoothScrollSteps;
            int line = 0;
            if (num2 != 0f)
            {
                for (int i = 0; i < defaultSmoothScrollSteps; i++)
                {
                    line = this.windowOriginY;
                    this.windowOriginY = start + ((int) (i * num2));
                    this.WindowOriginChanged(line, this.windowOriginX, true);
                    OSUtils.Sleep(EditConsts.DefaultSmoothScrollDelay);
                }
            }
            line = this.windowOriginY;
            this.windowOriginY = end;
            this.WindowOriginChanged(line, this.windowOriginX, false);
        }

        protected void SplitterMouseDown(object sender, MouseEventArgs e)
        {
            Splitter vertSplitter = null;
            if ((this.vertSplitButton != null) && (sender == ((ScrollingButton) this.vertSplitButton).Button))
            {
                this.owner.SplitViewVert();
                vertSplitter = this.owner.VertSplitter;
            }
            else if ((this.horzSplitButton != null) && (sender == ((ScrollingButton) this.horzSplitButton).Button))
            {
                this.owner.SplitViewHorz();
                vertSplitter = this.owner.HorzSplitter;
            }
            if ((vertSplitter != null) && vertSplitter.IsHandleCreated)
            {
                if (this.owner.IsHandleCreated)
                {
                    this.owner.Update();
                    if (this.vScrollBar != null)
                    {
                        this.vScrollBar.Update();
                    }
                    if (this.hScrollBar != null)
                    {
                        this.hScrollBar.Update();
                    }
                }
                OSUtils.SendMessage(vertSplitter.Handle, 0x201, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public virtual void SystemScroll(int code, bool vert)
        {
            if (vert)
            {
                this.DoVerticalScroll(OSUtils.GetScrollType(code), OSUtils.GetScrollPos(this.owner.Handle, this.flatBars, true));
            }
            else
            {
                this.DoHorizontalScroll(OSUtils.GetScrollType(code), OSUtils.GetScrollPos(this.owner.Handle, this.flatBars, false));
            }
        }

        public virtual void Update()
        {
            this.UpdateScroll(true);
        }

        public virtual void UpdateFlat()
        {
            if (this.owner.IsHandleCreated)
            {
                if ((this.Options & ScrollingOptions.FlatScrollbars) != ScrollingOptions.None)
                {
                    this.flatBars = OSUtils.InitializeFlatSB(this.owner.Handle);
                }
                else if (this.flatBars)
                {
                    OSUtils.UninitializeFlatSB(this.owner.Handle);
                    this.flatBars = false;
                }
            }
        }

        public virtual void UpdateScroll()
        {
            this.UpdateScroll(false);
        }

        public virtual void UpdateScroll(bool updateSize)
        {
            if (this.updateCount <= 0)
            {
                this.WindowOriginY = this.windowOriginY;
                this.WindowOriginX = this.windowOriginX;
                this.UpdateScrollSize(updateSize);
                this.UpdateScrollPosition();
                if (this.owner != null)
                {
                    this.owner.OnStateChanged(this, NotifyState.ScrollingOptionsChanged);
                }
            }
        }

        protected virtual void UpdateScrollButtons(IScrollingButtons buttons, bool vert, ref Rectangle r)
        {
            this.UpdateScrollSplitters(vert, ref r);
            ScrollBar bar = vert ? this.vScrollBar : this.hScrollBar;
            foreach (IScrollingButton button in buttons)
            {
                SpeedButton button2 = button.Button;
                bool flag = ((button.Visible && (bar != null)) && bar.Visible) && ((vert && ((this.options & ScrollingOptions.VertButtons) != ScrollingOptions.None)) || (!vert && ((this.options & ScrollingOptions.HorzButtons) != ScrollingOptions.None)));
                if (flag)
                {
                    if (vert)
                    {
                        button2.Width = bar.Width;
                        button2.Height = bar.Width;
                        button2.Location = new Point(r.Right, r.Bottom - button2.Height);
                        r.Height -= button2.Height;
                    }
                    else
                    {
                        button2.Height = bar.Height;
                        button2.Width = bar.Height;
                        button2.Location = new Point(r.X, r.Bottom);
                        r.X += button2.Width;
                        r.Width -= button2.Width;
                    }
                }
                button2.Visible = flag;
            }
        }

        protected void UpdateScrollPosition()
        {
            if ((this.owner != null) && this.owner.IsHandleCreated)
            {
                this.scrollUpdateCount++;
                try
                {
                    bool flag;
                    bool flag2;
                    bool flag3;
                    this.GetScrollCodes(out flag, out flag2, out flag3);
                    if (flag)
                    {
                        this.SetScrollPos(true, this.windowOriginY);
                    }
                    if (flag2)
                    {
                        this.SetScrollPos(false, this.windowOriginX);
                    }
                }
                finally
                {
                    this.scrollUpdateCount--;
                }
            }
        }

        protected void UpdateScrollSize(bool updateSize)
        {
            if ((this.owner != null) && this.owner.IsHandleCreated)
            {
                bool flag;
                bool flag2;
                bool flag3;
                int size = 0;
                int pageSize = 0;
                this.GetScrollCodes(out flag, out flag2, out flag3);
                bool hasVScrollBar = this.HasVScrollBar;
                bool hasHScrollBar = this.HasHScrollBar;
                if (flag)
                {
                    size = this.ScrollHeight();
                    pageSize = this.ScrollByPixels ? this.owner.ClientRect.Height : this.owner.LinesInHeight;
                    if (!flag3 && (size <= pageSize))
                    {
                        pageSize = -1;
                    }
                    this.SetScrollBar(size, pageSize, true);
                }
                else
                {
                    this.SetScrollBar(0, -1, true);
                }
                if (flag2)
                {
                    size = this.ScrollWidth();
                    pageSize = (this.owner.Pages.PageType == PageType.PageLayout) ? this.owner.ClientRect.Width : this.owner.CharsInWidth;
                    if (!flag3 && (size <= pageSize))
                    {
                        pageSize = -1;
                    }
                    this.SetScrollBar(size, pageSize, false);
                }
                else
                {
                    this.SetScrollBar(0, -1, false);
                }
                if (((this.options & ScrollingOptions.SystemScrollbars) == ScrollingOptions.None) && ((updateSize || (hasVScrollBar != this.HasVScrollBar)) || (hasHScrollBar != this.HasHScrollBar)))
                {
                    this.RepositionScrollBars();
                }
            }
        }

        protected virtual void UpdateScrollSplitters(bool vert, ref Rectangle r)
        {
            IScrollingButton vertSplitButton;
            ScrollBar bar = vert ? this.vScrollBar : this.hScrollBar;
            vert = !vert;
            if ((((bar == null) || !bar.Visible) || (vert && !this.CanSplitVert())) || (!vert && !this.CanSplitHorz()))
            {
                vertSplitButton = vert ? this.vertSplitButton : this.horzSplitButton;
                if (vertSplitButton != null)
                {
                    vertSplitButton.Visible = false;
                }
            }
            else
            {
                if (vert && this.CanSplitVert())
                {
                    vertSplitButton = this.VertSplitButton;
                    if (vertSplitButton != null)
                    {
                        SpeedButton button = ((ScrollingButton) vertSplitButton).Button;
                        button.Height = bar.Height;
                        button.Location = new Point(0, r.Bottom);
                        r.X += button.Width;
                        r.Width -= button.Width;
                        button.Visible = true;
                    }
                }
                if (!vert && this.CanSplitHorz())
                {
                    vertSplitButton = this.HorzSplitButton;
                    if (vertSplitButton != null)
                    {
                        SpeedButton button3 = vertSplitButton.Button;
                        button3.Width = bar.Width;
                        button3.Location = new Point(r.Right, 0);
                        r.Y += button3.Height;
                        r.Height -= button3.Height;
                        button3.Visible = true;
                    }
                }
            }
        }

        protected void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.DoVerticalScroll(e.Type, e.NewValue);
        }

        protected virtual void WindowOriginChanged(int line, int ch, bool invalidateOnly)
        {
            bool flag = this.InvalidateLines(ch, line);
            if (invalidateOnly)
            {
                if (flag)
                {
                    this.owner.Update();
                }
            }
            else
            {
                this.owner.UpdateCaret();
                if (line != this.WindowOriginY)
                {
                    if ((this.owner.Pages.Rulers & EditRulers.Vertical) != EditRulers.None)
                    {
                        this.owner.Pages.DisplayRulers();
                    }
                    if (this.owner.Gutter.InvalidateLineNumberArea())
                    {
                        this.owner.Invalidate();
                        flag = false;
                    }
                }
                if (ch != this.WindowOriginX)
                {
                    if ((this.owner.Pages.Rulers & EditRulers.Horizonal) != EditRulers.None)
                    {
                        this.owner.Pages.DisplayRulers();
                    }
                    this.owner.OnStateChanged(this, NotifyState.ScrollingOriginChanged);
                }
                if (flag)
                {
                    this.owner.Update();
                }
            }
        }

        [Description("Gets or sets a default size of horizontal scrollbar.")]
        public virtual int DefaultHorzScrollSize
        {
            get
            {
                return this.defaultHorzScrollSize;
            }
            set
            {
                if (this.defaultHorzScrollSize != value)
                {
                    this.defaultHorzScrollSize = value;
                    this.OnDefaultHorzScrollSizeChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool FixedScrollSize
        {
            get
            {
                if (this.owner.WordWrap)
                {
                    return true;
                }
                if ((this.ScrollBars == RichTextBoxScrollBars.Both) || (this.ScrollBars == RichTextBoxScrollBars.Horizontal))
                {
                    return false;
                }
                return (((this.ScrollBars != RichTextBoxScrollBars.ForcedBoth) && (this.ScrollBars != RichTextBoxScrollBars.ForcedHorizontal)) || (this.DefaultHorzScrollSize > 0));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual bool HasHScrollBar
        {
            get
            {
                return ((this.hScrollBar != null) && this.hScrollBar.Visible);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool HasVScrollBar
        {
            get
            {
                return ((this.vScrollBar != null) && this.vScrollBar.Visible);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Editor("QWhale.Design.ScrollingButtonsEditor, QWhale.Editor", typeof(UITypeEditor)), Category("Scrolling"), TypeConverter(typeof(ScrollingButtonsConverter))]
        public virtual IScrollingButtons HorzButtons
        {
            get
            {
                return this.horzButtons;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual bool HorzScrollbarVisible
        {
            get
            {
                if ((this.options & ScrollingOptions.SystemScrollbars) == ScrollingOptions.None)
                {
                    return this.HasHScrollBar;
                }
                return (this.owner.IsHandleCreated && (OSUtils.GetScrollSize(this.owner.Handle, this.flatBars, false) > 0));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IScrollingButton HorzSplitButton
        {
            get
            {
                if ((this.horzSplitButton == null) && this.CanSplitHorz())
                {
                    this.horzSplitButton = this.CreateSplitterButton(false);
                }
                return this.horzSplitButton;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ScrollBar HScrollBar
        {
            get
            {
                if (this.hScrollBar == null)
                {
                    this.hScrollBar = new System.Windows.Forms.HScrollBar();
                    this.hScrollBar.Visible = false;
                    this.hScrollBar.Width = 0;
                    this.hScrollBar.Cursor = Cursors.Default;
                    this.hScrollBar.Parent = (Control) this.owner;
                    this.hScrollBar.Scroll += new ScrollEventHandler(this.hScrollBar_Scroll);
                    this.hScrollBar.BringToFront();
                }
                return this.hScrollBar;
            }
        }

        [Description("Gets or sets a \"ScrollingOptions\" that determine scrolling behaviour."), Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
        public virtual ScrollingOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    bool flag = ((this.options ^ value) & ScrollingOptions.SystemScrollbars) != ScrollingOptions.None;
                    if (flag)
                    {
                        if ((value & ScrollingOptions.SystemScrollbars) != ScrollingOptions.None)
                        {
                            if ((value & ScrollingOptions.AllowSplitHorz) != ScrollingOptions.None)
                            {
                                value &= ~ScrollingOptions.AllowSplitHorz;
                            }
                            if ((value & ScrollingOptions.AllowSplitVert) != ScrollingOptions.None)
                            {
                                value &= ~ScrollingOptions.AllowSplitVert;
                            }
                            if ((value & ScrollingOptions.VertButtons) != ScrollingOptions.None)
                            {
                                value &= ~ScrollingOptions.VertButtons;
                            }
                            if ((value & ScrollingOptions.HorzButtons) != ScrollingOptions.None)
                            {
                                value &= ~ScrollingOptions.HorzButtons;
                            }
                            this.RemoveScrollbars(false);
                        }
                        else
                        {
                            if ((value & ScrollingOptions.FlatScrollbars) != ScrollingOptions.None)
                            {
                                value &= ~ScrollingOptions.FlatScrollbars;
                            }
                            this.RemoveScrollbars(true);
                        }
                    }
                    bool flag2 = ((this.options ^ value) & ScrollingOptions.FlatScrollbars) != ScrollingOptions.None;
                    this.options = value;
                    if (flag)
                    {
                        this.RemoveScrollbars((this.options & ScrollingOptions.SystemScrollbars) == ScrollingOptions.None);
                    }
                    if (flag2)
                    {
                        this.UpdateFlat();
                    }
                    this.UpdateScroll(true);
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISyntaxEdit Owner
        {
            get
            {
                return this.owner;
            }
        }

        [Description("Gets or sets the type of scroll bars displayed in the control."), DefaultValue(3)]
        public virtual RichTextBoxScrollBars ScrollBars
        {
            get
            {
                return this.scrollBars;
            }
            set
            {
                if (this.scrollBars != value)
                {
                    this.scrollBars = value;
                    this.OnScrollBarsChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool ScrollByPixels
        {
            get
            {
                if (this.owner.Pages.PageType != PageType.PageLayout)
                {
                    return ((this.options & ScrollingOptions.ScrollByPixels) != ScrollingOptions.None);
                }
                return true;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlScrollingInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        protected Timer SmoothTimer
        {
            get
            {
                if (this.smoothTimer == null)
                {
                    this.smoothTimer = new Timer();
                    this.smoothTimer.Enabled = false;
                    this.smoothTimer.Interval = EditConsts.DefaultSmoothScrollPause;
                    this.smoothTimer.Tick += new EventHandler(this.OnSmoothTimer);
                }
                return this.smoothTimer;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [Category("Scrolling"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Editor("QWhale.Design.ScrollingButtonsEditor, QWhale.Editor", typeof(UITypeEditor)), TypeConverter(typeof(ScrollingButtonsConverter))]
        public virtual IScrollingButtons VertButtons
        {
            get
            {
                return this.vertButtons;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool VertScrollbarVisible
        {
            get
            {
                if ((this.options & ScrollingOptions.SystemScrollbars) == ScrollingOptions.None)
                {
                    return this.HasVScrollBar;
                }
                return (this.owner.IsHandleCreated && (OSUtils.GetScrollSize(this.owner.Handle, this.flatBars, true) > 0));
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IScrollingButton VertSplitButton
        {
            get
            {
                if ((this.vertSplitButton == null) && this.CanSplitVert())
                {
                    this.vertSplitButton = this.CreateSplitterButton(true);
                }
                return this.vertSplitButton;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ScrollBar VScrollBar
        {
            get
            {
                if (this.vScrollBar == null)
                {
                    this.vScrollBar = new System.Windows.Forms.VScrollBar();
                    this.vScrollBar.Visible = false;
                    this.vScrollBar.Height = 0;
                    this.vScrollBar.Cursor = Cursors.Default;
                    this.vScrollBar.Parent = (Control) this.owner;
                    this.vScrollBar.Scroll += new ScrollEventHandler(this.vScrollBar_Scroll);
                    this.vScrollBar.BringToFront();
                }
                return this.vScrollBar;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int WindowOriginX
        {
            get
            {
                return this.windowOriginX;
            }
            set
            {
                value = Math.Max(value, 0);
                if (this.owner != null)
                {
                    if (this.ScrollByPixels)
                    {
                        int num = this.owner.Pages.Width - this.owner.ClientRect.Width;
                        if ((value >= num) && (num >= 0))
                        {
                            value = num;
                        }
                    }
                    else if ((((NavigateOptions.BeyondEol & this.owner.NavigateOptions) == NavigateOptions.None) && this.owner.Painter.IsMonoSpaced) && ((this.owner.Painter.FontWidth != 0) && !this.FixedScrollSize))
                    {
                        int num2 = (this.owner.DisplayLines.MaxLineWidth / this.owner.Painter.FontWidth) - this.owner.CharsInWidth;
                        if (value >= num2)
                        {
                            value = Math.Max(num2, 0);
                        }
                    }
                }
                if (this.windowOriginX != value)
                {
                    int windowOriginX = this.windowOriginX;
                    this.windowOriginX = value;
                    this.UpdateScrollPosition();
                    this.WindowOriginChanged(this.windowOriginY, windowOriginX, false);
                    this.OnHorizontalScroll();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int WindowOriginY
        {
            get
            {
                return this.windowOriginY;
            }
            set
            {
                if (this.updateCount > 0)
                {
                    this.windowOriginY = value;
                }
                else
                {
                    value = Math.Max(value, 0);
                    if (this.owner != null)
                    {
                        if (this.owner.Pages.PageType == PageType.PageLayout)
                        {
                            int num = this.owner.Pages.Height - this.owner.ClientRect.Height;
                            if ((value >= num) && (num >= 0))
                            {
                                value = num;
                            }
                        }
                        else if ((NavigateOptions.BeyondEof & this.owner.NavigateOptions) == NavigateOptions.None)
                        {
                            int num2 = 0;
                            if (this.ScrollByPixels)
                            {
                                num2 = ((this.owner.DisplayLines.DisplayCount * this.owner.Painter.FontHeight) - this.owner.ClientHeight) + 1;
                            }
                            else
                            {
                                num2 = (this.owner.DisplayLines.DisplayCount - this.owner.LinesInHeight) + 1;
                            }
                            if (value >= num2)
                            {
                                value = Math.Max(num2, 0);
                            }
                        }
                        if (this.windowOriginY != value)
                        {
                            int windowOriginY = this.windowOriginY;
                            this.windowOriginY = value;
                            this.UpdateScrollPosition();
                            if (this.NeedSmoothScroll())
                            {
                                this.SmoothScroll(windowOriginY, value);
                            }
                            else
                            {
                                this.WindowOriginChanged(windowOriginY, this.windowOriginX, false);
                            }
                            this.OnVerticalScroll();
                        }
                    }
                }
            }
        }
    }
}

