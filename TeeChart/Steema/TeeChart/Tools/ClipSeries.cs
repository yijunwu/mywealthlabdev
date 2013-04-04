namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(ClipSeries), "ToolsIcons.ClipSeries.bmp"), Description("Clip to Series")]
    public class ClipSeries : ToolSeries
    {
        public ClipSeries() : this(null)
        {
        }

        public ClipSeries(Chart c) : base(c)
        {
        }

        protected internal override void SeriesEvent(EventArgs e)
        {
            if (((base.Series != null) && (base.Chart != null)) && base.Chart.CanClip())
            {
                if (e is BeforeDrawSeriesEventArgs)
                {
                    Rectangle r = Utils.FromLTRB(base.Series.GetHorizAxis.IStartPos, base.Series.GetVertAxis.IStartPos, base.Series.GetHorizAxis.IEndPos, base.Series.GetVertAxis.IEndPos);
                    base.Chart.Graphics3D.ClipRectangle(r);
                }
                else if (e is AfterDrawSeriesEventsArgs)
                {
                    base.Chart.Graphics3D.UnClip();
                }
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ClipSeries;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ClipSeriesSummary;
            }
        }
    }
}

