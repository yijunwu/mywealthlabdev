namespace QWhale.Syntax.Serialization
{
    using QWhale.Common;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class XmlCodeSnippetInfo : ISerializationInfo
    {
        private XmlCodeSnippetCodeInfo code;
        private XmlCodeSnippetDeclarationInfo[] declarations;
        private XmlCodeSnippetHeaderInfo header;
        private int imageIndex;
        private XmlCodeSnippetImportInfo[] imports;
        private ICodeSnippet owner;
        private XmlCodeSnippetReferenceInfo[] references;

        public XmlCodeSnippetInfo()
        {
            this.declarations = new XmlCodeSnippetDeclarationInfo[0];
            this.imports = new XmlCodeSnippetImportInfo[0];
            this.references = new XmlCodeSnippetReferenceInfo[0];
            this.imageIndex = -1;
        }

        public XmlCodeSnippetInfo(ICodeSnippet owner)
        {
            this.declarations = new XmlCodeSnippetDeclarationInfo[0];
            this.imports = new XmlCodeSnippetImportInfo[0];
            this.references = new XmlCodeSnippetReferenceInfo[0];
            this.imageIndex = -1;
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ICodeSnippet) owner;
            this.Header = this.header;
            this.Declarations = this.declarations;
            this.Imports = this.imports;
            this.References = this.references;
            this.Code = this.code;
            this.ImageIndex = this.imageIndex;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.header = this.Header;
                this.declarations = this.Declarations;
                this.imports = this.Imports;
                this.references = this.References;
                this.code = this.Code;
                this.imageIndex = this.ImageIndex;
                if (this.header != null)
                {
                    this.header.Load();
                }
                if (this.code != null)
                {
                    this.code.Load();
                }
                foreach (XmlCodeSnippetDeclarationInfo info in this.declarations)
                {
                    info.Load();
                }
                foreach (XmlCodeSnippetImportInfo info2 in this.imports)
                {
                    info2.Load();
                }
                foreach (XmlCodeSnippetReferenceInfo info3 in this.references)
                {
                    info3.Load();
                }
            }
        }

        public XmlCodeSnippetCodeInfo Code
        {
            get
            {
                if (this.owner == null)
                {
                    return this.code;
                }
                return (XmlCodeSnippetCodeInfo) this.owner.Code.SerializationInfo;
            }
            set
            {
                this.code = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Code.SerializationInfo = value;
                }
            }
        }

        [XmlArrayItem("Declaration"), XmlArray("Declarations")]
        public XmlCodeSnippetDeclarationInfo[] Declarations
        {
            get
            {
                if (this.owner == null)
                {
                    return this.declarations;
                }
                XmlCodeSnippetDeclarationInfo[] infoArray = new XmlCodeSnippetDeclarationInfo[this.owner.Declarations.Count];
                for (int i = 0; i < this.owner.Declarations.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetDeclarationInfo) this.owner.Declarations[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.declarations = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Declarations.Clear();
                    foreach (XmlCodeSnippetDeclarationInfo info in value)
                    {
                        this.owner.Declarations.AddDeclaration().SerializationInfo = info;
                    }
                }
            }
        }

        public XmlCodeSnippetHeaderInfo Header
        {
            get
            {
                if (this.owner == null)
                {
                    return this.header;
                }
                return (XmlCodeSnippetHeaderInfo) this.owner.Header.SerializationInfo;
            }
            set
            {
                this.header = value;
                if (this.owner != null)
                {
                    this.owner.Header.SerializationInfo = value;
                }
            }
        }

        [DefaultValue(-1)]
        public int ImageIndex
        {
            get
            {
                if (this.owner == null)
                {
                    return this.imageIndex;
                }
                return this.owner.ImageIndex;
            }
            set
            {
                this.imageIndex = value;
                if (this.owner != null)
                {
                    this.owner.ImageIndex = value;
                }
            }
        }

        [XmlArrayItem("Import"), XmlArray("Imports")]
        public XmlCodeSnippetImportInfo[] Imports
        {
            get
            {
                if (this.owner == null)
                {
                    return this.imports;
                }
                XmlCodeSnippetImportInfo[] infoArray = new XmlCodeSnippetImportInfo[this.owner.Imports.Count];
                for (int i = 0; i < this.owner.Imports.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetImportInfo) this.owner.Imports[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.imports = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Imports.Clear();
                    foreach (XmlCodeSnippetImportInfo info in value)
                    {
                        this.owner.Imports.AddImport().SerializationInfo = info;
                    }
                }
            }
        }

        [XmlArray("References"), XmlArrayItem("Reference")]
        public XmlCodeSnippetReferenceInfo[] References
        {
            get
            {
                if (this.owner == null)
                {
                    return this.references;
                }
                XmlCodeSnippetReferenceInfo[] infoArray = new XmlCodeSnippetReferenceInfo[this.owner.References.Count];
                for (int i = 0; i < this.owner.References.Count; i++)
                {
                    infoArray[i] = (XmlCodeSnippetReferenceInfo) this.owner.References[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.references = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.References.Clear();
                    foreach (XmlCodeSnippetReferenceInfo info in value)
                    {
                        this.owner.References.AddReference().SerializationInfo = info;
                    }
                }
            }
        }
    }
}

