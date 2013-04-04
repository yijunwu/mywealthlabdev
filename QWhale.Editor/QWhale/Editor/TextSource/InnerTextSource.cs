namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Syntax;
    using System.ComponentModel;

    [ToolboxItem(false)]
    public class InnerTextSource : QWhale.Editor.TextSource.TextSource, IInnerTextSource, ITextSource, IEdit, INavigate, IUndo, ITextNotify, INotify, IUpdate, INotifier, ITextImport, IImport, ITextExport, IExport, IHyperText, ISpelling, IBraceMatching, ITextParsing, ITextSnippets, ITextErrors
    {
    }
}

