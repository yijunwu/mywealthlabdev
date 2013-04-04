namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PointSeries : BaseSeriesForm
    {
        private ButtonColor button1;
        private ComboBox comboBox1;
        private ComboBox comboBoxTreatNulls;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        private Points series;

        public PointSeries()
        {
            this.InitializeComponent();
        }

        public PointSeries(Series s) : this(s, null)
        {
        }

        public PointSeries(Series s, Control parent) : this()
        {
            this.series = (Points) s;
            switch (this.series.Stacked)
            {
                case CustomStack.None:
                    this.comboBox1.SelectedIndex = 0;
                    break;

                case CustomStack.Overlap:
                    this.comboBox1.SelectedIndex = 1;
                    break;

                case CustomStack.Stack:
                    this.comboBox1.SelectedIndex = 2;
                    break;

                case CustomStack.Stack100:
                    this.comboBox1.SelectedIndex = 3;
                    break;
            }
            this.comboBoxTreatNulls.SelectedIndex = (int) this.series.TreatNulls;
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.series.Color = this.button1.Color;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    this.series.Stacked = CustomStack.None;
                    return;

                case 1:
                    this.series.Stacked = CustomStack.Overlap;
                    return;

                case 2:
                    this.series.Stacked = CustomStack.Stack;
                    return;

                case 3:
                    this.series.Stacked = CustomStack.Stack100;
                    return;
            }
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
            this.button1 = new ButtonColor();
            this.label1 = new Label();
            this.comboBox1 = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.comboBoxTreatNulls = new ComboBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(8, 8);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "&Color...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x18, 50);
            this.label1.Name = "label1";
            this.label1.Size = new Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Stacked:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] { "None", "Overlap", "Stack", "Stack 100%" });
            this.comboBox1.Location = new Point(80, 0x30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x79, 0x15);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.groupBox1.Controls.Add(this.comboBoxTreatNulls);
            this.groupBox1.Location = new Point(8, 0x4b);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xc1, 0x2f);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Treat Nulls";
            this.comboBoxTreatNulls.FormattingEnabled = true;
            this.comboBoxTreatNulls.Items.AddRange(new object[] { "Don't paint", "Skip", "Ignore" });
            this.comboBoxTreatNulls.Location = new Point(6, 0x13);
            this.comboBoxTreatNulls.Name = "comboBoxTreatNulls";
            this.comboBoxTreatNulls.Size = new Size(0x79, 0x15);
            this.comboBoxTreatNulls.TabIndex = 0;
            this.comboBoxTreatNulls.SelectedIndexChanged += new EventHandler(this.comboBoxTreatNulls_SelectedIndexChanged);
            base.ClientSize = new Size(0xf8, 0x7d);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.button1);
            base.Name = "PointSeries";
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                if (this.pointerEditor == null)
                {
                    this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(Parent, this.series.Pointer);
                    this.pointerEditor.CBDrawPoint.Visible = false;
                }
                this.button1.Color = this.series.Color;
            }
        }
    }
}

