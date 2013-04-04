namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class XmlGutterInfo : ISerializationInfo
    {
        private string backColor;
        private int bookMarkImageIndex;
        private bool drawLineBookmarks;
        private string lineBookmarksColor;
        private string lineModificatorChangedColor;
        private string lineModificatorSavedColor;
        private StringAlignment lineNumbersAlignment;
        private string lineNumbersBackColor;
        private string lineNumbersForeColor;
        private int lineNumbersLeftIndent;
        private int lineNumbersRightIndent;
        private int lineNumbersStart;
        private GutterOptions options;
        private int outliningLeftIndent;
        private int outliningRightIndent;
        private IGutter owner;
        private string penColor;
        private float penWidth;
        private bool showBookmarkHints;
        private string userMarginBackColor;
        private string userMarginForeColor;
        private string userMarginText;
        private int userMarginWidth;
        private bool visible;
        private int width;
        private int wrapImageIndex;

        public XmlGutterInfo()
        {
            this.width = EditConsts.DefaultGutterWidth;
            this.backColor = XmlColorInfo.SerializeColor(EditConsts.DefaultGutterBackColor);
            this.penColor = XmlColorInfo.SerializeColor(EditConsts.DefaultGutterForeColor);
            this.penWidth = 1f;
            this.visible = true;
            this.showBookmarkHints = true;
            this.lineNumbersStart = EditConsts.DefaultLineNumbersStart;
            this.lineNumbersLeftIndent = EditConsts.DefaultLineNumbersIndent;
            this.lineNumbersRightIndent = EditConsts.DefaultLineNumbersIndent;
            this.lineNumbersForeColor = XmlColorInfo.SerializeColor(Consts.DefaultControlForeColor);
            this.lineNumbersBackColor = XmlColorInfo.SerializeColor(Consts.DefaultControlBackColor);
            this.lineBookmarksColor = XmlColorInfo.SerializeColor(EditConsts.DefaultLineBookmarksColor);
            this.lineModificatorChangedColor = XmlColorInfo.SerializeColor(EditConsts.DefaultLineModificatorChangedColor);
            this.lineModificatorSavedColor = XmlColorInfo.SerializeColor(EditConsts.DefaultLineModificatorSavedColor);
            this.userMarginForeColor = XmlColorInfo.SerializeColor(EditConsts.DefaultUserMarginForeColor);
            this.userMarginBackColor = XmlColorInfo.SerializeColor(EditConsts.DefaultUserMarginBackColor);
            this.userMarginText = EditConsts.DefaultUserMarginText;
            this.userMarginWidth = EditConsts.DefaultUserMarginWidth;
            this.options = EditConsts.DefaultGutterOptions;
            this.bookMarkImageIndex = EditConsts.DefaultBookMarkImageIndex;
            this.wrapImageIndex = EditConsts.DefaultWrapImageIndex;
            this.outliningLeftIndent = EditConsts.DefaultOutliningIndent;
            this.outliningRightIndent = EditConsts.DefaultOutliningIndent;
        }

        public XmlGutterInfo(IGutter owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IGutter) owner;
            this.Width = this.width;
            this.BackColor = this.backColor;
            this.PenColor = this.penColor;
            this.PenWidth = this.penWidth;
            this.Visible = this.visible;
            this.LineNumbersStart = this.lineNumbersStart;
            this.LineNumbersLeftIndent = this.lineNumbersLeftIndent;
            this.LineNumbersRightIndent = this.lineNumbersRightIndent;
            this.LineNumbersForeColor = this.lineNumbersForeColor;
            this.LineNumbersBackColor = this.lineNumbersBackColor;
            this.LineNumbersAlignment = this.lineNumbersAlignment;
            this.Options = this.options;
            this.BookMarkImageIndex = this.bookMarkImageIndex;
            this.WrapImageIndex = this.wrapImageIndex;
            this.DrawLineBookmarks = this.drawLineBookmarks;
            this.ShowBookmarkHints = this.showBookmarkHints;
            this.LineBookmarksColor = this.lineBookmarksColor;
            this.LineModificatorChangedColor = this.lineModificatorChangedColor;
            this.LineModificatorSavedColor = this.lineModificatorSavedColor;
            this.UserMarginForeColor = this.userMarginForeColor;
            this.UserMarginBackColor = this.userMarginBackColor;
            this.UserMarginText = this.userMarginText;
            this.UserMarginWidth = this.userMarginWidth;
            this.OutliningLeftIndent = this.outliningLeftIndent;
            this.OutliningRightIndent = this.outliningRightIndent;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.width = this.Width;
                this.backColor = this.BackColor;
                this.penColor = this.PenColor;
                this.penWidth = this.PenWidth;
                this.visible = this.Visible;
                this.lineNumbersStart = this.LineNumbersStart;
                this.lineNumbersLeftIndent = this.LineNumbersLeftIndent;
                this.lineNumbersRightIndent = this.LineNumbersRightIndent;
                this.lineNumbersForeColor = this.LineNumbersForeColor;
                this.lineNumbersBackColor = this.LineNumbersBackColor;
                this.lineNumbersAlignment = this.LineNumbersAlignment;
                this.options = this.Options;
                this.bookMarkImageIndex = this.BookMarkImageIndex;
                this.wrapImageIndex = this.WrapImageIndex;
                this.drawLineBookmarks = this.DrawLineBookmarks;
                this.showBookmarkHints = this.ShowBookmarkHints;
                this.lineBookmarksColor = this.LineBookmarksColor;
                this.lineModificatorChangedColor = this.LineModificatorChangedColor;
                this.lineModificatorSavedColor = this.LineModificatorSavedColor;
                this.userMarginForeColor = this.UserMarginForeColor;
                this.userMarginBackColor = this.UserMarginBackColor;
                this.userMarginText = this.UserMarginText;
                this.userMarginWidth = this.UserMarginWidth;
                this.outliningLeftIndent = this.OutliningLeftIndent;
                this.outliningRightIndent = this.OutliningRightIndent;
            }
        }

        public bool ShouldSerializeBackColor()
        {
            return (this.BackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultGutterBackColor));
        }

        public bool ShouldSerializeBookMarkImageIndex()
        {
            return (this.BookMarkImageIndex != EditConsts.DefaultBookMarkImageIndex);
        }

        public bool ShouldSerializeLineBookmarksColor()
        {
            return (this.LineBookmarksColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineBookmarksColor));
        }

        public bool ShouldSerializeLineModificatorChangedColor()
        {
            return (this.LineModificatorChangedColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineModificatorChangedColor));
        }

        public virtual bool ShouldSerializeLineModificatorSavedColor()
        {
            return (this.LineModificatorSavedColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineModificatorSavedColor));
        }

        public bool ShouldSerializeLineNumbersBackColor()
        {
            return (this.LineNumbersBackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineNumbersBackColor));
        }

        public bool ShouldSerializeLineNumbersForeColor()
        {
            return (this.LineNumbersForeColor != XmlColorInfo.SerializeColor(EditConsts.DefaultLineNumbersForeColor));
        }

        public bool ShouldSerializeLineNumbersLeftIndent()
        {
            return (this.LineNumbersLeftIndent != EditConsts.DefaultLineNumbersIndent);
        }

        public bool ShouldSerializeLineNumbersRightIndent()
        {
            return (this.LineNumbersRightIndent != EditConsts.DefaultLineNumbersIndent);
        }

        public bool ShouldSerializeLineNumbersStart()
        {
            return (this.LineNumbersStart != EditConsts.DefaultLineNumbersStart);
        }

        public bool ShouldSerializeOptions()
        {
            return (this.Options != EditConsts.DefaultGutterOptions);
        }

        public bool ShouldSerializeOutliningLeftIndent()
        {
            return (this.OutliningLeftIndent != EditConsts.DefaultOutliningIndent);
        }

        public bool ShouldSerializeOutliningRightIndent()
        {
            return (this.OutliningRightIndent != EditConsts.DefaultOutliningIndent);
        }

        public bool ShouldSerializePenColor()
        {
            return (this.PenColor != XmlColorInfo.SerializeColor(EditConsts.DefaultGutterForeColor));
        }

        public bool ShouldSerializeUserMarginBackColor()
        {
            return (this.UserMarginBackColor != XmlColorInfo.SerializeColor(EditConsts.DefaultUserMarginBackColor));
        }

        public bool ShouldSerializeUserMarginForeColor()
        {
            return (this.UserMarginForeColor != XmlColorInfo.SerializeColor(EditConsts.DefaultUserMarginForeColor));
        }

        public bool ShouldSerializeUserMarginText()
        {
            return (this.UserMarginText != EditConsts.DefaultUserMarginText);
        }

        public bool ShouldSerializeUserMarginWidth()
        {
            return (this.UserMarginWidth != EditConsts.DefaultUserMarginWidth);
        }

        public bool ShouldSerializeWidth()
        {
            return (this.Width != EditConsts.DefaultGutterWidth);
        }

        public bool ShouldSerializeWrapImageIndex()
        {
            return (this.WrapImageIndex != EditConsts.DefaultWrapImageIndex);
        }

        public string BackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.backColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.BrushColor);
            }
            set
            {
                this.backColor = value;
                if (this.owner != null)
                {
                    this.owner.BrushColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public int BookMarkImageIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.bookMarkImageIndex;
                }
                return this.owner.BookMarkImageIndex;
            }
            set
            {
                this.bookMarkImageIndex = value;
                if (this.owner != null)
                {
                    this.owner.BookMarkImageIndex = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool DrawLineBookmarks
        {
            get
            {
                if (this.owner == null)
                {
                    return this.drawLineBookmarks;
                }
                return this.owner.DrawLineBookmarks;
            }
            set
            {
                this.drawLineBookmarks = value;
                if (this.owner != null)
                {
                    this.owner.DrawLineBookmarks = value;
                }
            }
        }

        public string LineBookmarksColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineBookmarksColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.LineBookmarksColor);
            }
            set
            {
                this.lineBookmarksColor = value;
                if (this.owner != null)
                {
                    this.owner.LineBookmarksColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string LineModificatorChangedColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineModificatorChangedColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.LineModificatorChangedColor);
            }
            set
            {
                this.lineModificatorChangedColor = value;
                if (this.owner != null)
                {
                    this.owner.LineModificatorChangedColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string LineModificatorSavedColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineModificatorSavedColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.LineModificatorSavedColor);
            }
            set
            {
                this.lineModificatorSavedColor = value;
                if (this.owner != null)
                {
                    this.owner.LineModificatorSavedColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        [DefaultValue(0)]
        public StringAlignment LineNumbersAlignment
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersAlignment;
                }
                return this.owner.LineNumbersAlignment;
            }
            set
            {
                this.lineNumbersAlignment = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersAlignment = value;
                }
            }
        }

        public string LineNumbersBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersBackColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.LineNumbersBackColor);
            }
            set
            {
                this.lineNumbersBackColor = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersBackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string LineNumbersForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersForeColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.LineNumbersForeColor);
            }
            set
            {
                this.lineNumbersForeColor = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersForeColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public int LineNumbersLeftIndent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersLeftIndent;
                }
                return this.owner.LineNumbersLeftIndent;
            }
            set
            {
                this.lineNumbersLeftIndent = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersLeftIndent = value;
                }
            }
        }

        public int LineNumbersRightIndent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersRightIndent;
                }
                return this.owner.LineNumbersRightIndent;
            }
            set
            {
                this.lineNumbersRightIndent = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersRightIndent = value;
                }
            }
        }

        public int LineNumbersStart
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineNumbersStart;
                }
                return this.owner.LineNumbersStart;
            }
            set
            {
                this.lineNumbersStart = value;
                if (this.owner != null)
                {
                    this.owner.LineNumbersStart = value;
                }
            }
        }

        public GutterOptions Options
        {
            get
            {
                if (this.owner == null)
                {
                    return this.options;
                }
                return this.owner.Options;
            }
            set
            {
                this.options = value;
                if (this.owner != null)
                {
                    this.owner.Options = value;
                }
            }
        }

        public int OutliningLeftIndent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outliningLeftIndent;
                }
                return this.owner.OutliningLeftIndent;
            }
            set
            {
                this.outliningLeftIndent = value;
                if (this.owner != null)
                {
                    this.owner.OutliningLeftIndent = value;
                }
            }
        }

        public int OutliningRightIndent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outliningRightIndent;
                }
                return this.owner.OutliningRightIndent;
            }
            set
            {
                this.outliningRightIndent = value;
                if (this.owner != null)
                {
                    this.owner.OutliningRightIndent = value;
                }
            }
        }

        public string PenColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.Pen.Color);
            }
            set
            {
                this.penColor = value;
                if (this.owner != null)
                {
                    this.owner.Pen.Color = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        [DefaultValue(1)]
        public float PenWidth
        {
            get
            {
                if (this.owner == null)
                {
                    return this.penWidth;
                }
                return this.owner.Pen.Width;
            }
            set
            {
                this.penWidth = value;
                if (this.owner != null)
                {
                    this.owner.Pen.Width = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowBookmarkHints
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showBookmarkHints;
                }
                return this.owner.ShowBookmarkHints;
            }
            set
            {
                this.showBookmarkHints = value;
                if (this.owner != null)
                {
                    this.owner.ShowBookmarkHints = value;
                }
            }
        }

        public string UserMarginBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.userMarginBackColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.UserMarginBackColor);
            }
            set
            {
                this.userMarginBackColor = value;
                if (this.owner != null)
                {
                    this.owner.UserMarginBackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string UserMarginForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.userMarginForeColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.UserMarginForeColor);
            }
            set
            {
                this.userMarginForeColor = value;
                if (this.owner != null)
                {
                    this.owner.UserMarginForeColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string UserMarginText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.userMarginText;
                }
                return this.owner.UserMarginText;
            }
            set
            {
                this.userMarginText = value;
                if (this.owner != null)
                {
                    this.owner.UserMarginText = value;
                }
            }
        }

        public int UserMarginWidth
        {
            get
            {
                if (this.owner == null)
                {
                    return this.userMarginWidth;
                }
                return this.owner.UserMarginWidth;
            }
            set
            {
                this.userMarginWidth = value;
                if (this.owner != null)
                {
                    this.owner.UserMarginWidth = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                if (this.owner == null)
                {
                    return this.visible;
                }
                return this.owner.Visible;
            }
            set
            {
                this.visible = value;
                if (this.owner != null)
                {
                    this.owner.Visible = value;
                }
            }
        }

        public int Width
        {
            get
            {
                if (this.owner == null)
                {
                    return this.width;
                }
                return this.owner.Width;
            }
            set
            {
                this.width = value;
                if (this.owner != null)
                {
                    this.owner.Width = value;
                }
            }
        }

        public int WrapImageIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wrapImageIndex;
                }
                return this.owner.WrapImageIndex;
            }
            set
            {
                this.wrapImageIndex = value;
                if (this.owner != null)
                {
                    this.owner.WrapImageIndex = value;
                }
            }
        }
    }
}

