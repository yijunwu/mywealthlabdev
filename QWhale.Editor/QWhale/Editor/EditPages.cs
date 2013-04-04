namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Printing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class EditPages : IEditPages, IUpdate, IEnumerable<IEditPage>, IEnumerable
    {
        private bool applyRulerToAllPages;
        private Color backColor;
        private Color borderColor;
        private Size caps;
        private bool defaultLandscape;
        private Margins defaultMargins;
        private PaperKind defaultPageKind;
        private Size defaultPageSize;
        private bool displayWhiteSpace;
        private IEditRuler horzRuler;
        private bool initialized;
        private ISortList<IEditPage> list;
        private ISyntaxEdit owner;
        private IComparer pageComparer;
        private IComparer pageTextComparer;
        private QWhale.Editor.PageType pageType;
        private QWhale.Editor.RulerOptions rulerOptions;
        private EditRulers rulers;
        private QWhale.Editor.RulerUnits rulerUnits;
        private bool transparent;
        private int updateCount;
        private IEditRuler vertRuler;

        [Browsable(false)]
        public event DrawHeaderEvent DrawHeader;

        public EditPages()
        {
            this.list = new SortList<IEditPage>();
            this.displayWhiteSpace = true;
            this.backColor = EditConsts.DefaultPageBackColor;
            this.borderColor = EditConsts.DefaultPageBorderColor;
            this.defaultMargins = new Margins(EditConsts.DefaultPageMargin, EditConsts.DefaultPageMargin, EditConsts.DefaultPageMargin, EditConsts.DefaultPageMargin);
            this.pageComparer = new PageComparer();
            this.pageTextComparer = new PageTextComparer();
            this.rulerUnits = QWhale.Editor.RulerUnits.Inches;
            this.rulerOptions = EditConsts.DefaultRulerOptions;
            this.caps = this.InitCaps();
            this.InitDefaultPageSettings();
            this.InitDefaultPrinterSettings();
        }

        public EditPages(ISyntaxEdit owner) : this()
        {
            this.owner = owner;
        }

        public virtual IEditPage Add()
        {
            IEditPage item = new EditPage(this, this.owner);
            this.list.Add(item);
            return item;
        }

        public virtual int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual void CancelDragging()
        {
            if (this.horzRuler != null)
            {
                this.horzRuler.CancelDragging();
            }
            if (this.vertRuler != null)
            {
                this.vertRuler.CancelDragging();
            }
        }

        public virtual void Clear()
        {
            this.list.Clear();
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual void DisplayRulers()
        {
            if ((this.owner != null) && (this.rulers != EditRulers.None))
            {
                IEditPage pageAt = this.GetPageAt(this.owner.DisplayLines.PointToDisplayPoint(this.owner.Position));
                if (pageAt != null)
                {
                    Rectangle boundsRect = pageAt.BoundsRect;
                    Rectangle clientRect = pageAt.ClientRect;
                    if ((this.rulers & EditRulers.Horizonal) != EditRulers.None)
                    {
                        IEditRuler horzRuler = this.HorzRuler;
                        horzRuler.RulerStart = boundsRect.Left - horzRuler.Left;
                        horzRuler.RulerWidth = boundsRect.Width;
                        horzRuler.PageStart = clientRect.Left - horzRuler.Left;
                        horzRuler.PageWidth = clientRect.Width;
                        horzRuler.MarkWidth = this.owner.Painter.FontWidth;
                        horzRuler.Update();
                    }
                    if ((this.rulers & EditRulers.Vertical) != EditRulers.None)
                    {
                        IEditRuler vertRuler = this.VertRuler;
                        vertRuler.RulerStart = boundsRect.Top - vertRuler.Top;
                        vertRuler.RulerWidth = boundsRect.Height;
                        vertRuler.PageStart = clientRect.Top - vertRuler.Top;
                        vertRuler.PageWidth = clientRect.Height;
                        vertRuler.MarkWidth = this.owner.Painter.FontWidth;
                        vertRuler.Update();
                    }
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

        protected IEditPage GetPage(int index)
        {
            if (this.Count == 0)
            {
                this.Add();
            }
            if (index < 0)
            {
                index = 0;
            }
            while (index >= this.Count)
            {
                this.Add().Assign(this[this.Count - 2]);
            }
            return this[index];
        }

        public virtual IEditPage GetPageAt(Point position)
        {
            return this.GetPage(this.GetPageIndexAt(position));
        }

        public virtual IEditPage GetPageAt(int x, int y)
        {
            return this.GetPage(this.GetPageIndexAt(x, y));
        }

        public virtual IEditPage GetPageAtCursor()
        {
            return this.GetPage(this.GetPageIndexAtCursor());
        }

        public virtual IEditPage GetPageAtPoint(Point position)
        {
            return this.GetPage(this.GetPageIndexAtPoint(position));
        }

        public virtual IEditPage GetPageAtPoint(int x, int y)
        {
            return this.GetPage(this.GetPageIndexAtPoint(x, y));
        }

        public virtual int GetPageIndexAt(Point position)
        {
            return this.GetPageIndexAt(position.X, position.Y);
        }

        public virtual int GetPageIndexAt(int x, int y)
        {
            int num;
            if (this.list.FindExact(y, out num, this.pageTextComparer))
            {
                return num;
            }
            if (y < 0)
            {
                return 0;
            }
            return (this.Count - 1);
        }

        public virtual int GetPageIndexAtCursor()
        {
            if (this.owner == null)
            {
                return -1;
            }
            return this.GetPageIndexAt(this.owner.DisplayLines.PointToDisplayPoint(this.owner.Position));
        }

        public virtual int GetPageIndexAtPoint(Point position)
        {
            return this.GetPageIndexAtPoint(position.X, position.Y);
        }

        public virtual int GetPageIndexAtPoint(int x, int y)
        {
            int num;
            if (this.list.FindExact(y, out num, this.pageComparer))
            {
                return num;
            }
            if ((this.Count > 0) && (y < this[0].PageRect.Top))
            {
                return 0;
            }
            return (this.Count - 1);
        }

        public virtual int IndexOf(IEditPage page)
        {
            return this.list.IndexOf(page);
        }

        private Size InitCaps()
        {
            return OSUtils.GetScreenCaps();
        }

        protected void InitDefaultPageSettings()
        {
            this.defaultPageSize = new Size((0x33b * this.caps.Width) / 100, (0x491 * this.caps.Height) / 100);
            this.defaultPageKind = PaperKind.Custom;
            this.defaultMargins = new Margins(100, 100, 100, 100);
            this.defaultLandscape = false;
        }

        public virtual void InitDefaultPageSettings(PageSettings pageSettings)
        {
            this.InitDefaultPageSettings(pageSettings, true);
        }

        protected void InitDefaultPageSettings(PageSettings pageSettings, bool updateSize)
        {
            try
            {
                if (updateSize)
                {
                    PaperSize paperSize = pageSettings.PaperSize;
                    this.defaultPageSize = new Size(paperSize.Width, paperSize.Height);
                    this.defaultPageKind = paperSize.Kind;
                    this.defaultMargins = pageSettings.Margins;
                    this.defaultLandscape = pageSettings.Landscape;
                }
                else
                {
                    this.InitDefaultPageSettings();
                }
            }
            catch
            {
                this.InitDefaultPageSettings();
            }
        }

        protected void InitDefaultPrinterSettings()
        {
            if (this.owner != null)
            {
                this.InitDefaultPageSettings(this.owner.Printing.PrinterSettings.DefaultPageSettings, false);
            }
        }

        protected void InitPages()
        {
            if (this.Count > 0)
            {
                this.BeginUpdate();
                try
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        IEditPage page = this[i];
                        if (page.UsePrinterSettings)
                        {
                            page.Margins = this.defaultMargins;
                            page.PageKind = this.defaultPageKind;
                            page.PageSize = this.defaultPageSize;
                            page.Landscape = this.defaultLandscape;
                        }
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public virtual void Invalidate(IEditPage page)
        {
            if (this.updateCount == 0)
            {
                if (page != null)
                {
                    this.owner.Invalidate(page.PageRect);
                }
                else
                {
                    this.owner.Invalidate();
                }
            }
        }

        public virtual bool IsTransparent(bool nonClient)
        {
            return ((nonClient && this.transparent) || ((this.owner != null) && this.owner.Transparent));
        }

        protected virtual void OnApplyRulerToAllPagesChanged()
        {
        }

        protected virtual void OnBackColorChanged()
        {
            this.owner.Invalidate();
        }

        protected virtual void OnBorderColorChanged()
        {
            this.owner.Invalidate();
        }

        protected virtual void OnDefaultPageChanged()
        {
        }

        protected virtual void OnDisplayWhiteSpaceChanged()
        {
            this.Update();
        }

        public virtual void OnDrawHeader(ref string text)
        {
            if (this.DrawHeader != null)
            {
                DrawHeaderEventArgs e = new DrawHeaderEventArgs(text);
                this.DrawHeader(this, e);
                if (e.Handled)
                {
                    text = e.Text;
                }
            }
        }

        protected virtual void OnPageTypeChanged()
        {
            this.InitPages();
            this.RecalculatePages();
            this.UpdateRulerAllowDrag();
        }

        protected virtual void OnRulerBackColorChanged()
        {
        }

        protected virtual void OnRulerIndentBackColorChanged()
        {
        }

        protected virtual void OnRulerOptionsChanged()
        {
            if (this.horzRuler != null)
            {
                this.horzRuler.Options = this.rulerOptions;
            }
            if (this.vertRuler != null)
            {
                this.vertRuler.Options = this.rulerOptions;
            }
        }

        protected virtual void OnRulersChanged()
        {
            if ((this.rulers & EditRulers.Horizonal) != EditRulers.None)
            {
                this.HorzRuler.Visible = true;
            }
            else if (this.horzRuler != null)
            {
                this.horzRuler.Visible = false;
            }
            if ((this.rulers & EditRulers.Vertical) != EditRulers.None)
            {
                this.VertRuler.Visible = true;
            }
            else if (this.vertRuler != null)
            {
                this.vertRuler.Visible = false;
            }
            this.UpdateRulerSize();
            this.RecalculatePages();
        }

        protected virtual void OnRulerUnitsChanged()
        {
            if (this.horzRuler != null)
            {
                this.horzRuler.Units = this.rulerUnits;
            }
            if (this.vertRuler != null)
            {
                this.vertRuler.Units = this.rulerUnits;
            }
        }

        protected virtual void OnTransparentChanged()
        {
            if (this.owner != null)
            {
                this.owner.OnStateChanged(this, NotifyState.PageOptionsChanged);
            }
        }

        public virtual void Paint(IPainter painter, Rectangle rect)
        {
            if (this.Count != 0)
            {
                int y = 0;
                for (int i = Math.Max(this.GetPageIndexAtPoint(rect.Location), 0); i < this.Count; i++)
                {
                    IEditPage page = this[i];
                    page.Paint(painter);
                    y = page.BoundsRect.Bottom + 1;
                    if (this.displayWhiteSpace)
                    {
                        y += 2;
                    }
                    if (y >= rect.Bottom)
                    {
                        return;
                    }
                }
                if (((this.pageType == QWhale.Editor.PageType.PageLayout) && (y < rect.Bottom)) && !this.IsTransparent(true))
                {
                    Rectangle rectangle = new Rectangle(rect.Left, y, rect.Width, rect.Bottom - y);
                    Color backColor = painter.BackColor;
                    painter.BackColor = this.BackColor;
                    try
                    {
                        painter.FillRectangle(rectangle);
                    }
                    finally
                    {
                        painter.BackColor = backColor;
                    }
                }
            }
        }

        protected void RecalculatePages()
        {
            this.UpdatePageSize();
            this.Update();
            this.Invalidate(null);
            if (this.owner != null)
            {
                this.owner.UpdateCaret();
                this.owner.Scrolling.UpdateScroll();
            }
        }

        public virtual void ResetBackColor()
        {
            this.BackColor = EditConsts.DefaultPageBackColor;
        }

        public virtual void ResetBorderColor()
        {
            this.BorderColor = EditConsts.DefaultPageBorderColor;
        }

        public virtual void ResetDisplayWhiteSpace()
        {
            this.DisplayWhiteSpace = true;
        }

        public virtual void ResetPageType()
        {
            this.PageType = QWhale.Editor.PageType.Normal;
        }

        public virtual void ResetRulerBackColor()
        {
            this.RulerBackColor = EditConsts.DefaultRulerBackColor;
        }

        public virtual void ResetRulerIndentBackColor()
        {
            this.RulerIndentBackColor = EditConsts.DefaultRulerIndentBackColor;
        }

        public virtual void ResetRulerOptions()
        {
            this.RulerOptions = EditConsts.DefaultRulerOptions;
        }

        public virtual void ResetRulers()
        {
            this.Rulers = EditRulers.None;
        }

        public virtual void ResetRulerUnits()
        {
            this.RulerUnits = QWhale.Editor.RulerUnits.Inches;
        }

        protected virtual void RulerChanged(object sender, EventArgs e)
        {
            if ((this.owner != null) && (sender is IEditRuler))
            {
                int pageIndexAt;
                int num2;
                if (this.ApplyRulerToAllPages)
                {
                    pageIndexAt = 0;
                    num2 = this.Count - 1;
                }
                else if (this.owner.Selection.IsEmpty)
                {
                    pageIndexAt = this.GetPageIndexAt(this.owner.DisplayLines.PointToDisplayPoint(this.owner.Position));
                    num2 = pageIndexAt;
                }
                else
                {
                    Rectangle selectionRect = this.owner.Selection.SelectionRect;
                    pageIndexAt = this.GetPageIndexAt(this.owner.DisplayLines.PointToDisplayPoint(selectionRect.Location));
                    num2 = this.GetPageIndexAt(this.owner.DisplayLines.PointToDisplayPoint(selectionRect.Right, selectionRect.Bottom));
                }
                this.BeginUpdate();
                try
                {
                    IEditRuler ruler = (IEditRuler) sender;
                    RulerIndent indent = ((RulerEventArgs) e).Object as RulerIndent;
                    for (int i = Math.Max(pageIndexAt, 0); i <= num2; i++)
                    {
                        IEditPage page = this[i];
                        if (ruler.Vertical)
                        {
                            if (indent.Orientation == IndentOrientation.Near)
                            {
                                page.TopIndent = indent.Indent;
                            }
                            else
                            {
                                page.BottomIndent = indent.Indent;
                            }
                        }
                        else if (indent.Orientation == IndentOrientation.Near)
                        {
                            page.LeftIndent = indent.Indent;
                        }
                        else
                        {
                            page.RightIndent = indent.Indent;
                        }
                    }
                    this.owner.UpdateCaret();
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.backColor != EditConsts.DefaultPageBackColor);
        }

        public bool ShouldSerializeBorderColor()
        {
            return (this.borderColor != EditConsts.DefaultPageBorderColor);
        }

        public bool ShouldSerializePageKind()
        {
            return (this.PageKind != this.DefaultPageKind);
        }

        public bool ShouldSerializeRulerBackColor()
        {
            return (this.RulerBackColor != EditConsts.DefaultRulerBackColor);
        }

        public bool ShouldSerializeRulerIndentBackColor()
        {
            return (this.RulerIndentBackColor != EditConsts.DefaultRulerIndentBackColor);
        }

        public bool ShouldSerializeRulerOptions()
        {
            return (this.rulerOptions != EditConsts.DefaultRulerOptions);
        }

        IEnumerator<IEditPage> IEnumerable<IEditPage>.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public virtual void Update()
        {
            this.UpdatePages(0, 0x7fffffff, true);
            this.Invalidate(null);
        }

        public virtual void Update(IEditPage page)
        {
            this.Update(page, false);
        }

        public virtual void Update(IEditPage page, bool changed)
        {
            this.UpdatePages(page.Index, 0x7fffffff, changed);
        }

        public virtual void UpdatePages(int index)
        {
            this.UpdatePages(index, 0x7fffffff, false);
        }

        protected void UpdatePages(int startIndex, int endIndex, bool changed)
        {
            if ((this.updateCount <= 0) || !changed)
            {
                this.updateCount++;
                try
                {
                    int index = Math.Max(startIndex, 0);
                    int displayCount = this.owner.DisplayLines.DisplayCount;
                    int first = 0;
                    bool flag = this.pageType == QWhale.Editor.PageType.PageLayout;
                    bool flag2 = this.displayWhiteSpace && flag;
                    int bottom = 0;
                    if (index > 0)
                    {
                        IEditPage page = this[index - 1];
                        bottom = page.GetBounds(true).Bottom;
                        first = page.EndLine + 1;
                    }
                    if (changed)
                    {
                        this.owner.UpdateWordWrap(first, 0x7fffffff);
                    }
                    int fontHeight = this.owner.Painter.FontHeight;
                    while (((first < displayCount) || (displayCount == 0)) && (index <= endIndex))
                    {
                        IEditPage page2 = this.GetPage(index);
                        int num6 = Math.Max((fontHeight > 0) ? (page2.ClientRect.Height / fontHeight) : 1, 1);
                        int num7 = ((this.rulers & EditRulers.Vertical) != EditRulers.None) ? EditConsts.DefaultRulerHeight : 0;
                        int num8 = ((index == 0) && ((this.rulers & EditRulers.Horizonal) != EditRulers.None)) ? EditConsts.DefaultRulerHeight : 0;
                        page2.Update(first, (first + num6) - 1, new Point((flag ? page2.HorzOffset : 0) + num7, (bottom + (flag2 ? page2.VertOffset : (((index == 0) && flag) ? 4 : 0))) + num8));
                        bottom += page2.GetBounds(true).Height + num8;
                        first += num6;
                        if (first >= displayCount)
                        {
                            break;
                        }
                        index++;
                    }
                    if ((endIndex == 0x7fffffff) && this.owner.DisplayLines.Loaded)
                    {
                        for (int i = this.Count - 1; i > index; i--)
                        {
                            this.list.RemoveAt(i);
                        }
                    }
                    if (changed)
                    {
                        this.DisplayRulers();
                    }
                }
                finally
                {
                    this.updateCount--;
                }
            }
        }

        protected void UpdatePageSize()
        {
            this.UpdatePageSize(false);
        }

        protected void UpdatePageSize(bool update)
        {
            if ((!this.initialized && (this.owner != null)) && ((update || (this.PageType != QWhale.Editor.PageType.Normal)) || (this.rulers != EditRulers.None)))
            {
                this.InitDefaultPageSettings(this.owner.Printing.PrinterSettings.DefaultPageSettings, true);
                this.initialized = true;
            }
        }

        protected void UpdateRulerAllowDrag()
        {
            if (this.horzRuler != null)
            {
                if ((this.pageType == QWhale.Editor.PageType.PageLayout) && ((this.rulerOptions & QWhale.Editor.RulerOptions.AllowDrag) != QWhale.Editor.RulerOptions.None))
                {
                    this.horzRuler.Options |= QWhale.Editor.RulerOptions.AllowDrag;
                }
                else
                {
                    this.horzRuler.Options &= ~QWhale.Editor.RulerOptions.AllowDrag;
                }
            }
            if (this.vertRuler != null)
            {
                if ((this.pageType == QWhale.Editor.PageType.PageLayout) && ((this.rulerOptions & QWhale.Editor.RulerOptions.AllowDrag) != QWhale.Editor.RulerOptions.None))
                {
                    this.vertRuler.Options |= QWhale.Editor.RulerOptions.AllowDrag;
                }
                else
                {
                    this.vertRuler.Options &= ~QWhale.Editor.RulerOptions.AllowDrag;
                }
            }
        }

        protected void UpdateRulerSize()
        {
            Rectangle clientRectangle = this.owner.ClientRectangle;
            if (((this.rulers & EditRulers.Horizonal) != EditRulers.None) && ((this.rulers & EditRulers.Vertical) != EditRulers.None))
            {
                this.VertRuler.Top = this.HorzRuler.Height;
                this.VertRuler.Height = clientRectangle.Height - this.VertRuler.Top;
                this.HorzRuler.Left = this.VertRuler.Width;
                this.HorzRuler.Width = clientRectangle.Width - this.HorzRuler.Left;
            }
            else if ((this.rulers & EditRulers.Horizonal) != EditRulers.None)
            {
                this.HorzRuler.Left = 0;
                this.HorzRuler.Width = clientRectangle.Width;
            }
            else if ((this.rulers & EditRulers.Vertical) != EditRulers.None)
            {
                this.VertRuler.Top = 0;
                this.VertRuler.Height = clientRectangle.Height;
            }
        }

        [DefaultValue(false), Description("Gets or sets a boolean value that indicates that changing of rulers indentation applies to all pages rather than to current page.")]
        public virtual bool ApplyRulerToAllPages
        {
            get
            {
                return this.applyRulerToAllPages;
            }
            set
            {
                if (this.applyRulerToAllPages != value)
                {
                    this.applyRulerToAllPages = value;
                    this.OnApplyRulerToAllPagesChanged();
                }
            }
        }

        [Description("Gets or sets a background color of each page in the collection.")]
        public virtual Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    this.OnBackColorChanged();
                }
            }
        }

        [Description("Gets or sets a border color of each page in the collection.")]
        public virtual Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                if (this.borderColor != value)
                {
                    this.borderColor = value;
                    this.OnBorderColorChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Size Caps
        {
            get
            {
                return this.caps;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool DefaultLandscape
        {
            get
            {
                return this.defaultLandscape;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Margins DefaultMargins
        {
            get
            {
                return this.defaultMargins;
            }
        }

        [Description("Gets or sets properties for default \"IEditPage\" object in the pages collection."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual IEditPage DefaultPage
        {
            get
            {
                if (this.Count == 0)
                {
                    this.Add();
                }
                return this[0];
            }
            set
            {
                this.DefaultPage.Assign(value);
                this.OnDefaultPageChanged();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual PaperKind DefaultPageKind
        {
            get
            {
                return this.defaultPageKind;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Size DefaultPageSize
        {
            get
            {
                return this.defaultPageSize;
            }
        }

        [DefaultValue(true), Description("Gets or sets a boolean value that indicates whether Edit control should draw whitespace area between pages.")]
        public virtual bool DisplayWhiteSpace
        {
            get
            {
                return this.displayWhiteSpace;
            }
            set
            {
                if (this.displayWhiteSpace != value)
                {
                    this.displayWhiteSpace = value;
                    this.OnDisplayWhiteSpaceChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int Height
        {
            get
            {
                IEditPage page = (this.Count > 0) ? this[this.Count - 1] : this.DefaultPage;
                return ((page.Origin.Y + page.BoundsRect.Height) + page.VertOffset);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEditRuler HorzRuler
        {
            get
            {
                if (this.horzRuler == null)
                {
                    this.horzRuler = new EditRuler();
                    this.horzRuler.Visible = false;
                    this.horzRuler.Parent = (Control) this.owner;
                    this.horzRuler.SendToBack();
                    this.horzRuler.Location = new Point(0, 0);
                    this.horzRuler.Width = this.owner.ClientRect.Width;
                    this.horzRuler.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                    this.horzRuler.Change += new EventHandler(this.RulerChanged);
                    this.UpdateRulerAllowDrag();
                }
                return this.horzRuler;
            }
        }

        [Browsable(false)]
        public virtual IEditPage this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                this.list[index] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IList<IEditPage> List
        {
            get
            {
                return this.list;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISyntaxEdit Owner
        {
            get
            {
                return this.owner;
            }
        }

        [Description("Gets or sets kind of the pages specifying standart paper size.")]
        public virtual PaperKind PageKind
        {
            get
            {
                return this.DefaultPage.PageKind;
            }
            set
            {
                this.UpdatePageSize(true);
                Size size = new Size(0, 0);
                if (this.owner != null)
                {
                    foreach (PaperSize size2 in this.owner.Printing.PrinterSettings.PaperSizes)
                    {
                        if (size2.Kind == value)
                        {
                            size = new Size(size2.Width, size2.Height);
                            break;
                        }
                    }
                }
                if (!size.IsEmpty)
                {
                    this.BeginUpdate();
                    try
                    {
                        foreach (IEditPage page in (IEnumerable<IEditPage>) this)
                        {
                            page.PageKind = value;
                            page.PageSize = size;
                        }
                    }
                    finally
                    {
                        this.EndUpdate();
                    }
                }
            }
        }

        [DefaultValue(0), Description("Gets or sets value specifying the way of viewing Edit control's content.")]
        public virtual QWhale.Editor.PageType PageType
        {
            get
            {
                return this.pageType;
            }
            set
            {
                if (this.pageType != value)
                {
                    this.pageType = value;
                    this.OnPageTypeChanged();
                }
            }
        }

        [Description("Gets or sets background color of the pages rulers.")]
        public virtual Color RulerBackColor
        {
            get
            {
                if (this.horzRuler != null)
                {
                    return this.horzRuler.BackColor;
                }
                if (this.vertRuler != null)
                {
                    return this.vertRuler.BackColor;
                }
                return EditConsts.DefaultRulerBackColor;
            }
            set
            {
                if ((this.HorzRuler.BackColor != value) || (this.VertRuler.BackColor != value))
                {
                    this.HorzRuler.BackColor = value;
                    this.VertRuler.BackColor = value;
                    this.OnRulerBackColorChanged();
                }
            }
        }

        [Description("Gets or sets background color of the indentations parts of the pages rulers.")]
        public Color RulerIndentBackColor
        {
            get
            {
                if (this.horzRuler != null)
                {
                    return this.horzRuler.IndentBackColor;
                }
                if (this.vertRuler != null)
                {
                    return this.vertRuler.IndentBackColor;
                }
                return EditConsts.DefaultRulerIndentBackColor;
            }
            set
            {
                if ((this.HorzRuler.IndentBackColor != value) || (this.VertRuler.IndentBackColor != value))
                {
                    this.HorzRuler.IndentBackColor = value;
                    this.VertRuler.IndentBackColor = value;
                    this.OnRulerIndentBackColorChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets options of the pages rulers.")]
        public virtual QWhale.Editor.RulerOptions RulerOptions
        {
            get
            {
                return this.rulerOptions;
            }
            set
            {
                if (this.rulerOptions != value)
                {
                    this.rulerOptions = value;
                    this.OnRulerOptionsChanged();
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor)), Description("Gets or sets the rulers displayed withing Edit control."), DefaultValue(0)]
        public virtual EditRulers Rulers
        {
            get
            {
                return this.rulers;
            }
            set
            {
                if (this.rulers != value)
                {
                    this.rulers = value;
                    this.OnRulersChanged();
                }
            }
        }

        [Description("Gets or sets measurement units of the pages rulers."), DefaultValue(1)]
        public virtual QWhale.Editor.RulerUnits RulerUnits
        {
            get
            {
                return this.rulerUnits;
            }
            set
            {
                if (this.rulerUnits != value)
                {
                    this.rulerUnits = value;
                    this.OnRulerUnitsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlEditPagesInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets or sets a boolean value that indicates whether Edit control should draw background around the pages."), DefaultValue(false)]
        public virtual bool Transparent
        {
            get
            {
                return this.transparent;
            }
            set
            {
                if (this.transparent != value)
                {
                    this.transparent = value;
                    this.OnTransparentChanged();
                }
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual IEditRuler VertRuler
        {
            get
            {
                if (this.vertRuler == null)
                {
                    this.vertRuler = new EditRuler();
                    this.vertRuler.Vertical = true;
                    this.vertRuler.Visible = false;
                    this.vertRuler.Parent = (Control) this.owner;
                    this.vertRuler.SendToBack();
                    this.vertRuler.Change += new EventHandler(this.RulerChanged);
                    this.vertRuler.Location = new Point(0, 0);
                    this.vertRuler.Height = this.owner.ClientRect.Height;
                    this.vertRuler.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                    this.UpdateRulerAllowDrag();
                }
                return this.vertRuler;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int Width
        {
            get
            {
                return (this.DefaultPage.BoundsRect.Width + (this.DefaultPage.HorzOffset * 2));
            }
        }

        private class PageComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                Rectangle pageRect = ((IEditPage) x).PageRect;
                int num = (int) y;
                if (pageRect.Top > num)
                {
                    return 1;
                }
                if (pageRect.Bottom <= num)
                {
                    return -1;
                }
                return 0;
            }
        }

        private class PageTextComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                int num = (int) y;
                IEditPage page = (IEditPage) x;
                if (page.StartLine > num)
                {
                    return 1;
                }
                if (page.EndLine < num)
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}

