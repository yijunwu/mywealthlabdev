namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public class PageHeader : IEditPageHeader, IUpdate
    {
        private string centerText;
        private System.Drawing.Font font;
        private Color fontColor;
        private string leftText;
        private Point offset;
        private ISyntaxEdit owner;
        private IEditPage page;
        private bool reverseOnEvenPages;
        private string rightText;
        private Regex textRegex;
        private int updateCount;
        private bool visible;

        public PageHeader()
        {
            this.leftText = string.Empty;
            this.rightText = string.Empty;
            this.centerText = string.Empty;
            this.font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
            this.fontColor = EditConsts.DefaultHeaderFontColor;
            this.offset = new Point(0x18, 8);
            this.visible = true;
        }

        public PageHeader(IEditPage page, ISyntaxEdit owner) : this()
        {
            this.page = page;
            this.owner = owner;
        }

        public virtual void Assign(IEditPageHeader source)
        {
            this.BeginUpdate();
            try
            {
                this.LeftText = source.LeftText;
                this.RightText = source.RightText;
                this.CenterText = source.CenterText;
                this.Offset = source.Offset;
                this.ReverseOnEvenPages = source.ReverseOnEvenPages;
                this.Font = new System.Drawing.Font(source.Font.FontFamily, source.Font.Size, source.Font.Style);
                this.FontColor = source.FontColor;
                this.Visible = source.Visible;
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

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
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

        ~PageHeader()
        {
            this.font.Dispose();
        }

        public virtual string GetTextToPaint(string text, int pageIndex, int pageCount)
        {
            MatchCollection matchs = this.TextRegex.Matches(text);
            for (int i = matchs.Count - 1; i >= 0; i--)
            {
                Match match = matchs[i];
                if (match.Success)
                {
                    string strA = text.Substring(match.Index, match.Length);
                    if (string.Compare(strA, EditConsts.PageTag) == 0)
                    {
                        strA = (pageIndex + 1).ToString();
                    }
                    if (string.Compare(strA, EditConsts.PagesTag) == 0)
                    {
                        strA = pageCount.ToString();
                    }
                    if (string.Compare(strA, EditConsts.DateTag) == 0)
                    {
                        strA = DateTime.Now.ToLongDateString();
                    }
                    if (string.Compare(strA, EditConsts.TimeTag) == 0)
                    {
                        strA = DateTime.Now.ToLongTimeString();
                    }
                    if (string.Compare(strA, EditConsts.UserTag) == 0)
                    {
                        strA = Environment.UserName;
                    }
                    else
                    {
                        IEditPages pages = this.Pages;
                        if (pages != null)
                        {
                            pages.OnDrawHeader(ref strA);
                        }
                    }
                    text = text.Remove(match.Index, match.Length);
                    text = text.Insert(match.Index, strA);
                }
            }
            return text;
        }

        protected virtual void OnCenterTextChanged()
        {
            this.Update();
        }

        protected virtual void OnFontChanged()
        {
            this.Update();
        }

        protected virtual void OnFontColorChanged()
        {
            this.Update();
        }

        protected virtual void OnLeftTextChanged()
        {
            this.Update();
        }

        protected virtual void OnOffsetChanged()
        {
            this.Update();
        }

        protected virtual void OnReverseOnEvenPagesChanged()
        {
            if (this.page != null)
            {
                this.page.Invalidate();
            }
        }

        protected virtual void OnRightTextChanged()
        {
            this.Update();
        }

        protected virtual void OnVisibleChanged()
        {
            this.Update();
        }

        public virtual void Paint(IPainter painter, Rectangle rect, int pageIndex, int pageCount, bool pageNumbers)
        {
            System.Drawing.Font font = painter.Font;
            Color textColor = painter.TextColor;
            painter.Opaque = false;
            try
            {
                painter.Font = this.Font;
                painter.TextColor = this.fontColor;
                IEditPages pages = this.Pages;
                IDrawInfo info = new DrawInfo {
                    Page = pageIndex
                };
                if ((this.owner == null) || !this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.PageHeader, info))
                {
                    bool flag = ((pages != null) && this.reverseOnEvenPages) && (((pageIndex + 1) % 2) == 0);
                    string str2 = pageNumbers ? ((pageIndex + 1)).ToString() : this.RightText;
                    string text = flag ? str2 : this.LeftText;
                    if ((text != null) && (text != string.Empty))
                    {
                        painter.TextOut(this.GetTextToPaint(text, pageIndex, pageCount), -1, rect);
                    }
                    text = this.CenterText;
                    if ((text != null) && (text != string.Empty))
                    {
                        text = this.GetTextToPaint(text, pageIndex, pageCount);
                        int num = painter.StringWidth(text);
                        painter.TextOut(text, -1, ((rect.Left + rect.Right) - num) >> 1, rect.Top);
                    }
                    text = flag ? this.LeftText : str2;
                    if ((text != null) && (text != string.Empty))
                    {
                        text = this.GetTextToPaint(text, pageIndex, pageCount);
                        int num2 = painter.StringWidth(text);
                        painter.TextOut(text, -1, rect.Right - num2, rect.Top);
                    }
                }
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.OnCustomDraw(painter, rect, DrawStage.After, DrawState.PageHeader, info);
                }
            }
            finally
            {
                painter.Opaque = true;
                painter.Font = font;
                painter.TextColor = textColor;
            }
        }

        public virtual void ResetFont()
        {
            this.Font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
        }

        public virtual void ResetFontColor()
        {
            this.FontColor = EditConsts.DefaultHeaderFontColor;
        }

        public virtual void ResetOffset()
        {
            this.Offset = new Point(8, 8);
        }

        public virtual void ResetReverseOnEvenPages()
        {
            this.ReverseOnEvenPages = false;
        }

        public bool ShouldSerializeFont()
        {
            if (!(this.font.FontFamily.Name != FontFamily.GenericMonospace.Name) && (this.font.Size == 10.0))
            {
                return (this.font.Style != EditConsts.DefaultHeaderFontStyle);
            }
            return true;
        }

        public bool ShouldSerializeFontColor()
        {
            return (this.fontColor != EditConsts.DefaultHeaderFontColor);
        }

        public bool ShouldSerializeOffset()
        {
            if (this.offset.X == 0x18)
            {
                return (this.offset.Y != 8);
            }
            return true;
        }

        public virtual void Update()
        {
            if ((this.updateCount == 0) && (this.page != null))
            {
                this.page.Invalidate();
            }
        }

        [Description("Gets or sets a string that appears at the center of the header/footer area."), DefaultValue("")]
        public virtual string CenterText
        {
            get
            {
                return this.centerText;
            }
            set
            {
                if (this.centerText != value)
                {
                    this.centerText = value;
                    this.OnCenterTextChanged();
                }
            }
        }

        [Description("Gets or sets font used to draw header/footer text.")]
        public virtual System.Drawing.Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                if (this.font != value)
                {
                    this.font = value;
                    this.OnFontChanged();
                }
            }
        }

        [Description("Gets or sets font color used to draw header/footer text.")]
        public virtual Color FontColor
        {
            get
            {
                return this.fontColor;
            }
            set
            {
                if (this.fontColor != value)
                {
                    this.fontColor = value;
                    this.OnFontColorChanged();
                }
            }
        }

        [Description("Gets or sets a string that appears at the left part of the header/footer area."), DefaultValue("")]
        public virtual string LeftText
        {
            get
            {
                return this.leftText;
            }
            set
            {
                if (this.leftText != value)
                {
                    this.leftText = value;
                    this.OnLeftTextChanged();
                }
            }
        }

        [Description("Gets or sets indentation of the primary text edge.")]
        public virtual Point Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                if (this.offset != value)
                {
                    this.offset = value;
                    this.OnOffsetChanged();
                }
            }
        }

        protected IEditPages Pages
        {
            get
            {
                if (this.page == null)
                {
                    return null;
                }
                return this.page.Pages;
            }
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether the \"LeftText\" and \"RightText\" interchanging on even page.")]
        public virtual bool ReverseOnEvenPages
        {
            get
            {
                return this.reverseOnEvenPages;
            }
            set
            {
                if (this.reverseOnEvenPages != value)
                {
                    this.reverseOnEvenPages = value;
                    this.OnReverseOnEvenPagesChanged();
                }
            }
        }

        [DefaultValue(""), Description("Gets or sets a string that appears at the right part of the header/footer area.")]
        public virtual string RightText
        {
            get
            {
                return this.rightText;
            }
            set
            {
                if (this.rightText != value)
                {
                    this.rightText = value;
                    this.OnRightTextChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlPageHeaderInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        protected Regex TextRegex
        {
            get
            {
                if (this.textRegex == null)
                {
                    this.textRegex = new Regex(@"\\\[[a-zA-Z_0-9]+\]", RegexOptions.Singleline);
                }
                return this.textRegex;
            }
        }

        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [DefaultValue(true), Description("Gets or sets a value indicating whether the \"PageHeader\" is visible.")]
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
    }
}

