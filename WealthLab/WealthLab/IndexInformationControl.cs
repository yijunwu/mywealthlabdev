namespace WealthLab
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Label = System.Windows.Forms.Label;

    public class IndexInformationControl : UserControl
    {
        private IContainer icontainer_0;
        private Label label1;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label lblDataSet;
        private TextBox txtIndexDataSet;
        private TextBox txtIndexDef;
        private TextBox txtIndexName;
        private TextBox txtSourceDataSet;

        public IndexInformationControl()
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

        public void Fillup(CustomIndex index)
        {
            this.txtIndexDataSet.Text = index.DataSourceParentName;
            this.txtSourceDataSet.Text = index.DataSourceName;
            this.txtIndexDef.Text = index.IndexDefinition.FriendlyName;
            this.txtIndexName.Text = index.Symbol;
        }

        private void InitializeComponent()
        {
            this.label6 = new Label();
            this.txtIndexDef = new TextBox();
            this.label5 = new Label();
            this.txtIndexName = new TextBox();
            this.label4 = new Label();
            this.lblDataSet = new Label();
            this.txtIndexDataSet = new TextBox();
            this.txtSourceDataSet = new TextBox();
            this.label1 = new Label();
            base.SuspendLayout();
            this.label6.AutoSize = true;
            this.label6.Location = new Point(3, 0x6a);
            this.label6.Name = "label6";
            this.label6.Size = new Size(80, 13);
            this.label6.TabIndex = 0x25;
            this.label6.Text = "Index Definition";
            this.txtIndexDef.Location = new Point(0x79, 0x6a);
            this.txtIndexDef.Name = "txtIndexDef";
            this.txtIndexDef.ReadOnly = true;
            this.txtIndexDef.Size = new Size(0xe1, 20);
            this.txtIndexDef.TabIndex = 0x24;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(3, 150);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x43, 13);
            this.label5.TabIndex = 0x23;
            this.label5.Text = "Index  Name";
            this.txtIndexName.CharacterCasing = CharacterCasing.Upper;
            this.txtIndexName.Location = new Point(0x79, 0x93);
            this.txtIndexName.Name = "txtIndexName";
            this.txtIndexName.Size = new Size(0xe1, 20);
            this.txtIndexName.TabIndex = 0x22;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(2, 0x44);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x70, 13);
            this.label4.TabIndex = 0x21;
            this.label4.Text = "Source Dataset Name";
            this.lblDataSet.AutoSize = true;
            this.lblDataSet.Location = new Point(3, 30);
            this.lblDataSet.Name = "lblDataSet";
            this.lblDataSet.Size = new Size(0x76, 13);
            this.lblDataSet.TabIndex = 0x20;
            this.lblDataSet.Text = "Index Dataset Name (s)";
            this.txtIndexDataSet.Location = new Point(0x79, 30);
            this.txtIndexDataSet.Name = "txtIndexDataSet";
            this.txtIndexDataSet.ReadOnly = true;
            this.txtIndexDataSet.Size = new Size(0xe1, 20);
            this.txtIndexDataSet.TabIndex = 0x1f;
            this.txtSourceDataSet.Location = new Point(0x79, 0x44);
            this.txtSourceDataSet.Name = "txtSourceDataSet";
            this.txtSourceDataSet.ReadOnly = true;
            this.txtSourceDataSet.Size = new Size(0xe1, 20);
            this.txtSourceDataSet.TabIndex = 0x26;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x66, 150);
            this.label1.Name = "label1";
            this.label1.Size = new Size(15, 13);
            this.label1.TabIndex = 0x27;
            this.label1.Text = "%";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.Controls.Add(this.label1);
            base.Controls.Add(this.txtSourceDataSet);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.txtIndexDef);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.txtIndexName);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.lblDataSet);
            base.Controls.Add(this.txtIndexDataSet);
            base.Name = "IndexInformationControl";
            base.Size = new Size(0x15f, 0xb8);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        internal bool method_0(CustomIndex customIndex_0)
        {
            string str = this.txtIndexName.Text.Trim();
            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("Index name cannot be empty.", "Error");
                return false;
            }
            if (str.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                MessageBox.Show("The index name contains some invalid character.", "Error");
                return false;
            }
            customIndex_0.Symbol = this.txtIndexName.Text;
            return true;
        }
    }
}

