namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CodeCompletionRepository : ICodeCompletionRepository
    {
        private bool caseSensitive;
        private DescriptionLookupEventArgs descriptionLookupArgs;
        private bool fillBaseMembers = true;
        private MemberLookupEventArgs memberLookupArgs;
        private Hashtable membersTable;
        private Hashtable snippets;
        private ISyntaxTree syntaxTree;
        private List<ISyntaxTree> syntaxTrees;

        public event DescriptionLookupEvent DescriptionLookup;

        public event MemberLookupEvent MemberLookup;

        public CodeCompletionRepository(bool caseSensitive, ISyntaxTree syntaxTree)
        {
            this.caseSensitive = caseSensitive;
            this.syntaxTree = syntaxTree;
            this.syntaxTrees = new List<ISyntaxTree>();
            this.membersTable = new Hashtable();
            this.snippets = new Hashtable();
            this.memberLookupArgs = new MemberLookupEventArgs(null, string.Empty);
            this.descriptionLookupArgs = new DescriptionLookupEventArgs(null, string.Empty);
        }

        protected virtual IListMember AddMember(IListMembers members, string name, int index)
        {
            IListMember member = members.AddListMember();
            member.Name = name;
            member.MemberType = index;
            member.ImageIndex = index;
            if (!this.ShouldDuplicate(members))
            {
                this.membersTable[this.GetMemberKey(name, index)] = member;
            }
            return member;
        }

        protected virtual bool CanAddMember(IListMembers members, string name, int index, bool overloads)
        {
            if (name == string.Empty)
            {
                return false;
            }
            if (this.ShouldDuplicate(members))
            {
                return true;
            }
            object obj2 = this.membersTable[this.GetMemberKey(name, index)];
            if ((obj2 != null) && overloads)
            {
                IListMember member1 = (IListMember) obj2;
                member1.Overloads++;
            }
            return (obj2 == null);
        }

        protected virtual void DoFillMembers(ISyntaxNode node, Point position, IListMembers members, IList<ISyntaxNode> types, object member, string name, CodeCompletionScope scope, ref int selIndex)
        {
        }

        protected virtual object DoFindDeclaration(string text, ISyntaxNode node, ISyntaxNode refNode, Point position, int paramCount)
        {
            return null;
        }

        public virtual void FillMember(IListMembers members, object member, string name, CodeCompletionScope scope)
        {
        }

        public virtual void FillMember(IListMembers members, object member, string name, int paramIndex, CodeCompletionScope scope)
        {
            this.membersTable.Clear();
            this.FillMember(members, member, name, scope);
            this.UpdateMethodParams(members, paramIndex);
        }

        public virtual void FillMembers(ISyntaxNode node, Point position, IListMembers members, object member, string name, CodeCompletionScope scope, ref int selIndex)
        {
            this.membersTable.Clear();
            IList<ISyntaxNode> types = new List<ISyntaxNode>();
            this.DoFillMembers(node, position, members, types, member, name, scope, ref selIndex);
        }

        public virtual object FindDeclaration(string text, ISyntaxNode node, Point position)
        {
            return this.DoFindDeclaration(text, node, node, position, 0);
        }

        public virtual int FindReferences(ISyntaxNode node, ISyntaxNodes references)
        {
            references.Clear();
            references.Add(node);
            return 0;
        }

        public virtual ICodeSnippetsProvider GetCodeSnippets(string language)
        {
            return null;
        }

        public virtual string GetDescription(IListMembers members, ISyntaxNode node, object member, string name, bool fullDescription)
        {
            if (this.DescriptionLookup != null)
            {
                this.descriptionLookupArgs.Member = member;
                this.descriptionLookupArgs.Name = name;
                this.descriptionLookupArgs.Description = string.Empty;
                this.DescriptionLookup(this, this.descriptionLookupArgs);
                return this.descriptionLookupArgs.Description;
            }
            return string.Empty;
        }

        protected virtual object GetMemberKey(string name, int index)
        {
            return (index.ToString() + "|" + name);
        }

        public virtual object GetMemberType(ISyntaxNode node, Point position, object member, string name, out CodeCompletionScope scope)
        {
            if (this.MemberLookup != null)
            {
                this.memberLookupArgs.Member = member;
                this.memberLookupArgs.Name = name;
                this.memberLookupArgs.Result = null;
                this.memberLookupArgs.Scope = CodeCompletionScope.None;
                this.MemberLookup(this, this.memberLookupArgs);
                if (this.memberLookupArgs.Result != null)
                {
                    scope = this.memberLookupArgs.Scope;
                    return this.memberLookupArgs.Result;
                }
            }
            scope = CodeCompletionScope.None;
            return null;
        }

        public virtual object GetMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.None;
            return null;
        }

        public virtual object GetMethodType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out int paramIndex, out int paramCount, out CodeCompletionScope scope)
        {
            paramIndex = -1;
            paramCount = 0;
            scope = CodeCompletionScope.None;
            return null;
        }

        public virtual object GetNodeType(string text, ISyntaxNode node, Point position)
        {
            return this.DoFindDeclaration(text, node, node, position, 0);
        }

        public virtual int GetPriority(object member)
        {
            return 0;
        }

        public virtual object GetSpecialMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out CodeCompletionScope scope)
        {
            scope = CodeCompletionScope.None;
            return null;
        }

        protected virtual void OnFillBaseMembersChanged()
        {
        }

        public virtual void RegisterSnippet(string snippet, bool isStatement)
        {
            this.snippets.Add(snippet, isStatement);
        }

        public virtual void RegisterSyntaxTree(ISyntaxTree tree)
        {
            this.syntaxTrees.Add(tree);
        }

        protected virtual bool ShouldDuplicate(IListMembers members)
        {
            return (members is IParameterInfo);
        }

        public virtual bool UnregisterSnippet(string snippet)
        {
            if (this.snippets.Contains(snippet))
            {
                this.snippets.Remove(snippet);
                return true;
            }
            return false;
        }

        public virtual bool UnregisterSyntaxTree(ISyntaxTree tree)
        {
            int index = this.syntaxTrees.IndexOf(tree);
            if (index >= 0)
            {
                this.syntaxTrees.RemoveAt(index);
            }
            return (index >= 0);
        }

        protected virtual void UpdateMethodParams(IListMember member, int paramIndex)
        {
            member.CurrentParamIndex = paramIndex;
        }

        protected virtual void UpdateMethodParams(IListMembers members, int paramIndex)
        {
            foreach (IListMember member in members)
            {
                this.UpdateMethodParams(member, paramIndex);
            }
        }

        public virtual bool CaseSensitive
        {
            get
            {
                return this.caseSensitive;
            }
        }

        public virtual bool FillBaseMembers
        {
            get
            {
                return this.fillBaseMembers;
            }
            set
            {
                if (this.fillBaseMembers != value)
                {
                    this.fillBaseMembers = value;
                    this.OnFillBaseMembersChanged();
                }
            }
        }

        public virtual Hashtable Snippets
        {
            get
            {
                return this.snippets;
            }
        }

        public virtual ISyntaxTree SyntaxTree
        {
            get
            {
                return this.syntaxTree;
            }
        }

        public virtual IList<ISyntaxTree> SyntaxTrees
        {
            get
            {
                return this.syntaxTrees;
            }
        }
    }
}

