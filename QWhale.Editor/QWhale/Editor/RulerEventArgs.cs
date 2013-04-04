namespace QWhale.Editor
{
    using System;

    public class RulerEventArgs : EventArgs
    {
        public object Object;

        public RulerEventArgs(object obj)
        {
            this.Object = obj;
        }
    }
}

