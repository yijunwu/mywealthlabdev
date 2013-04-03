namespace WealthLab.DataProviders.MarketManagerService
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [Serializable]
    public class SupportedProviders
    {
        private List<MarketManagerInfoAttribute> _items = new List<MarketManagerInfoAttribute>();

        private bool method_0(string string_0)
        {
            bool flag;
            using (List<MarketManagerInfoAttribute>.Enumerator enumerator = this._items.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    MarketManagerInfoAttribute current = enumerator.Current;
                    if (current.Name == string_0)
                    {
                        goto Label_0030;
                    }
                }
                return false;
            Label_0030:
                flag = true;
            }
            return flag;
        }

        public void Search()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if ((type.BaseType != null) && (type.BaseType.Name == "MarketManagerInfo"))
                        {
                            try
                            {
                                string str = ((MarketManagerInfo) Activator.CreateInstance(type)).ProviderName();
                                if (!this.method_0(str))
                                {
                                    this._items.Add(new MarketManagerInfoAttribute(str));
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public List<MarketManagerInfoAttribute> Items
        {
            get
            {
                return this._items;
            }
        }
    }
}

