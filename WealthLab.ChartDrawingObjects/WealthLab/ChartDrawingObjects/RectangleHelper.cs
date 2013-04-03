namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class RectangleHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a rectangle on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDORectangle);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Rectangle";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Rectangle;
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

