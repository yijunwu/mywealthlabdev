namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class RotateEditor : Form
    {
        private ButtonPen buttonPen1;
        private CheckBox checkBox1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Container components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Rotate rotate;
        private NumericUpDown udSpeed;

        public RotateEditor()
        {
            this.InitializeComponent();
            this.comboBox1.Items.Add("Rotation");
            this.comboBox1.Items.Add("Elevation");
            this.comboBox1.Items.Add("Both");
            this.comboBox2.Items.Add("Left");
            this.comboBox2.Items.Add("Middle");
            this.comboBox2.Items.Add("Right");
            this.comboBox2.Items.Add("X Button 1");
            this.comboBox2.Items.Add("X Button 2");
        }

        public RotateEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.rotate = (Rotate) t;
            this.checkBox1.Checked = this.rotate.Inverted;
            switch (this.rotate.Style)
            {
                case RotateStyles.Rotation:
                    this.comboBox1.SelectedIndex = 0;
                    break;

                case RotateStyles.Elevation:
                    this.comboBox1.SelectedIndex = 1;
                    break;

                default:
                    this.comboBox1.SelectedIndex = 2;
                    break;
            }
            this.comboBox2.SelectedIndex = EditorUtils.MouseButtonIndex(this.rotate.Button);
            this.buttonPen1.Pen = this.rotate.Pen;
            this.udSpeed.Value = this.rotate.Speed;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.rotate.Inverted = this.checkBox1.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    this.rotate.Style = RotateStyles.Rotation;
                    return;

                case 1:
                    this.rotate.Style = RotateStyles.Elevation;
                    return;
            }
            this.rotate.Style = RotateStyles.All;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rotate.Button = EditorUtils.MouseButtonFromIndex(this.comboBox2.SelectedIndex);
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
            this.checkBox1 = new CheckBox();
            this.label1 = new Label();
            this.comboBox1 = new ComboBox();
            this.comboBox2 = new ComboBox();
            this.label2 = new Label();
            this.buttonPen1 = new ButtonPen();
            this.udSpeed = new NumericUpDown();
            this.label3 = new Label();
            this.udSpeed.BeginInit();
            base.SuspendLayout();
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(12, -1);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x68, 0x18);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "&Inverted";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(9, 0x1a);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x21, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Style:";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new Point(12, 0x2a);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x79, 0x15);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox2.Location = new Point(15, 0x58);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(0x79, 0x15);
            this.comboBox2.TabIndex = 4;
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 0x48);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4b, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Mouse button:";
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(15, 0x7a);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.Size = new Size(0x4b, 0x17);
            this.buttonPen1.TabIndex = 5;
            this.buttonPen1.Text = "&Outline...";
            this.udSpeed.Location = new Point(0x8f, 0x7d);
            this.udSpeed.Name = "udSpeed";
            this.udSpeed.Size = new Size(0x2d, 20);
            this.udSpeed.TabIndex = 6;
            int[] bits = new int[4];
            bits[0] = 50;
            this.udSpeed.Value = new decimal(bits);
            this.udSpeed.Click += new EventHandler(this.udSpeed_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x60, 0x7f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x29, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Sp&eed:";
            base.ClientSize = new Size(200, 0x9d);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.udSpeed);
            base.Controls.Add(this.buttonPen1);
            base.Controls.Add(this.comboBox2);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.checkBox1);
            base.Name = "RotateEditor";
            this.Text = "RotateEditor";
            this.udSpeed.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void udSpeed_Click(object sender, EventArgs e)
        {
            this.rotate.Speed = (int) this.udSpeed.Value;
        }
    }
}

