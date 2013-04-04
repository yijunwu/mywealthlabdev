namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class HotspotEditor : ToolSeriesEditor
    {
        private ComboBox cbAction;
        private ComboBox cbHelperScript;
        private ComboBox cbStyle;
        private Container components;
        private Label label2;
        private Label label3;
        private Label label4;
        private System.Windows.Forms.Panel panel1;
        private SeriesHotspot tool;

        public HotspotEditor()
        {
            this.InitializeComponent();
        }

        public HotspotEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (SeriesHotspot) t;
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
            this.cbAction.SelectedIndex = (int) this.tool.MapAction;
            this.cbHelperScript.SelectedIndex = (int) this.tool.HelperScript;
            this.cbHelperScript.Enabled = this.tool.MapAction == MapAction.Script;
            base.setting = false;
            EditorUtils.Translate(this);
        }

        private void cbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.MapAction = (MapAction) this.cbAction.SelectedIndex;
            }
            this.cbHelperScript.Enabled = this.tool.MapAction == MapAction.Script;
        }

        private void cbHelperScript_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.HelperScript = (HotspotHelperScripts) this.cbHelperScript.SelectedIndex;
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
            this.cbAction = new ComboBox();
            this.label4 = new Label();
            this.cbStyle = new ComboBox();
            this.label2 = new Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new Label();
            this.cbHelperScript = new ComboBox();
            base.SuspendLayout();
            base.CBSeries.Visible = false;
            this.cbAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbAction.Items.AddRange(new object[] { "Mark", "URL", "Script" });
            this.cbAction.Location = new Point(0x7a, 0x59);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new Size(0x68, 0x15);
            this.cbAction.TabIndex = 11;
            this.cbAction.SelectedIndexChanged += new EventHandler(this.cbAction_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x3a, 0x5d);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x40, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "&Map Action:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Items.AddRange(new object[] { "Value", "Percent", "Label", "Label and Percent", "Label and Value", "Legend", "Percent and Total", "Label, Percent and Total", "X Value", "X and Y" });
            this.cbStyle.Location = new Point(0x59, 0x2f);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x88, 0x15);
            this.cbStyle.TabIndex = 9;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x36, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "S&tyle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x124, 40);
            this.panel1.TabIndex = 12;
            this.label3.Location = new Point(7, 0x84);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x6f, 0x17);
            this.label3.TabIndex = 13;
            this.label3.Text = "&Helper script:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.cbHelperScript.Enabled = false;
            this.cbHelperScript.Items.AddRange(new object[] { "None", "Annotations" });
            this.cbHelperScript.Location = new Point(0x7b, 0x84);
            this.cbHelperScript.Name = "cbHelperScript";
            this.cbHelperScript.Size = new Size(0x68, 0x15);
            this.cbHelperScript.TabIndex = 14;
            this.cbHelperScript.SelectedIndexChanged += new EventHandler(this.cbHelperScript_SelectedIndexChanged);
            base.ClientSize = new Size(0x124, 0x10a);
            base.Controls.Add(this.cbHelperScript);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.cbAction);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.cbStyle);
            base.Controls.Add(this.label2);
            base.Name = "HotspotEditor";
            this.Text = "HotspotEditor";
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.cbStyle, 0);
            base.Controls.SetChildIndex(this.label4, 0);
            base.Controls.SetChildIndex(this.cbAction, 0);
            base.Controls.SetChildIndex(this.panel1, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.cbHelperScript, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

