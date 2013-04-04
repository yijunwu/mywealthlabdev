namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CodeSnippetDeclarations : List<ICodeSnippetDeclaration>, ICodeSnippetDeclarations, IList<ICodeSnippetDeclaration>, ICollection<ICodeSnippetDeclaration>, IEnumerable<ICodeSnippetDeclaration>, IEnumerable
    {
        public virtual ICodeSnippetDeclaration AddDeclaration()
        {
            ICodeSnippetDeclaration item = new CodeSnippetDeclaration();
            base.Add(item);
            return item;
        }

        public virtual ICodeSnippetDeclaration InsertDeclaration(int index)
        {
            ICodeSnippetDeclaration item = new CodeSnippetDeclaration();
            base.Insert(index, item);
            return item;
        }
    }
}

