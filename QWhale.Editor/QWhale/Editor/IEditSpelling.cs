namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using System;
    using System.Drawing;

    public interface IEditSpelling : ISpelling
    {
        void Assign(IEditSpelling source);
        void ResetSpellColor();

        bool CheckSpelling { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        Color SpellColor { get; set; }
    }
}

