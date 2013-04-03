namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class DiamondHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a Diamond on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDODiamond);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Diamond";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Diamond;
            }
        }

        public override DrawingObjectHelper.ToolBarGroup Grouping
        {
            get
            {
                return DrawingObjectHelper.ToolBarGroup.Polygon;
            }
        }
    }
}

