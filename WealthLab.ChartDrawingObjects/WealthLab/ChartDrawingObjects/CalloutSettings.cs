namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class CalloutSettings : UserControl
    {
        private Label ColorBkgrnd;
        private Button fontButton;
        private FontDialog fontDialog_0;
        private GroupBox groupBox1;
        private IContainer icontainer_0;
        private Label label1;
        private ColorPickerPanel pnlColor;
        private RichTextBox richTextBox1;
        private Label transparencyLabel;
        private NumericUpDown transparencyUpDown;

        public CalloutSettings()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void fontButton_Click(object sender, EventArgs e)
        {
            this.fontDialog_0.ShowColor = true;
            this.fontDialog_0.Color = this.richTextBox1.ForeColor;
            this.fontDialog_0.Font = this.richTextBox1.Font;
            if (this.fontDialog_0.ShowDialog() != DialogResult.Cancel)
            {
                this.richTextBox1.Font = this.fontDialog_0.Font;
                this.richTextBox1.ForeColor = this.fontDialog_0.Color;
            }
        }

        private void InitializeComponent()
        {
            this.fontDialog_0 = new FontDialog();
            this.richTextBox1 = new RichTextBox();
            this.label1 = new Label();
            this.fontButton = new Button();
            this.pnlColor = new ColorPickerPanel();
            this.groupBox1 = new GroupBox();
            this.transparencyUpDown = new NumericUpDown();
            this.transparencyLabel = new Label();
            this.ColorBkgrnd = new Label();
            this.groupBox1.SuspendLayout();
            this.transparencyUpDown.BeginInit();
            base.SuspendLayout();
            this.richTextBox1.Location = new Point(1, 0x19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(180, 100);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Enter Text";
            this.fontButton.Location = new Point(0x60, 0x86);
            this.fontButton.Name = "fontButton";
            this.fontButton.Size = new Size(0x57, 0x17);
            this.fontButton.TabIndex = 2;
            this.fontButton.Text = "Change Font";
            this.fontButton.UseVisualStyleBackColor = true;
            this.fontButton.Click += new EventHandler(this.fontButton_Click);
            this.pnlColor.BackColor = Color.Yellow;
            this.pnlColor.Cursor = Cursors.Hand;
            this.pnlColor.DrawOutline = false;
            this.pnlColor.ForeColor = SystemColors.ControlText;
            this.pnlColor.Location = new Point(0x25, 0x15);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.OutlineColor = Color.Black;
            this.pnlColor.Size = new Size(0x17, 0x17);
            this.pnlColor.TabIndex = 3;
            this.pnlColor.Text = "colorPickerPanel1";
            this.groupBox1.Controls.Add(this.transparencyUpDown);
            this.groupBox1.Controls.Add(this.transparencyLabel);
            this.groupBox1.Controls.Add(this.ColorBkgrnd);
            this.groupBox1.Controls.Add(this.pnlColor);
            this.groupBox1.Location = new Point(4, 0xa3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xb3, 0x33);
            this.groupBox1.TabIndex = 0x13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Background";
            int[] bits = new int[4];
            bits[0] = 5;
            this.transparencyUpDown.Increment = new decimal(bits);
            this.transparencyUpDown.Location = new Point(0x85, 0x17);
            this.transparencyUpDown.Name = "transparencyUpDown";
            this.transparencyUpDown.Size = new Size(40, 20);
            this.transparencyUpDown.TabIndex = 4;
            int[] numArray2 = new int[4];
            numArray2[0] = 100;
            this.transparencyUpDown.Value = new decimal(numArray2);
            this.transparencyLabel.AutoSize = true;
            this.transparencyLabel.Location = new Point(60, 0x1a);
            this.transparencyLabel.Name = "transparencyLabel";
            this.transparencyLabel.Size = new Size(0x48, 13);
            this.transparencyLabel.TabIndex = 20;
            this.transparencyLabel.Text = "Transparency";
            this.ColorBkgrnd.AutoSize = true;
            this.ColorBkgrnd.Location = new Point(4, 0x1a);
            this.ColorBkgrnd.Name = "ColorBkgrnd";
            this.ColorBkgrnd.Size = new Size(0x1f, 13);
            this.ColorBkgrnd.TabIndex = 0x13;
            this.ColorBkgrnd.Text = "Color";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.fontButton);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.richTextBox1);
            base.Name = "CalloutSettings";
            base.Size = new Size(0xb9, 0xde);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.transparencyUpDown.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0(object sender, KeyEventArgs e)
        {
            char keyValue = (char) e.KeyValue;
            if (keyValue == '|')
            {
                e.SuppressKeyPress = true;
            }
        }

        public Color BackColor
        {
            get
            {
                return this.pnlColor.BackColor;
            }
            set
            {
                this.pnlColor.BackColor = value;
            }
        }

        public string Text
        {
            get
            {
                return this.richTextBox1.Text;
            }
            set
            {
                this.richTextBox1.Text = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return this.richTextBox1.ForeColor;
            }
            set
            {
                this.richTextBox1.ForeColor = value;
            }
        }

        public Font TextFont
        {
            get
            {
                return this.richTextBox1.Font;
            }
            set
            {
                this.richTextBox1.Font = value;
            }
        }

        public int Transparency
        {
            get
            {
                return (int) this.transparencyUpDown.Value;
            }
            set
            {
                this.transparencyUpDown.Value = value;
            }
        }
    }
}

