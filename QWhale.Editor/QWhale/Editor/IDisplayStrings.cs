namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public interface IDisplayStrings : IStringList, IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable, ITabulation, IWordWrap, ITextSearch, IUpdate, IWordBreak, ICollapsable, ITextExport, IExport, ITextImport, IImport
    {
        void BlockDeleting(Rectangle rect);
        Point DisplayPointToPoint(Point position);
        Point DisplayPointToPoint(int x, int y);
        Point DisplayPointToPoint(int x, int y, ref bool lineEnd);
        Point DisplayPointToPoint(int x, int y, bool wrapEnd, bool rangeStart, bool tabEnd);
        short[] GetColorData(int index);
        int GetLexStyle(Point position);
        int GetStringAndColorData(int index, ref string text, ref short[] data);
        bool IsPointCollapsed(Point position, out IRange range);
        bool IsPointVisible(Point position);
        void Notify(NotifyState state, int first, int last);
        Point PointToDisplayPoint(Point position);
        Point PointToDisplayPoint(int x, int y);
        Point PointToDisplayPoint(int x, int y, bool lineEnd);
        void PositionChanged(UpdateReason reason, int x, int y, int deltaX, int deltaY);
        void UpdateNeeded();

        int DisplayCount { get; }

        string this[int index] { get; }

        bool LineEnd { get; set; }

        ITextStrings Lines { get; set; }

        bool Loaded { get; set; }

        int MaxLineWidth { get; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

