namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GLEditor : Form
    {
        private ButtonColor BColor;
        private ComboBox cBDrawStyle;
        private CheckBox cBFixedPos;
        private CheckBox cBLightVisible;
        private Chart chart;
        private CheckBox checkActive;
        private CheckBox checkFontOutline;
        private CheckBox checkShading;
        private Container components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private System.Windows.Forms.Panel lightPanel;
        private NumericUpDown numUpDownExtr;
        private GroupBox Options;
        private Control parent;
        private TabControl tabControlLights;
        private TabPage tabLight0;
        private TabPage tabLight1;
        private TabPage tabLight2;
        private TrackBar tBAmbient;
        private TrackBar tBShininess;
        private TrackBar trackBarIntensity;
        private TrackBar trackBarX;
        private TrackBar trackBarY;
        private TrackBar trackBarZ;

        public GLEditor()
        {
            this.InitializeComponent();
        }

        public GLEditor(Chart c, Control p) : this()
        {
            this.chart = c;
            this.parent = p;
            if (((this.parent != null) && (this.chart != null)) && (TeeBase.TeeOpenGL != null))
            {
                TeeBase.TeeOpenGL.SetChart(this.chart);
                this.GLShowEditor();
                EditorUtils.InsertForm(this, this.parent);
            }
        }

        private void BColor_Click(object sender, EventArgs e)
        {
            TeeBase.TeeGLLight.Color = this.BColor.Color;
        }

        private void cBDrawStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            TeeBase.TeeOpenGL.DrawStyle = (SurfaceStyle) this.cBDrawStyle.SelectedIndex;
        }

        private void cBFixedPos_CheckedChanged(object sender, EventArgs e)
        {
            TeeBase.TeeGLLight.FixedPosition = this.cBFixedPos.Checked;
        }

        private void cBLightVisible_CheckedChanged(object sender, EventArgs e)
        {
            TeeBase.TeeGLLight.Visible = this.cBLightVisible.Checked;
        }

        private void checkActive_CheckedChanged(object sender, EventArgs e)
        {
            TeeBase.TeeOpenGL.Active = this.checkActive.Checked;
            this.chart.parent.RefreshControl();
        }

        private void checkFontOutline_CheckedChanged(object sender, EventArgs e)
        {
            TeeBase.TeeOpenGL.FontOutlines = this.checkFontOutline.Checked;
            this.chart.parent.RefreshControl();
        }

        private void checkShading_CheckedChanged(object sender, EventArgs e)
        {
            TeeBase.TeeOpenGL.ShadeQuality = this.checkShading.Checked;
            this.chart.parent.RefreshControl();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GetSettings()
        {
            this.cBLightVisible.Checked = TeeBase.TeeGLLight.Visible;
            this.trackBarX.Value = Convert.ToInt32(TeeBase.TeeGLLight.Position.X);
            this.trackBarY.Value = Convert.ToInt32(TeeBase.TeeGLLight.Position.Y);
            this.trackBarZ.Value = Convert.ToInt32(TeeBase.TeeGLLight.Position.Z);
            this.cBFixedPos.Checked = TeeBase.TeeGLLight.FixedPosition;
            this.trackBarIntensity.Value = TeeBase.TeeGLLight.Color.R;
            this.BColor.Color = TeeBase.TeeGLLight.Color;
        }

        private void GLShowEditor()
        {
            this.numUpDownExtr.Value = TeeBase.TeeOpenGL.FontExtrusion;
            this.tBAmbient.Value = TeeBase.TeeOpenGL.AmbientLight;
            this.checkActive.Checked = TeeBase.TeeOpenGL.Active;
            this.checkShading.Checked = TeeBase.TeeOpenGL.ShadeQuality;
            this.checkFontOutline.Checked = TeeBase.TeeOpenGL.FontOutlines;
            this.tBShininess.Value = Convert.ToInt32((double) (TeeBase.TeeOpenGL.Shininess * 100.0));
            this.cBDrawStyle.SelectedIndex = Convert.ToInt32(TeeBase.TeeOpenGL.DrawStyle);
            this.UpdateLightSettings();
        }

        private void InitializeComponent()
        {
            this.tabControlLights = new TabControl();
            this.tabLight0 = new TabPage();
            this.lightPanel = new System.Windows.Forms.Panel();
            this.cBFixedPos = new CheckBox();
            this.cBLightVisible = new CheckBox();
            this.BColor = new ButtonColor();
            this.label8 = new Label();
            this.trackBarZ = new TrackBar();
            this.label7 = new Label();
            this.trackBarY = new TrackBar();
            this.label6 = new Label();
            this.trackBarX = new TrackBar();
            this.label5 = new Label();
            this.trackBarIntensity = new TrackBar();
            this.tabLight1 = new TabPage();
            this.tabLight2 = new TabPage();
            this.checkActive = new CheckBox();
            this.Options = new GroupBox();
            this.label4 = new Label();
            this.tBShininess = new TrackBar();
            this.label3 = new Label();
            this.cBDrawStyle = new ComboBox();
            this.label2 = new Label();
            this.numUpDownExtr = new NumericUpDown();
            this.checkFontOutline = new CheckBox();
            this.checkShading = new CheckBox();
            this.label1 = new Label();
            this.tBAmbient = new TrackBar();
            this.tabControlLights.SuspendLayout();
            this.tabLight0.SuspendLayout();
            this.lightPanel.SuspendLayout();
            this.trackBarZ.BeginInit();
            this.trackBarY.BeginInit();
            this.trackBarX.BeginInit();
            this.trackBarIntensity.BeginInit();
            this.Options.SuspendLayout();
            this.tBShininess.BeginInit();
            this.numUpDownExtr.BeginInit();
            this.tBAmbient.BeginInit();
            base.SuspendLayout();
            this.tabControlLights.Controls.Add(this.tabLight0);
            this.tabControlLights.Controls.Add(this.tabLight1);
            this.tabControlLights.Controls.Add(this.tabLight2);
            this.tabControlLights.Location = new Point(0xbc, 0x12);
            this.tabControlLights.Name = "tabControlLights";
            this.tabControlLights.SelectedIndex = 0;
            this.tabControlLights.Size = new Size(0xaf, 0xc6);
            this.tabControlLights.TabIndex = 0;
            this.tabControlLights.SelectedIndexChanged += new EventHandler(this.tabControlLights_SelectedIndexChanged);
            this.tabLight0.Controls.Add(this.lightPanel);
            this.tabLight0.Location = new Point(4, 0x16);
            this.tabLight0.Name = "tabLight0";
            this.tabLight0.Size = new Size(0x9d, 0xac);
            this.tabLight0.TabIndex = 0;
            this.tabLight0.Text = "Light0";
            this.lightPanel.Controls.Add(this.cBFixedPos);
            this.lightPanel.Controls.Add(this.cBLightVisible);
            this.lightPanel.Controls.Add(this.BColor);
            this.lightPanel.Controls.Add(this.label8);
            this.lightPanel.Controls.Add(this.trackBarZ);
            this.lightPanel.Controls.Add(this.label7);
            this.lightPanel.Controls.Add(this.trackBarY);
            this.lightPanel.Controls.Add(this.label6);
            this.lightPanel.Controls.Add(this.trackBarX);
            this.lightPanel.Controls.Add(this.label5);
            this.lightPanel.Controls.Add(this.trackBarIntensity);
            this.lightPanel.Dock = DockStyle.Fill;
            this.lightPanel.Location = new Point(0, 0);
            this.lightPanel.Name = "lightPanel";
            this.lightPanel.Size = new Size(0x9d, 0xac);
            this.lightPanel.TabIndex = 1;
            this.cBFixedPos.Location = new Point(5, 0x94);
            this.cBFixedPos.Name = "cBFixedPos";
            this.cBFixedPos.Size = new Size(0x93, 0x10);
            this.cBFixedPos.TabIndex = 0x17;
            this.cBFixedPos.Text = "&Fixed Position";
            this.cBFixedPos.CheckedChanged += new EventHandler(this.cBFixedPos_CheckedChanged);
            this.cBLightVisible.Location = new Point(5, 10);
            this.cBLightVisible.Name = "cBLightVisible";
            this.cBLightVisible.Size = new Size(0x40, 0x10);
            this.cBLightVisible.TabIndex = 0x16;
            this.cBLightVisible.Text = "&Visible";
            this.cBLightVisible.CheckedChanged += new EventHandler(this.cBLightVisible_CheckedChanged);
            this.BColor.Color = Color.Empty;
            this.BColor.Location = new Point(0x4f, 6);
            this.BColor.Name = "BColor";
            this.BColor.Size = new Size(0x4b, 0x18);
            this.BColor.TabIndex = 0x15;
            this.BColor.Text = "&Color";
            this.BColor.Click += new EventHandler(this.BColor_Click);
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x30, 0x75);
            this.label8.Name = "label8";
            this.label8.Size = new Size(14, 0x10);
            this.label8.TabIndex = 20;
            this.label8.Text = "&Z:";
            this.label8.TextAlign = ContentAlignment.TopRight;
            this.trackBarZ.AutoSize = false;
            this.trackBarZ.LargeChange = 20;
            this.trackBarZ.Location = new Point(0x41, 0x75);
            this.trackBarZ.Maximum = 500;
            this.trackBarZ.Minimum = -500;
            this.trackBarZ.Name = "trackBarZ";
            this.trackBarZ.Size = new Size(0x56, 20);
            this.trackBarZ.TabIndex = 0x13;
            this.trackBarZ.TickStyle = TickStyle.None;
            this.trackBarZ.ValueChanged += new EventHandler(this.trackBarZ_ValueChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x30, 0x5b);
            this.label7.Name = "label7";
            this.label7.Size = new Size(15, 0x10);
            this.label7.TabIndex = 0x12;
            this.label7.Text = "&Y:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.trackBarY.AutoSize = false;
            this.trackBarY.LargeChange = 20;
            this.trackBarY.Location = new Point(0x41, 0x5b);
            this.trackBarY.Maximum = 500;
            this.trackBarY.Minimum = -500;
            this.trackBarY.Name = "trackBarY";
            this.trackBarY.Size = new Size(0x56, 20);
            this.trackBarY.TabIndex = 0x11;
            this.trackBarY.TickStyle = TickStyle.None;
            this.trackBarY.ValueChanged += new EventHandler(this.trackBarY_ValueChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x30, 0x43);
            this.label6.Name = "label6";
            this.label6.Size = new Size(15, 0x10);
            this.label6.TabIndex = 0x10;
            this.label6.Text = "&X:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.trackBarX.AutoSize = false;
            this.trackBarX.LargeChange = 20;
            this.trackBarX.Location = new Point(0x41, 0x42);
            this.trackBarX.Maximum = 500;
            this.trackBarX.Minimum = -500;
            this.trackBarX.Name = "trackBarX";
            this.trackBarX.Size = new Size(0x56, 20);
            this.trackBarX.TabIndex = 15;
            this.trackBarX.TickStyle = TickStyle.None;
            this.trackBarX.ValueChanged += new EventHandler(this.trackBarX_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(14, 40);
            this.label5.Name = "label5";
            this.label5.Size = new Size(50, 0x10);
            this.label5.TabIndex = 14;
            this.label5.Text = "&Intensity:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.trackBarIntensity.AutoSize = false;
            this.trackBarIntensity.Location = new Point(0x41, 40);
            this.trackBarIntensity.Maximum = 0xff;
            this.trackBarIntensity.Name = "trackBarIntensity";
            this.trackBarIntensity.Size = new Size(0x56, 20);
            this.trackBarIntensity.TabIndex = 13;
            this.trackBarIntensity.TickStyle = TickStyle.None;
            this.trackBarIntensity.ValueChanged += new EventHandler(this.trackBarIntensity_ValueChanged);
            this.tabLight1.Location = new Point(4, 0x16);
            this.tabLight1.Name = "tabLight1";
            this.tabLight1.Size = new Size(0x9d, 0xac);
            this.tabLight1.TabIndex = 1;
            this.tabLight1.Text = "Light1";
            this.tabLight2.Location = new Point(4, 0x16);
            this.tabLight2.Name = "tabLight2";
            this.tabLight2.Size = new Size(0x9d, 0xac);
            this.tabLight2.TabIndex = 2;
            this.tabLight2.Text = "Light2";
            this.checkActive.Location = new Point(5, 9);
            this.checkActive.Name = "checkActive";
            this.checkActive.Size = new Size(0x83, 0x10);
            this.checkActive.TabIndex = 1;
            this.checkActive.Text = "&Active";
            this.checkActive.CheckedChanged += new EventHandler(this.checkActive_CheckedChanged);
            this.Options.Controls.Add(this.label4);
            this.Options.Controls.Add(this.tBShininess);
            this.Options.Controls.Add(this.label3);
            this.Options.Controls.Add(this.cBDrawStyle);
            this.Options.Controls.Add(this.label2);
            this.Options.Controls.Add(this.numUpDownExtr);
            this.Options.Controls.Add(this.checkFontOutline);
            this.Options.Controls.Add(this.checkShading);
            this.Options.Controls.Add(this.label1);
            this.Options.Controls.Add(this.tBAmbient);
            this.Options.Location = new Point(5, 0x1c);
            this.Options.Name = "Options";
            this.Options.Size = new Size(0xb3, 0xbc);
            this.Options.TabIndex = 2;
            this.Options.TabStop = false;
            this.Options.Text = "Options";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x18, 0x9e);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x39, 0x10);
            this.label4.TabIndex = 9;
            this.label4.Text = "S&hininess:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.tBShininess.AutoSize = false;
            this.tBShininess.LargeChange = 1;
            this.tBShininess.Location = new Point(0x58, 0x9f);
            this.tBShininess.Maximum = 100;
            this.tBShininess.Name = "tBShininess";
            this.tBShininess.Size = new Size(0x56, 20);
            this.tBShininess.TabIndex = 8;
            this.tBShininess.TickStyle = TickStyle.None;
            this.tBShininess.Value = 10;
            this.tBShininess.ValueChanged += new EventHandler(this.tBShininess_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x16, 0x7e);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x3e, 0x10);
            this.label3.TabIndex = 7;
            this.label3.Text = "&Draw Style:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.cBDrawStyle.Items.AddRange(new object[] { "Solid", "Wire", "Dot" });
            this.cBDrawStyle.Location = new Point(0x57, 0x7b);
            this.cBDrawStyle.Name = "cBDrawStyle";
            this.cBDrawStyle.Size = new Size(80, 0x15);
            this.cBDrawStyle.TabIndex = 6;
            this.cBDrawStyle.Text = "comboBox1";
            this.cBDrawStyle.SelectedIndexChanged += new EventHandler(this.cBDrawStyle_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(40, 100);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x51, 0x10);
            this.label2.TabIndex = 5;
            this.label2.Text = "Font &3D Depth:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.numUpDownExtr.Location = new Point(0x7f, 0x62);
            this.numUpDownExtr.Name = "numUpDownExtr";
            this.numUpDownExtr.Size = new Size(40, 20);
            this.numUpDownExtr.TabIndex = 4;
            this.numUpDownExtr.KeyUp += new KeyEventHandler(this.numUpDownExtr_KeyUp);
            this.numUpDownExtr.ValueChanged += new EventHandler(this.numUpDownExtr_ValueChanged);
            this.checkFontOutline.Location = new Point(11, 0x4a);
            this.checkFontOutline.Name = "checkFontOutline";
            this.checkFontOutline.Size = new Size(0x9d, 0x10);
            this.checkFontOutline.TabIndex = 3;
            this.checkFontOutline.Text = "Font &Outlines";
            this.checkFontOutline.CheckedChanged += new EventHandler(this.checkFontOutline_CheckedChanged);
            this.checkShading.Location = new Point(11, 50);
            this.checkShading.Name = "checkShading";
            this.checkShading.Size = new Size(0x95, 0x10);
            this.checkShading.TabIndex = 2;
            this.checkShading.Text = "&Smooth shading";
            this.checkShading.CheckedChanged += new EventHandler(this.checkShading_CheckedChanged);
            this.label1.Location = new Point(9, 0x16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(80, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "A&mbient light:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.tBAmbient.AutoSize = false;
            this.tBAmbient.Location = new Point(0x58, 0x16);
            this.tBAmbient.Maximum = 100;
            this.tBAmbient.Name = "tBAmbient";
            this.tBAmbient.Size = new Size(0x56, 20);
            this.tBAmbient.TabIndex = 0;
            this.tBAmbient.TickStyle = TickStyle.None;
            this.tBAmbient.Value = 8;
            this.tBAmbient.ValueChanged += new EventHandler(this.tBAmbient_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x170, 0xe5);
            base.Controls.Add(this.Options);
            base.Controls.Add(this.checkActive);
            base.Controls.Add(this.tabControlLights);
            base.Name = "GLEditor";
            this.Text = "GLEditor";
            this.tabControlLights.ResumeLayout(false);
            this.tabLight0.ResumeLayout(false);
            this.lightPanel.ResumeLayout(false);
            this.trackBarZ.EndInit();
            this.trackBarY.EndInit();
            this.trackBarX.EndInit();
            this.trackBarIntensity.EndInit();
            this.Options.ResumeLayout(false);
            this.tBShininess.EndInit();
            this.numUpDownExtr.EndInit();
            this.tBAmbient.EndInit();
            base.ResumeLayout(false);
        }

        private void numUpDownExtr_KeyUp(object sender, KeyEventArgs e)
        {
            TeeBase.TeeOpenGL.FontExtrusion = (int) this.numUpDownExtr.Value;
        }

        private void numUpDownExtr_ValueChanged(object sender, EventArgs e)
        {
            TeeBase.TeeOpenGL.FontExtrusion = (int) this.numUpDownExtr.Value;
        }

        private void tabControlLights_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateLightSettings();
            this.lightPanel.Parent = this.tabControlLights.SelectedTab;
        }

        private void tBAmbient_ValueChanged(object sender, EventArgs e)
        {
            TeeBase.TeeOpenGL.AmbientLight = this.tBAmbient.Value;
            this.chart.parent.RefreshControl();
        }

        private void tBShininess_ValueChanged(object sender, EventArgs e)
        {
            TeeBase.TeeOpenGL.Shininess = ((double) this.tBShininess.Value) / 100.0;
            this.chart.parent.RefreshControl();
        }

        private void trackBarIntensity_ValueChanged(object sender, EventArgs e)
        {
            TeeBase.TeeGLLight.Color = Color.FromArgb(this.trackBarIntensity.Value, this.trackBarIntensity.Value, this.trackBarIntensity.Value);
            this.BColor.Color = TeeBase.TeeGLLight.Color;
        }

        private void trackBarX_ValueChanged(object sender, EventArgs e)
        {
            TeeBase.TeeGLLight.Position.X = this.trackBarX.Value;
        }

        private void trackBarY_ValueChanged(object sender, EventArgs e)
        {
            TeeBase.TeeGLLight.Position.Y = this.trackBarY.Value;
        }

        private void trackBarZ_ValueChanged(object sender, EventArgs e)
        {
            TeeBase.TeeGLLight.Position.Z = this.trackBarZ.Value;
        }

        private void UpdateLightSettings()
        {
            switch (this.tabControlLights.SelectedIndex)
            {
                case 0:
                    TeeBase.TeeGLLight = TeeBase.TeeOpenGL.Light0;
                    this.GetSettings();
                    return;

                case 1:
                    TeeBase.TeeGLLight = TeeBase.TeeOpenGL.Light1;
                    this.GetSettings();
                    return;

                case 2:
                    TeeBase.TeeGLLight = TeeBase.TeeOpenGL.Light2;
                    this.GetSettings();
                    return;
            }
        }
    }
}

