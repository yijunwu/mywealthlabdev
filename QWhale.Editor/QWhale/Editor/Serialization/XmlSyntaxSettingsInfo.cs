namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.Dialogs;
    using QWhale.Editor.TextSource;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class XmlSyntaxSettingsInfo : ISerializationInfo
    {
        private int activeThemeIndex;
        private bool allowOutlining;
        private XmlColorThemesInfo colorThemes;
        private IKeyData[] eventData;
        private System.Drawing.Font font;
        private string fontName;
        private float fontSize;
        private System.Drawing.FontStyle fontStyle;
        private QWhale.Editor.GutterOptions gutterOptions;
        private int gutterWidth;
        private bool highlightHyperText;
        private int marginPos;
        private NavigateOptions navigateOptions;
        private QWhale.Editor.OutlineOptions outlineOptions;
        private ISyntaxSettings owner;
        private RichTextBoxScrollBars scrollBars;
        private SelectionOptions selectionOptions;
        private bool showGutter;
        private bool showMargin;
        private int[] tabStops;
        private bool useSpaces;
        private bool wordWrap;

        public XmlSyntaxSettingsInfo()
        {
            this.font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
            this.fontName = FontFamily.GenericMonospace.Name;
            this.fontSize = 10f;
            this.navigateOptions = EditConsts.DefaultNavigateOptions;
            this.scrollBars = RichTextBoxScrollBars.Both;
            this.selectionOptions = EditConsts.DefaultSelectionOptions;
            this.gutterOptions = EditConsts.DefaultGutterOptions;
            this.outlineOptions = EditConsts.DefaultOutlineOptions;
            this.showGutter = true;
            this.showMargin = true;
            this.highlightHyperText = true;
            this.gutterWidth = EditConsts.DefaultGutterWidth;
            this.marginPos = EditConsts.DefaultMarginPosition;
            this.tabStops = new int[] { EditConsts.DefaultTabStop };
            this.eventData = new KeyData[0];
        }

        public XmlSyntaxSettingsInfo(ISyntaxSettings owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ISyntaxSettings) owner;
            this.NavOptions = this.navigateOptions;
            this.ScrollBars = this.scrollBars;
            this.SelOptions = this.selectionOptions;
            this.GutterOptions = this.gutterOptions;
            this.OutlineOptions = this.outlineOptions;
            this.ShowGutter = this.showGutter;
            this.ShowMargin = this.showMargin;
            this.HighlightHyperText = this.highlightHyperText;
            this.AllowOutlining = this.allowOutlining;
            this.UseSpaces = this.useSpaces;
            this.WordWrap = this.wordWrap;
            this.GutterWidth = this.gutterWidth;
            this.MarginPos = this.marginPos;
            this.TabStops = this.tabStops;
            this.ColorThemes = this.colorThemes;
            this.Font = new System.Drawing.Font(this.fontName, this.fontSize, this.fontStyle);
            this.ActiveThemeIndex = this.activeThemeIndex;
            this.EventData = this.eventData;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.fontName = this.Font.Name;
                this.fontSize = this.Font.Size;
                this.fontStyle = this.Font.Style;
                this.navigateOptions = this.NavOptions;
                this.scrollBars = this.ScrollBars;
                this.selectionOptions = this.SelOptions;
                this.gutterOptions = this.GutterOptions;
                this.outlineOptions = this.OutlineOptions;
                this.showGutter = this.ShowGutter;
                this.showMargin = this.ShowMargin;
                this.highlightHyperText = this.HighlightHyperText;
                this.allowOutlining = this.AllowOutlining;
                this.useSpaces = this.UseSpaces;
                this.wordWrap = this.WordWrap;
                this.gutterWidth = this.GutterWidth;
                this.marginPos = this.MarginPos;
                this.tabStops = this.TabStops;
                this.colorThemes = this.ColorThemes;
                this.activeThemeIndex = this.ActiveThemeIndex;
                this.eventData = this.EventData;
                foreach (XmlMacroKeyDataInfo info in this.eventData)
                {
                    info.Load();
                }
            }
        }

        public bool ShouldSerializeFontName()
        {
            return (this.FontName != FontFamily.GenericMonospace.Name);
        }

        public bool ShouldSerializeFontSize()
        {
            return (this.FontSize != 10.0);
        }

        public bool ShouldSerializeFontStyle()
        {
            return (this.FontStyle != System.Drawing.FontStyle.Regular);
        }

        public bool ShouldSerializeGutterOptions()
        {
            return (this.GutterOptions != EditConsts.DefaultGutterOptions);
        }

        public bool ShouldSerializeGutterWidth()
        {
            return (this.GutterWidth != EditConsts.DefaultGutterWidth);
        }

        public bool ShouldSerializeMarginPos()
        {
            return (this.MarginPos != EditConsts.DefaultMarginPosition);
        }

        public bool ShouldSerializeNavOptions()
        {
            return (this.NavOptions != EditConsts.DefaultNavigateOptions);
        }

        public bool ShouldSerializeOutlineOptions()
        {
            return (this.OutlineOptions != EditConsts.DefaultOutlineOptions);
        }

        public bool ShouldSerializeSelOptions()
        {
            return (this.SelOptions != EditConsts.DefaultSelectionOptions);
        }

        public bool ShouldSerializeTabStops()
        {
            if (this.TabStops.Length == 1)
            {
                return (this.TabStops[0] != EditConsts.DefaultTabStop);
            }
            return true;
        }

        public int ActiveThemeIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.ActiveThemeIndex;
                }
                return this.owner.ColorThemes.ActiveThemeIndex;
            }
            set
            {
                this.activeThemeIndex = value;
                if (this.owner != null)
                {
                    this.owner.ColorThemes.ActiveThemeIndex = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool AllowOutlining
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowOutlining;
                }
                return this.owner.AllowOutlining;
            }
            set
            {
                this.allowOutlining = value;
                if (this.owner != null)
                {
                    this.owner.AllowOutlining = value;
                }
            }
        }

        public XmlColorThemesInfo ColorThemes
        {
            get
            {
                if (this.owner == null)
                {
                    return this.colorThemes;
                }
                return (XmlColorThemesInfo) this.owner.ColorThemes.SerializationInfo;
            }
            set
            {
                this.colorThemes = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.ColorThemes.SerializationInfo = value;
                }
            }
        }

        [XmlIgnore]
        public IKeyData[] EventData
        {
            get
            {
                if (this.owner == null)
                {
                    return this.eventData;
                }
                return this.owner.EventData;
            }
            set
            {
                this.eventData = value;
                if ((this.owner != null) && (value.Length > 0))
                {
                    this.owner.EventData = value;
                }
            }
        }

        [XmlIgnore]
        public System.Drawing.Font Font
        {
            get
            {
                if (this.owner == null)
                {
                    return this.font;
                }
                return this.owner.Font;
            }
            set
            {
                this.font = value;
                if (this.owner != null)
                {
                    this.owner.Font = value;
                }
            }
        }

        public string FontName
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontName;
                }
                return this.owner.Font.Name;
            }
            set
            {
                this.fontName = value;
            }
        }

        public float FontSize
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontSize;
                }
                return this.owner.Font.Size;
            }
            set
            {
                this.fontSize = value;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fontStyle;
                }
                return this.owner.Font.Style;
            }
            set
            {
                this.fontStyle = value;
            }
        }

        public QWhale.Editor.GutterOptions GutterOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.gutterOptions;
                }
                return this.owner.GutterOptions;
            }
            set
            {
                this.gutterOptions = value;
                if (this.owner != null)
                {
                    this.owner.GutterOptions = value;
                }
            }
        }

        public int GutterWidth
        {
            get
            {
                if (this.owner == null)
                {
                    return this.gutterWidth;
                }
                return this.owner.GutterWidth;
            }
            set
            {
                this.gutterWidth = value;
                if (this.owner != null)
                {
                    this.owner.GutterWidth = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool HighlightHyperText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.highlightHyperText;
                }
                return this.owner.HighlightHyperText;
            }
            set
            {
                this.highlightHyperText = value;
                if (this.owner != null)
                {
                    this.owner.HighlightHyperText = value;
                }
            }
        }

        public int MarginPos
        {
            get
            {
                if (this.owner == null)
                {
                    return this.marginPos;
                }
                return this.owner.MarginPos;
            }
            set
            {
                this.marginPos = value;
                if (this.owner != null)
                {
                    this.owner.MarginPos = value;
                }
            }
        }

        public NavigateOptions NavOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.navigateOptions;
                }
                return this.owner.NavigateOptions;
            }
            set
            {
                this.navigateOptions = value;
                if (this.owner != null)
                {
                    this.owner.NavigateOptions = value;
                }
            }
        }

        public QWhale.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outlineOptions;
                }
                return this.owner.OutlineOptions;
            }
            set
            {
                this.outlineOptions = value;
                if (this.owner != null)
                {
                    this.owner.OutlineOptions = value;
                }
            }
        }

        [DefaultValue(3)]
        public RichTextBoxScrollBars ScrollBars
        {
            get
            {
                if (this.owner == null)
                {
                    return this.scrollBars;
                }
                return this.owner.ScrollBars;
            }
            set
            {
                this.scrollBars = value;
                if (this.owner != null)
                {
                    this.owner.ScrollBars = value;
                }
            }
        }

        public SelectionOptions SelOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.selectionOptions;
                }
                return this.owner.SelectionOptions;
            }
            set
            {
                this.selectionOptions = value;
                if (this.owner != null)
                {
                    this.owner.SelectionOptions = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowGutter
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showGutter;
                }
                return this.owner.ShowGutter;
            }
            set
            {
                this.showGutter = value;
                if (this.owner != null)
                {
                    this.owner.ShowGutter = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowMargin
        {
            get
            {
                if (this.owner == null)
                {
                    return this.showMargin;
                }
                return this.owner.ShowMargin;
            }
            set
            {
                this.showMargin = value;
                if (this.owner != null)
                {
                    this.owner.ShowMargin = value;
                }
            }
        }

        public int[] TabStops
        {
            get
            {
                if (this.owner == null)
                {
                    return this.tabStops;
                }
                return this.owner.TabStops;
            }
            set
            {
                this.tabStops = value;
                if (this.owner != null)
                {
                    this.owner.TabStops = new int[value.Length];
                    Array.Copy(value, this.owner.TabStops, value.Length);
                }
            }
        }

        [DefaultValue(false)]
        public bool UseSpaces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.useSpaces;
                }
                return this.owner.UseSpaces;
            }
            set
            {
                this.useSpaces = value;
                if (this.owner != null)
                {
                    this.owner.UseSpaces = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool WordWrap
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wordWrap;
                }
                return this.owner.WordWrap;
            }
            set
            {
                this.wordWrap = value;
                if (this.owner != null)
                {
                    this.owner.WordWrap = value;
                }
            }
        }
    }
}

