namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICommentInfoItem : ICodeCompletionProviderItem
    {
        string Text { get; set; }
    }
}

