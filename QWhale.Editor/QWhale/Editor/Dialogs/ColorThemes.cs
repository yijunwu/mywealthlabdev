namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class ColorThemes : List<IColorTheme>, IColorThemes, IList<IColorTheme>, ICollection<IColorTheme>, IEnumerable<IColorTheme>, IEnumerable
    {
        private int activeThemeIndex = -1;

        protected virtual void OnActiveThemeChanged()
        {
        }

        protected virtual void OnActiveThemeIndexChanged()
        {
        }

        public virtual IColorTheme ActiveTheme
        {
            get
            {
                if (this.activeThemeIndex < 0)
                {
                    return null;
                }
                return base[this.activeThemeIndex];
            }
            set
            {
                if (base[this.activeThemeIndex] != value)
                {
                    base[this.activeThemeIndex] = value;
                    this.OnActiveThemeChanged();
                }
            }
        }

        public virtual int ActiveThemeIndex
        {
            get
            {
                return this.activeThemeIndex;
            }
            set
            {
                if (this.activeThemeIndex != value)
                {
                    this.activeThemeIndex = value;
                    this.OnActiveThemeIndexChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlColorThemesInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

