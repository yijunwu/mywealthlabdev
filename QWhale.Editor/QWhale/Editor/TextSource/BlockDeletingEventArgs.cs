namespace QWhale.Editor.TextSource
{
    using System;
    using System.Drawing;

    public class BlockDeletingEventArgs : EventArgs
    {
        public Rectangle Rect;

        public BlockDeletingEventArgs(Rectangle rect)
        {
            this.Rect = rect;
        }
    }
}

