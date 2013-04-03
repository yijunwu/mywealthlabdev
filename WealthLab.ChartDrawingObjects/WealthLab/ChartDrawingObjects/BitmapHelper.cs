namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class BitmapHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a bitmap on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOBitmap);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Bitmap";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Bitmap;
            }
        }

        public override DrawingObjectHelper.ToolBarGroup Grouping
        {
            get
            {
                return DrawingObjectHelper.ToolBarGroup.Simple;
            }
        }
    }
}

