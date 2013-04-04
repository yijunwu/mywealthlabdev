namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CodeSnippetTypes : List<ICodeSnippetType>, ICodeSnippetTypes, IList<ICodeSnippetType>, ICollection<ICodeSnippetType>, IEnumerable<ICodeSnippetType>, IEnumerable
    {
        public virtual ICodeSnippetType AddSnippetType()
        {
            ICodeSnippetType item = new CodeSnippetType();
            base.Add(item);
            return item;
        }

        public virtual ICodeSnippetType InsertSnippetType(int index)
        {
            ICodeSnippetType item = new CodeSnippetType();
            base.Insert(0, item);
            return item;
        }
    }
}

