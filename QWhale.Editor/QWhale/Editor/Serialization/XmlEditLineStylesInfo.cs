namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using System;
    using System.Xml.Serialization;

    public class XmlEditLineStylesInfo : ISerializationInfo
    {
        private IEditLineStyles owner;
        private XmlEditLineStyleInfo[] styles;

        public XmlEditLineStylesInfo()
        {
            this.styles = new XmlEditLineStyleInfo[0];
        }

        public XmlEditLineStylesInfo(IEditLineStyles owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IEditLineStyles) owner;
            this.Styles = this.styles;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.styles = this.Styles;
                foreach (XmlEditLineStyleInfo info in this.styles)
                {
                    info.Load();
                }
            }
        }

        public bool ShouldSerializeStyles()
        {
            return ((this.styles.Length > 0) || ((this.owner != null) && (this.owner.Count > 0)));
        }

        [XmlArray, XmlArrayItem("Style")]
        public XmlEditLineStyleInfo[] Styles
        {
            get
            {
                if (this.owner == null)
                {
                    return this.styles;
                }
                XmlEditLineStyleInfo[] infoArray = new XmlEditLineStyleInfo[this.owner.Count];
                for (int i = 0; i < this.owner.Count; i++)
                {
                    infoArray[i] = (XmlEditLineStyleInfo) this.owner[i].SerializationInfo;
                }
                return infoArray;
            }
            set
            {
                this.styles = value;
                if (this.owner != null)
                {
                    this.owner.Clear();
                    foreach (XmlEditLineStyleInfo info in value)
                    {
                        this.owner[this.owner.AddLineStyle()].SerializationInfo = info;
                    }
                }
            }
        }
    }
}

