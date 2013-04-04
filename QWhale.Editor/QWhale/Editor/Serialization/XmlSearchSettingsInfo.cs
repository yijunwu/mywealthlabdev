namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Editor.Dialogs;
    using QWhale.Editor.TextSource;
    using System;

    public class XmlSearchSettingsInfo : ISerializationInfo
    {
        private ISearchSettings owner;
        private string[] replaceList;
        private string[] searchList;
        private QWhale.Editor.TextSource.SearchOptions searchOptions;

        public XmlSearchSettingsInfo()
        {
            this.searchOptions = EditConsts.DefaultSearchOptions;
            this.searchList = new string[0];
            this.replaceList = new string[0];
        }

        public XmlSearchSettingsInfo(ISearchSettings owner) : this()
        {
            this.owner = owner;
        }

        public virtual void FixupReferences(object owner)
        {
            this.owner = (ISearchSettings) owner;
            this.SearchList = this.searchList;
            this.ReplaceList = this.replaceList;
            this.SearchOptions = this.searchOptions;
        }

        public virtual void Load()
        {
            if (this.owner != null)
            {
                this.searchList = this.SearchList;
                this.replaceList = this.ReplaceList;
                this.searchOptions = this.SearchOptions;
            }
        }

        public string[] ReplaceList
        {
            get
            {
                if (this.owner == null)
                {
                    return this.replaceList;
                }
                string[] strArray = new string[this.owner.ReplaceList.Count];
                for (int i = 0; i < this.owner.ReplaceList.Count; i++)
                {
                    strArray[i] = this.owner.ReplaceList[i];
                }
                return strArray;
            }
            set
            {
                this.replaceList = value;
                if (this.owner != null)
                {
                    this.owner.ReplaceList.Clear();
                    foreach (string str in value)
                    {
                        this.owner.ReplaceList.Add(str);
                    }
                }
            }
        }

        public string[] SearchList
        {
            get
            {
                if (this.owner == null)
                {
                    return this.searchList;
                }
                string[] strArray = new string[this.owner.SearchList.Count];
                for (int i = 0; i < this.owner.SearchList.Count; i++)
                {
                    strArray[i] = this.owner.SearchList[i];
                }
                return strArray;
            }
            set
            {
                this.searchList = value;
                if (this.owner != null)
                {
                    this.owner.SearchList.Clear();
                    foreach (string str in value)
                    {
                        this.owner.SearchList.Add(str);
                    }
                }
            }
        }

        public QWhale.Editor.TextSource.SearchOptions SearchOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return this.searchOptions;
                }
                return this.owner.SearchOptions;
            }
            set
            {
                this.searchOptions = value;
                if (this.owner != null)
                {
                    this.owner.SearchOptions = value;
                }
            }
        }
    }
}

