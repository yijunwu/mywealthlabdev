namespace QWhale.Editor.CodeCompletion
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ICodeCompletionColumns : IList<ICodeCompletionColumn>, ICollection<ICodeCompletionColumn>, IEnumerable<ICodeCompletionColumn>, IEnumerable
    {
    }
}

