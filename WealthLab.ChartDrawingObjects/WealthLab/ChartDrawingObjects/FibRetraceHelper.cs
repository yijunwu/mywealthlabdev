namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class FibRetraceHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a Fib Retracement on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOFibRetrace);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Fib Retrace";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.FibRetrace;
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

