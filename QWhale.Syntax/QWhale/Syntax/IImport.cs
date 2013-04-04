namespace QWhale.Syntax
{
    using System;
    using System.IO;
    using System.Text;

    public interface IImport
    {
        bool LoadFile(string fileName);
        bool LoadFile(string fileName, Encoding encoding);
        bool LoadStream(Stream stream);
        bool LoadStream(TextReader reader);
        bool LoadStream(Stream stream, Encoding encoding);
    }
}

