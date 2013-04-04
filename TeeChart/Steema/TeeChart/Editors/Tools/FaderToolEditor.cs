namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class FaderToolEditor : Form
    {
        private Button bReset;
        private Button bStart;
        private ButtonColor buttonColor1;
        private IContainer components;
        private FaderTool fader;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label lSpeed;
        private RadioButton rbFadeIn;
        private RadioButton rbFadeOut;
        private HScrollBar sbSpeed;
        private NumericUpDown udDelay;

        public FaderToolEditor()
        {
            this.InitializeComponent();
        }

        public FaderToolEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.fader = s as FaderTool;
            if (this.fader != null)
            {
                this.fader.FaderStop += new EventHandler(this.fader_FaderStop);
                this.sbSpeed.Value = Utils.Round((double) (this.fader.Speed * 10.0));
                this.buttonColor1.Color = this.fader.Color;
                switch (this.fader.Style)
                {
                    case FaderStyle.FadeIn:
                        this.rbFadeIn.Checked = true;
                        break;

                    case FaderStyle.FadeOut:
                        this.rbFadeOut.Checked = true;
                        break;
                }
                this.udDelay.Value = this.fader.InitialDelay;
                this.SetLabelSpeed();
            }
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            this.fader.Reset();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            this.bStart.Enabled = false;
            this.bReset.Enabled = false;
            this.fader.Start();
        }

        private void buttonColor1_Click(object sender, EventArgs e)
        {
            this.fader.Color = this.buttonColor1.Color;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void fader_FaderStop(object sender, EventArgs e)
        {
            this.SetEnabled(true);
        }

        private void FaderToolEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.fader != null)
            {
                this.fader.FaderStop -= new EventHandler(this.fader_FaderStop);
            }
        }

        private void InitializeComponent()
        {
            this.bStart = new Button();
            this.bReset = new Button();
            this.label1 = new Label();
            this.sbSpeed = new HScrollBar();
            this.lSpeed = new Label();
            this.label2 = new Label();
            this.udDelay = new NumericUpDown();
            this.groupBox1 = new GroupBox();
            this.rbFadeOut = new RadioButton();
            this.rbFadeIn = new RadioButton();
            this.buttonColor1 = new ButtonColor();
            this.udDelay.BeginInit();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.bStart.FlatStyle = FlatStyle.Flat;
            this.bStart.Location = new Point(12, 12);
            this.bStart.Name = "bStart";
            this.bStart.Size = new Size(0x4b, 0x17);
            this.bStart.TabIndex = 0;
            this.bStart.Text = "&Fade";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new EventHandler(this.bStart_Click);
            this.bReset.FlatStyle = FlatStyle.Flat;
            this.bReset.Location = new Point(0x84, 12);
            this.bReset.Name = "bReset";
            this.bReset.Size = new Size(0x4b, 0x17);
            this.bReset.TabIndex = 1;
            this.bReset.Text = "&Reset";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new EventHandler(this.bReset_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(9, 0x34);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x26, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Speed";
            this.sbSpeed.Location = new Point(9, 0x4a);
            this.sbSpeed.Name = "sbSpeed";
            this.sbSpeed.Size = new Size(0xb6, 0x11);
            this.sbSpeed.TabIndex = 3;
            this.sbSpeed.Scroll += new ScrollEventHandler(this.sbSpeed_Scroll);
            this.lSpeed.AutoSize = true;
            this.lSpeed.Location = new Point(0xc2, 0x4a);
            this.lSpeed.Name = "lSpeed";
            this.lSpeed.Size = new Size(13, 13);
            this.lSpeed.TabIndex = 4;
            this.lSpeed.Text = "0";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 0x90);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3b, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Initial &delay";
            this.udDelay.Location = new Point(12, 160);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.udDelay.Maximum = new decimal(bits);
            this.udDelay.Name = "udDelay";
            this.udDelay.Size = new Size(0x4b, 20);
            this.udDelay.TabIndex = 7;
            this.udDelay.ValueChanged += new EventHandler(this.udDelay_ValueChanged);
            this.groupBox1.Controls.Add(this.rbFadeOut);
            this.groupBox1.Controls.Add(this.rbFadeIn);
            this.groupBox1.Location = new Point(0x63, 0x5e);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x6c, 0x55);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Style";
            this.rbFadeOut.AutoSize = true;
            this.rbFadeOut.Location = new Point(10, 0x30);
            this.rbFadeOut.Name = "rbFadeOut";
            this.rbFadeOut.Size = new Size(0x45, 0x11);
            this.rbFadeOut.TabIndex = 1;
            this.rbFadeOut.TabStop = true;
            this.rbFadeOut.Text = "Fade &Out";
            this.rbFadeOut.UseVisualStyleBackColor = true;
            this.rbFadeOut.CheckedChanged += new EventHandler(this.rbFadeOut_CheckedChanged);
            this.rbFadeIn.AutoSize = true;
            this.rbFadeIn.Location = new Point(10, 0x15);
            this.rbFadeIn.Name = "rbFadeIn";
            this.rbFadeIn.Size = new Size(0x3d, 0x11);
            this.rbFadeIn.TabIndex = 0;
            this.rbFadeIn.TabStop = true;
            this.rbFadeIn.Text = "Fade &In";
            this.rbFadeIn.UseVisualStyleBackColor = true;
            this.rbFadeIn.CheckedChanged += new EventHandler(this.rbFadeIn_CheckedChanged);
            this.buttonColor1.Color = Color.Empty;
            this.buttonColor1.Location = new Point(12, 0x6d);
            this.buttonColor1.Name = "buttonColor1";
            this.buttonColor1.Size = new Size(0x4b, 0x17);
            this.buttonColor1.TabIndex = 5;
            this.buttonColor1.Text = "Color...";
            this.buttonColor1.UseVisualStyleBackColor = true;
            this.buttonColor1.Click += new EventHandler(this.buttonColor1_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0xe3, 0xbf);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.udDelay);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.buttonColor1);
            base.Controls.Add(this.lSpeed);
            base.Controls.Add(this.sbSpeed);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.bReset);
            base.Controls.Add(this.bStart);
            base.Name = "FaderToolEditor";
            this.Text = "FaderToolEditor";
            base.FormClosed += new FormClosedEventHandler(this.FaderToolEditor_FormClosed);
            this.udDelay.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void rbFadeIn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbFadeIn.Checked)
            {
                this.fader.Style = FaderStyle.FadeIn;
            }
            else
            {
                this.fader.Style = FaderStyle.FadeOut;
            }
        }

        private void rbFadeOut_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbFadeOut.Checked)
            {
                this.fader.Style = FaderStyle.FadeOut;
            }
            else
            {
                this.fader.Style = FaderStyle.FadeIn;
            }
        }

        private void sbSpeed_Scroll(object sender, ScrollEventArgs e)
        {
            this.fader.Speed = this.sbSpeed.Value * 0.1;
            this.SetLabelSpeed();
        }

        private void SetEnabled(bool enabled)
        {
            if (this.bStart.InvokeRequired)
            {
                SetEnabledCallback method = new SetEnabledCallback(this.SetEnabled);
                if (!base.IsDisposed)
                {
                    base.Invoke(method, new object[] { enabled });
                }
            }
            else
            {
                this.bStart.Enabled = enabled;
            }
            if (this.bReset.InvokeRequired)
            {
                SetEnabledCallback callback2 = new SetEnabledCallback(this.SetEnabled);
                if (!base.IsDisposed)
                {
                    base.Invoke(callback2, new object[] { enabled });
                }
            }
            else
            {
                this.bReset.Enabled = enabled;
            }
        }

        private void SetLabelSpeed()
        {
            this.lSpeed.Text = this.fader.Speed.ToString("#.0");
        }

        private void udDelay_ValueChanged(object sender, EventArgs e)
        {
            this.fader.InitialDelay = (int) this.udDelay.Value;
        }

        private delegate void SetEnabledCallback(bool enabled);
    }
}

