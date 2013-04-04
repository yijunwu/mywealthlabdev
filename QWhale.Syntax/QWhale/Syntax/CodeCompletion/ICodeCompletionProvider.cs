namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public interface ICodeCompletionProvider : IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        event ClosePopupEvent ClosePopup;

        event ShowPopupEvent ShowPopup;

        bool ColumnVisible(int column);
        string GetColumnText(int index, int column);
        string GetName(int index);
        ICodeCompletionProvider GetParent();
        int GetPriority(int index);
        string GetText(int index);
        int IndexOfName(string name, bool caseSensitive);
        void OnClosePopup(object sender, ClosingEventArgs e);
        void OnShowPopup(object sender, ShowingEventArgs e);
        void Sort();
        void Sort(IComparer<ICodeCompletionProviderItem> comparer);

        int ColumnCount { get; }

        string[] Descriptions { get; }

        string EditField { get; set; }

        string EditPath { get; }

        bool FormatDisplayText { get; }

        int[] ImageIndexes { get; }

        ImageList Images { get; set; }

        int SelIndex { get; set; }

        bool ShowDescriptions { get; set; }

        string[] Strings { get; }

        bool UseHtmlFormatting { get; set; }

        bool UseIndent { get; set; }
    }
}

