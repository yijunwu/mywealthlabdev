namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class EllipseHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw an Ellipse on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOEllipse);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Ellipse";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Ellipse;
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

