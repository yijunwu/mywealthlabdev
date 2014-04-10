namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    public class IndicatorTreeView : SmartTreeView
    {
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private static int int_0 = 0;
        private static int int_1 = 1;
        private static int int_2 = 2;
        private static int int_3 = 3;
        private static int int_4 = 4;
        private static int int_5 = 5;
        private static int int_6 = 6;
        private static int indicatorCount = 0;
        private static List<TreeNode> list_1 = null;
        private static List<Bitmap> list_2 = new List<Bitmap>();

        private EventHandler<IndicatorEventArgs> eventHandler_0;

        public event EventHandler<IndicatorEventArgs> IndicatorSelected
        {
            add
            {
                EventHandler<IndicatorEventArgs> eventHandler;
                EventHandler<IndicatorEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<IndicatorEventArgs> eventHandler1 = (EventHandler<IndicatorEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<IndicatorEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<IndicatorEventArgs> eventHandler;
                EventHandler<IndicatorEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<IndicatorEventArgs> eventHandler1 = (EventHandler<IndicatorEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<IndicatorEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public IndicatorTreeView()
        {
            this.method_0();
            base.ImageList = this.imageList_0;
            if (list_1 == null)
            {
                list_1 = new List<TreeNode>();
                AssemblyLoader loader = new AssemblyLoader {
                    BaseClass = "IndicatorHelper",
                    Path = Path.GetDirectoryName(Application.ExecutablePath)
                };
                int count = this.imageList_0.Images.Count;
                foreach (Assembly assembly in loader.Assemblies)
                {
                    bool flag = false;
                    TreeNode item = new TreeNode(loader.GetAssemblyDescription(assembly)) {
                        ImageIndex = int_1,
                        SelectedImageIndex = int_0,
                        Tag = assembly
                    };
                    list_1.Add(item);
                    foreach (System.Type type in loader.TypesInAssembly(assembly))
                    {
                        if (!flag)
                        {
                            Stream manifestResourceStream = assembly.GetManifestResourceStream(type, "glyph.bmp");
                            if (manifestResourceStream != null)
                            {
                                Bitmap bitmap = new Bitmap(manifestResourceStream);
                                list_2.Add(bitmap);
                                int num2 = count++;
                                item.ImageIndex = num2;
                                item.SelectedImageIndex = num2;
                            }
                            flag = true;
                        }
                        IndicatorHelper helper = (IndicatorHelper) loader.CreateInstance(type);
                        TreeNode node = new TreeNode(helper.IndicatorType.Name);
                        if (helper.Glyph != null)
                        {
                            this.imageList_0.Images.Add(helper.Glyph);
                            node.ImageIndex = this.imageList_0.Images.Count - 1;
                        }
                        else if (helper.PartnerBandIndicatorType != null)
                        {
                            node.ImageIndex = int_3;
                        }
                        else if (helper.IsOscillator)
                        {
                            node.ImageIndex = int_4;
                        }
                        else if (helper.DefaultStyle == LineStyle.Histogram)
                        {
                            node.ImageIndex = int_5;
                        }
                        else if (helper.DefaultStyle == LineStyle.Dots)
                        {
                            node.ImageIndex = int_6;
                        }
                        else
                        {
                            node.ImageIndex = int_2;
                        }
                        node.SelectedImageIndex = node.ImageIndex;
                        node.Tag = helper;
                        item.Nodes.Add(node);
                        indicatorCount++;
                    }
                }
            }
            foreach (Bitmap bitmap2 in list_2)
            {
                this.imageList_0.Images.Add(bitmap2);
            }
            foreach (TreeNode node3 in list_1)
            {
                TreeNode node4 = (TreeNode) node3.Clone();
                base.Nodes.Add(node4);
            }
            base.RecallExpandState();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public TreeNode FindIndicatorNode(string indicatorName)
        {
            foreach (TreeNode node in base.Nodes)
            {
                foreach (TreeNode node2 in node.Nodes)
                {
                    if (node2.Text == indicatorName)
                    {
                        return node2;
                    }
                }
            }
            return null;
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(IndicatorTreeView));
            this.imageList_0 = new ImageList(this.icontainer_0);
            base.SuspendLayout();
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("imgList.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "FolderOpen.bmp");
            this.imageList_0.Images.SetKeyName(1, "FolderClosed.bmp");
            this.imageList_0.Images.SetKeyName(2, "Indicator.bmp");
            this.imageList_0.Images.SetKeyName(3, "Bands.bmp");
            this.imageList_0.Images.SetKeyName(4, "Oscillator.bmp");
            this.imageList_0.Images.SetKeyName(5, "Histogram.bmp");
            this.imageList_0.Images.SetKeyName(6, "Dots.bmp");
            base.ResumeLayout(false);
        }

        protected override void OnAfterSelect(TreeViewEventArgs treeViewEventArgs_0)
        {
            base.OnAfterSelect(treeViewEventArgs_0);
            if (treeViewEventArgs_0.Node.Level == 1)
            {
                IndicatorHelper tag = treeViewEventArgs_0.Node.Tag as IndicatorHelper;
                if ((tag != null) && (this.eventHandler_0 != null))
                {
                    this.eventHandler_0(this, new IndicatorEventArgs(tag));
                }
            }
        }

        public void SynchIndicatorHelpers(List<IndicatorHelper> helpers)
        {
            foreach (IndicatorHelper helper in helpers)
            {
                TreeNode node = this.FindIndicatorNode(helper.IndicatorType.Name);
                if (node != null)
                {
                    node.Tag = helper;
                }
            }
        }

        [Browsable(false)]
        public int IndicatorCount
        {
            get
            {
                return indicatorCount;
            }
        }

        [Browsable(false)]
        public IndicatorHelper SelectedIndicator
        {
            get
            {
                TreeNode selectedNode = base.SelectedNode;
                if (selectedNode == null)
                {
                    return null;
                }
                if (selectedNode.Level == 0)
                {
                    return null;
                }
                return (selectedNode.Tag as IndicatorHelper);
            }
        }
    }
}

