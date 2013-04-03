namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab.Properties;

    [ToolboxBitmap(typeof(DataSourceListView), "DataSourceListView")]
    public class DataSourceListView : SortableListView, IItemTracker<WealthLab.DataSource>
    {
        private WealthLab.DataSource dataSource_0;
        private DataSourceManager dataSourceManager_0;
        private Dictionary<StaticDataProvider, int> dictionary_0 = new Dictionary<StaticDataProvider, int>();
        private IContainer icontainer_1;
        private ImageList imageList_0;
        private ToolStripMenuItem mniAddSymbols;
        private ToolStripMenuItem mniDelete;
        private ToolStripMenuItem mniNewDS;
        private ToolStripMenuItem mniRename;
        private ContextMenuStrip popup;
        private ToolStripSeparator sepNewDS;
        private ToolStripSeparator sepSymbol;

        private EventHandler<DataSourceEventArgs> eventHandler_0;

        private EventHandler<EventArgs> eventHandler_1;

        public event EventHandler<DataSourceEventArgs> DataSourceSelected
        {
            add
            {
                EventHandler<DataSourceEventArgs> eventHandler;
                EventHandler<DataSourceEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<DataSourceEventArgs> eventHandler1 = (EventHandler<DataSourceEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<DataSourceEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<DataSourceEventArgs> eventHandler;
                EventHandler<DataSourceEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<DataSourceEventArgs> eventHandler1 = (EventHandler<DataSourceEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<DataSourceEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public event EventHandler<EventArgs> NewDataSourceClicked
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<EventArgs> eventHandler2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler1 = this.eventHandler_1;
                do
                {
                    eventHandler = eventHandler1;
                    EventHandler<EventArgs> eventHandler2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler1 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_1, eventHandler2, eventHandler);
                }
                while (eventHandler1 != eventHandler);
            }
        }

        public DataSourceListView()
        {
            this.method_1();
            base.View = View.Details;
            base.Columns.Add("Name", 120, HorizontalAlignment.Left);
            base.Columns.Add("Scale", 60, HorizontalAlignment.Left);
            base.Columns.Add("Symbols", 60, HorizontalAlignment.Right);
            base.Columns[2].Tag = "N";
            base.FullRowSelect = true;
            base.SmallImageList = this.imageList_0;
            base.MultiSelect = false;
            this.ContextMenuStrip = this.popup;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_1 != null))
            {
                this.icontainer_1.Dispose();
            }
            base.Dispose(disposing);
        }

        public void ItemAdded(WealthLab.DataSource item)
        {
            this.method_2(item);
        }

        public void ItemChanged(WealthLab.DataSource item)
        {
            foreach (ListViewItem item2 in base.Items)
            {
                if (item2.Tag == item)
                {
                    item2.Text = item.Name;
                    item2.SubItems[1].Text = item.BarDataScale.ToString();
                    item2.SubItems[2].Text = item.Symbols.Count.ToString("N0");
                }
            }
        }

        public void ItemRemoved(WealthLab.DataSource item)
        {
            IEnumerator enumerator = base.Items.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ListViewItem current = (ListViewItem)enumerator.Current;
                        if (current.Tag == item)
                        {
                            base.Items.Remove(current);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }


        private void method_1()
        {
            this.icontainer_1 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DataSourceListView));
            this.imageList_0 = new ImageList(this.icontainer_1);
            this.popup = new ContextMenuStrip(this.icontainer_1);
            this.mniRename = new ToolStripMenuItem();
            this.mniDelete = new ToolStripMenuItem();
            this.sepSymbol = new ToolStripSeparator();
            this.mniAddSymbols = new ToolStripMenuItem();
            this.sepNewDS = new ToolStripSeparator();
            this.mniNewDS = new ToolStripMenuItem();
            this.popup.SuspendLayout();
            base.SuspendLayout();
            this.imageList_0.ColorDepth = ColorDepth.Depth8Bit;
            this.imageList_0.ImageSize = new Size(0x10, 0x10);
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.popup.Items.AddRange(new ToolStripItem[] { this.mniRename, this.mniDelete, this.sepSymbol, this.mniAddSymbols, this.sepNewDS, this.mniNewDS });
            this.popup.Name = "popup";
            this.popup.Size = new Size(0xea, 0x68);
            this.mniRename.Enabled = false;
            this.mniRename.Image = (Image) manager.GetObject("mniRename.Image");
            this.mniRename.ImageTransparentColor = Color.Fuchsia;
            this.mniRename.Name = "mniRename";
            this.mniRename.Size = new Size(0xe9, 0x16);
            this.mniRename.Text = "Rename this DataSet ...";
            this.mniRename.Click += new EventHandler(this.mniRename_Click);
            this.mniDelete.Enabled = false;
            this.mniDelete.Image = (Image) manager.GetObject("mniDelete.Image");
            this.mniDelete.ImageTransparentColor = Color.Fuchsia;
            this.mniDelete.Name = "mniDelete";
            this.mniDelete.Size = new Size(0xe9, 0x16);
            this.mniDelete.Text = "Delete this DataSet";
            this.mniDelete.Click += new EventHandler(this.mniDelete_Click);
            this.sepSymbol.Name = "sepSymbol";
            this.sepSymbol.Size = new Size(230, 6);
            this.mniAddSymbols.Enabled = false;
            this.mniAddSymbols.Image = (Image) manager.GetObject("mniAddSymbols.Image");
            this.mniAddSymbols.ImageTransparentColor = Color.Fuchsia;
            this.mniAddSymbols.Name = "mniAddSymbols";
            this.mniAddSymbols.Size = new Size(0xe9, 0x16);
            this.mniAddSymbols.Text = "Add Symbol(s) to this DataSet ...";
            this.mniAddSymbols.Click += new EventHandler(this.mniAddSymbols_Click);
            this.sepNewDS.Name = "sepNewDS";
            this.sepNewDS.Size = new Size(230, 6);
            this.mniNewDS.Image = (Image) manager.GetObject("mniNewDS.Image");
            this.mniNewDS.ImageTransparentColor = Color.Fuchsia;
            this.mniNewDS.Name = "mniNewDS";
            this.mniNewDS.Size = new Size(0xe9, 0x16);
            this.mniNewDS.Text = "Create a new DataSet ...";
            this.mniNewDS.Click += new EventHandler(this.mniNewDS_Click);
            this.popup.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void method_2(WealthLab.DataSource dataSource_1)
        {
            StaticDataProvider provider = dataSource_1.Provider;
            if (provider != null)
            {
                ListViewItem item = base.Items.Add(dataSource_1.Name);
                int num = this.dictionary_0[provider];
                item.ImageIndex = num;
                item.Tag = dataSource_1;
                item.SubItems.Add(dataSource_1.BarDataScale.ToString());
                item.SubItems.Add(dataSource_1.Symbols.Count.ToString("N0"));
            }
        }

        private void mniAddSymbols_Click(object sender, EventArgs e)
        {
            WealthLab.DataSource dataSource = this.DataSource;
            if (dataSource != null)
            {
                this.dataSourceManager_0.AddSymbols(dataSource);
            }
        }

        private void mniDelete_Click(object sender, EventArgs e)
        {
            WealthLab.DataSource dataSource = this.DataSource;
            if (dataSource != null)
            {
                this.dataSourceManager_0.DeleteDataSource(dataSource, true);
            }
        }

        private void mniNewDS_Click(object sender, EventArgs e)
        {
            if (this.eventHandler_1 != null)
            {
                this.eventHandler_1(this, EventArgs.Empty);
            }
        }

        private void mniRename_Click(object sender, EventArgs e)
        {
            WealthLab.DataSource dataSource = this.DataSource;
            if (dataSource != null)
            {
                this.dataSourceManager_0.RenameDataSource(dataSource);
            }
        }

        protected override void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs listViewItemSelectionChangedEventArgs_0)
        {
            ListViewItem item = listViewItemSelectionChangedEventArgs_0.Item;
            if (item != null)
            {
                this.dataSource_0 = (WealthLab.DataSource) item.Tag;
                this.mniDelete.Enabled = this.dataSource_0 != null;
                this.mniRename.Enabled = this.dataSource_0 != null;
                this.mniAddSymbols.Enabled = (this.dataSource_0 != null) && this.dataSource_0.Provider.CanModifySymbols;
                if (this.eventHandler_0 != null)
                {
                    this.eventHandler_0(this, new DataSourceEventArgs(this.dataSource_0));
                }
            }
            base.OnItemSelectionChanged(listViewItemSelectionChangedEventArgs_0);
        }

        public void Populate(DataSourceManager dataSourceManager_1)
        {
            this.dataSourceManager_0 = dataSourceManager_1;
            base.BeginUpdate();
            base.Items.Clear();
            this.imageList_0.Images.Clear();
            this.imageList_0.Images.Add(Resources.Sphere);
            foreach (StaticDataProvider provider in dataSourceManager_1.Providers)
            {
                this.imageList_0.Images.Add(provider.Glyph);
                int num = this.imageList_0.Images.Count - 1;
                this.dictionary_0.Add(provider, num);
            }
            foreach (WealthLab.DataSource source in dataSourceManager_1.DataSources)
            {
                this.method_2(source);
            }
            base.EndUpdate();
        }

        public void Populate(DataSourceManager dataSourceManager_1, bool showIndexDataSet)
        {
            if (showIndexDataSet)
            {
                this.Populate(dataSourceManager_1);
            }
            else
            {
                this.dataSourceManager_0 = dataSourceManager_1;
                base.BeginUpdate();
                base.Items.Clear();
                this.imageList_0.Images.Clear();
                this.imageList_0.Images.Add(Resources.Sphere);
                this.dictionary_0.Clear();
                foreach (StaticDataProvider provider in dataSourceManager_1.Providers)
                {
                    if (!(provider is IndexStaticProvider))
                    {
                        this.imageList_0.Images.Add(provider.Glyph);
                        int num = this.imageList_0.Images.Count - 1;
                        this.dictionary_0.Add(provider, num);
                    }
                }
                foreach (WealthLab.DataSource source in dataSourceManager_1.DataSources)
                {
                    if (!source.IsIndexLabDataset)
                    {
                        this.method_2(source);
                    }
                }
                base.EndUpdate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.DataSource DataSource
        {
            get
            {
                return this.dataSource_0;
            }
        }
    }
}

