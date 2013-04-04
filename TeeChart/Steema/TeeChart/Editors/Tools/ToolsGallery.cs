namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolsGallery : Form
    {
        private Button button1;
        private Button button2;
        private CheckBox cbView3D;
        private Container components;
        private ToolsGalleryDemos galleryDemos;
        private static bool IsWebForm;
        private Label lDescription;
        private ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private TabControl tabControl1;
        private ArrayList tools;

        public ToolsGallery()
        {
            this.InitializeComponent();
            this.CreateSplitControls();
            this.cbView3D.Visible = false;
        }

        private void cbView3D_CheckedChanged(object sender, EventArgs e)
        {
            this.galleryDemos.View3D = this.cbView3D.Checked;
        }

        public static Steema.TeeChart.Tools.Tool CreateNew(Chart chart, IContainer container)
        {
            IsWebForm = chart.Parent.IsWebForm();
            using (ToolsGallery gallery = new ToolsGallery())
            {
                Steema.TeeChart.Tools.Tool component = null;
                if (gallery.ShowDialog() == DialogResult.OK)
                {
                    component = gallery.ToolAt(gallery.listBox1.SelectedIndices[0]);
                    chart.AddToContainer(component);
                }
                return component;
            }
        }

        private void CreateSplitControls()
        {
            Splitter splitter = new Splitter();
            this.listBox1 = new ListBox();
            this.galleryDemos = new ToolsGalleryDemos();
            this.tabControl1 = new TabControl();
            TabPage page = new TabPage("Series");
            TabPage page2 = new TabPage("Axis");
            TabPage page3 = new TabPage("Other");
            this.tabControl1.TabPages.Add(page);
            this.tabControl1.TabPages.Add(page2);
            this.tabControl1.TabPages.Add(page3);
            page.Controls.Add(this.listBox1);
            this.listBox1.Dock = DockStyle.Fill;
            this.listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBox1.DrawItem += new DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.DoubleClick += new EventHandler(this.listView1_DoubleClick);
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.Dock = DockStyle.Left;
            splitter.Dock = DockStyle.Left;
            splitter.MinExtra = 100;
            splitter.MinSize = 0x4b;
            this.galleryDemos.Dock = DockStyle.Fill;
            this.panel3.Controls.AddRange(new Control[] { this.galleryDemos, splitter, this.tabControl1 });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FillTools()
        {
            this.tools = new ArrayList();
            this.listBox1.BeginUpdate();
            this.listBox1.Items.Clear();
            foreach (System.Type type in Utils.ToolTypesOf)
            {
                Steema.TeeChart.Tools.Tool t = (Steema.TeeChart.Tools.Tool) Activator.CreateInstance(type);
                if (this.FilterTool(t))
                {
                    this.tools.Add(t);
                    this.listBox1.Items.Add(t);
                }
            }
            this.listBox1.EndUpdate();
        }

        private bool FilterTool(Steema.TeeChart.Tools.Tool t)
        {
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    if (!IsWebForm)
                    {
                        return ((t is ToolSeries) && !(t is SeriesHotspot));
                    }
                    return ((t is SeriesHotspot) || (t is ExtraLegend));

                case 1:
                    if (!IsWebForm)
                    {
                        return (t is ToolAxis);
                    }
                    return ((t is ColorBand) || (t is GridBand));

                case 2:
                    if (!IsWebForm)
                    {
                        if (((t is ToolSeries) || (t is ToolAxis)) || ((t is ZoomTool) || (t is ScrollTool)))
                        {
                            return false;
                        }
                        return !(t is Marker);
                    }
                    return ((((t is Annotation) || (t is PageNumber)) || (t is ZoomTool)) || (t is ScrollTool));
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new Button();
            this.button1 = new Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lDescription = new Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbView3D = new CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.cbView3D);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x171);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x274, 40);
            this.panel1.TabIndex = 0;
            this.button2.DialogResult = DialogResult.Cancel;
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x21d, 8);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button1.DialogResult = DialogResult.OK;
            this.button1.Enabled = false;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(460, 8);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "&Add";
            this.panel2.Controls.Add(this.lDescription);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 0x141);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x274, 0x30);
            this.panel2.TabIndex = 2;
            this.lDescription.BorderStyle = BorderStyle.FixedSingle;
            this.lDescription.Dock = DockStyle.Fill;
            this.lDescription.Location = new Point(0, 0);
            this.lDescription.Name = "lDescription";
            this.lDescription.Size = new Size(0x274, 0x30);
            this.lDescription.TabIndex = 0;
            this.lDescription.UseMnemonic = false;
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x274, 0x141);
            this.panel3.TabIndex = 3;
            this.cbView3D.AutoSize = true;
            this.cbView3D.Checked = true;
            this.cbView3D.CheckState = CheckState.Checked;
            this.cbView3D.Location = new Point(12, 12);
            this.cbView3D.Name = "cbView3D";
            this.cbView3D.Size = new Size(0x42, 0x11);
            this.cbView3D.TabIndex = 2;
            this.cbView3D.Text = "View 3D";
            this.cbView3D.UseVisualStyleBackColor = true;
            this.cbView3D.CheckedChanged += new EventHandler(this.cbView3D_CheckedChanged);
            base.AcceptButton = this.button1;
            base.CancelButton = this.button2;
            base.ClientSize = new Size(0x274, 0x199);
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Name = "ToolsGallery";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chart Tools Gallery";
            base.Load += new EventHandler(this.ToolsGallery_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index > -1)
            {
                using (Image image = this.ToolAt(e.Index).GetBitmapEditor())
                {
                    if (image != null)
                    {
                        e.Graphics.DrawImage(image, e.Bounds.Left, e.Bounds.Top + 1);
                    }
                }
                Point point = new Point(0x16, e.Bounds.Top + ((this.listBox1.ItemHeight - this.listBox1.Font.Height) / 2));
                string s = this.listBox1.Items[e.Index].ToString();
                e.Graphics.DrawString(s, this.listBox1.Font, new SolidBrush(e.ForeColor), (PointF) point);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            this.lDescription.Text = this.ToolDescription(this.ToolAt(this.listBox1.SelectedIndex));
            this.galleryDemos.CreateGallery(this.ToolAt(this.listBox1.SelectedIndex).GetType());
            EditorUtils.Translate(this.galleryDemos);
            this.cbView3D.Visible = true;
            this.cbView3D.Checked = this.galleryDemos.View3D;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillTools();
            this.listBox1.Parent = this.tabControl1.SelectedTab;
            this.lDescription.Text = "";
        }

        private Steema.TeeChart.Tools.Tool ToolAt(int index)
        {
            Steema.TeeChart.Tools.Tool tool = null;
            if (index > -1)
            {
                tool = (Steema.TeeChart.Tools.Tool) this.listBox1.Items[index];
            }
            return tool;
        }

        private string ToolDescription(Steema.TeeChart.Tools.Tool t)
        {
            return t.Summary;
        }

        private void ToolsGallery_Load(object sender, EventArgs e)
        {
            this.FillTools();
            EditorUtils.Translate(this);
        }
    }
}

