namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Editor.TextSource.Serialization;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    [XmlRoot("SyntaxEdit")]
    public class XmlSyntaxEditInfo : ISerializationInfo
    {
        private bool acceptReturns;
        private bool acceptTabs;
        private bool autoCorrection;
        private EditBorderStyle borderStyle;
        private XmlBracesInfo braces;
        private char[] codeCompletionChars;
        private string columnsIndentForeColor;
        private System.Windows.Forms.ContextMenu contextMenu;
        private bool disableColorPaint;
        private string disabledBackColor;
        private string disabledForeColor;
        private bool disableSyntaxPaint;
        private XmlDisplayStringsInfo displayStrings;
        private bool drawColumnsIndent;
        private System.Drawing.Font font;
        private XmlGutterInfo gutter;
        private bool hideCaret;
        private XmlEditHyperTextInfo hyperText;
        private bool keepCaretOnLostFocus;
        private XmlLineSeparatorInfo lineSeparator;
        private XmlEditLineStylesInfo lineStyles;
        private XmlMarginInfo margin;
        private XmlOutliningInfo outlining;
        private ISyntaxEdit owner;
        private XmlEditPagesInfo pages;
        private XmlPrintingInfo printing;
        private string readonlyBackColor;
        private string readonlyForeColor;
        private XmlScrollingInfo scrolling;
        private QWhale.Editor.TextSource.SearchOptions searchOptions;
        private XmlSelectionInfo selection;
        private XmlEditSpellingInfo spelling;
        private bool syntaxErrorsHints;
        private XmlTextSourceInfo textSource;
        private bool transparent;
        private bool useDefaultMenu;
        private XmlWhiteSpaceInfo whiteSpace;

        public XmlSyntaxEditInfo()
        {
            this.acceptTabs = true;
            this.acceptReturns = true;
            this.searchOptions = EditConsts.DefaultSearchOptions;
            this.codeCompletionChars = EditConsts.DefaultCodeCompletionChars.ToCharArray();
            this.readonlyForeColor = string.Empty;
            this.readonlyBackColor = string.Empty;
            this.disabledForeColor = string.Empty;
            this.disabledBackColor = string.Empty;
            this.syntaxErrorsHints = true;
            this.useDefaultMenu = true;
            this.columnsIndentForeColor = XmlColorInfo.SerializeColor(EditConsts.DefaultColumnsIndentForeColor);
        }

        public XmlSyntaxEditInfo(ISyntaxEdit owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ISyntaxEdit) owner;
            this.HideCaret = this.hideCaret;
            this.KeepCaretOnLostFocus = this.keepCaretOnLostFocus;
            this.DisableColorPaint = this.disableColorPaint;
            this.DisableSyntaxPaint = this.disableSyntaxPaint;
            this.AcceptTabs = this.acceptTabs;
            this.AcceptReturns = this.acceptReturns;
            this.DisplayStrings = this.displayStrings;
            this.Selection = this.selection;
            this.Gutter = this.gutter;
            this.Margin = this.margin;
            this.LineStyles = this.lineStyles;
            this.LineSeparator = this.lineSeparator;
            this.Printing = this.printing;
            this.WhiteSpace = this.whiteSpace;
            this.TextSource = this.textSource;
            this.Scrolling = this.scrolling;
            this.Outlining = this.outlining;
            this.HyperText = this.hyperText;
            this.Spelling = this.spelling;
            this.Pages = this.pages;
            this.Braces = this.braces;
            this.Transparent = this.transparent;
            this.SearchOptions = this.searchOptions;
            this.CodeCompletionChars = this.codeCompletionChars;
            this.AutoCorrection = this.autoCorrection;
            this.ReadonlyForeColor = this.readonlyForeColor;
            this.ReadonlyBackColor = this.readonlyBackColor;
            this.DisabledForeColor = this.disabledForeColor;
            this.DisabledBackColor = this.disabledBackColor;
            this.SyntaxErrorsHints = this.syntaxErrorsHints;
            this.DrawColumnsIndent = this.drawColumnsIndent;
            this.UseDefaultMenu = this.useDefaultMenu;
            this.ContextMenu = this.contextMenu;
            this.Font = this.font;
            this.BorderStyle = this.borderStyle;
            this.ColumnsIndentForeColor = this.columnsIndentForeColor;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.hideCaret = this.HideCaret;
                this.keepCaretOnLostFocus = this.KeepCaretOnLostFocus;
                this.disableColorPaint = this.DisableColorPaint;
                this.disableSyntaxPaint = this.DisableSyntaxPaint;
                this.acceptTabs = this.AcceptTabs;
                this.acceptReturns = this.AcceptReturns;
                this.transparent = this.Transparent;
                this.searchOptions = this.SearchOptions;
                this.codeCompletionChars = this.CodeCompletionChars;
                this.autoCorrection = this.AutoCorrection;
                this.readonlyForeColor = this.ReadonlyForeColor;
                this.readonlyBackColor = this.ReadonlyBackColor;
                this.disabledForeColor = this.DisabledForeColor;
                this.disabledBackColor = this.DisabledBackColor;
                this.syntaxErrorsHints = this.SyntaxErrorsHints;
                this.displayStrings = this.DisplayStrings;
                this.selection = this.Selection;
                this.gutter = this.Gutter;
                this.margin = this.Margin;
                this.lineStyles = this.LineStyles;
                this.lineSeparator = this.LineSeparator;
                this.printing = this.Printing;
                this.whiteSpace = this.WhiteSpace;
                this.textSource = this.TextSource;
                this.scrolling = this.Scrolling;
                this.outlining = this.Outlining;
                this.hyperText = this.HyperText;
                this.spelling = this.Spelling;
                this.pages = this.Pages;
                this.braces = this.Braces;
                this.drawColumnsIndent = this.DrawColumnsIndent;
                this.useDefaultMenu = this.UseDefaultMenu;
                this.contextMenu = this.ContextMenu;
                this.font = this.Font;
                this.borderStyle = this.BorderStyle;
                this.columnsIndentForeColor = this.ColumnsIndentForeColor;
                this.displayStrings = this.DisplayStrings;
                this.selection = this.Selection;
                if (this.textSource != null)
                {
                    this.textSource.Load();
                }
                if (this.displayStrings != null)
                {
                    this.displayStrings.Load();
                }
                if (this.selection != null)
                {
                    this.selection.Load();
                }
                if (this.gutter != null)
                {
                    this.gutter.Load();
                }
                if (this.margin != null)
                {
                    this.margin.Load();
                }
                if (this.lineStyles != null)
                {
                    this.lineStyles.Load();
                }
                if (this.lineSeparator != null)
                {
                    this.lineSeparator.Load();
                }
                if (this.printing != null)
                {
                    this.printing.Load();
                }
                if (this.whiteSpace != null)
                {
                    this.whiteSpace.Load();
                }
                if (this.scrolling != null)
                {
                    this.scrolling.Load();
                }
                if (this.outlining != null)
                {
                    this.outlining.Load();
                }
                if (this.hyperText != null)
                {
                    this.hyperText.Load();
                }
                if (this.spelling != null)
                {
                    this.scrolling.Load();
                }
                if (this.pages != null)
                {
                    this.pages.Load();
                }
                if (this.braces != null)
                {
                    this.braces.Load();
                }
            }
        }

        public virtual bool ShouldSerializeBorderStyle()
        {
            return (this.BorderStyle != EditBorderStyle.Fixed3D);
        }

        public bool ShouldSerializeCodeCompletionChars()
        {
            return (new string(this.CodeCompletionChars) != EditConsts.DefaultCodeCompletionChars);
        }

        public bool ShouldSerializeColumnsIndentForeColor()
        {
            return (this.ColumnsIndentForeColor != XmlColorInfo.SerializeColor(EditConsts.DefaultColumnsIndentForeColor));
        }

        public bool ShouldSerializeDisabledBackColor()
        {
            return (this.DisabledBackColor != string.Empty);
        }

        public bool ShouldSerializeDisabledForeColor()
        {
            return (this.DisabledForeColor != string.Empty);
        }

        public bool ShouldSerializeReadonlyBackColor()
        {
            return (this.ReadonlyBackColor != string.Empty);
        }

        public bool ShouldSerializeReadonlyForeColor()
        {
            return (this.ReadonlyForeColor != string.Empty);
        }

        public bool ShouldSerializeSearchOptions()
        {
            return (this.SearchOptions != EditConsts.DefaultSearchOptions);
        }

        [DefaultValue(true)]
        public bool AcceptReturns
        {
            get
            {
                if (this.owner == null)
                {
                    return this.acceptReturns;
                }
                return this.owner.AcceptReturns;
            }
            set
            {
                this.acceptReturns = value;
                if (this.owner != null)
                {
                    this.owner.AcceptReturns = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool AcceptTabs
        {
            get
            {
                if (this.owner == null)
                {
                    return this.acceptTabs;
                }
                return this.owner.AcceptTabs;
            }
            set
            {
                this.acceptTabs = value;
                if (this.owner != null)
                {
                    this.owner.AcceptTabs = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool AutoCorrection
        {
            get
            {
                if (this.owner == null)
                {
                    return this.autoCorrection;
                }
                return this.owner.AutoCorrection;
            }
            set
            {
                this.autoCorrection = value;
                if (this.owner != null)
                {
                    this.owner.AutoCorrection = value;
                }
            }
        }

        public EditBorderStyle BorderStyle
        {
            get
            {
                if (this.owner == null)
                {
                    return this.borderStyle;
                }
                return this.owner.BorderStyle;
            }
            set
            {
                this.borderStyle = value;
                if (this.owner != null)
                {
                    this.owner.BorderStyle = value;
                }
            }
        }

        public XmlBracesInfo Braces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.braces;
                }
                return (XmlBracesInfo) this.owner.Braces.SerializationInfo;
            }
            set
            {
                this.braces = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Braces.SerializationInfo = value;
                }
            }
        }

        public char[] CodeCompletionChars
        {
            get
            {
                if (this.owner == null)
                {
                    return this.codeCompletionChars;
                }
                return this.owner.CodeCompletionChars;
            }
            set
            {
                this.codeCompletionChars = value;
                if (this.owner != null)
                {
                    this.owner.CodeCompletionChars = value;
                }
            }
        }

        public string ColumnsIndentForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.columnsIndentForeColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.SyntaxPaint.ColumnsIndentForeColor);
            }
            set
            {
                this.columnsIndentForeColor = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.ColumnsIndentForeColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        [XmlIgnore]
        public System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            {
                if (this.owner == null)
                {
                    return this.contextMenu;
                }
                return this.owner.ContextMenu;
            }
            set
            {
                this.contextMenu = value;
                if (this.owner != null)
                {
                    this.owner.ContextMenu = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool DisableColorPaint
        {
            get
            {
                if (this.owner == null)
                {
                    return this.disableColorPaint;
                }
                return this.owner.SyntaxPaint.DisableColorPaint;
            }
            set
            {
                this.disableColorPaint = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.DisableColorPaint = value;
                }
            }
        }

        public string DisabledBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.disabledBackColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.SyntaxPaint.DisabledBackColor);
            }
            set
            {
                this.disabledBackColor = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.DisabledBackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string DisabledForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.disabledForeColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.SyntaxPaint.DisabledForeColor);
            }
            set
            {
                this.disabledForeColor = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.DisabledForeColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        [DefaultValue(false)]
        public bool DisableSyntaxPaint
        {
            get
            {
                if (this.owner == null)
                {
                    return this.disableSyntaxPaint;
                }
                return this.owner.SyntaxPaint.DisableSyntaxPaint;
            }
            set
            {
                this.disableSyntaxPaint = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.DisableSyntaxPaint = value;
                }
            }
        }

        public XmlDisplayStringsInfo DisplayStrings
        {
            get
            {
                if (this.owner == null)
                {
                    return this.displayStrings;
                }
                return (XmlDisplayStringsInfo) this.owner.DisplayLines.SerializationInfo;
            }
            set
            {
                this.displayStrings = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.DisplayLines.SerializationInfo = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool DrawColumnsIndent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.drawColumnsIndent;
                }
                return this.owner.SyntaxPaint.DrawColumnsIndent;
            }
            set
            {
                this.drawColumnsIndent = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.DrawColumnsIndent = value;
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
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Font = value;
                }
            }
        }

        public XmlGutterInfo Gutter
        {
            get
            {
                if (this.owner == null)
                {
                    return this.gutter;
                }
                return (XmlGutterInfo) this.owner.Gutter.SerializationInfo;
            }
            set
            {
                this.gutter = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Gutter.SerializationInfo = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool HideCaret
        {
            get
            {
                if (this.owner == null)
                {
                    return this.hideCaret;
                }
                return this.owner.HideCaret;
            }
            set
            {
                this.hideCaret = value;
                if (this.owner != null)
                {
                    this.owner.HideCaret = value;
                }
            }
        }

        public XmlEditHyperTextInfo HyperText
        {
            get
            {
                if (this.owner == null)
                {
                    return this.hyperText;
                }
                return (XmlEditHyperTextInfo) this.owner.HyperText.SerializationInfo;
            }
            set
            {
                this.hyperText = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.HyperText.SerializationInfo = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool KeepCaretOnLostFocus
        {
            get
            {
                if (this.owner == null)
                {
                    return this.keepCaretOnLostFocus;
                }
                return this.owner.KeepCaretOnLostFocus;
            }
            set
            {
                this.keepCaretOnLostFocus = value;
                if (this.owner != null)
                {
                    this.owner.KeepCaretOnLostFocus = value;
                }
            }
        }

        public XmlLineSeparatorInfo LineSeparator
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineSeparator;
                }
                return (XmlLineSeparatorInfo) this.owner.LineSeparator.SerializationInfo;
            }
            set
            {
                this.lineSeparator = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.LineSeparator.SerializationInfo = value;
                }
            }
        }

        public XmlEditLineStylesInfo LineStyles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineStyles;
                }
                return (XmlEditLineStylesInfo) this.owner.LineStyles.SerializationInfo;
            }
            set
            {
                this.lineStyles = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.LineStyles.SerializationInfo = value;
                }
            }
        }

        public XmlMarginInfo Margin
        {
            get
            {
                if (this.owner == null)
                {
                    return this.margin;
                }
                return (XmlMarginInfo) this.owner.EditMargin.SerializationInfo;
            }
            set
            {
                this.margin = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.EditMargin.SerializationInfo = value;
                }
            }
        }

        public XmlOutliningInfo Outlining
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outlining;
                }
                return (XmlOutliningInfo) this.owner.Outlining.SerializationInfo;
            }
            set
            {
                this.outlining = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Outlining.SerializationInfo = value;
                }
            }
        }

        public XmlEditPagesInfo Pages
        {
            get
            {
                if (this.owner == null)
                {
                    return this.Pages;
                }
                return (XmlEditPagesInfo) this.owner.Pages.SerializationInfo;
            }
            set
            {
                this.pages = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Pages.SerializationInfo = value;
                }
            }
        }

        public XmlPrintingInfo Printing
        {
            get
            {
                if (this.owner == null)
                {
                    return this.printing;
                }
                return (XmlPrintingInfo) this.owner.Printing.SerializationInfo;
            }
            set
            {
                this.printing = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Printing.SerializationInfo = value;
                }
            }
        }

        public string ReadonlyBackColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.readonlyBackColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.SyntaxPaint.ReadonlyBackColor);
            }
            set
            {
                this.readonlyBackColor = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.ReadonlyBackColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public string ReadonlyForeColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.readonlyForeColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.SyntaxPaint.ReadonlyForeColor);
            }
            set
            {
                this.readonlyForeColor = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.ReadonlyForeColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public XmlScrollingInfo Scrolling
        {
            get
            {
                if (this.owner == null)
                {
                    return this.scrolling;
                }
                return (XmlScrollingInfo) this.owner.Scrolling.SerializationInfo;
            }
            set
            {
                this.scrolling = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Scrolling.SerializationInfo = value;
                }
            }
        }

        public QWhale.Editor.TextSource.SearchOptions SearchOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.searchOptions;
                }
                return this.owner.SearchOptions;
            }
            set
            {
                this.searchOptions = value;
                if (this.owner != null)
                {
                    this.owner.SearchOptions = value;
                }
            }
        }

        public XmlSelectionInfo Selection
        {
            get
            {
                if (this.owner == null)
                {
                    return this.selection;
                }
                return (XmlSelectionInfo) this.owner.Selection.SerializationInfo;
            }
            set
            {
                this.selection = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Selection.SerializationInfo = value;
                }
            }
        }

        public XmlEditSpellingInfo Spelling
        {
            get
            {
                if (this.owner == null)
                {
                    return this.spelling;
                }
                return (XmlEditSpellingInfo) this.owner.Spelling.SerializationInfo;
            }
            set
            {
                this.spelling = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Spelling.SerializationInfo = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool SyntaxErrorsHints
        {
            get
            {
                if (this.owner == null)
                {
                    return this.syntaxErrorsHints;
                }
                return this.owner.SyntaxPaint.SyntaxErrorsHints;
            }
            set
            {
                this.syntaxErrorsHints = value;
                if (this.owner != null)
                {
                    this.owner.SyntaxPaint.SyntaxErrorsHints = value;
                }
            }
        }

        public XmlTextSourceInfo TextSource
        {
            get
            {
                if (this.owner == null)
                {
                    return this.textSource;
                }
                return (XmlTextSourceInfo) this.owner.Source.SerializationInfo;
            }
            set
            {
                this.textSource = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Source.SerializationInfo = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool Transparent
        {
            get
            {
                if (this.owner == null)
                {
                    return this.transparent;
                }
                return this.owner.Transparent;
            }
            set
            {
                this.transparent = value;
                if (this.owner != null)
                {
                    this.owner.Transparent = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool UseDefaultMenu
        {
            get
            {
                if (this.owner == null)
                {
                    return this.useDefaultMenu;
                }
                return this.owner.UseDefaultMenu;
            }
            set
            {
                this.useDefaultMenu = value;
                if (this.owner != null)
                {
                    this.owner.UseDefaultMenu = value;
                }
            }
        }

        public XmlWhiteSpaceInfo WhiteSpace
        {
            get
            {
                if (this.owner == null)
                {
                    return this.whiteSpace;
                }
                return (XmlWhiteSpaceInfo) this.owner.WhiteSpace.SerializationInfo;
            }
            set
            {
                this.whiteSpace = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.WhiteSpace.SerializationInfo = value;
                }
            }
        }
    }
}

