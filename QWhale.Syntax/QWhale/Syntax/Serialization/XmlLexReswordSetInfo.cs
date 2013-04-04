namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [XmlRoot("ResWordSet")]
    public class XmlLexReswordSetInfo : ISerializationInfo
    {
        private string name;
        private ILexReswordSet owner;
        private string[] reswords;
        private int reswordStyle;

        public XmlLexReswordSetInfo()
        {
            this.name = string.Empty;
            this.reswordStyle = -1;
            this.reswords = new string[0];
        }

        public XmlLexReswordSetInfo(ILexReswordSet owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ILexReswordSet) owner;
            this.Name = this.name;
            this.ReswordStyle = this.reswordStyle;
            this.Reswords = this.reswords;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.reswordStyle = this.ReswordStyle;
                this.reswords = this.Reswords;
            }
        }

        public bool ShouldSerializeReswords()
        {
            return ((this.reswords.Length > 0) || ((this.owner != null) && (this.owner.Reswords.Count > 0)));
        }

        [DefaultValue("")]
        public string Name
        {
            get
            {
                if (this.owner == null)
                {
                    return this.name;
                }
                return this.owner.Name;
            }
            set
            {
                this.name = value;
                if (this.owner != null)
                {
                    this.owner.Name = value;
                }
            }
        }

        [XmlArray("ResWords"), XmlArrayItem("word")]
        public virtual string[] Reswords
        {
            get
            {
                if (this.owner != null)
                {
                    string[] array = new string[this.owner.Reswords.Count];
                    this.owner.Reswords.CopyTo(array, 0);
                    return array;
                }
                return this.reswords;
            }
            set
            {
                this.reswords = value;
                if ((this.owner != null) && (value != null))
                {
                    IList<string> list = new List<string>();
                    foreach (string str in value)
                    {
                        list.Add(str);
                    }
                    this.owner.Reswords = list;
                }
            }
        }

        [DefaultValue(-1), XmlElement("ResWordStyle")]
        public int ReswordStyle
        {
            get
            {
                if ((this.owner != null) && (this.owner.ReswordStyle != null))
                {
                    return this.owner.ReswordStyle.Index;
                }
                return this.reswordStyle;
            }
            set
            {
                this.reswordStyle = value;
                if (this.owner != null)
                {
                    if (value >= 0)
                    {
                        ILexScheme scheme = ((this.owner.Block != null) && (this.owner.Block.State != null)) ? this.owner.Block.State.Scheme : null;
                        if (scheme != null)
                        {
                            this.owner.ReswordStyle = scheme.Styles[value];
                        }
                    }
                    else
                    {
                        this.owner.ReswordStyle = null;
                    }
                }
            }
        }
    }
}

