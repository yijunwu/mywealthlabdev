namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IColorThemes : IList<IColorTheme>, ICollection<IColorTheme>, IEnumerable<IColorTheme>, IEnumerable
    {
        IColorTheme ActiveTheme { get; set; }

        int ActiveThemeIndex { get; set; }

        ISerializationInfo SerializationInfo { get; set; }
    }
}

