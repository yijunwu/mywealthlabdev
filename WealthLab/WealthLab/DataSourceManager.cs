namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(DataSourceManager), "DataSourceManager")]
    public class DataSourceManager : Component, IComparer<DataSource>, IDataHost
    {
        private AssemblyLoader assemblyLoader;
        private AuthenticationProvider authProvider;
        private bool onDemandUpdatesEnabled;
        private Dictionary<string, StaticDataProvider> dictionary_0;
        private IContainer components;
        private ISettingsHost isettingsHost_0;
        private List<DataSource> dataSources;
        private List<IItemTracker<DataSource>> observers;
        private MarketHours marketHours;
        private string rootPath;

        private EventHandler<StockSplitEventArgs> eventHandler_0;

        public event EventHandler<StockSplitEventArgs> StockSplitDataAdjusted
        {
            add
            {
                EventHandler<StockSplitEventArgs> eventHandler;
                EventHandler<StockSplitEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<StockSplitEventArgs> eventHandler1 = (EventHandler<StockSplitEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<StockSplitEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<StockSplitEventArgs> eventHandler;
                EventHandler<StockSplitEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<StockSplitEventArgs> eventHandler1 = (EventHandler<StockSplitEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<StockSplitEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public DataSourceManager()
        {
            this.assemblyLoader = new AssemblyLoader();
            this.dataSources = new List<DataSource>();
            this.dictionary_0 = new Dictionary<string, StaticDataProvider>();
            this.onDemandUpdatesEnabled = true;
            this.observers = new List<IItemTracker<DataSource>>();
            this.marketHours = new MarketHours();
            this.method_0();
        }

        public DataSourceManager(IContainer container)
        {
            this.assemblyLoader = new AssemblyLoader();
            this.dataSources = new List<DataSource>();
            this.dictionary_0 = new Dictionary<string, StaticDataProvider>();
            this.onDemandUpdatesEnabled = true;
            this.observers = new List<IItemTracker<DataSource>>();
            this.marketHours = new MarketHours();
            container.Add(this);
            this.method_0();
        }

        public void Add(DataSource dataSource_0)
        {
            this.dataSources.Add(dataSource_0);
            this.SaveDataSources();
            foreach (IItemTracker<DataSource> tracker in this.observers)
            {
                tracker.ItemAdded(dataSource_0);
            }
        }

        public void AddSymbol(DataSource dataSource_0, string symbol)
        {
            List<string> symbols = new List<string>();
            symbols.AddRange(dataSource_0.Symbols);
            symbols.Add(symbol);
            this.ModifyDataSourceSymbols(dataSource_0, symbols);
        }

        public void AddSymbols(DataSource dataSource_0)
        {
            string str = InputBox.Show("Add Symbols", "Enter Symbol(s) separated by spaces or commas:", "", CharacterCasing.Upper);
            if (str != "")
            {
                SymbolParser parser = new SymbolParser {
                    Text = str
                };
                if (parser.Symbols.Count > 0)
                {
                    foreach (string str2 in dataSource_0.Symbols)
                    {
                        parser.Symbols.Add(str2);
                    }
                    try
                    {
                        this.ModifyDataSourceSymbols(dataSource_0, parser.Symbols);
                    }
                    catch (DataSourceException exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
        }

        public int Compare(DataSource dataSource_0, DataSource dataSource_1)
        {
            return dataSource_0.Name.CompareTo(dataSource_1.Name);
        }

        public void DeleteDataSource(DataSource dataSource_0)
        {
            if (dataSource_0.IsIndexLabDataset)
            {
                dataSource_0.Provider.ModifySymbols(dataSource_0, new List<string>());
            }
            if (this.dataSources.Contains(dataSource_0))
            {
                this.dataSources.Remove(dataSource_0);
            }
            this.DeleteDataSourceFile(dataSource_0);
            foreach (IItemTracker<DataSource> tracker in this.observers)
            {
                tracker.ItemRemoved(dataSource_0);
            }
        }

        public void DeleteDataSource(DataSource dataSource_0, bool prompt)
        {
            if (!prompt)
            {
                this.DeleteDataSource(dataSource_0);
            }
            if (MessageBox.Show("Delete this DataSet?", "Delete DataSet", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    this.DeleteDataSource(dataSource_0);
                }
                catch (DataSourceException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        public void DeleteDataSourceFile(DataSource dataSource_0)
        {
            string path = this.RootPath + @"\DataSets\" + SymbolFileNameConverter.SymbolToFileName(dataSource_0.Name) + ".xml";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void DeleteSymbolFromDataSets(string symbol, StaticDataProvider staticDataProvider_0)
        {
            foreach (DataSource source in this.DataSources)
            {
                if (((source.Provider != null) && (source.Provider.FriendlyName == staticDataProvider_0.FriendlyName)) && source.Symbols.Contains(symbol))
                {
                    List<string> symbols = new List<string>();
                    foreach (string str in source.Symbols)
                    {
                        if (str != symbol)
                        {
                            symbols.Add(str);
                        }
                    }
                    this.ModifyDataSourceSymbols(source, symbols);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public DataSource FindDataSource(string name)
        {
            DataSource source2;
            string str = name.ToUpper();
            using (List<DataSource>.Enumerator enumerator = this.dataSources.GetEnumerator())
            {
                DataSource current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (str == current.Name.ToUpper())
                    {
                        ///goto  Label_003C; ///WYJ fix, simplify the flow
                        return current;
                    }
                }
                return null;
            }
        }

        public StaticDataProvider FindProvider(string name)
        {
            StaticDataProvider provider2;
            using (IEnumerator<StaticDataProvider> enumerator = this.Providers.GetEnumerator())
            {
                StaticDataProvider current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.GetType().Name == name)
                    {
                        goto Label_0033;
                    }
                }
                return null;
            Label_0033:
                provider2 = current;
            }
            return provider2;
        }

        public StaticDataProvider GetProviderInstance(System.Type type_0)
        {
            StaticDataProvider provider = (StaticDataProvider) this.assemblyLoader.CreateInstance(type_0);
            provider.Initialize(this);
            return provider;
        }

        public void LoadDataSources()
        {
            this.dataSources.Clear();
            string path = this.RootPath + @"\DataSets\";
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                int index = 0;
                while (true)
                {
                    if (index >= files.Length)
                    {
                        break;
                    }
                    string fileName = files[index];
                    DataSource item = DataSource.FromFile(fileName);
                    if (item != null)
                    {
                        if (this.dictionary_0.ContainsKey(item.ProviderName))
                        {
                            item.Provider = this.dictionary_0[item.ProviderName];
                        }
                        try
                        {
                            int count = item.Symbols.Count;
                            this.dataSources.Add(item);
                        }
                        catch
                        {
                        }
                    }
                    index++;
                }
                this.dataSources.Sort(this);
            }
        }

        private void method_0()
        {
            this.components = new Container();
        }

        private void method_1()
        {
            if (!base.DesignMode)
            {
                this.dictionary_0.Clear();
                this.assemblyLoader.BaseClass = "StaticDataProvider";
                this.assemblyLoader.Path = Path.GetDirectoryName(Application.ExecutablePath);
                foreach (System.Type type in this.assemblyLoader.Types)
                {
                    try
                    {
                        StaticDataProvider provider = (StaticDataProvider) this.assemblyLoader.CreateInstance(type);
                        provider.Initialize(this);
                        this.dictionary_0.Add(provider.GetType().Name, provider);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void ModifyDataSourceSymbols(DataSource dataSource_0, List<string> symbols)
        {
            List<string> list = new List<string>();
            foreach (string str2 in symbols)
            {
                if (!list.Contains(str2))
                {
                    list.Add(str2);
                }
            }
            list.Sort();
            string str = dataSource_0.Provider.ModifySymbols(dataSource_0, list);
            dataSource_0.DSString = str;
            this.SaveDataSource(dataSource_0);
            dataSource_0.method_0();
            foreach (IItemTracker<DataSource> tracker in this.observers)
            {
                tracker.ItemChanged(dataSource_0);
            }
        }

        public void ProcessStockSplit(DataSource dataSource_0, string symbol, DateTime splitDate, double splitFactor)
        {
            Bars bars = dataSource_0.Provider.RequestData(dataSource_0, symbol, DateTime.MinValue, DateTime.MaxValue, 0, false);
            double num = 1.0 / splitFactor;
            for (int i = 0; i < bars.Count; i++)
            {
                DateTime time = bars.Date[i];
                if (time.Date <= splitDate)
                {
                    DataSeries series;
                    int num3;
                    DataSeries series2;
                    int num4;
                    DataSeries series3;
                    int num5;
                    DataSeries series4;
                    int num6;
                    DataSeries series5;
                    int num7;
                    (series2 = bars.Open)[num4 = i] = series2[num4] * splitFactor;
                    (series3 = bars.High)[num5 = i] = series3[num5] * splitFactor;
                    (series4 = bars.Low)[num6 = i] = series4[num6] * splitFactor;
                    (series5 = bars.Close)[num7 = i] = series5[num7] * splitFactor;
                    foreach (DataSeries series6 in bars.NamedSeries)
                    {
                        DataSeries series7;
                        int num8;
                        (series7 = series6)[num8 = i] = series7[num8] * splitFactor;
                    }
                    (series = bars.Volume)[num3 = i] = series[num3] * num;
                }
            }
            dataSource_0.Provider.SaveEditedSymbolDataFile(dataSource_0, bars);
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(this, new StockSplitEventArgs(dataSource_0.Provider, symbol, splitFactor, splitDate));
            }
        }

        public void RegisterObserver(IItemTracker<DataSource> observer)
        {
            this.observers.Add(observer);
        }

        public void RemoveSymbol(DataSource dataSource_0, string symbol)
        {
            List<string> symbols = new List<string> {
                symbol
            };
            this.RemoveSymbols(dataSource_0, symbols);
        }

        public void RemoveSymbols(DataSource dataSource_0, List<string> symbols)
        {
            List<string> list = new List<string>();
            foreach (string str in dataSource_0.Symbols)
            {
                if (!symbols.Contains(str))
                {
                    list.Add(str);
                }
            }
            try
            {
                this.ModifyDataSourceSymbols(dataSource_0, list);
            }
            catch (DataSourceException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void RenameDataSource(DataSource dataSource_0)
        {
            string name = InputBox.Show("Rename DataSet", "Enter new name for DataSet:", dataSource_0.Name);
            if (name != "")
            {
                try
                {
                    this.RenameDataSource(dataSource_0, name);
                }
                catch (DataSourceException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        public void RenameDataSource(DataSource dataSource_0, string name)
        {
            if (dataSource_0.Name != name)
            {
                if (this.FindDataSource(name) != null)
                {
                    throw new DataSourceException("Cannot rename DataSet to \"" + name + "\", a DataSet already exists with that name");
                }
                this.DeleteDataSourceFile(dataSource_0);
                dataSource_0.Name = name;
                this.SaveDataSource(dataSource_0);
                foreach (IItemTracker<DataSource> tracker in this.observers)
                {
                    tracker.ItemChanged(dataSource_0);
                }
            }
        }

        public void SaveDataSource(DataSource dataSource_0)
        {
            string path = this.RootPath + @"\DataSets\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = path + SymbolFileNameConverter.SymbolToFileName(dataSource_0.Name) + ".xml";
            FileNameValidator.ValidateFileName(fileName);
            dataSource_0.SaveToFile(fileName);
        }

        public void SaveDataSources()
        {
            foreach (DataSource source in this.dataSources)
            {
                this.SaveDataSource(source);
            }
        }

        public void UnregisterObserver(IItemTracker<DataSource> observer)
        {
            this.observers.Remove(observer);
        }

        void IDataHost.AdjustForStockSplit(StaticDataProvider staticDataProvider_0, string symbol, double splitFactor, DateTime exDate)
        {
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(this, new StockSplitEventArgs(staticDataProvider_0, symbol, splitFactor, exDate));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public AuthenticationProvider AuthProvider
        {
            get
            {
                return this.authProvider;
            }
            set
            {
                this.authProvider = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<DataSource> DataSources
        {
            get
            {
                return this.dataSources;
            }
        }

        public bool OnDemandUpdatesEnabled
        {
            get
            {
                return this.onDemandUpdatesEnabled;
            }
            set
            {
                this.onDemandUpdatesEnabled = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection<StaticDataProvider> Providers
        {
            get
            {
                return this.dictionary_0.Values;
            }
        }

        public string RootPath
        {
            get
            {
                return this.rootPath;
            }
            set
            {
                if ((value != null) && !Directory.Exists(value))
                {
                    throw new ArgumentException("Directory does not exist: " + this.rootPath);
                }
                this.rootPath = value;
                if (value != null)
                {
                    this.method_1();
                    this.LoadDataSources();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISettingsHost SettingsHost
        {
            get
            {
                return this.isettingsHost_0;
            }
            set
            {
                this.isettingsHost_0 = value;
            }
        }

        AuthenticationProvider IDataHost.AuthProvider
        {
            get
            {
                return this.authProvider;
            }
        }

        string IDataHost.BaseDataFolder
        {
            get
            {
                return this.rootPath;
            }
        }

        MarketInfo IDataHost.DefaultMarketInfo
        {
            get
            {
                return this.marketHours.Market;
            }
        }

        bool IDataHost.OnDemandUpdateEnabled
        {
            get
            {
                return this.OnDemandUpdatesEnabled;
            }
        }

        ISettingsHost IDataHost.SettingsHost
        {
            get
            {
                return this.isettingsHost_0;
            }
        }
    }
}

