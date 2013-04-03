namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class SpeedResistanceHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw Speed Resistance lines on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOSpeedResistance);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Speed Resistance";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.SpeedResistance;
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

