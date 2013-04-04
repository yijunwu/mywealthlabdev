namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false)]
    internal class InnerChart : TChart
    {
        private SubChart iParent;

        public InnerChart()
        {
            base.Legend.Visible = false;
            base.Header.Font.Size = 7;
            base.Footer.Visible = false;
            base.SubHeader.Visible = false;
            base.SubFooter.Visible = false;
            base.Panel.MarginLeft = 0.0;
            base.Panel.MarginRight = 0.0;
            base.Panel.MarginBottom = 0.0;
            base.Panel.Bevel.Outer = BevelStyles.None;
            base.Panel.Bevel.Inner = BevelStyles.None;
            base.Bounds = new Rectangle(50, 50, 150, 100);
        }

        public override void DoInvalidate()
        {
            if (this.iParent != null)
            {
                this.iParent.ITool.Chart.Parent.DoInvalidate();
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
            if ((this.iParent != null) && (this.iParent.ITool != null))
            {
                this.iParent.ITool.Chart.Parent.DoInvalidate();
            }
        }

        public SubChart IParent
        {
            get
            {
                return this.iParent;
            }
            set
            {
                this.iParent = value;
            }
        }
    }
}

