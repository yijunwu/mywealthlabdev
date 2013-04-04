namespace QWhale.Syntax
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class SyntaxTree : ISyntaxTree
    {
        private IComparer<ISyntaxNode> pointComparer = new PointComparer();
        private ISyntaxNode root = new SyntaxNode();
        private IList<ISyntaxNode> stack = new List<ISyntaxNode>();

        public virtual void BlockDeleting(Rectangle rect)
        {
            this.root.BlockDeleting(rect, this.pointComparer);
        }

        public virtual void Clear()
        {
            this.root.Clear();
            this.stack.Clear();
        }

        public virtual ISyntaxNode FindNode(ISyntaxNode obj, IComparer<ISyntaxNode> comparer)
        {
            ISyntaxNode root = this.Root;
            ISyntaxNode node2 = null;
            while (root != null)
            {
                node2 = root;
                root = root.FindNode(obj, comparer);
            }
            return node2;
        }

        public virtual void FindNodes(ISyntaxNode obj, IComparer<ISyntaxNode> comparer, ISyntaxNodes nodes)
        {
            if (this.Root != null)
            {
                nodes.Add(this.Root);
                this.Root.FindNodes(obj, comparer, nodes);
            }
        }

        public virtual ISyntaxNode Pop()
        {
            ISyntaxNode node = (this.stack.Count > 0) ? this.stack[this.stack.Count - 1] : null;
            if (node != null)
            {
                this.stack.RemoveAt(this.stack.Count - 1);
            }
            return node;
        }

        public virtual void PositionChanged(int x, int y, int deltaX, int deltaY)
        {
            this.root.PositionChanged(x, y, deltaX, deltaY, this.pointComparer);
        }

        public virtual void Push(ISyntaxNode node)
        {
            this.stack.Add(node);
        }

        public virtual void Sort(IComparer<ISyntaxNode> comparer)
        {
            this.Root.Sort(comparer);
        }

        public virtual ISyntaxNode Current
        {
            get
            {
                if (this.stack.Count <= 0)
                {
                    return this.root;
                }
                return this.stack[this.stack.Count - 1];
            }
        }

        public virtual ISyntaxNode Root
        {
            get
            {
                return this.root;
            }
        }

        internal class PointComparer : IComparer<ISyntaxNode>
        {
            public int Compare(ISyntaxNode x, ISyntaxNode y)
            {
                Point startPoint = x.Range.StartPoint;
                Point point2 = y.Range.StartPoint;
                int num = startPoint.Y - point2.Y;
                if (num == 0)
                {
                    num = startPoint.X - point2.X;
                }
                return num;
            }
        }
    }
}

