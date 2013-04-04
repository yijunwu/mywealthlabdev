namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class CodeSnippetLiteral : ICodeSnippetLiteral, ICodeCompletionProviderItem
    {
        private string defaultValue = string.Empty;
        private bool editable = true;
        private string function = string.Empty;
        private string id = string.Empty;
        private string toolTip = string.Empty;
        private string type = string.Empty;

        protected virtual void OnDefaultChanged()
        {
        }

        protected virtual void OnEditableChanged()
        {
        }

        protected virtual void OnFunctionChanged()
        {
        }

        protected virtual void OnIDChanged()
        {
        }

        protected virtual void OnToolTipChanged()
        {
        }

        protected virtual void OnTypeChanged()
        {
        }

        public virtual string Default
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                if (this.defaultValue != value)
                {
                    this.defaultValue = value;
                    this.OnDefaultChanged();
                }
            }
        }

        public virtual bool Editable
        {
            get
            {
                return this.editable;
            }
            set
            {
                if (this.editable != value)
                {
                    this.editable = value;
                    this.OnEditableChanged();
                }
            }
        }

        public virtual string Function
        {
            get
            {
                return this.function;
            }
            set
            {
                if (this.function != value)
                {
                    this.function = value;
                    this.OnFunctionChanged();
                }
            }
        }

        public virtual string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    this.OnIDChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetLiteralInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual string ToolTip
        {
            get
            {
                return this.toolTip;
            }
            set
            {
                if (this.toolTip != value)
                {
                    this.toolTip = value;
                    this.OnToolTipChanged();
                }
            }
        }

        public virtual string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                if (this.type != value)
                {
                    this.type = value;
                    this.OnTypeChanged();
                }
            }
        }
    }
}

