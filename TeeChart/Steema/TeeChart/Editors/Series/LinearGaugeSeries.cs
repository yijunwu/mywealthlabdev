namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class LinearGaugeSeries : CustomGaugeSeries
    {
        private Button bMaxValueIndicator;
        private Button bValueAreaBrush;
        private CheckBox cbValueColorPalette;
        private IContainer components;
        private LinearGauge linearGauge;

        public LinearGaugeSeries()
        {
            this.InitializeComponent();
        }

        public LinearGaugeSeries(Series s) : this()
        {
            this.linearGauge = (LinearGauge) s;
            base.series = this.linearGauge;
        }

        private void bMaxValueIndicator_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.GaugeSeriesPointer f = new Steema.TeeChart.Editors.GaugeSeriesPointer(this.linearGauge.MaxValueIndicator);
            EditorUtils.ShowFormModal(f);
        }

        private void bValueAreaBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.linearGauge.ValueAreaBrush);
        }

        private void cbValueColorPalette_Click(object sender, EventArgs e)
        {
            this.linearGauge.UseValueColorPalette = this.cbValueColorPalette.Checked;
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
            this.bValueAreaBrush = new Button();
            this.bMaxValueIndicator = new Button();
            this.cbValueColorPalette = new CheckBox();
            base.tabOptions.SuspendLayout();
            base.SuspendLayout();
            base.tabOptions.Controls.Add(this.cbValueColorPalette);
            base.tabOptions.Controls.Add(this.bMaxValueIndicator);
            base.tabOptions.Controls.Add(this.bValueAreaBrush);
            base.tabOptions.Controls.SetChildIndex(this.bValueAreaBrush, 0);
            base.tabOptions.Controls.SetChildIndex(this.bMaxValueIndicator, 0);
            base.tabOptions.Controls.SetChildIndex(this.cbValueColorPalette, 0);
            this.bValueAreaBrush.FlatStyle = FlatStyle.Flat;
            this.bValueAreaBrush.Location = new Point(8, 0x5e);
            this.bValueAreaBrush.Name = "bValueAreaBrush";
            this.bValueAreaBrush.Size = new Size(0x79, 0x17);
            this.bValueAreaBrush.TabIndex = 0x17;
            this.bValueAreaBrush.Text = "Value Area Brush...";
            this.bValueAreaBrush.UseVisualStyleBackColor = true;
            this.bValueAreaBrush.Click += new EventHandler(this.bValueAreaBrush_Click);
            this.bMaxValueIndicator.FlatStyle = FlatStyle.Flat;
            this.bMaxValueIndicator.Location = new Point(8, 0x7b);
            this.bMaxValueIndicator.Name = "bMaxValueIndicator";
            this.bMaxValueIndicator.Size = new Size(0x79, 0x17);
            this.bMaxValueIndicator.TabIndex = 0x18;
            this.bMaxValueIndicator.Text = "Max Value Indicator...";
            this.bMaxValueIndicator.UseVisualStyleBackColor = true;
            this.bMaxValueIndicator.Click += new EventHandler(this.bMaxValueIndicator_Click);
            this.cbValueColorPalette.AutoSize = true;
            this.cbValueColorPalette.Location = new Point(0x97, 40);
            this.cbValueColorPalette.Name = "cbValueColorPalette";
            this.cbValueColorPalette.Size = new Size(0x8a, 0x11);
            this.cbValueColorPalette.TabIndex = 0x19;
            this.cbValueColorPalette.Text = "Use Value Color Palette";
            this.cbValueColorPalette.UseVisualStyleBackColor = true;
            this.cbValueColorPalette.Click += new EventHandler(this.cbValueColorPalette_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x158, 0xb6);
            base.Name = "LinearGaugeSeries";
            this.Text = "LinearGaugeEditor";
            base.tabOptions.ResumeLayout(false);
            base.tabOptions.PerformLayout();
            base.ResumeLayout(false);
        }

        public override void SetParent(TabPage Parent)
        {
            base.SetParent(Parent);
            if (this.linearGauge != null)
            {
                this.cbValueColorPalette.Checked = this.linearGauge.UseValueColorPalette;
            }
        }
    }
}

