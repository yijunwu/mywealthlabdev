namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class BitmapSettings : UserControl
    {
        private Button BrowseBtn;
        private decimal decimal_0;
        private IContainer icontainer_0;
        private TextBox ImagePathTextBox;
        private Label label1;
        private Label label2;
        private OpenFileDialog openFileDialog_0;
        private PictureBox pictureBox1;
        private CheckBox TransparencyCB;
        private Label TransparencyLabel;
        private NumericUpDown transparencyUpDown;

        public BitmapSettings()
        {
            this.InitializeComponent();
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            Stream stream = null;
            this.openFileDialog_0.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            this.openFileDialog_0.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.TIF)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.TIF|All files (*.*)|*.*";
            this.openFileDialog_0.FilterIndex = 1;
            this.openFileDialog_0.RestoreDirectory = true;
            if (this.openFileDialog_0.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    stream = this.openFileDialog_0.OpenFile();
                    if (stream != null)
                    {
                        this.ImagePath = this.openFileDialog_0.FileName;
                        using (stream)
                        {
                            this.pictureBox1.Image = Image.FromStream(stream);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.openFileDialog_0 = new OpenFileDialog();
            this.BrowseBtn = new Button();
            this.pictureBox1 = new PictureBox();
            this.ImagePathTextBox = new TextBox();
            this.label1 = new Label();
            this.TransparencyCB = new CheckBox();
            this.label2 = new Label();
            this.TransparencyLabel = new Label();
            this.transparencyUpDown = new NumericUpDown();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            this.transparencyUpDown.BeginInit();
            base.SuspendLayout();
            this.BrowseBtn.Location = new Point(240, 0x12);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new Size(0x35, 0x17);
            this.BrowseBtn.TabIndex = 0;
            this.BrowseBtn.Text = "&Browse";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new EventHandler(this.BrowseBtn_Click);
            this.pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            this.pictureBox1.Location = new Point(2, 0x48);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x123, 0xa5);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.ImagePathTextBox.Location = new Point(2, 0x13);
            this.ImagePathTextBox.Name = "ImagePathTextBox";
            this.ImagePathTextBox.ReadOnly = true;
            this.ImagePathTextBox.Size = new Size(0xe8, 20);
            this.ImagePathTextBox.TabIndex = 2;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(2, 3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path";
            this.TransparencyCB.AutoSize = true;
            this.TransparencyCB.Location = new Point(0xab, 250);
            this.TransparencyCB.Name = "TransparencyCB";
            this.TransparencyCB.Size = new Size(0x7a, 0x11);
            this.TransparencyCB.TabIndex = 4;
            this.TransparencyCB.Text = "Transparent Borders";
            this.TransparencyCB.UseVisualStyleBackColor = true;
            this.TransparencyCB.CheckedChanged += new EventHandler(this.TransparencyCB_CheckedChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(2, 0x37);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Image";
            this.TransparencyLabel.AutoSize = true;
            this.TransparencyLabel.Location = new Point(3, 250);
            this.TransparencyLabel.Name = "TransparencyLabel";
            this.TransparencyLabel.Size = new Size(0x48, 13);
            this.TransparencyLabel.TabIndex = 6;
            this.TransparencyLabel.Text = "Transparency";
            int[] bits = new int[4];
            bits[0] = 5;
            this.transparencyUpDown.Increment = new decimal(bits);
            this.transparencyUpDown.Location = new Point(0x4f, 0xf7);
            this.transparencyUpDown.Name = "transparencyUpDown";
            this.transparencyUpDown.Size = new Size(0x2b, 20);
            this.transparencyUpDown.TabIndex = 9;
            int[] numArray2 = new int[4];
            numArray2[0] = 100;
            this.transparencyUpDown.Value = new decimal(numArray2);
            this.transparencyUpDown.ValueChanged += new EventHandler(this.transparencyUpDown_ValueChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.transparencyUpDown);
            base.Controls.Add(this.TransparencyLabel);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.TransparencyCB);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.ImagePathTextBox);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.BrowseBtn);
            base.Name = "BitmapSettings";
            base.Size = new Size(0x129, 0x116);
            ((ISupportInitialize) this.pictureBox1).EndInit();
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

        private void TransparencyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.TransparencyCB.Checked)
            {
                this.decimal_0 = this.transparencyUpDown.Value;
                this.transparencyUpDown.Value = 100M;
            }
            else
            {
                if (this.decimal_0 > 75M)
                {
                    this.decimal_0 = 0M;
                }
                this.transparencyUpDown.Value = this.decimal_0;
            }
        }

        private void transparencyUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this.transparencyUpDown.Value != 100M)
            {
                this.TransparencyCB.CheckedChanged -= new EventHandler(this.TransparencyCB_CheckedChanged);
                this.TransparencyCB.Checked = false;
                this.TransparencyCB.CheckedChanged += new EventHandler(this.TransparencyCB_CheckedChanged);
            }
        }

        public string ImagePath
        {
            get
            {
                return this.ImagePathTextBox.Text;
            }
            set
            {
                this.pictureBox1.Image = Image.FromFile(value);
                this.ImagePathTextBox.Text = value;
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

        public bool Transparent
        {
            get
            {
                return this.TransparencyCB.Checked;
            }
            set
            {
                this.TransparencyCB.Checked = value;
            }
        }
    }
}

