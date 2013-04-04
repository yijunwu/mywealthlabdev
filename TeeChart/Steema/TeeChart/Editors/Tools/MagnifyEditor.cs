namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MagnifyEditor : Form
    {
        private Button bBorder;
        private CheckBox cbCircled;
        private ComboBox cbCursor;
        private CheckBox cbDrag;
        private CheckBox cbFollow;
        private CheckBox cbResize;
        private IContainer components;
        private GroupBox groupBox1;
        private AnnotationEditor iAnnEditor;
        private Label label1;
        private Label label2;
        private Label label3;
        private Magnify magnify;
        private TabControl tabControl1;
        private TabPage tabOptions;
        private TabPage tabProperties;
        private TrackBar tbFactor;
        private NumericUpDown udWheel;

        public MagnifyEditor()
        {
            this.InitializeComponent();
        }

        public MagnifyEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.magnify = s as Magnify;
            if (this.magnify != null)
            {
                this.cbFollow.Checked = this.magnify.FollowMouse;
                this.tbFactor.Value = Utils.Round(this.magnify.Percent);
                this.cbCircled.Checked = this.magnify.Circled;
                this.cbDrag.Checked = this.magnify.AllowDrag;
                this.cbResize.Checked = this.magnify.AllowResize;
                this.udWheel.Value = this.magnify.WheelZoom;
                EditorUtils.FillCursors(this.cbCursor, this.Cursor);
                this.iAnnEditor = new AnnotationEditor(this.magnify);
                EditorUtils.InsertForm(this.iAnnEditor, this.tabProperties);
            }
        }

        private void bBorder_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.magnify.Shape.Pen);
        }

        private void cbCircled_CheckedChanged(object sender, EventArgs e)
        {
            this.magnify.Circled = this.cbCircled.Checked;
        }

        private void cbCursor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.magnify.Cursor = EditorUtils.StringToCursor((string) this.cbCursor.SelectedItem);
        }

        private void cbDrag_CheckedChanged(object sender, EventArgs e)
        {
            this.magnify.AllowDrag = this.cbDrag.Checked;
        }

        private void cbFollow_CheckedChanged(object sender, EventArgs e)
        {
            this.magnify.FollowMouse = this.cbFollow.Checked;
        }

        private void cbResize_CheckedChanged(object sender, EventArgs e)
        {
            this.magnify.AllowResize = this.cbResize.Checked;
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
            this.tabControl1 = new TabControl();
            this.tabOptions = new TabPage();
            this.label3 = new Label();
            this.udWheel = new NumericUpDown();
            this.label2 = new Label();
            this.cbCircled = new CheckBox();
            this.cbFollow = new CheckBox();
            this.cbResize = new CheckBox();
            this.cbDrag = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.tbFactor = new TrackBar();
            this.bBorder = new Button();
            this.label1 = new Label();
            this.cbCursor = new ComboBox();
            this.tabProperties = new TabPage();
            this.tabControl1.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.udWheel.BeginInit();
            this.groupBox1.SuspendLayout();
            this.tbFactor.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabOptions);
            this.tabControl1.Controls.Add(this.tabProperties);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x11d, 0xe9);
            this.tabControl1.TabIndex = 0;
            this.tabOptions.Controls.Add(this.label3);
            this.tabOptions.Controls.Add(this.udWheel);
            this.tabOptions.Controls.Add(this.label2);
            this.tabOptions.Controls.Add(this.cbCircled);
            this.tabOptions.Controls.Add(this.cbFollow);
            this.tabOptions.Controls.Add(this.cbResize);
            this.tabOptions.Controls.Add(this.cbDrag);
            this.tabOptions.Controls.Add(this.groupBox1);
            this.tabOptions.Controls.Add(this.bBorder);
            this.tabOptions.Controls.Add(this.label1);
            this.tabOptions.Controls.Add(this.cbCursor);
            this.tabOptions.Location = new Point(4, 0x16);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Padding = new Padding(3);
            this.tabOptions.Size = new Size(0x115, 0xcf);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(90, 180);
            this.label3.Name = "label3";
            this.label3.Size = new Size(15, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "%";
            this.udWheel.Location = new Point(14, 0xb2);
            this.udWheel.Name = "udWheel";
            this.udWheel.Size = new Size(70, 20);
            this.udWheel.TabIndex = 10;
            this.udWheel.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(11, 0xa2);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x6a, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Mouse &Wheel Zoom:";
            this.cbCircled.AutoSize = true;
            this.cbCircled.Checked = true;
            this.cbCircled.CheckState = CheckState.Checked;
            this.cbCircled.Location = new Point(14, 140);
            this.cbCircled.Name = "cbCircled";
            this.cbCircled.Size = new Size(0x3a, 0x11);
            this.cbCircled.TabIndex = 7;
            this.cbCircled.Text = "C&ircled";
            this.cbCircled.UseVisualStyleBackColor = true;
            this.cbCircled.CheckedChanged += new EventHandler(this.cbCircled_CheckedChanged);
            this.cbFollow.AutoSize = true;
            this.cbFollow.Checked = true;
            this.cbFollow.CheckState = CheckState.Checked;
            this.cbFollow.Location = new Point(0x97, 140);
            this.cbFollow.Name = "cbFollow";
            this.cbFollow.Size = new Size(0x5b, 0x11);
            this.cbFollow.TabIndex = 6;
            this.cbFollow.Text = "&Follow Mouse";
            this.cbFollow.UseVisualStyleBackColor = true;
            this.cbFollow.CheckedChanged += new EventHandler(this.cbFollow_CheckedChanged);
            this.cbResize.AutoSize = true;
            this.cbResize.Location = new Point(0xb1, 0x6a);
            this.cbResize.Name = "cbResize";
            this.cbResize.Size = new Size(0x56, 0x11);
            this.cbResize.TabIndex = 5;
            this.cbResize.Text = "Allow &Resize";
            this.cbResize.UseVisualStyleBackColor = true;
            this.cbResize.CheckedChanged += new EventHandler(this.cbResize_CheckedChanged);
            this.cbDrag.AutoSize = true;
            this.cbDrag.Location = new Point(0xb1, 0x53);
            this.cbDrag.Name = "cbDrag";
            this.cbDrag.Size = new Size(0x4d, 0x11);
            this.cbDrag.TabIndex = 4;
            this.cbDrag.Text = "Allow &Drag";
            this.cbDrag.UseVisualStyleBackColor = true;
            this.cbDrag.CheckedChanged += new EventHandler(this.cbDrag_CheckedChanged);
            this.groupBox1.Controls.Add(this.tbFactor);
            this.groupBox1.Location = new Point(8, 0x42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xa3, 0x44);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Zoom %:";
            this.tbFactor.LargeChange = 20;
            this.tbFactor.Location = new Point(6, 0x11);
            this.tbFactor.Maximum = 100;
            this.tbFactor.Name = "tbFactor";
            this.tbFactor.Size = new Size(0x97, 0x2d);
            this.tbFactor.SmallChange = 10;
            this.tbFactor.TabIndex = 4;
            this.tbFactor.Scroll += new EventHandler(this.tbFactor_Scroll);
            this.bBorder.FlatStyle = FlatStyle.Flat;
            this.bBorder.Location = new Point(0xa7, 0x1b);
            this.bBorder.Name = "bBorder";
            this.bBorder.Size = new Size(0x4b, 0x17);
            this.bBorder.TabIndex = 2;
            this.bBorder.Text = "&Border...";
            this.bBorder.UseVisualStyleBackColor = true;
            this.bBorder.Click += new EventHandler(this.bBorder_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Cursor:";
            this.cbCursor.FormattingEnabled = true;
            this.cbCursor.Location = new Point(8, 0x1d);
            this.cbCursor.Name = "cbCursor";
            this.cbCursor.Size = new Size(0x79, 0x15);
            this.cbCursor.TabIndex = 0;
            this.cbCursor.SelectedIndexChanged += new EventHandler(this.cbCursor_SelectedIndexChanged);
            this.tabProperties.Location = new Point(4, 0x16);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new Size(0x115, 0xcf);
            this.tabProperties.TabIndex = 1;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x11d, 0xe9);
            base.Controls.Add(this.tabControl1);
            base.Name = "MagnifyEditor";
            this.Text = "MagnifyEditor";
            this.tabControl1.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabOptions.PerformLayout();
            this.udWheel.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tbFactor.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.magnify.WheelZoom = (int) this.udWheel.Value;
        }

        private void tbFactor_Scroll(object sender, EventArgs e)
        {
            this.magnify.Percent = this.tbFactor.Value;
        }
    }
}

