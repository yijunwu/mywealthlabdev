namespace WealthLab
{
    using System;
    using System.Collections.Generic;

    public abstract class FundamentalDataProvider : HistoricalProvider
    {
        private IDataHost idataHost_0;

        protected FundamentalDataProvider()
        {
        }

        public virtual bool CanDragDropItem(string itemName)
        {
            return false;
        }

        public abstract FundamentalItem CreateItem(string itemName);
        public virtual void Initialize(IDataHost dataHost)
        {
            this.idataHost_0 = dataHost;
        }

        public virtual string ItemDescription(string itemName)
        {
            return "";
        }

        public virtual string ItemPane(string itemName)
        {
            return itemName;
        }

        public virtual string ItemURL(string itemName)
        {
            return "";
        }

        public virtual IList<string> NonSymbolSpecificDragDropItems(bool ignoreDragandDrop)
        {
            return this.NonSymbolSpecificItemsProvided;
        }

        public abstract IList<FundamentalItem> RequestItems(string itemName);
        public abstract IList<FundamentalItem> RequestItems(string symbol, string itemName);
        public virtual IList<string> SymbolSpecificDragDropItems(bool ignoreDragandDrop)
        {
            if (!ignoreDragandDrop && !this.HasDragDropItems)
            {
                return null;
            }
            return this.SymbolSpecificItemsProvided;
        }

        public virtual IList<string> ChartableItems
        {
            get
            {
                return this.SymbolSpecificItemsProvided;
            }
        }

        public IDataHost DataHost
        {
            get
            {
                return this.idataHost_0;
            }
        }

        public bool HasDragDropItems
        {
            get
            {
                IList<string> symbolSpecificItemsProvided = this.SymbolSpecificItemsProvided;
                if (symbolSpecificItemsProvided != null)
                {
                    foreach (string str2 in symbolSpecificItemsProvided)
                    {
                        if (this.CanDragDropItem(str2))
                        {
                            return true;
                        }
                    }
                }
                symbolSpecificItemsProvided = this.NonSymbolSpecificItemsProvided;
                if (symbolSpecificItemsProvided != null)
                {
                    bool flag;
                    using (IEnumerator<string> enumerator2 = symbolSpecificItemsProvided.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            string current = enumerator2.Current;
                            if (this.CanDragDropItem(current))
                            {
                                goto Label_006D;
                            }
                        }
                        goto Label_007F;
                    Label_006D:
                        flag = true;
                    }
                    return flag;
                }
            Label_007F:
                return false;
            }
        }

        public abstract IList<string> NonSymbolSpecificItemsProvided { get; }

        public abstract IList<string> SymbolSpecificItemsProvided { get; }
    }
}

