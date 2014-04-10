namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class FundamentalTreeView : SmartTreeView
    {
        private bool _ignoreDragandDrop;
        private FundamentalsLoader fundamentalsLoader_0;
        private IContainer components;
        private IDataHost idataHost_0;
        private ImageList imageList_0;
        private static int int_0 = 0;
        private static int int_1 = 1;
        private static int int_2 = 2;
        private static int int_3 = 3;
        private static int fundamentalsCount = 0;
        private static List<TreeNode> list_1 = null;
        private static List<Bitmap> list_2 = new List<Bitmap>();

        private EventHandler<FundamentalSelectedEventArgs> eventHandler_0;

        public event EventHandler<FundamentalSelectedEventArgs> FundamentalSelected
        {
            add
            {
                EventHandler<FundamentalSelectedEventArgs> eventHandler;
                EventHandler<FundamentalSelectedEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<FundamentalSelectedEventArgs> eventHandler1 = (EventHandler<FundamentalSelectedEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<FundamentalSelectedEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<FundamentalSelectedEventArgs> eventHandler;
                EventHandler<FundamentalSelectedEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<FundamentalSelectedEventArgs> eventHandler1 = (EventHandler<FundamentalSelectedEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<FundamentalSelectedEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public FundamentalTreeView()
        {
            this.method_0();
            base.ImageList = this.imageList_0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public TreeNode FindItem(string itemName)
        {
            foreach (TreeNode node in base.Nodes)
            {
                foreach (TreeNode node2 in node.Nodes)
                {
                    if (node2.Text == itemName)
                    {
                        return node2;
                    }
                }
            }
            return null;
        }

        public void LoadNodes()
        {
            if (this.idataHost_0 == null)
            {
                throw new InvalidOperationException("DataHost property must be set in FundamentalTreeView");
            }
            list_1 = new List<TreeNode>();
            list_2 = new List<Bitmap>();
            int count = this.imageList_0.Images.Count;
            fundamentalsCount = 0;
            foreach (FundamentalDataProvider provider in this.fundamentalsLoader_0.Providers)
            {
                if (provider.HasDragDropItems || this.ignoreDragandDrop)
                {
                    TreeNode node3 = new TreeNode(provider.FriendlyName);
                    list_1.Add(node3);
                    Stream manifestResourceStream = provider.GetType().Assembly.GetManifestResourceStream(provider.GetType(), "glyph.bmp");
                    if (manifestResourceStream != null)
                    {
                        Bitmap bitmap2 = new Bitmap(manifestResourceStream);
                        list_2.Add(bitmap2);
                        node3.ImageIndex = count;
                        node3.SelectedImageIndex = count;
                        count++;
                    }
                    else
                    {
                        node3.ImageIndex = int_0;
                        node3.SelectedImageIndex = int_1;
                    }
                    IList<string> list = provider.SymbolSpecificDragDropItems(this.ignoreDragandDrop);
                    if (list != null)
                    {
                        foreach (string str in list)
                        {
                            TreeNode node4 = node3.Nodes.Add(str);
                            FundamentalItem item = provider.CreateItem(str);
                            if (item.Glyph != null)
                            {
                                list_2.Add(item.Glyph);
                                node4.ImageIndex = count;
                                count++;
                            }
                            else
                            {
                                node4.ImageIndex = int_3;
                            }
                            node4.SelectedImageIndex = node4.ImageIndex;
                            node4.Tag = provider;
                            fundamentalsCount++;
                        }
                    }
                    list = provider.NonSymbolSpecificDragDropItems(this.ignoreDragandDrop);
                    if (list != null)
                    {
                        foreach (string str2 in list)
                        {
                            TreeNode node5 = node3.Nodes.Add(str2);
                            FundamentalItem item2 = provider.CreateItem(str2);
                            if (item2.Glyph != null)
                            {
                                list_2.Add(item2.Glyph);
                                node5.ImageIndex = count;
                                count++;
                            }
                            else
                            {
                                node5.ImageIndex = int_2;
                            }
                            node5.SelectedImageIndex = node5.ImageIndex;
                            node5.Tag = provider;
                            fundamentalsCount++;
                        }
                    }
                }
            }
            base.Sort();
            foreach (Bitmap bitmap in list_2)
            {
                this.imageList_0.Images.Add(bitmap);
            }
            foreach (TreeNode node in list_1)
            {
                TreeNode node2 = (TreeNode) node.Clone();
                base.Nodes.Add(node2);
            }
            base.RecallExpandState();
        }

        private void method_0()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FundamentalTreeView));
            this.imageList_0 = new ImageList(this.components);
            this.fundamentalsLoader_0 = new FundamentalsLoader(this.components);
            base.SuspendLayout();
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("imgList.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "FolderClosed.bmp");
            this.imageList_0.Images.SetKeyName(1, "FolderOpen.bmp");
            this.imageList_0.Images.SetKeyName(2, "Fundamental.bmp");
            this.imageList_0.Images.SetKeyName(3, "FundamentalSymbol.bmp");
            base.ResumeLayout(false);
        }

        protected override void OnAfterSelect(TreeViewEventArgs treeViewEventArgs_0)
        {
            base.OnAfterSelect(treeViewEventArgs_0);
            if ((treeViewEventArgs_0.Node.Level == 1) && (this.eventHandler_0 != null))
            {
                this.eventHandler_0(this, new FundamentalSelectedEventArgs(this.SelectedProvider, treeViewEventArgs_0.Node.Text));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IDataHost DataHost
        {
            get
            {
                return this.idataHost_0;
            }
            set
            {
                this.idataHost_0 = value;
                this.fundamentalsLoader_0.DataHost = this.idataHost_0;
            }
        }

        [Browsable(false)]
        public int FundamentalsCount
        {
            get
            {
                return fundamentalsCount;
            }
        }

        [Browsable(true)]
        public bool ignoreDragandDrop
        {
            get
            {
                return this._ignoreDragandDrop;
            }
            set
            {
                this._ignoreDragandDrop = value;
            }
        }

        [Browsable(false)]
        public string SelectedItemName
        {
            get
            {
                TreeNode selectedNode = base.SelectedNode;
                if ((selectedNode != null) && (selectedNode.Level != 0))
                {
                    return selectedNode.Text;
                }
                return "";
            }
        }

        [Browsable(false)]
        public FundamentalDataProvider SelectedProvider
        {
            get
            {
                TreeNode selectedNode = base.SelectedNode;
                if (selectedNode == null)
                {
                    return null;
                }
                return (selectedNode.Tag as FundamentalDataProvider);
            }
        }
    }
}

