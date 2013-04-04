namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GeneralEditor : Form
    {
        private Button BZoomColor;
        private ButtonPen BZoomPen;
        private CheckBox CBAllowZoom;
        private CheckBox CBAnimatedZoom;
        public ComboBox cbCursor;
        private ComboBox CBDir;
        private ComboBox CBScrollMouse;
        private ComboBox CBZoomMouse;
        private Chart chart;
        private Zoom chartZoom;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label labelCursor;
        private TabControl PageControl1;
        private PictureBox pbCursor;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private GroupBox RGPanning;
        private Cursor selectedCursor;
        private TabPage tabPage1;
        private TabPage TabSheet2;
        private TabPage tpCursor;
        private NumericUpDown UDAniZoomSteps;
        private NumericUpDown UDMinPix;

        public GeneralEditor()
        {
            this.InitializeComponent();
            this.CBZoomMouse.Items.Add("Left");
            this.CBZoomMouse.Items.Add("Middle");
            this.CBZoomMouse.Items.Add("Right");
            this.CBZoomMouse.Items.Add("X Button 1");
            this.CBZoomMouse.Items.Add("X Button 2");
            this.CBDir.Items.Add("Horizontal");
            this.CBDir.Items.Add("Vertical");
            this.CBDir.Items.Add("Both");
            this.CBScrollMouse.Items.Add("Left");
            this.CBScrollMouse.Items.Add("Middle");
            this.CBScrollMouse.Items.Add("Right");
            this.CBScrollMouse.Items.Add("X Button 1");
            this.CBScrollMouse.Items.Add("X Button 2");
        }

        public GeneralEditor(Chart c, Control parent) : this()
        {
            this.chart = c;
            this.chartZoom = c.Zoom;
            EditorUtils.InsertForm(this, parent);
            this.UDAniZoomSteps.Value = this.chart.Zoom.AnimatedSteps;
            this.CBAllowZoom.Checked = this.chart.Zoom.Allow;
            this.CBAnimatedZoom.Checked = this.chart.Zoom.Animated;
            this.UDMinPix.Value = this.chart.Zoom.MinPixels;
            this.BZoomPen.Pen = this.chart.Zoom.Pen;
            this.CBDir.SelectedIndex = (int) this.chart.Zoom.Direction;
            this.CBZoomMouse.SelectedIndex = EditorUtils.MouseButtonIndex(this.chart.Zoom.MouseButton);
            this.CBScrollMouse.SelectedIndex = EditorUtils.MouseButtonIndex(this.chart.Panning.MouseButton);
            switch (this.chart.Panning.Allow)
            {
                case ScrollModes.None:
                    this.radioButton1.Checked = true;
                    break;

                case ScrollModes.Vertical:
                    this.radioButton3.Checked = true;
                    break;

                case ScrollModes.Horizontal:
                    this.radioButton2.Checked = true;
                    break;

                case ScrollModes.Both:
                    this.radioButton4.Checked = true;
                    break;
            }
            if (this.chart.parent != null)
            {
                Cursor cursor = this.chart.parent.GetCursor();
                if (cursor == null)
                {
                    this.cbCursor.Visible = false;
                    this.labelCursor.Visible = false;
                }
                else
                {
                    this.selectedCursor = cursor;
                    EditorUtils.FillCursors(this.cbCursor, cursor);
                }
            }
        }

        private void BPrint_Click(object sender, EventArgs e)
        {
            PrintPreview.ShowModal(this.chart);
        }

        private void BZoomColor_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.chart.Zoom.Brush);
        }

        private void CBAllowZoom_CheckedChanged(object sender, EventArgs e)
        {
            this.chartZoom.Allow = this.CBAllowZoom.Checked;
        }

        private void CBAnimatedZoom_CheckedChanged(object sender, EventArgs e)
        {
            this.chartZoom.Animated = this.CBAnimatedZoom.Checked;
        }

        private void cbCursor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedCursor = EditorUtils.StringToCursor(this.cbCursor.SelectedItem.ToString());
            this.chart.parent.SetCursor(this.selectedCursor);
            this.pbCursor.Invalidate();
        }

        private void CBDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CBDir.SelectedIndex == 0)
            {
                this.chart.zoom.Direction = ZoomDirections.Horizontal;
            }
            else if (this.CBDir.SelectedIndex == 1)
            {
                this.chart.zoom.Direction = ZoomDirections.Vertical;
            }
            else
            {
                this.chart.zoom.Direction = ZoomDirections.Both;
            }
        }

        private void CBScrollMouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chart.Panning.MouseButton = EditorUtils.MouseButtonFromIndex(this.CBScrollMouse.SelectedIndex);
        }

        private void CBZoomMouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chart.Zoom.MouseButton = EditorUtils.MouseButtonFromIndex(this.CBZoomMouse.SelectedIndex);
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
            this.PageControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.UDAniZoomSteps = new NumericUpDown();
            this.UDMinPix = new NumericUpDown();
            this.CBZoomMouse = new ComboBox();
            this.CBDir = new ComboBox();
            this.BZoomColor = new Button();
            this.BZoomPen = new ButtonPen();
            this.CBAnimatedZoom = new CheckBox();
            this.CBAllowZoom = new CheckBox();
            this.TabSheet2 = new TabPage();
            this.label1 = new Label();
            this.CBScrollMouse = new ComboBox();
            this.RGPanning = new GroupBox();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.tpCursor = new TabPage();
            this.pbCursor = new PictureBox();
            this.cbCursor = new ComboBox();
            this.labelCursor = new Label();
            this.groupBox1 = new GroupBox();
            this.PageControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((ISupportInitialize) this.pbCursor).BeginInit();
            this.UDAniZoomSteps.BeginInit();
            this.UDMinPix.BeginInit();
            this.TabSheet2.SuspendLayout();
            this.RGPanning.SuspendLayout();
            this.tpCursor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.PageControl1.Controls.Add(this.tabPage1);
            this.PageControl1.Controls.Add(this.TabSheet2);
            this.PageControl1.Controls.Add(this.tpCursor);
            this.PageControl1.Dock = DockStyle.Fill;
            this.PageControl1.HotTrack = true;
            this.PageControl1.Location = new Point(0, 0);
            this.PageControl1.Name = "PageControl1";
            this.PageControl1.SelectedIndex = 0;
            this.PageControl1.Size = new Size(0x170, 0xda);
            this.PageControl1.TabIndex = 2;
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.UDAniZoomSteps);
            this.tabPage1.Controls.Add(this.UDMinPix);
            this.tabPage1.Controls.Add(this.CBZoomMouse);
            this.tabPage1.Controls.Add(this.CBDir);
            this.tabPage1.Controls.Add(this.BZoomColor);
            this.tabPage1.Controls.Add(this.BZoomPen);
            this.tabPage1.Controls.Add(this.CBAnimatedZoom);
            this.tabPage1.Controls.Add(this.CBAllowZoom);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(360, 0xc0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Zoom";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(11, 0x9a);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x4c, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "&Mouse Button:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x24, 130);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x34, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "&Direction:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(60, 0x6a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(80, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "&Minimum pixels:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x6c, 0x29);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x25, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Steps:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDAniZoomSteps.BorderStyle = BorderStyle.FixedSingle;
            this.UDAniZoomSteps.Location = new Point(0x94, 0x27);
            this.UDAniZoomSteps.Name = "UDAniZoomSteps";
            this.UDAniZoomSteps.Size = new Size(40, 20);
            this.UDAniZoomSteps.TabIndex = 3;
            this.UDAniZoomSteps.TextAlign = HorizontalAlignment.Right;
            int[] bits = new int[4];
            bits[0] = 8;
            this.UDAniZoomSteps.Value = new decimal(bits);
            this.UDAniZoomSteps.ValueChanged += new EventHandler(this.UDAniZoomSteps_ValueChanged);
            this.UDAniZoomSteps.TextChanged += new EventHandler(this.UDAniZoomSteps_ValueChanged);
            this.UDMinPix.BorderStyle = BorderStyle.FixedSingle;
            this.UDMinPix.Location = new Point(0x94, 0x68);
            this.UDMinPix.Name = "UDMinPix";
            this.UDMinPix.Size = new Size(40, 20);
            this.UDMinPix.TabIndex = 7;
            this.UDMinPix.TextAlign = HorizontalAlignment.Right;
            int[] numArray2 = new int[4];
            numArray2[0] = 0x10;
            this.UDMinPix.Value = new decimal(numArray2);
            this.UDMinPix.ValueChanged += new EventHandler(this.UDMinPix_ValueChanged);
            this.UDMinPix.TextChanged += new EventHandler(this.UDMinPix_ValueChanged);
            this.CBZoomMouse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBZoomMouse.Location = new Point(0x5c, 0x98);
            this.CBZoomMouse.Name = "CBZoomMouse";
            this.CBZoomMouse.Size = new Size(0x60, 0x15);
            this.CBZoomMouse.TabIndex = 11;
            this.CBZoomMouse.SelectedIndexChanged += new EventHandler(this.CBZoomMouse_SelectedIndexChanged);
            this.CBDir.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBDir.Location = new Point(0x5c, 0x80);
            this.CBDir.Name = "CBDir";
            this.CBDir.Size = new Size(0x60, 0x15);
            this.CBDir.TabIndex = 9;
            this.CBDir.SelectedIndexChanged += new EventHandler(this.CBDir_SelectedIndexChanged);
            this.BZoomColor.FlatStyle = FlatStyle.Flat;
            this.BZoomColor.Location = new Point(0x6c, 0x48);
            this.BZoomColor.Name = "BZoomColor";
            this.BZoomColor.Size = new Size(80, 0x17);
            this.BZoomColor.TabIndex = 5;
            this.BZoomColor.Text = "P&attern...";
            this.BZoomColor.Click += new EventHandler(this.BZoomColor_Click);
            this.BZoomPen.FlatStyle = FlatStyle.Flat;
            this.BZoomPen.Location = new Point(12, 0x48);
            this.BZoomPen.Name = "BZoomPen";
            this.BZoomPen.Size = new Size(80, 0x17);
            this.BZoomPen.TabIndex = 4;
            this.BZoomPen.Text = "P&en...";
            this.CBAnimatedZoom.FlatStyle = FlatStyle.Flat;
            this.CBAnimatedZoom.Location = new Point(12, 40);
            this.CBAnimatedZoom.Name = "CBAnimatedZoom";
            this.CBAnimatedZoom.Size = new Size(0x5c, 0x10);
            this.CBAnimatedZoom.TabIndex = 1;
            this.CBAnimatedZoom.Text = "An&imated";
            this.CBAnimatedZoom.CheckedChanged += new EventHandler(this.CBAnimatedZoom_CheckedChanged);
            this.CBAllowZoom.FlatStyle = FlatStyle.Flat;
            this.CBAllowZoom.Location = new Point(12, 0x10);
            this.CBAllowZoom.Name = "CBAllowZoom";
            this.CBAllowZoom.Size = new Size(80, 0x10);
            this.CBAllowZoom.TabIndex = 0;
            this.CBAllowZoom.Text = "&Allow";
            this.CBAllowZoom.CheckedChanged += new EventHandler(this.CBAllowZoom_CheckedChanged);
            this.TabSheet2.Controls.Add(this.label1);
            this.TabSheet2.Controls.Add(this.CBScrollMouse);
            this.TabSheet2.Controls.Add(this.RGPanning);
            this.TabSheet2.Location = new Point(4, 0x16);
            this.TabSheet2.Name = "TabSheet2";
            this.TabSheet2.Size = new Size(360, 0xc0);
            this.TabSheet2.TabIndex = 1;
            this.TabSheet2.Text = "Scroll";
            this.TabSheet2.UseVisualStyleBackColor = true;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 0x90);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4c, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Mouse Button:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBScrollMouse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBScrollMouse.Location = new Point(0x5d, 140);
            this.CBScrollMouse.Name = "CBScrollMouse";
            this.CBScrollMouse.Size = new Size(0x60, 0x15);
            this.CBScrollMouse.TabIndex = 2;
            this.CBScrollMouse.SelectedIndexChanged += new EventHandler(this.CBScrollMouse_SelectedIndexChanged);
            this.RGPanning.Controls.Add(this.radioButton4);
            this.RGPanning.Controls.Add(this.radioButton3);
            this.RGPanning.Controls.Add(this.radioButton2);
            this.RGPanning.Controls.Add(this.radioButton1);
            this.RGPanning.Location = new Point(40, 0x10);
            this.RGPanning.Name = "RGPanning";
            this.RGPanning.Size = new Size(120, 0x70);
            this.RGPanning.TabIndex = 0;
            this.RGPanning.TabStop = false;
            this.RGPanning.Text = "Allow Scroll:";
            this.radioButton4.FlatStyle = FlatStyle.Flat;
            this.radioButton4.Location = new Point(8, 0x58);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(0x60, 0x10);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "&Both";
            this.radioButton4.CheckedChanged += new EventHandler(this.radioButton4_CheckedChanged);
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(8, 0x40);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x60, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "&Vertical";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 40);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x60, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Horizontal";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x60, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&None";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.tpCursor.Controls.Add(this.groupBox1);
            this.tpCursor.Controls.Add(this.cbCursor);
            this.tpCursor.Controls.Add(this.labelCursor);
            this.tpCursor.Location = new Point(4, 0x16);
            this.tpCursor.Name = "tpCursor";
            this.tpCursor.Size = new Size(360, 0xc0);
            this.tpCursor.TabIndex = 2;
            this.tpCursor.Text = "Cursor";
            this.tpCursor.UseVisualStyleBackColor = true;
            this.pbCursor.Location = new Point(0x25, 0x13);
            this.pbCursor.Name = "pbCursor";
            this.pbCursor.Size = new Size(0x2a, 0x24);
            this.pbCursor.TabIndex = 5;
            this.pbCursor.TabStop = false;
            this.pbCursor.Paint += new PaintEventHandler(this.pbCursor_Paint);
            this.cbCursor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCursor.Location = new Point(0x39, 0x12);
            this.cbCursor.Name = "cbCursor";
            this.cbCursor.Size = new Size(0x60, 0x15);
            this.cbCursor.TabIndex = 4;
            this.cbCursor.SelectedIndexChanged += new EventHandler(this.cbCursor_SelectedIndexChanged);
            this.labelCursor.AutoSize = true;
            this.labelCursor.Location = new Point(0x11, 20);
            this.labelCursor.Name = "labelCursor";
            this.labelCursor.Size = new Size(40, 13);
            this.labelCursor.TabIndex = 3;
            this.labelCursor.Text = "&Cursor:";
            this.labelCursor.TextAlign = ContentAlignment.TopRight;
            this.groupBox1.Controls.Add(this.pbCursor);
            this.groupBox1.Location = new Point(20, 0x3a);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x77, 0x48);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview:";
            base.ClientSize = new Size(0x170, 0xda);
            base.Controls.Add(this.PageControl1);
            base.Name = "GeneralEditor";
            this.PageControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.TabSheet2.ResumeLayout(false);
            this.TabSheet2.PerformLayout();
            this.RGPanning.ResumeLayout(false);
            this.tpCursor.ResumeLayout(false);
            this.tpCursor.PerformLayout();
            ((ISupportInitialize) this.pbCursor).EndInit();
            this.UDAniZoomSteps.EndInit();
            this.UDMinPix.EndInit();
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void pbCursor_Paint(object sender, PaintEventArgs e)
        {
            this.selectedCursor.Draw(e.Graphics, e.ClipRectangle);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Panning.Allow = ScrollModes.None;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Panning.Allow = ScrollModes.Horizontal;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Panning.Allow = ScrollModes.Vertical;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Panning.Allow = ScrollModes.Both;
        }

        private void UDAniZoomSteps_ValueChanged(object sender, EventArgs e)
        {
            if (this.chart != null)
            {
                this.chartZoom.AnimatedSteps = (int) this.UDAniZoomSteps.Value;
            }
        }

        private void UDMinPix_ValueChanged(object sender, EventArgs e)
        {
            if (this.chart != null)
            {
                this.chart.Zoom.MinPixels = (int) this.UDMinPix.Value;
            }
        }
    }
}

