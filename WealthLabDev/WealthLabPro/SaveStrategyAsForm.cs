namespace WealthLabPro
{
    using Fidelity.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;

    public class SaveStrategyAsForm : Form
    {
        private Button btnCancel;
        private Button btnNewFolder;
        private Button btnOK;
        private GroupBox grpFolder;
        private GroupBox grpName;
        private IContainer components;
        private ImageList imageList_0;
        private const int int_0 = 2;
        private static string string_0 = "";
        private TreeView tree;
        private TextBox txtStrategyName;

        public SaveStrategyAsForm()
        {
            this.InitializeComponent();
            if (string_0 == "")
            {
                foreach (char ch in Path.GetInvalidFileNameChars())
                {
                    string_0 = string_0 + ch;
                }
            }
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            string folder = InputBox.Show("New Folder", "Folder Name:");
            if (folder != "")
            {
                string networkPath = "";
                if (this.tree.SelectedNode != null)
                {
                    if (this.tree.SelectedNode.ImageIndex == 2)
                    {
                        networkPath = this.tree.SelectedNode.Text;
                    }
                    else if (this.tree.SelectedNode.Parent != null)
                    {
                        networkPath = this.tree.SelectedNode.Parent.Text;
                    }
                }
                if ((networkPath != null) && (networkPath != ""))
                {
                    if (MainModule.Instance.Strategies.AddFolderToNetworkPath(networkPath, folder))
                    {
                        TreeNode selectedNode = this.tree.SelectedNode;
                        if (selectedNode.Parent != null)
                        {
                            selectedNode = selectedNode.Parent;
                        }
                        selectedNode.Nodes.Add(folder);
                        selectedNode.ImageIndex = 0;
                        selectedNode.SelectedImageIndex = 1;
                    }
                }
                else if (MainModule.Instance.Strategies.AddFolder(folder, true))
                {
                    TreeNode node = this.tree.Nodes.Add(folder);
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 1;
                    this.tree.SelectedNode = node;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtStrategyName.Text == "")
            {
                MessageBox.Show("Please enter a Strategy Name");
                this.txtStrategyName.Focus();
            }
            else if ((this.tree.SelectedNode != null) && (this.tree.SelectedNode.ImageIndex != 2))
            {
                string networkPath = "";
                if (this.tree.SelectedNode.Parent != null)
                {
                    networkPath = this.tree.SelectedNode.Parent.Text;
                }
                Strategy strategy = MainModule.Instance.Strategies.Lookup(this.txtStrategyName.Text, this.tree.SelectedNode.Text, networkPath);
                if (strategy != null)
                {
                    if (MessageBox.Show("A Strategy with this name already exists in the Folder, overwrite it?", "Save Strategy", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        this.txtStrategyName.SelectAll();
                        this.txtStrategyName.Focus();
                        return;
                    }
                    MainModule.Instance.Strategies.DeleteStrategy(strategy);
                }
                base.DialogResult = DialogResult.OK;
            }
            else if (this.tree.Nodes.Count == 0)
            {
                MessageBox.Show("You must first create a Folder where the Strategy will be saved to");
                this.btnNewFolder.PerformClick();
            }
            else
            {
                MessageBox.Show("Please select a Folder where the Strategy should be saved to");
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

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(SaveStrategyAsForm));
            this.grpName = new GroupBox();
            this.txtStrategyName = new TextBox();
            this.grpFolder = new GroupBox();
            this.btnNewFolder = new Button();
            this.tree = new TreeView();
            this.imageList_0 = new ImageList(this.components);
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.grpName.SuspendLayout();
            this.grpFolder.SuspendLayout();
            base.SuspendLayout();
            this.grpName.Controls.Add(this.txtStrategyName);
            this.grpName.Location = new Point(13, 13);
            this.grpName.Name = "grpName";
            this.grpName.Size = new Size(0xeb, 0x36);
            this.grpName.TabIndex = 0;
            this.grpName.TabStop = false;
            this.grpName.Text = "Strategy Name";
            this.txtStrategyName.Location = new Point(7, 20);
            this.txtStrategyName.Name = "txtStrategyName";
            this.txtStrategyName.Size = new Size(0xde, 20);
            this.txtStrategyName.TabIndex = 0;
            this.txtStrategyName.KeyPress += new KeyPressEventHandler(this.txtStrategyName_KeyPress);
            this.grpFolder.Controls.Add(this.btnNewFolder);
            this.grpFolder.Controls.Add(this.tree);
            this.grpFolder.Location = new Point(13, 0x4a);
            this.grpFolder.Name = "grpFolder";
            this.grpFolder.Size = new Size(0xeb, 0xe2);
            this.grpFolder.TabIndex = 1;
            this.grpFolder.TabStop = false;
            this.grpFolder.Text = "Select Folder for Strategy";
            this.btnNewFolder.Location = new Point(0x5e, 0xbc);
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.Size = new Size(0x86, 0x17);
            this.btnNewFolder.TabIndex = 1;
            this.btnNewFolder.Text = "Create a new Folder ...";
            this.btnNewFolder.UseVisualStyleBackColor = true;
            this.btnNewFolder.Click += new EventHandler(this.btnNewFolder_Click);
            this.tree.HideSelection = false;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.imageList_0;
            this.tree.Location = new Point(7, 20);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new Size(0xde, 0xa1);
            this.tree.TabIndex = 0;
            this.imageList_0.ImageStream = (ImageListStreamer) resources.GetObject("images.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "FolderClosed.bmp");
            this.imageList_0.Images.SetKeyName(1, "FolderOpen.bmp");
            this.imageList_0.Images.SetKeyName(2, "NetworkDrive.bmp");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xac, 0x132);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Location = new Point(0x5b, 0x132);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(260, 0x152);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.grpFolder);
            base.Controls.Add(this.grpName);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SaveStrategyAsForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Save Strategy As";
            base.Load += new EventHandler(this.SaveStrategyAsForm_Load);
            this.grpName.ResumeLayout(false);
            this.grpName.PerformLayout();
            this.grpFolder.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void SaveStrategyAsForm_Load(object sender, EventArgs e)
        {
            foreach (string str3 in MainModule.Instance.Strategies.FolderNames)
            {
                TreeNode node3 = this.tree.Nodes.Add(str3);
                node3.ImageIndex = 0;
                node3.SelectedImageIndex = 1;
            }
            foreach (string str2 in MainModule.Instance.StrategyNetworkPaths)
            {
                TreeNode node = this.tree.Nodes.Add(str2);
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
                foreach (string str in MainModule.Instance.Strategies.GetNetworkPathFolders(str2))
                {
                    TreeNode node2 = node.Nodes.Add(str);
                    node2.ImageIndex = 0;
                    node2.SelectedImageIndex = 1;
                }
            }
        }

        private void txtStrategyName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar != '\x0016') && (e.KeyChar != '\x0003')) && (e.KeyChar != '\b'))
            {
                string str = e.KeyChar.ToString();
                if (string_0.Contains(str))
                {
                    e.KeyChar = '\0';
                }
            }
        }

        public string FolderName
        {
            get
            {
                return this.tree.SelectedNode.Text;
            }
        }

        public string NetworkPath
        {
            get
            {
                if (this.tree.SelectedNode.Parent != null)
                {
                    return this.tree.SelectedNode.Parent.Text;
                }
                return "";
            }
        }

        public string StrategyName
        {
            get
            {
                return this.txtStrategyName.Text;
            }
        }
    }
}

