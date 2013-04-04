namespace QWhale.Editor.TextSource
{
    using System;
    using System.IO;

    public interface IStringImport
    {
        void BeginRead(TextReader reader, object userData);
        void EndRead();
        bool Read();
    }
}

