namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.ComponentModel;

    public class QuickInfoItem : IQuickInfoItem, ICodeCompletionProviderItem
    {
        private string text = string.Empty;

        public QuickInfoItem(string text)
        {
            this.text = text;
        }

        protected virtual void OnTextChanged()
        {
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlQuickInfoItemInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnTextChanged();
                }
            }
        }
    }
}

