namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.Lexer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [XmlRoot("SyntaxBlock")]
    public class XmlLexSyntaxBlockInfo : ISerializationInfo
    {
        private string desc;
        private string[] expressions;
        private int leaveState;
        private int lexStyle;
        private string name;
        private ILexSyntaxBlock owner;
        private XmlLexReswordSetInfo[] reswordSets;

        public XmlLexSyntaxBlockInfo()
        {
            this.name = string.Empty;
            this.desc = string.Empty;
            this.expressions = new string[0];
            this.reswordSets = new XmlLexReswordSetInfo[0];
            this.lexStyle = -1;
            this.leaveState = -1;
        }

        public XmlLexSyntaxBlockInfo(ILexSyntaxBlock owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ILexSyntaxBlock) owner;
            this.Name = this.name;
            this.Desc = this.desc;
            this.LexStyle = this.lexStyle;
            this.LeaveState = this.leaveState;
            this.Expressions = this.expressions;
            this.ReswordSets = this.reswordSets;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.name = this.Name;
                this.desc = this.Desc;
                this.lexStyle = this.LexStyle;
                this.leaveState = this.LeaveState;
                this.expressions = this.Expressions;
                this.reswordSets = this.ReswordSets;
                foreach (XmlLexReswordSetInfo info in this.reswordSets)
                {
                    info.Load();
                }
            }
        }

        public bool ShouldSerializeExpressions()
        {
            return ((this.expressions.Length > 0) || ((this.owner != null) && (this.owner.Expressions.Count > 0)));
        }

        public bool ShouldSerializeReswordSets()
        {
            return ((this.reswordSets.Length > 0) || ((this.owner != null) && (this.owner.ReswordSets.Count > 0)));
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

        [XmlArray]
        public string[] Expressions
        {
            get
            {
                if (this.owner != null)
                {
                    string[] array = new string[this.owner.Expressions.Count];
                    this.owner.Expressions.CopyTo(array, 0);
                    return array;
                }
                return this.expressions;
            }
            set
            {
                this.expressions = value;
                if ((this.owner != null) && (value != null))
                {
                    IList<string> list = new List<string>();
                    foreach (string str in value)
                    {
                        list.Add(str);
                    }
                    this.owner.Expressions = list;
                }
            }
        }

        [DefaultValue(-1)]
        public int LeaveState
        {
            get
            {
                if ((this.owner != null) && (this.owner.LeaveState != null))
                {
                    return this.owner.LeaveState.Index;
                }
                return this.leaveState;
            }
            set
            {
                this.leaveState = value;
                if ((this.owner != null) && (this.owner.State != null))
                {
                    if (value >= 0)
                    {
                        ILexScheme scheme = this.owner.State.Scheme;
                        if (scheme != null)
                        {
                            this.owner.LeaveState = scheme.States[value];
                        }
                    }
                    else
                    {
                        this.owner.LeaveState = null;
                    }
                }
            }
        }

        [DefaultValue(-1)]
        public int LexStyle
        {
            get
            {
                if ((this.owner != null) && (this.owner.Style != null))
                {
                    return this.owner.Style.Index;
                }
                return this.lexStyle;
            }
            set
            {
                this.lexStyle = value;
                if ((this.owner != null) && (this.owner.State != null))
                {
                    if (value >= 0)
                    {
                        ILexScheme scheme = this.owner.State.Scheme;
                        if (scheme != null)
                        {
                            this.owner.Style = scheme.Styles[value];
                        }
                    }
                    else
                    {
                        this.owner.Style = null;
                    }
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

        [XmlArrayItem("ResWordSet"), XmlArray("ResWordSets")]
        public XmlLexReswordSetInfo[] ReswordSets
        {
            get
            {
                if (this.owner == null)
                {
                    return this.reswordSets;
                }
                XmlLexReswordSetInfo[] infoArray = new XmlLexReswordSetInfo[this.owner.ReswordSets.Count];
                for (int i = 0; i < this.owner.ReswordSets.Count; i++)
                {
                    infoArray[i] = (XmlLexReswordSetInfo) this.owner.ReswordSets[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.reswordSets = value;
                if ((this.owner != null) && (value != null))
                {
                    ILexReswordSets sets = new LexReswordSets(this.owner);
                    XmlLexReswordSetInfo[] infoArray = value;
                    for (int i = 0; i < infoArray.Length; i++)
                    {
                        XmlLexReswordSetInfo info1 = infoArray[i];
                        ILexReswordSet item = new LexReswordSet(this.owner);
                        sets.Add(item);
                    }
                    this.owner.ReswordSets = sets;
                    for (int j = 0; j < sets.Count; j++)
                    {
                        sets[j].SerializationInfo = value[j];
                    }
                }
            }
        }
    }
}

