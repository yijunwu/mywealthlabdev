namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TagCloudSeries : BaseSeriesForm
    {
        private Button button1;
        private ButtonPen buttonPen1;
        private CheckBox cbClip;
        private IContainer components;
        private GradientEditor gradientEdit;
        private Grid3DSeries grid3DEditor;
        private Label label1;
        private Label label2;
        private TagCloud series;
        private TextEditor shapeText;
        private TextBox tbFilter;
        private TabControl tcTabs;
        private TabPage tpFont;
        private TabPage tpGradient;
        private TabPage tpOptions;
        private NumericUpDown udSeparation;

        public TagCloudSeries()
        {
            this.InitializeComponent();
        }

        public TagCloudSeries(Series s) : this()
        {
            this.series = (TagCloud) s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void cbClip_Click(object sender, EventArgs e)
        {
            this.series.Chart.Aspect.ClipPoints = this.cbClip.Checked;
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
            this.tcTabs = new TabControl();
            this.tpOptions = new TabPage();
            this.button1 = new Button();
            this.buttonPen1 = new ButtonPen();
            this.udSeparation = new NumericUpDown();
            this.label2 = new Label();
            this.cbClip = new CheckBox();
            this.tbFilter = new TextBox();
            this.label1 = new Label();
            this.tpFont = new TabPage();
            this.tpGradient = new TabPage();
            this.tcTabs.SuspendLayout();
            this.tpOptions.SuspendLayout();
            this.udSeparation.BeginInit();
            base.SuspendLayout();
            this.tcTabs.Controls.Add(this.tpOptions);
            this.tcTabs.Controls.Add(this.tpFont);
            this.tcTabs.Controls.Add(this.tpGradient);
            this.tcTabs.Dock = DockStyle.Fill;
            this.tcTabs.Location = new Point(0, 0);
            this.tcTabs.Name = "tcTabs";
            this.tcTabs.SelectedIndex = 0;
            this.tcTabs.Size = new Size(0x12f, 0xc3);
            this.tcTabs.TabIndex = 0;
            this.tcTabs.SelectedIndexChanged += new EventHandler(this.tcTabs_SelectedIndexChanged);
            this.tpOptions.Controls.Add(this.button1);
            this.tpOptions.Controls.Add(this.buttonPen1);
            this.tpOptions.Controls.Add(this.udSeparation);
            this.tpOptions.Controls.Add(this.label2);
            this.tpOptions.Controls.Add(this.cbClip);
            this.tpOptions.Controls.Add(this.tbFilter);
            this.tpOptions.Controls.Add(this.label1);
            this.tpOptions.Location = new Point(4, 0x16);
            this.tpOptions.Name = "tpOptions";
            this.tpOptions.Padding = new Padding(3);
            this.tpOptions.UseVisualStyleBackColor = true;
            this.tpOptions.Size = new Size(0x127, 0xa9);
            this.tpOptions.TabIndex = 0;
            this.tpOptions.Text = "Options";
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x92, 0x3b);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 6;
            this.button1.Text = "&Pattern...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(0x92, 30);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.Size = new Size(0x4b, 0x17);
            this.buttonPen1.TabIndex = 5;
            this.buttonPen1.Text = "&Border...";
            this.buttonPen1.UseVisualStyleBackColor = true;
            this.udSeparation.Location = new Point(11, 0x6f);
            this.udSeparation.Name = "udSeparation";
            this.udSeparation.Size = new Size(0x45, 20);
            this.udSeparation.TabIndex = 4;
            this.udSeparation.ValueChanged += new EventHandler(this.udSeparation_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(8, 0x5f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Separation %:";
            this.cbClip.AutoSize = true;
            this.cbClip.UseVisualStyleBackColor = true;
            this.cbClip.Location = new Point(11, 0x3a);
            this.cbClip.Name = "cbClip";
            this.cbClip.Size = new Size(0x2b, 0x11);
            this.cbClip.TabIndex = 2;
            this.cbClip.Text = "&Clip";
            this.cbClip.Click += new EventHandler(this.cbClip_Click);
            this.tbFilter.Location = new Point(11, 0x20);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new Size(100, 20);
            this.tbFilter.TabIndex = 1;
            this.tbFilter.TextChanged += new EventHandler(this.tbFilter_TextChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Filter:";
            this.tpFont.Location = new Point(4, 0x16);
            this.tpFont.Name = "tpFont";
            this.tpFont.Padding = new Padding(3);
            this.tpFont.UseVisualStyleBackColor = true;
            this.tpFont.Size = new Size(0x127, 0xa9);
            this.tpFont.TabIndex = 1;
            this.tpFont.Text = "Font";
            this.tpGradient.Location = new Point(4, 0x16);
            this.tpGradient.Name = "tpGradient";
            this.tpGradient.Size = new Size(0x127, 0xa9);
            this.tpGradient.TabIndex = 2;
            this.tpGradient.Text = "Gradient";
            this.tpGradient.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x12f, 0xc3);
            base.Controls.Add(this.tcTabs);
            base.Name = "TagCloudSeries";
            this.Text = "TagCloudEditor";
            this.tcTabs.ResumeLayout(false);
            this.tpOptions.ResumeLayout(false);
            this.tpOptions.PerformLayout();
            this.udSeparation.EndInit();
            base.ResumeLayout(false);
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.series != null)
            {
                this.tbFilter.Text = this.series.Filter;
                this.cbClip.Checked = this.series.Chart.Aspect.ClipPoints;
                this.buttonPen1.Pen = this.series.Pen;
                this.udSeparation.Value = this.series.TagSeparation;
                if (this.grid3DEditor == null)
                {
                    this.grid3DEditor = new Grid3DSeries(this.series, Parent);
                }
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            this.series.Filter = this.tbFilter.Text;
        }

        private void tcTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tcTabs.SelectedTab == this.tpFont)
            {
                if (this.shapeText == null)
                {
                    this.shapeText = new TextEditor(this.series.Font, this.tpFont);
                    EditorUtils.Translate(this.shapeText);
                }
            }
            else if ((this.tcTabs.SelectedTab == this.tpGradient) && (this.gradientEdit == null))
            {
                this.gradientEdit = new GradientEditor(this.series.Gradient, this.tpGradient);
                EditorUtils.Translate(this.gradientEdit);
            }
        }

        private void udSeparation_ValueChanged(object sender, EventArgs e)
        {
            this.series.TagSeparation = Utils.Round((float) this.udSeparation.Value);
        }
    }
}

