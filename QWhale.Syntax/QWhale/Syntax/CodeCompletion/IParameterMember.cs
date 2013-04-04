namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface IParameterMember : ICodeCompletionProviderItem
    {
        string DataType { get; set; }

        string Description { get; set; }

        ParameterModifer Modifiers { get; set; }

        string Name { get; set; }

        string Qualifier { get; set; }

        string Text { get; set; }
    }
}

