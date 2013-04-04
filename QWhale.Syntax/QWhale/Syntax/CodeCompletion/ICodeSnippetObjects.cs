namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICodeSnippetObjects : IList<ICodeSnippetObject>, ICollection<ICodeSnippetObject>, IEnumerable<ICodeSnippetObject>, IEnumerable
    {
        ICodeSnippetObject AddObject();
        ICodeSnippetObject InsertObject(int index);
    }
}

