namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Syntax;

    public interface IInnerTextSource : ITextSource, IEdit, INavigate, IUndo, ITextNotify, INotify, IUpdate, INotifier, ITextImport, IImport, ITextExport, IExport, IHyperText, ISpelling, IBraceMatching, ITextParsing, ITextSnippets, ITextErrors
    {
    }
}

