namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class TextNoteSettings : UserControl
    {
        private Button fontButton;
        private FontDialog fontDialog_0;
        private IContainer icontainer_0;
        private Label label1;
        private RichTextBox richTextBox1;

        public TextNoteSettings()
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
            base.SuspendLayout();
            this.richTextBox1.Location = new Point(3, 0x19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(200, 100);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Enter Text";
            this.fontButton.Location = new Point(5, 0x86);
            this.fontButton.Name = "fontButton";
            this.fontButton.Size = new Size(0x56, 0x17);
            this.fontButton.TabIndex = 14;
            this.fontButton.Text = "Change Font";
            this.fontButton.UseVisualStyleBackColor = true;
            this.fontButton.Click += new EventHandler(this.fontButton_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.fontButton);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.richTextBox1);
            base.Name = "TextNoteSettings";
            base.Size = new Size(0xcf, 0xb5);
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
    }
}

