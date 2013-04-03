namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class FibTimeZonesHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw Fib Time Zones on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOFibTimeZones);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Fib Time Zones";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.FibTimeZones;
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

