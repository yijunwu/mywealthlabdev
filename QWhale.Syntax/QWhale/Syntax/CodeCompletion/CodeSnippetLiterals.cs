namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CodeSnippetLiterals : List<ICodeSnippetLiteral>, ICodeSnippetLiterals, IList<ICodeSnippetLiteral>, ICollection<ICodeSnippetLiteral>, IEnumerable<ICodeSnippetLiteral>, IEnumerable
    {
        public virtual ICodeSnippetLiteral AddLiteral()
        {
            ICodeSnippetLiteral item = new CodeSnippetLiteral();
            base.Add(item);
            return item;
        }

        public virtual ICodeSnippetLiteral InsertLiteral(int index)
        {
            ICodeSnippetLiteral item = new CodeSnippetLiteral();
            base.Insert(index, item);
            return item;
        }
    }
}

