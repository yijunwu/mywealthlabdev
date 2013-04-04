namespace QWhale.Editor.TextSource.Serialization
{
    using QWhale.Common;
    using QWhale.Editor.TextSource;
    using System;

    public class XmlBookmarkInfo : ISerializationInfo
    {
        private string description;
        private int index;
        private int line;
        private string name;
        private IBookMark owner;
        private int pos;
        private string url;

        public XmlBookmarkInfo()
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.url = string.Empty;
        }

        public XmlBookmarkInfo(IBookMark owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IBookMark) owner;
            this.Line = this.Line;
            this.Pos = this.Pos;
            this.Index = this.Index;
            this.Name = this.name;
            this.Description = this.description;
            this.Url = this.url;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.line = this.Line;
                this.pos = this.Pos;
                this.index = this.Index;
                this.name = this.Name;
                this.description = this.Description;
                this.url = this.Url;
            }
        }

        public string Description
        {
            get
            {
                if (this.owner is IBookMarkEx)
                {
                    return ((IBookMarkEx) this.owner).Description;
                }
                return this.description;
            }
            set
            {
                this.description = value;
                if (this.owner is IBookMarkEx)
                {
                    ((IBookMarkEx) this.owner).Description = value;
                }
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

        public string Name
        {
            get
            {
                if (this.owner is IBookMarkEx)
                {
                    return ((IBookMarkEx) this.owner).Name;
                }
                return this.name;
            }
            set
            {
                this.name = value;
                if (this.owner is IBookMarkEx)
                {
                    ((IBookMarkEx) this.owner).Name = value;
                }
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

        public string Url
        {
            get
            {
                if (this.owner is IBookMarkEx)
                {
                    return ((IBookMarkEx) this.owner).Url;
                }
                return this.url;
            }
            set
            {
                this.url = value;
                if (this.owner is IBookMarkEx)
                {
                    ((IBookMarkEx) this.owner).Url = value;
                }
            }
        }
    }
}

