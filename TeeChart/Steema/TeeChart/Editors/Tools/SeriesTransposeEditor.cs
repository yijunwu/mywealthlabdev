namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SeriesTransposeEditor : Form
    {
        private Button btnTranspose;
        private IContainer components;
        private SeriesTranspose tool;

        public SeriesTransposeEditor()
        {
            this.InitializeComponent();
        }

        public SeriesTransposeEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.tool = (SeriesTranspose) s;
        }

        private void btnTranspose_Click(object sender, EventArgs e)
        {
            if (this.tool != null)
            {
                this.tool.Transpose();
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

        private void InitializeComponent()
        {
            this.btnTranspose = new Button();
            base.SuspendLayout();
            this.btnTranspose.FlatStyle = FlatStyle.Flat;
            this.btnTranspose.Location = new Point(12, 12);
            this.btnTranspose.Name = "btnTranspose";
            this.btnTranspose.Size = new Size(0x4b, 0x17);
            this.btnTranspose.TabIndex = 0;
            this.btnTranspose.Text = "Transpose";
            this.btnTranspose.UseVisualStyleBackColor = true;
            this.btnTranspose.Click += new EventHandler(this.btnTranspose_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x77, 50);
            base.Controls.Add(this.btnTranspose);
            base.Name = "SeriesTransposeEditor";
            this.Text = "SeriesTransposeEditor";
            base.ResumeLayout(false);
        }
    }
}

