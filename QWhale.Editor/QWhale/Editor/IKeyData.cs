namespace QWhale.Editor
{
    using System;
    using System.Windows.Forms;

    public interface IKeyData
    {
        KeyEvent Action { get; set; }

        KeyEventEx ActionEx { get; set; }

        string EventName { get; set; }

        System.Windows.Forms.Keys Keys { get; set; }

        int LeaveState { get; set; }

        object Param { get; set; }

        int State { get; set; }
    }
}

