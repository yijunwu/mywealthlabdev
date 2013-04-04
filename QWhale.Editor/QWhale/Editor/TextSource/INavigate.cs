namespace QWhale.Editor.TextSource
{
    using System;
    using System.Drawing;

    public interface INavigate
    {
        void DisablePositionUpdate();
        void EnablePositionUpdate();
        void MoveTo(Point position);
        void MoveTo(int x, int y);
        void MoveToChar(int x);
        void MoveToLine(int y);
        void MoveToLine(int y, int linesAbove);
        void Navigate(int deltaX, int deltaY);
        void ResetNavigateOptions();
        Point RestorePosition(int index);
        void SetNavigateOptions(QWhale.Editor.TextSource.NavigateOptions navigateOptions);
        int StorePosition(Point position);
        void ValidatePosition(ref Point position);

        QWhale.Editor.TextSource.NavigateOptions NavigateOptions { get; set; }

        Point Position { get; set; }

        Point PrevPosition { get; }
    }
}

