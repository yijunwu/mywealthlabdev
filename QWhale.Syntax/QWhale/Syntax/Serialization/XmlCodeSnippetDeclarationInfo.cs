namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Xml.Serialization;

    public class XmlCodeSnippetDeclarationInfo : ISerializationInfo
    {
        private XmlCodeSnippetLiteralInfo[] literals;
        private XmlCodeSnippetObjectInfo[] objects;
        private ICodeSnippetDeclaration owner;

        public XmlCodeSnippetDeclarationInfo()
        {
            this.literals = new XmlCodeSnippetLiteralInfo[0];
            this.objects = new XmlCodeSnippetObjectInfo[0];
        }

        public XmlCodeSnippetDeclarationInfo(ICodeSnippetDeclaration owner)
        {
            this.literals = new XmlCodeSnippetLiteralInfo[0];
            this.objects = new XmlCodeSnippetObjectInfo[0];
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippetDeclaration) owner;
            this.Literals = this.literals;
            this.Objects = this.objects;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.literals = this.Literals;
                this.objects = this.Objects;
                foreach (XmlCodeSnippetLiteralInfo info in this.literals)
                {
                    info.Load();
                }
                foreach (XmlCodeSnippetObjectInfo info2 in this.objects)
                {
                    info2.Load();
                }
            }
        }

        [XmlArrayItem("Literal"), XmlArray("Literals")]
        public XmlCodeSnippetLiteralInfo[] Literals
        {
            get
            {
                if (this.owner == null)
                {
                    return this.literals;
                }
                XmlCodeSnippetLiteralInfo[] infoArray = new XmlCodeSnippetLiteralInfo[this.owner.Literals.Count];
                for (int i = 0; i < this.owner.Literals.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetLiteralInfo) this.owner.Literals[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.literals = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Literals.Clear();
                    foreach (XmlCodeSnippetLiteralInfo info in value)
                    {
                        this.owner.Literals.AddLiteral().SerializationInfo = info;
                    }
                }
            }
        }

        [XmlArray("Objects"), XmlArrayItem("Objects")]
        public XmlCodeSnippetObjectInfo[] Objects
        {
            get
            {
                if (this.owner == null)
                {
                    return this.objects;
                }
                XmlCodeSnippetObjectInfo[] infoArray = new XmlCodeSnippetObjectInfo[this.owner.Objects.Count];
                for (int i = 0; i < this.owner.Objects.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetObjectInfo) this.owner.Objects[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.objects = value;
                if (this.owner != null)
                {
                    this.owner.Objects.Clear();
                    foreach (XmlCodeSnippetObjectInfo info in value)
                    {
                        this.owner.Objects.AddObject().SerializationInfo = info;
                    }
                }
            }
        }
    }
}

