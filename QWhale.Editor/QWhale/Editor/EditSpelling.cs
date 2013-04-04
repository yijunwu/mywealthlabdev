namespace QWhale.Editor
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;

    public class EditSpelling : IEditSpelling, ISpelling
    {
        private ISyntaxEdit owner;
        private Color spellColor = EditConsts.DefaultSpellForeColor;

        [Browsable(false)]
        public event WordSpellEvent WordSpell
        {
            add
            {
                if (this.owner != null)
                {
                    this.owner.Source.WordSpell += value;
                }
            }
            remove
            {
                if (this.owner != null)
                {
                    this.owner.Source.WordSpell -= value;
                }
            }
        }

        public EditSpelling(ISyntaxEdit owner)
        {
            this.owner = owner;
        }

        public virtual void Assign(IEditSpelling source)
        {
            this.CheckSpelling = source.CheckSpelling;
            this.SpellColor = source.SpellColor;
        }

        public virtual bool IsWordCorrect(string text)
        {
            return ((this.owner == null) || this.owner.Source.IsWordCorrect(text));
        }

        protected virtual void OnCheckSpellingChanged()
        {
        }

        protected virtual void OnSpellColorChanged()
        {
            if (this.CheckSpelling || (this.owner != null))
            {
                this.owner.Invalidate();
            }
        }

        public virtual void ResetCheckSpelling()
        {
            this.CheckSpelling = false;
        }

        public virtual void ResetSpellColor()
        {
            this.SpellColor = EditConsts.DefaultSpellForeColor;
        }

        public bool ShouldSerializeSpellColor()
        {
            return (this.spellColor != EditConsts.DefaultSpellForeColor);
        }

        [DefaultValue(false), Description("Gets or sets a value indicating whether the document can check spelling for its content.")]
        public virtual bool CheckSpelling
        {
            get
            {
                if (this.owner == null)
                {
                    return false;
                }
                return this.owner.Source.CheckSpelling;
            }
            set
            {
                if ((this.owner != null) && (this.owner.Source.CheckSpelling != value))
                {
                    this.owner.Source.CheckSpelling = value;
                    this.OnCheckSpellingChanged();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlEditSpellingInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        [Description("Gets or sets a value representing color to draw wavy underlines under mispelled words.")]
        public virtual Color SpellColor
        {
            get
            {
                return this.spellColor;
            }
            set
            {
                if (this.spellColor != value)
                {
                    this.spellColor = value;
                    this.OnSpellColorChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual Hashtable SpellTable
        {
            get
            {
                if (this.owner == null)
                {
                    return null;
                }
                return this.owner.Source.SpellTable;
            }
        }
    }
}

