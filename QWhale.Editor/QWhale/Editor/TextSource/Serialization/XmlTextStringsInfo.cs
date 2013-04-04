namespace QWhale.Editor.TextSource.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.TextSource;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class XmlTextStringsInfo : ISerializationInfo
    {
        private string delimiters;
        private string[] lines;
        private string lineTerminator;
        private ITextStrings owner;
        private bool removeTrailingSpaces;
        private int[] tabStops;
        private bool useSpaces;

        public XmlTextStringsInfo()
        {
            this.tabStops = new int[] { EditConsts.DefaultTabStop };
            this.lines = new string[0];
            this.delimiters = EditConsts.DefaultDelimiters;
            this.lineTerminator = "\r\n";
        }

        public XmlTextStringsInfo(ITextStrings owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ITextStrings) owner;
            this.TabStops = this.tabStops;
            this.UseSpaces = this.useSpaces;
            this.Delimiters = this.delimiters;
            this.RemoveTrailingSpaces = this.removeTrailingSpaces;
            this.LineTerminator = this.lineTerminator;
            this.Lines = this.lines;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.tabStops = this.TabStops;
                this.useSpaces = this.UseSpaces;
                this.delimiters = this.Delimiters;
                this.removeTrailingSpaces = this.RemoveTrailingSpaces;
                this.lineTerminator = this.LineTerminator;
                this.lines = this.Lines;
            }
        }

        public bool ShouldSerializeDelimiters()
        {
            return (this.Delimiters != EditConsts.DefaultDelimiters);
        }

        public bool ShouldSerializeLineTerminator()
        {
            return (this.LineTerminator != "\r\n");
        }

        public bool ShouldSerializeTabStops()
        {
            if (this.TabStops.Length == 1)
            {
                return (this.TabStops[0] != EditConsts.DefaultTabStop);
            }
            return true;
        }

        public string Delimiters
        {
            get
            {
                if (this.owner == null)
                {
                    return this.delimiters;
                }
                return this.owner.DelimiterString;
            }
            set
            {
                this.delimiters = value;
                if (this.owner != null)
                {
                    this.owner.DelimiterString = value;
                }
            }
        }

        public string[] Lines
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lines;
                }
                string[] strArray = new string[this.owner.Count];
                for (int i = 0; i < this.owner.Count; i++)
                {
                    strArray[i] = this.owner[i];
                }
                return strArray;
            }
            set
            {
                this.lines = value;
                if (this.owner != null)
                {
                    this.owner.BeginUpdate();
                    try
                    {
                        this.owner.Clear();
                        foreach (string str in value)
                        {
                            this.owner.Add(str);
                        }
                    }
                    finally
                    {
                        this.owner.EndUpdate();
                    }
                }
            }
        }

        [XmlIgnore]
        public string LineTerminator
        {
            get
            {
                if (this.owner == null)
                {
                    return this.lineTerminator;
                }
                return this.owner.LineTerminator;
            }
            set
            {
                this.lineTerminator = value;
                if (this.owner != null)
                {
                    this.owner.LineTerminator = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool RemoveTrailingSpaces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.removeTrailingSpaces;
                }
                return this.owner.RemoveTrailingSpaces;
            }
            set
            {
                this.removeTrailingSpaces = value;
                if (this.owner != null)
                {
                    this.owner.RemoveTrailingSpaces = value;
                }
            }
        }

        public int[] TabStops
        {
            get
            {
                if (this.owner == null)
                {
                    return this.tabStops;
                }
                return this.owner.TabStops;
            }
            set
            {
                this.tabStops = value;
                if (this.owner != null)
                {
                    this.owner.TabStops = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool UseSpaces
        {
            get
            {
                if (this.owner == null)
                {
                    return this.useSpaces;
                }
                return this.owner.UseSpaces;
            }
            set
            {
                this.useSpaces = value;
                if (this.owner != null)
                {
                    this.owner.UseSpaces = value;
                }
            }
        }
    }
}

