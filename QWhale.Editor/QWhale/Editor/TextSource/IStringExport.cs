namespace QWhale.Editor.TextSource
{
    using QWhale.Syntax;
    using System;
    using System.IO;

    public interface IStringExport
    {
        void BeginWrite(TextWriter writer, object userData);
        void EndWrite();
        bool Write();
        void WriteLine(IStringItem item);
    }
}

