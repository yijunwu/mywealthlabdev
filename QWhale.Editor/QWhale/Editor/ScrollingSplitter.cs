namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.ComponentModel;

    [ToolboxItem(false)]
    public class ScrollingSplitter : ScrollingButton
    {
        public ScrollingSplitter()
        {
            this.Button.BorderStyle = EditBorderStyle.Fixed3D;
        }
    }
}

