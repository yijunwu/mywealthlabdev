namespace QWhale.Editor.TextSource
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface IBookMarks : IList<IBookMark>, ICollection<IBookMark>, IEnumerable<IBookMark>, IEnumerable
    {
        void Assign(IBookMarks source);
        bool BlockDeleting(Rectangle rect);
        void ClearAllBookMarks();
        void ClearAllUnnumberedBookmarks();
        void ClearBookMark(int bookMark);
        void ClearBookMark(int line, int bookmark);
        void ClearBookMarks(int line);
        IBookMark FindBookMark(int bookMark);
        IBookMark FindBookMark(string name);
        bool FindBookMark(int bookMark, out Point position);
        int FindBookMark(int bookMark, int line);
        int GetBookMark(int line);
        int GetBookMark(Point startPoint, Point endPoint);
        int GetBookMarks(Point startPoint, Point endPoint, IList<IBookMark> list);
        void GotoBookMark(int bookMark);
        void GotoNextBookMark();
        void GotoPrevBookMark();
        int NextBookMark();
        bool PositionChanged(int x, int y, int deltaX, int deltaY);
        void SetBookMark(IBookMark bookMark);
        void SetBookMark(Point position, int bookMark);
        void SetBookMark(int line, int bookMark);
        void SetBookMark(Point position, int bookMark, string name, string description, string url);
        void ToggleBookMark();
        void ToggleBookMark(IBookMark bookMark);
        void ToggleBookMark(int bookMark);
        void ToggleBookMark(Point position, int bookMark);
        void ToggleBookMark(int line, int bookMark);
        void ToggleBookMark(Point position, int bookMark, string name, string description, string url);
    }
}

