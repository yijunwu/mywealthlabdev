namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class FibArcsHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw Fib Arcs on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOFibArcs);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Fib Arcs";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.FibArcs;
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

