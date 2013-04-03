namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class TradableTrendlineHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Tradable Trendlines can trigger Trade Alerts when prices cross above or below the Trendline value.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOTradableTrendline);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Tradable Trendline";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.TradableTrendline;
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

