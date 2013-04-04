namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class CommentInfo : CodeCompletionProvider, ICommentInfo, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        public CommentInfo()
        {
            base.Add(new CommentInfoItem(string.Empty));
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
            return typeof(XmlCommentInfo);
        }

        protected virtual void OnTextChanged()
        {
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCommentInfo(this);
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
                return ((ICommentInfoItem) base[0]).Text;
            }
            set
            {
                if (base.Count > 0)
                {
                    ((ICommentInfoItem) base[0]).Text = value;
                }
                else
                {
                    base.Add(new CommentInfoItem(value));
                }
                this.OnTextChanged();
            }
        }
    }
}

