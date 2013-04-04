namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ZoomToolEditor : Form
    {
        private ButtonColor buttonColor1;
        private Container components;
        private Label label1;
        private Label label3;
        private NumericUpDown numUDTransp;
        private NumericUpDown numUDZIndex;
        private ZoomTool zoom;

        public ZoomToolEditor()
        {
            this.InitializeComponent();
        }

        public ZoomToolEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.zoom = (ZoomTool) t;
            this.buttonColor1.Color = this.zoom.ZoomPenColor;
            this.numUDZIndex.Value = this.zoom.ZoomCanvasIndex;
            this.numUDTransp.Value = this.zoom.ZoomFillTransparency;
            EditorUtils.Translate(this);
        }

        private void buttonColor1_Click(object sender, EventArgs e)
        {
            this.zoom.ZoomPenColor = this.buttonColor1.Color;
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
            this.label3 = new Label();
            this.numUDZIndex = new NumericUpDown();
            this.label1 = new Label();
            this.numUDTransp = new NumericUpDown();
            this.buttonColor1 = new ButtonColor();
            this.numUDZIndex.BeginInit();
            this.numUDTransp.BeginInit();
            base.SuspendLayout();
            this.label3.Location = new Point(0x26, 0x51);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x42, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Z-Index:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.numUDZIndex.Location = new Point(0x6d, 0x4f);
            int[] bits = new int[4];
            bits[0] = 500;
            this.numUDZIndex.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 500;
            numArray2[3] = -2147483648;
            this.numUDZIndex.Minimum = new decimal(numArray2);
            this.numUDZIndex.Name = "numUDZIndex";
            this.numUDZIndex.Size = new Size(50, 20);
            this.numUDZIndex.TabIndex = 12;
            this.numUDZIndex.ValueChanged += new EventHandler(this.numUDZIndex_ValueChanged);
            this.numUDZIndex.Leave += new EventHandler(this.numUDZIndex_Leave);
            this.label1.Location = new Point(0x26, 0x6b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x42, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Z-Index:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.numUDTransp.Location = new Point(0x6d, 0x69);
            this.numUDTransp.Name = "numUDTransp";
            this.numUDTransp.Size = new Size(50, 20);
            this.numUDTransp.TabIndex = 14;
            this.numUDTransp.ValueChanged += new EventHandler(this.numUDTransp_ValueChanged);
            this.numUDTransp.Leave += new EventHandler(this.numUDTransp_Leave);
            this.buttonColor1.Color = Color.Empty;
            this.buttonColor1.Location = new Point(0x54, 0x20);
            this.buttonColor1.Name = "buttonColor1";
            this.buttonColor1.Size = new Size(0x4b, 0x17);
            this.buttonColor1.TabIndex = 6;
            this.buttonColor1.Text = "Color...";
            this.buttonColor1.Click += new EventHandler(this.buttonColor1_Click);
            base.ClientSize = new Size(200, 0x9d);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.numUDTransp);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.numUDZIndex);
            base.Controls.Add(this.buttonColor1);
            base.Name = "ZoomToolEditor";
            this.Text = "ZoomEditor";
            this.numUDZIndex.EndInit();
            this.numUDTransp.EndInit();
            base.ResumeLayout(false);
        }

        private void numUDTransp_Leave(object sender, EventArgs e)
        {
            this.numUDTransp_ValueChanged(sender, e);
        }

        private void numUDTransp_ValueChanged(object sender, EventArgs e)
        {
            this.zoom.ZoomFillTransparency = (int) this.numUDTransp.Value;
        }

        private void numUDZIndex_Leave(object sender, EventArgs e)
        {
            this.numUDZIndex_ValueChanged(sender, e);
        }

        private void numUDZIndex_ValueChanged(object sender, EventArgs e)
        {
            this.zoom.ZoomCanvasIndex = (int) this.numUDZIndex.Value;
        }
    }
}

