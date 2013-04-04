namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolSeriesEditor : Form
    {
        protected ComboBox CBSeries;
        private Container components;
        protected Label label1;
        protected internal bool setting;
        private ToolSeries tool;

        public ToolSeriesEditor()
        {
            this.InitializeComponent();
        }

        protected virtual void CBSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                if ((this.CBSeries.SelectedIndex == -1) || (this.CBSeries.SelectedIndex == 0))
                {
                    this.tool.Series = null;
                }
                else
                {
                    this.tool.Series = this.tool.chart.series[this.CBSeries.SelectedIndex - 1];
                }
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

        protected void FillSeriesCombo(ComboBox aCombo, Series aSeries, Chart aChart)
        {
            this.FillSeriesCombo(aCombo, aSeries, aChart, null);
        }

        protected virtual void FillSeriesCombo(ComboBox aCombo, Series aSeries, Chart aChart, System.Type aInclude)
        {
            if (aChart != null)
            {
                aCombo.BeginUpdate();
                aCombo.Items.Clear();
                aCombo.Items.Add(Texts.None);
                foreach (Series series in aChart.Series)
                {
                    if (!series.InternalUse)
                    {
                        if (aInclude == null)
                        {
                            aCombo.Items.Add(series.ToString());
                        }
                        else if (series.GetType() == aInclude)
                        {
                            aCombo.Items.Add(series.ToString());
                        }
                    }
                }
                if (aSeries == null)
                {
                    aCombo.SelectedIndex = 0;
                }
                else
                {
                    aCombo.SelectedIndex = aCombo.Items.IndexOf(aSeries.ToString());
                }
                aCombo.EndUpdate();
            }
        }

        private void InitializeComponent()
        {
            this.CBSeries = new ComboBox();
            this.label1 = new Label();
            base.SuspendLayout();
            this.CBSeries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBSeries.Location = new Point(0x52, 8);
            this.CBSeries.Name = "CBSeries";
            this.CBSeries.Size = new Size(0x88, 0x15);
            this.CBSeries.TabIndex = 1;
            this.CBSeries.SelectedIndexChanged += new EventHandler(this.CBSeries_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x2c, 12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x27, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Series:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.CBSeries);
            base.Name = "ToolSeriesEditor";
            base.ResumeLayout(false);
        }

        internal void SetTool(ToolSeries t, System.Type include)
        {
            this.setting = true;
            this.tool = t;
            this.FillSeriesCombo(this.CBSeries, this.tool.Series, this.tool.Chart, include);
            this.setting = false;
        }
    }
}

