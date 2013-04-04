namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class SyntaxNode : ISyntaxNode, ICloneable
    {
        private ISyntaxAttributes attributes;
        private ISyntaxNodes childs;
        private ISyntaxErrors errors;
        private string name;
        private int nodeType;
        private SyntaxNodeOptions options;
        private ISyntaxNode parent;
        private IRange range;

        public SyntaxNode()
        {
            this.range = new QWhale.Common.Range(0, 0, 0, 0);
            this.name = string.Empty;
        }

        public SyntaxNode(Point position, string name)
        {
            this.range = new QWhale.Common.Range(0, 0, 0, 0);
            this.name = string.Empty;
            this.range.StartPoint = position;
            this.range.EndPoint = new Point(position.X + name.Length, position.Y);
            this.name = name;
        }

        public SyntaxNode(Point position, string name, int nodeType) : this(position, name)
        {
            this.nodeType = nodeType;
        }

        public SyntaxNode(Point position, string name, int nodeType, SyntaxNodeOptions options) : this(position, name, nodeType)
        {
            this.options = options;
        }

        public virtual ISyntaxAttribute AddAttribute()
        {
            if (this.attributes == null)
            {
                this.attributes = new SyntaxAttributes();
            }
            ISyntaxAttribute item = new SyntaxAttribute();
            this.attributes.Add(item);
            return item;
        }

        public virtual int AddAttribute(ISyntaxAttribute attr)
        {
            if (this.attributes == null)
            {
                this.attributes = new SyntaxAttributes();
            }
            this.attributes.Add(attr);
            return (this.attributes.Count - 1);
        }

        public virtual void AddAttributes(ISyntaxAttributes attrs)
        {
            if (this.attributes == null)
            {
                this.attributes = new SyntaxAttributes();
            }
            foreach (ISyntaxAttribute attribute in attrs)
            {
                this.attributes.Add(attribute);
            }
        }

        public virtual ISyntaxNode AddChild()
        {
            if (this.childs == null)
            {
                this.childs = new SyntaxNodes();
            }
            ISyntaxNode item = new SyntaxNode {
                Parent = this
            };
            this.childs.Add(item);
            return item;
        }

        public virtual int AddChild(ISyntaxNode node)
        {
            if (this.childs == null)
            {
                this.childs = new SyntaxNodes();
            }
            node.Parent = this;
            this.childs.Add(node);
            return (this.childs.Count - 1);
        }

        public virtual void AddChildren(ISyntaxNodes nodes)
        {
            if (this.childs == null)
            {
                this.childs = new SyntaxNodes();
            }
            foreach (ISyntaxNode node in nodes)
            {
                node.Parent = this;
                this.childs.Add(node);
            }
        }

        public virtual ISyntaxError AddError()
        {
            if (this.errors == null)
            {
                this.errors = new SyntaxErrors();
            }
            ISyntaxError item = new SyntaxError();
            this.errors.Add(item);
            return item;
        }

        public virtual int AddError(ISyntaxError err)
        {
            if (this.errors == null)
            {
                this.errors = new SyntaxErrors();
            }
            this.errors.Add(err);
            return (this.errors.Count - 1);
        }

        public virtual void AddErrors(ISyntaxErrors errs)
        {
            if (this.errors == null)
            {
                this.errors = new SyntaxErrors();
            }
            foreach (ISyntaxError error in errs)
            {
                this.errors.Add(error);
            }
        }

        public virtual bool BlockDeleting(Rectangle rect, IComparer<ISyntaxNode> comparer)
        {
            bool flag = false;
            if (this.HasAttributes && this.attributes.BlockDeleting(rect))
            {
                flag = true;
            }
            if (this.HasErrors && this.errors.BlockDeleting(rect))
            {
                flag = true;
            }
            if (this.HasChildren && this.childs.BlockDeleting(rect, comparer))
            {
                flag = true;
            }
            return flag;
        }

        public virtual void Clear()
        {
            if (this.childs != null)
            {
                this.childs.Clear();
            }
            this.childs = null;
            if (this.attributes != null)
            {
                this.attributes.Clear();
            }
            this.attributes = null;
            if (this.errors != null)
            {
                this.errors.Clear();
            }
            this.errors = null;
        }

        public void ClearAfter(Point position)
        {
            if (this.childs != null)
            {
                for (int i = this.childs.Count - 1; i >= 0; i--)
                {
                    ISyntaxNode node = this.childs[i];
                    if ((node.Position.Y > position.Y) || ((node.Position.Y == position.Y) && (node.Position.X >= position.Y)))
                    {
                        this.childs.RemoveAt(i);
                    }
                }
            }
            if (this.attributes != null)
            {
                for (int j = this.attributes.Count - 1; j >= 0; j--)
                {
                    ISyntaxAttribute attribute = this.attributes[j];
                    if ((attribute.Position.Y > position.Y) || ((attribute.Position.Y == position.Y) && (attribute.Position.X >= position.Y)))
                    {
                        this.attributes.RemoveAt(j);
                    }
                }
            }
            if (this.errors != null)
            {
                for (int k = this.errors.Count - 1; k >= 0; k--)
                {
                    ISyntaxError error = this.errors[k];
                    if ((error.Position.Y > position.Y) || ((error.Position.Y == position.Y) && (error.Position.X >= position.Y)))
                    {
                        this.errors.RemoveAt(k);
                    }
                }
            }
        }

        public virtual object Clone()
        {
            ISyntaxNode node = new SyntaxNode {
                Range = (IRange) this.range.Clone(),
                Name = this.Name,
                NodeType = this.nodeType
            };
            if (this.HasAttributes)
            {
                foreach (ISyntaxAttribute attribute in this.attributes)
                {
                    node.AddAttribute((ISyntaxAttribute) attribute.Clone());
                }
            }
            if (this.HasErrors)
            {
                foreach (ISyntaxError error in this.errors)
                {
                    node.AddError((ISyntaxError) error.Clone());
                }
            }
            if (this.HasChildren)
            {
                foreach (ISyntaxNode node2 in this.childs)
                {
                    node.AddChild((ISyntaxNode) node2.Clone());
                }
            }
            return node;
        }

        public bool ContainsErrors()
        {
            if (this.HasErrors)
            {
                return true;
            }
            if (this.HasChildren)
            {
                foreach (ISyntaxNode node in this.ChildList)
                {
                    if (node.ContainsErrors())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual ISyntaxAttribute FindAttribute(string name)
        {
            if (this.attributes != null)
            {
                foreach (ISyntaxAttribute attribute in this.attributes)
                {
                    if (attribute.Name == name)
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }

        public virtual ISyntaxAttribute[] FindAttributes(string name)
        {
            if (this.attributes == null)
            {
                return null;
            }
            int index = 0;
            foreach (ISyntaxAttribute attribute in this.attributes)
            {
                if (attribute.Name == name)
                {
                    index++;
                }
            }
            ISyntaxAttribute[] attributeArray = new SyntaxAttribute[index];
            index = 0;
            foreach (ISyntaxAttribute attribute2 in this.attributes)
            {
                if (attribute2.Name == name)
                {
                    attributeArray[index] = attribute2;
                    index++;
                }
            }
            return attributeArray;
        }

        public virtual ISyntaxNode FindNode(int nodeType)
        {
            if (this.childs != null)
            {
                foreach (ISyntaxNode node in this.childs)
                {
                    if (node.NodeType == nodeType)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        public virtual ISyntaxNode FindNode(string name)
        {
            if (this.childs != null)
            {
                foreach (ISyntaxNode node in this.childs)
                {
                    if (node.Name == name)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        public virtual ISyntaxNode FindNode(ISyntaxNode node, IComparer<ISyntaxNode> comparer)
        {
            int num;
            if (this.childs == null)
            {
                return null;
            }
            if (!this.childs.FindLast(node, out num, comparer))
            {
                return null;
            }
            return this.childs[num];
        }

        public virtual void FindNodes(ISyntaxNode obj, IComparer<ISyntaxNode> comparer, ISyntaxNodes nodes)
        {
            int num;
            if ((this.childs != null) && this.childs.FindFirst(obj, out num, comparer))
            {
                ISyntaxNode item = this.childs[num];
                nodes.Add(item);
                item.FindNodes(obj, comparer, nodes);
                num++;
                while (num < this.childs.Count)
                {
                    item = this.childs[num];
                    if (comparer.Compare(item, obj) != 0)
                    {
                        break;
                    }
                    nodes.Add(item);
                    item.FindNodes(obj, comparer, nodes);
                    num++;
                }
            }
        }

        public virtual int GetIndent(int index, int indent)
        {
            if ((this.Options & SyntaxNodeOptions.KeepIndentation) != SyntaxNodeOptions.None)
            {
                indent = -1;
                return indent;
            }
            if (indent > 0)
            {
                if (index == this.range.StartPoint.Y)
                {
                    if (((this.options & SyntaxNodeOptions.Indentation) == SyntaxNodeOptions.None) || ((this.options & SyntaxNodeOptions.BackIndentation) == SyntaxNodeOptions.None))
                    {
                        indent--;
                    }
                    return indent;
                }
                if (!this.HasAttributes)
                {
                    return indent;
                }
                foreach (ISyntaxAttribute attribute in this.attributes)
                {
                    if (((attribute.Name == SyntaxConsts.DefinitionScope) || (attribute.Name == SyntaxConsts.DefinitionScopeEnd)) && (attribute.Position.Y == index))
                    {
                        indent--;
                        return indent;
                    }
                }
            }
            return indent;
        }

        public virtual int InsertChild(ISyntaxNode node, IComparer<ISyntaxNode> comparer)
        {
            int num;
            if (!this.HasChildren)
            {
                return this.AddChild(node);
            }
            this.childs.FindLast(node, out num, comparer);
            this.childs.Insert(num, node);
            node.Parent = this;
            return num;
        }

        protected bool InsideRange(Point pt, Rectangle rect)
        {
            return QWhale.Common.Range.InsideRange(pt, rect);
        }

        protected virtual void OnAttributesChanged()
        {
        }

        protected virtual void OnChildsChanged()
        {
        }

        protected virtual void OnErrorsChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnNodeTypeChanged()
        {
        }

        protected virtual void OnOptionsChanged()
        {
        }

        protected virtual void OnParentChanged()
        {
        }

        protected virtual void OnPositionChanged()
        {
        }

        protected virtual void OnRangeChanged()
        {
        }

        protected virtual void OnSizeChanged()
        {
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer<ISyntaxNode> comparer)
        {
            bool flag = false;
            if (this.HasAttributes && this.attributes.PositionChanged(x, y, deltaX, deltaY))
            {
                flag = true;
            }
            if (this.HasErrors && this.errors.PositionChanged(x, y, deltaX, deltaY))
            {
                flag = true;
            }
            if (this.HasChildren && this.childs.PositionChanged(x, y, deltaX, deltaY, comparer))
            {
                flag = true;
            }
            return flag;
        }

        public virtual void Sort(IComparer<ISyntaxNode> comparer)
        {
            if (this.childs != null)
            {
                this.childs.Sort(comparer);
                foreach (ISyntaxNode node in this.childs)
                {
                    node.Sort(comparer);
                }
            }
        }

        public override string ToString()
        {
            if (!(this.name != string.Empty))
            {
                return base.ToString();
            }
            return this.name;
        }

        public virtual int AttributeCount
        {
            get
            {
                if (this.attributes == null)
                {
                    return 0;
                }
                return this.attributes.Count;
            }
        }

        public virtual ISyntaxAttributes AttributeList
        {
            get
            {
                return this.attributes;
            }
        }

        public virtual ISyntaxAttribute[] Attributes
        {
            get
            {
                if (this.attributes == null)
                {
                    return new ISyntaxAttribute[0];
                }
                ISyntaxAttribute[] array = new ISyntaxAttribute[this.attributes.Count];
                this.attributes.CopyTo(array, 0);
                return array;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    if (this.attributes != null)
                    {
                        this.attributes.Clear();
                    }
                    this.attributes = null;
                }
                else
                {
                    if (this.attributes == null)
                    {
                        this.attributes = new SyntaxAttributes();
                    }
                    this.attributes.Clear();
                    foreach (ISyntaxAttribute attribute in value)
                    {
                        this.attributes.Add(attribute);
                    }
                }
                this.OnAttributesChanged();
            }
        }

        public virtual int ChildCount
        {
            get
            {
                if (this.childs == null)
                {
                    return 0;
                }
                return this.childs.Count;
            }
        }

        public virtual ISyntaxNodes ChildList
        {
            get
            {
                return this.childs;
            }
        }

        public virtual ISyntaxNode[] Childs
        {
            get
            {
                if (this.childs == null)
                {
                    return new ISyntaxNode[0];
                }
                ISyntaxNode[] array = new ISyntaxNode[this.childs.Count];
                this.childs.CopyTo(array, 0);
                return array;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    if (this.childs != null)
                    {
                        this.childs.Clear();
                    }
                    this.childs = null;
                }
                else
                {
                    if (this.childs == null)
                    {
                        this.childs = new SyntaxNodes();
                    }
                    this.childs.Clear();
                    foreach (ISyntaxNode node in value)
                    {
                        this.childs.Add(node);
                    }
                }
                this.OnChildsChanged();
            }
        }

        public virtual int ErrorCount
        {
            get
            {
                if (this.errors == null)
                {
                    return 0;
                }
                return this.errors.Count;
            }
        }

        public virtual ISyntaxErrors ErrorList
        {
            get
            {
                return this.errors;
            }
        }

        public virtual ISyntaxError[] Errors
        {
            get
            {
                if (this.errors == null)
                {
                    return new ISyntaxError[0];
                }
                ISyntaxError[] array = new ISyntaxError[this.errors.Count];
                this.errors.CopyTo(array, 0);
                return array;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    if (this.errors != null)
                    {
                        this.errors.Clear();
                    }
                    this.errors = null;
                }
                else
                {
                    if (this.errors == null)
                    {
                        this.errors = new SyntaxErrors();
                    }
                    this.errors.Clear();
                    foreach (ISyntaxError error in value)
                    {
                        this.errors.Add(error);
                    }
                }
                this.OnErrorsChanged();
            }
        }

        public virtual bool HasAttributes
        {
            get
            {
                return ((this.attributes != null) && (this.attributes.Count > 0));
            }
        }

        public virtual bool HasChildren
        {
            get
            {
                return ((this.childs != null) && (this.childs.Count > 0));
            }
        }

        public virtual bool HasErrors
        {
            get
            {
                return ((this.errors != null) && (this.errors.Count > 0));
            }
        }

        public virtual int Index
        {
            get
            {
                if ((this.parent != null) && (this.parent.ChildList != null))
                {
                    return this.parent.ChildList.IndexOf(this);
                }
                return -1;
            }
        }

        public virtual int Level
        {
            get
            {
                int num = 0;
                ISyntaxNode parent = this.parent;
                while (parent != null)
                {
                    parent = parent.Parent;
                    num++;
                }
                return num;
            }
        }

        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNameChanged();
                }
            }
        }

        public virtual int NodeType
        {
            get
            {
                return this.nodeType;
            }
            set
            {
                if (this.nodeType != value)
                {
                    this.nodeType = value;
                    this.OnNodeTypeChanged();
                }
            }
        }

        public virtual SyntaxNodeOptions Options
        {
            get
            {
                return this.options;
            }
            set
            {
                if (this.options != value)
                {
                    this.options = value;
                    this.OnOptionsChanged();
                }
            }
        }

        public virtual ISyntaxNode Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (this.parent != value)
                {
                    this.parent = value;
                    this.OnParentChanged();
                }
            }
        }

        public virtual Point Position
        {
            get
            {
                return this.range.StartPoint;
            }
            set
            {
                if (this.range.StartPoint != value)
                {
                    this.range.StartPoint = value;
                    this.range.EndPoint = new Point(value.X + this.name.Length, value.Y);
                    this.OnPositionChanged();
                }
            }
        }

        public virtual IRange Range
        {
            get
            {
                return this.range;
            }
            set
            {
                if (this.range != value)
                {
                    this.range = value;
                    this.OnRangeChanged();
                }
            }
        }

        public virtual ISyntaxNode Root
        {
            get
            {
                ISyntaxNode parent = this.parent;
                while ((parent != null) && (parent.Parent != null))
                {
                    parent = parent.Parent;
                }
                return parent;
            }
        }

        public virtual System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.range.EndPoint.X - this.range.StartPoint.X, this.range.EndPoint.Y - this.range.StartPoint.Y);
            }
            set
            {
                this.range.EndPoint = new Point(this.range.StartPoint.X + value.Width, this.range.StartPoint.Y + value.Height);
                this.OnSizeChanged();
            }
        }
    }
}

