namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DonutSeries : BaseSeriesForm
    {
        private CircledSeries circledEditor;
        private Container components;
        private Donut donut;
        private Label label1;
        private PieSeries pieEditor;
        private NumericUpDown UDDonut;

        public DonutSeries()
        {
            this.InitializeComponent();
        }

        public DonutSeries(Series s) : this()
        {
            this.donut = (Donut) s;
            this.UDDonut.Value = this.donut.DonutPercent;
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
            this.label1 = new Label();
            this.UDDonut = new NumericUpDown();
            this.UDDonut.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x19, 0x1b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2c, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Hole %:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.UDDonut.BorderStyle = BorderStyle.FixedSingle;
            int[] bits = new int[4];
            bits[0] = 5;
            this.UDDonut.Increment = new decimal(bits);
            this.UDDonut.Location = new Point(80, 0x18);
            this.UDDonut.Name = "UDDonut";
            this.UDDonut.Size = new Size(0x30, 20);
            this.UDDonut.TabIndex = 1;
            this.UDDonut.TextAlign = HorizontalAlignment.Right;
            this.UDDonut.TextChanged += new EventHandler(this.UDDonut_TextChanged);
            this.UDDonut.ValueChanged += new EventHandler(this.UDDonut_ValueChanged);
            base.ClientSize = new Size(0xa8, 0x4c);
            base.Controls.Add(this.UDDonut);
            base.Controls.Add(this.label1);
            base.Name = "DonutSeries";
            this.UDDonut.EndInit();
            base.ResumeLayout(false);
        }

        public override void SetParent(TabPage Parent)
        {
            if (this.circledEditor == null)
            {
                this.circledEditor = CircledSeries.InsertForm(Parent, this.donut);
            }
            if (this.pieEditor == null)
            {
                TabControl parent = (TabControl) Parent.Parent;
                TabPage page = new TabPage(Texts.GalleryDonut);
                parent.TabPages.Add(page);
                this.pieEditor = new PieSeries(this.donut, page, this.circledEditor);
                this.pieEditor.SetParent(Parent);
                TabPage page2 = parent.TabPages[1];
                parent.TabPages[1] = page;
                parent.TabPages[parent.TabCount - 1] = page2;
            }
        }

        private void UDDonut_TextChanged(object sender, EventArgs e)
        {
            this.UDDonut_ValueChanged(sender, e);
        }

        private void UDDonut_ValueChanged(object sender, EventArgs e)
        {
            this.donut.DonutPercent = (int) this.UDDonut.Value;
        }
    }
}

