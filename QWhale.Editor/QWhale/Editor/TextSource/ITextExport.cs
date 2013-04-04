namespace QWhale.Editor.TextSource
{
    using QWhale.Syntax;
    using System;
    using System.IO;
    using System.Text;

    public interface ITextExport : IExport
    {
        bool SaveFile(string fileName, IStringExport exporter);
        bool SaveFile(string fileName, IStringExport exporter, Encoding encoding);
        bool SaveStream(Stream stream, IStringExport exporter);
        bool SaveStream(TextWriter writer, IStringExport exporter);
        bool SaveStream(Stream stream, IStringExport exporter, Encoding encoding);

        string LineTerminator { get; set; }
    }
}

