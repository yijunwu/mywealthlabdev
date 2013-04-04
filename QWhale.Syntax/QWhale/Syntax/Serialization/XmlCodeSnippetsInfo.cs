namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Xml.Serialization;

    [XmlRoot("CodeSnippetCollection")]
    public class XmlCodeSnippetsInfo : ISerializationInfo
    {
        private ICodeSnippets owner;
        private XmlCodeSnippetInfo[] snippets;

        public XmlCodeSnippetsInfo()
        {
        }

        public XmlCodeSnippetsInfo(ICodeSnippets owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippets) owner;
            this.Snippets = this.snippets;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.snippets = this.Snippets;
                foreach (XmlCodeSnippetInfo info in this.snippets)
                {
                    info.Load();
                }
            }
        }

        [XmlArrayItem("Snippet"), XmlArray("CodeSnippets")]
        public XmlCodeSnippetInfo[] Snippets
        {
            get
            {
                if (this.owner == null)
                {
                    return this.snippets;
                }
                XmlCodeSnippetInfo[] infoArray = new XmlCodeSnippetInfo[this.owner.Count];
                for (int i = 0; i < this.owner.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetInfo) this.owner[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.snippets = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Clear();
                    foreach (XmlCodeSnippetInfo info in value)
                    {
                        this.owner.AddSnippet().SerializationInfo = info;
                    }
                }
            }
        }
    }
}

