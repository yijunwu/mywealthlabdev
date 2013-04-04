namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BevelImageEditor : Form
    {
        private Button bBrush;
        private Button bPen;
        private CheckBox cbVisible;
        private Container components;
        private ImageBevel ibevel;
        private Label label2;
        private bool setting;
        private NumericUpDown UDWidth;

        public BevelImageEditor()
        {
            this.InitializeComponent();
        }

        public BevelImageEditor(ImageBevel b, Control parent)
        {
            this.setting = true;
            this.ibevel = b;
            this.InitializeComponent();
            this.UDWidth.Value = this.ibevel.Width;
            this.cbVisible.Checked = this.ibevel.Visible;
            if (parent != null)
            {
                EditorUtils.InsertForm(this, parent);
            }
            this.setting = false;
        }

        private void bBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.ibevel.Brush);
        }

        private void bPen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.ibevel.Pen);
        }

        private void cbVisible_CheckedChanged(object sender, EventArgs e)
        {
            this.ibevel.Visible = this.cbVisible.Checked;
        }

        public void DisableControls()
        {
            foreach (Control control in base.Controls)
            {
                control.Enabled = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EnableControls()
        {
            foreach (Control control in base.Controls)
            {
                control.Enabled = true;
            }
            this.ibevel.Visible = this.cbVisible.Checked;
        }

        private void InitializeComponent()
        {
            this.label2 = new Label();
            this.UDWidth = new NumericUpDown();
            this.cbVisible = new CheckBox();
            this.bPen = new Button();
            this.bBrush = new Button();
            this.UDWidth.BeginInit();
            base.SuspendLayout();
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x36, 0x5d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 0x10);
            this.label2.TabIndex = 8;
            this.label2.Text = "Wi&dth:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDWidth.BorderStyle = BorderStyle.FixedSingle;
            this.UDWidth.Location = new Point(0x60, 0x5b);
            int[] bits = new int[4];
            bits[0] = 200;
            this.UDWidth.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.UDWidth.Minimum = new decimal(numArray2);
            this.UDWidth.Name = "UDWidth";
            this.UDWidth.Size = new Size(0x30, 20);
            this.UDWidth.TabIndex = 9;
            this.UDWidth.TextAlign = HorizontalAlignment.Right;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            this.UDWidth.Value = new decimal(numArray3);
            this.UDWidth.TextChanged += new EventHandler(this.UDWidth_ValueChanged);
            this.UDWidth.ValueChanged += new EventHandler(this.UDWidth_ValueChanged);
            this.cbVisible.FlatStyle = FlatStyle.Flat;
            this.cbVisible.Location = new Point(8, 8);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new Size(0x58, 0x18);
            this.cbVisible.TabIndex = 10;
            this.cbVisible.Text = "Visible";
            this.cbVisible.CheckedChanged += new EventHandler(this.cbVisible_CheckedChanged);
            this.bPen.FlatStyle = FlatStyle.Flat;
            this.bPen.Location = new Point(8, 0x30);
            this.bPen.Name = "bPen";
            this.bPen.TabIndex = 11;
            this.bPen.Text = "Pen";
            this.bPen.Click += new EventHandler(this.bPen_Click);
            this.bBrush.FlatStyle = FlatStyle.Flat;
            this.bBrush.Location = new Point(0x58, 0x30);
            this.bBrush.Name = "bBrush";
            this.bBrush.TabIndex = 12;
            this.bBrush.Text = "Brush";
            this.bBrush.Click += new EventHandler(this.bBrush_Click);
            base.ClientSize = new Size(0xa8, 0x8d);
            base.Controls.Add(this.bBrush);
            base.Controls.Add(this.bPen);
            base.Controls.Add(this.cbVisible);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.UDWidth);
            base.Name = "BevelImageEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ImageBevel Editor";
            this.UDWidth.EndInit();
            base.ResumeLayout(false);
        }

        public static void ShowModal(ImageBevel b)
        {
            using (BevelImageEditor editor = new BevelImageEditor(b, null))
            {
                editor.ShowDialog();
            }
        }

        private void UDWidth_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.ibevel.Width = (int) this.UDWidth.Value;
            }
        }
    }
}

