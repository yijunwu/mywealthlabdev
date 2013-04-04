namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class LightToolEditor : Form
    {
        private ComboBox CBStyle;
        private CheckBox checkBox1;
        private Container components;
        private HScrollBar hSBFactor;
        private HScrollBar hSBLeft;
        private HScrollBar hSBTop;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private LightTool lightTool;

        public LightToolEditor()
        {
            this.InitializeComponent();
        }

        public LightToolEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.lightTool = (LightTool) t;
            this.checkBox1.Checked = this.lightTool.FollowMouse;
            this.hSBLeft.Maximum = this.lightTool.Chart.Width;
            this.hSBTop.Maximum = this.lightTool.Chart.Height;
            if (this.lightTool.Left == -1)
            {
                this.hSBLeft.Value = this.lightTool.Chart.Width / 2;
            }
            else
            {
                this.hSBLeft.Value = this.lightTool.Left;
            }
            if (this.lightTool.Top == -1)
            {
                this.hSBTop.Value = this.lightTool.Chart.Height / 2;
            }
            else
            {
                this.hSBTop.Value = this.lightTool.Top;
            }
            this.hSBFactor.Value = (int) this.lightTool.Factor;
            if (this.lightTool.Style == LightStyle.Linear)
            {
                this.CBStyle.SelectedIndex = 0;
            }
            else
            {
                this.CBStyle.SelectedIndex = 1;
            }
        }

        private void CBStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CBStyle.SelectedIndex == 0)
            {
                this.lightTool.Style = LightStyle.Linear;
            }
            else
            {
                this.lightTool.Style = LightStyle.SpotLight;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            this.lightTool.FollowMouse = this.checkBox1.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void hSBFactor_Scroll(object sender, ScrollEventArgs e)
        {
            this.lightTool.Factor = this.hSBFactor.Value;
        }

        private void hSBLeft_Scroll(object sender, ScrollEventArgs e)
        {
            this.lightTool.Left = this.hSBLeft.Value;
        }

        private void hSBTop_Scroll(object sender, ScrollEventArgs e)
        {
            this.lightTool.Top = this.hSBTop.Value;
        }

        private void InitializeComponent()
        {
            this.checkBox1 = new CheckBox();
            this.CBStyle = new ComboBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.hSBLeft = new HScrollBar();
            this.hSBTop = new HScrollBar();
            this.hSBFactor = new HScrollBar();
            base.SuspendLayout();
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.Location = new Point(0x38, 0x10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Follow Mouse";
            this.checkBox1.Click += new EventHandler(this.checkBox1_Click);
            this.CBStyle.Items.AddRange(new object[] { "Linear", "SpotLight" });
            this.CBStyle.Location = new Point(0x58, 0x38);
            this.CBStyle.Name = "CBStyle";
            this.CBStyle.Size = new Size(0x68, 0x15);
            this.CBStyle.TabIndex = 1;
            this.CBStyle.SelectedIndexChanged += new EventHandler(this.CBStyle_SelectedIndexChanged);
            this.label1.Location = new Point(40, 0x3b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 0x11);
            this.label1.TabIndex = 2;
            this.label1.Text = "Style:";
            this.label2.Location = new Point(0x29, 0x57);
            this.label2.Name = "label2";
            this.label2.Size = new Size(40, 0x11);
            this.label2.TabIndex = 3;
            this.label2.Text = "Left:";
            this.label3.Location = new Point(0x29, 0x6f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(40, 0x11);
            this.label3.TabIndex = 4;
            this.label3.Text = "Top:";
            this.label4.Location = new Point(0x29, 0x8b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(40, 0x11);
            this.label4.TabIndex = 5;
            this.label4.Text = "Factor:";
            this.hSBLeft.Location = new Point(0x58, 0x58);
            this.hSBLeft.Name = "hSBLeft";
            this.hSBLeft.Size = new Size(0x66, 0x10);
            this.hSBLeft.TabIndex = 6;
            this.hSBLeft.Scroll += new ScrollEventHandler(this.hSBLeft_Scroll);
            this.hSBTop.Location = new Point(0x58, 110);
            this.hSBTop.Name = "hSBTop";
            this.hSBTop.Size = new Size(0x66, 0x10);
            this.hSBTop.TabIndex = 7;
            this.hSBTop.Scroll += new ScrollEventHandler(this.hSBTop_Scroll);
            this.hSBFactor.Location = new Point(0x58, 0x8b);
            this.hSBFactor.Name = "hSBFactor";
            this.hSBFactor.Size = new Size(0x66, 0x10);
            this.hSBFactor.TabIndex = 8;
            this.hSBFactor.Scroll += new ScrollEventHandler(this.hSBFactor_Scroll);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(200, 0xa6);
            base.Controls.Add(this.hSBFactor);
            base.Controls.Add(this.hSBTop);
            base.Controls.Add(this.hSBLeft);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.CBStyle);
            base.Controls.Add(this.checkBox1);
            base.Name = "LightToolEditor";
            base.ResumeLayout(false);
        }
    }
}

