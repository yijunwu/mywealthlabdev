namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [XmlRoot("State")]
    public class XmlLexStateInfo : ISerializationInfo
    {
        private bool caseSensitive;
        private string desc;
        private string name;
        private ILexState owner;
        private XmlLexSyntaxBlockInfo[] syntaxBlocks;

        public XmlLexStateInfo()
        {
            this.name = string.Empty;
            this.desc = string.Empty;
            this.syntaxBlocks = new XmlLexSyntaxBlockInfo[0];
        }

        public XmlLexStateInfo(ILexState owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ILexState) owner;
            this.Name = this.name;
            this.Desc = this.desc;
            this.SyntaxBlocks = this.syntaxBlocks;
            this.CaseSensitive = this.caseSensitive;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.desc = this.Desc;
                this.caseSensitive = this.CaseSensitive;
                this.syntaxBlocks = this.SyntaxBlocks;
                foreach (XmlLexSyntaxBlockInfo info in this.syntaxBlocks)
                {
                    info.Load();
                }
            }
        }

        public bool ShouldSerializeSyntaxBlocks()
        {
            return ((this.syntaxBlocks.Length > 0) || ((this.owner != null) && (this.owner.SyntaxBlocks.Count > 0)));
        }

        [DefaultValue(false)]
        public bool CaseSensitive
        {
            get
            {
                if (this.owner == null)
                {
                    return this.caseSensitive;
                }
                return this.owner.CaseSensitive;
            }
            set
            {
                this.caseSensitive = value;
                if (this.owner != null)
                {
                    this.owner.CaseSensitive = value;
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

        [XmlArrayItem("SyntaxBlock"), XmlArray]
        public XmlLexSyntaxBlockInfo[] SyntaxBlocks
        {
            get
            {
                if (this.owner == null)
                {
                    return this.syntaxBlocks;
                }
                XmlLexSyntaxBlockInfo[] infoArray = new XmlLexSyntaxBlockInfo[this.owner.SyntaxBlocks.Count];
                for (int i = 0; i < this.owner.SyntaxBlocks.Count; i++)
                {
                    infoArray[i] = (XmlLexSyntaxBlockInfo) this.owner.SyntaxBlocks[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.syntaxBlocks = value;
                if ((this.owner != null) && (value != null))
                {
                    ILexSyntaxBlocks blocks = new LexSyntaxBlocks(this.owner);
                    foreach (XmlLexSyntaxBlockInfo info in value)
                    {
                        ILexSyntaxBlock item = new LexSyntaxBlock(this.owner) {
                            SerializationInfo = info
                        };
                        blocks.Add(item);
                    }
                    this.owner.SyntaxBlocks = blocks;
                }
            }
        }
    }
}

