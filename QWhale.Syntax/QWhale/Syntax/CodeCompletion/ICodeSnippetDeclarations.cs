namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICodeSnippetDeclarations : IList<ICodeSnippetDeclaration>, ICollection<ICodeSnippetDeclaration>, IEnumerable<ICodeSnippetDeclaration>, IEnumerable
    {
        ICodeSnippetDeclaration AddDeclaration();
        ICodeSnippetDeclaration InsertDeclaration(int index);
    }
}

