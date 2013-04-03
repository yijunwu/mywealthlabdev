namespace WealthLab.ChartControl
{
    using System;
    using System.Drawing;

    public abstract class DrawingObjectHelper
    {
        protected DrawingObjectHelper()
        {
        }

        public virtual bool GroupSeparator(ToolBarGroup toolBarGroup_0)
        {
            if (toolBarGroup_0 == ToolBarGroup.SimpleLine)
            {
                return false;
            }
            return true;
        }

        public abstract string Description { get; }

        public abstract Type DrawingObjectType { get; }

        public abstract string FriendlyName { get; }

        public abstract Bitmap Glyph { get; }

        public virtual ToolBarGroup Grouping
        {
            get
            {
                return ToolBarGroup.UserDefined;
            }
        }

        public virtual string URL
        {
            get
            {
                return "";
            }
        }

        public enum ToolBarGroup
        {
            None,
            Simple,
            SimpleLine,
            Line,
            Polygon,
            Annotation,
            Complex,
            UserDefined
        }
    }
}

