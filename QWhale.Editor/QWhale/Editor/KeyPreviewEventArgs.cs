namespace QWhale.Editor
{
    using System;
    using System.Windows.Forms;

    public class KeyPreviewEventArgs : EventArgs
    {
        public bool Handled;
        public System.Windows.Forms.Message Message;

        public KeyPreviewEventArgs(System.Windows.Forms.Message m)
        {
            this.Message = m;
        }
    }
}

