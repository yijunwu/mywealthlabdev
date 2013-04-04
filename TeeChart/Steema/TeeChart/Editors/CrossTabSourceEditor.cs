namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class CrossTabSourceEditor : BaseSourceEditor
    {
        private ComboBox cbCalc;
        private ComboBox cbCalcOf;
        private ComboBox cbGroupBy;
        private ComboBox cbLabels;
        private CheckBox chkCaseSensitive;
        private IContainer components;
        private Label labelCalc;
        private Label labelGroup;
        private Label labelLabel;
        private Label labelOf;
        private bool updating;

        public CrossTabSourceEditor()
        {
            this.InitializeComponent();
            int right = base.labelSource.Right;
            base.labelSource.Text = Texts.AskDataSet;
            base.labelSource.Left = right - base.labelSource.Width;
        }

        public CrossTabSourceEditor(Steema.TeeChart.Styles.Series s)
            : this()
        {
            base.series = s;
        }

        protected override void ApplyChanges()
        {
            CrossTabSource dataSource;
            base.series.Tag = null;
            if (base.series.DataSource is CrossTabSource)
            {
                dataSource = (CrossTabSource) base.series.DataSource;
                dataSource.DataSource = base.CBSources.SelectedItem;
            }
            else
            {
                base.series.DataSource = null;
                dataSource = new CrossTabSource(base.CBSources.SelectedItem);
                base.series.DataSource = dataSource;
                base.series.Chart.AddToContainer(dataSource);
            }
            dataSource = (CrossTabSource) base.series.DataSource;
            dataSource.Formula = (this.cbCalc.SelectedIndex == 0) ? GroupFormula.Sum : GroupFormula.Count;
            dataSource.ValueField = this.GetComboText(this.cbCalcOf);
            dataSource.GroupField = this.GetComboText(this.cbGroupBy);
            dataSource.LabelField = this.GetComboText(this.cbLabels);
            dataSource.CaseSensitive = this.chkCaseSensitive.Checked;
            base.series.CheckDataSource();
            base.BApply.Enabled = false;
        }

        private void cbCalc_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.BApply.Enabled = true;
        }

        private void cbLabels_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.BApply.Enabled = true;
        }

        protected override void CBSourcesSelectedIndexChanged(object sender, EventArgs e)
        {
            base.CBSourcesSelectedIndexChanged(sender, e);
            this.SetFields();
            base.BApply.Enabled = true;
        }

        private void chkCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            base.BApply.Enabled = true;
        }

        private void DatabaseEditor_Load(object sender, EventArgs e)
        {
            if (base.series != null)
            {
                this.updating = true;
                try
                {
                    DataSeriesSource.FillSources(base.series, base.CBSources);
                    this.SetCBSource();
                }
                finally
                {
                    this.updating = false;
                    this.SetFields();
                }
                if ((base.series.DataSource != null) && (base.series.DataSource is CrossTabSource))
                {
                    CrossTabSource dataSource = (CrossTabSource) base.series.DataSource;
                    if ((dataSource.DataSource is DataSet) && (((DataSet) dataSource.DataSource).Tables.Count > 0))
                    {
                        base.CBSources.SelectedIndex = base.CBSources.Items.IndexOf(((DataSet) dataSource.DataSource).Tables[0]);
                    }
                    else
                    {
                        base.CBSources.SelectedIndex = base.CBSources.Items.IndexOf(dataSource.DataSource);
                    }
                    this.cbCalc.SelectedIndex = (dataSource.Formula == GroupFormula.Sum) ? 0 : 1;
                    this.cbCalcOf.SelectedIndex = this.cbCalcOf.Items.IndexOf(dataSource.ValueField);
                    this.cbGroupBy.SelectedIndex = this.cbGroupBy.Items.IndexOf(dataSource.GroupField);
                    this.cbLabels.SelectedIndex = this.cbLabels.Items.IndexOf(dataSource.LabelField);
                    this.chkCaseSensitive.Checked = dataSource.CaseSensitive;
                }
            }
            base.BApply.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetComboText(ComboBox c)
        {
            if (c.SelectedIndex == -1)
            {
                return c.Text;
            }
            return c.SelectedItem.ToString();
        }

        private void InitializeComponent()
        {
            this.labelCalc = new Label();
            this.cbCalc = new ComboBox();
            this.cbCalcOf = new ComboBox();
            this.labelOf = new Label();
            this.cbGroupBy = new ComboBox();
            this.labelGroup = new Label();
            this.cbLabels = new ComboBox();
            this.labelLabel = new Label();
            this.chkCaseSensitive = new CheckBox();
            base.SuspendLayout();
            base.CBSources.ItemHeight = 13;
            base.CBSources.Name = "CBSources";
            base.CBSources.Size = new Size(0xb8, 0x15);
            base.CBSources.SelectedIndexChanged += new EventHandler(this.CBSourcesSelectedIndexChanged);
            base.BApply.Name = "BApply";
            base.labelSource.Name = "labelSource";
            this.labelCalc.AutoSize = true;
            this.labelCalc.Location = new Point(0x1a, 0x29);
            this.labelCalc.Name = "labelCalc";
            this.labelCalc.Size = new Size(30, 0x10);
            this.labelCalc.TabIndex = 1;
            this.labelCalc.Text = "&Calc:";
            this.labelCalc.TextAlign = ContentAlignment.TopRight;
            this.cbCalc.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCalc.Items.AddRange(new object[] { "Sum", "Count" });
            this.cbCalc.Location = new Point(0x40, 40);
            this.cbCalc.Name = "cbCalc";
            this.cbCalc.Size = new Size(0x48, 0x15);
            this.cbCalc.TabIndex = 2;
            this.cbCalc.SelectedIndexChanged += new EventHandler(this.cbCalc_SelectedIndexChanged);
            this.cbCalcOf.Location = new Point(0xa8, 40);
            this.cbCalcOf.Name = "cbCalcOf";
            this.cbCalcOf.Size = new Size(0x8f, 0x15);
            this.cbCalcOf.TabIndex = 4;
            this.cbCalcOf.TextChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbCalcOf.SelectedValueChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbCalcOf.SelectedIndexChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.labelOf.AutoSize = true;
            this.labelOf.Location = new Point(0x92, 0x2a);
            this.labelOf.Name = "labelOf";
            this.labelOf.Size = new Size(14, 0x10);
            this.labelOf.TabIndex = 3;
            this.labelOf.Text = "of";
            this.labelOf.TextAlign = ContentAlignment.TopRight;
            this.cbGroupBy.Location = new Point(0xa8, 0x48);
            this.cbGroupBy.Name = "cbGroupBy";
            this.cbGroupBy.Size = new Size(0x8f, 0x15);
            this.cbGroupBy.TabIndex = 6;
            this.cbGroupBy.TextChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbGroupBy.SelectedValueChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbGroupBy.SelectedIndexChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.labelGroup.AutoSize = true;
            this.labelGroup.Location = new Point(0x6d, 0x4a);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new Size(0x36, 0x10);
            this.labelGroup.TabIndex = 5;
            this.labelGroup.Text = "&Group by:";
            this.labelGroup.TextAlign = ContentAlignment.TopRight;
            this.cbLabels.Location = new Point(0xa8, 0x68);
            this.cbLabels.Name = "cbLabels";
            this.cbLabels.Size = new Size(0x8f, 0x15);
            this.cbLabels.TabIndex = 8;
            this.cbLabels.TextChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbLabels.SelectedValueChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbLabels.SelectedIndexChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.labelLabel.AutoSize = true;
            this.labelLabel.Location = new Point(120, 0x6a);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new Size(0x29, 0x10);
            this.labelLabel.TabIndex = 7;
            this.labelLabel.Text = "&Labels:";
            this.labelLabel.TextAlign = ContentAlignment.TopRight;
            this.chkCaseSensitive.FlatStyle = FlatStyle.Flat;
            this.chkCaseSensitive.Location = new Point(0x40, 0x90);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new Size(0x7c, 0x10);
            this.chkCaseSensitive.TabIndex = 9;
            this.chkCaseSensitive.Text = "C&ase sensitive";
            this.chkCaseSensitive.CheckedChanged += new EventHandler(this.chkCaseSensitive_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x184, 0xc5);
            base.Controls.Add(this.chkCaseSensitive);
            base.Controls.Add(this.cbLabels);
            base.Controls.Add(this.labelLabel);
            base.Controls.Add(this.cbGroupBy);
            base.Controls.Add(this.labelGroup);
            base.Controls.Add(this.cbCalcOf);
            base.Controls.Add(this.labelOf);
            base.Controls.Add(this.cbCalc);
            base.Controls.Add(this.labelCalc);
            base.Name = "CrossTabSourceEditor";
            base.Load += new EventHandler(this.DatabaseEditor_Load);
            base.Controls.SetChildIndex(this.labelCalc, 0);
            base.Controls.SetChildIndex(this.cbCalc, 0);
            base.Controls.SetChildIndex(this.labelOf, 0);
            base.Controls.SetChildIndex(this.cbCalcOf, 0);
            base.Controls.SetChildIndex(this.labelGroup, 0);
            base.Controls.SetChildIndex(this.cbGroupBy, 0);
            base.Controls.SetChildIndex(this.labelLabel, 0);
            base.Controls.SetChildIndex(this.cbLabels, 0);
            base.Controls.SetChildIndex(this.chkCaseSensitive, 0);
            base.ResumeLayout(false);
        }

        protected override bool IsValid(object c)
        {
            return DataSeriesSource.IsValidSource(c);
        }

        private void SetFields()
        {
            if (!this.updating)
            {
                this.cbLabels.Items.Clear();
                this.cbCalcOf.Items.Clear();
                this.cbGroupBy.Items.Clear();
                this.cbLabels.Enabled = base.CBSources.SelectedIndex != -1;
                this.cbCalcOf.Enabled = base.CBSources.SelectedIndex != -1;
                this.cbGroupBy.Enabled = base.CBSources.SelectedIndex != -1;
                if (base.CBSources.SelectedIndex != -1)
                {
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        DataSeriesSource.FillFields(base.CBSources.SelectedItem, this.cbLabels, null);
                        DataSeriesSource.FillFields(base.CBSources.SelectedItem, this.cbGroupBy, null);
                        DataSeriesSource.FillFields(base.CBSources.SelectedItem, this.cbCalcOf, null);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }
    }
}

