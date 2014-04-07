namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class FundamentalsForm : Form
    {
        private ToolStripButton btnTop;
        private IContainer icontainer_0;
        public static FundamentalsForm Instance;
        private ToolStripLabel lblDrag;
        private SplitContainer splitIndicators;
        private StatusStrip status;
        private ToolStripStatusLabel stlblItems;
        private ToolStripStatusLabel stLink;
        private ToolStrip toolbar;
        private FundamentalTreeView treeItems;
        private TextBox txtDescription;

        public FundamentalsForm()
        {
            this.InitializeComponent();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            this.btnTop.Checked = !this.btnTop.Checked;
            base.TopMost = this.btnTop.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FundamentalsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainModule.Instance.Settings.Set(this, "FundamentalsForm");
            Instance = null;
        }

        private void FundamentalsForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            MainModule.Instance.Settings.Get(this, "FundamentalsForm");
            this.treeItems.DataHost = MainModule.Instance.DataSources;
            this.treeItems.LoadNodes();
            this.stlblItems.Text = this.treeItems.FundamentalsCount + " Fundamental Items";
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FundamentalsForm));
            this.splitIndicators = new SplitContainer();
            this.treeItems = new FundamentalTreeView();
            this.toolbar = new ToolStrip();
            this.btnTop = new ToolStripButton();
            this.lblDrag = new ToolStripLabel();
            this.txtDescription = new TextBox();
            this.status = new StatusStrip();
            this.stlblItems = new ToolStripStatusLabel();
            this.stLink = new ToolStripStatusLabel();
            this.splitIndicators.Panel1.SuspendLayout();
            this.splitIndicators.Panel2.SuspendLayout();
            this.splitIndicators.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.status.SuspendLayout();
            base.SuspendLayout();
            this.splitIndicators.BackColor = Color.FromArgb(0xff, 0xf1, 0xff);
            this.splitIndicators.Dock = DockStyle.Fill;
            this.splitIndicators.Location = new Point(0, 0);
            this.splitIndicators.Name = "splitIndicators";
            this.splitIndicators.Orientation = Orientation.Horizontal;
            this.splitIndicators.Panel1.BackColor = Color.FromArgb(0xff, 0xf1, 0xff);
            this.splitIndicators.Panel1.Controls.Add(this.treeItems);
            this.splitIndicators.Panel1.Controls.Add(this.toolbar);
            this.splitIndicators.Panel2.BackColor = Color.AliceBlue;
            this.splitIndicators.Panel2.Controls.Add(this.txtDescription);
            this.splitIndicators.Size = new Size(0xe8, 0x1c4);
            this.splitIndicators.SplitterDistance = 0x16c;
            this.splitIndicators.TabIndex = 6;
            this.treeItems.AllowDrop = true;
            this.treeItems.BackColor = Color.FromArgb(0xff, 0xf1, 0xff);
            this.treeItems.Dock = DockStyle.Fill;
            this.treeItems.FullRowSelect = true;
            this.treeItems.HideSelection = false;
            this.treeItems.ignoreDragandDrop = false;
            this.treeItems.ImageIndex = 0;
            this.treeItems.Location = new Point(0, 0x19);
            this.treeItems.Name = "treeItems";
            this.treeItems.SelectedImageIndex = 0;
            this.treeItems.Size = new Size(0xe8, 0x153);
            this.treeItems.StandardNodeName = "Fidelity Economic Indicators Data";
            this.treeItems.TabIndex = 4;
            this.treeItems.DoubleClick += new EventHandler(this.treeItems_DoubleClick);
            this.treeItems.AfterSelect += new TreeViewEventHandler(this.treeItems_AfterSelect);
            this.treeItems.MouseDown += new MouseEventHandler(this.treeItems_MouseDown);
            this.treeItems.DragEnter += new DragEventHandler(this.treeItems_DragOver);
            this.treeItems.ItemDrag += new ItemDragEventHandler(this.treeItems_ItemDrag);
            this.treeItems.DragOver += new DragEventHandler(this.treeItems_DragOver);
            this.toolbar.BackColor = Color.FromArgb(0xff, 0xf1, 0xff);
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.btnTop, this.lblDrag });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0xe8, 0x19);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            this.btnTop.Alignment = ToolStripItemAlignment.Right;
            this.btnTop.ForeColor = SystemColors.ControlText;
            this.btnTop.Image = (Image) resources.GetObject("btnTop.Image");
            this.btnTop.ImageTransparentColor = Color.Magenta;
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new Size(0x44, 0x16);
            this.btnTop.Text = "Topmost";
            this.btnTop.ToolTipText = "Keep this Window on top";
            this.btnTop.Click += new EventHandler(this.btnTop_Click);
            this.lblDrag.ForeColor = SystemColors.Highlight;
            this.lblDrag.Name = "lblDrag";
            this.lblDrag.Size = new Size(130, 0x16);
            this.lblDrag.Text = "Drag && Drop onto a Chart";
            this.txtDescription.BackColor = Color.FromArgb(0xff, 0xf1, 0xff);
            this.txtDescription.Dock = DockStyle.Fill;
            this.txtDescription.Location = new Point(0, 0);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(0xe8, 0x54);
            this.txtDescription.TabIndex = 0;
            this.status.BackColor = Color.FromArgb(0xff, 0xf1, 0xff);
            this.status.Items.AddRange(new ToolStripItem[] { this.stlblItems, this.stLink });
            this.status.Location = new Point(0, 0x1c4);
            this.status.Name = "status";
            this.status.Size = new Size(0xe8, 0x16);
            this.status.TabIndex = 4;
            this.stlblItems.ForeColor = SystemColors.ControlText;
            this.stlblItems.Name = "stlblItems";
            this.stlblItems.Size = new Size(90, 0x11);
            this.stlblItems.Text = "No Fundamentals";
            this.stLink.IsLink = true;
            this.stLink.Name = "stLink";
            this.stLink.Size = new Size(0x42, 0x11);
            this.stLink.Text = "More Info...";
            this.stLink.Visible = false;
            this.stLink.Click += new EventHandler(this.stLink_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0xe8, 0x1da);
            base.Controls.Add(this.splitIndicators);
            base.Controls.Add(this.status);
            this.ForeColor = Color.FromArgb(0xff, 0xe3, 0xff);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            base.Name = "FundamentalsForm";
            base.ShowInTaskbar = false;
            this.Text = "Fundamental Data";
            base.Load += new EventHandler(this.FundamentalsForm_Load);
            base.FormClosed += new FormClosedEventHandler(this.FundamentalsForm_FormClosed);
            this.splitIndicators.Panel1.ResumeLayout(false);
            this.splitIndicators.Panel1.PerformLayout();
            this.splitIndicators.Panel2.ResumeLayout(false);
            this.splitIndicators.Panel2.PerformLayout();
            this.splitIndicators.ResumeLayout(false);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private Rectangle method_0(TreeNode treeNode_0)
        {
            TreeView treeView = treeNode_0.TreeView;
            Rectangle bounds = treeNode_0.Bounds;
            if (treeNode_0.Tag != null)
            {
                Graphics graphics = treeView.CreateGraphics();
                int num = ((int) graphics.MeasureString(treeNode_0.Tag.ToString(), treeView.Font).Width) + 6;
                bounds.Offset(num / 2, 0);
                bounds = Rectangle.Inflate(bounds, num / 2, 0);
                graphics.Dispose();
            }
            return bounds;
        }

        private void stLink_Click(object sender, EventArgs e)
        {
            string tag = (string) this.stLink.Tag;
            if (MainModule.Instance.NavigateToThirdPartySite(tag))
            {
                Process.Start(tag);
            }
        }

        private void treeItems_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((e.Node != null) && (e.Node.Level == 1))
            {
                FundamentalDataProvider tag = (FundamentalDataProvider) e.Node.Tag;
                this.txtDescription.Text = tag.ItemDescription(e.Node.Text);
                string str = tag.ItemURL(e.Node.Text);
                if (str != "")
                {
                    this.stLink.Tag = str;
                    this.stLink.Visible = true;
                }
                else
                {
                    this.stLink.Visible = false;
                }
            }
            else
            {
                this.txtDescription.Text = "";
                this.stLink.Visible = false;
            }
        }

        private void treeItems_DoubleClick(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.treeItems.SelectedNode;
            if ((selectedNode != null) && (selectedNode.Level != 0))
            {
                DraggedFundamentalItem item = new DraggedFundamentalItem(selectedNode.Tag as FundamentalDataProvider, selectedNode.Text);
                MainForm.LastActivated.method_42(item);
            }
        }

        private void treeItems_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void treeItems_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode selectedNode = this.treeItems.SelectedNode;
            if ((selectedNode != null) && (selectedNode.Level == 1))
            {
                DraggedFundamentalItem data = new DraggedFundamentalItem(selectedNode.Tag as FundamentalDataProvider, selectedNode.Text);
                this.treeItems.DoDragDrop(data, DragDropEffects.Copy);
            }
        }

        private void treeItems_MouseDown(object sender, MouseEventArgs e)
        {
            TreeView view = sender as TreeView;
            TreeNode nodeAt = view.GetNodeAt(e.X, e.Y);
            if ((nodeAt != null) && this.method_0(nodeAt).Contains(e.X, e.Y))
            {
                view.SelectedNode = nodeAt;
            }
        }
    }
}

