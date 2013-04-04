namespace QWhale.Editor
{
    using System;

    public class DrawUserMarginEventArgs : DrawHeaderEventArgs
    {
        public int Line;

        public DrawUserMarginEventArgs(string tag, int line) : base(tag)
        {
            this.Line = line;
        }
    }
}

