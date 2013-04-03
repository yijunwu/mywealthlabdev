namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class CalloutHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a callout on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOCallout);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Callout";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Callout;
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

