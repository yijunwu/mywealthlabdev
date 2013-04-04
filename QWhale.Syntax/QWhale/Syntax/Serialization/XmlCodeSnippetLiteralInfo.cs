namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;

    public class XmlCodeSnippetLiteralInfo : ISerializationInfo
    {
        private string defaultValue;
        private bool editable;
        private string function;
        private string id;
        private ICodeSnippetLiteral owner;
        private string toolTip;
        private string type;

        public XmlCodeSnippetLiteralInfo()
        {
            this.defaultValue = string.Empty;
            this.function = string.Empty;
            this.id = string.Empty;
            this.toolTip = string.Empty;
            this.editable = true;
            this.type = string.Empty;
        }

        public XmlCodeSnippetLiteralInfo(ICodeSnippetLiteral owner)
        {
            this.defaultValue = string.Empty;
            this.function = string.Empty;
            this.id = string.Empty;
            this.toolTip = string.Empty;
            this.editable = true;
            this.type = string.Empty;
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetLiteral) owner;
            this.Default = this.defaultValue;
            this.Function = this.function;
            this.ID = this.id;
            this.ToolTip = this.toolTip;
            this.Editable = this.editable;
            this.Type = this.type;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.defaultValue = this.Default;
                this.function = this.Function;
                this.id = this.ID;
                this.toolTip = this.ToolTip;
                this.editable = this.Editable;
                this.type = this.Type;
            }
        }

        [DefaultValue("")]
        public string Default
        {
            get
            {
                if (this.owner == null)
                {
                    return this.defaultValue;
                }
                return this.owner.Default;
            }
            set
            {
                this.defaultValue = value;
                if (this.owner != null)
                {
                    this.owner.Default = value;
                }
            }
        }

        [DefaultValue(true)]
        public bool Editable
        {
            get
            {
                if (this.owner == null)
                {
                    return this.editable;
                }
                return this.owner.Editable;
            }
            set
            {
                this.editable = value;
                if (this.owner != null)
                {
                    this.owner.Editable = value;
                }
            }
        }

        [DefaultValue("")]
        public string Function
        {
            get
            {
                if (this.owner == null)
                {
                    return this.function;
                }
                return this.owner.Function;
            }
            set
            {
                this.function = value;
                if (this.owner != null)
                {
                    this.owner.Function = value;
                }
            }
        }

        [DefaultValue("")]
        public string ID
        {
            get
            {
                if (this.owner == null)
                {
                    return this.id;
                }
                return this.owner.ID;
            }
            set
            {
                this.id = value;
                if (this.owner != null)
                {
                    this.owner.ID = value;
                }
            }
        }

        [DefaultValue("")]
        public string ToolTip
        {
            get
            {
                if (this.owner == null)
                {
                    return this.toolTip;
                }
                return this.owner.ToolTip;
            }
            set
            {
                this.toolTip = value;
                if (this.owner != null)
                {
                    this.owner.ToolTip = value;
                }
            }
        }

        [DefaultValue("")]
        public string Type
        {
            get
            {
                if (this.owner == null)
                {
                    return this.type;
                }
                return this.owner.Type;
            }
            set
            {
                this.type = value;
                if (this.owner != null)
                {
                    this.owner.Type = value;
                }
            }
        }
    }
}

