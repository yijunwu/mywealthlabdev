namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class QuickRefForm : Form
    {
        private ToolStripDropDownButton btnPrint;
        private ToolStripMenuItem btnPrintPreview;
        private ToolStripMenuItem btnPrintPrint;
        private ToolStripButton btnTop;
        private ToolStripComboBox comboFind;
        private WebBrowser html;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        public static QuickRefForm Instance;
        private const int int_0 = 0;
        private const int int_1 = 1;
        private const int int_2 = 2;
        private const int int_3 = 3;
        private const int int_4 = 4;
        private const int int_5 = 5;
        private ToolStripStatusLabel lblEntries;
        private ToolStripStatusLabel lblMoreInfo;
        private QuickRefManager quickRefManager_0;
        private SplitContainer split;
        private StatusStrip status;
        private const string string_0 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html><hr><font color=\"0000FF\"><h2>Example</h2></font><xmlns='http://www.w3.org/1999/xhtml'>\r\n<head>\r\n<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />\r\n<title></title>\r\n<style type='text/css' media='all'>\r\n<pre.wlnetcode>\r\n{\r\n\tbackground: #FFFFFF;\r\n\tcolor: #000000;;\r\n\tfont: 10pt \"Courier New\";\r\n}\r\n.wlnet01{\r\n\tcolor: #007F00;\r\n}\r\n.wlnet03{\r\n}\r\n.wlnet04{\r\n\tcolor: #007F7F;\r\n}\r\n.wlnet05{\r\n\tcolor: #0000FF;\r\n\tfont-weight: bold;\r\n}\r\n.wlnet06{\r\n\tcolor: #DC143C;\r\n}\r\n.wlnet07{\r\n\tcolor: #A52A00;\r\n}\r\n.wlnet10{\r\n\tcolor: #7F007F;\r\n}\r\n.wlnet12{\r\n\tcolor: #7F7F00;\r\n}\r\n.wlnet13{\r\n\tcolor: #478698;\r\n}\r\n.wlnet32{\r\n}\r\n.wlnet34{\r\n\tcolor: #0000EE;\r\n\tfont-weight: bold;\r\n}\r\n.wlnet35{\r\n\tcolor: #EE0000;\r\n\tfont-weight: bold;\r\n}\r\n.wlnet36{\r\n\tcolor: #808080;\r\n}\r\n</style>\r\n</head>\r\n<body>";
        private const string string_1 = "<pre class='wlnetcode'><span class='wlnet05'>using</span> <span class='wlnet03'>System</span>;";
        private const string string_2 = "<pre class='wlnetcode'><span class='wlnet05'>protected</span> <span class='wlnet05'>override</span> <span class='wlnet05'>void</span> <span class='wlnet03'>Execute</span>(){";
        private string string_3;
        private ToolStrip toolbar;
        private ToolStripLabel toolStripFindLabel;
        private TreeView treeQuickRef;

        public QuickRefForm()
        {
            this.InitializeComponent();
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            this.html.ShowPrintPreviewDialog();
        }

        private void btnPrintPrint_Click(object sender, EventArgs e)
        {
            this.html.ShowPrintDialog();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            this.btnTop.Checked = !this.btnTop.Checked;
            base.TopMost = this.btnTop.Checked;
        }

        private void comboFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowTopicByName(this.comboFind.SelectedItem.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(QuickRefForm));
            this.status = new StatusStrip();
            this.lblEntries = new ToolStripStatusLabel();
            this.lblMoreInfo = new ToolStripStatusLabel();
            this.toolbar = new ToolStrip();
            this.btnTop = new ToolStripButton();
            this.toolStripFindLabel = new ToolStripLabel();
            this.comboFind = new ToolStripComboBox();
            this.split = new SplitContainer();
            this.treeQuickRef = new TreeView();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.html = new WebBrowser();
            this.btnPrint = new ToolStripDropDownButton();
            this.btnPrintPrint = new ToolStripMenuItem();
            this.btnPrintPreview = new ToolStripMenuItem();
            this.quickRefManager_0 = new QuickRefManager(this.icontainer_0);
            this.status.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            base.SuspendLayout();
            this.status.BackColor = Color.Beige;
            this.status.Items.AddRange(new ToolStripItem[] { this.lblEntries, this.lblMoreInfo });
            this.status.Location = new Point(0, 390);
            this.status.Name = "status";
            this.status.Size = new Size(0x19b, 0x16);
            this.status.TabIndex = 1;
            this.status.Text = "statusStrip1";
            this.lblEntries.Name = "lblEntries";
            this.lblEntries.Size = new Size(0x31, 0x11);
            this.lblEntries.Text = "0 Entries";
            this.lblMoreInfo.IsLink = true;
            this.lblMoreInfo.Name = "lblMoreInfo";
            this.lblMoreInfo.Size = new Size(0x45, 0x11);
            this.lblMoreInfo.Text = "More Info ...";
            this.lblMoreInfo.Visible = false;
            this.toolbar.BackColor = Color.Beige;
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.toolStripFindLabel, this.comboFind, this.btnTop, this.btnPrint });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x19b, 0x19);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            this.btnTop.Alignment = ToolStripItemAlignment.Right;
            this.btnTop.Image = (Image) resources.GetObject("btnTop.Image");
            this.btnTop.ImageTransparentColor = Color.Magenta;
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new Size(0x44, 0x16);
            this.btnTop.Text = "Topmost";
            this.btnTop.ToolTipText = "Keep this Window on top";
            this.btnTop.Click += new EventHandler(this.btnTop_Click);
            this.toolStripFindLabel.Name = "toolStripFindLabel";
            this.toolStripFindLabel.Size = new Size(0x1f, 0x16);
            this.toolStripFindLabel.Text = "Find:";
            this.comboFind.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.comboFind.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.comboFind.DropDownHeight = 300;
            this.comboFind.MaxDropDownItems = 0x19;
            this.comboFind.MaxLength = 100;
            this.comboFind.Name = "comboFind";
            this.comboFind.Size = new Size(0x9b, 0x19);
            this.comboFind.Sorted = true;
            this.comboFind.ToolTipText = "Find WealthScript Function or Property";
            this.comboFind.SelectedIndexChanged += new EventHandler(this.comboFind_SelectedIndexChanged);
            this.split.Dock = DockStyle.Fill;
            this.split.Location = new Point(0, 0x19);
            this.split.Name = "split";
            this.split.Panel1.Controls.Add(this.treeQuickRef);
            this.split.Panel2.Controls.Add(this.html);
            this.split.Size = new Size(0x19b, 0x16d);
            this.split.SplitterDistance = 180;
            this.split.TabIndex = 2;
            this.treeQuickRef.BackColor = Color.Beige;
            this.treeQuickRef.Dock = DockStyle.Fill;
            this.treeQuickRef.HideSelection = false;
            this.treeQuickRef.ImageIndex = 0;
            this.treeQuickRef.ImageList = this.imageList_0;
            this.treeQuickRef.Location = new Point(0, 0);
            this.treeQuickRef.Name = "treeQuickRef";
            this.treeQuickRef.SelectedImageIndex = 0;
            this.treeQuickRef.Size = new Size(180, 0x16d);
            this.treeQuickRef.TabIndex = 0;
            this.treeQuickRef.AfterSelect += new TreeViewEventHandler(this.treeQuickRef_AfterSelect);
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "FolderClosed.bmp");
            this.imageList_0.Images.SetKeyName(1, "FolderOpen.bmp");
            this.imageList_0.Images.SetKeyName(2, "method.bmp");
            this.imageList_0.Images.SetKeyName(3, "property.bmp");
            this.imageList_0.Images.SetKeyName(4, "ObjectClosed.bmp");
            this.imageList_0.Images.SetKeyName(5, "ObjectOpen.bmp");
            this.html.AllowWebBrowserDrop = false;
            this.html.Dock = DockStyle.Fill;
            this.html.Location = new Point(0, 0);
            this.html.MinimumSize = new Size(20, 20);
            this.html.Name = "html";
            this.html.Size = new Size(0xe3, 0x16d);
            this.html.TabIndex = 0;
            this.btnPrint.Alignment = ToolStripItemAlignment.Right;
            this.btnPrint.DropDownItems.AddRange(new ToolStripItem[] { this.btnPrintPrint, this.btnPrintPreview });
            this.btnPrint.Image = (Image) resources.GetObject("btnPrint.Image");
            this.btnPrint.ImageTransparentColor = Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x3a, 0x16);
            this.btnPrint.Text = "Print";
            this.btnPrintPrint.Name = "btnPrintPrint";
            this.btnPrintPrint.Size = new Size(0x98, 0x16);
            this.btnPrintPrint.Text = "Print...";
            this.btnPrintPrint.Click += new EventHandler(this.btnPrintPrint_Click);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new Size(0x98, 0x16);
            this.btnPrintPreview.Text = "Preview...";
            this.btnPrintPreview.Click += new EventHandler(this.btnPrintPreview_Click);
            this.BackColor = Color.Beige;
            base.ClientSize = new Size(0x19b, 0x19c);
            base.Controls.Add(this.split);
            base.Controls.Add(this.toolbar);
            base.Controls.Add(this.status);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            base.Name = "QuickRefForm";
            base.ShowInTaskbar = false;
            this.Text = "WealthScript QuickRef";
            base.FormClosed += new FormClosedEventHandler(this.QuickRefForm_FormClosed);
            base.Load += new EventHandler(this.QuickRefForm_Load);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            this.split.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, ProcessNodeEventArgs e)
        {
            if (e.Node.Text == this.string_3)
            {
                this.treeQuickRef.SelectedNode = e.Node;
                e.StopProcessing = true;
            }
        }

        private void QuickRefForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.Tag = this.split.SplitterDistance.ToString();
            MainModule.Instance.Settings.Set(this, "QuickRefForm");
            Instance = null;
        }

        private void QuickRefForm_Load(object sender, EventArgs e)
        {
            int num = 0;
            if (MainModule.Instance.Settings.Get(this, "QuickRefForm"))
            {
                string tag = base.Tag as string;
                try
                {
                    this.split.SplitterDistance = int.Parse(tag);
                }
                catch
                {
                }
            }
            Instance = this;
            string fileName = MainModule.Instance.AppPath + @"\QuickRef.xml";
            this.quickRefManager_0.LoadFromFile(fileName);
            this.treeQuickRef.BeginUpdate();
            foreach (QuickRefCategory category in this.quickRefManager_0.Categories)
            {
                TreeNode node2 = this.treeQuickRef.Nodes.Add(category.Name);
                if (category.CategoryType == CategoryType.Functional)
                {
                    node2.ImageIndex = 0;
                    node2.SelectedImageIndex = 1;
                }
                else
                {
                    node2.ImageIndex = 4;
                    node2.SelectedImageIndex = 5;
                }
                if (-1 == this.comboFind.FindStringExact(category.Name))
                {
                    this.comboFind.Items.Add(category.Name);
                }
                node2.Tag = category;
                foreach (QuickRefEntry entry in category.Entries)
                {
                    num++;
                    TreeNode node = node2.Nodes.Add(entry.Name);
                    if (entry.EntryType == EntryType.Method)
                    {
                        node.ImageIndex = 2;
                    }
                    else
                    {
                        node.ImageIndex = 3;
                    }
                    node.SelectedImageIndex = node.ImageIndex;
                    node.Tag = entry;
                    if (-1 == this.comboFind.FindStringExact(entry.Name))
                    {
                        this.comboFind.Items.Add(entry.Name);
                    }
                }
            }
            this.treeQuickRef.EndUpdate();
            this.lblEntries.Text = num + " Entries";
        }

        public bool ShowTopicByName(string topicToShow)
        {
            TreeViewWalker walker = new TreeViewWalker(this.treeQuickRef);
            walker.ProcessNode += new ProcessNodeEventHandler(this.method_0);
            this.string_3 = topicToShow;
            walker.ProcessTree();
            return false;
        }

        private void treeQuickRef_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                string path = MainModule.Instance.DataPath + @"\temp.html";
                if (node.Level == 0)
                {
                    QuickRefCategory tag = (QuickRefCategory) node.Tag;
                    File.WriteAllText(path, tag.Description);
                }
                else
                {
                    QuickRefEntry entry = (QuickRefEntry) node.Tag;
                    File.WriteAllText(path, entry.Description);
                    if (entry.Example.Length > 0)
                    {
                        File.AppendAllText(path, "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html><hr><font color=\"0000FF\"><h2>Example</h2></font><xmlns='http://www.w3.org/1999/xhtml'>\r\n<head>\r\n<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />\r\n<title></title>\r\n<style type='text/css' media='all'>\r\n<pre.wlnetcode>\r\n{\r\n\tbackground: #FFFFFF;\r\n\tcolor: #000000;;\r\n\tfont: 10pt \"Courier New\";\r\n}\r\n.wlnet01{\r\n\tcolor: #007F00;\r\n}\r\n.wlnet03{\r\n}\r\n.wlnet04{\r\n\tcolor: #007F7F;\r\n}\r\n.wlnet05{\r\n\tcolor: #0000FF;\r\n\tfont-weight: bold;\r\n}\r\n.wlnet06{\r\n\tcolor: #DC143C;\r\n}\r\n.wlnet07{\r\n\tcolor: #A52A00;\r\n}\r\n.wlnet10{\r\n\tcolor: #7F007F;\r\n}\r\n.wlnet12{\r\n\tcolor: #7F7F00;\r\n}\r\n.wlnet13{\r\n\tcolor: #478698;\r\n}\r\n.wlnet32{\r\n}\r\n.wlnet34{\r\n\tcolor: #0000EE;\r\n\tfont-weight: bold;\r\n}\r\n.wlnet35{\r\n\tcolor: #EE0000;\r\n\tfont-weight: bold;\r\n}\r\n.wlnet36{\r\n\tcolor: #808080;\r\n}\r\n</style>\r\n</head>\r\n<body>");
                        if (!entry.Example.StartsWith("<pre class='wlnetcode'><span class='wlnet05'>using</span> <span class='wlnet03'>System</span>;"))
                        {
                            File.AppendAllText(path, "<pre class='wlnetcode'><span class='wlnet05'>protected</span> <span class='wlnet05'>override</span> <span class='wlnet05'>void</span> <span class='wlnet03'>Execute</span>(){");
                        }
                        File.AppendAllText(path, entry.Example);
                    }
                }
                this.html.Navigate(path);
            }
        }
    }
}

