namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using System;
    using System.Collections.Generic;

    public interface ILexSyntaxBlock
    {
        int AddExpression(string expression);
        int FindResword(string resword);

        bool CaseSensitive { get; }

        string Desc { get; set; }

        string Expression { get; }

        IList<string> Expressions { get; set; }

        int Index { get; }

        ILexState LeaveState { get; set; }

        string Name { get; set; }

        ILexReswordSets ReswordSets { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        ILexState State { get; set; }

        ILexStyle Style { get; set; }
    }
}

