namespace QWhale.Syntax
{
    using System;
    using System.Drawing;

    public class ReadFormattedTextEventArgs : EventArgs
    {
        public Color BackColor;
        public System.Drawing.FontStyle FontStyle;
        public Color ForeColor;
        public string Text;
        public object UserData;
    }
}

