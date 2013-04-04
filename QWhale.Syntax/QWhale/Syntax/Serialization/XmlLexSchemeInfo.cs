namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [XmlRoot("LexScheme")]
    public class XmlLexSchemeInfo : ISerializationInfo
    {
        private string author;
        private string copyright;
        private string desc;
        private string fileExtension;
        private string fileType;
        private string name;
        private ILexScheme owner;
        private XmlLexStateInfo[] states;
        private XmlLexStyleInfo[] styles;
        private string version;

        public XmlLexSchemeInfo()
        {
            this.author = string.Empty;
            this.name = string.Empty;
            this.desc = string.Empty;
            this.copyright = string.Empty;
            this.fileExtension = string.Empty;
            this.fileType = string.Empty;
            this.version = string.Empty;
            this.styles = new XmlLexStyleInfo[0];
            this.states = new XmlLexStateInfo[0];
        }

        public XmlLexSchemeInfo(ILexScheme owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ILexScheme) owner;
            this.Author = this.author;
            this.Name = this.name;
            this.Desc = this.desc;
            this.Copyright = this.copyright;
            this.FileExtension = this.fileExtension;
            this.FileType = this.fileType;
            this.Version = this.version;
            this.Styles = this.styles;
            this.States = this.states;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.author = this.Author;
                this.name = this.Name;
                this.desc = this.Desc;
                this.copyright = this.Copyright;
                this.fileExtension = this.FileExtension;
                this.fileType = this.FileType;
                this.version = this.Version;
                this.styles = this.Styles;
                this.states = this.States;
                foreach (XmlLexStyleInfo info in this.styles)
                {
                    info.Load();
                }
                foreach (XmlLexStateInfo info2 in this.states)
                {
                    info2.Load();
                }
            }
        }

        public bool ShouldSerializeStates()
        {
            return ((this.states.Length > 0) || ((this.owner != null) && (this.owner.States.Count > 0)));
        }

        public bool ShouldSerializeStyles()
        {
            return ((this.styles.Length > 0) || ((this.owner != null) && (this.owner.Styles.Count > 0)));
        }

        [DefaultValue("")]
        public string Author
        {
            get
            {
                if (this.owner == null)
                {
                    return this.author;
                }
                return this.owner.Author;
            }
            set
            {
                this.author = value;
                if (this.owner != null)
                {
                    this.owner.Author = value;
                }
            }
        }

        [DefaultValue("")]
        public string Copyright
        {
            get
            {
                if (this.owner == null)
                {
                    return this.copyright;
                }
                return this.owner.Copyright;
            }
            set
            {
                this.copyright = value;
                if (this.owner != null)
                {
                    this.owner.Copyright = value;
                }
            }
        }

        [DefaultValue("")]
        public string Desc
        {
            get
            {
                if (this.owner == null)
                {
                    return this.desc;
                }
                return this.owner.Desc;
            }
            set
            {
                this.desc = value;
                if (this.owner != null)
                {
                    this.owner.Desc = value;
                }
            }
        }

        [DefaultValue("")]
        public string FileExtension
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fileExtension;
                }
                return this.owner.FileExtension;
            }
            set
            {
                this.fileExtension = value;
                if (this.owner != null)
                {
                    this.owner.FileExtension = value;
                }
            }
        }

        [DefaultValue("")]
        public string FileType
        {
            get
            {
                if (this.owner == null)
                {
                    return this.fileType;
                }
                return this.owner.FileType;
            }
            set
            {
                this.fileType = value;
                if (this.owner != null)
                {
                    this.owner.FileType = value;
                }
            }
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

        [XmlArrayItem("State"), XmlArray]
        public XmlLexStateInfo[] States
        {
            get
            {
                if (this.owner == null)
                {
                    return this.states;
                }
                XmlLexStateInfo[] infoArray = new XmlLexStateInfo[this.owner.States.Count];
                for (int i = 0; i < this.owner.States.Count; i++)
                {
                    infoArray[i] = (XmlLexStateInfo) this.owner.States[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.states = value;
                if ((this.owner != null) && (value != null))
                {
                    ILexStates states = new LexStates(this.owner);
                    XmlLexStateInfo[] infoArray = value;
                    for (int i = 0; i < infoArray.Length; i++)
                    {
                        XmlLexStateInfo info1 = infoArray[i];
                        ILexState item = new LexState(this.owner);
                        states.Add(item);
                    }
                    this.owner.States = states;
                    for (int j = 0; j < states.Count; j++)
                    {
                        states[j].SerializationInfo = value[j];
                    }
                }
            }
        }

        [XmlArray, XmlArrayItem("Style")]
        public XmlLexStyleInfo[] Styles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.styles;
                }
                XmlLexStyleInfo[] infoArray = new XmlLexStyleInfo[this.owner.Styles.Count];
                for (int i = 0; i < this.owner.Styles.Count; i++)
                {
                    infoArray[i] = (XmlLexStyleInfo) this.owner.Styles[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.styles = value;
                if ((this.owner != null) && (value != null))
                {
                    ILexStyles styles = new LexStyles(this.owner);
                    XmlLexStyleInfo[] infoArray = value;
                    for (int i = 0; i < infoArray.Length; i++)
                    {
                        XmlLexStyleInfo info1 = infoArray[i];
                        ILexStyle item = new LexStyle(this.owner);
                        styles.Add(item);
                    }
                    this.owner.Styles = styles;
                    for (int j = 0; j < styles.Count; j++)
                    {
                        styles[j].SerializationInfo = value[j];
                    }
                }
            }
        }

        [DefaultValue("")]
        public string Version
        {
            get
            {
                if (this.owner == null)
                {
                    return this.version;
                }
                return this.owner.Version;
            }
            set
            {
                this.version = value;
                if (this.owner != null)
                {
                    this.owner.Version = value;
                }
            }
        }
    }
}

