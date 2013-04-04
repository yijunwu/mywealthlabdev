namespace QWhale.Syntax
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface IParser : ILexer, INotify, IUpdate
    {
        int NextToken();
        int NextToken(out string str);
        int NextValidToken();
        int NextValidToken(out string str);
        int PeekToken();
        int PeekToken(out string str);
        int PeekValidToken();
        int PeekValidToken(out string str);
        void Reset();
        void Reset(int line, int pos, int state);
        void RestoreState();
        void RestoreState(bool restore);
        void SaveState();

        Point CurrentPosition { get; }

        bool Eof { get; }

        string[] Lines { get; set; }

        int State { get; }

        IStringList Strings { get; set; }

        int Token { get; }

        Point TokenPosition { get; }

        string TokenString { get; }
    }
}

