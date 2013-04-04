namespace QWhale.Editor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class RulerIndent : IRulerIndent
    {
        private bool dragging;
        private int indent;
        private int oldIndent;
        private IndentOrientation orientation;

        public RulerIndent()
        {
        }

        public RulerIndent(IndentOrientation orientation, int indent) : this()
        {
            this.orientation = orientation;
            this.indent = indent;
        }

        public void CancelDragging()
        {
            this.Indent = this.oldIndent;
            this.dragging = false;
        }

        public void DrawIndent(Graphics graph, Rectangle rect, bool vertical, Color indentBackColor, Color backColor)
        {
            Rectangle rectangle;
            Brush brush = new SolidBrush(indentBackColor);
            graph.FillRectangle(brush, rect);
            brush.Dispose();
            if (vertical)
            {
                if (this.orientation == IndentOrientation.Near)
                {
                    rectangle = new Rectangle(rect.Left, rect.Bottom - EditConsts.DefaultRulerIndentSize, rect.Width, EditConsts.DefaultRulerIndentSize);
                }
                else
                {
                    rectangle = new Rectangle(rect.Left, rect.Top, rect.Width, EditConsts.DefaultRulerIndentSize);
                }
            }
            else if (this.orientation == IndentOrientation.Near)
            {
                rectangle = new Rectangle(rect.Right - EditConsts.DefaultRulerIndentSize, rect.Top, EditConsts.DefaultRulerIndentSize, rect.Height);
            }
            else
            {
                rectangle = new Rectangle(rect.Left, rect.Top, EditConsts.DefaultRulerIndentSize, rect.Height);
            }
            brush = new SolidBrush(backColor);
            graph.FillRectangle(brush, rectangle);
            brush.Dispose();
        }

        protected virtual void OnDraggingChanged()
        {
            if (this.dragging)
            {
                this.oldIndent = this.indent;
            }
        }

        protected virtual void OnIndentChanged()
        {
        }

        protected virtual void OnOrientationChanged()
        {
        }

        [Description("Indicates whether \"RulerIndent\" is in dragging state.")]
        public bool Dragging
        {
            get
            {
                return this.dragging;
            }
            set
            {
                if (this.dragging != value)
                {
                    this.dragging = value;
                    this.OnDraggingChanged();
                }
            }
        }

        [Description("Gets or sets size of this \"RulerIndent\".")]
        public int Indent
        {
            get
            {
                return this.indent;
            }
            set
            {
                if (this.indent != value)
                {
                    this.indent = Math.Max(value, 0);
                    this.OnIndentChanged();
                }
            }
        }

        [Description("Gets or sets \"RulerIndent\" alignment.")]
        public IndentOrientation Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                if (this.orientation != value)
                {
                    this.orientation = value;
                    this.OnOrientationChanged();
                }
            }
        }
    }
}

