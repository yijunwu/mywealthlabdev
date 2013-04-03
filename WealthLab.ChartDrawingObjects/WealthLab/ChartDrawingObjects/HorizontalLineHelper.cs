namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class HorizontalLineHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a horizontal line on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOHorizontalLine);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Horizontal Line";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.HorizontalLine;
            }
        }

        public override DrawingObjectHelper.ToolBarGroup Grouping
        {
            get
            {
                return DrawingObjectHelper.ToolBarGroup.SimpleLine;
            }
        }
    }
}

