namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface IEditLineStyle
    {
        void Assign(IEditLineStyle source);
        Color GetBackColor(Color color);
        Color GetForeColor(Color color);
        void ResetBackColor();
        void ResetForeColor();
        void ResetImageIndex();
        void ResetOptions();
        void ResetPenColor();

        Color BackColor { get; set; }

        Color ForeColor { get; set; }

        int ImageIndex { get; set; }

        string Name { get; set; }

        LineStyleOptions Options { get; set; }

        Color PenColor { get; set; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

