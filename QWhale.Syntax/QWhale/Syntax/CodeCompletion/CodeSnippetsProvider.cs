namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class CodeSnippetsProvider : CodeCompletionProvider, ICodeSnippetsProvider, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        private ICodeSnippetMember parent;

        public CodeSnippetsProvider()
        {
            this.ShowDescriptions = true;
            this.UseIndent = false;
            this.EditField = StringConsts.CodeSnippetsEditField;
        }

        public CodeSnippetsProvider(ICodeSnippetMember parent) : this()
        {
            this.parent = parent;
        }

        public override bool ColumnVisible(int column)
        {
            return true;
        }

        public virtual ICodeSnippet FindByShortcut(string shortcut, bool caseSensitive)
        {
            foreach (object obj2 in this)
            {
                if (obj2 is ICodeSnippet)
                {
                    ICodeSnippet snippet = (ICodeSnippet) obj2;
                    if (string.Compare(shortcut, snippet.Header.Shortcut, !caseSensitive) == 0)
                    {
                        return snippet;
                    }
                }
                else if (obj2 is ICodeSnippetMember)
                {
                    ICodeSnippetMember member = (ICodeSnippetMember) obj2;
                    ICodeSnippet snippet2 = member.Snippets.FindByShortcut(shortcut, caseSensitive);
                    if (snippet2 != null)
                    {
                        return snippet2;
                    }
                    snippet2 = member.Members.FindByShortcut(shortcut, caseSensitive);
                    if (snippet2 != null)
                    {
                        return snippet2;
                    }
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
            object obj2 = base[index];
            if (obj2 is ICodeSnippet)
            {
                return ((ICodeSnippet) obj2).Description;
            }
            return string.Empty;
        }

        public override int GetImageIndex(int index)
        {
            object obj2 = base[index];
            if (obj2 is ICodeSnippet)
            {
                return ((ICodeSnippet) obj2).ImageIndex;
            }
            if (obj2 is ICodeSnippetMember)
            {
                return ((ICodeSnippetMember) obj2).ImageIndex;
            }
            return -1;
        }

        public override string GetName(int index)
        {
            object obj2 = base[index];
            if (obj2 is ICodeSnippet)
            {
                return ((ICodeSnippet) obj2).Header.Title;
            }
            if (obj2 is ICodeSnippetMember)
            {
                return ((ICodeSnippetMember) obj2).Name;
            }
            return string.Empty;
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
            object obj2 = base[index];
            if (obj2 is ICodeSnippet)
            {
                return ((ICodeSnippet) obj2).Code.Code;
            }
            return string.Empty;
        }

        public override void OnClosePopup(object sender, ClosingEventArgs e)
        {
            if ((e.Accepted && (this.SelIndex >= 0)) && (this.SelIndex < base.Count))
            {
                object obj2 = base[this.SelIndex];
                if (obj2 is ICodeSnippetMember)
                {
                    e.Provider = ((ICodeSnippetMember) obj2).SnippetsAndMembers;
                    e.Handled = e.Provider.Count > 0;
                }
            }
            base.OnClosePopup(sender, e);
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
    }
}

