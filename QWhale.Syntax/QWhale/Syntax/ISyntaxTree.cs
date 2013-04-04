namespace QWhale.Syntax
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public interface ISyntaxTree
    {
        void BlockDeleting(Rectangle rect);
        void Clear();
        ISyntaxNode FindNode(ISyntaxNode obj, IComparer<ISyntaxNode> comparer);
        void FindNodes(ISyntaxNode obj, IComparer<ISyntaxNode> comparer, ISyntaxNodes nodes);
        ISyntaxNode Pop();
        void PositionChanged(int x, int y, int deltaX, int deltaY);
        void Push(ISyntaxNode node);
        void Sort(IComparer<ISyntaxNode> comparer);

        ISyntaxNode Current { get; }

        ISyntaxNode Root { get; }
    }
}

