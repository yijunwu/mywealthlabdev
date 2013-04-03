namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class GannFanHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a Gann Fan on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOGannFan);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Gann Fan";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.GannFan;
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

