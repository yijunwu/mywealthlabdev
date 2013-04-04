namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CursorEditor : ToolSeriesEditor
    {
        private ButtonPen button1;
        private CheckBox cbFastCursor;
        private CheckBox cbFollow;
        private ComboBox cbScopeStyle;
        private CheckBox cbSnap;
        private ComboBox cbSnapStyle;
        private ComboBox cbStyle;
        private CheckBox cbUseSeriesZ;
        private IContainer components;
        private CursorTool cursor;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TabControl tabControl1;
        private TabPage tpMouse;
        private TabPage tpSnap;
        private TabPage tpStyle;
        private NumericUpDown udScopeSize;
        private NumericUpDown udVertSize;
        private NumericUpDown upClickTolerance;
        private NumericUpDown upHorizSize;

        public CursorEditor()
        {
            this.InitializeComponent();
        }

        public CursorEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            base.setting = true;
            this.cursor = (CursorTool) s;
            base.SetTool(this.cursor, null);
            switch (this.cursor.Style)
            {
                case CursorToolStyles.Horizontal:
                    this.cbStyle.SelectedIndex = 0;
                    break;

                case CursorToolStyles.Vertical:
                    this.cbStyle.SelectedIndex = 1;
                    break;

                case CursorToolStyles.Both:
                    this.cbStyle.SelectedIndex = 2;
                    break;

                case CursorToolStyles.Scope:
                    this.cbStyle.SelectedIndex = 3;
                    break;

                case CursorToolStyles.ScopeOnly:
                    this.cbStyle.SelectedIndex = 4;
                    break;
            }
            this.cbSnap.Checked = this.cursor.Snap;
            this.cbSnapStyle.Items.Clear();
            this.cbSnapStyle.Items.Add(Enum.GetName(typeof(SnapStyle), SnapStyle.Default));
            this.cbSnapStyle.Items.Add(Enum.GetName(typeof(SnapStyle), SnapStyle.Horizontal));
            this.cbSnapStyle.Items.Add(Enum.GetName(typeof(SnapStyle), SnapStyle.Vertical));
            this.cbSnapStyle.SelectedItem = Enum.GetName(typeof(SnapStyle), this.cursor.SnapStyle);
            this.cbUseSeriesZ.Enabled = this.cursor.Chart.Aspect.View3D;
            this.cbUseSeriesZ.Checked = this.cursor.UseSeriesZ;
            this.cbFastCursor.Checked = this.cursor.FastCursor;
            this.cbFollow.Checked = this.cursor.FollowMouse;
            this.button1.Pen = this.cursor.Pen;
            this.upHorizSize.Value = this.cursor.HorizSize;
            this.udVertSize.Value = this.cursor.VertSize;
            this.udScopeSize.Value = this.cursor.ScopeSize;
            this.cbScopeStyle.Items.Clear();
            this.cbScopeStyle.Items.Add(Enum.GetName(typeof(ScopeCursorStyle), ScopeCursorStyle.Camera));
            this.cbScopeStyle.Items.Add(Enum.GetName(typeof(ScopeCursorStyle), ScopeCursorStyle.Circle));
            this.cbScopeStyle.Items.Add(Enum.GetName(typeof(ScopeCursorStyle), ScopeCursorStyle.Diamond));
            this.cbScopeStyle.Items.Add(Enum.GetName(typeof(ScopeCursorStyle), ScopeCursorStyle.Empty));
            this.cbScopeStyle.Items.Add(Enum.GetName(typeof(ScopeCursorStyle), ScopeCursorStyle.Rectangle));
            this.cbScopeStyle.SelectedItem = Enum.GetName(typeof(ScopeCursorStyle), this.cursor.ScopeStyle);
            this.upClickTolerance.Value = this.cursor.CursorClickTolerance;
            this.EnableSnap();
            this.EnableScope();
            this.EnableFastCursor();
            base.setting = false;
        }

        private void cbFastCursor_Click(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.FastCursor = this.cbFastCursor.Checked;
            }
        }

        private void cbFollow_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.FollowMouse = this.cbFollow.Checked;
                this.EnableFastCursor();
            }
        }

        private void cbScopeStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                switch (this.cbScopeStyle.SelectedIndex)
                {
                    case 0:
                        this.cursor.ScopeStyle = ScopeCursorStyle.Camera;
                        return;

                    case 1:
                        this.cursor.ScopeStyle = ScopeCursorStyle.Circle;
                        return;

                    case 2:
                        this.cursor.ScopeStyle = ScopeCursorStyle.Diamond;
                        return;

                    case 3:
                        this.cursor.ScopeStyle = ScopeCursorStyle.Empty;
                        return;

                    case 4:
                        this.cursor.ScopeStyle = ScopeCursorStyle.Rectangle;
                        break;

                    default:
                        return;
                }
            }
        }

        protected override void CBSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.CBSeries_SelectedIndexChanged(sender, e);
            this.EnableSnap();
            this.cbUseSeriesZ.Enabled = this.cursor.Chart.Aspect.View3D;
        }

        private void cbSnap_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.Snap = this.cbSnap.Checked;
                this.EnableSnap();
            }
        }

        private void cbSnapStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                switch (this.cbSnapStyle.SelectedIndex)
                {
                    case 0:
                        this.cursor.SnapStyle = SnapStyle.Default;
                        return;

                    case 1:
                        this.cursor.SnapStyle = SnapStyle.Horizontal;
                        return;

                    case 2:
                        this.cursor.SnapStyle = SnapStyle.Vertical;
                        break;

                    default:
                        return;
                }
            }
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                switch (this.cbStyle.SelectedIndex)
                {
                    case 0:
                        this.cursor.Style = CursorToolStyles.Horizontal;
                        break;

                    case 1:
                        this.cursor.Style = CursorToolStyles.Vertical;
                        break;

                    case 2:
                        this.cursor.Style = CursorToolStyles.Both;
                        break;

                    case 3:
                        this.cursor.Style = CursorToolStyles.Scope;
                        break;

                    case 4:
                        this.cursor.Style = CursorToolStyles.ScopeOnly;
                        break;
                }
                this.EnableScope();
            }
        }

        private void cbUseSeriesZ_Click(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.UseSeriesZ = this.cbUseSeriesZ.Checked;
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

        private void EnableFastCursor()
        {
            this.cbFastCursor.Enabled = this.cursor.FollowMouse;
        }

        private void EnableScope()
        {
            this.cbScopeStyle.Enabled = (this.cursor.Style == CursorToolStyles.Scope) || (this.cursor.Style == CursorToolStyles.ScopeOnly);
            this.udScopeSize.Enabled = this.cbScopeStyle.Enabled;
            if (!this.cbScopeStyle.Enabled)
            {
                this.cursor.HorizSize = 0;
                this.cursor.VertSize = 0;
                this.udVertSize.Value = 0M;
                this.upHorizSize.Value = 0M;
            }
        }

        private void EnableSnap()
        {
            this.cbSnap.Enabled = this.cursor.Series != null;
            this.cbSnapStyle.Enabled = this.cbSnap.Enabled && this.cursor.Snap;
        }

        private void InitializeComponent()
        {
            this.label2 = new Label();
            this.cbStyle = new ComboBox();
            this.cbSnap = new CheckBox();
            this.cbFollow = new CheckBox();
            this.button1 = new ButtonPen();
            this.tabControl1 = new TabControl();
            this.tpStyle = new TabPage();
            this.cbFastCursor = new CheckBox();
            this.cbScopeStyle = new ComboBox();
            this.label6 = new Label();
            this.label5 = new Label();
            this.udScopeSize = new NumericUpDown();
            this.label4 = new Label();
            this.udVertSize = new NumericUpDown();
            this.label3 = new Label();
            this.upHorizSize = new NumericUpDown();
            this.tpSnap = new TabPage();
            this.cbUseSeriesZ = new CheckBox();
            this.cbSnapStyle = new ComboBox();
            this.label7 = new Label();
            this.tpMouse = new TabPage();
            this.label9 = new Label();
            this.label8 = new Label();
            this.upClickTolerance = new NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tpStyle.SuspendLayout();
            this.udScopeSize.BeginInit();
            this.udVertSize.BeginInit();
            this.upHorizSize.BeginInit();
            this.tpSnap.SuspendLayout();
            this.tpMouse.SuspendLayout();
            this.upClickTolerance.BeginInit();
            base.SuspendLayout();
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x12, 0x29);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "S&tyle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Items.Add("Horizontal");
            this.cbStyle.Items.Add("Vertical");
            this.cbStyle.Items.Add("Both");
            this.cbStyle.Items.Add("Scope");
            this.cbStyle.Items.Add("ScopeOnly");
            this.cbStyle.Location = new Point(0x12, 0x39);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x88, 0x15);
            this.cbStyle.TabIndex = 3;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.cbSnap.FlatStyle = FlatStyle.Flat;
            this.cbSnap.Location = new Point(6, 6);
            this.cbSnap.Name = "cbSnap";
            this.cbSnap.Size = new Size(0x90, 0x18);
            this.cbSnap.TabIndex = 4;
            this.cbSnap.Text = "S&nap";
            this.cbSnap.CheckedChanged += new EventHandler(this.cbSnap_CheckedChanged);
            this.cbFollow.FlatStyle = FlatStyle.Flat;
            this.cbFollow.Location = new Point(0x12, 6);
            this.cbFollow.Name = "cbFollow";
            this.cbFollow.Size = new Size(0x90, 0x18);
            this.cbFollow.TabIndex = 5;
            this.cbFollow.Text = "&Follow Mouse";
            this.cbFollow.CheckedChanged += new EventHandler(this.cbFollow_CheckedChanged);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 6;
            this.button1.Text = "&Pen...";
            this.tabControl1.Controls.Add(this.tpStyle);
            this.tabControl1.Controls.Add(this.tpSnap);
            this.tabControl1.Controls.Add(this.tpMouse);
            this.tabControl1.Location = new Point(12, 0x23);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(250, 0xe1);
            this.tabControl1.TabIndex = 7;
            this.tpStyle.Controls.Add(this.cbFastCursor);
            this.tpStyle.Controls.Add(this.cbScopeStyle);
            this.tpStyle.Controls.Add(this.label6);
            this.tpStyle.Controls.Add(this.label5);
            this.tpStyle.Controls.Add(this.udScopeSize);
            this.tpStyle.Controls.Add(this.label4);
            this.tpStyle.Controls.Add(this.udVertSize);
            this.tpStyle.Controls.Add(this.label3);
            this.tpStyle.Controls.Add(this.upHorizSize);
            this.tpStyle.Controls.Add(this.button1);
            this.tpStyle.Controls.Add(this.cbStyle);
            this.tpStyle.Controls.Add(this.label2);
            this.tpStyle.Location = new Point(4, 0x16);
            this.tpStyle.Name = "tpStyle";
            this.tpStyle.Size = new Size(0xe2, 0xc7);
            this.tpStyle.TabIndex = 0;
            this.tpStyle.Text = "Style";
            this.tpStyle.Padding = new Padding(3);
            this.tpStyle.UseVisualStyleBackColor = true;
            this.cbFastCursor.Location = new Point(0x7e, 12);
            this.cbFastCursor.Name = "cbFastCursor";
            this.cbFastCursor.Size = new Size(0x4c, 0x11);
            this.cbFastCursor.TabIndex = 15;
            this.cbFastCursor.Text = "&FastCursor";
            this.cbFastCursor.AutoSize = true;
            this.cbFastCursor.UseVisualStyleBackColor = true;
            this.cbFastCursor.Click += new EventHandler(this.cbFastCursor_Click);
            this.cbScopeStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbScopeStyle.Items.Add("Horizontal");
            this.cbScopeStyle.Items.Add("Vertical");
            this.cbScopeStyle.Items.Add("Both");
            this.cbScopeStyle.Location = new Point(0x63, 0xa3);
            this.cbScopeStyle.Name = "cbScopeStyle";
            this.cbScopeStyle.Size = new Size(0x88, 0x15);
            this.cbScopeStyle.TabIndex = 14;
            this.cbScopeStyle.SelectedIndexChanged += new EventHandler(this.cbScopeStyle_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x63, 0x93);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x43, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Scope Sty&le:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x12, 0x93);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x40, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "S&cope Size:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.udScopeSize.Location = new Point(0x12, 0xa3);
            this.udScopeSize.Name = "udScopeSize";
            this.udScopeSize.Size = new Size(0x39, 20);
            this.udScopeSize.TabIndex = 11;
            this.udScopeSize.ValueChanged += new EventHandler(this.udScopeSize_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x63, 0x62);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x34, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "&Vert Size:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.udVertSize.Location = new Point(0x63, 0x72);
            this.udVertSize.Name = "udVertSize";
            this.udVertSize.Size = new Size(0x39, 20);
            this.udVertSize.TabIndex = 9;
            this.udVertSize.ValueChanged += new EventHandler(this.udVertSize_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x12, 0x62);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x39, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "&Horiz Size:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.upHorizSize.Location = new Point(0x12, 0x72);
            this.upHorizSize.Name = "upHorizSize";
            this.upHorizSize.Size = new Size(0x39, 20);
            this.upHorizSize.TabIndex = 7;
            this.upHorizSize.ValueChanged += new EventHandler(this.upHorizSize_ValueChanged);
            this.tpSnap.Controls.Add(this.cbUseSeriesZ);
            this.tpSnap.Controls.Add(this.cbSnapStyle);
            this.tpSnap.Controls.Add(this.label7);
            this.tpSnap.Controls.Add(this.cbSnap);
            this.tpSnap.Location = new Point(4, 0x16);
            this.tpSnap.Name = "tpSnap";
            this.tpSnap.Size = new Size(0xe2, 0xc7);
            this.tpSnap.TabIndex = 1;
            this.tpSnap.Text = "Snap";
            this.tpSnap.Padding = new Padding(3);
            this.tpSnap.UseVisualStyleBackColor = true;
            this.cbUseSeriesZ.FlatStyle = FlatStyle.Flat;
            this.cbUseSeriesZ.Location = new Point(0x12, 0x57);
            this.cbUseSeriesZ.Name = "cbUseSeriesZ";
            this.cbUseSeriesZ.Size = new Size(0x90, 0x18);
            this.cbUseSeriesZ.TabIndex = 0x11;
            this.cbUseSeriesZ.Text = "Use Series &Z";
            this.cbUseSeriesZ.Click += new EventHandler(this.cbUseSeriesZ_Click);
            this.cbSnapStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbSnapStyle.Items.Add("Horizontal");
            this.cbSnapStyle.Items.Add("Vertical");
            this.cbSnapStyle.Items.Add("Both");
            this.cbSnapStyle.Location = new Point(6, 0x31);
            this.cbSnapStyle.Name = "cbSnapStyle";
            this.cbSnapStyle.Size = new Size(0x88, 0x15);
            this.cbSnapStyle.TabIndex = 0x10;
            this.cbSnapStyle.SelectedIndexChanged += new EventHandler(this.cbSnapStyle_SelectedIndexChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x12, 0x21);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x3d, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Snap Sty&le:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.tpMouse.Controls.Add(this.label9);
            this.tpMouse.Controls.Add(this.label8);
            this.tpMouse.Controls.Add(this.upClickTolerance);
            this.tpMouse.Controls.Add(this.cbFollow);
            this.tpMouse.Location = new Point(4, 0x16);
            this.tpMouse.Name = "tpMouse";
            this.tpMouse.Size = new Size(0xe2, 0xc7);
            this.tpMouse.TabIndex = 2;
            this.tpMouse.Text = "Mouse";
            this.tpMouse.UseVisualStyleBackColor = true;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x3f, 0x3f);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x21, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "pixels";
            this.label9.TextAlign = ContentAlignment.TopRight;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(3, 0x2d);
            this.label8.Name = "label8";
            this.label8.Size = new Size(80, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Click &tolerance:";
            this.label8.TextAlign = ContentAlignment.TopRight;
            this.upClickTolerance.Location = new Point(3, 0x3d);
            this.upClickTolerance.Name = "upClickTolerance";
            this.upClickTolerance.Size = new Size(0x39, 20);
            this.upClickTolerance.TabIndex = 9;
            this.upClickTolerance.ValueChanged += new EventHandler(this.upClickTolerance_ValueChanged);
            base.ClientSize = new Size(0x105, 0x107);
            base.Controls.Add(this.tabControl1);
            base.Name = "CursorEditor";
            base.Controls.SetChildIndex(this.tabControl1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            this.tabControl1.ResumeLayout(false);
            this.tpStyle.ResumeLayout(false);
            this.tpStyle.PerformLayout();
            this.udScopeSize.EndInit();
            this.udVertSize.EndInit();
            this.upHorizSize.EndInit();
            this.tpSnap.ResumeLayout(false);
            this.tpSnap.PerformLayout();
            this.tpMouse.ResumeLayout(false);
            this.tpMouse.PerformLayout();
            this.upClickTolerance.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void udScopeSize_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.ScopeSize = (int) this.udScopeSize.Value;
            }
        }

        private void udVertSize_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.VertSize = (int) this.udVertSize.Value;
            }
        }

        private void upClickTolerance_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.CursorClickTolerance = (int) this.upClickTolerance.Value;
            }
        }

        private void upHorizSize_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.HorizSize = (int) this.upHorizSize.Value;
            }
        }
    }
}

