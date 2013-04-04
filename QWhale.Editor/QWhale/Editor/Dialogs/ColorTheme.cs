namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;

    public class ColorTheme : IColorTheme
    {
        private System.Drawing.Font font;
        private ILexStyles lexStyles;
        private string name;
        private bool readOnly;

        public ColorTheme()
        {
            this.name = string.Empty;
            this.font = new System.Drawing.Font(FontFamily.GenericMonospace.Name, 10f, FontStyle.Regular);
            this.lexStyles = new QWhale.Syntax.Lexer.LexStyles(null);
        }

        public ColorTheme(string name, bool readOnly, System.Drawing.Font Font, ILexStyles lexStyles)
        {
            this.name = string.Empty;
            this.font = new System.Drawing.Font(FontFamily.GenericMonospace.Name, 10f, FontStyle.Regular);
            this.lexStyles = new QWhale.Syntax.Lexer.LexStyles(null);
            this.name = name;
            this.readOnly = readOnly;
            this.font = Font;
            this.LexStyles = lexStyles;
        }

        protected virtual void OnFontChanged()
        {
        }

        protected virtual void OnLexStylesChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnReadonlyChanged()
        {
        }

        public virtual System.Drawing.Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                if (this.font != value)
                {
                    this.font = value;
                    this.OnFontChanged();
                }
            }
        }

        public ILexStyle this[string name]
        {
            get
            {
                return this.lexStyles.FindLexStyle(name);
            }
        }

        public virtual ILexStyles LexStyles
        {
            get
            {
                return this.lexStyles;
            }
            set
            {
                this.lexStyles.Clear();
                foreach (ILexStyle style in value)
                {
                    ILexStyle item = new LexStyle(null);
                    item.Assign(style);
                    this.lexStyles.Add(item);
                }
            }
        }

        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNameChanged();
                }
            }
        }

        public virtual bool Readonly
        {
            get
            {
                return this.readOnly;
            }
            set
            {
                if (this.readOnly != value)
                {
                    this.readOnly = value;
                    this.OnReadonlyChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlColorThemeInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

