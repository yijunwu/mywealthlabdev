namespace QWhale.Editor.Dialogs
{
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;

    public interface ISearchSettings : IPersistentSettings, IImport, IExport
    {
        void Assign(ISearchSettings source);

        bool ClearBookmarks { get; set; }

        IList<string> ReplaceList { get; }

        IList<string> SearchList { get; }

        QWhale.Editor.TextSource.SearchOptions SearchOptions { get; set; }
    }
}

