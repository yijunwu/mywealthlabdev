namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public interface ILexState
    {
        void ResetCaseSensitive();

        Dictionary<int, ILexSyntaxBlock> Blocks { get; }

        bool CaseSensitive { get; set; }

        string Desc { get; set; }

        string Expression { get; }

        int Index { get; }

        string Name { get; set; }

        System.Text.RegularExpressions.Regex Regex { get; }

        ILexScheme Scheme { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        ILexSyntaxBlocks SyntaxBlocks { get; set; }
    }
}

