namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Windows.Forms;

    public interface IScrollingButton
    {
        void Assign(IScrollingButton source);

        bool AllowCheck { get; set; }

        EditBorderStyle BorderStyle { get; set; }

        SpeedButton Button { get; }

        bool Checked { get; set; }

        string Description { get; set; }

        int GroupIndex { get; set; }

        int ImageIndex { get; set; }

        ImageList Images { get; set; }

        string Name { get; set; }

        IScrolling Scrolling { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        bool Visible { get; set; }
    }
}

