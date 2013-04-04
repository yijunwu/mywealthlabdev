namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface IOutlining : ICollapsable
    {
        void Assign(IOutlining source);
        void OutlineText();
        void ResetOutlineColor();
        void UnOutlineText();

        bool AllowOutlining { get; set; }

        Color OutlineColor { get; set; }

        QWhale.Editor.OutlineOptions OutlineOptions { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        bool UseRoundRect { get; set; }
    }
}

