namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface IQuickInfoItem : ICodeCompletionProviderItem
    {
        string Text { get; set; }
    }
}

