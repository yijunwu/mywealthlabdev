namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class OrgChartEditor : Form
    {
        private Button buttonAdd;
        private ButtonPen buttonPen;
        private Button buttonRemove;
        private CheckBox cbClipText;
        private ComboBox cboxLineStyle;
        private ComboBox cBoxTextAlignment;
        private CheckBox cbSizeAutomatic;
        private CheckBox cbTextVisible;
        private IContainer components;
        private GroupBox groupBoxSpacing;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private NumericUpDown numericUDHeight;
        private NumericUpDown numericUDHorizontal;
        private NumericUpDown numericUDVertical;
        private NumericUpDown numericUDWidth;
        private OrgSeries org;
        private System.Windows.Forms.Panel panel1;
        private bool setting;
        private CustomShapeEditor shapeForm;
        private SplitContainer splitContainer1;
        private TabControl tabControl1;
        private TabControl tabControlItem;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPageSize;
        private TabPage tabPageText;
        private TextBox textBox1;
        private TreeView treeViewNodes;

        public OrgChartEditor()
        {
            this.InitializeComponent();
        }

        public OrgChartEditor(Series s) : this()
        {
            this.org = (OrgSeries) s;
            this.setting = true;
            try
            {
                this.numericUDHorizontal.Value = this.org.ItemSpacing.Horizontal;
                this.numericUDVertical.Value = this.org.ItemSpacing.Vertical;
                this.buttonPen.Pen = this.org.Pen;
                this.cboxLineStyle.SelectedIndex = (int) this.org.LineStyle;
                this.buttonRemove.Enabled = this.org.Count > 0;
            }
            finally
            {
                this.setting = false;
            }
        }

        private void AddNodes()
        {
            TreeNode node2 = null;
            OrgItem i = null;
            for (int j = 0; j < this.org.Count; j++)
            {
                i = this.org.Items[j];
                OrgNode node = new OrgNode(i);
                if (i.Superior != -1)
                {
                    OrgItem item = this.org.Items[i.Superior];
                    node2 = this.FindNode(this.treeViewNodes.Nodes, item);
                }
                if (node2 == null)
                {
                    this.treeViewNodes.Nodes.Add(node);
                }
                else
                {
                    node2.Nodes.Add(node);
                }
            }
        }

        private void AddTWNodes(OrgSeries ser, int current)
        {
            this.treeViewNodes.SuspendLayout();
            try
            {
                this.treeViewNodes.Nodes.Clear();
                this.AddNodes();
                if ((current != -1) && (this.org.Count > current))
                {
                    this.tabControlItem.Visible = true;
                    this.RefreshNode(current);
                }
                else
                {
                    this.tabControlItem.Visible = false;
                }
            }
            finally
            {
                this.treeViewNodes.ExpandAll();
                this.treeViewNodes.ResumeLayout();
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (this.org.Count == 0)
            {
                this.AddTWNodes(this.org, this.org.Add("#" + this.org.Count.ToString(), -1));
            }
            else
            {
                this.AddTWNodes(this.org, this.org.Add("#" + this.org.Count.ToString(), this.CurrentItem().Index));
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int index = this.CurrentIndex();
            if (index != -1)
            {
                this.org.Delete(index);
                this.AddTWNodes(this.org, index - 1);
            }
            this.buttonRemove.Enabled = this.org.Count > 0;
        }

        private void cbClipText_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.CurrentItem() != null))
            {
                this.CurrentItem().Format.ClipText = this.cbClipText.Checked;
                this.org.Repaint();
            }
        }

        private void cboxLineStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.org.LineStyle = (OrgLineStyle) this.cboxLineStyle.SelectedIndex;
            }
        }

        private void cBoxTextAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.CurrentItem() != null))
            {
                this.CurrentItem().Format.TextAlign = (StringAlignment) this.cBoxTextAlignment.SelectedIndex;
                this.org.Repaint();
            }
        }

        private void cbSizeAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.CurrentItem() != null))
            {
                this.CurrentItem().Format.AutoSize = this.cbSizeAutomatic.Checked;
                this.numericUDHeight.Enabled = !this.cbSizeAutomatic.Checked;
                this.numericUDWidth.Enabled = !this.cbSizeAutomatic.Checked;
                this.org.Repaint();
            }
        }

        private void cbTextVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.CurrentItem() != null))
            {
                this.CurrentItem().Format.Visible = this.cbTextVisible.Checked;
                this.org.Repaint();
            }
        }

        private int CurrentIndex()
        {
            OrgNode node = (this.treeViewNodes.SelectedNode != null) ? (this.treeViewNodes.SelectedNode as OrgNode) : null;
            if (node != null)
            {
                return (node.Tag as OrgItem).Index;
            }
            return -1;
        }

        private OrgItem CurrentItem()
        {
            OrgNode node = (this.treeViewNodes.SelectedNode != null) ? (this.treeViewNodes.SelectedNode as OrgNode) : null;
            if (node != null)
            {
                return (node.Tag as OrgItem);
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private TreeNode FindNode(TreeNodeCollection nodes, OrgItem Item)
        {
            foreach (TreeNode node in nodes)
            {
                if ((node.Tag as OrgItem).Equals(Item))
                {
                    return node;
                }
                TreeNode node2 = this.FindNode(node.Nodes, Item);
                if (node2 != null)
                {
                    return node2;
                }
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.cboxLineStyle = new ComboBox();
            this.label3 = new Label();
            this.buttonPen = new ButtonPen();
            this.groupBoxSpacing = new GroupBox();
            this.numericUDVertical = new NumericUpDown();
            this.numericUDHorizontal = new NumericUpDown();
            this.label2 = new Label();
            this.label1 = new Label();
            this.tabPage2 = new TabPage();
            this.splitContainer1 = new SplitContainer();
            this.treeViewNodes = new TreeView();
            this.tabControlItem = new TabControl();
            this.tabPageText = new TabPage();
            this.cbClipText = new CheckBox();
            this.cBoxTextAlignment = new ComboBox();
            this.label5 = new Label();
            this.cbTextVisible = new CheckBox();
            this.textBox1 = new TextBox();
            this.label4 = new Label();
            this.tabPageSize = new TabPage();
            this.numericUDHeight = new NumericUpDown();
            this.label7 = new Label();
            this.numericUDWidth = new NumericUpDown();
            this.label6 = new Label();
            this.cbSizeAutomatic = new CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonRemove = new Button();
            this.buttonAdd = new Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxSpacing.SuspendLayout();
            this.numericUDVertical.BeginInit();
            this.numericUDHorizontal.BeginInit();
            this.tabPage2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlItem.SuspendLayout();
            this.tabPageText.SuspendLayout();
            this.tabPageSize.SuspendLayout();
            this.numericUDHeight.BeginInit();
            this.numericUDWidth.BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x124, 0x10a);
            this.tabControl1.TabIndex = 0;
            this.tabPage1.Controls.Add(this.cboxLineStyle);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.buttonPen);
            this.tabPage1.Controls.Add(this.groupBoxSpacing);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x11c, 240);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.cboxLineStyle.FlatStyle = FlatStyle.Flat;
            this.cboxLineStyle.FormattingEnabled = true;
            this.cboxLineStyle.Items.AddRange(new object[] { "Squared", "Diagonal" });
            this.cboxLineStyle.Location = new Point(0x56, 0x7f);
            this.cboxLineStyle.Name = "cboxLineStyle";
            this.cboxLineStyle.Size = new Size(80, 0x15);
            this.cboxLineStyle.TabIndex = 3;
            this.cboxLineStyle.SelectedIndexChanged += new EventHandler(this.cboxLineStyle_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(8, 130);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x33, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Line style";
            this.buttonPen.FlatStyle = FlatStyle.Flat;
            this.buttonPen.Location = new Point(8, 0x5e);
            this.buttonPen.Name = "buttonPen";
            this.buttonPen.Size = new Size(0x4b, 0x17);
            this.buttonPen.TabIndex = 1;
            this.buttonPen.Text = "Lines";
            this.buttonPen.UseVisualStyleBackColor = true;
            this.groupBoxSpacing.Controls.Add(this.numericUDVertical);
            this.groupBoxSpacing.Controls.Add(this.numericUDHorizontal);
            this.groupBoxSpacing.Controls.Add(this.label2);
            this.groupBoxSpacing.Controls.Add(this.label1);
            this.groupBoxSpacing.Location = new Point(8, 0x11);
            this.groupBoxSpacing.Name = "groupBoxSpacing";
            this.groupBoxSpacing.Size = new Size(0xa7, 0x47);
            this.groupBoxSpacing.TabIndex = 0;
            this.groupBoxSpacing.TabStop = false;
            this.groupBoxSpacing.Text = "Spacing";
            this.numericUDVertical.Location = new Point(0x4e, 0x2d);
            this.numericUDVertical.Name = "numericUDVertical";
            this.numericUDVertical.Size = new Size(80, 20);
            this.numericUDVertical.TabIndex = 3;
            this.numericUDVertical.ValueChanged += new EventHandler(this.numericUDVertical_ValueChanged);
            this.numericUDHorizontal.Location = new Point(0x4e, 0x13);
            this.numericUDHorizontal.Name = "numericUDHorizontal";
            this.numericUDHorizontal.Size = new Size(80, 20);
            this.numericUDHorizontal.TabIndex = 2;
            this.numericUDHorizontal.ValueChanged += new EventHandler(this.numericUDHorizontal_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x12, 0x2f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2a, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Vertical";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x12, 0x15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Horizontal";
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x11c, 240);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nodes";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.UseVisualStyleBackColor = true;
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(3, 0x24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.treeViewNodes);
            this.splitContainer1.Panel2.Controls.Add(this.tabControlItem);
            this.splitContainer1.SplitterDistance = 0x5c;
            this.splitContainer1.Size = new Size(0x116, 0xc9);
            this.splitContainer1.TabIndex = 1;
            this.treeViewNodes.Dock = DockStyle.Fill;
            this.treeViewNodes.FullRowSelect = true;
            this.treeViewNodes.Location = new Point(0, 0);
            this.treeViewNodes.Name = "treeViewNodes";
            this.treeViewNodes.Size = new Size(0x5c, 0xc9);
            this.treeViewNodes.TabIndex = 0;
            this.treeViewNodes.AfterSelect += new TreeViewEventHandler(this.treeViewNodes_AfterSelect);
            this.tabControlItem.Controls.Add(this.tabPageText);
            this.tabControlItem.Controls.Add(this.tabPageSize);
            this.tabControlItem.Dock = DockStyle.Fill;
            this.tabControlItem.Location = new Point(0, 0);
            this.tabControlItem.Name = "tabControlItem";
            this.tabControlItem.SelectedIndex = 0;
            this.tabControlItem.Size = new Size(0xb6, 0xc9);
            this.tabControlItem.TabIndex = 0;
            this.tabPageText.Controls.Add(this.cbClipText);
            this.tabPageText.Controls.Add(this.cBoxTextAlignment);
            this.tabPageText.Controls.Add(this.label5);
            this.tabPageText.Controls.Add(this.cbTextVisible);
            this.tabPageText.Controls.Add(this.textBox1);
            this.tabPageText.Controls.Add(this.label4);
            this.tabPageText.Location = new Point(4, 0x16);
            this.tabPageText.Name = "tabPageText";
            this.tabPageText.Padding = new Padding(3);
            this.tabPageText.UseVisualStyleBackColor = true;
            this.tabPageText.Size = new Size(0xae, 0xaf);
            this.tabPageText.TabIndex = 0;
            this.tabPageText.Text = "Text";
            this.cbClipText.AutoSize = true;
            this.cbClipText.UseVisualStyleBackColor = true;
            this.cbClipText.FlatStyle = FlatStyle.Flat;
            this.cbClipText.Location = new Point(9, 0x75);
            this.cbClipText.Name = "cbClipText";
            this.cbClipText.Size = new Size(40, 0x11);
            this.cbClipText.TabIndex = 5;
            this.cbClipText.Text = "Clip";
            this.cbClipText.CheckedChanged += new EventHandler(this.cbClipText_CheckedChanged);
            this.cBoxTextAlignment.FlatStyle = FlatStyle.Flat;
            this.cBoxTextAlignment.FormattingEnabled = true;
            this.cBoxTextAlignment.Items.AddRange(new object[] { "Left", "Center", "Right" });
            this.cBoxTextAlignment.Location = new Point(0x54, 0x70);
            this.cBoxTextAlignment.Name = "cBoxTextAlignment";
            this.cBoxTextAlignment.Size = new Size(0x49, 0x15);
            this.cBoxTextAlignment.TabIndex = 4;
            this.cBoxTextAlignment.SelectedIndexChanged += new EventHandler(this.cBoxTextAlignment_SelectedIndexChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x51, 0x60);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x4c, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Text alignment";
            this.cbTextVisible.AutoSize = true;
            this.cbTextVisible.UseVisualStyleBackColor = true;
            this.cbTextVisible.FlatStyle = FlatStyle.Flat;
            this.cbTextVisible.Location = new Point(9, 0x5e);
            this.cbTextVisible.Name = "cbTextVisible";
            this.cbTextVisible.Size = new Size(0x35, 0x11);
            this.cbTextVisible.TabIndex = 2;
            this.cbTextVisible.Text = "Visible";
            this.cbTextVisible.CheckedChanged += new EventHandler(this.cbTextVisible_CheckedChanged);
            this.textBox1.AcceptsReturn = true;
            this.textBox1.Location = new Point(9, 0x20);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x9f, 0x38);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(6, 0x10);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1c, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "&Text";
            this.tabPageSize.Controls.Add(this.numericUDHeight);
            this.tabPageSize.Controls.Add(this.label7);
            this.tabPageSize.Controls.Add(this.numericUDWidth);
            this.tabPageSize.Controls.Add(this.label6);
            this.tabPageSize.Controls.Add(this.cbSizeAutomatic);
            this.tabPageSize.Location = new Point(4, 0x16);
            this.tabPageSize.Name = "tabPageSize";
            this.tabPageSize.Padding = new Padding(3);
            this.tabPageSize.UseVisualStyleBackColor = true;
            this.tabPageSize.Size = new Size(0xae, 0xaf);
            this.tabPageSize.TabIndex = 1;
            this.tabPageSize.Text = "Size";
            this.numericUDHeight.Location = new Point(50, 80);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.numericUDHeight.Maximum = new decimal(bits);
            this.numericUDHeight.Name = "numericUDHeight";
            this.numericUDHeight.Size = new Size(0x43, 20);
            this.numericUDHeight.TabIndex = 4;
            this.numericUDHeight.ValueChanged += new EventHandler(this.numericUDHeight_ValueChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(6, 0x52);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x26, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Height";
            this.numericUDWidth.Location = new Point(50, 0x36);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x3e8;
            this.numericUDWidth.Maximum = new decimal(numArray2);
            this.numericUDWidth.Name = "numericUDWidth";
            this.numericUDWidth.Size = new Size(0x43, 20);
            this.numericUDWidth.TabIndex = 2;
            this.numericUDWidth.ValueChanged += new EventHandler(this.numericUDWidth_ValueChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(6, 0x38);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x23, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Width";
            this.cbSizeAutomatic.AutoSize = true;
            this.cbSizeAutomatic.UseVisualStyleBackColor = true;
            this.cbSizeAutomatic.Location = new Point(6, 0x15);
            this.cbSizeAutomatic.Name = "cbSizeAutomatic";
            this.cbSizeAutomatic.Size = new Size(0x49, 0x11);
            this.cbSizeAutomatic.TabIndex = 0;
            this.cbSizeAutomatic.Text = "Automatic";
            this.cbSizeAutomatic.CheckedChanged += new EventHandler(this.cbSizeAutomatic_CheckedChanged);
            this.panel1.Controls.Add(this.buttonRemove);
            this.panel1.Controls.Add(this.buttonAdd);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x116, 0x21);
            this.panel1.TabIndex = 0;
            this.buttonRemove.FlatStyle = FlatStyle.Flat;
            this.buttonRemove.Location = new Point(0x22, 7);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new Size(0x17, 0x17);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.Text = "-";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
            this.buttonAdd.FlatStyle = FlatStyle.Flat;
            this.buttonAdd.Location = new Point(5, 7);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new Size(0x17, 0x17);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "+";
            this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x124, 0x10a);
            base.Controls.Add(this.tabControl1);
            base.Name = "OrgChartEditor";
            this.Text = "OrgChartEditor";
            base.Load += new EventHandler(this.OrgChartEditor_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBoxSpacing.ResumeLayout(false);
            this.groupBoxSpacing.PerformLayout();
            this.numericUDVertical.EndInit();
            this.numericUDHorizontal.EndInit();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControlItem.ResumeLayout(false);
            this.tabPageText.ResumeLayout(false);
            this.tabPageText.PerformLayout();
            this.tabPageSize.ResumeLayout(false);
            this.tabPageSize.PerformLayout();
            this.numericUDHeight.EndInit();
            this.numericUDWidth.EndInit();
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void numericUDHeight_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.CurrentItem() != null))
            {
                this.CurrentItem().Format.Height = (int) this.numericUDHeight.Value;
                this.cbSizeAutomatic.Checked = false;
                this.org.Repaint();
            }
        }

        private void numericUDHorizontal_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.org.ItemSpacing.Horizontal = (int) this.numericUDHorizontal.Value;
            }
        }

        private void numericUDVertical_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.org.ItemSpacing.Vertical = (int) this.numericUDVertical.Value;
            }
        }

        private void numericUDWidth_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.CurrentItem() != null))
            {
                this.CurrentItem().Format.Width = (int) this.numericUDWidth.Value;
                this.cbSizeAutomatic.Checked = false;
                this.org.Repaint();
            }
        }

        private void OrgChartEditor_Load(object sender, EventArgs e)
        {
            this.setting = true;
            try
            {
                if (this.org != null)
                {
                    this.AddTWNodes(this.org, 0);
                }
            }
            finally
            {
                this.setting = false;
            }
        }

        private void RefreshNode(int index)
        {
            this.treeViewNodes.SelectedNode = this.FindNode(this.treeViewNodes.Nodes, this.org.Items[index]);
            this.treeViewNodes_AfterSelect(this, new TreeViewEventArgs(this.treeViewNodes.SelectedNode));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!this.setting && (this.CurrentItem() != null))
            {
                this.CurrentItem().Format.Text = this.textBox1.Text;
                this.org.Labels[this.CurrentIndex()] = this.textBox1.Text;
                this.org.Repaint();
            }
        }

        private void treeViewNodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.buttonAdd.Enabled = true;
            this.buttonRemove.Enabled = true;
            this.setting = true;
            try
            {
                if (this.CurrentItem() != null)
                {
                    this.textBox1.Text = this.CurrentItem().Format.Text;
                    this.cbTextVisible.Checked = this.CurrentItem().Format.Visible;
                    this.cBoxTextAlignment.SelectedIndex = (int) this.CurrentItem().Format.TextAlign;
                    this.cbSizeAutomatic.Checked = this.CurrentItem().Format.AutoSize;
                    this.cbClipText.Checked = this.CurrentItem().Format.ClipText;
                    this.numericUDHeight.Value = this.CurrentItem().Format.Height;
                    this.numericUDWidth.Value = this.CurrentItem().Format.Width;
                    this.numericUDHeight.Enabled = !this.CurrentItem().Format.AutoSize;
                    this.numericUDWidth.Enabled = this.numericUDHeight.Enabled;
                    if (this.shapeForm != null)
                    {
                        CustomShapeEditor.Remove(this.tabControlItem, this.shapeForm);
                    }
                    this.shapeForm = CustomShapeEditor.Add(this.tabControlItem, this.CurrentItem().Format);
                }
            }
            finally
            {
                this.setting = false;
            }
        }

        private class OrgNode : TreeNode
        {
            public OrgNode(OrgItem i)
            {
                base.Tag = i;
                base.Text = this.FirstLine(i.Text);
            }

            private string FirstLine(string s)
            {
                int index = s.IndexOf('\n');
                if (index != -1)
                {
                    return s.Substring(0, index);
                }
                return s;
            }
        }
    }
}

