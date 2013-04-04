namespace QWhale.Editor.Dialogs
{
    using QWhale.Common;
    using QWhale.Editor.Serialization;
    using QWhale.Editor.TextSource;
    using QWhale.Syntax;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class SearchSettings : PersistentSettings, ISearchSettings, IPersistentSettings, IImport, IExport
    {
        private bool clearBookmarks;
        private IList<string> replaceList = new List<string>();
        private IList<string> searchList = new List<string>();
        private QWhale.Editor.TextSource.SearchOptions searchOptions = EditConsts.DefaultSearchOptions;

        public override void Assign(IPersistentSettings source)
        {
            if (source is ISearchSettings)
            {
                this.Assign((ISearchSettings) source);
            }
        }

        public virtual void Assign(ISearchSettings source)
        {
            this.CopyList(source.SearchList, this.searchList);
            this.CopyList(source.ReplaceList, this.replaceList);
            this.searchOptions = source.SearchOptions;
        }

        private void CopyList(IList<string> fromList, IList<string> toList)
        {
            toList.Clear();
            foreach (string str in fromList)
            {
                toList.Add(str);
            }
        }

        public override Type GetXmlType()
        {
            return typeof(XmlSearchSettingsInfo);
        }

        protected virtual void OnClearBookmarksChanged()
        {
        }

        protected virtual void OnSearchOptionsChanged()
        {
        }

        public virtual bool ClearBookmarks
        {
            get
            {
                return this.clearBookmarks;
            }
            set
            {
                if (this.clearBookmarks != value)
                {
                    this.clearBookmarks = value;
                    this.OnClearBookmarksChanged();
                }
            }
        }

        public virtual IList<string> ReplaceList
        {
            get
            {
                return this.replaceList;
            }
        }

        public virtual IList<string> SearchList
        {
            get
            {
                return this.searchList;
            }
        }

        public virtual QWhale.Editor.TextSource.SearchOptions SearchOptions
        {
            get
            {
                return this.searchOptions;
            }
            set
            {
                if (this.searchOptions != value)
                {
                    this.searchOptions = value;
                    this.OnSearchOptionsChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlSearchSettingsInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }
    }
}

