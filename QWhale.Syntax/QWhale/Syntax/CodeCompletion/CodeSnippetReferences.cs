namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CodeSnippetReferences : List<ICodeSnippetReference>, ICodeSnippetReferences, IList<ICodeSnippetReference>, ICollection<ICodeSnippetReference>, IEnumerable<ICodeSnippetReference>, IEnumerable
    {
        public virtual ICodeSnippetReference AddReference()
        {
            ICodeSnippetReference item = new CodeSnippetReference();
            base.Add(item);
            return item;
        }

        public virtual ICodeSnippetReference InsertReference(int index)
        {
            ICodeSnippetReference item = new CodeSnippetReference();
            base.Insert(index, item);
            return item;
        }
    }
}

