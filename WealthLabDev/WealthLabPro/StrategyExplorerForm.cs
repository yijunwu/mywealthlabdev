namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using WealthLab;

    public class StrategyExplorerForm : Form
    {
        private bool bool_0 = true;
        private WebBrowser browser;
        private ToolStripButton btnAccountNumber;
        private ToolStripButton btnAddNetwork;
        private Button btnCancel;
        private ToolStripButton btnDelete;
        private ToolStripButton btnDeleteFolder;
        private ToolStripButton btnDownload;
        private ToolStripButton btnHelp;
        private ToolStripButton btnImport;
        private ToolStripButton btnNewFolder;
        private ToolStripButton btnNewRules;
        private ToolStripButton btnNewScript;
        private Button btnOK;
        private ToolStripButton btnRemoveNetwork;
        private ToolStripButton btnSearch;
        private Button btnSearchStrategies;
        private Button btnShowDownloaded;
        private ToolStripButton btnTree;
        private ComboBox cmbSearchAuthor;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private FolderBrowserDialog folderBrowserDialog_0;
        private GroupBox grpSearch;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private const int int_0 = 0;
        private const int int_1 = 1;
        private const int int_2 = 2;
        private const int int_3 = 3;
        private const int int_4 = 4;
        private const int int_5 = 5;
        private const int int_6 = 6;
        private Label lblSearchAuthor;
        private Label lblSearchCode;
        private Label lblSearchCriteria;
        private Label lblSearchName;
        private ToolStripStatusLabel lblStrategies;
        private LinkLabel linkMoreInfo;
        private List<Strategy> list_0 = new List<Strategy>();
        private SortableListView lvStrategies;
        private OpenFileDialog openFileDialog_0;
        private Panel pnlBottom;
        private Panel pnlSearch;
        private ToolStripSeparator sepAccount;
        private ToolStripSeparator sepDelete;
        private ToolStripSeparator sepFolders;
        private ToolStripSeparator sepImport;
        private SplitContainer split;
        private SplitContainer splitDetails;
        private StatusStrip status;
        private StrategyDownload strategyDownload_0 = new StrategyDownload();
        private ToolStrip toolbar;
        private ToolStrip toolbarNetFolder;
        private ToolStrip toolbarTree;
        private ToolStripSeparator toolStripSeparator1;
        private TreeView tree;
        private TextBox txtSearchCode;
        private TextBox txtSearchName;

        public StrategyExplorerForm()
        {
            this.InitializeComponent();
        }

        private void browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string str = e.Url.ToString().ToUpper();
            if (str.StartsWith("HTTP"))
            {
                if (this.bool_0)
                {
                    e.Cancel = !MainModule.Instance.NavigateToThirdPartySite(str);
                }
                if (!e.Cancel)
                {
                    this.bool_0 = false;
                }
            }
        }

        private void btnAccountNumber_Click(object sender, EventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count != 0)
            {
                StrategyAccountForm form = new StrategyAccountForm();
                Strategy tag = (Strategy) this.lvStrategies.SelectedItems[0].Tag;
                form.AccountNumber = tag.AccountNumber;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (ListViewItem item in this.lvStrategies.SelectedItems)
                    {
                        tag = (Strategy) item.Tag;
                        tag.AccountNumber = form.AccountNumber;
                        item.SubItems[4].Text = form.AccountNumber;
                        if (tag.StrategyType == StrategyType.Compiled)
                        {
                            MainModule.Instance.Strategies.SetAccountNumberForPrecompiledStrategy(tag, tag.AccountNumber);
                        }
                        else
                        {
                            MainModule.Instance.Strategies.SaveStrategy(tag);
                        }
                    }
                    if (this.lvStrategies.SelectedItems.Count == 1)
                    {
                        MessageBox.Show("Set Account Number for 1 Strategy");
                    }
                    else
                    {
                        MessageBox.Show("Set Account Number for " + this.lvStrategies.SelectedItems.Count + " Strategies");
                    }
                }
            }
        }

        private void btnAddNetwork_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog_0.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = this.folderBrowserDialog_0.SelectedPath;
                if (!MainModule.Instance.StrategyNetworkPaths.Contains(selectedPath))
                {
                    MainModule.Instance.StrategyNetworkPaths.Add(selectedPath);
                    MainModule.Instance.SaveSettings();
                    MainModule.Instance.Strategies.LoadStrategiesFromNetworkPath(selectedPath);
                    try
                    {
                        TreeNode node = this.method_0(selectedPath);
                        if (node.Nodes.Count == 0)
                        {
                            try
                            {
                                MainModule.Instance.Strategies.AddFolderToNetworkPath(selectedPath, "Strategies");
                                TreeNode node2 = node.Nodes.Add("Strategies");
                                node2.ImageIndex = 0;
                                node2.SelectedImageIndex = 1;
                                node.ExpandAll();
                                this.tree.SelectedNode = node2;
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Failed to add Network Folder: " + exception.Message);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete the selected Strategies?", "Delete Strategies", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (ListViewItem item in this.lvStrategies.SelectedItems)
                {
                    Strategy tag = item.Tag as Strategy;
                    if (tag.StrategyType != StrategyType.Compiled)
                    {
                        MainModule.Instance.Strategies.DeleteStrategy(tag);
                        MainModule.Instance.DeleteStrategyFromMRU(tag);
                    }
                }
                this.tree_AfterSelect(this.tree, new TreeViewEventArgs(this.tree.SelectedNode));
            }
        }

        private void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.tree.SelectedNode;
            if (((selectedNode != null) && (selectedNode.ImageIndex != 2)) && (selectedNode.ImageIndex != 6))
            {
                string networkPath = "";
                if (selectedNode.Parent != null)
                {
                    networkPath = selectedNode.Parent.Text;
                }
                if (MainModule.Instance.Strategies.DeleteFolder(selectedNode.Text, networkPath))
                {
                    this.tree.Nodes.Remove(selectedNode);
                }
                this.lblStrategies.Text = MainModule.Instance.Strategies.Strategies.Count + " Strategies";
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.tree.Nodes.Count != 0)
            {
                if (this.tree.SelectedNode == null)
                {
                    this.tree.SelectedNode = this.tree.Nodes[0];
                }
                if ((this.strategyDownload_0.ShowDialog() == DialogResult.OK) && this.strategyDownload_0.StrategiesAdded)
                {
                    this.method_1();
                    this.method_2();
                    this.tree_AfterSelect(this, new TreeViewEventArgs(this.tree.SelectedNode));
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("strategy_explorer.htm");
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.tree.Nodes.Count != 0)
            {
                if (this.tree.SelectedNode == null)
                {
                    this.tree.SelectedNode = this.tree.Nodes[0];
                }
                if (this.openFileDialog_0.ShowDialog() == DialogResult.OK)
                {
                    int num2 = 0;
                    string[] fileNames = this.openFileDialog_0.FileNames;
                    int index = 0;
                    while (true)
                    {
                        if (index >= fileNames.Length)
                        {
                            break;
                        }
                        string path = fileNames[index];
                        XmlSerializer serializer = new XmlSerializer(typeof(Strategy));
                        TextReader textReader = new StreamReader(path);
                        try
                        {
                            Strategy strategy = (Strategy) serializer.Deserialize(textReader);
                            string networkPath = "";
                            if (this.tree.SelectedNode.Parent != null)
                            {
                                networkPath = this.tree.SelectedNode.Parent.Text;
                            }
                            MainModule.Instance.Strategies.SaveStrategy(strategy, this.tree.SelectedNode.Text, networkPath);
                            textReader.Close();
                            num2++;
                        }
                        catch (Exception)
                        {
                            textReader.Close();
                            MessageBox.Show("Failed to import: " + path);
                        }
                        index++;
                    }
                    MessageBox.Show("Imported " + num2 + " Strategies");
                    this.tree_AfterSelect(this.tree, new TreeViewEventArgs(this.tree.SelectedNode));
                }
            }
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            string folder = InputBox.Show("New Folder", "Folder Name:");
            if (folder != "")
            {
                TreeNode selectedNode = this.tree.SelectedNode;
                if ((selectedNode != null) && (selectedNode.Parent != null))
                {
                    selectedNode = selectedNode.Parent;
                }
                if ((selectedNode != null) && (selectedNode.ImageIndex == 6))
                {
                    if (MainModule.Instance.Strategies.AddFolderToNetworkPath(selectedNode.Text, folder))
                    {
                        TreeNode node2 = selectedNode.Nodes.Add(folder);
                        node2.ImageIndex = 0;
                        node2.SelectedImageIndex = 1;
                        selectedNode.Expand();
                    }
                }
                else if (MainModule.Instance.Strategies.AddFolder(folder, true))
                {
                    TreeNode node3 = this.tree.Nodes.Add(folder);
                    node3.ImageIndex = 0;
                    node3.SelectedImageIndex = 1;
                }
            }
        }

        private void btnNewRules_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.No;
        }

        private void btnNewScript_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Yes;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count > 0)
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void btnRemoveNetwork_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Unmap this Network Path?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TreeNode selectedNode = this.tree.SelectedNode;
                if (MainModule.Instance.StrategyNetworkPaths.Contains(selectedNode.Text))
                {
                    MainModule.Instance.StrategyNetworkPaths.Remove(selectedNode.Text);
                    MainModule.Instance.SaveSettings();
                    MainModule.Instance.Strategies.RemoveNetworkPath(selectedNode.Text);
                }
                this.tree.Nodes.Remove(selectedNode);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.btnSearch.Checked = true;
            this.btnTree.Checked = false;
            base.AcceptButton = this.btnSearchStrategies;
            this.pnlSearch.BringToFront();
        }

        private void btnSearchStrategies_Click(object sender, EventArgs e)
        {
            string str = this.txtSearchName.Text.ToUpper();
            string str2 = this.txtSearchCode.Text.ToUpper();
            string str3 = this.cmbSearchAuthor.Text.ToUpper();
            this.lvStrategies.BeginUpdate();
            try
            {
                this.lvStrategies.Items.Clear();
                foreach (Strategy strategy in MainModule.Instance.Strategies.Strategies)
                {
                    bool flag = true;
                    if ((this.txtSearchName.Text != "") && !strategy.Name.ToUpper().Contains(str))
                    {
                        flag = false;
                    }
                    if ((flag && (this.txtSearchCode.Text != "")) && !strategy.Code.ToUpper().Contains(str2))
                    {
                        flag = false;
                    }
                    if ((flag && (this.cmbSearchAuthor.Text != "")) && !strategy.Author.ToUpper().Contains(str3))
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        this.method_3(strategy);
                    }
                }
            }
            finally
            {
                this.lvStrategies.EndUpdate();
            }
        }

        private void btnShowDownloaded_Click(object sender, EventArgs e)
        {
            this.lvStrategies.BeginUpdate();
            try
            {
                this.lvStrategies.Items.Clear();
                foreach (Strategy strategy in MainModule.Instance.Strategies.Strategies)
                {
                    if (!string.IsNullOrEmpty(strategy.Origin))
                    {
                        this.method_3(strategy);
                    }
                }
            }
            finally
            {
                this.lvStrategies.EndUpdate();
            }
        }

        private void btnTree_Click(object sender, EventArgs e)
        {
            this.btnTree.Checked = true;
            this.btnSearch.Checked = false;
            base.AcceptButton = this.btnOK;
            this.tree.BringToFront();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(StrategyExplorerForm));
            this.status = new StatusStrip();
            this.lblStrategies = new ToolStripStatusLabel();
            this.toolbar = new ToolStrip();
            this.btnNewRules = new ToolStripButton();
            this.btnNewScript = new ToolStripButton();
            this.sepImport = new ToolStripSeparator();
            this.btnImport = new ToolStripButton();
            this.btnDownload = new ToolStripButton();
            this.sepDelete = new ToolStripSeparator();
            this.btnDelete = new ToolStripButton();
            this.sepAccount = new ToolStripSeparator();
            this.btnAccountNumber = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnHelp = new ToolStripButton();
            this.split = new SplitContainer();
            this.toolbarTree = new ToolStrip();
            this.btnNewFolder = new ToolStripButton();
            this.btnDeleteFolder = new ToolStripButton();
            this.sepFolders = new ToolStripSeparator();
            this.btnTree = new ToolStripButton();
            this.btnSearch = new ToolStripButton();
            this.toolbarNetFolder = new ToolStrip();
            this.btnAddNetwork = new ToolStripButton();
            this.btnRemoveNetwork = new ToolStripButton();
            this.tree = new TreeView();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.pnlSearch = new Panel();
            this.grpSearch = new GroupBox();
            this.lblSearchCriteria = new Label();
            this.btnSearchStrategies = new Button();
            this.cmbSearchAuthor = new ComboBox();
            this.lblSearchAuthor = new Label();
            this.lblSearchName = new Label();
            this.txtSearchName = new TextBox();
            this.txtSearchCode = new TextBox();
            this.lblSearchCode = new Label();
            this.btnShowDownloaded = new Button();
            this.splitDetails = new SplitContainer();
            this.lvStrategies = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.browser = new WebBrowser();
            this.pnlBottom = new Panel();
            this.linkMoreInfo = new LinkLabel();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.openFileDialog_0 = new OpenFileDialog();
            this.folderBrowserDialog_0 = new FolderBrowserDialog();
            this.status.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.split.BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.toolbarTree.SuspendLayout();
            this.toolbarNetFolder.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.splitDetails.BeginInit();
            this.splitDetails.Panel1.SuspendLayout();
            this.splitDetails.Panel2.SuspendLayout();
            this.splitDetails.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            base.SuspendLayout();
            this.status.Items.AddRange(new ToolStripItem[] { this.lblStrategies });
            this.status.Location = new Point(0, 0x19f);
            this.status.Name = "status";
            this.status.Size = new Size(0x2c3, 0x16);
            this.status.TabIndex = 0;
            this.status.Text = "statusStrip1";
            this.lblStrategies.Name = "lblStrategies";
            this.lblStrategies.Size = new Size(0x41, 0x11);
            this.lblStrategies.Text = "0 Strategies";
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.btnNewRules, this.btnNewScript, this.sepImport, this.btnImport, this.btnDownload, this.sepDelete, this.btnDelete, this.sepAccount, this.btnAccountNumber, this.toolStripSeparator1, this.btnHelp });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.ShowItemToolTips = false;
            this.toolbar.Size = new Size(0x2c3, 0x19);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            this.btnNewRules.Image = (Image) manager.GetObject("btnNewRules.Image");
            this.btnNewRules.ImageTransparentColor = Color.Magenta;
            this.btnNewRules.Name = "btnNewRules";
            this.btnNewRules.Size = new Size(150, 0x16);
            this.btnNewRules.Text = "New Rule-Based Strategy";
            this.btnNewRules.Click += new EventHandler(this.btnNewRules_Click);
            this.btnNewScript.Image = (Image) manager.GetObject("btnNewScript.Image");
            this.btnNewScript.ImageTransparentColor = Color.Magenta;
            this.btnNewScript.Name = "btnNewScript";
            this.btnNewScript.Size = new Size(0x9c, 0x16);
            this.btnNewScript.Text = "New Script-Based Strategy";
            this.btnNewScript.Click += new EventHandler(this.btnNewScript_Click);
            this.sepImport.Name = "sepImport";
            this.sepImport.Size = new Size(6, 0x19);
            this.btnImport.Image = (Image) manager.GetObject("btnImport.Image");
            this.btnImport.ImageTransparentColor = Color.FromArgb(0xeb, 0xe9, 0xed);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new Size(0x4a, 0x16);
            this.btnImport.Text = "Import ...";
            this.btnImport.ToolTipText = "Import Strategies from external XML files";
            this.btnImport.Click += new EventHandler(this.btnImport_Click);
            this.btnDownload.Image = (Image) manager.GetObject("btnDownload.Image");
            this.btnDownload.ImageTransparentColor = Color.Magenta;
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new Size(0x59, 0x16);
            this.btnDownload.Text = "Download ...";
            this.btnDownload.Click += new EventHandler(this.btnDownload_Click);
            this.sepDelete.Name = "sepDelete";
            this.sepDelete.Size = new Size(6, 0x19);
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = (Image) manager.GetObject("btnDelete.Image");
            this.btnDelete.ImageTransparentColor = Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(0x3a, 0x16);
            this.btnDelete.Text = "Delete";
            this.btnDelete.ToolTipText = "Delete the Selected Strategies";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.sepAccount.Name = "sepAccount";
            this.sepAccount.Size = new Size(6, 0x19);
            this.btnAccountNumber.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnAccountNumber.Enabled = false;
            this.btnAccountNumber.Image = (Image) manager.GetObject("btnAccountNumber.Image");
            this.btnAccountNumber.ImageTransparentColor = Color.Magenta;
            this.btnAccountNumber.Name = "btnAccountNumber";
            this.btnAccountNumber.Size = new Size(0x54, 0x16);
            this.btnAccountNumber.Text = "Set Account ...";
            this.btnAccountNumber.ToolTipText = "Set the Account Number to use when this Strategy generates Orders";
            this.btnAccountNumber.Click += new EventHandler(this.btnAccountNumber_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.btnHelp.Image = (Image) manager.GetObject("btnHelp.Image");
            this.btnHelp.ImageTransparentColor = Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x30, 0x16);
            this.btnHelp.Text = "Help";
            this.btnHelp.ToolTipText = "Help on Strategy Explorer";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.split.Dock = DockStyle.Fill;
            this.split.Location = new Point(0, 0x19);
            this.split.Name = "split";
            this.split.Panel1.Controls.Add(this.toolbarTree);
            this.split.Panel1.Controls.Add(this.toolbarNetFolder);
            this.split.Panel1.Controls.Add(this.tree);
            this.split.Panel1.Controls.Add(this.pnlSearch);
            this.split.Panel2.Controls.Add(this.splitDetails);
            this.split.Size = new Size(0x2c3, 390);
            this.split.SplitterDistance = 0x109;
            this.split.TabIndex = 2;
            this.toolbarTree.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarTree.Items.AddRange(new ToolStripItem[] { this.btnNewFolder, this.btnDeleteFolder, this.sepFolders, this.btnTree, this.btnSearch });
            this.toolbarTree.Location = new Point(0, 0);
            this.toolbarTree.Name = "toolbarTree";
            this.toolbarTree.Size = new Size(0x109, 0x19);
            this.toolbarTree.TabIndex = 0;
            this.toolbarTree.Text = "toolStrip1";
            this.btnNewFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnNewFolder.Image = (Image) manager.GetObject("btnNewFolder.Image");
            this.btnNewFolder.ImageTransparentColor = Color.Magenta;
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.Size = new Size(0x17, 0x16);
            this.btnNewFolder.Text = "Create a New Folder";
            this.btnNewFolder.Click += new EventHandler(this.btnNewFolder_Click);
            this.btnDeleteFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnDeleteFolder.Enabled = false;
            this.btnDeleteFolder.Image = (Image) manager.GetObject("btnDeleteFolder.Image");
            this.btnDeleteFolder.ImageTransparentColor = Color.Magenta;
            this.btnDeleteFolder.Name = "btnDeleteFolder";
            this.btnDeleteFolder.Size = new Size(0x17, 0x16);
            this.btnDeleteFolder.Text = "toolStripButton1";
            this.btnDeleteFolder.ToolTipText = "Delete selected Folder";
            this.btnDeleteFolder.Click += new EventHandler(this.btnDeleteFolder_Click);
            this.sepFolders.Name = "sepFolders";
            this.sepFolders.Size = new Size(6, 0x19);
            this.btnTree.Checked = true;
            this.btnTree.CheckState = CheckState.Checked;
            this.btnTree.Image = (Image) manager.GetObject("btnTree.Image");
            this.btnTree.ImageTransparentColor = Color.Magenta;
            this.btnTree.Name = "btnTree";
            this.btnTree.Size = new Size(0x3e, 0x16);
            this.btnTree.Text = "Folders";
            this.btnTree.Click += new EventHandler(this.btnTree_Click);
            this.btnSearch.Image = (Image) manager.GetObject("btnSearch.Image");
            this.btnSearch.ImageTransparentColor = Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(60, 0x16);
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.toolbarNetFolder.Dock = DockStyle.Bottom;
            this.toolbarNetFolder.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarNetFolder.Items.AddRange(new ToolStripItem[] { this.btnAddNetwork, this.btnRemoveNetwork });
            this.toolbarNetFolder.Location = new Point(0, 0x16d);
            this.toolbarNetFolder.Name = "toolbarNetFolder";
            this.toolbarNetFolder.Size = new Size(0x109, 0x19);
            this.toolbarNetFolder.TabIndex = 11;
            this.toolbarNetFolder.Text = "toolStrip1";
            this.btnAddNetwork.Image = (Image) manager.GetObject("btnAddNetwork.Image");
            this.btnAddNetwork.ImageTransparentColor = Color.Magenta;
            this.btnAddNetwork.Name = "btnAddNetwork";
            this.btnAddNetwork.Size = new Size(0x73, 0x16);
            this.btnAddNetwork.Text = "Map Network Path";
            this.btnAddNetwork.Click += new EventHandler(this.btnAddNetwork_Click);
            this.btnRemoveNetwork.Enabled = false;
            this.btnRemoveNetwork.Image = (Image) manager.GetObject("btnRemoveNetwork.Image");
            this.btnRemoveNetwork.ImageTransparentColor = Color.Magenta;
            this.btnRemoveNetwork.Name = "btnRemoveNetwork";
            this.btnRemoveNetwork.Size = new Size(0x80, 0x16);
            this.btnRemoveNetwork.Text = "Unmap Network Path";
            this.btnRemoveNetwork.Click += new EventHandler(this.btnRemoveNetwork_Click);
            this.tree.AllowDrop = true;
            this.tree.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tree.HideSelection = false;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.imageList_0;
            this.tree.Location = new Point(0, 0x19);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new Size(0x109, 0x151);
            this.tree.TabIndex = 1;
            this.tree.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
            this.tree.DragDrop += new DragEventHandler(this.tree_DragDrop);
            this.tree.DragEnter += new DragEventHandler(this.tree_DragOver);
            this.tree.DragOver += new DragEventHandler(this.tree_DragOver);
            this.tree.KeyDown += new KeyEventHandler(this.tree_KeyDown);
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "FolderClosed.bmp");
            this.imageList_0.Images.SetKeyName(1, "FolderOpen.bmp");
            this.imageList_0.Images.SetKeyName(2, "assembly.bmp");
            this.imageList_0.Images.SetKeyName(3, "Editor.bmp");
            this.imageList_0.Images.SetKeyName(4, "Wizard.bmp");
            this.imageList_0.Images.SetKeyName(5, "precompiled.bmp");
            this.imageList_0.Images.SetKeyName(6, "NetworkDrive.bmp");
            this.pnlSearch.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlSearch.Controls.Add(this.grpSearch);
            this.pnlSearch.Controls.Add(this.btnShowDownloaded);
            this.pnlSearch.Location = new Point(0, 0x19);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new Size(0x109, 0x14e);
            this.pnlSearch.TabIndex = 2;
            this.pnlSearch.Enter += new EventHandler(this.pnlSearch_Enter);
            this.pnlSearch.Leave += new EventHandler(this.pnlSearch_Leave);
            this.grpSearch.Controls.Add(this.lblSearchCriteria);
            this.grpSearch.Controls.Add(this.btnSearchStrategies);
            this.grpSearch.Controls.Add(this.cmbSearchAuthor);
            this.grpSearch.Controls.Add(this.lblSearchAuthor);
            this.grpSearch.Controls.Add(this.lblSearchName);
            this.grpSearch.Controls.Add(this.txtSearchName);
            this.grpSearch.Controls.Add(this.txtSearchCode);
            this.grpSearch.Controls.Add(this.lblSearchCode);
            this.grpSearch.Dock = DockStyle.Top;
            this.grpSearch.Location = new Point(0, 0);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new Size(0x109, 0xac);
            this.grpSearch.TabIndex = 10;
            this.grpSearch.TabStop = false;
            this.lblSearchCriteria.Dock = DockStyle.Top;
            this.lblSearchCriteria.Location = new Point(3, 0x10);
            this.lblSearchCriteria.Name = "lblSearchCriteria";
            this.lblSearchCriteria.Size = new Size(0x103, 0x24);
            this.lblSearchCriteria.TabIndex = 1;
            this.lblSearchCriteria.Text = "Enter your search criteria in one or more of the fields below, then click the Search button.";
            this.btnSearchStrategies.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnSearchStrategies.Location = new Point(0xb8, 0x8f);
            this.btnSearchStrategies.Name = "btnSearchStrategies";
            this.btnSearchStrategies.Size = new Size(0x4b, 0x17);
            this.btnSearchStrategies.TabIndex = 8;
            this.btnSearchStrategies.Text = "Search";
            this.btnSearchStrategies.UseVisualStyleBackColor = true;
            this.btnSearchStrategies.Click += new EventHandler(this.btnSearchStrategies_Click);
            this.cmbSearchAuthor.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.cmbSearchAuthor.FormattingEnabled = true;
            this.cmbSearchAuthor.Location = new Point(0x5b, 0x69);
            this.cmbSearchAuthor.Name = "cmbSearchAuthor";
            this.cmbSearchAuthor.Size = new Size(0xa8, 0x15);
            this.cmbSearchAuthor.TabIndex = 7;
            this.lblSearchAuthor.AutoSize = true;
            this.lblSearchAuthor.Location = new Point(6, 0x6c);
            this.lblSearchAuthor.Name = "lblSearchAuthor";
            this.lblSearchAuthor.Size = new Size(0x29, 13);
            this.lblSearchAuthor.TabIndex = 6;
            this.lblSearchAuthor.Text = "Author:";
            this.lblSearchName.AutoSize = true;
            this.lblSearchName.Location = new Point(5, 0x37);
            this.lblSearchName.Name = "lblSearchName";
            this.lblSearchName.Size = new Size(80, 13);
            this.lblSearchName.TabIndex = 2;
            this.lblSearchName.Text = "Strategy Name:";
            this.txtSearchName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.txtSearchName.Location = new Point(0x5b, 0x34);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new Size(0xa8, 20);
            this.txtSearchName.TabIndex = 3;
            this.txtSearchCode.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.txtSearchCode.Location = new Point(0x5b, 0x4e);
            this.txtSearchCode.Name = "txtSearchCode";
            this.txtSearchCode.Size = new Size(0xa8, 20);
            this.txtSearchCode.TabIndex = 5;
            this.lblSearchCode.AutoSize = true;
            this.lblSearchCode.Location = new Point(5, 0x51);
            this.lblSearchCode.Name = "lblSearchCode";
            this.lblSearchCode.Size = new Size(0x4d, 13);
            this.lblSearchCode.TabIndex = 4;
            this.lblSearchCode.Text = "Strategy Code:";
            this.btnShowDownloaded.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnShowDownloaded.AutoSize = true;
            this.btnShowDownloaded.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.btnShowDownloaded.Location = new Point(0x66, 0xb2);
            this.btnShowDownloaded.Name = "btnShowDownloaded";
            this.btnShowDownloaded.Size = new Size(0x9d, 0x17);
            this.btnShowDownloaded.TabIndex = 9;
            this.btnShowDownloaded.Text = "Show Downloaded Strategies";
            this.btnShowDownloaded.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.btnShowDownloaded.UseVisualStyleBackColor = true;
            this.btnShowDownloaded.Click += new EventHandler(this.btnShowDownloaded_Click);
            this.splitDetails.Dock = DockStyle.Fill;
            this.splitDetails.Location = new Point(0, 0);
            this.splitDetails.Name = "splitDetails";
            this.splitDetails.Orientation = Orientation.Horizontal;
            this.splitDetails.Panel1.Controls.Add(this.lvStrategies);
            this.splitDetails.Panel2.Controls.Add(this.browser);
            this.splitDetails.Panel2.Controls.Add(this.pnlBottom);
            this.splitDetails.Size = new Size(0x1b6, 390);
            this.splitDetails.SplitterDistance = 0xc5;
            this.splitDetails.TabIndex = 0;
            this.lvStrategies.AllowDrop = true;
            this.lvStrategies.Columns.AddRange(new ColumnHeader[] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4 });
            this.lvStrategies.Dock = DockStyle.Fill;
            this.lvStrategies.FullRowSelect = true;
            this.lvStrategies.HideSelection = false;
            this.lvStrategies.Location = new Point(0, 0);
            this.lvStrategies.Name = "lvStrategies";
            this.lvStrategies.Size = new Size(0x1b6, 0xc5);
            this.lvStrategies.SmallImageList = this.imageList_0;
            this.lvStrategies.TabIndex = 0;
            this.lvStrategies.UseCompatibleStateImageBehavior = false;
            this.lvStrategies.View = View.Details;
            this.lvStrategies.ItemDrag += new ItemDragEventHandler(this.lvStrategies_ItemDrag);
            this.lvStrategies.SelectedIndexChanged += new EventHandler(this.lvStrategies_SelectedIndexChanged);
            this.lvStrategies.DragEnter += new DragEventHandler(this.lvStrategies_DragOver);
            this.lvStrategies.DragOver += new DragEventHandler(this.lvStrategies_DragOver);
            this.lvStrategies.DoubleClick += new EventHandler(this.btnOK_Click);
            this.lvStrategies.KeyDown += new KeyEventHandler(this.lvStrategies_KeyDown);
            this.columnHeader_0.Tag = "S";
            this.columnHeader_0.Text = "Strategy Name";
            this.columnHeader_0.Width = 160;
            this.columnHeader_1.Tag = "S";
            this.columnHeader_1.Text = "Author";
            this.columnHeader_1.Width = 80;
            this.columnHeader_2.Tag = "DT";
            this.columnHeader_2.Text = "Creation Date";
            this.columnHeader_2.Width = 100;
            this.columnHeader_3.Tag = "DT";
            this.columnHeader_3.Text = "Modified";
            this.columnHeader_3.Width = 100;
            this.columnHeader_4.Tag = "S";
            this.columnHeader_4.Text = "Account";
            this.browser.AllowWebBrowserDrop = false;
            this.browser.Dock = DockStyle.Fill;
            this.browser.Location = new Point(0, 0);
            this.browser.MinimumSize = new Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new Size(0x1b6, 0x9e);
            this.browser.TabIndex = 0;
            this.browser.Navigating += new WebBrowserNavigatingEventHandler(this.browser_Navigating);
            this.pnlBottom.Controls.Add(this.linkMoreInfo);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = DockStyle.Bottom;
            this.pnlBottom.Location = new Point(0, 0x9e);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new Size(0x1b6, 0x1f);
            this.pnlBottom.TabIndex = 1;
            this.linkMoreInfo.AutoSize = true;
            this.linkMoreInfo.Location = new Point(4, 4);
            this.linkMoreInfo.Name = "linkMoreInfo";
            this.linkMoreInfo.Size = new Size(0x40, 13);
            this.linkMoreInfo.TabIndex = 0;
            this.linkMoreInfo.TabStop = true;
            this.linkMoreInfo.Text = "More Info ...";
            this.linkMoreInfo.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkMoreInfo_LinkClicked);
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnOK.Location = new Point(0x117, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(360, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.openFileDialog_0.DefaultExt = "xml";
            this.openFileDialog_0.Filter = "XML Files|*.xml";
            this.openFileDialog_0.Multiselect = true;
            this.openFileDialog_0.Title = "Select XML Files to Import";
            this.folderBrowserDialog_0.Description = "Select Network Path";
            this.folderBrowserDialog_0.RootFolder = Environment.SpecialFolder.MyComputer;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x2c3, 0x1b5);
            base.Controls.Add(this.split);
            base.Controls.Add(this.toolbar);
            base.Controls.Add(this.status);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "StrategyExplorerForm";
            this.Text = "Strategy Explorer";
            base.FormClosed += new FormClosedEventHandler(this.StrategyExplorerForm_FormClosed);
            base.Load += new EventHandler(this.StrategyExplorerForm_Load);
            base.HelpRequested += new HelpEventHandler(this.StrategyExplorerForm_HelpRequested);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel1.PerformLayout();
            this.split.Panel2.ResumeLayout(false);
            this.split.EndInit();
            this.split.ResumeLayout(false);
            this.toolbarTree.ResumeLayout(false);
            this.toolbarTree.PerformLayout();
            this.toolbarNetFolder.ResumeLayout(false);
            this.toolbarNetFolder.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.splitDetails.Panel1.ResumeLayout(false);
            this.splitDetails.Panel2.ResumeLayout(false);
            this.splitDetails.EndInit();
            this.splitDetails.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void linkMoreInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count != 0)
            {
                ListViewItem item = this.lvStrategies.SelectedItems[0];
                Strategy tag = (Strategy) item.Tag;
                if (MainModule.Instance.NavigateToThirdPartySite(tag.URL))
                {
                    Process.Start(tag.URL);
                }
            }
        }

        private void lvStrategies_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lvStrategies_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (this.lvStrategies.SelectedItems.Count != 0)
            {
                this.lvStrategies.DoDragDrop("Strategies", DragDropEffects.Move);
            }
        }

        private void lvStrategies_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.btnDelete.PerformClick();
            }
        }

        private void lvStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnAccountNumber.Enabled = this.lvStrategies.SelectedItems.Count > 0;
            base.AcceptButton = this.btnOK;
            if (this.lvStrategies.SelectedItems.Count != 0)
            {
                ListViewItem item = this.lvStrategies.SelectedItems[0];
                Strategy tag = (Strategy) item.Tag;
                File.WriteAllText(MainModule.Instance.DataPath + @"\temp.html", tag.Description);
                this.browser.Navigate(MainModule.Instance.DataPath + @"\temp.html");
                this.linkMoreInfo.Visible = (tag.URL != null) && (tag.URL != "");
                this.bool_0 = true;
                this.btnDelete.Enabled = false;
                using (IEnumerator enumerator = this.lvStrategies.SelectedItems.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ListViewItem current = (ListViewItem) enumerator.Current;
                        Strategy strategy2 = current.Tag as Strategy;
                        if (strategy2.StrategyType != StrategyType.Compiled)
                        {
                            goto Label_0115;
                        }
                    }
                    return;
                Label_0115:
                    this.btnDelete.Enabled = true;
                }
            }
        }

        private TreeNode method_0(string string_0)
        {
            TreeNode node = this.tree.Nodes.Add(string_0);
            node.ImageIndex = 6;
            node.SelectedImageIndex = 6;
            foreach (string str in MainModule.Instance.Strategies.GetNetworkPathFolders(string_0))
            {
                TreeNode node2 = node.Nodes.Add(str);
                node2.ImageIndex = 0;
                node2.SelectedImageIndex = 1;
            }
            node.Expand();
            return node;
        }

        private void method_1()
        {
            foreach (string str in MainModule.Instance.Strategies.FolderNames)
            {
                bool flag = false;
                using (IEnumerator enumerator2 = this.tree.Nodes.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        TreeNode current = (TreeNode) enumerator2.Current;
                        if (current.Text == str)
                        {
                            goto Label_005D;
                        }
                    }
                    goto Label_0076;
                Label_005D:
                    flag = true;
                }
            Label_0076:
                if (!flag)
                {
                    TreeNode node2 = this.tree.Nodes.Add(str);
                    node2.ImageIndex = 0;
                    node2.SelectedImageIndex = 1;
                }
            }
        }

        private void method_2()
        {
            foreach (Strategy strategy in MainModule.Instance.Strategies.Strategies)
            {
                if (!this.cmbSearchAuthor.Items.Contains(strategy.Author))
                {
                    this.cmbSearchAuthor.Items.Add(strategy.Author);
                }
            }
        }

        private void method_3(Strategy strategy_0)
        {
            ListViewItem item = this.lvStrategies.Items.Add(strategy_0.Name);
            item.Tag = strategy_0;
            item.SubItems.Add(strategy_0.Author);
            item.SubItems.Add(strategy_0.CreationDate.ToShortDateString() + " " + strategy_0.CreationDate.ToShortTimeString());
            item.SubItems.Add(strategy_0.LastModified.ToShortDateString() + " " + strategy_0.LastModified.ToShortTimeString());
            switch (strategy_0.StrategyType)
            {
                case StrategyType.Rules:
                    item.ImageIndex = 4;
                    break;

                case StrategyType.Compiled:
                    item.ImageIndex = 5;
                    break;

                default:
                    item.ImageIndex = 3;
                    break;
            }
            item.SubItems.Add(strategy_0.AccountNumber);
            if (!string.IsNullOrEmpty(strategy_0.Origin))
            {
                item.ForeColor = Color.Blue;
            }
        }

        private void pnlSearch_Enter(object sender, EventArgs e)
        {
            base.AcceptButton = this.btnSearchStrategies;
        }

        private void pnlSearch_Leave(object sender, EventArgs e)
        {
            base.AcceptButton = this.btnOK;
        }

        private void StrategyExplorerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((this.tree.SelectedNode != null) && (this.lvStrategies.SelectedItems.Count > 0))
            {
                string str = this.tree.SelectedNode.Text + "|" + this.lvStrategies.SelectedItems[0].Text;
                base.Tag = str;
            }
            MainModule.Instance.Settings.Set(this, "StrategyExplorerForm");
        }

        private void StrategyExplorerForm_HelpRequested(object sender, HelpEventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("strategy_explorer.htm");
        }

        private void StrategyExplorerForm_Load(object sender, EventArgs e)
        {
            MainModule.Instance.Settings.Get(this, "StrategyExplorerForm");
            this.method_1();
            foreach (string str4 in MainModule.Instance.Strategies.LibraryNames)
            {
                TreeNode node2 = this.tree.Nodes.Add(str4);
                Bitmap customGlyph = MainModule.Instance.Strategies.GetCustomGlyph(str4);
                if (customGlyph == null)
                {
                    node2.ImageIndex = 2;
                    node2.SelectedImageIndex = 2;
                }
                else
                {
                    this.imageList_0.Images.Add(customGlyph);
                    int num = this.imageList_0.Images.Count - 1;
                    node2.ImageIndex = num;
                    node2.SelectedImageIndex = num;
                }
            }
            this.lblStrategies.Text = MainModule.Instance.Strategies.Strategies.Count + " Strategies";
            string tag = (string) base.Tag;
            if ((tag != null) && (tag != ""))
            {
                string[] strArray = tag.Split(new char[] { '|' });
                string str = strArray[0];
                string str2 = strArray[1];
                foreach (TreeNode node in this.tree.Nodes)
                {
                    if (node.Text == str)
                    {
                        this.tree.SelectedNode = node;
                        foreach (ListViewItem item in this.lvStrategies.Items)
                        {
                            item.Selected = item.Text == str2;
                        }
                    }
                }
            }
            this.method_2();
            foreach (string str3 in MainModule.Instance.StrategyNetworkPaths)
            {
                this.method_0(str3);
            }
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                this.btnDeleteFolder.Enabled = false;
            }
            else
            {
                this.btnDeleteFolder.Enabled = e.Node.ImageIndex == 0;
                this.btnRemoveNetwork.Enabled = e.Node.ImageIndex == 6;
                this.lvStrategies.BeginUpdate();
                try
                {
                    this.lvStrategies.Items.Clear();
                    string networkPath = "";
                    if (e.Node.Parent != null)
                    {
                        networkPath = e.Node.Parent.Text;
                    }
                    foreach (Strategy strategy in MainModule.Instance.Strategies.StrategiesInFolder(e.Node.Text, networkPath))
                    {
                        this.method_3(strategy);
                    }
                    if (this.lvStrategies.Items.Count > 0)
                    {
                        this.lvStrategies.Items[0].Selected = true;
                    }
                }
                finally
                {
                    this.lvStrategies.EndUpdate();
                }
            }
        }

        private void tree_DragDrop(object sender, DragEventArgs e)
        {
            Point point = this.tree.PointToClient(new Point(e.X, e.Y));
            TreeNode nodeAt = this.tree.GetNodeAt(point.X, point.Y);
            if ((nodeAt != null) && (nodeAt.ImageIndex == 0))
            {
                foreach (ListViewItem item in this.lvStrategies.SelectedItems)
                {
                    Strategy tag = (Strategy) item.Tag;
                    MainModule.Instance.Strategies.DeleteStrategy(tag);
                    string networkPath = "";
                    if (nodeAt.Parent != null)
                    {
                        networkPath = nodeAt.Parent.Text;
                    }
                    MainModule.Instance.Strategies.SaveStrategy(tag, nodeAt.Text, networkPath);
                }
                this.tree_AfterSelect(this.tree, new TreeViewEventArgs(this.tree.SelectedNode));
            }
        }

        private void tree_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void tree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.btnDeleteFolder.PerformClick();
            }
        }

        public bool MultiSelect
        {
            get
            {
                return this.lvStrategies.MultiSelect;
            }
            set
            {
                this.lvStrategies.MultiSelect = value;
            }
        }

        public IList<Strategy> StrategiesSelected
        {
            get
            {
                this.list_0.Clear();
                foreach (ListViewItem item in this.lvStrategies.Items)
                {
                    if (item.Selected)
                    {
                        Strategy tag = (Strategy) item.Tag;
                        this.list_0.Add(tag);
                    }
                }
                return this.list_0.AsReadOnly();
            }
        }

        public bool ToolBarVisible
        {
            get
            {
                return this.toolbar.Visible;
            }
            set
            {
                this.toolbar.Visible = value;
            }
        }
    }
}

