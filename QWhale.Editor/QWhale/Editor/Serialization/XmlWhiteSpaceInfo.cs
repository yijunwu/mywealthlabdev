namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class XmlWhiteSpaceInfo : ISerializationInfo
    {
        private char eofSymbol;
        private char eolSymbol;
        private IWhiteSpace owner;
        private char spaceSymbol;
        private string symbolColor;
        private char tabSymbol;
        private bool visible;
        private char wordWrapSymbol;

        public XmlWhiteSpaceInfo()
        {
            this.tabSymbol = EditConsts.TabSymbol;
            this.spaceSymbol = EditConsts.SpaceSymbol;
            this.eolSymbol = EditConsts.EolSymbol;
            this.eofSymbol = EditConsts.EofSymbol;
            this.wordWrapSymbol = EditConsts.WordWrapSymbol;
            this.symbolColor = XmlColorInfo.SerializeColor(EditConsts.DefaultWhiteSpaceForeColor);
        }

        public XmlWhiteSpaceInfo(IWhiteSpace owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IWhiteSpace) owner;
            this.Visible = this.visible;
            this.TabSymbol = this.tabSymbol;
            this.SpaceSymbol = this.spaceSymbol;
            this.EolSymbol = this.eolSymbol;
            this.EofSymbol = this.eofSymbol;
            this.SymbolColor = this.symbolColor;
            this.WordWrapSymbol = this.wordWrapSymbol;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.visible = this.Visible;
                this.tabSymbol = this.TabSymbol;
                this.spaceSymbol = this.SpaceSymbol;
                this.eolSymbol = this.EolSymbol;
                this.eofSymbol = this.EofSymbol;
                this.symbolColor = this.SymbolColor;
                this.wordWrapSymbol = this.WordWrapSymbol;
            }
        }

        public bool ShouldSerializeEofSymbol()
        {
            return (this.EofSymbol != EditConsts.EofSymbol);
        }

        public bool ShouldSerializeEolSymbol()
        {
            return (this.EolSymbol != EditConsts.EolSymbol);
        }

        public bool ShouldSerializeSpaceSymbol()
        {
            return (this.SpaceSymbol != EditConsts.SpaceSymbol);
        }

        public bool ShouldSerializeSymbolColor()
        {
            return (this.SymbolColor != XmlColorInfo.SerializeColor(EditConsts.DefaultWhiteSpaceForeColor));
        }

        public bool ShouldSerializeTabSymbol()
        {
            return (this.TabSymbol != EditConsts.TabSymbol);
        }

        public bool ShouldSerializeWordWrapSymbol()
        {
            return (this.WordWrapSymbol != EditConsts.WordWrapSymbol);
        }

        public char EofSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.eofSymbol;
                }
                return this.owner.EofSymbol;
            }
            set
            {
                this.eofSymbol = value;
                if (this.owner != null)
                {
                    this.owner.EofSymbol = value;
                }
            }
        }

        public char EolSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.eolSymbol;
                }
                return this.owner.EolSymbol;
            }
            set
            {
                this.eolSymbol = value;
                if (this.owner != null)
                {
                    this.owner.EolSymbol = value;
                }
            }
        }

        public char SpaceSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.spaceSymbol;
                }
                return this.owner.SpaceSymbol;
            }
            set
            {
                this.spaceSymbol = value;
                if (this.owner != null)
                {
                    this.owner.SpaceSymbol = value;
                }
            }
        }

        public string SymbolColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.symbolColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.SymbolColor);
            }
            set
            {
                this.symbolColor = value;
                if (this.owner != null)
                {
                    this.owner.SymbolColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public char TabSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.tabSymbol;
                }
                return this.owner.TabSymbol;
            }
            set
            {
                this.tabSymbol = value;
                if (this.owner != null)
                {
                    this.owner.TabSymbol = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool Visible
        {
            get
            {
                if (this.owner == null)
                {
                    return this.visible;
                }
                return this.owner.Visible;
            }
            set
            {
                this.visible = value;
                if (this.owner != null)
                {
                    this.owner.Visible = value;
                }
            }
        }

        public char WordWrapSymbol
        {
            get
            {
                if (this.owner == null)
                {
                    return this.wordWrapSymbol;
                }
                return this.owner.WordWrapSymbol;
            }
            set
            {
                this.wordWrapSymbol = value;
                if (this.owner != null)
                {
                    this.owner.WordWrapSymbol = value;
                }
            }
        }
    }
}

