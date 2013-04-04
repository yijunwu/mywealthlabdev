namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System.Collections;
    using System.Collections.Generic;

    public interface IParameterInfo : IListMembers, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
    }
}

