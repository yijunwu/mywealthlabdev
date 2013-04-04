namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using QWhale.Syntax;
    using System;

    public interface ILexScheme : IImport, IExport
    {
        void Clear();
        bool IsEmpty();
        bool IsPlainText(int style);

        string Author { get; set; }

        string Copyright { get; set; }

        string Desc { get; set; }

        string FileExtension { get; set; }

        string FileType { get; set; }

        string Name { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        ILexStates States { get; set; }

        ILexStyles Styles { get; set; }

        string Version { get; set; }
    }
}

