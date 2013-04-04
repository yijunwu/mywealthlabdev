namespace QWhale.Editor.Dialogs
{
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface ISyntaxSettings : IPersistentSettings, IImport, IExport
    {
        event HelpEventHandler HelpRequested;

        void ApplyToEdit(ISyntaxEdit edit);
        void ApplyToEdit(ISyntaxEdit edit, bool withStyles);
        bool IsBackColorEnabled(int index);
        bool IsDescriptionEnabled(int index);
        bool IsFontStyleEnabled(int index);
        void LoadFromEdit(ISyntaxEdit edit);
        void Localize();
        void OnHelpRequest(object sender, HelpEventArgs e);

        bool AllowOutlining { get; set; }

        IColorThemes ColorThemes { get; set; }

        ILexStyle[] DefaultLexStyles { get; set; }

        IKeyData[] EventData { get; set; }

        System.Drawing.Font Font { get; set; }

        QWhale.Editor.GutterOptions GutterOptions { get; set; }

        int GutterWidth { get; set; }

        bool HighlightHyperText { get; set; }

        ILexStyles LexStyles { get; set; }

        int MarginPos { get; set; }

        QWhale.Editor.TextSource.NavigateOptions NavigateOptions { get; set; }

        QWhale.Editor.OutlineOptions OutlineOptions { get; set; }

        RichTextBoxScrollBars ScrollBars { get; set; }

        QWhale.Editor.SelectionOptions SelectionOptions { get; set; }

        QWhale.Editor.SeparatorOptions SeparatorOptions { get; set; }

        bool ShowGutter { get; set; }

        bool ShowMargin { get; set; }

        int[] TabStops { get; set; }

        bool UseSpaces { get; set; }

        bool WhiteSpaceVisible { get; set; }

        bool WordWrap { get; set; }
    }
}

