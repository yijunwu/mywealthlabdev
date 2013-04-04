namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Tools;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ScrollToolEditor : Form
    {
        private ComboBox comboBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown numericUDStartPos;
        private NumericUpDown numUDViewZone;
        private ScrollTool scroll;

        public ScrollToolEditor()
        {
            this.InitializeComponent();
        }

        public ScrollToolEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            string[] names = Enum.GetNames(typeof(ScrollToolViewUnit));
            for (int i = 0; i < names.Length; i++)
            {
                object item = names[i];
                this.comboBox1.Items.Add(item);
            }
            this.scroll = (ScrollTool) t;
            this.numUDViewZone.Value = this.scroll.ViewSegmentSize;
            this.numericUDStartPos.Value = this.scroll.StartPosition;
            this.comboBox1.SelectedIndex = (int) this.scroll.SegmentViewUnits;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.scroll.SegmentViewUnits = (ScrollToolViewUnit) this.comboBox1.SelectedIndex;
        }

        private void InitializeComponent()
        {
            this.label3 = new Label();
            this.numUDViewZone = new NumericUpDown();
            this.label1 = new Label();
            this.numericUDStartPos = new NumericUpDown();
            this.comboBox1 = new ComboBox();
            this.label2 = new Label();
            this.numUDViewZone.BeginInit();
            this.numericUDStartPos.BeginInit();
            base.SuspendLayout();
            this.label3.Location = new Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new Size(110, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Segment View Size:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            int[] bits = new int[4];
            bits[0] = 5;
            this.numUDViewZone.Increment = new decimal(bits);
            this.numUDViewZone.Location = new Point(0x80, 0x44);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.numUDViewZone.Minimum = new decimal(numArray2);
            this.numUDViewZone.Name = "numUDViewZone";
            this.numUDViewZone.Size = new Size(50, 20);
            this.numUDViewZone.TabIndex = 14;
            int[] numArray3 = new int[4];
            numArray3[0] = 20;
            this.numUDViewZone.Value = new decimal(numArray3);
            this.numUDViewZone.ValueChanged += new EventHandler(this.numUDViewZone_ValueChanged);
            this.numUDViewZone.Leave += new EventHandler(this.numUDViewZone_Leave);
            this.label1.Location = new Point(12, 0x61);
            this.label1.Name = "label1";
            this.label1.Size = new Size(110, 13);
            this.label1.TabIndex = 0x11;
            this.label1.Text = "Segment Start Pos:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            int[] numArray4 = new int[4];
            numArray4[0] = 5;
            this.numericUDStartPos.Increment = new decimal(numArray4);
            this.numericUDStartPos.Location = new Point(0x80, 0x5f);
            int[] numArray5 = new int[4];
            numArray5[0] = 1;
            this.numericUDStartPos.Minimum = new decimal(numArray5);
            this.numericUDStartPos.Name = "numericUDStartPos";
            this.numericUDStartPos.Size = new Size(50, 20);
            this.numericUDStartPos.TabIndex = 0x10;
            int[] numArray6 = new int[4];
            numArray6[0] = 20;
            this.numericUDStartPos.Value = new decimal(numArray6);
            this.numericUDStartPos.ValueChanged += new EventHandler(this.numericUDStartPos_ValueChanged);
            this.numericUDStartPos.Leave += new EventHandler(this.numericUDStartPos_Leave);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(0x74, 0x24);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x3d, 0x15);
            this.comboBox1.TabIndex = 0x12;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label2.Location = new Point(0x2f, 0x27);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3f, 13);
            this.label2.TabIndex = 0x13;
            this.label2.Text = "Unit:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(200, 0x9d);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.numericUDStartPos);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.numUDViewZone);
            base.Name = "ScrollToolEditor";
            this.Text = "ScrollToolEditor";
            this.numUDViewZone.EndInit();
            this.numericUDStartPos.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUDStartPos_Leave(object sender, EventArgs e)
        {
            this.numericUDStartPos_ValueChanged(sender, e);
        }

        private void numericUDStartPos_ValueChanged(object sender, EventArgs e)
        {
            this.scroll.StartPosition = (int) this.numericUDStartPos.Value;
        }

        private void numUDViewZone_Leave(object sender, EventArgs e)
        {
            this.numUDViewZone_ValueChanged(sender, e);
        }

        private void numUDViewZone_ValueChanged(object sender, EventArgs e)
        {
            this.scroll.ViewSegmentSize = (int) this.numUDViewZone.Value;
        }
    }
}

