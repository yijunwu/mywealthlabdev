namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Description("Displays a scrollbar at Chart Legend to enable scroll it by dragging the mouse."), ToolboxBitmap(typeof(LegendScrollBar), "ToolsIcons.LegendScrollBar.bmp")]
    public class LegendScrollBar : ScrollBar
    {
        private Legend l;

        public LegendScrollBar() : this(null)
        {
        }

        public LegendScrollBar(Chart c) : base(c)
        {
        }

        protected override int CurrentCount()
        {
            int num = 0;
            if (base.Chart == null)
            {
                return num;
            }
            if (base.Chart.Page.MaxPointsPerPage > 0)
            {
                return 0;
            }
            return ((this.L.iLastValue - this.L.FirstValue) + 1);
        }

        protected override int DeltaMain()
        {
            int num = 0;
            if (base.Chart == null)
            {
                return num;
            }
            if (base.Chart.Page.MaxPointsPerPage > 0)
            {
                return 1;
            }
            return base.DeltaMain();
        }

        protected override int GetPosition()
        {
            int num = 0;
            if (base.Chart == null)
            {
                return num;
            }
            if (base.Chart.Page.MaxPointsPerPage > 0)
            {
                return (base.Chart.Page.Current - 1);
            }
            return this.L.FirstValue;
        }

        internal override void LegendCalcSize(object sender, GetLegendSizeEventArgs e)
        {
            if (base.Active && this.ShouldDraw())
            {
                e.Size = (e.Size + base.Size) + 1;
            }
        }

        protected override void SetIPosition(int value)
        {
            if (base.Chart != null)
            {
                if (base.Chart.Page.MaxPointsPerPage > 0)
                {
                    base.Chart.Page.Current = value + 1;
                }
                else
                {
                    this.L.FirstValue = value;
                }
            }
        }

        protected override bool ShouldDraw()
        {
            bool flag = false;
            if (base.Chart != null)
            {
                flag = (this.L.Visible && (this.L.iLastValue > -1)) && (((base.DrawStyle == ScrollBarDrawStyle.Always) || ((this.L.iLastValue + 1) < this.L.iTotalItems)) || (this.L.firstValue > 0));
                if (flag)
                {
                    base.Horizontal = !this.L.Vertical;
                    base.R = this.L.ShapeBounds;
                }
            }
            return flag;
        }

        protected override int TotalCount()
        {
            if (base.Chart.Page.MaxPointsPerPage > 0)
            {
                return base.Chart.Page.Count;
            }
            return (this.L.iTotalItems + this.L.FirstValue);
        }

        private Legend L
        {
            get
            {
                if ((this.l == null) && (base.Chart != null))
                {
                    this.l = base.Chart.Legend;
                }
                return this.l;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.LegendScrollBarSummary;
            }
        }
    }
}

