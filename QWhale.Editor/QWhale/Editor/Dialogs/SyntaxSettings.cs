namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SyntaxSettings : PersistentSettings, ISyntaxSettings, IPersistentSettings, IImport, IExport
    {
        private bool allowOutlining;
        private int cColorStyles = -1;
        private IColorThemes colorThemes = new QWhale.Editor.Dialogs.ColorThemes();
        private ILexStyles defaultLexStyles;
        private IKeyData[] eventData = new IKeyData[0];
        private string[] eventNames = new string[0];
        private QWhale.Editor.GutterOptions gutterOptions = EditConsts.DefaultGutterOptions;
        private int gutterWidth = EditConsts.DefaultGutterWidth;
        private bool highlightHyperText = true;
        private QWhale.Editor.KeyList keyList;
        private int marginPos = EditConsts.DefaultMarginPosition;
        private QWhale.Editor.TextSource.NavigateOptions navigateOptions = EditConsts.DefaultNavigateOptions;
        private QWhale.Editor.OutlineOptions outlineOptions = EditConsts.DefaultOutlineOptions;
        private QWhale.Editor.PageType pageType;
        private RichTextBoxScrollBars scrollBars = RichTextBoxScrollBars.Both;
        private QWhale.Editor.SelectionOptions selectionOptions = EditConsts.DefaultSelectionOptions;
        private QWhale.Editor.SeparatorOptions separatorOptions = QWhale.Editor.SeparatorOptions.None;
        private bool showGutter = true;
        private bool showMargin;
        private int[] tabStops = new int[] { EditConsts.DefaultTabStop };
        private bool useSpaces;
        private bool whiteSpaceVisible;
        private bool wordWrap;

        private event HelpEventHandler helpRequested;

        public event HelpEventHandler HelpRequested;

        public SyntaxSettings()
        {
            this.gutterOptions |= EditConsts.DefaultGutterOptions;
            this.defaultLexStyles = new QWhale.Syntax.Lexer.LexStyles(null);
            this.colorThemes.Add(new ColorTheme("Default", true, new System.Drawing.Font(FontFamily.GenericMonospace, 10f), this.defaultLexStyles));
            this.colorThemes.ActiveThemeIndex = 0;
            this.InitializeDefaultColorThemes();
        }

        public virtual void ApplyToEdit(ISyntaxEdit edit)
        {
            this.ApplyToEdit(edit, true);
        }

        public virtual void ApplyToEdit(ISyntaxEdit edit, bool withStyles)
        {
            edit.Source.BeginUpdate(UpdateReason.Other);
            try
            {
                edit.Outlining.AllowOutlining = this.AllowOutlining;
                edit.Outlining.OutlineOptions = this.OutlineOptions;
                edit.Font = this.Font;
                edit.Gutter.Options = this.GutterOptions;
                edit.LineSeparator.Options = this.SeparatorOptions;
                edit.Gutter.Width = this.GutterWidth;
                edit.EditMargin.Position = this.MarginPos;
                edit.NavigateOptions = this.NavigateOptions;
                edit.Scrolling.ScrollBars = this.ScrollBars;
                edit.Selection.Options = this.SelectionOptions;
                edit.Gutter.Visible = this.ShowGutter;
                edit.EditMargin.Visible = this.ShowMargin;
                edit.Source.Lines.TabStops = this.TabStops;
                edit.Source.Lines.UseSpaces = this.UseSpaces;
                edit.WordWrap = this.WordWrap;
                edit.WhiteSpace.Visible = this.WhiteSpaceVisible;
                edit.Pages.PageType = this.PageType;
                if (withStyles)
                {
                    this.CopyStyles(this.LexStyles, this.GetLexStyles(edit));
                }
                edit.HyperText.HighlightHyperText = this.HighlightHyperText;
                IColorTheme activeTheme = this.colorThemes.ActiveTheme;
                if (activeTheme != null)
                {
                    if (activeTheme[StringConsts.MisspelledWordsInternalName] != null)
                    {
                        edit.Spelling.SpellColor = activeTheme[StringConsts.MisspelledWordsInternalName].ForeColor;
                    }
                    if (activeTheme[StringConsts.HyperTextInternalName] != null)
                    {
                        edit.HyperText.UrlColor = activeTheme[StringConsts.HyperTextInternalName].ForeColor;
                        edit.HyperText.UrlStyle = activeTheme[StringConsts.HyperTextInternalName].FontStyle;
                    }
                    if (activeTheme[StringConsts.LineNumbersInternalName] != null)
                    {
                        edit.Gutter.LineNumbersForeColor = activeTheme[StringConsts.LineNumbersInternalName].ForeColor;
                    }
                    if (activeTheme[StringConsts.WindowColorInternalName] != null)
                    {
                        edit.Gutter.LineNumbersBackColor = activeTheme[StringConsts.WindowColorInternalName].BackColor;
                    }
                    if (activeTheme[StringConsts.GutterPenColorInternalName] != null)
                    {
                        edit.Gutter.BrushColor = activeTheme[StringConsts.GutterPenColorInternalName].BackColor;
                        edit.Gutter.PenColor = activeTheme[StringConsts.GutterPenColorInternalName].ForeColor;
                    }
                    if (activeTheme[StringConsts.WhiteSpaceInternalName] != null)
                    {
                        edit.WhiteSpace.SymbolColor = activeTheme[StringConsts.WhiteSpaceInternalName].ForeColor;
                    }
                    if (activeTheme[StringConsts.LineModificatorChangedInternalName] != null)
                    {
                        edit.Gutter.LineModificatorChangedColor = activeTheme[StringConsts.LineModificatorChangedInternalName].ForeColor;
                    }
                    if (activeTheme[StringConsts.LineModificatorSavedInternalName] != null)
                    {
                        edit.Gutter.LineModificatorSavedColor = activeTheme[StringConsts.LineModificatorSavedInternalName].ForeColor;
                    }
                    if (activeTheme[StringConsts.LineSeparatorInternalName] != null)
                    {
                        edit.LineSeparator.LineColor = activeTheme[StringConsts.LineSeparatorInternalName].ForeColor;
                    }
                    if ((activeTheme[StringConsts.WindowColorInternalName] != null) && (activeTheme[StringConsts.WindowColorInternalName].BackColor != Color.Empty))
                    {
                        edit.BackColor = activeTheme[StringConsts.WindowColorInternalName].BackColor;
                    }
                    if (activeTheme[StringConsts.BraceMatchingInternalName] != null)
                    {
                        edit.Braces.ForeColor = activeTheme[StringConsts.BraceMatchingInternalName].ForeColor;
                        edit.Braces.BackColor = activeTheme[StringConsts.BraceMatchingInternalName].BackColor;
                    }
                    if (activeTheme[StringConsts.CodeOutlinePenColorInternalName] != null)
                    {
                        edit.Outlining.OutlineColor = activeTheme[StringConsts.CodeOutlinePenColorInternalName].ForeColor;
                    }
                    if (activeTheme[StringConsts.ActiveSelectionInternalName] != null)
                    {
                        edit.Selection.ForeColor = activeTheme[StringConsts.ActiveSelectionInternalName].ForeColor;
                        edit.Selection.BackColor = activeTheme[StringConsts.ActiveSelectionInternalName].BackColor;
                    }
                    if (activeTheme[StringConsts.InactiveSelectionInternalName] != null)
                    {
                        edit.Selection.InActiveForeColor = activeTheme[StringConsts.InactiveSelectionInternalName].ForeColor;
                        edit.Selection.InActiveBackColor = activeTheme[StringConsts.InactiveSelectionInternalName].BackColor;
                    }
                }
            }
            finally
            {
                edit.Source.EndUpdate();
            }
            edit.Invalidate();
        }

        public override void Assign(IPersistentSettings source)
        {
            if (source is SyntaxSettings)
            {
                SyntaxSettings settings = (SyntaxSettings) source;
                this.navigateOptions = settings.navigateOptions;
                this.scrollBars = settings.scrollBars;
                this.selectionOptions = settings.selectionOptions;
                this.gutterOptions = settings.gutterOptions;
                this.separatorOptions = settings.separatorOptions;
                this.outlineOptions = settings.outlineOptions;
                this.showMargin = settings.showMargin;
                this.showGutter = settings.showGutter;
                this.highlightHyperText = settings.highlightHyperText;
                this.allowOutlining = settings.allowOutlining;
                this.useSpaces = settings.useSpaces;
                this.wordWrap = settings.wordWrap;
                this.whiteSpaceVisible = settings.whiteSpaceVisible;
                this.pageType = settings.pageType;
                this.gutterWidth = settings.gutterWidth;
                this.marginPos = settings.marginPos;
                this.TabStops = settings.TabStops;
                this.DefaultLexStyles = settings.DefaultLexStyles;
                ISerializationInfo serializationInfo = settings.ColorThemes.SerializationInfo;
                serializationInfo.Load();
                this.colorThemes.SerializationInfo = serializationInfo;
                this.eventNames = new string[settings.EventNames.Length];
                settings.EventNames.CopyTo(this.eventNames, 0);
                this.eventData = new IKeyData[settings.EventData.Length];
                settings.EventData.CopyTo(this.eventData, 0);
                this.KeyList = settings.KeyList;
            }
        }

        private void CopyStyles(ILexStyles fromStyles, ILexStyles toStyles)
        {
            if ((fromStyles != null) && (toStyles != null))
            {
                foreach (ILexStyle style in fromStyles)
                {
                    int styleByName = this.GetStyleByName(toStyles, style.Name);
                    if (styleByName >= 0)
                    {
                        toStyles[styleByName].Assign(style);
                    }
                }
            }
        }

        private ILexStyles GetLexStyles(ISyntaxEdit edit)
        {
            if (edit.Source.Lexer == null)
            {
                return null;
            }
            return edit.Source.Lexer.Scheme.Styles;
        }

        private int GetStyleByName(ILexStyles styles, string name)
        {
            for (int i = 0; i < styles.Count; i++)
            {
                if (string.Compare(styles[i].Name, name, true) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public override System.Type GetXmlType()
        {
            return typeof(XmlSyntaxSettingsInfo);
        }

        protected virtual ILexStyle InitDefaultColorStyle(string name, string desc, Color foreColor, Color backColor)
        {
            ILexStyle style = this.InitDefaultStyle(name, desc, foreColor, backColor, FontStyle.Regular, false);
            style.BackColorEnabled = backColor != Color.Empty;
            style.ForeColorEnabled = foreColor != Color.Empty;
            return style;
        }

        protected virtual void InitDefaultColorStyles()
        {
            this.InitDefaultColorStyle(StringConsts.WindowColorInternalName, StringConsts.WindowColorName, Color.Empty, Consts.DefaultControlBackColor);
            this.InitDefaultColorStyle(StringConsts.BraceMatchingInternalName, StringConsts.BraceMatchingName, EditConsts.DefaultBracesForeColor, EditConsts.DefaultBracesBackColor);
            this.InitDefaultColorStyle(StringConsts.GutterPenColorInternalName, StringConsts.GutterPenColorName, EditConsts.DefaultGutterForeColor, EditConsts.DefaultGutterBackColor);
            this.InitDefaultColorStyle(StringConsts.LineModificatorChangedInternalName, StringConsts.LineModificatorChangedName, EditConsts.DefaultLineModificatorChangedColor, Color.Empty);
            this.InitDefaultColorStyle(StringConsts.LineModificatorSavedInternalName, StringConsts.LineModificatorSavedName, EditConsts.DefaultLineModificatorSavedColor, Color.Empty);
            this.InitDefaultColorStyle(StringConsts.LineSeparatorInternalName, StringConsts.LineSeparatorName, EditConsts.DefaultLineSeparatorLineColor, Color.Empty);
            this.InitDefaultColorStyle(StringConsts.CodeOutlinePenColorInternalName, StringConsts.CodeOutlinePenColorName, EditConsts.DefaultOutlineForeColor, Color.Empty);
            this.InitDefaultColorStyle(StringConsts.ActiveSelectionInternalName, StringConsts.ActiveSelectionName, EditConsts.DefaultHighlightForeColor, EditConsts.DefaultHighlightBackColor);
            this.InitDefaultColorStyle(StringConsts.InactiveSelectionInternalName, StringConsts.InactiveSelectionName, EditConsts.DefaultInactiveHighlightForeColor, EditConsts.DefaultInactiveHighlightBackColor);
        }

        protected virtual ILexStyle InitDefaultStyle(string name, string desc, Color foreColor, Color backColor, FontStyle fontStyle, bool plainText)
        {
            ILexStyle item = new LexStyle(null) {
                Name = name,
                Desc = desc,
                ForeColor = foreColor,
                BackColor = backColor,
                FontStyle = fontStyle,
                PlainText = plainText
            };
            this.defaultLexStyles.Add(item);
            return item;
        }

        protected virtual void InitDefaultStyles()
        {
            this.defaultLexStyles.Clear();
            this.InitDefaultStyle(StringConsts.SymbolsInternalName, StringConsts.SymbolsName, SyntaxConsts.DefaultSymbolForeColor, Color.Empty, FontStyle.Regular, false);
            this.InitDefaultStyle(StringConsts.IdentsInternalName, StringConsts.IdentsName, SyntaxConsts.DefaultIndentsForeColor, Color.Empty, FontStyle.Regular, false);
            this.InitDefaultStyle(StringConsts.NumbersInternalName, StringConsts.NumbersName, SyntaxConsts.DefaultNumbersForeColor, Color.Empty, FontStyle.Regular, false);
            this.InitDefaultStyle(StringConsts.ReswordsInternalName, StringConsts.ReswordsName, SyntaxConsts.DefaultReswordForeColor, Color.Empty, FontStyle.Regular, false);
            this.InitDefaultStyle(StringConsts.CommentsInternalName, StringConsts.CommentsName, SyntaxConsts.DefaultCommentsForeColor, Color.Empty, FontStyle.Regular, true);
            this.InitDefaultStyle(StringConsts.StringsInternalName, StringConsts.StringsName, SyntaxConsts.DefaultStringsForeColor, Color.Empty, FontStyle.Regular, true);
            this.InitDefaultStyle(StringConsts.DirectivesInternalName, StringConsts.DirectivesName, SyntaxConsts.DefaultDirectivesForeColor, Color.Empty, FontStyle.Regular, false);
            this.InitDefaultStyle(StringConsts.XMLCommentsInternalName, StringConsts.XMLCommentsName, SyntaxConsts.DefaultXmlCommentsForeColor, Color.Empty, FontStyle.Regular, true);
            this.InitDefaultStyle(StringConsts.HTMLParamsInternalName, StringConsts.HTMLParamsName, SyntaxConsts.DefaultHtmlParamsForeColor, Color.Empty, FontStyle.Regular, false);
            this.InitDefaultStyle(StringConsts.WhiteSpaceInternalName, StringConsts.WhiteSpaceName, Consts.DefaultControlForeColor, Color.Empty, FontStyle.Regular, false);
            this.InitDefaultStyle(StringConsts.LineNumbersInternalName, StringConsts.LineNumbersName, EditConsts.DefaultLineNumbersForeColor, Color.Empty, FontStyle.Regular, false);
            this.InitDefaultStyle(StringConsts.HyperTextInternalName, StringConsts.HyperTextName, EditConsts.DefaultHyperTextForeColor, Color.Empty, EditConsts.DefaultUrlFontStyle, false);
            this.InitDefaultStyle(StringConsts.MisspelledWordsInternalName, StringConsts.MisspelledWordsName, EditConsts.DefaultSpellForeColor, Color.Empty, FontStyle.Regular, false);
        }

        protected virtual void InitializeDefaultColorThemes()
        {
            this.InitDefaultStyles();
            this.cColorStyles = this.defaultLexStyles.Count;
            this.InitDefaultColorStyles();
            this.DefaultColorTheme.LexStyles = this.defaultLexStyles;
        }

        public virtual bool IsBackColorEnabled(int index)
        {
            return (((index >= 0) && (this.colorThemes.ActiveTheme != null)) && this.colorThemes.ActiveTheme.LexStyles[index].BackColorEnabled);
        }

        public virtual bool IsDescriptionEnabled(int index)
        {
            return (((index >= 0) && (this.colorThemes.ActiveTheme != null)) && (this.colorThemes.ActiveTheme.LexStyles[index].Desc != string.Empty));
        }

        public virtual bool IsFontStyleEnabled(int index)
        {
            return ((index >= 0) && (index < this.cColorStyles));
        }

        public virtual void LoadFromEdit(ISyntaxEdit edit)
        {
            this.GutterOptions = edit.Gutter.Options;
            this.SeparatorOptions = edit.LineSeparator.Options;
            this.GutterWidth = edit.Gutter.Width;
            this.HighlightHyperText = edit.HyperText.HighlightHyperText;
            this.MarginPos = edit.EditMargin.Position;
            this.NavigateOptions = edit.NavigateOptions;
            this.AllowOutlining = edit.Outlining.AllowOutlining;
            this.OutlineOptions = edit.Outlining.OutlineOptions;
            this.ScrollBars = edit.Scrolling.ScrollBars;
            this.SelectionOptions = edit.Selection.Options;
            this.ShowGutter = edit.Gutter.Visible;
            this.ShowMargin = edit.EditMargin.Visible;
            this.TabStops = edit.Source.Lines.TabStops;
            this.UseSpaces = edit.Source.Lines.UseSpaces;
            this.WordWrap = edit.WordWrap;
            this.WhiteSpaceVisible = edit.WhiteSpace.Visible;
            this.PageType = edit.Pages.PageType;
            this.EventNames = edit.KeyList.Handlers.EventNames;
            this.EventData = edit.KeyList.EventData;
            this.KeyList = (QWhale.Editor.KeyList) edit.KeyList;
            this.Font = edit.Font;
            IColorTheme activeTheme = this.colorThemes.ActiveTheme;
            if (activeTheme != null)
            {
                if (activeTheme[StringConsts.WindowColorInternalName] != null)
                {
                    activeTheme[StringConsts.WindowColorInternalName].BackColor = edit.BackColor;
                }
                if (activeTheme[StringConsts.MisspelledWordsInternalName] != null)
                {
                    activeTheme[StringConsts.MisspelledWordsInternalName].ForeColor = edit.Spelling.SpellColor;
                }
                if (activeTheme[StringConsts.HyperTextInternalName] != null)
                {
                    activeTheme[StringConsts.HyperTextInternalName].ForeColor = edit.HyperText.UrlColor;
                    activeTheme[StringConsts.HyperTextInternalName].FontStyle = edit.HyperText.UrlStyle;
                }
                if (activeTheme[StringConsts.LineNumbersInternalName] != null)
                {
                    activeTheme[StringConsts.LineNumbersInternalName].ForeColor = edit.Gutter.LineNumbersForeColor;
                }
                if (activeTheme[StringConsts.GutterPenColorInternalName] != null)
                {
                    activeTheme[StringConsts.GutterPenColorInternalName].BackColor = edit.Gutter.BrushColor;
                    activeTheme[StringConsts.GutterPenColorInternalName].ForeColor = edit.Gutter.PenColor;
                }
                if (activeTheme[StringConsts.WhiteSpaceInternalName] != null)
                {
                    activeTheme[StringConsts.WhiteSpaceInternalName].ForeColor = edit.WhiteSpace.SymbolColor;
                }
                if (activeTheme[StringConsts.LineModificatorChangedInternalName] != null)
                {
                    activeTheme[StringConsts.LineModificatorChangedInternalName].ForeColor = edit.Gutter.LineModificatorChangedColor;
                }
                if (activeTheme[StringConsts.LineModificatorSavedInternalName] != null)
                {
                    activeTheme[StringConsts.LineModificatorSavedInternalName].ForeColor = edit.Gutter.LineModificatorSavedColor;
                }
                if (activeTheme[StringConsts.LineSeparatorInternalName] != null)
                {
                    activeTheme[StringConsts.LineSeparatorInternalName].ForeColor = edit.LineSeparator.LineColor;
                }
                if (activeTheme[StringConsts.BraceMatchingInternalName] != null)
                {
                    activeTheme[StringConsts.BraceMatchingInternalName].ForeColor = edit.Braces.ForeColor;
                    activeTheme[StringConsts.BraceMatchingInternalName].BackColor = edit.Braces.BackColor;
                }
                if (activeTheme[StringConsts.CodeOutlinePenColorInternalName] != null)
                {
                    activeTheme[StringConsts.CodeOutlinePenColorInternalName].ForeColor = edit.Outlining.OutlineColor;
                }
                if (activeTheme[StringConsts.ActiveSelectionInternalName] != null)
                {
                    activeTheme[StringConsts.ActiveSelectionInternalName].ForeColor = edit.Selection.ForeColor;
                    activeTheme[StringConsts.ActiveSelectionInternalName].BackColor = edit.Selection.BackColor;
                }
                if (activeTheme[StringConsts.InactiveSelectionInternalName] != null)
                {
                    activeTheme[StringConsts.InactiveSelectionInternalName].ForeColor = edit.Selection.InActiveForeColor;
                    activeTheme[StringConsts.InactiveSelectionInternalName].BackColor = edit.Selection.InActiveBackColor;
                }
            }
        }

        public virtual void Localize()
        {
            this.InitializeDefaultColorThemes();
        }

        protected virtual void OnAllowOutliningChanged()
        {
        }

        protected virtual void OnColorThemesChanged()
        {
        }

        protected virtual void OnEventDataChanged()
        {
        }

        protected virtual void OnEventNamesChanged()
        {
        }

        protected virtual void OnGutterOptionsChanged()
        {
        }

        protected virtual void OnGutterWidthChanged()
        {
        }

        public void OnHelpRequest(object sender, HelpEventArgs e)
        {
            if (this.helpRequested != null)
            {
                this.helpRequested(this, e);
            }
        }

        protected virtual void OnHighlightHyperTextChanged()
        {
        }

        protected virtual void OnMarginPosChanged()
        {
        }

        protected virtual void OnNavigateOptionsChanged()
        {
        }

        protected virtual void OnOutlineOptionsChanged()
        {
        }

        protected virtual void OnScrollBarsChanged()
        {
        }

        protected virtual void OnSelectionOptionsChanged()
        {
        }

        protected virtual void OnSeparatorOptionsChanged()
        {
        }

        protected virtual void OnShowGutterChanged()
        {
        }

        protected virtual void OnShowMarginChanged()
        {
        }

        protected virtual void OnTabStopsChanged()
        {
        }

        protected virtual void OnUseSpacesChanged()
        {
        }

        protected virtual void OnWhiteSpaceVisibleChanged()
        {
        }

        protected virtual void OnWordWrapChanged()
        {
        }

        public virtual bool AllowOutlining
        {
            get
            {
                return this.allowOutlining;
            }
            set
            {
                if (this.allowOutlining != value)
                {
                    this.allowOutlining = value;
                    this.OnAllowOutliningChanged();
                }
            }
        }

        public virtual IColorThemes ColorThemes
        {
            get
            {
                return this.colorThemes;
            }
            set
            {
                if (this.colorThemes != value)
                {
                    this.colorThemes = value;
                    this.OnColorThemesChanged();
                }
            }
        }

        public virtual IColorTheme DefaultColorTheme
        {
            get
            {
                return this.colorThemes[0];
            }
        }

        public virtual ILexStyle[] DefaultLexStyles
        {
            get
            {
                ILexStyle[] array = new LexStyle[this.defaultLexStyles.Count];
                this.defaultLexStyles.CopyTo(array, 0);
                return array;
            }
            set
            {
                this.defaultLexStyles.Clear();
                foreach (ILexStyle style in value)
                {
                    this.defaultLexStyles.Add(style);
                }
                this.DefaultColorTheme.LexStyles = this.defaultLexStyles;
            }
        }

        public virtual IKeyData[] EventData
        {
            get
            {
                return this.eventData;
            }
            set
            {
                if (this.eventData != value)
                {
                    this.eventData = value;
                    this.OnEventDataChanged();
                }
            }
        }

        public virtual string[] EventNames
        {
            get
            {
                return this.eventNames;
            }
            set
            {
                if (this.eventNames != value)
                {
                    this.eventNames = value;
                    this.OnEventNamesChanged();
                }
            }
        }

        public virtual System.Drawing.Font Font
        {
            get
            {
                if (this.colorThemes.ActiveTheme == null)
                {
                    return null;
                }
                return this.colorThemes.ActiveTheme.Font;
            }
            set
            {
                if (this.colorThemes.ActiveTheme != null)
                {
                    this.colorThemes.ActiveTheme.Font = value;
                }
            }
        }

        public virtual QWhale.Editor.GutterOptions GutterOptions
        {
            get
            {
                return this.gutterOptions;
            }
            set
            {
                if (this.gutterOptions != value)
                {
                    this.gutterOptions = value;
                    this.OnGutterOptionsChanged();
                }
            }
        }

        public virtual int GutterWidth
        {
            get
            {
                return this.gutterWidth;
            }
            set
            {
                if (this.gutterWidth != value)
                {
                    this.gutterWidth = value;
                    this.OnGutterWidthChanged();
                }
            }
        }

        public virtual bool HighlightHyperText
        {
            get
            {
                return this.highlightHyperText;
            }
            set
            {
                if (this.highlightHyperText != value)
                {
                    this.highlightHyperText = value;
                    this.OnHighlightHyperTextChanged();
                }
            }
        }

        public QWhale.Editor.KeyList KeyList
        {
            get
            {
                return this.keyList;
            }
            set
            {
                this.keyList = value;
            }
        }

        public virtual ILexStyles LexStyles
        {
            get
            {
                if (this.colorThemes.ActiveTheme == null)
                {
                    return null;
                }
                return this.colorThemes.ActiveTheme.LexStyles;
            }
            set
            {
                if (this.colorThemes.ActiveTheme != null)
                {
                    this.colorThemes.ActiveTheme.LexStyles = value;
                }
            }
        }

        public virtual int MarginPos
        {
            get
            {
                return this.marginPos;
            }
            set
            {
                if (this.marginPos != value)
                {
                    this.marginPos = value;
                    this.OnMarginPosChanged();
                }
            }
        }

        public virtual QWhale.Editor.TextSource.NavigateOptions NavigateOptions
        {
            get
            {
                return this.navigateOptions;
            }
            set
            {
                if (this.navigateOptions != value)
                {
                    this.navigateOptions = value;
                    this.OnNavigateOptionsChanged();
                }
            }
        }

        public virtual QWhale.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                return this.outlineOptions;
            }
            set
            {
                if (this.outlineOptions != value)
                {
                    this.outlineOptions = value;
                    this.OnOutlineOptionsChanged();
                }
            }
        }

        public QWhale.Editor.PageType PageType
        {
            get
            {
                return this.pageType;
            }
            set
            {
                this.pageType = value;
            }
        }

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

        public virtual QWhale.Editor.SelectionOptions SelectionOptions
        {
            get
            {
                return this.selectionOptions;
            }
            set
            {
                if (this.selectionOptions != value)
                {
                    this.selectionOptions = value;
                    this.OnSelectionOptionsChanged();
                }
            }
        }

        public virtual QWhale.Editor.SeparatorOptions SeparatorOptions
        {
            get
            {
                return this.separatorOptions;
            }
            set
            {
                if (this.separatorOptions != value)
                {
                    this.separatorOptions = value;
                    this.OnSeparatorOptionsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlSyntaxSettingsInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual bool ShowGutter
        {
            get
            {
                return this.showGutter;
            }
            set
            {
                if (this.showGutter != value)
                {
                    this.showGutter = value;
                    this.OnShowGutterChanged();
                }
            }
        }

        public virtual bool ShowMargin
        {
            get
            {
                return this.showMargin;
            }
            set
            {
                if (this.showMargin != value)
                {
                    this.showMargin = value;
                    this.OnShowMarginChanged();
                }
            }
        }

        public virtual int[] TabStops
        {
            get
            {
                return this.tabStops;
            }
            set
            {
                if (this.tabStops != value)
                {
                    this.tabStops = new int[value.Length];
                    Array.Copy(value, this.tabStops, value.Length);
                    this.OnTabStopsChanged();
                }
            }
        }

        public virtual bool UseSpaces
        {
            get
            {
                return this.useSpaces;
            }
            set
            {
                if (this.useSpaces != value)
                {
                    this.useSpaces = value;
                    this.OnUseSpacesChanged();
                }
            }
        }

        public virtual bool WhiteSpaceVisible
        {
            get
            {
                return this.whiteSpaceVisible;
            }
            set
            {
                if (this.whiteSpaceVisible != value)
                {
                    this.whiteSpaceVisible = value;
                    this.OnWhiteSpaceVisibleChanged();
                }
            }
        }

        public virtual bool WordWrap
        {
            get
            {
                return this.wordWrap;
            }
            set
            {
                if (this.wordWrap != value)
                {
                    this.wordWrap = value;
                    this.OnWordWrapChanged();
                }
            }
        }
    }
}

