namespace WealthLab.ChartControl
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl.Properties;

    public class TrendlineHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Trendlines can be used to record trends in price movement, or to indicate support and resistance areas.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOTrendline);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Trendline";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Trendline;
            }
        }

        public override DrawingObjectHelper.ToolBarGroup Grouping
        {
            get
            {
                return DrawingObjectHelper.ToolBarGroup.Line;
            }
        }
    }
}

