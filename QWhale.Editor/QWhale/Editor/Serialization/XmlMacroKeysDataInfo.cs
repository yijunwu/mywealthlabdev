namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.Xml.Serialization;

    public class XmlMacroKeysDataInfo : ISerializationInfo
    {
        private XmlMacroKeyDataInfo[] macros;
        private IMacroKeyList owner;

        public XmlMacroKeysDataInfo()
        {
            this.macros = new XmlMacroKeyDataInfo[0];
        }

        public XmlMacroKeysDataInfo(IMacroKeyList owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IMacroKeyList) owner;
            this.Macros = this.macros;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.macros = this.Macros;
                foreach (XmlMacroKeyDataInfo info in this.macros)
                {
                    info.Load();
                }
            }
        }

        public bool ShouldSerializeMarcos()
        {
            return ((this.macros.Length > 0) || ((this.owner != null) && (this.owner.Count > 0)));
        }

        [XmlArrayItem("Macro"), XmlArray]
        public XmlMacroKeyDataInfo[] Macros
        {
            get
            {
                if (this.owner == null)
                {
                    return this.macros;
                }
                XmlMacroKeyDataInfo[] infoArray = new XmlMacroKeyDataInfo[this.owner.Count];
                for (int i = 0; i < this.owner.Count; i++)
                {
                    infoArray[i] = (XmlMacroKeyDataInfo) this.owner[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.macros = value;
                if ((this.owner != null) && (value != null))
                {
                    this.owner.Clear();
                    foreach (XmlMacroKeyDataInfo info in value)
                    {
                        IMacroKeyData item = new MacroKeyData {
                            SerializationInfo = info
                        };
                        this.owner.Add(item);
                    }
                }
            }
        }
    }
}

