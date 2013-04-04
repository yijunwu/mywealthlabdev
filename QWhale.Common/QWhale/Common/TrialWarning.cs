namespace QWhale.Common
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class TrialWarning : Form
    {
        private Button btOk;
        private Container components;
        private Label laAdress;
        private Label label2;
        private Label label4;
        private Label laMailTo;
        private Label laTrialWarning;
        private Label lblDaysLeft;
        private Label lblDaysLeftCaption;
        private PictureBox pictureBox1;

        public TrialWarning()
        {
            this.InitializeComponent();
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
            ResourceManager manager = new ResourceManager(typeof(TrialWarning));
            this.btOk = new Button();
            this.pictureBox1 = new PictureBox();
            this.laTrialWarning = new Label();
            this.laMailTo = new Label();
            this.laAdress = new Label();
            this.label4 = new Label();
            this.label2 = new Label();
            this.lblDaysLeftCaption = new Label();
            this.lblDaysLeft = new Label();
            base.SuspendLayout();
            this.btOk.DialogResult = DialogResult.OK;
            this.btOk.FlatStyle = FlatStyle.Flat;
            this.btOk.Location = new Point(0x130, 0x90);
            this.btOk.Name = "btOk";
            this.btOk.TabIndex = 1;
            this.btOk.Text = "Ok";
            this.pictureBox1.Image = (Image) manager.GetObject("pictureBox1.Image");
            this.pictureBox1.Location = new Point(8, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(180, 80);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.laTrialWarning.Location = new Point(8, 0x60);
            this.laTrialWarning.Name = "laTrialWarning";
            this.laTrialWarning.Size = new Size(0x178, 0x20);
            this.laTrialWarning.TabIndex = 3;
            this.laTrialWarning.Text = "This application uses unregistered version of Quantum Whale Editor.NET Please register. ";
            this.laMailTo.AutoSize = true;
            this.laMailTo.Cursor = Cursors.Hand;
            this.laMailTo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline, GraphicsUnit.Point, 0xcc);
            this.laMailTo.ForeColor = Color.Blue;
            this.laMailTo.Location = new Point(0xe8, 0x40);
            this.laMailTo.Name = "laMailTo";
            this.laMailTo.Size = new Size(0x8d, 0x10);
            this.laMailTo.TabIndex = 9;
            this.laMailTo.Text = "mailto:contact@qwhale.net";
            this.laMailTo.Click += new EventHandler(this.laMailTo_Click);
            this.laAdress.AutoSize = true;
            this.laAdress.Cursor = Cursors.Hand;
            this.laAdress.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline, GraphicsUnit.Point, 0xcc);
            this.laAdress.ForeColor = Color.Blue;
            this.laAdress.Location = new Point(0xe8, 40);
            this.laAdress.Name = "laAdress";
            this.laAdress.Size = new Size(0x73, 0x10);
            this.laAdress.TabIndex = 7;
            this.laAdress.Text = "http://www.qwhale.net";
            this.laAdress.Click += new EventHandler(this.laAdress_Click);
            this.label4.AutoSize = true;
            this.label4.BackColor = Color.White;
            this.label4.Location = new Point(0xc0, 0x40);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x26, 0x10);
            this.label4.TabIndex = 8;
            this.label4.Text = "e-mail:";
            this.label2.AutoSize = true;
            this.label2.BackColor = Color.White;
            this.label2.Location = new Point(0xc0, 40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x20, 0x10);
            this.label2.TabIndex = 6;
            this.label2.Text = "www:";
            this.lblDaysLeftCaption.AutoSize = true;
            this.lblDaysLeftCaption.Location = new Point(8, 0x88);
            this.lblDaysLeftCaption.Name = "lblDaysLeftCaption";
            this.lblDaysLeftCaption.Size = new Size(0x33, 0x10);
            this.lblDaysLeftCaption.TabIndex = 10;
            this.lblDaysLeftCaption.Text = "Days left:";
            this.lblDaysLeft.AutoSize = true;
            this.lblDaysLeft.Location = new Point(0x38, 0x88);
            this.lblDaysLeft.Name = "lblDaysLeft";
            this.lblDaysLeft.Size = new Size(15, 0x10);
            this.lblDaysLeft.TabIndex = 11;
            this.lblDaysLeft.Text = "xx";
            this.AutoScaleBaseSize = new Size(5, 13);
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x188, 0xae);
            base.Controls.Add(this.lblDaysLeft);
            base.Controls.Add(this.lblDaysLeftCaption);
            base.Controls.Add(this.laMailTo);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.laAdress);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.laTrialWarning);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.btOk);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "TrialWarning";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Unregistered version";
            base.ResumeLayout(false);
        }

        private void laAdress_Click(object sender, EventArgs e)
        {
            this.laAdress.ForeColor = Color.Purple;
            try
            {
                Process.Start(this.laAdress.Text);
            }
            catch
            {
            }
        }

        private void laMailTo_Click(object sender, EventArgs e)
        {
            this.laMailTo.ForeColor = Color.Purple;
            try
            {
                Process.Start(this.laMailTo.Text);
            }
            catch
            {
            }
        }

        public void ShowTrialDialog(int DaysLeft)
        {
            if (DaysLeft > 0)
            {
                this.lblDaysLeft.Text = DaysLeft.ToString();
                this.lblDaysLeft.Left = this.lblDaysLeftCaption.Right + 4;
            }
            else
            {
                this.lblDaysLeft.Text = "EXPIRED";
                this.lblDaysLeft.Font = new Font(this.lblDaysLeft.Font.FontFamily, this.lblDaysLeft.Font.Size, FontStyle.Bold);
            }
            base.ShowDialog();
        }
    }
}

