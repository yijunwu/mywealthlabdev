namespace QWhale.Syntax
{
    using System;
    using System.IO;
    using System.Text;

    public interface IExport
    {
        bool SaveFile(string fileName);
        bool SaveFile(string fileName, Encoding encoding);
        bool SaveStream(Stream stream);
        bool SaveStream(TextWriter writer);
        bool SaveStream(Stream stream, Encoding encoding);
    }
}

