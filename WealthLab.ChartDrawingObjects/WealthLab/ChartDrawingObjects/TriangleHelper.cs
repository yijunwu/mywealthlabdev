namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class TriangleHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a triangle on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOTriangle);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Triangle";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Triangle;
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

