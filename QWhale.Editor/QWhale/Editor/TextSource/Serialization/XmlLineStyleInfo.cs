namespace QWhale.Editor.TextSource.Serialization
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using System;

    public class XmlLineStyleInfo : ISerializationInfo
    {
        private int index;
        private int line;
        private ILineStyle owner;
        private int pos;
        private int priority;
        private QWhale.Common.Range range;

        public XmlLineStyleInfo()
        {
        }

        public XmlLineStyleInfo(ILineStyle owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ILineStyle) owner;
            this.Line = this.Line;
            this.Pos = this.Pos;
            this.Index = this.Index;
            this.Range = this.range;
            this.Priority = this.priority;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.line = this.Line;
                this.pos = this.Pos;
                this.index = this.Index;
                this.range = this.Range;
                this.priority = this.Priority;
            }
        }

        public int Index
        {
            get
            {
                if (this.owner == null)
                {
                    return this.index;
                }
                return this.owner.Index;
            }
            set
            {
                this.index = value;
            }
        }

        public int Line
        {
            get
            {
                if (this.owner == null)
                {
                    return this.line;
                }
                return this.owner.Line;
            }
            set
            {
                this.line = value;
            }
        }

        public int Pos
        {
            get
            {
                if (this.owner == null)
                {
                    return this.pos;
                }
                return this.owner.Pos;
            }
            set
            {
                this.pos = value;
            }
        }

        public int Priority
        {
            get
            {
                if (this.owner == null)
                {
                    return this.priority;
                }
                return this.owner.Priority;
            }
            set
            {
                this.priority = value;
                if (this.owner != null)
                {
                    this.owner.Priority = value;
                }
            }
        }

        public QWhale.Common.Range Range
        {
            get
            {
                if (this.owner == null)
                {
                    return this.range;
                }
                IRange range = this.owner.Range;
                if (range == null)
                {
                    return null;
                }
                return new QWhale.Common.Range(range.StartPoint, range.EndPoint);
            }
            set
            {
                this.range = value;
                if (this.owner != null)
                {
                    this.owner.Range = value;
                }
            }
        }
    }
}

