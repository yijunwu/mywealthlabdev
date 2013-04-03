namespace WealthLab.ChartDrawingObjects
{
    using System;
    using System.Drawing;
    using WealthLab.ChartControl;
    using WealthLab.ChartDrawingObjects.Properties;

    public class TextNoteHelper : DrawingObjectHelper
    {
        public override string Description
        {
            get
            {
                return "Draw a text note on the chart.";
            }
        }

        public override Type DrawingObjectType
        {
            get
            {
                return typeof(CDOTextNote);
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Text Note";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.TextNote;
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

