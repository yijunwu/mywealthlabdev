namespace QWhale.Syntax.CodeCompletion
{
    using System;

    public interface IListMember : ICodeCompletionProviderItem
    {
        void AddDescription(string text);
        string GetParamText(bool useFormatting);
        string GetTemplate(bool compact);

        MemberAttribute Attributes { get; set; }

        int CurrentParamIndex { get; set; }

        object CustomData { get; set; }

        string DataType { get; set; }

        string Description { get; set; }

        string DisplayText { get; set; }

        int ImageIndex { get; set; }

        int MemberType { get; set; }

        string Name { get; set; }

        int Overloads { get; set; }

        IListMembers Owner { get; set; }

        IParameterMembers Parameters { get; set; }

        string ParamText { get; set; }

        int Priority { get; set; }

        string Qualifier { get; set; }
    }
}

