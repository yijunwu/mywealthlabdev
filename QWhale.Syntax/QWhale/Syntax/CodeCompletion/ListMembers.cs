namespace QWhale.Syntax.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Syntax;
    using QWhale.Syntax.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    public class ListMembers : CodeCompletionProvider, IListMembers, ICodeCompletionProvider, IList<ICodeCompletionProviderItem>, ICollection<ICodeCompletionProviderItem>, IEnumerable<ICodeCompletionProviderItem>, IEnumerable, IExport, IImport
    {
        private bool showHints = true;
        private bool showParams;
        private bool showQualifiers = true;
        private bool showResults = true;

        public ListMembers()
        {
            this.UseHtmlFormatting = true;
        }

        public virtual IListMember AddListMember()
        {
            IListMember item = this.CreateListMember();
            base.Add(item);
            return item;
        }

        public override bool ColumnVisible(int column)
        {
            switch (column)
            {
                case 0:
                    return this.showQualifiers;

                case 1:
                    return this.showResults;

                case 2:
                    return true;

                case 3:
                    return this.showParams;
            }
            return false;
        }

        public virtual IListMember CreateListMember()
        {
            return new ListMember(this);
        }

        public override string GetColumnText(int index, int column)
        {
            switch (column)
            {
                case 0:
                    return this[index].Qualifier;

                case 1:
                    return this[index].DataType;

                case 2:
                    return this[index].DisplayText;

                case 3:
                    return this[index].ParamText;

                case 4:
                    return this[index].Description;
            }
            return string.Empty;
        }

        public override string GetDescription(int index)
        {
            if (this.showHints)
            {
                return this[index].Description;
            }
            return string.Empty;
        }

        public override int GetImageIndex(int index)
        {
            return this[index].ImageIndex;
        }

        public override string GetName(int index)
        {
            return this[index].Name;
        }

        public override int GetPriority(int index)
        {
            return this[index].Priority;
        }

        public override string GetText(int index)
        {
            return this[index].Name;
        }

        protected override Type GetXmlType()
        {
            return typeof(XmlListMembersInfo);
        }

        public virtual IListMember InsertListMember(int index)
        {
            IListMember item = this.CreateListMember();
            base.Insert(index, item);
            return item;
        }

        protected virtual void OnShowHintsChanged()
        {
        }

        protected virtual void OnShowParamsChanged()
        {
        }

        protected virtual void OnShowQualifiersChanged()
        {
        }

        protected virtual void OnShowResultsChanged()
        {
        }

        public virtual void ResetShowHints()
        {
            this.ShowHints = true;
        }

        public virtual void ResetShowParams()
        {
            this.ShowParams = false;
        }

        public virtual void ResetShowQualifiers()
        {
            this.ShowQualifiers = true;
        }

        public virtual void ResetShowResults()
        {
            this.ShowResults = true;
        }

        public override int ColumnCount
        {
            get
            {
                return 4;
            }
        }

        public virtual IListMember this[int index]
        {
            get
            {
                return (IListMember) base[index];
            }
            set
            {
                base[index] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlListMembersInfo(this);
            }
            set
            {
                value.FixupReferences(this);
            }
        }

        public virtual bool ShowHints
        {
            get
            {
                return this.showHints;
            }
            set
            {
                if (this.showHints != value)
                {
                    this.showHints = value;
                    this.OnShowHintsChanged();
                }
            }
        }

        public virtual bool ShowParams
        {
            get
            {
                return this.showParams;
            }
            set
            {
                if (this.showParams != value)
                {
                    this.showParams = value;
                    this.OnShowParamsChanged();
                }
            }
        }

        public virtual bool ShowQualifiers
        {
            get
            {
                return this.showQualifiers;
            }
            set
            {
                if (this.showQualifiers != value)
                {
                    this.showQualifiers = value;
                    this.OnShowQualifiersChanged();
                }
            }
        }

        public virtual bool ShowResults
        {
            get
            {
                return this.showResults;
            }
            set
            {
                if (this.showResults != value)
                {
                    this.showResults = value;
                    this.OnShowResultsChanged();
                }
            }
        }
    }
}

