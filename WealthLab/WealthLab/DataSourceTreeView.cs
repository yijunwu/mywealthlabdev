namespace WealthLab
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab.Properties;

    [ToolboxBitmap(typeof(DataSourceTreeView), "DataSourceTreeView")]
    public class DataSourceTreeView : TreeView, IItemTracker<WealthLab.DataSource>
    {
        private bool bool_0;
        private DataSourceManager dataSourceManager_0;
        private Dictionary<StaticDataProvider, int> dictionary_0 = new Dictionary<StaticDataProvider, int>();
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private ToolStripMenuItem mniAddSymbols;
        private ToolStripMenuItem mniDataManager;
        private ToolStripMenuItem mniDelete;
        private ToolStripMenuItem mniNewDataSource;
        private ToolStripMenuItem mniRemoveSymbol;
        private ToolStripMenuItem mniRename;
        private ContextMenuStrip popup;
        private ToolStripSeparator sepNewDS;
        private ToolStripSeparator sepSymbol;
        private TreeNode treeNode_0;

        public event EventHandler<EventArgs> DataManagerClicked;

        public event EventHandler<DataSourceEventArgs> DataSourceSelected;

        public event EventHandler<EventArgs> DataSourceTreeViewRenameClicked;

        public event EventHandler<EventArgs> IndexManagerClicked;

        public event EventHandler<EventArgs> NewDataSourceClicked;

        public event EventHandler<DataSourceSymbolEventArgs> SymbolSelected;

        public DataSourceTreeView()
        {
            this.method_1();
            base.ImageList = this.imageList_0;
            base.HideSelection = false;
            this.ContextMenuStrip = this.popup;
        }

        public void ClearLastSelection()
        {
            this.treeNode_0 = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public TreeNode FindNode(WealthLab.DataSource dataSource_0, string symbol)
        {
            //using (IEnumerator enumerator = base.Nodes.GetEnumerator())
            IEnumerator enumerator = base.Nodes.GetEnumerator();
            {
                TreeNode current;
                while (enumerator.MoveNext())
                {
                    current = (TreeNode) enumerator.Current;
                    if (current.Tag == dataSource_0)
                    {
                        goto Label_0030;
                    }
                }
                goto Label_0096;
            Label_0030:
                foreach (TreeNode node3 in current.Nodes)
                {
                    if (node3.Text == symbol)
                    {
                        return node3;
                    }
                }
                return current;
            }
        Label_0096:
            return null;
        }

        public void ItemAdded(WealthLab.DataSource item)
        {
            int index = 0;
            while (index < base.Nodes.Count)
            {
                TreeNode node = base.Nodes[index];
                if (((node.Tag != null) && (node.ImageIndex != 0)) && (item.Name.CompareTo(node.Text) < 0))
                {
                    break;
                }
                index++;
            }
            TreeNode node2 = base.Nodes.Insert(index, item.Name);
            index = this.dictionary_0[item.Provider];
            node2.ImageIndex = index;
            node2.SelectedImageIndex = index;
            node2.Tag = item;
            this.method_0(node2, item);
        }

        public void ItemChanged(WealthLab.DataSource item)
        {
            string symbol = this.Symbol;
            foreach (TreeNode node in base.Nodes)
            {
                if ((node.Tag == item) && (node.ImageIndex != 0))
                {
                    node.Text = item.Name;
                    node.Nodes.Clear();
                    this.method_0(node, item);
                }
            }
            if (symbol != "")
            {
                this.SelectSymbol(item, symbol);
            }
        }

        public void ItemRemoved(WealthLab.DataSource item)
        {
            using (IEnumerator enumerator = base.Nodes.GetEnumerator())
            {
                TreeNode current;
                while (enumerator.MoveNext())
                {
                    current = (TreeNode) enumerator.Current;
                    if ((current.Tag == item) && (current.ImageIndex != 0))
                    {
                        goto Label_0036;
                    }
                }
                return;
            Label_0036:
                base.Nodes.Remove(current);
            }
        }

        private void method_0(TreeNode treeNode_1, WealthLab.DataSource dataSource_0)
        {
            foreach (string str in dataSource_0.Symbols)
            {
                TreeNode node = treeNode_1.Nodes.Add(str);
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
                node.Tag = dataSource_0;
            }
        }

        private void method_1()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DataSourceTreeView));
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.popup = new ContextMenuStrip(this.icontainer_0);
            this.mniRename = new ToolStripMenuItem();
            this.mniDelete = new ToolStripMenuItem();
            this.sepSymbol = new ToolStripSeparator();
            this.mniAddSymbols = new ToolStripMenuItem();
            this.mniRemoveSymbol = new ToolStripMenuItem();
            this.sepNewDS = new ToolStripSeparator();
            this.mniNewDataSource = new ToolStripMenuItem();
            this.mniDataManager = new ToolStripMenuItem();
            this.popup.SuspendLayout();
            base.SuspendLayout();
            this.imageList_0.ColorDepth = ColorDepth.Depth8Bit;
            this.imageList_0.ImageSize = new Size(0x10, 0x10);
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.popup.ImageScalingSize = new Size(0x12, 0x12);
            this.popup.Items.AddRange(new ToolStripItem[] { this.mniRename, this.mniDelete, this.sepSymbol, this.mniAddSymbols, this.mniRemoveSymbol, this.sepNewDS, this.mniNewDataSource, this.mniDataManager });
            this.popup.Name = "popup";
            this.popup.Size = new Size(270, 160);
            this.popup.Opening += new CancelEventHandler(this.popup_Opening);
            this.mniRename.Enabled = false;
            this.mniRename.Image = (Image) manager.GetObject("mniRename.Image");
            this.mniRename.ImageTransparentColor = Color.Fuchsia;
            this.mniRename.Name = "mniRename";
            this.mniRename.Size = new Size(0x10d, 0x18);
            this.mniRename.Text = "Rename this DataSet ...";
            this.mniRename.Click += new EventHandler(this.mniRename_Click);
            this.mniDelete.Enabled = false;
            this.mniDelete.Image = (Image) manager.GetObject("mniDelete.Image");
            this.mniDelete.ImageTransparentColor = Color.Fuchsia;
            this.mniDelete.Name = "mniDelete";
            this.mniDelete.Size = new Size(0x10d, 0x18);
            this.mniDelete.Text = "Delete this DataSet";
            this.mniDelete.Click += new EventHandler(this.mniDelete_Click);
            this.sepSymbol.Name = "sepSymbol";
            this.sepSymbol.Size = new Size(0x10a, 6);
            this.mniAddSymbols.Enabled = false;
            this.mniAddSymbols.Image = (Image) manager.GetObject("mniAddSymbols.Image");
            this.mniAddSymbols.ImageTransparentColor = Color.Fuchsia;
            this.mniAddSymbols.Name = "mniAddSymbols";
            this.mniAddSymbols.Size = new Size(0x10d, 0x18);
            this.mniAddSymbols.Text = "Add Symbol(s) to this DataSet ...";
            this.mniAddSymbols.Click += new EventHandler(this.mniAddSymbols_Click);
            this.mniRemoveSymbol.Enabled = false;
            this.mniRemoveSymbol.Image = (Image) manager.GetObject("mniRemoveSymbol.Image");
            this.mniRemoveSymbol.ImageTransparentColor = Color.Fuchsia;
            this.mniRemoveSymbol.Name = "mniRemoveSymbol";
            this.mniRemoveSymbol.Size = new Size(0x10d, 0x18);
            this.mniRemoveSymbol.Text = "Remove this Symbol from the DataSet";
            this.mniRemoveSymbol.Click += new EventHandler(this.mniRemoveSymbol_Click);
            this.sepNewDS.Name = "sepNewDS";
            this.sepNewDS.Size = new Size(0x10a, 6);
            this.mniNewDataSource.Image = (Image) manager.GetObject("mniNewDataSource.Image");
            this.mniNewDataSource.ImageTransparentColor = Color.Fuchsia;
            this.mniNewDataSource.Name = "mniNewDataSource";
            this.mniNewDataSource.Size = new Size(0x10d, 0x18);
            this.mniNewDataSource.Text = "Create a new DataSet ...";
            this.mniNewDataSource.Click += new EventHandler(this.mniNewDataSource_Click);
            this.mniDataManager.Image = (Image) manager.GetObject("mniDataManager.Image");
            this.mniDataManager.ImageTransparentColor = Color.Fuchsia;
            this.mniDataManager.Name = "mniDataManager";
            this.mniDataManager.Size = new Size(0x10d, 0x18);
            this.mniDataManager.Text = "Data Manager";
            this.mniDataManager.Click += new EventHandler(this.mniDataManager_Click);
            base.LineColor = Color.Black;
            this.popup.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void mniAddSymbols_Click(object sender, EventArgs e)
        {
            WealthLab.DataSource dataSource = this.DataSource;
            if ((this.eventHandler_4 != null) && dataSource.IsIndexLabDataset)
            {
                this.eventHandler_4(this, EventArgs.Empty);
            }
            else if ((dataSource != null) && !dataSource.IsIndexLabDataset)
            {
                this.dataSourceManager_0.AddSymbols(dataSource);
            }
        }

        private void mniDataManager_Click(object sender, EventArgs e)
        {
            if (this.eventHandler_3 != null)
            {
                this.eventHandler_3(this, EventArgs.Empty);
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

        private void mniNewDataSource_Click(object sender, EventArgs e)
        {
            if (this.eventHandler_2 != null)
            {
                this.eventHandler_2(this, EventArgs.Empty);
            }
        }

        private void mniRemoveSymbol_Click(object sender, EventArgs e)
        {
            WealthLab.DataSource dataSource = this.DataSource;
            if ((dataSource != null) && (this.Symbol != ""))
            {
                this.dataSourceManager_0.RemoveSymbol(dataSource, this.Symbol);
            }
        }

        private void mniRename_Click(object sender, EventArgs e)
        {
            WealthLab.DataSource dataSource = this.DataSource;
            if (dataSource != null)
            {
                this.dataSourceManager_0.RenameDataSource(dataSource);
            }
            if ((this.eventHandler_5 != null) && dataSource.IsIndexLabDataset)
            {
                this.eventHandler_5(this, EventArgs.Empty);
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs treeViewEventArgs_0)
        {
            if ((treeViewEventArgs_0.Node != this.treeNode_0) && !this.bool_0)
            {
                this.OnNodeMouseClick(new TreeNodeMouseClickEventArgs(treeViewEventArgs_0.Node, MouseButtons.Left, 1, 0, 0));
            }
            base.OnAfterSelect(treeViewEventArgs_0);
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs treeNodeMouseClickEventArgs_0)
        {
            this.treeNode_0 = treeNodeMouseClickEventArgs_0.Node;
            base.SelectedNode = treeNodeMouseClickEventArgs_0.Node;
            if ((treeNodeMouseClickEventArgs_0.Node != null) && (treeNodeMouseClickEventArgs_0.Node.Tag != null))
            {
                WealthLab.DataSource tag = (WealthLab.DataSource) treeNodeMouseClickEventArgs_0.Node.Tag;
                this.mniRename.Enabled = tag != null;
                this.mniDelete.Enabled = tag != null;
                this.mniAddSymbols.Enabled = (tag != null) && tag.Provider.CanModifySymbols;
                this.mniRemoveSymbol.Enabled = this.mniAddSymbols.Enabled && (this.Symbol != "");
                if (treeNodeMouseClickEventArgs_0.Node.ImageIndex > 0)
                {
                    if (this.eventHandler_0 != null)
                    {
                        this.eventHandler_0(this, new DataSourceEventArgs(tag));
                    }
                }
                else if (this.eventHandler_1 != null)
                {
                    this.eventHandler_1(this, new DataSourceSymbolEventArgs(tag, treeNodeMouseClickEventArgs_0.Node.Text));
                }
            }
            base.OnNodeMouseClick(treeNodeMouseClickEventArgs_0);
        }

        public void Populate(DataSourceManager dataSourceManager_1)
        {
            this.dataSourceManager_0 = dataSourceManager_1;
            base.BeginUpdate();
            base.Nodes.Clear();
            this.imageList_0.Images.Clear();
            this.imageList_0.Images.Add(Resources.Sphere);
            foreach (StaticDataProvider provider2 in dataSourceManager_1.Providers)
            {
                this.imageList_0.Images.Add(provider2.Glyph);
                int num2 = this.imageList_0.Images.Count - 1;
                this.dictionary_0.Add(provider2, num2);
            }
            foreach (WealthLab.DataSource source in dataSourceManager_1.DataSources)
            {
                StaticDataProvider provider = source.Provider;
                if (provider != null)
                {
                    TreeNode node = base.Nodes.Add(source.Name);
                    int num = this.dictionary_0[provider];
                    node.ImageIndex = num;
                    node.SelectedImageIndex = num;
                    node.Tag = source;
                    this.method_0(node, source);
                }
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
                base.Nodes.Clear();
                this.imageList_0.Images.Clear();
                this.imageList_0.Images.Add(Resources.Sphere);
                foreach (StaticDataProvider provider2 in dataSourceManager_1.Providers)
                {
                    if (!(provider2 is IndexStaticProvider))
                    {
                        this.imageList_0.Images.Add(provider2.Glyph);
                        int num2 = this.imageList_0.Images.Count - 1;
                        this.dictionary_0.Add(provider2, num2);
                    }
                }
                foreach (WealthLab.DataSource source in dataSourceManager_1.DataSources)
                {
                    if (!source.IsIndexLabDataset)
                    {
                        StaticDataProvider provider = source.Provider;
                        if (provider != null)
                        {
                            TreeNode node = base.Nodes.Add(source.Name);
                            int num = this.dictionary_0[provider];
                            node.ImageIndex = num;
                            node.SelectedImageIndex = num;
                            node.Tag = source;
                            this.method_0(node, source);
                        }
                    }
                }
                base.EndUpdate();
            }
        }

        private void popup_Opening(object sender, CancelEventArgs e)
        {
            if (this.DataSource.IsIndexLabDataset)
            {
                this.mniAddSymbols.Text = "Add Index to this DataSet ...";
            }
            else
            {
                this.mniAddSymbols.Text = "Add Symbol(s) to this DataSet ...";
            }
        }

        public void SelectDataSource(WealthLab.DataSource dataSource_0)
        {
            using (IEnumerator enumerator = base.Nodes.GetEnumerator())
            {
                TreeNode current;
                while (enumerator.MoveNext())
                {
                    current = (TreeNode) enumerator.Current;
                    if ((current.Level == 0) && (current.Tag == dataSource_0))
                    {
                        goto Label_0038;
                    }
                }
                return;
            Label_0038:
                base.SelectedNode = current;
            }
        }

        public void SelectSymbol(string symbol)
        {
            foreach (WealthLab.DataSource source in this.dataSourceManager_0.DataSources)
            {
                this.SelectSymbol(source, symbol);
                if (base.SelectedNode.Text.Equals(symbol, StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
            }
        }

        public void SelectSymbol(WealthLab.DataSource dataSource_0, string symbol)
        {
            this.bool_0 = true;
            try
            {
                TreeNode node = this.FindNode(dataSource_0, symbol);
                if (node != null)
                {
                    base.SelectedNode = node;
                    if (node.Parent != null)
                    {
                        node.Parent.Expand();
                        base.TopNode = node.Parent;
                    }
                    node.EnsureVisible();
                }
            }
            finally
            {
                this.bool_0 = false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.DataSource DataSource
        {
            get
            {
                TreeNode selectedNode = base.SelectedNode;
                if (selectedNode == null)
                {
                    return null;
                }
                return (selectedNode.Tag as WealthLab.DataSource);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Symbol
        {
            get
            {
                TreeNode selectedNode = base.SelectedNode;
                if (selectedNode == null)
                {
                    return "";
                }
                if (selectedNode.ImageIndex != 0)
                {
                    return "";
                }
                return selectedNode.Text;
            }
        }
    }
}

