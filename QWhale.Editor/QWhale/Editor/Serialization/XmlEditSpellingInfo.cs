namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;

    public class XmlEditSpellingInfo : ISerializationInfo
    {
        private IEditSpelling owner;
        private string spellColor;

        public XmlEditSpellingInfo()
        {
            this.spellColor = XmlColorInfo.SerializeColor(EditConsts.DefaultSpellForeColor);
        }

        public XmlEditSpellingInfo(IEditSpelling owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IEditSpelling) owner;
            this.SpellColor = this.spellColor;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.spellColor = this.SpellColor;
            }
        }

        public bool ShouldSerializeSpellColor()
        {
            return (this.SpellColor != XmlColorInfo.SerializeColor(EditConsts.DefaultSpellForeColor));
        }

        public string SpellColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.spellColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.SpellColor);
            }
            set
            {
                this.spellColor = value;
                if (this.owner != null)
                {
                    this.owner.SpellColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }
    }
}

