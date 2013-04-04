namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class WhiteSpace : IWhiteSpace, IUpdate
    {
        private string eofString;
        private char eofSymbol;
        private string eolString;
        private char eolSymbol;
        private ISyntaxEdit owner;
        private string spaceString;
        private char spaceSymbol;
        private Color symbolColor;
        private string tabString;
        private char tabSymbol;
        private int updateCount;
        private bool visible;
        private string wordWrapString;
        private char wordWrapSymbol;

        public WhiteSpace()
        {
            this.tabSymbol = EditConsts.TabSymbol;
            this.spaceSymbol = EditConsts.SpaceSymbol;
            this.eolSymbol = EditConsts.EolSymbol;
            this.eofSymbol = EditConsts.EofSymbol;
            this.wordWrapSymbol = EditConsts.WordWrapSymbol;
            this.symbolColor = EditConsts.DefaultWhiteSpaceForeColor;
            this.spaceString = new string(this.spaceSymbol, 1);
            this.tabString = new string(this.tabSymbol, 1);
            this.eolString = new string(this.eolSymbol, 1);
            this.eofString = new string(this.eofSymbol, 1);
            this.wordWrapString = new string(this.wordWrapSymbol, 1);
        }

        public WhiteSpace(ISyntaxEdit owner) : this()
        {
            this.owner = owner;
        }

        public virtual void Assign(IWhiteSpace source)
        {
            this.BeginUpdate();
            try
            {
                this.SpaceSymbol = source.SpaceSymbol;
                this.TabSymbol = source.TabSymbol;
                this.EofSymbol = source.EofSymbol;
                this.EolSymbol = source.EolSymbol;
                this.WordWrapSymbol = source.WordWrapSymbol;
                this.Visible = source.Visible;
                this.SymbolColor = source.SymbolColor;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public virtual int BeginUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual int DisableUpdate()
        {
            this.updateCount++;
            return this.updateCount;
        }

        public virtual int EnableUpdate()
        {
            this.updateCount--;
            return this.updateCount;
        }

        public virtual int EndUpdate()
        {
            this.updateCount--;
            if (this.updateCount == 0)
            {
                this.Update();
            }
            return this.updateCount;
        }

        protected virtual void OnEofSymbolChanged()
        {
            this.eofString = new string(this.eofSymbol, 1);
            this.Update();
        }

        protected virtual void OnEolSymbolChanged()
        {
            this.eolString = new string(this.eolSymbol, 1);
            this.Update();
        }

        protected virtual void OnSpaceSymbolchanged()
        {
            this.spaceString = new string(this.spaceSymbol, 1);
            this.Update();
        }

        protected virtual void OnSymbolColorChanged()
        {
            this.Update();
        }

        protected virtual void OnTabSymbolChanged()
        {
            this.tabString = new string(this.tabSymbol, 1);
            this.Update();
        }

        protected virtual void OnVisibleChanged()
        {
            this.Update();
        }

        protected virtual void OnWordWrapSymbolChanged()
        {
            this.wordWrapString = new string(this.wordWrapSymbol, 1);
            this.Update();
        }

        public virtual void ResetEofSymbol()
        {
            this.EofSymbol = EditConsts.EofSymbol;
        }

        public virtual void ResetEolSymbol()
        {
            this.EolSymbol = EditConsts.EolSymbol;
        }

        public virtual void ResetSpaceSymbol()
        {
            this.SpaceSymbol = EditConsts.SpaceSymbol;
        }

        public virtual void ResetSymbolColor()
        {
            this.SymbolColor = EditConsts.DefaultWhiteSpaceForeColor;
        }

        public virtual void ResetTabSymbol()
        {
            this.TabSymbol = EditConsts.TabSymbol;
        }

        public virtual void ResetVisible()
        {
            this.Visible = false;
        }

        public virtual void ResetWordWrapSymbol()
        {
            this.WordWrapSymbol = EditConsts.WordWrapSymbol;
        }

        public bool ShouldSerializeEofSymbol()
        {
            return (this.eofSymbol != EditConsts.EofSymbol);
        }

        public bool ShouldSerializeEolSymbol()
        {
            return (this.eolSymbol != EditConsts.EolSymbol);
        }

        public bool ShouldSerializeSpaceSymbol()
        {
            return (this.spaceSymbol != EditConsts.SpaceSymbol);
        }

        public bool ShouldSerializeSymbolColor()
        {
            return (this.symbolColor != EditConsts.DefaultWhiteSpaceForeColor);
        }

        public bool ShouldSerializeTabSymbol()
        {
            return (this.tabSymbol != EditConsts.TabSymbol);
        }

        public bool ShouldSerializeWordWrapSymbol()
        {
            return (this.wordWrapSymbol != EditConsts.WordWrapSymbol);
        }

        public void Update()
        {
            if ((this.updateCount == 0) && (this.owner != null))
            {
                this.owner.Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string EofString
        {
            get
            {
                return this.eofString;
            }
        }

        [Description("Gets or sets character that specifies end of file.")]
        public virtual char EofSymbol
        {
            get
            {
                return this.eofSymbol;
            }
            set
            {
                if (this.eofSymbol != value)
                {
                    this.eofSymbol = value;
                    this.OnEofSymbolChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual string EolString
        {
            get
            {
                return this.eolString;
            }
        }

        [Description("Gets or sets character that specifies end of line.")]
        public virtual char EolSymbol
        {
            get
            {
                return this.eolSymbol;
            }
            set
            {
                if (this.eolSymbol != value)
                {
                    this.eolSymbol = value;
                    this.OnEolSymbolChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlWhiteSpaceInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SpaceString
        {
            get
            {
                return this.spaceString;
            }
        }

        [Description("Gets or sets character that introduces special symbol to paint instead of the space character.")]
        public virtual char SpaceSymbol
        {
            get
            {
                return this.spaceSymbol;
            }
            set
            {
                if (this.spaceSymbol != value)
                {
                    this.spaceSymbol = value;
                    this.OnSpaceSymbolchanged();
                }
            }
        }

        [Description("Gets or sets color used to paint special symbols.")]
        public virtual Color SymbolColor
        {
            get
            {
                return this.symbolColor;
            }
            set
            {
                if (this.symbolColor != value)
                {
                    this.symbolColor = value;
                    this.OnSymbolColorChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string TabString
        {
            get
            {
                return this.tabString;
            }
        }

        [Description("Gets or sets character that introduces special symbol to paint instead of the tab character.")]
        public virtual char TabSymbol
        {
            get
            {
                return this.tabSymbol;
            }
            set
            {
                if (this.tabSymbol != value)
                {
                    this.tabSymbol = value;
                    this.OnTabSymbolChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether white space symbols are visible in the contol's text content.")]
        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnVisibleChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual string WordWrapString
        {
            get
            {
                return this.wordWrapString;
            }
        }

        [Description("Gets or sets character that specifies wrapped line.")]
        public virtual char WordWrapSymbol
        {
            get
            {
                return this.wordWrapSymbol;
            }
            set
            {
                if (this.wordWrapSymbol != value)
                {
                    this.wordWrapSymbol = value;
                    this.OnWordWrapSymbolChanged();
                }
            }
        }
    }
}

