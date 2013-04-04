namespace QWhale.Editor.TextSource
{
    using System;
    using System.ComponentModel;

    public class BookMarkEx : BookMark, IBookMarkEx, IBookMark
    {
        private string description;
        private string name;
        private string url;

        public BookMarkEx() : this(0, 0, 0, string.Empty, string.Empty, string.Empty)
        {
        }

        public BookMarkEx(int line, int pos, int index, string name, string description, string url) : base(line, pos, index)
        {
            this.name = string.Empty;
            this.description = string.Empty;
            this.url = string.Empty;
            this.name = name;
            this.description = description;
            this.url = url;
        }

        public override void Assign(IBookMark source)
        {
            base.Assign(source);
            if (source is IBookMarkEx)
            {
                IBookMarkEx ex = (IBookMarkEx) source;
                this.Name = ex.Name;
                this.Description = ex.Description;
                this.Url = ex.Url;
            }
        }

        protected virtual void OnDescriptionChanged()
        {
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnUrlChanged()
        {
        }

        [Description("Gets or sets bookmark description.")]
        public virtual string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.OnDescriptionChanged();
                }
            }
        }

        [Description("Gets or sets name of the bookmark.")]
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

        [Description("Gets or sets bookmark url.")]
        public virtual string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                if (this.url != value)
                {
                    this.url = value;
                    this.OnUrlChanged();
                }
            }
        }
    }
}

