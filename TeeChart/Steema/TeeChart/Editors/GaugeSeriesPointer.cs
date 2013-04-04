namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class GaugeSeriesPointer : Steema.TeeChart.Editors.SeriesPointer
    {
        private IContainer components;
        private Steema.TeeChart.Styles.GaugeSeriesPointer gPoint;

        public GaugeSeriesPointer()
        {
            this.InitializeComponent();
        }

        public GaugeSeriesPointer(Steema.TeeChart.Styles.GaugeSeriesPointer p) : this(p, null, true)
        {
        }

        public GaugeSeriesPointer(Steema.TeeChart.Styles.GaugeSeriesPointer p, bool showStyles) : this(p, null, showStyles)
        {
        }

        public GaugeSeriesPointer(Steema.TeeChart.Styles.GaugeSeriesPointer p, Control parent, bool showStyles) : base(p, parent)
        {
            this.gPoint = p;
            base.point = this.gPoint;
            this.Text = "Gauge Series Pointer";
            base.CBStyle.Items.Add("Hand");
            base.CBStyle.Items.Add("Center");
            base.CBStyle.Items.Add("Tick");
            base.CBStyle.Items.Add("MinorTick");
            base.CBStyle.Items.Add("ColorLine");
            base.CBStyle.SelectedIndex = (int) this.gPoint.Style;
            base.CBColorEach.Visible = false;
            base.CB3dPoint.Visible = false;
            base.CBInflate.Visible = false;
            base.CBPoDark.Visible = false;
            if (!showStyles)
            {
                base.label3.Visible = false;
                base.label1.Visible = false;
                base.label2.Visible = false;
                base.UDPointHorizSize.Visible = false;
                base.UDPointVertSize.Visible = false;
                base.CBStyle.Visible = false;
            }
        }

        protected override void CBStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (base.CBStyle.SelectedIndex > 13)
            {
                switch (base.CBStyle.SelectedIndex)
                {
                    case 15:
                        this.gPoint.Style = GaugePointerStyles.Hand;
                        goto Label_009D;

                    case 0x10:
                        this.gPoint.Style = GaugePointerStyles.Center;
                        goto Label_009D;

                    case 0x11:
                        this.gPoint.Style = GaugePointerStyles.Tick;
                        goto Label_009D;

                    case 0x12:
                        this.gPoint.Style = GaugePointerStyles.MinorTick;
                        goto Label_009D;
                }
                this.gPoint.Style = 9;
            }
            else
            {
                base.CBStyle_SelectedIndexChanged(sender, e);
            }
        Label_009D:
            if (this.gPoint != null)
            {
                this.gPoint.Invalidate();
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
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
            this.Text = "GaugeSeriesPointer";
        }
    }
}

