namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using System;
    using System.Runtime.CompilerServices;

    public interface ILexer : INotify, IUpdate
    {
        event ParseTextEvent Parse;

        int ParseText(int state, int line, string str, ref short[] colorData);
        int ParseText(int state, int line, string str, ref int pos, ref int len, ref int style);
        string RemovePlainText(string s, short[] textData);
        void ResetDefaultState();

        int DefaultState { get; set; }

        ILexScheme Scheme { get; set; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

