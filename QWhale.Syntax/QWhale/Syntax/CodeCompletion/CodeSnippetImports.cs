namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CodeSnippetImports : List<ICodeSnippetImport>, ICodeSnippetImports, IList<ICodeSnippetImport>, ICollection<ICodeSnippetImport>, IEnumerable<ICodeSnippetImport>, IEnumerable
    {
        public virtual ICodeSnippetImport AddImport()
        {
            ICodeSnippetImport item = new CodeSnippetImport();
            base.Add(item);
            return item;
        }

        public virtual ICodeSnippetImport InsertImport(int index)
        {
            ICodeSnippetImport item = new CodeSnippetImport();
            base.Insert(index, item);
            return item;
        }
    }
}

