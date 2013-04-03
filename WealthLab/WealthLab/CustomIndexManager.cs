namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class CustomIndexManager
    {
        private static CustomIndexManager customIndexManager_0;
        private IDataHost idataHost_0;
        private List<CustomIndex> list_0;
        private List<System.Type> list_1;
        private List<IIndexManagerUI> list_2 = new List<IIndexManagerUI>();
        private string string_0;

        private CustomIndexManager(string string_1, IDataHost idataHost_1)
        {
            this.idataHost_0 = idataHost_1;
            this.list_1 = new List<System.Type>();
            AssemblyLoader loader = new AssemblyLoader {
                BaseClass = "IndexDefinition",
                DLLNameFilter = "",
                Interface = null,
                PathMask = "*.dll",
                Path = Path.GetDirectoryName(Application.ExecutablePath)
            };
            foreach (Assembly assembly in loader.Assemblies)
            {
                foreach (System.Type type in loader.TypesInAssembly(assembly))
                {
                    this.list_1.Add(type);
                }
            }
            this.string_0 = string_1;
            if (!this.string_0.EndsWith(@"\"))
            {
                this.string_0 = this.string_0 + @"\";
            }
            this.list_0 = new List<CustomIndex>();
            this.LoadCustomIndices();
        }

        public void AddIndexManagerUI(IIndexManagerUI indexManagerUI)
        {
            if (indexManagerUI != null)
            {
                this.list_2.Add(indexManagerUI);
            }
        }

        public void AddIndices(List<CustomIndex> indices)
        {
            if ((indices != null) && (indices.Count > 0))
            {
                lock (this.list_0)
                {
                    this.list_0.AddRange(indices);
                }
                this.method_3();
            }
        }

        public CustomIndex CreateNewIndex(string symbol, string parameters, Guid indexDefinitionTypeID, string dataSourceName, BarScale scale, int barInterval, string dataSourceParentName, DataSource dataSourceParent)
        {
            //CustomIndex index;
            //return new CustomIndex(symbol, parameters, indexDefinitionTypeID, dataSourceName, scale, barInterval, dataSourceParentName, dataSourceParent) { IndexDefinition = this.GetIndexDefinition(index.IndexDefinitionTypeID), DataHost = this.idataHost_0 };
            CustomIndex customIndex = new CustomIndex(symbol, parameters, indexDefinitionTypeID, dataSourceName, scale, barInterval, dataSourceParentName, dataSourceParent);
            customIndex.IndexDefinition = this.GetIndexDefinition(customIndex.IndexDefinitionTypeID);
            customIndex.DataHost = this.idataHost_0;
            return customIndex;
        }

        public CustomIndex FindCustomIndex(string symbol)
        {
            if (symbol == null)
            {
                return null;
            }
            symbol = symbol.Trim().ToUpper();
            if (symbol == "")
            {
                return null;
            }
            Class36 class2 = new Class36(symbol);
            lock (this.list_0)
            {
                return this.list_0.Find(new Predicate<CustomIndex>(class2.method_0));
            }
        }

        public IndexDefinition GetIndexDefinition(Guid indexDefinitionID)
        {
            System.Type type = this.method_2(indexDefinitionID);
            return this.method_1(type);
        }

        public static void Initialize(string dataLocation, IDataHost dataHost)
        {
            customIndexManager_0 = new CustomIndexManager(dataLocation, dataHost);
        }

        public void InitListView(ListView listView)
        {
            lock (this.list_0)
            {
                foreach (CustomIndex index in this.list_0)
                {
                    string friendlyName = index.IndexDefinition.FriendlyName;
                    string[] items = new string[] { index.Symbol, friendlyName, index.DataSourceName, index.Parameters };
                    ListViewItem item = new ListViewItem(items);
                    listView.Items.Add(item);
                }
            }
        }

        public void LoadCustomIndices()
        {
            string path = this.string_0 + CustomIndex.FolderName + @"\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string[] files = Directory.GetFiles(path, "*.xml");
            lock (this.list_0)
            {
                this.list_0.Clear();
                foreach (string str2 in files)
                {
                    CustomIndex item = CustomIndex.FromFile(str2);
                    item.IndexDefinition = this.GetIndexDefinition(item.IndexDefinitionTypeID);
                    item.DataHost = this.idataHost_0;
                    this.list_0.Add(item);
                }
            }
            this.method_3();
        }

        internal void method_0(List<string> list_3)
        {
            bool flag = false;
            Class36 class2 = new Class36(list_3);
            lock (this.list_0)
            {
                int count = this.list_0.Count;
                this.list_0.RemoveAll(new Predicate<CustomIndex>(class2.method_0));
                flag = this.list_0.Count != count;
            }
            if (flag)
            {
                this.method_3();
            }
        }

        private IndexDefinition method_1(System.Type type_0)
        {
            if (type_0 == null)
            {
                return null;
            }
            return (IndexDefinition) Activator.CreateInstance(type_0);
        }

        private System.Type method_2(Guid guid_0)
        {
            System.Type type2;
            using (List<System.Type>.Enumerator enumerator = this.list_1.GetEnumerator())
            {
                System.Type current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.GUID == guid_0)
                    {
                        goto Label_0030;
                    }
                }
                return null;
            Label_0030:
                type2 = current;
            }
            return type2;
        }

        private void method_3()
        {
            foreach (IIndexManagerUI rui in this.list_2)
            {
                rui.RefreshCustomIndexesList();
            }
        }

        public void RemoveIndexManagerUI(IIndexManagerUI indexManagerUI)
        {
            if (indexManagerUI != null)
            {
                this.list_2.Remove(indexManagerUI);
            }
        }

        public List<IndexDefinition> IndexDefinitions
        {
            get
            {
                List<IndexDefinition> list = new List<IndexDefinition>();
                foreach (System.Type type in this.list_1)
                {
                    list.Add((IndexDefinition) Activator.CreateInstance(type));
                }
                return list;
            }
        }

        public static CustomIndexManager Instance
        {
            get
            {
                return customIndexManager_0;
            }
        }

        private class Class36
        {
            private List<string> list_0;

            public Class36(List<string> list_1)
            {
                this.list_0 = list_1;
            }

            public Class36(string string_0)
            {
                this.list_0 = new List<string>();
                this.list_0.Add(string_0);
            }

            public bool method_0(CustomIndex customIndex_0)
            {
                return ((((customIndex_0 != null) && (this.list_0 != null)) && (this.list_0.Count != 0)) && this.list_0.Contains(customIndex_0.Symbol));
            }
        }
    }
}

