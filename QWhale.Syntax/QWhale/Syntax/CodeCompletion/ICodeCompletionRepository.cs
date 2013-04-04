namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface ICodeCompletionRepository
    {
        event DescriptionLookupEvent DescriptionLookup;

        event MemberLookupEvent MemberLookup;

        void FillMember(IListMembers members, object member, string name, int paramIndex, CodeCompletionScope scope);
        void FillMembers(ISyntaxNode node, Point position, IListMembers members, object member, string name, CodeCompletionScope scope, ref int selIndex);
        object FindDeclaration(string text, ISyntaxNode node, Point position);
        int FindReferences(ISyntaxNode node, ISyntaxNodes references);
        ICodeSnippetsProvider GetCodeSnippets(string language);
        string GetDescription(IListMembers members, ISyntaxNode node, object member, string name, bool fullDescription);
        object GetMemberType(ISyntaxNode node, Point position, object member, string name, out CodeCompletionScope scope);
        object GetMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out CodeCompletionScope scope);
        object GetMethodType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out int paramIndex, out int paramCount, out CodeCompletionScope scope);
        object GetNodeType(string text, ISyntaxNode node, Point position);
        int GetPriority(object member);
        object GetSpecialMemberType(string text, ISyntaxNode node, ref string name, ref Point position, ref Point endPos, out CodeCompletionScope scope);
        void RegisterSnippet(string snippet, bool isStatement);
        void RegisterSyntaxTree(ISyntaxTree tree);
        bool UnregisterSnippet(string snippet);
        bool UnregisterSyntaxTree(ISyntaxTree tree);

        bool CaseSensitive { get; }

        bool FillBaseMembers { get; set; }

        ISyntaxTree SyntaxTree { get; }

        IList<ISyntaxTree> SyntaxTrees { get; }
    }
}

