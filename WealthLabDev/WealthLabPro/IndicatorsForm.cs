namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class IndicatorsForm : Form
    {
        private ToolStripButton btnTop;
        private IContainer icontainer_0;
        public static IndicatorsForm Instance;
        private ToolStripLabel lblDrag;
        private SplitContainer splitIndicators;
        private StatusStrip status;
        private ToolStripStatusLabel stlblIndicators;
        private ToolStripStatusLabel stLink;
        private ToolStrip toolbar;
        private IndicatorTreeView treeIndicators;
        private TextBox txtDescription;

        public IndicatorsForm()
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

        private void IndicatorsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainModule.Instance.Settings.Set(this, "IndicatorsForm");
            Instance = null;
        }

        private void IndicatorsForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            MainModule.Instance.Settings.Get(this, "IndicatorsForm");
            this.stlblIndicators.Text = this.treeIndicators.IndicatorCount + " Indicators";
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(IndicatorsForm));
            this.status = new StatusStrip();
            this.stlblIndicators = new ToolStripStatusLabel();
            this.stLink = new ToolStripStatusLabel();
            this.splitIndicators = new SplitContainer();
            this.treeIndicators = new IndicatorTreeView();
            this.toolbar = new ToolStrip();
            this.btnTop = new ToolStripButton();
            this.lblDrag = new ToolStripLabel();
            this.txtDescription = new TextBox();
            this.status.SuspendLayout();
            this.splitIndicators.Panel1.SuspendLayout();
            this.splitIndicators.Panel2.SuspendLayout();
            this.splitIndicators.SuspendLayout();
            this.toolbar.SuspendLayout();
            base.SuspendLayout();
            this.status.BackColor = Color.AliceBlue;
            this.status.Items.AddRange(new ToolStripItem[] { this.stlblIndicators, this.stLink });
            this.status.Location = new Point(0, 0x1c4);
            this.status.Name = "status";
            this.status.Size = new Size(0xe8, 0x16);
            this.status.TabIndex = 0;
            this.stlblIndicators.Name = "stlblIndicators";
            this.stlblIndicators.Size = new Size(0x47, 0x11);
            this.stlblIndicators.Text = "No Indicators";
            this.stLink.IsLink = true;
            this.stLink.Name = "stLink";
            this.stLink.Size = new Size(0x42, 0x11);
            this.stLink.Text = "More Info...";
            this.stLink.Visible = false;
            this.stLink.Click += new EventHandler(this.stLink_Click);
            this.splitIndicators.Dock = DockStyle.Fill;
            this.splitIndicators.Location = new Point(0, 0);
            this.splitIndicators.Name = "splitIndicators";
            this.splitIndicators.Orientation = Orientation.Horizontal;
            this.splitIndicators.Panel1.Controls.Add(this.treeIndicators);
            this.splitIndicators.Panel1.Controls.Add(this.toolbar);
            this.splitIndicators.Panel2.BackColor = Color.AliceBlue;
            this.splitIndicators.Panel2.Controls.Add(this.txtDescription);
            this.splitIndicators.Size = new Size(0xe8, 0x1c4);
            this.splitIndicators.SplitterDistance = 0x16c;
            this.splitIndicators.TabIndex = 3;
            this.treeIndicators.AllowDrop = true;
            this.treeIndicators.BackColor = Color.AliceBlue;
            this.treeIndicators.Dock = DockStyle.Fill;
            this.treeIndicators.FullRowSelect = true;
            this.treeIndicators.HideSelection = false;
            this.treeIndicators.Location = new Point(0, 0x19);
            this.treeIndicators.Name = "treeIndicators";
            this.treeIndicators.Size = new Size(0xe8, 0x153);
            this.treeIndicators.StandardNodeName = "TASC Magazine Indicators";
            this.treeIndicators.TabIndex = 4;
            this.treeIndicators.DragOver += new DragEventHandler(this.treeIndicators_DragEnter);
            this.treeIndicators.DoubleClick += new EventHandler(this.treeIndicators_DoubleClick);
            this.treeIndicators.AfterSelect += new TreeViewEventHandler(this.treeIndicators_AfterSelect);
            this.treeIndicators.DragEnter += new DragEventHandler(this.treeIndicators_DragEnter);
            this.treeIndicators.ItemDrag += new ItemDragEventHandler(this.treeIndicators_ItemDrag);
            this.treeIndicators.MouseDown += new MouseEventHandler(this.treeIndicators_MouseDown);
            this.toolbar.BackColor = Color.AliceBlue;
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.btnTop, this.lblDrag });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0xe8, 0x19);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            this.btnTop.Alignment = ToolStripItemAlignment.Right;
            this.btnTop.Image = (Image) manager.GetObject("btnTop.Image");
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
            this.txtDescription.BackColor = Color.AliceBlue;
            this.txtDescription.Dock = DockStyle.Fill;
            this.txtDescription.Location = new Point(0, 0);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(0xe8, 0x54);
            this.txtDescription.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.AliceBlue;
            base.ClientSize = new Size(0xe8, 0x1da);
            base.Controls.Add(this.splitIndicators);
            base.Controls.Add(this.status);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.Name = "IndicatorsForm";
            base.ShowInTaskbar = false;
            this.Text = "Technical Indicators";
            base.FormClosed += new FormClosedEventHandler(this.IndicatorsForm_FormClosed);
            base.Load += new EventHandler(this.IndicatorsForm_Load);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.splitIndicators.Panel1.ResumeLayout(false);
            this.splitIndicators.Panel1.PerformLayout();
            this.splitIndicators.Panel2.ResumeLayout(false);
            this.splitIndicators.Panel2.PerformLayout();
            this.splitIndicators.ResumeLayout(false);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
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

        private void treeIndicators_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((e.Node != null) && (e.Node.Level == 1))
            {
                IndicatorHelper tag = (IndicatorHelper) e.Node.Tag;
                this.txtDescription.Text = tag.Description;
                if (tag.URL != "")
                {
                    this.stLink.Tag = tag.URL;
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

        private void treeIndicators_DoubleClick(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.treeIndicators.SelectedNode;
            if ((selectedNode != null) && (selectedNode.Level != 0))
            {
                IndicatorHelper tag = (IndicatorHelper) selectedNode.Tag;
                MainForm.LastActivated.PlotIndicator(tag);
            }
        }

        private void treeIndicators_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void treeIndicators_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode selectedNode = this.treeIndicators.SelectedNode;
            if ((selectedNode != null) && (selectedNode.Level == 1))
            {
                DraggedIndicatorHelper data = new DraggedIndicatorHelper(selectedNode.Tag as IndicatorHelper);
                this.treeIndicators.DoDragDrop(data, DragDropEffects.Copy);
            }
        }

        private void treeIndicators_MouseDown(object sender, MouseEventArgs e)
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

