namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;

    public class CRepository : ReflectionRepository
    {
        public CRepository(bool caseSensitive, ISyntaxTree syntaxTree) : base(caseSensitive, syntaxTree)
        {
        }
    }
}

