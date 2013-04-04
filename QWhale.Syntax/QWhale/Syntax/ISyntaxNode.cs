namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public interface ISyntaxNode : ICloneable
    {
        ISyntaxAttribute AddAttribute();
        int AddAttribute(ISyntaxAttribute attr);
        void AddAttributes(ISyntaxAttributes attrs);
        ISyntaxNode AddChild();
        int AddChild(ISyntaxNode node);
        void AddChildren(ISyntaxNodes nodes);
        ISyntaxError AddError();
        int AddError(ISyntaxError err);
        void AddErrors(ISyntaxErrors errs);
        bool BlockDeleting(Rectangle rect, IComparer<ISyntaxNode> comparer);
        void Clear();
        void ClearAfter(Point position);
        bool ContainsErrors();
        ISyntaxAttribute FindAttribute(string name);
        ISyntaxAttribute[] FindAttributes(string name);
        ISyntaxNode FindNode(int nodeType);
        ISyntaxNode FindNode(string name);
        ISyntaxNode FindNode(ISyntaxNode obj, IComparer<ISyntaxNode> comparer);
        void FindNodes(ISyntaxNode obj, IComparer<ISyntaxNode> comparer, ISyntaxNodes nodes);
        int GetIndent(int index, int indent);
        int InsertChild(ISyntaxNode node, IComparer<ISyntaxNode> comparer);
        bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer<ISyntaxNode> comparer);
        void Sort(IComparer<ISyntaxNode> comparer);

        int AttributeCount { get; }

        ISyntaxAttributes AttributeList { get; }

        ISyntaxAttribute[] Attributes { get; set; }

        int ChildCount { get; }

        ISyntaxNodes ChildList { get; }

        ISyntaxNode[] Childs { get; set; }

        int ErrorCount { get; }

        ISyntaxErrors ErrorList { get; }

        ISyntaxError[] Errors { get; set; }

        bool HasAttributes { get; }

        bool HasChildren { get; }

        bool HasErrors { get; }

        int Index { get; }

        int Level { get; }

        string Name { get; set; }

        int NodeType { get; set; }

        SyntaxNodeOptions Options { get; set; }

        ISyntaxNode Parent { get; set; }

        Point Position { get; set; }

        IRange Range { get; set; }

        ISyntaxNode Root { get; }

        System.Drawing.Size Size { get; set; }
    }
}

