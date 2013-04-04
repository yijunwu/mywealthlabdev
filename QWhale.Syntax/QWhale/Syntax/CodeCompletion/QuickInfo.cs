namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class QuickInfo : CodeCompletionProvider, IQuickInfo, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        public QuickInfo()
        {
            this.UseHtmlFormatting = true;
            base.Add(new QuickInfoItem(string.Empty));
        }

        public override string GetName(int index)
        {
            return this.Text;
        }

        public override string GetText(int index)
        {
            return this.Text;
        }

        protected override Type GetXmlType()
        {
            return typeof(XmlQuickInfo);
        }

        protected virtual void OnTextChanged()
        {
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlQuickInfo(this);
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
                if (base.Count <= 0)
                {
                    return string.Empty;
                }
                return ((IQuickInfoItem) base[0]).Text;
            }
            set
            {
                if (base.Count > 0)
                {
                    ((IQuickInfoItem) base[0]).Text = value;
                }
                else
                {
                    base.Add(new QuickInfoItem(value));
                }
                this.OnTextChanged();
            }
        }
    }
}

