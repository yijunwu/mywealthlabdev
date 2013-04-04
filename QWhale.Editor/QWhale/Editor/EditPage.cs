namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Printing;

    public class EditPage : IEditPage, IUpdate
    {
        private int bottomIndent;
        private Size caps;
        private int endLine;
        private IEditPageHeader footer;
        private IEditPageHeader header;
        private int horzOffset = EditConsts.DefaultPageHorzOffset;
        private bool landscape;
        private int leftIndent;
        private System.Drawing.Printing.Margins margins;
        private Point origin;
        private ISyntaxEdit owner;
        private PaperKind pageKind;
        private IEditPages pages;
        private Size pageSize;
        private bool paintNumber = true;
        private int rightIndent;
        private int startLine;
        private int topIndent;
        private bool usePrinterSettings = true;
        private int vertOffset = EditConsts.DefaultPageVertOffset;

        public EditPage(IEditPages pages, ISyntaxEdit owner)
        {
            this.pages = pages;
            this.owner = owner;
            this.header = new PageHeader(this, owner);
            this.footer = new PageHeader(this, owner);
            this.origin = new Point(0, 0);
            this.pageSize = pages.DefaultPageSize;
            this.margins = new System.Drawing.Printing.Margins(pages.DefaultMargins.Left, pages.DefaultMargins.Right, pages.DefaultMargins.Top, pages.DefaultMargins.Bottom);
            this.pageKind = pages.DefaultPageKind;
            this.landscape = pages.DefaultLandscape;
            this.caps = pages.Caps;
            this.InitMargins();
        }

        public virtual void Assign(IEditPage source)
        {
            this.BeginUpdate();
            try
            {
                this.PageKind = source.PageKind;
                this.PageSize = source.PageSize;
                this.Landscape = source.Landscape;
                this.Margins = source.Margins;
                this.HorzOffset = source.HorzOffset;
                this.VertOffset = source.VertOffset;
                this.Header = source.Header;
                this.Footer = source.Footer;
                this.PaintNumber = source.PaintNumber;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual int BeginUpdate()
        {
            if (this.pages == null)
            {
                return 0;
            }
            return this.pages.BeginUpdate();
        }

        public virtual int DisableUpdate()
        {
            if (this.pages == null)
            {
                return 0;
            }
            return this.pages.DisableUpdate();
        }

        public virtual int EnableUpdate()
        {
            if (this.pages == null)
            {
                return 0;
            }
            return this.pages.EnableUpdate();
        }

        public virtual int EndUpdate()
        {
            if (this.pages == null)
            {
                return 0;
            }
            return this.pages.EndUpdate();
        }

        protected void FillBoundsRect(IPainter painter, Rectangle outerRect, Rectangle innerRect, Color color, bool transparent)
        {
            if (((!transparent && !innerRect.IsEmpty) && !outerRect.IsEmpty) && !innerRect.IsEmpty)
            {
                Color backColor = painter.BackColor;
                painter.BackColor = color;
                try
                {
                    this.FillRect(painter, outerRect, new Rectangle(outerRect.Left, outerRect.Top, innerRect.Left - outerRect.Left, outerRect.Height));
                    this.FillRect(painter, outerRect, new Rectangle(innerRect.Right, outerRect.Top, outerRect.Right - innerRect.Right, outerRect.Height));
                    this.FillRect(painter, outerRect, new Rectangle(innerRect.Left, outerRect.Top, innerRect.Width, innerRect.Top - outerRect.Top));
                    this.FillRect(painter, outerRect, new Rectangle(innerRect.Left, innerRect.Bottom, innerRect.Width, outerRect.Bottom - innerRect.Bottom));
                }
                finally
                {
                    painter.BackColor = backColor;
                }
            }
        }

        private void FillRect(IPainter painter, Rectangle r1, Rectangle r2)
        {
            Rectangle rect = Rectangle.Intersect(r1, r2);
            if (!rect.IsEmpty)
            {
                painter.FillRectangle(rect);
            }
        }

        private void FrameRect(IPainter painter, ref Rectangle rect, Color color, Color backColor, bool drawBorder)
        {
            Color color2 = painter.BackColor;
            Color foreColor = painter.ForeColor;
            painter.BackColor = color;
            painter.ForeColor = color;
            try
            {
                painter.DrawRectangle(new Rectangle(rect.Location, new Size(rect.Width - 1, rect.Height - 1)));
                painter.DrawLine(rect.Right, rect.Top + 1, rect.Right, rect.Bottom);
                painter.ForeColor = backColor;
                painter.DrawLine(rect.Right, rect.Top, rect.Right, rect.Top + 1);
                if (drawBorder || this.IsLastPage)
                {
                    painter.ForeColor = color;
                    painter.DrawLine(rect.Left + 1, rect.Bottom, rect.Right - 1, rect.Bottom);
                    painter.ForeColor = backColor;
                    painter.DrawLine(rect.Left, rect.Bottom, rect.Left + 1, rect.Bottom);
                }
                if (drawBorder)
                {
                    painter.ForeColor = color;
                    painter.DrawLine(rect.Right + 1, rect.Top + 1, rect.Right + 1, rect.Bottom + 1);
                    painter.DrawLine(rect.Left + 1, rect.Bottom + 1, rect.Right + 1, rect.Bottom + 1);
                    painter.ForeColor = backColor;
                    painter.DrawLine(rect.Right + 1, rect.Top, rect.Right + 1, rect.Top + 1);
                    painter.DrawLine(rect.Left, rect.Bottom + 1, rect.Left, rect.Bottom + 2);
                }
                rect.Width++;
                if (drawBorder || this.IsLastPage)
                {
                    rect.Height++;
                }
                if (drawBorder)
                {
                    rect.Width++;
                    rect.Height++;
                }
            }
            finally
            {
                painter.ForeColor = foreColor;
                painter.BackColor = color2;
            }
        }

        public virtual Rectangle GetBounds(bool includeSpace)
        {
            Rectangle rectangle;
            if (this.landscape)
            {
                rectangle = new Rectangle(this.origin.X, this.origin.Y, this.pageSize.Height, this.pageSize.Width);
            }
            else
            {
                rectangle = new Rectangle(this.origin.X, this.origin.Y, this.pageSize.Width, this.pageSize.Height);
            }
            if (this.pages != null)
            {
                int fontHeight = this.owner.Painter.FontHeight;
                if (this.pages.PageType == PageType.PageLayout)
                {
                    if (!this.pages.DisplayWhiteSpace)
                    {
                        rectangle.Height -= this.bottomIndent + this.topIndent;
                        if (fontHeight != 0)
                        {
                            rectangle.Height = (rectangle.Height / fontHeight) * fontHeight;
                        }
                        rectangle.Height += 8;
                        if (includeSpace)
                        {
                            int num2 = (this.Index == 0) ? 4 : 0;
                            rectangle = new Rectangle(rectangle.Left - this.horzOffset, rectangle.Top - num2, rectangle.Width + this.horzOffset, (rectangle.Height + num2) + 1);
                        }
                        return rectangle;
                    }
                    if (includeSpace)
                    {
                        rectangle = new Rectangle(rectangle.Left - this.horzOffset, rectangle.Top - this.vertOffset, rectangle.Width + this.horzOffset, (rectangle.Height + this.vertOffset) + 3);
                    }
                    return rectangle;
                }
                if (fontHeight != 0)
                {
                    rectangle.Height = (rectangle.Height / fontHeight) * fontHeight;
                }
            }
            return rectangle;
        }

        private void InitMargins()
        {
            this.leftIndent = (this.margins.Left * this.caps.Width) / 100;
            this.rightIndent = (this.margins.Right * this.caps.Width) / 100;
            this.topIndent = (this.margins.Top * this.caps.Height) / 100;
            this.bottomIndent = (this.margins.Bottom * this.caps.Height) / 100;
        }

        public virtual void Invalidate()
        {
            if (this.pages != null)
            {
                this.pages.Invalidate(this);
            }
        }

        private bool IsTransparent(bool nonClient)
        {
            return ((nonClient && this.pages.Transparent) || ((this.owner != null) && this.owner.Transparent));
        }

        protected virtual void OnBottomIndentChanged()
        {
            this.margins.Bottom = (this.bottomIndent * 100) / this.caps.Height;
        }

        protected virtual void OnFooterChanged()
        {
            this.Invalidate();
        }

        protected virtual void OnHeaderChanged()
        {
            this.Invalidate();
        }

        protected virtual void OnHorzOffsetChanged()
        {
            this.Update();
        }

        protected virtual void OnIndexChanged()
        {
        }

        protected virtual void OnLandscapeChanged()
        {
            this.Update(true);
        }

        protected virtual void OnLeftIndentChanged()
        {
            this.margins.Left = (this.leftIndent * 100) / this.caps.Width;
        }

        protected virtual void OnMarginsChanged()
        {
            this.InitMargins();
            this.Update(true);
        }

        protected virtual void OnOriginChanged()
        {
        }

        protected virtual void OnPageKindChanged()
        {
            this.UpdatePageSize(this.PageKind);
        }

        protected virtual void OnPagesChanged()
        {
        }

        protected virtual void OnPageSizeChanged()
        {
            this.Update(true);
        }

        protected virtual void OnPaintNumberChanged()
        {
            this.Invalidate();
        }

        protected virtual void OnRightIndentChanged()
        {
            this.margins.Right = (this.rightIndent * 100) / this.caps.Width;
        }

        protected virtual void OnTopIndentChanged()
        {
            this.margins.Top = (this.topIndent * 100) / this.caps.Height;
        }

        protected virtual void OnUsePrinterSettingsChanged()
        {
            if (this.usePrinterSettings && (this.pages != null))
            {
                this.Margins = this.pages.DefaultMargins;
                this.PageKind = this.pages.DefaultPageKind;
                this.PageSize = this.pages.DefaultPageSize;
                this.Landscape = this.pages.DefaultLandscape;
            }
        }

        protected virtual void OnVertOffsetChanged()
        {
            this.Update();
        }

        public virtual void Paint(IPainter painter)
        {
            if ((this.pages != null) && (this.pages.PageType != PageType.Normal))
            {
                Rectangle clientRect = this.ClientRect;
                IDrawInfo info = new DrawInfo();
                if (this.pages.PageType == PageType.PageBreaks)
                {
                    Rectangle rectangle2 = this.owner.ClientRect;
                    clientRect.X = rectangle2.Left + this.owner.Gutter.DisplayWidth;
                    clientRect.Width = rectangle2.Right - clientRect.Left;
                    if (!this.IsLastPage)
                    {
                        info.Page = this.Index;
                        if (!this.owner.SyntaxPaint.OnCustomDraw(painter, clientRect, DrawStage.Before, DrawState.PageBorder, info))
                        {
                            painter.DrawLine(clientRect.Left, clientRect.Bottom - 1, clientRect.Right - 1, clientRect.Bottom - 1, this.pages.BorderColor, 1, DashStyle.Dot);
                            this.owner.SyntaxPaint.OnCustomDraw(painter, clientRect, DrawStage.After, DrawState.PageBorder, info);
                        }
                    }
                }
                else
                {
                    Rectangle boundsRect = this.BoundsRect;
                    info.Page = this.Index;
                    if (!this.owner.SyntaxPaint.OnCustomDraw(painter, boundsRect, DrawStage.Before, DrawState.PageBorder, info))
                    {
                        bool displayWhiteSpace = this.pages.DisplayWhiteSpace;
                        int fontHeight = this.owner.Painter.FontHeight;
                        if (fontHeight != 0)
                        {
                            clientRect.Height = (clientRect.Height / fontHeight) * fontHeight;
                        }
                        Rectangle paintRect = this.PaintRect;
                        boundsRect.Inflate(1, 1);
                        this.FrameRect(painter, ref boundsRect, this.pages.BorderColor, this.pages.BackColor, displayWhiteSpace);
                        if (!this.IsTransparent(true))
                        {
                            this.FillBoundsRect(painter, paintRect, boundsRect, this.pages.BackColor, false);
                        }
                        boundsRect = this.BoundsRect;
                        this.FillBoundsRect(painter, boundsRect, clientRect, this.owner.BackColor, this.IsTransparent(false));
                        this.PaintMargins(painter, boundsRect, clientRect, this.pages.BackColor);
                        if ((this.pages == null) || this.pages.DisplayWhiteSpace)
                        {
                            int x = Math.Max(clientRect.Left - this.header.Offset.X, boundsRect.Left);
                            int num3 = Math.Min((clientRect.Left + clientRect.Width) + this.header.Offset.X, boundsRect.Right);
                            Rectangle rect = new Rectangle(x, Math.Max(clientRect.Top - (this.header.Offset.Y + this.header.Font.Height), this.BoundsRect.Top), num3 - x, this.header.Font.Height);
                            this.header.Paint(painter, rect, this.Index, this.pages.Count, false);
                            x = Math.Max(clientRect.Left - this.footer.Offset.X, boundsRect.Left);
                            num3 = Math.Min((clientRect.Left + clientRect.Width) + this.footer.Offset.X, boundsRect.Right);
                            rect = new Rectangle(x, Math.Min((int) (clientRect.Bottom + this.footer.Offset.Y), (int) (boundsRect.Bottom - this.footer.Font.Height)), num3 - x, this.footer.Font.Height);
                            this.footer.Paint(painter, rect, this.Index, this.pages.Count, this.paintNumber);
                        }
                        this.owner.SyntaxPaint.OnCustomDraw(painter, boundsRect, DrawStage.After, DrawState.PageBorder, info);
                    }
                    this.owner.SyntaxPaint.PaintWindow(painter, this.startLine, new Rectangle(0, 0, clientRect.Width, clientRect.Height), clientRect.Location, 1f, 1f, true, false);
                }
            }
        }

        protected void PaintMargins(IPainter painter, Rectangle boundsR, Rectangle clientR, Color color)
        {
            if (!clientR.IsEmpty)
            {
                Color foreColor = painter.ForeColor;
                painter.ForeColor = color;
                try
                {
                    int defaultPageMarginSize = EditConsts.DefaultPageMarginSize;
                    painter.DrawLine(clientR.Left, clientR.Top, Math.Max(clientR.Left - defaultPageMarginSize, boundsR.Left), clientR.Top);
                    painter.DrawLine(clientR.Left, clientR.Top, clientR.Left, Math.Max(clientR.Top - defaultPageMarginSize, boundsR.Top));
                    painter.DrawLine(clientR.Right, clientR.Top, Math.Min((int) (clientR.Right + defaultPageMarginSize), (int) (boundsR.Right - 1)), clientR.Top);
                    painter.DrawLine(clientR.Right, clientR.Top, clientR.Right, Math.Max(clientR.Top - defaultPageMarginSize, boundsR.Top));
                    painter.DrawLine(clientR.Left, clientR.Bottom, Math.Max(clientR.Left - defaultPageMarginSize, boundsR.Left), clientR.Bottom);
                    painter.DrawLine(clientR.Left, clientR.Bottom, clientR.Left, Math.Min((int) (clientR.Bottom + defaultPageMarginSize), (int) (boundsR.Bottom - 1)));
                    painter.DrawLine(clientR.Right, clientR.Bottom, Math.Min((int) (clientR.Right + defaultPageMarginSize), (int) (boundsR.Right - 1)), clientR.Bottom);
                    painter.DrawLine(clientR.Right, clientR.Bottom, clientR.Right, Math.Min((int) (clientR.Bottom + defaultPageMarginSize), (int) (boundsR.Bottom - 1)));
                }
                finally
                {
                    painter.ForeColor = foreColor;
                }
            }
        }

        private void ScrollRect(ref Rectangle rect)
        {
            if (this.pages != null)
            {
                if (this.owner.Scrolling.ScrollByPixels)
                {
                    rect.Offset(-this.owner.Scrolling.WindowOriginX, -this.owner.Scrolling.WindowOriginY);
                }
                else
                {
                    rect.Offset(-this.owner.Scrolling.WindowOriginX * this.owner.Painter.FontWidth, -this.owner.Scrolling.WindowOriginY * this.owner.Painter.FontHeight);
                }
            }
        }

        public bool ShouldSerializeHorzOffset()
        {
            return (this.horzOffset != EditConsts.DefaultPageHorzOffset);
        }

        public bool ShouldSerializeLandscape()
        {
            if (this.pages != null)
            {
                return (this.landscape != this.pages.DefaultLandscape);
            }
            return true;
        }

        public bool ShouldSerializeMargins()
        {
            if (((this.pages != null) && (this.margins.Left == this.pages.DefaultMargins.Left)) && ((this.margins.Right == this.pages.DefaultMargins.Right) && (this.margins.Top == this.pages.DefaultMargins.Top)))
            {
                return (this.margins.Bottom != this.pages.DefaultMargins.Bottom);
            }
            return true;
        }

        public bool ShouldSerializePageKind()
        {
            if (this.pages != null)
            {
                return (this.pageKind != this.pages.DefaultPageKind);
            }
            return true;
        }

        public bool ShouldSerializePageSize()
        {
            if (((this.pages != null) && this.usePrinterSettings) && (this.pageSize.Width == this.pages.DefaultPageSize.Width))
            {
                return (this.pageSize.Height != this.pages.DefaultPageSize.Height);
            }
            return true;
        }

        public bool ShouldSerializeVertOffset()
        {
            return (this.vertOffset != EditConsts.DefaultPageVertOffset);
        }

        public virtual void Update()
        {
            this.Update(false);
        }

        public virtual void Update(bool changed)
        {
            if (this.pages != null)
            {
                this.pages.Update(this, changed);
            }
        }

        public virtual void Update(int startLine, int endLine, Point origin)
        {
            this.startLine = startLine;
            this.endLine = endLine;
            this.Origin = origin;
        }

        protected void UpdatePageSize(PaperKind pageKind)
        {
            if (((this.owner != null) && (this.pages.UpdateCount == 0)) && (this.owner.Printing.PrinterSettings != null))
            {
                foreach (PaperSize size in this.owner.Printing.PrinterSettings.PaperSizes)
                {
                    if (size.Kind == pageKind)
                    {
                        this.pageSize = new Size(size.Width, size.Height);
                        this.owner.Scrolling.UpdateScroll();
                        break;
                    }
                }
            }
            this.Update(true);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int BottomIndent
        {
            get
            {
                return this.bottomIndent;
            }
            set
            {
                if (this.bottomIndent != value)
                {
                    this.bottomIndent = value;
                    this.OnBottomIndentChanged();
                }
            }
        }

        [Browsable(false)]
        public virtual Rectangle BoundsRect
        {
            get
            {
                Rectangle bounds = this.GetBounds(false);
                this.ScrollRect(ref bounds);
                return bounds;
            }
        }

        [Browsable(false)]
        public virtual Rectangle ClientRect
        {
            get
            {
                Rectangle boundsRect = this.BoundsRect;
                if ((this.pages != null) && (this.pages.PageType == PageType.PageLayout))
                {
                    bool displayWhiteSpace = this.pages.DisplayWhiteSpace;
                    boundsRect = new Rectangle(boundsRect.X + this.leftIndent, boundsRect.Y + (displayWhiteSpace ? this.topIndent : 4), boundsRect.Width - (this.leftIndent + this.rightIndent), boundsRect.Height - (displayWhiteSpace ? (this.topIndent + this.bottomIndent) : 8));
                }
                return boundsRect;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int DisplayWidth
        {
            get
            {
                int width = this.ClientRect.Width;
                if (this.pages == null)
                {
                    return width;
                }
                if (this.pages.PageType != PageType.Normal)
                {
                    width = this.pageSize.Width - (this.leftIndent + this.rightIndent);
                }
                return (width - this.owner.Gutter.DisplayWidth);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int EndLine
        {
            get
            {
                return this.endLine;
            }
        }

        [Description("Represents \"IEditPageHeader\" specifying page footer."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual IEditPageHeader Footer
        {
            get
            {
                return this.footer;
            }
            set
            {
                if (this.footer != value)
                {
                    this.footer = value;
                    this.OnFooterChanged();
                }
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Represents \"IEditPageHeader\" specifying page header.")]
        public virtual IEditPageHeader Header
        {
            get
            {
                return this.header;
            }
            set
            {
                if (this.header != value)
                {
                    this.header = value;
                    this.OnHeaderChanged();
                    this.Invalidate();
                }
            }
        }

        [Description("Gets or sets a horizontal indentation between pages.")]
        public virtual int HorzOffset
        {
            get
            {
                return this.horzOffset;
            }
            set
            {
                if (this.HorzOffset != value)
                {
                    this.horzOffset = value;
                    this.OnHorzOffsetChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int Index
        {
            get
            {
                return this.pages.IndexOf(this);
            }
        }

        [Browsable(false)]
        public virtual bool IsFirstPage
        {
            get
            {
                return (this.Index == 0);
            }
        }

        [Browsable(false)]
        public virtual bool IsLastPage
        {
            get
            {
                return ((this.pages != null) && (this.Index == (this.pages.Count - 1)));
            }
        }

        [Description("Gets or sets boolean value that indicates the page orientation (landscape or portrait).")]
        public virtual bool Landscape
        {
            get
            {
                return this.landscape;
            }
            set
            {
                if (this.landscape != value)
                {
                    this.landscape = value;
                    this.OnLandscapeChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int LeftIndent
        {
            get
            {
                return this.leftIndent;
            }
            set
            {
                if (this.leftIndent != value)
                {
                    this.leftIndent = value;
                    this.OnLeftIndentChanged();
                }
            }
        }

        [Description("Gets or sets margins specifying offsets of editing area of this \"EditPage\".")]
        public virtual System.Drawing.Printing.Margins Margins
        {
            get
            {
                return this.margins;
            }
            set
            {
                if ((this.margins != value) && (value != null))
                {
                    this.margins.Left = value.Left;
                    this.margins.Top = value.Top;
                    this.margins.Right = value.Right;
                    this.margins.Bottom = value.Bottom;
                    this.OnMarginsChanged();
                }
            }
        }

        [Browsable(false)]
        public virtual IEditPage NextPage
        {
            get
            {
                int index = this.Index;
                if ((this.pages != null) && (index < (this.pages.Count - 1)))
                {
                    return this.pages[index + 1];
                }
                return null;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Point Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                if (this.origin != value)
                {
                    this.origin = value;
                    this.OnOriginChanged();
                }
            }
        }

        [Description("Gets or sets kind of the page specifying standart paper size.")]
        public virtual PaperKind PageKind
        {
            get
            {
                return this.pageKind;
            }
            set
            {
                if (this.pageKind != value)
                {
                    this.pageKind = value;
                    this.OnPageKindChanged();
                }
            }
        }

        [Browsable(false)]
        public virtual Rectangle PageRect
        {
            get
            {
                Rectangle bounds = this.GetBounds(true);
                this.ScrollRect(ref bounds);
                return bounds;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IEditPages Pages
        {
            get
            {
                return this.pages;
            }
            set
            {
                if (this.pages != value)
                {
                    this.pages = value;
                    this.OnPagesChanged();
                }
            }
        }

        [Description("Gets or sets size (width and height) of this \"EditPage\".")]
        public virtual Size PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                if (!this.pageSize.Equals(value))
                {
                    this.pageSize = value;
                    this.OnPageSizeChanged();
                }
            }
        }

        [DefaultValue(true), Description("Get or sets a boolean value that indicates whether control should paint it's number at the bottom of page.")]
        public virtual bool PaintNumber
        {
            get
            {
                return this.paintNumber;
            }
            set
            {
                if (this.paintNumber != value)
                {
                    this.paintNumber = value;
                    this.OnPaintNumberChanged();
                }
            }
        }

        [Browsable(false)]
        public Rectangle PaintRect
        {
            get
            {
                Rectangle pageRect = this.PageRect;
                if (this.pages != null)
                {
                    pageRect.X = 0;
                    pageRect.Width = this.owner.ClientArea.Width;
                }
                return pageRect;
            }
        }

        [Browsable(false)]
        public virtual IEditPage PrevPage
        {
            get
            {
                int index = this.Index;
                if ((index > 0) && (this.pages != null))
                {
                    return this.pages[index - 1];
                }
                return null;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int RightIndent
        {
            get
            {
                return this.rightIndent;
            }
            set
            {
                if (this.rightIndent != value)
                {
                    this.rightIndent = value;
                    this.OnRightIndentChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlEditPageInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int StartLine
        {
            get
            {
                return this.startLine;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int TopIndent
        {
            get
            {
                return this.topIndent;
            }
            set
            {
                if (this.topIndent != value)
                {
                    this.topIndent = value;
                    this.OnTopIndentChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual int UpdateCount
        {
            get
            {
                if (this.pages == null)
                {
                    return 0;
                }
                return this.pages.UpdateCount;
            }
        }

        [DefaultValue(true), Description("Get or sets a boolean value that indicates whether control should use printer settings for calcuating page size, margin and orientation.")]
        public virtual bool UsePrinterSettings
        {
            get
            {
                return this.usePrinterSettings;
            }
            set
            {
                if (this.usePrinterSettings != value)
                {
                    this.usePrinterSettings = value;
                    this.OnUsePrinterSettingsChanged();
                }
            }
        }

        [Description("Gets or sets a vertical indentation between pages.")]
        public virtual int VertOffset
        {
            get
            {
                return this.vertOffset;
            }
            set
            {
                if (this.vertOffset != value)
                {
                    this.vertOffset = value;
                    this.OnVertOffsetChanged();
                }
            }
        }

        [Browsable(false)]
        public virtual Rectangle WhiteSpaceBottomRect
        {
            get
            {
                Rectangle boundsRect = this.BoundsRect;
                if (this.pages == null)
                {
                    return boundsRect;
                }
                if (this.pages.PageType == PageType.PageLayout)
                {
                    bool displayWhiteSpace = this.pages.DisplayWhiteSpace;
                    return new Rectangle(boundsRect.Left, boundsRect.Bottom, boundsRect.Width, displayWhiteSpace ? this.vertOffset : (this.vertOffset / 2));
                }
                return new Rectangle(0, 0, 0, 0);
            }
        }

        [Browsable(false)]
        public virtual Rectangle WhiteSpaceTopRect
        {
            get
            {
                Rectangle boundsRect = this.BoundsRect;
                if (this.pages == null)
                {
                    return boundsRect;
                }
                if (this.pages.PageType == PageType.PageLayout)
                {
                    bool displayWhiteSpace = this.pages.DisplayWhiteSpace;
                    return new Rectangle(boundsRect.Left, boundsRect.Top - (displayWhiteSpace ? this.vertOffset : (this.vertOffset / 2)), boundsRect.Width, displayWhiteSpace ? this.vertOffset : (this.vertOffset / 2));
                }
                return new Rectangle(0, 0, 0, 0);
            }
        }
    }
}

