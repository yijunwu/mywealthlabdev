namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FastLineSeries : PenEditor
    {
        private CheckBox CBDrawAll;
        private CheckBox cbInvStairs;
        private CheckBox cbStairs;
        private CheckBox checkBox1;
        private ComboBox comboBoxTreatNulls;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private FastLine series;

        public FastLineSeries()
        {
            this.InitializeComponent();
        }

        public FastLineSeries(Series s) : this()
        {
            this.series = (FastLine) s;
            base.SetPen(this.series.LinePen);
            this.CBDrawAll.Checked = this.series.DrawAllPoints;
            this.checkBox1.Checked = this.series.ColorEach;
            this.cbStairs.Checked = this.series.Stairs;
            this.cbInvStairs.Checked = this.series.InvertedStairs;
            this.cbInvStairs.Enabled = this.cbStairs.Checked;
            this.comboBoxTreatNulls.SelectedIndex = (int) this.series.TreatNulls;
            base.OkButton.Visible = false;
            base.VisibleCheckBox.Visible = false;
        }

        private void CBDrawAll_CheckedChanged(object sender, EventArgs e)
        {
            this.series.DrawAllPoints = this.CBDrawAll.Checked;
        }

        private void cbInvStairs_CheckedChanged(object sender, EventArgs e)
        {
            this.series.InvertedStairs = this.cbInvStairs.Checked;
        }

        private void cbStairs_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Stairs = this.cbStairs.Checked;
            this.cbInvStairs.Enabled = this.cbStairs.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.checkBox1.Checked;
        }

        private void comboBoxTreatNulls_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.series.TreatNulls = (TreatNullsStyle) this.comboBoxTreatNulls.SelectedIndex;
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
            this.CBDrawAll = new CheckBox();
            this.checkBox1 = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.cbInvStairs = new CheckBox();
            this.cbStairs = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.comboBoxTreatNulls = new ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.CBDrawAll.FlatStyle = FlatStyle.Flat;
            this.CBDrawAll.Location = new Point(0xb0, 0x8d);
            this.CBDrawAll.Name = "CBDrawAll";
            this.CBDrawAll.Size = new Size(0x67, 0x12);
            this.CBDrawAll.TabIndex = 9;
            this.CBDrawAll.Text = "Draw &All";
            this.CBDrawAll.CheckedChanged += new EventHandler(this.CBDrawAll_CheckedChanged);
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0xb0, 0x70);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(110, 0x17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Color &Each";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.groupBox1.Controls.Add(this.cbInvStairs);
            this.groupBox1.Controls.Add(this.cbStairs);
            this.groupBox1.Location = new Point(8, 0x70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x98, 40);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Line mode:";
            this.cbInvStairs.FlatStyle = FlatStyle.Flat;
            this.cbInvStairs.Location = new Point(0x4a, 0x10);
            this.cbInvStairs.Name = "cbInvStairs";
            this.cbInvStairs.Size = new Size(70, 0x10);
            this.cbInvStairs.TabIndex = 1;
            this.cbInvStairs.Text = "&Inverted";
            this.cbInvStairs.CheckedChanged += new EventHandler(this.cbInvStairs_CheckedChanged);
            this.cbStairs.FlatStyle = FlatStyle.Flat;
            this.cbStairs.Location = new Point(8, 0x10);
            this.cbStairs.Name = "cbStairs";
            this.cbStairs.Size = new Size(0x40, 0x10);
            this.cbStairs.TabIndex = 0;
            this.cbStairs.Text = "&Stairs";
            this.cbStairs.CheckedChanged += new EventHandler(this.cbStairs_CheckedChanged);
            this.groupBox2.Controls.Add(this.comboBoxTreatNulls);
            this.groupBox2.Location = new Point(8, 0x9e);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x98, 50);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Treat Nulls";
            this.comboBoxTreatNulls.FormattingEnabled = true;
            this.comboBoxTreatNulls.Items.AddRange(new object[] { "Don't paint", "Skip", "Ignore" });
            this.comboBoxTreatNulls.Location = new Point(6, 0x13);
            this.comboBoxTreatNulls.Name = "comboBoxTreatNulls";
            this.comboBoxTreatNulls.Size = new Size(0x79, 0x15);
            this.comboBoxTreatNulls.TabIndex = 0;
            this.comboBoxTreatNulls.SelectedIndexChanged += new EventHandler(this.comboBoxTreatNulls_SelectedIndexChanged);
            base.ClientSize = new Size(0x12e, 0xd5);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.CBDrawAll);
            base.Name = "FastLineSeries";
            base.Controls.SetChildIndex(base.cbVisible, 0);
            base.Controls.SetChildIndex(this.CBDrawAll, 0);
            base.Controls.SetChildIndex(this.checkBox1, 0);
            base.Controls.SetChildIndex(this.groupBox1, 0);
            base.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

