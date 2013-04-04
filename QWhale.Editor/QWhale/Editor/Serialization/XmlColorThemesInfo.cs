namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor.Dialogs;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class XmlColorThemesInfo : ISerializationInfo
    {
        private int activeThemeIndex;
        private XmlColorThemeInfo[] colorThemes;
        private IColorThemes owner;

        public XmlColorThemesInfo()
        {
            this.colorThemes = new XmlColorThemeInfo[0];
            this.activeThemeIndex = -1;
        }

        public XmlColorThemesInfo(IColorThemes owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IColorThemes) owner;
            this.ColorThemes = this.colorThemes;
            this.ActiveThemeIndex = this.activeThemeIndex;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.colorThemes = this.ColorThemes;
                this.activeThemeIndex = this.ActiveThemeIndex;
                foreach (XmlColorThemeInfo info in this.colorThemes)
                {
                    info.Load();
                }
            }
        }

        public bool ShouldSerializeColorTheme()
        {
            return ((this.colorThemes.Length > 0) || ((this.owner != null) && (this.owner.Count > 0)));
        }

        [DefaultValue(-1)]
        public int ActiveThemeIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.activeThemeIndex;
                }
                return this.owner.ActiveThemeIndex;
            }
            set
            {
                this.activeThemeIndex = value;
                if (this.owner != null)
                {
                    this.owner.ActiveThemeIndex = value;
                }
            }
        }

        [XmlArrayItem("ColorTheme"), XmlArray]
        public XmlColorThemeInfo[] ColorThemes
        {
            get
            {
                if (this.owner == null)
                {
                    return this.colorThemes;
                }
                XmlColorThemeInfo[] infoArray = new XmlColorThemeInfo[this.owner.Count];
                for (int i = 0; i < this.owner.Count; i++)
                {
                    infoArray[i] = (XmlColorThemeInfo) this.owner[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.colorThemes = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Clear();
                    new QWhale.Editor.Dialogs.ColorThemes();
                    foreach (XmlColorThemeInfo info in value)
                    {
                        IColorTheme item = new ColorTheme {
                            SerializationInfo = info
                        };
                        this.owner.Add(item);
                    }
                }
            }
        }
    }
}

