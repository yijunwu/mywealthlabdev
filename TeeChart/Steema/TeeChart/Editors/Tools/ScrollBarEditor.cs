namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ScrollBarEditor : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private ButtonPen buttonPen1;
        private CheckBox checkBox1;
        private ComboBox comboBox1;
        private Container components;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private LegendScrollBar TeeScroll;

        public ScrollBarEditor()
        {
            this.InitializeComponent();
            this.comboBox1.Items.Add("None");
            this.comboBox1.Items.Add("Lowered");
            this.comboBox1.Items.Add("Raised");
        }

        public ScrollBarEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.TeeScroll = (LegendScrollBar) t;
            if (this.TeeScroll != null)
            {
                this.buttonPen1.Pen = this.TeeScroll.Pen;
                switch (this.TeeScroll.Bevel.Outer)
                {
                    case BevelStyles.None:
                        this.comboBox1.SelectedIndex = 0;
                        break;

                    case BevelStyles.Lowered:
                        this.comboBox1.SelectedIndex = 1;
                        break;

                    case BevelStyles.Raised:
                        this.comboBox1.SelectedIndex = 2;
                        break;
                }
                this.checkBox1.Checked = this.TeeScroll.AutoRepeat;
                this.numericUpDown1.Value = this.TeeScroll.Size;
                this.numericUpDown2.Value = this.TeeScroll.InitialDelay;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.TeeScroll.Brush);
            this.TeeScroll.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.TeeScroll.ThumbBrush);
            this.TeeScroll.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.TeeScroll.ArrowBrush);
            this.TeeScroll.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.TeeScroll.Gradient);
            this.TeeScroll.Invalidate();
        }

        private void buttonPen1_Click(object sender, EventArgs e)
        {
            this.TeeScroll.Invalidate();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            this.TeeScroll.AutoRepeat = this.checkBox1.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    this.TeeScroll.Bevel.Outer = BevelStyles.None;
                    break;

                case 1:
                    this.TeeScroll.Bevel.Outer = BevelStyles.Lowered;
                    break;

                case 2:
                    this.TeeScroll.Bevel.Outer = BevelStyles.Raised;
                    break;
            }
            this.TeeScroll.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static bool Edit(LegendScrollBar lsb)
        {
            using (ScrollBarEditor editor = new ScrollBarEditor(lsb))
            {
                EditorUtils.Translate(editor);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        private void InitializeComponent()
        {
            this.buttonPen1 = new ButtonPen();
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new Button();
            this.button4 = new Button();
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.label2 = new Label();
            this.label3 = new Label();
            this.numericUpDown2 = new NumericUpDown();
            this.checkBox1 = new CheckBox();
            this.numericUpDown1.BeginInit();
            this.numericUpDown2.BeginInit();
            base.SuspendLayout();
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(8, 8);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.Size = new Size(0x4b, 0x17);
            this.buttonPen1.TabIndex = 0;
            this.buttonPen1.Text = "Border...";
            this.buttonPen1.Click += new EventHandler(this.buttonPen1_Click);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(8, 40);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 1;
            this.button1.Text = "Back...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x71, 8);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x62, 0x17);
            this.button2.TabIndex = 3;
            this.button2.Text = "Thumb...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button3.FlatStyle = FlatStyle.Flat;
            this.button3.Location = new Point(0x88, 40);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x4b, 0x17);
            this.button3.TabIndex = 4;
            this.button3.Text = "Arrows...";
            this.button3.Click += new EventHandler(this.button3_Click);
            this.button4.FlatStyle = FlatStyle.Flat;
            this.button4.Location = new Point(0x88, 0x48);
            this.button4.Name = "button4";
            this.button4.Size = new Size(0x4b, 0x17);
            this.button4.TabIndex = 5;
            this.button4.Text = "Gradient...";
            this.button4.Click += new EventHandler(this.button4_Click);
            this.comboBox1.Location = new Point(0x7d, 0x70);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x58, 0x15);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label1.Location = new Point(0x54, 0x74);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x24, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Bevel:";
            this.numericUpDown1.Location = new Point(0xad, 0x90);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.numericUpDown1.Maximum = new decimal(bits);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(40, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label2.Location = new Point(0x8e, 0x92);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1d, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Size:";
            this.label3.Location = new Point(0x5f, 0xab);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x42, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Initial Delay:";
            this.numericUpDown2.Location = new Point(0xa3, 0xa9);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x1388;
            this.numericUpDown2.Maximum = new decimal(numArray2);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(50, 20);
            this.numericUpDown2.TabIndex = 10;
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown2.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(8, 0xb8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x68, 0x10);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Auto-Repeat";
            this.checkBox1.Click += new EventHandler(this.checkBox1_Click);
            base.ClientSize = new Size(0xe9, 0xce);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.numericUpDown2);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.button4);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.buttonPen1);
            base.Name = "ScrollBarEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ScrollBarEditor";
            this.numericUpDown1.EndInit();
            this.numericUpDown2.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.TeeScroll.Size = (int) this.numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.TeeScroll.InitialDelay = (int) this.numericUpDown2.Value;
        }
    }
}

