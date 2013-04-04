namespace QWhale.Editor
{
    using QWhale.Editor.Dialogs;
    using QWhale.Editor.TextSource;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    public interface ISearch : ITextSearch
    {
        bool CanFindNext();
        bool CanFindNextSelected();
        bool CanFindPrevious();
        bool CanFindPreviousSelected();
        bool CanSearchSelection(out string selectedText);
        bool Find(string text);
        bool Find(string text, QWhale.Editor.TextSource.SearchOptions options);
        bool Find(string text, QWhale.Editor.TextSource.SearchOptions options, Regex expression);
        bool Find(string text, QWhale.Editor.TextSource.SearchOptions options, Regex expression, IList<QWhale.Common.IRange> ranges);
        bool FindNext();
        bool FindNextSelected();
        bool FindPrevious();
        bool FindPreviousSelected();
        void FinishIncrementalSearch();
        string GetTextToSearchAtCursor();
        bool IncrementalSearch(string key, bool deleteLast);
        int MarkAll(string text, bool clearPrevious);
        int MarkAll(string text, QWhale.Editor.TextSource.SearchOptions options, bool clearPrevious);
        int MarkAll(string text, QWhale.Editor.TextSource.SearchOptions options, Regex expression, bool clearPrevious);
        bool NeedReplaceCurrent(out Match match);
        bool Replace(string text, string replaceWith);
        bool Replace(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options);
        bool Replace(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expression);
        bool ReplaceAll(string text, string replaceWith, out int count);
        bool ReplaceAll(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, out int count);
        bool ReplaceAll(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expression, out int count);
        bool ReplaceAll(string text, string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Regex expression, out int count, out bool abort);
        bool ReplaceCurrent(string replaceWith, QWhale.Editor.TextSource.SearchOptions options, Match match);
        void ShowNotFound(string caption);
        void StartIncrementalSearch();
        void StartIncrementalSearch(bool backwardSearch);

        bool FirstSearch { get; set; }

        IGotoLineDialog GotoLineDialog { get; set; }

        string IncrementalSearchString { get; }

        bool InIncrementalSearch { get; }

        ISearchDialog SearchDialog { get; set; }

        int SearchLen { get; }

        QWhale.Editor.TextSource.SearchOptions SearchOptions { get; set; }

        Point SearchPos { get; set; }
    }
}

