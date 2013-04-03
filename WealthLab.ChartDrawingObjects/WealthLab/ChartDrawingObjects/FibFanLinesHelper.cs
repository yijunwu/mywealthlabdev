namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class FibFanLinesHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw Fib Fan lines on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOFibFanLines);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Fib Fan Lines";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.FibFan;
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

