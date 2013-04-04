namespace QWhale.Editor
{
    using QWhale.Editor.TextSource;
    using System;
    using System.Drawing;

    public interface IEditNavigate : INavigate
    {
        void MoveCharLeft();
        void MoveCharRight();
        void MoveFileBegin();
        void MoveFileEnd();
        void MoveLineBegin();
        void MoveLineBeginCycled();
        void MoveLineDown();
        void MoveLineEnd();
        void MoveLineEndCycled();
        void MoveLineUp();
        void MovePageDown();
        void MovePageUp();
        void MoveScreenBottom();
        void MoveScreenTop();
        void MoveToBrace();
        void MoveToCloseBrace();
        void MoveToOpenBrace();
        void MoveWordLeft();
        void MoveWordRight();
        bool ProcessEnter();
        bool ProcessShiftTab(Point position);
        bool ProcessTab(Point position);
        void ScrollLineDown();
        void ScrollLineUp();
    }
}

