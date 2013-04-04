namespace QWhale.Editor
{
    using System;

    public class DrawHeaderEventArgs : EventArgs
    {
        public bool Handled;
        public string Tag;
        public string Text;

        public DrawHeaderEventArgs(string tag)
        {
            this.Tag = tag;
        }
    }
}

