namespace QWhale.Syntax.Lexer
{
    using QWhale.Common;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class LexReswordSet : ILexReswordSet
    {
        private ILexSyntaxBlock block;
        private bool caseSensitive;
        private string name = string.Empty;
        private IList<string> reswords;
        private Hashtable reswordsHash;
        private ILexStyle reswordStyle;

        public LexReswordSet(ILexSyntaxBlock block)
        {
            this.block = block;
            this.caseSensitive = (block != null) && block.CaseSensitive;
            this.reswords = new List<string>();
            this.reswordsHash = new Hashtable();
        }

        public virtual int AddResword(string resword)
        {
            this.reswords.Add(resword);
            string key = this.CaseSensitive ? resword : resword.ToLower();
            if (!this.reswordsHash.Contains(key))
            {
                this.reswordsHash.Add(key, key);
            }
            return (this.reswords.Count - 1);
        }

        private void BuildReswordHash()
        {
            this.reswordsHash.Clear();
            foreach (string str in this.reswords)
            {
                string str2 = this.CaseSensitive ? str : str.ToLower();
                this.reswordsHash[str2] = str2;
            }
        }

        public virtual void Clear()
        {
            this.reswords.Clear();
            this.reswordsHash.Clear();
        }

        public virtual bool FindResword(string resword)
        {
            return this.reswordsHash.Contains(this.CaseSensitive ? resword : resword.ToLower());
        }

        protected virtual void OnBlockChanged()
        {
            this.CaseSensitive = this.block.CaseSensitive;
        }

        protected void OnCaseSensitiveChanged()
        {
            this.BuildReswordHash();
        }

        protected virtual void OnNameChanged()
        {
        }

        protected virtual void OnReswordsChanged()
        {
            this.BuildReswordHash();
        }

        protected virtual void OnReswordStyleChanged()
        {
        }

        public ILexSyntaxBlock Block
        {
            get
            {
                return this.block;
            }
            set
            {
                if (this.block != value)
                {
                    this.block = value;
                    this.OnBlockChanged();
                }
            }
        }

        public virtual bool CaseSensitive
        {
            get
            {
                return ((this.block != null) && this.block.CaseSensitive);
            }
            set
            {
                if (this.caseSensitive != value)
                {
                    this.caseSensitive = value;
                    this.OnCaseSensitiveChanged();
                }
            }
        }

        [Description("Represents index of the \"LexReswordSet\" within the reswords collection.")]
        public virtual int Index
        {
            get
            {
                if (this.block == null)
                {
                    return -1;
                }
                return this.block.ReswordSets.IndexOf(this);
            }
        }

        [Description("Gets or set name for the \"LexReswordSet\".")]
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

        [Description("Represents a list of regular expression patterns for the \">LexReswordSet.Reswords\".")]
        public IList<string> Reswords
        {
            get
            {
                return this.reswords;
            }
            set
            {
                this.reswords.Clear();
                foreach (string str in value)
                {
                    this.reswords.Add(str);
                }
                this.OnReswordsChanged();
            }
        }

        [Description("Gets or sets an \"ILexStyle\" of resword collection.")]
        public virtual ILexStyle ReswordStyle
        {
            get
            {
                return this.reswordStyle;
            }
            set
            {
                if (this.reswordStyle != value)
                {
                    this.reswordStyle = value;
                    this.OnReswordStyleChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlLexReswordSetInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

