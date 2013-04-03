namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class VerticalLineHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a vertical line on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOVerticalLine);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Vertical Line";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.VerticalLine;
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

