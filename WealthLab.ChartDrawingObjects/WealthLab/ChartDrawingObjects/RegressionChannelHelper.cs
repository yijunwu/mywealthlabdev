namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class RegressionChannelHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "RegressionChannels can be used to record trends in price\n movement, or to indicate support and resistance areas.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDORegressionChannel);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "RegressionChannel";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.RegressionChannel;
            }
        }

        public override DrawingObjectHelper.ToolBarGroup Grouping
        {
            get
            {
                return DrawingObjectHelper.ToolBarGroup.Complex;
            }
        }
    }
}

