namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class XmlOutliningInfo : ISerializationInfo
    {
        private bool allowOutlining;
        private string outlineColor;
        private QWhale.Editor.OutlineOptions outlineOptions;
        private IOutlining owner;
        private OutlineRange[] ranges;
        private bool useRoundRect;

        public XmlOutliningInfo()
        {
            this.useRoundRect = true;
            this.outlineOptions = EditConsts.DefaultOutlineOptions;
            this.ranges = new OutlineRange[0];
            this.outlineColor = XmlColorInfo.SerializeColor(EditConsts.DefaultOutlineForeColor);
        }

        public XmlOutliningInfo(IOutlining owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (IOutlining) owner;
            this.AllowOutlining = this.allowOutlining;
            this.UseRoundRect = this.useRoundRect;
            this.OutlineOptions = this.outlineOptions;
            this.OutlineColor = this.outlineColor;
            this.Ranges = this.ranges;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.allowOutlining = this.AllowOutlining;
                this.useRoundRect = this.UseRoundRect;
                this.outlineOptions = this.OutlineOptions;
                this.outlineColor = this.OutlineColor;
                this.ranges = this.Ranges;
            }
        }

        public bool ShouldSerializeOutlineColor()
        {
            return (this.OutlineColor != XmlColorInfo.SerializeColor(EditConsts.DefaultOutlineForeColor));
        }

        public bool ShouldSerializeOutlineOptions()
        {
            return (this.OutlineOptions != EditConsts.DefaultOutlineOptions);
        }

        [DefaultValue(false)]
        public bool AllowOutlining
        {
            get
            {
                if (this.owner == null)
                {
                    return this.allowOutlining;
                }
                return this.owner.AllowOutlining;
            }
            set
            {
                this.allowOutlining = value;
                if (this.owner != null)
                {
                    this.owner.AllowOutlining = value;
                }
            }
        }

        public string OutlineColor
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outlineColor;
                }
                return XmlColorInfo.SerializeColor(this.owner.OutlineColor);
            }
            set
            {
                this.outlineColor = value;
                if (this.owner != null)
                {
                    this.owner.OutlineColor = XmlColorInfo.DeserializeColor(value);
                }
            }
        }

        public QWhale.Editor.OutlineOptions OutlineOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.outlineOptions;
                }
                return this.owner.OutlineOptions;
            }
            set
            {
                this.outlineOptions = value;
                if (this.owner != null)
                {
                    this.owner.OutlineOptions = value;
                }
            }
        }

        [XmlArray, XmlArrayItem("Range")]
        public OutlineRange[] Ranges
        {
            get
            {
                if (this.owner == null)
                {
                    return this.ranges;
                }
                IList<IRange> ranges = new List<IRange>();
                this.owner.GetOutlineRanges(ranges);
                OutlineRange[] rangeArray = new OutlineRange[ranges.Count];
                for (int i = 0; i < ranges.Count; i++)
                {
                    IOutlineRange range = ranges[i] as IOutlineRange;
                    rangeArray[i] = new OutlineRange(range.StartPoint, range.EndPoint, range.Level, range.Text, range.Visible);
                }
                return rangeArray;
            }
            set
            {
                this.ranges = value;
                if (this.owner != null)
                {
                    this.owner.SetOutlineRanges(value);
                }
            }
        }

        [DefaultValue(true)]
        public bool UseRoundRect
        {
            get
            {
                if (this.owner == null)
                {
                    return this.useRoundRect;
                }
                return this.owner.UseRoundRect;
            }
            set
            {
                this.useRoundRect = value;
                if (this.owner != null)
                {
                    this.owner.UseRoundRect = value;
                }
            }
        }
    }
}

