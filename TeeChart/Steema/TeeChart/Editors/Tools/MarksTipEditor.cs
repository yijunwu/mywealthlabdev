namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MarksTipEditor : ToolSeriesEditor
    {
        private ComboBox cbAction;
        private ComboBox cbStyle;
        private IContainer components;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown ndDelay;
        private NumericUpDown ndHideDelay;
        private MarksTip tool;

        public MarksTipEditor()
        {
            this.InitializeComponent();
            this.cbStyle.Items.Add("Value");
            this.cbStyle.Items.Add("Percent");
            this.cbStyle.Items.Add("Label");
            this.cbStyle.Items.Add("Label and Percent");
            this.cbStyle.Items.Add("Label and Value");
            this.cbStyle.Items.Add("Legend");
            this.cbStyle.Items.Add("Percent and Total");
            this.cbStyle.Items.Add("Label, Percent and Total");
            this.cbStyle.Items.Add("X Value");
            this.cbStyle.Items.Add("X and Y");
            this.cbAction.Items.Add("Move");
            this.cbAction.Items.Add("Click");
        }

        public MarksTipEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (MarksTip) t;
            base.SetTool(this.tool, null);
            base.CBSeries.Items[0] = Texts.All;
            if (this.tool.Series == null)
            {
                base.CBSeries.SelectedIndex = 0;
            }
            else
            {
                base.CBSeries.SelectedIndex = 1 + this.tool.chart.series.IndexOf(this.tool.Series);
            }
            this.cbStyle.SelectedIndex = (int) this.tool.Style;
            this.ndDelay.Value = this.tool.MouseDelay;
            this.ndHideDelay.Value = this.tool.HideDelay;
            if (this.tool.MouseAction == MarksTipMouseAction.Move)
            {
                this.cbAction.SelectedIndex = 0;
            }
            else
            {
                this.cbAction.SelectedIndex = 1;
            }
            base.setting = false;
        }

        private void cbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                if (this.cbAction.SelectedIndex == 0)
                {
                    this.tool.MouseAction = MarksTipMouseAction.Move;
                }
                else
                {
                    this.tool.MouseAction = MarksTipMouseAction.Click;
                }
            }
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Style = (MarksStyles) this.cbStyle.SelectedIndex;
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
            this.label2 = new Label();
            this.cbStyle = new ComboBox();
            this.label3 = new Label();
            this.ndDelay = new NumericUpDown();
            this.cbAction = new ComboBox();
            this.label4 = new Label();
            this.ndHideDelay = new NumericUpDown();
            this.ndHideDelay.BeginInit();
            this.ndDelay.BeginInit();
            this.label5 = new Label();
            base.SuspendLayout();
            base.CBSeries.Location = new Point(0x62, 8);
            base.CBSeries.ItemHeight = 13;
            base.label1.Location = new Point(60, 12);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x3d, 0x2a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "S&tyle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Location = new Point(0x62, 40);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x88, 0x15);
            this.cbStyle.TabIndex = 3;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x3a, 0x6f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x25, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Delay:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.ndDelay.BorderStyle = BorderStyle.FixedSingle;
            this.ndDelay.Location = new Point(100, 0x6d);
            int[] bits = new int[4];
            bits[0] = 0x2710;
            this.ndDelay.Maximum = new decimal(bits);
            this.ndDelay.Name = "ndDelay";
            this.ndDelay.Size = new Size(0x3e, 20);
            this.ndDelay.TabIndex = 5;
            this.ndDelay.TextAlign = HorizontalAlignment.Right;
            this.ndDelay.ValueChanged += new EventHandler(this.ndDelay_ValueChanged);
            this.ndDelay.TextChanged += new EventHandler(this.ndDelay_ValueChanged);
            this.cbAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbAction.Location = new Point(0x62, 0x48);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new Size(0x88, 0x15);
            this.cbAction.TabIndex = 7;
            this.cbAction.SelectedIndexChanged += new EventHandler(this.cbAction_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x3a, 0x4a);
            this.label4.Name = "label4";
            this.label4.Size = new Size(40, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "&Action:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.ndHideDelay.BorderStyle = BorderStyle.FixedSingle;
            this.ndHideDelay.Location = new Point(100, 0x88);
            int[] numArray2 = new int[4];
            numArray2[0] = 0x2710;
            this.ndHideDelay.Maximum = new decimal(numArray2);
            this.ndHideDelay.Name = "ndHideDelay";
            this.ndHideDelay.Size = new Size(0x3e, 20);
            this.ndHideDelay.TabIndex = 9;
            this.ndHideDelay.TextAlign = HorizontalAlignment.Right;
            this.ndHideDelay.ValueChanged += new EventHandler(this.ndHideDelay_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x20, 0x8a);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x3e, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "&Hide Delay:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.Add(this.ndHideDelay);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.cbAction);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.ndDelay);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cbStyle);
            base.Controls.Add(this.label2);
            base.Name = "MarksTipEditor";
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.cbStyle, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.ndDelay, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.Controls.SetChildIndex(this.label4, 0);
            base.Controls.SetChildIndex(this.cbAction, 0);
            base.Controls.SetChildIndex(this.label5, 0);
            base.Controls.SetChildIndex(this.ndHideDelay, 0);
            this.ndHideDelay.EndInit();
            this.ndDelay.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ndDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.MouseDelay = (int) this.ndDelay.Value;
            }
        }

        private void ndHideDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.HideDelay = (int) this.ndHideDelay.Value;
            }
        }
    }
}

