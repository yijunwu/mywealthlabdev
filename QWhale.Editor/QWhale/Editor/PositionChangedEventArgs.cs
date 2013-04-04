namespace QWhale.Editor
{
    using QWhale.Editor.TextSource;
    using System;

    public class PositionChangedEventArgs : EventArgs
    {
        public int DeltaX;
        public int DeltaY;
        public UpdateReason Reason;

        public PositionChangedEventArgs(UpdateReason reason, int deltaX, int deltaY)
        {
            this.Reason = reason;
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
        }
    }
}

