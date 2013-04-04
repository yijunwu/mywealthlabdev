namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using System;
    using System.Collections.Generic;

    public interface ILexReswordSet
    {
        int AddResword(string resword);
        void Clear();
        bool FindResword(string resword);

        ILexSyntaxBlock Block { get; set; }

        bool CaseSensitive { get; set; }

        int Index { get; }

        string Name { get; set; }

        IList<string> Reswords { get; set; }

        ILexStyle ReswordStyle { get; set; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

