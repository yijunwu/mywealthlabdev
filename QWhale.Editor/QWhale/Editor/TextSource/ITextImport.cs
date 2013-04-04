namespace QWhale.Editor.TextSource
{
    using QWhale.Syntax;
    using System;
    using System.IO;
    using System.Text;

    public interface ITextImport : IImport
    {
        bool LoadFile(string fileName, IStringImport importer);
        bool LoadFile(string fileName, IStringImport importer, Encoding encoding);
        bool LoadStream(Stream stream, IStringImport importer);
        bool LoadStream(TextReader reader, IStringImport importer);
        bool LoadStream(Stream stream, IStringImport importer, Encoding encoding);
    }
}

