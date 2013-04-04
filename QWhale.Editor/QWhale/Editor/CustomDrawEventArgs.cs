namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public class CustomDrawEventArgs : EventArgs
    {
        public IDrawInfo DrawInfo;
        public QWhale.Editor.DrawStage DrawStage;
        public QWhale.Editor.DrawState DrawState;
        public bool Handled;
        public IPainter Painter;
        public Rectangle Rect;
    }
}

