namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

    public class CodeSnippets : CodeCompletionProvider, ICodeSnippets, ICodeSnippetsProvider, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        private ICodeSnippetMember parent;

        public CodeSnippets()
        {
            this.ShowDescriptions = true;
            this.UseIndent = false;
            this.EditField = StringConsts.CodeSnippetsEditField;
        }

        public CodeSnippets(ICodeSnippetMember parent) : this()
        {
            this.parent = parent;
        }

        public virtual ICodeSnippet AddSnippet()
        {
            ICodeSnippet item = new CodeSnippet(this);
            base.Add(item);
            return item;
        }

        public override bool ColumnVisible(int column)
        {
            return true;
        }

        public virtual ICodeSnippet FindByShortcut(string shortcut, bool caseSensitive)
        {
            foreach (ICodeSnippet snippet in this)
            {
                if (string.Compare(shortcut, snippet.Header.Shortcut, !caseSensitive) == 0)
                {
                    return snippet;
                }
            }
            return null;
        }

        public override string GetColumnText(int index, int column)
        {
            if (column != 0)
            {
                return string.Empty;
            }
            return this.GetName(index);
        }

        public override string GetDescription(int index)
        {
            return this[index].Description;
        }

        public override int GetImageIndex(int index)
        {
            return this[index].ImageIndex;
        }

        public override string GetName(int index)
        {
            return this[index].Header.Title;
        }

        public override ICodeCompletionProvider GetParent()
        {
            if (this.parent == null)
            {
                return null;
            }
            return this.parent.Parent;
        }

        public override string GetText(int index)
        {
            return this[index].Code.Code;
        }

        protected override System.Type GetXmlType()
        {
            return typeof(XmlCodeSnippetsInfo);
        }

        public virtual ICodeSnippet InsertSnippet(int index)
        {
            ICodeSnippet item = new CodeSnippet(this);
            base.Insert(index, item);
            return item;
        }

        public override int ColumnCount
        {
            get
            {
                return 1;
            }
        }

        public override string EditPath
        {
            get
            {
                if (this.parent == null)
                {
                    return string.Empty;
                }
                return this.parent.EditPath;
            }
        }

        public override ImageList Images
        {
            get
            {
                if (base.Images != null)
                {
                    return base.Images;
                }
                if (this.GetParent() == null)
                {
                    return null;
                }
                return this.GetParent().Images;
            }
            set
            {
                base.Images = value;
            }
        }

        public ICodeSnippet this[int index]
        {
            get
            {
                return (base[index] as ICodeSnippet);
            }
            set
            {
                base[index] = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlCodeSnippetsInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

