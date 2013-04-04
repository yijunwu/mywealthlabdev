namespace QWhale.Editor.TextSource
{
    using QWhale.Common;
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public interface ITextSource : IEdit, INavigate, IUndo, ITextNotify, INotify, IUpdate, INotifier, ITextImport, IImport, ITextExport, IExport, IHyperText, ISpelling, IBraceMatching, ITextParsing, ITextSnippets, ITextErrors
    {
        Point AbsolutePositionToTextPoint(int position);
        void Clear();
        IStringItem CreateStringItem(string s);
        int GetCharIndexFromPosition(Point position);
        Point GetPositionFromCharIndex(int charIndex);
        int TextPointToAbsolutePosition(Point position);

        object ActiveEdit { get; set; }

        IBookMarks BookMarks { get; set; }

        IList<ISyntaxEdit> Edits { get; }

        string FileName { get; set; }

        ITextStrings Lines { get; set; }

        ILineStyles LineStyles { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        string Text { get; set; }
    }
}

