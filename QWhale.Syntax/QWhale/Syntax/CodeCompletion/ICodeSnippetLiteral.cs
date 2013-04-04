namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface ICodeSnippetLiteral : ICodeCompletionProviderItem
    {
        string Default { get; set; }

        bool Editable { get; set; }

        string Function { get; set; }

        string ID { get; set; }

        string ToolTip { get; set; }

        string Type { get; set; }
    }
}

