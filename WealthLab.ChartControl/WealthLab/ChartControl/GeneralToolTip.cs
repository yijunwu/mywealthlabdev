namespace WealthLab.ChartControl
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class GeneralToolTip : UserControl
    {
        private IContainer icontainer_0;
        private Label lblText;

        public GeneralToolTip()
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

        private void GeneralToolTip_MouseMove(object sender, MouseEventArgs e)
        {
            base.Visible = false;
        }

        private void InitializeComponent()
        {
            this.lblText = new Label();
            base.SuspendLayout();
            this.lblText.AutoSize = true;
            this.lblText.Location = new Point(3, 3);
            this.lblText.Margin = new Padding(3, 0, 3, 2);
            this.lblText.Name = "lblText";
            this.lblText.Size = new Size(0, 13);
            this.lblText.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = SystemColors.Info;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.lblText);
            this.ForeColor = SystemColors.InfoText;
            base.Name = "GeneralToolTip";
            base.Size = new Size(0x51, 0x13);
            base.MouseMove += new MouseEventHandler(this.GeneralToolTip_MouseMove);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void RenderValue(int objHashCode, string text)
        {
            this.lblText.Text = text;
        }
    }
}

