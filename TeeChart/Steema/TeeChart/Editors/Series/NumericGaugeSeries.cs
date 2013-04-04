namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Editors.Tools;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class NumericGaugeSeries : CustomGaugeSeries
    {
        internal ComboBox cbDigitalFont;
        private IContainer components;
        private Label label2;
        private NumericGauge numericGauge;
        private TabPage tabMarkers;
        private MarkersEditor toolsEditor;

        public NumericGaugeSeries()
        {
            this.InitializeComponent();
        }

        public NumericGaugeSeries(Series s) : this()
        {
            this.numericGauge = (NumericGauge) s;
            base.series = this.numericGauge;
            base.bHand.Visible = false;
            base.tabControl1.TabPages.Clear();
            base.tabControl1.TabPages.Add(base.tabOptions);
            base.tabControl1.TabPages.Add(this.tabMarkers);
            base.tabControl1.TabPages.Add(base.tabFrame);
        }

        private void cbDigitalFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbDigitalFont.SelectedIndex)
            {
                case 0:
                    this.numericGauge.DigitalFontType = DigitalFont.Bar;
                    return;

                case 1:
                    this.numericGauge.DigitalFontType = DigitalFont.Dot;
                    return;
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
            this.tabMarkers = new TabPage();
            this.cbDigitalFont = new ComboBox();
            this.label2 = new Label();
            base.tabControl1.SuspendLayout();
            base.tabOptions.SuspendLayout();
            base.SuspendLayout();
            base.tabControl1.Controls.Add(this.tabMarkers);
            base.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            base.tabControl1.Controls.SetChildIndex(this.tabMarkers, 0);
            base.tabControl1.Controls.SetChildIndex(base.tabFrame, 0);
            base.tabControl1.Controls.SetChildIndex(base.tabGreenLine, 0);
            base.tabControl1.Controls.SetChildIndex(base.tabRedLine, 0);
            base.tabControl1.Controls.SetChildIndex(base.tabLabels, 0);
            base.tabControl1.Controls.SetChildIndex(base.tabTicks, 0);
            base.tabControl1.Controls.SetChildIndex(base.tabOptions, 0);
            base.tabOptions.Controls.Add(this.cbDigitalFont);
            base.tabOptions.Controls.Add(this.label2);
            base.tabOptions.Controls.SetChildIndex(this.label2, 0);
            base.tabOptions.Controls.SetChildIndex(this.cbDigitalFont, 0);
            base.tabOptions.Controls.SetChildIndex(base.bHand, 0);
            this.tabMarkers.Location = new Point(4, 0x16);
            this.tabMarkers.Name = "tabMarkers";
            this.tabMarkers.Size = new Size(0x150, 0x9c);
            this.tabMarkers.TabIndex = 6;
            this.tabMarkers.Text = "Markers";
            this.tabMarkers.UseVisualStyleBackColor = true;
            this.cbDigitalFont.FormattingEnabled = true;
            this.cbDigitalFont.Location = new Point(0xca, 110);
            this.cbDigitalFont.Name = "cbDigitalFont";
            this.cbDigitalFont.Size = new Size(0x79, 0x15);
            this.cbDigitalFont.TabIndex = 0x17;
            this.cbDigitalFont.Text = "Bar";
            this.cbDigitalFont.SelectedIndexChanged += new EventHandler(this.cbDigitalFont_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x6a, 0x71);
            this.label2.Name = "label2";
            this.label2.Size = new Size(90, 13);
            this.label2.TabIndex = 0x18;
            this.label2.Text = "Digital Font Type:";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x158, 0xb6);
            base.Name = "NumericGaugeSeries";
            this.Text = "NumericGaugeEditor";
            base.tabControl1.ResumeLayout(false);
            base.tabOptions.ResumeLayout(false);
            base.tabOptions.PerformLayout();
            base.ResumeLayout(false);
        }

        protected override void SetGaugeColorPalette(int index)
        {
            switch (index)
            {
                case 1:
                    base.series.GaugeColorPalette = NumericGauge.LCDPalette;
                    return;

                case 2:
                    base.series.GaugeColorPalette = NumericGauge.LEDPalette;
                    return;

                case 3:
                    base.series.GaugeColorPalette = CustomGauge.BlackPalette;
                    return;

                case 4:
                    base.series.GaugeColorPalette = CustomGauge.BluesPalette;
                    return;

                case 5:
                    base.series.GaugeColorPalette = Theme.TeeChartPalette;
                    return;

                case 6:
                    base.series.GaugeColorPalette = Theme.ExcelPalette;
                    return;

                case 7:
                    base.series.GaugeColorPalette = Theme.ClassicPalette;
                    return;

                case 8:
                    base.series.GaugeColorPalette = Theme.WindowsXPPalette;
                    return;

                case 9:
                    base.series.GaugeColorPalette = Theme.WebPalette;
                    return;

                case 10:
                    base.series.GaugeColorPalette = Theme.VictorianPalette;
                    return;

                case 11:
                    base.series.GaugeColorPalette = Theme.PastelsPalette;
                    return;

                case 12:
                    base.series.GaugeColorPalette = Theme.GrayscalePalette;
                    return;

                case 13:
                    base.series.GaugeColorPalette = Theme.SolidPalette;
                    return;

                case 14:
                    base.series.GaugeColorPalette = Theme.ModernPalette;
                    return;

                case 15:
                    base.series.GaugeColorPalette = Theme.RainbowPalette;
                    return;
            }
        }

        public override void SetParent(TabPage Parent)
        {
            base.SetParent(Parent);
            if (this.numericGauge != null)
            {
                base.CBPalettes.Items.Insert(1, "LCDPalette");
                base.CBPalettes.Items.Insert(2, "LEDPalette");
                base.CBPalettes.SelectedIndex = 0;
                this.cbDigitalFont.Items.Add(Enum.GetName(typeof(DigitalFont), DigitalFont.Bar));
                this.cbDigitalFont.Items.Add(Enum.GetName(typeof(DigitalFont), DigitalFont.Dot));
                this.cbDigitalFont.SelectedItem = Enum.GetName(typeof(DigitalFont), this.numericGauge.DigitalFontType);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (base.tabControl1.SelectedTab == this.tabMarkers)
            {
                if (this.toolsEditor == null)
                {
                    foreach (Marker marker in this.numericGauge.Markers)
                    {
                        marker.UsePalette = false;
                    }
                    this.toolsEditor = new MarkersEditor(this.numericGauge.Markers, this.tabMarkers, typeof(Marker));
                    EditorUtils.Translate(this.toolsEditor);
                }
                else
                {
                    this.toolsEditor.Reload();
                }
            }
        }
    }
}

