namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Drawing;
    using System.Reflection;

    public interface IColorTheme
    {
        System.Drawing.Font Font { get; set; }

        ILexStyle this[string name] { get; }

        ILexStyles LexStyles { get; set; }

        string Name { get; set; }

        bool Readonly { get; set; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

